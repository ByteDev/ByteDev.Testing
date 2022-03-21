using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ByteDev.Testing.Http;
using NUnit.Framework;

namespace ByteDev.Testing.UnitTests.Http
{
    [TestFixture]
    public class FakeHttpMessageHandlerTests
    {
        private readonly Uri GoogleUri = new Uri("http://www.google.com/");

        [Test]
        public void WhenOutcomesIsNullOutcome_ThenThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new FakeHttpMessageHandler(null as FakeRequestOutcome));
        }

        [Test]
        public void WhenOutcomesIsNullList_ThenThrowException()
        {
            Assert.Throws<ArgumentException>(() => _ = new FakeHttpMessageHandler(null as List<FakeRequestOutcome>));
        }

        [Test]
        public void WhenOutcomesIsEmpty_ThenThrowException()
        {
            Assert.Throws<ArgumentException>(() => _ = new FakeHttpMessageHandler(new List<FakeRequestOutcome>()));
        }

        [Test]
        public void WhenSingleOutcomeIsException_ThenThrowException()
        {
            var ex = new InvalidOperationException("Test exception");

            var sut = new FakeHttpMessageHandler(new FakeRequestOutcome(ex));

            var httpClient = new HttpClient(sut);

            var resultEx = Assert.ThrowsAsync<InvalidOperationException>(() => httpClient.GetAsync(GoogleUri));
            Assert.That(resultEx, Is.SameAs(ex));
            Assert.That(sut.RequestsMade, Is.EqualTo(1));
        }

        [Test]
        public async Task WhenSingleOutcomeIsHttpResponse_ThenReturnResponse()
        {
            var sut = new FakeHttpMessageHandler(new FakeRequestOutcome(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Test content")
            }));

            var httpClient = new HttpClient(sut);

            var result = await httpClient.GetAsync(GoogleUri);
            var resultContent = await result.Content.ReadAsStringAsync();

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(resultContent, Is.EqualTo("Test content"));
            Assert.That(sut.RequestsMade, Is.EqualTo(1));
        }

        [Test]
        public async Task WhenTwoRequests_AndOnlyOneOutcomeDefined_ThenThrowException()
        {
            var sut = new FakeHttpMessageHandler(new FakeRequestOutcome(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Test content")
            }));

            var httpClient = new HttpClient(sut);

            await httpClient.GetAsync(GoogleUri);
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => httpClient.GetAsync(GoogleUri));
            Assert.That(ex.Message, Is.EqualTo("Request number 2 has no corresponding outcome."));
        }

        [Test]
        public async Task WhenTwoRequests_AndOneOutcomeDefined_AndRepeatIsTrue_ThenReturnSameResponse()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Test content")
            };

            var sut = new FakeHttpMessageHandler(new FakeRequestOutcome(response))
            {
                RepeatOutcomes = true
            };

            var httpClient = new HttpClient(sut);

            var result1 = await httpClient.GetAsync(GoogleUri);
            var result2 = await httpClient.GetAsync(GoogleUri);
            
            Assert.That(sut.RequestsMade, Is.EqualTo(2));
            Assert.That(result1, Is.SameAs(response));
            Assert.That(result2, Is.SameAs(response));
        }

        [Test]
        public async Task WhenThreeRequests_AndTwoOutcomeDefined_AndRepeatIsTrue_ThenRepeat()
        {
            var response1 = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Test content 1") };
            var response2 = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Test content 1") };

            var sut = new FakeHttpMessageHandler(new List<FakeRequestOutcome>
            {
                new FakeRequestOutcome(response1),
                new FakeRequestOutcome(response2),
                new FakeRequestOutcome(response1)
            })
            {
                RepeatOutcomes = true
            };

            var httpClient = new HttpClient(sut);

            var result1 = await httpClient.GetAsync(GoogleUri);
            var result2 = await httpClient.GetAsync(GoogleUri);
            var result3 = await httpClient.GetAsync(GoogleUri);

            Assert.That(sut.RequestsMade, Is.EqualTo(3));
            Assert.That(result1, Is.SameAs(response1));
            Assert.That(result2, Is.SameAs(response2));
            Assert.That(result3, Is.SameAs(response1));
        }

        [Test]
        public async Task WhenTwoOutcomes_AndTwoRequest_ThenReturnBothOutcomes()
        {
            var sut = new FakeHttpMessageHandler(new List<FakeRequestOutcome>
            {
                new FakeRequestOutcome(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("Test content 1")
                }),
                new FakeRequestOutcome(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Test content 2")
                })
            });

            var httpClient = new HttpClient(sut);

            var result1 = await httpClient.GetAsync(GoogleUri);
            var resultContent1 = await result1.Content.ReadAsStringAsync();

            var result2 = await httpClient.GetAsync(GoogleUri);
            var resultContent2 = await result2.Content.ReadAsStringAsync();

            Assert.That(sut.RequestsMade, Is.EqualTo(2));
            
            Assert.That(result1.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(resultContent1, Is.EqualTo("Test content 1"));
            
            Assert.That(result2.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(resultContent2, Is.EqualTo("Test content 2"));
        }
    }
}
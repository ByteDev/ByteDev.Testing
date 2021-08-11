using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ByteDev.Testing.Http;
using NUnit.Framework;

namespace ByteDev.Testing.UnitTests.Http
{
    [TestFixture]
    public class FakeResponseHandlerTests
    {
        private readonly Uri GoogleUri = new Uri("http://www.google.com/");

        [Test]
        public async Task WhenCodeSet_ThenReturnCode()
        {
            var code = HttpStatusCode.OK;

            var sut = new HttpClient(new FakeResponseHandler(code));

            var result = await sut.GetAsync(GoogleUri);

            Assert.That(result.StatusCode, Is.EqualTo(code));
            Assert.That(result.Content, Is.Null);
        }

        [Test]
        public async Task WhenCodeAndContentSet_ThenReturnCodeAndContent()
        {
            var code = HttpStatusCode.OK;
            var content = new StringContent("test");

            var sut = new HttpClient(new FakeResponseHandler(code, content));

            var result = await sut.GetAsync(GoogleUri);
            var resultContent = await result.Content.ReadAsStringAsync();

            Assert.That(result.StatusCode, Is.EqualTo(code));
            Assert.That(resultContent, Is.EqualTo("test"));
        }
    }
}
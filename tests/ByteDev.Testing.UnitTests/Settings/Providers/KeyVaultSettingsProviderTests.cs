using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ByteDev.Azure.KeyVault.Secrets;
using ByteDev.Collections;
using ByteDev.Testing.Settings.Providers;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace ByteDev.Testing.UnitTests.Settings.Providers
{
    [TestFixture]
    public class KeyVaultSettingsProviderTests
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void WhenKeyVaultClientIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _ = new KeyVaultSettingsProvider(null));
            }
        }

        [TestFixture]
        public class GetSettings
        {
            private IKeyVaultSecretClient _kvClient;

            [SetUp]
            public void SetUp()
            {
                _kvClient = Substitute.For<IKeyVaultSecretClient>();
            }

            [Test]
            public void WhenKvClientThrowsException_ThenThrowException()
            {
                var testEx = new Exception("Test exception");

                WhenKvClientThrowsException(testEx);

                var ex = Assert.Throws<TestingException>(() => _ = CreateSut().GetSettings<TestPerson>());
                Assert.That(ex.InnerException, Is.TypeOf(typeof(AggregateException)));
                Assert.That(ex.InnerException.InnerException, Is.SameAs(testEx));
            }

            [Test]
            public void WhenKvClientGetsValues_ThenSetMatchingProperties()
            {
                var nameValues = new Dictionary<string, string>
                {
                    {nameof(TestPerson.Name), "John"}, 
                    {nameof(TestPerson.City), "London"}
                };

                WhenKvClientReturnsNameValues(nameValues);

                var result = CreateSut().GetSettings<TestPerson>();

                Assert.That(result.Name, Is.EqualTo(nameValues.Values.First()));
                Assert.That(result.City, Is.EqualTo(nameValues.Values.Second()));
                Assert.That(result.Postcode, Is.Null);
            }

            private void WhenKvClientReturnsNameValues(IDictionary<string, string> nameValues)
            {
                _kvClient.GetValuesIfExistsAsync(
                        Arg.Any<IEnumerable<string>>(),
                        Arg.Any<bool>(),
                        Arg.Any<CancellationToken>())
                    .Returns(nameValues);
            }

            private void WhenKvClientThrowsException(Exception ex)
            {
                _kvClient.GetValuesIfExistsAsync(
                        Arg.Any<IEnumerable<string>>(),
                        Arg.Any<bool>(),
                        Arg.Any<CancellationToken>())
                    .Throws(ex);
            }

            private KeyVaultSettingsProvider CreateSut()
            {
                return new KeyVaultSettingsProvider(_kvClient);
            }
        }

        public class TestPerson
        {
            public string Name { get; set; }

            public string City { get; set; }

            public string Postcode { get; set; }
        }
    }
}
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace ByteDev.Testing.UnitTests
{
    [TestFixture]
    public class TestingExceptionTests
    {
        private const string ExMessage = "some message";

        [Test]
        public void WhenNoArgs_ThenSetMessageToDefault()
        {
            var sut = new TestingException();

            Assert.That(sut.Message, Is.EqualTo("Error occurred in the Testing library."));
        }

        [Test]
        public void WhenMessageSpecified_ThenSetMessage()
        {
            var sut = new TestingException(ExMessage);

            Assert.That(sut.Message, Is.EqualTo(ExMessage));
        }

        [Test]
        public void WhenMessageAndInnerExSpecified_ThenSetMessageAndInnerEx()
        {
            var innerException = new Exception();

            var sut = new TestingException(ExMessage, innerException);

            Assert.That(sut.Message, Is.EqualTo(ExMessage));
            Assert.That(sut.InnerException, Is.SameAs(innerException));
        }

        [Test]
        public void WhenSerialized_ThenDeserializeCorrectly()
        {
            var sut = new TestingException(ExMessage);

            var formatter = new BinaryFormatter();
            
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, sut);

                stream.Seek(0, 0);

                var result = (TestingException)formatter.Deserialize(stream);

                Assert.That(result.ToString(), Is.EqualTo(sut.ToString()));
            }
        }
    }
}
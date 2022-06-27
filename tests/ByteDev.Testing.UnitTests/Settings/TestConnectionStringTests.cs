using System;
using System.Linq;
using System.Reflection;
using ByteDev.Collections;
using ByteDev.Testing.Settings;
using NUnit.Framework;

namespace ByteDev.Testing.UnitTests.Settings
{
    [TestFixture]
    public class TestConnectionStringTests
    {
        [TestFixture]
        public class Constructor : TestSettingsTests
        {
            [Test]
            public void WhenNoParam_ThenSetDefaults()
            {
                var sut = new TestConnectionString();

                Assert.That(sut.EnvironmentVarName, Is.Null);
                Assert.That(sut.FilePaths, Is.Empty);
            }

            [Test]
            public void WhenAssemblyIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _ = new TestConnectionString(null));
            }

            [Test]
            public void WhenInit_ThenSetsFilePaths()
            {
                string userName = Environment.UserName;

                var sut = new TestConnectionString(Assembly.GetAssembly(typeof(TestConnectionStringTests)));

                var result = sut.FilePaths.ToList();

                Assert.That(result.Count, Is.EqualTo(5));
                Assert.That(result.First(), Is.EqualTo(@"C:\Temp\ByteDev.Testing.UnitTests.connstring"));
                Assert.That(result.Second(), Is.EqualTo(@"C:\Dev\ByteDev.Testing.UnitTests.connstring"));
                Assert.That(result.Third(), Is.EqualTo(@"Z:\Dev\ByteDev.Testing.UnitTests.connstring"));
                Assert.That(result.Fourth(), Is.EqualTo(@"C:\Users\" + userName + @"\ByteDev.Testing.UnitTests.connstring"));
                Assert.That(result.Fifth(), Is.EqualTo(@"C:\Users\" + userName + @"\Documents\ByteDev.Testing.UnitTests.connstring"));
            }
        }
    }
}
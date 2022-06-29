using System;
using System.Collections.Generic;
using System.Linq;
using ByteDev.Collections;
using ByteDev.Testing.Settings.Providers;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ByteDev.Testing.UnitTests.Settings.Providers
{
    [TestFixture]
    public class JsonFileSettingsProviderTests
    {
        [TestFixture]
        public class Constructor
        {
            private const string FirstFile = @"C:\Temp\SomeFile1.txt";
            private const string SecondFile = @"C:\Temp\SomeFile2.txt";

            [Test]
            public void WhenAddNullStringParams_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _ = new JsonFileSettingsProvider(null as string[]));
            }

            [Test]
            public void WhenAddOneString_ThenAddToList()
            {
                var sut = new JsonFileSettingsProvider(FirstFile);

                Assert.That(sut.FilePaths.First(), Is.EqualTo(FirstFile));
            }

            [Test]
            public void WhenAddTwoStrings_ThenAddToList()
            {
                var sut = new JsonFileSettingsProvider(FirstFile, SecondFile);

                Assert.That(sut.FilePaths.First(), Is.EqualTo(FirstFile));
                Assert.That(sut.FilePaths.Second(), Is.EqualTo(SecondFile));
            }

            [Test]
            public void WhenAddStringList_ThenAddToList()
            {
                var sut = new JsonFileSettingsProvider(new List<string> { FirstFile, SecondFile });

                Assert.That(sut.FilePaths.First(), Is.EqualTo(FirstFile));
                Assert.That(sut.FilePaths.Second(), Is.EqualTo(SecondFile));
            }
        }
    }
}
using System;
using System.IO;
using System.IO.Fakes;
using System.Text;
using Core_AMS.Utilities;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.Core_AMS.Utilities.FileFunctions_cs
{
    [TestFixture]
    public class WriteToFileTest
    {
        private const string DummyMessage = "MyDummyMessage";
        private IDisposable context;

        [SetUp]
        public void SetUp()
        {
            context = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        [Test]
        public void WriteToFile_InValidParameter_ArgumentNullException()
        {
            // Arrange
            var fileFunctions = new FileFunctions();

            // Act, Assert
            Should.Throw<ArgumentNullException>(
                () => fileFunctions.WriteToFile(string.Empty, null));
        }

        [Test]
        public void WriteToFile_ValidParameter_TextWrittenStreamFlushed()
        {
            // Arrange
            var buffer = new byte[1000];
            var actualMessages = new StringBuilder();
            var closeCalled = false;

            ShimTextWriter.AllInstances.WriteLineString = (writer, message) =>
                {
                    actualMessages.Append(message);
                };

            ShimStreamWriter.AllInstances.Close = writer =>
                {
                    closeCalled = true;
                };

            // Act
            using (var memoryStream = new MemoryStream(buffer))
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    var fileFunctions = new FileFunctions();
                    fileFunctions.WriteToFile(DummyMessage, streamWriter);
                }
            }

            // Assert
            actualMessages.ShouldSatisfyAllConditions(
                () => actualMessages.ToString().ShouldBe(DummyMessage),
                () => closeCalled.ShouldBeTrue());
        }
    }
}

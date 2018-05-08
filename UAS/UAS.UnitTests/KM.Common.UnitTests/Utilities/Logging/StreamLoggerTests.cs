using System;
using System.IO;
using KM.Common.Utilities.Logging;
using Moq;
using NUnit.Framework;

namespace KM.Common.UnitTests.Utilities.Logging
{
    [TestFixture]
    public class StreamLoggerTests
    {
        [Test]
        public void LogAndFlushWriter_SampleStringPassed_ShouldCallWriteLineAndFlush()
        {
            // Arrange
            var mockStreamWriter = new Mock<StreamWriter>("output.txt");
            string sampleText = "Hello World";

            // Act
            StreamLogger.LogAndFlushWriter(mockStreamWriter.Object, sampleText);

            // Assert
            mockStreamWriter.Verify(a => a.WriteLine(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Exactly(1));
            mockStreamWriter.Verify(a => a.Flush(), Times.Exactly(1));
        }
    }
}

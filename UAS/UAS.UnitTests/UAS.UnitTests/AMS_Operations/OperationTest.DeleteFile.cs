using System;
using System.IO;
using System.IO.Fakes;
using AMS_Operations;
using KMPlatform.BusinessLogic.Fakes;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.AMS_Operations
{
    public partial class OperationTest
    {
        private const string SampleMessage = "sample message";

        [Test]
        public void DeleteFileCycle_FirstException_Deleted()
        {
            // Arrange
            const string sampleFileName = "Sample.txt";
            var counter = 1;
            ShimFile.DeleteString = _ =>
            {
                if (counter > 0)
                {
                    counter--;
                    throw new IOException();
                }
            };

            // Act
            Operations.DeleteFileCycle(sampleFileName);

            // Assert
            counter.ShouldBe(0);
        }

        [Test]
        public void LogFileException_ValidData_Logged()
        {
            // Arrange
            var messageLogged = string.Empty;
            var sourceMethodLogged = string.Empty;
            ShimApplicationLog.AllInstances
                    .LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (log, message, sourceMethod, _, __, ___, ____) =>
                {
                    messageLogged = message;
                    sourceMethodLogged = sourceMethod;
                    return 0;
                };

            // Act
            Operations.LogFileException(new Exception(SampleMessage), $".{nameof(Operations.MoveFiles)}");

            // Assert
            messageLogged.ShouldSatisfyAllConditions(
                () => messageLogged.ShouldContain(SampleMessage),
                () => sourceMethodLogged.ShouldBe($"{typeof(Operations).Name}.{nameof(Operations.MoveFiles)}")
            );
        }
    }
}

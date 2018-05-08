using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Fakes;
using System.Text;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.SqlServer.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace KM.Common.UnitTests.Functions
{
    [TestFixture]
    public class FileFunctionsTests
    {
        private const string ExceptionMessageSample = "Sample Exception Message";
        private const string InnerExceptionMessageSample = "Sample Inner Exception Message";
        private const string ExceptionStackTraceSample = "Sample StackTrace Message";
        private IDisposable _shims;
        private string MessageAndInnerMessageAndStackTraceIsNullMessage { get; set; }
        private string MessageAndInnerMessageAndStackTraceIsNotNullMessage { get; set; }

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
            ShimDateTime.NowGet = () => new DateTime(2000, 1, 1, 1, 1, 1);
            InitErrorMessages();
        }

        private void InitErrorMessages()
        {
            var fullMessageBuilder = new StringBuilder();
            fullMessageBuilder.AppendLine("**********************");
            fullMessageBuilder.AppendLine("Exception - 1/1/2000 1:01:01 AM");
            fullMessageBuilder.AppendLine("-- Message --");
            fullMessageBuilder.AppendLine("Sample Exception Message");
            fullMessageBuilder.AppendLine("-- InnerException --");
            fullMessageBuilder.AppendLine("System.Exception: Sample Inner Exception Message");
            fullMessageBuilder.AppendLine("-- Stack Trace --");
            fullMessageBuilder.AppendLine("Sample StackTrace Message");
            fullMessageBuilder.AppendLine("**********************");
            MessageAndInnerMessageAndStackTraceIsNotNullMessage = fullMessageBuilder.ToString();

            var emptyMessageBuilder = new StringBuilder();
            emptyMessageBuilder.AppendLine("**********************");
            emptyMessageBuilder.AppendLine("Exception - 1/1/2000 1:01:01 AM");
            emptyMessageBuilder.AppendLine("-- Message --");
            emptyMessageBuilder.AppendLine("-- InnerException --");
            emptyMessageBuilder.AppendLine("-- Stack Trace --");
            emptyMessageBuilder.AppendLine("**********************");
            MessageAndInnerMessageAndStackTraceIsNullMessage = emptyMessageBuilder.ToString();
        }

        [TearDown]
        public void Teardown()
        {
            _shims?.Dispose();
        }

        [Test]
        public void BuildDownloadExceptionMessage_MessageNotNullInnerExceptionNotNullStackTraceNotNull()
        {
            //Arrange
            var ex = new StubException(ExceptionMessageSample, new Exception(InnerExceptionMessageSample));
            ex.MessageGet = () => ExceptionMessageSample;
            ex.StackTraceGet = () => ExceptionStackTraceSample;

            //Act
            var exceptionMessage = FileFunctions.BuildDownloadExceptionMessage(ex);

            //Assert
            exceptionMessage.ShouldBe(MessageAndInnerMessageAndStackTraceIsNotNullMessage);
        }

        [Test]
        public void BuildDownloadExceptionMessage_MessageNullInnerExceptionNullStackTraceNull()
        {
            //Arrange
            var ex = new StubException(null, null);
            ex.MessageGet = () => null;
            ex.StackTraceGet = () => null;

            //Act
            var exceptionMessage = FileFunctions.BuildDownloadExceptionMessage(ex);

            //Assert
            exceptionMessage.ShouldBe(MessageAndInnerMessageAndStackTraceIsNullMessage);
        }
    }
}

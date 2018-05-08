using System;
using System.IO;
using KM.Common.Entity.Fakes;
using KMPS.Hubspot.Integration;
using KMPS.Hubspot.Integration.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KMPS.HubImport.Integration.Tests
{
    [TestFixture]
    public class KMPSLoggerTests
    {
        private const string LogException = "LogExeception";
        public readonly string ExpectedNullReferenceException = "**********************" + Environment.NewLine +
                                                                "-- Data --" + Environment.NewLine +
                                                                "-- Message --" + Environment.NewLine +
                                                                "Object reference not set to an instance of an object." + Environment.NewLine +
                                                                "**********************";

        private KMPSLogger logger;
        private IDisposable shims;
        
        [SetUp]
        public void Setup()
        {
            shims = ShimsContext.Create();
            logger = new KMPSLogger(StreamWriter.Null, StreamWriter.Null, 0);
        }

        [TearDown]
        public void TearDown()
        {
            if (shims != null)
            {
                shims.Dispose();
            }
        }

        [Test]
        public void FormatException_SendTestException_ConstructCorrectString()
        {
            // Arrange
            var ex = new NullReferenceException();

            // Act
            var result = logger.FormatException(ex).ToString();

            // Assert
            result.ShouldContain(ExpectedNullReferenceException);
        }

        [Test]
        public void LogCustomerExeception_SendTestException_ConstructCorrectString()
        {
            // Arrange
            var ex = new NullReferenceException();
            var expectedException = string.Empty;
            ShimKMPSLogger.AllInstances.CustomerLogWriteString = (_, s) => { expectedException = s; };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (_, __, ___, ____, _____, ______) => 0;

            // Act
            logger.LogCustomerExeception(ex, LogException);

            // Assert
            expectedException.ShouldContain(ExpectedNullReferenceException);
        }

        [Test]
        public void LogMainExeception_SendTestException_ConstructCorrectString()
        {
            // Arrange
            var ex = new NullReferenceException();
            var expectedException = string.Empty;
            ShimKMPSLogger.AllInstances.MainLogWriteString = (_, s) => { expectedException = s; };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (_, __, ___, ____, _____, ______) => 0;

            // Act
            logger.LogMainExeception(ex, LogException);

            // Assert
            expectedException.ShouldContain(ExpectedNullReferenceException);
        }
    }
}

using System;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using ApplicationEnum = KMPlatform.BusinessLogic.Enums.Applications;
using EntityApplicationLog = KMPlatform.Entity.ApplicationLog;
using SeverityTypeEnum = KMPlatform.BusinessLogic.Enums.SeverityTypes;
using ServiceApplicationLog = UAS_WS.Service.ApplicationLog;
using ShimWorker = KMPlatform.BusinessLogic.Fakes.ShimApplicationLog;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ApplicationLogTest : Fakes
    {
        private const string SampleString = "message1";
        private const int AffectedCountPositive = 2;
        private const int AffectedCountNegative = -1;

        private ServiceApplicationLog _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceApplicationLog();
        }

        [Test]
        public void Save_WithEntityAndWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityApplicationLog();
            ShimWorker.AllInstances.SaveApplicationLogEnumsApplicationsEnumsSeverityTypesString
                = (a, b, c, d, e) => throw new InvalidOperationException();
            ShimForJsonFunction<EntityApplicationLog>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, ApplicationEnum.ADMS_Engine, SeverityTypeEnum.Critical);

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Save_WithEntityAnd_ReturnsExpectedResponse(bool workerSuccess)
        {
            // Arrange
            var entity = new EntityApplicationLog();
            ShimWorker.AllInstances.SaveApplicationLogEnumsApplicationsEnumsSeverityTypesString = (a, b, c, d, e) => workerSuccess
                ? AffectedCountPositive
                : AffectedCountNegative;
            ShimForJsonFunction<EntityApplicationLog>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, ApplicationEnum.ADMS_Engine, SeverityTypeEnum.Critical);

            // Assert
            if (workerSuccess)
            {
                VerifySuccessResponse(result, AffectedCountPositive);
            }
            else
            {
                VerifyErrorResponse(result, AffectedCountNegative);
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void LogCriticalError_WithExceptionMessage_ReturnsExpectedResponse(bool workerSuccess)
        {
            // Arrange
            ShimWorker.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String = (a, b, c, d, e, f, g) => workerSuccess
                ? AffectedCountPositive
                : AffectedCountNegative;

            // Act
            var result = _testEntity.LogCriticalError(Guid.Empty, SampleString, SampleString, ApplicationEnum.ADMS_Engine);

            // Assert
            if (workerSuccess)
            {
                VerifySuccessResponse(result, AffectedCountPositive);
            }
            else
            {
                VerifyErrorResponse(result, AffectedCountNegative);
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void LogNonCriticalError_WithException_ReturnsExpectedResponse(bool workerSuccess)
        {
            // Arrange
            var exception = new InvalidOperationException();
            ShimWorker.AllInstances.LogNonCriticalErrorExceptionStringEnumsApplicationsStringInt32String = (a, b, c, d, e, f, g) => workerSuccess
                ? AffectedCountPositive
                : AffectedCountNegative;

            // Act
            var result = _testEntity.LogNonCriticalError(Guid.Empty, exception, SampleString, ApplicationEnum.ADMS_Engine);

            // Assert
            if (workerSuccess)
            {
                VerifySuccessResponse(result, AffectedCountPositive);
            }
            else
            {
                VerifyErrorResponse(result, AffectedCountNegative);
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void LogNonCriticalErrorNote_WithNote_ReturnsExpectedResponse(bool workerSuccess)
        {
            // Arrange
            ShimWorker.AllInstances.LogNonCriticalErrorNoteStringStringEnumsApplicationsInt32String = (a, b, c, d, e, f) => workerSuccess
                ? AffectedCountPositive
                : AffectedCountNegative;

            // Act
            var result = _testEntity.LogNonCriticalErrorNote(Guid.Empty, SampleString, SampleString, ApplicationEnum.ADMS_Engine);

            // Assert
            if (workerSuccess)
            {
                VerifySuccessResponse(result, AffectedCountPositive);
            }
            else
            {
                VerifyErrorResponse(result, AffectedCountNegative);
            }
        }
    }
}

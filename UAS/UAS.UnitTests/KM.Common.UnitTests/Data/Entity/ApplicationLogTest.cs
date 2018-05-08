using System;
using KM.Common.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using KM.Common.Entity.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KM.Common.Tests.Data.Entity
{
    [TestFixture]
    public class ApplicationLogTest
    {
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
        public void LogCriticalError_AllParameterSet_ApplicationLogWritten()
        {
            // Arrange
            const string expectedSourceMethod = "FrameworkSubgen.Data.Save";
            const Application.Applications expectedApplications = Application.Applications.GD_Services;
            const string expectedNote = "RandomNote";
            const int expectedCharityId = 42;
            const int expectedCustomerId = 24;
            var expecedException = new AggregateException("MySuperAggregateException");
            ApplicationLog applicationLog = null;

            ShimApplicationLog.SaveApplicationLogRef = (ref ApplicationLog appLog) =>
                {
                    applicationLog = appLog;
                    return true;
                };

            // Act
            ApplicationLog.LogCriticalError(
                expecedException,
                expectedSourceMethod,
                expectedApplications,
                expectedNote,
                expectedCharityId,
                expectedCustomerId);

            // Assert
            applicationLog.ShouldSatisfyAllConditions(
                () => applicationLog.SeverityID.ShouldBe((int)Severity.SeverityLevel.Critical),
                () => applicationLog.NotificationSent?.ShouldBeFalse(),
                () => applicationLog.IsBug.HasValue.ShouldBeTrue(),
                () => applicationLog.IsBug?.ShouldBeTrue(),
                () => applicationLog.IsUserSubmitted.HasValue.ShouldBeTrue(),
                () => applicationLog.IsUserSubmitted?.ShouldBeFalse(),
                () => applicationLog.Exception.ShouldBe(ApplicationLog.FormatException(expecedException)),
                () => applicationLog.ApplicationID.ShouldBe((int)expectedApplications),
                () => applicationLog.SourceMethod.ShouldBe(expectedSourceMethod),
                () => applicationLog.LogNote.ShouldBe(expectedNote),
                () => applicationLog.GDCharityID.HasValue.ShouldBeTrue(),
                () => applicationLog.GDCharityID?.ShouldBe(expectedCharityId),
                () => applicationLog.ECNCustomerID.HasValue.ShouldBeTrue(),
                () => applicationLog.ECNCustomerID?.ShouldBe(expectedCustomerId));
        }

        [Test]
        public void LogCriticalError_AllParameterSetApplicationId_ApplicationLogWritten()
        {
            // Arrange
            const string expectedSourceMethod = "FrameworkSubgen.Data.Save";
            const Application.Applications expectedApplications = Application.Applications.GD_Services;
            const string expectedNote = "RandomNote";
            const int expectedCharityId = 42;
            const int expectedCustomerId = 24;
            const int logId = 666;
            var expecedException = new AggregateException("MySuperAggregateException");
            ApplicationLog applicationLog = null;

            ShimApplicationLog.SaveApplicationLogRef = (ref ApplicationLog appLog) =>
                {
                    applicationLog = appLog;
                    applicationLog.LogID = logId;
                    return true;
                };

            // Act
            var result = ApplicationLog.LogCriticalError(
                expecedException,
                expectedSourceMethod,
                (int)expectedApplications,
                expectedNote,
                expectedCharityId,
                expectedCustomerId);

            // Assert
            applicationLog.ShouldSatisfyAllConditions(
                () => result.ShouldBe(logId),
                () => applicationLog.SeverityID.ShouldBe((int)Severity.SeverityLevel.Critical),
                () => applicationLog.NotificationSent?.ShouldBeFalse(),
                () => applicationLog.IsBug.HasValue.ShouldBeTrue(),
                () => applicationLog.IsBug?.ShouldBeTrue(),
                () => applicationLog.IsUserSubmitted.HasValue.ShouldBeTrue(),
                () => applicationLog.IsUserSubmitted?.ShouldBeFalse(),
                () => applicationLog.Exception.ShouldBe(ApplicationLog.FormatException(expecedException)),
                () => applicationLog.ApplicationID.ShouldBe((int)expectedApplications),
                () => applicationLog.SourceMethod.ShouldBe(expectedSourceMethod),
                () => applicationLog.LogNote.ShouldBe(expectedNote),
                () => applicationLog.GDCharityID.HasValue.ShouldBeTrue(),
                () => applicationLog.GDCharityID?.ShouldBe(expectedCharityId),
                () => applicationLog.ECNCustomerID.HasValue.ShouldBeTrue(),
                () => applicationLog.ECNCustomerID?.ShouldBe(expectedCustomerId));
        }

        [Test]
        public void LogCriticalError_DefaultParameterNotSet_ApplicationLogWritten()
        {
            // Arrange
            const string expectedSourceMethod = "FrameworkSubgen.Data.Save";
            const Application.Applications expectedApplications = Application.Applications.GD_Services;
            var expecedException = new AggregateException("MySuperAggregateException");
            ApplicationLog applicationLog = null;

            ShimApplicationLog.SaveApplicationLogRef = (ref ApplicationLog appLog) =>
            {
                applicationLog = appLog;
                return true;
            };

            // Act
            ApplicationLog.LogCriticalError(expecedException, expectedSourceMethod, expectedApplications);

            // Assert
            applicationLog.ShouldSatisfyAllConditions(
                () => applicationLog.SeverityID.ShouldBe((int)Severity.SeverityLevel.Critical),
                () => applicationLog.NotificationSent?.ShouldBeFalse(),
                () => applicationLog.IsBug.HasValue.ShouldBeTrue(),
                () => applicationLog.IsBug?.ShouldBeTrue(),
                () => applicationLog.IsUserSubmitted.HasValue.ShouldBeTrue(),
                () => applicationLog.IsUserSubmitted?.ShouldBeFalse(),
                () => applicationLog.Exception.ShouldBe(ApplicationLog.FormatException(expecedException)),
                () => applicationLog.ApplicationID.ShouldBe((int)expectedApplications),
                () => applicationLog.SourceMethod.ShouldBe(expectedSourceMethod),
                () => applicationLog.LogNote.ShouldBe(string.Empty),
                () => applicationLog.GDCharityID.HasValue.ShouldBeTrue(),
                () => applicationLog.GDCharityID?.ShouldBe(-1),
                () => applicationLog.ECNCustomerID.HasValue.ShouldBeTrue(),
                () => applicationLog.ECNCustomerID?.ShouldBe(-1));
        }

        [Test]
        public void LogNonCriticalError_AllParameterSet_ApplicationLogWritten()
        {
            // Arrange
            const string expectedSourceMethod = "FrameworkSubgen.Data.Save";
            const Application.Applications expectedApplications = Application.Applications.GD_Services;
            const string expectedNote = "RandomNote";
            const int expectedCharityId = 42;
            const int expectedCustomerId = 24;
            var expecedException = new AggregateException("MySuperAggregateException");
            ApplicationLog applicationLog = null;

            ShimApplicationLog.SaveApplicationLogRef = (ref ApplicationLog appLog) =>
            {
                applicationLog = appLog;
                return true;
            };

            // Act
            ApplicationLog.LogNonCriticalError(
                expecedException,
                expectedSourceMethod,
                (int)expectedApplications,
                expectedNote,
                expectedCharityId,
                expectedCustomerId);

            // Assert
            applicationLog.ShouldSatisfyAllConditions(
                () => applicationLog.SeverityID.ShouldBe((int)Severity.SeverityLevel.Non_Critical),
                () => applicationLog.NotificationSent?.ShouldBeFalse(),
                () => applicationLog.IsBug.HasValue.ShouldBeTrue(),
                () => applicationLog.IsBug?.ShouldBeTrue(),
                () => applicationLog.IsUserSubmitted.HasValue.ShouldBeTrue(),
                () => applicationLog.IsUserSubmitted?.ShouldBeFalse(),
                () => applicationLog.Exception.ShouldBe(ApplicationLog.FormatException(expecedException)),
                () => applicationLog.ApplicationID.ShouldBe((int)expectedApplications),
                () => applicationLog.SourceMethod.ShouldBe(expectedSourceMethod),
                () => applicationLog.LogNote.ShouldBe(expectedNote),
                () => applicationLog.GDCharityID.HasValue.ShouldBeTrue(),
                () => applicationLog.GDCharityID?.ShouldBe(expectedCharityId),
                () => applicationLog.ECNCustomerID.HasValue.ShouldBeTrue(),
                () => applicationLog.ECNCustomerID?.ShouldBe(expectedCustomerId));
        }

        [Test]
        public void LogNonCriticalError_DefaultParameterNotSet_ApplicationLogWritten()
        {
            // Arrange
            const string expectedSourceMethod = "FrameworkSubgen.Data.Save";
            const Application.Applications expectedApplications = Application.Applications.GD_Services;
            var expecedException = new AggregateException("MySuperAggregateException");
            ApplicationLog applicationLog = null;

            ShimApplicationLog.SaveApplicationLogRef = (ref ApplicationLog appLog) =>
            {
                applicationLog = appLog;
                return true;
            };

            // Act
            ApplicationLog.LogNonCriticalError(expecedException, expectedSourceMethod, (int)expectedApplications);

            // Assert
            applicationLog.ShouldSatisfyAllConditions(
                () => applicationLog.SeverityID.ShouldBe((int)Severity.SeverityLevel.Non_Critical),
                () => applicationLog.NotificationSent?.ShouldBeFalse(),
                () => applicationLog.IsBug.HasValue.ShouldBeTrue(),
                () => applicationLog.IsBug?.ShouldBeTrue(),
                () => applicationLog.IsUserSubmitted.HasValue.ShouldBeTrue(),
                () => applicationLog.IsUserSubmitted?.ShouldBeFalse(),
                () => applicationLog.Exception.ShouldBe(ApplicationLog.FormatException(expecedException)),
                () => applicationLog.ApplicationID.ShouldBe((int)expectedApplications),
                () => applicationLog.SourceMethod.ShouldBe(expectedSourceMethod),
                () => applicationLog.LogNote.ShouldBe(string.Empty),
                () => applicationLog.GDCharityID.HasValue.ShouldBeTrue(),
                () => applicationLog.GDCharityID?.ShouldBe(-1),
                () => applicationLog.ECNCustomerID.HasValue.ShouldBeTrue(),
                () => applicationLog.ECNCustomerID?.ShouldBe(-1));
        }

        [Test]
        public void LogNonCriticalError_DefaultParameterNotSetException_ApplicationLogWritten()
        {
            // Arrange
            const string expectedSourceMethod = "FrameworkSubgen.Data.Save";
            const Application.Applications expectedApplications = Application.Applications.GD_Services;
            const int logId = 666;
            var expecedException = "MySuperAggregateException";
            ApplicationLog applicationLog = null;

            ShimApplicationLog.SaveApplicationLogRef = (ref ApplicationLog appLog) =>
            {
                applicationLog = appLog;
                applicationLog.LogID = logId;
                return true;
            };

            // Act
            var result = ApplicationLog.LogNonCriticalError(expecedException, expectedSourceMethod, (int)expectedApplications);

            // Assert
            applicationLog.ShouldSatisfyAllConditions(
                () => result.ShouldBe(logId),
                () => applicationLog.SeverityID.ShouldBe((int)Severity.SeverityLevel.Non_Critical),
                () => applicationLog.NotificationSent?.ShouldBeFalse(),
                () => applicationLog.IsBug.HasValue.ShouldBeTrue(),
                () => applicationLog.IsBug?.ShouldBeTrue(),
                () => applicationLog.IsUserSubmitted.HasValue.ShouldBeTrue(),
                () => applicationLog.IsUserSubmitted?.ShouldBeFalse(),
                () => applicationLog.Exception.ShouldBe(expecedException),
                () => applicationLog.ApplicationID.ShouldBe((int)expectedApplications),
                () => applicationLog.SourceMethod.ShouldBe(expectedSourceMethod),
                () => applicationLog.LogNote.ShouldBe(string.Empty),
                () => applicationLog.GDCharityID.HasValue.ShouldBeTrue(),
                () => applicationLog.GDCharityID?.ShouldBe(-1),
                () => applicationLog.ECNCustomerID.HasValue.ShouldBeTrue(),
                () => applicationLog.ECNCustomerID?.ShouldBe(-1));
        }
    }
}

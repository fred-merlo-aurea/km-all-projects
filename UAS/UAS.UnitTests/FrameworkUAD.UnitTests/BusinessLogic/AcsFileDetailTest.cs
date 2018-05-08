using System;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.BusinessLogic;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using EntityUserLog = KMPlatform.Entity.UserLog;

namespace FrameworkUAD.UnitTests.BusinessLogic
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AcsFileDetailTest
    {
        private readonly AcsFileDetail _acsFileDetail = new AcsFileDetail();
        private readonly DateTime DefaultDateTime = DateTime.Now;
        private const string JobAcsUpdateSubscriberAddress = "job_ACS_UpdateSubscriberAddress";
        private const string ObjectSubscriber = "Subscriber";
        private int _userLogTypeId = 10;
        private int _appId = 20;
        private int _userId = 30;
        private IDisposable _context;

        [SetUp]
        public void Initialize()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void CleanUp() => _context?.Dispose();

        [Test]
        public void AcsFileDetail_SaveUserLog_ShoudlSaveUserLog_ReturnsNewUserLogId()
        {
            // Arrange
            var userLogResult = new EntityUserLog();
            ShimUserLog.AllInstances.SaveUserLog = (_, userLog) =>
            {
                userLogResult = userLog;
                return 100;
            };

            // Act
            var result = _acsFileDetail.SaveUserLog(_userLogTypeId, _appId, _userId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => userLogResult.ApplicationID.ShouldBe(_appId),
                () => userLogResult.DateCreated.ToShortDateString().ShouldBe(DefaultDateTime.ToShortDateString()),
                () => userLogResult.FromObjectValues.ShouldBe(JobAcsUpdateSubscriberAddress),
                () => userLogResult.Object.ShouldBe(ObjectSubscriber),
                () => userLogResult.ToObjectValues.ShouldBe(string.Empty),
                () => userLogResult.UserID.ShouldBe(_userId),
                () => userLogResult.UserLogTypeID.ShouldBe(_userLogTypeId));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ECN.TestHelpers;
using ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;
using Assert = NUnit.Framework.Assert;
using Blast = ECN_Framework_BusinessLayer.Communicator.Blast;
using CampaignItemEntity = ECN_Framework_Entities.Communicator.CampaignItem;
using CommBlastAbstract = ECN_Framework_Entities.Communicator.BlastAbstract;
using CommBlastRegular = ECN_Framework_Entities.Communicator.BlastRegular;
using CommEnums = ECN_Framework_Common.Objects.Communicator.Enums;
using CommTestBlast = ECN_Framework_Entities.Communicator.CampaignItemTestBlast;
using KmUser = KMPlatform.Entity.User;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class BlastTest
    {
        private const string SetBlastRegularPropertiesMethod = "SetBlastRegularProperties";
        private const string SetBlastTypePropertyMethod = "SetBlastTypeProperty";
        private const string YesCode = "Y";

        private CampaignItemEntity _campaignItemEntity;
        private CommTestBlast _ciTestBlast;
        private CommBlastRegular _blastRegular;
        private bool _useCampaignItem;

        [Test]
        public void SetBlastRegularProperties_WhenUserIsNull_ThrowsException()
        {
            // Arrange
            _user = null;

            // Act
            var exception = Assert.Throws<TargetInvocationException>(
                () => CallMethodForSetBlastRegularProperties(new object[]
                {
                    _user,
                    _campaignItemEntity,
                    _ciTestBlast,
                    _useCampaignItem
                }, SetBlastRegularPropertiesMethod));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void SetBlastRegularProperties_WhenCampaignItemEntityIsNull_ThrowsException()
        {
            // Arrange
            _user = new KmUser();
            _campaignItemEntity = null;

            // Act
            var exception = Assert.Throws<TargetInvocationException>(
                () => CallMethodForSetBlastRegularProperties(new object[]
                {
                    _user,
                    _campaignItemEntity,
                    _ciTestBlast,
                    _useCampaignItem
                }, SetBlastRegularPropertiesMethod));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void SetBlastRegularProperties_WhenCiTestBlastIsNull_ThrowsException()
        {
            // Arrange
            _user = new KmUser();
            _campaignItemEntity = new CampaignItemEntity();
            _ciTestBlast = null;

            // Act
            var exception = Assert.Throws<TargetInvocationException>(
                () => CallMethodForSetBlastRegularProperties(new object[]
                {
                    _user,
                    _campaignItemEntity,
                    _ciTestBlast,
                    _useCampaignItem
                }, SetBlastRegularPropertiesMethod));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void SetBlastRegularProperties_WhenUseCampaignItemIsTrue_ReturnsCommBlastAbstract()
        {
            // Arrange
            _user = CreateKmUser();
            _campaignItemEntity = CreateCampaignItemEntity();
            _ciTestBlast = CreateCommTestBlast();
            _useCampaignItem = true;

            // Act
            var blastResult = CallMethodForSetBlastRegularProperties(new object[]
            {
                _user,
                _campaignItemEntity,
                _ciTestBlast,
                _useCampaignItem
            }, SetBlastRegularPropertiesMethod) as CommBlastAbstract;

            // Assert
            blastResult.ShouldNotBeNull();
            blastResult.ShouldSatisfyAllConditions(
                () => blastResult.CreatedUserID.ShouldBe(_user.UserID),
                () => blastResult.CustomerID.ShouldBe(_user.CustomerID),
                () => blastResult.TestBlast.ShouldBe(YesCode),
                () => blastResult.StatusCode.ShouldBe(CommEnums.BlastStatusCode.Pending.ToString()),
                () => blastResult.SendTime.ShouldNotBeNull(),
                () => blastResult.BlastScheduleID.ShouldBe(_campaignItemEntity.BlastScheduleID),
                () => blastResult.OverrideAmount.ShouldBe(_campaignItemEntity.OverrideAmount),
                () => blastResult.OverrideIsAmount.ShouldBe(_campaignItemEntity.OverrideIsAmount),
                () => blastResult.HasEmailPreview.ShouldBe(_ciTestBlast.HasEmailPreview),
                () => blastResult.GroupID.ShouldBe(_ciTestBlast.GroupID),
                () => blastResult.EnableCacheBuster.ShouldBe(_campaignItemEntity.EnableCacheBuster),
                () => blastResult.EmailSubject.ShouldBe(_campaignItemEntity.BlastList[0].EmailSubject),
                () => blastResult.EmailFrom.ShouldBe(_campaignItemEntity.FromEmail),
                () => blastResult.EmailFromName.ShouldBe(_campaignItemEntity.FromName),
                () => blastResult.ReplyTo.ShouldBe(_campaignItemEntity.ReplyTo),
                () => blastResult.LayoutID.ShouldBe(_campaignItemEntity.BlastList[0].LayoutID),
                () => blastResult.AddOptOuts_to_MS.ShouldBe(_campaignItemEntity.BlastList[0].AddOptOuts_to_MS),
                () => blastResult.DynamicFromEmail.ShouldBe(_campaignItemEntity.BlastList[0].DynamicFromEmail),
                () => blastResult.DynamicFromName.ShouldBe(_campaignItemEntity.BlastList[0].DynamicFromName),
                () => blastResult.DynamicReplyToEmail.ShouldBe(_campaignItemEntity.BlastList[0].DynamicReplyTo));
        }

        [Test]
        public void SetBlastRegularProperties_WhenUseCampaignItemIsFalse_ReturnsCommBlastAbstract()
        {
            // Arrange
            _user = CreateKmUser();
            _campaignItemEntity = CreateCampaignItemEntity();
            _ciTestBlast = CreateCommTestBlast();
            _useCampaignItem = false;

            // Act
            var blastResult = CallMethodForSetBlastRegularProperties(new object[]
            {
                _user,
                _campaignItemEntity,
                _ciTestBlast,
                _useCampaignItem
            }, SetBlastRegularPropertiesMethod) as CommBlastAbstract;

            // Assert
            blastResult.ShouldNotBeNull();
            blastResult.ShouldSatisfyAllConditions(
                () => blastResult.CreatedUserID.ShouldBe(_user.UserID),
                () => blastResult.CustomerID.ShouldBe(_user.CustomerID),
                () => blastResult.TestBlast.ShouldBe(YesCode),
                () => blastResult.StatusCode.ShouldBe(CommEnums.BlastStatusCode.Pending.ToString()),
                () => blastResult.SendTime.ShouldNotBeNull(),
                () => blastResult.BlastScheduleID.ShouldBe(_campaignItemEntity.BlastScheduleID),
                () => blastResult.OverrideAmount.ShouldBe(_campaignItemEntity.OverrideAmount),
                () => blastResult.OverrideIsAmount.ShouldBe(_campaignItemEntity.OverrideIsAmount),
                () => blastResult.HasEmailPreview.ShouldBe(_ciTestBlast.HasEmailPreview),
                () => blastResult.GroupID.ShouldBe(_ciTestBlast.GroupID),
                () => blastResult.EnableCacheBuster.ShouldBe(_campaignItemEntity.EnableCacheBuster),
                () => blastResult.EmailSubject.ShouldBe(_ciTestBlast.EmailSubject),
                () => blastResult.EmailFrom.ShouldBe(_ciTestBlast.FromEmail),
                () => blastResult.EmailFromName.ShouldBe(_ciTestBlast.FromName),
                () => blastResult.ReplyTo.ShouldBe(_ciTestBlast.ReplyTo),
                () => blastResult.LayoutID.ShouldBe(_ciTestBlast.LayoutID),
                () => blastResult.AddOptOuts_to_MS.ShouldBe(false),
                () => blastResult.DynamicFromEmail.ShouldBeNullOrWhiteSpace(),
                () => blastResult.DynamicFromName.ShouldBeNullOrWhiteSpace(),
                () => blastResult.DynamicReplyToEmail.ShouldBeNullOrWhiteSpace());
        }

        [Test]
        public void SetBlastTypeProperty_WhenCampaignItemEntityIsNull_ThrowsException()
        {
            // Arrange
            _campaignItemEntity = null;

            // Act
            var exception = Assert.Throws<TargetInvocationException>(
                () => CallMethodForSetBlastRegularProperties(new object[]
                {
                    _campaignItemEntity,
                    _ciTestBlast,
                    _blastRegular
                }, SetBlastTypePropertyMethod));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void SetBlastTypeProperty_WhenCommTestBlastIsNull_ThrowsException()
        {
            // Arrange
            _campaignItemEntity = new CampaignItemEntity();
            _ciTestBlast = null;

            // Act
            var exception = Assert.Throws<TargetInvocationException>(
                () => CallMethodForSetBlastRegularProperties(new object[]
                {
                    _campaignItemEntity,
                    _ciTestBlast,
                    _blastRegular
                }, SetBlastTypePropertyMethod));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void SetBlastTypeProperty_WhenCommBlastRegularIsNull_ThrowsException()
        {
            // Arrange
            _campaignItemEntity = new CampaignItemEntity();
            _ciTestBlast = new CommTestBlast();
            _blastRegular = null;

            // Act
            var exception = Assert.Throws<TargetInvocationException>(
                () => CallMethodForSetBlastRegularProperties(new object[]
                {
                    _campaignItemEntity,
                    _ciTestBlast,
                    _blastRegular
                }, SetBlastTypePropertyMethod));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void SetBlastTypeProperty_WhenCampaignItemTypeIsRegularAndCampaignItemTestBlastTypeIsHtml_ShouldSetBlastRegular()
        {
            // Arrange
            _campaignItemEntity = new CampaignItemEntity();
            _ciTestBlast = new CommTestBlast();
            _blastRegular = new CommBlastRegular();

            _campaignItemEntity.CampaignItemType = CommEnums.CampaignItemType.Regular.ToString();
            _ciTestBlast.CampaignItemTestBlastType = CommEnums.CampaignItemFormatType.HTML.ToString();

            // Act
            CallMethodForSetBlastRegularProperties(new object[]
            {
                _campaignItemEntity,
                _ciTestBlast,
                _blastRegular
            }, SetBlastTypePropertyMethod);

            // Assert;
            _blastRegular.ShouldSatisfyAllConditions(
                () => _blastRegular.BlastType.ShouldNotBeNullOrWhiteSpace(),
                () => _blastRegular.BlastType.ShouldBe(CommEnums.BlastType.HTML.ToString()));
        }

        [Test]
        public void SetBlastTypeProperty_WhenCampaignItemTypeIsRegularAndCampaignItemTestBlastTypeIsText_ShouldSetBlastRegular()
        {
            // Arrange
            _campaignItemEntity = new CampaignItemEntity();
            _ciTestBlast = new CommTestBlast();
            _blastRegular = new CommBlastRegular();

            _campaignItemEntity.CampaignItemType = CommEnums.CampaignItemType.Regular.ToString();
            _ciTestBlast.CampaignItemTestBlastType = CommEnums.CampaignItemFormatType.TEXT.ToString();

            // Act
            CallMethodForSetBlastRegularProperties(new object[]
            {
                _campaignItemEntity,
                _ciTestBlast,
                _blastRegular
            }, SetBlastTypePropertyMethod);

            // Assert;
            _blastRegular.ShouldSatisfyAllConditions(
                () => _blastRegular.BlastType.ShouldNotBeNullOrWhiteSpace(),
                () => _blastRegular.BlastType.ShouldBe(CommEnums.BlastType.TEXT.ToString()));
        }

        [Test]
        public void SetBlastTypeProperty_WhenCampaignItemTypeIsAb_ShouldSetBlastRegular()
        {
            // Arrange
            _campaignItemEntity = new CampaignItemEntity();
            _ciTestBlast = new CommTestBlast();
            _blastRegular = new CommBlastRegular();

            _campaignItemEntity.CampaignItemType = CommEnums.CampaignItemType.AB.ToString();

            // Act
            CallMethodForSetBlastRegularProperties(new object[]
            {
                _campaignItemEntity,
                _ciTestBlast,
                _blastRegular
            }, SetBlastTypePropertyMethod);

            // Assert;
            _blastRegular.ShouldSatisfyAllConditions(
                () => _blastRegular.BlastType.ShouldNotBeNullOrWhiteSpace(),
                () => _blastRegular.BlastType.ShouldBe(CommEnums.BlastType.Sample.ToString()));
        }

        [Test]
        public void SetBlastTypeProperty_WhenCampaignItemTypeIsChampion_ShouldSetBlastRegular()
        {
            // Arrange
            _campaignItemEntity = new CampaignItemEntity();
            _ciTestBlast = new CommTestBlast();
            _blastRegular = new CommBlastRegular();

            _campaignItemEntity.CampaignItemType = CommEnums.CampaignItemType.Champion.ToString();

            // Act
            CallMethodForSetBlastRegularProperties(new object[]
            {
                _campaignItemEntity,
                _ciTestBlast,
                _blastRegular
            }, SetBlastTypePropertyMethod);

            // Assert;
            _blastRegular.ShouldSatisfyAllConditions(
                () => _blastRegular.BlastType.ShouldNotBeNullOrWhiteSpace(),
                () => _blastRegular.BlastType.ShouldBe(CommEnums.BlastType.Champion.ToString()));
        }

        [Test]
        public void SetBlastTypeProperty_WhenCampaignItemTypeIsSms_ShouldSetBlastRegular()
        {
            // Arrange
            _campaignItemEntity = new CampaignItemEntity();
            _ciTestBlast = new CommTestBlast();
            _blastRegular = new CommBlastRegular();

            _campaignItemEntity.CampaignItemType = CommEnums.CampaignItemType.SMS.ToString();

            // Act
            CallMethodForSetBlastRegularProperties(new object[]
            {
                _campaignItemEntity,
                _ciTestBlast,
                _blastRegular
            }, SetBlastTypePropertyMethod);

            // Assert;
            _blastRegular.ShouldSatisfyAllConditions(
                () => _blastRegular.BlastType.ShouldNotBeNullOrWhiteSpace(),
                () => _blastRegular.BlastType.ShouldBe(CommEnums.BlastType.SMS.ToString()));
        }

        private static object CallMethodForSetBlastRegularProperties(object[] parametersValues, string methodName)
        {
            var methodInfo = typeof(Blast).GetAllMethods()
                .FirstOrDefault(x => x.Name == methodName && x.IsPrivate);

            return methodInfo?.Invoke(null, parametersValues);
        }

        private static KmUser CreateKmUser()
        {
            return new KmUser
            {
                UserID = 20,
                CustomerID = 30
            };
        }

        private static CampaignItemEntity CreateCampaignItemEntity()
        {
            return new CampaignItemEntity
            {
                BlastScheduleID = 1,
                OverrideAmount = 2,
                OverrideIsAmount = true,
                EnableCacheBuster = true,
                FromEmail = "From Email 1",
                FromName = "From Name 1",
                ReplyTo = "Reply To 1",

                BlastList = new List<CampaignItemBlast>
                {
                    new CampaignItemBlast
                    {
                        EmailSubject = "EmailSubject 1",
                        LayoutID = 3,
                        AddOptOuts_to_MS = false,
                        DynamicFromEmail = "Dynamic From Email 1",
                        DynamicFromName = "Dynamic From Name 1",
                        DynamicReplyTo = "Dynamic Reply To 1"
                    }
                }
            };
        }

        private static CommTestBlast CreateCommTestBlast()
        {
            return new CommTestBlast
            {
                HasEmailPreview = true,
                GroupID = 4,
                EmailSubject = "Email Subject 2",
                FromEmail = "From Email 2",
                FromName = "From Name 2",
                ReplyTo = "Reply To 2",
                LayoutID = 5
            };
        }
    }
}

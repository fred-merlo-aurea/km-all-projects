using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Transactions.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN.Tests.Helpers;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using DataLayerFakes = ECN_Framework_DataLayer.Communicator.Fakes;
using Entities = ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CampaignItemTest
    {
        private const string DummyString = "DummyString";
        private const string MethodValidateUseAmbientTransaction = "Validate_UseAmbientTransaction";
        private Entities.CampaignItem _campaignItem;
        private User _user;
        private IDisposable _shimObject;

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void Validate_UseAmbientTransaction_Success_CampaignItemIsValidated()
        {
            // Arrange
            var isValidated = false;
            SetupShimsForValidate(true);
            DataLayerFakes.ShimCampaignItem.ExistsInt32Int32Int32 = (a, b, c) =>
            {
                isValidated = true;
                return true;
            };

            // Act	
            CampaignItem.Validate_UseAmbientTransaction(_campaignItem, _user);

            // Assert
            isValidated.ShouldBeTrue();
        }

        [Test]
        public void Validate_UseAmbientTransaction_WithNullPropertiesAndCampaignItemIdIsZero_ThrowsException()
        {
            // Arrange
            SetupShimsForValidate(false);
            SetupCampaignItemWithNullProperties();

            // Act //Assert
            var exception = Should.Throw<ECNException>(() =>
            {
                CampaignItem.Validate_UseAmbientTransaction(_campaignItem, _user);
            });
        }

        [Test]
        public void Validate_UseAmbientTransaction_WithNullPropertiesAndCampaignItemIdIsGreaterZero_ThrowsException()
        {
            // Arrange
            SetupShimsForValidate(false);
            SetupCampaignItemWithNullProperties();
            _campaignItem.CampaignItemType = CampaignItemType.AB.ToString();
            _campaignItem.CampaignItemID = 1;

            // Act //Assert
            var exception = Should.Throw<ECNException>(() =>
            {
                CampaignItem.Validate_UseAmbientTransaction(_campaignItem, _user);
            });
        }

        [Test]
        public void Validate_NoAccessCheck_Success_CampaignItemIsValidated()
        {
            // Arrange
            var isValidated = false;
            SetupShimsForValidate(true);
            DataLayerFakes.ShimCampaignItem.ExistsInt32Int32Int32 = (a, b, c) =>
            {
                isValidated = true;
                return true;
            };

            // Act	
            CampaignItem.Validate_NoAccessCheck(_campaignItem, _user);

            // Assert
            isValidated.ShouldBeTrue();
        }

        [Test]
        public void Validate_NoAccessCheck_WithNullPropertiesAndCampaignItemIdIsZero_ThrowsException()
        {
            // Arrange
            SetupShimsForValidate(false);
            SetupCampaignItemWithNullProperties();

            // Act //Assert
            var exception = Should.Throw<ECNException>(() =>
            {
                CampaignItem.Validate_NoAccessCheck(_campaignItem, _user);
            });
        }

        [Test]
        public void Validate_NoAccessCheck_WithNullPropertiesAndCampaignItemIdIsGreaterZero_ThrowsException()
        {
            // Arrange
            SetupShimsForValidate(false);
            SetupCampaignItemWithNullProperties();
            _campaignItem.CampaignItemType = CampaignItemType.AB.ToString();
            _campaignItem.CampaignItemID = 1;

            // Act //Assert
            var exception = Should.Throw<ECNException>(() =>
            {
                CampaignItem.Validate_NoAccessCheck(_campaignItem, _user);
            });
        }

        [Test]
        public void Validate_WithNullPropertiesAndCampaignItemIdIsZero_ThrowsException()
        {
            // Arrange
            SetupShimsForValidate(false);
            SetupCampaignItemWithNullProperties();

            // Act //Assert
            var exception = Should.Throw<ECNException>(() =>
            {
                CampaignItem.Validate(_campaignItem, _user);
            });
        }

        [Test]
        public void Validate_WithNullPropertiesAndCampaignItemIdIsGreaterZero_ThrowsException()
        {
            // Arrange
            SetupShimsForValidate(false);
            SetupCampaignItemWithNullProperties();
            _campaignItem.CampaignItemType = CampaignItemType.AB.ToString();
            _campaignItem.CampaignItemID = 1;

            // Act //Assert
            var exception = Should.Throw<ECNException>(() =>
            {
                CampaignItem.Validate(_campaignItem, _user);
            });
        }

        [Test]
        public void Validate_Success_CampaignItemIsValidated()
        {
            // Arrange
            var isValidated = false; 
            SetupShimsForValidate(true);
            DataLayerFakes.ShimCampaignItem.ExistsInt32Int32Int32 = (a, b, c) =>
            {
                isValidated = true;
                return true;
            };

            // Act	
            CampaignItem.Validate(_campaignItem, _user);

            // Assert
            isValidated.ShouldBeTrue();
        }

        [Test]
        public void CopyCampaignItem_OnValidCall_ReturnPositiveNumber()
        {
            // Arrange
            var campaignItemID = 1;
            var user = new User
            {
                UserID = 1,
                CustomerID = 1
            };
            DefineShimsForCampaignItem(campaignItemID);
            DefineShimsForSave(campaignItemID);
            ShimTransactionScope.Constructor = (obj) => { };
            ShimTransactionScope.AllInstances.Complete = (obj) => { };
            ShimTransactionScope.AllInstances.Dispose = (obj) => { };

            // Act	
            var actualResult = CampaignItem.CopyCampaignItem(campaignItemID, user);

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.ShouldBe(campaignItemID);
        }

        private void DefineShimsForCampaignItem(int campaignItemID)
        {
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (id, usr, child) =>
            {
                return new Entities::CampaignItem
                {
                    SampleID = 1,
                    CampaignItemType = CampaignItemType.AB.ToString()
                };
            };
            ShimCampaignItemSuppression.GetByCampaignItemIDInt32UserBoolean = (id, usr, child) =>
            {
                return new List<Entities::CampaignItemSuppression>
                {
                    new Entities::CampaignItemSuppression
                    {
                        CampaignItemID = campaignItemID
                    }
                };
            };
            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (id, usr, child) =>
            {
                return new List<Entities::CampaignItemBlast>
                {
                    new Entities::CampaignItemBlast
                    {
                        CampaignItemBlastID = 1
                    }
                };
            };
            ShimCampaignItemOptOutGroup.GetByCampaignItemIDInt32User = (id, usr) =>
            {
                return new List<Entities::CampaignItemOptOutGroup>
                {
                    new Entities::CampaignItemOptOutGroup
                    {
                        CampaignItemID = campaignItemID
                    }
                };
            };
            ShimCampaignItemLinkTracking.GetByCampaignItemIDInt32User = (id, usr) =>
            {
                return new List<Entities::CampaignItemLinkTracking>
                {
                    new Entities::CampaignItemLinkTracking
                    {
                        CampaignItemID = campaignItemID
                    }
                };
            };
            ShimCampaignItemSocialMedia.GetByCampaignItemIDInt32 = (id) =>
            {
                return new List<Entities::CampaignItemSocialMedia>
                {
                    new Entities::CampaignItemSocialMedia
                    {
                        CampaignItemID = campaignItemID
                    }
                };
            };
            ShimCampaignItemLinkTracking.GetByCampaignItemIDInt32User = (id, usr) =>
            {
                return new List<Entities::CampaignItemLinkTracking>
                {
                    new Entities::CampaignItemLinkTracking
                    {
                        CampaignItemID = campaignItemID
                    }
                };
            };
            ShimCampaignItemSocialMedia.GetByCampaignItemIDInt32 = (id) =>
            {
                return new List<Entities::CampaignItemSocialMedia>
                {
                    new Entities::CampaignItemSocialMedia
                    {
                        CampaignItemID = campaignItemID,
                        SimpleShareDetailID = 1
                    }
                };
            };
            ShimSample.GetBySampleIDInt32User = (id, usr) => new Entities::Sample();
            ShimCampaignItemBlastRefBlast.GetByCampaignItemBlastIDInt32User = (id, usr) => null;
            ShimSimpleShareDetail.GetBySimpleShareDetailIDInt32 = (id) =>
            {
                return new Entities::SimpleShareDetail
                {
                    SocialMediaAuthID = 1,
                    SocialMediaID = 1
                };
            };
            ShimCampaignItemMetaTag.GetByCampaignItemIDInt32 = (id) =>
            {
                return new List<Entities::CampaignItemMetaTag>
                {
                    new Entities::CampaignItemMetaTag
                    {
                        CampaignItemID = campaignItemID
                    }
                };
            };
        }

        private void DefineShimsForSave(int campaignItemID)
        {
            ShimSample.SaveSampleUser = (obj, user) =>
            {
                obj.SampleID = 1;
            };
            ShimCampaignItem.SaveCampaignItemUser = (obj, user) =>
            {
                obj.CampaignItemID = campaignItemID;
                return campaignItemID;
            };
            ShimCampaignItemSuppression.SaveCampaignItemSuppressionUser = (obj, user) => 1;
            ShimCampaignItemBlast.SaveCampaignItemBlastUserBoolean = (obj, user, ignore) => 1;
            ShimCampaignItemOptOutGroup.SaveCampaignItemOptOutGroupUser = (obj, user) => { };
            ShimCampaignItemLinkTracking.SaveCampaignItemLinkTrackingUser = (obj, user) => 1;
            ShimCampaignItemSocialMedia.SaveCampaignItemSocialMedia = (obj) => 1;
            ShimCampaignItemMetaTag.SaveCampaignItemMetaTag = (obj) => 1;
            ShimSimpleShareDetail.SaveSimpleShareDetail = (obj) => 1;
        }

        private void SetupCampaignItemWithNullProperties()
        {
            ShimCampaign.ExistsInt32Int32 = (a, b) => true;
            ShimCampaign.Exists_UseAmbientTransactionInt32Int32 = (a, b) => true;
            ShimCampaignItem.ValidateScheduleCampaignItemUser = (a, b) => false;
            ShimCampaignItem.ValidateSchedule_NoAccessCheckCampaignItemUser = (a, b) => false;
            _campaignItem.CampaignItemName = string.Empty;
            _campaignItem.CampaignItemID = 0;
            _campaignItem.CreatedUserID = 0;
            _campaignItem.OverrideAmount = 0;
            _campaignItem.IsHidden = null;
            _campaignItem.CampaignItemType = DummyString;
            _campaignItem.CampaignItemFormatType = DummyString;
            _campaignItem.CampaignItemNameOriginal = string.Empty;
        }

        private void SetupShimsForValidate(bool forSuccess)
        {
            _campaignItem = ReflectionHelper.CreateInstance(typeof(Entities.CampaignItem));
            _campaignItem.CampaignItemType = CampaignItemType.Regular.ToString();
            _campaignItem.CampaignItemFormatType = CampaignItemFormatType.HTML.ToString();
            var campaignItemBlast = ReflectionHelper.CreateInstance(typeof(Entities.CampaignItemBlast));
            campaignItemBlast.CampaignItemBlastID = 1;
            _campaignItem.BlastList = new List<Entities.CampaignItemBlast> { campaignItemBlast };
            _user = new User
            {
                UserID = 1,
                CustomerID = 1
            };
            ShimCampaign.ExistsInt32Int32 = (a, b) => forSuccess;
            ShimCampaign.Exists_UseAmbientTransactionInt32Int32 = (a, b) => forSuccess;
            ShimCustomer.ExistsInt32 = (a) => forSuccess;
            ShimUser.ExistsInt32Int32 = (a, b) => forSuccess;
            ShimSample.ExistsInt32Int32 = (a, b) => forSuccess;
            ShimSample.Exists_UseAmbientTransactionInt32Int32 = (a, b) => forSuccess;
            ShimBlastSchedule.ExistsInt32 = (a) => forSuccess;
            ShimBlastSchedule.Exists_UseAmbientTransactionInt32 = (a) => forSuccess;
            ShimEmail.ExistsInt32Int32 = (a, b) => forSuccess;
            ShimEmail.IsValidEmailAddressString = (a) => forSuccess;
            ShimEmail.IsValidEmailAddress_UseAmbientTransactionString = (a) => forSuccess;
            ShimBlast.ActiveOrSentInt32Int32 = (a, b) => !forSuccess;
            ShimBlast.ActiveOrSent_UseAmbientTransactionInt32Int32 = (a, b) => !forSuccess;
            ShimCampaignItemBlast.GetByCampaignItemID_NoAccessCheckInt32Boolean = (a, b) => _campaignItem.BlastList;
            ShimCampaignItemBlast.GetByCampaignItemID_UseAmbientTransactionInt32UserBoolean = (a, b, c) => _campaignItem.BlastList;
            DataLayerFakes.ShimCampaignItem.ExistsInt32Int32Int32 = (a, b, c) => forSuccess;
        }
    }
}
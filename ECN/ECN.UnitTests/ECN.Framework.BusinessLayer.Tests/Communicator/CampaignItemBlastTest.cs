using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
using ShimPlatformUser = KM.Platform.Fakes.ShimUser;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CampaignItemBlastTest
    {
        private const string DummyString = "DummyString";
        private const string One = "1";
        private const int CampaignItemID = 1;
        private Entities.CampaignItemBlast _campaignItemBlast;
        private Entities.CampaignItem _campaignItem;
        private List<Entities.CampaignItemBlast> _campaignItemBlastList;
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
        public void Save_UseAmbientTransaction_Success_CampaignItemIsSaved()
        {
            // Arrange
            SetupShimsForSaveMethod();
            var campaignItemSaved = false;
            _campaignItem.CampaignItemType = CampaignItemType.Regular.ToString();
            ShimCampaignItemBlastRefBlast.SaveCampaignItemBlastRefBlastUserBoolean = (a, b, c) =>
            {
                campaignItemSaved = true;
                return 0;
            };

            // Act	
            CampaignItemBlast.Save_UseAmbientTransaction(CampaignItemID, _campaignItemBlastList, _user);
            _campaignItemBlastList[0].Filters[0].CampaignItemBlastID = 1;
            CampaignItemBlast.Save(_campaignItemBlastList[0], _user);
            _campaignItemBlastList[0].Filters[0].CampaignItemBlastID = 2;
            var result = CampaignItemBlast.Save_UseAmbientTransaction(_campaignItemBlastList[0], _user);

            // Assert
            _campaignItemBlast.ShouldSatisfyAllConditions(
                () => result.ShouldBe(0),
                () => campaignItemSaved.ShouldBeTrue());
        }

        [Test]
        public void Save_UseAmbientTransaction_GroupIdsAreDifferent_DuplicateCampaignItemBlastsAreDeleted()
        {
            // Arrange
            SetupShimsForSaveMethod();
            var campaignItemBlastDeleted = false;
            _campaignItem.CampaignItemType = CampaignItemType.Regular.ToString();
            _campaignItemBlast.GroupID = 1;
            ShimCampaignItemBlast.DeleteInt32Int32User = (a, b, c) =>
            {
                campaignItemBlastDeleted = true;
            };

            // Act	
            CampaignItemBlast.Save_UseAmbientTransaction(CampaignItemID, _campaignItemBlastList, _user);

            // Assert
            campaignItemBlastDeleted.ShouldBeTrue();
        }

        [Test]
        public void Save_UseAmbientTransaction_WhenCampaignItemTypeIsAb_CampaignItemIsSaved()
        {
            // Arrange
            SetupShimsForSaveMethod();
            var campaignItemSaved = false;
            _campaignItem.CampaignItemType = CampaignItemType.AB.ToString();
            ShimCampaignItemBlastRefBlast.SaveCampaignItemBlastRefBlastUserBoolean = (a, b, c) =>
            {
                campaignItemSaved = true;
                return 0;
            };

            // Act	
            CampaignItemBlast.Save_UseAmbientTransaction(CampaignItemID, _campaignItemBlastList, _user);

            // Assert
            campaignItemSaved.ShouldBeTrue();
        }

        [Test]
        public void Save_Success_CampaignItemIsSaved()
        {
            // Arrange
            SetupShimsForSaveMethod();
            var campaignItemSaved = false;
            _campaignItem.CampaignItemType = CampaignItemType.Regular.ToString();
            ShimCampaignItemBlastRefBlast.SaveCampaignItemBlastRefBlastUserBoolean = (a, b, c) =>
            {
                campaignItemSaved = true;
                return 0;
            };

            // Act	
            CampaignItemBlast.Save(CampaignItemID, _campaignItemBlastList, _user);

            // Assert
            campaignItemSaved.ShouldBeTrue();
        }

        [Test]
        public void Save_GroupIdsAreDifferent_DuplicateCampaignItemBlastsAreDeleted()
        {
            // Arrange
            SetupShimsForSaveMethod();
            var campaignItemBlastDeleted = false;
            _campaignItem.CampaignItemType = CampaignItemType.Regular.ToString();
            _campaignItemBlast.GroupID = 1;
            ShimCampaignItemBlast.DeleteInt32Int32User = (a, b, c) =>
            {
                campaignItemBlastDeleted = true;
            };

            // Act	
            CampaignItemBlast.Save(CampaignItemID, _campaignItemBlastList, _user);

            // Assert
            campaignItemBlastDeleted.ShouldBeTrue();
        }


        [Test]
        public void Save_WhenCampaignItemTypeIsAb_CampaignItemIsSaved()
        {
            // Arrange
            SetupShimsForSaveMethod();
            var campaignItemSaved = false;
            _campaignItem.CampaignItemType = CampaignItemType.AB.ToString();
            ShimCampaignItemBlastRefBlast.SaveCampaignItemBlastRefBlastUserBoolean = (a, b, c) =>
            {
                campaignItemSaved = true;
                return 0;
            };

            // Act	
            CampaignItemBlast.Save(CampaignItemID, _campaignItemBlastList, _user);

            // Assert
            campaignItemSaved.ShouldBeTrue();
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
            CampaignItemBlast.Validate_UseAmbientTransaction(_campaignItemBlast, _user);

            // Assert
            isValidated.ShouldBeTrue();
        }

        [Test]
        public void Validate_UseAmbientTransaction_WithActiveBlast_ThrowsExcpetion()
        {
            // Arrange
            SetupShimsForValidate(false);
            SetupCampaignItemWithNullProperties();
            ShimBlast.ActiveOrSent_UseAmbientTransactionInt32Int32 = (a, b) => true;

            // Act //Assert
            var exception = Should.Throw<ECNException>(() =>
            {
                CampaignItemBlast.Validate_UseAmbientTransaction(_campaignItemBlast, _user);
            });
        }

        [Test]
        public void Validate_UseAmbientTransaction_WithNullPropertiesAndCampaignItemBlastIdIsGreaterThanZero_ThrowsExcpetion()
        {
            // Arrange
            SetupShimsForValidate(false);
            SetupCampaignItemWithNullProperties();
            _campaignItemBlast.CampaignItemBlastID = 1;


            // Act //Assert
            var exception = Should.Throw<ECNException>(() =>
            {
                CampaignItemBlast.Validate_UseAmbientTransaction(_campaignItemBlast, _user);
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
            CampaignItemBlast.Validate_NoAccessCheck(_campaignItemBlast, _user);

            // Assert
            isValidated.ShouldBeTrue();
        }

        [Test]
        public void Validate_NoAccessCheck_WithNullPropertiesAndCampaignItemBlastIdIsZero_ThrowsExcpetion()
        {
            // Arrange
            SetupShimsForValidate(false);
            SetupCampaignItemWithNullProperties();

            // Act //Assert
            var exception = Should.Throw<ECNException>(() =>
            {
                CampaignItemBlast.Validate_NoAccessCheck(_campaignItemBlast, _user);
            });
        }

        [Test]
        public void Validate_NoAccessCheck_WithNullPropertiesAndCampaignItemBlastIdIsGreaterThanZero_ThrowsExcpetion()
        {
            // Arrange
            SetupShimsForValidate(false);
            SetupCampaignItemWithNullProperties();
            _campaignItemBlast.CampaignItemBlastID = 0;

            // Act //Assert
            var exception = Should.Throw<ECNException>(() =>
            {
                CampaignItemBlast.Validate_NoAccessCheck(_campaignItemBlast, _user);
            });
        }

        [Test]
        public void Validate_WithNullPropertiesAndCampaignItemBlastIdIsZero_ThrowsExcpetion()
        {
            // Arrange
            SetupShimsForValidate(false);
            SetupCampaignItemWithNullProperties();
            _campaignItemBlast.CampaignItemBlastID = 0;

            // Act //Assert
            var exception = Should.Throw<ECNException>(() =>
            {
                CampaignItemBlast.Validate(_campaignItemBlast, _user);
            });
        }

        [Test]
        public void Validate_WithNullPropertiesAndCampaignItemBlastIdIsGreaterThanZero_ThrowsExcpetion()
        {
            // Arrange
            SetupShimsForValidate(false);
            SetupCampaignItemWithNullProperties();

            // Act //Assert
            var exception = Should.Throw<ECNException>(() =>
            {
                CampaignItemBlast.Validate(_campaignItemBlast, _user);
            });
        }

        [Test]
        public void Validate_Success_CampaignItemBlastIsValidated()
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
            CampaignItemBlast.Validate(_campaignItemBlast, _user);

            // Assert
            isValidated.ShouldBeTrue();
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
            DataLayerFakes.ShimCampaignItem.ExistsInt32Int32Int32 = (a, b, c) => true;
            ShimBlast.ActiveOrSentInt32Int32 = (a, b) => false;
            ShimCampaign.Exists_UseAmbientTransactionInt32Int32 = (a, b) => true;
            ShimCampaignItem.ValidateScheduleCampaignItemUser = (a, b) => false;
            ShimCampaignItem.ValidateSchedule_NoAccessCheckCampaignItemUser = (a, b) => false;
            _campaignItemBlast.CampaignItemID = 0;
            _campaignItemBlast.CreatedUserID = 0;
            ShimSocialMedia.Exists_UseAmbientTransactionInt32 = (a) => false;
            ShimBlast.ActiveOrSent_UseAmbientTransactionInt32Int32 = (a, b) => false;
        }

        private void SetupShimsForValidate(bool forSuccess)
        {
            _campaignItemBlast = ReflectionHelper.CreateInstance(typeof(Entities.CampaignItemBlast));
            var campaignItemBlastFilter = ReflectionHelper.CreateInstance(typeof(Entities.CampaignItemBlastFilter));
            campaignItemBlastFilter.RefBlastIDs = One;
            _campaignItemBlast.Filters = new List<Entities.CampaignItemBlastFilter> { campaignItemBlastFilter };
            _campaignItem = ReflectionHelper.CreateInstance(typeof(Entities.CampaignItem));
            var campaignItemBlast = ReflectionHelper.CreateInstance(typeof(Entities.CampaignItemBlast));
            campaignItemBlast.CampaignItemBlastID = 1;
            _user = new User
            {
                UserID = 1,
                CustomerID = 1
            };
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (a, b, c) => _campaignItem;
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (a, b) => _campaignItem;
            ShimCampaignItem.GetByCampaignItemID_UseAmbientTransactionInt32UserBoolean = (a, b, c) => _campaignItem;
            ShimCampaign.ExistsInt32Int32 = (a, b) => forSuccess;
            ShimCampaign.Exists_UseAmbientTransactionInt32Int32 = (a, b) => forSuccess;
            ShimSocialMedia.ExistsInt32 = (a) => forSuccess;
            ShimSocialMedia.Exists_UseAmbientTransactionInt32 = (a) => forSuccess;
            ShimGroup.ExistsInt32Int32 = (a, b) => forSuccess;
            ShimGroup.Exists_UseAmbientTransactionInt32Int32 = (a, b) => forSuccess;
            ShimGroup.ValidateDynamicStringsListOfStringInt32User = (a, b, c) => { };
            ShimGroup.GetByGroupIDInt32User = (a, b) => ReflectionHelper.CreateInstance(typeof(Entities.Group));
            ShimGroup.ValidateDynamicStringsForTemplateListOfStringInt32User = (a, b, c) => { };
            ShimGroup.ValidateDynamicTagsInt32Int32User = (a, b, c) => { };
            ShimGroup.ValidateDynamicTags_UseAmbientTransactionInt32Int32User = (a, b, c) => { };
            ShimLayout.ExistsInt32Int32 = (a, b) => forSuccess;
            ShimLayout.Exists_UseAmbientTransactionInt32Int32 = (a, b) => forSuccess;
            ShimLayout.IsArchivedInt32Int32 = (a, b) => !forSuccess;
            ShimLayout.ValidateLayoutContentInt32 = (a) => null;
            ShimLayout.ValidateLayoutContent_UseAmbientTransactionInt32 = (a) => null;
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (a, b) => ReflectionHelper.CreateInstance(typeof(Entities.Layout));
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (a) => ReflectionHelper.CreateInstance(typeof(Entities.Template));
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
            ShimBlast.ExistsInt32Int32 = (a, b) => forSuccess;
            ShimBlast.Exists_UseAmbientTransactionInt32Int32 = (a, b) => forSuccess;
            ShimBlast.ActiveOrSent_UseAmbientTransactionInt32Int32 = (a, b) => !forSuccess;
            DataLayerFakes.ShimCampaignItem.ExistsInt32Int32Int32 = (a, b, c) => forSuccess;
        }

        private void SetupShimsForSaveMethod()
        {
            _campaignItemBlastList = new List<Entities.CampaignItemBlast> { ReflectionHelper.CreateInstance(typeof(Entities.CampaignItemBlast)) };
            ShimPlatformUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, d) => true;
            _campaignItem = ReflectionHelper.CreateInstance(typeof(Entities.CampaignItem));
            _campaignItemBlast = ReflectionHelper.CreateInstance(typeof(Entities.CampaignItemBlast));
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (a, b, c) => _campaignItem;
            ShimCampaignItem.GetByCampaignItemID_UseAmbientTransactionInt32UserBoolean = (a, b, c) => _campaignItem;
            ShimCampaignItem.ExistsInt32Int32Int32 = (a, b, c) => true;
            ShimCampaignItem.Exists_UseAmbientTransactionInt32Int32Int32 = (a, b, c) => true;
            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (a, b, c) => new List<Entities.CampaignItemBlast> { _campaignItemBlast };
            ShimCampaignItemBlast.GetByCampaignItemID_UseAmbientTransactionInt32UserBoolean = (a, b, c) => new List<Entities.CampaignItemBlast> { _campaignItemBlast };
            ShimCampaignItemBlast.ValidateCampaignItemBlastUserBoolean = (a, b, c) => { };
            ShimCampaignItemBlast.Validate_UseAmbientTransactionCampaignItemBlastUser = (a, b) => { };
            ShimCampaignItemBlast.DeleteInt32Int32User = (a, b, c) => { };
            ShimAccessCheck.CanAccessByCustomerOf1M0EnumsServicesEnumsServiceFeaturesEnumsAccessUser<Entities.CampaignItemBlast>((a, b, c, d, e) => true);
            ShimAccessCheck.CanAccessByCustomerOf1M0User<Entities.CampaignItemBlast>((a, b) => true);
            DataLayerFakes.ShimCampaignItemBlast.SaveCampaignItemBlast = (a) => 0;
            ShimCampaignItemBlastFilter.DeleteByCampaignItemBlastIDInt32 = (a) => { };
            ShimCampaignItemBlastFilter.SaveCampaignItemBlastFilter = (a) => 0;
            ShimCampaignItemBlastRefBlast.SaveCampaignItemBlastRefBlastUserBoolean = (a, b, c) => 0;
            ShimCampaignItemBlastRefBlast.DeleteInt32UserBoolean = (a, b, c) => { };
            ShimCampaignItemBlastFilter.DeleteByCampaignItemBlastIDInt32 = (a) => { };
        }
    }
}
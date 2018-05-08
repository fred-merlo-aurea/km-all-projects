using System.Collections.Generic;
using System.Data;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using ECN.Tests.Helpers;
using KMPlatform.Entity;
using KM.Platform.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class QuickTestBlastTest
    {
        private const string MethodCreateQuickTestBlast = "CreateQuickTestBlast";

        [Test]
        public void CreateQuickTestBlast_Success_TestBlastIsCreated()
        {
            // Arrange
            Initialize();
            SetupShimsForCreatingQuickTestBlast();
            var campaignId = 0;
            var testBlastCreated = false;
            var groupName = string.Empty;
            var groupId = (int?)null;
            var campaignItemId = (int?)null;
            ShimCampaignItemTestBlast.InsertCampaignItemTestBlastUserBoolean = (a, b, c) =>
            {
                testBlastCreated = true;
                return 0;
            };

            _methodArgs = new object[] { 1, 1, groupId, groupName, DummyString, 1, campaignItemId, DummyString, campaignId, DummyString, true, true, true, DummyString, DummyString, DummyString, DummyString, new User() };

            // Act
            var result = CallMethod(_typeQuickBlastTest, MethodCreateQuickTestBlast, _methodArgs, _objectQuickBlastTest) as List<string>;

            // Assert
            testBlastCreated.ShouldBeTrue();
        }

        [Test]
        public void CreateQuickTestBlast_WhenCampaignItemIdHasValue_TestBlastIsCreated()
        {
            // Arrange
            Initialize();
            SetupShimsForCreatingQuickTestBlast();
            var groupId = 1;
            var campaignId = 1;
            var campaignItemId = 1;
            var groupName = DummyString;
            var testBlastCreated = false;
            ShimCampaignItemTestBlast.InsertCampaignItemTestBlastUserBoolean = (a, b, c) =>
            {
                testBlastCreated = true;
                return 0;
            };

            _methodArgs = new object[] { 1, 1, groupId, groupName, DummyString, 1, campaignItemId, DummyString, campaignId, DummyString, true, true, true, DummyString, DummyString, DummyString, DummyString, new User() };

            // Act
            var result = CallMethod(_typeQuickBlastTest, MethodCreateQuickTestBlast, _methodArgs, _objectQuickBlastTest) as List<string>;

            // Assert
            testBlastCreated.ShouldBeTrue();
        }

        private void SetupShimsForCreatingQuickTestBlast()
        {
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, d) => true;
            var quickTestBlastConfig = ReflectionHelper.CreateInstance(typeof(QuickTestBlastConfig));
            quickTestBlastConfig.CustomerCanOverride = true;
            ShimQuickTestBlastConfig.GetByBaseChannelIDInt32 = (a) => quickTestBlastConfig;
            ShimQuickTestBlastConfig.GetByCustomerIDInt32 = (a) => quickTestBlastConfig;
            ShimQuickTestBlastConfig.GetKMDefaultConfig = () => ReflectionHelper.CreateInstance(typeof(QuickTestBlastConfig));
            ShimQuickTestBlast.ValidateStringInt32BooleanStringStringStringStringInt32NullableOfInt32StringInt32NullableOfInt32StringNullableOfInt32StringUserQuickTestBlastConfig = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q) => { };
            var campaignItem = ReflectionHelper.CreateInstance(typeof(CampaignItem));
            campaignItem.BlastList = new List<CampaignItemBlast>();
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (a, b, c) => campaignItem;
            ShimCampaign.GetByCampaignIDInt32UserBoolean = (a, b, c) => ReflectionHelper.CreateInstance(typeof(Campaign));
            ShimFolder.ExistsInt32StringInt32Int32String = (a, b, c, d, e) => false;
            ShimFolder.SaveFolderUser = (a, b) => 0;
            ShimGroup.GetByGroupIDInt32User = (a, b) => ReflectionHelper.CreateInstance(typeof(Group));
            ShimCampaignItem.SaveCampaignItemUser = (a, b) => 0;
            ShimBlast.GetByCampaignItemTestBlastIDInt32UserBoolean = (a, b, c) => ReflectionHelper.CreateInstance(typeof(BlastRegular));
            ShimLayout.GetByLayoutIDInt32UserBoolean = (a, b, c) => ReflectionHelper.CreateInstance(typeof(Layout));
            ShimCampaign.SaveCampaignUser = (a, b) => 0;
            ShimGroup.SaveGroupUser = (a, b) => 0;
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString = (a, b, c, d, e, f, g, h, i, j) => new DataTable();
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (a, b) => ReflectionHelper.CreateInstance(typeof(Layout));
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (a) => ReflectionHelper.CreateInstance(typeof(Template));
            ShimQuickTestBlast.GetShortNamesForLayoutTemplateListOfStringInt32User = (a, b, c) => new List<string> { DummyString };
            ShimQuickTestBlast.GetShortNamesForDynamicStringsListOfStringInt32User = (a, b, c) => new List<string> { DummyString };
            ShimLayout.ValidateLayoutContentInt32 = (a) => new List<string> { DummyString };
            ShimGroupDataFields.SaveGroupDataFieldsUser = (a, b) => 0;
            ShimCampaignItemBlast.SaveCampaignItemBlastUserBoolean = (a, b, c) => 0;
            ShimCampaignItemTestBlast.InsertCampaignItemTestBlastUserBoolean = (a, b, c) => 0;
        }
    }
}
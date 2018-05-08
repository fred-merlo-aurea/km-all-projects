using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.Content;
using ecn.communicator.main.ECNWizard.Content.Fakes;
using ecn.communicator.main.ECNWizard.Controls;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Functions.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using ECN.Tests.Helpers;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntitiesContent = ECN_Framework_Entities.Communicator.Content;
using EntitiesGroup = ECN_Framework_Entities.Communicator.Group;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class WizardContentTest : PageHelper
    {
        private const string MethoProcessSave = "ProcessSave";
        private const string MethodInitialize = "Initialize";
        private const string MethodLoadData = "loadData";
        private const string TestUser = "TestUser";
        private const string DummyString = "dummyString";
        private const string LayoutValueString = "1"; 
        private const int LayoutId = 1; 
        private WizardContent _testEntity;
        private PrivateObject _privateTestObject;
        private IDisposable _context;


        [SetUp]
        protected override void SetPageSessionContext()
        {
            _context = ShimsContext.Create();
            base.SetPageSessionContext();
            _testEntity = new WizardContent { CampaignItemID = 1 };
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            InitializeSessionFakes();
        }

        [TearDown]
        public void TearDwond()
        {
            _context.Dispose();
        }

        [TestCase(1, true)]
        [TestCase(2, false)]
        public void LoadData_WithDifferentNumberOfBlastListItems_DynamicFieldsToggle(int numberOfBlastListItems, bool isEnabled)
        {
            // Arrange 
            Initialize();
            CreateShims();
            var campaignItem = CreateCampaignItemForLoadData(numberOfBlastListItems);

            // Act
            _privateTestObject.Invoke(MethodLoadData, campaignItem);

            // Assert
            var dynamicEmailFrom = GetField(_testEntity, "dyanmicEmailFrom") as DropDownList;
            var dynamicReplyToEmail = GetField(_testEntity, "dyanmicReplyToEmail") as DropDownList;
            var dynamicEmailFromName = GetField(_testEntity, "dyanmicEmailFromName") as DropDownList;
            _testEntity.ShouldSatisfyAllConditions(
                () => dynamicEmailFrom.ShouldNotBeNull(),
                () => dynamicReplyToEmail.ShouldNotBeNull(),
                () => dynamicEmailFromName.ShouldNotBeNull(),
                () => dynamicEmailFrom.Enabled.ShouldBe(isEnabled),
                () => dynamicReplyToEmail.Enabled.ShouldBe(isEnabled),
                () => dynamicEmailFromName.Enabled.ShouldBe(isEnabled));
        }
        
        [Test]
        public void Initialize_WhenFromEmailIsMissing_CampaignItemTemplateIsLoaded()
        {
            // Arrange 
            Initialize();
            CreateShims();
            var templateLoaded = false;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, d) => true;
            var campaignItem = CreateInstance(typeof(CampaignItem));
            campaignItem.FromEmail = string.Empty;
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) => campaignItem;
            ShimCampaignItemTemplate.GetByCampaignItemTemplateIDInt32UserBoolean = (x, y, z) =>
            {
                templateLoaded = true;
                return CreateInstance(typeof(CampaignItemTemplate));
            };

            // Act
            _privateTestObject.Invoke(MethodInitialize, null);

            // Assert
            templateLoaded.ShouldBeTrue();
        }

        [Test]
        public void Initialize_WhenCustomerHaveBlastEnvelopes_EnvelopsAreLoaded()
        {
            // Arrange 
            Initialize();
            CreateShims();
            var blastEnvelopLoaded = false;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, d) => true;
            var blastEnvelope = CreateInstance(typeof(BlastEnvelope));
            var blastEnvelopeList = new List<BlastEnvelope> { blastEnvelope };
            ShimBlastEnvelope.GetByCustomerID_NoAccessCheckInt32 = (x) =>
            {
                blastEnvelopLoaded = true;
                return blastEnvelopeList;
            };

            // Act
            _privateTestObject.Invoke(MethodInitialize, null);

            // Assert
            blastEnvelopLoaded.ShouldBeTrue();
        }

        [Test]
        public void ProcessSave_WhenLayOutIdIsNotZero_ItemIsSaved()
        {
            // Arrange 
            Initialize();
            CreateShims();
            var campaignItemBlastSaved = false;
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (x, y ,z) => true;
            ShimCampaignItemBlast.SaveInt32ListOfCampaignItemBlastUser = (x, y, z) => 
            {
                campaignItemBlastSaved = true;
            };

            // Act
            _privateTestObject.Invoke(MethoProcessSave, true, true);

            // Assert
            campaignItemBlastSaved.ShouldBeTrue();
        }

        [Test]
        public void ProcessSave_WhenErrorListContainsErrors_ExceptionIsThrown()
        {
            // Arrange 
            Initialize();
            CreateShims();
            CreateCampaignItemShims();
            ShimContent.TopicParamExistsContent = (x) => true;

            // Act// Assert
            NUnit.Framework.Assert.That(() =>
                _privateTestObject.Invoke(MethoProcessSave, true, true),
                Throws.TargetInvocationException);
        }

        [TestCase("errorList")]
        [TestCase("emptyErrorList")]
        public void ProcessSave_WhenLayOutIdIsNotZeroAndInvalidHtmlContent_ThrowsException(string errorList)
        {
            // Arrange 
            Initialize();
            CreateShims();
            var ecnError = CreateInstance(typeof(ECNError));
            var ecnErrorList = new List<ECNError>();
            if (errorList == "errorList")
            {
                ecnErrorList.Add(ecnError);
            }
            ShimContent.ValidateHTMLContentString = (x) => throw new ECNException(ecnErrorList);

            // Act// Assert
            NUnit.Framework.Assert.That(() =>
                _privateTestObject.Invoke(MethoProcessSave, true, true),
                Throws.TargetInvocationException);
        }

        [Test]
        public void ProcessSave_WhenLayOutIdIsZero_ThrowsException()
        {
            // Arrange 
            Initialize();
            CreateShims();
            SetField(_testEntity, "rbNewLayout", new RadioButton { Checked = false });
            ShimlayoutExplorer.AllInstances.selectedLayoutIDGet = (x) => 0;

            // Act// Assert
            NUnit.Framework.Assert.That(() =>
                _privateTestObject.Invoke(MethoProcessSave, true, true),
                Throws.TargetInvocationException);
        }

        [Test]
        public void ProcessSave_WithInvalidFields_ExceptionIsThrown()
        {
            // Arrange 
            Initialize();
            CreateShims();
            SetField(_testEntity, "txtEmailFromName", new TextBox { Text = string.Empty });

            // Act// Assert
            NUnit.Framework.Assert.That(() =>
                _privateTestObject.Invoke(MethoProcessSave, true, true),
                Throws.TargetInvocationException);
        }

        [Test]
        public void ProcessSave_WithInvalidEmailSubject_ExceptionIsThrown()
        {
            // Arrange 
            Initialize();
            CreateShims();
            ShimRegexUtilities.IsValidEmailSubjectString = (x) => DummyString;

            // Act// Assert
            NUnit.Framework.Assert.That(() =>
                _privateTestObject.Invoke(MethoProcessSave, true, true),
                Throws.TargetInvocationException);
        }

        private void Initialize()
        {
            CreateInstance(typeof(WizardContent));
            SetField(_testEntity, "hfSelectedLayoutA", new HiddenField { Value = LayoutValueString });
            SetField(_testEntity, "hfSelectedLayoutB", new HiddenField { Value = LayoutValueString });
            SetField(_testEntity, "txtEmailFromA", new TextBox { Text = DummyString });
            SetField(_testEntity, "txtFromNameA", new TextBox { Text = DummyString });
            SetField(_testEntity, "txtReplyToA", new TextBox { Text = DummyString });
            SetField(_testEntity, "txtSubjectA", new HiddenField { Value = LayoutValueString });
            SetField(_testEntity, "txtSubjectB", new HiddenField { Value = LayoutValueString });
            SetField(_testEntity, "txtEmailFromB", new TextBox { Text = DummyString });
            SetField(_testEntity, "txtFromNameB", new TextBox { Text = DummyString });
            SetField(_testEntity, "txtReplyToB", new TextBox { Text = DummyString });
            var drpEmailFrom = new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Text = DummyString
                    }
                }
            };
            var drpReplyTo = new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Text = DummyString
                    }
                }
            };
            var drpEmailFromName = new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Text = DummyString
                    }
                }
            };
            SetField(_testEntity, "rbNewLayout", new RadioButton { Checked = true });
            SetField(_testEntity, "layoutExplorer1", new layoutExplorer());
            SetField(_testEntity, "HiddenSelectedLayoutID", new HiddenField { Value = string.Empty });
            SetField(_testEntity, "txtEmailFrom", new TextBox { Text = DummyString });
            SetField(_testEntity, "txtEmailFromName", new TextBox { Text = DummyString });
            SetField(_testEntity, "txtReplyTo", new TextBox { Text = DummyString });
            SetField(_testEntity, "txtEmailSubject", new HiddenField { Value = DummyString });
            SetField(_testEntity, "drpEmailFrom", drpEmailFrom);
            SetField(_testEntity, "drpReplyTo", drpReplyTo);
            SetField(_testEntity, "drpEmailFromName", drpEmailFromName);
        }

        private CampaignItem CreateCampaignItemForLoadData(int numberOfBlastListItems)
        {
            var emailFrom = "km";
            var campaignItemBlast = CreateInstance(typeof(CampaignItemBlast));
            campaignItemBlast.GroupID = 1;
            var dynamicFromEmail = new
            {
                ShortNameText = emailFrom,
                ShortNameValue = string.Format("%%{0}%%", emailFrom)
            }.ToString();
            campaignItemBlast.DynamicFromEmail = dynamicFromEmail;
            campaignItemBlast.DynamicReplyTo = dynamicFromEmail;
            campaignItemBlast.DynamicFromName = dynamicFromEmail;
            var campaignItemBlastList = new List<CampaignItemBlast>();
            campaignItemBlastList.Add(campaignItemBlast);
            if (numberOfBlastListItems == 2)
            {
                campaignItemBlastList.Add(campaignItemBlast);
            }
            var campaignItem = CreateInstance(typeof(CampaignItem));
            campaignItem.BlastList = campaignItemBlastList;
            SetField(_testEntity, "rbExistingLayout", new RadioButton { Checked = true });
            var groupDataFields = CreateInstance(typeof(GroupDataFields));
            groupDataFields.ShortName = emailFrom;
            var groupDataFieldsList = new List<GroupDataFields> { groupDataFields };
            ShimGroupDataFields.GetByGroupID_NoAccessCheckInt32 = (x) => groupDataFieldsList;
            return campaignItem;
        }

        private void CreateCampaignItemShims()
        {
            var campaignItemBlast = CreateInstance(typeof(CampaignItemBlast));
            campaignItemBlast.GroupID = 1;
            var campaignItemBlastList = new List<CampaignItemBlast>();
            campaignItemBlastList.Add(campaignItemBlast);
            campaignItemBlastList.Add(campaignItemBlast);
            var campaignItem = CreateInstance(typeof(CampaignItem));
            campaignItem.BlastList = campaignItemBlastList;
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) => campaignItem;
        }

        private void CreateShims()
        {
            var template = CreateInstance(typeof(Template));
            template.SlotsTotal = 1;
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (x) => template;
            ShimlayoutEditor.AllInstances.SaveLayout = (x) => LayoutId;
            ShimlayoutEditor.AllInstances.Initialize = (x) => { };
            ShimlayoutExplorer.AllInstances.selectedLayoutIDSetInt32 = (x, y) => { };
            ShimlayoutExplorer.AllInstances.selectedLayoutIDGet = (x) => LayoutId;
            ShimlayoutExplorer.AllInstances.enableSelectMode = (x) => { };
            ShimlayoutExplorer.AllInstances.reset = (x) => { };
            ShimlayoutExplorer.AllInstances.reset = (x) => { };
            ShimlayoutExplorer.AllInstances.loadFolders = (x) => { };
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (a, b, c) => true;
            var layout = CreateInstance(typeof(Layout));
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (x, y) => layout;
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (x) => template;
            var campaignItem = CreateInstance(typeof(CampaignItem));
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) => campaignItem;
            ShimSample.GetBySampleIDInt32User = (x, y) => CreateInstance(typeof(Sample));
            ShimSample.SaveSampleUser = (x, y) => { };
            ShimRegexUtilities.IsValidEmailSubjectString = (x) => string.Empty;
            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (x, y) => CreateInstance(typeof(EntitiesContent));
            ShimContent.ValidateHTMLContentString = (x) => true;
            ShimCampaignItemBlast.SaveCampaignItemBlastUserBoolean = (a, b, c) => 0;
            ShimCampaignItem.SaveCampaignItemUser = (x, y) => 0;
            ShimBlast.CreateBlastsFromCampaignItemInt32UserBoolean = (x, y, z) => { };
            ShimLayout.GetByLayoutIDInt32UserBoolean = (x, y, z) => CreateInstance(typeof(Layout));
            var groupDataFields = CreateInstance(typeof(GroupDataFields));
            var groupDataFieldsList = new List<GroupDataFields> { groupDataFields };
            ShimGroupDataFields.GetByGroupID_NoAccessCheckInt32 = (x) => groupDataFieldsList;
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) => CreateInstance(typeof(EntitiesGroup));
            ShimContent.GetByContentIDInt32UserBoolean = (x, y, z) => CreateInstance(typeof(EntitiesContent));
            ShimContent.TopicParamExistsContent = (x) => true;
            var blastEnvelope = CreateInstance(typeof(BlastEnvelope));
            var blastEnvelopeList = new List<BlastEnvelope> { blastEnvelope };
            ShimBlastEnvelope.GetByCustomerID_NoAccessCheckInt32 = (x) => blastEnvelopeList;
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            var client = CreateInstance(typeof(Client));
            shimSession.Instance.CurrentUser = new User() { UserID = 1, UserName = TestUser, IsActive = true, CurrentClient = client };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }

        private dynamic CallMethod(Type type, string methodName, object[] parametersValues, object instance = null)
        {
            return ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
        }

        private dynamic CreateInstance(Type type)
        {
            return ReflectionHelper.CreateInstance(type);
        }

        private dynamic GetField(dynamic obj, string fieldName)
        {
            return ReflectionHelper.GetFieldValue(obj, fieldName);
        }

        private void SetField(dynamic obj, string fieldName, dynamic value)
        {
            ReflectionHelper.SetField(obj, fieldName, value);
        }

        private void SetProperty(dynamic obj, string fieldName, dynamic value)
        {
            ReflectionHelper.SetProperty(obj, fieldName, value);
        }

        private dynamic GetProperty(dynamic obj, string fieldName)
        {
            return ReflectionHelper.GetPropertyValue(obj, fieldName);
        }
    }
}

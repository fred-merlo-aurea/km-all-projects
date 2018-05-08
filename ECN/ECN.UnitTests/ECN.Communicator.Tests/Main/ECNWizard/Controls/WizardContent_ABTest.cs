using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.Controls;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Functions.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using ECN.Tests.Helpers;
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
    public partial class WizardContent_ABTest : PageHelper
    {
        private const string MethoProcessSave = "ProcessSave";
        private const string TestUser = "TestUser";
        private const string DummyString = "dummyString";
        private const string LayoutValueString = "1";
        private WizardContent_AB _testEntity;
        private PrivateObject _privateTestObject;
        private IDisposable _context;


        [SetUp]
        protected override void SetPageSessionContext()
        {
            _context = ShimsContext.Create();
            base.SetPageSessionContext();
            _testEntity = new WizardContent_AB { CampaignItemID = 1 };
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            InitializeSessionFakes();
        }

        [TearDown]
        public void TearDwond()
        {
            _context.Dispose();
        }

        [Test]
        public void ProcessSave_WhenSampleIdIsNotNull_CampaignItemIsSaved()
        {
            //Arrange 
            Initialize();
            CreateShims();
            var campaignItemSaved = false;
            ShimCampaignItem.SaveCampaignItemUser = (x, y) =>
            {
                campaignItemSaved = true;
                return 0;
            };

            //Act
            var resultGroupId = _privateTestObject.Invoke(MethoProcessSave, true);

            //Assert
            campaignItemSaved.ShouldBeTrue();
        }

        [Test]
        public void ProcessSave_WhenSampleIdIsNull_CampaignItemIsSaved()
        {
            //Arrange 
            Initialize();
            CreateShims();
            var campaignItemSaved = false;
            ShimCampaignItem.SaveCampaignItemUser = (x, y) =>
            {
                campaignItemSaved = true;
                return 0;
            };
            var campaignItem = CreateInstance(typeof(CampaignItem));
            campaignItem.SampleID = null;
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) => campaignItem;

            //Act
            var resultGroupId = _privateTestObject.Invoke(MethoProcessSave, true);

            //Assert
            campaignItemSaved.ShouldBeTrue();
        }

        [Test]
        public void ProcessSave_WhenSlotsTotalIsOne_ValidateHTMLContentIsCalled()
        {
            //Arrange 
            Initialize();
            CreateShims();
            var htmlContentValidated = false;
            var template = CreateInstance(typeof(Template));
            template.SlotsTotal = 1;
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (x) => template;
            ShimContent.ValidateHTMLContentString = (x) =>
            {
                htmlContentValidated = true;
                return true;
            };

            //Act
            var resultGroupId = _privateTestObject.Invoke(MethoProcessSave, true);

            //Assert
            htmlContentValidated.ShouldBeTrue();
        }

        [Test]
        public void ProcessSave_WhenBlastGroupIdIsNotNull_UdfTopicIsValidated()
        {
            //Arrange 
            Initialize();
            CreateShims();
            CreateCampaignItemShims();
            var udfTopicValidated = false;
            ShimContent.TopicParamExistsContent = (x) =>
            {
                udfTopicValidated = true;
                return false;
            };

            //Act
            var resultGroupId = _privateTestObject.Invoke(MethoProcessSave, true);

            //Assert
            udfTopicValidated.ShouldBeTrue();
        }

        [Test]
        public void ProcessSave_WhenErrorListContainsErrors_ExceptionIsThrown()
        {
            //Arrange 
            Initialize();
            CreateShims();
            CreateCampaignItemShims();
            ShimContent.TopicParamExistsContent = (x) => true;

            //Act//Assert
            NUnit.Framework.Assert.That(() =>
                _privateTestObject.Invoke(MethoProcessSave, true),
                Throws.TargetInvocationException);
        }

        [TestCase("hfSelectedLayoutA")]
        [TestCase("hfSelectedLayoutB")]
        [TestCase("txtSubjectA")]
        public void ProcessSave_WithInvalidFields_ExceptionIsThrown(string fieldName)
        {
            //Arrange 
            Initialize();
            CreateShims();
            SetField(_testEntity, fieldName, new HiddenField { Value = string.Empty });

            //Act//Assert
            NUnit.Framework.Assert.That(() =>
                _privateTestObject.Invoke(MethoProcessSave, true),
                Throws.TargetInvocationException);
        }

        [Test]
        public void ProcessSave_WithInvalidEmailSubject_ExceptionIsThrown()
        {
            //Arrange 
            Initialize();
            CreateShims();
            ShimRegexUtilities.IsValidEmailSubjectString = (x) => DummyString;

            //Act//Assert
            NUnit.Framework.Assert.That(() =>
                _privateTestObject.Invoke(MethoProcessSave, true),
                Throws.TargetInvocationException);
        }

        [Test]
        public void ProcessSave_WithInvalidHtmlContent_ExceptionIsThrown()
        {
            //Arrange 
            Initialize();
            CreateShims();
            var template = CreateInstance(typeof(Template));
            template.SlotsTotal = 1;
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (x) => template;
            ShimContent.ValidateHTMLContentString = (x) => throw new Exception();

            //Act//Assert
            NUnit.Framework.Assert.That(() =>
                _privateTestObject.Invoke(MethoProcessSave, true),
                Throws.TargetInvocationException);
        }

        private void Initialize()
        {
            CreateInstance(typeof(WizardContent_AB));
            var ddlFromNameA = new DropDownList
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
            var ddlFromEmailA = new DropDownList
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
            var ddlReplyToA = new DropDownList
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
            SetField(_testEntity, "ddlFromNameA", ddlFromNameA);
            SetField(_testEntity, "ddlFromEmailA", ddlFromEmailA);
            SetField(_testEntity, "ddlReplyToA", ddlReplyToA);
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
            var campaignItem = CreateInstance(typeof(CampaignItem));
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) => campaignItem;
            ShimSample.GetBySampleIDInt32User = (x, y) => CreateInstance(typeof(Sample));
            ShimSample.SaveSampleUser = (x, y) => { };
            ShimRegexUtilities.IsValidEmailSubjectString = (x) => string.Empty;
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (x, y) => CreateInstance(typeof(Layout));
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (x) => CreateInstance(typeof(Template));
            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (x, y) => CreateInstance(typeof(EntitiesContent));
            ShimContent.ValidateHTMLContentString = (x) => true;
            ShimCampaignItemBlast.SaveCampaignItemBlastUserBoolean = (a, b, c) => 0;
            ShimCampaignItem.SaveCampaignItemUser = (x, y) => 0;
            ShimBlast.CreateBlastsFromCampaignItemInt32UserBoolean = (x, y, z) => { };
            ShimLayout.GetByLayoutIDInt32UserBoolean = (x, y, z) => CreateInstance(typeof(Layout));
            var groupDataFields = CreateInstance(typeof(GroupDataFields));
            var groupDataFieldsList = new List<GroupDataFields> { groupDataFields };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (x, y, z) => groupDataFieldsList;
            ShimGroup.GetByGroupIDInt32User = (x, y) => CreateInstance(typeof(EntitiesGroup));
            ShimContent.GetByContentIDInt32UserBoolean = (x, y, z) => CreateInstance(typeof(EntitiesContent));
            ShimContent.TopicParamExistsContent = (x) => true;
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            shimSession.Instance.CurrentUser = new User() { UserID = 1, UserName = TestUser, IsActive = true };
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


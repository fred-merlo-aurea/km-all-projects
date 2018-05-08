using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes.Shims;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_Entities.Accounts;
using KMWeb.Controllers;
using KMWeb.Tests.Helper;
using KMWeb.Services.Fakes;
using KMModels.PostModels;
using KMPlatform.Entity;
using KMManagers.Fakes;
using KM.Common.Entity.Fakes;
using KMModels.ViewModels;
using KMEnums;
using KMModels;
using KMSite;
using NUnit.Framework;
using Shouldly;
using ControlModel = KMModels.ControlModel;

namespace KMWeb.Tests.Controllers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class FormsControllerTests : ControllerHelper
    {
        private const string SavedKey = "Saved";
        private const string SampleHrefString = "<input href=\"abc\" />";
        private const string PartialFormRulesView = "Partials/_FormRulesContent";
        private FormsController _testEntity;
        private PrivateObject _privateTestObject;
        private bool _isFormsRulesSaved;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            SetPageManagerConstructorFakes();
            var applicationManagerStub = GetApplicationFactoryFakes();
            var userServiceStub = new StubIUserSelfServicing();
            _testEntity = new FormsController(applicationManagerStub, userServiceStub, new KMAuthenticationManager());
            InitializeAllControls(_testEntity);
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeSessionFakes();
        }
        
        [Test]
        public void SaveRules_WhenControlIsDuplicated_AddsValidationError()
        {
            // Arrange
            var model = new FormRulesPostModel()
            {
                Rules = new List<RuleModel>
                {
                    new RuleModel { Id = 1 ,ControlId = 1 , Type = RuleTypes.Field },
                    new RuleModel { Id = 1 ,ControlId = 1 , Type = RuleTypes.Field }
                }
            };
            SetFakesForSaveRulesMethod();

            // Act
            var result = _testEntity.SaveRules(model);

            // Assert
            result.ShouldSatisfyAllConditions(
                () =>
                    {
                        var partialViewResult = result.ShouldBeOfType<PartialViewResult>();
                        partialViewResult.ShouldSatisfyAllConditions(
                            () => partialViewResult.ShouldNotBeNull(),
                            () => partialViewResult.ViewName.ShouldContain(PartialFormRulesView));
                    },
                () => ResponseHeaders.ShouldNotBeEmpty(),
                () => ResponseHeaders[SavedKey].ShouldBe(bool.FalseString),
                () => AssertViewBagProperties());
        }

        [Test]
        public void SaveRules_WhenControlWhenRuleTypeForm_AddsValidationError()
        {
            // Arrange
            SetFakesForSaveRulesMethod();
            var model = new FormRulesPostModel()
            {
                Rules = new List<RuleModel>
                {
                    new RuleModel
                    {
                        Id = 1,
                        ControlId = 1,
                        Type = RuleTypes.Form,
                        IsOverWriteDataPost = true,
                        OverwritePostValue = new List<OverwriteDataValueModel>
                        {
                            new OverwriteDataValueModel { FormField = 1, Value = string.Empty },
                            new OverwriteDataValueModel { FormField = 1, Value = "SampleOverwriteValue" },
                            new OverwriteDataValueModel { FormField = 1, Value = new string('S',101) }
                        },
                        ResultOnSubmit = ResultType.KMPaidPage,
                        RequestQueryValue = new List<RequestQueryDataValueModel>
                        {
                            new RequestQueryDataValueModel { Value = 1 },
                            new RequestQueryDataValueModel { Value = 1 , Name = new string('N',101) },
                            new RequestQueryDataValueModel { Value = 2 , Name = "Inv@lid$tring" }
                        }
                    },
                }
            };

            // Act
            var result = _testEntity.SaveRules(model);

            // Assert
            result.ShouldSatisfyAllConditions(
                () =>
                {
                    var partialViewResult = result.ShouldBeOfType<PartialViewResult>();
                    partialViewResult.ShouldSatisfyAllConditions(
                        () => partialViewResult.ShouldNotBeNull(),
                        () => partialViewResult.ViewName.ShouldContain(PartialFormRulesView));
                },
                () => _testEntity.ModelState["error"].Errors.Count.ShouldBe(8),
                () => AssertViewBagProperties());
        }

        [Test]
        [TestCase("")]
        [TestCase(SampleHrefString)]
        public void SaveRules_WhenControlRuleTypeMessage_AddsValidationError(string actionString)
        {
            // Arrange
            SetFakesForSaveRulesMethod();
            var model = new FormRulesPostModel()
            {
                Rules = new List<RuleModel>
                {
                    new RuleModel
                    {
                        Id = 1,
                        ControlId = 1,
                        Type = RuleTypes.Form,
                        IsOverWriteDataPost = false,
                        ResultOnSubmit = ResultType.Message,
                        Action = actionString
                    },
                }
            };

            // Act
            var result = _testEntity.SaveRules(model);

            // Assert
            result.ShouldSatisfyAllConditions(
                () =>
                {
                    var partialViewResult = result.ShouldBeOfType<PartialViewResult>();
                    partialViewResult.ShouldSatisfyAllConditions(
                        () => partialViewResult.ShouldNotBeNull(),
                        () => partialViewResult.ViewName.ShouldContain(PartialFormRulesView));
                },
                () => _testEntity.ModelState["error"].Errors.Count.ShouldBe(1),
                () => AssertViewBagProperties());
        }

        [Test]
        public void SaveRules_WhenControlRuleTypeUrl_AddsValidationError()
        {
            // Arrange
            SetFakesForSaveRulesMethod();
            var model = new FormRulesPostModel()
            {
                Rules = new List<RuleModel>
                {
                    new RuleModel
                    {
                        Id = 1,
                        ControlId = 1,
                        Type = RuleTypes.Form,
                        IsOverWriteDataPost = false,
                        ResultOnSubmit = ResultType.URL,
                        UrlToRedirect = string.Empty
                    },
                }
            };

            // Act
            var result = _testEntity.SaveRules(model);

            // Assert
            result.ShouldSatisfyAllConditions(
                () =>
                {
                    var partialViewResult = result.ShouldBeOfType<PartialViewResult>();
                    partialViewResult.ShouldSatisfyAllConditions(
                        () => partialViewResult.ShouldNotBeNull(),
                        () => partialViewResult.ViewName.ShouldContain(PartialFormRulesView));
                },
                () => _testEntity.ModelState["error"].Errors.Count.ShouldBe(1),
                () => AssertViewBagProperties());
        }

        [Test]
        public void SaveRules_WhenControlWithvalid_RulesSaved()
        {
            // Arrange
            SetFakesForSaveRulesMethod();
            var model = new FormRulesPostModel()
            {
                Rules = new List<RuleModel>
                {
                    new RuleModel
                    {
                        Id = 1,
                        ControlId = 1,
                        Type = RuleTypes.Form,
                        IsOverWriteDataPost = false,
                        ResultOnSubmit = ResultType.URL,
                        UrlToRedirect = "SomeSampleUrl",
                        ConditionGroup = new List<ConditionGroupModel>
                        {
                            new ConditionGroupModel
                            {
                                Id = 1 ,Conditions = new List<ConditionModel>
                                {
                                    new ConditionModel { ControlId = 1 }
                                }
                            }
                        }
                    },
                }
            };

            // Act
            var result = _testEntity.SaveRules(model);

            // Assert
            result.ShouldSatisfyAllConditions(
                () =>
                {
                    var partialViewResult = result.ShouldBeOfType<PartialViewResult>();
                    partialViewResult.ShouldSatisfyAllConditions(
                        () => partialViewResult.ShouldNotBeNull(),
                        () => partialViewResult.ViewName.ShouldContain(PartialFormRulesView));
                },
                () => ResponseHeaders.Count.ShouldBe(1),
                () => _isFormsRulesSaved.ShouldBeTrue(),
                () => ResponseHeaders[SavedKey].ShouldBe(bool.TrueString),
                () => AssertViewBagProperties());
        }

        private void SetFakesForSaveRulesMethod()
        {
            _isFormsRulesSaved = false;
            ShimControlManager.AllInstances.GetAllValuedByFormIDOf1Int32<ControlModel>((c, fid) => new List<ControlModel>
            {
                new ControlModel { FormId = 1, Id = 1, }
            });
            ShimControlManager.AllInstances.GetAllVisibleByFormIDOf1Int32<ControlModel>((c, fid) => new List<ControlModel>
            {
                new ControlModel { FormId = 1, Id = 1, }
            });

            ShimControlManager.AllInstances.GetPageBreaksByFormIDOf1Int32<ControlModel>((c, fid) => new List<ControlModel>
            {
                new ControlModel { FormId = 1, Id = 1, }
            });

            ShimControlManager.AllInstances.GetAllByFormIDOf1Int32<ControlModel>((c, fid) => new List<ControlModel>
            {
                new ControlModel { FormId = 1, Id = 1, }
            });
            ShimControlManager.AllInstances.GetAllVisibleOverwriteDataFormIDOf1Int32<ControlModel>((c, id) => new List<ControlModel>
            {
                new ControlModel { FormId = 1, Id = 1, }
            });

            ShimControlManager.AllInstances.GetAllRequestQueryByFormIDOf1Int32<ControlModel>((c, id) => new List<ControlModel>
            {
                new ControlModel { FormId = 1, Id = 1, FieldID = 1,Control_Type = new ControlTypeModel{ Name = "SampleControlTypeName" } }
            });
            ShimControlTypeManager.AllInstances.GetPaidQueryStringByNameOf1String<ControlTypeModel>((c, id) => new ControlTypeModel
            {
                KMPaidQueryString = "SamplePaidQueryString"
            });


            ShimFormManager.AllInstances.GetByIDOf1Int32Int32<FormViewModel>((f, cid, id) => new FormViewModel
            {
                Type = FormType.Subscription.ToString()
            });
            ShimFormManager.AllInstances.GetFieldsByFormIDInt32Int32User = (f, id, bid, user) => new List<GroupDataFields>
            {
                new GroupDataFields { GroupDataFieldsID  = 1, ShortName = "SampleShortName" }
            };
            ShimRuleManager.AllInstances.SaveUserInt32FormRulesPostModel = (rm, user, cid, model) =>
            {
                _isFormsRulesSaved = true;
            };
        }

        private StubIApplicationManagersFactory GetApplicationFactoryFakes()
        {
            var applicationManagerStub = new StubIApplicationManagersFactory()
            {
                CreateControlManager = () => new KMManagers.ControlManager(),
                CreateFormManager = () => new KMManagers.FormManager(),
                CreateNotificationManager = () => new KMManagers.NotificationManager(),
                CreateRuleManager = () => new KMManagers.RuleManager(),
                CreateCssFileManager = () => new KMManagers.CssFileManager(),
                CreateControlTypeManager = () => new KMManagers.ControlTypeManager(),
                CreateSubscriberLoginManager = () => new KMManagers.SubscriberLoginManager(),
            };
            return applicationManagerStub;
        }

        private void SetPageManagerConstructorFakes()
        {
            ShimFormManager.Constructor = manager => { };
            ShimControlManager.Constructor = manager => { };
            ShimNotificationManager.Constructor = manager => { };
            ShimCssFileManager.Constructor = manager => { };
            ShimRuleManager.Constructor = manager => { };
            ShimControlTypeManager.Constructor = manager => { };
            ShimSubscriberLoginManager.Constructor = manager => { };
            ShimAPIRunnerBase.Constructor = apirunnerbase => { };
            ShimApplicationLog.Behavior = ShimBehaviors.DefaultValue;
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            shimSession.BaseChannelIDGet = () => 1;
            shimSession.Instance.CurrentUser = new User() { UserID = 1, UserName = "TestUser", IsActive = true };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }

        private void AssertViewBagProperties()
        {
            _testEntity.ShouldSatisfyAllConditions(
                () => ((IEnumerable<ControlModel>)_testEntity.ViewBag.Controls).Count().ShouldBe(1),
                () => ((IEnumerable<ControlModel>)_testEntity.ViewBag.VisibleControls).Count().ShouldBe(1),
                () => ((IEnumerable<ControlModel>)_testEntity.ViewBag.PageBreaks).Count().ShouldBe(1),
                () => ((IEnumerable<ControlModel>)_testEntity.ViewBag.SubVisibleControls).Count().ShouldBe(1),
                () => ((IEnumerable<ControlModel>)_testEntity.ViewBag.URLControls).Count().ShouldBe(1));
        }
    }
}

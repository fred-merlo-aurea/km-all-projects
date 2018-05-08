using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Web.Security.Fakes;
using ecn.MarketingAutomation.Controllers;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.Fakes;
using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ecn.MarketingAutomation.Models.PostModels.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using Kendo.Mvc.UI;
using KMPlatform.BusinessLogic.Fakes;
using KMSite;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using Entities = ECN_Framework_Entities.Communicator;
using EntityFakes = KM.Common.Entity.Fakes;
using MarketingAutomationModels = ecn.MarketingAutomation.Models;
using PostModelsControlsFakes = ecn.MarketingAutomation.Models.PostModels.Controls.Fakes;

namespace ECN.MarketingAutomation.Tests.Controllers
{
    public partial class HomeControllerTest
    {
        private const string TotalCount = "TotalCount";
        private const string MethodCompletedCheckAndStrip = "CompletedCheckAndStrip";
        private const int TestZero = 0;
        private const string GetGroupAndCampaignItem = "GetGroupAndCampaignItem";
        private const string AppSettingKmApplication = "KMCommon_Application";
        private const string CheckNameIsUnique = "CheckNameIsUnique";
        private const string MethodSave = "Save";
        private const string FullCopy = "FullCopy";
        private const string GetChildren = "GetChildren";
        private const string FindFormParent = "FindFormParent";
        private const string FindParentBaseType = "FindParentBaseType";
        private const string IsParentDirectOpen = "IsParentDirectOpen";
        private const string GetParentDirectOrForm = "GetParentDirectOrForm";
        private static readonly object[] _controlsForValidateGroupTrigger =
        {
            new Subscribe {ControlType = MarketingAutomationControlType.Subscribe, ControlID = DummyString},
            new Unsubscribe {ControlType = MarketingAutomationControlType.Unsubscribe, ControlID = DummyString}
        };

        [Test]
        public void Index_OnHasAccessIsTrue_ReturnViewResult()
        {
            // Arrange
            ShimCurrentUser();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => true;

            // Act	
            var actualResult = _controller.Index() as ViewResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ViewName.ShouldBeNullOrWhiteSpace());
        }

        [Test]
        public void Index_OnHasAccessIsFalse_RedirectToMain()
        {
            // Arrange
            ShimCurrentUser();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => false;
            const string ExpectedUrl = "/ecn.accounts/main";

            // Act	
            var actualResult = _controller.Index() as RedirectResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Url.ShouldContain(ExpectedUrl));
        }

        [Test]
        public void MAReadToGrid_OnValidCall_ReturnJsonResult()
        {
            // Arrange
            ShimCurrentUser();
            var dataSourceRequest = new DataSourceRequest { PageSize = 1 };
            ShimKendoGridHelper<MarketingAutomationModels::MarketingAutomtionViewModel>.AllInstances
                    .GetGridSortDataSourceRequestString =
                (_, __, ___) => new List<MarketingAutomationModels::GridSort>
                {
                    new MarketingAutomationModels::GridSort(DummyString, DummyString)
                };
            ShimMarketingAutomation
                    .GetAllMarketingAutomationsbySearchInt32UserStringStringStringInt32Int32StringString =
                (_, __, ___, ____, _____, ______, _______, ________, _________) =>
                {
                    var dataSet = new DataSet();
                    var dataTable = new DataTable();
                    dataTable.Columns.Add(
                        nameof(MarketingAutomationModels::MarketingAutomtionViewModel.Name), typeof(string));
                    dataTable.Columns.Add(
                        nameof(MarketingAutomationModels::MarketingAutomtionViewModel.State), typeof(string));
                    dataTable.Columns.Add(
                        nameof(MarketingAutomationModels::MarketingAutomtionViewModel.CreatedDate), typeof(DateTime));
                    dataTable.Columns.Add(
                        nameof(MarketingAutomationModels::MarketingAutomtionViewModel.EndDate), typeof(DateTime));
                    dataTable.Columns.Add
                        (nameof(MarketingAutomationModels::MarketingAutomtionViewModel.StartDate), typeof(DateTime));
                    dataTable.Columns.Add(
                        nameof(MarketingAutomationModels::MarketingAutomtionViewModel.LastPublishedDate), typeof(DateTime));
                    dataTable.Columns.Add(
                        nameof(MarketingAutomationModels::MarketingAutomtionViewModel.UpdatedDate), typeof(DateTime));
                    dataTable.Columns.Add(TotalCount, typeof(int));
                    dataTable.Columns.Add(
                        nameof(MarketingAutomationModels::MarketingAutomtionViewModel.MarketingAutomationID), typeof(int));
                    var row = dataTable.NewRow();
                    row[0] = DummyString;
                    row[1] = DummyString;
                    row[2] = DateTime.Now.ToShortDateString();
                    row[3] = DateTime.Now.ToShortDateString();
                    row[4] = DateTime.Now.ToShortDateString();
                    row[5] = DateTime.Now.ToShortDateString();
                    row[6] = DateTime.Now.ToShortDateString();
                    row[7] = DummyId;
                    row[8] = DummyId;
                    dataTable.Rows.Add(row);
                    dataSet.Tables.Add(dataTable);
                    return dataSet;
                };

            // Act	
            var actualResult = _controller.MAReadToGrid(dataSourceRequest) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var result = actualResult.Data as DataSourceResult;
                    result.ShouldNotBeNull();
                    result.Total.ShouldBe(DummyId);
                });
        }

        [Test]
        public void CompletedCheckAndStrip_OnValidCall_ChangeMarketingAutomationState()
        {
            // Arrange
            var marketingAutomation = new Entities::MarketingAutomation
            {
                EndDate = DateTime.Now.AddMonths(-1),
                State = MarketingAutomationStatus.Published
            };

            // Act	
            _privateObject.Invoke(MethodCompletedCheckAndStrip, marketingAutomation);

            // Assert
            marketingAutomation.ShouldSatisfyAllConditions(
                () => marketingAutomation.State.ShouldBe(MarketingAutomationStatus.Completed),
                () => marketingAutomation.JSONDiagram.ShouldBeNullOrWhiteSpace());
        }

        [Test]
        [TestCase(MarketingAutomationStatus.Published, 1)]
        [TestCase(MarketingAutomationStatus.Archived, -1)]
        public void Archive_UnArchive_OnValidCall_ReturnJsonResult(
            MarketingAutomationStatus marketingAutomationState,
            int monthsToAdd)
        {
            // Arrange
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation
                {
                    State = marketingAutomationState,
                    EndDate = DateTime.Now.AddMonths(monthsToAdd)
                };
            ShimCurrentUser();
            ShimMarketingAutomation.SaveMarketingAutomationUser = (_, __) => DummyId;
            ShimMarketingAutomationHistory.InsertInt32Int32String = (_, __, ___) => { };

            // Act	
            var actualResult = _controller.Archive_UnArchive(DummyId) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(StatusOKCode);
                });
        }

        [Test]
        public void Archive_UnArchive_OnException_ReturnJsonResult()
        {
            // Arrange
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation
                {
                    State = MarketingAutomationStatus.Published,
                    EndDate = DateTime.Now
                };
            ShimCurrentUser();
            ShimMarketingAutomation.SaveMarketingAutomationUser = (_, __) => throw new Exception();
            const string ErrorString = "Could not update automation";

            // Act	
            var actualResult = _controller.Archive_UnArchive(DummyId) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList.Count.ShouldBeGreaterThanOrEqualTo(2);
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldNotBeNull();
                    jsonList[1].ShouldBe(ErrorString);
                });
        }

        [Test]
        [TestCase(DummyId, true)]
        [TestCase(DummyId, false)]
        [TestCase(null, true)]
        public void LoadForm_MultipleValues_ReturnPartialViewResult(int? id, bool isCopy)
        {
            // Arrange
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation();
            ShimCurrentUser();
            const string ExpectedViewName = "Partials/_Automation";

            // Act	
            var actualResult = _controller.LoadForm(id, isCopy) as PartialViewResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ViewName.ShouldContain(ExpectedViewName),
                () =>
                {
                    var model = actualResult.Model as MarketingAutomationModels::DiagramPostModel;
                    model.ShouldNotBeNull();
                    model.IsCopy.ShouldBe(isCopy);
                    if (id.HasValue)
                    {
                        model.IsCreate.ShouldBeFalse();
                    }
                    else
                    {
                        model.IsCreate.ShouldBeTrue();
                    }
                });
        }

        [Test]
        [TestCase(DummyId)]
        [TestCase(TestZero)]
        public void LoadPause_MultipleValues_ReturnPartialViewResult(int id)
        {
            // Arrange
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation();
            ShimCurrentUser();
            const string ExpectedViewName = "Partials/_Automation";

            // Act	
            var actualResult = _controller.LoadPause(id) as PartialViewResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ViewName.ShouldContain(ExpectedViewName),
                () =>
                {
                    var model = actualResult.Model as MarketingAutomationModels::DiagramPostModel;
                    model.ShouldNotBeNull();
                    if (id > 0)
                    {
                        model.IsCreate.ShouldBeFalse();
                    }
                    else
                    {
                        model.IsCreate.ShouldBeTrue();
                    }
                });
        }

        [Test]
        public void Create_OnValidCall_InvokeProcessCreate()
        {
            // Arrange
            var processCreateInvoked = false;
            ShimHomeController.AllInstances.ProcessCreateDiagramPostModelBoolean = (_, __, ___) =>
            {
                processCreateInvoked = true;
                return null;
            };

            // Act	
            _controller.Create(new MarketingAutomationModels::DiagramPostModel());

            // Assert
            _controller.ShouldSatisfyAllConditions(() => processCreateInvoked.ShouldBeTrue());
        }

        [Test]
        public void Delete_OnValidCall_ReturnJavaScriptRedirectResult()
        {
            // Arrange
            var itemDeleted = false;
            SetShimForCurrentUser();
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation();
            ShimMarketingAutomation.DeleteMarketingAutomationUser = (_, __) => { itemDeleted = true; };
            ShimMarketingAutomationHistory.InsertInt32Int32String = (_, __, ___) => { };
            const string ExpectedActionName = "Index";

            // Act	
            var actualResult = _controller.Delete(DummyId) as JavaScriptRedirectResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ActionName.ShouldContain(ExpectedActionName),
                () => itemDeleted.ShouldBeTrue());
        }

        [Test]
        public void Copy_OnValidCall_InvokeProcessCreate()
        {
            // Arrange
            var processCreateInvoked = false;
            ShimHomeController.AllInstances.ProcessCreateDiagramPostModelBoolean = (_, __, ___) =>
            {
                processCreateInvoked = true;
                return null;
            };

            // Act	
            _controller.Copy(new MarketingAutomationModels::DiagramPostModel());

            // Assert
            _controller.ShouldSatisfyAllConditions(() => processCreateInvoked.ShouldBeTrue());
        }

        [Test]
        public void ValidateCampaignItem_OnValidCall_ReturnJsonResult()
        {
            // Arrange
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation();
            ShimCurrentUser();
            PostModelsControlsFakes::ShimCampaignItem.AllInstances.ValidateUser = (_, __) => { };

            // Act	
            var actualResult = _controller.ValidateCampaignItem(new CampaignItem
            {
                MarketingAutomationID = DummyId
            }) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(StatusOKCode);
                });
        }

        [Test]
        public void ValidateCampaignItem_OnECNException_ReturnJsonResult()
        {
            // Arrange
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation();
            ShimCurrentUser();
            const string ErrorString = "An error occurred";
            PostModelsControlsFakes::ShimCampaignItem.AllInstances.ValidateUser = (_, __) =>
                throw new ECNException(new List<ECNError>
                {
                    new ECNError
                    {
                        ErrorMessage = ErrorString
                    }
                });

            // Act	
            var actualResult = _controller.ValidateCampaignItem(new CampaignItem
            {
                MarketingAutomationID = DummyId
            }) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList.Count.ShouldBeGreaterThanOrEqualTo(2);
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldNotBeNull();
                    jsonList[1].ShouldContain(ErrorString);
                });
        }

        [Test]
        public void ValidateCampaignItem_OnException_ReturnJsonResult()
        {
            // Arrange
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation();
            ShimCurrentUser();
            const string ErrorString = "An error occurred";
            PostModelsControlsFakes::ShimCampaignItem.AllInstances.ValidateUser = (_, __) => throw new Exception();

            // Act	
            var actualResult = _controller.ValidateCampaignItem(new CampaignItem
            {
                MarketingAutomationID = DummyId
            }) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList.Count.ShouldBeGreaterThanOrEqualTo(2);
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldNotBeNull();
                    jsonList[1].ShouldBe(ErrorString);
                });
        }

        [Test]
        public void ValidateWait_OnValidCall_ReturnJsonResult()
        {
            // Arrange
            ShimAutomationBaseController.AllInstances.FindParentControlBase = (_, __) => new Wait();
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation();
            ShimCurrentUser();
            PostModelsControlsFakes::ShimWait.AllInstances.ValidateControlBaseMarketingAutomationUser =
                (_, __, ___, ____) => { };
            ShimAutomationBaseController.AllInstances.GetTotalWaitTimeWaitListOfControlBaseListOfConnectorDateTimeRef =
            (
                AutomationBaseController obj,
                Wait wait,
                List<ControlBase> controls,
                List<Connector> connectors,
                ref DateTime parentSendTime) => DummyId;

            // Act	
            var actualResult = _controller.ValidateWait(
                new MarketingAutomationPostModel
                {
                    EndDate = DateTime.Now.AddMonths(1)
                },
                new Wait()) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(StatusOKCode);
                });
        }

        [Test]
        public void ValidateWait_OnInValidEndDate_ReturnJsonResult()
        {
            // Arrange
            ShimAutomationBaseController.AllInstances.FindParentControlBase = (_, __) => new Wait();
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation();
            ShimCurrentUser();
            PostModelsControlsFakes::ShimWait.AllInstances.ValidateControlBaseMarketingAutomationUser =
                (_, __, ___, ____) => { };
            ShimAutomationBaseController.AllInstances.GetTotalWaitTimeWaitListOfControlBaseListOfConnectorDateTimeRef =
            (
                AutomationBaseController obj,
                Wait wait,
                List<ControlBase> controls,
                List<Connector> connectors,
                ref DateTime parentSendTime) =>
            {
                parentSendTime = DateTime.Now;
                return DummyId;
            };
            const string ErrorString = "Wait time is outside of the Automation date range";

            // Act	
            var actualResult = _controller.ValidateWait(
                new MarketingAutomationPostModel
                {
                    EndDate = DateTime.Now.AddMonths(-1)
                },
                new Wait()) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList.Count.ShouldBeGreaterThanOrEqualTo(2);
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldNotBeNull();
                    jsonList[1].ShouldContain(ErrorString);
                });
        }

        [Test]
        public void ValidateWait_OnECNException_ReturnJsonResult()
        {
            // Arrange
            ShimAutomationBaseController.AllInstances.FindParentControlBase = (_, __) => new Wait();
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation();
            ShimCurrentUser();
            const string ErrorString = "ErrorString";
            PostModelsControlsFakes::ShimWait.AllInstances.ValidateControlBaseMarketingAutomationUser =
                (_, __, ___, ____) => throw new ECNException(new List<ECNError>
                {
                    new ECNError
                    {
                        ErrorMessage = ErrorString
                    }
                });
            ShimAutomationBaseController.AllInstances.GetTotalWaitTimeWaitListOfControlBaseListOfConnectorDateTimeRef =
            (
                AutomationBaseController obj,
                Wait wait,
                List<ControlBase> controls,
                List<Connector> connectors,
                ref DateTime parentSendTime) => DummyId;

            // Act	
            var actualResult = _controller.ValidateWait(
                new MarketingAutomationPostModel
                {
                    EndDate = DateTime.Now.AddMonths(-1)
                },
                new Wait()) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList.Count.ShouldBeGreaterThanOrEqualTo(2);
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldNotBeNull();
                    jsonList[1].ShouldContain(ErrorString);
                });
        }

        [Test]
        public void ValidateWait_OnException_ReturnJsonResult()
        {
            // Arrange
            ShimAutomationBaseController.AllInstances.FindParentControlBase = (_, __) => new Wait();
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation();
            ShimCurrentUser();
            PostModelsControlsFakes::ShimWait.AllInstances.ValidateControlBaseMarketingAutomationUser =
                (_, __, ___, ____) => throw new Exception();
            ShimAutomationBaseController.AllInstances.GetTotalWaitTimeWaitListOfControlBaseListOfConnectorDateTimeRef =
            (
                AutomationBaseController obj,
                Wait wait,
                List<ControlBase> controls,
                List<Connector> connectors,
                ref DateTime parentSendTime) => DummyId;
            const string ErrorString = "An error occurred";

            // Act	
            var actualResult = _controller.ValidateWait(
                new MarketingAutomationPostModel
                {
                    EndDate = DateTime.Now.AddMonths(-1)
                },
                new Wait()) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList.Count.ShouldBeGreaterThanOrEqualTo(2);
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldNotBeNull();
                    jsonList[1].ShouldContain(ErrorString);
                });
        }

        [Test]
        public void GetGroupAndCampaignItem_OnException_LogErrorAndReturnFalse()
        {
            // Arrange
            var errorLogged = false;
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase =
                (_, __) => throw new Exception();
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase =
                (_, __) => throw new Exception();
            EntityFakes::ShimApplicationLog.LogNonCriticalErrorExceptionStringInt32StringInt32Int32 =
                (_, __, ___, ____, _____, ______) => { errorLogged = true; };
            _appSettings.Add(AppSettingKmApplication, DummyId.ToString());

            // Act	
            var actualResult = _privateObject.Invoke(GetGroupAndCampaignItem,
               new CampaignItem(),
               new Group(),
               new Wait()) as bool?;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.HasValue.ShouldBeTrue(),
                () => actualResult.Value.ShouldBeFalse(),
                () => errorLogged.ShouldBeTrue());
        }

        [Test]
        public void ValidateFormControl_OnValidCall_ReturnJsonResult()
        {
            // Arrange
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation();
            ShimCurrentUser();
            PostModelsControlsFakes::ShimForm.AllInstances.ValidateControlBaseUser = (_, __, ___) => { };
            ShimAutomationBaseController.AllInstances.FindParentControlBase = (_, __) => new Form();
            ShimMarketingAutomationPostModel.AllInstances.GetAllControls = (_) => new List<ControlBase>
            {
                new Form
                {
                    ControlID = DummyString
                }
            };

            // Act	
            var actualResult = _controller.ValidateFormControl(
                new MarketingAutomationPostModel(),
                new Form(),
                DummyString) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(StatusOKCode);
                });
        }

        [Test]
        public void ValidateFormControl_OnECNException_ReturnJsonResult()
        {
            // Arrange
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation();
            ShimCurrentUser();
            const string ErrorString = "An error occurred";
            ShimAutomationBaseController.AllInstances.FindParentControlBase = (_, __) => new Form();
            PostModelsControlsFakes::ShimForm.AllInstances.ValidateControlBaseUser = (_, __, ___) =>
                throw new ECNException(new List<ECNError>
                {
                    new ECNError
                    {
                        ErrorMessage = ErrorString
                    }
                });
            ShimMarketingAutomationPostModel.AllInstances.GetAllControls = (_) => new List<ControlBase>
            {
                new Form
                {
                    ControlID = DummyString
                }
            };

            // Act	
            var actualResult = _controller.ValidateFormControl(
                new MarketingAutomationPostModel(),
                new Form(),
                DummyString) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList.Count.ShouldBeGreaterThanOrEqualTo(2);
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldNotBeNull();
                    jsonList[1].ShouldContain(ErrorString);
                });
        }

        [Test]
        public void ValidateFormControl_OnException_ReturnJsonResult()
        {
            // Arrange
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation();
            ShimCurrentUser();
            const string ErrorString = "An error occurred";
            ShimAutomationBaseController.AllInstances.FindParentControlBase = (_, __) => new Form();
            PostModelsControlsFakes::ShimForm.AllInstances.ValidateControlBaseUser = (_, __, ___) => throw new Exception();
            ShimMarketingAutomationPostModel.AllInstances.GetAllControls = _ => new List<ControlBase>
            {
                new Form
                {
                    ControlID = DummyString
                }
            };

            // Act	
            var actualResult = _controller.ValidateFormControl(
                new MarketingAutomationPostModel(),
                new Form(),
                DummyString) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList.Count.ShouldBeGreaterThanOrEqualTo(2);
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldNotBeNull();
                    jsonList[1].ShouldBe(ErrorString);
                });
        }

        [Test]
        public void ValidateGroup_OnValidCall_ReturnJsonResult()
        {
            // Arrange
            PostModelsControlsFakes::ShimGroup.AllInstances.Validate = (_) => { };

            // Act	
            var actualResult = _controller.ValidateGroup(new Group()) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(StatusOKCode);
                });
        }

        [Test]
        public void ValidateGroup_OnECNException_ReturnJsonResult()
        {
            // Arrange
            const string ErrorString = "An error occurred";
            PostModelsControlsFakes::ShimGroup.AllInstances.Validate = (_) =>
                throw new ECNException(new List<ECNError>
                {
                    new ECNError
                    {
                        ErrorMessage = ErrorString
                    }
                });

            // Act	
            var actualResult = _controller.ValidateGroup(new Group()) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList.Count.ShouldBeGreaterThanOrEqualTo(2);
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldNotBeNull();
                    jsonList[1].ShouldContain(ErrorString);
                });
        }

        [Test]
        public void ValidateGroup_OnException_ReturnJsonResult()
        {
            // Arrange
            const string ErrorString = "An error occurred";
            PostModelsControlsFakes::ShimGroup.AllInstances.Validate = (_) => throw new Exception();

            // Act	
            var actualResult = _controller.ValidateGroup(new Group()) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList.Count.ShouldBeGreaterThanOrEqualTo(2);
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldNotBeNull();
                    jsonList[1].ShouldBe(ErrorString);
                });
        }

        [Test]
        [TestCaseSource(nameof(_controlsForValidateGroupTrigger))]
        public void ValidateGroupTrigger_OnValidCall_ReturnJsonResult(ControlBase control)
        {
            // Arrange
            ShimCurrentUser();
            ShimMarketingAutomationPostModel.AllInstances.GetAllControls = _ => new List<ControlBase> { control };
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (_, __) => new Group();
            PostModelsControlsFakes::ShimSubscribe.AllInstances.ValidateGroupUser = (_, __, ___) => { };
            PostModelsControlsFakes::ShimUnsubscribe.AllInstances.ValidateGroupUser = (_, __, ___) => { };

            // Act	
            var actualResult = _controller.ValidateGroupTrigger(
                new MarketingAutomationPostModel(),
                new Wait(),
                DummyString) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(StatusOKCode);
                });
        }

        [Test]
        public void ValidateGroupTrigger_OnECNException_ReturnJsonResult()
        {
            // Arrange
            ShimCurrentUser();
            const string ErrorString = "ErrorString";
            ShimMarketingAutomationPostModel.AllInstances.GetAllControls = _ => new List<ControlBase>
            {
                new Subscribe {ControlType = MarketingAutomationControlType.Subscribe, ControlID = DummyString}
            };
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (_, __) => new Group();
            PostModelsControlsFakes::ShimSubscribe.AllInstances.ValidateGroupUser = (_, __, ___) => throw new ECNException(new List<ECNError>
            {
                new ECNError
                {
                    ErrorMessage = ErrorString
                }
            });

            // Act	
            var actualResult = _controller.ValidateGroupTrigger(
                new MarketingAutomationPostModel(),
                new Wait(),
                DummyString) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList.Count.ShouldBeGreaterThanOrEqualTo(2);
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldNotBeNull();
                    jsonList[1].ShouldContain(ErrorString);
                });
        }

        [Test]
        public void ValidateGroupTrigger_OnException_ReturnJsonResult()
        {
            // Arrange
            ShimCurrentUser();
            const string ErrorString = "An error occurred";
            ShimMarketingAutomationPostModel.AllInstances.GetAllControls = _ => new List<ControlBase>
            {
                new Subscribe {ControlType = MarketingAutomationControlType.Subscribe, ControlID = DummyString}
            };
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (_, __) => new Group();
            PostModelsControlsFakes::ShimSubscribe.AllInstances.ValidateGroupUser = (_, __, ___) => throw new Exception();

            // Act	
            var actualResult = _controller.ValidateGroupTrigger(
                new MarketingAutomationPostModel(),
                new Wait(),
                DummyString) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.ShouldNotBeEmpty();
                    jsonList.Count.ShouldBeGreaterThanOrEqualTo(2);
                    jsonList[0].ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldNotBeNull();
                    jsonList[1].ShouldBe(ErrorString);
                });
        }

        [Test]
        public void CheckNameIsUnique_OnValidCall_ReturnBoolean()
        {
            // Arrange
            ShimCurrentUser();
            ShimECNSession.AllInstances.BaseChannelIDGet = _ => DummyId;
            ShimMarketingAutomation.ExistsInt32StringInt32 = (_, __, ___) => true;

            // Act	
            var actualResult = _privateObject.Invoke(CheckNameIsUnique, DummyString, DummyId) as bool?;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.HasValue.ShouldBeTrue(),
                () => actualResult.Value.ShouldBeFalse());
        }

        [Test]
        [TestCase(true, false, MarketingAutomationModels::DiagramFromTemplate.No)]
        [TestCase(true, false, MarketingAutomationModels::DiagramFromTemplate.Yes)]
        [TestCase(false, true, MarketingAutomationModels::DiagramFromTemplate.No)]
        [TestCase(false, false, MarketingAutomationModels::DiagramFromTemplate.No)]
        public void Save_OnValidCall_ReturnInt(
            bool isCreate,
            bool isCopy,
            MarketingAutomationModels::DiagramFromTemplate fromTemplate)
        {
            // Arrange
            ShimCurrentUser();
            ShimTemplateViewModel.AllInstances.getSingleDiagramInt32 = (_, __) =>
                new MarketingAutomationModels::TemplateViewModel();
            ShimMAControl.GetByMarketingAutomationIDInt32 = _ => null;
            ShimMAConnector.GetByMarketingAutomationIDInt32 = _ => null;
            ShimControlBase.GetModelsForCopyFromControlBaseListOfControlBaseListOfConnectorListOfConnectorRefInt32 = (
                List<ControlBase> controls,
                List<Connector> origConnectors,
                ref List<Connector> connectors,
                int maId) => new List<ControlBase>();
            ShimConnector.GetConnectorsForCopyListOfConnectorInt32 = (_, __) => new List<Connector>();
            ShimMarketingAutomation.SaveMarketingAutomationUser = (_, __) => DummyId;
            ShimMarketingAutomationHistory.InsertInt32Int32String = (_, __, ___) => { };
            const string JsonDiagram = "{shapes:[], connections: []}";

            // Act	
            var actualResult = _privateObject.Invoke(
                MethodSave,
                new MarketingAutomationModels::DiagramPostModel
                {
                    IsCreate = isCreate,
                    Diagram = JsonDiagram,
                    FromTemplate = fromTemplate,
                    TemplateId = DummyId,
                    Id = DummyId
                },
                isCopy) as int?;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.HasValue.ShouldBeTrue(),
                () => actualResult.Value.ShouldBe(DummyId));
        }

        [Test]
        public void FullCopy_OnValidCall_ReturnInt()
        {
            // Arrange
            ShimCurrentUser();
            ShimMarketingAutomation.SaveMarketingAutomationUser = (_, __) => DummyId;
            ShimMarketingAutomationHistory.InsertInt32Int32String = (_, __, ___) => { };

            // Act	
            var actualResult = _privateObject.Invoke(
                FullCopy,
                new MarketingAutomationModels::DiagramPostModel()) as int?;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.HasValue.ShouldBeTrue(),
                () => actualResult.Value.ShouldBe(DummyId));
        }

        [Test]
        public void GetTemplateView_OnValidCall_ReturnInt()
        {
            // Arrange
            ShimCurrentUser();
            ShimMarketingAutomation.SaveMarketingAutomationUser = (_, __) => DummyId;
            ShimMarketingAutomationHistory.InsertInt32Int32String = (_, __, ___) => { };
            const string ExpectedViewName = "Partials/_TemplateViewer";
            ShimTemplateViewModel.AllInstances.getSingleDiagramInt32 =
                (_, __) => new MarketingAutomationModels::TemplateViewModel();

            // Act	
            var actualResult = _controller.GetTemplateView(DummyId) as PartialViewResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ViewName.ShouldBe(ExpectedViewName));
        }

        [Test]
        public void Logout_OnValidCall_ReturnInt()
        {
            // Arrange
            ShimFormsAuthentication.SignOut = () => { };
            const string ExpectedUrl = "/EmailMarketing.Site/Login/Logout";

            // Act	
            var actualResult = _controller.Logout() as RedirectResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Url.ShouldContain(ExpectedUrl));
        }

        [Test]
        public void GetChildren_OnValidCall_ReturnList()
        {
            // Arrange
            ShimFormsAuthentication.SignOut = () => { };

            // Act	
            var actualResult = _privateObject.Invoke(
                GetChildren,
                new Connector
                {
                    to = new to
                    {
                        shapeId = DummyString
                    }
                },
                new List<ControlBase>
                {
                    new Wait
                    {
                        ControlID = DummyString
                    }
                }) as List<ControlBase>;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldNotBeEmpty(),
                () =>
                {
                    actualResult[0].ShouldNotBeNull();
                    actualResult[0].ControlID.ShouldBe(DummyString);
                });
        }

        [Test]
        [TestCase(MarketingAutomationControlType.Direct_Open)]
        [TestCase(MarketingAutomationControlType.Form)]
        public void IsParentDirectOpen_OnValidCall_ReturnBoolean(MarketingAutomationControlType type)
        {
            // Arrange
            _privateObject.SetProperty(AllConnectorsProperty, new List<Connector>
            {
                new Connector
                {
                    to = new to {shapeId = DummyString},
                    from = new from {shapeId = DummyString}
                }
            });
            _privateObject.SetProperty(AllControlsProperty, new List<ControlBase>
            {
                new Form { ControlID = DummyString, ControlType = type }
            });

            // Act	
            var actualResult = _privateObject.Invoke(
                IsParentDirectOpen,
                new Wait
                {
                    ControlID = DummyString
                }) as bool?;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.HasValue.ShouldBeTrue(),
                () =>
                {
                    if (type.Equals(MarketingAutomationControlType.Form))
                    {
                        actualResult.Value.ShouldBeFalse();
                    }
                    else
                    {
                        actualResult.Value.ShouldBeTrue();
                    }
                });
        }

        [Test]
        public void ClientDropDown_OnValidCall_ReturnPartialViewResult()
        {
            // Arrange
            ShimHomeController.AllInstances.CurrentClientGroupIDGet = _ => DummyId;
            ShimHomeController.AllInstances.CurrentClientIDGet = _ => DummyId;
            ShimHomeController.AllInstances.RepopulateDropDownsClientDropDown =
                (_, __) => new MarketingAutomationModels::ClientDropDown();
            const string ExpectedViewName = "~/Views/Shared/Partials/_ClientDropDown.cshtml";

            // Act	
            var actualResult = _controller._ClientDropDown(
                new MarketingAutomationModels::ClientDropDown(),
                false) as PartialViewResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ViewName.ShouldContain(ExpectedViewName));
        }
    }
}

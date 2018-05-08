using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using ecn.MarketingAutomation.Controllers;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ecn.MarketingAutomation.Models.PostModels.Fakes;
using ecn.MarketingAutomation.Tests.Setup;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using Entities = ECN_Framework_Entities.Communicator;
using MarketingAutomationModels = ecn.MarketingAutomation.Models;
using PrivateObject = Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject;

namespace ecn.MarketingAutomation.Tests.Controllers
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class DiagramsControllerTest
    {
        private const int DummyId = 1;
        private const string DummyString = "DummyString";
        private const string AllControlsProperty = "AllControls";
        private const string AllConnectorsProperty = "AllConnectors";
        private const string IsParentDirectOpen = "IsParentDirectOpen";
        private const string MethodDeleteEcnObject = "DeleteECNObject";
        const string EmptyString = "";
        private const string GetChildren = "GetChildren";
        private readonly DateTime DateTimeNow = DateTime.Now;
        private IDisposable _shimsContext;
        private PrivateObject _diagramsControllerPrivateObject;
        private DiagramsController _diagramsController;
        private ExternalFakesContext _externalFakesContext;
        private Random _random = new Random();

        [SetUp]
        public void Setup()
        {
            _shimsContext = ShimsContext.Create();
            _diagramsController = new DiagramsController();
            _diagramsControllerPrivateObject = new PrivateObject(_diagramsController);
            _externalFakesContext = new ExternalFakesContext();
            ShimCurrentUser();
            SetAllConnectorsProperty();
            SetAllControlsProperty();
            _connectorId = GetUniqueString();
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
        }

        private string GetUniqueString()
        {
            return Guid.NewGuid().ToString();
        }

        private decimal GetAnyDecimal()
        {
            var random = new Random();
            return (decimal)random.NextDouble();
        }

        private int GetAnyNumber(int? rangeMinimum = null, int? rangeMaximum = null)
        {
            const int RandomRangeMinimum = 1000000;
            const int RandomRangeMaximum = RandomRangeMinimum * 10;
            return _random.Next(rangeMinimum ?? RandomRangeMinimum, rangeMaximum ?? RandomRangeMaximum);
        }

        private DateTime GetAnyTime()
        {
            return DateTime.Now.AddSeconds(GetAnyNumber());
        }

        private void ShimCurrentUser()
        {
            ShimECNSession.Constructor = (instance) => { };
            var shimSession = new ShimECNSession();
            shimSession.ClearSession = () => { };
            shimSession.Instance.CurrentUser = new User { UserID = 1, UserName = "TestUser" };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = DummyId };
            shimSession.Instance.CurrentCustomer = new Customer { CustomerID = DummyId };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }

        [Test]
        [TestCase(MarketingAutomationControlType.CampaignItem)]
        [TestCase(MarketingAutomationControlType.Click)]
        [TestCase(MarketingAutomationControlType.NoClick)]
        [TestCase(MarketingAutomationControlType.NoOpen)]
        [TestCase(MarketingAutomationControlType.NotSent)]
        [TestCase(MarketingAutomationControlType.Open)]
        [TestCase(MarketingAutomationControlType.Open_NoClick)]
        [TestCase(MarketingAutomationControlType.Sent)]
        [TestCase(MarketingAutomationControlType.Suppressed)]
        [TestCase(MarketingAutomationControlType.Direct_Click)]
        [TestCase(MarketingAutomationControlType.Direct_Open)]
        [TestCase(MarketingAutomationControlType.Subscribe)]
        [TestCase(MarketingAutomationControlType.Unsubscribe)]
        public void DeleteECNObject_MultipleCases_ECNObjectDeleted(MarketingAutomationControlType controlType)
        {
            // Arrange
            var maControl = new Entities::MAControl
            {
                ECNID = DummyId,
                ControlType = controlType
            };
            var itemDeleted = false;
            ShimCampaignItem.DeleteInt32UserBoolean = (_, __, ___) =>
            {
                itemDeleted = true;
            };
            ShimCurrentUser();
            ShimLayoutPlans.GetByLayoutPlanIDInt32User = (_, __) => new Entities::LayoutPlans
            {
                LayoutPlanID = DummyId,
                BlastID = DummyId
            };
            ShimLayoutPlans.DeleteByLayoutPlanIDInt32User = (_, __) => { };
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (_, __) => new Entities::CampaignItem
            {
                CampaignItemID = DummyId
            };

            // Act	
            _diagramsControllerPrivateObject.Invoke(MethodDeleteEcnObject, maControl);

            // Assert
            itemDeleted.ShouldBeTrue();
        }

        [Test]
        [TestCase(MarketingAutomationControlType.Start)]
        [TestCase(MarketingAutomationControlType.End)]
        [TestCase(MarketingAutomationControlType.Wait)]
        [TestCase(MarketingAutomationControlType.Group)]
        public void DeleteECNObject_NoDeleteAction_ECNObjectNotDeleted(MarketingAutomationControlType controlType)
        {
            // Arrange
            var maControl = new Entities::MAControl
            {
                ECNID = DummyId,
                ControlType = controlType
            };
            var itemDeleted = false;
            ShimCampaignItem.DeleteInt32UserBoolean = (_, __, ___) =>
            {
                itemDeleted = true;
            };

            // Act	
            _diagramsControllerPrivateObject.Invoke(MethodDeleteEcnObject, maControl);

            // Assert
            itemDeleted.ShouldBeFalse();
        }

        [Test]
        public void DeleteECNObject_DirectNoOpen_ECNObjectDeleted()
        {
            // Arrange
            var maControl = new Entities::MAControl
            {
                ECNID = DummyId,
                ControlType = MarketingAutomationControlType.Direct_NoOpen
            };
            var itemDeleted = false;
            var triggerPlansDeleted = false;
            ShimCampaignItem.DeleteInt32UserBoolean = (_, __, ___) =>
            {
                itemDeleted = true;
            };
            ShimTriggerPlans.DeleteInt32User = (i, user) =>
            {
                triggerPlansDeleted = true;
            };
            ShimTriggerPlans.GetByTriggerPlanIDInt32User = (i, u) => new Entities.TriggerPlans()
            {
                TriggerPlanID = 1,
                BlastID = 1
            };
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (_, __) => new Entities::CampaignItem
            {
                CampaignItemID = DummyId
            };

            // Act	
            var result = _diagramsControllerPrivateObject.Invoke(MethodDeleteEcnObject, maControl);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => itemDeleted.ShouldBeTrue(),
                () => triggerPlansDeleted.ShouldBeTrue()
            );
        }

        [Test]
        [TestCase(false, DummyId)]
        [TestCase(true, default(int))]
        [TestCase(true, DummyId)]
        public void Edit_OnHasAccessIsFalseOrIdIsZeroOrException_ReturnRedirectToAction(bool hasAccess, int Id)
        {
            // Arrange
            ShimCurrentUser();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => hasAccess;
            const string ExpectedViewName = "Index";
            ShimCustomer.GetByBaseChannelIDInt32 = _ => new List<Customer>();
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => throw new SecurityException();

            // Act	
            var actualResult = _diagramsController.Edit(Id) as RedirectToRouteResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.RouteName.Contains(ExpectedViewName));
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void Edit_OnValidCall_ReturnViewResult(bool hasControls)
        {
            // Arrange
            ShimCurrentUser();
            const string JsonDiagram = "{shapes:[], connections: []}";
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => true;
            ShimCustomer.GetByBaseChannelIDInt32 = _ => new List<Customer>();
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation
                {
                    JSONDiagram = JsonDiagram,
                    EndDate = DateTimeNow,
                    StartDate = DateTimeNow,
                    State = MarketingAutomationStatus.Archived,
                    Controls = new List<Entities::MAControl>(),
                    CreatedDate = DateTimeNow,
                    CreatedUserID = DummyId
                };
            ShimControlBase.GetModelsFromObjectListOfMAControlListOfMAConnectorListOfControlBaseUser =
                (_, __, ___, ____) =>
                {
                    if(hasControls)
                    {
                        return new List<ControlBase>
                        {
                            new Wait()
                        };
                    }
                    return new List<ControlBase>();
                };
            ShimControlBase.GetJSONDiagramFromControlsListOfControlBaseListOfConnectorInt32 = (_, __, ___) => DummyString;
            ShimControlBase.SerializeListOfControlBaseListOfConnectorInt32 = (_, __, ___) => DummyString;

            // Act	
            var actualResult = _diagramsController.Edit(DummyId) as ViewResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () =>
                {
                    var model = actualResult.Model as MarketingAutomationPostModel;
                    model.ShouldNotBeNull();
                    model.JSONDiagram.ShouldContain(DummyString);
                    model.StartDate.ShouldBe(DateTimeNow);
                    model.EndDate.ShouldBe(DateTimeNow);
                    model.CreatedDate.ShouldBe(DateTimeNow);
                    model.CreatedUserID.ShouldBe(DummyId);
                });
        }

        [Test]
        public void Heatmap_OnIdIsZero_ReturnRedirectToAction()
        {
            // Arrange
            const string ExpectedViewName = "Index";

            // Act	
            var actualResult = _diagramsController.Heatmap(0) as RedirectToRouteResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.RouteName.Contains(ExpectedViewName));
        }

        [Test]
        public void Heatmap_OnValidCall_ReturnViewResult()
        {
            // Arrange
            ShimCurrentUser();
            const string JsonDiagram = "{shapes:[], connections: []}";
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (_, __, ___) => new Entities::MarketingAutomation
                {
                    JSONDiagram = JsonDiagram,
                    EndDate = DateTimeNow,
                    StartDate = DateTimeNow,
                    State = MarketingAutomationStatus.Archived,
                    Controls = new List<Entities::MAControl>()
                };
            ShimControlBase.GetModelsFromObjectListOfMAControlListOfMAConnectorListOfControlBaseUser =
                (_, __, ___, ____) => new List<ControlBase>();
            ShimControlBase.SerializeListOfControlBaseListOfConnectorInt32 = (_, __, ___) => DummyString;
            ShimDiagramsController.AllInstances.GetHeatMapStatsListOfControlBaseListOfConnectorRef =
                (DiagramsController obj, List<ControlBase> controls, ref List<Connector> connectors) =>
                new List<ControlBase>();

            // Act	
            var actualResult = _diagramsController.Heatmap(DummyId) as ViewResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () =>
                {
                    var model = actualResult.Model as MarketingAutomationModels::DiagramViewModel;
                    model.ShouldNotBeNull();
                    model.StartDate.ShouldBe(DateTimeNow);
                    model.EndDate.ShouldBe(DateTimeNow);
                });
        }

        [Test]
        public void GetChildren_OnIdIsZero_ReturnRedirectToAction()
        {
            // Arrange
            var connector = new Connector
            {
                to = new to
                {
                    shapeId = DummyString
                }
            };
            var controls = new List<ControlBase>
            {
                new Wait
                {
                    ControlID = DummyString
                }
            };

            // Act	
            var actualResult = _diagramsControllerPrivateObject.
                Invoke(GetChildren, connector, controls) as List<ControlBase>;

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
        public void IsParentDirectOpen_OnValidCall_ReturnBoolean()
        {
            // Arrange
            var control = new Wait
            {
                ControlID = DummyString
            };
            _diagramsControllerPrivateObject.SetProperty(AllConnectorsProperty, new List<Connector>
            {
                new Connector
                {
                    to = new to {shapeId = DummyString},
                    from = new from {shapeId = DummyString}
                }
            });
            _diagramsControllerPrivateObject.SetProperty(AllControlsProperty, new List<ControlBase>
            {
                new Direct_Open { ControlID = DummyString, ControlType = MarketingAutomationControlType.Direct_Open }
            });

            // Act	
            var actualResult = _diagramsControllerPrivateObject.Invoke(IsParentDirectOpen, control) as bool?;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                 () => actualResult.ShouldNotBeNull(),
                 () => actualResult.HasValue.ShouldBeTrue(),
                 () => actualResult.Value.ShouldBeTrue());
        }
    }
}

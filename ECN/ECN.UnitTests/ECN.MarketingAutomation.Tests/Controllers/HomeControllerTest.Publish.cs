using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Reflection;
using System.Web.Mvc;
using System.Linq;
using System.Text;
using System.Transactions.Fakes;
using ecn.MarketingAutomation.Controllers;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Entities = ECN_Framework_Entities.Communicator;
using Shouldly;
using KMPlatform.Entity;
using ecn.MarketingAutomation.Models.PostModels.ECN_Objects;
using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects;
using KM.Common.Entity.Fakes;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.MarketingAutomation.Tests.Controllers
{
    public partial class HomeControllerTest
    {
        private readonly NameValueCollection _appSettings = new NameValueCollection();

        [Test]
        public void Publish_OnException_ReturnErrorResponse()
        {
            // Arrange
            SetShimsForPublish(DummyString);

            // Act	
            var actualResult = _controller.Publish(1) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.Count.ShouldBeGreaterThan(0);
                    jsonList[0].ShouldBe(ErrorCode);
                });
        }

        [Test]
        public void Publish_OnShapesIsEmpty_ReturnErrorResponse()
        {
            // Arrange
            const string JsonDiagram = "{shapes: [], connections: []}";
            SetShimForCurrentUser();
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (id, child, usr) => new Entities::MarketingAutomation
                {
                    JSONDiagram = JsonDiagram
                };
            ShimAutomationBaseController.AllInstances.ValidatePublishListOfControlBaseMarketingAutomationECNExceptionRef=
                (AutomationBaseController obj, List<ControlBase> shapes, Entities::MarketingAutomation ma,
                    ref ECNException ecnEx) =>
                {
                    ecnEx.ErrorList.Add(new ECNError());
                };

            // Act	
            var actualResult = _controller.Publish(1) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.Count.ShouldBeGreaterThan(0);
                    jsonList[0].ShouldBe(ErrorCode);
                });
        }

        [Test]
        public void Publish_OnECNException_ReturnErrorResponse()
        {
            // Arrange
            const string JsonDiagram = "{shapes: [{category:16}, {category:5}], connections: []}";
            SetShimForCurrentUser();
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (id, child, usr) => new Entities::MarketingAutomation
                {
                    JSONDiagram = JsonDiagram
                };
            ShimAutomationBaseController.AllInstances.ValidatePublishListOfControlBaseMarketingAutomationECNExceptionRef=
                (AutomationBaseController obj, List<ControlBase> shapes, Entities::MarketingAutomation ma,
                    ref ECNException ecnEx) =>
                {
                    ecnEx.ErrorList.Add(new ECNError
                    {
                        ErrorMessage = DummyString
                    });
                };

            // Act	
            var actualResult = _controller.Publish(1) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.Count.ShouldBeGreaterThan(0);
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldContain(DummyString);
                });
        }

        [Test]
        public void Publish_OnSecurityException_ReturnErrorResponse()
        {
            // Arrange
            const string JsonDiagram = "{shapes: [{category:16}, {category:5}], connections: []}";
            const string ErrorString = "You do not have sufficient permissions to publish an automation.";
            SetShimForCurrentUser();
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (id, child, usr) => new Entities::MarketingAutomation
                {
                    JSONDiagram = JsonDiagram
                };
            ShimAutomationBaseController.AllInstances.ValidatePublishListOfControlBaseMarketingAutomationECNExceptionRef=
                (AutomationBaseController obj, List<ControlBase> shapes, Entities::MarketingAutomation ma,
                    ref ECNException ecnEx) =>
                { };
            ShimTransactionScope.ConstructorTransactionScopeOptionTimeSpan = (obj, option, time) =>
                throw new SecurityException();

            // Act	
            var actualResult = _controller.Publish(1) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.Count.ShouldBeGreaterThan(0);
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldContain(ErrorString);
                });
        }

        [Test]
        public void Publish_OnTransactionScopeException_ReturnErrorResponse()
        {
            // Arrange
            const string JsonDiagram = "{shapes: [{category:16}, {category:5}], connections: []}";
            const string ErrorString = "An error occurred";
            SetShimForCurrentUser();
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (id, child, usr) => new Entities::MarketingAutomation
                {
                    JSONDiagram = JsonDiagram
                };
            ShimAutomationBaseController.AllInstances.ValidatePublishListOfControlBaseMarketingAutomationECNExceptionRef=
                (AutomationBaseController obj, List<ControlBase> shapes, Entities::MarketingAutomation ma,
                    ref ECNException ecnEx) =>
                { };
            ShimTransactionScope.ConstructorTransactionScopeOptionTimeSpan = (obj, option, time) =>
                throw new Exception();
            _appSettings.Add("KMCommon_Application", "1");
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (exc, method, id, note, cId, cusId) => 1;

            // Act	
            var actualResult = _controller.Publish(1) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.Count.ShouldBeGreaterThan(0);
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldContain(ErrorString);
                });
        }

        [Test]
        public void Publish_OnValidCal_ReturnSuccessResponse()
        {
            // Arrange
            var jsonDiagram = new StringBuilder();
            jsonDiagram.Append("{");
            jsonDiagram.Append($"shapes: [{{category:16, controlID: \"{DummyString}\"}}, {{category:5}}]");
            jsonDiagram.Append($", connections: []");
            jsonDiagram.Append("}");
            SetShimsForPublish(jsonDiagram.ToString());

            // Act	
            var actualResult = _controller.Publish(1) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.Count.ShouldBeGreaterThan(0);
                    jsonList[0].ShouldBe(StatusOKCode);
                });
        }

        private void SetShimsForPublish(string jsonDiagram)
        {
            SetShimForCurrentUser();
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (id, child, usr) => new Entities::MarketingAutomation
                {
                    JSONDiagram = jsonDiagram,
                    Controls = new List<Entities::MAControl>
                    {
                        new Entities::MAControl
                        {
                            ControlID = DummyString
                        }
                    },
                    MarketingAutomationID = 1,
                    Connectors = new List<Entities.MAConnector>
                    {
                        new Entities::MAConnector
                        {
                            From = DummyString
                        }
                    }
                };
            ShimAutomationBaseController.AllInstances.ValidatePublishListOfControlBaseMarketingAutomationECNExceptionRef =
                (AutomationBaseController obj, List<ControlBase> shapes, Entities::MarketingAutomation ma,
                    ref ECNException ecnEx) =>
                { };
            ShimTransactionScope.ConstructorTransactionScopeOptionTimeSpan = (obj, option, time) => { };
            ShimTransactionScope.AllInstances.Complete = (obj) => { };
            ShimTransactionScope.AllInstances.Dispose = (obj) => { };
            ShimMAControl.SaveMAControl = (ma) => 1;
            ShimMAControl.DeleteInt32 = (id) => { };
            ShimAutomationBaseController.AllInstances.ValidateDeleteMAControl = (obj, ma) => { };
            ShimHomeController.AllInstances.SaveChildrenControlBaseListOfControlBaseInt32 = (obj, start, child, id) => { };
            ShimHomeController.AllInstances.DeleteECNObjectMAControl = (obj, ma) => { };
            ShimMAConnector.DeleteInt32 = (id) => { };
            ShimMarketingAutomation.SaveMarketingAutomationUser = (ma, usr) => 1;
            ShimMarketingAutomationHistory.InsertInt32Int32String = (id, usr, action) => { };
            ShimMAControl.GetByMarketingAutomationIDInt32 = (id) => null;
            ShimMAConnector.GetByMarketingAutomationIDInt32 = (id) => null;
            ShimConnector.GetPostModelFromObjectListOfMAConnector = (list) => null;
            ShimControlBase.GetModelsFromObjectListOfMAControlListOfMAConnectorListOfControlBaseUser =
                (ctrls, conns, shapes, usr) => null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Web;
using System.IO;
using System.Web.Fakes;
using System.Web.Mvc;
using System.Web.Mvc.Fakes;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Controllers;
using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Fakes;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using KM.Common.Entity.Fakes;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ecn.MarketingAutomation.Tests.Controllers
{
    public partial class DiagramsControllerTest
    {
        private const string HomeUrl = "Home//Index";
        private const string SampleUTExceptionMessage = "UT Exception";
        private const string KMCommonApplicationConfigKey = "KMCommon_Application";
        private const string SampleHostUrl = "http://km.com";

        private bool _isPublishControlsValidated = false;
        private bool _isAutomationHistoryInserted = false;
        private bool _isConnecterDeleted = false;
        private bool _isECNObjectDeleted = false;

        [Test]
        public void Save_WhenEmptyJsonDiagram_ResturnsErrorJsonResponse()
        {
            // Arrange
            var postModel = new MarketingAutomationPostModel
            {
                State = MarketingAutomationStatus.Published
            };

            // Act
            var actionResult = _diagramsController.Save(postModel);

            // Assert
            actionResult.ShouldNotBeNull();
            actionResult.ShouldSatisfyAllConditions(
                () => actionResult.ShouldBeOfType(typeof(JsonResult)),
                () => 
                {
                    var jsonResult = (JsonResult)actionResult;
                    jsonResult.Data.ShouldNotBeNull();
                    jsonResult.Data.ShouldBeOfType(typeof(List<string>));
                    var data = (List<string>)jsonResult.Data;
                    data.ShouldSatisfyAllConditions(
                        () => data.Count.ShouldBe(2),
                        () => data.ShouldContain("500"),
                        () => data.ShouldContain("Could not save automation"));
                });
        }

        [Test]
        public void Save_WhenJsonDiagram_SavesDiagramSuccessFully()
        {
            // Arrange
            SetUpSaveMethodFakes();
            var connectors = GetConnectorList();
            var postModel = GetMarketingAutomationPostModel();
            postModel.JSONDiagram = ControlBase.Serialize(
                postModel.Controls,
                connectors,
                MAID: 1);

            // Act
            var actionResult = _diagramsController.Save(postModel);

            // Assert
            actionResult.ShouldNotBeNull();
            actionResult.ShouldSatisfyAllConditions(
                () => actionResult.ShouldBeOfType(typeof(JsonResult)),
                () =>
                {
                    var jsonResult = (JsonResult)actionResult;
                    jsonResult.Data.ShouldNotBeNull();
                    jsonResult.Data.ShouldBeOfType(typeof(List<string>));
                    var data = (List<string>)jsonResult.Data;
                    data.ShouldSatisfyAllConditions(
                        () => data.Count.ShouldBe(3),
                        () => data.ShouldContain("200"),
                        () => data.ShouldContain("Diagram saved successfully"));
                });
        }

        [Test]
        public void Save_WhenJsonDiagramAndStateSaved_SavesDiagramSuccessFully()
        {
            // Arrange
            SetUpSaveMethodFakes();
            var connectors = GetConnectorList();
            var postModel = GetMarketingAutomationPostModel();
            postModel.State = MarketingAutomationStatus.Saved;
            postModel.JSONDiagram = ControlBase.Serialize(
                postModel.Controls,
                connectors,
                MAID: 1);

            // Act
            var actionResult = _diagramsController.Save(postModel);

            // Assert
            actionResult.ShouldNotBeNull();
            actionResult.ShouldSatisfyAllConditions(
                () => actionResult.ShouldBeOfType(typeof(JsonResult)),
                () =>
                {
                    var jsonResult = (JsonResult)actionResult;
                    jsonResult.Data.ShouldNotBeNull();
                    jsonResult.Data.ShouldBeOfType(typeof(List<string>));
                    var data = (List<string>)jsonResult.Data;
                    data.ShouldSatisfyAllConditions(
                        () => data.Count.ShouldBe(3),
                        () => data.ShouldContain("200"),
                        () => data.ShouldContain(postModel.JSONDiagram),
                        () => data.ShouldContain("Diagram saved successfully"));
                });
        }

        [Test]
        public void Save_WhenJsonDiagramDoesNotHaveStartOrEndControl_ReturnsErrorResponse()
        {
            // Arrange
            SetUpSaveMethodFakes();
            var connectors = GetConnectorList();
            var postModel = GetMarketingAutomationPostModel();
            postModel.Controls.RemoveAt(2);
            postModel.Controls.RemoveAt(1);
            postModel.JSONDiagram = ControlBase.Serialize(
                postModel.Controls,
                connectors,
                MAID: 1);

            // Act
            var actionResult = _diagramsController.Save(postModel);

            // Assert
            actionResult.ShouldNotBeNull();
            actionResult.ShouldSatisfyAllConditions(
                () => actionResult.ShouldBeOfType(typeof(JsonResult)),
                () =>
                {
                    var jsonResult = (JsonResult)actionResult;
                    jsonResult.Data.ShouldNotBeNull();
                    jsonResult.Data.ShouldBeOfType(typeof(List<string>));
                    var data = (List<string>)jsonResult.Data;
                    data.ShouldSatisfyAllConditions(
                        () => data.Count.ShouldBe(2),
                        () => data.ShouldContain("500"),
                        () => data.ShouldContain("Automation is missing start or end controls"));
                });
        }

        [Test]
        public void Save_ThrowsSecurityException_ReturnsErrorResponse()
        {
            // Arrange
            SetUpSaveMethodFakes();
            var connectors = GetConnectorList();
            var postModel = GetMarketingAutomationPostModel();
            postModel.JSONDiagram = ControlBase.Serialize(
                postModel.Controls,
                connectors,
                MAID: 1);
            ShimMarketingAutomation.SaveMarketingAutomationUser = (m, u) => throw new SecurityException(SampleUTExceptionMessage);

            // Act
            var actionResult = _diagramsController.Save(postModel);

            // Assert
            actionResult.ShouldNotBeNull();
            actionResult.ShouldSatisfyAllConditions(
                () => actionResult.ShouldBeOfType(typeof(JsonResult)),
                () =>
                {
                    var jsonResult = (JsonResult)actionResult;
                    jsonResult.Data.ShouldNotBeNull();
                    jsonResult.Data.ShouldBeOfType(typeof(List<string>));
                    var data = (List<string>)jsonResult.Data;
                    data.ShouldSatisfyAllConditions(
                        () => data.Count.ShouldBe(2),
                        () => data.ShouldContain("302"),
                        () => data.ShouldContain(HomeUrl));
                });
        }

        [Test]
        public void Save_ThrowsECNException_ReturnsErrorResponse()
        {
            // Arrange
            SetUpSaveMethodFakes();
            var connectors = GetConnectorList();
            var postModel = GetMarketingAutomationPostModel();
            postModel.JSONDiagram = ControlBase.Serialize(
                postModel.Controls,
                connectors,
                MAID: 1);
            ShimMarketingAutomation.SaveMarketingAutomationUser =
                (m, u) => throw new ECNException(
                    new List<ECNError>
                    {
                        new ECNError { ErrorMessage = SampleUTExceptionMessage,  Method = Enums.Method.Save }
                    });

            // Act
            var actionResult = _diagramsController.Save(postModel);

            // Assert
            actionResult.ShouldNotBeNull();
            actionResult.ShouldSatisfyAllConditions(
                () => actionResult.ShouldBeOfType(typeof(JsonResult)),
                () =>
                {
                    var jsonResult = (JsonResult)actionResult;
                    jsonResult.Data.ShouldNotBeNull();
                    jsonResult.Data.ShouldBeOfType(typeof(List<string>));
                    var data = (List<string>)jsonResult.Data;
                    data.ShouldSatisfyAllConditions(
                        () => data.Count.ShouldBe(3),
                        () => data.ShouldContain("500"),
                        () => data.ShouldContain(x => x.Contains(SampleUTExceptionMessage)));
                });
        }

        [Test]
        public void Save_ThrowsGeneralException_ReturnsErrorResponse()
        {
            // Arrange
            SetUpSaveMethodFakes();
            var connectors = GetConnectorList();
            var postModel = GetMarketingAutomationPostModel();
            postModel.JSONDiagram = ControlBase.Serialize(
                postModel.Controls,
                connectors,
                MAID: 1);
            ShimMarketingAutomation.SaveMarketingAutomationUser =
                (m, u) => throw new InvalidOperationException(SampleUTExceptionMessage);

            // Act
            var actionResult = _diagramsController.Save(postModel);

            // Assert
            actionResult.ShouldNotBeNull();
            actionResult.ShouldSatisfyAllConditions(
                () => actionResult.ShouldBeOfType(typeof(JsonResult)),
                () =>
                {
                    var jsonResult = (JsonResult)actionResult;
                    jsonResult.Data.ShouldNotBeNull();
                    jsonResult.Data.ShouldBeOfType(typeof(List<string>));
                    var data = (List<string>)jsonResult.Data;
                    data.ShouldSatisfyAllConditions(
                        () => data.Count.ShouldBe(3),
                        () => data.ShouldContain("500"),
                        () => data.ShouldContain("An error occurred"));
                });
        }

        [Test]
        public void Save_WhenStateSavedAndThrowsSecurityException_ReturnsErrorResponse()
        {
            // Arrange
            SetUpSaveMethodFakes();
            var connectors = GetConnectorList();
            var postModel = GetMarketingAutomationPostModel();
            postModel.State = MarketingAutomationStatus.Saved;
            postModel.JSONDiagram = ControlBase.Serialize(
                postModel.Controls,
                connectors,
                MAID: 1);
            ShimMarketingAutomation.SaveMarketingAutomationUser = (m, u) => throw new SecurityException(SampleUTExceptionMessage);

            // Act
            var actionResult = _diagramsController.Save(postModel);

            // Assert
            actionResult.ShouldNotBeNull();
            actionResult.ShouldSatisfyAllConditions(
                () => actionResult.ShouldBeOfType(typeof(JsonResult)),
                () =>
                {
                    var jsonResult = (JsonResult)actionResult;
                    jsonResult.Data.ShouldNotBeNull();
                    jsonResult.Data.ShouldBeOfType(typeof(List<string>));
                    var data = (List<string>)jsonResult.Data;
                    data.ShouldSatisfyAllConditions(
                        () => data.Count.ShouldBe(2),
                        () => data.ShouldContain("302"),
                        () => data.ShouldContain(HomeUrl));
                });
        }

        [Test]
        public void Save_WhenStateSavedAndThrowsGeneralException_ReturnsErrorResponse()
        {
            // Arrange
            SetUpSaveMethodFakes();
            var connectors = GetConnectorList();
            var postModel = GetMarketingAutomationPostModel();
            postModel.State = MarketingAutomationStatus.Saved;
            postModel.JSONDiagram = ControlBase.Serialize(
                postModel.Controls,
                connectors,
                MAID: 1);
            ShimMarketingAutomation.SaveMarketingAutomationUser = (m, u) => throw new InvalidOperationException(SampleUTExceptionMessage);

            // Act
            var actionResult = _diagramsController.Save(postModel);

            // Assert
            actionResult.ShouldNotBeNull();
            actionResult.ShouldSatisfyAllConditions(
                () => actionResult.ShouldBeOfType(typeof(JsonResult)),
                () =>
                {
                    var jsonResult = (JsonResult)actionResult;
                    jsonResult.Data.ShouldNotBeNull();
                    jsonResult.Data.ShouldBeOfType(typeof(List<string>));
                    var data = (List<string>)jsonResult.Data;
                    data.ShouldSatisfyAllConditions(
                        () => data.Count.ShouldBe(2),
                        () => data.ShouldContain("500"),
                        () => data.ShouldContain("Could not save automation"));
                });
        }

        [Test]
        public void Save_WhenStatePublishAndThrowsOuterSecurityException_ReturnsErrorResponse()
        {
            // Arrange
            SetUpSaveMethodFakes();
            var connectors = GetConnectorList();
            var postModel = GetMarketingAutomationPostModel();
            postModel.JSONDiagram = ControlBase.Serialize(
                postModel.Controls,
                connectors,
                MAID: 1);
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = 
                (m, u,i) => throw new SecurityException(SampleUTExceptionMessage);

            // Act
            var actionResult = _diagramsController.Save(postModel);

            // Assert
            actionResult.ShouldNotBeNull();
            actionResult.ShouldSatisfyAllConditions(
                () => actionResult.ShouldBeOfType(typeof(JsonResult)),
                () =>
                {
                    var jsonResult = (JsonResult)actionResult;
                    jsonResult.Data.ShouldNotBeNull();
                    jsonResult.Data.ShouldBeOfType(typeof(List<string>));
                    var data = (List<string>)jsonResult.Data;
                    data.ShouldSatisfyAllConditions(
                        () => data.Count.ShouldBe(2),
                        () => data.ShouldContain("302"),
                        () => data.ShouldContain(HomeUrl));
                });
        }

        [Test]
        public void Save_WhenStatePublishAndMarketingAutomationHasControls_ReturnsErrorResponse()
        {
            // Arrange
            SetUpSaveMethodFakes();
            var connectors = GetConnectorList();
            var postModel = GetMarketingAutomationPostModel();
            postModel.JSONDiagram = ControlBase.Serialize(
                postModel.Controls,
                connectors,
                MAID: 1);
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser =
                (m, u, i) => new CommunicatorEntities.MarketingAutomation
                {
                    Controls = new List<CommunicatorEntities.MAControl>
                    {
                        new CommunicatorEntities.MAControl { ControlID = "99" }
                    },
                    Connectors = new List<CommunicatorEntities.MAConnector>
                    {
                        new CommunicatorEntities.MAConnector
                        {
                            From = "99"
                        }
                    }
                };
            
            // Act
            var actionResult = _diagramsController.Save(postModel);

            // Assert
            actionResult.ShouldNotBeNull();
            actionResult.ShouldSatisfyAllConditions(
                () => actionResult.ShouldBeOfType(typeof(JsonResult)),
                () =>
                {
                    var jsonResult = (JsonResult)actionResult;
                    jsonResult.Data.ShouldNotBeNull();
                    jsonResult.Data.ShouldBeOfType(typeof(List<string>));
                    var data = (List<string>)jsonResult.Data;
                    data.ShouldSatisfyAllConditions(
                        () => data.Count.ShouldBe(3),
                        () => data.ShouldContain("200"),
                        () => data.ShouldContain("Diagram saved successfully"));
                });
        }

        private void SetUpSaveMethodFakes()
        {
            SetWebControllerFakes();
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = 
                (id, b, u) => new CommunicatorEntities.MarketingAutomation { };

            ShimMAControl.GetByMarketingAutomationIDInt32 = (maId) => GetMAControlList();
            ShimMAConnector.GetByMarketingAutomationIDInt32 = (maId) => GetMAConnectorList();

            ShimControlBase.GetModelsFromObjectListOfMAControlListOfMAConnectorListOfControlBaseUser =
                (scontrols, sconnectors, shapes, user) => new List<ControlBase>
                {
                    new CampaignItem { ControlID = "1" }
                };

            ShimAutomationBaseController.AllInstances.ValidatePublishListOfControlBaseMarketingAutomationECNExceptionRef =
                (AutomationBaseController dc ,List<ControlBase> c, CommunicatorEntities.MarketingAutomation ma, ref ECNException exp) => 
                {
                    _isPublishControlsValidated = true;
                };
            ShimAutomationBaseController.AllInstances.ValidateDeleteMAControl = (dc, ma) => { _isPublishControlsValidated = true; };
            ShimDiagramsController.AllInstances.DeleteECNObjectMAControl = (dc, ma) => { _isECNObjectDeleted = true; };
            
            ShimMAControl.SaveMAControl = (maControl) => { return maControl.MarketingAutomationID; };
            ShimMarketingAutomation.SaveMarketingAutomationUser = (ma, user) => { return ma.MarketingAutomationID; };
            ShimMarketingAutomationHistory.InsertInt32Int32String = (maId, uid, action) => { _isAutomationHistoryInserted = true; };
            ShimMAConnector.DeleteInt32 = (connID) => { _isConnecterDeleted = true; };
            ShimMAControl.DeleteInt32 = (controlID) => { _isConnecterDeleted = true; };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (q, a, r, y, u, f) => 1;
        }

        private void SetWebControllerFakes()
        {
            var settings = new NameValueCollection();
            settings.Add(KMCommonApplicationConfigKey, "1");
            ShimConfigurationManager.AppSettingsGet = () => settings;

            var context = new HttpContext(new HttpRequest(string.Empty, SampleHostUrl, string.Empty),
                            new HttpResponse(TextWriter.Null));

            ShimHttpContext.CurrentGet = () => { return context; };

            ShimController.AllInstances.RequestGet = (c) =>
            {
                return new HttpRequestWrapper(new HttpRequest("test", SampleHostUrl, ""));
            };

            ShimUrlHelper.AllInstances.ActionStringStringObject = 
                (uh, action, controller, routeValues) => String.Join("//", controller, action);
        }

        private MarketingAutomationPostModel GetMarketingAutomationPostModel()
        {
          return  new MarketingAutomationPostModel
            {
                State = MarketingAutomationStatus.Published,
                Start = new Start { },
                Controls = new List<ControlBase>
                {
                    new CampaignItem { ControlID  = "2" },
                     new Start { ControlID  = "1" },
                    new End { ControlID  = "1" }
                },
            };
        }

        private List<Connector> GetConnectorList()
        {
           return new List<Connector>
            {
                new Connector
                {
                    from = new from { shapeId = "1"},
                    to = new to { shapeId = "2"}
                }
            };
        }

        private List<CommunicatorEntities.MAConnector> GetMAConnectorList()
        {
            return new List<CommunicatorEntities.MAConnector>
            {
                new CommunicatorEntities.MAConnector
                {
                    ConnectorID = 1,
                    ControlID = "1",
                    MarketingAutomationID = 1
                }
            };
        }

        private List<CommunicatorEntities.MAControl> GetMAControlList()
        {
            return new List<CommunicatorEntities.MAControl>
            {
                new CommunicatorEntities.MAControl
                {
                    ControlID = "1",
                    MarketingAutomationID = 1,
                    MAControlID = 1
                }
            };
        }
    }
}

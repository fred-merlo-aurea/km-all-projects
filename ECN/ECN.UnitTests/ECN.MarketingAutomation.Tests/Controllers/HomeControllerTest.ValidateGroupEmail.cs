using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using NUnit.Framework;
using Entities = ECN_Framework_Entities.Communicator;
using Shouldly;
using KMPlatform.Entity;
using ecn.MarketingAutomation.Models.PostModels.ECN_Objects;
using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Controls.Fakes;
using ecn.MarketingAutomation.Models.PostModels.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using KM.Common.Entity.Fakes;
using ECN_Framework_BusinessLayer.FormDesigner.Fakes;
using FormEntities = ECN_Framework_Entities.FormDesigner;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Interfaces.Fakes;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.MarketingAutomation.Tests.Controllers
{
    public partial class HomeControllerTest
    {
        private static readonly object[] _controlTypesForValidateGroupEmail =
        {
            new Click { ECNID = 1, CampaignItemTemplateID = 1, ControlID = DummyString},
            new NoClick { ECNID = 1, CampaignItemTemplateID = 1, ControlID = DummyString},
            new NoOpen { ECNID = 1, CampaignItemTemplateID = 1, ControlID = DummyString},
            new NotSent { ECNID = 1, CampaignItemTemplateID = 1, ControlID = DummyString},
            new Open { ECNID = 1, CampaignItemTemplateID = 1, ControlID = DummyString},
            new Open_NoClick { ECNID = 1, CampaignItemTemplateID = 1, ControlID = DummyString},
            new Sent { ECNID = 1, CampaignItemTemplateID = 1, ControlID = DummyString},
            new Suppressed { ECNID = 1, CampaignItemTemplateID = 1, ControlID = DummyString}
        };

        [Test]
        public void ValidateGroupEmail_OnECNException_ReturnErrorResponse()
        {
            // Arrange
            SetupForValidatePublish();
            var automation = new MarketingAutomationPostModel
            {
                MarketingAutomationID = DummyId
            };
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (_, __, ___) =>
                throw new ECNException(DummyString, new List<ECNError>
                {
                    new ECNError
                    {
                        ErrorMessage = DummyString
                    }
                });

            // Act	
            var actualResult = _controller.ValidateGroupEmail(
                automation,
                GetWaitObject(DummyId, false, DummyId),
                DummyString) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.Count.ShouldBeGreaterThan(CountOfZeroItems);
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldContain(DummyString);
                });
        }

        [Test]
        public void ValidateGroupEmail_OnException_ReturnErrorResponse()
        {
            // Arrange
            SetupForValidatePublish();
            var automation = new MarketingAutomationPostModel
            {
                MarketingAutomationID = DummyId
            };
            const string ExpectedError = "An error occurred";
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (_, __, ___) =>
                throw new Exception();

            // Act	
            var actualResult = _controller.ValidateGroupEmail(
                automation,
                GetWaitObject(DummyId, false, DummyId),
                DummyString) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.Count.ShouldBeGreaterThan(CountOfZeroItems);
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldContain(ExpectedError);
                });
        }

        [Test]
        [TestCaseSource(nameof(_controlTypesForValidateGroupEmail))]
        public void ValidateGroupEmail_MultipleCases_Validate(ControlBase control)
        {
            // Arrange
            SetupForValidatePublish();
            var automation = new MarketingAutomationPostModel
            {
                MarketingAutomationID = DummyId,
                Connectors = GetAllConnectorsForValidateGroupEmail()
            };
            SetShimsForValidateGroupEmail(control);

            // Act	
            var actualResult = _controller.ValidateGroupEmail(
                automation,
                GetWaitObject(DummyId, false, DummyId),
                DummyString) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList.Count.ShouldBeGreaterThan(CountOfZeroItems);
                    jsonList[0].ShouldBe(StatusOKCode);
                    _itemIsValid.ShouldBeTrue();
                });
        }

        private void SetShimsForValidateGroupEmail(ControlBase control)
        {
            _itemIsValid = false;
            ShimMarketingAutomationPostModel.AllInstances.GetAllControls = (_) => new List<ControlBase> {control};
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (_, __, ___) =>
                new Entities::MarketingAutomation();
            Action validateAction = () => { _itemIsValid = true; };
            ShimClick.AllInstances.ValidateWaitCampaignItemMarketingAutomationUser =
                (_, __, ___, ____, _____) => { validateAction(); };
            ShimNoClick.AllInstances.ValidateWaitCampaignItemMarketingAutomationUser =
                (_, __, ___, ____, _____) => { validateAction(); };
            ShimNoOpen.AllInstances.ValidateWaitCampaignItemMarketingAutomationUser =
                (_, __, ___, ____, _____) => { validateAction(); };
            ShimNotSent.AllInstances.ValidateWaitCampaignItemMarketingAutomationUser =
                (_, __, ___, ____, _____) => { validateAction(); };
            ShimOpen.AllInstances.ValidateWaitCampaignItemMarketingAutomationUser =
                (_, __, ___, ____, _____) => { validateAction(); };
            ShimOpen_NoClick.AllInstances.ValidateWaitCampaignItemMarketingAutomationUser =
                (_, __, ___, ____, _____) => { validateAction(); };
            ShimSent.AllInstances.ValidateWaitCampaignItemMarketingAutomationUser =
                (_, __, ___, ____, _____) => { validateAction(); };
            ShimSuppressed.AllInstances.ValidateWaitCampaignItemMarketingAutomationUser =
                (_, __, ___, ____, _____) => { validateAction(); };
        }

        private List<Connector> GetAllConnectorsForValidateGroupEmail()
        {
            return new List<Connector>
            {
                new Connector
                {
                    to = new to
                    {
                        shapeId = DummyString
                    },
                    @from = new @from
                    {
                        shapeId = DummyString
                    }
                }
            };
        }
    }
}

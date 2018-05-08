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
        private bool _itemIsValid;
        private static readonly object[] _controlTypesForValidateDirectEmail =
        {
            new FormAbandon { ECNID = 1, CampaignItemTemplateID = 1, ControlID = DummyString},
            new FormSubmit { ECNID = 1, CampaignItemTemplateID = 1, ControlID = DummyString},
            new Direct_Click { ECNID = 1, CampaignItemTemplateID = 1, ControlID = DummyString},
            new Direct_Open { ECNID = 1, CampaignItemTemplateID = 1, ControlID = DummyString},
            new Direct_NoOpen { ECNID = 1, CampaignItemTemplateID = 1, ControlID = DummyString},
        };

        [Test]
        public void ValidateDirectEmail_OnECNException_ReturnErrorResponse()
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
            var actualResult = _controller.ValidateDirectEmail(
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
        public void ValidateDirectEmail_OnException_ReturnErrorResponse()
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
            var actualResult = _controller.ValidateDirectEmail(
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
        [TestCaseSource(nameof(_controlTypesForValidateDirectEmail))]
        public void ValidateDirectEmail_MultipleCases_Validate(ControlBase control)
        {
            // Arrange
            SetupForValidatePublish();
            var automation = new MarketingAutomationPostModel
            {
                MarketingAutomationID = DummyId,
                Connectors = GetAllConnectorsForValidateDirectEmail()
            };
            SetShimsForValidateDirectEmail(control);

            // Act	
            var actualResult = _controller.ValidateDirectEmail(
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

        private void SetShimsForValidateDirectEmail(ControlBase control)
        {
            _itemIsValid = false;
            ShimMarketingAutomationPostModel.AllInstances.GetAllControls = (_) => new List<ControlBase> {control};
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (_, __, ___) =>
                new Entities::MarketingAutomation();
            Action validateAction = () => { _itemIsValid = true; };
            ShimDirect_Click.AllInstances.ValidateWaitBooleanCampaignItemGroupControlBaseUser =
                (_, __, ___, ____, _____, ______, _______) => { validateAction(); };
            ShimDirect_NoOpen.AllInstances.ValidateWaitBooleanCampaignItemGroupMarketingAutomationUser =
                (_, __, ___, ____, _____, ______, _______) => { validateAction(); };
            ShimDirect_Open.AllInstances.ValidateWaitBooleanCampaignItemGroupMarketingAutomationUser =
                (_, __, ___, ____, _____, ______, _______) => { validateAction(); };
            ShimFormAbandon.AllInstances.ValidateWaitBooleanCampaignItemGroupControlBaseUser =
                (_, __, ___, ____, _____, ______, _______) => { validateAction(); };
            ShimFormSubmit.AllInstances.ValidateWaitBooleanCampaignItemGroupControlBaseUser =
                (_, __, ___, ____, _____, ______, _______) => { validateAction(); };
        }

        private List<Connector> GetAllConnectorsForValidateDirectEmail()
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

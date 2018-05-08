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
using ecn.MarketingAutomation.Models;
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
using KMSite;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.MarketingAutomation.Tests.Controllers
{
    public partial class HomeControllerTest
    {
        private const string MethodProcessCreate = "ProcessCreate";

        [Test]
        public void ProcessCreate_OnModelIsInvalid_ReturnPartialView()
        {
            // Arrange
            SetupForValidatePublish();
            var diagramPostModel = new DiagramPostModel
            {
                IsCopy = true
            };
            const string expectedViewName = "Partials/_Automation";
            _controller.ModelState.AddModelError(DummyString, DummyString);

            // Act	
            var actualResult = _privateObject.Invoke(MethodProcessCreate, diagramPostModel, false) as PartialViewResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ViewName.ShouldContain(expectedViewName)
               );
        }

        [Test]
        public void ProcessCreate_OnValidDate_ReturnPartialView()
        {
            // Arrange
            SetupForValidatePublish();
            var diagramPostModel = new DiagramPostModel
            {
                IsCreate = false,
                IsCopy = false
            };
            const string expectedViewName = "Partials/_Automation";
            _controller.ModelState.AddModelError(nameof(DiagramPostModel.StartFrom), DummyString);
            _controller.ModelState.AddModelError(nameof(DiagramPostModel.StartTo), DummyString);
            _controller.ModelState.AddModelError(nameof(DiagramPostModel.Goal), DummyString);
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (_, __, ___) =>
                new Entities::MarketingAutomation
                {
                    State = MarketingAutomationStatus.Archived,
                    EndDate = DateTime.Now,
                    StartDate = DateTime.Now.AddMonths(-1)
                };

            // Act	
            var actualResult = _privateObject.Invoke(MethodProcessCreate, diagramPostModel, false) as PartialViewResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ViewName.ShouldContain(expectedViewName)
               );
        }

        [Test]
        [TestCase(0, false, DiagramFromTemplate.No)]
        [TestCase(0, false, DiagramFromTemplate.Yes)]
        [TestCase(1, false, DiagramFromTemplate.Yes)]
        [TestCase(1, true, DiagramFromTemplate.Yes)]
        public void ProcessCreate_OnModelStateIsValid_ReturnPartialView(
            int modelId, 
            bool isCreate, 
            DiagramFromTemplate fromTemplate)
        {
            // Arrange
            var jsonDiagram = GetJsonDiagram();
            var diagramPostModel = new DiagramPostModel
            {
                IsCreate = isCreate,
                IsCopy = isCreate,
                StartTo = DateTime.Now,
                Id = modelId,
                FromTemplate = fromTemplate
            };
            const string expectedViewName = "Partials/_Automation";
            SetupForProcessCreate(false, jsonDiagram.ToString());

            // Act	
            var actualResult = _privateObject.Invoke(MethodProcessCreate, diagramPostModel, false) as PartialViewResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ViewName.ShouldContain(expectedViewName)
               );
        }

        [Test]
        [TestCase(0, false, "Index")]
        [TestCase(1, false, "Index")]
        [TestCase(1, true, "Edit")]
        public void ProcessCreate_OnModelStateIsValidAndNameIsUnique_ReturnJavaScriptRedirectResult(int modelId, bool isCreate, string expectedViewName)
        {
            // Arrange
            var jsonDiagram = GetJsonDiagram();
            var diagramPostModel = new DiagramPostModel
            {
                IsCreate = isCreate,
                IsCopy = isCreate,
                StartTo = DateTime.Now,
                Id = modelId
            };
            SetupForProcessCreate(true, jsonDiagram.ToString());

            // Act	
            var actualResult = _privateObject.Invoke(MethodProcessCreate, diagramPostModel, false) as JavaScriptRedirectResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ActionName.ShouldContain(expectedViewName)
            );
        }

        [Test]
        public void ProcessCreate_OnECNException_ReturnPartialViewResult()
        {
            // Arrange
            var jsonDiagram = GetJsonDiagram();
            var diagramPostModel = new DiagramPostModel
            {
                IsCreate = false,
                IsCopy = false
            };
            const string expectedViewName = "Partials/_Automation";
            SetupForProcessCreate(false, jsonDiagram.ToString());
            ShimAutomationBaseController.AllInstances.ValidatePublishListOfControlBaseMarketingAutomationECNExceptionRef =
                (
                    AutomationBaseController obj,
                    List<ControlBase> controls,
                    Entities::MarketingAutomation ma,
                    ref ECNException ecnException) =>
                {
                    ecnException.ErrorList.Add(new ECNError
                    {
                        ErrorMessage = DummyString
                    });
                };

            // Act	
            var actualResult = _privateObject.Invoke(MethodProcessCreate, diagramPostModel, false) as PartialViewResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ViewName.ShouldContain(expectedViewName)
            );
        }

        [Test]
        public void ProcessCreate_OnException_ReturnPartialViewResult()
        {
            // Arrange
            var jsonDiagram = GetJsonDiagram();
            var diagramPostModel = new DiagramPostModel
            {
                IsCreate = false,
                IsCopy = false
            };
            _appSettings.Add("KMCommon_Application", DummyId.ToString());
            const string expectedViewName = "Partials/_Automation";
            SetupForProcessCreate(false, jsonDiagram.ToString());
            var errorLogged = false;
            ShimAutomationBaseController.AllInstances.ValidatePublishListOfControlBaseMarketingAutomationECNExceptionRef=
            (
                AutomationBaseController obj,
                List<ControlBase> controls,
                Entities::MarketingAutomation ma,
                ref ECNException ecnException) => throw new Exception();
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (_, __, ___, ____, _____, ______) =>
                {
                    errorLogged = true;
                    return DummyId;
                };

            // Act	
            var actualResult = _privateObject.Invoke(MethodProcessCreate, diagramPostModel, false) as PartialViewResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ViewName.ShouldContain(expectedViewName),
                () => errorLogged.ShouldBeTrue()
            );
        }

        public void ProcessCreate_OnPropSaveIsTrue_ReturnJavaScriptRedirectResult()
        {
            // Arrange
            var jsonDiagram = GetJsonDiagram();
            var diagramPostModel = new DiagramPostModel
            {
                IsCreate = true,
                IsCopy = true
            };
            SetupForProcessCreate(true, jsonDiagram.ToString());
            const string expectedViewName = "Index";

            // Act	
            var actualResult = _privateObject.Invoke(MethodProcessCreate, diagramPostModel, false) as JavaScriptRedirectResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ActionName.ShouldContain(expectedViewName)
            );
        }

        private void SetupForProcessCreate(bool nameIsUnique, string jsonDiagram)
        {
            SetupForValidatePublish();
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (_, __, ___) =>
                new Entities::MarketingAutomation
                {
                    State = MarketingAutomationStatus.Paused,
                    JSONDiagram = jsonDiagram
                };
            ShimMarketingAutomationHistory.InsertInt32Int32String = (id, usr, action) => { };
            ShimHomeController.AllInstances.CheckNameIsUniqueStringInt32 = (_, __, ___) => nameIsUnique;
            ShimAutomationBaseController.AllInstances.ValidatePublishListOfControlBaseMarketingAutomationECNExceptionRef=
                (
                    AutomationBaseController obj,
                    List<ControlBase> controls,
                    Entities::MarketingAutomation ma,
                    ref ECNException ecnEx) =>
                {
                };
            ShimHomeController.AllInstances.SaveDiagramPostModelBoolean = (_, __, ___) => DummyId;
        }

        private StringBuilder GetJsonDiagram()
        {
            var jsonDiagram = new StringBuilder();
            jsonDiagram.Append($"{{ shapes: [ {{ category:16, ControlID: \"{DummyString}\", controlType:16");
            jsonDiagram.Append($", to: {{ shapeId: \"{DummyString}\" }} }}]");
            jsonDiagram.Append($", connections: [ {{ category:16, from: {{ shapeId: \"{DummyString}\" }}");
            jsonDiagram.Append($", controlID: \"{DummyString}\" }}  ] }}");
            return jsonDiagram;
        }
    }
}

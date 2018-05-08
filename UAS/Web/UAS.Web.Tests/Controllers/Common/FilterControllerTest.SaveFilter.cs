using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using UAS.Web.Models.UAD.Filter;

namespace UAS.Web.Tests.Controllers.Common
{
    public partial class FilterControllerTest
    {
        private const string MessageFilterSaved = "Filter has been saved successfully.";
        private const string MessageFilterSaveFailed = "Filter save failed.";
        private const string PropertyError = "error";
        private const string PropertyErrorMessage = "errormessage";
        private const string MessageEnterValidFilterName = "Please enter a valid filter name.";
        private const string MessageFilterNameExists = "The filter Name you entered already exists. Please save under a different name.";
        private const string MessageSelectFilter = "Please select filter.";
        private const string MessageSelectQuestionCategory = "Please select Question Category.";
        private const string MessageQuestionNameExists = "The Question Name you entered already exists. Please save under a different question name.";
        private const string SaveModeAddNew = "AddNew";
        private const string SaveModeEdit = "Edit";
        private const string SaveModeDelete = "Delete";
        private const string SaveModeExisting = "Existing";
        private const string FilterName = "name1";
        private const string FilterIds = "1,2";

        [Test]
        [TestCase("")]
        [TestCase("  ")]
        [TestCase(null)]
        public void SaveFilter_AddWithInvalidName_ReturnsError(string invalidName)
        {
            // Arrange
            var model = new SaveFilterViewModel
            {
                Mode = SaveModeAddNew,
                FilterName = invalidName
            };

            // Act
            var result = _testEntity.SaveFilter(model) as JsonResult;

            // Assert
            VerifySaveFilterJsonResult(result, true, MessageEnterValidFilterName);
        }

        [Test]
        public void SaveFilter_AddNewWithExistingName_ReturnsError()
        {
            // Arrange
            var model = new SaveFilterViewModel
            {
                Mode = SaveModeAddNew,
                FilterName = FilterName
            };
            ShimUADFilter.AllInstances.ExistsByFilterNameClientConnectionsInt32String = (a, b, c, d) => true;

            // Act
            var result = _testEntity.SaveFilter(model) as JsonResult;

            // Assert
            VerifySaveFilterJsonResult(result, true, MessageFilterNameExists);
        }

        [Test]
        public void SaveFilter_DeleteWithDefaultFilterId_ReturnsError()
        {
            // Arrange
            var model = new SaveFilterViewModel
            {
                Mode = SaveModeDelete,
                FilterID = 0
            };

            // Act
            var result = _testEntity.SaveFilter(model) as JsonResult;

            // Assert
            VerifySaveFilterJsonResult(result, true, MessageSelectFilter);
        }

        [Test]
        public void SaveFilter_IsAddedToSalesViewAndQuestionCategoryZero_ReturnsError()
        {
            // Arrange
            var model = new SaveFilterViewModel
            {
                Mode = SaveModeDelete,
                IsAddedToSalesView = true,
                FilterID = 1,
                QuestionCategoryID = 0
            };

            // Act
            var result = _testEntity.SaveFilter(model) as JsonResult;

            // Assert
            VerifySaveFilterJsonResult(result, true, MessageSelectQuestionCategory);
        }

        [Test]
        public void SaveFilter_IsAddedToSalesViewAndQuestionNameExists_ReturnsError()
        {
            // Arrange
            var model = new SaveFilterViewModel
            {
                Mode = SaveModeDelete,
                IsAddedToSalesView = true,
                FilterID = 1,
                QuestionCategoryID = 1
            };
            ShimUADFilter.AllInstances.ExistsQuestionNameClientConnectionsInt32String = (a, b, c, d) => true;

            // Act
            var result = _testEntity.SaveFilter(model) as JsonResult;

            // Assert
            VerifySaveFilterJsonResult(result, true, MessageQuestionNameExists);
        }

        [Test]
        public void SaveFilter_AddNewAndRefreshDetails_InsertsFilterAndReturnsSuccess()
        {
            // Arrange
            var saved = false;
            var deletedDetail = false;
            var deletedGroup = false;
            var savedDetail = false;
            var savedGroup = false;
            var model = new SaveFilterViewModel
            {
                Mode = SaveModeAddNew,
                FilterName = FilterName,
                NewExisting = SaveModeExisting,
                FilterIDs = FilterIds
            };
            model.CurrentFilter.Fields.Add(new FilterDetails());
            ShimUADFilter.AllInstances.insertClientConnectionsUADFilter = (a, b, c) =>
            {
                saved = true;
                return SampleId;
            };
            ShimFilterGroup.AllInstances.getByFilterIDClientConnectionsInt32 = (a, b, c) => new List<FilterGroup>
            {
                new FilterGroup(),
                new FilterGroup()
            };
            ShimFilterGroup.AllInstances.SaveClientConnectionsInt32Int32 = (a, b, c, d) =>
            {
                savedGroup = true;
                return SampleId;
            };
            ShimFilterDetails.AllInstances.SaveClientConnectionsFilterDetails = (a, b, c) =>
            {
                savedDetail = true;
                return SampleId;
            };
            ShimFilterGroup.AllInstances.Delete_ByFilterIDClientConnectionsInt32 = (a, b, c) => deletedGroup = true;
            ShimFilterDetails.AllInstances.Delete_ByFilterIDClientConnectionsInt32 = (a, b, c) => deletedDetail = true;
            ShimFilterSchedule.AllInstances.ExistsByFilterIDClientConnectionsInt32 = (a, b, c) => false;
            ShimUADFilter.AllInstances.ExistsByFilterNameClientConnectionsInt32String = (a, b, c, d) => false;

            // Act
            var result = _testEntity.SaveFilter(model) as JsonResult;

            // Assert
            saved.ShouldSatisfyAllConditions(
                () => saved.ShouldBeTrue(),
                () => deletedDetail.ShouldBeTrue(),
                () => deletedGroup.ShouldBeTrue(),
                () => savedDetail.ShouldBeTrue(),
                () => savedGroup.ShouldBeTrue());
            VerifySaveFilterJsonResult(result, false, MessageFilterSaved);
        }

        [Test]
        public void SaveFilter_EditMode_SavesFilterAndReturnsSuccess()
        {
            // Arrange
            var saved = false;
            var model = new SaveFilterViewModel
            {
                Mode = SaveModeEdit,
                FilterName = FilterName,
                IsAddedToSalesView = true,
                FilterID = 1,
                QuestionCategoryID = 1,
                NewExisting = string.Empty
            };
            model.CurrentFilter.Fields.Add(new FilterDetails());
            ShimUADFilter.AllInstances.ExistsByFilterNameClientConnectionsInt32String = (a, b, c, d) => false;
            ShimUADFilter.AllInstances.ExistsQuestionNameClientConnectionsInt32String = (a, b, c, d) => false;
            ShimUADFilter.AllInstances.GetByIDClientConnectionsInt32 = (a, b, c) => new UADFilter();
            ShimUADFilter.AllInstances.insertClientConnectionsUADFilter = (a, b, c) =>
            {
                saved = true;
                return SampleId;
            };

            // Act
            var result = _testEntity.SaveFilter(model) as JsonResult;

            // Assert
            saved.ShouldBeTrue();
            VerifySaveFilterJsonResult(result, false, MessageFilterSaved);
        }

        private void VerifySaveFilterJsonResult(JsonResult result, bool error, string message)
        {
            result.ShouldNotBeNull();

            object jsonObject = result.Data;
            jsonObject.ShouldSatisfyAllConditions(
                () => jsonObject.ShouldNotBeNull(),
                () => VerifyProperty(jsonObject, PropertyError, error),
                () => VerifyProperty(jsonObject, PropertyErrorMessage, message));
        }

        private void VerifyProperty(object jsonObject, string name, object expected)
        {
            jsonObject.GetType()
                .GetProperty(name)
                .GetValue(jsonObject)
                .ShouldBe(expected);
        }
    }
}

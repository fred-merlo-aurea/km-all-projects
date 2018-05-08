using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using UAS.Web.Controllers.FileMapperWizard;
using UAS.Web.Controllers.FileMapperWizard.Fakes;

namespace UAS.Web.Tests.Controllers.FileMapperWizard
{
    /// <summary>
    ///     Unit Tests for <see cref="FileMapperWizardController.SaveDataMapTransformation"/>
    /// </summary>
    public partial class FileMapperWizardControllerTest
    {
        private TransformDataMap _expectedTransformationDataMap;

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void SaveDataMapTransformation_NullOrEmptyDataMappings_VerifyJSONResult(string dataMapping)
        {
            // Arrange, Act
            JsonResult result = _controller.SaveDataMapTransformation(TransformationId, false, dataMapping);
            Data data = GetDataMapData(result);

            // Assert
            data.ShouldSatisfyAllConditions(
                () => data.FieldMappingID.ShouldBe(0),
                () => data.IsComplete.ShouldBeFalse(),
                () => data.Message.ShouldBe("<li>No conditions were added.</li>"),
                () => data.SourceFileID.ShouldBe("0"));

            result.JsonRequestBehavior.ShouldBe(JsonRequestBehavior.AllowGet);
        }

        [Test]
        public void SaveDataMapTransformation_DataMapMappings_VerifySaveNewDataMapMapping()
        {
            // Arrange
            SetupSaveDataMapFakes();
            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append(
                "{\"TransformDataMapID\":2,\"RowID\":3,\"PubID\":\"4\",\"MatchType\":\"2\",\"SourceData\":\"source-data\",\"DesiredData\":\"desired-data\"}");
            builder.Append("]");

            var dataMapId = 2;
            _deleteId = 2;
            _expectedTransformationDataMap = new TransformDataMap
            {
                TransformDataMapID = dataMapId,
                TransformationID = TransformationId,
                MatchType = "2",
                SourceData = "source-data",
                DesiredData = "desired-data",
                IsActive = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                CreatedByUserID = UserId,
                UpdatedByUserID = UserId,
                PubID = 4
            };

            _dataMapTransforms.Add(_expectedTransformationDataMap);

            _pubIds = new List<string> { "4" };

            // Act
            JsonResult result = _controller.SaveDataMapTransformation(TransformationId, true, builder.ToString());
            Data data = GetDataMapData(result);

            // Assert
            data.ShouldSatisfyAllConditions(
                () => data.FieldMappingID.ShouldBe(FieldMappingId),
                () => data.IsComplete.ShouldBeTrue(),
                () => data.Message.ShouldBeEmpty(),
                () => data.SourceFileID.ShouldBe(SourceFileId.ToString()));

            result.JsonRequestBehavior.ShouldBe(JsonRequestBehavior.AllowGet);
        }

        [Test]
        public void SaveDataMapTransformation_DataDataMappingsWithEmptyPubId_VerifySaveNewDataMapMappingWithoutPub()
        {
            // Arrange
            SetupSaveDataMapFakes();

            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append(
                "{\"TransformDataMapID\":2,\"RowID\":3,\"PubID\":\"\",\"MatchType\":\"2\",\"SourceData\":\"source-data\",\"DesiredData\":\"desired-data\"}");
            builder.Append("]");

            var dataMapId = 2;
            _deleteId = 2;
            _expectedTransformationDataMap = new TransformDataMap
            {
                TransformDataMapID = dataMapId,
                TransformationID = TransformationId,
                MatchType = "2",
                SourceData = "source-data",
                DesiredData = "desired-data",
                IsActive = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                CreatedByUserID = UserId,
                UpdatedByUserID = UserId,
                PubID = 0
            };

            _dataMapTransforms.Add(_expectedTransformationDataMap);

            _pubIds = new List<string> { "0" };

            // Act
            JsonResult result = _controller.SaveDataMapTransformation(TransformationId, true, builder.ToString());
            Data data = GetDataMapData(result);

            // Assert
            data.ShouldSatisfyAllConditions(
                () => data.FieldMappingID.ShouldBe(FieldMappingId),
                () => data.IsComplete.ShouldBeTrue(),
                () => data.Message.ShouldBeEmpty(),
                () => data.SourceFileID.ShouldBe(SourceFileId.ToString()));

            result.JsonRequestBehavior.ShouldBe(JsonRequestBehavior.AllowGet);
        }

        [Test]
        public void SaveDataMapTransformation_DataMapMappingsWithEmptyPubWithoutMapsProductCode_ReturnErrorMessage()
        {
            // Arrange
            SetupSaveDataMapFakes();
            var expectedError = "<li>Row 3: Product must be selected.</li>";

            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append(
                "{\"TransformDataMapID\":2,\"RowID\":3,\"PubID\":\"\",\"MatchType\":\"2\",\"SourceData\":\"source-data\",\"DesiredData\":\"desired-data\"}");
            builder.Append("]");

            // Act
            JsonResult result = _controller.SaveDataMapTransformation(TransformationId, false, builder.ToString());
            Data data = GetDataMapData(result);

            // Assert
            data.ShouldSatisfyAllConditions(
                () => data.FieldMappingID.ShouldBe(0),
                () => data.IsComplete.ShouldBeFalse(),
                () => data.Message.ShouldBe(expectedError),
                () => data.SourceFileID.ShouldBe("0"));

            result.JsonRequestBehavior.ShouldBe(JsonRequestBehavior.AllowGet);
        }

        [Test]
        public void SaveDataMapTransformation_DuplicateDataMapMappings_ReturnErrorMessage()
        {
            // Arrange
            SetupSaveDataMapFakes();
            StringBuilder expectedError = new StringBuilder();
            Enumerable.Range(0, 10).ToList().ForEach(x => expectedError.Append("<li>Row 3: Row has duplicate entry.</li>"));

            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append(
                "{\"TransformDataMapID\":2,\"RowID\":3,\"PubID\":\"1\",\"MatchType\":\"2\",\"SourceData\":\"source-data\",\"DesiredData\":\"desired-data\"},");
            builder.Append(
                "{\"TransformDataMapID\":2,\"RowID\":3,\"PubID\":\"0\",\"MatchType\":\"2\",\"SourceData\":\"source-data\",\"DesiredData\":\"desired-data\"},");
            builder.Append(
                "{\"TransformDataMapID\":2,\"RowID\":3,\"PubID\":\"0\",\"MatchType\":\"2\",\"SourceData\":\"source-data\",\"DesiredData\":\"desired-data\"},");
            builder.Append(
                "{\"TransformDataMapID\":2,\"RowID\":3,\"PubID\":\"1\",\"MatchType\":\"2\",\"SourceData\":\"source-data\",\"DesiredData\":\"desired-data\"}");
            builder.Append("]");

            // Act
            JsonResult result = _controller.SaveDataMapTransformation(TransformationId, true, builder.ToString());
            Data data = GetDataMapData(result);

            // Assert
            data.ShouldSatisfyAllConditions(
                () => data.FieldMappingID.ShouldBe(0),
                () => data.IsComplete.ShouldBeFalse(),
                () => data.Message.ShouldBe(expectedError.ToString()),
                () => data.SourceFileID.ShouldBe("0"));

            result.JsonRequestBehavior.ShouldBe(JsonRequestBehavior.AllowGet);
        }

        private void SetupSaveDataMapFakes()
        {
            ShimFileMapperWizardController.AllInstances.ApplyTransformationPubMapInt32ListOfString =
                (_, transformationId, pubIds) =>
                {
                    transformationId.ShouldBe(TransformationId);
                    _pubIds.ShouldBe(pubIds);
                    return string.Empty;
                };

            ShimFileMapperWizardController.AllInstances.VerifyIfTransformationIsCompleteInt32StringOutInt32OutInt32Out =
                VerifyIfTransformationIsComplete;

            ShimTransformDataMap.AllInstances.SaveTransformDataMap = (map, dataMap) =>
            {
                _expectedTransformationDataMap.IsContentMatched(dataMap).ShouldBeTrue();
                return dataMap.TransformDataMapID;
            };

            ShimTransformDataMap.AllInstances.DeleteInt32 = (map, id) =>
            {
                _deleteId.ShouldBe(id);

                return null;
            };
        }

        private static Data GetDataMapData(JsonResult result)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<Data>(serializer.Serialize(result.Data));
        }
    }
}
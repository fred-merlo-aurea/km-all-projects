using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FrameworkUAD.Entity;
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
    ///     Unit Tests for <see cref="FileMapperWizardController.SaveTransformation"/>
    /// </summary>
    public partial class FileMapperWizardControllerTest
    {
        private const int FieldMappingId = 45;

        private TransformAssign _expectedTransformationAssign;
        private List<string> _pubIds;
        private int _deleteId;

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void SaveTransformation_NullOrEmptyDataMappings_VerifyJSONResult(string dataMapping)
        {
            // Arrange, Act
            JsonResult result = _controller.SaveAssignTransformation(1, TransformationId, dataMapping);
            Data data = GetTransformAssignData(result);

            // Assert
            data.ShouldSatisfyAllConditions(
               () => data.FieldMappingID.ShouldBe(0),
               () => data.IsComplete.ShouldBeTrue(),
               () => data.Message.ShouldBeEmpty(),
               () => data.SourceFileID.ShouldBe("0"),
               () => data.TransformationAssignID.ShouldBe(1));

            result.JsonRequestBehavior.ShouldBe(JsonRequestBehavior.AllowGet);
        }

        [Test]
        public void SaveTransformation_AssignDataMappings_VerifySaveNewAssignDataMapping()
        {
            // Arrange
            SetupSaveTransformationFakes();
            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append("{\"TransformAssignID\":1,\"RowID\":3,\"PubID\":\"4\",\"Value\":\"5\"}");
            builder.Append("]");

            var transformAssignId = 1;
            _deleteId = 1;
            _expectedTransformationAssign = new TransformAssign
            {
                TransformAssignID = transformAssignId,
                TransformationID = TransformationId,
                Value = "5",
                HasPubID = true,
                IsActive = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                CreatedByUserID = UserId,
                UpdatedByUserID = UserId,
                PubID = 4
            };

            _assignTransforms.Add(_expectedTransformationAssign);

            _pubIds = new List<string> { "4" };

            // Act
            JsonResult result = _controller.SaveAssignTransformation(transformAssignId, TransformationId, builder.ToString());
            Data data = GetTransformAssignData(result);

            // Assert
            data.ShouldSatisfyAllConditions(
                () => data.FieldMappingID.ShouldBe(FieldMappingId),
                () => data.IsComplete.ShouldBeTrue(),
                () => data.Message.ShouldBeEmpty(),
                () => data.SourceFileID.ShouldBe(SourceFileId.ToString()),
                () => data.TransformationAssignID.ShouldBe(transformAssignId));

            result.JsonRequestBehavior.ShouldBe(JsonRequestBehavior.AllowGet);
        }

        [Test]
        public void SaveTransformation_AssignDataMappingsWithEmptyPubId_VerifySaveNewAssignDataMappingWithoutPub()
        {
            // Arrange
            SetupSaveTransformationFakes();

            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append("{\"TransformAssignID\":1,\"RowID\":3,\"PubID\":\"\",\"Value\":\"5\"}");
            builder.Append("]");

            var transformAssignId = 1;
            _deleteId = 1;
            _expectedTransformationAssign = new TransformAssign
            {
                TransformAssignID = transformAssignId,
                TransformationID = TransformationId,
                Value = "5",
                HasPubID = false,
                IsActive = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                CreatedByUserID = UserId,
                UpdatedByUserID = UserId,
                PubID = -1
            };

            _assignTransforms.Add(_expectedTransformationAssign);

            _pubIds = new List<string> { "-1" };

            // Act
            JsonResult result = _controller.SaveAssignTransformation(transformAssignId, TransformationId, builder.ToString());
            Data data = GetTransformAssignData(result);

            // Assert
            data.ShouldSatisfyAllConditions(
                () => data.FieldMappingID.ShouldBe(FieldMappingId),
                () => data.IsComplete.ShouldBeTrue(),
                () => data.Message.ShouldBeEmpty(),
                () => data.SourceFileID.ShouldBe(SourceFileId.ToString()),
                () => data.TransformationAssignID.ShouldBe(transformAssignId));

            result.JsonRequestBehavior.ShouldBe(JsonRequestBehavior.AllowGet);
        }

        [Test]
        public void SaveTransformation_AssignDataMappingsWithEmptyPubAndValue_ReturnErrorMessage()
        {
            // Arrange
            SetupSaveTransformationFakes();
            var expectedError = "<li>Value To Assign must be entered or a product must be selected.</li>";

            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append("{\"TransformAssignID\":1,\"RowID\":3,\"PubID\":\"\",\"Value\":\"\"}");
            builder.Append("]");

            var transformAssignId = 1;

            // Act
            JsonResult result = _controller.SaveAssignTransformation(transformAssignId, TransformationId, builder.ToString());
            Data data = GetTransformAssignData(result);

            // Assert
            data.ShouldSatisfyAllConditions(
                () => data.FieldMappingID.ShouldBe(0),
                () => data.IsComplete.ShouldBeFalse(),
                () => data.Message.ShouldBe(expectedError),
                () => data.SourceFileID.ShouldBe("0"),
                () => data.TransformationAssignID.ShouldBe(transformAssignId));

            result.JsonRequestBehavior.ShouldBe(JsonRequestBehavior.AllowGet);
        }

        [Test]
        public void SaveTransformation_AssignMultipleDataMappingsWithPubIdLessThanEqualToZero_ReturnErrorMessage()
        {
            // Arrange
            SetupSaveTransformationFakes();
            var expectedError = 
                "<li>Row 3: Row has duplicate product used - ALL PRODUCTS.</li><li>Row 3: Row has duplicate product used - ALL PRODUCTS.</li>";

            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append("{\"TransformAssignID\":1,\"RowID\":3,\"PubID\":\"0\",\"Value\":\"5\"},");
            builder.Append("{\"TransformAssignID\":1,\"RowID\":3,\"PubID\":\"0\",\"Value\":\"5\"}");
            builder.Append("]");

            var transformAssignId = 1;

            // Act
            JsonResult result = _controller.SaveAssignTransformation(transformAssignId, TransformationId, builder.ToString());
            Data data = GetTransformAssignData(result);

            // Assert
            data.ShouldSatisfyAllConditions(
                () => data.FieldMappingID.ShouldBe(0),
                () => data.IsComplete.ShouldBeFalse(),
                () => data.Message.ShouldBe(expectedError),
                () => data.SourceFileID.ShouldBe("0"),
                () => data.TransformationAssignID.ShouldBe(transformAssignId));

            result.JsonRequestBehavior.ShouldBe(JsonRequestBehavior.AllowGet);
        }

        [Test]
        public void SaveTransformation_AssignMultipleDataMappingsWithPubIdGreaterThanZero_ReturnErrorMessage()
        {
            // Arrange
            SetupSaveTransformationFakes();
            var expectedError =
                "<li>Row 3: Row has duplicate product used - pub-code.</li><li>Row 3: Row has duplicate product used - pub-code.</li>";

            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append("{\"TransformAssignID\":1,\"RowID\":3,\"PubID\":\"1\",\"Value\":\"5\"},");
            builder.Append("{\"TransformAssignID\":1,\"RowID\":3,\"PubID\":\"1\",\"Value\":\"5\"}");
            builder.Append("]");

            var transformAssignId = 1;

            _products.Add(new Product
            {
                PubID = 1,
                PubCode = "pub-code"
            });

            // Act
            JsonResult result = _controller.SaveAssignTransformation(transformAssignId, TransformationId, builder.ToString());
            Data data = GetTransformAssignData(result);

            // Assert
            data.ShouldSatisfyAllConditions(
                () => data.FieldMappingID.ShouldBe(0),
                () => data.IsComplete.ShouldBeFalse(),
                () => data.Message.ShouldBe(expectedError),
                () => data.SourceFileID.ShouldBe("0"),
                () => data.TransformationAssignID.ShouldBe(transformAssignId));

            result.JsonRequestBehavior.ShouldBe(JsonRequestBehavior.AllowGet);
        }

        [Test]
        public void SaveTransformation_AssignDataMappingsWithNullPubId_ShouldThrowNullReferenceException()
        {
            // Arrange
            SetupSaveTransformationFakes();
            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append("{\"TransformAssignID\":1,\"RowID\":3,\"PubID\":null,\"Value\":\"5\"}");
            builder.Append("]");

            var transformAssignId = 1;
            _pubIds = new List<string> { "4" };

            // Act, Assert
            Should.Throw<NullReferenceException>(
                () => _controller.SaveAssignTransformation(transformAssignId, TransformationId, builder.ToString()));
        }

        private void SetupSaveTransformationFakes()
        {
            ShimFileMapperWizardController.AllInstances.ApplyTransformationPubMapInt32ListOfString =
                (_, transformationId, pubIds) =>
                {
                    transformationId.ShouldBe(TransformationId);
                    _pubIds.ShouldBe(pubIds);
                    return string.Empty;
                };

            ShimFileMapperWizardController.AllInstances.VerifyIfTransformationIsCompleteInt32StringOutInt32OutInt32Out = VerifyIfTransformationIsComplete;

            ShimTransformAssign.AllInstances.SaveTransformAssign = (assign, transformAssign) =>
            {
                _expectedTransformationAssign.IsContentMatched(transformAssign).ShouldBeTrue();
                return transformAssign.TransformAssignID;
            };

            ShimTransformAssign.AllInstances.DeleteInt32 = (assign, id) =>
            {
                _deleteId.ShouldBe(id);

                return id;
            };
        }

        private bool VerifyIfTransformationIsComplete(
            FileMapperWizardController @this,
            int id,
            out string errorMessage,
            out int fieldMappingId,
            out int fileId)
        {
            errorMessage = string.Empty;
            fieldMappingId = FieldMappingId;
            fileId = SourceFileId;
            return true;
        }

        private static Data GetTransformAssignData(JsonResult result)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<Data>(serializer.Serialize(result.Data));
        }

        internal class Data
        {
            public int TransformationAssignID { get; set; }
            public bool IsComplete { get; set; }
            public string Message { get; set; }
            public string SourceFileID { get; set; }
            public int FieldMappingID { get; set; }
        }
    }
}

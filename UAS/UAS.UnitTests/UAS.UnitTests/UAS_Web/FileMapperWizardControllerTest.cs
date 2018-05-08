using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAS.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAS_Web.Common;
using UAS.Web.Controllers.FileMapperWizard;
using UAS.Web.Controllers.FileMapperWizard.Fakes;
using UAS.Web.Models.FileMapperWizard;
using ShimTransformationFieldMapWorker = FrameworkUAS.BusinessLogic.Fakes.ShimTransformationFieldMap;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimTransformationFieldMap;

namespace UAS.UnitTests.UAS_Web
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FileMapperWizardControllerTest : Fakes
    {
        private const int TestFieldMappingId = 100;
        private const int TestSourceFileId = 200;

        private int _transformationId = 1;
        private string _errorMessage = string.Empty;
        private int _fieldMappingId = 1;
        private int _sourceFileId = 1;

        private FileMapperWizardController _testController;
        
        [SetUp]
        public void Setup()
        {
            SetupContext();
            ShimForCurrentUser();
            ShimForApplyTransformationPubMap();

            _testController = new FileMapperWizardController();
        }

        [TearDown]
        public void Cleanup() => DisposeContext();

        [Test]
        public void VerifyIfTransformationIsComplete_FmwModelIsNull_ThrowsException()
        {
            // Arrange
            ShimFileMapperWizardController.AllInstances.fmwModelGet = fmModel => null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => { 
                var result = _testController.VerifyIfTransformationIsComplete(
                    _transformationId, 
                    out _errorMessage,
                    out _fieldMappingId, 
                    out _sourceFileId);
            });
        }

        [Test]
        public void VerifyIfTransformationIsComplete_IfStandardTransformationDataModelIsNull_ReturnsFalse()
        {
            // Arrange
            ShimFileMapperWizardController.AllInstances.fmwModelGet = fmModel => new FileMapperWizardViewModel();

            // Act
            var result = _testController.VerifyIfTransformationIsComplete(
                _transformationId, 
                out _errorMessage,
                out _fieldMappingId, 
                out _sourceFileId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(false),
                () => _errorMessage.ShouldBeEmpty(),
                () => _fieldMappingId.ShouldBe(0),
                () => _sourceFileId.ShouldBe(0)
            );
        }

        [Test]
        public void VerifyIfTransformationIsComplete_ShouldApplyTransformationFieldMap_ReturnsCorrectValue()
        {
            // Arrange
            ShimFileMapperWizardController.AllInstances.fmwModelGet =
                fmModel => new FileMapperWizardViewModel()
                {
                    standardTransformationDataModel = new StandardTransformationDataModel()
                    {
                        FieldMappingId = TestFieldMappingId
                    }
                };

            ShimFileMapperWizardController.AllInstances.ApplyTransformationFieldMapInt32Int32Int32 =
                (_, transformationId, sourceFileId, fieldMappingId) => string.Empty;

            // Act
            var result = _testController.VerifyIfTransformationIsComplete(
                _transformationId, 
                out _errorMessage,
                out _fieldMappingId, 
                out _sourceFileId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(true),
                () => _errorMessage.ShouldBeEmpty(),
                () => _fieldMappingId.ShouldBe(TestFieldMappingId),
                () => _sourceFileId.ShouldBe(0)
            );
        }

        [Test]
        public void VerifyIfTransformationIsComplete_ShouldTryParseSourceFileId_ReturnsCorrectValue()
        {
            // Arrange
            ShimFileMapperWizardController.AllInstances.fmwModelGet =
                fmModel => new FileMapperWizardViewModel()
                {
                    standardTransformationDataModel = new StandardTransformationDataModel()
                    {
                        SourceFileId = TestSourceFileId
                    }
                };

            ShimFileMapperWizardController.AllInstances.ApplyTransformationFieldMapInt32Int32Int32 =
                (_, transformationId, sourceFileId, fieldMappingId) => string.Empty;

            // Act
            var result = _testController.VerifyIfTransformationIsComplete(
                _transformationId, 
                out _errorMessage,
                out _fieldMappingId, 
                out _sourceFileId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(true),
                () => _errorMessage.ShouldBeEmpty(),
                () => _fieldMappingId.ShouldBe(0),
                () => _sourceFileId.ShouldBe(TestSourceFileId)
            );
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void ApplyTransformationFieldMap_WithTransformationIDLessOrEqualZero_ReturnsErrorMessage(int transformationId)
        {
            // Act
            var result = _testController.ApplyTransformationFieldMap(
                transformationId, 
                _sourceFileId, 
                _fieldMappingId);

            // Assert
            result.ShouldBe(TransformationFieldMapExceptionMessage);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void ApplyTransformationFieldMap_WithFieldMappingIDLessOrEqualZero_ReturnsErrorMessage(int fieldMappingId)
        {
            // Act
            var result = _testController.ApplyTransformationFieldMap(
                _transformationId, 
                _sourceFileId, 
                fieldMappingId);

            // Assert
            result.ShouldBe(FieldMappingIdExceptionMessage);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void ApplyTransformationFieldMap_WithSourceFileIDLessOrEqualZero_ReturnsErrorMessage(int sourceFileId)
        {
            // Act
            var result = _testController.ApplyTransformationFieldMap(
                _transformationId, 
                sourceFileId, 
                _fieldMappingId);

            // Assert
            result.ShouldBe(SourceFileIdExceptionMessage);
        }

        [Test]
        public void ApplyTransformationFieldMap_ShouldFindCurrentTfmWorker_ReturnsEmptyErrorMessage()
        {
            // Arrange
            ShimWorker.AllInstances.Select =
                (_) => new List<TransformationFieldMap>()
                {
                    new TransformationFieldMap
                    {
                        SourceFileID = _sourceFileId,
                        TransformationID = _transformationId,
                        FieldMappingID = _fieldMappingId
                    }
                };

            // Act
            var result = _testController.ApplyTransformationFieldMap(
                _transformationId, 
                _sourceFileId, 
                _fieldMappingId);

            // Assert
            result.ShouldBeEmpty();
        }
        
        [Test]
        [TestCase(0, 0, 0)]
        public void ApplyTransformationFieldMap_ShouldNotFindCurrentTfmWorkerAndOnSaveThrows_ReturnsErrorMessage(
            int sourceFileId, int transformationId, int fieldMappingId)
        {
            // Arrange
            ShimTransformationFieldMapWorker.AllInstances.SaveTransformationFieldMap = 
                (_, x) => throw new InvalidOperationException();

            ShimWorker.AllInstances.Select =
                (_) => new List<TransformationFieldMap>()
                {
                    new TransformationFieldMap
                    {
                        SourceFileID = sourceFileId,
                        TransformationID = transformationId,
                        FieldMappingID = fieldMappingId
                    }
                };

            // Act 
            var result = _testController.ApplyTransformationFieldMap(
                _transformationId, 
                _sourceFileId, 
                _fieldMappingId);

            // Assert
            result.ShouldBe(ErrorApplyingTransformationMessage);
        }

        [Test]
        [TestCase(0, 0, 0)]
        public void ApplyTransformationFieldMap_ShouldNotFindCurrentTfmWorkerAndCreateNewOne_ReturnsEmptyErrorMessage(
            int sourceFileId, int transformationId, int fieldMappingId)
        {
            // Arrange
            ShimTransformationFieldMapWorker.AllInstances.SaveTransformationFieldMap = (_, x) => 1;

            ShimWorker.AllInstances.Select =
                (_) => new List<TransformationFieldMap>()
                {
                    new TransformationFieldMap
                    {
                        SourceFileID = sourceFileId,
                        TransformationID = transformationId,
                        FieldMappingID = fieldMappingId
                    }
                };

            // Act 
            var result = _testController.ApplyTransformationFieldMap(
                _transformationId, 
                _sourceFileId, 
                _fieldMappingId);

            // Assert
            result.ShouldBeEmpty();
        }
        
    }
}

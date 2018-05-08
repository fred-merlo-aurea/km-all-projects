using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Fakes;
using Core_AMS.Utilities.Fakes;
using FileMapperWizard.Helpers;
using FileMapperWizard.Modules;
using FileMapperWizard.Modules.Fakes;
using FrameworkServices;
using FrameworkServices.Fakes;
using FrameworkUAS.Entity;
using FrameworkUAS.Service;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS_WS.Interface;
using UAS_WS.Interface.Fakes;

namespace UAS.UnitTests.FileMapperWizard.Helpers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ApplyTransformationHelperTest
    {
        private const int InvalidID = -1;
        private const int ValidID = 7;
        private const int NotFoundID = 25;
        private const int NewTransformFieldMapId = 100;

        private string messageBoxMessage = string.Empty;
        private ServiceClient<ITransformationFieldMap> transformationFieldMapClient;
        private IDisposable _shims;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
            CreateServiceClients();
        }

        [TearDown]
        public void Teardown()
        {
            _shims?.Dispose();
        }

        [Test]
        public void ApplyTransformation_TransformationIDNotValid_ReturnsFalse()
        {
            // Arrange
            var parameters = new ApplyTransformationParameters();
            parameters.TransformationID = InvalidID;
            parameters.FieldMappingID = ValidID;
            parameters.SourceFileID = ValidID;
            parameters.AuthorizedUserId = ValidID;

            ShimWPF.MessageStringMessageBoxButtonMessageBoxImageMessageBoxResultString =
                (message, y, z, p, q) => { messageBoxMessage = message; };
            var shim = new ShimFMUniversal();

            // Act
            var result = ApplyTransformationHelper.ApplyTransformation(parameters, transformationFieldMapClient, shim);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeFalse();
            messageBoxMessage.ShouldBe(ApplyTransformationHelper.TransformationNotClearErrorMessage);
        }

        [Test]
        public void ApplyTransformation_FieldMappingIDNotValid_ReturnsFalse()
        {
            // Arrange
            var parameters = new ApplyTransformationParameters();
            parameters.TransformationID = ValidID;
            parameters.FieldMappingID = InvalidID;
            parameters.SourceFileID = ValidID;
            parameters.AuthorizedUserId = ValidID;
            ShimWPF.MessageStringMessageBoxButtonMessageBoxImageMessageBoxResultString =
                (message, y, z, p, q) => { messageBoxMessage = message; };
            var fmUniversal = new ShimFMUniversal();

            // Act
            var result = ApplyTransformationHelper.ApplyTransformation(
                parameters,
                transformationFieldMapClient,
                fmUniversal);

            // Assert
            result.ShouldBeFalse();
            messageBoxMessage.ShouldBe(ApplyTransformationHelper.ColumnNotClearErrorMessage);
        }

        [Test]
        public void ApplyTransformation_SourceFileIDNotValid_ReturnsFalse()
        {
            // Arrange
            var parameters = new ApplyTransformationParameters();
            parameters.TransformationID = ValidID;
            parameters.FieldMappingID = ValidID;
            parameters.SourceFileID = InvalidID;
            parameters.AuthorizedUserId = ValidID;
            ShimWPF.MessageStringMessageBoxButtonMessageBoxImageMessageBoxResultString =
                (message, y, z, p, q) => { messageBoxMessage = message; };
            var fmUniversal = new ShimFMUniversal();

            // Act
            var result = ApplyTransformationHelper.ApplyTransformation(
                parameters,
                transformationFieldMapClient,
                fmUniversal);

            // Assert
            result.ShouldBeFalse();
            messageBoxMessage.ShouldBe(ApplyTransformationHelper.SourceNotClearErrorMessage);
        }

        [Test]
        public void ApplyTransformation_AlreadyAdded_ReturnsTrue()
        {
            // Arrange
            var parameters = new ApplyTransformationParameters();
            parameters.TransformationID = ValidID;
            parameters.FieldMappingID = ValidID;
            parameters.SourceFileID = ValidID;
            parameters.AuthorizedUserId = ValidID;
            ShimWPF.MessageStringMessageBoxButtonMessageBoxImageString =
                (message, y, z, p) => { messageBoxMessage = message; };
            var fmUniversal = new ShimFMUniversal();

            // Act
            var result = ApplyTransformationHelper.ApplyTransformation(
                parameters,
                transformationFieldMapClient,
                fmUniversal);

            // Assert
            result.ShouldBeTrue();
            messageBoxMessage.ShouldBe(ApplyTransformationHelper.AlreadyAddedErrorMessage);
        }

        [Test]
        public void ApplyTransformation_AllParametersValid_ReturnsTrue()
        {
            // Arrange
            var parameters = new ApplyTransformationParameters();
            parameters.TransformationID = ValidID;
            parameters.FieldMappingID = NewTransformFieldMapId;
            parameters.SourceFileID = ValidID;
            parameters.AuthorizedUserId = ValidID;
            var fieldMappings = new List<int>();
            var shim = new ShimFMUniversal();
            shim.fieldMappingsWithTransformationsGet = () => fieldMappings;

            // Act
            var result = ApplyTransformationHelper.ApplyTransformation(
                parameters,
                transformationFieldMapClient,
                shim);

            // Assert
            result.ShouldBeTrue();
            fieldMappings.ShouldContain(NewTransformFieldMapId);
        }

        private void CreateServiceClients()
        {
            ShimServiceClient<ITransformationFieldMap>.AllInstances.ProxyGet = (_) =>
            {
                var client = new StubITransformationFieldMap();
                client.SaveGuidTransformationFieldMap = (accessKey, transformationFieldMap) =>
                {
                    return new Response<int>
                    {
                        Result = NewTransformFieldMapId
                    };
                };
                client.SelectGuid = (accessKey) =>
                {
                    return new Response<List<TransformationFieldMap>>
                    {
                        Result = new List<TransformationFieldMap>
                                      {
                                       new TransformationFieldMap{
                                           TransformationID=ValidID,
                                            SourceFileID = ValidID,
                                            FieldMappingID = ValidID
                                       }
                        }
                    };

                };
                return client;
            };

            transformationFieldMapClient = new ShimServiceClient<ITransformationFieldMap>();
        }
    }
}

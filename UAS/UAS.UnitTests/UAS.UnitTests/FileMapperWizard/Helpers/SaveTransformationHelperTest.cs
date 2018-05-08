using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Fakes;
using Core_AMS.Utilities.Fakes;
using FileMapperWizard.Helpers;
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
    public class SaveTransformationHelperTest
    {
        private const string TransformNameNotfound = "transform_0";
        private const string TransformNameFoundFirst = "transform_1";
        private const string TransformNameFoundSecond = "transform_2";
        private const string TransformNameFoundThird = "transform_3";
        private const string SampleDescription = "SampleDescription";
        private const string SampleName = "SampleName";
        private const int NameExistTransformationID = 0;
        private const int ValidTransformationID = 7;
        private const int SaveFailTransformationID = -1;

        private int NewTransformId = 7;
        private string messageBoxMessage = string.Empty;
        private ServiceClient<ITransformation> transformationClient;
        private ServiceClient<IDBWorker> dbWorker;
        private IDisposable _shims;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
            NewTransformId = ValidTransformationID;
            CreateServiceClients();
        }

        [TearDown]
        public void Teardown()
        {
            _shims?.Dispose();
        }

        [TestCase(null, null)]
        [TestCase(SampleName, "")]
        [TestCase("", SampleDescription)]
        [TestCase("", "")]
        public void SaveTransformSplit_NameIsEmpty_ReturnsFalse(string name, string description)
        {
            // Arrange
            var parameters = new SaveTransformationParameters();
            parameters.Name = string.Empty;
            ShimWPF.MessageStringMessageBoxButtonMessageBoxImageMessageBoxResultString = 
                (message, y, z, p, q) => 
                { messageBoxMessage = message; };

            // Act
            var result = SaveTransformationHelper.SaveTransformSplit(
                parameters,
                transformationClient,
                dbWorker);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            messageBoxMessage.ShouldBe(SaveTransformationHelper.DescriptionOrNameIsEmptyErrorMessage);
        }

        [Test]
        public void SaveTransformSplit_TransformNotFound_ReturnsNewTransform()
        {
            // Arrange
            var parameters = new SaveTransformationParameters();
            parameters.Name = TransformNameNotfound;
            parameters.Description = SampleDescription;
 
            // Act
            var result = SaveTransformationHelper.SaveTransformSplit(
                parameters,
                transformationClient,
                dbWorker);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Success.ShouldBeTrue());
            result.TransformID.ShouldBe(NewTransformId);
        }

        [Test]
        public void SaveTransformSplit_TransformNotFoundSaveFails_ReturnsFalse()
        {
            // Arrange
            var parameters = new SaveTransformationParameters();
            parameters.Name = TransformNameNotfound;
            parameters.Description = SampleDescription;
            NewTransformId = SaveFailTransformationID;
            ShimWPF.MessageStringMessageBoxButtonMessageBoxImageMessageBoxResultString = 
                (message, y, z, p, q) =>
                { messageBoxMessage = message; };

            // Act
            var result = SaveTransformationHelper.SaveTransformSplit(
                parameters,
                transformationClient,
                dbWorker);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            messageBoxMessage.ShouldBe(SaveTransformationHelper.SaveErrorMessage);
        }

        [Test]
        public void SaveTransformSplit_TransformFound_ShowsNameExistErrorReturnsFalse()
        {
            // Arrange
            var parameters = new SaveTransformationParameters();
            parameters.Name = TransformNameFoundFirst;
            parameters.Description = SampleDescription;
            parameters.TransformationID = 0;
            NewTransformId = SaveFailTransformationID;
            ShimWPF.MessageStringMessageBoxButtonMessageBoxImageMessageBoxResultString = 
                (message, y, z, p, q) => 
                { messageBoxMessage = message; };
            
            // Act
            var result = SaveTransformationHelper.SaveTransformSplit(
                parameters,
                transformationClient,
                dbWorker);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Success.ShouldBeFalse());
            messageBoxMessage.ShouldBe(SaveTransformationHelper.NameExistErrorMessage);
        }

        [Test]
        public void SaveTransformSplit_TransformFound_SavesAndReturnsTrue()
        {
            // Arrange
            var parameters = new SaveTransformationParameters();
            parameters.Name = TransformNameFoundFirst;
            parameters.Description = SampleDescription;
            parameters.TransformationID = 6;
            ShimMessageBox.ShowStringStringMessageBoxButtonMessageBoxImage =
                (message, caption, p, q) =>
                { messageBoxMessage = message; return MessageBoxResult.Yes; };

            // Act
            var result = SaveTransformationHelper.SaveTransformSplit(
                parameters,
                transformationClient,
                dbWorker);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Success.ShouldBeTrue());
            messageBoxMessage.ShouldBe(SaveTransformationHelper.ConfirmOverriteMessage);
            result.TransformID.ShouldBe(NewTransformId);
        }

        [Test]
        public void SaveTransformSplit_TransformFound_ReturnsFalse()
        {
            // Arrange
            var parameters = new SaveTransformationParameters();
            parameters.Name = TransformNameFoundFirst;
            parameters.Description = SampleDescription;
            parameters.TransformationID = 6;
            ShimMessageBox.ShowStringStringMessageBoxButtonMessageBoxImage =
                (message, caption, p, q) =>
                { messageBoxMessage = message; return MessageBoxResult.No; };

            // Act
            var result = SaveTransformationHelper.SaveTransformSplit(
                parameters,
                transformationClient,
                dbWorker);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Success.ShouldBeFalse());
            messageBoxMessage.ShouldBe(SaveTransformationHelper.ConfirmOverriteMessage);
        }

        private void CreateServiceClients()
        {
            ShimServiceClient<ITransformation>.AllInstances.ProxyGet = (_) =>
            {
                var client = new StubITransformation();
                client.SelectGuidInt32 = (a, b) =>
                {
                    return new Response<List<Transformation>>
                    {
                        Result = new List<Transformation>
                                      {
                                       new Transformation{TransformationName=TransformNameFoundFirst },
                                       new Transformation{TransformationName=TransformNameFoundSecond },
                                       new Transformation{TransformationName=TransformNameFoundThird},
                                      }
                    };
                };
                return client;
            };

            ShimServiceClient<IDBWorker>.AllInstances.ProxyGet = (a) =>
            {
                var client = new StubIDBWorker();
                client.SaveGuidInt32Int32StringStringInt32Int32BooleanBoolean = (b, c, d, e, f, g, h, j, k) =>
                {
                    return new Response<int>
                    {
                        Result = NewTransformId
                    };
                };
                return client;
            };

            transformationClient = new ShimServiceClient<ITransformation>();
            dbWorker = new ShimServiceClient<IDBWorker>();
        }
    }
}

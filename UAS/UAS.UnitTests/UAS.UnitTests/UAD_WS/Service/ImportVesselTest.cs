using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Core_AMS.Utilities;
using KM.Common.Import;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using ServiceImportVessel = UAD_WS.Service.ImportVessel;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimImportVessel;
using ResponseStatus = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes;
using EntityImportVessel = FrameworkUAD.Object.ImportVessel;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ImportVesselTest : Fakes
    {
        private const string SampleString = "some values.";
        private const string SampleFileName = "file1.txt";

        private ServiceImportVessel _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceImportVessel();
            ShimForJsonFunction<EntityImportVessel>();
        }

        [Test]
        public void GetBadData_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityImportVessel();
            ShimWorker.GetBadDataImportVessel = _ => throw new InvalidOperationException();

            // Act
            var result = _testEntity.GetBadData(Guid.Empty, entity);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Error);
        }

        [Test]
        public void GetBadData_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityImportVessel();
            ShimWorker.GetBadDataImportVessel = importVessel => SampleString;

            // Act
            var result = _testEntity.GetBadData(Guid.Empty, entity);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBe(SampleString);
        }

        [Test]
        public void GetTransformedData_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityImportVessel();
            ShimWorker.GetTransformedDataImportVessel = importVessel => SampleString;

            // Act
            var result = _testEntity.GetTransformedData(Guid.Empty, entity);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBe(SampleString);
        }

        [Test]
        public void GetImportVessel_WithFileInfo_ReturnsSuccessResponse()
        {
            // Arrange
            var fileInfo = new FileInfo(SampleFileName);
            var entity = new EntityImportVessel();
            ShimWorker.AllInstances.GetImportVesselFileInfoInt32Int32FileConfiguration = (a, b, c, d, e) => entity;

            // Act
            var result = _testEntity.GetImportVessel(Guid.Empty, fileInfo, 0, 0);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBeSameAs(entity);
        }

        [Test]
        public void GetImportVesselExcel_WithFileInfo_ReturnsSuccessResponse()
        {
            // Arrange
            var fileInfo = new FileInfo(SampleFileName);
            var entity = new EntityImportVessel();
            ShimWorker.AllInstances.GetImportVesselExcelFileInfo = (_, __) => entity;

            // Act
            var result = _testEntity.GetImportVesselExcel(Guid.Empty, fileInfo);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBeSameAs(entity);
        }

        [Test]
        public void GetImportVesselDbf_WithFileInfo_ReturnsSuccessResponse()
        {
            // Arrange
            var fileInfo = new FileInfo(SampleFileName);
            var entity = new EntityImportVessel();
            ShimWorker.AllInstances.GetImportVesselDbfFileInfoInt32Int32 = (a, b, c, d) => entity;

            // Act
            var result = _testEntity.GetImportVesselDbf(Guid.Empty, fileInfo, 0, 0);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBeSameAs(entity);
        }

        [Test]
        public void GetImportVesselText_WithFileInfoAndFileConfiguration_ReturnsSuccessResponse()
        {
            // Arrange
            var fileInfo = new FileInfo(SampleFileName);
            var fileConfig = new FileConfiguration();
            var entity = new EntityImportVessel();
            ShimWorker.AllInstances.GetImportVesselTextFileInfoFileConfiguration = (a, b, c) => entity;

            // Act
            var result = _testEntity.GetImportVesselText(Guid.Empty, fileInfo, fileConfig);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBeSameAs(entity);
        }

        [Test]
        public void GetImportVesselText_WithFileInfoAndRowNumbers_ReturnsSuccessResponse()
        {
            // Arrange
            var fileInfo = new FileInfo(SampleFileName);
            var fileConfig = new FileConfiguration();
            var entity = new EntityImportVessel();
            ShimWorker.AllInstances.GetImportVesselTextFileInfoInt32Int32FileConfiguration = (a, b, c, e, f) => entity;

            // Act
            var result = _testEntity.GetImportVesselText(Guid.Empty, fileInfo, 0, 0, fileConfig);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBeSameAs(entity);
        }

        [Test]
        public void LoadFileImportVessel_WithFileInfoAndRowNumbers_ReturnsSuccessResponse()
        {
            // Arrange
            var fileInfo = new FileInfo(SampleFileName);
            var fileConfig = new FileConfiguration();
            var entity = new EntityImportVessel();
            ShimWorker.AllInstances.LoadFileImportVesselFileInfoInt32Int32FileConfiguration = (a, b, c, e, f) => entity;

            // Act
            var result = _testEntity.LoadFileImportVessel(Guid.Empty, fileInfo, 0, 0, fileConfig);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBeSameAs(entity);
        }
    }
}

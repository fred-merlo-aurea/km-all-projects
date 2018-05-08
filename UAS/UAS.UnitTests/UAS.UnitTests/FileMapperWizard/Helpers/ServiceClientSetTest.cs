using System;
using System.Diagnostics.CodeAnalysis;
using FileMapperWizard.Helpers;
using FrameworkServices.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAD_Lookup_WS.Interface;
using UAD_WS.Interface;
using UAS_WS.Interface;

namespace UAS.UnitTests.FileMapperWizard.Helpers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    class ServiceClientSetTest
    {
        private IDisposable _shimObject;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void ServiceClientSet_DefaultConstructor_AllServiceClientsCreated()
        {
            // Arrange
            InitServiceClients();

            // Act
            var serviceSet = new ServiceClientSet();

            // Assert
            serviceSet.ShouldNotBeNull();
            serviceSet.ShouldSatisfyAllConditions(
                () => serviceSet.Clients.ShouldNotBeNull(),
                () => serviceSet.DbWorker.ShouldNotBeNull(),
                () => serviceSet.FieldMapping.ShouldNotBeNull(),
                () => serviceSet.FieldMultiMap.ShouldNotBeNull(),
                () => serviceSet.FileMappingColumn.ShouldNotBeNull(),
                () => serviceSet.LookUpCode.ShouldNotBeNull(),
                () => serviceSet.ProductSubscriptionsExtensionWorker.ShouldNotBeNull(),
                () => serviceSet.PublicationWorker.ShouldNotBeNull(),
                () => serviceSet.ResponseGroupWorker.ShouldNotBeNull(),
                () => serviceSet.ServiceFeature.ShouldNotBeNull(),
                () => serviceSet.Services.ShouldNotBeNull(),
                () => serviceSet.SourceFile.ShouldNotBeNull(),
                () => serviceSet.SubscriptionsExtensionMapperWorker.ShouldNotBeNull(),
                () => serviceSet.TransformationFieldMapData.ShouldNotBeNull(),
                () => serviceSet.TransformationFieldMultiMap.ShouldNotBeNull(),
                () => serviceSet.Transformations.ShouldNotBeNull());
        }

        private void InitServiceClients()
        {
            ShimServiceClient.UAD_FileMappingColumnClient = () => new ShimServiceClient<IFileMappingColumn>();
            ShimServiceClient.UAS_ClientClient = () => new ShimServiceClient<IClient>();
            ShimServiceClient.UAS_FieldMappingClient = () => new ShimServiceClient<IFieldMapping>();
            ShimServiceClient.UAD_Lookup_CodeClient = () => new ShimServiceClient<ICode>();
            ShimServiceClient.UAS_FieldMultiMapClient = () => new ShimServiceClient<IFieldMultiMap>();
            ShimServiceClient.UAS_ServiceClient = () => new ShimServiceClient<IService>();
            ShimServiceClient.UAS_ServiceFeatureClient = () => new ShimServiceClient<IServiceFeature>();
            ShimServiceClient.UAS_SourceFileClient = () => new ShimServiceClient<ISourceFile>();
            ShimServiceClient.UAS_TransformationClient = () => new ShimServiceClient<ITransformation>();
            ShimServiceClient.UAS_TransformationFieldMapClient = () => new ShimServiceClient<ITransformationFieldMap>();
            ShimServiceClient.UAS_DBWorkerClient = () => new ShimServiceClient<IDBWorker>();
            ShimServiceClient.UAS_TransformationFieldMultiMapClient = () => new ShimServiceClient<ITransformationFieldMultiMap>();
            ShimServiceClient.UAD_SubscriptionsExtensionMapperClient = () => new ShimServiceClient<ISubscriptionsExtensionMapper>();
            ShimServiceClient.UAD_ProductSubscriptionsExtensionClient = () => new ShimServiceClient<IProductSubscriptionsExtension>();
            ShimServiceClient.UAD_ResponseGroupClient = () => new ShimServiceClient<IResponseGroup>();
            ShimServiceClient.UAD_ProductClient = () => new ShimServiceClient<IProduct>();
        }
    }
}
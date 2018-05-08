using FrameworkServices;
using UAD_Lookup_WS.Interface;
using UAD_WS.Interface;
using UAS_WS.Interface;

namespace FileMapperWizard.Helpers
{
    public class ServiceClientSet
    {
        public ServiceClient<IFileMappingColumn> FileMappingColumn { get; }
        public ServiceClient<IClient> Clients { get; }
        public ServiceClient<IFieldMapping> FieldMapping { get; }
        public ServiceClient<ICode> LookUpCode { get; }
        public ServiceClient<IFieldMultiMap> FieldMultiMap { get; }
        public ServiceClient<IService> Services { get; }
        public ServiceClient<IServiceFeature> ServiceFeature { get; }
        public ServiceClient<ISourceFile> SourceFile { get; }
        public ServiceClient<ITransformation> Transformations { get; }
        public ServiceClient<ITransformationFieldMap> TransformationFieldMapData { get; }
        public ServiceClient<IDBWorker> DbWorker { get; }
        public ServiceClient<ITransformationFieldMultiMap> TransformationFieldMultiMap { get; }
        public ServiceClient<ISubscriptionsExtensionMapper> SubscriptionsExtensionMapperWorker { get; }
        public ServiceClient<IProductSubscriptionsExtension> ProductSubscriptionsExtensionWorker { get; }
        public ServiceClient<IResponseGroup> ResponseGroupWorker { get; }
        public ServiceClient<IProduct> PublicationWorker { get; }

        public ServiceClientSet()
        {
            FileMappingColumn = ServiceClient.UAD_FileMappingColumnClient();
            Clients = ServiceClient.UAS_ClientClient();
            FieldMapping = ServiceClient.UAS_FieldMappingClient();
            LookUpCode = ServiceClient.UAD_Lookup_CodeClient();
            FieldMultiMap = ServiceClient.UAS_FieldMultiMapClient();
            Services = ServiceClient.UAS_ServiceClient();
            ServiceFeature = ServiceClient.UAS_ServiceFeatureClient();
            SourceFile = ServiceClient.UAS_SourceFileClient();
            Transformations = ServiceClient.UAS_TransformationClient();
            TransformationFieldMapData = ServiceClient.UAS_TransformationFieldMapClient();
            DbWorker = ServiceClient.UAS_DBWorkerClient();
            TransformationFieldMultiMap = ServiceClient.UAS_TransformationFieldMultiMapClient();
            SubscriptionsExtensionMapperWorker = ServiceClient.UAD_SubscriptionsExtensionMapperClient();
            ProductSubscriptionsExtensionWorker = ServiceClient.UAD_ProductSubscriptionsExtensionClient();
            ResponseGroupWorker = ServiceClient.UAD_ResponseGroupClient();
            PublicationWorker = ServiceClient.UAD_ProductClient();
        }
    }
}

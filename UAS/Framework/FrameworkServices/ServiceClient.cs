using System;
using System.Linq;
using System.ServiceModel;

namespace FrameworkServices
{
    public class ServiceClient<T> : ClientBase<T> where T : class
    {
        private bool _disposed = false;
        public ServiceClient()
            : base(typeof(T).FullName)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
        }
        public ServiceClient(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
        }
        public T Proxy
        {
            get { return this.Channel; }
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    if (this.State == CommunicationState.Faulted)
                    {
                        base.Abort();
                    }
                    else
                    {
                        try
                        {
                            base.Close();
                        }
                        catch
                        {
                            base.Abort();
                        }
                    }
                    _disposed = true;
                }
            }
        }

    }

    public class ServiceClient
    {
        #region UAD_Lookup Clients
        public static ServiceClient<UAD_Lookup_WS.Interface.IAction> UAD_Lookup_ActionClient()
        {
            ServiceClient<UAD_Lookup_WS.Interface.IAction> client = new ServiceClient<UAD_Lookup_WS.Interface.IAction>("BasicHttpBinding_IAction");
            return client;
        }
        public static ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> UAD_Lookup_CategoryCodeClient()
        {
            ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> client = new ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode>("BasicHttpBinding_ICategoryCode");
            return client;
        }
        public static ServiceClient<UAD_Lookup_WS.Interface.ICategoryCodeType> UAD_Lookup_CategoryCodeTypeClient()
        {
            ServiceClient<UAD_Lookup_WS.Interface.ICategoryCodeType> client = new ServiceClient<UAD_Lookup_WS.Interface.ICategoryCodeType>("BasicHttpBinding_ICategoryCodeType");
            return client;
        }
        public static ServiceClient<UAD_Lookup_WS.Interface.ICode> UAD_Lookup_CodeClient()
        {
            ServiceClient<UAD_Lookup_WS.Interface.ICode> client = new ServiceClient<UAD_Lookup_WS.Interface.ICode>("BasicHttpBinding_ICode");
            return client;
        }
        public static ServiceClient<UAD_Lookup_WS.Interface.ICodeType> UAD_Lookup_CodeTypeClient()
        {
            ServiceClient<UAD_Lookup_WS.Interface.ICodeType> client = new ServiceClient<UAD_Lookup_WS.Interface.ICodeType>("BasicHttpBinding_ICodeType");
            return client;
        }
        public static ServiceClient<UAD_Lookup_WS.Interface.ICountry> UAD_Lookup_CountryClient()
        {
            ServiceClient<UAD_Lookup_WS.Interface.ICountry> client = new ServiceClient<UAD_Lookup_WS.Interface.ICountry>("BasicHttpBinding_ICountry");
            return client;
        }
        public static ServiceClient<UAD_Lookup_WS.Interface.ICountryMap> UAD_Lookup_CountryMapClient()
        {
            ServiceClient<UAD_Lookup_WS.Interface.ICountryMap> client = new ServiceClient<UAD_Lookup_WS.Interface.ICountryMap>("BasicHttpBinding_ICountryMap");
            return client;
        }
        public static ServiceClient<UAD_Lookup_WS.Interface.IRegion> UAD_Lookup_RegionClient()
        {
            ServiceClient<UAD_Lookup_WS.Interface.IRegion> client = new ServiceClient<UAD_Lookup_WS.Interface.IRegion>("BasicHttpBinding_IRegion");
            return client;
        }
        public static ServiceClient<UAD_Lookup_WS.Interface.IRegionGroup> UAD_Lookup_RegionGroupClient()
        {
            ServiceClient<UAD_Lookup_WS.Interface.IRegionGroup> client = new ServiceClient<UAD_Lookup_WS.Interface.IRegionGroup>("BasicHttpBinding_IRegionGroup");
            return client;
        }
        public static ServiceClient<UAD_Lookup_WS.Interface.IRegionMap> UAD_Lookup_RegionMapClient()
        {
            ServiceClient<UAD_Lookup_WS.Interface.IRegionMap> client = new ServiceClient<UAD_Lookup_WS.Interface.IRegionMap>("BasicHttpBinding_IRegionMap");
            return client;
        }
        public static ServiceClient<UAD_Lookup_WS.Interface.ISubscriptionStatus> UAD_Lookup_SubscriptionStatusClient()
        {
            ServiceClient<UAD_Lookup_WS.Interface.ISubscriptionStatus> client = new ServiceClient<UAD_Lookup_WS.Interface.ISubscriptionStatus>("BasicHttpBinding_ISubscriptionStatus");
            return client;
        }
        public static ServiceClient<UAD_Lookup_WS.Interface.ISubscriptionStatusMatrix> UAD_Lookup_SubscriptionStatusMatrixClient()
        {
            ServiceClient<UAD_Lookup_WS.Interface.ISubscriptionStatusMatrix> client = new ServiceClient<UAD_Lookup_WS.Interface.ISubscriptionStatusMatrix>("BasicHttpBinding_ISubscriptionStatusMatrix");
            return client;
        }
        public static ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> UAD_Lookup_TransactionCodeClient()
        {
            ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> client = new ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode>("BasicHttpBinding_ITransactionCode");
            return client;
        }
        public static ServiceClient<UAD_Lookup_WS.Interface.ITransactionCodeType> UAD_Lookup_TransactionCodeTypeClient()
        {
            ServiceClient<UAD_Lookup_WS.Interface.ITransactionCodeType> client = new ServiceClient<UAD_Lookup_WS.Interface.ITransactionCodeType>("BasicHttpBinding_ITransactionCodeType");
            return client;
        }
        public static ServiceClient<UAD_Lookup_WS.Interface.IZipCode> UAD_Lookup_ZipCodeClient()
        {
            ServiceClient<UAD_Lookup_WS.Interface.IZipCode> client = new ServiceClient<UAD_Lookup_WS.Interface.IZipCode>("BasicHttpBinding_IZipCode");
            return client;
        }
        #endregion
        #region UAS Clients
        public static ServiceClient<UAS_WS.Interface.IAdHocDimension> UAS_AdHocDimensionClient()
        {
            ServiceClient<UAS_WS.Interface.IAdHocDimension> client = new ServiceClient<UAS_WS.Interface.IAdHocDimension>("BasicHttpBinding_IAdHocDimension");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IAdHocDimensionGroup> UAS_AdHocDimensionGroupClient()
        {
            ServiceClient<UAS_WS.Interface.IAdHocDimensionGroup> client = new ServiceClient<UAS_WS.Interface.IAdHocDimensionGroup>("BasicHttpBinding_IAdHocDimensionGroup");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IAdHocDimensionGroupPubcodeMap> UAS_AdHocDimensionGroupPubcodeMapClient()
        {
            ServiceClient<UAS_WS.Interface.IAdHocDimensionGroupPubcodeMap> client = new ServiceClient<UAS_WS.Interface.IAdHocDimensionGroupPubcodeMap>("BasicHttpBinding_IAdHocDimensionGroupPubcodeMap");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IAggregateDimension> UAS_AggregateDimensionClient()
        {
            ServiceClient<UAS_WS.Interface.IAggregateDimension> client = new ServiceClient<UAS_WS.Interface.IAggregateDimension>("BasicHttpBinding_IAggregateDimension");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IApi> UAS_ApiClient()
        {
            ServiceClient<UAS_WS.Interface.IApi> client = new ServiceClient<UAS_WS.Interface.IApi>("BasicHttpBinding_IApi");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IApiLog> UAS_ApiLogClient()
        {
            ServiceClient<UAS_WS.Interface.IApiLog> client = new ServiceClient<UAS_WS.Interface.IApiLog>("BasicHttpBinding_IApiLog");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IApplication> UAS_ApplicationClient()
        {
            ServiceClient<UAS_WS.Interface.IApplication> client = new ServiceClient<UAS_WS.Interface.IApplication>("BasicHttpBinding_IApplication");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IApplicationLog> UAS_ApplicationLogClient()
        {
            ServiceClient<UAS_WS.Interface.IApplicationLog> client = new ServiceClient<UAS_WS.Interface.IApplicationLog>("BasicHttpBinding_IApplicationLog");
            return client;
        }
        //public static ServiceClient<UAS_WS.Interface.IApplicationSecurityGroupMap> UAS_ApplicationSecurityGroupMapClient()
        //{
        //    ServiceClient<UAS_WS.Interface.IApplicationSecurityGroupMap> client = new ServiceClient<UAS_WS.Interface.IApplicationSecurityGroupMap>("BasicHttpBinding_IApplicationSecurityGroupMap");
        //    return client;
        //}
        public static ServiceClient<UAS_WS.Interface.IApplicationServiceMap> UAS_ApplicationServiceMapClient()
        {
            ServiceClient<UAS_WS.Interface.IApplicationServiceMap> client = new ServiceClient<UAS_WS.Interface.IApplicationServiceMap>("BasicHttpBinding_IApplicationServiceMap");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IClient> UAS_ClientClient()
        {
            ServiceClient<UAS_WS.Interface.IClient> client = new ServiceClient<UAS_WS.Interface.IClient>("BasicHttpBinding_IClient");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IClientCustomProcedure> UAS_ClientCustomProcedureClient()
        {
            ServiceClient<UAS_WS.Interface.IClientCustomProcedure> client = new ServiceClient<UAS_WS.Interface.IClientCustomProcedure>("BasicHttpBinding_IClientCustomProcedure");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IClientFTP> UAS_ClientFTPClient()
        {
            ServiceClient<UAS_WS.Interface.IClientFTP> client = new ServiceClient<UAS_WS.Interface.IClientFTP>("BasicHttpBinding_IClientFTP");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IClientGroup> UAS_ClientGroupClient()
        {
            ServiceClient<UAS_WS.Interface.IClientGroup> client = new ServiceClient<UAS_WS.Interface.IClientGroup>("BasicHttpBinding_IClientGroup");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IClientGroupClientMap> UAS_ClientGroupClientMapClient()
        {
            ServiceClient<UAS_WS.Interface.IClientGroupClientMap> client = new ServiceClient<UAS_WS.Interface.IClientGroupClientMap>("BasicHttpBinding_IClientGroupClientMap");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IClientGroupSecurityGroupMap> UAS_ClientGroupSecurityGroupMapClient()
        {
            ServiceClient<UAS_WS.Interface.IClientGroupSecurityGroupMap> client = new ServiceClient<UAS_WS.Interface.IClientGroupSecurityGroupMap>("BasicHttpBinding_IClientGroupSecurityGroupMap");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IClientGroupServiceFeatureMap> UAS_ClientGroupServiceFeatureMapClient()
        {
            ServiceClient<UAS_WS.Interface.IClientGroupServiceFeatureMap> client = new ServiceClient<UAS_WS.Interface.IClientGroupServiceFeatureMap>("BasicHttpBinding_IClientGroupServiceFeatureMap");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IClientGroupServiceMap> UAS_ClientGroupServiceMapClient()
        {
            ServiceClient<UAS_WS.Interface.IClientGroupServiceMap> client = new ServiceClient<UAS_WS.Interface.IClientGroupServiceMap>("BasicHttpBinding_IClientGroupServiceMap");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IClientGroupUserMap> UAS_ClientGroupUserMapClient()
        {
            ServiceClient<UAS_WS.Interface.IClientGroupUserMap> client = new ServiceClient<UAS_WS.Interface.IClientGroupUserMap>("BasicHttpBinding_IClientGroupUserMap");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IClientUADUsersMap> UAS_ClientUADUsersMapClient()
        {
            ServiceClient<UAS_WS.Interface.IClientUADUsersMap> client = new ServiceClient<UAS_WS.Interface.IClientUADUsersMap>("BasicHttpBinding_IClientUADUsersMap");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IDataCompareCostBase> UAS_DataCompareCostClient()
        {
            ServiceClient<UAS_WS.Interface.IDataCompareCostBase> client = new ServiceClient<UAS_WS.Interface.IDataCompareCostBase>("BasicHttpBinding_IDataCompareCost");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IDataCompareCostClient> UAS_DataCompareCostClientClient()
        {
            ServiceClient<UAS_WS.Interface.IDataCompareCostClient> client = new ServiceClient<UAS_WS.Interface.IDataCompareCostClient>("BasicHttpBinding_IDataCompareCostClient");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IDataCompareCostUser> UAS_DataCompareCostUserClient()
        {
            ServiceClient<UAS_WS.Interface.IDataCompareCostUser> client = new ServiceClient<UAS_WS.Interface.IDataCompareCostUser>("BasicHttpBinding_IDataCompareCostUser");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IDataMapping> UAS_DataMappingClient()
        {
            ServiceClient<UAS_WS.Interface.IDataMapping> client = new ServiceClient<UAS_WS.Interface.IDataMapping>("BasicHttpBinding_IDataMapping");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IDBWorker> UAS_DBWorkerClient()
        {
            ServiceClient<UAS_WS.Interface.IDBWorker> client = new ServiceClient<UAS_WS.Interface.IDBWorker>("BasicHttpBinding_IDBWorker");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IDQMQue> UAS_DQMQueClient()
        {
            ServiceClient<UAS_WS.Interface.IDQMQue> client = new ServiceClient<UAS_WS.Interface.IDQMQue>("BasicHttpBinding_IDQMQue");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IEncryption> UAS_EncryptionClient()
        {
            ServiceClient<UAS_WS.Interface.IEncryption> client = new ServiceClient<UAS_WS.Interface.IEncryption>("BasicHttpBinding_IEncryption");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IFieldMapping> UAS_FieldMappingClient()
        {
            ServiceClient<UAS_WS.Interface.IFieldMapping> client = new ServiceClient<UAS_WS.Interface.IFieldMapping>("BasicHttpBinding_IFieldMapping");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IFieldMultiMap> UAS_FieldMultiMapClient()
        {
            ServiceClient<UAS_WS.Interface.IFieldMultiMap> client = new ServiceClient<UAS_WS.Interface.IFieldMultiMap>("BasicHttpBinding_IFieldMultiMap");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IFileLog> UAS_FileLogClient()
        {
            ServiceClient<UAS_WS.Interface.IFileLog> client = new ServiceClient<UAS_WS.Interface.IFileLog>("BasicHttpBinding_IFileLog");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IFpsArchive> UAS_FpsArchiveClient()
        {
            ServiceClient<UAS_WS.Interface.IFpsArchive> client = new ServiceClient<UAS_WS.Interface.IFpsArchive>("BasicHttpBinding_IFpsArchive");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IFpsCustomRule> UAS_FpsCustomRuleClient()
        {
            ServiceClient<UAS_WS.Interface.IFpsCustomRule> client = new ServiceClient<UAS_WS.Interface.IFpsCustomRule>("BasicHttpBinding_IFpsCustomRule");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IFpsMap> UAS_FpsMapClient()
        {
            ServiceClient<UAS_WS.Interface.IFpsMap> client = new ServiceClient<UAS_WS.Interface.IFpsMap>("BasicHttpBinding_IFpsMapRule");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IFpsStandardRule> UAS_FpsStandardRuleClient()
        {
            ServiceClient<UAS_WS.Interface.IFpsStandardRule> client = new ServiceClient<UAS_WS.Interface.IFpsStandardRule>("BasicHttpBinding_IFpsStandardRule");
            return client;
        }
        //public static ServiceClient<UAS_WS.Interface.IFileStatus> UAS_FileStatusClient()
        //{
        //    ServiceClient<UAS_WS.Interface.IFileStatus> client = new ServiceClient<UAS_WS.Interface.IFileStatus>("BasicHttpBinding_IFileStatus");
        //    return client;
        //}
        public static ServiceClient<UAS_WS.Interface.IFilter> UAS_FilterClient()
        {
            ServiceClient<UAS_WS.Interface.IFilter> client = new ServiceClient<UAS_WS.Interface.IFilter>("BasicHttpBinding_IFilter");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IFilterDetail> UAS_FilterDetailClient()
        {
            ServiceClient<UAS_WS.Interface.IFilterDetail> client = new ServiceClient<UAS_WS.Interface.IFilterDetail>("BasicHttpBinding_IFilterDetail");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IFilterDetailSelectedValue> UAS_FilterDetailSelectedValueClient()
        {
            ServiceClient<UAS_WS.Interface.IFilterDetailSelectedValue> client = new ServiceClient<UAS_WS.Interface.IFilterDetailSelectedValue>("BasicHttpBinding_IFilterDetailSelectedValue");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IFilterExportField> UAS_FilterExportFieldClient()
        {
            ServiceClient<UAS_WS.Interface.IFilterExportField> client = new ServiceClient<UAS_WS.Interface.IFilterExportField>("BasicHttpBinding_IFilterExportField");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IFilterGroup> UAS_FilterGroupClient()
        {
            ServiceClient<UAS_WS.Interface.IFilterGroup> client = new ServiceClient<UAS_WS.Interface.IFilterGroup>("BasicHttpBinding_IFilterGroup");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IFilterSchedule> UAS_FilterScheduleClient()
        {
            ServiceClient<UAS_WS.Interface.IFilterSchedule> client = new ServiceClient<UAS_WS.Interface.IFilterSchedule>("BasicHttpBinding_IFilterSchedule");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IMenu> UAS_MenuClient()
        {
            ServiceClient<UAS_WS.Interface.IMenu> client = new ServiceClient<UAS_WS.Interface.IMenu>("BasicHttpBinding_IMenu");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IProfileClientMap> UAS_ProfileClientMapClient()
        {
            ServiceClient<UAS_WS.Interface.IProfileClientMap> client = new ServiceClient<UAS_WS.Interface.IProfileClientMap>("BasicHttpBinding_IProfileClientMap");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IRelationalPubCode> UAS_RelationalPubCodeClient()
        {
            ServiceClient<UAS_WS.Interface.IRelationalPubCode> client = new ServiceClient<UAS_WS.Interface.IRelationalPubCode>("BasicHttpBinding_IRelationalPubCode");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IReports> UAS_ReportsClient()
        {
            ServiceClient<UAS_WS.Interface.IReports> client = new ServiceClient<UAS_WS.Interface.IReports>("BasicHttpBinding_IReports");
            return client;
        }        
        public static ServiceClient<UAS_WS.Interface.ISecurityGroup> UAS_SecurityGroupClient()
        {
            ServiceClient<UAS_WS.Interface.ISecurityGroup> client = new ServiceClient<UAS_WS.Interface.ISecurityGroup>("BasicHttpBinding_ISecurityGroup");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.ISecurityGroupServiceMap> UAS_SecurityGroupServiceMapClient()
        {
            ServiceClient<UAS_WS.Interface.ISecurityGroupServiceMap> client = new ServiceClient<UAS_WS.Interface.ISecurityGroupServiceMap>("BasicHttpBinding_ISecurityGroupServiceMap");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IServerVariable> UAS_ServerVariableClient()
        {
            ServiceClient<UAS_WS.Interface.IServerVariable> client = new ServiceClient<UAS_WS.Interface.IServerVariable>("BasicHttpBinding_IServerVariable");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IService> UAS_ServiceClient()
        {
            ServiceClient<UAS_WS.Interface.IService> client = new ServiceClient<UAS_WS.Interface.IService>("BasicHttpBinding_IService");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IServiceFeature> UAS_ServiceFeatureClient()
        {
            ServiceClient<UAS_WS.Interface.IServiceFeature> client = new ServiceClient<UAS_WS.Interface.IServiceFeature>("BasicHttpBinding_IServiceFeature");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.ISourceFile> UAS_SourceFileClient()
        {
            ServiceClient<UAS_WS.Interface.ISourceFile> client = new ServiceClient<UAS_WS.Interface.ISourceFile>("BasicHttpBinding_ISourceFile");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.ISplitTransform> UAS_SplitTransformClient()
        {
            ServiceClient<UAS_WS.Interface.ISplitTransform> client = new ServiceClient<UAS_WS.Interface.ISplitTransform>("BasicHttpBinding_ISplitTransform");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.ITable> UAS_TableClient()
        {
            ServiceClient<UAS_WS.Interface.ITable> client = new ServiceClient<UAS_WS.Interface.ITable>("BasicHttpBinding_ITable");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.ITransformAssign> UAS_TransformAssignClient()
        {
            ServiceClient<UAS_WS.Interface.ITransformAssign> client = new ServiceClient<UAS_WS.Interface.ITransformAssign>("BasicHttpBinding_ITransformAssign");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.ITransformation> UAS_TransformationClient()
        {
            ServiceClient<UAS_WS.Interface.ITransformation> client = new ServiceClient<UAS_WS.Interface.ITransformation>("BasicHttpBinding_ITransformation");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.ITransformationDetail> UAS_TransformationDetailClient()
        {
            ServiceClient<UAS_WS.Interface.ITransformationDetail> client = new ServiceClient<UAS_WS.Interface.ITransformationDetail>("BasicHttpBinding_ITransformationDetail");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.ITransformationFieldMap> UAS_TransformationFieldMapClient()
        {
            ServiceClient<UAS_WS.Interface.ITransformationFieldMap> client = new ServiceClient<UAS_WS.Interface.ITransformationFieldMap>("BasicHttpBinding_ITransformationFieldMap");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.ITransformationFieldMultiMap> UAS_TransformationFieldMultiMapClient()
        {
            ServiceClient<UAS_WS.Interface.ITransformationFieldMultiMap> client = new ServiceClient<UAS_WS.Interface.ITransformationFieldMultiMap>("BasicHttpBinding_ITransformationFieldMultiMap");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.ITransformationPubMap> UAS_TransformationPubMapClient()
        {
            ServiceClient<UAS_WS.Interface.ITransformationPubMap> client = new ServiceClient<UAS_WS.Interface.ITransformationPubMap>("BasicHttpBinding_ITransformationPubMap");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.ITransformDataMap> UAS_TransformDataMapClient()
        {
            ServiceClient<UAS_WS.Interface.ITransformDataMap> client = new ServiceClient<UAS_WS.Interface.ITransformDataMap>("BasicHttpBinding_ITransformDataMap");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.ITransformJoin> UAS_TransformJoinClient()
        {
            ServiceClient<UAS_WS.Interface.ITransformJoin> client = new ServiceClient<UAS_WS.Interface.ITransformJoin>("BasicHttpBinding_ITransformJoin");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.ITransformSplit> UAS_TransformSplitClient()
        {
            ServiceClient<UAS_WS.Interface.ITransformSplit> client = new ServiceClient<UAS_WS.Interface.ITransformSplit>("BasicHttpBinding_ITransformSplit");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.ITransformSplitTrans> UAS_TransformSplitTransClient()
        {
            ServiceClient<UAS_WS.Interface.ITransformSplitTrans> client = new ServiceClient<UAS_WS.Interface.ITransformSplitTrans>("BasicHttpBinding_ITransformSplitTrans");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IUASBridgeECN> UAS_UASBridgeECNClient()
        {
            ServiceClient<UAS_WS.Interface.IUASBridgeECN> client = new ServiceClient<UAS_WS.Interface.IUASBridgeECN>("BasicHttpBinding_IUASBridgeECN");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IUser> UAS_UserClient()
        {
            ServiceClient<UAS_WS.Interface.IUser> client = new ServiceClient<UAS_WS.Interface.IUser>("BasicHttpBinding_IUser");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IUserAuthorization> UAS_UserAuthorizationClient()
        {
            ServiceClient<UAS_WS.Interface.IUserAuthorization> client = new ServiceClient<UAS_WS.Interface.IUserAuthorization>("BasicHttpBinding_IUserAuthorization");

            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IUserAuthorizationLog> UAS_UserAuthorizationLogClient()
        {
            ServiceClient<UAS_WS.Interface.IUserAuthorizationLog> client = new ServiceClient<UAS_WS.Interface.IUserAuthorizationLog>("BasicHttpBinding_IUserAuthorizationLog");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IUserClientSecurityGroupMap> UAS_UserClientSecurityGroupMapClient()
        {
            ServiceClient<UAS_WS.Interface.IUserClientSecurityGroupMap> client = new ServiceClient<UAS_WS.Interface.IUserClientSecurityGroupMap>("BasicHttpBinding_IUserClientSecurityGroupMap");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IUserLog> UAS_UserLogClient()
        {
            ServiceClient<UAS_WS.Interface.IUserLog> client = new ServiceClient<UAS_WS.Interface.IUserLog>("BasicHttpBinding_IUserLog");
            return client;
        }        
        #endregion

        #region UAD Clients
        public static ServiceClient<UAD_WS.Interface.IAdhoc> UAD_AdhocClient()
        {
            ServiceClient<UAD_WS.Interface.IAdhoc> client = new ServiceClient<UAD_WS.Interface.IAdhoc>("BasicHttpBinding_IAdhoc");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IAdhocCategory> UAD_AdhocCategoryClient()
        {
            ServiceClient<UAD_WS.Interface.IAdhocCategory> client = new ServiceClient<UAD_WS.Interface.IAdhocCategory>("BasicHttpBinding_IAdhocCategory");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IArchivePubSubscriptionsExtension> UAD_ArchivePubSubscriptionsExtensionClient()
        {
            ServiceClient<UAD_WS.Interface.IArchivePubSubscriptionsExtension> client = new ServiceClient<UAD_WS.Interface.IArchivePubSubscriptionsExtension>("BasicHttpBinding_IArchivePubSubscriptionsExtension");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IBrand> UAD_BrandClient()
        {
            ServiceClient<UAD_WS.Interface.IBrand> client = new ServiceClient<UAD_WS.Interface.IBrand>("BasicHttpBinding_IBrand");
            return client;
        }
        //public static ServiceClient<UAD_WS.Interface.IBrandProductMap> UAD_BrandProductMapClient()
        //{
        //    ServiceClient<UAD_WS.Interface.IBrandProductMap> client = new ServiceClient<UAD_WS.Interface.IBrandProductMap>("BasicHttpBinding_IBrandProductMap");
        //    return client;
        //}
        public static ServiceClient<UAD_WS.Interface.ICampaign> UAD_CampaignClient()
        {
            ServiceClient<UAD_WS.Interface.ICampaign> client = new ServiceClient<UAD_WS.Interface.ICampaign>("BasicHttpBinding_ICampaign");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ICodeSheet> UAD_CodeSheetClient()
        {
            ServiceClient<UAD_WS.Interface.ICodeSheet> client = new ServiceClient<UAD_WS.Interface.ICodeSheet>("BasicHttpBinding_ICodeSheet");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ICodeSheetMasterCodeSheetBridge> UAD_CodeSheetMasterCodeSheetBridgeClient()
        {
            ServiceClient<UAD_WS.Interface.ICodeSheetMasterCodeSheetBridge> client = new ServiceClient<UAD_WS.Interface.ICodeSheetMasterCodeSheetBridge>("BasicHttpBinding_ICodeSheetMasterCodeSheetBridge");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IConsensusDimension> UAD_ConsensusDimensionClient()
        {
            ServiceClient<UAD_WS.Interface.IConsensusDimension> client = new ServiceClient<UAD_WS.Interface.IConsensusDimension>("BasicHttpBinding_IConsensusDimension");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IDatabases> UAD_DatabasesClient()
        {
            ServiceClient<UAD_WS.Interface.IDatabases> client = new ServiceClient<UAD_WS.Interface.IDatabases>("BasicHttpBinding_IDatabases");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IEmailStatus> UAD_EmailStatusClient()
        {
            ServiceClient<UAD_WS.Interface.IEmailStatus> client = new ServiceClient<UAD_WS.Interface.IEmailStatus>("BasicHttpBinding_IEmailStatus");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IFileAudit> UAD_FileAuditClient()
        {
            ServiceClient<UAD_WS.Interface.IFileAudit> client = new ServiceClient<UAD_WS.Interface.IFileAudit>("BasicHttpBinding_IFileAudit");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IFileMappingColumn> UAD_FileMappingColumnClient()
        {
            ServiceClient<UAD_WS.Interface.IFileMappingColumn> client = new ServiceClient<UAD_WS.Interface.IFileMappingColumn>("BasicHttpBinding_IFileMappingColumn");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IFileValidator_ImportError> UAD_FileValidator_ImportErrorClient()
        {
            ServiceClient<UAD_WS.Interface.IFileValidator_ImportError> client = new ServiceClient<UAD_WS.Interface.IFileValidator_ImportError>("BasicHttpBinding_IFileValidator_ImportError");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IFileValidator_Transformed> UAD_FileValidator_TransformedClient()
        {
            ServiceClient<UAD_WS.Interface.IFileValidator_Transformed> client = new ServiceClient<UAD_WS.Interface.IFileValidator_Transformed>("BasicHttpBinding_IFileValidator_Transformed");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IFilter> UAD_FilterClient()
        {
            ServiceClient<UAD_WS.Interface.IFilter> client = new ServiceClient<UAD_WS.Interface.IFilter>("BasicHttpBinding_IUADFilter");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IFilterCategory> UAD_FilterCategoryClient()
        {
            ServiceClient<UAD_WS.Interface.IFilterCategory> client = new ServiceClient<UAD_WS.Interface.IFilterCategory>("BasicHttpBinding_IFilterCategory");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IImportError> UAD_ImportErrorClient()
        {
            ServiceClient<UAD_WS.Interface.IImportError> client = new ServiceClient<UAD_WS.Interface.IImportError>("BasicHttpBinding_IImportError");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IImportErrorSummary> UAD_ImportErrorSummaryClient()
        {
            ServiceClient<UAD_WS.Interface.IImportErrorSummary> client = new ServiceClient<UAD_WS.Interface.IImportErrorSummary>("BasicHttpBinding_IImportErrorSummary");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IImportSummary> UAD_ImportSummaryClient()
        {
            ServiceClient<UAD_WS.Interface.IImportSummary> client = new ServiceClient<UAD_WS.Interface.IImportSummary>("BasicHttpBinding_IImportSummary");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IImportVessel> UAD_ImportVesselClient()
        {
            ServiceClient<UAD_WS.Interface.IImportVessel> client = new ServiceClient<UAD_WS.Interface.IImportVessel>("BasicHttpBinding_IImportVessel");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IMarket> UAD_MarketClient()
        {
            ServiceClient<UAD_WS.Interface.IMarket> client = new ServiceClient<UAD_WS.Interface.IMarket>("BasicHttpBinding_IMarket");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IMasterCodeSheet> UAD_MasterCodeSheetClient()
        {
            ServiceClient<UAD_WS.Interface.IMasterCodeSheet> client = new ServiceClient<UAD_WS.Interface.IMasterCodeSheet>("BasicHttpBinding_IMasterCodeSheet");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IMasterData> UAD_MasterDataClient()
        {
            ServiceClient<UAD_WS.Interface.IMasterData> client = new ServiceClient<UAD_WS.Interface.IMasterData>("BasicHttpBinding_IMasterData");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IMasterGroup> UAD_MasterGroupClient()
        {
            ServiceClient<UAD_WS.Interface.IMasterGroup> client = new ServiceClient<UAD_WS.Interface.IMasterGroup>("BasicHttpBinding_IMasterGroup");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IOperations> UAD_OperationsClient()
        {
            ServiceClient<UAD_WS.Interface.IOperations> client = new ServiceClient<UAD_WS.Interface.IOperations>("BasicHttpBinding_IOperations");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IProduct> UAD_ProductClient()
        {
            ServiceClient<UAD_WS.Interface.IProduct> client = new ServiceClient<UAD_WS.Interface.IProduct>("BasicHttpBinding_IProduct");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IProductAudit> UAD_ProductAuditClient()
        {
            ServiceClient<UAD_WS.Interface.IProductAudit> client = new ServiceClient<UAD_WS.Interface.IProductAudit>("BasicHttpBinding_IProductAudit");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IProductGroup> UAD_ProductGroupClient()
        {
            ServiceClient<UAD_WS.Interface.IProductGroup> client = new ServiceClient<UAD_WS.Interface.IProductGroup>("BasicHttpBinding_IProductGroup");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IProductSubscription> UAD_ProductSubscriptionClient()
        {
            ServiceClient<UAD_WS.Interface.IProductSubscription> client = new ServiceClient<UAD_WS.Interface.IProductSubscription>("BasicHttpBinding_IProductSubscription");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IProductSubscriptionsExtension> UAD_ProductSubscriptionsExtensionClient()
        {
            ServiceClient<UAD_WS.Interface.IProductSubscriptionsExtension> client = new ServiceClient<UAD_WS.Interface.IProductSubscriptionsExtension>("BasicHttpBinding_IProductSubscriptionsExtension");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IProductTypes> UAD_ProductTypesClient()
        {
            ServiceClient<UAD_WS.Interface.IProductTypes> client = new ServiceClient<UAD_WS.Interface.IProductTypes>("BasicHttpBinding_IProductTypes");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IPubCodes> UAD_PubCodesClient()
        {
            ServiceClient<UAD_WS.Interface.IPubCodes> client = new ServiceClient<UAD_WS.Interface.IPubCodes>("BasicHttpBinding_IPubCodes");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IPubSubscriptionDetail> UAD_PubSubscriptionDetailClient()
        {
            ServiceClient<UAD_WS.Interface.IPubSubscriptionDetail> client = new ServiceClient<UAD_WS.Interface.IPubSubscriptionDetail>("BasicHttpBinding_IPubSubscriptionDetail");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IQuestionCategory> UAD_QuestionCategoryClient()
        {
            ServiceClient<UAD_WS.Interface.IQuestionCategory> client = new ServiceClient<UAD_WS.Interface.IQuestionCategory>("BasicHttpBinding_IQuestionCategory");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IReportGroups> UAD_ReportGroupsClient()
        {
            ServiceClient<UAD_WS.Interface.IReportGroups> client = new ServiceClient<UAD_WS.Interface.IReportGroups>("BasicHttpBinding_IReportGroups");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IReports> UAD_ReportsClient()
        {
            ServiceClient<UAD_WS.Interface.IReports> client = new ServiceClient<UAD_WS.Interface.IReports>("BasicHttpBinding_IReports");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IResponseGroup> UAD_ResponseGroupClient()
        {
            ServiceClient<UAD_WS.Interface.IResponseGroup> client = new ServiceClient<UAD_WS.Interface.IResponseGroup>("BasicHttpBinding_IResponseGroup");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISecurityGroupBrandMap> UAD_SecurityGroupBrandMapClient()
        {
            ServiceClient<UAD_WS.Interface.ISecurityGroupBrandMap> client = new ServiceClient<UAD_WS.Interface.ISecurityGroupBrandMap>("BasicHttpBinding_ISecurityGroupBrandMap");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISecurityGroupProductMap> UAD_SecurityGroupProductMapClient()
        {
            ServiceClient<UAD_WS.Interface.ISecurityGroupProductMap> client = new ServiceClient<UAD_WS.Interface.ISecurityGroupProductMap>("BasicHttpBinding_ISecurityGroupProductMap");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriberArchive> UAD_SubscriberArchiveClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriberArchive> client = new ServiceClient<UAD_WS.Interface.ISubscriberArchive>("BasicHttpBinding_ISubscriberArchive");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriberConsensusDemographic> UAD_SubscriberConsensusDemographicClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriberConsensusDemographic> client = new ServiceClient<UAD_WS.Interface.ISubscriberConsensusDemographic>("BasicHttpBinding_ISubscriberConsensusDemographic");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriberDemographicArchive> UAD_SubscriberDemographicArchiveClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriberDemographicArchive> client = new ServiceClient<UAD_WS.Interface.ISubscriberDemographicArchive>("BasicHttpBinding_ISubscriberDemographicArchive");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriberDemographicFinal> UAD_SubscriberDemographicFinalClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriberDemographicFinal> client = new ServiceClient<UAD_WS.Interface.ISubscriberDemographicFinal>("BasicHttpBinding_ISubscriberDemographicFinal");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriberDemographicInvalid> UAD_SubscriberDemographicInvalidClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriberDemographicInvalid> client = new ServiceClient<UAD_WS.Interface.ISubscriberDemographicInvalid>("BasicHttpBinding_ISubscriberDemographicInvalid");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriberDemographicOriginal> UAD_SubscriberDemographicOriginalClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriberDemographicOriginal> client = new ServiceClient<UAD_WS.Interface.ISubscriberDemographicOriginal>("BasicHttpBinding_ISubscriberDemographicOriginal");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriberDemographicTransformed> UAD_SubscriberDemographicTransformedClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriberDemographicTransformed> client = new ServiceClient<UAD_WS.Interface.ISubscriberDemographicTransformed>("BasicHttpBinding_ISubscriberDemographicTransformed");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriberFinal> UAD_SubscriberFinalClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriberFinal> client = new ServiceClient<UAD_WS.Interface.ISubscriberFinal>("BasicHttpBinding_ISubscriberFinal");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriberInvalid> UAD_SubscriberInvalidClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriberInvalid> client = new ServiceClient<UAD_WS.Interface.ISubscriberInvalid>("BasicHttpBinding_ISubscriberInvalid");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriberMasterValue> UAD_SubscriberMasterValueClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriberMasterValue> client = new ServiceClient<UAD_WS.Interface.ISubscriberMasterValue>("BasicHttpBinding_ISubscriberMasterValue");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriberOriginal> UAD_SubscriberOriginalClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriberOriginal> client = new ServiceClient<UAD_WS.Interface.ISubscriberOriginal>("BasicHttpBinding_ISubscriberOriginal");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriberProductDemographic> UAD_SubscriberProductDemographicClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriberProductDemographic> client = new ServiceClient<UAD_WS.Interface.ISubscriberProductDemographic>("BasicHttpBinding_ISubscriberProductDemographic");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriberTransformed> UAD_SubscriberTransformedClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriberTransformed> client = new ServiceClient<UAD_WS.Interface.ISubscriberTransformed>("BasicHttpBinding_ISubscriberTransformed");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscription> UAD_SubscriptionClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscription> client = new ServiceClient<UAD_WS.Interface.ISubscription>("BasicHttpBinding_IUADSubscription");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriptionDetail> UAD_SubscriptionDetailClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriptionDetail> client = new ServiceClient<UAD_WS.Interface.ISubscriptionDetail>("BasicHttpBinding_ISubscriptionDetail");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriptionsExtensionMapper> UAD_SubscriptionsExtensionMapperClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriptionsExtensionMapper> client = new ServiceClient<UAD_WS.Interface.ISubscriptionsExtensionMapper>("BasicHttpBinding_ISubscriptionsExtensionMapper");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriptionSearchResult> UAD_SubscriptionSearchResultClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriptionSearchResult> client = new ServiceClient<UAD_WS.Interface.ISubscriptionSearchResult>("BasicHttpBinding_ISubscriptionSearchResult");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISuppressed> UAD_SuppressedClient()
        {
            ServiceClient<UAD_WS.Interface.ISuppressed> client = new ServiceClient<UAD_WS.Interface.ISuppressed>("BasicHttpBinding_ISuppressed");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ITable> UAD_TableClient()
        {
            ServiceClient<UAD_WS.Interface.ITable> client = new ServiceClient<UAD_WS.Interface.ITable>("BasicHttpBinding_IUADTable");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IValidationResult> UAD_ValidationResultClient()
        {
            ServiceClient<UAD_WS.Interface.IValidationResult> client = new ServiceClient<UAD_WS.Interface.IValidationResult>("BasicHttpBinding_IValidationResult");
            return client;
        }
        #endregion

        #region Circulation Clients
        public static ServiceClient<UAD_WS.Interface.IAcsMailerInfo> UAD_AcsMailerInfoClient()
        {
            ServiceClient<UAD_WS.Interface.IAcsMailerInfo> client = new ServiceClient<UAD_WS.Interface.IAcsMailerInfo>("BasicHttpBinding_IAcsMailerInfo");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IAcsShippingDetail> UAD_AcsShippingDetailClient()
        {
            ServiceClient<UAD_WS.Interface.IAcsShippingDetail> client = new ServiceClient<UAD_WS.Interface.IAcsShippingDetail>("BasicHttpBinding_IAcsShippingDetail");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IActionBackUp> UAD_ActionBackUpClient()
        {
            ServiceClient<UAD_WS.Interface.IActionBackUp> client = new ServiceClient<UAD_WS.Interface.IActionBackUp>("BasicHttpBinding_IActionBackUp");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IBatch> UAD_BatchClient()
        {
            ServiceClient<UAD_WS.Interface.IBatch> client = new ServiceClient<UAD_WS.Interface.IBatch>("BasicHttpBinding_IBatch");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IBatchHistoryDetail> UAD_BatchHistoryDetailClient()
        {
            ServiceClient<UAD_WS.Interface.IBatchHistoryDetail> client = new ServiceClient<UAD_WS.Interface.IBatchHistoryDetail>("BasicHttpBinding_IBatchHistoryDetail");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ICircImportExport> UAD_CircImportExportClient()
        {
            ServiceClient<UAD_WS.Interface.ICircImportExport> client = new ServiceClient<UAD_WS.Interface.ICircImportExport>("BasicHttpBinding_ICircImportExport");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IDataImportExport> UAD_DataImportExportClient()
        {
            ServiceClient<UAD_WS.Interface.IDataImportExport> client = new ServiceClient<UAD_WS.Interface.IDataImportExport>("BasicHttpBinding_IDataImportExport");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IFinalizeBatch> UAD_FinalizeBatchClient()
        {
            ServiceClient<UAD_WS.Interface.IFinalizeBatch> client = new ServiceClient<UAD_WS.Interface.IFinalizeBatch>("BasicHttpBinding_IFinalizeBatch");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IFrequency> UAD_FrequencyClient()
        {
            ServiceClient<UAD_WS.Interface.IFrequency> client = new ServiceClient<UAD_WS.Interface.IFrequency>("BasicHttpBinding_IFrequency");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IHistory> UAD_HistoryClient()
        {
            ServiceClient<UAD_WS.Interface.IHistory> client = new ServiceClient<UAD_WS.Interface.IHistory>("BasicHttpBinding_IHistory");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IHistoryMarketingMap> UAD_HistoryMarketingMapClient()
        {
            ServiceClient<UAD_WS.Interface.IHistoryMarketingMap> client = new ServiceClient<UAD_WS.Interface.IHistoryMarketingMap>("BasicHttpBinding_IHistoryMarketingMap");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IHistoryPaid> UAD_HistoryPaidClient()
        {
            ServiceClient<UAD_WS.Interface.IHistoryPaid> client = new ServiceClient<UAD_WS.Interface.IHistoryPaid>("BasicHttpBinding_IHistoryPaid");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IHistoryPaidBillTo> UAD_HistoryPaidBillToClient()
        {
            ServiceClient<UAD_WS.Interface.IHistoryPaidBillTo> client = new ServiceClient<UAD_WS.Interface.IHistoryPaidBillTo>("BasicHttpBinding_IHistoryPaidBillTo");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IHistoryResponseMap> UAD_HistoryResponseMapClient()
        {
            ServiceClient<UAD_WS.Interface.IHistoryResponseMap> client = new ServiceClient<UAD_WS.Interface.IHistoryResponseMap>("BasicHttpBinding_IHistoryResponseMap");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IHistorySubscription> UAD_HistorySubscriptionClient()
        {
            ServiceClient<UAD_WS.Interface.IHistorySubscription> client = new ServiceClient<UAD_WS.Interface.IHistorySubscription>("BasicHttpBinding_IHistorySubscription");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IHistoryToHistoryMarketingMap> UAD_HistoryToHistoryMarketingMapClient()
        {
            ServiceClient<UAD_WS.Interface.IHistoryToHistoryMarketingMap> client = new ServiceClient<UAD_WS.Interface.IHistoryToHistoryMarketingMap>("BasicHttpBinding_IHistoryToHistoryMarketingMap");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IHistoryToUserLog> UAD_HistoryToUserLogClient()
        {
            ServiceClient<UAD_WS.Interface.IHistoryToUserLog> client = new ServiceClient<UAD_WS.Interface.IHistoryToUserLog>("BasicHttpBinding_IHistoryToUserLog");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IIssue> UAD_IssueClient()
        {
            ServiceClient<UAD_WS.Interface.IIssue> client = new ServiceClient<UAD_WS.Interface.IIssue>("BasicHttpBinding_IIssue");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IIssueArchiveProductSubscription> UAD_IssueArchiveProductSubscriptionClient()
        {
            ServiceClient<UAD_WS.Interface.IIssueArchiveProductSubscription> client = new ServiceClient<UAD_WS.Interface.IIssueArchiveProductSubscription>("BasicHttpBinding_IIssueArchiveProductSubscription");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IIssueArchiveProductSubscriptionDetail> UAD_IssueArchiveProductSubscriptionDetailClient()
        {
            ServiceClient<UAD_WS.Interface.IIssueArchiveProductSubscriptionDetail> client = new ServiceClient<UAD_WS.Interface.IIssueArchiveProductSubscriptionDetail>("BasicHttpBinding_IIssueArchiveProductSubscriptionDetail");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IIssueComp> UAD_IssueCompClient()
        {
            ServiceClient<UAD_WS.Interface.IIssueComp> client = new ServiceClient<UAD_WS.Interface.IIssueComp>("BasicHttpBinding_IIssueComp");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IIssueCompDetail> UAD_IssueCompDetailClient()
        {
            ServiceClient<UAD_WS.Interface.IIssueCompDetail> client = new ServiceClient<UAD_WS.Interface.IIssueCompDetail>("BasicHttpBinding_IIssueCompDetail");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IIssueCompError> UAD_IssueCompErrorClient()
        {
            ServiceClient<UAD_WS.Interface.IIssueCompError> client = new ServiceClient<UAD_WS.Interface.IIssueCompError>("BasicHttpBinding_IIssueCompError");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IIssueSplit> UAD_IssueSplitClient()
        {
            ServiceClient<UAD_WS.Interface.IIssueSplit> client = new ServiceClient<UAD_WS.Interface.IIssueSplit>("BasicHttpBinding_IIssueSplit");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IMarketingMap> UAD_MarketingMapClient()
        {
            ServiceClient<UAD_WS.Interface.IMarketingMap> client = new ServiceClient<UAD_WS.Interface.IMarketingMap>("BasicHttpBinding_IMarketingMap");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IPaidBillTo> UAD_PaidBillToClient()
        {
            ServiceClient<UAD_WS.Interface.IPaidBillTo> client = new ServiceClient<UAD_WS.Interface.IPaidBillTo>("BasicHttpBinding_IPaidBillTo");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IPriceCode> UAD_PriceCodeClient()
        {
            ServiceClient<UAD_WS.Interface.IPriceCode> client = new ServiceClient<UAD_WS.Interface.IPriceCode>("BasicHttpBinding_IPriceCode");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IProfile> UAS_ProfileClient()
        {
            ServiceClient<UAS_WS.Interface.IProfile> client = new ServiceClient<UAS_WS.Interface.IProfile>("BasicHttpBinding_IProfile");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IProspect> UAD_ProspectClient()
        {
            ServiceClient<UAD_WS.Interface.IProspect> client = new ServiceClient<UAD_WS.Interface.IProspect>("BasicHttpBinding_IProspect");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IPublicationReports> UAS_PublicationReportsClient()
        {
            ServiceClient<UAS_WS.Interface.IPublicationReports> client = new ServiceClient<UAS_WS.Interface.IPublicationReports>("BasicHttpBinding_IPublicationReports");
            return client;
        }
        public static ServiceClient<UAS_WS.Interface.IPublicationSequence> UAD_PublicationSequenceClient()
        {
            ServiceClient<UAS_WS.Interface.IPublicationSequence> client = new ServiceClient<UAS_WS.Interface.IPublicationSequence>("BasicHttpBinding_IPublicationSequence");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISearchData> UAD_SearchDataClient()
        {
            ServiceClient<UAD_WS.Interface.ISearchData> client = new ServiceClient<UAD_WS.Interface.ISearchData>("BasicHttpBinding_ISearchData");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriberAddKill> UAD_SubscriberAddKillClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriberAddKill> client = new ServiceClient<UAD_WS.Interface.ISubscriberAddKill>("BasicHttpBinding_ISubscriberAddKill");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriberMarketingMap> UAD_SubscriberMarketingMapClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriberMarketingMap> client = new ServiceClient<UAD_WS.Interface.ISubscriberMarketingMap>("BasicHttpBinding_ISubscriberMarketingMap");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.ISubscriptionPaid> UAD_SubscriptionPaidClient()
        {
            ServiceClient<UAD_WS.Interface.ISubscriptionPaid> client = new ServiceClient<UAD_WS.Interface.ISubscriptionPaid>("BasicHttpBinding_ISubscriptionPaid");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IWaveMailing> UAD_WaveMailingClient()
        {
            ServiceClient<UAD_WS.Interface.IWaveMailing> client = new ServiceClient<UAD_WS.Interface.IWaveMailing>("BasicHttpBinding_IWaveMailing");
            return client;
        }
        public static ServiceClient<UAD_WS.Interface.IWaveMailingDetail> UAD_WaveMailingDetailClient()
        {
            ServiceClient<UAD_WS.Interface.IWaveMailingDetail> client = new ServiceClient<UAD_WS.Interface.IWaveMailingDetail>("BasicHttpBinding_IWaveMailingDetail");
            return client;
        }
        #endregion

        #region SubGen
        public static ServiceClient<SubGen_WS.Interface.ISubGenUtils> SubGen_SubGenUtilsClient()
        {
            ServiceClient<SubGen_WS.Interface.ISubGenUtils> client = new ServiceClient<SubGen_WS.Interface.ISubGenUtils>("BasicHttpBinding_ISubGenUtils");
            return client;
        }
        #endregion
    }
}

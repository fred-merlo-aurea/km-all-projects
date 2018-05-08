using System;
using System.Linq;
using System.Web.Routing;
using System.ServiceModel.Activation;

namespace AudienceServicesHost
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes();
        }
        private void RegisterRoutes()
        {
            // Edit the base address of Service1 by replacing the "Service1" string below
            #region UAS services
            RouteTable.Routes.Add(new ServiceRoute("AdHocDimension", new WebServiceHostFactory(), typeof(UAS_WS.Service.AdHocDimension)));
            RouteTable.Routes.Add(new ServiceRoute("AdHocDimensionGroup", new WebServiceHostFactory(), typeof(UAS_WS.Service.AdHocDimensionGroup)));
            RouteTable.Routes.Add(new ServiceRoute("AdHocDimensionGroupPubcodeMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.AdHocDimensionGroupPubcodeMap)));
            RouteTable.Routes.Add(new ServiceRoute("AggregateDimension", new WebServiceHostFactory(), typeof(UAS_WS.Service.AggregateDimension)));
            RouteTable.Routes.Add(new ServiceRoute("Api", new WebServiceHostFactory(), typeof(UAS_WS.Service.Api)));
            RouteTable.Routes.Add(new ServiceRoute("ApiLog", new WebServiceHostFactory(), typeof(UAS_WS.Service.ApiLog)));
            RouteTable.Routes.Add(new ServiceRoute("Application", new WebServiceHostFactory(), typeof(UAS_WS.Service.Application)));
            RouteTable.Routes.Add(new ServiceRoute("ApplicationLog", new WebServiceHostFactory(), typeof(UAS_WS.Service.ApplicationLog)));
           // RouteTable.Routes.Add(new ServiceRoute("ApplicationSecurityGroupMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.ApplicationSecurityGroupMap)));
            RouteTable.Routes.Add(new ServiceRoute("ApplicationServiceMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.ApplicationServiceMap)));
            RouteTable.Routes.Add(new ServiceRoute("Client", new WebServiceHostFactory(), typeof(UAS_WS.Service.Client)));
            RouteTable.Routes.Add(new ServiceRoute("ClientCustomProcedure", new WebServiceHostFactory(), typeof(UAS_WS.Service.ClientCustomProcedure)));
            RouteTable.Routes.Add(new ServiceRoute("ClientFTP", new WebServiceHostFactory(), typeof(UAS_WS.Service.ClientFTP)));
            RouteTable.Routes.Add(new ServiceRoute("ClientGroup", new WebServiceHostFactory(), typeof(UAS_WS.Service.ClientGroup)));
            RouteTable.Routes.Add(new ServiceRoute("ClientGroupClientMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.ClientGroupClientMap)));
            RouteTable.Routes.Add(new ServiceRoute("ClientGroupSecurityGroupMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.ClientGroupSecurityGroupMap)));
            RouteTable.Routes.Add(new ServiceRoute("ClientGroupServiceFeatureMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.ClientGroupServiceFeatureMap)));
            RouteTable.Routes.Add(new ServiceRoute("ClientGroupServiceMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.ClientGroupServiceMap)));
            RouteTable.Routes.Add(new ServiceRoute("ClientUADUsersMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.ClientUADUsersMap)));
            RouteTable.Routes.Add(new ServiceRoute("DataCompareCostBase", new WebServiceHostFactory(), typeof(UAS_WS.Service.DataCompareCostBase)));
            RouteTable.Routes.Add(new ServiceRoute("DataCompareCostClient", new WebServiceHostFactory(), typeof(UAS_WS.Service.DataCompareCostClient)));
            RouteTable.Routes.Add(new ServiceRoute("DataCompareCostUser", new WebServiceHostFactory(), typeof(UAS_WS.Service.DataCompareCostUser)));
            RouteTable.Routes.Add(new ServiceRoute("DataMapping", new WebServiceHostFactory(), typeof(UAS_WS.Service.DataMapping)));
            RouteTable.Routes.Add(new ServiceRoute("DBWorker", new WebServiceHostFactory(), typeof(UAS_WS.Service.DBWorker)));
            RouteTable.Routes.Add(new ServiceRoute("DQMQue", new WebServiceHostFactory(), typeof(UAS_WS.Service.DQMQue)));
            RouteTable.Routes.Add(new ServiceRoute("Encryption", new WebServiceHostFactory(), typeof(UAS_WS.Service.Encryption)));
            RouteTable.Routes.Add(new ServiceRoute("FieldMapping", new WebServiceHostFactory(), typeof(UAS_WS.Service.FieldMapping)));
            RouteTable.Routes.Add(new ServiceRoute("FieldMultiMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.FieldMultiMap)));
            RouteTable.Routes.Add(new ServiceRoute("FileLog", new WebServiceHostFactory(), typeof(UAS_WS.Service.FileLog)));
            //RouteTable.Routes.Add(new ServiceRoute("FileRule", new WebServiceHostFactory(), typeof(UAS_WS.Service.FileRule)));
            //RouteTable.Routes.Add(new ServiceRoute("FileRuleMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.FileRuleMap)));
            RouteTable.Routes.Add(new ServiceRoute("FpsArchive", new WebServiceHostFactory(), typeof(UAS_WS.Service.FpsArchive)));
            RouteTable.Routes.Add(new ServiceRoute("FpsCustomRule", new WebServiceHostFactory(), typeof(UAS_WS.Service.FpsCustomRule)));
            RouteTable.Routes.Add(new ServiceRoute("FpsMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.FpsMap)));
            RouteTable.Routes.Add(new ServiceRoute("FpsStandardRule", new WebServiceHostFactory(), typeof(UAS_WS.Service.FpsStandardRule)));
            RouteTable.Routes.Add(new ServiceRoute("Filter", new WebServiceHostFactory(), typeof(UAS_WS.Service.Filter)));
            RouteTable.Routes.Add(new ServiceRoute("FilterDetail", new WebServiceHostFactory(), typeof(UAS_WS.Service.FilterDetail)));
            RouteTable.Routes.Add(new ServiceRoute("FilterDetailSelectedValue", new WebServiceHostFactory(), typeof(UAS_WS.Service.FilterDetailSelectedValue)));
            RouteTable.Routes.Add(new ServiceRoute("FilterExportField", new WebServiceHostFactory(), typeof(UAS_WS.Service.FilterExportField)));
            RouteTable.Routes.Add(new ServiceRoute("FilterGroup", new WebServiceHostFactory(), typeof(UAS_WS.Service.FilterGroup)));
            RouteTable.Routes.Add(new ServiceRoute("FilterSchedule", new WebServiceHostFactory(), typeof(UAS_WS.Service.FilterSchedule)));
            RouteTable.Routes.Add(new ServiceRoute("Menu", new WebServiceHostFactory(), typeof(UAS_WS.Service.Menu)));
            RouteTable.Routes.Add(new ServiceRoute("Profile", new WebServiceHostFactory(), typeof(UAS_WS.Service.Profile)));
            RouteTable.Routes.Add(new ServiceRoute("ProfileClientMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.ProfileClientMap)));
            RouteTable.Routes.Add(new ServiceRoute("RelationalPubCode", new WebServiceHostFactory(), typeof(UAS_WS.Service.RelationalPubCode)));
            RouteTable.Routes.Add(new ServiceRoute("UAS_Reports", new WebServiceHostFactory(), typeof(UAS_WS.Service.Reports)));            
            RouteTable.Routes.Add(new ServiceRoute("SecurityGroup", new WebServiceHostFactory(), typeof(UAS_WS.Service.SecurityGroup)));
            RouteTable.Routes.Add(new ServiceRoute("SecurityGroupServiceMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.SecurityGroupServiceMap)));
            RouteTable.Routes.Add(new ServiceRoute("ServerVariable", new WebServiceHostFactory(), typeof(UAS_WS.Service.ServerVariable)));
            RouteTable.Routes.Add(new ServiceRoute("Service", new WebServiceHostFactory(), typeof(UAS_WS.Service.Service)));
            RouteTable.Routes.Add(new ServiceRoute("ServiceFeature", new WebServiceHostFactory(), typeof(UAS_WS.Service.ServiceFeature)));
            RouteTable.Routes.Add(new ServiceRoute("SourceFile", new WebServiceHostFactory(), typeof(UAS_WS.Service.SourceFile)));
            RouteTable.Routes.Add(new ServiceRoute("SplitTransform", new WebServiceHostFactory(), typeof(UAS_WS.Service.SplitTransform)));
            RouteTable.Routes.Add(new ServiceRoute("Table", new WebServiceHostFactory(), typeof(UAS_WS.Service.Table)));
            RouteTable.Routes.Add(new ServiceRoute("TransformAssign", new WebServiceHostFactory(), typeof(UAS_WS.Service.TransformAssign)));
            RouteTable.Routes.Add(new ServiceRoute("Transformation", new WebServiceHostFactory(), typeof(UAS_WS.Service.Transformation)));
            RouteTable.Routes.Add(new ServiceRoute("TransformationDetail", new WebServiceHostFactory(), typeof(UAS_WS.Service.TransformationDetail)));
            RouteTable.Routes.Add(new ServiceRoute("TransformationFieldMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.TransformationFieldMap)));
            RouteTable.Routes.Add(new ServiceRoute("TransformationFieldMultiMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.TransformationFieldMultiMap)));
            RouteTable.Routes.Add(new ServiceRoute("TransformationPubMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.TransformationPubMap)));
            RouteTable.Routes.Add(new ServiceRoute("TransformDataMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.TransformDataMap)));
            RouteTable.Routes.Add(new ServiceRoute("TransformJoin", new WebServiceHostFactory(), typeof(UAS_WS.Service.TransformJoin)));
            RouteTable.Routes.Add(new ServiceRoute("TransformSplit", new WebServiceHostFactory(), typeof(UAS_WS.Service.TransformSplit)));
            RouteTable.Routes.Add(new ServiceRoute("TransformSplitTrans", new WebServiceHostFactory(), typeof(UAS_WS.Service.TransformSplitTrans)));
            RouteTable.Routes.Add(new ServiceRoute("UASBridgeECN", new WebServiceHostFactory(), typeof(UAS_WS.Service.UASBridgeECN)));
            RouteTable.Routes.Add(new ServiceRoute("User", new WebServiceHostFactory(), typeof(UAS_WS.Service.User)));
            RouteTable.Routes.Add(new ServiceRoute("UserAuthorization", new WebServiceHostFactory(), typeof(UAS_WS.Service.UserAuthorization)));
            RouteTable.Routes.Add(new ServiceRoute("UserAuthorizationLog", new WebServiceHostFactory(), typeof(UAS_WS.Service.UserAuthorizationLog)));
            RouteTable.Routes.Add(new ServiceRoute("UserClientSecurityGroupMap", new WebServiceHostFactory(), typeof(UAS_WS.Service.UserClientSecurityGroupMap)));
            RouteTable.Routes.Add(new ServiceRoute("UserLog", new WebServiceHostFactory(), typeof(UAS_WS.Service.UserLog)));

            #endregion

            #region Circulation services 
            RouteTable.Routes.Add(new ServiceRoute("AcsMailerInfo", new WebServiceHostFactory(), typeof(UAD_WS.Service.AcsMailerInfo)));
            RouteTable.Routes.Add(new ServiceRoute("AcsShippingDetail", new WebServiceHostFactory(), typeof(UAD_WS.Service.AcsShippingDetail)));
            RouteTable.Routes.Add(new ServiceRoute("ActionBackUp", new WebServiceHostFactory(), typeof(UAD_WS.Service.ActionBackUp)));
            RouteTable.Routes.Add(new ServiceRoute("Batch", new WebServiceHostFactory(), typeof(UAD_WS.Service.Batch)));
            RouteTable.Routes.Add(new ServiceRoute("BatchHistoryDetail", new WebServiceHostFactory(), typeof(UAD_WS.Service.BatchHistoryDetail)));
            RouteTable.Routes.Add(new ServiceRoute("DataImportExport", new WebServiceHostFactory(), typeof(UAD_WS.Service.DataImportExport)));
            RouteTable.Routes.Add(new ServiceRoute("FinalizeBatch", new WebServiceHostFactory(), typeof(UAD_WS.Service.FinalizeBatch)));
            RouteTable.Routes.Add(new ServiceRoute("Frequency", new WebServiceHostFactory(), typeof(UAD_WS.Service.Frequency)));
            RouteTable.Routes.Add(new ServiceRoute("History", new WebServiceHostFactory(), typeof(UAD_WS.Service.History)));
            RouteTable.Routes.Add(new ServiceRoute("HistoryMarketingMap", new WebServiceHostFactory(), typeof(UAD_WS.Service.HistoryMarketingMap)));
            RouteTable.Routes.Add(new ServiceRoute("HistoryPaid", new WebServiceHostFactory(), typeof(UAD_WS.Service.HistoryPaid)));
            RouteTable.Routes.Add(new ServiceRoute("HistoryPaidBillTo", new WebServiceHostFactory(), typeof(UAD_WS.Service.HistoryPaidBillTo)));
            RouteTable.Routes.Add(new ServiceRoute("HistoryResponseMap", new WebServiceHostFactory(), typeof(UAD_WS.Service.HistoryResponseMap)));
            RouteTable.Routes.Add(new ServiceRoute("HistorySubscription", new WebServiceHostFactory(), typeof(UAD_WS.Service.HistorySubscription)));
            RouteTable.Routes.Add(new ServiceRoute("Issue", new WebServiceHostFactory(), typeof(UAD_WS.Service.Issue)));
            RouteTable.Routes.Add(new ServiceRoute("IssueArchiveProductSubscription", new WebServiceHostFactory(), typeof(UAD_WS.Service.IssueArchiveProductSubscription)));
            RouteTable.Routes.Add(new ServiceRoute("IssueArchiveProductSubscriptionDetail", new WebServiceHostFactory(), typeof(UAD_WS.Service.IssueArchiveProductSubscriptionDetail)));
            RouteTable.Routes.Add(new ServiceRoute("IssueComp", new WebServiceHostFactory(), typeof(UAD_WS.Service.IssueComp)));
            RouteTable.Routes.Add(new ServiceRoute("IssueCompDetail", new WebServiceHostFactory(), typeof(UAD_WS.Service.IssueCompDetail)));
            RouteTable.Routes.Add(new ServiceRoute("IssueCompError", new WebServiceHostFactory(), typeof(UAD_WS.Service.IssueCompError)));
            RouteTable.Routes.Add(new ServiceRoute("IssueSplit", new WebServiceHostFactory(), typeof(UAD_WS.Service.IssueSplit)));
            RouteTable.Routes.Add(new ServiceRoute("PaidBillTo", new WebServiceHostFactory(), typeof(UAD_WS.Service.PaidBillTo)));
            RouteTable.Routes.Add(new ServiceRoute("PriceCode", new WebServiceHostFactory(), typeof(UAD_WS.Service.PriceCode)));
            RouteTable.Routes.Add(new ServiceRoute("Prospect", new WebServiceHostFactory(), typeof(UAD_WS.Service.Prospect)));
            RouteTable.Routes.Add(new ServiceRoute("PublicationReports", new WebServiceHostFactory(), typeof(UAS_WS.Service.PublicationReports)));
            RouteTable.Routes.Add(new ServiceRoute("PublicationSequence", new WebServiceHostFactory(), typeof(UAS_WS.Service.PublicationSequence)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriberAddKill", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriberAddKill)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriberMarketingMap", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriberMarketingMap)));
            RouteTable.Routes.Add(new ServiceRoute("Subscription", new WebServiceHostFactory(), typeof(UAD_WS.Service.Subscription)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriptionPaid", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriptionPaid)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriptionSearchResult", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriptionSearchResult)));
            RouteTable.Routes.Add(new ServiceRoute("WaveMailing", new WebServiceHostFactory(), typeof(UAD_WS.Service.WaveMailing)));
            RouteTable.Routes.Add(new ServiceRoute("WaveMailingDetail", new WebServiceHostFactory(), typeof(UAD_WS.Service.WaveMailingDetail)));
            #endregion

            #region UAD services
            RouteTable.Routes.Add(new ServiceRoute("Adhoc", new WebServiceHostFactory(), typeof(UAD_WS.Service.Adhoc)));
            RouteTable.Routes.Add(new ServiceRoute("AdhocCategory", new WebServiceHostFactory(), typeof(UAD_WS.Service.AdhocCategory)));
            RouteTable.Routes.Add(new ServiceRoute("ArchivePubSubscriptionsExtension", new WebServiceHostFactory(), typeof(UAD_WS.Service.ArchivePubSubscriptionsExtension)));
            RouteTable.Routes.Add(new ServiceRoute("Brand", new WebServiceHostFactory(), typeof(UAD_WS.Service.Brand)));
            //RouteTable.Routes.Add(new ServiceRoute("BrandProductMap", new WebServiceHostFactory(), typeof(UAD_WS.Service.BrandProductMap)));
            RouteTable.Routes.Add(new ServiceRoute("Campaign", new WebServiceHostFactory(), typeof(UAD_WS.Service.Campaign)));
            RouteTable.Routes.Add(new ServiceRoute("CodeSheet", new WebServiceHostFactory(), typeof(UAD_WS.Service.CodeSheet)));
            RouteTable.Routes.Add(new ServiceRoute("CodeSheetMasterCodeSheetBridge", new WebServiceHostFactory(), typeof(UAD_WS.Service.CodeSheetMasterCodeSheetBridge)));
            RouteTable.Routes.Add(new ServiceRoute("ConsensusDimension", new WebServiceHostFactory(), typeof(UAD_WS.Service.ConsensusDimension)));
            RouteTable.Routes.Add(new ServiceRoute("Databases", new WebServiceHostFactory(), typeof(UAD_WS.Service.Databases)));
            RouteTable.Routes.Add(new ServiceRoute("EmailStatus", new WebServiceHostFactory(), typeof(UAD_WS.Service.EmailStatus)));
            RouteTable.Routes.Add(new ServiceRoute("FileAudit", new WebServiceHostFactory(), typeof(UAD_WS.Service.FileAudit)));
            RouteTable.Routes.Add(new ServiceRoute("FileMappingColumn", new WebServiceHostFactory(), typeof(UAD_WS.Service.FileAudit)));
            RouteTable.Routes.Add(new ServiceRoute("FileValidator_ImportError", new WebServiceHostFactory(), typeof(UAD_WS.Service.FileValidator_ImportError)));
            RouteTable.Routes.Add(new ServiceRoute("FileValidator_Transformed", new WebServiceHostFactory(), typeof(UAD_WS.Service.FileValidator_Transformed)));
            RouteTable.Routes.Add(new ServiceRoute("UADFilter", new WebServiceHostFactory(), typeof(UAD_WS.Service.Filter)));
            RouteTable.Routes.Add(new ServiceRoute("FilterCategory", new WebServiceHostFactory(), typeof(UAD_WS.Service.FilterCategory)));
            RouteTable.Routes.Add(new ServiceRoute("ImportError", new WebServiceHostFactory(), typeof(UAD_WS.Service.ImportError)));
            RouteTable.Routes.Add(new ServiceRoute("ImportErrorSummary", new WebServiceHostFactory(), typeof(UAD_WS.Service.ImportErrorSummary)));
            RouteTable.Routes.Add(new ServiceRoute("ImportSummary", new WebServiceHostFactory(), typeof(UAD_WS.Service.ImportSummary)));
            RouteTable.Routes.Add(new ServiceRoute("ImportVessel", new WebServiceHostFactory(), typeof(UAD_WS.Service.ImportVessel)));
            RouteTable.Routes.Add(new ServiceRoute("Market", new WebServiceHostFactory(), typeof(UAD_WS.Service.Market)));
            RouteTable.Routes.Add(new ServiceRoute("MarketingMap", new WebServiceHostFactory(), typeof(UAD_WS.Service.MarketingMap)));
            RouteTable.Routes.Add(new ServiceRoute("MasterCodeSheet", new WebServiceHostFactory(), typeof(UAD_WS.Service.MasterCodeSheet)));
            RouteTable.Routes.Add(new ServiceRoute("MasterData", new WebServiceHostFactory(), typeof(UAD_WS.Service.MasterData)));
            RouteTable.Routes.Add(new ServiceRoute("MasterGroup", new WebServiceHostFactory(), typeof(UAD_WS.Service.MasterGroup)));
            RouteTable.Routes.Add(new ServiceRoute("Operations", new WebServiceHostFactory(), typeof(UAD_WS.Service.Operations)));
            RouteTable.Routes.Add(new ServiceRoute("PaidOrder", new WebServiceHostFactory(), typeof(UAD_WS.Service.PaidOrder)));
            RouteTable.Routes.Add(new ServiceRoute("PaidOrderDetail", new WebServiceHostFactory(), typeof(UAD_WS.Service.PaidOrderDetail)));
            RouteTable.Routes.Add(new ServiceRoute("Product", new WebServiceHostFactory(), typeof(UAD_WS.Service.Product)));
            RouteTable.Routes.Add(new ServiceRoute("ProductAudit", new WebServiceHostFactory(), typeof(UAD_WS.Service.ProductAudit)));
            RouteTable.Routes.Add(new ServiceRoute("ProductGroup", new WebServiceHostFactory(), typeof(UAD_WS.Service.ProductGroup)));
            RouteTable.Routes.Add(new ServiceRoute("ProductSubscription", new WebServiceHostFactory(), typeof(UAD_WS.Service.ProductSubscription)));
            RouteTable.Routes.Add(new ServiceRoute("ProductSubscriptionsExtension", new WebServiceHostFactory(), typeof(UAD_WS.Service.ProductSubscriptionsExtension)));
            RouteTable.Routes.Add(new ServiceRoute("ProductTypes", new WebServiceHostFactory(), typeof(UAD_WS.Service.ProductTypes)));
            RouteTable.Routes.Add(new ServiceRoute("PubCodes", new WebServiceHostFactory(), typeof(UAD_WS.Service.PubCodes)));
            RouteTable.Routes.Add(new ServiceRoute("PubSubscriptionDetail", new WebServiceHostFactory(), typeof(UAD_WS.Service.PubSubscriptionDetail)));
            RouteTable.Routes.Add(new ServiceRoute("QuestionCategory", new WebServiceHostFactory(), typeof(UAD_WS.Service.QuestionCategory)));
            RouteTable.Routes.Add(new ServiceRoute("ReportGroups", new WebServiceHostFactory(), typeof(UAD_WS.Service.ReportGroups)));
            RouteTable.Routes.Add(new ServiceRoute("Circ_Reports", new WebServiceHostFactory(), typeof(UAD_WS.Service.Reports)));
            RouteTable.Routes.Add(new ServiceRoute("ResponseGroup", new WebServiceHostFactory(), typeof(UAD_WS.Service.ResponseGroup)));
            RouteTable.Routes.Add(new ServiceRoute("SecurityGroupBrandMap", new WebServiceHostFactory(), typeof(UAD_WS.Service.SecurityGroupBrandMap)));
            RouteTable.Routes.Add(new ServiceRoute("SecurityGroupProductMap", new WebServiceHostFactory(), typeof(UAD_WS.Service.SecurityGroupProductMap)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriberArchive", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriberArchive)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriberDemographicArchive", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriberDemographicArchive)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriberDemographicFinal", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriberDemographicFinal)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriberDemographicInvalid", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriberDemographicInvalid)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriberDemographicOriginal", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriberDemographicOriginal)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriberDemographicTransformed", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriberDemographicTransformed)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriberFinal", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriberFinal)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriberInvalid", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriberInvalid)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriberMasterValue", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriberMasterValue)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriberOriginal", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriberOriginal)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriberProductDemographic", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriberProductDemographic)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriberTransformed", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriberTransformed)));
            RouteTable.Routes.Add(new ServiceRoute("UADSubscription", new WebServiceHostFactory(), typeof(UAD_WS.Service.Subscription)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriptionDetail", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriptionDetail)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriptionsExtensionMapper", new WebServiceHostFactory(), typeof(UAD_WS.Service.SubscriptionsExtensionMapper)));
            RouteTable.Routes.Add(new ServiceRoute("Suppressed", new WebServiceHostFactory(), typeof(UAD_WS.Service.Suppressed)));
            RouteTable.Routes.Add(new ServiceRoute("UADTable", new WebServiceHostFactory(), typeof(UAD_WS.Service.Table)));
            RouteTable.Routes.Add(new ServiceRoute("ValidationResult", new WebServiceHostFactory(), typeof(UAD_WS.Service.ValidationResult)));
            #endregion

            #region UAD_Lookup services
            RouteTable.Routes.Add(new ServiceRoute("Action", new WebServiceHostFactory(), typeof(UAD_Lookup_WS.Service.Action)));
            RouteTable.Routes.Add(new ServiceRoute("CategoryCode", new WebServiceHostFactory(), typeof(UAD_Lookup_WS.Service.CategoryCode)));
            RouteTable.Routes.Add(new ServiceRoute("CategoryCodeType", new WebServiceHostFactory(), typeof(UAD_Lookup_WS.Service.CategoryCodeType)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriptionStatus", new WebServiceHostFactory(), typeof(UAD_Lookup_WS.Service.SubscriptionStatus)));
            RouteTable.Routes.Add(new ServiceRoute("SubscriptionStatusMatrix", new WebServiceHostFactory(), typeof(UAD_Lookup_WS.Service.SubscriptionStatusMatrix)));
            RouteTable.Routes.Add(new ServiceRoute("TransactionCode", new WebServiceHostFactory(), typeof(UAD_Lookup_WS.Service.TransactionCode)));
            RouteTable.Routes.Add(new ServiceRoute("TransactionCodeType", new WebServiceHostFactory(), typeof(UAD_Lookup_WS.Service.TransactionCodeType)));
            RouteTable.Routes.Add(new ServiceRoute("Code", new WebServiceHostFactory(), typeof(UAD_Lookup_WS.Service.Code)));
            RouteTable.Routes.Add(new ServiceRoute("CodeType", new WebServiceHostFactory(), typeof(UAD_Lookup_WS.Service.CodeType)));
            RouteTable.Routes.Add(new ServiceRoute("Country", new WebServiceHostFactory(), typeof(UAD_Lookup_WS.Service.Country)));
            RouteTable.Routes.Add(new ServiceRoute("CountryMap", new WebServiceHostFactory(), typeof(UAD_Lookup_WS.Service.CountryMap)));
            RouteTable.Routes.Add(new ServiceRoute("Region", new WebServiceHostFactory(), typeof(UAD_Lookup_WS.Service.Region)));
            RouteTable.Routes.Add(new ServiceRoute("RegionGroup", new WebServiceHostFactory(), typeof(UAD_Lookup_WS.Service.RegionGroup)));
            RouteTable.Routes.Add(new ServiceRoute("RegionMap", new WebServiceHostFactory(), typeof(UAD_Lookup_WS.Service.RegionMap)));
            RouteTable.Routes.Add(new ServiceRoute("ZipCode", new WebServiceHostFactory(), typeof(UAD_Lookup_WS.Service.ZipCode)));
            #endregion

            #region SubGen
            RouteTable.Routes.Add(new ServiceRoute("SubGenUtils", new WebServiceHostFactory(), typeof(SubGen_WS.Service.SubGenUtils)));
            #endregion
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
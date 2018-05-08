using System.Collections.Generic;
using ecn.webservice.Facades.Params;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using KMPlatform.Entity;

namespace ecn.webservice.Facades
{
    public interface IBlastFacade
    {
        IBlastManager BlastsManager { get; set; }
        ICampaignItemBlastManager CampaignItemBlastsManager { get; set; }
        ICampaignItemTestBlastManager CampaignItemTestBlastManager { get; set; }
        ICampaignItemBlastRefBlastManager CampaignItemBlastRefBlastManager { get; set; }
        string GetSubscriberCount(WebMethodExecutionContext context, Dictionary<string, object> parameters);
        string AddBlast(WebMethodExecutionContext context, AddBlastParams parameters);
        string AddScheduledBlast(WebMethodExecutionContext context, Dictionary<string, object> parameters);
        string UpdateBlast(WebMethodExecutionContext context, UpdateBlastParams parameters);
        string GetBlast(WebMethodExecutionContext context, Dictionary<string, object> parameters);
        string SearchForBlasts(WebMethodExecutionContext context, string XMLSearch);
        string DeleteBlast(WebMethodExecutionContext context, int blastId);
        string GetBlastReport(WebMethodExecutionContext context, int blastId);
        string GetBlastReportByISP(WebMethodExecutionContext context, GetBlastReportByISPParams parameters);
        string GetBlastOpensReport(WebMethodExecutionContext context, GetBlastReportParams parameters);
        string GetBlastClicksReport(WebMethodExecutionContext context, GetBlastReportParams parameters);
        string GetBlastBounceReport(WebMethodExecutionContext context, GetBlastReportParams parameters);
        string GetBlastUnsubscribeReport(WebMethodExecutionContext context, GetBlastReportParams parameters);
        string GetBlastDeliveryReport(WebMethodExecutionContext context, GetBlastDeliveryReportParams parameters);
        string BuildBlastReturnXML(IList<ECN_Framework_Entities.Communicator.BlastAbstract> blastList, User user);
    }
}
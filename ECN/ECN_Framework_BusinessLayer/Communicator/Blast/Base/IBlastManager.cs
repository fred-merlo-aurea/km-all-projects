using System;
using System.Collections.Generic;
using System.Data;
using KMPlatform.Entity;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public interface IBlastManager
    {
        CommunicatorEntities.BlastAbstract GetByBlastId(int blastId, User user, bool getChildren);

        DataTable GetBlastEmailListForDynamicContent(
            int customerId,
            int blastId,
            int groupId,
            List<CommunicatorEntities.CampaignItemBlastFilter> filters,
            string blastIdBounceDomain,
            string actionType,
            string suppressionList,
            bool onlyCounts,
            bool logSuppressed);

        IList<CommunicatorEntities.BlastAbstract> GetBySearch(
            int customerId,
            string emailSubject,
            int? userId,
            int? groupId,
            bool? isTest,
            string statusCode,
            DateTime? modifiedFrom,
            DateTime? modifiedTo,
            int? campaignId,
            string campaignName,
            string campaignItemName,
            User user,
            bool getChildren);
    }
}
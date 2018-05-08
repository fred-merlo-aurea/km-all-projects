using System;
using System.Collections.Generic;
using System.Data;
using KMPlatform.Entity;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class BlastManager : IBlastManager
    {
        public CommunicatorEntities.BlastAbstract GetByBlastId(int blastId, User user, bool getChildren)
        {
            return Blast.GetByBlastID(blastId, user, getChildren);
        }

        public DataTable GetBlastEmailListForDynamicContent(
            int customerId,
            int blastId,
            int groupId,
            List<CommunicatorEntities.CampaignItemBlastFilter> filters,
            string blastIdBounceDomain,
            string actionType,
            string suppressionList,
            bool onlyCounts,
            bool logSuppressed)
        {
            return Blast.GetBlastEmailListForDynamicContent(
                customerId,
                blastId,
                groupId,
                filters,
                blastIdBounceDomain,
                actionType,
                suppressionList,
                onlyCounts,
                logSuppressed);
        }

        public IList<CommunicatorEntities.BlastAbstract> GetBySearch(
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
            bool getChildren)
        {
            return Blast.GetBySearch(
                customerId,
                emailSubject,
                userId,
                groupId,
                isTest,
                statusCode,
                modifiedFrom,
                modifiedTo,
                campaignId,
                campaignName,
                campaignItemName,
                user,
                getChildren);
        }
    }
}

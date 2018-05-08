using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class SocialMedia
    {
        public static bool Exists(int socialMediaID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.SocialMedia.Exists(socialMediaID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists_UseAmbientTransaction(int socialMediaID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope())
            {
                exists = ECN_Framework_DataLayer.Communicator.SocialMedia.Exists(socialMediaID);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.SocialMedia GetSocialMediaByID(int socialMediaID)
        {
            ECN_Framework_Entities.Communicator.SocialMedia sm = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                sm = ECN_Framework_DataLayer.Communicator.SocialMedia.GetSocialMediaByID(socialMediaID);
                scope.Complete();
            }
            return sm;
        }

        public static List<ECN_Framework_Entities.Communicator.SocialMedia> GetSocialMediaCanShare()
        {
            List<ECN_Framework_Entities.Communicator.SocialMedia> smList = new List<ECN_Framework_Entities.Communicator.SocialMedia>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                smList = ECN_Framework_DataLayer.Communicator.SocialMedia.GetSocialMediaCanShare();
                scope.Complete();
            }
            return smList;
        }

        public static List<ECN_Framework_Entities.Communicator.SocialMedia> GetSocialMediaCanPublish()
        {
            List<ECN_Framework_Entities.Communicator.SocialMedia> smList = new List<ECN_Framework_Entities.Communicator.SocialMedia>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                smList = ECN_Framework_DataLayer.Communicator.SocialMedia.GetSocialMediaCanPublish();
                scope.Complete();
            }
            return smList;
        }
    }
}

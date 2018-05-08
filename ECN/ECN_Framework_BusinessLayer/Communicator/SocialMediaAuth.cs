using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class SocialMediaAuth
    {
        /// <summary>
        /// SocialMediaIDs: 1-Facebook, 2-Twitter,3-LinkedIn
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="SocialMediaID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.SocialMediaAuth> GetByCustomerID_SocialMediaID(int CustomerID, int SocialMediaID)
        {
            List<ECN_Framework_Entities.Communicator.SocialMediaAuth> sma = new List<ECN_Framework_Entities.Communicator.SocialMediaAuth>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                sma = ECN_Framework_DataLayer.Communicator.SocialMediaAuth.GetByCustomer_SocialMediaID(CustomerID, SocialMediaID);
                scope.Complete();

            }
            return sma;
        }

        public static List<ECN_Framework_Entities.Communicator.SocialMediaAuth> GetByCustomerID(int CustomerID)
        {
            List<ECN_Framework_Entities.Communicator.SocialMediaAuth> sma = new List<ECN_Framework_Entities.Communicator.SocialMediaAuth>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                sma = ECN_Framework_DataLayer.Communicator.SocialMediaAuth.GetByCustomerID(CustomerID);
                scope.Complete();
            }
            return sma;
        }
        public static bool UsedInBlasts(int SocialMediaAuthID)
        {
            bool used = false;
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                used = ECN_Framework_DataLayer.Communicator.SocialMediaAuth.UsedInBlasts(SocialMediaAuthID);
                scope.Complete();
            }
            return used;
        }
        public static ECN_Framework_Entities.Communicator.SocialMediaAuth GetBySocialMediaAuthID(int authID)
        {
            ECN_Framework_Entities.Communicator.SocialMediaAuth sma = new ECN_Framework_Entities.Communicator.SocialMediaAuth();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                sma = ECN_Framework_DataLayer.Communicator.SocialMediaAuth.GetBySocialMediaAuthID(authID);
                scope.Complete();
            }
            return sma;

        }

        public static bool ExistsByUserID(string UserID, int CustomerID, int SocialMediaID)
        {
            bool exists = false;
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.SocialMediaAuth.UserIDExists(UserID, CustomerID, SocialMediaID);
                scope.Complete();
            }
            return exists;
        }

        public static List<ECN_Framework_Entities.Communicator.SocialMediaAuth> GetByUserID_CustomerID_SocialMediaID(string UserID, int CustomerID, int SocialMediaID)
        {
            List<ECN_Framework_Entities.Communicator.SocialMediaAuth> sma = new List<ECN_Framework_Entities.Communicator.SocialMediaAuth>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                sma = ECN_Framework_DataLayer.Communicator.SocialMediaAuth.GetByUserID_CustomerID_SocialMediaID(UserID, CustomerID, SocialMediaID);
                scope.Complete();
            }
            return sma;
        }

        public static int Save(ECN_Framework_Entities.Communicator.SocialMediaAuth sma, KMPlatform.Entity.User user)
        {
            int retId = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                retId = ECN_Framework_DataLayer.Communicator.SocialMediaAuth.Save(sma, user.UserID);
                scope.Complete();
            }
            return retId;
        }


    }
}

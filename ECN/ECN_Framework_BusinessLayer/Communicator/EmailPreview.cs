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
    public class EmailPreview
    {
        public static ECN_Framework_Entities.Communicator.EmailPreview GetByEmailTestID(int emailTestID)
        {
            ECN_Framework_Entities.Communicator.EmailPreview ep = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ep = ECN_Framework_DataLayer.Communicator.EmailPreview.GetByEmailTestID(emailTestID);
                scope.Complete();
            }

            return ep;
        }

        public static List<ECN_Framework_Entities.Communicator.EmailPreview> GetByBlastID(int blastID)
        {
            List<ECN_Framework_Entities.Communicator.EmailPreview> epList = new List<ECN_Framework_Entities.Communicator.EmailPreview>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                epList = ECN_Framework_DataLayer.Communicator.EmailPreview.GetByBlastID(blastID);
                scope.Complete();
            }

            return epList;
        }

        public static List<ECN_Framework_Entities.Communicator.EmailPreview> GetByCustomerID(int customerID)
        {
            List<ECN_Framework_Entities.Communicator.EmailPreview> epList = new List<ECN_Framework_Entities.Communicator.EmailPreview>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                epList = ECN_Framework_DataLayer.Communicator.EmailPreview.GetByCustomerID(customerID);
                scope.Complete();
            }

            return epList;
        }

        public static bool Insert(ECN_Framework_Entities.Communicator.EmailPreview x)
        {
            bool exists = false;
            //using (TransactionScope scope = new TransactionScope())
            //{
                exists = ECN_Framework_DataLayer.Communicator.EmailPreview.Insert(x);
            //}
            return exists;
        }

        public static DataTable GetUsage(int customerID, int month, int year)
        {
            DataTable dtProfiles = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtProfiles = ECN_Framework_DataLayer.Communicator.EmailPreview.GetUsage(customerID, month, year);
                scope.Complete();
            }
            return dtProfiles;
        }

        public static DataTable GetUsageByBaseChannelID(int baseChannelID, int month, int year)
        {
            DataTable dtProfiles = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtProfiles = ECN_Framework_DataLayer.Communicator.EmailPreview.GetUsageByBaseChannelID(baseChannelID, month, year);
                scope.Complete();
            }
            return dtProfiles;
        }

        public static DataTable GetUsageDetails(int baseChannelID, int month, int year)
        {
            DataTable dtUsageDetails = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtUsageDetails = ECN_Framework_DataLayer.Communicator.EmailPreview.GetUsageDetails(baseChannelID, month, year);
                scope.Complete();
            }
            return dtUsageDetails;
        }
    }
}

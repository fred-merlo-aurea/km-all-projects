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
    public class SmartSegment
    {
        public static ECN_Framework_Entities.Communicator.SmartSegment GetSmartSegmentByID(int smartSegmentID)
        {
            ECN_Framework_Entities.Communicator.SmartSegment ss = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ss = ECN_Framework_DataLayer.Communicator.SmartSegment.GetSmartSegmentByID(smartSegmentID);
                scope.Complete();
            }
            return ss;
        }

        public static List<ECN_Framework_Entities.Communicator.SmartSegment> GetSmartSegments()
        {
            List<ECN_Framework_Entities.Communicator.SmartSegment> smartSegmentList = new List<ECN_Framework_Entities.Communicator.SmartSegment>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                smartSegmentList = ECN_Framework_DataLayer.Communicator.SmartSegment.GetSmartSegments();
                scope.Complete();
            }
            return smartSegmentList;
        }

        public static int GetNewIDFromOldID(int oldSmartSegmentID)
        {
            int count = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                count = ECN_Framework_DataLayer.Communicator.SmartSegment.GetNewIDFromOldID(oldSmartSegmentID);
                scope.Complete();
            }
            return count;
        }

        public static bool SmartSegmentExists(int smartSegmentID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.SmartSegment.SmartSegmentExists(smartSegmentID);
                scope.Complete();
            }
            return exists;
        }

        public static bool SmartSegmentOldExists(int smartSegmentID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.SmartSegment.SmartSegmentOldExists(smartSegmentID);
                scope.Complete();
            }
            return exists;
        }
    }
}

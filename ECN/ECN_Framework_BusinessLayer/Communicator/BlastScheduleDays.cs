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
    public class BlastScheduleDays
    {
        public static ECN_Framework_Entities.Communicator.BlastScheduleDays GetByBlastScheduleDaysID(int blastScheduleDaysID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastScheduleDays days = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                days = ECN_Framework_DataLayer.Communicator.BlastScheduleDays.GetByBlastScheduleDaysID(blastScheduleDaysID);
                scope.Complete();
            }
            return days;
        }

        public static List<ECN_Framework_Entities.Communicator.BlastScheduleDays> GetByBlastScheduleID(int blastScheduleID)
        {
            List<ECN_Framework_Entities.Communicator.BlastScheduleDays> daysList = new List<ECN_Framework_Entities.Communicator.BlastScheduleDays>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                daysList = ECN_Framework_DataLayer.Communicator.BlastScheduleDays.GetByBlastScheduleID(blastScheduleID);
                scope.Complete();
            }
            return daysList;
        }

        public static List<ECN_Framework_Entities.Communicator.BlastScheduleDays> GetByBlastScheduleID_UseAmbientTransaction(int blastScheduleID)
        {
            List<ECN_Framework_Entities.Communicator.BlastScheduleDays> daysList = new List<ECN_Framework_Entities.Communicator.BlastScheduleDays>();
            using (TransactionScope scope = new TransactionScope())
            {
                daysList = ECN_Framework_DataLayer.Communicator.BlastScheduleDays.GetByBlastScheduleID(blastScheduleID);
                scope.Complete();
            }
            return daysList;
        }
    }
}

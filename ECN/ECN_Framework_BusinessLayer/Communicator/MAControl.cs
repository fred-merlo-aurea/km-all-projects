using System;
using System.Collections.Generic;
using System.Transactions;


namespace ECN_Framework_BusinessLayer.Communicator
{
    public class MAControl
    {
        public static int Save(ECN_Framework_Entities.Communicator.MAControl MAC)
        {
            int retID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Communicator.MAControl.Save(MAC);
                scope.Complete();
            }

            return retID;
        }

        public static ECN_Framework_Entities.Communicator.MAControl GetByMAControlID(int MAControlID)
        {
            ECN_Framework_Entities.Communicator.MAControl retItem = new ECN_Framework_Entities.Communicator.MAControl();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retItem = ECN_Framework_DataLayer.Communicator.MAControl.GetByMAControlID(MAControlID);
                scope.Complete();
            }
            return retItem;
        }
        public static bool ExistsByECNID(int ECNID,string ControlType,string MAState)
        {
            bool bExist = false;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                bExist = ECN_Framework_DataLayer.Communicator.MAControl.ExistsByECNID(ECNID,ControlType, MAState);
                scope.Complete();
            }

            return bExist;
        }
        
        public static void UpdateECNID(int newID,int currentID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.MAControl.UpdateECNID(newID,currentID);
                scope.Complete();
            }
        }
        public static ECN_Framework_Entities.Communicator.MAControl GetByControlID(string ControlID, int MAID)
        {
            ECN_Framework_Entities.Communicator.MAControl retItem = new ECN_Framework_Entities.Communicator.MAControl();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retItem = ECN_Framework_DataLayer.Communicator.MAControl.GetByControlID(ControlID,MAID);
                scope.Complete();
            }
            return retItem;
        }

        public static void Delete(int MACID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.MAControl.Delete(MACID);
                scope.Complete();
            }
        }

        public static List<ECN_Framework_Entities.Communicator.MAControl> GetByMarketingAutomationID(int MAID)
        {
            List<ECN_Framework_Entities.Communicator.MAControl> retList = new List<ECN_Framework_Entities.Communicator.MAControl>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.MAControl.GetByMarketingAutomationID(MAID);
                scope.Complete();
            }

            return retList;
        }

        

    }
}

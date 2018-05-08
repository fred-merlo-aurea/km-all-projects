using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class SmartFormActivityLog  //similar to smartFormsTracking
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.SmartFormActivityLog;

        public static int Insert(ECN_Framework_Entities.Communicator.SmartFormActivityLog sfaLog, KMPlatform.Entity.User user)
        {
            Validate(sfaLog, user);

            //if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(sfaLog, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Regular_Blast, user))
            //    throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                sfaLog.SALID = ECN_Framework_DataLayer.Communicator.SmartFormActivityLog.Insert(sfaLog);
                scope.Complete();
            }

            return sfaLog.SALID;
        }


        public static void Validate(ECN_Framework_Entities.Communicator.SmartFormActivityLog sfaLog, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (sfaLog.CustomerID <= 0)
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            else
            {
                if (sfaLog.SFID <= 0 || (!ECN_Framework_BusinessLayer.Communicator.SmartFormsHistory.Exists(sfaLog.SFID, user.CustomerID)))
                    errorList.Add(new ECNError(Entity, Method, "BlastID is invalid"));
                if (sfaLog.GroupID <= 0 || (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(sfaLog.GroupID, user.CustomerID)))
                    errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
                if (sfaLog.EmailID == null || (!ECN_Framework_BusinessLayer.Communicator.Email.Exists(sfaLog.EmailID, user.CustomerID)))
                    errorList.Add(new ECNError(Entity, Method, "EmailID is invalid"));
                if (sfaLog.EmailType.Trim().Length == 0)
                    errorList.Add(new ECNError(Entity, Method, "EmailType is invalid"));
                if (sfaLog.EmailTo.Trim().Length == 0)
                    errorList.Add(new ECNError(Entity, Method, "EmailTo is invalid"));
                if (sfaLog.EmailFrom.Trim().Length == 0)
                    errorList.Add(new ECNError(Entity, Method, "EmailFrom is invalid"));
                if (sfaLog.EmailSubject.Trim().Length == 0)
                    errorList.Add(new ECNError(Entity, Method, "EmailSubject is invalid"));
                if (sfaLog.SendTime <= Convert.ToDateTime("1/1/2013"))
                    errorList.Add(new ECNError(Entity, Method, "SendTime is invalid"));

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(sfaLog.CustomerID))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                    if (sfaLog.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(sfaLog.CreatedUserID.Value, sfaLog.CustomerID))
                        errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    scope.Complete();
                }
            }
            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }
    }
}

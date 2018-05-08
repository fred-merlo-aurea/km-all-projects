//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using System.Transactions;
//using ECN_Framework_Common.Objects;

//namespace ECN_Framework_BusinessLayer.Communicator
//{
//    [Serializable]
//    public class AutoResponders
//    {
        //public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.AutoResponders;

        //public static bool Exists(int autoResponderID, int customerID)
        //{
        //    bool exists = false;
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        exists = ECN_Framework_DataLayer.Communicator.AutoResponders.Exists(autoResponderID, customerID);
        //        scope.Complete();
        //    }
        //    return exists;
        //}

        //public static int Save(ECN_Framework_Entities.Communicator.AutoResponders responder, KMPlatform.Entity.User user)
        //{
        //    ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
        //    if (responder.AutoResponderID > 0)
        //    {
        //        if (!Exists(responder.AutoResponderID, responder.CustomerID.Value))
        //        {
        //            List<ECNError> errorList = new List<ECNError>();
        //            errorList.Add(new ECNError(Entity, Method, "AutoResponderID is invalid"));
        //            throw new ECNException(errorList);
        //        }
        //    }
        //    Validate(responder, user);

        //    if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(responder, user))
        //        throw new ECN_Framework_Common.Objects.SecurityException();

        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        responder.AutoResponderID = ECN_Framework_DataLayer.Communicator.AutoResponders.Save(responder);
        //        scope.Complete();
        //    }
        //    return responder.AutoResponderID;
        //}

        //public static void Validate(ECN_Framework_Entities.Communicator.AutoResponders responder, KMPlatform.Entity.User user)
        //{
        //    ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
        //    List<ECNError> errorList = new List<ECNError>();

        //    if (responder.CustomerID == null)
        //    {
        //        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
        //    }
        //    else
        //    {
        //        if (responder.BlastID == null || !Blast.Exists(responder.BlastID.Value, responder.CustomerID.Value))
        //        {
        //            errorList.Add(new ECNError(Entity, Method, "BlastID is invalid"));
        //        }

        //        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //        {
        //            if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(responder.CustomerID.Value))
        //                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

        //            if (responder.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(responder.CreatedUserID.Value, responder.CustomerID.Value))
        //                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

        //            if (responder.AutoResponderID > 0 && (responder.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(responder.UpdatedUserID.Value, responder.CustomerID.Value)))
        //                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

        //            scope.Complete();
        //        }
        //    }



        //    if (errorList.Count > 0)
        //    {
        //        throw new ECNException(errorList);
        //    }          
        //}
//    }
//}

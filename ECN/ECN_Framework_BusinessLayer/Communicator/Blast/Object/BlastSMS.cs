using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Transactions;
using ECN_Framework_Common.Objects;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class BlastSMS : BlastAbstract
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Blast;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.BlastSMS;

        public override BlastType BlastType
        {
            get
            {
                return BlastType.SMS;
            }
        }

        public override bool HasPermission(KMPlatform.Enums.Access AccessCode, KMPlatform.Entity.User user)
        {
            if (AccessCode == KMPlatform.Enums.Access.View)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns.ToString()) ||
                //    KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Regular_Blast.ToString()))
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                    return true;
            }
            else if (AccessCode == KMPlatform.Enums.Access.Edit)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Regular_Blast.ToString()))
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                    return true;
            }
            return false;
        }


        public override string Send(ref ECN_Framework_Entities.Communicator.BlastAbstract blast)
        {
            return "This is an SMS Blast";
        }

        public override void Validate(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            //validate common fields
            PreValidate(blast);

            if (blast.StatusCode != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Pending.ToString())
                errorList.Add(new ECNError(Entity, Method, "StatusCode is invalid"));
            //if (blast.StatusCodeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Deployed)
            //    errorList.Add(new ECNError(Entity, Method, "StatusCodeID is invalid"));
            if (blast.BlastType != ECN_Framework_Common.Objects.Communicator.Enums.BlastType.SMS.ToString())
                errorList.Add(new ECNError(Entity, Method, "BlastType is invalid"));
            //if (blast.BlastTypeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.SMS)
            //    errorList.Add(new ECNError(Entity, Method, "BlastTypeID is invalid"));
            if (blast.LayoutID == null || blast.CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Layout.Exists(blast.LayoutID.Value, blast.CustomerID.Value)))
                errorList.Add(new ECNError(Entity, Method, "LayoutID is invalid"));
            if (blast.GroupID == null || blast.CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(blast.GroupID.Value, blast.CustomerID.Value)))
                errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
            if (blast.TestBlast.ToUpper() != "Y" && blast.TestBlast.ToUpper() != "N")
                errorList.Add(new ECNError(Entity, Method, "TestBlast is invalid"));
            if (((blast.RefBlastID.Trim() != string.Empty && blast.RefBlastID.Trim() != "-1")) && (blast.CustomerID == null || blast.SendTime == null || (!ECN_Framework_BusinessLayer.Communicator.Blast.RefBlastsExists(blast.RefBlastID.Trim(), blast.CustomerID.Value, blast.SendTime.Value))))
                errorList.Add(new ECNError(Entity, Method, "RefBlastID is invalid"));
            if (((blast.BlastSuppression.Trim() != string.Empty)) && (blast.CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.SuppressionGroupsExists(blast.BlastSuppression.Trim(), blast.CustomerID.Value))))
                errorList.Add(new ECNError(Entity, Method, "BlastSuppression is invalid"));
            //if (blast.AddOptOuts_to_MS == null)
            //    errorList.Add(new ECNError(Entity, Method, "AddOotOuts_to_MS is invalid"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public override void Validate_NoAccessCheck(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            //validate common fields
            PreValidate_NoAccessCheck(blast);

            if (blast.StatusCode != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Pending.ToString())
                errorList.Add(new ECNError(Entity, Method, "StatusCode is invalid"));
            //if (blast.StatusCodeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Deployed)
            //    errorList.Add(new ECNError(Entity, Method, "StatusCodeID is invalid"));
            if (blast.BlastType != ECN_Framework_Common.Objects.Communicator.Enums.BlastType.SMS.ToString())
                errorList.Add(new ECNError(Entity, Method, "BlastType is invalid"));
            //if (blast.BlastTypeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.SMS)
            //    errorList.Add(new ECNError(Entity, Method, "BlastTypeID is invalid"));
            if (blast.LayoutID == null || blast.CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Layout.Exists(blast.LayoutID.Value, blast.CustomerID.Value)))
                errorList.Add(new ECNError(Entity, Method, "LayoutID is invalid"));
            if (blast.GroupID == null || blast.CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(blast.GroupID.Value, blast.CustomerID.Value)))
                errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
            if (blast.TestBlast.ToUpper() != "Y" && blast.TestBlast.ToUpper() != "N")
                errorList.Add(new ECNError(Entity, Method, "TestBlast is invalid"));
            if (((blast.RefBlastID.Trim() != string.Empty && blast.RefBlastID.Trim() != "-1")) && (blast.CustomerID == null || blast.SendTime == null || (!ECN_Framework_BusinessLayer.Communicator.Blast.RefBlastsExists(blast.RefBlastID.Trim(), blast.CustomerID.Value, blast.SendTime.Value))))
                errorList.Add(new ECNError(Entity, Method, "RefBlastID is invalid"));
            if (((blast.BlastSuppression.Trim() != string.Empty)) && (blast.CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.SuppressionGroupsExists(blast.BlastSuppression.Trim(), blast.CustomerID.Value))))
                errorList.Add(new ECNError(Entity, Method, "BlastSuppression is invalid"));
            //if (blast.AddOptOuts_to_MS == null)
            //    errorList.Add(new ECNError(Entity, Method, "AddOotOuts_to_MS is invalid"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        internal override int Save(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user)
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
                Validate(blast, user);

                if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(blast,   user))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                Blast.Save(blast, user);
                //scope.Complete();
            //}
            return blast.BlastID;
        }

        internal override int Save_NoAccessCheck(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user)
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
            Validate_NoAccessCheck(blast, user);


            Blast.Save(blast, user);
            //scope.Complete();
            //}
            return blast.BlastID;
        }

        //public override bool Save(ref ECN_Framework_Entities.Communicator.BlastAbstract blast, ref SqlCommand command)
        //{
        //    if (Validate(ref blast))
        //    {
        //        //save the blast
        //        if (blast.BlastID > 0)
        //        {
        //            try
        //            {
        //                if (Blast.Exists(blast.BlastID, blast.CustomerID.Value))
        //                {
        //                    Blast.Update(ref blast, ref command);
        //                    return true;
        //                }
        //                else
        //                {
        //                    blast.ErrorList.Add(new ValidationError("Update", "Invalid BlastID"));
        //                    return false;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                blast.ErrorList.Add(new ValidationError("Update", ex.ToString()));
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            try
        //            {
        //                Blast.Insert(ref blast, ref command);
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                blast.ErrorList.Add(new ValidationError("Insert", ex.ToString()));
        //                return false;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Transactions;
using ECN_Framework_Common.Objects;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class BlastChampion : BlastExtendedAbstract
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Blast;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.BlastChampion;

        public override BlastType BlastType
        {
            get
            {
                return BlastType.Champion;
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
            return "This is a Champion Blast";
        }

        public override void Validate(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            //validate common fields
            PreValidate(blast);

            //do blast type specific validation            
            if (blast.EmailFrom.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "EmailFrom cannot be empty"));
            if (blast.EmailFromName.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "EmailFromName cannot be empty"));
            if (blast.StatusCode != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Pending.ToString())
                errorList.Add(new ECNError(Entity, Method, "StatusCode is invalid"));
            //if (blast.StatusCodeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Pending)
            //    errorList.Add(new ECNError(Entity, Method, "StatusCodeID is invalid"));
            if (blast.BlastType != ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Champion.ToString())
                errorList.Add(new ECNError(Entity, Method, "BlastType is invalid"));
            //if (blast.BlastTypeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Champion)
            //    errorList.Add(new ECNError(Entity, Method, "BlastTypeID is invalid"));
            if (blast.ReplyTo.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "ReplyTo cannot by empty"));
            if (blast.GroupID == null || blast.CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(blast.GroupID.Value, blast.CustomerID.Value)))
                errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
            if (blast.TestBlast.ToUpper() != "N")
                errorList.Add(new ECNError(Entity, Method, "TestBlast is invalid"));

            if (((blast.RefBlastID.Trim() != string.Empty && blast.RefBlastID.Trim() != "-1")) && (blast.CustomerID == null || blast.SendTime == null || (!ECN_Framework_BusinessLayer.Communicator.Blast.RefBlastsExists(blast.RefBlastID.Trim(), blast.CustomerID.Value, blast.SendTime.Value))))
                errorList.Add(new ECNError(Entity, Method, "RefBlastID is invalid"));
            if (((blast.BlastSuppression.Trim() != string.Empty)) && (blast.CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.SuppressionGroupsExists(blast.BlastSuppression.Trim(), blast.CustomerID.Value))))
                errorList.Add(new ECNError(Entity, Method, "BlastSuppression is invalid"));
            //if (blast.AddOptOuts_to_MS == null)
            //    errorList.Add(new ECNError(Entity, Method, "AddOotOuts_to_MS is invalid"));
            if (blast.SampleID == null || (!ECN_Framework_BusinessLayer.Communicator.Sample.Exists(blast.SampleID.Value, blast.CustomerID.Value)))
                errorList.Add(new ECNError(Entity, Method, "SampleID is invalid"));

            //validate content will not happen as content and subject are established when blast is sent.

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

            //do blast type specific validation            
            if (blast.EmailFrom.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "EmailFrom cannot be empty"));
            if (blast.EmailFromName.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "EmailFromName cannot be empty"));
            if (blast.StatusCode != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Pending.ToString())
                errorList.Add(new ECNError(Entity, Method, "StatusCode is invalid"));
            //if (blast.StatusCodeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Pending)
            //    errorList.Add(new ECNError(Entity, Method, "StatusCodeID is invalid"));
            if (blast.BlastType != ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Champion.ToString())
                errorList.Add(new ECNError(Entity, Method, "BlastType is invalid"));
            //if (blast.BlastTypeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Champion)
            //    errorList.Add(new ECNError(Entity, Method, "BlastTypeID is invalid"));
            if (blast.ReplyTo.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "ReplyTo cannot by empty"));
            if (blast.GroupID == null || blast.CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(blast.GroupID.Value, blast.CustomerID.Value)))
                errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
            if (blast.TestBlast.ToUpper() != "N")
                errorList.Add(new ECNError(Entity, Method, "TestBlast is invalid"));

            if (((blast.RefBlastID.Trim() != string.Empty && blast.RefBlastID.Trim() != "-1")) && (blast.CustomerID == null || blast.SendTime == null || (!ECN_Framework_BusinessLayer.Communicator.Blast.RefBlastsExists(blast.RefBlastID.Trim(), blast.CustomerID.Value, blast.SendTime.Value))))
                errorList.Add(new ECNError(Entity, Method, "RefBlastID is invalid"));
            if (((blast.BlastSuppression.Trim() != string.Empty)) && (blast.CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.SuppressionGroupsExists(blast.BlastSuppression.Trim(), blast.CustomerID.Value))))
                errorList.Add(new ECNError(Entity, Method, "BlastSuppression is invalid"));
            //if (blast.AddOptOuts_to_MS == null)
            //    errorList.Add(new ECNError(Entity, Method, "AddOotOuts_to_MS is invalid"));
            if (blast.SampleID == null || (!ECN_Framework_BusinessLayer.Communicator.Sample.Exists(blast.SampleID.Value, blast.CustomerID.Value)))
                errorList.Add(new ECNError(Entity, Method, "SampleID is invalid"));

            //validate content will not happen as content and subject are established when blast is sent.

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }
    }
}

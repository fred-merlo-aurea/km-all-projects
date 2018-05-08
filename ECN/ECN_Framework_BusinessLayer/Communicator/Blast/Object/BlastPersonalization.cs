using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Transactions;
using ECN_Framework_Common.Objects;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class BlastPersonalization : BlastExtendedAbstract
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Blast;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.BlastPersonalization;

        public override BlastType BlastType
        {
            get
            {
                return BlastType.Personalization;
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
            return "This is a Regular Blast";
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
            if (blast.StatusCode != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Pending.ToString() && blast.StatusCode != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.PendingContent.ToString())
                errorList.Add(new ECNError(Entity, Method, "StatusCode is invalid"));
            //if (blast.StatusCodeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Pending)
            //    errorList.Add(new ECNError(Entity, Method, "StatusCodeID is invalid"));
            if (blast.BlastType != ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Personalization.ToString())
                errorList.Add(new ECNError(Entity, Method, "BlastType is invalid"));
            //if (blast.BlastTypeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Regular)
            //    errorList.Add(new ECNError(Entity, Method, "BlastTypeID is invalid"));
            if (blast.LayoutID == null || blast.CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Layout.Exists(blast.LayoutID.Value, blast.CustomerID.Value)))
                errorList.Add(new ECNError(Entity, Method, "LayoutID is invalid"));

            if (blast.LayoutID != null && blast.BlastType == ECN_Framework_Common.Objects.Communicator.Enums.BlastType.HTML.ToString())
            {
                List<ECN_Framework_Entities.Communicator.Content> contentList = ECN_Framework_BusinessLayer.Communicator.Content.GetByLayoutID_NoAccessCheck(blast.LayoutID.Value, false);
                var blastExists = contentList.Where(x => x.ContentTypeCode == ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.TEXT.ToString());
                if (blastExists.Any())
                    errorList.Add(new ECNError(Entity, Method, "Layout contains content of type TEXT and is invalid for BlastType of HTML"));
            }

            if (blast.GroupID == null || blast.CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(blast.GroupID.Value, blast.CustomerID.Value)))
                errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
            if (blast.ReplyTo.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "ReplyTo cannot by empty"));
            if (blast.TestBlast.ToUpper() != "Y" && blast.TestBlast.ToUpper() != "N")
                errorList.Add(new ECNError(Entity, Method, "TestBlast is invalid"));
            if (((blast.BlastSuppression.Trim() != string.Empty)) && (blast.CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.SuppressionGroupsExists(blast.BlastSuppression.Trim(), blast.CustomerID.Value))))
                errorList.Add(new ECNError(Entity, Method, "BlastSuppression is invalid"));
            //if (blast.AddOptOuts_to_MS == null)
            //    errorList.Add(new ECNError(Entity, Method, "AddOotOuts_to_MS is invalid"));

            if (((blast.RefBlastID.Trim() != string.Empty && blast.RefBlastID.Trim() != "-1")) && (blast.CustomerID == null || blast.SendTime == null || (!ECN_Framework_BusinessLayer.Communicator.Blast.RefBlastsExists(blast.RefBlastID.Trim(), blast.CustomerID.Value, blast.SendTime.Value))))
                errorList.Add(new ECNError(Entity, Method, "RefBlastID is invalid"));
            if (blast.OverrideIsAmount != null)
                if (blast.OverrideAmount == null || blast.OverrideAmount <= 0)
                    errorList.Add(new ECNError(Entity, Method, "BlastAmount is invalid"));

            if (blast.EmailSubject.Trim().Length == 0)
                errorList.Add(new ECNError(Entity, Method, "EmailSubject is invalid"));
            else
            {
                string subjectValid = ECN_Framework_Common.Functions.RegexUtilities.IsValidEmailSubject(blast.EmailSubject);
                if (!string.IsNullOrWhiteSpace(subjectValid))
                    errorList.Add(new ECNError(Entity, Method, "Email Subject has the following issues, " + subjectValid));

            }


            //validate content
            if (blast.GroupID != null && blast.LayoutID != null)
            {
                ValidateBlastContent(blast, user);
            }

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
            if (blast.StatusCode != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Pending.ToString() && blast.StatusCode != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.PendingContent.ToString())
                errorList.Add(new ECNError(Entity, Method, "StatusCode is invalid"));
            //if (blast.StatusCodeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Pending)
            //    errorList.Add(new ECNError(Entity, Method, "StatusCodeID is invalid"));
            if (blast.BlastType != ECN_Framework_Common.Objects.Communicator.Enums.BlastType.HTML.ToString() && blast.BlastType != ECN_Framework_Common.Objects.Communicator.Enums.BlastType.TEXT.ToString())
                errorList.Add(new ECNError(Entity, Method, "BlastType is invalid"));
            //if (blast.BlastTypeID != ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Regular)
            //    errorList.Add(new ECNError(Entity, Method, "BlastTypeID is invalid"));
            if (blast.LayoutID == null || blast.CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Layout.Exists(blast.LayoutID.Value, blast.CustomerID.Value)))
                errorList.Add(new ECNError(Entity, Method, "LayoutID is invalid"));

            if (blast.LayoutID != null && blast.BlastType == ECN_Framework_Common.Objects.Communicator.Enums.BlastType.HTML.ToString())
            {
                List<ECN_Framework_Entities.Communicator.Content> contentList = ECN_Framework_BusinessLayer.Communicator.Content.GetByLayoutID_NoAccessCheck(blast.LayoutID.Value, false);
                var blastExists = contentList.Where(x => x.ContentTypeCode == ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.TEXT.ToString());
                if (blastExists.Any())
                    errorList.Add(new ECNError(Entity, Method, "Layout contains content of type TEXT and is invalid for BlastType of HTML"));
            }

            if (blast.GroupID == null || blast.CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(blast.GroupID.Value, blast.CustomerID.Value)))
                errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
            if (blast.ReplyTo.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "ReplyTo cannot by empty"));
            if (blast.TestBlast.ToUpper() != "Y" && blast.TestBlast.ToUpper() != "N")
                errorList.Add(new ECNError(Entity, Method, "TestBlast is invalid"));
            if (((blast.BlastSuppression.Trim() != string.Empty)) && (blast.CustomerID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.SuppressionGroupsExists(blast.BlastSuppression.Trim(), blast.CustomerID.Value))))
                errorList.Add(new ECNError(Entity, Method, "BlastSuppression is invalid"));
            //if (blast.AddOptOuts_to_MS == null)
            //    errorList.Add(new ECNError(Entity, Method, "AddOotOuts_to_MS is invalid"));

            if (((blast.RefBlastID.Trim() != string.Empty && blast.RefBlastID.Trim() != "-1")) && (blast.CustomerID == null || blast.SendTime == null || (!ECN_Framework_BusinessLayer.Communicator.Blast.RefBlastsExists(blast.RefBlastID.Trim(), blast.CustomerID.Value, blast.SendTime.Value))))
                errorList.Add(new ECNError(Entity, Method, "RefBlastID is invalid"));
            if (blast.OverrideIsAmount != null)
                if (blast.OverrideAmount == null || blast.OverrideAmount <= 0)
                    errorList.Add(new ECNError(Entity, Method, "BlastAmount is invalid"));

            if (blast.EmailSubject.Trim().Length == 0)
                errorList.Add(new ECNError(Entity, Method, "EmailSubject is invalid"));
            else
            {
                string subjectValid = ECN_Framework_Common.Functions.RegexUtilities.IsValidEmailSubject(blast.EmailSubject);
                if (!string.IsNullOrWhiteSpace(subjectValid))
                    errorList.Add(new ECNError(Entity, Method, "Email Subject has the following issues, " + subjectValid));

            }


            //validate content
            if (blast.GroupID != null && blast.LayoutID != null)
            {
                ValidateBlastContent(blast, user);
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }
    }
}

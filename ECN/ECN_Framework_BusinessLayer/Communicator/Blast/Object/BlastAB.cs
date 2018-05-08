using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using CommonEnums = ECN_Framework_Common.Objects.Enums;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class BlastAB : BlastExtendedAbstract
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Blast;
        private const string TestBlastValueN = "N";

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.BlastAB;

        public override BlastType BlastType
        {
            get
            {
                return BlastType.Sample;
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
            return "This is an A/B Blast";
        }

        public override void Validate(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user)
        {
            var errorList = new List<ECNError>();

            PreValidate(blast);
            ValidateBlastType(blast, user, errorList, true);
        }

        public override void Validate_NoAccessCheck(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user)
        {
            var errorList = new List<ECNError>();

            PreValidate_NoAccessCheck(blast);
            ValidateBlastType(blast, user, errorList);
        }

        private void ValidateBlastType(ECN_Framework_Entities.Communicator.BlastAbstract blast, User user, List<ECNError> errorList, bool shouldValidateEmailFromAndReplyTo = false)
        {
            var methodValidate = CommonEnums.Method.Validate;

            //do blast type specific validation
            if (blast.EmailFrom.Trim() == string.Empty)
            {
                errorList.Add(new ECNError(Entity, methodValidate, "EmailFrom cannot be empty"));
            }

            if (shouldValidateEmailFromAndReplyTo)
            {
                if (blast.EmailFrom.Trim() != string.Empty && !Email.IsValidEmailAddress(blast.EmailFrom.Trim()))
                {
                    errorList.Add(new ECNError(Entity, methodValidate, "EmailFrom is invalid"));
                }
            }

            if (blast.EmailFromName.Trim() == string.Empty)
            {
                errorList.Add(new ECNError(Entity, methodValidate, "EmailFromName cannot be empty"));
            }
            if (blast.StatusCode != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Pending.ToString())
            {
                errorList.Add(new ECNError(Entity, methodValidate, "StatusCode is invalid"));
            }
            if (blast.BlastType != ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Sample.ToString())
            {
                errorList.Add(new ECNError(Entity, methodValidate, "BlastType is invalid"));
            }
            if (blast.LayoutID == null || blast.CustomerID == null || (!Layout.Exists(blast.LayoutID.Value, blast.CustomerID.Value)))
            {
                errorList.Add(new ECNError(Entity, methodValidate, "LayoutID is invalid"));
            }
            if (blast.GroupID == null || blast.CustomerID == null || (!Group.Exists(blast.GroupID.Value, blast.CustomerID.Value)))
            {
                errorList.Add(new ECNError(Entity, methodValidate, "GroupID is invalid"));
            }
            if (blast.ReplyTo.Trim() == string.Empty)
            {
                errorList.Add(new ECNError(Entity, methodValidate, "ReplyTo cannot by empty"));
            }

            if (shouldValidateEmailFromAndReplyTo)
            {
                if (blast.ReplyTo.Trim() != string.Empty && !Email.IsValidEmailAddress(blast.ReplyTo.Trim()))
                {
                    errorList.Add(new ECNError(Entity, methodValidate, "ReplyTo is invalid"));
                }
            }

            if (blast.TestBlast.ToUpper() != TestBlastValueN)
            {
                errorList.Add(new ECNError(Entity, methodValidate, "TestBlast is invalid"));
            }
            if (blast.BlastSuppression.Trim() != string.Empty
                && (blast.CustomerID == null
                    || !Group.SuppressionGroupsExists(blast.BlastSuppression.Trim(), blast.CustomerID.Value)))
            {
                errorList.Add(new ECNError(Entity, methodValidate, "BlastSuppression is invalid"));
            }
            if ((blast.RefBlastID.Trim() != string.Empty && blast.RefBlastID.Trim() != "-1")
                && (blast.CustomerID == null 
                    || blast.SendTime == null 
                    || !Blast.RefBlastsExists(blast.RefBlastID.Trim(), blast.CustomerID.Value, blast.SendTime.Value)))
            {
                errorList.Add(new ECNError(Entity, methodValidate, "RefBlastID is invalid"));
            }
            if (blast.OverrideAmount == null || blast.OverrideAmount <= 0)
            {
                errorList.Add(new ECNError(Entity, methodValidate, "BlastAmount is invalid"));
            }
            if (blast.OverrideIsAmount == null)
            {
                errorList.Add(new ECNError(Entity, methodValidate, "BlastAmountType is invalid"));
            }
            if (blast.EmailSubject.Trim().Length == 0)
            {
                errorList.Add(new ECNError(Entity, methodValidate, "EmailSubject is invalid"));
            }
            else
            {
                var subjectValid = ECN_Framework_Common.Functions.RegexUtilities.IsValidEmailSubject(blast.EmailSubject);
                if (!string.IsNullOrWhiteSpace(subjectValid))
                {
                    errorList.Add(new ECNError(Entity, methodValidate, "Email Subject has the following issues, " + subjectValid));
                }
            }

            if (blast.SampleID == null || !Sample.Exists(blast.SampleID.Value, blast.CustomerID.Value))
            {
                errorList.Add(new ECNError(Entity, methodValidate, "SampleID is invalid"));
            }

            //validate content
            if (blast.GroupID != null && blast.LayoutID != null)
            {
                ValidateBlastContent(blast, user);
            }

            if (errorList.Any())
            {
                throw new ECNException(errorList);
            }
        }
    }
}

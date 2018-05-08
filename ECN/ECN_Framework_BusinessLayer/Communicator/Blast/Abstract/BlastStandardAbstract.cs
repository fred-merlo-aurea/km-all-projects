using System;
using System.Collections.Generic;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using Entities = ECN_Framework_Entities.Communicator;
using KMEnums = KMPlatform.Enums;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public abstract class BlastStandardAbstract : BlastAbstract
    {
        protected internal const string TestBlast = "N";
        protected internal const string EmailFromErrorMessage = "EmailFrom cannot be empty";
        protected internal const string EmailFromNameErrorMessage = "EmailFromName cannot be empty";
        protected internal const string StatusCodeErrorMessage = "StatusCode is invalid";
        protected internal const string BlastTypeErrorMessage = "BlastType is invalid";
        protected internal const string LayoutErrorMessage = "LayoutID is invalid";
        protected internal const string ReplyToErrorMessage = "ReplyTo cannot by empty";
        protected internal const string TestBlastErrorMessage = "TestBlast is invalid";
        protected internal const string EmailSubjectErrorMessage = "EmailSubject is invalid";

        public override void Validate(Entities.BlastAbstract blast, User user)
        {
            var errorList = new List<ECNError>();

            PreValidate(blast);            
            BlastTypeSpecificValidation(blast, errorList);
            
            if (blast.GroupID != null && blast.LayoutID != null)
            {
                ValidateBlastContent(blast, user);
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public override void Validate_NoAccessCheck(Entities.BlastAbstract blast, User user)
        {
            var errorList = new List<ECNError>();

            PreValidate_NoAccessCheck(blast);
            BlastTypeSpecificValidation(blast, errorList);

            if (blast.GroupID != null && blast.LayoutID != null)
            {
                ValidateBlastContent(blast, user);
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        internal override int Save(Entities.BlastAbstract blast, User user)
        {
            Validate(blast, user);

            if (!HasPermission(KMEnums.Access.Edit, user))
            {
                throw new SecurityException();
            }

            if (!AccessCheck.CanAccessByCustomer(blast, user))
            {
                throw new SecurityException();
            }

            Blast.Save(blast, user);

            return blast.BlastID;
        }

        internal override int Save_NoAccessCheck(Entities.BlastAbstract blast, User user)
        {
            Validate_NoAccessCheck(blast, user);
            Blast.Save(blast, user);

            return blast.BlastID;
        }

        protected virtual void BlastTypeSpecificValidation(Entities.BlastAbstract blast, List<ECNError> errorList)
        {
            if (string.IsNullOrWhiteSpace(blast.EmailFrom))
            {
                errorList.Add(new ECNError(Entity, Enums.Method.Validate, EmailFromErrorMessage));
            }

            if (string.IsNullOrWhiteSpace(blast.EmailFromName))
            {
                errorList.Add(new ECNError(Entity, Enums.Method.Validate, EmailFromNameErrorMessage));
            }

            if (blast.StatusCode != BlastStatusCode.System.ToString())
            {
                errorList.Add(new ECNError(Entity, Enums.Method.Validate, StatusCodeErrorMessage));
            }

            if (blast.BlastType != BlastType.ToString())
            {
                errorList.Add(new ECNError(Entity, Enums.Method.Validate, BlastTypeErrorMessage));
            }

            if (blast.LayoutID == null ||
                blast.CustomerID == null ||
                !Layout.Exists(blast.LayoutID.Value, blast.CustomerID.Value))
            {
                errorList.Add(new ECNError(Entity, Enums.Method.Validate, LayoutErrorMessage));
            }

            if (string.IsNullOrWhiteSpace(blast.ReplyTo))
            {
                errorList.Add(new ECNError(Entity, Enums.Method.Validate, ReplyToErrorMessage));
            }

            if (!blast.TestBlast.Equals(TestBlast, StringComparison.OrdinalIgnoreCase))
            {
                errorList.Add(new ECNError(Entity, Enums.Method.Validate, TestBlastErrorMessage));
            }

            if (string.IsNullOrWhiteSpace(blast.EmailSubject))
            {
                errorList.Add(new ECNError(Entity, Enums.Method.Validate, EmailSubjectErrorMessage));
            }
        }
    }
}

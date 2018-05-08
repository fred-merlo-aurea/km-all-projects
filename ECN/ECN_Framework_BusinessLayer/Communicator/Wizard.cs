using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECN_Framework_Common.Objects;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class Wizard
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Wizard;

        public static bool Exists(int wizardID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Wizard.Exists(wizardID);
                scope.Complete();
            }
            return exists;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.Wizard wizard, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!KM.Platform.User.IsSystemAdministrator(user))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            if (wizard.WizardName.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "WizardName Name is missing"));

            if (wizard.WizardID <= 0 && (wizard.CreatedUserID == null))
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (wizard.WizardID > 0 && (wizard.UpdatedUserID == null))
                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static int Save(ref ECN_Framework_Entities.Communicator.Wizard wizard, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (wizard.WizardID > 0)
            {
                if (!Exists(wizard.WizardID))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "WizardID is invalid"));
                    throw new ECNException(errorList);
                }
            }

            Validate(wizard, user);

            using (TransactionScope scope = new TransactionScope())
            {
                wizard.WizardID = ECN_Framework_DataLayer.Communicator.Wizard.Save(wizard);
                scope.Complete();
            }
            return wizard.WizardID;

        }
    }
}

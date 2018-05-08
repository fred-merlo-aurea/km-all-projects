using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ECN_Framework_Common.Objects;
using CommLayoutPlans = ECN_Framework_Entities.Communicator.LayoutPlans;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public abstract class LayoutPlansValidateBase
    {
        protected const Enums.Entity Entity = Enums.Entity.LayoutPlans;
        protected const Enums.Method Method = Enums.Method.Validate;
        protected List<ECNError> errorList = new List<ECNError>();

        public abstract void ValidateUser(CommLayoutPlans layoutPlan);

        public abstract bool Exists_GroupCriteria(int groupId, string criteria);

        public void Validate(CommLayoutPlans layoutPlan)
        {
            if (layoutPlan == null)
            {
                throw new ArgumentNullException("layoutPlan");
            }

            if (layoutPlan.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!Accounts.Customer.Exists(layoutPlan.CustomerID.Value))
                    {
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                    }

                    ValidateUser(layoutPlan);

                    scope.Complete();
                }

                if (layoutPlan.LayoutID != null && (!Layout.Exists(layoutPlan.LayoutID.Value, layoutPlan.CustomerID.Value)))
                {
                    errorList.Add(new ECNError(Entity, Method, "LayoutID is invalid"));
                }

                if (String.IsNullOrWhiteSpace(layoutPlan.EventType))
                {
                    errorList.Add(new ECNError(Entity, Method, "EventType cannot be empty"));
                }

                if (layoutPlan.BlastID == null || !Blast.Exists(layoutPlan.BlastID.Value, layoutPlan.CustomerID.Value))
                {
                    errorList.Add(new ECNError(Entity, Method, "BlastID is invalid"));
                }

                if (layoutPlan.Period == null)
                {
                    errorList.Add(new ECNError(Entity, Method, "Period is invalid"));
                }

                if (String.IsNullOrWhiteSpace(layoutPlan.ActionName))
                {
                    errorList.Add(new ECNError(Entity, Method, "ActionName is invalid"));
                }

                if (layoutPlan.GroupID != null && layoutPlan.GroupID != 0 && (!Group.Exists(layoutPlan.GroupID.Value, layoutPlan.CustomerID.Value)) && (Exists_GroupCriteria(layoutPlan.GroupID.Value, layoutPlan.Criteria)))
                {
                    errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }
    }
}

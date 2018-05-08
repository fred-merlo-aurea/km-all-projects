using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class LayoutPlansValidate : LayoutPlansValidateBase
    {
        public override void ValidateUser(ECN_Framework_Entities.Communicator.LayoutPlans layoutPlan)
        {
            var invalidation = Base.UserValidation.Invalidate(layoutPlan);
            if (!string.IsNullOrWhiteSpace(invalidation))
            {
                errorList.Add(new ECNError(Entity, Method, invalidation));
            }
        }

        public override bool Exists_GroupCriteria(int groupId, string criteria)
        {
            var exists = false;

            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.LayoutPlans.Exists(groupId, criteria);
                scope.Complete();
            }

            return exists;
        }
    }
}

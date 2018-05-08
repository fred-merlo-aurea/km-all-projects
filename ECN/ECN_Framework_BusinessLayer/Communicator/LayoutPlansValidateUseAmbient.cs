using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ECN_Framework_Common.Objects;
using CommLayoutPlans = ECN_Framework_Entities.Communicator.LayoutPlans;
using DataLayoutPlans = ECN_Framework_DataLayer.Communicator.LayoutPlans;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class LayoutPlansValidateUseAmbient : LayoutPlansValidateBase
    {
        public override void ValidateUser(CommLayoutPlans layoutPlan)
        {
            if (layoutPlan.CreatedUserID == null || (layoutPlan.CreatedUserID.HasValue &&
                !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(layoutPlan.CreatedUserID.Value, false))))
            {
                if (layoutPlan.CreatedUserID == null || (!KMPlatform.BusinessLogic.User.Exists(layoutPlan.CreatedUserID.Value, layoutPlan.CustomerID.Value)))
                {
                    errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                }
            }

            if (layoutPlan.LayoutPlanID > 0 && (layoutPlan.UpdatedUserID == null ||
                (layoutPlan.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(layoutPlan.UpdatedUserID.Value, false)))))
            {
                if (layoutPlan.UpdatedUserID == null || (!KMPlatform.BusinessLogic.User.Exists(layoutPlan.UpdatedUserID.Value, layoutPlan.CustomerID.Value)))
                {
                    errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                }
            }
        }

        public override bool Exists_GroupCriteria(int groupId, string criteria)
        {
            var exists = false;

            using (var scope = new TransactionScope())
            {
                exists = DataLayoutPlans.Exists(groupId, criteria);
                scope.Complete();
            }

            return exists;
        }
    }
}

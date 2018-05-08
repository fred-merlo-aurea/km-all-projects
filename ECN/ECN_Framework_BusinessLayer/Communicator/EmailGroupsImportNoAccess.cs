using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class EmailGroupsImportNoAccess : EmailGroupsImportBase
    {
        public override ECN_Framework_Entities.Communicator.Group GetMasterSuppressionGroup(int customerId, User user)
        {
            return Group.GetMasterSuppressionGroup_NoAccessCheck(customerId);
        }

        public override ECN_Framework_Entities.Communicator.EmailGroup GetByEmailAddressGroupID(string emailAddress, int groupID, User user)
        {
            return EmailGroup.GetByEmailAddressGroupID_NoAccessCheck(emailAddress, groupID);
        }
    }
}

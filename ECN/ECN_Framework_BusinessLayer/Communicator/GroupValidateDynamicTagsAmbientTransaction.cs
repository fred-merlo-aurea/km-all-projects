using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class GroupValidateDynamicTagsAmbientTransaction : GroupValidateDynamicTagsAbstract
    {
        public override IList<ECN_Framework_Entities.Communicator.GroupDataFields> GetGroupDataFields(int groupId)
        {
            return GroupDataFields.GetByGroupID_NoAccessCheck_UseAmbientTransaction(groupId);
        }

        public override DataTable GetDataTable()
        {
            return ECN_Framework_BusinessLayer.Communicator.Email.GetColumnNames_UseAmbientTransaction();
        }

        public override ECN_Framework_Entities.Communicator.Layout GetLayout(int layoutId)
        {
            return ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck_UseAmbientTransaction(layoutId, true);
        }
    }
}

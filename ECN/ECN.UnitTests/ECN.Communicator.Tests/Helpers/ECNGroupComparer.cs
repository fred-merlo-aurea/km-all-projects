using ecn.communicator.classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN.Communicator.Tests.Helpers
{
    public class ECNGroupComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            var firstGroup = x as ECN_Group;
            var secondGroup = y as ECN_Group;

            if (firstGroup != null
                && secondGroup != null
                && firstGroup.GroupID == secondGroup.GroupID
                && firstGroup.GroupName == secondGroup.GroupName)
            {
                return 0;
            }
            else if (firstGroup != null
                && secondGroup != null
                && firstGroup.GroupID > secondGroup.GroupID)
            {
                return 1;
            }

            return -1;
        }
    }
}

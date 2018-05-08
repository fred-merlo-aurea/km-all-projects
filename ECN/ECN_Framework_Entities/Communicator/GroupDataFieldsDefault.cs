using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class GroupDataFieldsDefault
    {
        public GroupDataFieldsDefault()
        {
            GDFID = -1;
            DataValue = string.Empty;
            SystemValue = string.Empty;
        }

        #region props

        public int GDFID { get; set; }

        public string DataValue { get; set; }

        public string SystemValue { get; set; }

        #endregion
    }
}

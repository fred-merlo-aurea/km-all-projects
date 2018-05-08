using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Collector
{
    [Serializable]
    [DataContract]
    public class GridStatements
    {
        public GridStatements()
        {
            GridStatementID = -1;
            QuestionID = -1;
            GridStatement = string.Empty;
        }

        [DataMember]
        public int GridStatementID { get; set; }
        [DataMember]
        public int QuestionID { get; set; }
        [DataMember]
        public string GridStatement { get; set; }
    }
}

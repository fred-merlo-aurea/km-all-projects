using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator

{

    [Serializable]
    [DataContract]
    public class SmartFormTracker
    
    {      public SmartFormTracker()
        {
            SALID = -1;
            CustomerID = -1;
            SFID = -1;
            GroupID = -1;
            ReferringUrl = string.Empty;
            ActivityDate = Convert.ToDateTime("1/1/1900");
        }


    [DataMember]
    public int SALID { get; set; }
    [DataMember]
    public int BlastID { get; set; }
    [DataMember]
    public int CustomerID { get; set; }
    [DataMember]
    public int SFID { get; set; }
    [DataMember]
    public int GroupID { get; set; }
    //[DataMember]
    //public string GroupID { get; set; }
    [DataMember]
    public string ReferringUrl { get; set; }
    [DataMember]
    public DateTime ActivityDate { get; set; }
    }
}

using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class DataCompareCostUser : CommonDataCompareProperties
    {
        public DataCompareCostUser() 
        {
            UserId = 0;
        }
        [DataMember]
        public int UserId { get; set; }
    }
}
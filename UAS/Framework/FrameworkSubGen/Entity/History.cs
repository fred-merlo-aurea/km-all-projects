using System;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class History : IEntity
    {
        public History()
        {
            date = DateTimeFunctions.GetMinDate();
        }
        #region Properties
        [DataMember]
        public int history_id { get; set; }
        [DataMember]
        public int user_id { get; set; }
        [DataMember]
        public string notes { get; set; }
        [DataMember]
        public DateTime date { get; set; }
        [DataMember]
        public int subscriber_id { get; set; }
        [DataMember]
        public int account_id { get; set; }
        #endregion
    }
}

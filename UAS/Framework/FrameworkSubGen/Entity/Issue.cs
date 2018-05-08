using System;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class Issue : IEntity
    {
        public Issue()
        {
            date = DateTimeFunctions.GetMinDate();
        }
        #region Properties
        [DataMember]
        public int issue_id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public DateTime date { get; set; }
        [DataMember]
        public int account_id { get; set; }
        #endregion
    }
}

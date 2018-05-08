using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class ProductGroup
    {
        public ProductGroup() { }

        #region Properties
        [DataMember(Name = "ProductID")]
        public int PubID { get; set; }
        [DataMember]
        public int GroupID { get; set; }
        [DataMember]
        public string Name { get; set; }
        #endregion
    }
}

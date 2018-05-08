using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class PaidBundleItem
    {
        public PaidBundleItem() 
        {
            BundleName = string.Empty;
            Price = 0;
        }
        #region Properties
        [DataMember]
        public string BundleName { get; set; }

        /// <summary>
        /// Price per Bundle.
        /// </summary>
        [DataMember]
        public double Price { get; set; }
        #endregion
    }
}

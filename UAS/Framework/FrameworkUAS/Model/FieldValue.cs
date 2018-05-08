using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAS.Model
{
    [Serializable]
    [DataContract]
    public class FieldValue
    {
        public FieldValue() { }
        public FieldValue(int _id, string _itemText, string _itemValue, int _itemOrder)
        {
            Id = _id;
            ItemText = _itemText;
            ItemValue = _itemValue;
            ItemOrder = _itemOrder;
        }
        #region Properties
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ItemText { get; set; }
        [DataMember]
        public string ItemValue { get; set; }
        [DataMember]
        public int ItemOrder { get; set; }
        #endregion
    }
}

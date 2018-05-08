using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace UAD.DataCompare.Web.Models
{
    public class FilterDetailsModel
    {
        public FilterDetailsModel()
        {
            FilterNo = -1;
            Field = string.Empty;
            Values = string.Empty;
        }
        #region Properties
        [DataMember]
        public int FilterNo { get; set; }
        [DataMember]
        public string Field { get; set; }
        [DataMember]
        public string Values { get; set; }
        #endregion
    }
}
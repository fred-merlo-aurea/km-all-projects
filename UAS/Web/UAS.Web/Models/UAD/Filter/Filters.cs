using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KMPlatform.Object;
using System.Data.SqlClient;
using FrameworkUAD.DataAccess;
using System.Text;
using UAS.Web.Models.UAD.Filter;
using System.Xml.Linq;

namespace UAS.Web.Models.UAD.Filter
{


    public class FilterVM
    {
        public int FilterNo { get; set; }
        public string FilterName { get; set; }
        public bool Executed { get; set; }
        public int Count { get; set; }
        public List<FieldVM> Fields { get; set; }
        public string ViewType { get; set; }
        public int PubID { get; set; }
        public int BrandID { get; set; }
        public int FilterGroupID { get; set; }
        public string FilterGroupName { get; set; }

    }
    public class FieldVM
    {
        public string Name { get; set; }
        public string Values { get; set; }
        public string Text { get; set; }
        public string SearchCondition { get; set; }
        public string FilterType { get; set; }
        public string Group { get; set; }
    }
   
}

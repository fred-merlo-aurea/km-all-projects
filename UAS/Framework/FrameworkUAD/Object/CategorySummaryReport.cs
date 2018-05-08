using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class CategorySummaryReport
    {
        #region Properties
        [DataMember]
        public int categorygroup_ID { get; set; }
        [DataMember]
        public int category_ID { get; set; }
        [DataMember]
        public string categorygroup_name { get; set; }
        [DataMember]
        public string category_name { get; set; }
        [DataMember]
        public int total { get; set; }
        #endregion

        public List<CategorySummaryReport> GetTestList() //TESTING PURPOSES
        {
            List<FrameworkUAD.Object.CategorySummaryReport> svProducts = new List<FrameworkUAD.Object.CategorySummaryReport>();
            FrameworkUAD.Object.CategorySummaryReport q = new FrameworkUAD.Object.CategorySummaryReport();
            FrameworkUAD.Object.CategorySummaryReport q2 = new FrameworkUAD.Object.CategorySummaryReport();
            FrameworkUAD.Object.CategorySummaryReport q3 = new FrameworkUAD.Object.CategorySummaryReport();
            q.categorygroup_ID = 1;
            q.category_ID = 2;
            q.categorygroup_name = "TEST";
            q.category_name = "SubTest";
            q.total = 5;
            svProducts.Add(q);
            q2.categorygroup_ID = 2;
            q2.category_ID = 3;
            q2.categorygroup_name = "TEST2";
            q2.category_name = "SubTest";
            q2.total = 10;
            svProducts.Add(q2);
            q3.categorygroup_ID = 2;
            q3.category_ID = 4;
            q3.categorygroup_name = "TEST2";
            q3.category_name = "SubTest2";
            q3.total = 5;
            svProducts.Add(q3);

            return svProducts;
        }
    }
}

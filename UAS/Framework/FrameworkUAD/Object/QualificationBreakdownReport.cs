using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [DataContract]
    public class QualificationBreakdownReport
    {
        #region Properties
        [DataMember]
        public string QSourceGroup { get; set; }
        [DataMember]
        public string QSource { get; set; }
        [DataMember]
        public int OneYearCount { get; set; }
        [DataMember]
        public int TwoYearCount { get; set; }
        [DataMember]
        public int ThreeYearCount { get; set; }
        [DataMember]
        public int FourYearCount { get; set; }
        [DataMember]
        public int OtherYearCount { get; set; }
        [DataMember]
        public int NonQualifiedCount { get; set; }
        [DataMember]
        public int QualifiedCount { get; set; }
        #endregion        

        public List<QualificationBreakdownReport> GetList() //TESTING PURPOSES
        {
            List<FrameworkUAD.Object.QualificationBreakdownReport> svProducts = new List<FrameworkUAD.Object.QualificationBreakdownReport>();
            FrameworkUAD.Object.QualificationBreakdownReport q = new FrameworkUAD.Object.QualificationBreakdownReport();
            FrameworkUAD.Object.QualificationBreakdownReport q2 = new FrameworkUAD.Object.QualificationBreakdownReport();
            q.OneYearCount = 1;
            q.QSourceGroup = "TEST";
            q.QSource = "SubTest";
            svProducts.Add(q);
            q2.OneYearCount = 2;
            q2.QSourceGroup = "TEST";
            q2.QSource = "SubTest2";
            svProducts.Add(q2);

            return svProducts;
        }
    }
}

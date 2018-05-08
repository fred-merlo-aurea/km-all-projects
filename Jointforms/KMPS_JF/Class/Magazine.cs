using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;  

namespace KMPS_JF.Class
{
    /// <summary>
    /// Represents magazines in payment page
    /// </summary>
    public class Magazine
    {      
        public string Header { get; set; }
        public string Footer { get; set; }   
        public string PubCode { get; set; }   
        public int GroupID { get; set; }
        public int CustID { get; set; }
        public string CoverImage { get; set; }
        public string Title { get; set; }  
        public int Issues { get; set; } 
        public decimal DigitalEmail { get; set; }  
        public decimal USPrice { get; set; } 
        public decimal CanPrice { get; set; }  
        public decimal IntPrice { get; set; }  
        public string JFLink { get;  set; }  
        public ccProcessors PaymentGateway { get; set; }

        public static List<Magazine> GetMagList(string path, string PubCode)  
        {
            DataSet ds = new DataSet();
            ds.ReadXml(path);     
            DataTable dtMagList = ds.Tables[0];

            List<Magazine> magList = new List<Magazine>(); 
            
            foreach (DataRow dr in dtMagList.Rows) 
            {
                Magazine m = new Magazine();              
                m.Header = !dr.IsNull("header") ? dr["header"].ToString() : string.Empty;
                m.Footer = !dr.IsNull("footer") ? dr["footer"].ToString() : string.Empty;      
                m.PubCode = dr["pubcode"].ToString();
                m.GroupID = Convert.ToInt32(dr["groupID"].ToString());
                m.CustID = Convert.ToInt32(dr["custID"].ToString());
                m.CoverImage = dr["coverimage"].ToString();
                m.Title = dr["title"].ToString();
                m.Issues = Convert.ToInt32(dr["issues"].ToString());
                m.DigitalEmail = Convert.ToDecimal(dr["digitalemail"].ToString());
                m.USPrice = Convert.ToDecimal(dr["usprice"].ToString());
                m.CanPrice = Convert.ToDecimal(dr["canprice"].ToString());
                m.IntPrice = Convert.ToDecimal(dr["intprice"].ToString());
                m.JFLink = dr["jflink"].ToString();
                m.PaymentGateway =  (ccProcessors)Enum.Parse(typeof(ccProcessors),dr["paymentgateway"].ToString(),true);       
                magList.Add(m);     
            }  

            if (String.IsNullOrEmpty(PubCode))
            {
                return magList;     
            }
            else
            {
                return magList.FindAll(x => x.PubCode.ToUpper() == PubCode.ToUpper());                       
            } 
        }
    }
}

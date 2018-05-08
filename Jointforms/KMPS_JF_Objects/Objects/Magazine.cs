using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KMPS_JF_Objects.Objects
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
        public int Term { get; set; }
        public decimal DigitalEmail { get; set; }
        public decimal USPrice1 { get; set; }
        public decimal CanPrice1 { get; set; }
        public decimal IntPrice1 { get; set; }
        public decimal USPrice2 { get; set; }
        public decimal CanPrice2 { get; set; }
        public decimal IntPrice2 { get; set; }
        public decimal USPrice3 { get; set; }
        public decimal CanPrice3 { get; set; }
        public decimal IntPrice3 { get; set; }
        public string JFLink { get; set; }
        public ccProcessors PaymentGateway { get; set; }
        public string Description { get; set; }
        public bool ShowContact { get; set; }
        public bool ShowBillingAddress { get; set; }
        public bool ShowShippingAddress { get; set; }

        public Magazine()
        {
            this.Header = string.Empty;
            this.Footer = string.Empty;
            this.PubCode = string.Empty;
            this.GroupID = -1;
            this.CustID = -1;
            this.CoverImage = string.Empty;
            this.Title = string.Empty;
            this.Issues = -1;
            this.Term = -1;
            this.DigitalEmail = -1;
            this.USPrice1 = -1;
            this.CanPrice1 = -1;
            this.IntPrice1 = -1;
            this.USPrice2 = -1;
            this.CanPrice2 = -1;
            this.IntPrice2 = -1;
            this.USPrice3 = -1;
            this.CanPrice3 = -1;
            this.IntPrice3 = -1;
            this.JFLink = string.Empty;
            this.PaymentGateway = ccProcessors.AuthorizeNet;
            this.Description = string.Empty;
            this.ShowContact = true;
            this.ShowBillingAddress = true;
            this.ShowShippingAddress = true; 
        }

        public static List<Magazine> GetMagList(string path, string PubCode)
        {
            List<Magazine> magList = new List<Magazine>();

            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(path);
                DataTable dtMagList = ds.Tables[0];
                try
                {
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
                        m.Term = Convert.ToInt32(dr["term"].ToString());
                        m.DigitalEmail = Convert.ToDecimal(dr["digitalemail"].ToString());                   
                        m.USPrice1 = Convert.ToDecimal(dr["usprice1"].ToString());
                        try
                        {
                            if (dr["usprice2"] != null)
                                m.USPrice2 = Convert.ToDecimal(dr["usprice2"].ToString());
                        }
                        catch { }
                        try
                        {
                        if (dr["usprice3"] != null)
                            m.USPrice3 = Convert.ToDecimal(dr["usprice3"].ToString());
                        }
                        catch { }
                        try
                        {
                        m.CanPrice1 = Convert.ToDecimal(dr["canprice1"].ToString());
                        }
                        catch { }
                        try
                        {
                        if (dr["canprice2"] != null)
                            m.CanPrice2 = Convert.ToDecimal(dr["canprice2"].ToString());
                        }
                        catch { }
                        try
                        {
                        if (dr["canprice3"] != null)
                            m.CanPrice3 = Convert.ToDecimal(dr["canprice3"].ToString());
                        }
                         catch { }
                         try
                        {
                        m.IntPrice1 = Convert.ToDecimal(dr["intprice1"].ToString());
                        }
                         catch { }
                        try
                        {
                        if (dr["intprice2"] != null)
                            m.IntPrice2 = Convert.ToDecimal(dr["intprice2"].ToString());
                        }
                        catch { }
                        try
                        {
                        if (dr["intprice3"] != null)
                            m.IntPrice3 = Convert.ToDecimal(dr["intprice3"].ToString());
                        }
                        catch { }
                        m.JFLink = dr["jflink"].ToString();
                        m.PaymentGateway = (ccProcessors)Enum.Parse(typeof(ccProcessors), dr["paymentgateway"].ToString(), true);
                        m.Description = dr["description"].ToString();
                        m.ShowContact = Convert.ToBoolean(dr["ShowContactInformation"]);
                        m.ShowBillingAddress = Convert.ToBoolean(dr["ShowBillingAddress"]);
                        m.ShowShippingAddress = Convert.ToBoolean(dr["ShowShippingAddress"]);  
                        magList.Add(m);
                    }
                }
                catch
                {  }

                if (String.IsNullOrEmpty(PubCode))
                    return magList;
                else
                    return magList.FindAll(x => x.PubCode.ToUpper() == PubCode.ToUpper());
            }
            catch
            {
                return magList;
            }
        }
    }
}

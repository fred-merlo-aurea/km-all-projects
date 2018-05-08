using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace KMPS_JF_Objects.Objects
{
    /// <summary>
    /// Represents magazines in payment page
    /// </summary>
    public class MagazineTest
    {
        public string Header { get; set; }
        public string Footer { get; set; }
        public string PubCode { get; set; }
        public int GroupID { get; set; }
        public int CustomerID { get; set; }
        public string CoverImage { get; set; }
        public string Description { get; set; }
        public string PageTitle { get; set; }
        public int Issues { get; set; }
        public string DigitalEditionLink { get; set; }
        public ccProcessors PaymentGateway { get; set; }
        public bool ShowContactInformation { get; set; }
        public bool ShowBillingAddress { get; set; }
        public bool ShowShippingAddress { get; set; }
        public string QunatityHeaderText { get; set; }
        public string PriceHeaderText { get; set; }
        public string TotalPriceHeaderText { get; set; }
        public List<Price> Prices { get; set; }


        public MagazineTest()
        {
            this.Header = string.Empty;
            this.Footer = string.Empty;
            this.PubCode = string.Empty;
            this.GroupID = -1;
            this.CustomerID = -1;
            this.CoverImage = string.Empty;
            this.PageTitle = string.Empty;
            this.Issues = -1;
            this.DigitalEditionLink = string.Empty;
            this.PaymentGateway = ccProcessors.AuthorizeNet;
            this.Description = string.Empty;
            this.ShowContactInformation = true;
            this.ShowBillingAddress = true;
            this.ShowShippingAddress = true;
            this.Prices = null;
        }

        public static List<MagazineTest> GetMagazines(string path, string PubCode)
        {
            List<MagazineTest> magList = new List<MagazineTest>();
            XElement doc = XElement.Load(path);
            var pcode = from p in doc.Descendants("magazines") select p;

            try
            {
                foreach (var code in pcode)
                {
                    MagazineTest m = new MagazineTest();
                    List<Price> pList = new List<Price>();
                    m.Header = code.Element("header").ToString();
                    m.Footer = code.Element("footer").ToString();
                    m.PubCode = code.Element("pubcode").ToString();
                    m.GroupID = Convert.ToInt32(code.Element("groupID").ToString());
                    m.CustomerID = Convert.ToInt32(code.Element("custID").ToString());
                    m.CoverImage = code.Element("coverimage").ToString();
                    m.PageTitle = code.Element("title").ToString();
                    m.Issues = Convert.ToInt32(code.Element("issues").ToString());
                    m.DigitalEditionLink = code.Element("jflink").ToString();
                    m.PaymentGateway = (ccProcessors)Enum.Parse(typeof(ccProcessors), code.Element("paymentgateway").ToString(), true);
                    m.Description = code.Element("description").ToString();
                    m.ShowContactInformation = Convert.ToBoolean(code.Element("ShowContactInformation"));
                    m.ShowBillingAddress = Convert.ToBoolean(code.Element("ShowBillingAddress"));
                    m.ShowShippingAddress = Convert.ToBoolean(code.Element("ShowShippingAddress"));
                    magList.Add(m);
                }

                if (String.IsNullOrEmpty(PubCode))
                    return magList;
                else
                    return magList.FindAll(x => x.PubCode.ToUpper() == PubCode.ToUpper());
            }
            catch
            {
                magList.Add(new MagazineTest());
                return magList;
            }
        }
    }
}

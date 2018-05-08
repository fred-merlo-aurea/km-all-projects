using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace ecn.webservice.CustomAPI
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://eforms.kmpsgroup.com/WattWebService/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class KmWattService : System.Web.Services.WebService
    {
        [WebMethod]
        public string AddProfile(string accessKey, CustomerData Profile, object[][] hUDF)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["AccessKey"].Equals(accessKey))
            {
                CustomerData cd = new CustomerData(Profile);
                WattLogic wl = new WattLogic();

                if (cd.EktronUserName != string.Empty && HelperFunctions.isValidEmail(cd.EktronUserName))
                {
                    return wl.AddProfile(cd, HelperFunctions.ToHashtable(hUDF), accessKey);
                }
                else
                {
                    return "Incorrect Email Address (EktronUserName)";
                }
            }
            else
            {
                //access key was incorrect so return false
                return "Incorrect AccessKey";
            }
        }

        [WebMethod]
        public CustomerData GetProfile(string accessKey, string Emailaddress, string pubCode)
        {
            WattLogic wl = new WattLogic();

            if (HelperFunctions.isValidEmail(Emailaddress))
            {
                return wl.GetProfile(pubCode, Emailaddress, accessKey);
            }
            else
            {
                throw new SoapException("Incorrect Email Address (EktronUserName)", SoapException.ClientFaultCode);
            }
        }



        [WebMethod]
        public string CreateUDF(string accessKey, string pubcode, string newUDF)
        {

            WattLogic wl = new WattLogic();
            if (newUDF.Trim() != string.Empty)
            {

                newUDF = newUDF.Replace(" ", "_").ToUpper();

                int v = wl.CreateUDF(pubcode, newUDF, accessKey);
                if (v > 0)
                    return "UDF Created Successfully";
                else if (v == -2)
                    return "UDF Name already exists";
                else
                    return "UDF NOT Created - try again";
            }
            else
            {
                return "UDF NOT Created - Cannot be blank";
            }
        }

        //[WebMethod]
        //public int GetGroupID(string accessKey, string pubCode)
        //{
        //    if (System.Configuration.ConfigurationManager.AppSettings["AccessKey"].Equals(accessKey))
        //    {
        //    System.Xml.Linq.XDocument xd = System.Xml.Linq.XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/PubCode.xml"));
        //    var gid = from x in xd.Descendants("Customer")
        //              where (string)x.Attribute("pubCode") == pubCode
        //              select (int)x.Element("GroupID");

        //    int groupID = gid.First();
        //        return groupID;
        //    }
        //    else
        //    {
        //        //access key was incorrect so return false
        //        throw new SoapException("Incorrect AccessKey", SoapException.ClientFaultCode);
        //    }
        //}
        [WebMethod]
        public List<GroupDataField> GetUDFList(string accessKey, string pubCode)
        {
            System.Xml.Linq.XDocument xd = System.Xml.Linq.XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/PubCode.xml"));
            var gid = from x in xd.Descendants("Customer")
                      where (string)x.Attribute("pubCode") == pubCode
                      select (int)x.Element("GroupID");

            int groupID = gid.First();
            return GroupDataFieldData.GetData(groupID);
        }
    }
}
using System;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using ecn.common.classes;
using ecn.communicator.classes;
using System.Data.SqlClient;
using ecn.webservice.classes;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace ecn.webservice.CustomAPI
{
    /// <summary>
    /// Summary description for AdvanstarServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AdvanstarServices : System.Web.Services.WebService
    {

        private readonly string AccessKey = "8666017F-AF14-40F2-937A-BCD76E0353DB"; //hardcoded for pharmalive account

        [WebMethod(Description = "Provides Authentication for Advanstar Websites . Parameters passed are username & password .<br>- Returns UserID or 0(failed login).")]
        public string Login(string ecnAccessKey, string emailaddress)
        {
            int emailID = 0;

            if (ecnAccessKey == AccessKey)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT e.EmailID FROM ecn5_communicator.dbo.emails e join ecn5_communicator.dbo.emailgroups eg on e.emailID = eg.emailID WHERE groupID = @groupID and emailaddress=@EmailAddress";
                cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar).Value = emailaddress;
                cmd.Parameters.Add("@groupID", SqlDbType.Int).Value = ConfigurationManager.AppSettings["Advanstar_MasterGroupID"].ToString();

                try
                {
                    emailID = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", cmd));

                    if (emailID == 0)
                    {
                        return SendResponse.response("Login", SendResponse.ResponseCode.Fail, 0, "LOGIN FAILED");
                    }
                    else
                    {
                        Emails e = Emails.GetEmailByID(emailID);

                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                        ns.Add("", "");

                        XmlSerializer serializer = new XmlSerializer(typeof(Emails));
                        StringWriter stringWriter = new StringWriter();
                        using (XmlWriter writer = new XmlTextWriterFormattedNoDeclaration(stringWriter))
                        {
                            serializer.Serialize(writer, e, ns);
                        }

                        string xmlText = stringWriter.ToString();


                        return SendResponse.response("Login", SendResponse.ResponseCode.Success, emailID, xmlText);
                    }
                }
                catch
                {
                    return SendResponse.response("Login", SendResponse.ResponseCode.Fail, 0, "UNKNOWN ERROR");
                }
            }
            else
            {
                return SendResponse.response("Login", SendResponse.ResponseCode.Fail, 0, "AUTHENTICATION FAILED - INVALID ACCESS KEY");
            }

        }

        private String UTF8ByteArrayToString(Byte[] characters)
        {

            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }


    }

    public class XmlTextWriterFormattedNoDeclaration : System.Xml.XmlTextWriter
    {
        public XmlTextWriterFormattedNoDeclaration(System.IO.TextWriter w)
            : base(w)
        {
            //Formatting = System.Xml.Formatting.Indented;
        }

        public override void WriteStartDocument()
        {
            // suppress
        }
    }



}

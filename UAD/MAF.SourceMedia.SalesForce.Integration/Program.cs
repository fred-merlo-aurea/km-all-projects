using KMPS.MD.Objects;
using MAF.SourceMedia.SalesForce.Integration.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MAF.SourceMedia.SalesForce.Integration
{
    public class Program
    {
        StreamWriter Log;
        int KMCommon_ApplicationID = int.Parse(ConfigurationManager.AppSettings["KMCommon_Application"].ToString());
        string AppName = "MAF.SourceMedia.SalesForce.Integration";
        string SMConnectionString = ConfigurationManager.ConnectionStrings["SourceMediaMasterDB"].ConnectionString;

        int quitonConsecutiveErrors = int.Parse(ConfigurationManager.AppSettings["quitonConsecutiveErrors"].ToString());
        int ErrorCount = 0;

        static void Main(string[] args)
        {
            Program p = new Program();
            p.Start();
        }

        private void Start()
        {
            List<int> lSubIDs = new List<int>();
            string StandardColumns = "s.SUBSCRIPTIONID, ps.Email, ps.FirstName, ps.LastName, ps.Title, ps.Company, ps.Address1, ps.Address2, ps.City, ps.regioncode, ps.ZipCode, ps.Country";
            string ResponseGroupID = string.Empty;
            bool bBusinessExists = false;
            bool bDemo1Exists = false;


            try
            {
                string path = Directory.GetCurrentDirectory();

                if (!Directory.Exists(path + "\\Log\\"))
                {
                    Directory.CreateDirectory(path + "\\Log\\");
                }

                Log = new StreamWriter(path + "\\Log\\" + DateTime.Now.ToString("MM-dd-yyyy") + ".log", true);
                LogWrite("Start Instance");

                //Get all records from SourceMedia where IsLead = 1 and created on Previous Data. (only Delta)

                DataFunctions.ConnectionString = SMConnectionString;

                List<Pubs> lpubs = Pubs.GetAll().FindAll(x => x.IsCirc == true);

                if (lpubs != null)
                {
                    foreach (Pubs p in lpubs)
                    {
                        LogWrite("PUBCODE : " + p.PubCode);

                        lSubIDs = new List<int>();
                        ResponseGroupID = string.Empty;
                        bBusinessExists = false;
                        bDemo1Exists = false;

                        DataTable dtDelta = Data.GetSalesForceIntegrationData(p.PubID);

                        if (dtDelta != null && dtDelta.Rows.Count > 0)
                        {
                            LogWrite(string.Format("    - {0} Records to sync.", dtDelta.Rows.Count));

                            for (int i = 0; i < dtDelta.Rows.Count; i++)
                            {
                                lSubIDs.Add(int.Parse(dtDelta.Rows[i]["SubscriptionID"].ToString()));
                            }

                            List<ResponseGroup> lResponseGroups = ResponseGroup.GetByPubID(p.PubID);

                            ResponseGroup rgBusiness = lResponseGroups.Find(r => r.ResponseGroupName.ToUpper() == "BUSINESS");
                            if (rgBusiness != null)
                            {

                                bBusinessExists = true;
                                ResponseGroupID = "<ResponseGroup ID=\"" + rgBusiness.ResponseGroupID.ToString() + "\"/>";
                            }

                            ResponseGroup rgDemo1 = lResponseGroups.Find(r => r.ResponseGroupName.ToUpper() == "DEMO1");
                            if (rgDemo1 != null)
                            {
                                bDemo1Exists = true;
                                ResponseGroupID = "<ResponseGroup ID=\"" + rgDemo1.ResponseGroupID.ToString() + "\"/>";
                            }

                            LogWrite(String.Format("    - Business Exists : {0} / Demo1 Exists : {1} ", (bBusinessExists ? "YES" : "NO"), (bDemo1Exists ? "YES" : "NO")));

                            DataTable dtSubscription = Subscriber.GetProductDimensionSubscriberData(lSubIDs, StandardColumns, (new List<int>() { p.PubID }), ResponseGroupID, null, string.Empty, 0);

                            foreach (DataRow drSubscription in dtSubscription.Rows)
                            {
                                try
                                {
                                    LeadData ld = new LeadData();

                                    LogWrite(String.Format("    - SubscriptionID : {0} / Emails : {1} / SyncTime : {2} ", drSubscription["SubscriptionID"].ToString(), drSubscription["Email"].ToString(), System.DateTime.Now.ToString("MM/dd/yy H:mm:ss")));

                                    ld.e_mail = drSubscription["Email"].ToString();

                                    ld.f_name = drSubscription["FirstName"].ToString();
                                    ld.l_name = drSubscription["LastName"].ToString();
                                    ld.title = drSubscription["Title"].ToString();
                                    ld.company = drSubscription["Company"].ToString();

                                    ld.street = drSubscription["Address1"].ToString();
                                    ld.addr2 = drSubscription["Address2"].ToString();
                                    ld.City = drSubscription["City"].ToString();
                                    ld.State = drSubscription["regioncode"].ToString();
                                    ld.Zip = drSubscription["ZipCode"].ToString();
                                    ld.Country = drSubscription["Country"].ToString();

                                    ld.acronym = p.PubCode;

                                    if (bBusinessExists)
                                        ld.bcode = drSubscription["BUSINESS"].ToString(); ;

                                    if (bDemo1Exists)
                                        ld.btitle = drSubscription["DEMO1"].ToString(); ;

                                    ld.SourceCode = p.PubCode + System.DateTime.Now.ToString("MMyy");
                                    ld.Rate = 0;
                                    ld.trackcode = "";
                                    ld.trial_expire_date = System.DateTime.Now.AddDays(13).ToString("MM/dd/yyyy");

                                    PostLeadtoSM(ld);

                                    ErrorCount = 0;

                                }
                                catch (Exception ex)
                                {
                                    LogWrite("ERROR : " + ex.Message);

                                    StringBuilder sbEx = new StringBuilder();
                                    sbEx.AppendLine("An exception Happened in MAF.SourceMedia.SalesForce.Integration.</br>");
                                    sbEx.AppendLine("<b>Exception Message:</b>" + ex.Message + "</br>");
                                    sbEx.AppendLine("<b>Exception Source:</b>" + ex.Source + "</br>");
                                    sbEx.AppendLine("<b>Stack Trace:</b>" + ex.StackTrace + "</br>");
                                    sbEx.AppendLine("<b>Inner Exception:</b>" + ex.InnerException + "</br>");

                                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, string.Format("{0} engine({1}) encountered an exception.", AppName, System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)), KMCommon_ApplicationID, sbEx.ToString());

                                    ErrorCount++;

                                    if (ErrorCount >= 5)
                                    {
                                        KM.Common.Entity.ApplicationLog.LogCriticalError(new Exception("Error Threshold reached - Quitting Sync"), string.Format("{0} engine({1}) Error Threshold reached - Quitting Sync.", AppName, System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)), KMCommon_ApplicationID, "Error Threshold reached - Quitting Sync");
                                        Environment.Exit(0);
                                    }

                                }
                            }
                        }
                        else
                        {
                            LogWrite("    - No Records to sync ");
                        }
                    }
                }
                else
                {
                    LogWrite("No CIRC Products.");
                }
            }
            catch (Exception ex1)
            {
                StringBuilder sbEx = new StringBuilder();
                sbEx.AppendLine("An exception Happened in MAF.SourceMedia.SalesForce.Integration.</br>");
                sbEx.AppendLine("<b>Exception Message:</b>" + ex1.Message + "</br>");
                sbEx.AppendLine("<b>Exception Source:</b>" + ex1.Source + "</br>");
                sbEx.AppendLine("<b>Stack Trace:</b>" + ex1.StackTrace + "</br>");
                sbEx.AppendLine("<b>Inner Exception:</b>" + ex1.InnerException + "</br>");

                KM.Common.Entity.ApplicationLog.LogCriticalError(ex1, string.Format("{0} engine({1}) encountered an exception.", AppName, System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)), KMCommon_ApplicationID, sbEx.ToString());

            }
        }

        private void PostLeadtoSM(LeadData lead)
        {
            //XmlWriterSettings settings = new XmlWriterSettings();
            //settings.OmitXmlDeclaration = true;

            ////Create our own namespaces for the output
            //XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            ////Add an empty namespace and empty value
            //ns.Add("", "");

            //XmlSerializer x = new XmlSerializer(typeof(LeadData));

            //StringWriter textWriter = new StringWriter();

            //x.Serialize(textWriter, lead, ns);

            //LogWrite(textWriter.ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["WriteXMLtoConsole"].ToString()));
            
            //serializer.Serialize(System.Xml.XmlWriter.Create(sb, new System.Xml.XmlWriterSettings { OmitXmlDeclaration = true, Indent = true }), typeToSerialize);

            SMSalesForceAPI.Service s = new SMSalesForceAPI.Service();

            string serializedLeadDate = Serialize(lead);
            LogWrite(serializedLeadDate, Convert.ToBoolean(ConfigurationManager.AppSettings["WriteXMLtoConsole"].ToString()));
            string response = s.GenerateLeadAndGetSalesRepCode(serializedLeadDate);

            if (response.Contains("Error Occurred"))
            {
                throw new Exception(response);
            }
        }

       private string Serialize<LeadData>(LeadData ld)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            var writer = new StringWriter();
            XmlWriter xmlWriter = XmlWriter.Create(writer, settings);

            XmlSerializerNamespaces names = new XmlSerializerNamespaces();
            names.Add("", "");

            XmlSerializer serializer = new XmlSerializer(typeof(LeadData));

            serializer.Serialize(xmlWriter, ld, names);
            var xml = writer.ToString();
            return xml;
        }

        void LogWrite(string text, bool WritetoConsole = true)
        {
            if (WritetoConsole)
                Console.WriteLine(text.ToString());

            Log.AutoFlush = true;
            Log.WriteLine(DateTime.Now.ToString() + " : " + text);
            Log.Flush();
        }
    }
}

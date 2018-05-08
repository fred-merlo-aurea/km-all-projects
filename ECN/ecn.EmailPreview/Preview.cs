using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Configuration;
using ECN_Framework_Common.Functions;

namespace EmailPreview
{
    public class Preview
    {
        public Preview() { }

        public string CreateCustomerEmailTestFromEngine(int customerID, int blastID)
        {
            LitmusApi la = new LitmusApi();
            EmailTest et = new EmailTest();
            EmailTest etReturn = null;

            
            et.Sandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["LitmusSandbox"].ToString());

            int baseChannelID = Convert.ToInt32(ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(customerID, false).BaseChannelID);
            et.UserGuid = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(baseChannelID).BaseChannelGuid.ToString();
            //et.Subject = "testing";
            try
            {
                etReturn = la.CreateEmailTests(et);
            }
            catch (Exception ex)
            {
            }

            int tries = 1;
            while ((etReturn == null || etReturn.Id <= 0) && tries < 10)
            {
                tries++;
                KM.Common.Entity.ApplicationLog.LogNonCriticalError("TestID from Litmus is invalid.  TestID: " +  et.Id.ToString(), "EmailPreview.Preview.CreateCustomerEmailTestFromEngine", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                try
                {
                    etReturn = la.CreateEmailTests(et);
                }
                catch (Exception)
                {
                }
            }


            int userID = Convert.ToInt32(ECN_Framework_DataLayer.DataFunctions.ExecuteScalar("SELECT CreatedUserID FROM [Blast] WHERE BlastID = " + blastID, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString()));

            KMPlatform.Entity.User CurrentUser = new KMPlatform.Entity.User();
            CurrentUser = new KMPlatform.BusinessLogic.User().SelectUser(userID, true);

            // Link Test
            ECN_Framework_Entities.Communicator.Blast blast =  ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(blastID, false);
            int layoutID = blast.LayoutID.GetValueOrDefault();
            string html = ECN_Framework_BusinessLayer.Communicator.Layout.GetPreviewNoAccessCheck(layoutID, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML, false,blast.CustomerID.Value);

            html = Regex.Replace(html, Regex.Escape("http://%%unsubscribelink%%/"), "%%unsubscribelink%%", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, Regex.Escape("http://%%emailtofriend%%/"), "%%emailtofriend%%", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, Regex.Escape("http://%%publicview%%/"), "%%publicview%%", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, Regex.Escape("http://%%reportabuselink%%/"), "%%reportabuselink%%", RegexOptions.IgnoreCase);

            html = Regex.Replace(html, Regex.Escape("%%publicview%%/"), "%%publicview%%", RegexOptions.IgnoreCase);

            html = Regex.Replace(html, Regex.Escape("http://%%unsubscribelink%%"), "%%unsubscribelink%%", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, Regex.Escape("http://%%emailtofriend%%"), "%%emailtofriend%%", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, Regex.Escape("http://%%publicview%%"), "%%publicview%%", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, Regex.Escape("http://%%reportabuselink%%"), "%%reportabuselink%%", RegexOptions.IgnoreCase);

            string unsubscribelinkpage = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/Unsubscribe.aspx";
            html = Regex.Replace(html, Regex.Escape("%%unsubscribelink%%"), unsubscribelinkpage, RegexOptions.IgnoreCase);

            string emailtofriendpage = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/emailtofriend.aspx";
            html = Regex.Replace(html, Regex.Escape("%%emailtofriend%%"), emailtofriendpage, RegexOptions.IgnoreCase);

            string publicviewpage = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/publicPreview.aspx";
            html = Regex.Replace(html, Regex.Escape("%%publicview%%"), publicviewpage, RegexOptions.IgnoreCase);

            string reportabusepage = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/reportspam.aspx";
            html = Regex.Replace(html, Regex.Escape("%%reportabuselink%%"), reportabusepage, RegexOptions.IgnoreCase); 

            html = html.Replace("\"", "'");

            html = CleanLinks(html);

            int linkTestID = 0;

            LinkTest lt = new LinkTest();
            try
            {
                lt = CreateLinkTest(html);
                
            }
            catch (Exception)
            {
            }

            if(lt != null)
                  linkTestID = lt.Id;

            try
            {
                //save emailTestID and zipFileURL to a table with ref to BlastID
                ECN_Framework_Entities.Communicator.EmailPreview ep = new ECN_Framework_Entities.Communicator.EmailPreview();
                ep.BlastID = Convert.ToInt32(blastID);
                ep.CreatedByID = userID;
                ep.CustomerID = customerID;
                ep.DateCreated = DateTime.Now;
                ep.EmailTestID = etReturn.Id;
                ep.TimeCreated = DateTime.Now.TimeOfDay;
                ep.ZipFile = etReturn.ZipFile;
                ep.LinkTestID = linkTestID;
                Guid bcGuid = Guid.NewGuid();
                
                Guid.TryParse(etReturn.UserGuid, out bcGuid);
                ep.BaseChannelGUID = bcGuid;
                ECN_Framework_BusinessLayer.Communicator.EmailPreview.Insert(ep);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "EmailPreview.Preview.CreateCustomerEmailTestFromEngine", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), "Unable to get a valid return from Litmus, we did send out the blast without the preview");
                return string.Empty;
            }
            return etReturn.InboxGuid + "@emailtests.com";
        }

        private static String CleanLinks(String strText)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(strText);
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//a[@href]");
            System.Collections.Generic.List<string> linkList = new System.Collections.Generic.List<string>();
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    string href = "";
                    HtmlAgilityPack.HtmlAttribute attHREF = node.Attributes["href"];
                    if (attHREF != null)
                    {
                        href = attHREF.Value;
                    }
                    linkList.Add(href);
                }
            }

            nodes = doc.DocumentNode.SelectNodes("//area[@href]");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    string href = "";
                    HtmlAgilityPack.HtmlAttribute attHREF = node.Attributes["href"];
                    if (attHREF != null)
                    {
                        href = attHREF.Value;
                    }
                    linkList.Add(href);
                }
            }

            for (int aLoop = 0; aLoop < linkList.Count; aLoop++)
            {
                strText = strText.Replace(linkList[aLoop], LinkCleanUP(linkList[aLoop]));
            }
            return strText;
        }

        private static string LinkCleanUP(string link)
        {
            if(string.IsNullOrWhiteSpace(link))
            {
                return "";
            }            
            link = link.Replace("&amp;", "&");
            link = link.Replace("&lt;", "<");
            link = link.Replace("&gt;", ">");
            link = RegexUtilities.GetCleanUrlContent(link);
            return link;
        }

        public static string[] GetSpamSeedAddresses()
        {
            string[] returnList = null;
            LitmusApi api = new LitmusApi();
            try
            {
                returnList = api.GetSpamSeedAddresses().ToArray();
            }
            catch (Exception)
            {
            }

            int tries = 1;
            while ((returnList == null || returnList.GetUpperBound(0) == 0) && tries < 10)
            {
                tries++;
                KM.Common.Entity.ApplicationLog.LogNonCriticalError("LitmusApi.GetSpamSeedAddresses() failed to return a list", "EmailPreview.Preview.GetSpamSeedAddresses", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                try
                {
                    returnList = api.GetSpamSeedAddresses().ToArray();
                }
                catch (Exception)
                {
                }
            }

            try
            {
                string checkValue = returnList[0];
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "EmailPreview.Preview.GetSpamSeedAddresses", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), "Unable to get a list from Litmus, we did send out the blast without the preview");
            }

            return returnList;
        }
        public List<EmailResult> GetExistingTestResults(int testID, string zipFile = "")
        {
            LitmusApi la = new LitmusApi();

            List<TestingApplication> lta = new List<TestingApplication>();

            lta.AddRange(la.GetExistingTestResults(testID));

            List<EmailResult> list = new List<EmailResult>();

            if (lta != null)
            {
                foreach (var result in lta)
                {
                    EmailResult er = new EmailResult();
                    er.EmailTestID = testID;
                    er.ZipFile = zipFile;

                    er.ApplicationLongName = result.ApplicationLongName;
                    er.ApplicationName = result.ApplicationName;
                    er.AverageTimeToProcess = result.AverageTimeToProcess;
                    er.IsBusinessClient = result.BusinessOrPopular.HasValue ? result.BusinessOrPopular.Value : false;
                    er.Completed = result.Completed.HasValue ? result.Completed.Value : false;
                    er.DesktopClient = result.DesktopClient.HasValue ? result.DesktopClient.Value : false;
                    er.FoundInSpam = result.FoundInSpam.HasValue ? result.FoundInSpam.Value : false;

                    er.FullpageImage = result.FullpageImage;
                    er.FullpageImageContentBlocking = result.FullpageImageContentBlocking;
                    er.FullpageImageNoContentBlocking = result.FullpageImageNoContentBlocking;

                    er.FullpageImageThumb = result.FullpageImageThumb;
                    er.FullpageImageThumbContentBlocking = result.FullpageImageThumbContentBlocking;
                    er.FullpageImageThumbNoContentBlocking = result.FullpageImageThumbNoContentBlocking;

                    er.Id = result.Id;
                    er.PlatformLongName = result.PlatformLongName;
                    er.PlatformName = result.PlatformName;
                    er.ResultType = Enum.Parse(typeof(EmailPreview.EmailResultEnum.ResultType), result.ResultType.ToString()).ToString();
                    er.SpamScore = result.SpamScore;
                    er.State = result.State;
                    er.Status = result.Status;
                    er.SupportsContentBlocking = result.SupportsContentBlocking.HasValue ? result.SupportsContentBlocking.Value : false;

                    er.WindowImage = result.WindowImage;
                    er.WindowImageContentBlocking = result.WindowImageContentBlocking;
                    er.WindowImageNoContentBlocking = result.WindowImageNoContentBlocking;

                    er.WindowImageThumb = result.WindowImageThumb;
                    er.WindowImageThumbContentBlocking = result.WindowImageThumbContentBlocking;
                    er.WindowImageThumbNoContentBlocking = result.WindowImageThumbNoContentBlocking;
                    er.SpamHeaders = result.SpamHeaders;

                    list.Add(er);
                }

                SetResultSummary(list);
            }
            return list;
        }
        public CodeAnalysisTest GetCodeAnalysisTest(string html)
        {
            LitmusApi la = new LitmusApi();
            CodeAnalysisTest caTest = la.GetCodeAnalysisTest("\"" + html + "\"");
            return caTest;
        }
        private LinkTest CreateLinkTest(string html)
        {
            LitmusApi la = new LitmusApi();
            LinkTest lt = la.CreateLinkTest("\"" + html + "\"");
            return lt;
        }
        public LinkTest GetLinkTestResults(int LinkTestID)
        {
            LitmusApi la = new LitmusApi();
            LinkTest lt = la.GetLinkTestResults(LinkTestID);
            return lt;
        }
        public List<TestingApplication> GetTestingApplication()
        {
            LitmusApi la = new LitmusApi();
            List<TestingApplication> ta = la.GetTestingApplication();
            return ta;
        } 
        #region Private Methods
        private void SetResultSummary(List<EmailResult> results)
        {
            foreach (var result in results)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("<div><strong>{0}</strong><br />", result.ApplicationLongName.ToUpper());

                if (result.ResultType.ToLower() == "spam" && (!result.ApplicationName.Equals(EmailResultEnum.EmailSpam.htmlvalidation.ToString())) && !result.ApplicationName.Equals(EmailResultEnum.EmailSpam.linkcheck.ToString()))
                {
                    sb.AppendFormat("<strong>{0}</strong> - ResultType<br />", result.ResultType);
                    sb.AppendFormat("<strong>Spam Score = {0}</strong><br/>", result.SpamScore);
                    sb.AppendFormat("<strong>Found In Spam = {0}</strong><br/>", result.FoundInSpam);
                    if (result.SpamHeaders != null)
                    {
                        foreach (var header in result.SpamHeaders)
                        {
                            if (!string.IsNullOrEmpty(header.Key) && !string.IsNullOrEmpty(header.Description))
                                sb.AppendFormat("<span>{0} :: {1}</span><br />", header.Key, header.Description);
                        }
                    }
                }
                else if (result.ResultType.ToLower() == "spam" && (result.ApplicationName.Equals(EmailResultEnum.EmailSpam.htmlvalidation.ToString())) || result.ApplicationName.Equals(EmailResultEnum.EmailSpam.linkcheck.ToString()))
                {
                    if (result.SpamHeaders != null)
                    {
                        foreach (var header in result.SpamHeaders)
                        {
                            if (!string.IsNullOrEmpty(header.Key) && !string.IsNullOrEmpty(header.Description))
                                sb.AppendFormat("<span>{0} :: {1}</span><br />", header.Key, header.Description);
                        }
                    }
                }
                else
                {
                    sb.AppendFormat("<img src=\"http://{0}\" ></img>", result.FullpageImageThumb);
                }
                sb.AppendLine("<br/>");
                sb.AppendLine("----------------------------------------------------</div>");

                result.Summary = sb;
            }
        }
        #endregion
    }
}

//using ecn.common.classes;
//using ecn.communicator.classes.EmailWriter;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Net.Mail;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace ecn.communicator.classes
//{

//    public class EmailQueue
//    {

//        public static void InserttoQueue(EmailBlast emailblast, DataRow dr, DataRow[] drArray)
//        {
//            int BlastID = int.Parse(dr["BlastID"].ToString());
//            int EmailID = int.Parse(dr["EmailID"].ToString());
//            string FromEmail_BounceAddress = dr["BounceAddress"].ToString().Trim();
//            string ToEmailAddress = dr["EmailAddress"].ToString().Trim();

//            string msg = GetMessage(emailblast, dr, drArray);

//            //Start of handling of period in content
//            StringBuilder tmpMessage = new StringBuilder();
//            string[] parts = msg.Split(new string[] { "\r\n" }, StringSplitOptions.None);
//            for (int i = 0; i < parts.Length; i++)
//            {
//                if (parts[i].Length > 0)
//                {
//                    if (parts[i].Substring(0, 1) != ".")
//                    {
//                        tmpMessage.Append(parts[i]);
//                        tmpMessage.Append("\r\n");
//                    }
//                    else
//                    {
//                        tmpMessage.Append(".");
//                        tmpMessage.Append(parts[i]);
//                        tmpMessage.Append("\r\n");
//                    }
//                }
//                else
//                {
//                    tmpMessage.Append(parts[i]);
//                    tmpMessage.Append("\r\n");
//                }
//            }
//            msg = tmpMessage.ToString();
//            //end of handling period in content
//            byte[] MessageinBytes = Encoding.ASCII.GetBytes(String.Copy(msg) + "\r\n.\r\n");

//            SqlCommand cmd = new SqlCommand();
//            cmd.CommandType = CommandType.Text;
//            cmd.CommandText = "Insert into EmailQueue (BlastID, EmailID, FromAddress, ToAddress, MessageBytes) values (@BlastID, @EmailID, @FromAddress, @ToAddress, @MessageBytes)";
//            cmd.Parameters.Add(new SqlParameter("@BlastID", emailblast.BlastID));
//            cmd.Parameters.Add(new SqlParameter("@EmailID", EmailID));
//            cmd.Parameters.Add(new SqlParameter("@FromAddress", FromEmail_BounceAddress));
//            cmd.Parameters.Add(new SqlParameter("@ToAddress", ToEmailAddress));
//            cmd.Parameters.Add(new SqlParameter("@MessageBytes", MessageinBytes));

//            DataFunctions.Execute(cmd);
//        }

//        private static void SendEmailNotification(string subject, string body)
//        {
//            MailMessage message = new MailMessage();
//            message.From = new MailAddress("domain_admin@teckman.com");
//            message.To.Add(ConfigurationManager.AppSettings["SendTo"]);
//            message.Subject = "Engine: " + System.AppDomain.CurrentDomain.FriendlyName.ToString() + " - " + subject;
//            message.Body = body;

//            SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
//            smtp.Send(message);
//        }


//        private static string GetMessage(EmailBlast emailBlast, DataRow dr, DataRow[] emailProfileDataSet)
//        {
//            StringBuilder sb;

//            StringBuilder html_body = new StringBuilder();
//            StringBuilder text_body = new StringBuilder();
//            StringBuilder dynamic_subject = new StringBuilder();
//            StringBuilder dynamic_fromEmail = new StringBuilder();
//            StringBuilder dynamic_fromName = new StringBuilder();
//            StringBuilder dynamic_replyTo = new StringBuilder();

//            try
//            {
//                // create message id 
//                string message_id = dr["BlastID"].ToString() + "." + dr["EmailID"].ToString() + "x" + QuotedPrintable.RandomString(5, true) + "@enterprisecommunicationnetwork.com";
//                string boundry_tag = "_=COMMUNICATOR=_" + QuotedPrintable.RandomString(32, true);
//                boundry_tag = boundry_tag.ToLower();

//                //Replace all codesnippets in the HTML email body
//                for (int i = 0; i < emailBlast.BreakupHTMLMail.Length; i++)
//                {
//                    string line_data = emailBlast.BreakupHTMLMail.GetValue(i).ToString();
//                    if (i % 2 == 0)
//                        html_body.Append(line_data);
//                    else
//                        html_body.Append(dr[line_data].ToString());
//                }
//                for (int i = 0; i < emailBlast.BreakupTextMail.Length; i++)
//                {
//                    string line_data = emailBlast.BreakupTextMail.GetValue(i).ToString();
//                    if (i % 2 == 0)
//                        text_body.Append(line_data);
//                    else
//                        text_body.Append(dr[line_data].ToString());
//                }

//                #region Dynamic tags

//                //Removing this because it's overwriting content that's already been processed JWelter 5/20/2014
//                //replace all of the dynamic tags content text and html
//                if (emailBlast.DynamicTags != null && emailBlast.DynamicTags.Count > 0)
//                {
//                    #region Replace Dynamic tag

//                    DataTable dt2 = null;
//                    foreach (var tag in emailBlast.DynamicTags)
//                    {
//                        if (dr[tag].ToString().Trim().Length > 0)
//                        {
//                            string sqlBlastQuery = " SELECT * " +
//                            " FROM Blast " +
//                            " WHERE BlastID=" + emailBlast.BlastID + " ";
//                            DataTable dt = DataFunctions.GetDataTable(sqlBlastQuery);
//                            DataRow blast_info = dt.Rows[0];

//                            StringBuilder DynamicContent = new StringBuilder();
//                            StringBuilder DynamicContentText = new StringBuilder();

//                            //Have to add img rewriting here so dynamic tags get their img's rewritten 6/3/2014
//                            dt2 = DataFunctions.GetDataTable("SELECT * FROM Content WHERE ContentID=" + dr[tag].ToString() + " and IsDeleted = 0");
//                            int userID = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastUserByBlastID(Convert.ToInt32(dr["BlastID"].ToString()));
//                            KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByUserID(userID, false);
//                            ECN_Framework_Entities.Communicator.BlastAbstract ba = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(Convert.ToInt32(dr["BlastID"].ToString()), user, false);
//                            try
//                            {
//                                if (blast_info["EnableCacheBuster"].ToString().ToLower().Equals("true"))
//                                {
//                                    DynamicContent.Append(TemplateFunctions.imgRewriter(dt2.Rows[0]["ContentSource"].ToString(), Convert.ToInt32(dr["BlastID"].ToString())));
//                                    DynamicContentText.Append(TemplateFunctions.imgRewriter(dt2.Rows[0]["ContentText"].ToString(), Convert.ToInt32(dr["BlastID"].ToString())));
//                                }
//                                else
//                                {
//                                    DynamicContent.Append(dt2.Rows[0]["ContentSource"].ToString());
//                                    DynamicContentText.Append(dt2.Rows[0]["ContentText"].ToString());
//                                }
//                            }
//                            catch
//                            {
//                                DynamicContent.Append(dt2.Rows[0]["ContentSource"].ToString());
//                                DynamicContentText.Append(dt2.Rows[0]["ContentText"].ToString());
//                            }
//                            //Adding this to parse dynamic content and replace code snippets: JWelter 5/20/2014

//                            ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck(user.CustomerID);

//                            DynamicContent = DynamicContent.Replace(DynamicContent.ToString(), TemplateFunctions.LinkReWriter(DynamicContent.ToString(), blast_info, user.CustomerID.ToString(), ConfigurationManager.AppSettings["Communicator_VirtualPath"].ToString(), cc.getHostName()));
//                            DynamicContentText = DynamicContentText.Replace(DynamicContentText.ToString(), TemplateFunctions.LinkReWriterText(DynamicContentText.ToString(), blast_info, user.CustomerID.ToString(), ConfigurationManager.AppSettings["Communicator_VirtualPath"].ToString(), cc.getHostName(), ""));

//                            Regex r = new Regex("%%"); // Split on percents.
//                            Array ContentCodeSnippets = r.Split(DynamicContent.ToString());
//                            DynamicContent = new StringBuilder();
//                            for (int i = 0; i < ContentCodeSnippets.Length; i++)
//                            {
//                                try
//                                {
//                                    string line_data = ContentCodeSnippets.GetValue(i).ToString();
//                                    if (i % 2 == 0)
//                                        DynamicContent.Append(line_data);
//                                    else
//                                        DynamicContent.Append(dr[line_data].ToString());
//                                }
//                                catch (Exception ex)
//                                {
//                                }
//                            }

//                            Array ContentCodeSnippetsText = r.Split(DynamicContentText.ToString());
//                            DynamicContentText = new StringBuilder();
//                            for (int i = 0; i < ContentCodeSnippetsText.Length; i++)
//                            {
//                                try
//                                {
//                                    string line_data = ContentCodeSnippetsText.GetValue(i).ToString();
//                                    if (i % 2 == 0)
//                                        DynamicContentText.Append(line_data);
//                                    else
//                                        DynamicContentText.Append(dr[line_data].ToString());
//                                }
//                                catch (Exception ex)
//                                {

//                                }
//                            }
//                            html_body = html_body.Replace(tag, DynamicContent.ToString());
//                            text_body = text_body.Replace(tag, DynamicContentText.ToString());

//                            if (html_body.ToString().IndexOf("##TRANSNIPPET|") > 0)
//                            {
//                                Regex transnippetRegEx = new Regex("#{2}.*#{2}");
//                                MatchCollection transnippetMatchs = transnippetRegEx.Matches(html_body.ToString());
//                                ArrayList Transnippet = new ArrayList();
//                                ArrayList TransnippetTables = new ArrayList();
//                                ArrayList TransnippetTablesTxt = new ArrayList();
//                                int TransnippetsCount = -1;
//                                if (transnippetMatchs.Count > 0)
//                                {

//                                    for (int i = 0; i < transnippetMatchs.Count; i++)
//                                    {
//                                        Transnippet.Add(transnippetMatchs[i].Value.ToString());
//                                    }

//                                    char[] splitter = { '|' };
//                                    char[] udfSplitter = { ',' };


//                                    for (int i = 0; i < Transnippet.Count; i++)
//                                    {
//                                        //Just Build the Table Frame Once for all the emails for the Transnippet History 
//                                        string output_htm = "", output_txt = "";
//                                        string[] transnippetSplits = (((Transnippet[i].ToString()).Replace("##", "")).Replace("$$", "")).ToString().Split(splitter);
//                                        if (transnippetSplits.Length > 0)
//                                        {
//                                            //[3]rd split is the comma separated list of UDF's. split them & build the column Headers. 
//                                            string[] transnippetUDFs = transnippetSplits[2].ToString().Split(udfSplitter);

//                                            //[4]th split is the Style of the Transnippet table Header. 
//                                            //[5]th split is the Style of the Transnippet table Items / Cells. 
//                                            output_htm += "<table cellpadding=1 cellspacing=1 style=\"" + transnippetSplits[4].ToString().Replace("TBL-STYLE=", "") + "\">";
//                                            output_txt += "\n";

//                                            if (transnippetUDFs.Length > 0)
//                                            {
//                                                output_htm += "<tr style=\"" + transnippetSplits[3].ToString().Replace("HDR-STYLE=", "") + "\">";
//                                                output_txt += "\n";

//                                                //Now Make the Header for this Table.
//                                                for (int j = 0; j < transnippetUDFs.Length; j++)
//                                                {
//                                                    output_htm += "<td><b>" + transnippetUDFs[j].ToString() + "</b></td>";
//                                                    output_txt += transnippetUDFs[j].ToString() + "\t";
//                                                }
//                                                output_htm += "</tr>";

//                                                //Add the Transnippet String here. It will be replaced with the real data in each cell later in the next method.
//                                                output_htm += Transnippet[i].ToString();
//                                                output_htm += "</table>";
//                                                output_txt += Transnippet[i].ToString();
//                                            }
//                                            try
//                                            {
//                                                TransnippetTables.Add(output_htm.ToString());
//                                                TransnippetTablesTxt.Add(output_txt.ToString());
//                                            }
//                                            catch (Exception ex)
//                                            {
//                                                string e = ex.ToString();
//                                            }
//                                        }
//                                    }
//                                }

//                                TransnippetsCount = Transnippet.Count;

//                                TransnippetHolder.Transnippet = Transnippet;
//                                TransnippetHolder.TransnippetsCount = TransnippetsCount;
//                                TransnippetHolder.TransnippetTablesHTML = TransnippetTables;
//                                TransnippetHolder.TransnippetTablesTxt = TransnippetTablesTxt;
//                            }
//                            else
//                            {
//                                if (ContentTransnippet.CheckForTransnippet(html_body.ToString()) <= 0)
//                                {
//                                    TransnippetHolder.TransnippetsCount = 0;
//                                }
//                            }
//                        }
//                    }

//                    #endregion
//                }
//                #endregion Dynamic tags

//                #region Social Media Replace
//                if (emailBlast.SocialShareUsed != null && emailBlast.SocialShareUsed.Count > 0)
//                {
//                    KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));

//                    foreach (KeyValuePair<int, string> sm in emailBlast.SocialShareUsed)
//                    {
//                        if (sm.Key.Equals(5))
//                        {
//                            html_body = html_body.Replace(sm.Value, System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"].ToString() + "/engines/emailtofriend.aspx?e=" + dr["EmailID"].ToString() + "&b=" + dr["BlastID"].ToString());
//                            text_body = text_body.Replace(sm.Value, System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"].ToString() + "/engines/emailtofriend.aspx?e=" + dr["EmailID"].ToString() + "&b=" + dr["BlastID"].ToString());
//                        }
//                        else if (sm.Key.Equals(4))
//                        {

//                        }
//                        else
//                        {
//                            if (ec != null && ec.ID > 0)
//                            {
//                                string encryptedQuery = string.Empty;
//                                string queryString = string.Empty;

//                                queryString = "b=" + dr["BlastID"].ToString() + "&g=" + dr["GroupID"].ToString() + "&e=" + dr["EmailID"].ToString() + "&m=" + sm.Key.ToString();

//                                encryptedQuery = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(queryString, ec));

//                                html_body = html_body.Replace(sm.Value, System.Configuration.ConfigurationManager.AppSettings["Social_DomainPath"] + System.Configuration.ConfigurationManager.AppSettings["SocialClick"] + encryptedQuery + "");
//                                text_body = text_body.Replace(sm.Value, System.Configuration.ConfigurationManager.AppSettings["Social_DomainPath"] + System.Configuration.ConfigurationManager.AppSettings["SocialClick"] + encryptedQuery + "");
//                            }
//                        }
//                    }

//                }

//                #endregion

//                #region Dynamic Personalization

//                // Replace all "fields" in the email subject
//                for (int i = 0; i < emailBlast.BreakupSubject.Length; i++)
//                {
//                    string line_data = emailBlast.BreakupSubject.GetValue(i).ToString();
//                    if (i % 2 == 0)
//                        dynamic_subject.Append(line_data);
//                    else
//                        dynamic_subject.Append(System.Web.HttpUtility.HtmlDecode(dr[line_data].ToString()));
//                }

//                //--
//                //--Added for Email Personalization - Ashok 01/12/09
//                //--
//                for (int i = 0; i < emailBlast.BreakupFromEmail.Length; i++)
//                {
//                    string line_data = emailBlast.BreakupFromEmail.GetValue(i).ToString();
//                    if (i % 2 == 0)
//                        dynamic_fromEmail.Append(line_data);
//                    else
//                        dynamic_fromEmail.Append(System.Web.HttpUtility.HtmlDecode(dr[line_data].ToString()));
//                }

//                // Replace all "fields" in the email subject
//                for (int i = 0; i < emailBlast.BreakupFromName.Length; i++)
//                {
//                    string line_data = emailBlast.BreakupFromName.GetValue(i).ToString();
//                    if (i % 2 == 0)
//                        dynamic_fromName.Append(line_data);
//                    else
//                        dynamic_fromName.Append(System.Web.HttpUtility.HtmlDecode(dr[line_data].ToString()));
//                }

//                // Replace all "fields" in the email subject
//                for (int i = 0; i < emailBlast.breakupReplyToEmail.Length; i++)
//                {
//                    string line_data = emailBlast.breakupReplyToEmail.GetValue(i).ToString();
//                    if (i % 2 == 0)
//                        dynamic_replyTo.Append(line_data);
//                    else
//                        dynamic_replyTo.Append(System.Web.HttpUtility.HtmlDecode(dr[line_data].ToString()));
//                }

//                #endregion

//                # region OLD Transnippet code - DONOTDELETE
//                //Check to see if there are any Transnippets & replace'em with the history data.
//                if (TransnippetHolder.TransnippetsCount > 0)
//                {
//                    char[] splitter = { '|' };
//                    char[] udfSplitter = { ',' };
//                    for (int i = 0; i < TransnippetHolder.Transnippet.Count; i++)
//                    {
//                        string output_htm = "", output_txt = "";
//                        string[] transnippetSplits = (((TransnippetHolder.Transnippet[i].ToString()).Replace("##", "")).Replace("$$", "")).ToString().Split(splitter);
//                        if (transnippetSplits.Length > 0)
//                        {
//                            //split the UDF's now.
//                            string[] transnippetUDFs = transnippetSplits[2].ToString().Split(udfSplitter);

//                            if (emailProfileDataSet.Length > 0 && transnippetUDFs.Length > 0)
//                            {
//                                for (int j = 0; j < emailProfileDataSet.Length; j++)
//                                {
//                                    output_htm += "<tr>";
//                                    output_txt += "\n";
//                                    for (int k = 0; k < transnippetUDFs.Length; k++)
//                                    {
//                                        try
//                                        {
//                                            output_htm += "<td>" + emailProfileDataSet[j][transnippetUDFs[k].ToString()].ToString() + "</td>";
//                                            output_txt += emailProfileDataSet[j][transnippetUDFs[k].ToString()].ToString() + "\t";
//                                        }
//                                        catch
//                                        {
//                                            output_htm += "<td>&nbsp;</td>";
//                                            output_txt += "\t";
//                                        }
//                                    }
//                                    output_htm += "</tr>";
//                                }
//                                html_body.Replace(TransnippetHolder.Transnippet[i].ToString(), TransnippetHolder.TransnippetTablesHTML[i].ToString().Replace(TransnippetHolder.Transnippet[i].ToString(), output_htm));
//                                text_body.Replace(TransnippetHolder.Transnippet[i].ToString(), TransnippetHolder.TransnippetTablesTxt[i].ToString().Replace(TransnippetHolder.Transnippet[i].ToString(), output_txt));
//                            }
//                        }
//                    }
//                }

//                #endregion

//                #region Check for new version of transnippets - WGH
//                DataTable emailRowsDT = new DataTable();

//                //html 
//                int transTotalCount = ContentTransnippet.CheckForTransnippet(html_body.ToString());
//                if (transTotalCount == -1)
//                {
//                    html_body.Remove(0, html_body.Length);
//                    throw new Exception("Error Transnippet in HTML content. ContentTransnippet. CheckForTransnippet validation failed");
//                    //html_body.Append("Error Transnippet in HTML content.");
//                }
//                else if (transTotalCount > 0)
//                {
//                    bool bfirst = true;
//                    foreach (DataRow row in emailProfileDataSet)
//                    {
//                        if (bfirst)
//                        {
//                            bfirst = false;

//                            foreach (DataColumn dc in row.Table.Columns)
//                            {
//                                emailRowsDT.Columns.Add(dc.ColumnName);
//                            }
//                        }
//                        emailRowsDT.ImportRow(row);
//                    }
//                    //split the html 
//                    try
//                    {
//                        string htmlModified = ContentTransnippet.ModifyHTML(html_body.ToString(), emailRowsDT);
//                        html_body.Remove(0, html_body.Length);
//                        html_body.Append(htmlModified);
//                    }
//                    catch (Exception)
//                    {
//                        html_body.Remove(0, html_body.Length);
//                        throw new Exception("Error Transnippet in HTML content. ContentTransnippet.ModifyHTML Failed");
//                    }
//                }

//                //text
//                transTotalCount = ContentTransnippet.CheckForTransnippet(text_body.ToString());
//                if (transTotalCount == -1)
//                {
//                    text_body.Remove(0, text_body.Length);
//                    throw new Exception("Error Transnippet in Text content. ContentTransnippet. CheckForTransnippet validation failed");
//                }
//                else if (transTotalCount > 0)
//                {
//                    //split the text
//                    try
//                    {
//                        string textModified = ContentTransnippet.ModifyHTML(text_body.ToString(), emailRowsDT);
//                        text_body.Remove(0, text_body.Length);
//                        text_body.Append(textModified);
//                    }
//                    catch (Exception)
//                    {
//                        text_body.Remove(0, text_body.Length);
//                        throw new Exception("Error Transnippet in Text content.  ContentTransnippet.ModifyHTML Failed");
//                    }
//                }
//                #endregion

//                //--
//                //--Added for Email Personalization - Ashok 01/12/09
//                //--
//                //dynamic FromEmailAddress
//                string messageFromEmailAddress = ((dynamic_fromEmail.ToString().Trim().Length > 0) ? dynamic_fromEmail.ToString().Trim() : emailBlast.blast_msg.FromAddress);
//                string messageFromName = ((dynamic_fromName.ToString().Trim().Length > 0) ? dynamic_fromName.ToString().Trim() : emailBlast.blast_msg.FromName);
//                string messageReplyToEmailAddress = ((dynamic_replyTo.ToString().Trim().Length > 0) ? dynamic_replyTo.ToString().Trim() : emailBlast.blast_msg.ReplyTo);

//                if (emailBlast.type.ToUpper() == "TEXT")
//                {
//                    EmailMessageProvider text_msgProvider = EmailMessageProvider.CreateInstance("text", "", "", "", "", "");
//                    text_msgProvider.update_delivery_stats(dr["EmailAddress"].ToString(), messageFromName, messageFromEmailAddress, messageReplyToEmailAddress, dr["BounceAddress"].ToString());
//                    sb = text_msgProvider.WriteEmailMessage(dr, dynamic_subject.ToString(), message_id, text_body.ToString(), boundry_tag, html_body.ToString(), emailBlast.blastconfig);
//                    text_msgProvider = null;
//                }
//                else
//                {
//                    // Dump the mail to the provider based on the format string.
//                    if (dr["FormatTypeCode"].ToString().ToLower() == "html")
//                    {
//                        //Prefetches of strings needed in engine
//                        // html message writer
//                        EmailMessageProvider html_msgProvider = EmailMessageProvider.CreateInstance("html", "", "", "", "", "");
//                        html_msgProvider.update_delivery_stats(dr["EmailAddress"].ToString(), messageFromName, messageFromEmailAddress, messageReplyToEmailAddress, dr["BounceAddress"].ToString());
//                        sb = html_msgProvider.WriteEmailMessage(dr, dynamic_subject.ToString(), message_id, text_body.ToString(), boundry_tag, html_body.ToString(), emailBlast.blastconfig);
//                        html_msgProvider = null;
//                    }
//                    else
//                    {
//                        EmailMessageProvider text_msgProvider = EmailMessageProvider.CreateInstance("text", "", "", "", "", "");
//                        text_msgProvider.update_delivery_stats(dr["EmailAddress"].ToString(), messageFromName, messageFromEmailAddress, messageReplyToEmailAddress, dr["BounceAddress"].ToString());
//                        sb = text_msgProvider.WriteEmailMessage(dr, dynamic_subject.ToString(), message_id, text_body.ToString(), boundry_tag, html_body.ToString(), emailBlast.blastconfig);
//                        text_msgProvider = null;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Exception original = ex;
//                try
//                {
//                    SendEmailNotification("Error in IronPortEmailWriter.deli.getEmailMessageObject for BlastID: " + emailBlast.BlastID + " EmailID: " + dr["EmailID"].ToString(), ex.ToString());
//                }
//                catch (Exception)
//                {
//                    SendEmailNotification("Error in IronPortEmailWriter.deli.getEmailMessageObject", original.ToString());
//                }
//                Console.WriteLine(original.ToString());
//                throw original;
//            }

//            return sb.ToString();
//        }
//    }
//}

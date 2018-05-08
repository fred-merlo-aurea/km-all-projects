//using System;
//using System.Collections;
//using System.Configuration;
//using System.Net.Mail;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading;
//using System.Data;
//using System.Data.SqlClient;
//using ecn.common.classes;
//using System.Xml;
//using System.Collections.Generic;
//using System.Linq;

//namespace ecn.communicator.classes.EmailWriter
//{
    //#region Class IronPortEMailWriter
    //public class IronPortEMailWriter : IEmailWriter
    //{

    //    public IronPortEMailWriter()
    //    {

    //    }

    //    public void Write(string pickupDirecctory, string fileName, EmailMessageProvider emailProvider, string subject, string body, string log)
    //    {
    //        _smtp_writer.SendEmail(emailProvider.BounceAddress, emailProvider.ToEmailAddress, body, log);
    //    }

        
    //}
    //#endregion
    

    //#region Class StateObject
    //public class StateObject
    //{
    //    public byte[] sBuffer;
    //    public bool sent_hello = false;
    //    public Socket sSocket;
    //    public string EmailFrom = string.Empty;
    //    public string EmailTo = string.Empty;
    //    public string EmailMessage = string.Empty;
    //    public int max_buf;
    //    public bool is_started;

    //    // messages
    //    public byte[] hello_msg;
    //    public byte[] data_msg;
    //    public Queue q;
    //    public bool second_q = false;
    //    public string ehloDomainString = ConfigurationManager.AppSettings["ehloDomainString"].ToString();
    //    public EmailBlast emailBlastObj;

    //    public StateObject(int size, Socket sock)
    //    {
    //        sSocket = sock;
    //        max_buf = size;
    //        is_started = false;
    //        hello_msg = Encoding.ASCII.GetBytes("ehlo " + ehloDomainString + "\r\n");
    //        data_msg = Encoding.ASCII.GetBytes("data\r\n");
    //        resetBuf();
    //    }

    //    public void resetBuf()
    //    {
    //        sBuffer = new byte[max_buf];
    //    }
    //    public bool getStarted()
    //    {
    //        return is_started;
    //    }
    //    public void flipStarted()
    //    {
    //        if (is_started) is_started = false;
    //        else is_started = true;
    //    }
    //}
    //#endregion

    //#region Class EmailHolder
    //public class EmailHolder
    //{
    //    public string to_eml;
    //    public string frm_eml;
    //    public string msg;
    //    public string log_line;
    //    public DataRow dr;
    //    public DataRow[] drArray;
    //    public EmailHolder() { }
    //}
    //#endregion

   

    //#region Class deli
    //public class deli
    //{
    //    private static void SendEmailNotification(string subject, string body)
    //    {
    //        MailMessage message = new MailMessage();
    //        message.From = new MailAddress("domain_admin@teckman.com");
    //        message.To.Add(ConfigurationManager.AppSettings["SendTo"]);
    //        message.Subject = "Engine: " + System.AppDomain.CurrentDomain.FriendlyName.ToString() + " - " + subject;
    //        message.Body = body;

    //        SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
    //        smtp.Send(message);
    //    }

    //    public static int max_input = 200;
    //    // Make the connection to the server and wait for the 220
    //    public static void connectCallback(IAsyncResult asyncConnect)
    //    {
    //        try
    //        {
    //            StateObject _stateObject =
    //                (StateObject)asyncConnect.AsyncState;
    //            _stateObject.sSocket.EndConnect(asyncConnect);

    //            if (_stateObject.sSocket.Connected == false)
    //            {
    //                SmtpWriter.holder.FinishFile();
    //                throw new SmtpException("Error initiating communication with Smtp server. (" + "XXXclient error msg" + ")");
    //            }

    //            _stateObject.sSocket.BeginReceive(
    //                    _stateObject.sBuffer,
    //                    0,
    //                    max_input,
    //                    SocketFlags.None,
    //                    new AsyncCallback(initial220Callback),
    //                    _stateObject);
    //        }
    //        catch (Exception ex)
    //        {
    //            SendEmailNotification("Error in IronPortEmailWriter.deli.connectCallback", ex.ToString());
    //            throw;
    //        }
    //    }


    //    // Get what should be a 220 then send the ehlo command
    //    public static void initial220Callback(IAsyncResult asyncReceive)
    //    {
    //        try
    //        {
    //            StateObject _stateObject =
    //                (StateObject)asyncReceive.AsyncState;

    //            int bytesReceived =
    //                _stateObject.sSocket.EndReceive(asyncReceive);

    //            string server_response = Encoding.ASCII.GetString(_stateObject.sBuffer);

    //            string bob = server_response.Substring(0, 3);
    //            if (server_response.Substring(0, 3) != "220")
    //            {
    //                SmtpWriter.holder.FinishFile();
    //                Console.Write("initial220Callback: " + server_response);
    //                throw new SmtpException("Error initiating communication with Smtp server. (" + "no 220" + ")");
    //            }

    //            _stateObject.sSocket.BeginSend(
    //                    _stateObject.hello_msg,
    //                    0,
    //                    _stateObject.hello_msg.Length,
    //                    SocketFlags.None,
    //                    new AsyncCallback(helloCallback),
    //                    _stateObject);
    //        }
    //        catch (Exception ex)
    //        {
    //            SendEmailNotification("Error in IronPortEmailWriter.deli.initial220Callback", ex.ToString());
    //            throw;
    //        }
    //    }

    //    // Finish waiting for server response to HELO and check vs 250
    //    // Then start sending header
    //    public static void helloCallback(IAsyncResult asyncSend)
    //    {
    //        try
    //        {
    //            StateObject _stateObject =
    //                (StateObject)asyncSend.AsyncState;

    //            int bytesReceived =
    //                _stateObject.sSocket.EndSend(asyncSend);


    //            _stateObject.sSocket.BeginReceive(
    //                    _stateObject.sBuffer,
    //                    0,
    //                    max_input,
    //                    SocketFlags.None,
    //                    new AsyncCallback(helloResponseCallback),
    //                    _stateObject);
    //        }
    //        catch (Exception ex)
    //        {
    //            SendEmailNotification("Error in IronPortEmailWriter.deli.helloCallback", ex.ToString());
    //            throw;
    //        }
    //    }

    //    // get response to EHLO and send the mailFrom
    //    public static void helloResponseCallback(IAsyncResult asyncReceive)
    //    {
    //        try
    //        {
    //            StateObject _stateObject =
    //                (StateObject)asyncReceive.AsyncState;

    //            int bytesReceived =
    //                _stateObject.sSocket.EndReceive(asyncReceive);

    //            string server_response = Encoding.ASCII.GetString(_stateObject.sBuffer);

    //            if (server_response.Substring(0, 3) != "250")
    //            {
    //                SmtpWriter.holder.FinishFile();
    //                Console.Write("helloResponseCallback: " + server_response);
    //                throw new SmtpException("Error initiating communication with Smtp server. (" + "no 250 after helo" + ")");
    //            }
    //            byte[] temp_buff = Encoding.ASCII.GetBytes("mail from:<" + String.Copy(((EmailHolder)_stateObject.q.Peek()).frm_eml) + ">\r\n");

    //            _stateObject.sSocket.BeginSend(
    //                    temp_buff,
    //                    0,
    //                    temp_buff.Length,
    //                    SocketFlags.None,
    //                    new AsyncCallback(mailFromCallback),
    //                    _stateObject);
    //        }
    //        catch (Exception ex)
    //        {
    //            SendEmailNotification("Error in IronPortEmailWriter.deli.helloResponseCallback", ex.ToString());
    //            throw;
    //        }

    //    }

    //    // Finish the send of "mail from" and start the recieve of the response
    //    public static void mailFromCallback(IAsyncResult asyncSend)
    //    {
    //        try
    //        {
    //            StateObject _stateObject =
    //                (StateObject)asyncSend.AsyncState;

    //            int bytesSent = _stateObject.sSocket.EndSend(asyncSend);

    //            //stateObject.resetBuf();

    //            _stateObject.sSocket.BeginReceive(
    //                    _stateObject.sBuffer,
    //                    0,
    //                    max_input,
    //                    SocketFlags.None,
    //                    new AsyncCallback(mailFromRecieveCallback),
    //                    _stateObject);
    //        }
    //        catch (Exception ex)
    //        {
    //            SendEmailNotification("Error in IronPortEmailWriter.deli.mailFromCallback", ex.ToString());
    //            throw;
    //        }
    //    }

    //    // Ensure that the MailFrom gave us 250 and send out the RECIPIENT List
    //    public static void mailFromRecieveCallback(IAsyncResult asyncReceive)
    //    {
    //        try
    //        {
    //            StateObject _stateObject =
    //                (StateObject)asyncReceive.AsyncState;

    //            int bytesReceived =
    //                _stateObject.sSocket.EndReceive(asyncReceive);

    //            string server_response = Encoding.ASCII.GetString(_stateObject.sBuffer);

    //            if (server_response.Substring(0, 3) != "250")
    //            {
    //                SmtpWriter.holder.FinishFile();
    //                Console.Write("mailFromRecieveCallback: " + server_response);
    //                throw new SmtpException("Error initiating communication with Smtp server. (" + "no 250 after mail from" + ")");
    //            }

    //            byte[] temp_buff = Encoding.ASCII.GetBytes("rcpt to:<" + String.Copy(((EmailHolder)_stateObject.q.Peek()).to_eml) + ">\r\n");

    //            _stateObject.sSocket.BeginSend(
    //                    temp_buff,
    //                    0,
    //                    temp_buff.Length,
    //                    SocketFlags.None,
    //                    new AsyncCallback(rcptToCallback),
    //                    _stateObject);
    //        }
    //        catch (Exception ex)
    //        {
    //            SendEmailNotification("Error in IronPortEmailWriter.deli.mailFromRecieveCallback", ex.ToString());
    //            throw;
    //        }

    //    }

    //    // Finish sending RCPT To and get the response
    //    public static void rcptToCallback(IAsyncResult asyncSend)
    //    {
    //        try
    //        {
    //            StateObject _stateObject =
    //                (StateObject)asyncSend.AsyncState;

    //            int bytesSent = _stateObject.sSocket.EndSend(asyncSend);

    //            //_stateObject.resetBuf();

    //            _stateObject.sSocket.BeginReceive(
    //                    _stateObject.sBuffer,
    //                    0,
    //                    max_input,
    //                    SocketFlags.None,
    //                    new AsyncCallback(rcptToRecieveCallback),
    //                    _stateObject);
    //        }
    //        catch (Exception ex)
    //        {
    //            SendEmailNotification("Error in IronPortEmailWriter.deli.rcptToCallback", ex.ToString());
    //            throw;
    //        }
    //    }

    //    // Get the RCPT TO response and send the Data command
    //    public static void rcptToRecieveCallback(IAsyncResult asyncReceive)
    //    {
    //        try
    //        {
    //            StateObject _stateObject =
    //                (StateObject)asyncReceive.AsyncState;

    //            int bytesReceived =
    //                _stateObject.sSocket.EndReceive(asyncReceive);

    //            string server_response = Encoding.ASCII.GetString(_stateObject.sBuffer);

    //            if (server_response.Substring(0, 3) != "250" && server_response.Substring(0, 3) != "251")
    //            {
    //                SmtpWriter.holder.FinishFile();
    //                Console.Write("rcptToRecieveCallback: " + server_response);
    //                throw new SmtpException("Error initiating communication with Smtp server. (" + "no 250/251 after rcpt" + ")");
    //            }

    //            _stateObject.sSocket.BeginSend(
    //                    _stateObject.data_msg,
    //                    0,
    //                    _stateObject.data_msg.Length,
    //                    SocketFlags.None,
    //                    new AsyncCallback(DataCallback),
    //                    _stateObject);
    //        }
    //        catch (Exception ex)
    //        {
    //            SendEmailNotification("Error in IronPortEmailWriter.deli.rcptToRecieveCallback", ex.ToString());
    //            throw;
    //        }
    //    }


    //    // Callback for data method, set up recieve
    //    public static void DataCallback(IAsyncResult asyncSend)
    //    {
    //        try
    //        {
    //            StateObject _stateObject =
    //                (StateObject)asyncSend.AsyncState;

    //            int bytesSent = _stateObject.sSocket.EndSend(asyncSend);

    //            //_stateObject.resetBuf();


    //            _stateObject.sSocket.BeginReceive(
    //                    _stateObject.sBuffer,
    //                    0,
    //                    max_input,
    //                    SocketFlags.None,
    //                    new AsyncCallback(DataRecieveCallback),
    //                    _stateObject);
    //        }
    //        catch (Exception ex)
    //        {
    //            SendEmailNotification("Error in IronPortEmailWriter.deli.DataCallback", ex.ToString());
    //            throw;
    //        }
    //    }

    //    // Get the response to the data command and sent out the email.
    //    public static void DataRecieveCallback(IAsyncResult asyncReceive)
    //    {
    //        try
    //        {
    //            StateObject _stateObject =
    //                (StateObject)asyncReceive.AsyncState;

    //            int bytesReceived =
    //                _stateObject.sSocket.EndReceive(asyncReceive);

    //            string server_response = Encoding.ASCII.GetString(_stateObject.sBuffer);

    //            if (server_response.Substring(0, 3) != "354")
    //            {
    //                SmtpWriter.holder.FinishFile();
    //                Console.Write("DataRecieveCallback: " + server_response);
    //                throw new SmtpException("Error initiating communication with Smtp server. (" + "no 354 after data" + ")");
    //            }

    //            //07/10/2007 - PERFORMANCE TWEAK 
    //            SmtpWriter.holder.Add(String.Copy(((EmailHolder)_stateObject.q.Peek()).log_line));

    //            EmailHolder tempEmailHolder = (EmailHolder)_stateObject.q.Dequeue();
    //            string msg = getEmailMessageObject((EmailBlast)_stateObject.emailBlastObj, (DataRow)(tempEmailHolder).dr, (DataRow[])(tempEmailHolder).drArray);
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
    //                        //tmpMessage.Append("\r\n");
    //                        //if (parts[i].Length > 1)
    //                        //{
    //                        //    tmpMessage.Append(parts[i].Substring(1, (parts[i].Length - 1)));
    //                        //    tmpMessage.Append("\r\n");
    //                        //}                        
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
    //            byte[] temp_buff = Encoding.ASCII.GetBytes(String.Copy(msg) + "\r\n.\r\n");

    //            //_stateObject.sBuffer = Encoding.ASCII.GetBytes(tmp.msg + "\r\n.\r\n");


    //            _stateObject.sSocket.BeginSend(
    //                    temp_buff,
    //                    0,
    //                    temp_buff.Length,
    //                    SocketFlags.None,
    //                    new AsyncCallback(DataPushCallback),
    //                    _stateObject);
    //        }
    //        catch (Exception ex)
    //        {
    //            SendEmailNotification("Error in IronPortEmailWriter.deli.DataRecieveCallback", ex.ToString());
    //            throw;
    //        }

    //    }

    //    //07/10/2007 - PERFORMANCE TWEAK 
    //    private static string getEmailMessageObject(EmailBlast emailBlastObj, DataRow dr, DataRow[] emailProfileDataSet)
    //    {

    //        StringBuilder sb;
    //        try
    //        {
    //            // create message id 
    //            string message_id = dr["BlastID"].ToString() + "." + dr["EmailID"].ToString() + "x" + QuotedPrintable.RandomString(5, true) + "@enterprisecommunicationnetwork.com";
    //            string boundry_tag = "_=COMMUNICATOR=_" + QuotedPrintable.RandomString(32, true);
    //            boundry_tag = boundry_tag.ToLower();




    //            // Replace all of our "fields" in the email body / Subject text part
    //            StringBuilder text_body = new StringBuilder();

    //            for (int i = 0; i < CodesnippetBreakupArrayHolder.BreakupTextMail.Length; i++)
    //            {
    //                try
    //                {
    //                    string line_data = CodesnippetBreakupArrayHolder.BreakupTextMail.GetValue(i).ToString();
    //                    if (i % 2 == 0)
    //                        text_body.Append(line_data);
    //                    else
    //                        text_body.Append(dr[line_data].ToString());
    //                }
    //                catch (Exception ex)
    //                {
    //                    string message = ex.Message;
    //                }
    //            }

    //            // ashok -8/17/07 
    //            // Replace all of our "fields" in the HTML email body 
    //            StringBuilder html_body = new StringBuilder();
    //            for (int i = 0; i < CodesnippetBreakupArrayHolder.BreakupHTMLMail.Length; i++)
    //            {
    //                string line_data = CodesnippetBreakupArrayHolder.BreakupHTMLMail.GetValue(i).ToString();
    //                if (i % 2 == 0)
    //                    html_body.Append(line_data);
    //                else
    //                    html_body.Append(dr[line_data].ToString());
    //            }

    //            //Removing this because it's overwriting content that's already been processed JWelter 5/20/2014
    //            //replace all of the dynamic tags content text and html
    //            if (dr != null && dr["BlastID"].ToString() != "-1")
    //            {
    //                System.Collections.Generic.List<string> toParse = new System.Collections.Generic.List<string>();
    //                toParse.Add(html_body.ToString());
    //                System.Collections.Generic.List<string> tags = ECN_Framework_BusinessLayer.Communicator.Content.GetTags(toParse, true);
    //                if (tags.Count > 0)
    //                {
    //                    DataTable dt2 = null;
    //                    foreach (var tag in tags)
    //                    {
    //                        if (dr[tag].ToString().Trim().Length > 0)
    //                        {
    //                            string sqlBlastQuery = " SELECT * " +
    //                            " FROM Blast " +
    //                            " WHERE BlastID=" + dr["BlastID"].ToString() + " ";
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

    //                }

    //            }
                
    //            // commented by Sunil - 01/12/2014 -- this section should be moved outside of this method.. this generated RSSFEED for each email. it should be generated once & update the RSS codesnippet.

    
    //            //wgh - social media replace - commented out for break/fix release
    //            DataTable socialDT = DataFunctions.GetDataTable("select * from socialmedia where canshare = 'true' and isactive = 'true'");
    //            if (socialDT != null && socialDT.Rows.Count > 0)
    //            {
    //                KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));
    //                //KM.Common.Entity.Encryption ec = new KM.Common.Entity.Encryption();
    //                if (ec != null && ec.ID > 0)
    //                {
    //                    string encryptedQuery = string.Empty;
    //                    string queryString = string.Empty;
    //                    foreach (DataRow row in socialDT.Rows)
    //                    {
    //                        if (row["SocialMediaId"].Equals(5))
    //                        {
    //                            html_body = html_body.Replace(row["MatchString"].ToString(), System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"].ToString() + "/engines/emailtofriend.aspx?e=" + dr["EmailID"].ToString() + "&b=" + dr["BlastID"].ToString());
    //                            text_body = text_body.Replace(row["MatchString"].ToString(), System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"].ToString() + "/engines/emailtofriend.aspx?e=" + dr["EmailID"].ToString() + "&b=" + dr["BlastID"].ToString());
    //                        }
    //                        else if (row["SocialMediaId"].Equals(4))
    //                        {

    //                        }
    //                        else
    //                        {
    //                            queryString = "b=" + dr["BlastID"].ToString() + "&g=" + dr["GroupID"].ToString() + "&e=" + dr["EmailID"].ToString() + "&m=" + row["SocialMediaID"].ToString();
    //                            encryptedQuery = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(queryString, ec));
    //                            html_body = html_body.Replace(row["MatchString"].ToString(), System.Configuration.ConfigurationManager.AppSettings["Social_DomainPath"] + System.Configuration.ConfigurationManager.AppSettings["SocialClick"] + encryptedQuery + "");
    //                            //added for text replacement
    //                            text_body = text_body.Replace(row["MatchString"].ToString(), System.Configuration.ConfigurationManager.AppSettings["Social_DomainPath"] + System.Configuration.ConfigurationManager.AppSettings["SocialClick"] + encryptedQuery + "");
    //                        }
    //                    }
    //                }
    //            }
                

    //            //facebook
    //            //encryptedQuery = string.Empty;
    //            //queryString = string.Empty;
    //            //try
    //            //{
    //            //    queryString = "b=" + dr["BlastID"].ToString() + "&g=" + dr["GroupID"].ToString() + "&e=" + dr["EmailID"].ToString() + "&media=FaceBook";
    //            //    KM.Common.Encryption ec = new KM.Common.Encryption();
    //            //    ec.PassPhrase = "p$yaQat3?U@r5truX6Vepra++8?&68t8-uB9CuW?UtHaZapUJ-2e8&!3-du2AMA*";
    //            //    ec.SaltValue = "7emAha2hEdrUCephekas3uzuje6uGasab5Axu5t64u8a*HEyUtr9pr+bra4uJeXE";
    //            //    ec.HashAlgorithm = "SHA1";
    //            //    ec.PasswordIterations = 2;
    //            //    ec.InitVector = "d3EdrEp=ucR-cAwr";
    //            //    ec.KeySize = 256;
    //            //    encryptedQuery = System.Web.HttpUtility.UrlEncode(ec.Encrypt(queryString, ec.PassPhrase, ec.SaltValue, ec.HashAlgorithm, ec.PasswordIterations, ec.InitVector, ec.KeySize));
    //            //    //encryptedQuery = System.Web.HttpContext.Current.Server.UrlEncode(ec.Encrypt(queryString, ec.PassPhrase, ec.SaltValue, ec.HashAlgorithm, ec.PasswordIterations, ec.InitVector, ec.KeySize));
    //            //    html_body = html_body.Replace("|facebooklink|", "<a href='" + System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/SClick.aspx?" + encryptedQuery + "'><img border=\"0\" src=\"" + System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"] + "/images/SocialIcons/facebook.jpg\"></a>&nbsp;");
    //            //}
    //            //catch (Exception)
    //            //{
    //            //}
    //            ////linkedin
    //            //encryptedQuery = string.Empty;
    //            //queryString = string.Empty;
    //            //try
    //            //{
    //            //    queryString = "b=" + dr["BlastID"].ToString() + "&g=" + dr["GroupID"].ToString() + "&e=" + dr["EmailID"].ToString() + "&media=LinkedIn";
    //            //    KM.Common.Encryption ec = new KM.Common.Encryption();
    //            //    ec.PassPhrase = "p$yaQat3?U@r5truX6Vepra++8?&68t8-uB9CuW?UtHaZapUJ-2e8&!3-du2AMA*";
    //            //    ec.SaltValue = "7emAha2hEdrUCephekas3uzuje6uGasab5Axu5t64u8a*HEyUtr9pr+bra4uJeXE";
    //            //    ec.HashAlgorithm = "SHA1";
    //            //    ec.PasswordIterations = 2;
    //            //    ec.InitVector = "d3EdrEp=ucR-cAwr";
    //            //    ec.KeySize = 256;
    //            //    encryptedQuery = System.Web.HttpUtility.UrlEncode(ec.Encrypt(queryString, ec.PassPhrase, ec.SaltValue, ec.HashAlgorithm, ec.PasswordIterations, ec.InitVector, ec.KeySize));
    //            //    //encryptedQuery = System.Web.HttpContext.Current.Server.UrlEncode(ec.Encrypt(queryString, ec.PassPhrase, ec.SaltValue, ec.HashAlgorithm, ec.PasswordIterations, ec.InitVector, ec.KeySize));
    //            //    html_body = html_body.Replace("|linkedinlink|", "<a href='" + System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/SClick.aspx?" + encryptedQuery + "'><img border=\"0\" src=\"" + System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"] + "/images/SocialIcons/linkedin.jpg\"></a>&nbsp;");
    //            //}
    //            //catch (Exception)
    //            //{
    //            //}


    //            // Replace all "fields" in the email subject
    //            StringBuilder dynamic_subject = new StringBuilder();
    //            for (int i = 0; i < CodesnippetBreakupArrayHolder.BreakupSubject.Length; i++)
    //            {
    //                string line_data = CodesnippetBreakupArrayHolder.BreakupSubject.GetValue(i).ToString();
    //                if (i % 2 == 0)
    //                    dynamic_subject.Append(line_data);
    //                else
    //                    dynamic_subject.Append(System.Web.HttpUtility.HtmlDecode(dr[line_data].ToString()));
    //            }

    //            //--
    //            //--Added for Email Personalization - Ashok 01/12/09
    //            //--
    //            StringBuilder dynamic_fromEmail = new StringBuilder();
    //            for (int i = 0; i < CodesnippetBreakupArrayHolder.BreakupFromEmail.Length; i++)
    //            {
    //                string line_data = CodesnippetBreakupArrayHolder.BreakupFromEmail.GetValue(i).ToString();
    //                if (i % 2 == 0)
    //                    dynamic_fromEmail.Append(line_data);
    //                else
    //                    dynamic_fromEmail.Append(System.Web.HttpUtility.HtmlDecode(dr[line_data].ToString()));
    //            }

    //            // Replace all "fields" in the email subject
    //            StringBuilder dynamic_fromName = new StringBuilder();
    //            for (int i = 0; i < CodesnippetBreakupArrayHolder.BreakupFromName.Length; i++)
    //            {
    //                string line_data = CodesnippetBreakupArrayHolder.BreakupFromName.GetValue(i).ToString();
    //                if (i % 2 == 0)
    //                    dynamic_fromName.Append(line_data);
    //                else
    //                    dynamic_fromName.Append(System.Web.HttpUtility.HtmlDecode(dr[line_data].ToString()));
    //            }

    //            // Replace all "fields" in the email subject
    //            StringBuilder dynamic_replyTo = new StringBuilder();
    //            for (int i = 0; i < CodesnippetBreakupArrayHolder.breakupReplyToEmail.Length; i++)
    //            {
    //                string line_data = CodesnippetBreakupArrayHolder.breakupReplyToEmail.GetValue(i).ToString();
    //                if (i % 2 == 0)
    //                    dynamic_replyTo.Append(line_data);
    //                else
    //                    dynamic_replyTo.Append(System.Web.HttpUtility.HtmlDecode(dr[line_data].ToString()));
    //            }

    //            # region OLD Transnippet code - DONOTDELETE
    //            //Check to see if there are any Transnippets & replace'em with the history data.
    //            if (TransnippetHolder.TransnippetsCount > 0)
    //            {
    //                char[] splitter = { '|' };
    //                char[] udfSplitter = { ',' };
    //                for (int i = 0; i < TransnippetHolder.Transnippet.Count; i++)
    //                {
    //                    string output_htm = "", output_txt = "";
    //                    string[] transnippetSplits = (((TransnippetHolder.Transnippet[i].ToString()).Replace("##", "")).Replace("$$", "")).ToString().Split(splitter);
    //                    if (transnippetSplits.Length > 0)
    //                    {
    //                        //split the UDF's now.
    //                        string[] transnippetUDFs = transnippetSplits[2].ToString().Split(udfSplitter);

    //                        if (emailProfileDataSet.Length > 0 && transnippetUDFs.Length > 0)
    //                        {
    //                            for (int j = 0; j < emailProfileDataSet.Length; j++)
    //                            {
    //                                output_htm += "<tr>";
    //                                output_txt += "\n";
    //                                for (int k = 0; k < transnippetUDFs.Length; k++)
    //                                {
    //                                    try
    //                                    {
    //                                        output_htm += "<td>" + emailProfileDataSet[j][transnippetUDFs[k].ToString()].ToString() + "</td>";
    //                                        output_txt += emailProfileDataSet[j][transnippetUDFs[k].ToString()].ToString() + "\t";
    //                                    }
    //                                    catch
    //                                    {
    //                                        output_htm += "<td>&nbsp;</td>";
    //                                        output_txt += "\t";
    //                                    }
    //                                }
    //                                output_htm += "</tr>";
    //                            }
    //                            html_body.Replace(TransnippetHolder.Transnippet[i].ToString(), TransnippetHolder.TransnippetTablesHTML[i].ToString().Replace(TransnippetHolder.Transnippet[i].ToString(), output_htm));
    //                            text_body.Replace(TransnippetHolder.Transnippet[i].ToString(), TransnippetHolder.TransnippetTablesTxt[i].ToString().Replace(TransnippetHolder.Transnippet[i].ToString(), output_txt));
    //                        }
    //                    }
    //                }
    //            }

    //            #endregion

    //            #region Check for new version of transnippets - WGH
    //            DataTable emailRowsDT = new DataTable();

    //            //html
    //            int transTotalCount = ContentTransnippet.CheckForTransnippet(html_body.ToString());
    //            if (transTotalCount == -1)
    //            {
    //                html_body.Remove(0, html_body.Length);
    //                throw new Exception("Error Transnippet in HTML content.");
    //                //html_body.Append("Error Transnippet in HTML content.");
    //            }
    //            else if (transTotalCount > 0)
    //            {
    //                bool bfirst = true;
    //                foreach (DataRow row in emailProfileDataSet)
    //                {
    //                    if (bfirst)
    //                    {
    //                        bfirst = false;

    //                        foreach (DataColumn dc in row.Table.Columns)
    //                        {
    //                            emailRowsDT.Columns.Add(dc.ColumnName);
    //                        }
    //                    }
    //                    emailRowsDT.ImportRow(row);
    //                }
    //                //split the html
    //                try
    //                {
    //                    string htmlModified = ContentTransnippet.ModifyHTML(html_body.ToString(), emailRowsDT);
    //                    html_body.Remove(0, html_body.Length);
    //                    html_body.Append(htmlModified);
    //                }
    //                catch (Exception)
    //                {
    //                    html_body.Remove(0, html_body.Length);
    //                    throw new Exception("Error Transnippet in HTML content.");
    //                    //html_body.Append("Error Transnippet in HTML content.");
    //                }
    //            }
    //            //text
    //            transTotalCount = ContentTransnippet.CheckForTransnippet(text_body.ToString());
    //            if (transTotalCount == -1)
    //            {
    //                text_body.Remove(0, text_body.Length);
    //                throw new Exception("Error Transnippet in Text content.");
    //                //text_body.Append("Error Transnippet in Text content.");
    //            }
    //            else if (transTotalCount > 0)
    //            {
    //                //split the text
    //                try
    //                {
    //                    string textModified = ContentTransnippet.ModifyHTML(text_body.ToString(), emailRowsDT);
    //                    text_body.Remove(0, text_body.Length);
    //                    text_body.Append(textModified);
    //                }
    //                catch (Exception)
    //                {
    //                    text_body.Remove(0, text_body.Length);
    //                    throw new Exception("Error Transnippet in Text content.");
    //                    //text_body.Append("Error Transnippet in Text content.");
    //                }
    //            }
    //            #endregion

    //            //--
    //            //--Added for Email Personalization - Ashok 01/12/09
    //            //--
    //            string messageFromName = "", messageFromEmailAddress = "", messageReplyToEmailAddress = "";

    //            //dynamic FromEmailAddress
    //            if (emailBlastObj.dynamicFromEmail.Length > 0)
    //            {
    //                messageFromEmailAddress = dynamic_fromEmail.ToString();
    //                if (messageFromEmailAddress.Trim().Length == 0)
    //                {
    //                    messageFromEmailAddress = emailBlastObj.blast_msg.FromAddress;
    //                }
    //            }
    //            else
    //            {
    //                messageFromEmailAddress = emailBlastObj.blast_msg.FromAddress;
    //            }

    //            //dynamic FromName
    //            if (emailBlastObj.dynamicFromName.Length > 0)
    //            {
    //                messageFromName = dynamic_fromName.ToString();
    //                if (messageFromName.Trim().Length == 0)
    //                {
    //                    messageFromName = emailBlastObj.blast_msg.FromName;
    //                }
    //            }
    //            else
    //            {
    //                messageFromName = emailBlastObj.blast_msg.FromName;
    //            }

    //            //dynamic ReplyTo
    //            if (emailBlastObj.dynamicReplyToEmail.Length > 0)
    //            {
    //                messageReplyToEmailAddress = dynamic_replyTo.ToString();
    //                if (messageReplyToEmailAddress.Trim().Length == 0)
    //                {
    //                    messageReplyToEmailAddress = emailBlastObj.blast_msg.ReplyTo;
    //                }
    //            }
    //            else
    //            {
    //                messageReplyToEmailAddress = emailBlastObj.blast_msg.ReplyTo;
    //            }

    //            //EmailMessageProvider msgProvider = new EmailMessageProvider(dr["EmailAddress"].ToString(), messageFromName, messageFromEmailAddress, messageReplyToEmailAddress, dr["BounceAddress"].ToString(), (emailBlastObj.type.ToUpper() == "TEXT" ? "TEXT" : ((dr["FormatTypeCode"].ToString().ToUpper() == "HTML" ? "HTML" : "TEXT"))));

    //            //sb = msgProvider.WriteEmailMessage(dr, dynamic_subject.ToString(), message_id, text_body.ToString(), boundry_tag, html_body.ToString());

    //            //msgProvider = null;

    //            if (emailBlastObj.type.ToUpper() == "TEXT")
    //            {
    //                EmailMessageProvider text_msgProvider = EmailMessageProvider.CreateInstance("text", "", "", "", "", "");
    //                text_msgProvider.update_delivery_stats(dr["EmailAddress"].ToString(), messageFromName, messageFromEmailAddress, messageReplyToEmailAddress, dr["BounceAddress"].ToString());
    //                sb = text_msgProvider.WriteEmailMessage(dr, dynamic_subject.ToString(), message_id, text_body.ToString(), boundry_tag, html_body.ToString());
    //                text_msgProvider = null;
    //            }
    //            else
    //            {
    //                // Dump the mail to the provider based on the format string.
    //                if (dr["FormatTypeCode"].ToString().ToLower() == "html")
    //                {
    //                    //Prefetches of strings needed in engine
    //                    // html message writer
    //                    EmailMessageProvider html_msgProvider = EmailMessageProvider.CreateInstance("html", "", "", "", "", "");
    //                    html_msgProvider.update_delivery_stats(dr["EmailAddress"].ToString(), messageFromName, messageFromEmailAddress, messageReplyToEmailAddress, dr["BounceAddress"].ToString());
    //                    sb = html_msgProvider.WriteEmailMessage(dr, dynamic_subject.ToString(), message_id, text_body.ToString(), boundry_tag, html_body.ToString());
    //                    html_msgProvider = null;
    //                }
    //                else
    //                {
    //                    EmailMessageProvider text_msgProvider = EmailMessageProvider.CreateInstance("text", "", "", "", "", "");
    //                    text_msgProvider.update_delivery_stats(dr["EmailAddress"].ToString(), messageFromName, messageFromEmailAddress, messageReplyToEmailAddress, dr["BounceAddress"].ToString());
    //                    sb = text_msgProvider.WriteEmailMessage(dr, dynamic_subject.ToString(), message_id, text_body.ToString(), boundry_tag, html_body.ToString());
    //                    text_msgProvider = null;
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Exception original = ex;
    //            try
    //            {
    //                SendEmailNotification("Error in IronPortEmailWriter.deli.getEmailMessageObject for BlastID: " + dr["BlastID"].ToString() + " EmailID: " + dr["EmailID"].ToString(), ex.ToString());
    //            }
    //            catch (Exception)
    //            {
    //                SendEmailNotification("Error in IronPortEmailWriter.deli.getEmailMessageObject", original.ToString());
    //            }
    //            Console.WriteLine(original.ToString());
    //            throw original;
    //        }

    //        return sb.ToString();
    //    }

    //    // Callback for data push, set up recieve push response
    //    public static void DataPushCallback(IAsyncResult asyncSend)
    //    {
    //        try
    //        {
    //            StateObject _stateObject =
    //                    (StateObject)asyncSend.AsyncState;

    //            int bytesSent = _stateObject.sSocket.EndSend(asyncSend);

    //            if (_stateObject.q.Count != 0)
    //            {
    //                _stateObject.sSocket.BeginReceive(
    //                            _stateObject.sBuffer,
    //                            0,
    //                            max_input,
    //                            SocketFlags.None,
    //                            new AsyncCallback(helloResponseCallback),
    //                            _stateObject);
    //            }
    //            else
    //            {  // FSM to final state as queue is empty
    //                _stateObject.sSocket.BeginReceive(
    //                            _stateObject.sBuffer,
    //                            0,
    //                            max_input,
    //                            SocketFlags.None,
    //                            new AsyncCallback(DataPushReceiveCallback),
    //                            _stateObject);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            SendEmailNotification("Error in IronPortEmailWriter.deli.DataPushCallback", ex.ToString());
    //            throw;
    //        }
    //    }


    //    // Get the response from the sending of data and close the connection.
    //    public static void DataPushReceiveCallback(IAsyncResult asyncReceive)
    //    {
    //        try
    //        {
    //            StateObject _stateObject =
    //                    (StateObject)asyncReceive.AsyncState;

    //            int bytesReceived =
    //                _stateObject.sSocket.EndReceive(asyncReceive);

    //            string server_response = Encoding.ASCII.GetString(_stateObject.sBuffer);

    //            if (server_response.Substring(0, 3) != "250")
    //            {
    //                SmtpWriter.holder.FinishFile();
    //                Console.Write("DataPushReceiveCallback: " + server_response);
    //                throw new SmtpException("Error initiating communication with Smtp server. (" + "no 250 after data" + ")");
    //            }

    //            _stateObject.flipStarted();
    //        }
    //        catch (Exception ex)
    //        {
    //            SendEmailNotification("Error in IronPortEmailWriter.deli.DataPushReceiveCallback", ex.ToString());
    //            throw;
    //        }
    //    }
    //}
    //#endregion
//}
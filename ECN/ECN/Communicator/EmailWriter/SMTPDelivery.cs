
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;
//using System.Net.Mail;

//namespace ecn.communicator.classes.EmailWriter
//{
//    #region Class StateObject
//    public class StateObject
//    {
//        public byte[] sBuffer;
//        public bool sent_hello = false;
//        public Socket sSocket;
//        public Recipient recipient;
//        public int max_buf  = 200;
//        // messages
//        public byte[] hello_msg;
//        public byte[] data_msg;
//        public string ehloDomainString = ConfigurationManager.AppSettings["ehloDomainString"].ToString();

//        public StateObject(string SMTPServer, int SMTPPort)
//        {
//            hello_msg = Encoding.ASCII.GetBytes("ehlo " + ehloDomainString + "\r\n");
//            data_msg = Encoding.ASCII.GetBytes("data\r\n");
//            resetBuf();

//            IPAddress ipAddress = Dns.GetHostEntry(SMTPServer).AddressList[0];

//            IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, SMTPPort);

//            this.sSocket = new Socket(
//                AddressFamily.InterNetwork,
//                SocketType.Stream,
//                ProtocolType.Tcp);
//        }

//        public void resetBuf()
//        {
//            sBuffer = new byte[max_buf];
//        }
//    }
//    #endregion

//    public class SMTPDelivery
//    {

//        public static int max_input = 200;
//        // Make the connection to the server and wait for the 220
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

//        public void StartSMTPConversation(StateObject _stateObject)
//        {
//            try
//            {
//                if (_stateObject != null)
//                    return;

//                if (!_stateObject.sent_hello)
//                {
//                    _stateObject.sent_hello = true;
//                    _stateObject.sSocket.BeginConnect(
//                        ipEndpoint,
//                        new AsyncCallback(connectCallback),
//                        _stateObject);
//                }
//                else
//                {

//                    byte[] temp_buff = Encoding.ASCII.GetBytes("mail from:<" + _stateObject.recipient.From + ">\r\n");

//                    _stateObject.sSocket.BeginSend(
//                        temp_buff,
//                        0,
//                        temp_buff.Length,
//                        SocketFlags.None,
//                        new AsyncCallback(mailFromCallback),
//                        _stateObject);
//                }
//            }
//            catch (Exception ex)
//            {
//                SendEmailNotification("Error in IronPortEmailWriter.SmtpWriter.StartSMTPConversation", ex.ToString());
//                throw;
//            }
//        }


//        public static void connectCallback(IAsyncResult asyncConnect)
//        {
//            try
//            {
//                StateObject _stateObject =
//                    (StateObject)asyncConnect.AsyncState;
//                _stateObject.sSocket.EndConnect(asyncConnect);

//                if (_stateObject.sSocket.Connected == false)
//                {
//                    throw new SmtpException("Error initiating communication with Smtp server. (" + "XXXclient error msg" + ")");
//                }

//                _stateObject.sSocket.BeginReceive(
//                        _stateObject.sBuffer,
//                        0,
//                        max_input,
//                        SocketFlags.None,
//                        new AsyncCallback(initial220Callback),
//                        _stateObject);
//            }
//            catch (Exception ex)
//            {
//                SendEmailNotification("Error in IronPortEmailWriter.deli.connectCallback", ex.ToString());
//                throw;
//            }
//        }


//        // Get what should be a 220 then send the ehlo command
//        public static void initial220Callback(IAsyncResult asyncReceive)
//        {
//            try
//            {
//                StateObject _stateObject =
//                    (StateObject)asyncReceive.AsyncState;

//                int bytesReceived =
//                    _stateObject.sSocket.EndReceive(asyncReceive);

//                string server_response = Encoding.ASCII.GetString(_stateObject.sBuffer);

//                string bob = server_response.Substring(0, 3);
//                if (server_response.Substring(0, 3) != "220")
//                {
//                    Console.Write("initial220Callback: " + server_response);
//                    throw new SmtpException("Error initiating communication with Smtp server. (" + "no 220" + ")");
//                }

//                _stateObject.sSocket.BeginSend(
//                        _stateObject.hello_msg,
//                        0,
//                        _stateObject.hello_msg.Length,
//                        SocketFlags.None,
//                        new AsyncCallback(helloCallback),
//                        _stateObject);
//            }
//            catch (Exception ex)
//            {
//                SendEmailNotification("Error in IronPortEmailWriter.deli.initial220Callback", ex.ToString());
//                throw;
//            }
//        }

//        // Finish waiting for server response to HELO and check vs 250
//        // Then start sending header
//        public static void helloCallback(IAsyncResult asyncSend)
//        {
//            try
//            {
//                StateObject _stateObject =
//                    (StateObject)asyncSend.AsyncState;

//                int bytesReceived =
//                    _stateObject.sSocket.EndSend(asyncSend);


//                _stateObject.sSocket.BeginReceive(
//                        _stateObject.sBuffer,
//                        0,
//                        max_input,
//                        SocketFlags.None,
//                        new AsyncCallback(helloResponseCallback),
//                        _stateObject);
//            }
//            catch (Exception ex)
//            {
//                SendEmailNotification("Error in IronPortEmailWriter.deli.helloCallback", ex.ToString());
//                throw;
//            }
//        }

//        // get response to EHLO and send the mailFrom
//        public static void helloResponseCallback(IAsyncResult asyncReceive)
//        {
//            try
//            {
//                StateObject _stateObject =
//                    (StateObject)asyncReceive.AsyncState;

//                int bytesReceived =
//                    _stateObject.sSocket.EndReceive(asyncReceive);

//                string server_response = Encoding.ASCII.GetString(_stateObject.sBuffer);

//                if (server_response.Substring(0, 3) != "250")
//                {
//                    Console.Write("helloResponseCallback: " + server_response);
//                    throw new SmtpException("Error initiating communication with Smtp server. (" + "no 250 after helo" + ")");
//                }
//                byte[] temp_buff = Encoding.ASCII.GetBytes("mail from:<" + _stateObject.recipient.From + ">\r\n");

//                _stateObject.sSocket.BeginSend(
//                        temp_buff,
//                        0,
//                        temp_buff.Length,
//                        SocketFlags.None,
//                        new AsyncCallback(mailFromCallback),
//                        _stateObject);
//            }
//            catch (Exception ex)
//            {
//                SendEmailNotification("Error in IronPortEmailWriter.deli.helloResponseCallback", ex.ToString());
//                throw;
//            }

//        }

//        // Finish the send of "mail from" and start the recieve of the response
//        public static void mailFromCallback(IAsyncResult asyncSend)
//        {
//            try
//            {
//                StateObject _stateObject =
//                    (StateObject)asyncSend.AsyncState;

//                int bytesSent = _stateObject.sSocket.EndSend(asyncSend);

//                //stateObject.resetBuf();

//                _stateObject.sSocket.BeginReceive(
//                        _stateObject.sBuffer,
//                        0,
//                        max_input,
//                        SocketFlags.None,
//                        new AsyncCallback(mailFromRecieveCallback),
//                        _stateObject);
//            }
//            catch (Exception ex)
//            {
//                SendEmailNotification("Error in IronPortEmailWriter.deli.mailFromCallback", ex.ToString());
//                throw;
//            }
//        }

//        // Ensure that the MailFrom gave us 250 and send out the RECIPIENT List
//        public static void mailFromRecieveCallback(IAsyncResult asyncReceive)
//        {
//            try
//            {
//                StateObject _stateObject =
//                    (StateObject)asyncReceive.AsyncState;

//                int bytesReceived =
//                    _stateObject.sSocket.EndReceive(asyncReceive);

//                string server_response = Encoding.ASCII.GetString(_stateObject.sBuffer);

//                if (server_response.Substring(0, 3) != "250")
//                {
//                    Console.Write("mailFromRecieveCallback: " + server_response);
//                    throw new SmtpException("Error initiating communication with Smtp server. (" + "no 250 after mail from" + ")");
//                }

//                byte[] temp_buff = Encoding.ASCII.GetBytes("rcpt to:<" + _stateObject.recipient.To + ">\r\n");

//                _stateObject.sSocket.BeginSend(
//                        temp_buff,
//                        0,
//                        temp_buff.Length,
//                        SocketFlags.None,
//                        new AsyncCallback(rcptToCallback),
//                        _stateObject);
//            }
//            catch (Exception ex)
//            {
//                SendEmailNotification("Error in IronPortEmailWriter.deli.mailFromRecieveCallback", ex.ToString());
//                throw;
//            }

//        }

//        // Finish sending RCPT To and get the response
//        public static void rcptToCallback(IAsyncResult asyncSend)
//        {
//            try
//            {
//                StateObject _stateObject =
//                    (StateObject)asyncSend.AsyncState;

//                int bytesSent = _stateObject.sSocket.EndSend(asyncSend);

//                //_stateObject.resetBuf();

//                _stateObject.sSocket.BeginReceive(
//                        _stateObject.sBuffer,
//                        0,
//                        max_input,
//                        SocketFlags.None,
//                        new AsyncCallback(rcptToRecieveCallback),
//                        _stateObject);
//            }
//            catch (Exception ex)
//            {
//                SendEmailNotification("Error in IronPortEmailWriter.deli.rcptToCallback", ex.ToString());
//                throw;
//            }
//        }

//        // Get the RCPT TO response and send the Data command
//        public static void rcptToRecieveCallback(IAsyncResult asyncReceive)
//        {
//            try
//            {
//                StateObject _stateObject =
//                    (StateObject)asyncReceive.AsyncState;

//                int bytesReceived =
//                    _stateObject.sSocket.EndReceive(asyncReceive);

//                string server_response = Encoding.ASCII.GetString(_stateObject.sBuffer);

//                if (server_response.Substring(0, 3) != "250" && server_response.Substring(0, 3) != "251")
//                {
//                    Console.Write("rcptToRecieveCallback: " + server_response);
//                    throw new SmtpException("Error initiating communication with Smtp server. (" + "no 250/251 after rcpt" + ")");
//                }

//                _stateObject.sSocket.BeginSend(
//                        _stateObject.data_msg,
//                        0,
//                        _stateObject.data_msg.Length,
//                        SocketFlags.None,
//                        new AsyncCallback(DataCallback),
//                        _stateObject);
//            }
//            catch (Exception ex)
//            {
//                SendEmailNotification("Error in IronPortEmailWriter.deli.rcptToRecieveCallback", ex.ToString());
//                throw;
//            }
//        }


//        // Callback for data method, set up recieve
//        public static void DataCallback(IAsyncResult asyncSend)
//        {
//            try
//            {
//                StateObject _stateObject =
//                    (StateObject)asyncSend.AsyncState;

//                int bytesSent = _stateObject.sSocket.EndSend(asyncSend);

//                //_stateObject.resetBuf();


//                _stateObject.sSocket.BeginReceive(
//                        _stateObject.sBuffer,
//                        0,
//                        max_input,
//                        SocketFlags.None,
//                        new AsyncCallback(DataRecieveCallback),
//                        _stateObject);
//            }
//            catch (Exception ex)
//            {
//                SendEmailNotification("Error in IronPortEmailWriter.deli.DataCallback", ex.ToString());
//                throw;
//            }
//        }

//        // Get the response to the data command and sent out the email.
//        public static void DataRecieveCallback(IAsyncResult asyncReceive)
//        {
//            try
//            {
//                StateObject _stateObject =
//                    (StateObject)asyncReceive.AsyncState;

//                int bytesReceived =
//                    _stateObject.sSocket.EndReceive(asyncReceive);

//                string server_response = Encoding.ASCII.GetString(_stateObject.sBuffer);

//                if (server_response.Substring(0, 3) != "354")
//                {
//                    Console.Write("DataRecieveCallback: " + server_response);
//                    throw new SmtpException("Error initiating communication with Smtp server. (" + "no 354 after data" + ")");
//                }

//                //end of handling period in content
//                byte[] temp_buff = Encoding.ASCII.GetBytes(_stateObject.recipient.Message);

//                //_stateObject.sBuffer = Encoding.ASCII.GetBytes(tmp.msg + "\r\n.\r\n");


//                _stateObject.sSocket.BeginSend(
//                        temp_buff,
//                        0,
//                        temp_buff.Length,
//                        SocketFlags.None,
//                        new AsyncCallback(DataPushCallback),
//                        _stateObject);
//            }
//            catch (Exception ex)
//            {
//                SendEmailNotification("Error in IronPortEmailWriter.deli.DataRecieveCallback", ex.ToString());
//                throw;
//            }

//        }

//        // Callback for data push, set up recieve push response
//        public static void DataPushCallback(IAsyncResult asyncSend)
//        {
//            try
//            {
//                StateObject _stateObject =
//                        (StateObject)asyncSend.AsyncState;

//                int bytesSent = _stateObject.sSocket.EndSend(asyncSend);

//                    _stateObject.sSocket.BeginReceive(
//                                _stateObject.sBuffer,
//                                0,
//                                max_input,
//                                SocketFlags.None,
//                                new AsyncCallback(DataPushReceiveCallback),
//                                _stateObject);
//            }
//            catch (Exception ex)
//            {
//                SendEmailNotification("Error in IronPortEmailWriter.deli.DataPushCallback", ex.ToString());
//                throw;
//            }
//        }


//        // Get the response from the sending of data and close the connection.
//        public static void DataPushReceiveCallback(IAsyncResult asyncReceive)
//        {
//            try
//            {
//                StateObject _stateObject =
//                        (StateObject)asyncReceive.AsyncState;

//                int bytesReceived =
//                    _stateObject.sSocket.EndReceive(asyncReceive);

//                string server_response = Encoding.ASCII.GetString(_stateObject.sBuffer);

//                if (server_response.Substring(0, 3) != "250")
//                {
//                    Console.Write("DataPushReceiveCallback: " + server_response);
//                    throw new SmtpException("Error initiating communication with Smtp server. (" + "no 250 after data" + ")");
//                }
//                            }
//            catch (Exception ex)
//            {
//                SendEmailNotification("Error in IronPortEmailWriter.deli.DataPushReceiveCallback", ex.ToString());
//                throw;
//            }
//        }
//    }
//}

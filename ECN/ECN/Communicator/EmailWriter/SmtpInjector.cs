//using System;
//using System.Text;
//using System.IO;
//using System.Net.Sockets;
//using System.Net;
//using System.Configuration;

//namespace ecn.communicator.classes.EmailWriter
//{
//    /// <summary>
//    /// provides methods to send email via smtp direct to mail server
//    /// </summary>
//    public class SmtpInjector
//    {
//        private enum SMTPResponse : int
//        {
//            CONNECT_SUCCESS = 220,
//            GENERIC_SUCCESS = 250,
//            DATA_SUCCESS = 354,
//            QUIT_SUCCESS = 221
//        }
//        public static bool Send(string From, string To, string Message, string SmtpServer, int SmtpPort)
//        {
//            string mailto = string.Empty;
            
//            //Console.WriteLine(string.Format("Starting : {0} / {1} ", To,  DateTime.Now));

//            //ConnectSocket 
//            IPAddress ipAddress = Dns.GetHostEntry(SmtpServer).AddressList[0];
//            IPEndPoint endPt = new IPEndPoint(ipAddress, SmtpPort);
//            Socket s = new Socket(endPt.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
//            s.Connect(endPt);

//            if (!Check_Response(s, SMTPResponse.CONNECT_SUCCESS, To, "Connect"))
//            {
//                s.Close();
//                return false;
//            }

//            //initial220Callback
//            Senddata(s, string.Format("HELO {0}\r\n", ConfigurationManager.AppSettings["ehloDomainString"].ToString()));
//            if (!Check_Response(s, SMTPResponse.GENERIC_SUCCESS, To, "initial220Callback"))
//            {
//                s.Close();
//                return false;
//            }

//            //helloResponseCallback
//            Senddata(s, string.Format("MAIL From: {0}\r\n", From));
//            if (!Check_Response(s, SMTPResponse.GENERIC_SUCCESS, To, "helloResponseCallback"))
//            {

//                s.Close();
//                return false;
//            }

//            //mailFromRecieveCallback
//            Senddata(s, string.Format("RCPT TO: {0}\r\n", To));
//            if (!Check_Response(s, SMTPResponse.GENERIC_SUCCESS, To, "mailFromRecieveCallback"))
//            {
//                s.Close();
//                return false;
//            }

//            //rcptToRecieveCallback
//            Senddata(s, ("DATA\r\n"));
//            if (!Check_Response(s, SMTPResponse.DATA_SUCCESS, To, "rcptToRecieveCallback"))
//            {
//                s.Close();
//                return false;
//            }

//            //DataRecieveCallback
//            Senddata(s, Message);
//            if (!Check_Response(s, SMTPResponse.GENERIC_SUCCESS, To, "DataRecieveCallback"))
//            {
//                s.Close();
//                return false;
//            }

//            Senddata(s, "QUIT\r\n");
//            Check_Response(s, SMTPResponse.QUIT_SUCCESS, To, "QUIT");

//            s.Shutdown(SocketShutdown.Both);
//            s.Close();

//            //Console.WriteLine(string.Format("Completed : {0} / {1} ", To,  DateTime.Now));

//            return true;
//        }
//        private static void Senddata(Socket s, string msg)
//        {
//            byte[] _msg = Encoding.ASCII.GetBytes(msg);
//            s.Send(_msg, 0, _msg.Length, SocketFlags.None);
//        }
//        private static bool Check_Response(Socket s, SMTPResponse response_expected, string To, string ResponseFor)
//        {
//            string sResponse;
//            int response;
//            byte[] bytes = new byte[1024];
//            while (s.Available == 0)
//            {
//                //Console.WriteLine(string.Format("Waiting to finish : {0} / {1} / {2} ", To, ResponseFor, DateTime.Now));
//                System.Threading.Thread.Sleep(100);
//            }

//            s.Receive(bytes, 0, s.Available, SocketFlags.None);
//            sResponse = Encoding.ASCII.GetString(bytes);
//            response = Convert.ToInt32(sResponse.Substring(0, 3));
//            if (response != (int)response_expected)
//                return false;
//            return true;
//        }
//    }

//}
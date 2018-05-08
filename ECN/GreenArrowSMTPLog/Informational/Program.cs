using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

namespace GreenArrow_SMTPLog
{
    class OLDProgram
    {
        static void OLDMain(string[] args)
        {

            string Url = "http://greenarrow.teckman.com/greenarrowadmin/h/stats_sends_view_logs.php?sendid=408383&format=csv&email=";

            // *** Establish the request 
            HttpWebRequest Http = (HttpWebRequest)WebRequest.Create(Url);

            // *** Set properties
            Http.Timeout = 90000;     // 90 secs
            Http.Method = "POST";

            Http.PreAuthenticate = false;
            byte[] credentialsAuth = new UTF8Encoding().GetBytes("admin:9keen8repute");

            Http.Headers["Authorization"] = "Basic " + Convert.ToBase64String(credentialsAuth);

            // *** Retrieve request info headers

            HttpWebResponse WebResponse = (HttpWebResponse)Http.GetResponse();

            Encoding enc = Encoding.GetEncoding(1252);  // Windows default Code Page

            StreamReader ResponseStream =

               new StreamReader(WebResponse.GetResponseStream(), enc);

            string data = ResponseStream.ReadToEnd();

            Console.WriteLine(data);

            Console.WriteLine("Press any key to quit");

            Console.ReadLine();

            WebResponse.Close();

            ResponseStream.Close();


        }
    }
}

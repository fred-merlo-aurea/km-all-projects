using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KM.Common.Utilities.Ftp
{
    public static class FtpUtility
    {
        public static void UploadFileToFtpLocation(string uri, string username, string password, string filename, string fileContent)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(string.Concat(uri, "/", Uri.EscapeDataString(filename)));
            request.Credentials = new NetworkCredential(username, password);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(fileContent);
            request.ContentLength = byteArray.Length;
            
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }

            FtpWebResponse response = null;

            try
            {
                response = (FtpWebResponse)request.GetResponse();
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }
    }
}

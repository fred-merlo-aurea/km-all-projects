using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace UAD.API.ExtentionMethods
{
    public static class HttpRequestBaseExtensions 
    {
        /// <summary>
        /// Dump the raw HTTP request to a string. 
        /// </summary>
        /// <param name="request">The <see cref="HttpRequest"/> that should be dumped.       </param>
        /// <returns>The raw HTTP request.</returns>
        public static string ToRaw(this HttpRequest request)
        {
            StringWriter writer = new StringWriter();

            WriteStartLine(request, writer);
            WriteHeaders(request, writer);
            WriteBody(request, writer);

            return writer.ToString();
        }

        private static void WriteStartLine(HttpRequest request, StringWriter writer)
        {
            const string SPACE = " ";

            writer.Write(request.HttpMethod);
            writer.Write(SPACE + request.Url);
            writer.WriteLine(SPACE + request.ServerVariables["SERVER_PROTOCOL"]);
        }

        private static void WriteHeaders(HttpRequest request, StringWriter writer)
        {
            foreach (string key in request.Headers.AllKeys)
            {
                writer.WriteLine(string.Format("{0}: {1}", key, request.Headers[key]));
            }

            writer.WriteLine();
        }

        private static void WriteBody(HttpRequest request, StringWriter writer)
        {
            try
            {
                StreamReader reader = new StreamReader(request.InputStream);

                try
                {
                    string body = reader.ReadToEnd();
                    writer.WriteLine(body);
                }
                finally
                {
                    reader.BaseStream.Position = 0;
                }
            }
            catch { }
        }
    }
}
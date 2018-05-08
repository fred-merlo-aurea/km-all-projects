using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace KMPS_JF.Class
{
    public class FileExtensionHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string extension = Path.GetExtension(context.Request.Url.AbsolutePath);

            if (extension.Contains("xml"))
            {
                context.Response.Write("Unauthorized access to the file!!");
            }
        }

        #endregion
    }
}


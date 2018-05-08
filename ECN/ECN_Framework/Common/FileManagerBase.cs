using System;
using System.IO;
using System.Web.UI;

namespace ECN_Framework.Common
{
    public class FileManagerBase : Page
    {
        private const string ChannelIdQueryString = "chID";
        private const string CustomerIdQueryString = "cuID";
        private const string ImagesVirtualPathConfig = "Images_VirtualPath";

        protected SecurityCheck SecurityCheck = new SecurityCheck();

        public string CustomerId
        {
            get
            {
                try
                {
                    if (SecurityCheck.CustomerID() > 0)
                    {
                        return SecurityCheck.CustomerID().ToString();
                    }
                    else
                    {
                        return Request.QueryString[CustomerIdQueryString];
                    }
                }
                catch
                {
                    return Request.QueryString[CustomerIdQueryString];
                }
            }
        }

        public string ChannelId
        {
            get
            {
                try 
                {
                    if (SecurityCheck.BasechannelID() > 0)
                    {
                        return SecurityCheck.BasechannelID().ToString();
                    }
                    else
                    {
                        return Request.QueryString[ChannelIdQueryString];
                    }
                }
                catch
                {
                    return Request.QueryString[ChannelIdQueryString];
                }
            }
        }

        protected void Page_Load(object sender, System.EventArgs e) 
        {
            var imagePath = $"/customers/{CustomerId}/images";

            if (!Directory.Exists(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings[ImagesVirtualPathConfig] + imagePath)))
            {
                Directory.CreateDirectory(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings[ImagesVirtualPathConfig] + imagePath));
            }

            InitializeImagePath(imagePath);
        }

        /// <summary>
        /// Override this method in child FileManager controls to set the ImagePath of the image gallery control
        /// </summary>
        /// <param name="imagePath"></param>
        protected virtual void InitializeImagePath(string imagePath)
        {
            throw new NotImplementedException();
        }
    }
}

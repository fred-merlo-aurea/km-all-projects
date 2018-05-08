using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Web;

namespace ECN_Framework_Common.Helpers
{
    public static class ImageGalleryHelper
    {
        private const string CommunicatorVirtualPathConfig = "Communicator_VirtualPath";
        private const string ImageDomainPathConfig = "Image_DomainPath";

        public static DataTable GetImagesTable(string customerId, string currentFolder, string[] currentFolderFiles, string imageDirectory = null)
        {
            var imagesTable = new DataTable();
            imagesTable.Columns.Add(new DataColumn("ImageName", typeof(string)));
            imagesTable.Columns.Add(new DataColumn("ImageSizeRaw", typeof(int)));
            imagesTable.Columns.Add(new DataColumn("ImageSize", typeof(string)));
            imagesTable.Columns.Add(new DataColumn("ImageType", typeof(string)));
            imagesTable.Columns.Add(new DataColumn("ImageDtModified", typeof(DateTime)));
            imagesTable.Columns.Add(new DataColumn("ImageKey", typeof(string)));
            imagesTable.Columns.Add(new DataColumn("ImagePath", typeof(string)));

            foreach (var filePath in currentFolderFiles)
            {
                var file = new FileInfo(filePath);
                var filename = file.Name;
                var fileExtension = GetFileType(filename);
                var encodedPath = HttpContext.Current.Server.UrlPathEncode($"/Customers/{customerId}/images{currentFolder}/{file.Name}");
                if (imageDirectory != null)
                {
                    encodedPath = HttpContext.Current.Server.UrlPathEncode($"{imageDirectory}{currentFolder}/{file.Name}");
                }

                var mouseAlt = new StringBuilder();
                mouseAlt.Append("<div style=\\'background-color:#FFFFFF;BORDER-TOP: #B6BCC6 1px solid;BORDER-LEFT: #B6BCC6 1px solid;BORDER-RIGHT: #B6BCC6 1px solid;	BORDER-BOTTOM: #B6BCC6 1px solid;position:absolute;\\'>");
                mouseAlt.Append("<table border=0 width=350><tr>");
                mouseAlt.Append($"<td width=200  align=center><img src=\\'{ConfigurationManager.AppSettings[CommunicatorVirtualPathConfig]}/includes/thumbnail.aspx?size=200&image={encodedPath}\\'></td>");
                mouseAlt.Append($"<td  width=150 class=TableContent><b>Name: {file.Name}</b><br>");
                mouseAlt.Append($"<br><b>TYPE:</b> {fileExtension}");
                mouseAlt.Append($"<br><b>Size:</b> {Convert.ToInt32(file.Length / 1000)}kb");
                mouseAlt.Append($"<br><b>Date:</b> {file.LastWriteTime}");
                mouseAlt.Append("<br><br><b><u>NOTE:</u></b><br>Click on the image to view its original size in a separate browser window.");
                mouseAlt.Append("</td></tr></table></div>");

                var dataRow = imagesTable.NewRow();
                dataRow[0] = $"<DIV onclick=\"javascript:window.open('{ConfigurationManager.AppSettings[ImageDomainPathConfig]}/Customers/{customerId}/images{currentFolder}/{file.Name}')\" onmouseover=\"return overlib('{mouseAlt}', FULLHTML, VAUTO, HAUTO, RIGHT, WIDTH, 350);\" onmouseout=\"return nd();\">{filename}</DIV>";
                dataRow[1] = file.Length.ToString();
                dataRow[2] = GetFileSize(file);
                dataRow[3] = fileExtension;
                dataRow[4] = file.LastWriteTime;
                dataRow[5] = filename;
                dataRow[6] = $"{ConfigurationManager.AppSettings[ImageDomainPathConfig]}/Customers/{customerId}/images{currentFolder}/{file.Name}";

                imagesTable.Rows.Add(dataRow);
            }

            return imagesTable;
        }

        public static DataTable GetPicturesTable(string customerId, string currentFolder, string[] currentFolderFiles, string thumbnailSize = "150", string imageDirectory = null)
        {
            var picturesTable = new DataTable();
            picturesTable.Columns.Add(new DataColumn("Image", typeof(string)));
            picturesTable.Columns.Add(new DataColumn("ImageName", typeof(string)));
            picturesTable.Columns.Add(new DataColumn("ImageDIV", typeof(string)));
            picturesTable.Columns.Add(new DataColumn("ImageKey", typeof(string)));
            picturesTable.Columns.Add(new DataColumn("ImagePath", typeof(string)));

            foreach (var filePath in currentFolderFiles)
            {
                try
                {
                    var file = new FileInfo(filePath);
                    var filename = file.Name;
                    var fileExtension = GetFileType(filename);
                    var shortFileName = filename.Length > 13 ? filename.Substring(0, 13) + "..." : filename;
                    var encodedPath = HttpContext.Current.Server.UrlPathEncode($"/Customers/{customerId}/images{currentFolder}/{file.Name}");
                    if (imageDirectory != null)
                    {
                        encodedPath = HttpContext.Current.Server.UrlPathEncode($"{imageDirectory}{currentFolder}/{file.Name}");
                    }
                    
                    var mouseAlt = new StringBuilder();
                    mouseAlt.Append("<div style=\\'background-color:#FFFFFF;BORDER-TOP: #B6BCC6 1px solid;BORDER-LEFT: #B6BCC6 1px solid;BORDER-RIGHT: #B6BCC6 1px solid;	BORDER-BOTTOM: #B6BCC6 1px solid;position:absolute;\\'>");
                    mouseAlt.Append("<table border=0 cellpadding=0 cellspacing=0 width=350><tr>");
                    mouseAlt.Append($"<td width=200 align=center><img src=\\'{ConfigurationManager.AppSettings[CommunicatorVirtualPathConfig]}/includes/thumbnail.aspx?size=200&image={encodedPath}\\'></td>");
                    mouseAlt.Append($"<td  width=150 class=TableContent><b>Name: {file.Name}</b><br>");
                    mouseAlt.Append($"<br><b>TYPE:</b> {fileExtension}");
                    mouseAlt.Append($"<br><b>Size:</b> {Convert.ToInt32(file.Length / 1000)}kb");
                    mouseAlt.Append($"<br><b>Date:</b> {file.LastWriteTime.ToShortDateString()}");
                    mouseAlt.Append("<br><br><b><u>NOTE:</u></b><br>Click on the image to view its original size in a separate browser window.");
                    mouseAlt.Append("</td></tr></table></div>");

                    

                    var thumbImage = $"{ConfigurationManager.AppSettings[CommunicatorVirtualPathConfig]}/includes/thumbnail.aspx?size={thumbnailSize}&image={encodedPath}";

                    var dataRow = picturesTable.NewRow();
                    dataRow[0] = thumbImage;
                    dataRow[1] = shortFileName;
                    dataRow[2] = $"<DIV onclick=\"javascript:window.open(\'{ConfigurationManager.AppSettings[ImageDomainPathConfig]}/Customers/{customerId}/images{currentFolder}/{file.Name}\')\" onmouseover=\"return overlib(\'{mouseAlt}\', FULLHTML, VAUTO, HAUTO, RIGHT, WIDTH, 350);\" onmouseout=\"return nd();\">";
                    dataRow[3] = filename;
                    dataRow[4] = $"{ConfigurationManager.AppSettings["Image_DomainPath"]}/Customers/{customerId}/images{currentFolder}/{file.Name}";

                    picturesTable.Rows.Add(dataRow);
                }
                catch
                {
                    break;
                }
            }

            return picturesTable;
        }

        private static string GetFileType(string filename)
        {
            string fileType;
            if (filename.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
            {
                fileType = "JPG";
            }
            else if (filename.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
            {
                fileType = "GIF";
            }
            else
            {
                fileType = "PNG";
            }

            return fileType;
        }
        
        private static string GetFileSize(System.IO.FileInfo file)
        {
            string fileSize;
            if (file.Length > 1024)
            {
                fileSize = Convert.ToDouble(file.Length / 1024) + " KB";
            }
            else if (file.Length == 1024)
            {
                fileSize = "1 KB";
            }
            else
            {
                fileSize = file.Length + " Bytes";
            }

            return fileSize;
        }
    }
}

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Helpers;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using CommonStringFunctions = KM.Common.StringFunctions;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class NewFile
    {
        private const int KbSize = 1024;

        #region Properties
        #endregion Properties

        #region Methods

        //Create Directory if it does not already exist. Takes Folder Type as a parameter.
        public static string CreateDirectoryIfDNE(string Path, ECN_Framework_Common.Objects.Enums.Entity type)
        {
            if (type == ECN_Framework_Common.Objects.Enums.Entity.ImageFolder)
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + Path)))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + Path));
                    return (HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + Path)).ToString();
                }
                else return (HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + Path)).ToString();
            }
            //different folder types to be added here.
            return "";
        }
        public static void DeleteImage(string fullPathName)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(fullPathName);
            file.Delete();
        }

        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns> A tuple containing (pagerPageSize, pagerCurrentPage)</returns>
        public static void DeleteSelectedImages(ArrayList filesToDelete)
        {
            System.IO.FileInfo file = null;

            for (int i = 0; i < filesToDelete.Count; i++)
            {
                file = new System.IO.FileInfo(HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/" + filesToDelete[i]));
                file.Delete();
            }
        }
        #region Files -- DataTable
        public static DataTable GetDataTable_Files(string datapath, string DataWebPath)
        {
            DataTable dtFiles = new DataTable();
            DataColumn dcFileName = new DataColumn("FileName", typeof(string));
            DataColumn dcSize = new DataColumn("Size", typeof(string));
            DataColumn dcDate = new DataColumn("Date", typeof(string));
            DataColumn dcPath = new DataColumn("BrowsePath", typeof(string));
            DataRow drFiles;
            dtFiles.Columns.Add(dcFileName);
            dtFiles.Columns.Add(dcSize);
            dtFiles.Columns.Add(dcDate);
            dtFiles.Columns.Add(dcPath);

            System.IO.FileInfo file = null;
            string[] files = null;
            string filename = "";
            files = System.IO.Directory.GetFiles(datapath, "*.*");

            for (int i = 0; i <= files.Length - 1; i++)
            {
                //Create a new FileInfo object for this filename
                file = new System.IO.FileInfo(files[i]);
                filename = file.Name.ToString();
                /*				if ( filename.ToLower().EndsWith(".ini") || 
                                    filename.ToLower().EndsWith(".txt") || 
                                    filename.ToLower().EndsWith(".csv") || 
                                    filename.ToLower().EndsWith(".xls")) 
                                {*/
                drFiles = dtFiles.NewRow();
                drFiles[0] = file.Name;
                drFiles[1] = (file.Length / 1000) + "kb";
                drFiles[2] = file.LastWriteTime.ToShortDateString();
                drFiles[3] = DataWebPath + "/" + file.Name;
                dtFiles.Rows.Add(drFiles);
                //				}
            }
            return dtFiles;
        }
        #endregion Files -- DataTable
        #region Image -- DataTable

        public static DataTable GetDataTable_Image(string customerId, string directory, string currentFolder, Page page, RadioButtonList imgListViewRb = null, string thumbnailSize = "")
        {
            var currentFolderFiles = Directory.GetFiles(directory, "*.*")
                .Where(fileName => fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                                   || fileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)
                                   || fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                .ToArray();
            
            if (imgListViewRb != null && imgListViewRb.SelectedValue.Equals("DETAILS", StringComparison.OrdinalIgnoreCase))
            {
                return ImageGalleryHelper.GetImagesTable(customerId, currentFolder, currentFolderFiles);
            }
            else
            {
                return ImageGalleryHelper.GetPicturesTable(customerId, currentFolder, currentFolderFiles);
            }
        }

        public static System.Data.DataTable GetDataTable_Image(string customerID, string currentImageDirectory, System.Web.UI.Page page, string thumbnailSize = "")
        {
            DataTable dtPictures = new DataTable();
            DataColumn dcImageName = new DataColumn("ImageName", typeof(string));
            DataColumn dcImageSizeRaw = new DataColumn("ImageSizeRaw", typeof(int));
            DataColumn dcImageSize = new DataColumn("ImageSize", typeof(string));
            DataColumn dcImageType = new DataColumn("ImageType", typeof(string));
            DataColumn dcImageDtModified = new DataColumn("ImageDtModified", typeof(DateTime));
            DataColumn dcImageKey = new DataColumn("ImageKey", typeof(string));
            DataColumn dcImagePath = new DataColumn("ImagePath", typeof(string));
            DataRow drPictures;

            dtPictures.Columns.Add(dcImageName);
            dtPictures.Columns.Add(dcImageSizeRaw);
            dtPictures.Columns.Add(dcImageSize);
            dtPictures.Columns.Add(dcImageType);
            dtPictures.Columns.Add(dcImageDtModified);
            dtPictures.Columns.Add(dcImageKey);
            dtPictures.Columns.Add(dcImagePath);

            System.IO.FileInfo file = null;
            string[] files = null;
            string filename = "";
            files = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath(currentImageDirectory), "*.*");

            for (int i = 0; i <= files.Length - 1; i++)
            {
                //Create a new FileInfo object for this filename
                file = new System.IO.FileInfo(files[i]);
                filename = file.Name.ToString();
                if (filename.ToLower().EndsWith(".jpg") || filename.ToLower().EndsWith(".gif") ||
                    filename.ToLower().EndsWith(".jpeg") || filename.ToLower().EndsWith(".bmp") ||
                    filename.ToLower().EndsWith(".tif") || filename.ToLower().EndsWith(".tiff") || filename.ToLower().EndsWith(".png"))
                {
                    drPictures = dtPictures.NewRow();
                    drPictures[0] = file.Name;
                    drPictures[1] = file.Length.ToString();//System.Configuration.ConfigurationManager.AppSettings["Accounts_VirtualPath"] + "/includes/thumbnail.aspx?size=" + thumbnailSize + "&image=" + HttpContext.Current.Server.UrlPathEncode(currentImageDirectory + "images/" + file.Name);
                    var fileSize = (int)file.Length;
                    var imgSize = $"{fileSize} Bytes";
                    if (fileSize > KbSize)
                    {
                        imgSize = $"{Convert.ToDouble(fileSize / KbSize)} KB";
                    }
                    else if (fileSize == KbSize)
                    {
                        imgSize = "1 KB";
                    }
                    drPictures[2] = imgSize;// HttpContext.Current.Server.UrlPathEncode(currentImageDirectory + "images/" + file.Name);
                    drPictures[3] = "";// filename.Substring(filename.LastIndexOf("." + 1));
                    drPictures[4] = file.LastWriteTime.ToShortDateString();
                    drPictures[5] = System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"] + "/includes/thumbnail.aspx?size=" + thumbnailSize + "&image=" + HttpContext.Current.Server.UrlPathEncode("/Customers/" + customerID + "/images/" + file.Name);//System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"] + currentImageDirectory + "/" + file.Name; 
                    drPictures[6] = "/Customers/" + customerID + "/images/" + file.Name;
                    dtPictures.Rows.Add(drPictures);
                }
            }
            return dtPictures;
        }
        #endregion Image -- DataTable
        #region ImageFolder -- DataTable
        public static System.Data.DataTable GetDataTable_ImageFolder(string customerID, string directory = null)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DirID");
            dt.Columns.Add("DirName");
            DataRow dr = null;
            string[] dirs = null;
            System.IO.DirectoryInfo dir = null;

            string path = "/customers/" + customerID + "/images";
            string append = directory != null ? "/" + directory : "";
            CreateDirectoryIfDNE(path, ECN_Framework_Common.Objects.Enums.Entity.ImageFolder);
            dirs = System.IO.Directory.GetDirectories(HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + customerID + "/images" + append));

            dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "ROOT Folder";
            dt.Rows.Add(dr);

            for (int i = 0; i <= dirs.Length - 1; i++)
            {
                dir = new System.IO.DirectoryInfo(dirs[i]);
                dt = GetRecursiveImageFolders(dt, dir, dir.Name, customerID);
            }
            return dt;
        }
        private static DataTable GetRecursiveImageFolders(DataTable dtFolders, System.IO.DirectoryInfo dir, string currentdirectory, string customerID)
        {
            DataRow drFolders;
            drFolders = dtFolders.NewRow();

            string imageDirectory = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + customerID + "/images";

            string dirName = dir.Name.ToString();
            drFolders = dtFolders.NewRow();
            drFolders[0] = currentdirectory;
            drFolders[1] = " - - - " + currentdirectory;
            dtFolders.Rows.Add(drFolders);

            string[] dirs = null;
            dirs = System.IO.Directory.GetDirectories(HttpContext.Current.Server.MapPath(imageDirectory + "/" + currentdirectory));

            for (int i = 0; i <= dirs.Length - 1; i++)
            {
                System.IO.DirectoryInfo subdir = new System.IO.DirectoryInfo(dirs[i]);
                dtFolders = GetRecursiveImageFolders(dtFolders, subdir, currentdirectory + "/" + subdir.Name, customerID);
            }

            return dtFolders;
        }
        #endregion ImageFolder
        #region ImageFolder -- Source

        public static string GetSourceForTable_ImageFolder_CKeditor(string customerID, int channelID, string directory, string currentFolder, string currentParent, string parentID, string CKEditorFuncNum, string langCode)
        {
            System.IO.DirectoryInfo dir = null;
            string[] dirs = null;
            string foldersCode = "";
            string selectedStyle = "class='gridaltrowWizard'";
            string normalStyle = "class='tableContent'";
            CreateDirectoryIfDNE(directory, Enums.Entity.ImageFolder);
            dirs = System.IO.Directory.GetDirectories(HttpContext.Current.Server.MapPath(directory));
            dir = new System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath(directory));

            //ckeditor foldersCode
            foldersCode += "<tr " + ((currentFolder.Length > 0) ? normalStyle : selectedStyle) + "><td colspan=2 align='left'><table border=0><tr><td><img src=" + ((currentFolder.Length > 0) ? "'/ecn.images/icons/folder_img_25_closed.gif'" : "'/ecn.images/icons/folder_img_25_open.gif'") + "></td><td align='left' valign=middle>&nbsp;&nbsp;<a href='filemanager.aspx?chID=" + channelID + "&cuID=" + customerID + "&CKEditor=" + parentID + "&CKEditorFuncNum=" + CKEditorFuncNum + "&langCode=" + langCode + "' >ROOT</a></td></tr></table></td></tr>";


            //Carried over from existing places, in case it stil holds some significance.
            //for (int i = 0; i <= dirs.Length - 1; i++)
            //{
            //    dir = new System.IO.DirectoryInfo(dirs[i]);
            //    dirname = dir.Name.ToString();
            //    foldersCode += "<tr><td width=20></td><td style='Padding-bottom:3px' align=center class='tableContent' " + ((currentFolder.Equals("/" + dirname)) ? "bgcolor='#EBEBEC'" : "") + "><img src=" + ((currentFolder.Equals("/" + dirname)) ? "'/ecn.images/icons/folder_img_25_open.gif'" : "'/ecn.images/icons/folder_img_25_closed.gif'") + "></td><td style='Padding-left:4px;Padding-top:4px' valign=middle " + ((currentFolder.Equals("/" + dirname)) ? "bgcolor='#EBEBEC'" : "") + ">";
            //    //if(currentParent.Equals("ImageManagerForm")){
            //    foldersCode += "<a href='filemanager.aspx?folder=/" + dirname + " '>" + dirname + "</a></td></tr>";
            //    //}else{
            //    //    foldersCode += "<a href='browse.aspx?chID="+channelID+"&cuID="+customerID+"&folder=/"+dirname+" '>"+ dirname+"</a></td></tr>";
            //    //}
            //}

            for (int i = 0; i <= dirs.Length - 1; i++)
            {
                dir = new System.IO.DirectoryInfo(dirs[i]);
                foldersCode = GetRecursiveImageFoldersSource_CKeditor(foldersCode, dir, dir.Name, currentFolder, currentParent, "", customerID, channelID.ToString(), parentID, CKEditorFuncNum, langCode);
            }
            return foldersCode;
        }
        private static string GetRecursiveImageFoldersSource_CKeditor(string foldersCode, System.IO.DirectoryInfo dir, string currentdirectory, string currentFolder, string currentParent, string spacer, string customerID, string channelID, string parentID, string CKEditorFuncNum, string langCode)
        {

            string imageDirectory = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + customerID + "/images";

            string dirName = dir.Name.ToString();

            foldersCode += "<tr><td width=5%></td><td width=95%  align=left class='tableContent' " + ((currentFolder.Equals("/" + currentdirectory)) ? "bgcolor='#EBEBEC'" : "") + ">" + spacer + "<img src='/ecn.images/images/L.gif' style='height:auto;vertical-align: middle;'>&nbsp;<img  style='height:auto;vertical-align: middle;' src=" + ((currentFolder.Equals("/" + currentdirectory)) ? "'/ecn.images/icons/folder_img_25_open.gif'" : "'/ecn.images/icons/folder_img_25_closed.gif'") + ">";
            if (currentParent.Equals("ImageManagerForm"))
            {
                foldersCode += "<span style='Padding-left:10px;'><a href='filemanager.aspx?chID=" + channelID + "&cuID=" + customerID + "&folder=/" + currentdirectory + "&CKEditor=" + parentID + "&CKEditorFuncNum=" + CKEditorFuncNum + "&langCode=" + langCode + " '>" + dirName + "</a></span></td></tr>";
            }
            else
            {
                foldersCode += "<span style='Padding-left:10px;'><a href='browse.aspx?chID=" + channelID + "&cuID=" + customerID + "&folder=/" + currentdirectory + "&CKEditor=" + parentID + "&CKEditorFuncNum=" + CKEditorFuncNum + "&langCode=" + langCode + " '>" + dirName + "</a></span></td></tr>";
            }

            string[] dirs = null;
            dirs = System.IO.Directory.GetDirectories(HttpContext.Current.Server.MapPath(imageDirectory + "/" + currentdirectory));

            for (int i = 0; i <= dirs.Length - 1; i++)
            {
                System.IO.DirectoryInfo subdir = new System.IO.DirectoryInfo(dirs[i]);
                foldersCode = GetRecursiveImageFoldersSource_CKeditor(foldersCode, subdir, currentdirectory + "/" + subdir.Name, currentFolder, currentParent, spacer + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", customerID, channelID, parentID, CKEditorFuncNum, langCode);
            }

            return foldersCode;
        }

        public static string GetSourceForTable_ImageFolder(string customerID, string directory, string currentFolder, string fileName, string queryString = null)
        {
            System.IO.DirectoryInfo dir = null;
            string[] dirs = null;
            string foldersCode = "";
            string selectedStyle = "class='gridaltrowWizard'";
            string normalStyle = "class='tableContent'";
            CreateDirectoryIfDNE(directory, Enums.Entity.ImageFolder);
            dirs = System.IO.Directory.GetDirectories(HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + directory));
            dir = new System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + directory));

            if (queryString.Contains("folder"))
            {
                queryString = queryString.Remove(queryString.IndexOf("folder"), queryString.IndexOf("&", queryString.IndexOf("folder")) - queryString.IndexOf("folder") + 1);
            }

            foldersCode += "<tr " + ((currentFolder.Length > 0) ? normalStyle : selectedStyle) + "><td colspan=3><table border=0><tr><td><img src=" + ((currentFolder.Length > 0) ? "'/ecn.images/icons/folder_img_25_closed.gif'" : "'/ecn.images/icons/folder_img_25_open.gif'") + "></td><td valign=middle>&nbsp;&nbsp;<a href='" + fileName + "?" + queryString + "' >ROOT</a></td></tr></table></td></tr>";

            //Carried over from existing places, in case it stil holds some significance.
            //for (int i = 0; i <= dirs.Length - 1; i++)
            //{
            //    dir = new System.IO.DirectoryInfo(dirs[i]);
            //    dirname = dir.Name.ToString();
            //    foldersCode += "<tr><td width=20></td><td style='Padding-bottom:3px' align=center class='tableContent' " + ((currentFolder.Equals("/" + dirname)) ? "bgcolor='#EBEBEC'" : "") + "><img src=" + ((currentFolder.Equals("/" + dirname)) ? "'/ecn.images/icons/folder_img_25_open.gif'" : "'/ecn.images/icons/folder_img_25_closed.gif'") + "></td><td style='Padding-left:4px;Padding-top:4px' valign=middle " + ((currentFolder.Equals("/" + dirname)) ? "bgcolor='#EBEBEC'" : "") + ">";
            //    //if(currentParent.Equals("ImageManagerForm")){
            //    foldersCode += "<a href='filemanager.aspx?folder=/" + dirname + " '>" + dirname + "</a></td></tr>";
            //    //}else{
            //    //    foldersCode += "<a href='browse.aspx?chID="+channelID+"&cuID="+customerID+"&folder=/"+dirname+" '>"+ dirname+"</a></td></tr>";
            //    //}
            //}

            for (int i = 0; i <= dirs.Length - 1; i++)
            {
                dir = new System.IO.DirectoryInfo(dirs[i]);
                foldersCode = GetRecursiveImageFoldersSource(foldersCode, dir, dir.Name, currentFolder, queryString, fileName, "", customerID);
            }
            return foldersCode;
        }
        private static string GetRecursiveImageFoldersSource(string foldersCode, DirectoryInfo dir, string currentdirectory, string currentFolder, string queryString, string fileName, string spacer, string customerID)
        {
            string imageDirectory = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + customerID + "/images";

            string dirName = dir.Name.ToString();

            if (queryString.Contains("folder"))
            {

                int indexOfAMP = -1;
                try
                {
                    indexOfAMP = queryString.IndexOf("&", queryString.IndexOf("folder") + 6);
                }
                catch
                {

                }
                if (indexOfAMP == -1)
                {
                    queryString = queryString.Remove(queryString.IndexOf("folder"));
                }
                else
                {
                    queryString = queryString.Remove(queryString.IndexOf("folder"), queryString.IndexOf("&", queryString.IndexOf("folder")) - queryString.IndexOf("folder") + 1);
                }
            }

            foldersCode += "<tr><td width=5%></td><td width=95%  align=left class='tableContent' " + ((currentFolder.Equals("/" + currentdirectory)) ? "bgcolor='#EBEBEC'" : "") + ">" + spacer + "<img src='/ecn.images/images/L.gif' style='height:auto;vertical-align: middle;'>&nbsp;<img  style='height:auto;vertical-align: middle;' src=" + ((currentFolder.Equals("/" + currentdirectory)) ? "'/ecn.images/icons/folder_img_25_open.gif'" : "'/ecn.images/icons/folder_img_25_closed.gif'") + ">";
            foldersCode += "<span style='Padding-left:10px;'><a href='" + fileName + "?folder=/" + currentdirectory + "&" + queryString + " '>" + dirName + "</a></span></td></tr>";

            string[] dirs = null;
            dirs = System.IO.Directory.GetDirectories(HttpContext.Current.Server.MapPath(imageDirectory + "/" + currentdirectory));

            for (int i = 0; i <= dirs.Length - 1; i++)
            {
                System.IO.DirectoryInfo subdir = new System.IO.DirectoryInfo(dirs[i]);
                foldersCode = GetRecursiveImageFoldersSource(foldersCode, subdir, currentdirectory + "/" + subdir.Name, currentFolder, queryString, fileName, spacer + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", customerID);
            }

            return foldersCode;
        }
        #endregion ImageFolder -- Source
        #region Storage
        public static decimal StorageUsed(string ImagesFilePath, string DataFilePath)
        {
            decimal MB = 0.0M;
            DataStorageCalculator imgStorageSize = new DataStorageCalculator();
            MB += imgStorageSize.GetSizeInMegaBytes(ImagesFilePath);

            DataStorageCalculator dataStorageSize = new DataStorageCalculator();
            MB += dataStorageSize.GetSizeInMegaBytes(DataFilePath);

            return MB;
        }

        public static decimal StoragePurchased(string CustomerID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Accounts.View.CustomerDiskUsage customerDiskUsage = ECN_Framework_BusinessLayer.Accounts.View.CustomerDiskUsage.GetByCustomerID(Convert.ToInt32(CustomerID), user);
            try
            {
                return Convert.ToDecimal(customerDiskUsage.AllowedStorage);
            }
            catch
            {
                return 10;
            }
        }

        public static decimal StoragePercentageUsed(decimal used, decimal purchased)
        {
            return (100 * (used / purchased));
        }
        #endregion Storage

        //private void throwECNException(string message, PlaceHolder phError, Label lblErrorMessage)
        //{
        //    ECNError ecnError = new ECNError(Enums.Entity.Link, Enums.Method.Get, message);
        //    List<ECNError> errorList = new List<ECNError> { ecnError };
        //    setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite), phError, lblErrorMessage);
        //}

        public static string GetBaseLocation(string uploadDirectory, string FolderName = "", string currentParent = "")
        {
            string baseLocation = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + uploadDirectory);
            if (currentParent.Equals("ImageManagerForm") || currentParent.Equals("PanelUpload") || currentParent.Equals("PanelUpload2"))
            {
                if (FolderName.Length > 0)
                {
                    baseLocation += @"\" + FolderName;
                }
            }
            else if (currentParent.Equals("SocialPanelUpload"))
            {
                if (FolderName.Length > 0)
                {
                    baseLocation += @"\" + FolderName;

                }

            }
            else if (currentParent.Equals("gatewayUpload"))
            {
                if (FolderName.Length > 0)
                {
                    baseLocation += @"\" + FolderName;

                }
            }
            else
            {
            }
            return baseLocation;
        }
        public static string GetRedirectURL(string currentParent, string uploadDirectory, string folderName, string pathAndQuery)
        {
            string redirectURL = "";
            if (currentParent.Equals("ImageManagerForm") || currentParent.Equals("PanelUpload"))
            {
                redirectURL = "filemanager.aspx";
                if (folderName.Length > 0)
                {
                    redirectURL += "?folder=/" + folderName;
                }
            }
            else if (currentParent.Equals("PanelUpload2"))
            {
                redirectURL = "";

                if (pathAndQuery.Contains("folder="))
                {
                    // pathAndQuery = pathAndQuery.Remove(pathAndQuery.IndexOf("folder="), (pathAndQuery.IndexOf("&", pathAndQuery.IndexOf("folder=")) - pathAndQuery.IndexOf("folder=")+1));
                    pathAndQuery = pathAndQuery.Remove(pathAndQuery.IndexOf("folder="));
                if (folderName.Length > 0)
                {
                    redirectURL = pathAndQuery + "&folder=/" + folderName;
                }
                else 
                {
                    redirectURL = pathAndQuery;
                }
            }

            else
            {
                if (folderName.Length > 0)
                {
                        redirectURL = pathAndQuery + "&folder=/" + folderName;
                }
                    else  redirectURL += pathAndQuery;
                }
            }
            return redirectURL;
        }


        //pathInfo = Request.ServerVariables["PATH_INFO"].ToString();
        public static bool Upload(ArrayList aFiles, string baseLocation, string customerID, string userID, Label MessageLabel, ListBox FilesListBox, string pathInfo)
        {
            int filesUploaded = 0;
            bool bError = false;
            string status = "";
            //Label MessageLabel = (Label)page.FindControl("MessageLabel");
            if (MessageLabel == null)
            {
                //List<ECNError> lError = new List<ECNError>();
                //ECNError error = new ECNError(ECN_Framework_Common.Objects.Enums.Entity.Image, ECN_Framework_Common.Objects.Enums.Method.Create, "MessageLabel is null");
                //lError.Add(error);
                //throw new ECNException(lError, Enums.ExceptionLayer.Business);
            }

            //ListBox FilesListBox = (ListBox)page.FindControl("FilesListBox");
            if (FilesListBox == null)
            {
                //List<ECNError> lError = new List<ECNError>();
                //ECNError error = new ECNError(ECN_Framework_Common.Objects.Enums.Entity.Image, ECN_Framework_Common.Objects.Enums.Method.Create, "FilesListBox is null");
                //lError.Add(error);
                //throw new ECNException(lError, Enums.ExceptionLayer.Business);
            }

            if ((FilesListBox.Items.Count == 0) && (filesUploaded == 0))
            {

                if (MessageLabel != null) MessageLabel.Text = "ERROR: Upload Function needs at least 1 file added in the list box.";
                return true;
            }
            else
            {
                foreach (System.Web.UI.HtmlControls.HtmlInputFile f in aFiles)
                {
                    string fn = string.Empty;
                    try
                    {
                        fn = System.IO.Path.GetFileNameWithoutExtension(f.PostedFile.FileName);
                        fn = CommonStringFunctions.ReplaceNonAlphaNumeric(fn, "_");
                        f.PostedFile.SaveAs(baseLocation + "\\" + fn + System.IO.Path.GetExtension(f.PostedFile.FileName));
                        filesUploaded++;
                        status += fn + "<br>";

                        try
                        {
                            ECN_Framework_DataLayer.DataFunctions.Execute("insert into Uploadlog (UserID,CustomerID,Path,FileName,uploaddate,PageSource) values (" + userID + "," + customerID + ",'" + baseLocation + "','" + fn.Replace("'", "''") + "',getdate(),'" + pathInfo + "')", System.Configuration.ConfigurationManager.ConnectionStrings["Communicator"].ToString());
                        }
                        catch
                        { }

                    }
                    catch (Exception err)
                    {
                        bError = true;
                        if (MessageLabel != null) MessageLabel.Text = "Error File Save " + fn + "<br>" + err.Message.ToString();
                    }
                }
                if (!bError)
                {
                    if (filesUploaded == aFiles.Count)
                    {
                        if (MessageLabel != null) MessageLabel.Text = "These " + filesUploaded + " file(s) were uploaded:<br>" + status;
                    }

                    return bError;
                    //Session["sFiles" + sc.UserID()] = new ArrayList();
                    //aFiles.Clear();
                    //FilesListBox.Items.Clear();

                    //if(!currentParent.Equals("gatewayUpload"))
                    //    Response.Redirect(redirectURL);
                    //else
                    //{
                    //    RaiseBubbleEvent("upload", new EventArgs());
                    //}
                }
            }
            return bError;
        }

        #endregion Methods
    }
}


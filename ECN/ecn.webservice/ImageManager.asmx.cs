using System;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Xml;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Services.Description;
using ecn.webservice.classes;
using ecn.common.classes;
using ecn.communicator.classes;
using System.Text.RegularExpressions;

namespace ecn.webservice
{
    [WebService(
          Namespace = "http://webservices.ecn5.com/",
          Description = "The ECN Application Programming Interface (API) is a web service that allows you to control your ECN account programatically via an HTTP POST, an HTTP GET, or an XML-based SOAP call. The following web service methods allow access to managing your Images in ECN. The supported methods are shown below. <u>IMPORTANT NOTE:</u> All methods need ECN ACCESS KEY to work properly.")
     ]
    public class ImageManager : System.Web.Services.WebService
    {
        public ImageManager()
        {

        }

        #region Get Image Folders - GetFolders()
        [WebMethod(
             Description = "Get Image Folders in ECN. ", MessageName = "GetFolders")
        ]
        public string GetFolders(string ecnAccessKey)
        {
            return GetFolderMain(ecnAccessKey, string.Empty);
        }
        #endregion

        #region Get Image Folders Within Folder - GetFolders()
        [WebMethod(
             Description = "Get Image Folders within a folder in ECN. ", MessageName = "GetFoldersWithinFolder")
        ]
        public string GetFolders(string ecnAccessKey, string FolderName)
        {
            return GetFolderMain(ecnAccessKey, FolderName);
        }
        #endregion

        #region Add Image Folder - AddFolder()
        [WebMethod(
             Description = "Add Image Folder to ECN. ", MessageName = "AddFolder")
        ]
        public string AddFolder(string ecnAccessKey, string FolderName)
        {
            return AddFolderMain(ecnAccessKey, FolderName, string.Empty);
        }
        #endregion

        #region Add Image Folder Within A Folder - AddFolder()
        [WebMethod(
             Description = "Add Image Folder within a folder in ECN. ", MessageName = "AddFolderWithinFolder")
        ]
        public string AddFolder(string ecnAccessKey, string FolderName, string ParentFolderName)
        {
            return AddFolderMain(ecnAccessKey, FolderName, ParentFolderName);
        }
        #endregion

        #region Get Images - GetImages()
        [WebMethod(
             Description = "Get Images from ECN. ", MessageName = "GetImages")
        ]
        public string GetImages(string ecnAccessKey)
        {
            return GetImagesMain(ecnAccessKey, string.Empty);
        }
        #endregion

        #region Get Images From Folder - GetImages()
        [WebMethod(
             Description = "Get Images within a folder in ECN. ", MessageName = "GetImagesFromFolder")
        ]
        public string GetImages(string ecnAccessKey, string FolderName)
        {
            return GetImagesMain(ecnAccessKey, FolderName);
        }
        #endregion

        #region Add Image - AddImage()
        [WebMethod(
             Description = "Add Image to ECN. ", MessageName = "AddImage")
        ]
        public string AddImage(string ecnAccessKey, byte[] Image, string ImageName)
        {
            return AddImageMain(ecnAccessKey, Image, ImageName, string.Empty);
        }
        #endregion

        #region Add Image Within A Folder - AddImage()
        [WebMethod(
             Description = "Add Image to a folder in ECN. ", MessageName = "AddImageToFolder")
        ]
        public string AddImage(string ecnAccessKey, byte[] Image, string ImageName, string FolderName)
        {
            return AddImageMain(ecnAccessKey, Image, ImageName, FolderName);
        }
        #endregion

        #region Private Methods

        private string AddFolderMain(string ecnAccessKey, string folderName, string parentFolderName)
        {
            StringWriter swFolders = new StringWriter();

            try
            {
                KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ecnAccessKey, true);
                user.CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(user.DefaultClientID, false).CustomerID;
                if (user != null)
                {
                    string currentImageDirectory = string.Empty;
                    if (parentFolderName.Trim().Length > 0)
                    {
                        currentImageDirectory = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Customers/" + user.CustomerID.ToString() + "/Images/" + parentFolderName + "/");
                    }
                    else
                    {
                        //currentImageDirectory = ConfigurationManager.AppSettings["Images_VirtualPath"] + "\\Customers\\" + authHandler.customerID.ToString() + "\\Images\\";
                        currentImageDirectory = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Customers/" + user.CustomerID.ToString() + "/Images/");
                    }
                    //check if valid directory
                    if(!Directory.Exists(currentImageDirectory))
                    {
                        return SendResponse.response("AddFolder", SendResponse.ResponseCode.Fail, 0, "DIRECTORY DOESN'T EXIST");
                    }
                    //check if new folder exists already
                    if (Directory.Exists(currentImageDirectory + folderName + "\\"))
                    {
                        return SendResponse.response("AddFolder", SendResponse.ResponseCode.Fail, 0, "FOLDER ALREADY EXISTS");
                    }
                    //create the folder
                    Directory.CreateDirectory(currentImageDirectory + folderName + "\\");
                    return SendResponse.response("AddFolder", SendResponse.ResponseCode.Success, 0, folderName);
                }
                else
                {
                    return SendResponse.response("AddFolder", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                }
            }
            catch (Exception Ex)
            {
                return SendResponse.response("AddFolder", SendResponse.ResponseCode.Fail, 0, Ex.ToString());
            }
        }
        
        private string GetFolderMain(string ecnAccessKey, string folderName)
        {
            string xmlFolder = "";
            StringWriter swFolders = new StringWriter();

            try
            {
                KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ecnAccessKey, true);
                user.CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(user.DefaultClientID, false).CustomerID;
                if (user != null)
                {
                    string currentImageDirectory = string.Empty;
                    if (folderName.Trim().Length > 0)
                    {
                        currentImageDirectory = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Customers/" + user.CustomerID.ToString() + "/Images/" + folderName + "/");
                    }
                    else
                    {
                        //currentImageDirectory = ConfigurationManager.AppSettings["Images_VirtualPath"] + "\\Customers\\" + authHandler.customerID.ToString() + "\\Images\\";
                        currentImageDirectory = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Customers/" + user.CustomerID.ToString() + "/Images/");
                    }
                    string[] folders = Directory.GetDirectories(currentImageDirectory);
                    for (int i = 0; i < folders.Length; i++)
                    {
                        folders[i] = folders[i].Substring(folders[i].LastIndexOf(@"\") + 1, folders[i].Length - folders[i].LastIndexOf(@"\") - 1);
                        xmlFolder += "<Folder><FolderName>" + folders[i] + "</FolderName></Folder>";

                    }

                    return SendResponse.response("GetFolders", SendResponse.ResponseCode.Success, 0, xmlFolder);
                }
                else
                {
                    return SendResponse.response("GetFolders", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                }
            }

            catch (Exception Ex)
            {
                return SendResponse.response("GetFolders", SendResponse.ResponseCode.Fail, 0, Ex.ToString());
            }
        }

        private string GetImagesMain(string ecnAccessKey, string folderName)
        {
            string xmlFolder = string.Empty;
            StringWriter swFolders = new StringWriter();
            System.IO.FileInfo file = null;
            string filename = string.Empty;

            try
            {
                KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ecnAccessKey, true);
                user.CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(user.DefaultClientID, false).CustomerID;
                if (user != null)
                {
                    string currentImageDirectory = string.Empty;
                    if (folderName.Trim().Length > 0)
                    {
                        currentImageDirectory = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Customers/" + user.CustomerID.ToString() + "/Images/" + folderName + "/");
                    }
                    else
                    {
                        //currentImageDirectory = ConfigurationManager.AppSettings["Images_VirtualPath"] + "\\Customers\\" + authHandler.customerID.ToString() + "\\Images\\";
                        currentImageDirectory = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Customers/" + user.CustomerID.ToString() + "/Images/");
                    }
                    //check if valid directory
                    if(!Directory.Exists(currentImageDirectory))
                    {
                        return SendResponse.response("GetImages", SendResponse.ResponseCode.Fail, 0, "DIRECTORY DOESN'T EXIST");
                    }

                    string[] images = Directory.GetFiles(currentImageDirectory, "*.*");
                    currentImageDirectory = ConfigurationManager.AppSettings["Image_DomainPath"] + "/Customers/" + user.CustomerID.ToString() + "/Images/";
                    for (int i = 0; i < images.Length; i++)
                    {
                        //check if correct file type
                        file = new System.IO.FileInfo(images[i]);
                        filename = file.Name.ToString();
                        if (filename.ToLower().EndsWith(".jpg") || filename.ToLower().EndsWith(".gif") | filename.ToLower().EndsWith(".png"))
                        {
                            if(xmlFolder == string.Empty)
                            {
                                xmlFolder = "<Images>";
                            }
                            //add path to imagePaths
                            xmlFolder += "<Image><ImageName>" + filename + "</ImageName><ImageURL>" + currentImageDirectory + filename + "</ImageURL></Image>";
                        }

                    }
                    if(xmlFolder != string.Empty)
                    {
                        xmlFolder += "</Images>";
                    }

                    return SendResponse.response("GetImages", SendResponse.ResponseCode.Success, 0, xmlFolder);
                }
                else
                {
                    return SendResponse.response("GetImages", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                }
            }
            catch (Exception Ex)
            {
                return SendResponse.response("GetImages", SendResponse.ResponseCode.Fail, 0, Ex.ToString());
            }
        }

        private string AddImageMain(string ecnAccessKey, byte[] image, string imageName, string folderName)
        {
            //for testing
            //FileInfo fInfo = new FileInfo(@"c:\temp\34293_1413154542070_1628736174_926716_6566178_n.jpg");
            //long numBytes = fInfo.Length;
            //FileStream fStream = new FileStream(@"c:\temp\34293_1413154542070_1628736174_926716_6566178_n.jpg", FileMode.Open, FileAccess.Read);
            //BinaryReader br = new BinaryReader(fStream);
            //byte[] encodedImageTest = br.ReadBytes((int)numBytes);
            //System.Text.UTF8Encoding encTest = new System.Text.UTF8Encoding();
            //image = encTest.GetString(encodedImageTest);
            //br.Close();
            //fStream.Close();
            //fStream.Dispose();
            //end testing

            StringWriter swFolders = new StringWriter();

            try
            {
                KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ecnAccessKey, true);
                user.CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(user.DefaultClientID, false).CustomerID;
                if (user != null)
                {
                    string currentImageDirectory = string.Empty;
                    if (folderName.Trim().Length > 0)
                    {
                        currentImageDirectory = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Customers/" + user.CustomerID.ToString() + "/Images/" + folderName + "/");
                    }
                    else
                    {
                        //currentImageDirectory = ConfigurationManager.AppSettings["Images_VirtualPath"] + "\\Customers\\" + authHandler.customerID.ToString() + "\\Images\\";
                        currentImageDirectory = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Customers/" + user.CustomerID.ToString() + "/Images/");
                    }
                    //check if valid directory
                    if (!Directory.Exists(currentImageDirectory))
                    {
                        return SendResponse.response("AddImage", SendResponse.ResponseCode.Fail, 0, "DIRECTORY DOESN'T EXIST");
                    }
                    //check if image exists already
                    string[] currentImages = Directory.GetFiles(currentImageDirectory, imageName);
                    if(currentImages.Length > 0)
                    {
                        return SendResponse.response("AddImage", SendResponse.ResponseCode.Fail, 0, "IMAGE ALREADY EXISTS");
                    }
                    //create the image
                    //System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                    //byte[] encodedImage = encoding.GetBytes(image);
                    MemoryStream ms = new MemoryStream(image);
                    FileStream fs = new FileStream(currentImageDirectory + imageName, FileMode.Create);
                    ms.WriteTo(fs);
                    ms.Close();
                    fs.Close();
                    fs.Dispose(); 
                    return SendResponse.response("AddImage", SendResponse.ResponseCode.Success, 0, imageName);
                }
                else
                {
                    return SendResponse.response("AddImage", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                }
            }
            catch (Exception Ex)
            {
                return SendResponse.response("AddImage", SendResponse.ResponseCode.Fail, 0, Ex.ToString());
            }

        }
        #endregion

    }
}

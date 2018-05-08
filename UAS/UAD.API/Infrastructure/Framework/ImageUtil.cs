using UAD.API.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using ECN_Framework_Common.Objects;

namespace UAD.API.Infrastructure.Framework
{
    // placeholder until an Image entity is created in framework
    using ImageFrameworkModel = ECN_Framework_Entities.Communicator.Image;
    using DirectoryFrameworkModel = ECN_Framework_Entities.Communicator.ImageFolder;

    internal static class ImageUtil
    {

        public const Enums.Entity RelatedFrameworkEntity = Enums.Entity.None ;

        /// <summary>
        /// Placeholder for Business Layer implementation of search of customer images by imageName, folderName, both, or neither.
        /// </summary>
        /// <param name="customerID">id of the customer which images are to be searched</param>
        /// <param name="imageName">(optional) constrain the search to images with a particular name</param>
        /// <param name="folderName">(optional) constrain the search to images within a particular folder.  folder name 
        /// should be relative to the base-path for images for the customer</param>
        /// <returns>an enumeration of framework image objects</returns>
        public static IEnumerable<ImageFrameworkModel> SearchImages(int customerID, string imageName = null, string folderName = null, bool partialMatch = false, bool recursiveSearch = false)
        {
            string imageNameLowercased = null == imageName ? "" : imageName.ToLower();
            bool notConstrainedByFileName = String.IsNullOrWhiteSpace(imageNameLowercased);
            string imageDirectory = MakeCustomerImageDirectoryPath(customerID, folderName);

            return SearchImagesInternal(customerID, imageDirectory, imageNameLowercased, notConstrainedByFileName, partialMatch, recursiveSearch);
        }

        static IEnumerable<ImageFrameworkModel> SearchImagesInternal(int customerID, string imageDirectory, string imageNameLowercased, bool notConstrainedByFileName, bool partialMatch, bool recursiveSearch)
        {
            if(false == Directory.Exists(imageDirectory))
            {
                yield break;
            }

            IEnumerable<string> ImagesInFolder = from x in Directory.GetFiles(imageDirectory, "*.*")
                                                 where x.ToLower().EndsWith(".jpg")
                                                    || x.ToLower().EndsWith(".gif")
                                                    || x.ToLower().EndsWith(".png")
                                                 select x;
            
            foreach (string imageFilePath in ImagesInFolder)
            {
                FileInfo file = new System.IO.FileInfo(imageFilePath);
                string filename = file.Name.ToLower();

                string relpath = MakeCustomerImageRelativePath(customerID, file.Directory.ToString());

                if (notConstrainedByFileName || (partialMatch ? filename.Contains(imageNameLowercased) : filename == imageNameLowercased))
                {
                    ImageFrameworkModel fileObject = new ImageFrameworkModel
                    {
                        FolderName = relpath,
                        ImageName = file.Name,
                        //ImageData = File.ReadAllBytes(Path.Combine(imageDirectory, file.Name)).ToArray(),
                        ImageURL = MakeCustomerImageHref(customerID, file.Name, relpath)
                    };
                    yield return fileObject;
                }
            }

            if (recursiveSearch)
            {
                foreach (string subdirectoryFullPath in Directory.GetDirectories(imageDirectory))
                {
                    /// !!! recursion !!!
                    foreach (ImageFrameworkModel match in SearchImagesInternal(customerID, subdirectoryFullPath, imageNameLowercased, notConstrainedByFileName, partialMatch, recursiveSearch))
                    {
                        yield return match;
                    }
                }
            }
        }

        public static DirectoryFrameworkModel CreateFolderFromFullPath(int customerID, string folderFullPath)
        {
            string folderName = MakeCustomerImageRelativePath(customerID, folderFullPath);
            return new DirectoryFrameworkModel
            {
                FolderName = folderName,
                FolderUrl = Infrastructure.Framework.ImageUtil.MakeCustomerImageHref(customerID,"",folderName),
                FolderFullName = Infrastructure.Framework.ImageUtil.MakeCustomerImageRelativePath(customerID, folderFullPath)
            };
        }

        public static IEnumerable<DirectoryFrameworkModel> SearchFolders(int customerID, string folderName = null, bool partialMatch = false, bool recursiveSearch = false)
        {
            string imageDirectory = MakeCustomerImageDirectoryPath(customerID);
            bool notConstrainedByFileName = String.IsNullOrWhiteSpace(folderName);

            IEnumerable<string> foldersInFolder = Directory.GetDirectories(imageDirectory, "*.*", recursiveSearch ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            string folderNameCleaned = notConstrainedByFileName ? null : folderName.Trim().ToLower();

            foreach(string directoryFullPath in foldersInFolder)
            {
                if(notConstrainedByFileName)
                {
                    yield return CreateFolderFromFullPath(customerID, directoryFullPath);
                }
                else if(partialMatch)
                { 
                    if(directoryFullPath.ToLower().Contains( folderName))
                    {
                        yield return CreateFolderFromFullPath(customerID, directoryFullPath);
                    }
                }
                else if(directoryFullPath.ToLower().EndsWith( "\\" + folderNameCleaned ))
                {
                    yield return CreateFolderFromFullPath(customerID, directoryFullPath);
                }
            }
        }

        public static void CreateNewImage(int customerID, ImageFrameworkModel model)
        {
            Validate_CustomerImageName(customerID, model.ImageName);

            string imageDirectory = MakeCustomerImageDirectoryPath(customerID, model.FolderName);

            // verify folder exists here
            if(false == Directory.Exists( imageDirectory))
            {
                throw new ImageException(Strings.Errors.FriendlyMessages.Images.NO_DIRECTORY);
            }

            //check if image exists already
            string[] ImagesWithOurImageName = Directory.GetFiles(imageDirectory, model.ImageName);
            if (ImagesWithOurImageName.Length != 0)
            {
                throw new ImageException(Strings.Errors.FriendlyMessages.Images.NOT_NEW_IMAGE);
            }

            //create new image
            using (MemoryStream ms = new MemoryStream(model.ImageData))
            {
                using(FileStream fs = new FileStream(ConcatenateImagePath(imageDirectory, model.ImageName), FileMode.Create))
                {
                    ms.WriteTo(fs);
                }
            }

            model.ImageURL = MakeCustomerImageHref(customerID, model.ImageName, model.FolderName);
        }


        private static string ImagesPhysicalPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["Images_PhysicalPath"];
            }
        }

        private static string ImageDomainPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"] ;
            }
        }

        public static string ImageBasePath(int customerID)
        {
            string returnValue = ImagesPhysicalPath + "\\Customers\\" + customerID.ToString() + "\\images\\";
            //string returnValue = ImageDomainPath + "/Customers/" + customerID.ToString() + "/Images/";
            if (false == Directory.Exists(returnValue))
            {
                throw new ApplicationException("CONFIGURATION ERROR:  customer image root folder does not exist: " + returnValue);
            }

            return returnValue;
        }

        public static string ImageBaseHref(int customerID)
        {
                string returnValue = ImageDomainPath + "/Customers/" + customerID.ToString() + "/Images/";

                return returnValue;
        }

        public static bool Validate_CustomerImageName(int customerID, string imageName, bool throwIfInvalid=true)
        {
            List<ECNError> errorList = new List<ECNError>();
            string imageNameTrimmed = imageName == null ? "" : imageName.Trim();

            if(String.IsNullOrWhiteSpace(imageNameTrimmed))
            {
                errorList.Add(new ECNError(RelatedFrameworkEntity, Enums.Method.Validate, "ImageName is required"));
            }

            if(    imageNameTrimmed.Contains('\\') 
                || imageNameTrimmed.Contains('/')
                || imageNameTrimmed.Contains(':')
                || imageNameTrimmed.StartsWith("."))
            {
                errorList.Add(new ECNError(RelatedFrameworkEntity, Enums.Method.Validate, "ImageName contains invalid characters"));
            }

            if(errorList.Count > 0)
            {
                if (throwIfInvalid)
                {
                    throw new ECNException(errorList);
                }
                return false;
            }

            return true;
        }

        public static bool Validate_CustomerImageFolderName(int customerID, string folderName, bool throwIfInvalid = true)
        {
            List<ECNError> errorList = new List<ECNError>();

            folderName = folderName == null ? "" : folderName.Trim();
            if(String.IsNullOrWhiteSpace(folderName))
            {
                errorList.Add(new ECNError(RelatedFrameworkEntity, Enums.Method.Validate, "FolderName is required"));
            }

            if(    folderName.StartsWith("\\")
                || folderName.StartsWith(".")
                || folderName.Contains(':')
                || folderName.Contains(".."))
            {
                errorList.Add(new ECNError(RelatedFrameworkEntity, Enums.Method.Validate, "FolderName contains invalid characters"));
            }

            if (errorList.Count > 0)
            {
                if (throwIfInvalid)
                {
                    throw new ECNException(errorList);
                }
                return false;
            }

            return true;
        }

        public static string MakeCustomerImageHref(int customerID, string imageName="", string folderName=null)
        {
            string safeFolderName = String.IsNullOrWhiteSpace(folderName) ? null : folderName.Trim();
            if(safeFolderName != null && safeFolderName.StartsWith("\\"))
            {
                safeFolderName = safeFolderName.Remove(0,1);  // remove leading slash
            }
            string physicalFolderPath = MakeCustomerImageDirectoryPath(customerID, safeFolderName);
            string relativeFolderPath = MakeCustomerImageRelativePath(customerID, physicalFolderPath);

            return ImageBaseHref(customerID) // 1. prefix with the domain path from web.config

                 + relativeFolderPath        // 2.1 add the image-storage location relative physical path
                 . Replace('\\', '/')        // 2.2 with directory path separator replaced by URL path separator 

                 + imageName;                // 3. then append the image name, if any
        }

        public static string MakeCustomerImageDirectoryPath(int customerID, string folderName = null, bool create=false)
        {
            string path = ImageBasePath(customerID);
            string folderNameTrimmed = folderName == null ? "" : folderName.Trim();
            if (false == String.IsNullOrWhiteSpace(folderName))
            {
                Validate_CustomerImageFolderName(customerID, folderNameTrimmed);

                // remote trailing slash from folderName, if present
                if (folderNameTrimmed.EndsWith("\\"))
                {
                    folderNameTrimmed = folderNameTrimmed.Substring(0, folderName.Length - 1);
                }
                path += folderNameTrimmed + "\\";
            }

            if(create && false == Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string MakeCustomerImageRelativePath(int customerID, string customerImageDirectoryPath)
        {
            if(String.IsNullOrWhiteSpace(customerImageDirectoryPath))
            {
                throw new ArgumentNullException("customerImageDirectoryPath");
            }
            int lengthOfImagePhysicalPath = MakeCustomerImageDirectoryPath(customerID).Length;

            string returnValue = customerImageDirectoryPath.Remove(0, lengthOfImagePhysicalPath - 1);
            return returnValue.StartsWith("\\") ? returnValue.Remove(0,1) : returnValue; // remove leading slash
        }

        private static string ConcatenateImagePath(string folderPath, string imageName)
        {
            return folderPath + imageName;
        }


        /* public static string MakeCustomerImageDirectoryPath(int customerID, string folderName=null)
        {
            string imageDirectory = MakeCustomerFolderPath(customerID, folderName);

            //create directory if it doesn't exist
            if (!Directory.Exists(imageDirectory))
            {
                //throw new ImageException(Strings.Errors.FriendlyMessages.Images.NO_DIRECTORY);
                System.Diagnostics.Trace.TraceWarning("creating image folder: " + imageDirectory);
                try
                {
                    DirectoryInfo dirInfo = Directory.CreateDirectory(imageDirectory);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("failed to create directory: " + imageDirectory, e);
                }
            }

            return imageDirectory;
        }*/
    }
}
using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text;
using KMPlatform;
using System.Web.Security;
using System.Configuration;
using System.IO;
using KMModels;

namespace KMWeb.Controllers
{
    public class ImageBrowserController : Kendo.Mvc.UI.EditorImageBrowserController
    {
        private const string contentFolderRoot = "/ecn.images/";
        private readonly DirectoryBrowser directoryBrowser;
        private readonly KMModels.ThumbnailCreator thumbnailCreator;
        private const string prettyName = "/images/";
        private static readonly string[] foldersToCopy = new[] { "~/MyImages/shared/" };

        public ImageBrowserController()
        {
            directoryBrowser = new DirectoryBrowser();
            thumbnailCreator = new ThumbnailCreator();
        }

        /// <summary>
        /// Gets the base paths from which content will be served.
        /// </summary>
        public override string ContentPath
        {
            get
            {
                return contentFolderRoot + "Customers/" + CurrentUser.CustomerID.ToString() + prettyName;
            }
        }
        private string CreateUserFolder()
        {
            var virtualPath = contentFolderRoot + "Customers/" + CurrentUser.CustomerID.ToString() + prettyName;

            var path = Server.MapPath(virtualPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                foreach (var sourceFolder in foldersToCopy)
                {
                    CopyFolder(Server.MapPath(sourceFolder), path);
                }
            }
            return virtualPath;
        }

        private void CopyFolder(string source, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            foreach (var file in Directory.EnumerateFiles(source))
            {
                var dest = Path.Combine(destination, Path.GetFileName(file));
                System.IO.File.Copy(file, dest);
            }

            foreach (var folder in Directory.EnumerateDirectories(source))
            {
                var dest = Path.Combine(destination, Path.GetFileName(folder));
                CopyFolder(folder, dest);
            }
        }  
        //
        // GET: /ImageBrowser/
        private KMPlatform.Entity.User CurrentUser
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser; }
        }

        private int CurrentClientGroupID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientGroupID; }
        }

        private int CurrentClientID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID; }
        }

        public ActionResult Index()
        {
            return View();
        }

         #region ImageActions
        private const string DefaultFilter = "*.png,*.gif,*.jpg,*.jpeg";
       
        private const int ThumbnailHeight = 80;
        private const int ThumbnailWidth = 80;

        private string GetContentPath()
        {
            return string.Format(ContentPath, CurrentUser.CustomerID);
        }
        private string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return ToAbsolute(GetContentPath());
            }

            return CombinePaths(ToAbsolute(GetContentPath()), path);
        }

        private string ToAbsolute(string virtualPath)
        {
            return VirtualPathUtility.ToAbsolute(virtualPath);
        }

        private string CombinePaths(string basePath, string relativePath)
        {
            return VirtualPathUtility.Combine(VirtualPathUtility.AppendTrailingSlash(basePath), relativePath);
        }

        public virtual bool AuthorizeUpload(string path, HttpPostedFileBase file)
        {
            return CanAccess(path) && IsValidFile(file.FileName);
        }

        protected virtual bool CanAccess(string path)
        {
            return path.StartsWith(ToAbsolute(GetContentPath()), StringComparison.OrdinalIgnoreCase);
        }

        private bool IsValidFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var allowedExtensions = DefaultFilter.Split(',');

            return allowedExtensions.Any(e => e.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase));
        }

        public virtual bool AuthorizeRead(string path)
        {
            return CanAccess(path);
        }

        public virtual bool AuthorizeThumbnail(string path)
        {
            return CanAccess(path);
        }

        public virtual bool AuthorizeDeleteFile(string path)
        {
            return CanAccess(path);
        }

        public virtual bool AuthorizeDeleteDirectory(string path)
        {
            return CanAccess(path);
        }

        protected virtual void DeleteFile(string path)
        {
            if (!AuthorizeDeleteFile(path))
            {
                throw new HttpException(403, "Forbidden");
            }

            var physicalPath = Server.MapPath(path);

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }
        }

        public virtual bool AuthorizeImage(string path)
        {
            return CanAccess(path) && IsValidFile(Path.GetExtension(path));
        }

        

        #region imagemethods
        //public virtual JsonResult Read(string path)
        //{

        //    path = NormalizePath(path);

        //    if (AuthorizeRead(path))
        //    {
        //        try
        //        {

        //            directoryBrowser.Server = Server;
                    
        //            var result = directoryBrowser.GetContent(path, DefaultFilter)
        //                .Select(f => new
        //                {
        //                    name = f.Name,
        //                    type = f.Type == EntryType.File ? "f" : "d",
        //                    size = f.Size
        //                });

        //            return Json(result, JsonRequestBehavior.AllowGet);
        //        }
        //        catch (DirectoryNotFoundException)
        //        {
        //            throw new HttpException(404, "File Not Found");
        //        }
        //    }

        //    throw new HttpException(403, "Forbidden");
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public virtual ActionResult Upload(string path, HttpPostedFileBase file)
        //{
        //    path = NormalizePath(path);
        //    var fileName = Path.GetFileName(file.FileName);

        //    if (AuthorizeUpload(path, file))
        //    {
        //        file.SaveAs(Path.Combine(Server.MapPath(path), fileName));

        //        return Json(new
        //        {
        //            size = file.ContentLength,
        //            name = fileName,
        //            type = "f"
        //        }, "text/plain");
        //    }

        //    throw new HttpException(403, "Forbidden");
        //}

        //[OutputCache(Duration = 3600, VaryByParam = "path")]
        //public virtual ActionResult Thumbnail(string path)
        //{
        //    path = NormalizePath(path);

        //    if (AuthorizeThumbnail(path))
        //    {
        //        var physicalPath = Server.MapPath(path);

        //        if (System.IO.File.Exists(physicalPath))
        //        {
        //            Response.AddFileDependency(physicalPath);

        //            return CreateThumbnail(physicalPath);
        //        }
        //        else
        //        {
        //            throw new HttpException(404, "File Not Found");
        //        }
        //    }
        //    else
        //    {
        //        throw new HttpException(403, "Forbidden");
        //    }
        //}

        //private FileContentResult CreateThumbnail(string physicalPath)
        //{
        //    using (var fileStream = System.IO.File.OpenRead(physicalPath))
        //    {
        //        var desiredSize = new ImageSize
        //        {
        //            Width = ThumbnailWidth,
        //            Height = ThumbnailHeight
        //        };

        //        const string contentType = "image/png";

        //        return File(new ThumbnailCreator().Create(fileStream, desiredSize, contentType), contentType);
        //    }
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public virtual ActionResult Destroy(string path, string name, string type)
        //{
        //    path = NormalizePath(path);

        //    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(type))
        //    {
        //        path = CombinePaths(path, name);
        //        if (type.ToLowerInvariant() == "f")
        //        {
        //            DeleteFile(path);
        //        }
                

        //        return Json(new object[0]);
        //    }
        //    throw new HttpException(404, "File Not Found");
        //}

        //[OutputCache(Duration = 360, VaryByParam = "path")]
        //public ActionResult Image(string path)
        //{
        //    path = NormalizePath(path);

        //    if (AuthorizeImage(path))
        //    {
        //        var physicalPath = Server.MapPath(path);

        //        if (System.IO.File.Exists(physicalPath))
        //        {
        //            const string contentType = "image/png";
        //            return File(System.IO.File.OpenRead(physicalPath), contentType);
        //        }
        //    }

        //    throw new HttpException(403, "Forbidden");
        //}

        #endregion
        #endregion
    }

    



    
}

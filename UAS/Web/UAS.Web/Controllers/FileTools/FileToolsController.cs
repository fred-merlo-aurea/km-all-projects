using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Core_AMS.Utilities;
using System.Dynamic;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Collections.ObjectModel;
using KM.Common.Import;
using CommonEnums = KM.Common.Enums;

namespace UAS.Web.Controllers.FileTools
{
    public class FileToolsController : Common.BaseController
    {
        // GET: FileTools
        public ActionResult Index()
        {
            return View();
        }

        #region FileUpload
        public ActionResult FileUpload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase FileUpload)
        {
            HttpPostedFileBase file = FileUpload;
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string path = Server.MapPath("~/App_Data") + "\\" + CurrentClientID;

                    #region Create Path
                    try
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.IsError = true;
                        ViewBag.Message = "Error moving the file. Please contact customer support.";
                    }
                    #endregion
                    #region Save File to Local
                    using (System.IO.FileStream output = new System.IO.FileStream(path + "\\" + file.FileName, FileMode.Create))
                    {
                        file.InputStream.CopyTo(output);
                    }
                    #endregion
                    #region Upload to FTP
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(path + "\\" + file.FileName);
                    List<FrameworkUAS.Entity.ClientFTP> clientFTP = new List<FrameworkUAS.Entity.ClientFTP>();
                    FrameworkUAS.BusinessLogic.ClientFTP clientFTPWorker = new FrameworkUAS.BusinessLogic.ClientFTP();
                    clientFTP = clientFTPWorker.SelectClient(CurrentClientID);
                    if (clientFTP.Where(x => x.IsActive == true && x.IsDeleted == false).ToList().Count > 1)
                    {
                        ViewBag.IsError = true;
                        ViewBag.Message = "Client has more than one active FTP settings. Please contact customer service to have this fixed before proceeding.";
                    }
                    else
                    {
                        FrameworkUAS.Entity.ClientFTP cFTP = clientFTP.FirstOrDefault();
                        if (cFTP != null)
                        {
                            string host = "";
                            host = cFTP.Server + "/ADMS/";

                            Core_AMS.Utilities.FtpFunctions ftp = new Core_AMS.Utilities.FtpFunctions(host, cFTP.UserName, cFTP.Password);

                            bool uploadSuccess = false;

                            //uploadSuccess = ftp.Upload(fileInfo.Name, fileInfo.FullName);

                            if (uploadSuccess == true)
                            {
                                ViewBag.IsError = false;
                                ViewBag.Message = "Your file has been imported. View the Dashboard page for file progress updates and import confirmation.";
                            }
                            else
                            {
                                ViewBag.IsError = true;
                                ViewBag.Message = "An error has occurred uploading your file.";
                            }
                        }
                        else
                        {
                            ViewBag.IsError = true;
                            ViewBag.Message = "FTP site is not configured for the selected client.  Please contact Customer Support.";
                        }
                    }
                    #endregion
                    #region Delete Local File
                    System.IO.File.Delete(path + "\\" + file.FileName);
                    #endregion
                }
                catch (Exception ex)
                {
                    ViewBag.IsError = true;
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            }
            else
            {
                ViewBag.IsError = true;
                ViewBag.Message = "You have not specified a file.";
            }
            return PartialView("_FileUploadResults");
        }
        #endregion

        #region FileValidator
        public ActionResult FileValidator()
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.FV, KMPlatform.Enums.Access.FullAccess) || KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FILEVALUAD, KMPlatform.Enums.Access.FullAccess))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        [HttpPost]
        public ActionResult FileValidator(HttpPostedFileBase FileUpload, FormCollection collection)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.FV, KMPlatform.Enums.Access.FullAccess) || KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FILEVALUAD, KMPlatform.Enums.Access.FullAccess))
            {
                var processType = collection["ProcessType"];
                HttpPostedFileBase file = FileUpload;
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        string path = Server.MapPath("~/App_Data") + "\\" + CurrentClientID;

                        #region Create Path
                        try
                        {
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                        }
                        catch (Exception ex)
                        {
                            ViewBag.IsError = true;
                            ViewBag.Message = "Error moving the file. Please contact customer support.";
                        }
                        #endregion
                        #region Save File to Local
                        using (System.IO.FileStream output = new System.IO.FileStream(path + "\\" + file.FileName, FileMode.Create))
                        {
                            file.InputStream.CopyTo(output);
                        }
                        #endregion
                        #region Validate File      
                        #region SourceFile check
                        var fileName = Path.GetFileName(file.FileName);

                        FileInfo fileInfo = new FileInfo(path + "\\" + fileName);
                        FrameworkUAS.BusinessLogic.SourceFile sourceFileWorker = new FrameworkUAS.BusinessLogic.SourceFile();

                        List<FrameworkUAS.Entity.SourceFile> sourceFilesList = new List<FrameworkUAS.Entity.SourceFile>();
                        sourceFilesList = sourceFileWorker.Select(CurrentClientID, false).Where(x => x.IsDeleted == false).ToList();

                        FrameworkUAS.Entity.SourceFile sf = null;
                        string incomingFile = fileInfo.Name.Replace(fileInfo.Extension, "").ToLower();
                        if (sourceFilesList.Exists(x => incomingFile.StartsWith(x.FileName.ToLower())))
                        {
                            if (sourceFilesList.Exists(x => incomingFile.Equals(x.FileName, StringComparison.CurrentCultureIgnoreCase)))
                                sf = sourceFilesList.FirstOrDefault(x => incomingFile.Equals(x.FileName, StringComparison.CurrentCultureIgnoreCase));
                            else
                                sf = sourceFilesList.FirstOrDefault(x => incomingFile.StartsWith(x.FileName.ToLower()));
                        }
                        #endregion
                        if (sf != null && sf.SourceFileID > 0)
                        {
                            #region Process File
                            if (processType.ToString().Equals("Local", StringComparison.CurrentCultureIgnoreCase))
                            {
                                #region Local Processing
                                ViewBag.IsError = false;
                                ViewBag.Message = "Test";
                                #endregion
                            }
                            else
                            {
                                #region Offline Processing
                                List<FrameworkUAS.Entity.ClientFTP> clientFTP = new List<FrameworkUAS.Entity.ClientFTP>();
                                FrameworkUAS.BusinessLogic.ClientFTP clientFTPWorker = new FrameworkUAS.BusinessLogic.ClientFTP();
                                clientFTP = clientFTPWorker.SelectClient(CurrentClientID);
                                if (clientFTP.Where(x => x.IsActive == true && x.IsDeleted == false).ToList().Count > 1)
                                {
                                    ViewBag.IsError = true;
                                    ViewBag.Message = "Client has more than one active FTP settings. Please contact customer service to have this fixed before proceeding.";
                                }
                                else
                                {
                                    FrameworkUAS.Entity.ClientFTP cFTP = clientFTP.FirstOrDefault();
                                    if (cFTP != null)
                                    {
                                        string host = "";
                                        host = cFTP.Server + "/ADMS/FileValidator";

                                        Core_AMS.Utilities.FtpFunctions ftp = new Core_AMS.Utilities.FtpFunctions(host, cFTP.UserName, cFTP.Password);

                                        bool uploadSuccess = false;

                                        //uploadSuccess = ftp.Upload(fileInfo.Name, fileInfo.FullName);

                                        if (uploadSuccess == true)
                                        {
                                            ViewBag.IsError = false;
                                            ViewBag.Message = "Your file has been imported. View the Dashboard page for file progress updates and import confirmation.";
                                        }
                                        else
                                        {
                                            ViewBag.IsError = true;
                                            ViewBag.Message = "An error has occurred uploading your file.";
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.IsError = true;
                                        ViewBag.Message = "FTP site is not configured for the selected client.  Please contact Customer Support.";
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            ViewBag.IsError = true;
                            ViewBag.Message = "The selected file is not setup as a valid Source File for current client";
                        }
                        #endregion
                        #region Delete Local File
                        System.IO.File.Delete(path + "\\" + file.FileName);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ViewBag.IsError = true;
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                }
                else
                {
                    ViewBag.IsError = true;
                    ViewBag.Message = "You have not specified a file.";
                }
                return PartialView("_FileValidatorResults");
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        #endregion

        #region FileViewer
        public ActionResult FileViewer_Read([DataSourceRequest] DataSourceRequest request)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.FV, KMPlatform.Enums.Access.FullAccess) || KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FILEVALUAD, KMPlatform.Enums.Access.FullAccess))
            {
                List<dynamic> sessionData = Session["FileViewerModel"] as List<dynamic>;

                DataSourceResult dsres = sessionData.ToDataSourceResult(request);

                var serializer = new JavaScriptSerializer();
                serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });
                var json = serializer.Serialize(dsres);

                return new MyJsonResult(json);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        public ActionResult FileViewer()
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.FV, KMPlatform.Enums.Access.FullAccess) || KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FILEVALUAD, KMPlatform.Enums.Access.FullAccess))
            {
                Models.FileTools.FileViewer fv = new Models.FileTools.FileViewer();
                fv.Data = new List<dynamic>();
                fv.Bind = new Dictionary<string, Type>();
                fv.ColumnDelimiter = new List<string>();
                fv.TrueFalseOptions = new List<string>();
                foreach (var dl in (CommonEnums.ColumnDelimiter[]) Enum.GetValues(typeof(CommonEnums.ColumnDelimiter)))
                {
                    fv.ColumnDelimiter.Add(dl.ToString().Replace("_", " "));
                }
                fv.TrueFalseOptions.Add("True");
                fv.TrueFalseOptions.Add("False");
                return View(fv);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        [HttpPost]
        public ActionResult FileViewer(HttpPostedFileBase FileUpload, FormCollection collection)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.FV, KMPlatform.Enums.Access.FullAccess) || KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FILEVALUAD, KMPlatform.Enums.Access.FullAccess))
            {
                DataTable dt = new DataTable();
                var dynamicDt = new List<dynamic>();
                var columnDelimiter = collection["ColumnDelimiter"];
                var isQuoteEncapsulated = collection["IsQuoteEncapsulated"];
                string path = Server.MapPath("~/App_Data") + "\\" + CurrentClientID;
                HttpPostedFileBase file = FileUpload;

                #region Create Path
                try
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.IsError = true;
                    ViewBag.Message = "Error moving the file. Please contact customer support.";
                }
                #endregion

                try
                {
                    var fileName = Path.GetFileName(file.FileName);

                    #region Save File to Local
                    using (System.IO.FileStream output = new System.IO.FileStream(path + "\\" + fileName, FileMode.Create))
                    {
                        file.InputStream.CopyTo(output);
                    }
                    #endregion
                    #region Setup to View File                
                    FileInfo importFile = new FileInfo(path + "\\" + fileName);
                    var fileConfig = new FileConfiguration();
                    if (System.IO.Path.GetExtension(path + "\\" + fileName).ToLower() == ".csv" || System.IO.Path.GetExtension(path + "\\" + fileName).ToLower() == ".txt")
                    {
                        fileConfig.FileColumnDelimiter = columnDelimiter.ToString();

                        bool isQuote = false;
                        bool.TryParse(isQuoteEncapsulated.ToString(), out isQuote);
                        fileConfig.IsQuoteEncapsulated = isQuote;
                    }
                    FrameworkUAD.BusinessLogic.ImportVessel iv = new FrameworkUAD.BusinessLogic.ImportVessel();
                    FrameworkUAD.Object.ImportVessel dataIV;
                    FileWorker fileWorker = new FileWorker();
                    int fileRowProcessedCount = 0;
                    int fileRowBatch = 1000;
                    int fileTotalRowCount = fileWorker.GetRowCount(importFile);
                    while (fileRowProcessedCount < fileTotalRowCount)
                    {
                        int endRow = fileRowProcessedCount + fileRowBatch;
                        if (endRow > fileTotalRowCount)
                            endRow = fileTotalRowCount;
                        int startRow = fileRowProcessedCount + 1;

                        dataIV = iv.GetImportVessel(importFile, startRow, fileRowBatch, fileConfig);
                        fileRowProcessedCount += dataIV.TotalRowCount;

                        dt = dataIV.DataOriginal;

                        #region Create List<dynamic>
                        foreach (DataRow row in dt.Rows)
                        {
                            dynamic dyna = new ExpandoObject();
                            foreach (DataColumn column in dt.Columns)
                            {
                                var dic = (IDictionary<string, object>) dyna;
                                dic[column.ColumnName] = row[column];
                            }
                            dynamicDt.Add(dyna);
                        }
                        #endregion

                        dataIV.DataOriginal.Dispose();
                    }
                    #endregion
                    #region Delete Local File
                    System.IO.File.Delete(path + "\\" + fileName);
                    #endregion
                }
                catch (Exception ex)
                {
                    ViewBag.IsError = true;
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }

                Models.FileTools.FileViewer fv = new Models.FileTools.FileViewer();
                fv.Data = dynamicDt;

                var dyn = new Dictionary<string, System.Type>();
                foreach (System.Data.DataColumn column in dt.Columns)
                {
                    var t = System.Type.GetType(column.DataType.FullName);
                    dyn.Add(column.ColumnName, t);
                }
                fv.Bind = dyn;

                Session["FileViewerModel"] = dynamicDt;

                return PartialView("_FileViewerResults", fv);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        #endregion

        private int CurrentClientID
        {
            //get { return 25; }
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID; }
        }
    }

    #region JSON Methods
    public class ExpandoJSONConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var result = new Dictionary<string, object>();
            var dictionary = obj as IDictionary<string, object>;
            foreach (var item in dictionary)
                result.Add(item.Key, item.Value);
            return result;
        }
        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return new ReadOnlyCollection<Type>(new Type[] { typeof(System.Dynamic.ExpandoObject) });
            }
        }
    }

    public class MyJsonResult : ActionResult
    {

        private string stringAsJson;

        public MyJsonResult(string stringAsJson)
        {
            this.stringAsJson = stringAsJson;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var httpCtx = context.HttpContext;
            httpCtx.Response.ContentType = "application/json";
            httpCtx.Response.Write(stringAsJson);
        }
    }
    #endregion
}
using FrameworkUAS.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for FileStatus.xaml
    /// </summary>
    public partial class FileStatus : UserControl
    {
        #region ServiceCalls
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IDQMQue> dqmData = FrameworkServices.ServiceClient.UAS_DQMQueClient();
        //private FrameworkServices.ServiceClient<UAS_WS.Interface.IFileLog> fileLogData = FrameworkServices.ServiceClient.UAS_FileLogClient();
        //private FrameworkServices.ServiceClient<UAS_WS.Interface.IFileStatus> fileStatusData = FrameworkServices.ServiceClient.UAS_FileStatusClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICodeType> codeTypeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeTypeClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.ISourceFile> sourceFileData = FrameworkServices.ServiceClient.UAS_SourceFileClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userData = FrameworkServices.ServiceClient.UAS_UserClient();
        #endregion

        #region ServiceResponses
        private FrameworkUAS.Service.Response<List<DQMQue>> dqmResponse = new FrameworkUAS.Service.Response<List<DQMQue>>();
        //private FrameworkUAS.Service.Response<List<FileLog>> fileLogResponse = new FrameworkUAS.Service.Response<List<FileLog>>();
        //private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FileStatus>> fileStatusResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FileStatus>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>> codeTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>>();
        private FrameworkUAS.Service.Response<List<SourceFile>> sourceFileResponse = new FrameworkUAS.Service.Response<List<SourceFile>>();
        private FrameworkUAS.Service.Response<List<KMPlatform.Entity.User>> userResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.User>>();
        #endregion

        private ObservableCollection<FileStatusDetail> details = new ObservableCollection<FileStatusDetail>();
        private List<KMPlatform.Entity.User> users = new List<KMPlatform.Entity.User>();
        private List<FrameworkUAD_Lookup.Entity.Code> statuses = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> fileTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
        private FrameworkUAD.Entity.Issue myIssue = new FrameworkUAD.Entity.Issue();
        private int processedID = 0;
        FrameworkUAS.Object.AppData myAppData;
        Guid accessKey;

        public class FileStatusDetail : INotifyPropertyChanged
        {
            public string FileName { get; set; }
            public DateTime Time { get; set; }
            public string Status { get; set; }
            public string FileType { get; set; }
            public string User { get; set; }

            public FileStatusDetail(string fileName, DateTime time, string status, string fileType, string user)
            {
                this.FileName = fileName;
                this.Time = time;
                this.Status = status;
                this.FileType = fileType;
                this.User = user;
            }
            public FileStatusDetail() {}

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public FileStatus(FrameworkUAD.Entity.Issue issue, FrameworkUAD.Entity.Product pub)
        {
            InitializeComponent();

            myIssue = issue;
            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            txtIssue.Text = issue.IssueName;
            //List<FrameworkUAS.Entity.SourceFile> sfList = myAppData.AuthorizedUser.User.CurrentClient.SourceFilesList;
            txtProduct.Text = pub.PubCode;

            List<SourceFile> filesTemp = new List<SourceFile>();
            List<DQMQue> dqm = new List<DQMQue>();
            //List<FrameworkUAS.Entity.FileStatus> fileStatuses = new List<FrameworkUAS.Entity.FileStatus>();
            //List<FileLog> logs = new List<FileLog>();
            HashSet<int> sourceFileIDs = new HashSet<int>();

            busy.IsBusy = true;
            busy.IsIndeterminate = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                #region Get Files & Info

                userResponse = userData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(userResponse.Result, userResponse.Status))
                    users = userResponse.Result;

                if (Home.Codes.Count > 0)
                {
                    int id = Home.CodeTypes.Where(x => x.CodeTypeName == FrameworkUAD_Lookup.Enums.CodeType.File_Status.ToString().Replace("_", " ")).FirstOrDefault().CodeTypeId;
                    statuses = Home.Codes.Where(x => x.CodeTypeId == id).ToList();
                    //processedID = statuses.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FileStatusTypes.Processed.ToString()).Select(x => x.CodeId).SingleOrDefault();
                }
                else
                {
                    codeResponse = codeData.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.File_Status); //12
                    if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
                    {
                        statuses = codeResponse.Result;
                        //processedID = statuses.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FileStatusTypes.Processed.ToString()).Select(x => x.CodeId).SingleOrDefault();
                    }
                }

                if (Home.Codes.Count > 0)
                {
                    int id = Home.CodeTypes.Where(x => x.CodeTypeName == FrameworkUAD_Lookup.Enums.CodeType.Database_File.ToString().Replace("_", " ")).FirstOrDefault().CodeTypeId;
                    fileTypes = Home.Codes.Where(x => x.CodeTypeId == id).ToList();
                }
                else
                {
                    codeResponse = codeData.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Database_File); //7
                    if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
                        fileTypes = codeResponse.Result;
                }

                sourceFileResponse = sourceFileData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID, false);
                if (Helpers.Common.CheckResponse(sourceFileResponse.Result, sourceFileResponse.Status))
                {
                    filesTemp = sourceFileResponse.Result;
                    filesTemp = filesTemp.Where(x => x.PublicationID == myIssue.PublicationId && x.IsDeleted == false && x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID).ToList();
                    sourceFileIDs = new HashSet<int>(filesTemp.Select(x => x.SourceFileID));
                }

                //fileStatusResponse = fileStatusData.Proxy.SelectForClient(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
                //if (Helpers.Common.CheckResponse(fileStatusResponse.Result, fileStatusResponse.Status))
                //    fileStatuses = fileStatusResponse.Result.Where(x => sourceFileIDs.Contains(x.SourceFileID) && x.DateCreated >= myIssue.DateCreated || x.DateUpdated >= myIssue.DateCreated).ToList();

                dqmResponse = dqmData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID, true, true);
                if (Helpers.Common.CheckResponse(dqmResponse.Result, dqmResponse.Status))
                    dqm = dqmResponse.Result.Where(x=> sourceFileIDs.Contains(x.SourceFileId)).OrderByDescending(x => x.DateCreated).ToList();

                dqmResponse = dqmData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID, false, true);
                if (Helpers.Common.CheckResponse(dqmResponse.Result, dqmResponse.Status))
                    dqm.AddRange(dqmResponse.Result.Where(x => sourceFileIDs.Contains(x.SourceFileId)).OrderByDescending(x => x.DateCreated).ToList());

                #endregion

                #region Parse Files

                foreach (SourceFile sf in filesTemp)
                {
                    FileStatusDetail fsd = new FileStatusDetail();
                    fsd.FileName = sf.FileName;
                    fsd.FileType = fileTypes.Where(x => x.CodeId == sf.DatabaseFileTypeId).Select(x => x.DisplayName).FirstOrDefault();

                    //FrameworkUAS.Entity.FileStatus fs = fileStatuses.Where(x => x.SourceFileID == sf.SourceFileID).SingleOrDefault();
                    DQMQue d = new DQMQue();
                    //if (fs != null)
                    //{
                    //    if (fs.DateUpdated == null)
                    //    {
                    //        fsd.User = users.Where(x => x.UserID == fs.CreatedByUserID).Select(x => x.UserName).FirstOrDefault();
                    //        fsd.Time = fs.DateCreated;
                    //    }
                    //    else
                    //    {
                    //        fsd.User = users.Where(x => x.UserID == fs.UpdatedByUserID).Select(x => x.UserName).FirstOrDefault();
                    //        fsd.Time = fs.DateUpdated.Value;
                    //    }

                    //    if (fs.FileStatusTypeID == processedID)
                    //    {
                    //        d = dqm.Where(x => x.SourceFileId == sf.SourceFileID).FirstOrDefault();
                    //        if (d == null || d.IsCompleted)
                    //            fsd.Status = "Processed";
                    //        else
                    //            fsd.Status = "Running DQM";
                    //    }
                    //    else
                    //    {
                    //        fsd.Status = statuses.Where(x => x.CodeId == fs.FileStatusTypeID).Select(x => x.DisplayName).FirstOrDefault();
                    //        if (fsd.Status == null || fsd.Status == "")
                    //            fsd.Status = "Invalid";
                    //    }
                    //}
                    
                    details.Add(fsd);
                }

                #endregion
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                busy.IsBusy = false;
                details = new ObservableCollection<FileStatusDetail>(details.OrderByDescending(i => i.Time));
                icFiles.ItemsSource = details;
            };
            bw.RunWorkerAsync();
        }

        private void Refresh_Files(object sender, System.Windows.RoutedEventArgs e)
        {
            ObservableCollection<FileStatusDetail> temp = new ObservableCollection<FileStatusDetail>();
            busy.IsBusy = true;
            busy.IsIndeterminate = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                #region Get Files & Info

                List<SourceFile> filesTemp = new List<SourceFile>();
                List<DQMQue> dqm = new List<DQMQue>();
                //List<FrameworkUAS.Entity.FileStatus> fileStatuses = new List<FrameworkUAS.Entity.FileStatus>();
                //List<FileLog> logs = new List<FileLog>();
                HashSet<int> sourceFileIDs = new HashSet<int>();

                sourceFileResponse = sourceFileData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID, false);
                if (Helpers.Common.CheckResponse(sourceFileResponse.Result, sourceFileResponse.Status))
                {
                    filesTemp = sourceFileResponse.Result;
                    filesTemp = filesTemp.Where(x => x.PublicationID == myIssue.PublicationId && x.IsDeleted == false && x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID).ToList();
                    sourceFileIDs = new HashSet<int>(filesTemp.Select(x => x.SourceFileID));
                }

                //fileStatusResponse = fileStatusData.Proxy.SelectForClient(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
                //if (Helpers.Common.CheckResponse(fileStatusResponse.Result, fileStatusResponse.Status))
                //    fileStatuses = fileStatusResponse.Result.Where(x => sourceFileIDs.Contains(x.SourceFileID) && x.DateCreated >= myIssue.DateCreated || x.DateUpdated >= myIssue.DateCreated).ToList();

                dqmResponse = dqmData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID, true, true);
                if (Helpers.Common.CheckResponse(dqmResponse.Result, dqmResponse.Status))
                    dqm = dqmResponse.Result.Where(x => sourceFileIDs.Contains(x.SourceFileId)).OrderByDescending(x => x.DateCreated).ToList();

                dqmResponse = dqmData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID, false, true);
                if (Helpers.Common.CheckResponse(dqmResponse.Result, dqmResponse.Status))
                    dqm.AddRange(dqmResponse.Result.Where(x => sourceFileIDs.Contains(x.SourceFileId)).OrderByDescending(x => x.DateCreated).ToList());

                #endregion

                #region Parse Files

                foreach (SourceFile sf in filesTemp)
                {
                    FileStatusDetail fsd = new FileStatusDetail();
                    fsd.FileName = sf.FileName;
                    fsd.FileType = fileTypes.Where(x => x.CodeId == sf.DatabaseFileTypeId).Select(x => x.DisplayName).FirstOrDefault();

                    //FrameworkUAS.Entity.FileStatus fs = fileStatuses.Where(x => x.SourceFileID == sf.SourceFileID).SingleOrDefault();
                    DQMQue d = new DQMQue();
                    //if (fs != null)
                    //{
                    //    if (fs.DateUpdated == null)
                    //    {
                    //        fsd.User = users.Where(x => x.UserID == fs.CreatedByUserID).Select(x => x.UserName).FirstOrDefault();
                    //        fsd.Time = fs.DateCreated;
                    //    }
                    //    else
                    //    {
                    //        fsd.User = users.Where(x => x.UserID == fs.UpdatedByUserID).Select(x => x.UserName).FirstOrDefault();
                    //        fsd.Time = fs.DateUpdated.Value;
                    //    }

                    //    if (fs.FileStatusTypeID == processedID)
                    //    {
                    //        d = dqm.Where(x => x.SourceFileId == sf.SourceFileID).FirstOrDefault();
                    //        if (d == null || d.IsCompleted)
                    //            fsd.Status = "Processed";
                    //        else
                    //            fsd.Status = "Running DQM";
                    //    }
                    //    else
                    //    {
                    //        fsd.Status = statuses.Where(x => x.CodeId == fs.FileStatusTypeID).Select(x => x.DisplayName).FirstOrDefault();
                    //        if (fsd.Status == null || fsd.Status == "")
                    //            fsd.Status = "Invalid";
                    //    }

                    //    temp.Add(fsd);
                    //}
                    
                }

                #endregion
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                busy.IsBusy = false;
                details = new ObservableCollection<FileStatusDetail>(temp.OrderByDescending(i => i.Time));
                icFiles.ItemsSource = details;
            };
            bw.RunWorkerAsync();
        }
    }
}

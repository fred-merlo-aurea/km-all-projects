using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for Rules.xaml
    /// </summary>
    public partial class Rules : UserControl
    {
        #region VARIABLES
        FileMapperWizard.Modules.FMUniversal thisContainer;

        //List<FrameworkUAS.Entity.FileRule> AllRules = new List<FrameworkUAS.Entity.FileRule>();
        //List<FrameworkUAS.Entity.FileRuleMap> AllRulesMap = new List<FrameworkUAS.Entity.FileRuleMap>();

        //FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FileRule>> svRules = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FileRule>>();
        //FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FileRuleMap>> svRulesMap = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FileRuleMap>>();

        FrameworkUAS.Service.Response<int> svSaved = new FrameworkUAS.Service.Response<int>();
        FrameworkUAS.Service.Response<bool> svDelete = new FrameworkUAS.Service.Response<bool>();
        #endregion

        public Rules(FileMapperWizard.Modules.FMUniversal container)
        {
            thisContainer = container;
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            Telerik.Windows.Controls.RadBusyIndicator busy = Core_AMS.Utilities.WPF.FindControl<Telerik.Windows.Controls.RadBusyIndicator>(thisContainer, "busyIcon");
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                //svRules = rWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
                //svRulesMap = rmWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Rules
                //if (svRules.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                //{
                //    AllRules = svRules.Result;
                //    foreach (FrameworkUAS.Entity.FileRule rule in AllRules)
                //    {
                //        lbxAvailable.Items.Add(rule.DisplayName);
                //    }
                //}
                //else
                //{
                //    Core_AMS.Utilities.WPF.MessageServiceError();
                //}
                #endregion
                #region RulesMap
                //if (svRulesMap.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                //{
                //    AllRulesMap = svRulesMap.Result;
                //    foreach (FrameworkUAS.Entity.FileRuleMap rule in AllRulesMap.Where(x => x.SourceFileID == thisContainer.sourceFileID))
                //    {
                //        FrameworkUAS.Entity.FileRule curRule = AllRules.FirstOrDefault(x => x.FileRuleID == rule.RulesID);
                //        if (curRule != null)
                //        {
                //            lbxSelected.Items.Add(curRule.DisplayName);
                //            if (lbxAvailable.Items.Contains(curRule.DisplayName))                            
                //                lbxAvailable.Items.Remove(curRule.DisplayName);
                            
                //        }

                //    }
                //}
                //else
                //{
                //    Core_AMS.Utilities.WPF.MessageServiceError();
                //}
                #endregion

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();            
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (lbxAvailable.SelectedItems.Count > 0)
            {
                List<string> selectedItems = new List<string>();
                foreach (string s in lbxAvailable.SelectedItems)
                {
                    lbxSelected.Items.Add(s);
                    selectedItems.Add(s);
                }
                foreach (string s in selectedItems)
                {
                    lbxAvailable.Items.Remove(s);
                }
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lbxSelected.SelectedItems.Count > 0)
            {
                List<string> selectedItems = new List<string>();
                foreach (string s in lbxSelected.SelectedItems)
                {
                    lbxAvailable.Items.Add(s);
                    selectedItems.Add(s);
                }
                foreach (string s in selectedItems)
                {
                    lbxSelected.Items.Remove(s);
                }    
                lbxAvailable.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            }
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            if (lbxSelected.SelectedItems.Count > 0)
            {
                for (int i = 0; i < lbxSelected.Items.Count; i++)
                {
                    if (lbxSelected.SelectedItems.Contains(lbxSelected.Items[i]))
                    {
                        if (i > 0 && !lbxSelected.SelectedItems.Contains(lbxSelected.Items[i - 1]))
                        {
                            var item = lbxSelected.Items[i];
                            lbxSelected.Items.Remove(item);
                            lbxSelected.Items.Insert(i - 1, item);
                        }
                    }
                }
            }
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if (lbxSelected.SelectedItems.Count > 0)
            {
                int startindex = lbxSelected.Items.Count - 1;

                for (int i = startindex; i > -1; i--)
                {
                    if (lbxSelected.SelectedItems.Contains(lbxSelected.Items[i]))
                    {
                        if (i < startindex && !lbxSelected.SelectedItems.Contains(lbxSelected.Items[i + 1]))
                        {
                            var item = lbxSelected.Items[i];
                            lbxSelected.Items.Remove(item);
                            lbxSelected.Items.Insert(i + 1, item);
                        }
                    }
                }
            }
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            //Delete old by sourcefile
            //svDelete = rmWorker.Proxy.Delete(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.sourceFileID);

            //Save
            int order = 1;
            List<string> failedSaves = new List<string>();
            foreach (string item in lbxSelected.Items)
            {
                //FrameworkUAS.Entity.FileRule rule = AllRules.FirstOrDefault(x => x.DisplayName.Equals(item, StringComparison.CurrentCultureIgnoreCase));
                //if (rule != null)
                //{
                //    FrameworkUAS.Entity.FileRuleMap rm = new FrameworkUAS.Entity.FileRuleMap
                //    {
                //        RulesAssignedID = 0,
                //        RulesID = rule.FileRuleID,
                //        SourceFileID = thisContainer.sourceFileID,
                //        RuleOrder = order,
                //        IsActive = true,
                //        DateCreated = DateTime.Now,
                //        DateUpdated = DateTime.Now,
                //        CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                //        UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                //    };
                //    svSaved = rmWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, rm);
                //    order++;
                //    if (svSaved.Status != FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                //    {
                //        failedSaves.Add(rule.DisplayName);
                //    }
                //}
            }

            if (failedSaves.Count > 0)
            {
                Core_AMS.Utilities.WPF.Message("Save was not complete. If the issue persists please contact customer support.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Incomplete");
                return;
            }
            else
            {
                //If Save complete close
                Core_AMS.Utilities.WPF.MessageSaveComplete();
                Windows.FMWindow win = this.ParentOfType<Windows.FMWindow>();

                if (thisContainer.isCirculation && !string.IsNullOrEmpty(thisContainer.filePath))
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to import this file now?", "Warning", MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        FileInfo fInfo = new FileInfo(thisContainer.filePath);
                        if (Core_AMS.Utilities.FileFunctions.IsFileLocked(fInfo))
                        {
                            Core_AMS.Utilities.WPF.Message("File is currently locked. This could be because the file is opened. Please close and import again.", MessageBoxButton.OK, MessageBoxImage.Warning, "File Exception");
                            return;
                        }
                        else
                        {
                            #region Upload to FTP
                            System.IO.FileInfo file = new System.IO.FileInfo(thisContainer.filePath);
                            KMPlatform.Entity.Client client = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
                            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[client.ClientID].ClientFtpDirectoriesList.Where(x => x.IsActive == true).ToList().Count > 1)
                            {
                                Core_AMS.Utilities.WPF.Message("Client has more than one active FTP settings. Please contact customer service to have this fixed before proceeding.", MessageBoxButton.OK, MessageBoxImage.Warning, "File Exception");
                                return;
                            }

                            FrameworkUAS.Entity.ClientFTP cFTP = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[client.ClientID].ClientFtpDirectoriesList.FirstOrDefault();
                            if (cFTP != null)
                            {
                                string host = "";
                                host = cFTP.Server + "/ADMS/";

                                Core_AMS.Utilities.FtpFunctions ftp = new Core_AMS.Utilities.FtpFunctions(host, cFTP.UserName, cFTP.Password);

                                bool uploadSuccess = false;
                                Telerik.Windows.Controls.RadBusyIndicator busy = Core_AMS.Utilities.WPF.FindControl<Telerik.Windows.Controls.RadBusyIndicator>(thisContainer, "busyIcon");
                                busy.IsBusy = true;
                                BackgroundWorker worker = new BackgroundWorker();
                                worker.DoWork += (o, ea) =>
                                {
                                    uploadSuccess = ftp.Upload(file.Name, file.FullName);
                                };

                                worker.RunWorkerCompleted += (o, ea) =>
                                {
                                    if (uploadSuccess == true)
                                    {
                                        Core_AMS.Utilities.WPF.Message("Your file has been imported. View the Import Status page for file progress updates and import confirmation.", MessageBoxButton.OK, MessageBoxImage.Information, "File Uploaded");

                                        //Close old window with import but cannot close main window
                                        var oo = Core_AMS.Utilities.WPF.GetWindow(this);
                                        if (oo.GetType().ToString().Replace(".", "_") != Core_AMS.Utilities.Enums.Windows.AMS_Desktop_Windows_Home.ToString())
                                        {
                                            oo.Close();
                                        }
                                    }
                                    else
                                    {
                                        Core_AMS.Utilities.WPF.MessageFileUploadError();
                                    }
                                    busy.IsBusy = false;
                                };

                                busy.IsBusy = true;
                                worker.RunWorkerAsync();

                            }
                            else
                                Core_AMS.Utilities.WPF.MessageError("FTP site is not configured for the selected client.  Please contact Customer Support.");

                            #endregion
                        }
                    }
                    else
                    {
                        //Close old window with import but cannot close main window
                        var oo = Core_AMS.Utilities.WPF.GetWindow(this);
                        if (oo.GetType().ToString().Replace(".", "_") != Core_AMS.Utilities.Enums.Windows.AMS_Desktop_Windows_Home.ToString())
                        {
                            oo.Close();
                        }
                    }
                }
                else
                {
                    if (win != null)
                        win.Close();
                    else
                    {                                
                        object pwin = Core_AMS.Utilities.WPF.GetWindow(this);                                
                        if (pwin != null)
                        {
                            WpfControls.WindowsAndDialogs.PopOut p = (WpfControls.WindowsAndDialogs.PopOut)pwin;
                            p.Close();
                        }
                    }
                }
               
            }
        }

        private void btnStep5Prev_Click(object sender, RoutedEventArgs e)
        {
            thisContainer.RulesToTransformations();
        }
    }
}

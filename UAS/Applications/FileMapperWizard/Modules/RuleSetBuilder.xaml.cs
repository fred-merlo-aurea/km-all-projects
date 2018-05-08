using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FrameworkUAD.Object;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Data.DataFilter;
using System.ComponentModel;

namespace FileMapperWizard.Modules
{
    /// <summary>
    /// Interaction logic for FpsBuilder.xaml
    /// </summary>
    public partial class RuleSetBuilder : UserControl
    {
        #region Services
        FrameworkServices.ServiceClient<UAD_WS.Interface.IFileMappingColumn> fmc = FrameworkServices.ServiceClient.UAD_FileMappingColumnClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> blc = FrameworkServices.ServiceClient.UAS_ClientClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ISourceFile> sfWorker = FrameworkServices.ServiceClient.UAS_SourceFileClient();
        #endregion

        #region Variables
        List<RadPanelSourceCollection> radPanelSourceCollections = new List<RadPanelSourceCollection>();
        public List<FileMappingColumn> _uadColumns;
        FMUniversal thisContainer;
        static int Clicks = 0;
        List<FrameworkUAS.Entity.SourceFile> SourceFilesList = new List<FrameworkUAS.Entity.SourceFile>();
        #endregion

        public RuleSetBuilder(FMUniversal container)
        {
            InitializeComponent();
            thisContainer = container;
            //LoadData();
        }
        public RuleSetBuilder(int sourceFileId, FMUniversal container)
        {
            InitializeComponent();
            thisContainer = container;
            //LoadData();
            //FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey
        }
        public RuleSetBuilder(Guid accessKey, KMPlatform.Entity.Client client, FMUniversal container)
        {
            InitializeComponent();
            thisContainer = container;
            //LoadData();
            _uadColumns = fmc.Proxy.Select(accessKey, client.ClientConnections).Result;

            foreach (var uadColumn in _uadColumns)
            {
                ItemPropertyDefinition propertyDefinition = new ItemPropertyDefinition(uadColumn.ColumnName.ToString(), typeof(string));
                //				radDataFilter.ItemPropertyDefinitions.Add(propertyDefinition);
            }
        }
        //void LoadData()
        //{
        //    btnFinishStep.Visibility = System.Windows.Visibility.Collapsed;
        //    //btnDCOptStep.Visibility = System.Windows.Visibility.Collapsed;


        //    if (thisContainer.isCirculation)
        //    {
        //        btnFinishStep.Visibility = System.Windows.Visibility.Visible;
        //        //btnDCOptStep.Visibility = System.Windows.Visibility.Collapsed;
        //    }
        //    else if (this.thisContainer.isCirculation == false && this.thisContainer.myUADFeature == KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
        //    {
        //        //btnDCOptStep.Visibility = System.Windows.Visibility.Visible;
        //        btnFinishStep.Visibility = System.Windows.Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        btnFinishStep.Visibility = System.Windows.Visibility.Visible;
        //        //btnDCOptStep.Visibility = System.Windows.Visibility.Collapsed;
        //    }
        //}

        private void RadDataFilter_OnEditorCreated(object sender, EditorCreatedEventArgs e)
        {
        }

        private void btnAddCondition_OnClick(object sender, RoutedEventArgs e)
        {
            gridRadPanels.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            _uadColumns = fmc.Proxy.Select(thisContainer.myClient.AccessKey, thisContainer.myClient.ClientConnections).Result;
            RadDataFilter radDataFilter = new RadDataFilter();
            foreach (var uadColumn in _uadColumns)
            {
                ItemPropertyDefinition propertyDefinition = new ItemPropertyDefinition(uadColumn.ColumnName.ToString(), typeof(string));
                radDataFilter.ItemPropertyDefinitions.Add(propertyDefinition);
            }

            RadPanelBar radPanelBar = new RadPanelBar();
            radPanelBar.ExpandMode = ExpandMode.Single;
            radPanelBar.HorizontalAlignment = HorizontalAlignment.Left;
            radPanelBar.VerticalAlignment = VerticalAlignment.Top;
            radPanelBar.Margin = new Thickness(10, 0, 10, 0);
            radPanelBar.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);

            Button configButton = new Button();
            configButton.Content = this.TryFindResource("configurationImage");
            configButton.Width = 100;
            configButton.Height = 30;
            configButton.Click += configButton_Click;
            configButton.Margin = new Thickness(10, 0, 10, 0);

            Button copyText = new Button();
            copyText.Content = "Copy Text";
            copyText.Name = "copy" + Clicks;
            copyText.Width = 100;
            copyText.Height = 30;
            copyText.Click += copyText_Click;


            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Children.Add(radDataFilter);
            stackPanel.Children.Add(configButton);
            stackPanel.Children.Add(copyText);

            Button button = new Button();
            button.Content = "Choose Rule Set";
            button.Name = "button" + Clicks;
            button.Width = 100;
            button.Height = 30;
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.Margin = new Thickness(10, 0, 10, 0);
            button.Click += button_Click;

            Grid.SetRow(radPanelBar, Clicks);
            Grid.SetColumn(radPanelBar, 0);
            gridRadPanels.Children.Add(radPanelBar);

            Grid.SetRow(button, Clicks);
            Grid.SetColumn(button, 1);

            var headerValue = Clicks + 1;
            RadPanelBarItem item = new RadPanelBarItem
            {
                Header = "Condition" + headerValue
            };

            item.Items.Add(stackPanel);
            radPanelBar.Items.Add(item);
            gridRadPanels.Children.Add(button);

            Clicks = Clicks + 1;
        }

        void copyText_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var count = Convert.ToInt32(button.Name.GetLast(1));
            var radDataFilter = Core_AMS.Utilities.WPF.FindVisualChildren<RadDataFilter>(gridRadPanels).ToArray()[count];
            Clipboard.SetData(DataFormats.Text, radDataFilter.FilterDescriptors);
        }

        void configButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.FpsSelectSource selectSource = new Windows.FpsSelectSource();
            selectSource.Width = 400;
            selectSource.Height = 400;
            selectSource.ShowDialog();
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var count = Convert.ToInt32(button.Name.GetLast(1));
            var radDataFilter = Core_AMS.Utilities.WPF.FindVisualChildren<RadDataFilter>(gridRadPanels).ToArray()[count];
            Windows.FpsSelectApplyRules chooseApplyRules = new Windows.FpsSelectApplyRules(radDataFilter.FilterDescriptors,thisContainer.myClient);
            chooseApplyRules.Width = 800;
            chooseApplyRules.Height = 800;
            chooseApplyRules.ShowDialog();
        }

        #region Buttons
        private void btnFileReview_Click(object sender, RoutedEventArgs e)
        {
            //thisContainer.TransformationsToReview();
            thisContainer.RulesToReview();

            //Find bordercontainer set the child.
            var borderList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.Border>(thisContainer);
            if (borderList.FirstOrDefault(x => x.Name.Equals("StepReview", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                Border thisBorder = borderList.FirstOrDefault(x => x.Name.Equals("StepReview", StringComparison.CurrentCultureIgnoreCase));
                thisBorder.Child = new FileMapperWizard.Controls.Review(thisContainer);
            }
        }
        /// <summary>
        /// skips the review module
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            RadButton me = sender as RadButton;
            if (me.Content.Equals("Finish"))
            {
                MessageBoxResult messBox = MessageBox.Show("Are you sure you have completed work on this file?", "Warning", MessageBoxButton.YesNo);

                if (messBox == MessageBoxResult.No)
                    return;

                #region UPLOAD CIRC FILE
                if (thisContainer.isCirculation && !string.IsNullOrEmpty(thisContainer.filePath))
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to import this file now?", "Warning", MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        System.IO.FileInfo fInfo = new System.IO.FileInfo(thisContainer.filePath);
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
                            List<FrameworkUAS.Entity.ClientFTP> clientftpDirectoriesList = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[client.ClientID].ClientFtpDirectoriesList;
                            if (clientftpDirectoriesList.Where(x => x.IsActive == true).ToList().Count > 1)
                            {
                                Core_AMS.Utilities.WPF.Message("Client has more than one active FTP settings. Please contact customer service to have this fixed before proceeding.", MessageBoxButton.OK, MessageBoxImage.Warning, "File Exception");
                                return;
                            }

                            FrameworkUAS.Entity.ClientFTP cFTP = clientftpDirectoriesList.FirstOrDefault();
                            if (cFTP != null)
                            {
                                string host = "";
                                host = cFTP.Server + "/ADMS/";

                                Core_AMS.Utilities.FtpFunctions ftp = new Core_AMS.Utilities.FtpFunctions(host, cFTP.UserName, cFTP.Password);

                                SourceFilesList = sfWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.myClient.ClientID, false, false).Result.ToList();
                                FrameworkUAS.Entity.SourceFile sf = SourceFilesList.FirstOrDefault(x => thisContainer.fileInfo.Name.ToLower().StartsWith(x.FileName.ToLower()) && x.SourceFileID != thisContainer.sourceFileID);
                                if (sf == null)
                                {
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
                                            else
                                            {
                                                CloseWindow();
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
                                {
                                    Core_AMS.Utilities.WPF.Message("There is a file with a similar name, please wait for a engine refresh before dropping your file.", MessageBoxButton.OK, MessageBoxImage.Information, "Upload not available");
                                    var oo = Core_AMS.Utilities.WPF.GetWindow(this);
                                    if (oo.GetType().ToString().Replace(".", "_") != Core_AMS.Utilities.Enums.Windows.AMS_Desktop_Windows_Home.ToString())
                                    {
                                        oo.Close();
                                    }
                                    else
                                    {
                                        CloseWindow();
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        CloseWindow();
                    }
                }
                else
                {
                    CloseWindow();
                }
                #endregion
            }
        }
        private void CloseWindow()
        {
            Windows.FMWindow win = this.ParentOfType<Windows.FMWindow>();
            if (win != null)
                win.Close();
            else
            {
                object pwin = Core_AMS.Utilities.WPF.GetWindow(this);
                if (pwin != null)
                {
                    if (pwin.GetType() == typeof(WpfControls.WindowsAndDialogs.PopOut))
                    {
                        WpfControls.WindowsAndDialogs.PopOut p = (WpfControls.WindowsAndDialogs.PopOut) pwin;
                        p.Close();
                    }
                }
            }
        }
        //private void btnDCOptStep_Click(object sender, RoutedEventArgs e)
        //{
        //    GoToDataCompareStep();
        //}
        //private void GoToDataCompareStep()
        //{
        //    thisContainer.TransformationsToDataCompareOptions();
        //    //need new window
        //    foreach (Window window in Application.Current.Windows.OfType<Windows.FMWindow>())
        //        ((Windows.FMWindow) window).Close();

        //    Windows.FMWindow newMappingWindow = new Windows.FMWindow("Data Compare");
        //    StackPanel stackPanel = new StackPanel();
        //    stackPanel.Children.Clear();
        //    FileMapperWizard.Modules.DataCompareSteps dc = new FileMapperWizard.Modules.DataCompareSteps(thisContainer);
        //    stackPanel.Children.Add(dc);
        //    newMappingWindow.spContent.Children.Add(stackPanel);
        //    newMappingWindow.Title = "Data Compare";
        //    newMappingWindow.SizeToContent = SizeToContent.WidthAndHeight;
        //    newMappingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //    newMappingWindow.ShowDialog();
        //    newMappingWindow.Activate();
        //}
        private void btnPrevStep_Click(object sender, RoutedEventArgs e)
        {
            //thisContainer.ReviewToTransformations();
            thisContainer.RulesToTransformations();
        }
        #endregion

        private void rbDefault_Checked(object sender, RoutedEventArgs e)
        {
            if (rbDefault.IsChecked == true)
            {
                if (rbCustom != null)
                {
                    rbCustom.IsChecked = false;
                    btnAddCondition.Visibility = Visibility.Hidden;
                }
            }
        }

        private void rbCustom_Checked(object sender, RoutedEventArgs e)
        {
            if (rbCustom.IsChecked == true)
            {
                btnAddCondition.Visibility = Visibility.Visible;
                if (rbDefault != null)
                    rbDefault.IsChecked = false;
            }
        }
    }

    public class RadPanelSourceCollection
    {
        public string Text { get; set; }

        public RadDataFilter RadDataFilter { get; set; }

    }

    public static class StringExtension
    {
        public static string GetLast(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }
    }
}

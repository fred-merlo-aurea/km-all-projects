using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using FileMapperWizard.Helpers;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for Transformations.xaml
    /// </summary>
    public partial class Transformations : UserControl
    {
        private readonly ServiceClientSet serviceClientSet = new ServiceClientSet();

        #region VARIABLES
        FileMapperWizard.Modules.FMUniversal thisContainer;
        FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> svCodes = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        List<FrameworkUAD_Lookup.Entity.Code> codes = new List<FrameworkUAD_Lookup.Entity.Code>();
        #endregion

        public Transformations(FileMapperWizard.Modules.FMUniversal container)
        {
            thisContainer = container;
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            //POPULATE THE YES NO FOR APPLY TRANSFORMATION STEP 4
            //rcbApplyTransformation.Items.Add("Yes");
            //rcbApplyTransformation.Items.Add("No");
            //POPULATE THE NEW EXISTING FOR APPLY EXISTS OR NEW STEP 4
            rcbNewExist.Items.Add("New");
            rcbNewExist.Items.Add("Existing");

            //POPULATE THE TRANSFORMATIONS
            //foreach (FrameworkUAD_Lookup.Enums.TransformationTypes transType in (FrameworkUAD_Lookup.Enums.TransformationTypes[])Enum.GetValues(typeof(FrameworkUAD_Lookup.Enums.TransformationTypes)))
            //{
            //    rcbTransformationType.Items.Add(transType.ToString().Replace('_', ' '));
            //}
            #region CODE
            svCodes = serviceClientSet.LookUpCode.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Transformation);
            codes = svCodes.Result;
            rcbTransformationType.ItemsSource = codes;
            rcbTransformationType.DisplayMemberPath = "DisplayName";
            rcbTransformationType.SelectedValuePath = "CodeName";
            #endregion


            ////POPULATE MATCH TYPE FOR DATA MAPPING TRANSFORMATION
            //foreach (FrameworkUAS.BusinessLogic.Enums.MatchTypes matchType in (FrameworkUAS.BusinessLogic.Enums.MatchTypes[])Enum.GetValues(typeof(FrameworkUAS.BusinessLogic.Enums.MatchTypes)))
            //{
            //    //rcbMatchType.Items.Add(matchType.ToString().Replace('_', ' '));
            //}
            //FrameworkServices.ServiceClient<UAS_WS.Interface.IFileRule> rWorker = FrameworkServices.ServiceClient.UAS_FileRuleClient();
            //FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FileRule>> svRules = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FileRule>>();
            //svRules = rWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);

            #region Show Step 5 - Import Rules - always show

            
            #endregion
        }

        #region Comboboxes
        private void rcbApplyTransformation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (rcbApplyTransformation.SelectedValue != null)
            //{
            //    if (rcbApplyTransformation.SelectedValue.ToString().Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        #region Yes Answered
            //        //SELECT FIELD MAPPING SOURCE COLUMNS WHO AREN'T IGNORED                    
            //        List<FrameworkUAS.Entity.FieldMapping> thisFM = fmData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.sourceFileID).Result.Where(x => x.FieldMappingTypeID != thisContainer.ignoreTypeID).ToList();
            //        rcbColumnSelection.ItemsSource = null;
            //        rcbColumnSelection.Items.Clear();
            //        rcbColumnSelection.ItemsSource = thisFM;
            //        rcbColumnSelection.DisplayMemberPath = "MAFField";
            //        rcbColumnSelection.SelectedValuePath = "FieldMappingID";

            //        txtSelectCol.Visibility = Visibility.Visible;
            //        rcbColumnSelection.Visibility = Visibility.Visible;
            //        #endregion
            //    }

            //    //comment this out 1/16/17 WAGNER - last step is now RuleSet selection
            //    #region old NO code to finalize mapping
            //    //else
            //    //{
            //    //    #region No Answered
            //    //    if (!thisContainer.isCirculation)
            //    //    {
            //    //        MessageBoxResult messBox = MessageBox.Show("Are you sure you are have completed work on this file?", "Warning", MessageBoxButton.YesNo);

            //    //        if (messBox == MessageBoxResult.No)
            //    //        {
            //    //            rcbApplyTransformation.SelectedValue = string.Empty;
            //    //            return;
            //    //        }

            //    //        if (thisContainer.myUADFeature == KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
            //    //            GoToImportRules();
            //    //        else
            //    //            CloseWindow();
            //    //    }
            //    //    else
            //    //    {
            //    //        //FrameworkServices.ServiceClient<UAS_WS.Interface.IFileRule> rWorker = FrameworkServices.ServiceClient.UAS_FileRuleClient();
            //    //        //FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FileRule>> svRules = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FileRule>>();
            //    //        //svRules = rWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);

            //    //        //if (svRules.Result.Count > 0)
            //    //        //    btnCircRulesStep_Click(new object(), new RoutedEventArgs());
            //    //        //else
            //    //        //{
            //    //        MessageBoxResult messBox = MessageBox.Show("Are you sure you are have completed work on this file?", "Warning", MessageBoxButton.YesNo);

            //    //        if (messBox == MessageBoxResult.No)
            //    //        {
            //    //            rcbApplyTransformation.SelectedValue = string.Empty;
            //    //            return;
            //    //        }

            //    //        if (thisContainer.isCirculation && !string.IsNullOrEmpty(thisContainer.filePath))
            //    //        {
            //    //            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to import this file now?", "Warning", MessageBoxButton.YesNo);

            //    //            if (messageBoxResult == MessageBoxResult.Yes)
            //    //            {
            //    //                System.IO.FileInfo fInfo = new System.IO.FileInfo(thisContainer.filePath);
            //    //                if (Core_AMS.Utilities.FileFunctions.IsFileLocked(fInfo))
            //    //                {
            //    //                    Core_AMS.Utilities.WPF.Message("File is currently locked. This could be because the file is opened. Please close and import again.", MessageBoxButton.OK, MessageBoxImage.Warning, "File Exception");
            //    //                    return;
            //    //                }
            //    //                else
            //    //                {
            //    //                    #region Upload to FTP
            //    //                    System.IO.FileInfo file = new System.IO.FileInfo(thisContainer.filePath);
            //    //                    KMPlatform.Entity.Client client = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
            //    //                    List<FrameworkUAS.Entity.ClientFTP> clientftpDirectoriesList = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[client.ClientID].ClientFtpDirectoriesList;
            //    //                    if (clientftpDirectoriesList.Where(x => x.IsActive == true).ToList().Count > 1)
            //    //                    {
            //    //                        Core_AMS.Utilities.WPF.Message("Client has more than one active FTP settings. Please contact customer service to have this fixed before proceeding.", MessageBoxButton.OK, MessageBoxImage.Warning, "File Exception");
            //    //                        return;
            //    //                    }

            //    //                    FrameworkUAS.Entity.ClientFTP cFTP = clientftpDirectoriesList.FirstOrDefault();
            //    //                    if (cFTP != null)
            //    //                    {
            //    //                        string host = "";
            //    //                        host = cFTP.Server + "/ADMS/";

            //    //                        Core_AMS.Utilities.FtpFunctions ftp = new Core_AMS.Utilities.FtpFunctions(host, cFTP.UserName, cFTP.Password);

            //    //                        bool uploadSuccess = false;
            //    //                        Telerik.Windows.Controls.RadBusyIndicator busy = Core_AMS.Utilities.WPF.FindControl<Telerik.Windows.Controls.RadBusyIndicator>(thisContainer, "busyIcon");
            //    //                        busy.IsBusy = true;
            //    //                        BackgroundWorker worker = new BackgroundWorker();
            //    //                        worker.DoWork += (o, ea) =>
            //    //                        {
            //    //                            uploadSuccess = ftp.Upload(file.Name, file.FullName);
            //    //                        };

            //    //                        worker.RunWorkerCompleted += (o, ea) =>
            //    //                        {
            //    //                            if (uploadSuccess == true)
            //    //                            {
            //    //                                Core_AMS.Utilities.WPF.Message("Your file has been imported. View the Import Status page for file progress updates and import confirmation.", MessageBoxButton.OK, MessageBoxImage.Information, "File Uploaded");

            //    //                                    //Close old window with import but cannot close main window
            //    //                                    var oo = Core_AMS.Utilities.WPF.GetWindow(this);
            //    //                                if (oo.GetType().ToString().Replace(".", "_") != Core_AMS.Utilities.Enums.Windows.AMS_Desktop_Windows_Home.ToString())
            //    //                                {
            //    //                                    oo.Close();
            //    //                                }
            //    //                                else
            //    //                                {
            //    //                                    CloseWindow();
            //    //                                }
            //    //                            }
            //    //                            else
            //    //                            {
            //    //                                Core_AMS.Utilities.WPF.MessageFileUploadError();
            //    //                            }
            //    //                            busy.IsBusy = false;
            //    //                        };

            //    //                        busy.IsBusy = true;
            //    //                        worker.RunWorkerAsync();

            //    //                    }
            //    //                    else
            //    //                        Core_AMS.Utilities.WPF.MessageError("FTP site is not configured for the selected client.  Please contact Customer Support.");

            //    //                    #endregion
            //    //                }
            //    //            }
            //    //            else
            //    //            {
            //    //                CloseWindow();
            //    //            }
            //    //        }
            //    //        else
            //    //        {
            //    //            CloseWindow();
            //    //        }
            //    //        //}
            //    //    }
            //    //    #endregion
            //    //}
            //    #endregion
            //}
        }

        private void rcbColumnSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rcbColumnSelection.SelectedValue != null)
            {
                int fmID = 0;
                int.TryParse(rcbColumnSelection.SelectedValue.ToString(), out fmID);
                if (fmID > 0)
                {
                    txtApplied.Text = "";
                    txtApplied.Text = "'" + rcbColumnSelection.Text.ToString() + "' Transformations:";

                    List<FrameworkUAS.Entity.Transformation> assignMappings = serviceClientSet.Transformations.Proxy.SelectAssigned(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, fmID).Result;
                    ListBoxClientAddedMappings.ItemsSource = null;
                    ListBoxClientAddedMappings.Items.Clear();
                    ListBoxClientAddedMappings.ItemsSource = assignMappings;
                    //ListBoxClientAddedMappings.DisplayMemberPath = "TransformationName";
                    //ListBoxClientAddedMappings.SelectedValuePath = "TransformationID";

                    if (assignMappings.Count > 0)
                    {
                        txtApplied.Visibility = Visibility.Visible;
                        ListBoxClientAddedMappings.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        txtApplied.Visibility = Visibility.Collapsed;
                        ListBoxClientAddedMappings.Visibility = Visibility.Collapsed;
                    }

                    txtApplyExistNew.Visibility = Visibility.Visible;
                    rcbNewExist.Visibility = Visibility.Visible;
                    spTransformations.Children.Clear();
                }
            }
        }

        private void rcbNewExist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rcbNewExist.SelectedValue != null)
            {
                if (rcbNewExist.SelectedValue.ToString().Equals("new", StringComparison.CurrentCultureIgnoreCase))
                {
                    spTransformations.Children.Clear();
                    thisContainer.myTransformationID = 0;
                    //spNewSelect.Visibility = Visibility.Visible;
                    txtSelectTrans.Visibility = Visibility.Visible;
                    rcbTransformationType.Visibility = Visibility.Visible;
                    spExist.Visibility = Visibility.Collapsed;
                }
                else if (rcbNewExist.SelectedValue.ToString().Equals("existing", StringComparison.CurrentCultureIgnoreCase))
                {
                    spTransformations.Children.Clear();
                    ListBoxDataMapping.ItemsSource = null;
                    ListBoxDataMapping.Items.Clear();
                    List<FrameworkUAS.Entity.Transformation> t = serviceClientSet.Transformations.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.myClient.ClientID, false).Result;
                    ListBoxDataMapping.ItemsSource = t.OrderBy(x => x.TransformationName);
                    ListBoxDataMapping.SelectedValuePath = "TransformationID";
                    ListBoxDataMapping.DisplayMemberPath = "TransformationName";

                    //spNewSelect.Visibility = Visibility.Collapsed;
                    txtSelectTrans.Visibility = Visibility.Collapsed;
                    rcbTransformationType.Visibility = Visibility.Collapsed;
                    spExist.Visibility = Visibility.Visible;
                }
            }
        }

        private void rcbTransformationType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rcbTransformationType.SelectedValue != null)
            {
                if (rcbTransformationType.SelectedValue.ToString().Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Assign_Value.ToString().Replace('_', ' '), StringComparison.CurrentCultureIgnoreCase))
                {
                    FrameworkUAS.Entity.FieldMapping fm = (FrameworkUAS.Entity.FieldMapping) rcbColumnSelection.SelectedItem;
                    spTransformations.Children.Clear();
                    spTransformations.Visibility = Visibility.Visible;
                    spTransformations.Children.Add(new FileMapperWizard.Controls.AssignTransformation(FrameworkUAS.Object.AppData.myAppData, thisContainer, thisContainer.sourceFileID, thisContainer.myClient, spTransformations, (ListBoxDataMapping.SelectedValue == null ? 0 : Convert.ToInt32(ListBoxDataMapping.SelectedValue.ToString())), fm, false));
                    spTransformations.LayoutUpdated += new EventHandler(layoutChanged);
                }
                else if (rcbTransformationType.SelectedValue.ToString().Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Data_Mapping.ToString().Replace('_', ' '), StringComparison.CurrentCultureIgnoreCase))
                {
                    spTransformations.Children.Clear();
                    spTransformations.Visibility = Visibility.Visible;
                    spTransformations.Children.Add(new FileMapperWizard.Controls.DataMapTransformation(FrameworkUAS.Object.AppData.myAppData, thisContainer, thisContainer.sourceFileID, thisContainer.myClient, spTransformations, (ListBoxDataMapping.SelectedValue == null ? "" : ListBoxDataMapping.SelectedValue.ToString()), rcbColumnSelection.SelectedValue.ToString(), false));
                    spTransformations.LayoutUpdated += new EventHandler(layoutChanged);
                }
                else if (rcbTransformationType.SelectedValue.ToString().Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Join_Columns.ToString().Replace('_', ' '), StringComparison.CurrentCultureIgnoreCase))
                {
                    spTransformations.Children.Clear();
                    spTransformations.Visibility = Visibility.Visible;
                    spTransformations.Children.Add(new FileMapperWizard.Controls.JoinTransformation(FrameworkUAS.Object.AppData.myAppData, thisContainer, thisContainer.sourceFileID, thisContainer.myClient, spTransformations, (ListBoxDataMapping.SelectedValue == null ? "" : ListBoxDataMapping.SelectedValue.ToString()), rcbColumnSelection.SelectedValue.ToString(), false));
                    spTransformations.LayoutUpdated += new EventHandler(layoutChanged);
                }
                else if (rcbTransformationType.SelectedValue.ToString().Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Into_Rows.ToString().Replace('_', ' '), StringComparison.CurrentCultureIgnoreCase))
                {
                    spTransformations.Children.Clear();
                    spTransformations.Visibility = Visibility.Visible;
                    spTransformations.Children.Add(new FileMapperWizard.Controls.SplitIntoRowTransformation(FrameworkUAS.Object.AppData.myAppData, thisContainer, thisContainer.sourceFileID, thisContainer.myClient, spTransformations, (ListBoxDataMapping.SelectedValue == null ? "" : ListBoxDataMapping.SelectedValue.ToString()), rcbColumnSelection.SelectedValue.ToString(), false));
                    spTransformations.LayoutUpdated += new EventHandler(layoutChanged);
                }
                else if (rcbTransformationType.SelectedValue.ToString().Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Transform.ToString().Replace('_', ' '), StringComparison.CurrentCultureIgnoreCase))
                {
                    spTransformations.Children.Clear();
                    spTransformations.Visibility = Visibility.Visible;
                    spTransformations.Children.Add(new FileMapperWizard.Controls.SplitTransformation(FrameworkUAS.Object.AppData.myAppData, thisContainer, thisContainer.sourceFileID, thisContainer.myClient, spTransformations, (ListBoxDataMapping.SelectedValue == null ? "" : ListBoxDataMapping.SelectedValue.ToString()), rcbColumnSelection, false));
                    spTransformations.LayoutUpdated += new EventHandler(layoutChanged);
                }
            }
        }
        #endregion

        #region Buttons
        private void btnDeleteTrans_Click(object sender, RoutedEventArgs e)
        {
            RadButton btnMe = sender as RadButton;
            Grid grdMe = btnMe.Parent as Grid;
            TextBlock txtMe = grdMe.FindName("txtInfo") as TextBlock;
            ListBoxItem item = grdMe.GetVisualParent<ListBoxItem>();
            int fmID = 0;
            int tID = 0;
            int.TryParse(btnMe.Tag.ToString(), out tID);
            int.TryParse(rcbColumnSelection.SelectedValue.ToString(), out fmID);
            if (fmID > 0)
            {
                //List<FrameworkUAS.Entity.TransformationFieldMap> tfm = tfmData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, tID).Result;
                //MessageBoxResult messBox = MessageBox.Show("Are you sure you want to delete this Transformation? Number of file(s) affected by deleting this transformation: " + tfm.Distinct().Count(), "Warning", MessageBoxButton.YesNo);
                MessageBoxResult messBox = MessageBox.Show("Are you sure you want to remove this Transformation?", "Warning", MessageBoxButton.YesNo);

                if (messBox == MessageBoxResult.No)
                    return;

                FrameworkUAS.Entity.Transformation t = new FrameworkUAS.Entity.Transformation();
                t = ListBoxClientAddedMappings.SelectedItem as FrameworkUAS.Entity.Transformation;
                int del = serviceClientSet.TransformationFieldMapData.Proxy.DeleteTransformationFieldMapping(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, txtMe.Text, thisContainer.myClient.ClientID, fmID).Result;
                //int del = blt.Proxy.Delete(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, fmID).Result;

                List<FrameworkUAS.Entity.Transformation> assignMappings = serviceClientSet.Transformations.Proxy.SelectAssigned(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, fmID).Result;
                ListBoxClientAddedMappings.ItemsSource = null;
                ListBoxClientAddedMappings.Items.Clear();
                ListBoxClientAddedMappings.ItemsSource = assignMappings;
                ListBoxClientAddedMappings.UpdateLayout();
                ListBoxClientAddedMappings.Items.Refresh();

                if (assignMappings.Count == 0)
                {
                    if (thisContainer.fieldMappingsWithTransformations.Contains(fmID))
                        thisContainer.fieldMappingsWithTransformations.Remove(fmID);

                }

                Core_AMS.Utilities.WPF.Message("Transformation removed.", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, "Transformation Removed.");
            }
        }

        private void btnStep4Prev_Click(object sender, RoutedEventArgs e)
        {
            thisContainer.TransformationsToNewColumns();
        }

        private void btnImportRules_Click(object sender, RoutedEventArgs e)
        {
            GoToImportRules();
        }
        void GoToImportRules()
        {
            #region Setup Next Tile
            thisContainer.TransformationsToRules();
            //Find bordercontainer set the child.
            var borderList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.Border>(thisContainer);
            if (borderList.FirstOrDefault(x => x.Name.Equals("StepFiveContainer", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                Border thisBorder = borderList.FirstOrDefault(x => x.Name.Equals("StepFiveContainer", StringComparison.CurrentCultureIgnoreCase));
                thisBorder.Child = new Modules.RuleSetBuilder(thisContainer); 
            }
            #endregion
        }
        #endregion

        private void txtTransformSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchString = txtTransformSearch.Text;
            if (searchString != string.Empty)
            {
                int index = -1;
                // Find the item in the list and store the index to the item. 
                foreach (FrameworkUAS.Entity.Transformation s in ListBoxDataMapping.Items)
                {
                    if (s.TransformationName.ToLower().StartsWith(searchString.ToLower()))
                    {
                        index = ListBoxDataMapping.Items.IndexOf(s);
                        ListBoxDataMapping.SelectedIndex = index;
                        ListBoxDataMapping.UpdateLayout();
                        ScrollViewer sv = ListBoxDataMapping.Template.FindName("svListBox", ListBoxDataMapping) as ScrollViewer;
                        sv.ScrollToVerticalOffset(index);
                        break;
                    }
                }
                if (index < 0)
                {
                    ListBoxDataMapping.SelectedIndex = -1;
                }
            }
        }

        private void ListBoxDataMapping_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int tID = 0;
            if (ListBoxDataMapping.SelectedValue != null)
            {
                int.TryParse(ListBoxDataMapping.SelectedValue.ToString(), out tID);
                if (tID > 0)
                {
                    thisContainer.myTransformationID = tID;
                    List<FrameworkUAS.Entity.Transformation> allTrans = serviceClientSet.Transformations.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                    FrameworkUAS.Entity.Transformation thisTrans = allTrans.SingleOrDefault(x => x.TransformationID == tID);
                    if (thisTrans != null)
                    {
                        if (thisTrans.TransformationTypeID == codes.FirstOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.TransformationTypes.Data_Mapping.ToString().Replace('_', ' ')).CodeId)
                        {
                            spTransformations.Visibility = Visibility.Visible;
                            spTransformations.Children.Clear();
                            FileMapperWizard.Controls.DataMapTransformation child = new FileMapperWizard.Controls.DataMapTransformation(FrameworkUAS.Object.AppData.myAppData, thisContainer, thisContainer.sourceFileID, thisContainer.myClient, spTransformations, (ListBoxDataMapping.SelectedValue == null ? "" : ListBoxDataMapping.SelectedValue.ToString()), rcbColumnSelection.SelectedValue.ToString(), true);
                            spTransformations.Children.Add(child);
                            spTransformations.LayoutUpdated += new EventHandler(layoutChanged);
                        }
                        else if (thisTrans.TransformationTypeID == codes.FirstOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.TransformationTypes.Join_Columns.ToString().Replace('_', ' ')).CodeId)
                        {
                            spTransformations.Visibility = Visibility.Visible;
                            spTransformations.Children.Clear();
                            FileMapperWizard.Controls.JoinTransformation child = new FileMapperWizard.Controls.JoinTransformation(FrameworkUAS.Object.AppData.myAppData, thisContainer, thisContainer.sourceFileID, thisContainer.myClient, spTransformations, ListBoxDataMapping.SelectedValue.ToString(), rcbColumnSelection.SelectedValue.ToString(), true);
                            spTransformations.Children.Add(child);
                            spTransformations.LayoutUpdated += new EventHandler(layoutChanged);
                        }
                        else if (thisTrans.TransformationTypeID == codes.FirstOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Into_Rows.ToString().Replace('_', ' ')).CodeId)
                        {
                            spTransformations.Visibility = Visibility.Visible;
                            spTransformations.Children.Clear();
                            FileMapperWizard.Controls.SplitIntoRowTransformation child = new FileMapperWizard.Controls.SplitIntoRowTransformation(FrameworkUAS.Object.AppData.myAppData, thisContainer, thisContainer.sourceFileID, thisContainer.myClient, spTransformations, (ListBoxDataMapping.SelectedValue == null ? "" : ListBoxDataMapping.SelectedValue.ToString()), rcbColumnSelection.SelectedValue.ToString(), true);
                            spTransformations.Children.Add(child);
                            spTransformations.LayoutUpdated += new EventHandler(layoutChanged);
                        }
                        else if (thisTrans.TransformationTypeID == codes.FirstOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.TransformationTypes.Assign_Value.ToString().Replace('_', ' ')).CodeId)
                        {
                            FrameworkUAS.Entity.FieldMapping fm = (FrameworkUAS.Entity.FieldMapping) rcbColumnSelection.SelectedItem;
                            spTransformations.Visibility = Visibility.Visible;
                            spTransformations.Children.Clear();
                            FileMapperWizard.Controls.AssignTransformation child = new FileMapperWizard.Controls.AssignTransformation(FrameworkUAS.Object.AppData.myAppData, thisContainer, thisContainer.sourceFileID, thisContainer.myClient, spTransformations, (ListBoxDataMapping.SelectedValue == null ? 0 : Convert.ToInt32(ListBoxDataMapping.SelectedValue.ToString())), fm, true);
                            spTransformations.Children.Add(child);
                            spTransformations.LayoutUpdated += new EventHandler(layoutChanged);
                        }
                        else if (thisTrans.TransformationTypeID == codes.FirstOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Transform.ToString().Replace('_', ' ')).CodeId)
                        {
                            spTransformations.Visibility = Visibility.Visible;
                            spTransformations.Children.Clear();
                            FileMapperWizard.Controls.SplitTransformation child = new FileMapperWizard.Controls.SplitTransformation(FrameworkUAS.Object.AppData.myAppData, thisContainer, thisContainer.sourceFileID, thisContainer.myClient, spTransformations, (ListBoxDataMapping.SelectedValue == null ? "" : ListBoxDataMapping.SelectedValue.ToString()), rcbColumnSelection, true);
                            spTransformations.Children.Add(child);
                            spTransformations.LayoutUpdated += new EventHandler(layoutChanged);
                        }
                    }
                }
            }
        }

        private void layoutChanged(object sender, System.EventArgs e)
        {
            if (spTransformations.Children.Count == 0)
            {
                rbNo.IsChecked = true;
                //rcbApplyTransformation.SelectedItem = null;
                txtSelectCol.Visibility = Visibility.Collapsed;
                rcbColumnSelection.Visibility = Visibility.Collapsed;
                rcbColumnSelection.SelectedItem = null;
                txtApplied.Visibility = Visibility.Collapsed;
                ListBoxClientAddedMappings.Visibility = Visibility.Collapsed;
                txtApplyExistNew.Visibility = Visibility.Collapsed;
                rcbNewExist.Visibility = Visibility.Collapsed;
                rcbNewExist.SelectedItem = null;
                spExist.Visibility = Visibility.Collapsed;
                txtSelectTrans.Visibility = Visibility.Collapsed;
                rcbTransformationType.Visibility = Visibility.Collapsed;
                rcbTransformationType.SelectedItem = null;
                spTransformations.LayoutUpdated -= new EventHandler(layoutChanged);
            }
        }

        private void CloseWindow()
        {
            //thisContainer.TransformationsToReview();
            GoToImportRules();

            //Windows.FMWindow win = this.ParentOfType<Windows.FMWindow>();
            //if (win != null)
            //    win.Close();
            //else
            //{
            //    object pwin = Core_AMS.Utilities.WPF.GetWindow(this);
            //    if (pwin != null)
            //    {
            //        if (pwin.GetType() == typeof(WpfControls.WindowsAndDialogs.PopOut))
            //        {
            //            WpfControls.WindowsAndDialogs.PopOut p = (WpfControls.WindowsAndDialogs.PopOut)pwin;
            //            p.Close();
            //        }                    
            //    }
            //}
        }

        private void rbNo_Checked(object sender, RoutedEventArgs e)
        {
            if (rbNo.IsChecked == true)
            {
                if(rbYes != null)
                    rbYes.IsChecked = false;
            }
        }

        private void rbYes_Checked(object sender, RoutedEventArgs e)
        {
            if (rbYes.IsChecked == true)
            {
                if (rbNo != null)
                    rbNo.IsChecked = false;
                #region Yes Answered
                //SELECT FIELD MAPPING SOURCE COLUMNS WHO AREN'T IGNORED                    
                List<FrameworkUAS.Entity.FieldMapping> thisFM = serviceClientSet.FieldMapping.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.sourceFileID).Result.Where(x => x.FieldMappingTypeID != thisContainer.ignoreTypeID).ToList();
                rcbColumnSelection.ItemsSource = null;
                rcbColumnSelection.Items.Clear();
                rcbColumnSelection.ItemsSource = thisFM;
                rcbColumnSelection.DisplayMemberPath = "MAFField";
                rcbColumnSelection.SelectedValuePath = "FieldMappingID";

                txtSelectCol.Visibility = Visibility.Visible;
                rcbColumnSelection.Visibility = Visibility.Visible;
                #endregion
            }
        }
    }
}

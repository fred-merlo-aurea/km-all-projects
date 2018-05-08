using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;
using FileMapperWizard.Helpers;
using FrameworkUAD.Object;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for NewColumns.xaml
    /// </summary>
    public partial class NewColumns : UserControl
    {
        private readonly ServiceClientSet serviceClientSet = new ServiceClientSet();

        #region VARIABLES
        public static FrameworkUAS.Object.AppData myAppData { get; set; }
        List<string> addedColumnsUsed = new List<string>();
        FileMapperWizard.Modules.FMUniversal thisContainer;
        bool firstLoad;
        public List<FrameworkUAD_Lookup.Entity.Code> DemoUpdateOptions = new List<FrameworkUAD_Lookup.Entity.Code>();
        public List<string> adhocBitFields = new List<string>();
        public List<string> isRequiredFields = new List<string>();
        #endregion

        #region PAGE ENUMS
        public enum StepThreeActions
        {
            Add_Column_Not_Defined_In_File,
            Add_Additional_Column_Mapping
        }
        #endregion

        public NewColumns(FileMapperWizard.Modules.FMUniversal container)
        {
            thisContainer = container;
            InitializeComponent();
            LoadData();
            firstLoad = true;
        }

        public void LoadData()
        {
            //POPULATE STEP 3 ACTION LIST
            rcbStep3Actions.Items.Clear();
            foreach (StepThreeActions action in (StepThreeActions[])Enum.GetValues(typeof(StepThreeActions)))
            {
                rcbStep3Actions.Items.Add(action.ToString().Replace('_', ' '));
            }

            List<FrameworkUAD_Lookup.Entity.Code> demoUpdateTypes = serviceClientSet.LookUpCode.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update).Result;
            if (demoUpdateTypes != null)
                DemoUpdateOptions = demoUpdateTypes;
            else
                DemoUpdateOptions = new List<FrameworkUAD_Lookup.Entity.Code>();

            List<FrameworkUAD.Entity.SubscriptionsExtensionMapper> semList = new List<FrameworkUAD.Entity.SubscriptionsExtensionMapper>();
            semList = serviceClientSet.SubscriptionsExtensionMapperWorker.Proxy.SelectAll(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.myClient.ClientConnections).Result;
            List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> psemList = new List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper>();
            psemList = serviceClientSet.ProductSubscriptionsExtensionWorker.Proxy.SelectAll(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.myClient.ClientConnections).Result;

            adhocBitFields.AddRange(semList.Where(x => x.CustomFieldDataType == "bit").Select(x => x.CustomField).ToList());
            adhocBitFields.AddRange(psemList.Where(x => x.CustomFieldDataType == "bit").Select(x => x.CustomField).ToList());

            List<FrameworkUAD.Entity.ResponseGroup> rgList = new List<FrameworkUAD.Entity.ResponseGroup>();
            rgList = serviceClientSet.ResponseGroupWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.myClient.ClientConnections).Result;
            int pubID = 0;
            if (thisContainer.selectedProduct != null)
                int.TryParse(thisContainer.selectedProduct.PubID.ToString(), out pubID);

            isRequiredFields = rgList.Where(x => x.IsRequired == true && x.PubID == pubID).Select(x => x.ResponseGroupName).ToList();
        }

        #region Comboboxes
        private void rcbAddColumns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {                  
            if (rcbAddColumns.SelectedValue != null)
            {
                if (rlbAddedColumns.Items.Count > 0)
                {
                    if (addedColumnsUsed.Contains(rcbAddColumns.SelectedValue.ToString()))
                    {
                        Core_AMS.Utilities.WPF.Message("Value already added.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Selection");
                        return;
                    }
                }
                RadListBoxItem lItem = new RadListBoxItem();
                DataTemplate me = this.FindResource("roundedContent") as DataTemplate;
                Style round = this.FindResource("RoundedRadListBoxItem") as Style;
                lItem.Style = round;
                lItem.ContentTemplate = me;
                lItem.Tag = "addedColumn";
                rlbAddedColumns.Items.Add(lItem);
                lItem.UpdateLayout();
                ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(lItem);
                if (myContentPresenter != null)
                {
                    TextBlock me2 = (TextBlock)me.FindName("txtMe", myContentPresenter) as TextBlock;
                    me2.Text = rcbAddColumns.SelectedValue.ToString();
                    addedColumnsUsed.Add(rcbAddColumns.SelectedValue.ToString());                    
                }
            }
        }

        private void rcbStep3Actions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            #region FIRSTLOAD
            if (firstLoad)
            {
                firstLoad = false;
                if (thisContainer.isEdit)
                {                
                    if (thisContainer.currentFieldMappings != null)
                    {
                        foreach (FrameworkUAS.Entity.FieldMapping fm in thisContainer.currentFieldMappings.Where(x => x.IsNonFileColumn == true || x.HasMultiMapping == true))
                        {
                            #region USER ADDED COLUMNS LOAD
                            if (fm.IsNonFileColumn == true && fm.HasMultiMapping == false)
                            {
                                rlbAddedColumns.Visibility = Visibility.Visible;
                                RadListBoxItem lItem = new RadListBoxItem();
                                DataTemplate me = this.FindResource("roundedContent") as DataTemplate;
                                Style round = this.FindResource("RoundedRadListBoxItem") as Style;
                                lItem.Style = round;
                                lItem.ContentTemplate = me;
                                lItem.Tag = "addedColumn";
                                rlbAddedColumns.Items.Add(lItem);
                                lItem.UpdateLayout();
                            
                                ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(lItem);
                                if (myContentPresenter != null)
                                {
                                    TextBlock me2 = (TextBlock)me.FindName("txtMe", myContentPresenter) as TextBlock;
                                    me2.Text = fm.MAFField;
                                    addedColumnsUsed.Add(fm.MAFField);
                                    RadButton rb2 = (RadButton)me.FindName("btnMe", myContentPresenter) as RadButton;
                                    rb2.Tag = fm.FieldMappingID;
                                }
                            }
                            #endregion

                            #region MULTI MAPPINGS
                            if (fm.HasMultiMapping == true)
                            {
                                string column = fm.MAFField;                                

                                List<FrameworkUAS.Entity.FieldMultiMap> fmms = serviceClientSet.FieldMultiMap.Proxy.SelectFieldMappingID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, fm.FieldMappingID).Result;
                                foreach (FrameworkUAS.Entity.FieldMultiMap fmm in fmms)
                                {
                                    FrameworkUAD_Lookup.Entity.Code demoUpdateOption = DemoUpdateOptions.FirstOrDefault(x => x.CodeId == fm.DemographicUpdateCodeId);

                                    ColumnMapper cm = new ColumnMapper("MM", FrameworkUAS.Object.AppData.myAppData, thisContainer.myClient, thisContainer.uadColumns, ColumnMapperControlType.EditUser.ToString(), column, "", DemoUpdateOptions, adhocBitFields, isRequiredFields, demoUpdateOption, fmm.MAFField);
                                    cm.ClosePreviewData();
                                    cm.CloseLabelRow();
                                    cm.ButtonTag = fmm.FieldMultiMapID.ToString();
                                    cm.AddFieldMappingID = fmm.FieldMappingID;
                                    cm.TextBoxColumnName.Tag = fm.FieldMappingID;
                                    additionalLayout.Children.Add(cm);
                                    multiMapSV.Visibility = System.Windows.Visibility.Visible;
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            #endregion

            if (rcbStep3Actions.SelectedValue != null && !(String.IsNullOrEmpty(rcbStep3Actions.SelectedValue.ToString())))
            {
                #region POPULATE COLUMN COMBOS
                List<FileMappingColumn> notUsedUadColumns = new List<FileMappingColumn>();
                List<FrameworkUAS.Entity.FieldMapping> usedUadColumns = new List<FrameworkUAS.Entity.FieldMapping>();

                List<FrameworkUAS.Entity.FieldMapping> currentFieldMapping = serviceClientSet.FieldMapping.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.sourceFileID).Result;
                foreach (FileMappingColumn fmc in thisContainer.uadColumns)
                {
                    if (currentFieldMapping.FirstOrDefault(x => x.MAFField.Equals(fmc.ColumnName, StringComparison.CurrentCultureIgnoreCase) && x.FieldMappingTypeID != thisContainer.ignoreTypeID) == null)
                    {
                        notUsedUadColumns.Add(fmc);
                    }
                }

                foreach (FrameworkUAS.Entity.FieldMapping fm in currentFieldMapping)
                {
                    if (fm.IsNonFileColumn == true && fm.IncomingField.Length > 6)
                        fm.IncomingField = fm.MAFField;//fm.IncomingField = fm.IncomingField.Substring(6, fm.IncomingField.Length - 6);
                    if (fm.FieldMappingTypeID != thisContainer.ignoreTypeID)
                        usedUadColumns.Add(fm);

                }
                //rcbAddColumns
                rcbAddColumns.ItemsSource = null;
                rcbAddColumns.ItemsSource = notUsedUadColumns;
                rcbAddColumns.DisplayMemberPath = "ColumnName";
                rcbAddColumns.SelectedValuePath = "ColumnName";

                rcbAdditionalColumns.ItemsSource = null;
                rcbAdditionalColumns.ItemsSource = usedUadColumns;
                rcbAdditionalColumns.DisplayMemberPath = "IncomingField";
                rcbAdditionalColumns.SelectedValuePath = "FieldMappingID";
                #endregion

                if (rcbStep3Actions.SelectedValue.ToString() == StepThreeActions.Add_Column_Not_Defined_In_File.ToString().Replace('_', ' '))
                {
                    //ADD USER DEFINE COLUMN
                    txtBlckAddColumns.Visibility = System.Windows.Visibility.Visible;
                    rcbAddColumns.Visibility = System.Windows.Visibility.Visible;
                    rlbAddedColumns.Visibility = System.Windows.Visibility.Visible;
                    //rlbItem.Visibility = System.Windows.Visibility.Visible;
                    btnFinishAddColumns.Visibility = System.Windows.Visibility.Visible;

                    txtBlckAdditionalColumns.Visibility = System.Windows.Visibility.Collapsed;
                    rcbAdditionalColumns.Visibility = System.Windows.Visibility.Collapsed;
                    additionalLayout.Visibility = System.Windows.Visibility.Collapsed;
                    btnFinishAdditionalColumns.Visibility = System.Windows.Visibility.Collapsed;
                    btnAddAdditionalSetup.Visibility = System.Windows.Visibility.Collapsed;
                    multiMapSV.Visibility = System.Windows.Visibility.Collapsed;
                }
                else if (rcbStep3Actions.SelectedValue.ToString() == StepThreeActions.Add_Additional_Column_Mapping.ToString().Replace('_', ' '))
                {
                    //ADD MULTI MAPPING                    
                    txtBlckAdditionalColumns.Visibility = System.Windows.Visibility.Visible;
                    rcbAdditionalColumns.Visibility = System.Windows.Visibility.Visible;
                    additionalLayout.Visibility = System.Windows.Visibility.Visible;
                    btnFinishAdditionalColumns.Visibility = System.Windows.Visibility.Visible;
                    btnAddAdditionalSetup.Visibility = System.Windows.Visibility.Visible;

                    if (additionalLayout.Children.Count > 0)
                        multiMapSV.Visibility = System.Windows.Visibility.Visible;

                    txtBlckAddColumns.Visibility = System.Windows.Visibility.Collapsed;
                    rcbAddColumns.Visibility = System.Windows.Visibility.Collapsed;
                    rlbAddedColumns.Visibility = System.Windows.Visibility.Collapsed;
                    //rlbItem.Visibility = System.Windows.Visibility.Collapsed;
                    btnFinishAddColumns.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private void rcbAdditionalColumns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //additionalLayout.Children.Clear();
            //if (rcbAdditionalColumns.SelectedValue != null && !String.IsNullOrEmpty(rcbAdditionalColumns.SelectedValue.ToString()))
            //{
            //    string column = rcbAdditionalColumns.Text.ToString();
            //    int fmID = 0;
            //    int.TryParse(rcbAdditionalColumns.SelectedValue.ToString(), out fmID);
            //    FrameworkUAS.Entity.FieldMapping fm = fmData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.sourceFileID).Result.Where(x => x.FieldMappingID == fmID).FirstOrDefault();
            //    if (fm != null)
            //    {
            //        List<FrameworkUAS.Entity.FieldMultiMap> fmms = blfmm.Proxy.SelectFieldMappingID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, fm.FieldMappingID).Result;
            //        foreach (FrameworkUAS.Entity.FieldMultiMap fmm in fmms)
            //        {
            //            ColumnMapper cm = new ColumnMapper("MM", FrameworkUAS.Object.AppData.myAppData, thisContainer.myClient, thisContainer.uadColumns, ColumnMapperControlType.EditUser.ToString(), column, "", fmm.MAFField);
            //            cm.ClosePreviewData();
            //            cm.CloseLabelRow();
            //            cm.ButtonTag = fmm.FieldMultiMapID.ToString();
            //            cm.TextBoxColumnName.Tag = fmID;
            //            additionalLayout.Children.Add(cm);
            //            multiMapSV.Visibility = System.Windows.Visibility.Visible;
            //        }
            //    }
            //}
        }
        #endregion

        #region Buttons
        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            RadButton btnMe = sender as RadButton;
            Grid grdMe = btnMe.Parent as Grid;
            TextBlock test = grdMe.FindName("txtMe") as TextBlock;            
            RadListBoxItem l = grdMe.GetVisualParent<RadListBoxItem>();

            int fID = 0;
            if (btnMe.Tag != null)
                if (!String.IsNullOrEmpty(btnMe.Tag.ToString()))
                    int.TryParse(btnMe.Tag.ToString(), out fID);

            MessageBoxResult res = MessageBox.Show("Are you sure this should be deleted?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No) return;

            if (fID > 0)
            {
                int resultDeleteFieldMapping = serviceClientSet.TransformationFieldMapData.Proxy.DeleteFieldMapping(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, fID).Result;
                int resultDeleteMapping = serviceClientSet.FieldMapping.Proxy.DeleteMapping(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, fID).Result;
                ReorderColumns();
                Core_AMS.Utilities.WPF.MessageDeleteComplete();
            }

            if (l.Tag.Equals("addedColumn"))
            {                
                rlbAddedColumns.Items.Remove(l);
                if (addedColumnsUsed.Contains(test.Text))
                    addedColumnsUsed.Remove(test.Text);

            }            
        }        
                        
        private void btnStep3Prev_Click(object sender, RoutedEventArgs e)
        {
            //thisContainer.DataCompareCheck();
            thisContainer.NewColumnsToMapColumns();
        }
        
        private void btnStep3Next_Click(object sender, RoutedEventArgs e)
        {
            ReorderColumns();
            if (CheckBasicsMapped())
            {
                thisContainer.NewColumnsToTransformations();
                var borderList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.Border>(thisContainer);
                if (borderList.FirstOrDefault(x => x.Name.Equals("StepFourContainer", StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Border thisBorder = borderList.FirstOrDefault(x => x.Name.Equals("StepFourContainer", StringComparison.CurrentCultureIgnoreCase));
                    thisBorder.Child = new FileMapperWizard.Controls.Transformations(thisContainer);
                }
            }
        }
                        
        private void btnFinishAddColumns_Click(object sender, RoutedEventArgs e)
        {
            #region SAVE FIELDMAPPING
            List<FrameworkUAS.Entity.FieldMapping> fm = serviceClientSet.FieldMapping.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.sourceFileID).Result;
            int colCount = (fm.Count) + 1;
            foreach (RadListBoxItem rlbi in rlbAddedColumns.Items)
            {
                RadButton btn = FindVisualChild<RadButton>(rlbi);
                TextBlock tBlocks = rlbi.FindChildByType<TextBlock>();

                int fID = 0;
                if (btn.Tag != null)
                    if (!String.IsNullOrEmpty(btn.Tag.ToString()))
                        int.TryParse(btn.Tag.ToString(), out fID);

                //IF ID == 0 THIS MEANS IT IS NEW AND WE NEED TO CHECK FOR DUPLICATION
                if (fID == 0)
                {
                    foreach (FrameworkUAS.Entity.FieldMapping f in fm.Where(x => x.HasMultiMapping == true))
                    {
                        if (f.HasMultiMapping)
                        {
                            List<FrameworkUAS.Entity.FieldMultiMap> fmm = serviceClientSet.FieldMultiMap.Proxy.SelectFieldMappingID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, f.FieldMappingID).Result;
                            if (fmm.FirstOrDefault(x => x.MAFField.Equals(tBlocks.Text.Trim(), StringComparison.CurrentCultureIgnoreCase)) != null)
                            {
                                Core_AMS.Utilities.WPF.Message("Column '" + tBlocks.Text.ToString() + "' already used. Cannot use again." + thisContainer.issues, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Error");
                                return;
                            }
                        }
                    }
                    if (fm.FirstOrDefault(x => x.MAFField.Equals(tBlocks.Text.Trim(), StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        Core_AMS.Utilities.WPF.Message("Column '" + tBlocks.Text.ToString() + "' already used. Cannot use again." + thisContainer.issues, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Error");
                        return;
                    }
                }

                //FieldMapping Save Here
                #region New NonFileColumn FieldMapping Column
                if (fID == 0)
                {
                    FrameworkUAS.Entity.FieldMapping mapping = new FrameworkUAS.Entity.FieldMapping();
                    mapping.FieldMappingID = fID;
                    mapping.SourceFileID = thisContainer.sourceFileID;
                    mapping.IncomingField = "";
                    mapping.MAFField = tBlocks.Text;
                    mapping.PubNumber = 0;
                    mapping.DataType = "varchar";
                    mapping.PreviewData = "";
                    mapping.IsNonFileColumn = true;
                    mapping.DemographicUpdateCodeId = DemoUpdateOptions.FirstOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.DemographicUpdate.Replace.ToString())).CodeId;

                    //GET WHETHER STANDARD OR DEMOGRAPHIC   
                    FileMappingColumn fmc = thisContainer.uadColumns.FirstOrDefault(x => x.ColumnName.Equals(tBlocks.Text.Trim(), StringComparison.CurrentCultureIgnoreCase));
                    mapping.FieldMappingTypeID = thisContainer.ignoreTypeID;
                    if (fmc != null)
                    {
                        if (fmc.IsDemographic == true)
                            mapping.FieldMappingTypeID = thisContainer.demoTypeID; //DEMOGRAPHIC
                        else
                            mapping.FieldMappingTypeID = thisContainer.standardTypeID; //STANDARD

                    }
                    else
                    {
                        Core_AMS.Utilities.WPF.Message("Issue with User Added Column. Please contact Admin." + thisContainer.issues, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Error");
                        return;
                    }

                    mapping.ColumnOrder = colCount;
                    mapping.HasMultiMapping = false;

                    mapping.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    mapping.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    mapping.DateCreated = DateTime.Now;

                    if (mapping.IsNonFileColumn == true)
                        mapping.IncomingField = tBlocks.Text.ToString();


                    int i = serviceClientSet.FieldMapping.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, mapping).Result;

                    btn.Tag = i.ToString();
                    colCount++;
                }
                #endregion
                #region Update Existing NonFileColumn FieldMapping Column
                else
                {
                    FrameworkUAS.Entity.FieldMapping existFM = fm.FirstOrDefault(x => x.FieldMappingID == fID);
                    FrameworkUAS.Entity.FieldMapping mapping = new FrameworkUAS.Entity.FieldMapping();
                    mapping.FieldMappingID = existFM.FieldMappingID;
                    mapping.SourceFileID = existFM.SourceFileID;
                    mapping.IncomingField = existFM.IncomingField;
                    mapping.MAFField = existFM.MAFField;
                    mapping.PubNumber = existFM.PubNumber;
                    mapping.DataType = existFM.DataType;
                    mapping.PreviewData = existFM.PreviewData;
                    mapping.IsNonFileColumn = true;                   
                    mapping.FieldMappingTypeID = existFM.FieldMappingTypeID;
                    mapping.ColumnOrder = existFM.ColumnOrder;
                    mapping.HasMultiMapping = existFM.HasMultiMapping;
                    mapping.DemographicUpdateCodeId = existFM.DemographicUpdateCodeId;

                    mapping.CreatedByUserID = existFM.CreatedByUserID;
                    mapping.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    mapping.DateCreated = existFM.DateCreated;
                    mapping.DateUpdated = DateTime.Now;

                    int i = serviceClientSet.FieldMapping.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, mapping).Result;

                    btn.Tag = i.ToString();                    
                }
                #endregion
            }
            #endregion
            txtBlckAddColumns.Visibility = System.Windows.Visibility.Collapsed;
            rcbAddColumns.Visibility = System.Windows.Visibility.Collapsed;
            rlbAddedColumns.Visibility = System.Windows.Visibility.Collapsed;
            btnFinishAddColumns.Visibility = System.Windows.Visibility.Collapsed;
            rcbStep3Actions.SelectedItem = null;
            //rlbAddedColumns.Items.Clear();
            Core_AMS.Utilities.WPF.MessageSaveComplete();
        }
        
        private void btnFinishAdditionalColumns_Click(object sender, RoutedEventArgs e)
        {
            List<FrameworkUAS.Entity.FieldMapping> fm = serviceClientSet.FieldMapping.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.sourceFileID).Result;

            foreach (ColumnMapper mcm in additionalLayout.Children)
            {
                int mmID = 0;
                if (mcm.ButtonDelete.Tag != null)
                    if (!String.IsNullOrEmpty(mcm.ButtonDelete.Tag.ToString()))
                        int.TryParse(mcm.ButtonDelete.Tag.ToString(), out mmID);

                FrameworkUAS.Entity.FieldMapping thisFM = new FrameworkUAS.Entity.FieldMapping();
                int fmID = 0;
                int.TryParse(mcm.TextBoxColumnName.Tag.ToString(), out fmID);
                thisFM = fm.FirstOrDefault(x => x.FieldMappingID == fmID);

                if (thisFM != null)
                {
                    string mapTo = mcm.ComboBoxMappingText;
                    if (mapTo.Equals("ignore", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Core_AMS.Utilities.WPF.Message("Cannot map value to ignore.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Selection");
                        return;
                    }

                    if (mmID >= 0)
                    {
                        foreach (FrameworkUAS.Entity.FieldMapping f in fm.Where(x => x.HasMultiMapping == true))
                        {
                            if (f.HasMultiMapping)
                            {
                                List<FrameworkUAS.Entity.FieldMultiMap> fmm = serviceClientSet.FieldMultiMap.Proxy.SelectFieldMappingID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, f.FieldMappingID).Result;
                                if (fmm.FirstOrDefault(x => x.MAFField.Equals(mapTo.Trim(), StringComparison.CurrentCultureIgnoreCase)) != null && fmm.FirstOrDefault(x => x.MAFField.Equals(mapTo.Trim(), StringComparison.CurrentCultureIgnoreCase)).FieldMultiMapID != mmID)
                                {
                                    Core_AMS.Utilities.WPF.Message("Column '" + mapTo.Trim().ToString() + "' already used once and cannot be used again." + thisContainer.issues, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Error");
                                    return;
                                }
                            }
                        }
                        if (fm.FirstOrDefault(x => x.MAFField.Equals(mapTo.Trim().Trim(), StringComparison.CurrentCultureIgnoreCase)) != null)
                        {
                            Core_AMS.Utilities.WPF.Message("Column '" + mapTo.Trim().ToString() + "' already used once and cannot be used again.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Error");
                            return;
                        }
                    }

                    //UPDATE FIELD MAPPING
                    thisFM.HasMultiMapping = true;
                    thisFM.DemographicUpdateCodeId = mcm.DemographicUpdateCodeId;

                    serviceClientSet.FieldMapping.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisFM);

                    int mcmFieldMapTypeID = thisContainer.standardTypeID;
                    if (mcm.FieldMapType == FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString())
                        mcmFieldMapTypeID = thisContainer.ignoreTypeID;
                    else if (mcm.FieldMapType == FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString())
                        mcmFieldMapTypeID = thisContainer.demoTypeID;
                    else
                        mcmFieldMapTypeID = thisContainer.standardTypeID;

                    //SAVE FIELD MULTI MAPPING
                    FrameworkUAS.Entity.FieldMultiMap fMulti = new FrameworkUAS.Entity.FieldMultiMap
                    {
                        FieldMultiMapID = mmID,
                        FieldMappingID = thisFM.FieldMappingID,
                        FieldMappingTypeID = mcmFieldMapTypeID,
                        MAFField = mapTo,
                        DataType = "varchar",
                        PreviewData = thisFM.PreviewData,
                        ColumnOrder = 0,
                        DateCreated = DateTime.Now,
                        DateUpdated = DateTime.Now,
                        CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                        UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID
                    };

                    int y = serviceClientSet.FieldMultiMap.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, fMulti).Result;

                    mcm.ButtonDelete.Tag = y.ToString();
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("Error adding additional column mapping. Please contact Admin.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Error");
                    return;
                }
            }

            btnAddAdditionalSetup.Visibility = Visibility.Collapsed;
            txtBlckAdditionalColumns.Visibility = System.Windows.Visibility.Collapsed;
            rcbAdditionalColumns.Visibility = System.Windows.Visibility.Collapsed;
            additionalLayout.Visibility = System.Windows.Visibility.Collapsed;
            btnFinishAdditionalColumns.Visibility = System.Windows.Visibility.Collapsed;
            rcbStep3Actions.SelectedItem = null;
            Core_AMS.Utilities.WPF.MessageSaveComplete();
        }
        
        private void btnAddAdditionalSetup_Click(object sender, RoutedEventArgs e)
        {
            //List<FrameworkUAS.Entity.FieldMapping> fm = fmData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, sourceFileID).Result;
            if (rcbAdditionalColumns.SelectedValue != null && !String.IsNullOrEmpty(rcbAdditionalColumns.SelectedValue.ToString()))
            {
                List<FrameworkUAS.Entity.FieldMapping> originalFieldMappings = new List<FrameworkUAS.Entity.FieldMapping>();
                originalFieldMappings = serviceClientSet.FieldMapping.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.AccessKey, thisContainer.sourceFileID).Result;
                int fieldMappingID = 0;
                string column = rcbAdditionalColumns.Text.ToString();
                FrameworkUAS.Entity.FieldMapping originalFM = originalFieldMappings.FirstOrDefault(x => x.MAFField.Equals(column, StringComparison.CurrentCultureIgnoreCase));
                if (originalFM != null)
                    fieldMappingID = originalFM.FieldMappingID;

                ColumnMapper cm = new ColumnMapper("MM", FrameworkUAS.Object.AppData.myAppData, thisContainer.myClient, thisContainer.uadColumns, ColumnMapperControlType.New.ToString(), column, "", DemoUpdateOptions, adhocBitFields, isRequiredFields);
                cm.ClosePreviewData();
                cm.CloseLabelRow();
                cm.AddFieldMappingID = fieldMappingID;
                cm.TextBoxColumnName.Tag = rcbAdditionalColumns.SelectedValue.ToString();
                additionalLayout.Children.Add(cm);
                multiMapSV.Visibility = System.Windows.Visibility.Visible;
            }
        }
        #endregion

        public bool CheckBasicsMapped()
        {
            bool valid = true;

            List<FrameworkUAS.Entity.FieldMapping> fieldMappings = new List<FrameworkUAS.Entity.FieldMapping>();
            fieldMappings = serviceClientSet.FieldMapping.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.AccessKey, thisContainer.sourceFileID).Result;
            int dbFT = thisContainer.DatabaseFileTypeList.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FileTypes.Audience_Data.ToString().Replace("_", " "))).CodeId;
            if (fieldMappings.FirstOrDefault(x => x.MAFField.Equals("PUBCODE", StringComparison.CurrentCultureIgnoreCase)) == null && thisContainer.DatabaseFileType == dbFT)
            {
                valid = false;
                Core_AMS.Utilities.WPF.Message("Pubcode not present in mapping. Pubcode must be mapped before proceeding.", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, "Missing PubCode Mapping");
            }
            int demoRespOtherId = 0;
            List<FrameworkUAD_Lookup.Entity.Code> codes = serviceClientSet.LookUpCode.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.AccessKey, FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping).Result;
                
            demoRespOtherId = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Other.ToString().Replace('_', ' '))).CodeId;
            List<FrameworkUAS.Entity.FieldMapping> demoRespOtherMappings = new List<FrameworkUAS.Entity.FieldMapping>();
            demoRespOtherMappings.AddRange(fieldMappings.Where(x => x.FieldMappingTypeID == demoRespOtherId));
            if (demoRespOtherMappings != null && demoRespOtherMappings.Count > 0)
            {
                foreach(FrameworkUAS.Entity.FieldMapping fm in demoRespOtherMappings)
                {
                    string demoName = fm.MAFField.ToUpper().Replace("_RESPONSEOTHER", "").ToString();
                    if (fieldMappings.FirstOrDefault(x => x.MAFField.Equals(demoName, StringComparison.CurrentCultureIgnoreCase)) == null)
                    {
                        valid = false;                        
                        Core_AMS.Utilities.WPF.Message("In order to map: " + fm.MAFField + " please map the parent Demographic Response Group. Failure to do so will result in the loss of the 'other text' for this Demographic Response.");
                        break;
                    }
                }
            }

            //Check a Demo_ResponseOther has a based demo mapped
            return valid;
        }

        public void ReorderColumns()
        {
            serviceClientSet.FieldMapping.Proxy.ColumnReorder(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.sourceFileID);
        }

        #region Find Controls
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)
         where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj)
         where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        #endregion
    }
}

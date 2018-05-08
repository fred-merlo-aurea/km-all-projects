using Core_AMS.Utilities;
using FileMapperWizard.Controls;
using FrameworkUAD.Object;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using KM.Common.Import;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.TileView;

namespace FileMapperWizard.Modules
{
    /// <summary>
    /// Interaction logic for FMLite.xaml
    /// </summary>
    public partial class FMUniversal : UserControl
    {
        #region Properties
        public bool Expanded { get; set; }
        public string FieldMappingErrorMessage { get; set; }
        public string issues { get; set; }
        public DataTable fileData { get; set; }
        public Dictionary<int, ColumnMapper> columnMapperList { get; set; }// = new Dictionary<int, ColumnMapper>();
        public Dictionary<int, string> pubCode { get; set; }// = new Dictionary<int, string>();
        public Dictionary<string, string> delimiters { get; set; }// = new Dictionary<string, string>();
        public FileConfiguration myFileConfig { get; set; }// = new FileConfiguration();
        public FileWorker fw { get; set; }// = new FileWorker();
        public KMPlatform.Entity.Client myClient { get; set; }// = new KMPlatform.Entity.Client();
        public KMPlatform.Entity.Service myService { get; set; }// = new KMPlatform.Entity.Service();
        public int currentCol { get; set; }// = 0;
        public int myAssignID { get; set; }// = 0;
        public int myJoinID { get; set; }// = 0;
        public int mySplitTransformationID { get; set; }// = 0;
        public int myTransformationID { get; set; }// = 0;
        public int myTransSplitID { get; set; }// = 0;
        public List<FileMappingColumn> uadColumns { get; set; }// = new List<FileMappingColumn>();
        public List<KMPlatform.Entity.Client> AllClients { get; set; }// = new List<KMPlatform.Entity.Client>();
        public List<KMPlatform.Entity.Service> AllServices { get; set; }// = new List<KMPlatform.Entity.Service>();
        public List<KMPlatform.Entity.ServiceFeature> AllFeatures { get; set; }// = new List<KMPlatform.Entity.ServiceFeature>();
        public List<int> oldSavedPubCodes { get; set; }// = new List<int>();
        public List<RadTreeViewItem> fileColumns { get; set; }// = new List<RadTreeViewItem>();
        public List<string> transformingCols { get; set; }// = new List<string>();
        public ObservableCollection<string> AvailableCols { get; set; }// = new ObservableCollection<string>();
        public ObservableCollection<string> JoinedCols { get; set; }// = new ObservableCollection<string>();
        public StringDictionary columns { get; set; }// = new Dictionary<int, string>();
        public int fileRecurrenceTypeId { get; set; }
        public string FileName { get; set; }// = "";
        public string myFileWorkType { get; set; }// = "";
        public int ignoreTypeID { get; set; }// = 0;
        public int demoTypeID { get; set; }
        public int standardTypeID { get; set; }
        public string myDelimiter { get; set; }// = "";
        public bool myTextQualifier { get; set; }// = false;
        public int srcFileTypeID { get; set; }// = 0;
        public int myFeatureID { get; set; }// = 0;
        public KMPlatform.BusinessLogic.Enums.UADFeatures myUADFeature { get; set; }
        public string saveFileName { get; set; }// = txtFileName.Text;
        public FileInfo fileInfo { get; set; }
        public int sourceFileID { get; set; }
        public int batchSize { get; set; }
        public string qDateFormat { get; set; }
        public string extension { get; set; }
        public bool isCirculation { get; set; }
        public bool isEdit { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsIgnored { get; set; }
        public int FileSnippetID { get; set; }
        public bool IsDQMReady { get; set; }
        public int MasterGroupID { get; set; }
        public bool UseRealTimeGeocoding { get; set; }
        public bool IsSpecialFile { get; set; }
        public int ClientCustomProcedureID { get; set; }
        public int SpecialFileResultID { get; set; }
        public List<KMPlatform.Entity.Client> AllPublishers { get; set; }
        public List<FrameworkUAD.Entity.Product> AllPublications { get; set; }
        public KMPlatform.Entity.Client currentPublisher { get; set; }
        public FrameworkUAD.Entity.Product currentPublication { get; set; }
        public int DatabaseFileType { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> DatabaseFileTypeList { get; set; }
        public string filePath { get; set; }
        public FrameworkUAD.Entity.Product selectedProduct { get; set; }

        public List<FrameworkUAS.Entity.FieldMapping> currentFieldMappings { get; set; }
        public List<int> fieldMappingsWithTransformations { get; set; }
        #endregion

        public FMUniversal(bool edit, bool isCirc = false, KMPlatform.Entity.Client client = null, FrameworkUAD.Entity.Product prod = null, string filePath = "", bool isDataCompare = false)
        {
            this.fw = new FileWorker();
            this.fileData = new DataTable();
            this.columnMapperList = new Dictionary<int, ColumnMapper>();
            this.pubCode = new Dictionary<int, string>();
            this.delimiters = new Dictionary<string, string>();
            this.myFileConfig = new FileConfiguration();
            if (client != null)
                this.myClient = client;
            else
                this.myClient = new KMPlatform.Entity.Client();

            this.myService = new KMPlatform.Entity.Service();
            this.uadColumns = new List<FileMappingColumn>();
            this.AllClients = new List<KMPlatform.Entity.Client>();
            this.AllServices = new List<KMPlatform.Entity.Service>();
            this.AllFeatures = new List<KMPlatform.Entity.ServiceFeature>();
            this.oldSavedPubCodes = new List<int>();
            this.fileColumns = new List<RadTreeViewItem>();
            this.transformingCols = new List<string>();
            this.AvailableCols = new ObservableCollection<string>();
            this.JoinedCols = new ObservableCollection<string>();
            this.columns = new StringDictionary();
            this.sourceFileID = 0;
            this.batchSize = 2500;
            this.fileRecurrenceTypeId = 0;
            this.qDateFormat = "";
            this.extension = "";
            this.isCirculation = isCirc;
            this.isEdit = edit;
            this.AllPublishers = new List<KMPlatform.Entity.Client>();
            this.AllPublications = new List<FrameworkUAD.Entity.Product>();
            this.currentPublisher = null;
            this.currentPublication = null;
            this.DatabaseFileType = 0;
            if (!string.IsNullOrEmpty(filePath))
                this.filePath = filePath;
            else
                this.filePath = "";

            if (prod != null)
                this.selectedProduct = prod;
            else
                this.selectedProduct = null;

            this.currentFieldMappings = null;
            this.IsDeleted = false;
            this.IsIgnored = false;
            this.FileSnippetID = 0;
            this.IsDQMReady = true;
            this.MasterGroupID = 0;
            this.UseRealTimeGeocoding = false;
            this.IsSpecialFile = false;
            this.ClientCustomProcedureID = 0;
            this.SpecialFileResultID = 0;
            this.DatabaseFileTypeList = new List<FrameworkUAD_Lookup.Entity.Code>();
            this.fieldMappingsWithTransformations = new List<int>();

            if (isDataCompare == true)
                myUADFeature = KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare;

            InitializeComponent();
            LoadData(edit, isCirc, isDataCompare);
        }

        public void LoadData(bool inEdit, bool isCirc, bool isDataCompare)
        {
            if (this.isEdit)
                StepOneContainer.Child = new FileMapperWizard.Controls.EditSetup(this);
            else
            {
                if (this.isCirculation)
                    StepOneContainer.Child = new FileMapperWizard.Controls.CircSetup(this);
                else
                    StepOneContainer.Child = new FileMapperWizard.Controls.UADSetup(this,isDataCompare);

            }
        }

        public void SetupToSpecialFile()
        {
            tileClient.ContentTemplate = (DataTemplate)this.FindResource("step1Min") as DataTemplate;
            tileClient.HeaderTemplate = null;
            tileMapColumns.HeaderTemplate = (DataTemplate)this.FindResource("step2Header_Special") as DataTemplate;
            tileMapColumns.ContentTemplate = null;
            tileMapColumns.UpdateLayout();           
            tvMe.MaximizedItem = tileMapColumns;
            var textboxList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.TextBlock>(tileMapColumns);
            if (textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepTwoSpecial", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                TextBlock thisBlock = textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepTwoSpecial", StringComparison.CurrentCultureIgnoreCase));
                thisBlock.Text = "File - " + System.IO.Path.GetFileName(FileName);
            }
        }
        public void SetupToMapColumns()
        {
            tileClient.ContentTemplate = (DataTemplate)this.FindResource("step1Min") as DataTemplate;
            tileClient.HeaderTemplate = null;
            tileMapColumns.HeaderTemplate = (DataTemplate)this.FindResource("step2Header") as DataTemplate;
            tileMapColumns.ContentTemplate = null;
            tileMapColumns.UpdateLayout();
            tvMe.MaximizedItem = tileMapColumns;
            var textboxList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.TextBlock>(tileMapColumns);
            if (textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepTwo", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                TextBlock thisBlock = textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepTwo", StringComparison.CurrentCultureIgnoreCase));
                thisBlock.Text = "File - " + System.IO.Path.GetFileName(FileName);
            }
        }

        public void MapToSetupColumns()
        {
            tileMapColumns.ContentTemplate = (DataTemplate)this.FindResource("step2Min") as DataTemplate;
            tileMapColumns.HeaderTemplate = null;
            tileClient.HeaderTemplate = (DataTemplate)this.FindResource("step1Header") as DataTemplate;
            tileClient.ContentTemplate = null;
            tileClient.UpdateLayout();
            tvMe.MaximizedItem = tileClient;
            var textboxList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.TextBlock>(tileClient);
            if (textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepOne", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                TextBlock thisBlock = textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepOne", StringComparison.CurrentCultureIgnoreCase));                
            }
        }
        public void MapColumnsToNewColumns()
        {
            tileMapColumns.ContentTemplate = (DataTemplate)this.FindResource("step2Min") as DataTemplate;
            tileMapColumns.HeaderTemplate = null;
            tileAddColumns.HeaderTemplate = (DataTemplate)this.FindResource("step3Header") as DataTemplate;
            tileAddColumns.ContentTemplate = null;
            tileAddColumns.UpdateLayout();
            tvMe.MaximizedItem = tileAddColumns;
            var textboxList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.TextBlock>(tileAddColumns);
            if (textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepThree", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                TextBlock thisBlock = textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepThree", StringComparison.CurrentCultureIgnoreCase));
                thisBlock.Text = "File - " + System.IO.Path.GetFileName(FileName);
            }
        }

        public void NewColumnsToMapColumns()
        {
            tileAddColumns.ContentTemplate = (DataTemplate)this.FindResource("step3Min") as DataTemplate;
            tileAddColumns.HeaderTemplate = null;
            tileMapColumns.HeaderTemplate = (DataTemplate)this.FindResource("step2Header") as DataTemplate;
            tileMapColumns.ContentTemplate = null;
            tileMapColumns.UpdateLayout();
            tvMe.MaximizedItem = tileMapColumns;
            var textboxList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.TextBlock>(tileMapColumns);
            if (textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepTwo", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                TextBlock thisBlock = textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepTwo", StringComparison.CurrentCultureIgnoreCase));
                thisBlock.Text = "File - " + System.IO.Path.GetFileName(FileName);
            }
            var stackpanelList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.StackPanel>(tileMapColumns);
            if (stackpanelList.FirstOrDefault(x => x.Name.Equals("flowLayout", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                StackPanel thisPanel = stackpanelList.FirstOrDefault(x => x.Name.Equals("flowLayout", StringComparison.CurrentCultureIgnoreCase));
                foreach (ColumnMapper cm in thisPanel.Children)
                {
                    if (fieldMappingsWithTransformations.Contains(cm.myFieldMappingID))
                        cm.HasTransformation = true;
                    else
                        cm.HasTransformation = false;
                }
            }
            //flowLayout
        }
        public void NewColumnsToTransformations()
        {
            tileAddColumns.ContentTemplate = (DataTemplate)this.FindResource("step3Min") as DataTemplate;
            tileAddColumns.HeaderTemplate = null;
            tileCreateTransforms.HeaderTemplate = (DataTemplate)this.FindResource("step4Header") as DataTemplate;
            tileCreateTransforms.ContentTemplate = null;
            tileCreateTransforms.UpdateLayout();
            tvMe.MaximizedItem = tileCreateTransforms;
            var textboxList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.TextBlock>(tileCreateTransforms);
            if (textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepFour", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                TextBlock thisBlock = textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepFour", StringComparison.CurrentCultureIgnoreCase));
                thisBlock.Text = "File - " + System.IO.Path.GetFileName(FileName);
            }
        }

        public void TransformationsToNewColumns()
        {
            tileCreateTransforms.ContentTemplate = (DataTemplate)this.FindResource("step4Min") as DataTemplate;
            tileCreateTransforms.HeaderTemplate = null;
            tileAddColumns.HeaderTemplate = (DataTemplate)this.FindResource("step3Header") as DataTemplate;
            tileAddColumns.ContentTemplate = null;
            tileAddColumns.UpdateLayout();
            tvMe.MaximizedItem = tileAddColumns;
            var textboxList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.TextBlock>(tileAddColumns);
            if (textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepThree", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                TextBlock thisBlock = textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepThree", StringComparison.CurrentCultureIgnoreCase));
                thisBlock.Text = "File - " + System.IO.Path.GetFileName(FileName);
            }
        }
        public void TransformationsToRules()
        {
            //TransformationsToReview();

            // this is commented out for now until QA can be done 
            tileCreateTransforms.ContentTemplate = (DataTemplate)this.FindResource("step4Min") as DataTemplate;
            tileCreateTransforms.HeaderTemplate = null;
            tileRules.HeaderTemplate = (DataTemplate)this.FindResource("stepImportRulesHeader") as DataTemplate;
            tileRules.ContentTemplate = null;
            tileRules.UpdateLayout();
            tvMe.MaximizedItem = tileRules;
            var textboxList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.TextBlock>(tileRules);
            if (textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepImportRules", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                TextBlock thisBlock = textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepImportRules", StringComparison.CurrentCultureIgnoreCase));
                thisBlock.Text = "File - " + System.IO.Path.GetFileName(FileName);
            }
        }

        public void RulesToTransformations()
        {
            //ReviewToTransformations();

            // this is commented out for now until QA can be done 
            if (tileRules.ContentTemplate == null)
                tileRules.ContentTemplate = new DataTemplate();

            if (tileRules.ContentTemplate != null)
                tileRules.ContentTemplate = (DataTemplate) this.FindResource("stepImportRuleMin") as DataTemplate;
            tileRules.HeaderTemplate = null;
            tileCreateTransforms.HeaderTemplate = (DataTemplate) this.FindResource("step4Header") as DataTemplate;
            tileCreateTransforms.ContentTemplate = null;
            tileCreateTransforms.UpdateLayout();
            tvMe.MaximizedItem = tileCreateTransforms;
            var textboxList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.TextBlock>(tileCreateTransforms);
            if (textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepFour", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                TextBlock thisBlock = textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepFour", StringComparison.CurrentCultureIgnoreCase));
                thisBlock.Text = "File - " + System.IO.Path.GetFileName(FileName);
            }
        }
        public void RulesToReview()
        {
            if (tileRules.ContentTemplate == null)
                tileRules.ContentTemplate = new DataTemplate();

            if (tileRules.ContentTemplate != null)
                tileRules.ContentTemplate = (DataTemplate) this.FindResource("stepImportRuleMin") as DataTemplate;
            tileRules.HeaderTemplate = null;
            tileReview.HeaderTemplate = (DataTemplate) this.FindResource("stepReviewHeader") as DataTemplate;
            tileReview.ContentTemplate = null;
            tileReview.UpdateLayout();
            tvMe.MaximizedItem = tileReview;
        }
        public void ReviewToRules()
        {
            try
            {
                tileReview.ContentTemplate = (DataTemplate) this.FindResource("stepReviewMin") as DataTemplate;
                tileReview.HeaderTemplate = null;
                tileRules.HeaderTemplate = (DataTemplate) this.FindResource("stepImportRulesHeader") as DataTemplate;
                tileRules.ContentTemplate = null;
                tileRules.UpdateLayout();
                tvMe.MaximizedItem = tileRules;
                var textboxList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.TextBlock>(tileRules);
                if (textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepImportRules", StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    TextBlock thisBlock = textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepImportRules", StringComparison.CurrentCultureIgnoreCase));
                    thisBlock.Text = "File - " + System.IO.Path.GetFileName(FileName);
                }
            }
            catch(Exception ex)
            {
                string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            }
            //tileCreateTransforms.HeaderTemplate = (DataTemplate) this.FindResource("stepImportRulesHeader") as DataTemplate;
            //tileCreateTransforms.ContentTemplate = null;
            //tileCreateTransforms.UpdateLayout();
            //tvMe.MaximizedItem = tileCreateTransforms;
            //var textboxList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.TextBlock>(tileCreateTransforms);
            //if (textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepImportRules", StringComparison.CurrentCultureIgnoreCase)) != null)
            //{
            //    TextBlock thisBlock = textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepImportRules", StringComparison.CurrentCultureIgnoreCase));
            //    thisBlock.Text = "File - " + System.IO.Path.GetFileName(FileName);
            //}
        }

        public void TransformationsToReview()
        {
            tileCreateTransforms.ContentTemplate = (DataTemplate)this.FindResource("step4Min") as DataTemplate;
            tileCreateTransforms.HeaderTemplate = null;
            tileReview.HeaderTemplate = (DataTemplate)this.FindResource("stepReviewHeader") as DataTemplate;
            tileReview.ContentTemplate = null;
            tileReview.UpdateLayout();
            tvMe.MaximizedItem = tileReview;
        }
        public void ReviewToTransformations()
        {
            tileReview.ContentTemplate = (DataTemplate)this.FindResource("stepReviewMin") as DataTemplate;
            tileReview.HeaderTemplate = null;
            tileCreateTransforms.HeaderTemplate = (DataTemplate)this.FindResource("step4Header") as DataTemplate;
            tileCreateTransforms.ContentTemplate = null;
            tileCreateTransforms.UpdateLayout();
            tvMe.MaximizedItem = tileCreateTransforms;
            var textboxList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.TextBlock>(tileCreateTransforms);
            if (textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepFour", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                TextBlock thisBlock = textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepFour", StringComparison.CurrentCultureIgnoreCase));
                thisBlock.Text = "File - " + System.IO.Path.GetFileName(FileName);
            }
        }

        public void ShowRules()
        {
            if (this.isCirculation)
                tileRules.Visibility = System.Windows.Visibility.Visible;
        }
        public void HideRules()
        {
            tileRules.Visibility = System.Windows.Visibility.Collapsed;
        }
        #region Data Compare - old DEPRICATED
        //public void ShowDataCompareOptions()
        //{
        //    tileDCOpt.Visibility = System.Windows.Visibility.Visible;
        //}
        //public void DataCompareCheck()
        //{
        //    if (this.myUADFeature == KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
        //        tileDCOpt.Visibility = System.Windows.Visibility.Visible;
        //    else
        //        tileDCOpt.Visibility = System.Windows.Visibility.Collapsed;
        //}
        //public void HideDataCompareOptions()
        //{
        //    tileDCOpt.Visibility = System.Windows.Visibility.Collapsed;
        //}
        //public void TransformationsToDataCompareOptions()
        //{
        //    tileCreateTransforms.ContentTemplate = (DataTemplate)this.FindResource("step4Min") as DataTemplate;
        //    tileCreateTransforms.HeaderTemplate = null;
        //    tileDCOpt.HeaderTemplate = (DataTemplate)this.FindResource("stepDCOptHeader") as DataTemplate;
        //    tileDCOpt.ContentTemplate = null;
        //    tileDCOpt.UpdateLayout();
        //    tvMe.MaximizedItem = tileDCOpt;
        //    var textboxList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.TextBlock>(tileDCOpt);
        //    if (textboxList.FirstOrDefault(x => x.Name.Equals("tbFileDCOpt", StringComparison.CurrentCultureIgnoreCase)) != null)
        //    {
        //        TextBlock thisBlock = textboxList.FirstOrDefault(x => x.Name.Equals("tbFileDCOpt", StringComparison.CurrentCultureIgnoreCase));
        //        thisBlock.Text = "File - " + System.IO.Path.GetFileName(FileName);
        //    }
        //}

        //public void DataCompareOptionsToTransformations()
        //{
        //    tileDCOpt.ContentTemplate = (DataTemplate)this.FindResource("stepDCOptMin") as DataTemplate;
        //    tileDCOpt.HeaderTemplate = null;
        //    tileCreateTransforms.HeaderTemplate = (DataTemplate)this.FindResource("step4Header") as DataTemplate;
        //    tileCreateTransforms.ContentTemplate = null;
        //    tileCreateTransforms.UpdateLayout();
        //    tvMe.MaximizedItem = tileCreateTransforms;
        //    var textboxList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.TextBlock>(tileCreateTransforms);
        //    if (textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepFour", StringComparison.CurrentCultureIgnoreCase)) != null)
        //    {
        //        TextBlock thisBlock = textboxList.FirstOrDefault(x => x.Name.Equals("tbFileStepFour", StringComparison.CurrentCultureIgnoreCase));
        //        thisBlock.Text = "File - " + System.IO.Path.GetFileName(FileName);
        //    }
        //}
        #endregion

        private void tile_Loaded(object sender, RoutedEventArgs e)
        {
            Style s = this.FindResource("roundedComboBoxes") as Style;
            RadTileViewItem tvi = sender as RadTileViewItem;
            TileViewItemHeader tvh = (TileViewItemHeader)tvi.Template.FindName("HeaderPart", tvi);
            if (tvh != null)
            {
                RadToggleButton btn = (RadToggleButton)tvh.Template.FindName("MaximizeToggleButton", tvh);
                btn.Visibility = System.Windows.Visibility.Hidden;
                btn.IsEnabled = false;
            }

            if (tvi.Name.Equals("tileClient"))
            {
                var rcbs = Core_AMS.Utilities.WPF.FindVisualChildren<RadComboBox>(tvi);
                foreach (RadComboBox rcb in rcbs)
                {
                    rcb.Style = s;
                }
            }

            //if (this.isEdit)
            //{
            //    tileRules.Visibility = System.Windows.Visibility.Collapsed;
            //}
            //else
            //{
            //    if (!this.isCirculation)
            //        tileRules.Visibility = System.Windows.Visibility.Collapsed;
            //}

            //if (this.myUADFeature != KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
            //    tileDCOpt.Visibility = System.Windows.Visibility.Collapsed;
            //else if (this.myUADFeature == KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
            //    tileDCOpt.Visibility = System.Windows.Visibility.Visible;
        }

        private void TileStateChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Style s = this.FindResource("roundedComboBoxes") as Style;
            RadTileViewItem tvi = sender as RadTileViewItem;
            if (tvi.TileState.Equals(TileViewItemState.Maximized))
            {
                var rcbs = Core_AMS.Utilities.WPF.FindVisualChildren<RadComboBox>(tvi);
                foreach (RadComboBox rcb in rcbs)
                {
                    rcb.Style = s;
                }
            }
        }
    }
}

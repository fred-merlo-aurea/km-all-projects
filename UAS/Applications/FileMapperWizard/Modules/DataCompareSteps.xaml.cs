using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.TileView;

namespace FileMapperWizard.Modules
{
    /// <summary>
    /// Interaction logic for DataCompareSteps.xaml
    /// </summary>
    public partial class DataCompareSteps : UserControl
    {
        public FileMapperWizard.Modules.FMUniversal fmContainer;

        #region DataCompareSteps Container Variables
        public int targetCodeId { get; set; }
        public string targetName { get; set; }
        public string fileName { get; set; }
        public string emailAddresses { get; set; }
        public int? productId { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public int? brandId { get; set; }
        public string brandName { get; set; }
        public int? marketId { get; set; }
        public string marketName { get; set; }
        public bool isConsensus { get; set; }
        //public FrameworkUAS.Entity.DataCompareQue dataCompareResultQue { get; set; }
        public string profileSelection { get; set; }
        public string demoSelection { get; set; }
        public System.Xml.Linq.XDocument profileAttributes { get; set; }
        public System.Xml.Linq.XDocument demoAttributes { get; set; }
        public List<FrameworkUAD_Lookup.Entity.CodeType> codeTypes { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> profileCodes { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> demoCodes { get; set; }
        //public List<FrameworkUAS.Entity.DataCompareUserMatchCriteria> matchCriteriaList { get; set; }
        public string matchClause { get; set; }
        //public List<FrameworkUAS.Entity.DataCompareUserLikeCriteria> likeCriteriaList { get; set; }
        public string likeClause { get; set; }

        #endregion
        public DataCompareSteps(FileMapperWizard.Modules.FMUniversal container)
        {
            fmContainer = container;
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            StepOneContainer.Child = new FileMapperWizard.Controls.DCTarget(this);
            if (profileCodes == null)
                profileCodes = new List<FrameworkUAD_Lookup.Entity.Code>();
            if (demoCodes == null)
                demoCodes = new List<FrameworkUAD_Lookup.Entity.Code>();

            if (codeTypes == null || codeTypes.Count == 0)
            {
                FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICodeType> cWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeTypeClient();
                FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>> respCT = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>>();
                respCT = cWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);

                if (respCT.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success && respCT.Result != null)
                {
                    codeTypes = respCT.Result;
                }
            }
        }
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

            var rcbs = Core_AMS.Utilities.WPF.FindVisualChildren<RadComboBox>(tvi);
            foreach (RadComboBox rcb in rcbs)
            {
                rcb.Style = s;
            }
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
        #region Load Tiles
        #region Next
        /// <summary>
        /// tileTarget to tileResultQue
        /// </summary>
        public void Step1ToStep2()
        {
            tileTarget.ContentTemplate = (DataTemplate)this.FindResource("step1Min") as DataTemplate;
            tileTarget.HeaderTemplate = null;
            tileResultQue.HeaderTemplate = (DataTemplate)this.FindResource("step2Header") as DataTemplate;
            tileResultQue.ContentTemplate = null;
            tileResultQue.UpdateLayout();
            tvMe.MaximizedItem = tileResultQue;
        }
        /// <summary>
        /// tileResultQue to tileProfile
        /// </summary>
        public void Step2ToStep3()
        {
            tileResultQue.ContentTemplate = (DataTemplate)this.FindResource("step2Min") as DataTemplate;
            tileResultQue.HeaderTemplate = null;
            tileProfile.HeaderTemplate = (DataTemplate)this.FindResource("step3Header") as DataTemplate;
            tileProfile.ContentTemplate = null;
            tileProfile.UpdateLayout();
            tvMe.MaximizedItem = tileProfile;
        }
        /// <summary>
        /// tileProfile to tileDemo
        /// </summary>
        public void Step3ToStep4()
        {
            tileProfile.ContentTemplate = (DataTemplate)this.FindResource("step3Min") as DataTemplate;
            tileProfile.HeaderTemplate = null;
            tileDemo.HeaderTemplate = (DataTemplate)this.FindResource("step4Header") as DataTemplate;
            tileDemo.ContentTemplate = null;
            tileDemo.UpdateLayout();
            tvMe.MaximizedItem = tileDemo;
        }
        /// <summary>
        /// tileDemo to tileMatch
        /// </summary>
        public void Step4ToStep5()
        {
            tileDemo.ContentTemplate = (DataTemplate)this.FindResource("step4Min") as DataTemplate;
            tileDemo.HeaderTemplate = null;
            tileMatch.HeaderTemplate = (DataTemplate)this.FindResource("step5Header") as DataTemplate;
            tileMatch.ContentTemplate = null;
            tileMatch.UpdateLayout();
            tvMe.MaximizedItem = tileMatch;
        }
        /// <summary>
        /// tileMatch to tileLike
        /// </summary>
        public void Step5ToStep6()
        {
            tileMatch.ContentTemplate = (DataTemplate)this.FindResource("step5Min") as DataTemplate;
            tileMatch.HeaderTemplate = null;
            tileLike.HeaderTemplate = (DataTemplate)this.FindResource("step6Header") as DataTemplate;
            tileLike.ContentTemplate = null;
            tileLike.UpdateLayout();
            tvMe.MaximizedItem = tileLike;
        }
        #endregion
        #region Previous
        /// <summary>
        /// tileResultQue to tileTarget
        /// </summary>
        public void Step2ToStep1()
        {
            tileResultQue.ContentTemplate = (DataTemplate)this.FindResource("step2Min") as DataTemplate;
            tileResultQue.HeaderTemplate = null;
            tileTarget.HeaderTemplate = (DataTemplate)this.FindResource("step1Header") as DataTemplate;
            tileTarget.ContentTemplate = null;
            tileTarget.UpdateLayout();
            tvMe.MaximizedItem = tileTarget;
        }
        /// <summary>
        /// tileProfile to tileResultQue
        /// </summary>
        public void Step3ToStep2()
        {
            tileProfile.ContentTemplate = (DataTemplate)this.FindResource("step3Min") as DataTemplate;
            tileProfile.HeaderTemplate = null;
            tileResultQue.HeaderTemplate = (DataTemplate)this.FindResource("step2Header") as DataTemplate;
            tileResultQue.ContentTemplate = null;
            tileResultQue.UpdateLayout();
            tvMe.MaximizedItem = tileResultQue;
        }
        /// <summary>
        /// tileDemo to tileResultQue
        /// </summary>
        public void Step4ToStep3()
        {
            tileDemo.ContentTemplate = (DataTemplate)this.FindResource("step4Min") as DataTemplate;
            tileDemo.HeaderTemplate = null;
            tileProfile.HeaderTemplate = (DataTemplate)this.FindResource("step3Header") as DataTemplate;
            tileProfile.ContentTemplate = null;
            tileProfile.UpdateLayout();
            tvMe.MaximizedItem = tileProfile;
        }
        /// <summary>
        /// tileMatch to tileDemo
        /// </summary>
        public void Step5ToStep4()
        {
            tileMatch.ContentTemplate = (DataTemplate)this.FindResource("step5Min") as DataTemplate;
            tileMatch.HeaderTemplate = null;
            tileDemo.HeaderTemplate = (DataTemplate)this.FindResource("step4Header") as DataTemplate;
            tileDemo.ContentTemplate = null;
            tileDemo.UpdateLayout();
            tvMe.MaximizedItem = tileDemo;
        }
        /// <summary>
        /// tileLike to tileMatch
        /// </summary>
        public void Step6ToStep5()
        {
            tileLike.ContentTemplate = (DataTemplate)this.FindResource("step6Min") as DataTemplate;
            tileLike.HeaderTemplate = null;
            tileMatch.HeaderTemplate = (DataTemplate)this.FindResource("step5Header") as DataTemplate;
            tileMatch.ContentTemplate = null;
            tileMatch.UpdateLayout();
            tvMe.MaximizedItem = tileMatch;
        }
        #endregion
        #endregion
        public void Finish(DataCompareSteps dcSteps)
        {
            Window w = Core_AMS.Utilities.WPF.GetWindow(this);
            w.Close();
        }
    }
}

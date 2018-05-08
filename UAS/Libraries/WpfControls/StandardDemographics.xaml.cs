using FrameworkUAS.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace WpfControls
{
    /// <summary>
    /// Interaction logic for StandardDemographics.xaml
    /// </summary>
    public partial class StandardDemographics : UserControl, INotifyPropertyChanged
    {
        //DEPRECATED -- FilterControls now controls this.
        #region ServiceCalls
        //private FrameworkServices.ServiceClient<UAS_WS.Interface.IDeliverability> deliverabilityData = FrameworkServices.ServiceClient.Circ_DeliverabilityClient();
        //private FrameworkServices.ServiceClient<UAS_WS.Interface.IDeliverabilityMap> deliverabilityMapData = FrameworkServices.ServiceClient.Circ_DeliverabilityMapClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCodeType> catTypeData = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> catData = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCodeType> transTypeData = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> transData = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeClient();
        //private FrameworkServices.ServiceClient<UAS_WS.Interface.IQualificationSourceType> qSourceTypeData = FrameworkServices.ServiceClient.Circ_QualificationSourceTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICodeType> codeTypeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IRegion> regionData = FrameworkServices.ServiceClient.UAD_Lookup_RegionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICountry> countryData = FrameworkServices.ServiceClient.UAD_Lookup_CountryClient();
        #endregion
        #region Variables/Lists
        private Guid accessKey;
        public int lockedCount = 0;        
        private int myPub;
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodesAdd = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodesRemove = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> xactCodesAdd = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> xactCodesRemove = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catCodeTypeDisplay = new List<FrameworkUAD_Lookup.Entity.CategoryCodeType>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCodeType> xactCodeTypeDisplay = new List<FrameworkUAD_Lookup.Entity.TransactionCodeType>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCodeType> xactCodeTypeDisplaySplits = new List<FrameworkUAD_Lookup.Entity.TransactionCodeType>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodeListSplits = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodeList = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodeList = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catCodeTypeList = new List<FrameworkUAD_Lookup.Entity.CategoryCodeType>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCodeType> tCodeTypeList = new List<FrameworkUAD_Lookup.Entity.TransactionCodeType>();
        //private List<Code> QSourceList = new List<Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> codeList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList = new List<FrameworkUAD_Lookup.Entity.CodeType>();
        private List<FrameworkUAD_Lookup.Entity.Region> regionList = new List<FrameworkUAD_Lookup.Entity.Region>();
        private List<FrameworkUAD_Lookup.Entity.Country> countryList = new List<FrameworkUAD_Lookup.Entity.Country>();
        //private List<Deliverability> mediaList = new List<Deliverability>();
        private List<FrameworkUAD_Lookup.Entity.Code> qualList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> filterTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
        #endregion
        #region Properties
        private bool _isExpanded = true;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsExpanded"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region ServiceResponse
        //private FrameworkUAS.Service.Response<List<Deliverability>> deliverabilityResponse = new FrameworkUAS.Service.Response<List<Deliverability>>();
        //private FrameworkUAS.Service.Response<List<DeliverabilityMap>> deliverabilityMapResponse = new FrameworkUAS.Service.Response<List<DeliverabilityMap>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>> ccTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> ccResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> qualResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>> transTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> transResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>>();
        //private FrameworkUAS.Service.Response<List<FrameworkCirculation.Entity.QualificationSourceType>> qTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkCirculation.Entity.QualificationSourceType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>> codeTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>> regionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>> countryResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>>();
        #endregion

        public StandardDemographics(int publicationID)
        {
            InitializeComponent();

            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            myPub = publicationID;
        }

        #region Loading & Setting Controls
        private void BindAdditionalFilters()
        {
            lbCategory.ItemsSource = catCodeTypeList.OrderBy(x => x.CategoryCodeTypeID);
            lbCategory.DisplayMemberPath = "CategoryCodeTypeName";
            lbCategory.SelectedValuePath = "CategoryCodeTypeID";

            lbCatCode.ItemsSource = catCodeList;
            lbCatCode.DisplayMemberPath = "CategoryCodeName";
            lbCatCode.SelectedValuePath = "CategoryCodeID";

            lbTransaction.ItemsSource = tCodeTypeList.OrderBy(x => x.TransactionCodeTypeID);
            lbTransaction.DisplayMemberPath = "TransactionCodeTypeName";
            lbTransaction.SelectedValuePath = "TransactionCodeTypeID";

            lbTransCode.ItemsSource = transCodeList;
            lbTransCode.DisplayMemberPath = "TransactionCodeName";
            lbTransCode.SelectedValuePath = "TransactionCodeID";

            lbQSource.ItemsSource = qualList.OrderBy(x => x.DisplayOrder);
            lbQSource.DisplayMemberPath = "DisplayName";
            lbQSource.SelectedValuePath = "CodeId";

            lbState.ItemsSource = regionList.OrderBy(x => x.RegionCode);
            lbState.DisplayMemberPath = "RegionCode";
            lbState.SelectedValuePath = "RegionCode";

            countryList = countryList.Where(x => x.ShortName != " --- US AND CANADA --- " && x.ShortName != " --- INTERNATIONAL --- ").ToList();
            List<FrameworkUAD_Lookup.Entity.Country> finalCountryList = countryList.Where(x => x.ShortName == "UNITED STATES" || x.ShortName == "CANADA" || x.ShortName == "MEXICO").ToList();
            countryList = countryList.OrderBy(x => x.ShortName).ToList();
            finalCountryList.AddRange(countryList.Except(finalCountryList));

            lbCountry.ItemsSource = finalCountryList;
            lbCountry.DisplayMemberPath = "ShortName";
            lbCountry.SelectedValuePath = "CountryID";

            List<string> countries = new List<string>();
            countries = countryList.Where(x => x.Area != null && x.Area != String.Empty).OrderBy(x => x.Area).Select(x => x.Area.ToUpper().Trim()).Distinct().ToList();
            foreach (string s in countries)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi.Content = s;
                lbRegion.Items.Add(lbi);
            }

            int deliverId = 0;
            FrameworkUAD_Lookup.Entity.CodeType codeTypeEnt = codeTypeList.SingleOrDefault(x => x.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Deliver.ToString()));
            if (codeTypeEnt != null)
                deliverId = codeTypeEnt.CodeTypeId;

            lbMedia.ItemsSource = codeList.Where(c => c.CodeTypeId == deliverId).ToList();
            lbMedia.DisplayMemberPath = "DisplayName";
            lbMedia.SelectedValuePath = "CodeId";

            List<string> waveMail = new List<string>() { "All", "Is Wave Mailed", "Is Not Wave Mailed" };
            rcbWaveMail.ItemsSource = waveMail;
        }

        public void LoadData(List<FrameworkUAD_Lookup.Entity.Code> codes, List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodes, List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catCodeTypes,
                             List<FrameworkUAD_Lookup.Entity.TransactionCodeType> transCodetypes, List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodes,
                             List<FrameworkUAD_Lookup.Entity.CodeType> codeTypes, List<FrameworkUAD_Lookup.Entity.Country> countries, List<FrameworkUAD_Lookup.Entity.Region> regions)
        {
            if (codeTypes.Count == 0)
            {                
                codeTypeResponse = codeTypeData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(codeTypeResponse.Result, codeTypeResponse.Status) == true)
                    codeTypeList = codeTypeResponse.Result;
            }
            else
                codeTypeList = codeTypes;

            if (codes.Count == 0)
            {
                codeResponse = codeData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
                    codeList = codeResponse.Result;
            }
            else
                codeList = codes;

            int filterTypeID = codeTypes.Where(x => x.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Filter_Type.ToString().Replace("_", " "))).FirstOrDefault().CodeTypeId;
            filterTypes = codes.Where(x => x.CodeTypeId == filterTypeID).ToList();

            int qualTypeID = codeTypes.Where(x => x.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source.ToString().Replace("_", " "))).FirstOrDefault().CodeTypeId;
            qualList = codes.Where(x => x.CodeTypeId == qualTypeID).ToList();

            if (catCodeTypes.Count == 0)
            {
                ccTypeResponse = catTypeData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(ccTypeResponse.Result, ccTypeResponse.Status))
                    catCodeTypeList = ccTypeResponse.Result;
            }
            else
                catCodeTypeList = catCodeTypes;

            if (catCodes.Count == 0)
            {
                ccResponse = catData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(ccResponse.Result, ccResponse.Status))
                    catCodeList = ccResponse.Result.Where(x => x.IsActive == true).OrderBy(x => x.CategoryCodeValue).ToList();
            }
            else
            {
                catCodes.ForEach(x => catCodeList.Add(Core_AMS.Utilities.ObjectFunctions.DeepCopy(x)));                //catCodeList = catCodes;
                catCodeList = catCodeList.Where(x => x.IsActive == true).OrderBy(x => x.CategoryCodeValue).ToList();
            }

            foreach (FrameworkUAD_Lookup.Entity.CategoryCode cc in catCodeList)
                cc.CategoryCodeName = cc.CategoryCodeValue + "." + cc.CategoryCodeName;

            if (transCodetypes.Count == 0)
            {
                transTypeResponse = transTypeData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(transTypeResponse.Result, transTypeResponse.Status))
                    tCodeTypeList = transTypeResponse.Result;
            }
            else
                tCodeTypeList = transCodetypes;

            if (transCodes.Count == 0)
            {
                transResponse = transData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(transResponse.Result, transResponse.Status))
                    transCodeList = transResponse.Result.Where(x => x.IsActive == true).OrderBy(x => x.TransactionCodeValue).ToList();
            }
            else
            {
                transCodes.ForEach(x => transCodeList.Add(Core_AMS.Utilities.ObjectFunctions.DeepCopy(x)));
                transCodeList = transCodeList.Where(x => x.IsActive == true).OrderBy(x => x.TransactionCodeValue).ToList();
            }

            foreach (FrameworkUAD_Lookup.Entity.TransactionCode tCode in transCodeList)
                tCode.TransactionCodeName = tCode.TransactionCodeValue + "." + tCode.TransactionCodeName;

            if (regions.Count == 0)
            {
                regionResponse = regionData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(regionResponse.Result, regionResponse.Status))
                    regionList = regionResponse.Result;
            }
            else
                regionList = regions;

            if (countries.Count == 0)
            {
                countryResponse = countryData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(countryResponse.Result, countryResponse.Status))
                    countryList = countryResponse.Result;
            }
            else
                countryList = countries;

            #region Set Up Adds
            var xact = new List<int> { 10, 12, 21, 22, 23, 27, 31, 34, 38, 99 };
            var cat = new List<int> { 10, 11, 17, 18, 70, 50, 51, 52, 55, 57, 58, 61, 62, 63, 71 };

            foreach (FrameworkUAD_Lookup.Entity.CategoryCode cc in catCodeList)
            {
                if (cat.Contains(cc.CategoryCodeValue) && cc.CategoryCodeTypeID != 3 && cc.CategoryCodeTypeID != 4)
                    catCodesAdd.Add(cc);
            }

            foreach (FrameworkUAD_Lookup.Entity.TransactionCode tc in transCodeList)
            {
                if (xact.Contains(tc.TransactionCodeValue) && tc.TransactionCodeTypeID != 3 && tc.TransactionCodeTypeID != 4)
                    xactCodesAdd.Add(tc);
            }
            #endregion

            #region Set Up Removes
            var xact1 = new List<int> { 10, 12, 21, 22, 23, 27, 99 };
            var cat1 = new List<int> { 10, 11, 17, 18, 50, 51, 52, 55, 57, 58, 61, 62, 63 };

            foreach (FrameworkUAD_Lookup.Entity.CategoryCode cc in catCodeList)
            {
                if (cat1.Contains(cc.CategoryCodeValue) && cc.CategoryCodeTypeID != 3 && cc.CategoryCodeTypeID != 4)
                    catCodesRemove.Add(cc);
            }

            foreach (FrameworkUAD_Lookup.Entity.TransactionCode tc in transCodeList)
            {
                if (xact1.Contains(tc.TransactionCodeValue) && tc.TransactionCodeTypeID != 3 && tc.TransactionCodeTypeID != 4)
                    xactCodesRemove.Add(tc);
            }
            #endregion

            foreach (FrameworkUAD_Lookup.Entity.TransactionCodeType tct in tCodeTypeList)
            {
                if (tct.TransactionCodeTypeID == 1 || tct.TransactionCodeTypeID == 2)
                    xactCodeTypeDisplay.Add(tct);
            }
            foreach (FrameworkUAD_Lookup.Entity.CategoryCodeType cct in catCodeTypeList)
            {
                if (cct.CategoryCodeTypeID == 1 || cct.CategoryCodeTypeID == 2)
                    catCodeTypeDisplay.Add(cct);
            }
            foreach (FrameworkUAD_Lookup.Entity.TransactionCodeType tct in tCodeTypeList)
            {
                if (tct.TransactionCodeTypeID == 1 || tct.TransactionCodeTypeID == 3)
                    xactCodeTypeDisplaySplits.Add(tct);
            }
            foreach (FrameworkUAD_Lookup.Entity.TransactionCode tc in transCodeList)
            {
                if (tc.TransactionCodeTypeID == 1 || tc.TransactionCodeTypeID == 3)
                    transCodeListSplits.Add(tc);
            }
        }

        public void SetControls(bool setSplits = false)
        {
            lbYear.Items.Add(new ListBoxItem() { Content = "0" });
            lbYear.Items.Add(new ListBoxItem() { Content = "1" });
            lbYear.Items.Add(new ListBoxItem() { Content = "2" });
            lbYear.Items.Add(new ListBoxItem() { Content = "3" });
            lbYear.Items.Add(new ListBoxItem() { Content = "4" });
            lbYear.Items.Add(new ListBoxItem() { Content = "5" });

            BindAdditionalFilters();
            lbTransaction.SelectedItems.Add(lbTransaction.Items.GetItemAt(0));
            lbTransaction.SelectedItems.Add(lbTransaction.Items.GetItemAt(2));

            for (int i = 0; i < 5; i++)
            {
                lbCatCode.SelectedItems.Add(lbCatCode.Items.GetItemAt(i));
            }

            lbCategory.SelectedItems.Add(lbCategory.Items.GetItemAt(0));
            lbCategory.SelectedItems.Add(lbCategory.Items.GetItemAt(1));

            //GetDescendantByType(reAdditionalFilters, typeof(RadListBox));

            if (setSplits == true)
                SetTransCatCode("Splits");
            else
                SetTransCatCode("Add");
        }
        #endregion

        #region UI Events

        private void RadExpander_Loaded(object sender, RoutedEventArgs e)
        {
            RadExpander re = sender as RadExpander;
            Image img = re.FindChildByType<Image>();
            RadToggleButton header = re.Template.FindName("HeaderButton", re) as RadToggleButton;
            header.PreviewClick += (s, e2) =>
            {
                e2.Handled = true;
            };
            Ellipse innerC = re.Template.FindName("InnerCircle", re) as Ellipse;
            Ellipse outerC = re.Template.FindName("OuterCircle", re) as Ellipse;
            Ellipse normC = re.Template.FindName("NormalCircle", re) as Ellipse;
            Path arrow = re.Template.FindName("arrow", re) as Path;
            arrow.Visibility = System.Windows.Visibility.Collapsed;
            innerC.Visibility = System.Windows.Visibility.Collapsed;
            outerC.Visibility = System.Windows.Visibility.Collapsed;
            normC.Visibility = Visibility.Collapsed;
            SetLock(re, img);
        }

        private void RadExpander_MouseLeave(object sender, MouseEventArgs e)
        {
            RadExpander re = sender as RadExpander;
            re.IsExpanded = false;
        }

        private void RadExpander_MouseEnter(object sender, MouseEventArgs e)
        {
            RadExpander re = sender as RadExpander;
            re.IsExpanded = true;
        }

        private void btnImgLock_Click(object sender, RoutedEventArgs e)
        {
            RadButton btn = sender as RadButton;
            Image me = btn.FindChildByType<Image>();
            StackPanel sp = btn.Parent as StackPanel;
            RadExpander expander = sp.Parent as RadExpander;
            SetLock(expander, me);
        }

        private void btnExpand_Click(object sender, RoutedEventArgs e)
        {
            if (_isExpanded)
                this.IsExpanded = false;
            else
                this.IsExpanded = true;
        }

        private void lbTransaction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (FrameworkUAD_Lookup.Entity.TransactionCodeType tct in e.AddedItems)
            {
                foreach (FrameworkUAD_Lookup.Entity.TransactionCode tc in lbTransCode.Items)
                {
                    if (tc.TransactionCodeTypeID == tct.TransactionCodeTypeID)
                        lbTransCode.SelectedItems.Add(tc);
                }
            }
            foreach (FrameworkUAD_Lookup.Entity.TransactionCodeType tct in e.RemovedItems)
            {
                foreach (FrameworkUAD_Lookup.Entity.TransactionCode tc in lbTransCode.Items)
                {
                    if (tc.TransactionCodeTypeID == tct.TransactionCodeTypeID)
                        lbTransCode.SelectedItems.Remove(tc);
                }
            }
        }

        private void lbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (FrameworkUAD_Lookup.Entity.CategoryCodeType cct in e.AddedItems)
            {
                foreach (FrameworkUAD_Lookup.Entity.CategoryCode cc in lbCatCode.Items)
                {
                    if (cc.CategoryCodeTypeID == cct.CategoryCodeTypeID && cc.CategoryCodeValue != 70)
                        lbCatCode.SelectedItems.Add(cc);
                }
            }
            foreach (FrameworkUAD_Lookup.Entity.CategoryCodeType cct in e.RemovedItems)
            {
                foreach (FrameworkUAD_Lookup.Entity.CategoryCode cc in lbCatCode.Items)
                {
                    if (cc.CategoryCodeTypeID == cct.CategoryCodeTypeID)
                        lbCatCode.SelectedItems.Remove(cc);
                }
            }
        }

        private void lbRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach(ListBoxItem l in e.AddedItems)
            {
                foreach (FrameworkUAD_Lookup.Entity.Country c in lbCountry.Items)
                {
                    if (c.Area != null && c.Area.Trim().Equals(l.Content))
                        lbCountry.SelectedItems.Add(c);
                }
            }
            foreach (ListBoxItem l in e.RemovedItems)
            {
                foreach (FrameworkUAD_Lookup.Entity.Country c in lbCountry.Items)
                {
                    if (c.Area != null && c.Area.Trim().Equals(l.Content))
                        lbCountry.SelectedItems.Remove(c);
                }
            }
        }
        #endregion

        #region Helpers
        private void SetLock(RadExpander expander, Image me)
        {
            if (me.Tag.ToString() == "lock")
            {
                lockedCount--;
                me.Tag = "unlock";
                me.Source = new BitmapImage(new Uri(@"pack://application:,,,/ImageLibrary;component/Images/32/unlock-32.png", UriKind.Absolute));
                me.ToolTip = "Click to keep open.";
                expander.MouseLeave += RadExpander_MouseLeave;
            }
            else if (me.Tag.ToString() == "unlock")
            {
                lockedCount++;
                me.Tag = "lock";
                me.Source = new BitmapImage(new Uri(@"pack://application:,,,/ImageLibrary;component/Images/32/lock-32.png", UriKind.Absolute));
                me.ToolTip = "Click to unlock.";
                expander.MouseLeave -= RadExpander_MouseLeave;
            }
            if (lockedCount == 2)
            {
                ExpandCollapse(true);
            }
            else
                ExpandCollapse(false);
        }

        private void ExpandCollapse(bool expand)
        {
            if (expand)
            {
                //txtExpandCollapseFilters.Text = "-";
                //txtExpandCollapseFilters.FontSize = 24;
                //txtExpandCollapseFilters.FontWeight = FontWeights.Bold;
                //txtExpandCollapseFilters.ToolTip = "Collapse and Unlock All";
                //txtExpandCollapseFilters.MouseUp -= txtExpandFilters_MouseUp;
                //txtExpandCollapseFilters.MouseUp += txtCollapseFilters_MouseUp;
            }
            else
            {
                //txtExpandCollapseFilters.Text = "+";
                //txtExpandCollapseFilters.ToolTip = "Expand and Lock All";
                //txtExpandCollapseFilters.FontSize = 20;
                //txtExpandCollapseFilters.FontWeight = FontWeights.SemiBold;
                //txtExpandCollapseFilters.MouseUp -= txtCollapseFilters_MouseUp;
                //txtExpandCollapseFilters.MouseUp += txtExpandFilters_MouseUp;
            }
        }

        public List<Helpers.FilterOperations.FilterDetailContainer> GetSelection()
        {
            List<ListBox> lbs = new List<ListBox>();
            lbs = this.ChildrenOfType<ListBox>().ToList();
            List<Helpers.FilterOperations.FilterDetailContainer> myDetailContainers = new List<Helpers.FilterOperations.FilterDetailContainer>();
            foreach(ListBox lb in lbs)
            {
                if(lb.SelectedItems.Count > 0)
                {
                    Helpers.FilterOperations.FilterDetailContainer fdc = new Helpers.FilterOperations.FilterDetailContainer();
                    fdc.FilterDetail.FilterField = lb.Name;
                    fdc.FilterDetail.FilterTypeId = filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.Standard.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault();
                    fdc.FilterDetail.SearchCondition = "EQUAL";
                    if (lb.Name == "lbYear")
                        fdc.FilterDetail.FilterObjectType = "Year";
                    else if (lb.Name == "lbRegion")
                        fdc.FilterDetail.FilterObjectType = "Region";
                    else
                        fdc.FilterDetail.FilterObjectType = lb.SelectedValuePath.ToString();

                    if (lb.Name == "lbYear" || lb.Name == "lbRegion")
                    {
                        foreach (ListBoxItem i in lb.SelectedItems)
                        {
                            FrameworkUAS.Entity.FilterDetailSelectedValue fdsv = new FilterDetailSelectedValue();
                            fdsv.SelectedValue = i.Content.ToString();
                            fdc.Values.Add(fdsv);
                        }
                    }
                    else
                    {
                        foreach (var i in lb.SelectedItems)
                        {
                            Type t2 = i.GetType();
                            PropertyInfo p = t2.GetProperty(lb.SelectedValuePath); //The ListBoxes can have many different objects in them, so we dynamically find the type at runtime.
                            object v = p.GetValue(i, null);
                            FrameworkUAS.Entity.FilterDetailSelectedValue fdsv = new FilterDetailSelectedValue();
                            fdsv.SelectedValue = v.ToString();
                            fdc.Values.Add(fdsv);
                        }
                    }
                    myDetailContainers.Add(fdc);
                }
            }
            
            foreach(RadComboBox rcb in this.ChildrenOfType<RadComboBox>())
            {
                if (rcb.SelectedItem != null && rcb.Text != "")
                {
                    Helpers.FilterOperations.FilterDetailContainer fdc = new Helpers.FilterOperations.FilterDetailContainer();
                    fdc.FilterDetail.FilterField = rcb.Name;
                    fdc.FilterDetail.FilterTypeId = filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.Standard.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault();
                    fdc.FilterDetail.SearchCondition = "EQUAL";
                    fdc.FilterDetail.FilterObjectType = rcb.Tag.ToString();

                    FrameworkUAS.Entity.FilterDetailSelectedValue fdsv = new FilterDetailSelectedValue();
                    fdsv.SelectedValue = rcb.SelectionBoxItem.ToString();
                    fdc.Values.Add(fdsv);
                    myDetailContainers.Add(fdc);
                }
            }

            foreach(RadDatePicker rdp in this.ChildrenOfType<RadDatePicker>())
            {
                if (rdp.SelectedDate != null)
                {
                    Helpers.FilterOperations.FilterDetailContainer fdc = new Helpers.FilterOperations.FilterDetailContainer();
                    fdc.FilterDetail.FilterField = rdp.Name;
                    fdc.FilterDetail.FilterTypeId = filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.Standard.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault();
                    fdc.FilterDetail.SearchCondition = "EQUAL";
                    fdc.FilterDetail.FilterObjectType = rdp.Tag.ToString();

                    FrameworkUAS.Entity.FilterDetailSelectedValue fdsv = new FilterDetailSelectedValue();
                    DateTime dt = rdp.SelectedDate ?? DateTime.Now;
                    fdsv.SelectedValue = dt.Date.ToString("yyyy-MM-dd HH:mm:ss");
                    fdc.Values.Add(fdsv);
                    myDetailContainers.Add(fdc);
                }
            }

            return myDetailContainers;
        }

        public void SetTransCatCode(string addOrRemove)
        {
            var xactHighLight = new List<int> { 10, 12, 21, 22, 23, 27, 99 };

            lbTransCode.SelectedItems.Clear();
            lbCatCode.SelectedItems.Clear();
            lbTransaction.SelectedItems.Clear();
            lbCategory.SelectedItems.Clear();

            lbTransaction.ItemsSource = xactCodeTypeDisplay;
            lbCategory.ItemsSource = catCodeTypeDisplay;

            if (addOrRemove == "Add")
            {
                lbCatCode.ItemsSource = null;

                lbTransCode.ItemsSource = xactCodesAdd;
                lbCatCode.ItemsSource = catCodesAdd;
                lbCatCode.DisplayMemberPath = "CategoryCodeName";
                lbCatCode.SelectedValuePath = "CategoryCodeID";

                //foreach (FrameworkUAD_Lookup.Entity.CategoryCodeType cct in lbCategory.Items)
                //{
                //    if (cct.CategoryCodeTypeID == 1)
                //        lbCategory.SelectedItems.Add(cct);
                //}

                foreach (FrameworkUAD_Lookup.Entity.TransactionCode tc in lbTransCode.Items)
                {
                    if(xactHighLight.Contains(tc.TransactionCodeValue))
                    {
                        lbTransCode.SelectedItems.Add(tc);
                    }
                }

                foreach (FrameworkUAD_Lookup.Entity.TransactionCodeType tct in lbTransaction.Items)
                {
                    if (tct.TransactionCodeTypeID == 1)
                        lbTransaction.SelectedItems.Add(tct);
                }

                int[] cats = { 70 };

                foreach (FrameworkUAD_Lookup.Entity.CategoryCode cc in lbCatCode.Items)
                {
                    if(cats.Contains(cc.CategoryCodeValue))
                        lbCatCode.SelectedItems.Add(cc);
                    //if (lbCatCode.SelectedItems.Contains(cc))
                    //{
                    //    if (cats.Contains(cc.CategoryCodeValue))
                    //        lbCatCode.SelectedItems.Remove(cc);
                    //}
                }

                lbCatCode.ScrollIntoView(lbCatCode.SelectedItem);
            }
            else if(addOrRemove == "Remove")
            {
                lbCatCode.ItemsSource = catCodesRemove;
                lbTransCode.ItemsSource = xactCodesRemove;
                
                lbCatCode.DisplayMemberPath = "CategoryCodeName";
                lbCatCode.SelectedValuePath = "CategoryCodeID";

                //foreach (FrameworkUAD_Lookup.Entity.CategoryCode cc in lbCatCode.Items)
                //{
                //    if(cc.CategoryCodeValue == 10)
                //    {
                //        lbCatCode.SelectedItems.Add(cc);
                //        lbCatCode.ScrollIntoView(cc);
                //    }
                //}
                foreach (FrameworkUAD_Lookup.Entity.CategoryCodeType cct in lbCategory.Items)
                {
                    if (cct.CategoryCodeTypeID == 1)
                        lbCategory.SelectedItems.Add(cct);
                }

                lbTransCode.DisplayMemberPath = "TransactionCodeName";
                lbTransCode.SelectedValuePath = "TransactionCodeID";

                foreach (FrameworkUAD_Lookup.Entity.TransactionCode tc in lbTransCode.Items)
                {
                    if (xactHighLight.Contains(tc.TransactionCodeValue))
                        lbTransCode.SelectedItems.Add(tc);
                }

                foreach (FrameworkUAD_Lookup.Entity.TransactionCodeType tct in lbTransaction.Items)
                {
                    if (tct.TransactionCodeTypeID == 1)
                        lbTransaction.SelectedItems.Add(tct);
                }

                int[] cats = { 70, 71 };

                foreach (FrameworkUAD_Lookup.Entity.CategoryCode cc in lbCatCode.Items)
                {
                    if (lbCatCode.SelectedItems.Contains(cc))
                    {
                        if (cats.Contains(cc.CategoryCodeValue))
                            lbCatCode.SelectedItems.Remove(cc);
                    }
                }

                lbCatCode.ScrollIntoView(lbCatCode.Items[0]);
            }
            else if(addOrRemove == "Splits")
            {
                lbCatCode.ItemsSource = catCodeList.Where(x=> x.CategoryCodeValue != 71 && x.CategoryCodeValue != 70);
                //lbTransCode.ItemsSource = transCodeList;
                lbTransCode.ItemsSource = transCodeListSplits;
                lbCategory.ItemsSource = catCodeTypeList;
                //lbTransaction.ItemsSource = tCodeTypeList;
                lbTransaction.ItemsSource = xactCodeTypeDisplaySplits;
            }
            else if (addOrRemove == "Circulation Explorer")
            {
                lbCatCode.ItemsSource = catCodeList;
                lbTransCode.ItemsSource = transCodeList;
                lbCategory.ItemsSource = catCodeTypeList;
                lbTransaction.ItemsSource = tCodeTypeList;

                foreach (FrameworkUAD_Lookup.Entity.TransactionCodeType tct in lbTransaction.Items)
                {
                    if (tct.TransactionCodeTypeID == 1 || tct.TransactionCodeTypeID == 3)
                        lbTransaction.SelectedItems.Add(tct);
                }

                foreach (FrameworkUAD_Lookup.Entity.CategoryCodeType cct in lbCategory.Items)
                {
                    if (cct.CategoryCodeTypeID == 1 || cct.CategoryCodeTypeID == 3)
                        lbCategory.SelectedItems.Add(cct);
                }

                int[] cats = { 70,71 };

                foreach (FrameworkUAD_Lookup.Entity.CategoryCode cc in lbCatCode.Items)
                {
                    if (lbCatCode.SelectedItems.Contains(cc))
                    {
                        if (cats.Contains(cc.CategoryCodeValue))
                            lbCatCode.SelectedItems.Remove(cc);
                    }
                }
            }
        }

        public static Visual GetDescendantByType(Visual element, Type type)
        {
            if (element == null) return null;
            if (element.GetType() == type) return element;
            Visual foundElement = null;
            if (element is FrameworkElement)
                (element as FrameworkElement).ApplyTemplate();
            for (int i = 0;
                i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type);
                if (foundElement != null)
                    break;
            }
            return foundElement;
        }

        #endregion
    }
}

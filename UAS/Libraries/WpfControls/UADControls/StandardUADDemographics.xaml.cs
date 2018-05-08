using FrameworkUAS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace WpfControls.UADControls
{
    /// <summary>
    /// Interaction logic for StandardUADDemographics.xaml
    /// </summary>
    public partial class StandardUADDemographics : UserControl
    {

        #region ServiceCalls
        //private FrameworkServices.ServiceClient<UAS_WS.Interface.IDeliverability> deliverabilityData = FrameworkServices.ServiceClient.Circ_DeliverabilityClient();
        //private FrameworkServices.ServiceClient<UAS_WS.Interface.IDeliverabilityMap> deliverabilityMapData = FrameworkServices.ServiceClient.Circ_DeliverabilityMapClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCodeType> catTypeData = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> catData = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeClient();
        //private FrameworkServices.ServiceClient<UAS_WS.Interface.IQualificationSource> qualData = FrameworkServices.ServiceClient.UAS_QualificationSourceClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCodeType> transTypeData = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> transData = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeClient();
        //private FrameworkServices.ServiceClient<UAS_WS.Interface.IQualificationSourceType> qSourceTypeData = FrameworkServices.ServiceClient.Circ_QualificationSourceTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICodeType> codeTypeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IRegion> regionData = FrameworkServices.ServiceClient.UAD_Lookup_RegionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IRegionGroup> regionGroupData = FrameworkServices.ServiceClient.UAD_Lookup_RegionGroupClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICountry> countryData = FrameworkServices.ServiceClient.UAD_Lookup_CountryClient();
        #endregion
        #region Variables/Lists
        private Guid accessKey;
        public int lockedCount = 0;        
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodesAdd = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodesRemove = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> xactCodesAdd = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> xactCodesRemove = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catCodeTypeDisplay = new List<FrameworkUAD_Lookup.Entity.CategoryCodeType>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCodeType> xactCodeTypeDisplay = new List<FrameworkUAD_Lookup.Entity.TransactionCodeType>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodeList = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodeList = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catCodeTypeList = new List<FrameworkUAD_Lookup.Entity.CategoryCodeType>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCodeType> tCodeTypeList = new List<FrameworkUAD_Lookup.Entity.TransactionCodeType>();
        //private List<Code> QSourceList = new List<Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> codeList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList = new List<FrameworkUAD_Lookup.Entity.CodeType>();
        private List<FrameworkUAD_Lookup.Entity.Region> regionList = new List<FrameworkUAD_Lookup.Entity.Region>();
        private List<FrameworkUAD_Lookup.Entity.RegionGroup> regionGroupList = new List<FrameworkUAD_Lookup.Entity.RegionGroup>();
        private List<FrameworkUAD_Lookup.Entity.Country> countryList = new List<FrameworkUAD_Lookup.Entity.Country>();
        //private List<Deliverability> mediaList = new List<Deliverability>();
        private List<FrameworkUAD_Lookup.Entity.Code> qualList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> filterTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
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
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>> regionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.RegionGroup>> regionGroupResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.RegionGroup>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>> countryResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>> codeTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>>();
        #endregion

        public StandardUADDemographics()
        {
            InitializeComponent();

            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
        }

        #region Loading
        private void BindAdditionalFilters()
        {
            lbCatCode.ItemsSource = catCodeList;
            lbCatCode.DisplayMemberPath = "CategoryCodeName";
            lbCatCode.SelectedValuePath = "CategoryCodeValue";

            lbTransaction.ItemsSource = tCodeTypeList.OrderBy(x => x.TransactionCodeTypeID);
            lbTransaction.DisplayMemberPath = "TransactionCodeTypeName";
            lbTransaction.SelectedValuePath = "TransactionCodeTypeID";

            lbQSource.ItemsSource = qualList.OrderBy(x => x.DisplayOrder);
            lbQSource.DisplayMemberPath = "DisplayName";
            lbQSource.SelectedValuePath = "QSourceID";

            lbState.ItemsSource = regionList.Where(x=> x.RegionGroupID < 10 && x.RegionGroupID > 0).OrderBy(x => x.RegionCode);
            lbState.DisplayMemberPath = "RegionCode";
            lbState.SelectedValuePath = "RegionID";

            lbGeoCode.ItemsSource = regionGroupList;
            lbGeoCode.DisplayMemberPath = "RegionGroupName";
            lbGeoCode.SelectedValuePath = "RegionGroupID";

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

            int deliverId = codeTypeList.SingleOrDefault(x => x.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Deliver.ToString())).CodeTypeId;

            lbMedia.ItemsSource = codeList.Where(f => f.CodeTypeId == deliverId).ToList();
            lbMedia.DisplayMemberPath = "DisplayName";
            lbMedia.SelectedValuePath = "CodeId";
        }

        public void LoadData()
        {
            codeResponse = codeData.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Filter_Type);
            if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
            {
                filterTypes = codeResponse.Result;
            }

            ccTypeResponse = catTypeData.Proxy.Select(accessKey);
            if (Helpers.Common.CheckResponse(ccTypeResponse.Result, ccTypeResponse.Status))
            {
                catCodeTypeList = ccTypeResponse.Result;
            }

            ccResponse = catData.Proxy.Select(accessKey);
            if (Helpers.Common.CheckResponse(ccResponse.Result, ccResponse.Status))
            {
                catCodeList = ccResponse.Result.Where(x => x.IsActive == true).OrderBy(x => x.CategoryCodeValue).ToList();

                foreach (FrameworkUAD_Lookup.Entity.CategoryCode cc in catCodeList)
                {
                    cc.CategoryCodeName = cc.CategoryCodeValue + "-" + cc.CategoryCodeName;
                }
            }

            transTypeResponse = transTypeData.Proxy.Select(accessKey);
            if (Helpers.Common.CheckResponse(transTypeResponse.Result, transTypeResponse.Status))
            {
                tCodeTypeList = transTypeResponse.Result;
            }

            transResponse = transData.Proxy.Select(accessKey);
            if (Helpers.Common.CheckResponse(transResponse.Result, transResponse.Status))
            {
                transCodeList = transResponse.Result.Where(x => x.IsActive == true).OrderBy(x => x.TransactionCodeValue).ToList();

                foreach (FrameworkUAD_Lookup.Entity.TransactionCode tCode in transCodeList)
                {
                    tCode.TransactionCodeName = tCode.TransactionCodeValue + "." + tCode.TransactionCodeName;
                }
            }

            regionResponse = regionData.Proxy.Select(accessKey);
            if (Helpers.Common.CheckResponse(regionResponse.Result, regionResponse.Status))
            {
                regionList = regionResponse.Result;
            }

            regionGroupResponse = regionGroupData.Proxy.Select(accessKey);
            if(Helpers.Common.CheckResponse(regionGroupResponse.Result, regionGroupResponse.Status))
            {
                regionGroupList = regionGroupResponse.Result;
            }

            countryResponse = countryData.Proxy.Select(accessKey);
            if (Helpers.Common.CheckResponse(countryResponse.Result, countryResponse.Status))
            {
                countryList = countryResponse.Result;
            }

            //deliverabilityResponse = deliverabilityData.Proxy.Select(accessKey);

            //if (Helpers.Common.CheckResponse(deliverabilityResponse.Result, deliverabilityResponse.Status))
            //{
            //    mediaList = deliverabilityResponse.Result;
            //}

            codeTypeResponse = codeTypeData.Proxy.Select(accessKey);
            if (Helpers.Common.CheckResponse(codeTypeResponse.Result, codeTypeResponse.Status) == true)
            {
                codeTypeList = codeTypeResponse.Result;
            }

            codeResponse = codeData.Proxy.Select(accessKey);
            if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status) == true)
            {
                codeList = codeResponse.Result;
            }

            qualResponse = codeData.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source);
            if (Helpers.Common.CheckResponse(qualResponse.Result, qualResponse.Status))
            {
                qualList = qualResponse.Result.Where(x => x.IsActive == true).ToList();
            }

            #region Set Up Adds
            var xact = new List<int> { 10, 12, 21, 22, 23, 27, 31, 34, 38, 99 };
            var cat = new List<int> { 10, 11, 17, 70, 50, 51, 52, 55, 57, 58, 61, 62, 63, 71 };

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
            var cat1 = new List<int> { 10, 11, 17, 50, 51, 52, 55, 57, 58, 61, 62, 63 };

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
        }

        public void SetControls(bool setSplits = false)
        {
            BindAdditionalFilters();
            lbTransaction.SelectedItems.Add(lbTransaction.Items.GetItemAt(0));
            lbTransaction.SelectedItems.Add(lbTransaction.Items.GetItemAt(2));

            for (int i = 0; i < 5; i++)
            {
                lbCatCode.SelectedItems.Add(lbCatCode.Items.GetItemAt(i));
            }

            GetDescendantByType(reAdditionalFilters, typeof(RadListBox));
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

            if (re.Name == "rePermissions")
                grdPermission.Visibility = Visibility.Collapsed;
            else if (re.Name == "reContactFields")
                grdContactFields.Visibility = Visibility.Collapsed;
        }

        private void RadExpander_MouseEnter(object sender, MouseEventArgs e)
        {
            RadExpander re = sender as RadExpander;
            if (re.Name == "rePermissions")
                grdPermission.Visibility = Visibility.Visible;
            else if (re.Name == "reContactFields")
                grdContactFields.Visibility = Visibility.Visible;
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

        private void txtCollapseFilters_MouseUp(object sender, MouseButtonEventArgs e)
        {
            lockedCount = 0;
            ExpandCollapse(false);
            foreach (RadExpander re in this.ChildrenOfType<RadExpander>())
            {
                re.IsExpanded = false;
                StackPanel sp = re.Header as StackPanel;
                Image img = sp.FindChildByType<Image>();
                if (img.Tag.ToString() == "lock")
                {
                    img.Tag = "unlock";
                    img.Source = new BitmapImage(new Uri(@"pack://application:,,,/ImageLibrary;component/Images/32/unlock-32.png", UriKind.Absolute));
                    img.ToolTip = "Click to keep open.";
                    re.MouseLeave += RadExpander_MouseLeave;
                }
            }
            e.Handled = true;
        }

        private void txtExpandFilters_MouseUp(object sender, MouseButtonEventArgs e)
        {
            lockedCount = 3;
            ExpandCollapse(true);
            foreach (RadExpander re in this.ChildrenOfType<RadExpander>())
            {
                re.IsExpanded = true;
                StackPanel sp = re.Header as StackPanel;
                Image img = sp.FindChildByType<Image>();
                if (img.Tag.ToString() == "unlock")
                {
                    img.Tag = "lock";
                    img.Source = new BitmapImage(new Uri(@"pack://application:,,,/ImageLibrary;component/Images/32/lock-32.png", UriKind.Absolute));
                    img.ToolTip = "Click to unlock.";
                    re.MouseLeave -= RadExpander_MouseLeave;
                }
            }
            e.Handled = true;
        }

        private void lbGeoCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //RadListBox rlb = sender as RadListBox;
            foreach (FrameworkUAD_Lookup.Entity.RegionGroup rg in e.AddedItems)
            {
                foreach (FrameworkUAD_Lookup.Entity.Region r in lbState.Items)
                {
                    if (r.RegionGroupID == rg.RegionGroupID)
                        lbState.SelectedItems.Add(r);
                }
            }
            foreach (FrameworkUAD_Lookup.Entity.RegionGroup rg in e.RemovedItems)
            {
                foreach (FrameworkUAD_Lookup.Entity.Region r in lbState.Items)
                {
                    if (r.RegionGroupID == rg.RegionGroupID)
                        lbState.SelectedItems.Remove(r);
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
            if (lockedCount == 3)
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
                txtExpandFilters.Visibility = Visibility.Collapsed;
                txtCollapseFilters.Visibility = Visibility.Visible;
                //txtExpandCollapseFilters.Text = "-";
                //txtExpandCollapseFilters.FontSize = 24;
                //txtExpandCollapseFilters.FontWeight = FontWeights.Bold;
                //txtExpandCollapseFilters.ToolTip = "Collapse and Unlock All";
                //txtExpandCollapseFilters.MouseUp -= txtExpandFilters_MouseUp;
                //txtExpandCollapseFilters.MouseUp += txtCollapseFilters_MouseUp;
                grdContactFields.Visibility = Visibility.Visible;
                grdPermission.Visibility = Visibility.Visible;
            }
            else
            {
                txtCollapseFilters.Visibility = Visibility.Collapsed;
                txtExpandFilters.Visibility = Visibility.Visible;
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
            int filterTypeID = filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.Standard.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault();
            List<ListBox> lbs = new List<ListBox>();
            lbs = this.ChildrenOfType<ListBox>().ToList();
            List<Helpers.FilterOperations.FilterDetailContainer> myDetailContainers = new List<Helpers.FilterOperations.FilterDetailContainer>();
            foreach(ListBox lb in lbs)
            {
                if(lb.SelectedItems.Count > 0)
                {
                    Helpers.FilterOperations.FilterDetailContainer fdc = new Helpers.FilterOperations.FilterDetailContainer();
                    fdc.FilterDetail.FilterField = lb.Name;
                    fdc.FilterDetail.FilterTypeId = filterTypeID;
                    fdc.FilterDetail.SearchCondition = "EQUAL";
                    if (lb.Name == "lbYear")
                        fdc.FilterDetail.FilterObjectType = "Year";
                    else if (lb.Name == "lbRegion")
                        fdc.FilterDetail.FilterObjectType = "Region";
                    else
                        fdc.FilterDetail.FilterObjectType = lb.SelectedValuePath.ToString();

                    if (fdc.FilterDetail.FilterObjectType == "CategoryCodeValue")
                        fdc.FilterDetail.FilterObjectType = "CategoryCodeID";

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
                    fdc.FilterDetail.FilterTypeId = filterTypeID;
                    fdc.FilterDetail.SearchCondition = "EQUAL";
                    fdc.FilterDetail.FilterObjectType = rcb.Tag.ToString();

                    FrameworkUAS.Entity.FilterDetailSelectedValue fdsv = new FilterDetailSelectedValue();
                    fdsv.SelectedValue = rcb.SelectionBoxItem.ToString();
                    fdc.Values.Add(fdsv);
                    myDetailContainers.Add(fdc);
                }
            }

            foreach (TextBox tbx in this.ChildrenOfType<TextBox>())
            {
                if (tbx.Text != null && tbx.Text != string.Empty)
                {
                    Helpers.FilterOperations.FilterDetailContainer fdc = new Helpers.FilterOperations.FilterDetailContainer();
                    fdc.FilterDetail.FilterField = tbx.Name;
                    fdc.FilterDetail.FilterTypeId = filterTypeID;
                    fdc.FilterDetail.SearchCondition = "EQUAL";
                    fdc.FilterDetail.FilterObjectType = tbx.Tag.ToString();

                    FrameworkUAS.Entity.FilterDetailSelectedValue fdsv = new FilterDetailSelectedValue();
                    fdsv.SelectedValue = tbx.Text.ToString();
                    fdc.Values.Add(fdsv);
                    myDetailContainers.Add(fdc);
                }
            }

            return myDetailContainers;
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

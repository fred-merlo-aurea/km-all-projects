using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace WpfControls.UADControls
{
    /// <summary>
    /// Interaction logic for DynamicUADProductDemographics.xaml
    /// </summary>
    public partial class DynamicUADProductDemographics : UserControl
    {
        #region ServiceCalls
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IBrand> brandData = FrameworkServices.ServiceClient.UAD_BrandClient();
        //private FrameworkServices.ServiceClient<UAD_WS.Interface.IBrandProductMap> brandProductMapData = FrameworkServices.ServiceClient.UAD_BrandProductMapClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> responseGroupData = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
        //private FrameworkServices.ServiceClient<UAD_WS.Interface.IResponse> responseData = FrameworkServices.ServiceClient.UAD_ResponseClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ICodeSheet> codeSheetData = FrameworkServices.ServiceClient.UAD_CodeSheetClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> clientData = FrameworkServices.ServiceClient.UAS_ClientClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IMasterGroup> masterGroupData = FrameworkServices.ServiceClient.UAD_MasterGroupClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IMasterCodeSheet> masterCodeSheetData = FrameworkServices.ServiceClient.UAD_MasterCodeSheetClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IMarket> marketData = FrameworkServices.ServiceClient.UAD_MarketClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productData = FrameworkServices.ServiceClient.UAD_ProductClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProductTypes> prodTypeData = FrameworkServices.ServiceClient.UAD_ProductTypesClient();
        #endregion
        #region Variables/Lists
        private List<FrameworkUAD.Entity.Brand> brandList = new List<FrameworkUAD.Entity.Brand>();
        //private List<FrameworkUAD.Entity.BrandProductMap> brandProductMapList = new List<FrameworkUAD.Entity.BrandProductMap>();
        private List<FrameworkUAD.Entity.ResponseGroup> responseGroupList = new List<FrameworkUAD.Entity.ResponseGroup>();
        //private List<FrameworkUAD.Entity.Response> responseList = new List<FrameworkUAD.Entity.Response>();
        private List<FrameworkUAD.Entity.CodeSheet> codeSheetList = new List<FrameworkUAD.Entity.CodeSheet>();
        private List<FrameworkUAD.Entity.MasterGroup> masterGroupList = new List<FrameworkUAD.Entity.MasterGroup>();
        private List<FrameworkUAD.Entity.MasterCodeSheet> masterCodeSheetList = new List<FrameworkUAD.Entity.MasterCodeSheet>();
        private List<FrameworkUAD.Entity.Market> marketList = new List<FrameworkUAD.Entity.Market>();
        private List<FrameworkUAD.Entity.ProductTypes> prodTypesList = new List<FrameworkUAD.Entity.ProductTypes>();
        private List<FrameworkUAD.Entity.Product> productList = new List<FrameworkUAD.Entity.Product>();
        private Dictionary<int, List<FrameworkUAD.Entity.ResponseGroup>> responseGroupDictionary = new Dictionary<int, List<FrameworkUAD.Entity.ResponseGroup>>();
        private List<FrameworkUAD_Lookup.Entity.Code> filterTypes = new List<FrameworkUAD_Lookup.Entity.Code>();

        public int openItems = 0; //This dynamically sets the Expand/Collapse text block if you manually expand all items.
        public int ProductID;
        public int BrandID;
        private Guid accessKey;
        KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
        #endregion
        #region ServiceResponses
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Brand>> brandResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Brand>>();
        //private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.BrandProductMap>> brandProductMapResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.BrandProductMap>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> clientResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> productResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        //private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Response>> rResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Response>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>> csResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> rGroupResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterGroup>> masterGroupResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterGroup>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Market>> marketResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Market>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterCodeSheet>> masterCodeSheetResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterCodeSheet>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductTypes>> prodTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductTypes>>();
        #endregion

        public DynamicUADProductDemographics()
        {
            InitializeComponent();

            myClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
        }

        #region Loading
        public void LoadData()
        {
            if (myClient != null)
            {
                productResponse = productData.Proxy.Select(accessKey, myClient.ClientConnections);
                if (Helpers.Common.CheckResponse(productResponse.Result, productResponse.Status))
                {
                    productList = productResponse.Result;
                }

                codeResponse = codeData.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Filter_Type);
                if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
                {
                    filterTypes = codeResponse.Result;
                }

                marketResponse = marketData.Proxy.Select(accessKey, myClient.ClientConnections);
                if (Helpers.Common.CheckResponse(marketResponse.Result, marketResponse.Status))
                {
                    marketList = marketResponse.Result;
                }

                brandResponse = brandData.Proxy.Select(accessKey, myClient.ClientConnections);
                if (Helpers.Common.CheckResponse(brandResponse.Result, brandResponse.Status))
                {
                    brandList = brandResponse.Result;
                    brandList.Insert(0, new FrameworkUAD.Entity.Brand() { BrandName = "All Products", BrandID = 0});
                }

                csResponse = codeSheetData.Proxy.Select(accessKey, myClient.ClientConnections);
                if (Helpers.Common.CheckResponse(csResponse.Result, csResponse.Status))
                    codeSheetList = csResponse.Result.Where(x => x.IsActive == true).ToList();
            }
        }

        public void SetControls()
        {
            rcbBrands.ItemsSource = brandList;
            rcbBrands.SelectedValuePath = "BrandID";
            rcbBrands.DisplayMemberPath = "BrandName";
            rcbBrands.SelectedIndex = 0;
            rcbProducts.SelectedValuePath = "PubID";
            rcbProducts.DisplayMemberPath = "PubCode";
            rcbProducts.SelectedIndex = 0;
            #region Set Report Filters
            //Style s = this.FindResource("RadPanelBarItemStyle1") as Style;

            //foreach (FrameworkUAD.Entity.ResponseGroup rt in responseGroupList)
            //{
            //    RadPanelBarItem p = new RadPanelBarItem();
            //    p.Header = rt.DisplayName;
            //    p.Tag = rt.ResponseGroupID;
            //    p.Expanded += (sender, e) =>
            //    {
            //        openItems++;
            //        if (openItems == rpbReportFilters.Items.Count)
            //            ExpandCollapse(false);
            //        else
            //            ExpandCollapse(true);
            //    };
            //    p.Collapsed += (sender, e) =>
            //    {
            //        openItems--;
            //        if (openItems == rpbReportFilters.Items.Count)
            //            ExpandCollapse(false);
            //        else
            //            ExpandCollapse(true);
            //    };
            //    p.Background = new SolidColorBrush(Colors.White);
            //    p.Style = s;
            //    p.MaxHeight = 100;
            //    rpbReportFilters.Items.Add(p);
            //}
            //ToolTip t = this.FindResource("panelBarToolTip") as ToolTip;
            //foreach (RadPanelBarItem parent in rpbReportFilters.Items)
            //{
            //    List<FrameworkUAD.Entity.CodeSheet> codes = codeSheetList.Where(x => x.ResponseGroupID == (int)parent.Tag).OrderBy(x => x.ResponseDesc).ToList();
            //    List<FrameworkUAD.Entity.CodeSheet> displayValid = codes.Where(x => x.DisplayOrder > 0 && x.DisplayOrder != null).OrderBy(x => x.DisplayOrder).ToList();
            //    List<FrameworkUAD.Entity.CodeSheet> displayInvalid = codes.Except(displayValid)
            //    .OrderBy(s2 =>
            //    {
            //        int i = 0;
            //        return int.TryParse(s2.ResponseValue, out i) ? i : int.MaxValue;
            //    })
            //    .ThenBy(s2 => s2.ResponseValue)
            //    .ToList();
            //    codes.Clear();
            //    codes.AddRange(displayValid);
            //    codes.AddRange(displayInvalid);
            //    foreach (FrameworkUAD.Entity.CodeSheet r in codes)
            //    {
            //        RadPanelBarItem child = new RadPanelBarItem();
            //        child.Header = r.ResponseValue + ". " + r.ResponseDesc;
            //        child.Style = s;
            //        child.ToolTip = t;
            //        child.Name = "_" + r.CodeSheetID.ToString();
            //        parent.Items.Add(child);
            //    }
            //}
            #endregion
        }
        #endregion

        #region UI Events
        private void txtExpandCollapse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            foreach (RadPanelBarItem rpbItem in rpbReportFilters.Items)
            {
                rpbItem.IsExpanded = true;
            }
            openItems = rpbReportFilters.Items.Count;
            ExpandCollapse(false);
        }

        private void txtCollapse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            foreach (RadPanelBarItem rpbItem in rpbReportFilters.Items)
            {
                rpbItem.IsExpanded = false;
            }
            openItems = 0;
            ExpandCollapse(true);
        }

        private void txtExpandCollapseFilters_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock txt = sender as TextBlock;

            if (txt.Text == "+")
            {
                txt.Text = "-";
                txt.ToolTip = "Hide";
                grdFilters.Visibility = Visibility.Visible;
            }
            else
            {
                txt.Text = "+";
                txt.ToolTip = "Expand";
                grdFilters.Visibility = Visibility.Collapsed;
            }
        }

        private void rcbProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadComboBox rcb = sender as RadComboBox;
            int prodID = -1;
            if (rcb.SelectedItem != null)
            {
                FrameworkUAD.Entity.Product prod = rcb.SelectedItem as FrameworkUAD.Entity.Product;
                Style s = this.FindResource("RadPanelBarItemStyle1") as Style;
                //int.TryParse(rcb.SelectedValue.ToString(), out prodID);
                ProductID = prod.PubID;
                prodID = ProductID;

                if (!responseGroupDictionary.ContainsKey(prodID))
                {
                    rGroupResponse = responseGroupData.Proxy.Select(accessKey, myClient.ClientConnections, prodID);
                    if (Helpers.Common.CheckResponse(rGroupResponse.Result, rGroupResponse.Status))
                    {
                        responseGroupList = rGroupResponse.Result;
                        responseGroupDictionary[prodID] = responseGroupList;
                    }
                }
                else
                {
                    responseGroupList = responseGroupDictionary[prodID];
                }

                rpbReportFilters.Items.Clear();

                foreach (FrameworkUAD.Entity.ResponseGroup rt in responseGroupList.Where(x => x.PubID == prodID))
                {
                    RadPanelBarItem p = new RadPanelBarItem();
                    p.Header = rt.DisplayName;
                    p.Tag = rt.ResponseGroupID;
                    p.Expanded += (sender2, e2) =>
                    {
                        openItems++;
                        if (openItems == rpbReportFilters.Items.Count)
                            ExpandCollapse(false);
                        else
                            ExpandCollapse(true);
                    };
                    p.Collapsed += (sender2, e2) =>
                    {
                        openItems--;
                        if (openItems == rpbReportFilters.Items.Count)
                            ExpandCollapse(false);
                        else
                            ExpandCollapse(true);
                    };
                    p.Background = new SolidColorBrush(Colors.White);
                    p.Style = s;
                    p.MaxHeight = 100;
                    rpbReportFilters.Items.Add(p);
                }
                ToolTip t = this.FindResource("panelBarToolTip") as ToolTip;
                foreach (RadPanelBarItem parent in rpbReportFilters.Items)
                {
                    List<FrameworkUAD.Entity.CodeSheet> codes = codeSheetList.Where(x => x.ResponseGroupID == (int)parent.Tag).OrderBy(x => x.ResponseDesc).ToList();
                    List<FrameworkUAD.Entity.CodeSheet> displayValid = codes.Where(x => x.DisplayOrder > 0 && x.DisplayOrder != null).OrderBy(x => x.DisplayOrder).ToList();
                    List<FrameworkUAD.Entity.CodeSheet> displayInvalid = codes.Except(displayValid)
                    .OrderBy(s2 =>
                    {
                        int i = 0;
                        return int.TryParse(s2.ResponseValue, out i) ? i : int.MaxValue;
                    })
                    .ThenBy(s2 => s2.ResponseValue)
                    .ToList();
                    codes.Clear();
                    codes.AddRange(displayValid);
                    codes.AddRange(displayInvalid);
                    foreach (FrameworkUAD.Entity.CodeSheet r in codes)
                    {
                        RadPanelBarItem child = new RadPanelBarItem();
                        child.Header = r.ResponseValue + ". " + r.ResponseDesc;
                        child.Style = s;
                        child.ToolTip = t;
                        child.Name = "_" + r.CodeSheetID.ToString();
                        parent.Items.Add(child);
                    }
                }
                ExpandCollapse(true);
            }
        }

        private void rcbBrands_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadComboBox rcb = sender as RadComboBox;
            if (rcb.SelectedItem != null)
            {
                FrameworkUAD.Entity.Brand b = rcb.SelectedItem as FrameworkUAD.Entity.Brand;
                BrandID = b.BrandID;
                if (rcb.SelectedIndex == 0)
                {
                    rcbProducts.ItemsSource = productList.OrderBy(x => x.PubCode);
                }                
            }
        }
        #endregion

        #region Helper Methods
        private void ExpandCollapse(bool expanded)
        {
            //Support method for txt Routed events.
            if (expanded)
            {
                txtExpandCollapse.Text = "+";
                txtExpandCollapse.ToolTip = "Expand All";
                txtExpandCollapse.FontSize = 20;
                txtExpandCollapse.FontWeight = FontWeights.SemiBold;
                txtExpandCollapse.MouseUp -= txtCollapse_MouseUp;
                txtExpandCollapse.MouseUp += txtExpandCollapse_MouseUp;
            }
            else
            {
                txtExpandCollapse.Text = "-";
                txtExpandCollapse.FontSize = 24;
                txtExpandCollapse.FontWeight = FontWeights.Bold;
                txtExpandCollapse.ToolTip = "Collapse All";
                txtExpandCollapse.MouseUp -= txtExpandCollapse_MouseUp;
                txtExpandCollapse.MouseUp += txtCollapse_MouseUp;
            }
        }

        public Helpers.FilterOperations.FilterDetailContainer GetSelection()
        {
            Helpers.FilterOperations.FilterDetailContainer myDetailContainer = new Helpers.FilterOperations.FilterDetailContainer();
            myDetailContainer.FilterDetail.FilterObjectType = "UADResponses";
            myDetailContainer.FilterDetail.FilterField = rpbReportFilters.Name.ToString();
            myDetailContainer.FilterDetail.FilterTypeId = filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.Dynamic.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault();
            myDetailContainer.FilterDetail.SearchCondition = "EQUAL";
            foreach (RadPanelBarItem parent in rpbReportFilters.Items)
            {
                foreach (RadPanelBarItem child in parent.Items)
                {
                    if (child.IsSelected)
                        myDetailContainer.Values.Add(new FrameworkUAS.Entity.FilterDetailSelectedValue() { SelectedValue = parent.Tag.ToString() + ":" + child.Name.ToString().Replace("_", "") });
                }
            }
            return myDetailContainer;
        }

        public void SetProduct(int productID)
        {
            rcbProducts.SelectedValue = productID;
        }
        #endregion
    }
}

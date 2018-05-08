using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for DataCompareOptions.xaml
    /// </summary>
    public partial class DCTarget : UserControl
    {
        #region VARIABLES
        FileMapperWizard.Modules.DataCompareSteps thisDCSteps { get; set; }
        List<FrameworkUAD_Lookup.Entity.Code> dcTargets { get; set; }
        List<FrameworkUAD.Entity.Brand> brands { get; set; }
        List<FrameworkUAD.Entity.Market> markets { get; set; }
        List<FrameworkUAD.Entity.Product> products { get; set; }

        /// <summary>
        /// initial business rule is that all Demographics will be considered Premium cost
        /// </summary>
        List<FrameworkUAD_Lookup.Entity.Code> demoPremiums { get; set; }
        //List<FrameworkUAS.Entity.Code> demoStandards { get; set; }
        //List<FrameworkUAS.Entity.Code> demoCustoms { get; set; }

        FrameworkUAS.Service.Response<int> svSaved = new FrameworkUAS.Service.Response<int>();
        FrameworkUAS.Service.Response<bool> svDelete = new FrameworkUAS.Service.Response<bool>();
        #endregion

        public DCTarget(FileMapperWizard.Modules.DataCompareSteps dcSteps)
        {
            thisDCSteps = dcSteps;
            InitializeComponent();
            LoadData();
            LoadPreviousData();
        }
        public void LoadData()
        {
            Telerik.Windows.Controls.RadBusyIndicator busy = Core_AMS.Utilities.WPF.FindControl<Telerik.Windows.Controls.RadBusyIndicator>(thisDCSteps, "busyIcon");
            if (busy != null)
                busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> dcTargetWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
                FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> respDCTargets = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
                respDCTargets = dcTargetWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Data_Compare_Target);
                if (respDCTargets.Result != null)
                    dcTargets = respDCTargets.Result.Where(x => x.IsActive == true).ToList();
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                rcbTarget.ItemsSource = dcTargets;
                rcbTarget.DisplayMemberPath = "DisplayName";
                rcbTarget.SelectedValuePath = "CodeId";
                if (busy != null)
                    busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }
        private void LoadPreviousData()
        {
            //if (thisDCSteps.dataCompareResultQue != null)
            //{
            //    rcbTarget.SelectedValue = thisDCSteps.targetCodeId;
            //}
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (rcbTarget.SelectedValue == null)
                Core_AMS.Utilities.WPF.Message("Please select a Target.", MessageBoxButton.OK, MessageBoxImage.Warning, "Target selection required");
            else
            {
                int codeId = 0;
                int.TryParse(rcbTarget.SelectedValue.ToString(), out codeId);
                thisDCSteps.targetCodeId = codeId;
                FrameworkUAD_Lookup.Entity.Code selCode = (FrameworkUAD_Lookup.Entity.Code)rcbTarget.SelectedItem;
                thisDCSteps.targetName = selCode.CodeName;

                FrameworkUAD_Lookup.Enums.DataCompareTargetTypes dct = FrameworkUAD_Lookup.Enums.GetDataCompareTargetType(selCode.CodeName);

                if (rcbScope.SelectedValue == null && dct != FrameworkUAD_Lookup.Enums.DataCompareTargetTypes.Consensus)
                    Core_AMS.Utilities.WPF.Message("Please select a Scope.", MessageBoxButton.OK, MessageBoxImage.Warning, "Scope selection required");
                else
                {
                    int scopeId = 0;
                    if (dct != FrameworkUAD_Lookup.Enums.DataCompareTargetTypes.Consensus)
                        int.TryParse(rcbScope.SelectedValue.ToString(), out scopeId);
                                        
                    if (dct == FrameworkUAD_Lookup.Enums.DataCompareTargetTypes.Brand)
                    {
                        thisDCSteps.isConsensus = false;
                        thisDCSteps.marketId = null;
                        thisDCSteps.productId = null;
                        thisDCSteps.productCode = string.Empty;
                        thisDCSteps.brandId = scopeId;
                        FrameworkUAD.Entity.Brand b = (FrameworkUAD.Entity.Brand)rcbScope.SelectedItem;
                        thisDCSteps.brandName = b.BrandName;
                    }
                    else if (dct == FrameworkUAD_Lookup.Enums.DataCompareTargetTypes.Market)
                    {
                        thisDCSteps.isConsensus = false;
                        thisDCSteps.brandId = null;
                        thisDCSteps.productId = null;
                        thisDCSteps.productCode = string.Empty;
                        thisDCSteps.marketId = scopeId;
                        FrameworkUAD.Entity.Market m = (FrameworkUAD.Entity.Market)rcbScope.SelectedItem;
                        thisDCSteps.marketName = m.MarketName;
                    }
                    else if (dct == FrameworkUAD_Lookup.Enums.DataCompareTargetTypes.Product)
                    {
                        thisDCSteps.isConsensus = false;
                        thisDCSteps.brandId = null;
                        thisDCSteps.marketId = null;
                        thisDCSteps.productId = scopeId;
                        FrameworkUAD.Entity.Product p = (FrameworkUAD.Entity.Product)rcbScope.SelectedItem;
                        thisDCSteps.productCode = p.PubCode;
                        thisDCSteps.productName = p.PubName;
                    }
                    else if (dct == FrameworkUAD_Lookup.Enums.DataCompareTargetTypes.Consensus)
                    {
                        thisDCSteps.isConsensus = true;
                        thisDCSteps.brandId = null;
                        thisDCSteps.marketId = null;
                        thisDCSteps.productId = null;
                        thisDCSteps.productCode = string.Empty;
                    }

                    thisDCSteps.Step1ToStep2();

                    var borderList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.Border>(thisDCSteps);
                    if (borderList.FirstOrDefault(x => x.Name.Equals("StepTwoContainer", StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        Border thisBorder = borderList.FirstOrDefault(x => x.Name.Equals("StepTwoContainer", StringComparison.CurrentCultureIgnoreCase));
                        thisBorder.Child = new FileMapperWizard.Controls.DCResultQue(thisDCSteps);
                    }
                }
            }
        }

        private void rcbTarget_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbScope.Visibility = System.Windows.Visibility.Visible;
            rcbScope.Visibility = System.Windows.Visibility.Visible;
            btnScopeInfo.Visibility = System.Windows.Visibility.Visible;

            FrameworkUAD_Lookup.Entity.Code dctCode = (FrameworkUAD_Lookup.Entity.Code)rcbTarget.SelectedItem;
            FrameworkUAD_Lookup.Enums.DataCompareTargetTypes dct = FrameworkUAD_Lookup.Enums.GetDataCompareTargetType(dctCode.CodeName);
            if (dct == FrameworkUAD_Lookup.Enums.DataCompareTargetTypes.Brand)
            {
                //get and bind Brands
                if(brands == null || brands.Count == 0)
                {
                    FrameworkServices.ServiceClient<UAD_WS.Interface.IBrand> bWorker = FrameworkServices.ServiceClient.UAD_BrandClient();
                    FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Brand>> bResult = bWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisDCSteps.fmContainer.myClient.ClientConnections);
                    brands = new List<FrameworkUAD.Entity.Brand>();
                    if (bResult.Result != null)
                    {
                        brands = bResult.Result.Where(x => x.IsDeleted == false).OrderBy(x => x.BrandName).ToList();
                        rcbScope.ItemsSource = brands;
                        rcbScope.DisplayMemberPath = "BrandName";
                        rcbScope.SelectedValuePath = "BrandID";
                    }

                    //if (thisDCSteps.dataCompareResultQue != null)
                    //{
                    //    rcbScope.SelectedValue = thisDCSteps.brandId;
                    //}
                }
            }
            else if (dct == FrameworkUAD_Lookup.Enums.DataCompareTargetTypes.Market)
            {
                if (markets == null || markets.Count == 0)
                {
                    //get and bind Markets
                    FrameworkServices.ServiceClient<UAD_WS.Interface.IMarket> mWorker = FrameworkServices.ServiceClient.UAD_MarketClient();
                    FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Market>> mResult = mWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisDCSteps.fmContainer.myClient.ClientConnections);
                    markets = new List<FrameworkUAD.Entity.Market>();
                    if (mResult.Result != null)
                    {
                        markets = mResult.Result.OrderBy(x => x.MarketName).ToList();
                        rcbScope.ItemsSource = markets;
                        rcbScope.DisplayMemberPath = "MarketName";
                        rcbScope.SelectedValuePath = "MarketID";
                    }

                    //if (thisDCSteps.dataCompareResultQue != null)
                    //{
                    //    rcbScope.SelectedValue = thisDCSteps.marketId;
                    //}
                }
            }
            else if (dct == FrameworkUAD_Lookup.Enums.DataCompareTargetTypes.Product)
            {
                if (products == null || products.Count == 0)
                {
                    //get and bind Products
                    FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
                    FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> pResult = pWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisDCSteps.fmContainer.myClient.ClientConnections);
                    products = new List<FrameworkUAD.Entity.Product>();
                    if (pResult.Result != null)
                    {
                        products = pResult.Result.OrderBy(x => x.PubName).ToList();
                        rcbScope.ItemsSource = products;
                        rcbScope.DisplayMemberPath = "PubName";
                        rcbScope.SelectedValuePath = "PubID";
                    }

                    //if (thisDCSteps.dataCompareResultQue != null)
                    //{
                    //    rcbScope.SelectedValue = thisDCSteps.productId;
                    //}
                }
            }
            else if (dct == FrameworkUAD_Lookup.Enums.DataCompareTargetTypes.Consensus)
            {
                lbScope.Visibility = System.Windows.Visibility.Collapsed;
                rcbScope.Visibility = System.Windows.Visibility.Collapsed;
                btnScopeInfo.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}

using Circulation.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace Circulation.Windows
{
    /// <summary>
    /// Interaction logic for SuggestMatch.xaml
    /// </summary>
    public partial class SuggestMatch : Window
    {
        #region Workers & Responses
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> subscriptionWorker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IPubSubscriptionDetail> pubSubDetailW = FrameworkServices.ServiceClient.UAD_PubSubscriptionDetailClient();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscription>> subscriptionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscription>>();
        FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription> subsResponse = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscriptionDetail>> pubSubDetailResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscriptionDetail>>();
        List<FrameworkUAD.Entity.ProductSubscription> prodSubscribers = new List<FrameworkUAD.Entity.ProductSubscription>();
        FrameworkUAD.Entity.ProductSubscription prodSubscription = new FrameworkUAD.Entity.ProductSubscription();
        #endregion
        #region Entities
        private Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;

        // Search Vars
        public bool subscriberSelected = false;
        public int subscriberID = 0;
        #endregion
        public event Action<Core_AMS.Utilities.Enums.DialogResponses> DialogResponse;
        public SuggestMatch(DataTable dt)
        {
            InitializeComponent();
            GridAmaze.ItemsSource = null;
            GridAmaze.ItemsSource = dt.AsDataView();
            //GridAmaze.AutoGenerateColumns = true;
            GridAmaze.Rebind();
        }
        public void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if(DialogResponse != null)
                DialogResponse(Core_AMS.Utilities.Enums.DialogResponses.Cancel);
            this.Close();
        }
        public void btnCopyProfile_Click(object sender, RoutedEventArgs e)
        {
            FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> prodSubWorker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
            FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscription> subWorker = FrameworkServices.ServiceClient.UAD_SubscriptionClient();
            FrameworkUAS.Service.Response<FrameworkUAD.Entity.Subscription> subResponse = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.Subscription>();
            FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription> prodSubResponse = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription>();

            Button thisBtn = (Button)sender;
            int id = 0;

            DataRowView dv = thisBtn.DataContext as DataRowView;
            string type = (string)dv.Row["MatchType"];

            if (type == "PRODUCT")
            {
                id = (int)dv.Row["PubSubscriptionID"];
                if(id > 0)
                {
                    prodSubResponse = prodSubWorker.Proxy.SelectProductSubscription(accessKey, id, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.DisplayName);
                    if (prodSubResponse.Result != null && prodSubResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        prodSubscription = prodSubResponse.Result;
                        pubSubDetailResponse = pubSubDetailW.Proxy.Select(accessKey, prodSubscription.PubSubscriptionID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        if(Helpers.Common.CheckResponse(pubSubDetailResponse.Result, pubSubDetailResponse.Status))
                            prodSubscription.ProductMapList = pubSubDetailResponse.Result;
                        List<KMPlatform.Object.Product> productList = new List<KMPlatform.Object.Product>();
                        foreach (var cp in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients)
                            productList.AddRange(cp.Products.Where(x=> x.IsCirc == true));

                        FrameworkUAS.Object.AppData.myAppData.NewSubscriptionOpened = false;

                        foreach (Window w in Application.Current.Windows)
                        {
                            if (w.GetType() == typeof(Popout))
                            {
                                SubscriptionContainer sc = w.FindChildByType<SubscriptionContainer>();
                                if (sc != null)
                                {
                                    sc.BindProfile(prodSubscription);
                                    sc.CheckSubscriptionStatus();
                                }
                            }
                        }
                    }
                    else
                        Core_AMS.Utilities.WPF.MessageServiceError();
                }
            }
            else
            {
                id = (int)dv.Row["SubscriptionID"];
                subResponse = subWorker.Proxy.Select(accessKey, id, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, false);
                if (subResponse.Result != null && subResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    List<KMPlatform.Object.Product> productList = new List<KMPlatform.Object.Product>();
                    foreach (var cp in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients)
                        productList.AddRange(cp.Products.Where(x=> x.IsCirc == true));

                    FrameworkUAD.Entity.ProductSubscription sub = new FrameworkUAD.Entity.ProductSubscription(subResponse.Result);
                    FrameworkUAS.Object.AppData.myAppData.NewSubscriptionOpened = true;

                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w.GetType() == typeof(Popout))
                        {
                            SubscriptionContainer sc = w.FindChildByType<SubscriptionContainer>();
                            if (sc != null)
                            {
                                sc.BindProfile(sub);
                                sc.CheckSubscriptionStatus();
                            }
                        }
                    }
                }
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();
            }

            if(DialogResponse != null)
                DialogResponse(Core_AMS.Utilities.Enums.DialogResponses.Copy);
            this.Close();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SubscriptionContainer sc = null;
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType() == typeof(Popout))
                {
                    if(sc == null)
                        sc = w.FindChildByType<SubscriptionContainer>();
                }
            }
            if(DialogResponse != null)
                DialogResponse(Core_AMS.Utilities.Enums.DialogResponses.Save);
            this.Close();
        }
    }
}

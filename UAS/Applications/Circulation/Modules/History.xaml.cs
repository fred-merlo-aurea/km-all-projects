using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using FrameworkUAD.Object;
using Telerik.Windows.Controls;
using System.Data;
using Core_AMS.Utilities;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for Batch.xaml
    /// </summary>    
    public partial class History : UserControl
    {
        #region ServiceCalls
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IBatchHistoryDetail> bhdWorker;
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IFinalizeBatch> fbWorker;
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IBatch> bWorker;
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userData = FrameworkServices.ServiceClient.UAS_UserClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productData = FrameworkServices.ServiceClient.UAD_ProductClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> clientData = FrameworkServices.ServiceClient.UAS_ClientClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscription> subscriptionData = FrameworkServices.ServiceClient.UAD_SubscriptionClient();
        //private FrameworkServices.ServiceClient<UAD_WS.Interface.IResponse> responseData = FrameworkServices.ServiceClient.UAD_ResponseClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IBatch> batchData = FrameworkServices.ServiceClient.UAD_BatchClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IAction> aWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> ccWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCodeType> ccTypeW = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> tWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ISubscriptionStatus> stWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> qsWorker;
        //private FrameworkServices.ServiceClient<UAS_WS.Interface.IDeliverability> dWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> parWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> mWorker;
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> rtWorker;
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ICodeSheet> rWorker;
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> uWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICountry> countryWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IRegion> regionWorker;
        #endregion
        #region Variables
        Core_AMS.Utilities.FileFunctions ff = new FileFunctions();
        private List<FrameworkUAD.Entity.ResponseGroup> Questions;
        private List<FrameworkUAD.Entity.CodeSheet> Answers;
        private List<int> respId = new List<int>();
        private static List<string> dontDisplayColumn;
        private List<int> singleAnswer;
        private List<KMPlatform.Entity.User> userList = new List<KMPlatform.Entity.User>();
        private List<KMPlatform.Entity.User> users = new List<KMPlatform.Entity.User>();
        private List<FrameworkUAD_Lookup.Entity.Action> action;
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> cat;
        private List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catType;
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> trans;
        private List<FrameworkUAD_Lookup.Entity.SubscriptionStatus> sStatus;
        private List<FrameworkUAD_Lookup.Entity.Code> qSource;
        //private List<FrameworkUAS.Entity.Deliverability> de;
        private List<FrameworkUAD_Lookup.Entity.Code> par;
        private List<FrameworkUAD_Lookup.Entity.Code> codeList;//SubscriberSource
        private List<FrameworkUAD_Lookup.Entity.Code> marketing;
        private List<FrameworkUAD_Lookup.Entity.Country> country;
        private List<FrameworkUAD_Lookup.Entity.Region> region;
        private List<FrameworkUAD.Entity.Product> productList = new List<FrameworkUAD.Entity.Product>();
        private Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
        private bool isKM = false;
        private DataTable myDataTable = new DataTable();
        private FrameworkUAD_Lookup.Entity.CodeType codeTypeVar = new FrameworkUAD_Lookup.Entity.CodeType();
        #endregion
        #region ServiceResponses
        private FrameworkUAS.Service.Response<bool> boolResponse = new FrameworkUAS.Service.Response<bool>();
        private FrameworkUAS.Service.Response<List<KMPlatform.Entity.User>> userResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.User>>();
        private FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> clientResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> prodResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Object.FinalizeBatch>> finalizeBatchResponse = new FrameworkUAS.Service.Response<List<FinalizeBatch>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Object.BatchHistoryDetail>> batchHistoryResponse = new FrameworkUAS.Service.Response<List<BatchHistoryDetail>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>> rResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> rTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>> actionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> cCodeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>> ccTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> tCodeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>> subStatusResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> qSourceResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        //private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.Deliverability>> deResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.Deliverability>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> par3CResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> marketingResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>> countryResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>> regionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Batch>> batchResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Batch>>();

        #endregion

        //History allows the User to finalize Open Batches or download finalized batches. The download can include a list of the details pulled from the UserLog table.
        public History(bool finalBatchesTabOpen = false)
        {
            InitializeComponent();

            bhdWorker = FrameworkServices.ServiceClient.UAD_BatchHistoryDetailClient();
            fbWorker = FrameworkServices.ServiceClient.UAD_FinalizeBatchClient();
            bWorker = FrameworkServices.ServiceClient.UAD_BatchClient();
            aWorker = FrameworkServices.ServiceClient.UAD_Lookup_ActionClient();
            ccWorker = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeClient();
            tWorker = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeClient();
            stWorker = FrameworkServices.ServiceClient.UAD_Lookup_SubscriptionStatusClient();
            qsWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
            parWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
            codeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
            mWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
            rtWorker = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
            rWorker = FrameworkServices.ServiceClient.UAD_CodeSheetClient();
            bhdWorker = FrameworkServices.ServiceClient.UAD_BatchHistoryDetailClient();
            uWorker = FrameworkServices.ServiceClient.UAS_UserClient();
            countryWorker = FrameworkServices.ServiceClient.UAD_Lookup_CountryClient();
            regionWorker = FrameworkServices.ServiceClient.UAD_Lookup_RegionClient();

            BindGrid();

            dgSearchHistory.Visibility = Visibility.Hidden;            
            KMPlatform.Entity.Client client = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;

            rcbPublisher.DisplayMemberPath = "DisplayName";
            rcbPublisher.SelectedValuePath = "ClientID";
            rcbUserName.DisplayMemberPath = "UserName";
            rcbUserName.SelectedValuePath = "UserID";

            isKM = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.IsKmUser;

            List<KMPlatform.Entity.Client> clients = new List<KMPlatform.Entity.Client>();
            
            clients.AddRange(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.Where(x => x.IsActive == true).ToList());   

            if (client.ClientID == 1 && isKM)
            {
                userResponse = userData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(userResponse.Result, userResponse.Status))
                    userList = userResponse.Result;

                rcbPublisher.ItemsSource = clients;
                rcbPublisher.IsEnabled = true;

                rcbUserName.ItemsSource = userList;
                rcbUserName.IsEnabled = true;
            }
            else
            {
                rcbUserName.Items.Add(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User);
                rcbUserName.SelectedValue = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                colPublisher.IsVisible = false;
                colClientF.IsVisible = false;

                List<KMPlatform.Entity.Client> c = clients.Where(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID).ToList();
                rcbPublisher.ItemsSource = c;
                rcbPublisher.SelectedIndex = 0;
                rcbPublisher.IsEnabled = false;
            }

            FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICodeType> ctWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeTypeClient();
            FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.CodeType> ctResponse = new FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.CodeType>();
            ctResponse = ctWorker.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Deliver);
            if (ctResponse.Result != null && ctResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                codeTypeVar = ctResponse.Result;
            }

            if (finalBatchesTabOpen)
            {
                tabBatch.SelectedIndex = 2;
            }

            #region Set up/Validate Service Variables
            if (Home.Actions.Count > 0)
                action = Home.Actions;
            else
            {
                actionResponse = aWorker.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(actionResponse.Result, actionResponse.Status))
                    action = actionResponse.Result;
            }

            if (Home.CategoryCodes.Count > 0)
                cat = Home.CategoryCodes;
            else
            {
                cCodeResponse = ccWorker.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(cCodeResponse.Result, cCodeResponse.Status))
                    cat = cCodeResponse.Result;
            }

            if (Home.TransactionCodes.Count > 0)
                trans = Home.TransactionCodes;
            else
            {
                tCodeResponse = tWorker.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(tCodeResponse.Result, tCodeResponse.Status))
                    trans = tCodeResponse.Result;
            }

            if (Home.SubscriptionStatuses.Count > 0)
                sStatus = Home.SubscriptionStatuses;
            else
            {
                subStatusResponse = stWorker.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(subStatusResponse.Result, subStatusResponse.Status))
                    sStatus = subStatusResponse.Result;
            }

            if (Home.QSourceCodes.Count > 0)
                qSource = Home.QSourceCodes;
            else
            {
                qSourceResponse = qsWorker.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(qSourceResponse.Result, qSourceResponse.Status))
                    qSource = qSourceResponse.Result;
            }

            if (Home.CategoryCodeTypes.Count > 0)
                catType = Home.CategoryCodeTypes;
            else
            {
                ccTypeResponse = ccTypeW.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(ccTypeResponse.Result, ccTypeResponse.Status))
                    catType = ccTypeResponse.Result;
            }

            if (Home.Par3CCodes.Count > 0)
                par = Home.Par3CCodes;
            else
            {
                par3CResponse = parWorker.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(par3CResponse.Result, par3CResponse.Status))
                    par = par3CResponse.Result;
            }

            if (Home.Codes.Count > 0)
                codeList = Home.Codes;
            else
            {
                codeResponse = codeWorker.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
                    codeList = codeResponse.Result;
            }

            if (Home.MarketingCodes.Count > 0)
                marketing = Home.MarketingCodes;
            else
            {
                marketingResponse = mWorker.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(marketingResponse.Result, marketingResponse.Status))
                    marketing = marketingResponse.Result;
            }

            if (Home.Countries.Count > 0)
                country = Home.Countries;
            else
            {
                countryResponse = countryWorker.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(countryResponse.Result, countryResponse.Status))
                    country = countryResponse.Result;
            }

            if (Home.Regions.Count > 0)
                region = Home.Regions;
            else
            {
                regionResponse = regionWorker.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(regionResponse.Result, regionResponse.Status))
                    region = regionResponse.Result;
            }

            userResponse = uWorker.Proxy.Select(accessKey);
            if(Helpers.Common.CheckResponse(userResponse.Result, userResponse.Status))
                users = userResponse.Result;

            prodResponse = productData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (Helpers.Common.CheckResponse(prodResponse.Result, prodResponse.Status))
            {
                productList = prodResponse.Result.Where(x=> x.IsCirc == true).ToList();
                rcbPublication.ItemsSource = productList;
                rcbPublication.DisplayMemberPath = "PubCode";
                rcbPublication.SelectedValuePath = "PubID";
            }
            #endregion
        }

        private void BindGrid()
        {
            KMPlatform.Entity.Client myClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
            List<FrameworkUAD.Object.FinalizeBatch> listBHD = new List<FrameworkUAD.Object.FinalizeBatch>();
            finalizeBatchResponse = fbWorker.Proxy.SelectBatchUserName(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, myClient.ClientConnections, myClient.ClientID, myClient.DisplayName);
            if(Helpers.Common.CheckResponse(finalizeBatchResponse.Result, finalizeBatchResponse.Status))
                listBHD = finalizeBatchResponse.Result.Where(x=> x.DateFinalized == null).ToList();

            foreach (FinalizeBatch fb in listBHD)
            {
                if (fb.UserName.Equals("Admin Admin"))
                {
                    fb.UserName = "Admin";
                }
            }

            dgHistory.ItemsSource = null;
            dgHistory.ItemsSource = listBHD.OrderByDescending(x=> x.DateCreated);
            dgHistory.Rebind();
        }

        #region Button Clicks
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //Check BatchID is number and exists
            dgSearchHistory.Visibility = Visibility.Hidden;
            int batchID = -1;
            int pubID = -1;
            int publisherID = -1;
            int userID = -1;
            if(rcbUserName.SelectedItem != null)
                int.TryParse(rcbUserName.SelectedValue.ToString(), out userID);
            int.TryParse(txtBatchID.Text, out batchID);
            bool publisher = false;
            bool publication = false;
            bool haveBatchID = false;
            bool dateSearch = false;
            bool user = false;
            
            BackgroundWorker bw = new BackgroundWorker();
            busy.IsBusy = true;

            if (rcbPublisher.SelectedItem != null)
            {
                publisher = true;
                int.TryParse(rcbPublisher.SelectedValue.ToString(), out publisherID);
            }
            if (rcbPublication.SelectedItem != null)
            {
                publication = true;
                int.TryParse(rcbPublication.SelectedValue.ToString(), out pubID);
            }
            if (txtBatchID.Text != string.Empty && txtBatchID.Text != null)
            {
                haveBatchID = true;
            }
            if (rdpStart.SelectedDate != null || rdpEnd.SelectedDate != null)
                dateSearch = true;
            if(rcbUserName.SelectedItem != null)
            {
                user = true;
            }

            dgSearchHistory.ItemsSource = null;
            List<FrameworkUAD.Object.FinalizeBatch> listBHD = new List<FinalizeBatch>();

            DateTime from;
            DateTime to;
            if (rdpStart.SelectedDate == null)
                from = System.DateTime.MinValue.Date;
            else
                from = (DateTime)rdpStart.SelectedDate.Value.Date;

            if (rdpEnd.SelectedDate == null)
                to = System.DateTime.Now.Date;
            else
                to = (DateTime)rdpEnd.SelectedDate.Value.Date; 

            bw.DoWork += (o, ea) =>
            {
                KMPlatform.Entity.Client myClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
                if (!user && isKM)
                {
                    finalizeBatchResponse = fbWorker.Proxy.SelectBatchUserName(accessKey, -1, myClient.ClientConnections, myClient.ClientID, myClient.DisplayName);
                    if(Helpers.Common.CheckResponse(finalizeBatchResponse.Result, finalizeBatchResponse.Status))
                        listBHD = finalizeBatchResponse.Result.Where(x => x.DateFinalized != null).ToList();
                }
                else if (user)
                {
                    finalizeBatchResponse = fbWorker.Proxy.SelectBatchUserName(accessKey, userID, myClient.ClientConnections, myClient.ClientID, myClient.DisplayName);
                    if(Helpers.Common.CheckResponse(finalizeBatchResponse.Result, finalizeBatchResponse.Status))
                        listBHD = finalizeBatchResponse.Result.Where(x => x.DateFinalized != null).ToList();
                }

                if(publisher == true)
                    listBHD = listBHD.Where(x => x.ClientID == publisherID).ToList();
                    
                if(publication == true)
                    listBHD = listBHD.Where(x => x.ProductID == pubID).ToList();

                if(haveBatchID == true)
                    listBHD = listBHD.Where(x => x.BatchNumber == batchID).ToList();

                if (dateSearch == true)
                    listBHD = listBHD.Where(x => x.DateFinalized.Value.Date <= to && x.DateFinalized.Value.Date >= from).ToList();

                foreach(FinalizeBatch fb in listBHD)
                {
                    if(fb.UserName != null && fb.UserName.Equals("Admin Admin"))
                    {
                        fb.UserName = "Admin";
                    }
                }
            };

            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (listBHD.Count == 0)
                {
                    Core_AMS.Utilities.WPF.Message("No results found", MessageBoxButton.OK, MessageBoxImage.Exclamation, "No Results");
                    dgSearchHistory.Visibility = Visibility.Hidden;
                    busy.IsBusy = false;
                }
                else
                {
                    dgSearchHistory.ItemsSource = listBHD.OrderByDescending(x => x.DateFinalized);
                    dgSearchHistory.Items.Refresh();
                    dgSearchHistory.Visibility = Visibility.Visible;
                    busy.IsBusy = false;
                }
            };
            bw.RunWorkerAsync();
        }

        private void btnFinalize_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to finalize this batch?", "Finalize Batch", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Button btn = sender as Button;
                FrameworkUAD.Object.FinalizeBatch sr = btn.DataContext as FrameworkUAD.Object.FinalizeBatch;

                //Finalize Batch
                FrameworkUAD.Entity.Batch b = new FrameworkUAD.Entity.Batch()
                {
                    BatchID = sr.BatchID,
                    PublicationID = sr.ProductID,
                    UserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                    BatchCount = sr.LastCount,
                    IsActive = false,
                    DateCreated = System.DateTime.Now,
                    DateFinalized = System.DateTime.Now,
                    BatchNumber = sr.BatchNumber
                };
                bWorker.Proxy.Save(accessKey, b, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                //Add Printing
                FrameworkUAS.Object.Batch batchToRemove = FrameworkUAS.Object.AppData.myAppData.BatchList.Where(x => x.BatchID == sr.BatchID).FirstOrDefault();
                FrameworkUAS.Object.AppData.myAppData.BatchList.Remove(batchToRemove);
                BindGrid();
                Core_AMS.Utilities.WPF.Message("Batch Number " + sr.BatchNumber + " was finalized.", MessageBoxButton.OK, MessageBoxImage.Information, "Finalize Batch");
                PromptForReport(sr);
            }
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            Button thisButton = (Button)sender;
            Telerik.Windows.Controls.GridView.GridViewCell thisCell = (Telerik.Windows.Controls.GridView.GridViewCell)thisButton.Parent;
            Telerik.Windows.Controls.GridView.GridViewRow thisRow = (Telerik.Windows.Controls.GridView.GridViewRow)thisCell.ParentRow;

            FrameworkUAD.Object.FinalizeBatch sr = (FrameworkUAD.Object.FinalizeBatch)thisRow.Item;

            PromptForReport(sr);
        }

        private void btnDownloadDetails_Click(object sender, RoutedEventArgs e)
        {
            Button thisButton = (Button)sender;
            int userID = -1;
            Telerik.Windows.Controls.GridView.GridViewCell thisCell = (Telerik.Windows.Controls.GridView.GridViewCell)thisButton.Parent;
            Telerik.Windows.Controls.GridView.GridViewRow thisRow = (Telerik.Windows.Controls.GridView.GridViewRow)thisCell.ParentRow;

            FrameworkUAD.Object.FinalizeBatch sr = (FrameworkUAD.Object.FinalizeBatch)thisRow.Item;
            batchResponse = batchData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if(Helpers.Common.CheckResponse(batchResponse.Result, batchResponse.Status))
            {
                userID = batchResponse.Result.Where(x => x.BatchID == sr.BatchID).Select(x => x.UserID).FirstOrDefault();
            }

            PromptForDetails(sr, userID);
        }

        private void btnRefreshBatches_Click(object sender, RoutedEventArgs e)
        {
            KMPlatform.Entity.Client myClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
            List<FrameworkUAD.Object.FinalizeBatch> listBHD = new List<FinalizeBatch>();
            finalizeBatchResponse = fbWorker.Proxy.SelectBatchUserName(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, myClient.ClientConnections, myClient.ClientID, myClient.DisplayName);
            if (Helpers.Common.CheckResponse(finalizeBatchResponse.Result, finalizeBatchResponse.Status))
                listBHD = finalizeBatchResponse.Result.Where(x => x.DateFinalized == null).ToList();

            foreach (FinalizeBatch fb in listBHD)
            {
                if (fb.UserName.Equals("Admin Admin"))
                {
                    fb.UserName = "Admin";
                }
            }
            dgHistory.ItemsSource = null;
            dgHistory.ItemsSource = listBHD.OrderByDescending(x => x.DateCreated);
            dgHistory.Rebind();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            KMPlatform.Entity.Client client = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
            rcbPublication.ItemsSource = null;
            rcbPublication.SelectedItem = null;
            rcbPublication.EmptyText = "Select a Publisher";
            rcbPublication.IsEnabled = false;
            rcbPublisher.SelectedItem = null;
            rcbUserName.SelectedItem = null;
            if (!(client.ClientID == 1 && isKM))
            {
                rcbUserName.SelectedIndex = 0;
                rcbPublisher.SelectedIndex = 0;
            }
            txtBatchID.Text = "";
            rdpEnd.SelectedDate = null;
            rdpStart.SelectedDate = null;
            dgSearchHistory.ItemsSource = null;
            dgSearchHistory.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Reports/Downloads
        public void CreateReportCSV(FrameworkUAD.Object.FinalizeBatch sr, bool download, bool PassUserID = true)
        {
            int BatchID = sr.BatchID;
            StringBuilder sb = new StringBuilder();
            DataTable me = new DataTable();
            
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = System.IO.Path.Combine(pathUser, "Downloads");

            string filePath = pathDownload + "\\BatchReport" + "_" + sr.PublicationCode + "_" + sr.BatchNumber + ".csv";
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            else
            {
                int i = 1;
                while (File.Exists(filePath))
                {
                    filePath = pathDownload + "\\BatchReport" + "_" + sr.PublicationCode + "_" + sr.BatchNumber + "_" + i + ".csv";
                    i++;
                }
                File.Create(filePath).Close();
            }

            FinalizeBatch bhd = new FinalizeBatch();
            BackgroundWorker bw = new BackgroundWorker();
            busy.IsBusy = true;
            bw.DoWork += (o, ea) =>
                {
                    KMPlatform.Entity.Client myClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
                    if (PassUserID)
                    {
                        finalizeBatchResponse = fbWorker.Proxy.SelectBatch(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, myClient.ClientConnections, myClient.ClientID, myClient.DisplayName);
                        if(Helpers.Common.CheckResponse(finalizeBatchResponse.Result, finalizeBatchResponse.Status))
                            bhd = finalizeBatchResponse.Result.SingleOrDefault(x => x.BatchID == BatchID);
                    }
                    else
                    {
                        finalizeBatchResponse = fbWorker.Proxy.SelectBatch(accessKey, -1, myClient.ClientConnections, myClient.ClientID, myClient.DisplayName);
                        if(Helpers.Common.CheckResponse(finalizeBatchResponse.Result, finalizeBatchResponse.Status))
                            bhd = finalizeBatchResponse.Result.SingleOrDefault(x => x.BatchID == BatchID);
                    }

                    me.Columns.Add(new DataColumn(" ", typeof(String)));

                    if (bhd != null)
                    {
                        me.Rows.Add("Batch #: " + bhd.BatchNumber.ToString());
                        me.Rows.Add("User Name: " + bhd.UserName);
                        me.Rows.Add("Client: " + bhd.ClientName);
                        me.Rows.Add("Product: " + bhd.PublicationName);
                        me.Rows.Add("Date Created: " + bhd.DateCreated.ToString());
                        me.Rows.Add("Date Finalized: " + bhd.DateFinalized.ToString());
                        me.Rows.Add("Batch Count: " + bhd.LastCount.ToString());
                    }                    
                };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (download == true)
                {
                    ff.CreateCSVFromDataTable(me, filePath, true);
                    Core_AMS.Utilities.WPF.Message("File " + System.IO.Path.GetFileName(filePath).ToString() + " downloaded to User Downloads folder.", MessageBoxButton.OK, MessageBoxImage.Information, "File Downloaded");
                }
                else
                {
                    RadGridView grd = new RadGridView();
                    grd.ItemsSource = me.DefaultView;
                    grd.IsFilteringAllowed = false;
                    grd.ShowGroupPanel = false;
                    grd.UpdateLayout();
                    Helpers.RadGridViewPrinter.Print(grd, true);
                }
                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }

        public void CreateDetailsCSV(FrameworkUAD.Object.FinalizeBatch sr, int user, bool download, bool PassUserID = true)
        {
            int BatchID = sr.BatchID;
            DataTable details = new DataTable();
            List<Helpers.Common.HistoryData> history = new List<Helpers.Common.HistoryData>();
            StringBuilder sb = new StringBuilder();

            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = System.IO.Path.Combine(pathUser, "Downloads");

            string filePath = pathDownload + "\\BatchDetails" + "_" + sr.PublicationCode + "_" + sr.BatchNumber + ".csv";
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            else
            {
                int i = 1;
                while (File.Exists(filePath))
                {
                    filePath = pathDownload + "\\BatchDetails" + "_" + sr.PublicationCode + "_" + sr.BatchNumber + "_" + i + ".csv";
                    i++;
                }
                File.Create(filePath).Close();
            }

            FinalizeBatch bhd = new FinalizeBatch();
            BackgroundWorker bw = new BackgroundWorker();
            busy.IsBusy = true;
            bw.DoWork += (o, ea) =>
            {
                KMPlatform.Entity.Client myClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
                if (PassUserID)
                {
                    finalizeBatchResponse = fbWorker.Proxy.SelectBatch(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, myClient.ClientConnections, myClient.ClientID, myClient.DisplayName);
                    if (Helpers.Common.CheckResponse(finalizeBatchResponse.Result, finalizeBatchResponse.Status))
                        bhd = finalizeBatchResponse.Result.SingleOrDefault(x => x.BatchID == BatchID);
                }
                else
                {
                    finalizeBatchResponse = fbWorker.Proxy.SelectBatch(accessKey, 0, myClient.ClientConnections, myClient.ClientID, myClient.DisplayName);
                    if (Helpers.Common.CheckResponse(finalizeBatchResponse.Result, finalizeBatchResponse.Status))
                        bhd = finalizeBatchResponse.Result.SingleOrDefault(x => x.BatchID == BatchID);
                }

                List<FrameworkUAD.Object.BatchHistoryDetail> listBHD = new List<FrameworkUAD.Object.BatchHistoryDetail>();
                batchHistoryResponse = bhdWorker.Proxy.Select(accessKey, user, false, myClient.ClientConnections, myClient.DisplayName);
                if(Helpers.Common.CheckResponse(batchHistoryResponse.Result, batchHistoryResponse.Status))
                    listBHD = batchHistoryResponse.Result.Where(x => x.BatchID == BatchID).ToList();

                details.Columns.Add(new DataColumn(" ", typeof(String)));
                details.Columns.Add(new DataColumn("  "));
                details.Columns.Add(new DataColumn("   "));
                details.Columns.Add(new DataColumn("    "));

                details.Rows.Add("Batch #: " + bhd.BatchNumber.ToString());
                details.Rows.Add("User Name: " + bhd.UserName);
                details.Rows.Add("Client: " + bhd.ClientName);
                details.Rows.Add("Product: " + bhd.PublicationName);
                details.Rows.Add("Date Created: " + bhd.DateCreated.ToString());
                details.Rows.Add("Date Finalized: " + bhd.DateFinalized.ToString());
                details.Rows.Add("Batch Count: " + bhd.LastCount.ToString());

                details.Rows.Add("");
                var batchNum = listBHD.GroupBy(b => b.BatchID).Select(x => x.Key).ToList();
                bool isSameRow = false;
                List<string> listValue = new List<string>();

                List<FrameworkUAD.Entity.Product> UADProduct = new List<FrameworkUAD.Entity.Product>();
                FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> productResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
                FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productWorker = FrameworkServices.ServiceClient.UAD_ProductClient();

                List<KMPlatform.Entity.Client> clientList = new List<KMPlatform.Entity.Client>();
                clientList.AddRange(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.Where(x => x.IsActive == true).ToList());

                foreach (var b in batchNum)
                {
                    var historySubID = listBHD.Where(x => x.BatchID == b).GroupBy(k => k.HistorySubscriptionID).Select(m => m.Key).ToList();
                    foreach (var hs in historySubID)
                    {
                        var historyObject = listBHD.Where(x => x.BatchID == b && x.HistorySubscriptionID == hs).GroupBy(k => k.Object).Select(m => m.Key).ToList();
                        foreach (var hObject in historyObject)
                        {
                            foreach (var s in listBHD.Where(h => h.BatchID == b && h.HistorySubscriptionID == hs && h.Object == hObject))
                            {
                                if (s.Object != null && (!string.IsNullOrEmpty(s.ToObjectValues) || s.Object.Equals("ProductSubscriptionDetail")))
                                {
                                    // Need to move this to a better spot cause this sucks - q.k.
                                    if (UADProduct == null || UADProduct.Count == 0)
                                    {
                                        productResponse = productWorker.Proxy.Select(accessKey, clientList.SingleOrDefault(x => x.ClientID == sr.ClientID).ClientConnections);
                                        if (Helpers.Common.CheckResponse(productResponse.Result, productResponse.Status) == true)
                                            UADProduct = productResponse.Result;

                                        int prodId = UADProduct.SingleOrDefault(p => p.PubCode == s.PubCode).PubID;

                                        rTypeResponse = rtWorker.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, prodId);
                                        if (Helpers.Common.CheckResponse(rTypeResponse.Result, rTypeResponse.Status))
                                            Questions = rTypeResponse.Result.Where(x => x.IsActive == true).OrderBy(y => y.DisplayOrder).ThenBy(z => z.DisplayName).ToList();
                                        rResponse = rWorker.Proxy.Select(accessKey, prodId, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                        if (Helpers.Common.CheckResponse(rResponse.Result, rResponse.Status))
                                            Answers = rResponse.Result.Where(x => x.IsActive == true).ToList();
                                    }

                                    history = Helpers.Common.JsonComparer(s.Object, s.FromObjectValues, s.ToObjectValues, codeTypeVar, action, cat, catType, trans, sStatus, qSource, par, codeList,
                                                marketing, country, region, Questions, Answers);
                                    if (isSameRow == false) //We only want to process or add this information at the beginning of each row.
                                    {

                                        // Get answers that are the only answers
                                        singleAnswer = (from a in Answers
                                                        group a by a.ResponseGroupID into grp
                                                        where grp.Count() <= 1
                                                        select grp.Key).ToList();

                                        foreach (var s2 in singleAnswer)
                                        {
                                            var Id = (from a in Answers
                                                      where a.ResponseGroupID == s2
                                                      select a.CodeSheetID).SingleOrDefault();
                                            respId.Add(Id);
                                        }
                                        listValue.Add("Subscriber: " + s.FirstName + " " + s.LastName);
                                        listValue.Add("Sequence #: " + s.SequenceID);
                                        listValue.Add("SubscriptionID: " + s.SubscriptionID);
                                        listValue.Add("Date Updated: " + s.UserLogDateCreated);
                                        listValue.Add("");
                                        isSameRow = true;
                                    }
                                    foreach (var y in history.OrderBy(x=> x.SortIndex))
                                    {
                                        listValue.Add(y.PropertyName + ": " + y.DisplayText);
                                    }
                                }
                            }
                        }
                        if (listValue.Count > 0)
                        {
                            int i = 0;
                            string[] values = new string[listValue.Count];
                            foreach (string s2 in listValue)
                            {
                                values[i] = s2;
                                i++;
                                if (i > details.Columns.Count)
                                    details.Columns.Add("Edit " + (i-4), typeof(String));
                            }
                            details.Rows.Add(values);
                        }
                        listValue.Clear();
                        isSameRow = false;
                    }
                }
                
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (download == true)
                {
                    ff.CreateCSVFromDataTable(details, filePath, true);
                    Core_AMS.Utilities.WPF.Message("File " + System.IO.Path.GetFileName(filePath).ToString() + " downloaded to User Downloads folder.", MessageBoxButton.OK, MessageBoxImage.Information, "File Downloaded");
                }
                else
                {
                    DataTemplate dt = this.FindResource("CellTemplate") as DataTemplate;
                    RadGridView grd = new RadGridView();
                    grd.ItemsSource = details.DefaultView;
                    grd.IsFilteringAllowed = false;
                    grd.ShowGroupPanel = false;
                    grd.UpdateLayout();
                    Helpers.RadGridViewPrinter.Print(grd, true);
                    //Helpers.RadGridViewPrinter.PrintPreview(grd);
                }
                busy.IsBusy = false;

            };
            bw.RunWorkerAsync();
        }
        #endregion

        private void rcbPublisher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadComboBox rcb = sender as RadComboBox;
            rcbPublication.EmptyText = "";
            rcbPublication.IsEnabled = true;
            if (rcb.SelectedValue != null)
            {
                int clientID = (int)rcb.SelectedValue;

                rcbPublication.ItemsSource = productList.Where(x => x.ClientID == clientID && x.IsCirc == true).OrderBy(f => f.PubCode);
                rcbPublication.SelectedValuePath = "ProductID";
                rcbPublication.DisplayMemberPath = "ProductCode";
            }
            rcbPublication.UpdateLayout();
        }

        private void PromptForReport(FrameworkUAD.Object.FinalizeBatch sr)
        {
            Core_AMS.Utilities.Enums.DialogResponses? response = null;
            Windows.PrintSaveDialog dialog = new Windows.PrintSaveDialog();
            dialog.Answer += value => response = value;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dialog.ShowDialog();
            if (response == Core_AMS.Utilities.Enums.DialogResponses.Print)
            {
                CreateReportCSV(sr, false, false);
            }
            else if (response == Core_AMS.Utilities.Enums.DialogResponses.Save)
            {
                CreateReportCSV(sr, true, false);
            }
            dialog.Close();
        }

        private void PromptForDetails(FrameworkUAD.Object.FinalizeBatch sr, int userID)
        {
            Core_AMS.Utilities.Enums.DialogResponses? response = null;
            Windows.PrintSaveDialog dialog = new Windows.PrintSaveDialog();
            dialog.Answer += value => response = value;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dialog.ShowDialog();
            if (response == Core_AMS.Utilities.Enums.DialogResponses.Print)
            {
                CreateDetailsCSV(sr, userID, false, false);
            }
            else if (response == Core_AMS.Utilities.Enums.DialogResponses.Save)
            {
                CreateDetailsCSV(sr, userID, true, false);
            }
            dialog.Close();
        }

        private void TodayButton_Click(object sender, RoutedEventArgs e)
        {
            RadButton me = sender as RadButton;
            RadDropDownButton dp = me.ParentOfType<RadDropDownButton>();
            dp.IsOpen = false;
        }
    }
}

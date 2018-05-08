using Circulation.Helpers;
using FrameworkUAD.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for IssueSplits.xaml
    /// </summary>
    public partial class IssueSplits : UserControl
    {
        //DEPRECATED -- WpfControls.IssueSplitDataFetcher is now added to FilterWrapper.
        #region ServiceCall
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IIssue> issueData = FrameworkServices.ServiceClient.UAD_IssueClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> clientData = FrameworkServices.ServiceClient.UAS_ClientClient();
        //private FrameworkServices.ServiceClient<UAS_WS.Interface.IPublication> publicationData = FrameworkServices.ServiceClient.UAS_PublicationClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IWaveMailing> waveMailingData = FrameworkServices.ServiceClient.UAD_WaveMailingClient();
        #endregion
        #region Variables/Lists
        private Guid accessKey;
        private List<Issue> issuesList = new List<Issue>();
        private FrameworkUAD.Entity.Product myProduct = new Product();
        private int myProductID;
        private Issue myIssue = new Issue();
        private List<GridLength> cols = new List<GridLength>();
        private List<GridLength> rows = new List<GridLength>();
        private bool isExpanded = false;
        #endregion
        #region ServiceResponse
        private FrameworkUAS.Service.Response<List<Issue>> issueResponse = new FrameworkUAS.Service.Response<List<Issue>>();
        private FrameworkUAS.Service.Response<KMPlatform.Entity.Client> clientResponse = new FrameworkUAS.Service.Response<KMPlatform.Entity.Client>();
        //private FrameworkUAS.Service.Response<FrameworkUAS.Entity.Publication> pubResponse = new FrameworkUAS.Service.Response<FrameworkUAS.Entity.Publication>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> productResponse = new FrameworkUAS.Service.Response<List<Product>>();
        private FrameworkUAS.Service.Response<List<WaveMailing>> waveMailingResponse = new FrameworkUAS.Service.Response<List<WaveMailing>>();
        #endregion

        public IssueSplits(Issue selectedIssue)
        {
            InitializeComponent();
            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            myIssue = selectedIssue;

            if (selectedIssue != null)
            {
                myProductID = selectedIssue.PublicationId;
                LoadIssue(selectedIssue);
            }
        }

        #region Misc Buttons

        private void btnFilters_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            DoubleAnimation animateMenu = new DoubleAnimation();
            animateMenu = new DoubleAnimation
            {
                To = 250,
                Duration = TimeSpan.FromSeconds(.4)
            };

            //Point scrPos = btn.PointToScreen(new Point(0, 0));
            //FilterMenu menu = new FilterMenu(myProductID, FrameworkUAS.BusinessLogic.Enums.FilterGroupTypes.Circulation);
            //menu.Top = scrPos.Y;
            //menu.Left = scrPos.X - menu.Width + btn.Width;
            //menu.Height = 0;
            //menu.BeginAnimation(Window.HeightProperty, animateMenu);
            //menu.Topmost = true;
            //menu.Owner = Window.GetWindow(this);
            //menu.Show();
        }

        private void btnExpand_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            if (isExpanded == false)
            {
                cols.Clear();
                rows.Clear();
                foreach (ColumnDefinition c in grdMain.ColumnDefinitions)
                {
                    cols.Add(c.Width);
                    if (index == 2 || index == 3)
                        c.Width = new GridLength(1, GridUnitType.Star);
                    else if (c.Width != new GridLength(5, GridUnitType.Pixel))
                        c.Width = new GridLength(0, GridUnitType.Star);

                    index++;
                }
                index = 0;
                foreach (RowDefinition r in grdMain.RowDefinitions)
                {
                    rows.Add(r.Height);
                    if (index == 3)
                        r.Height = new GridLength(1, GridUnitType.Star);
                    else if (r.Height != new GridLength(5, GridUnitType.Pixel) && r.Height != GridLength.Auto)
                        r.Height = new GridLength(0, GridUnitType.Star);

                    index++;
                }
                isExpanded = true;
            }
            else
            {
                foreach (ColumnDefinition c in grdMain.ColumnDefinitions)
                {
                    c.Width = cols[index];
                    index++;
                }
                index = 0;
                foreach (RowDefinition r in grdMain.RowDefinitions)
                {
                    r.Height = rows[index];
                    index++;
                }
                isExpanded = false;
            }
        }

        #endregion

        private void LoadIssue(Issue i)
        {
            Issue myIssue = i;
            txtIssueName.Text = myIssue.IssueName;
            txtOpened.Text = myIssue.DateOpened.ToString();
            txtUpdated.Text = myIssue.DateUpdated.ToString();
            spIssueDetails.Visibility = System.Windows.Visibility.Visible;
            myProductID = myIssue.PublicationId;

            productResponse = productWorker.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (productResponse.Result != null && productResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                myProduct = productResponse.Result.SingleOrDefault(x => x.PubID == myIssue.PublicationId);

            waveMailingResponse = waveMailingData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (Common.CheckResponse(waveMailingResponse.Result, waveMailingResponse.Status))
            {
                WaveMailing wm = waveMailingResponse.Result.Where(x => x.IssueID == myIssue.IssueId).LastOrDefault();
                if (wm != null)
                {
                    txtWaveNumber.Text = (++wm.WaveNumber).ToString();
                    spWaveDetails.Visibility = System.Windows.Visibility.Visible;
                }
            }

            WpfControls.AdHocFilters adhocs = new WpfControls.AdHocFilters(myProductID);
            WpfControls.DynamicDemographics dynFilters = new WpfControls.DynamicDemographics(myProductID);
            WpfControls.StandardDemographics stndFiltes = new WpfControls.StandardDemographics(myIssue.PublicationId);
            WpfControls.IssueSplitDataFetcher issueDF = new WpfControls.IssueSplitDataFetcher(myIssue.IssueId, myProduct);

            spLeft.Children.Clear();
            spTop.Children.Clear();
            spBottom.Children.Clear();
            spBottomRight.Children.Clear();

            spTop.Visibility = System.Windows.Visibility.Hidden;
            spBottom.Visibility = Visibility.Hidden;
            spLeft.Visibility = Visibility.Hidden;
            spBottomRight.Visibility = Visibility.Hidden;

            spLeft.Children.Add(dynFilters);
            spTop.Children.Add(stndFiltes);
            spBottomRight.Children.Add(adhocs);
            spBottom.Children.Add(issueDF);

            busy.IsBusy = true;
            busy.IsIndeterminate = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                dynFilters.LoadData(Home.CodeTypes, Home.Codes);
                stndFiltes.LoadData(Home.Codes, Home.CategoryCodes, Home.CategoryCodeTypes, Home.TransactionCodeTypes, Home.TransactionCodes, Home.CodeTypes, Home.Countries, Home.Regions);
                issueDF.LoadData();
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                dynFilters.SetControls();
                stndFiltes.SetControls(true);
                issueDF.SetControls();
                spTop.Visibility = System.Windows.Visibility.Visible;
                spBottom.Visibility = Visibility.Visible;
                spLeft.Visibility = Visibility.Visible;
                spBottomRight.Visibility = Visibility.Visible;
                busy.IsBusy = false;
                btnFilters.IsEnabled = true;
                btnExpand.IsEnabled = true;
            };
            bw.RunWorkerAsync();
        }
    }
}

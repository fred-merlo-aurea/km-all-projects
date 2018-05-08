using FrameworkUAD.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using WpfControls;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for AddRemove.xaml
    /// </summary>
    public partial class AddRemove : UserControl
    {
        //DEPRECATED -- WpfControls.AddRemoveDataFetcher is now added to FilterWrapper.
        #region ServiceCalls
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IIssue> issueData = FrameworkServices.ServiceClient.UAD_IssueClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IPublicationReports> pubReportData = FrameworkServices.ServiceClient.UAS_PublicationReportsClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubData = FrameworkServices.ServiceClient.UAD_ProductClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> publisherData = FrameworkServices.ServiceClient.UAS_ClientClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userData = FrameworkServices.ServiceClient.UAS_UserClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IWaveMailing> waveMailingData = FrameworkServices.ServiceClient.UAD_WaveMailingClient();
        #endregion
        #region ServiceResponse
        private FrameworkUAS.Service.Response<List<Issue>> issueResponse = new FrameworkUAS.Service.Response<List<Issue>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> pubResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.PublicationReports>> pubReportResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.PublicationReports>>();
        private FrameworkUAS.Service.Response<KMPlatform.Entity.Client> publisherResponse = new FrameworkUAS.Service.Response<KMPlatform.Entity.Client>();
        private FrameworkUAS.Service.Response<List<KMPlatform.Entity.User>> userResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.User>>();
        private FrameworkUAS.Service.Response<List<WaveMailing>> waveMailingResponse = new FrameworkUAS.Service.Response<List<WaveMailing>>();
        #endregion
        #region Variables/Lists
        private Guid accessKey;
        private int myProductID;
        private List<GridLength> cols = new List<GridLength>();
        private List<GridLength> rows = new List<GridLength>();
        private bool isExpanded = false;
        #endregion

        public AddRemove(Issue selectedIssue)
        {
            InitializeComponent();

            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;

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

            Point scrPos = btn.PointToScreen(new Point(0, 0));
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
            txtIssueName.Text = i.IssueName;
            txtOpened.Text = i.DateOpened.ToString();
            txtUpdated.Text = i.DateUpdated.ToString();
            spIssueDetails.Visibility = System.Windows.Visibility.Visible;

            waveMailingResponse = waveMailingData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (Helpers.Common.CheckResponse(waveMailingResponse.Result, waveMailingResponse.Status))
            {
                WaveMailing wm = waveMailingResponse.Result.Where(x => x.IssueID == i.IssueId).LastOrDefault();
                if (wm != null)
                {
                    txtWaveNumber.Text = (++wm.WaveNumber).ToString();
                    spWaveDetails.Visibility = System.Windows.Visibility.Visible;
                }
            }

            spTop.Children.Clear();
            spBottom.Children.Clear();
            spLeftSide.Children.Clear();
            spBottomRight.Children.Clear();

            DynamicDemographics dyn = new DynamicDemographics(myProductID);
            StandardDemographics sd = new StandardDemographics(myProductID);
            AddRemoveDataFetcher adf = new AddRemoveDataFetcher(myProductID);
            AdHocFilters ahf = new AdHocFilters(myProductID);

            spTop.Visibility = System.Windows.Visibility.Hidden;
            spBottom.Visibility = Visibility.Hidden;
            spLeftSide.Visibility = Visibility.Hidden;
            spBottomRight.Visibility = Visibility.Hidden;

            spTop.Children.Add(sd);
            spBottom.Children.Add(adf);
            spLeftSide.Children.Add(dyn);
            spBottomRight.Children.Add(ahf);

            busy.IsBusy = true;
            busy.IsIndeterminate = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                dyn.LoadData(Home.CodeTypes, Home.Codes);
                sd.LoadData(Home.Codes, Home.CategoryCodes, Home.CategoryCodeTypes, Home.TransactionCodeTypes, Home.TransactionCodes, Home.CodeTypes, Home.Countries, Home.Regions);
                adf.LoadData();
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                dyn.SetControls();
                sd.SetControls();
                adf.SetControls();
                spTop.Visibility = System.Windows.Visibility.Visible;
                spBottom.Visibility = Visibility.Visible;
                spLeftSide.Visibility = Visibility.Visible;
                spBottomRight.Visibility = Visibility.Visible;
                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();

            btnFilters.IsEnabled = true;
            btnExpand.IsEnabled = true;
        }
    }
}

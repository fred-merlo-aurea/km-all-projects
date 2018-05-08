
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControls;

namespace Circulation_Explorer.Modules
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        #region ServiceCalls
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IPublicationReports> pubReportData = FrameworkServices.ServiceClient.UAS_PublicationReportsClient();
        //private FrameworkServices.ServiceClient<UAS_WS.Interface.IPublication> pubData = FrameworkServices.ServiceClient.UAS_PublicationClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productData = FrameworkServices.ServiceClient.UAD_ProductClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userData = FrameworkServices.ServiceClient.UAS_UserClient();
        #endregion
        #region ServiceResponse
        //private Response<List<FrameworkCirculation.Entity.Publication>> pubResponse = new Response<List<FrameworkCirculation.Entity.Publication>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> productResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.PublicationReports>> pubReportResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.PublicationReports>>();
        private FrameworkUAS.Service.Response<List<KMPlatform.Entity.User>> userResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.User>>();
        #endregion
        #region Variables/Lists
        private List<KMPlatform.Object.Product> productList = new List<KMPlatform.Object.Product>();

        private Guid accessKey;
        FrameworkUAS.Object.AppData myAppData;
        private int myProductID;
        private List<FrameworkUAD.Entity.Product> products = new List<FrameworkUAD.Entity.Product>();
        private List<GridLength> cols = new List<GridLength>();
        private List<GridLength> rows = new List<GridLength>();
        private bool isExpanded = false;
        #endregion

        public Home(FrameworkUAS.Object.AppData appData)
        {
            InitializeComponent();
            accessKey = appData.AuthorizedUser.AuthAccessKey;
            myAppData = appData;

            productList = new List<KMPlatform.Object.Product>();
            //foreach (var cp in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients)
            //    productList.AddRange(cp.CircProducts);

            productResponse = productData.Proxy.Select(accessKey, appData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (Helpers.Common.CheckResponse(productResponse.Result, productResponse.Status))
                products = productResponse.Result;

            rcbPublication.SelectedValuePath = "PubID";
            rcbPublication.DisplayMemberPath = "PubCode";
            rcbPublication.ItemsSource = products.Where(x => x.IsCirc == true).OrderBy(x => x.PubCode).ToList();
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
            FilterMenu menu = new FilterMenu(myProductID, FrameworkUAS.BusinessLogic.Enums.FilterGroupTypes.Circulation);
            menu.Top = scrPos.Y;
            menu.Left = scrPos.X - menu.Width + btn.Width;
            menu.Height = 0;
            menu.BeginAnimation(Window.HeightProperty, animateMenu);
            menu.Topmost = true;
            menu.Owner = Window.GetWindow(this);
            menu.Show();
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

        private void rcbPublication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rcbPublication.SelectedItem != null)
            {
                FrameworkUAD.Entity.Product prod = rcbPublication.SelectedItem as FrameworkUAD.Entity.Product;
                int id = (int)rcbPublication.SelectedValue;
                myProductID = id;
                //List<PublicationReports> reports = new List<PublicationReports>();

                //pubReportResponse = pubReportData.Proxy.SelectPublication(accessKey, id);
                //if (Helpers.Common.CheckResponse(pubReportResponse.Result, pubReportResponse.Status))
                //    reports = pubReportResponse.Result;

                //cbxReport.ItemsSource = reports;
                //cbxReport.SelectedValuePath = "ReportID";
                //cbxReport.DisplayMemberPath = "ReportName";

                spTop.Children.Clear();
                spBottom.Children.Clear();
                spLeftSide.Children.Clear();
                spBottomRight.Children.Clear();

                DynamicDemographics dyn = new DynamicDemographics(id);
                StandardDemographics sd = new StandardDemographics(id);
                DataFetcher df = new DataFetcher(id);
                AdHocFilters ahf = new AdHocFilters(id);

                spTop.Visibility = System.Windows.Visibility.Hidden;
                spBottom.Visibility = Visibility.Hidden;
                spLeftSide.Visibility = Visibility.Hidden;
                spBottomRight.Visibility = Visibility.Hidden;

                spTop.Children.Add(sd);
                spBottom.Children.Add(df);
                spLeftSide.Children.Add(dyn);
                spBottomRight.Children.Add(ahf);

                busy.IsBusy = true;
                busy.IsIndeterminate = true;
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (o, ea) =>
                {
                    dyn.LoadData(Circulation.Modules.Home.CodeTypes, Circulation.Modules.Home.Codes);
                    sd.LoadData(Circulation.Modules.Home.Codes, Circulation.Modules.Home.CategoryCodes, Circulation.Modules.Home.CategoryCodeTypes,
                        Circulation.Modules.Home.TransactionCodeTypes, Circulation.Modules.Home.TransactionCodes, Circulation.Modules.Home.CodeTypes, Circulation.Modules.Home.Countries,
                        Circulation.Modules.Home.Regions);
                };
                bw.RunWorkerCompleted += (o, ea) =>
                {
                    dyn.SetControls();
                    sd.SetControls();
                    sd.SetTransCatCode("Circulation Explorer");
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
}

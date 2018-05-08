using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using WpfControls.UADControls;

namespace UAD_Explorer.Modules
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        #region ServiceCalls
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IPublicationReports> pubReportData = FrameworkServices.ServiceClient.UAS_PublicationReportsClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubData = FrameworkServices.ServiceClient.UAD_ProductClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productData = FrameworkServices.ServiceClient.UAD_ProductClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userData = FrameworkServices.ServiceClient.UAS_UserClient();
        #endregion
        #region ServiceResponse
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> productResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.PublicationReports>> pubReportResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.PublicationReports>>();
        private FrameworkUAS.Service.Response<List<KMPlatform.Entity.User>> userResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.User>>();
        #endregion
        #region Variables/Lists
        private List<KMPlatform.Object.Product> productList = new List<KMPlatform.Object.Product>();
        private Guid accessKey;
        private int myProductID;
        private List<FrameworkUAD.Entity.Product> products = new List<FrameworkUAD.Entity.Product>();
        #endregion

        public Home()
        {
            InitializeComponent();

            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;

            spTop.Children.Clear();
            spBottom.Children.Clear();
            spLeftSide.Children.Clear();
            spBottomRight.Children.Clear();

            DynamicUADDemographics dyn = new DynamicUADDemographics();
            StandardUADDemographics sd = new StandardUADDemographics();
            UADConsensusDataFetcher df = new UADConsensusDataFetcher();
            ExtraFiltersTabControl xtrFilters = new ExtraFiltersTabControl(FrameworkUAD_Lookup.Enums.FilterGroupTypes.Consensus_View);

            spTop.Visibility = System.Windows.Visibility.Hidden;
            spBottom.Visibility = Visibility.Hidden;
            spLeftSide.Visibility = Visibility.Hidden;
            spBottomRight.Visibility = Visibility.Hidden;

            spTop.Children.Add(sd);
            spBottom.Children.Add(df);
            spLeftSide.Children.Add(dyn);
            spBottomRight.Children.Add(xtrFilters);

            busy.IsBusy = true;
            busy.IsIndeterminate = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                dyn.LoadData();
                sd.LoadData();
                df.LoadData();
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                dyn.SetControls();
                sd.SetControls();
                spTop.Visibility = System.Windows.Visibility.Visible;
                spBottom.Visibility = Visibility.Visible;
                spLeftSide.Visibility = Visibility.Visible;
                spBottomRight.Visibility = Visibility.Visible;
                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();

            imgFilters.IsEnabled = true;
        }

        #region Misc Buttons

        private void Open_FilterMenu(object sender, MouseButtonEventArgs e)
        {
            DoubleAnimation animateMenu = new DoubleAnimation();
            animateMenu = new DoubleAnimation
            {
                To = 250,
                Duration = TimeSpan.FromSeconds(.4)
            };

            Point scrPos = imgFilters.PointToScreen(new Point(0, 0));
            WpfControls.UADControls.UADFilterMenu menu = new WpfControls.UADControls.UADFilterMenu(FrameworkUAD_Lookup.Enums.FilterGroupTypes.Consensus_View);
            menu.Top = scrPos.Y;
            menu.Left = scrPos.X - menu.Width + imgFilters.Width;
            menu.Height = 0;
            menu.BeginAnimation(Window.HeightProperty, animateMenu);
            menu.Topmost = true;
            menu.Owner = Window.GetWindow(this);
            menu.Show();
        }

        #endregion
    }
}

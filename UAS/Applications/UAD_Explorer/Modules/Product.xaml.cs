using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Telerik.Windows.Controls;
using WpfControls.UADControls;

namespace UAD_Explorer.Modules
{
    /// <summary>
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Product : UserControl
    {
        #region ServiceCalls
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IPublicationReports> pubReportData = FrameworkServices.ServiceClient.UAS_PublicationReportsClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userData = FrameworkServices.ServiceClient.UAS_UserClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        #endregion
        #region ServiceResponse
        private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.PublicationReports>> pubReportResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.PublicationReports>>();
        private FrameworkUAS.Service.Response<List<KMPlatform.Entity.User>> userResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.User>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        #endregion
        #region Variables/Lists
        private Guid accessKey;
        private List<FrameworkUAD_Lookup.Entity.Code> filterGroups = new List<FrameworkUAD_Lookup.Entity.Code>();
        #endregion

        public Product()
        {
            InitializeComponent();

            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;

            spTop.Children.Clear();
            spBottom.Children.Clear();
            spLeftSide.Children.Clear();
            spBottomRight.Children.Clear();

            DynamicUADProductDemographics dyn = new DynamicUADProductDemographics();
            StandardUADDemographics sd = new StandardUADDemographics();
            UADProductDataFetcher df = new UADProductDataFetcher();
            ExtraFiltersTabControl xtrFilters = new ExtraFiltersTabControl(FrameworkUAD_Lookup.Enums.FilterGroupTypes.Product_View);

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
            int tempID = -1;
            DynamicUADProductDemographics dpd = this.FindChildByType<DynamicUADProductDemographics>();
            if (dpd != null)
                tempID = dpd.ProductID;

            DoubleAnimation animateMenu = new DoubleAnimation();
            animateMenu = new DoubleAnimation
            {
                To = 250,
                Duration = TimeSpan.FromSeconds(.4)
            };

            Point scrPos = imgFilters.PointToScreen(new Point(0, 0));
            UADProductFilterMenu menu = new UADProductFilterMenu(FrameworkUAD_Lookup.Enums.FilterGroupTypes.Product_View);
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

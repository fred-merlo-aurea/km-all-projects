using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfControls.UADControls
{
    /// <summary>
    /// Interaction logic for ExtraFiltersTabControl.xaml
    /// </summary>
    public partial class ExtraFiltersTabControl : UserControl
    {
        //FrameworkServices.ServiceClient<Circulation_WS.Interface.ISubscriber> subscriberData = FrameworkServices.ServiceClient.Circ_SubscriberClient();
        public AdHocUADFilters adhocs;
        public ActivityFilter activities;

        public ExtraFiltersTabControl(FrameworkUAD_Lookup.Enums.FilterGroupTypes group)
        {
            InitializeComponent();
            adhocs = new AdHocUADFilters();
            activities = new ActivityFilter(group);
            tabAdHoc.Content = adhocs;
            svActivity.Content = activities;
        }

        private void txtCollapseFilters_MouseUp(object sender, MouseButtonEventArgs e)
        {
            txtCollapseFilters.Visibility = Visibility.Collapsed;
            txtExpandFilters.Visibility = Visibility.Visible;
            tbctrlMain.Visibility = Visibility.Collapsed;
            brdMain.BorderThickness = new Thickness(0);
            //e.Handled = true;
        }

        private void txtExpandFilters_MouseUp(object sender, MouseButtonEventArgs e)
        {
            txtExpandFilters.Visibility = Visibility.Collapsed;
            txtCollapseFilters.Visibility = Visibility.Visible;
            tbctrlMain.Visibility = Visibility.Visible;
            brdMain.BorderThickness = new Thickness(1);
            //e.Handled = true;
        }
    }
}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ControlCenter.Windows
{
    /// <summary>
    /// Interaction logic for AnimatedPrompt.xaml
    /// </summary>
    public partial class AnimatedPrompt : Window
    {
        public AnimatedPrompt()
        {
            InitializeComponent();
            LoadData();
        }

        private bool publisherChange = false;
        private DoubleAnimation animationWidth;
        private bool cancelClicked = false;
        //private List<FrameworkUAD.Entity.Product> myProduct;
        private Guid AccessKey;
        private void LoadData()
        {
            AccessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;

            animationWidth = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(.4)
            };
            //Style style = this.FindResource("RadComboDark") as Style;
            //rcbPublication.Style = style;
            //rcbPublisher.Style = style;
            FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> worker = FrameworkServices.ServiceClient.UAS_ClientClient();
            rcbPublisher.ItemsSource = worker.Proxy.Select(AccessKey).Result;
            rcbPublisher.DisplayMemberPath = "PublisherName";
            rcbPublisher.SelectedValuePath = "PublisherID";
        }

        private void rcbCancel_Click(object sender, RoutedEventArgs e)
        {
            cancelClicked = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = animationWidth;
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(Rectangle.WidthProperty, animationWidth);
        }

        private void rcbPublisher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> worker = FrameworkServices.ServiceClient.UAD_ProductClient();
            publisherChange = true;
            Telerik.Windows.Controls.RadComboBox r = (Telerik.Windows.Controls.RadComboBox)sender;
            KMPlatform.Entity.Client c = (KMPlatform.Entity.Client)r.SelectedItem;

            rcbPublication.ItemsSource = worker.Proxy.Select(AccessKey, c.ClientConnections).Result;
            rcbPublication.DisplayMemberPath = "PubCode";
            rcbPublication.SelectedValuePath = "PubID";
            rcbPublication.IsEnabled = true;
            publisherChange = false;
        }

        private void rcbPublication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (publisherChange == false)
            {
                Telerik.Windows.Controls.RadComboBox r = (Telerik.Windows.Controls.RadComboBox)sender;
                int id = ((FrameworkUAD.Entity.Product)r.SelectedItem).PubID;
                string code = ((FrameworkUAD.Entity.Product)r.SelectedItem).PubCode;
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (!cancelClicked)
                this.Close();
        }
    }
}

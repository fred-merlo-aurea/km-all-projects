using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ControlCenter.Windows
{
    /// <summary>
    /// Interaction logic for CreateNewPublication.xaml
    /// </summary>
    public partial class CreateNewPublication : Window
    {

        private List<KMPlatform.Entity.Client> clientList = new List<KMPlatform.Entity.Client>();
        private FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> clientResponse;
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> clientWorker;

        public CreateNewPublication(int id, string name)
        {
            InitializeComponent();

            clientWorker = new FrameworkServices.ServiceClient<UAS_WS.Interface.IClient>();
            clientResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();
            clientResponse = clientWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            if (clientResponse.Result != null && clientResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                clientList = clientResponse.Result;
            else
                Core_AMS.Utilities.WPF.MessageServiceError();

            LoadData(id, name);
        }

        private FrameworkUAD.Entity.Product newPub {get; set;}
        private int pubID = 0;

        private void LoadData(int id, string name)
        {
            int currentYear = DateTime.Now.Year;
            tbYrEnd.Items.Add(currentYear);
            tbYrStart.Items.Add(currentYear);
            for(int i = 0; i<100; i++)
            {
                currentYear++;
                tbYrEnd.Items.Add(currentYear);
                tbYrStart.Items.Add(currentYear);
            }

            //tbID.ItemsSource = FrameworkCirculation.BusinessLogic.Publisher.Select().OrderBy(x=> x.PublisherID);
            //tbID.SelectedValuePath = "PublisherID";
            lbID.Content = id + "-" + name;
            pubID = id;
        }

        #region buttonMethods
        public void btnSave_Click(object sender, RoutedEventArgs e)
        {
            newPub = new FrameworkUAD.Entity.Product();

            string pubName = tbName.Text.Trim();
            string pubCode = tbCode.Text.Trim();
            bool isActive = false;
            if (cbIsActive.IsChecked.HasValue)
                isActive = cbIsActive.IsChecked.Value;
            bool isImported = false;
            if (cbIsImported.IsChecked.HasValue)
                isImported = cbIsImported.IsChecked.Value;
            bool dataEntry = false;
            if (cbDataEntry.IsChecked.HasValue)
                dataEntry = cbDataEntry.IsChecked.Value;
            int pubFID = int.Parse(tbFID.Text);
            string startYear = tbYrStart.SelectedValue.ToString();
            string endYear = tbYrEnd.SelectedValue.ToString();
            DateTime todayDate = DateTime.Now;

            newPub.PubName = pubName;
            newPub.PubCode = pubCode;
            newPub.PubID = pubID;
            newPub.IsActive = isActive;
            newPub.IsImported = isImported;
            newPub.AllowDataEntry = dataEntry;
            newPub.FrequencyID = pubFID;
            newPub.YearStartDate = startYear;
            newPub.YearEndDate = endYear;
            newPub.DateCreated = todayDate;
            newPub.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;

            KMPlatform.Entity.Client client = clientList.SingleOrDefault(c => c.ClientID == newPub.ClientID);

            FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> worker = FrameworkServices.ServiceClient.UAD_ProductClient();
            newPub.PubID = worker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, newPub, client.ClientConnections).Result;
            MessageBoxResult confirm = MessageBox.Show("New Publication has been added.", "Saved Publication", MessageBoxButton.OK);
        }

        public void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            tbName.Text = string.Empty;
            tbCode.Text = string.Empty;
            cbIsActive.IsChecked = false;
            cbIsImported.IsChecked = false;
            cbDataEntry.IsChecked = false;
            tbFID.Text = string.Empty;
            tbYrEnd.SelectedValue = null;
            tbYrStart.SelectedValue = null;
            this.Close();
        }

        public void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirm = MessageBox.Show("Any unsaved data will be lost by leaving the page. Do you wish to continue?", "Unsaved Data", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                this.Close();
            }
            else
                return;
        }

        private bool max;
        public void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (max)
            {
                max = false;
                this.WindowState = System.Windows.WindowState.Normal;
            }
            else
            {
                max = true;
                this.WindowState = System.Windows.WindowState.Maximized;
            }
        }

        public void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        public void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        #endregion
    }
}

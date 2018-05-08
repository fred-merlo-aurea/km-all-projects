using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for Export.xaml
    /// </summary>
    public partial class Export : UserControl
    {
        private FrameworkUAD.Entity.Product myPublisher { get; set; }
        private KMPlatform.Entity.Client myPublication { get; set; }
        public Export()
        {
            InitializeComponent();

            LoadPublishers();
            LoadDataExports();
        }
        private void LoadPublishers()
        {
            //FrameworkCirculation.BusinessLogic.Publisher worker = new FrameworkCirculation.BusinessLogic.Publisher();
            //List<FrameworkCirculation.Entity.Publisher> Publishers = worker.Select().ToList();

            //cbPublisher.ItemsSource = Publishers;
            //cbPublisher.DisplayMemberPath = "PublisherName";
            //cbPublisher.SelectedValuePath = "PublisherID";
        }
        private void LoadPublication(int publisherID)
        {
            //FrameworkUAD.BusinessLogic.Product worker = new FrameworkUAD.BusinessLogic.Product();
            //cbPublication.IsEnabled = true;
            //cbPublication.ItemsSource = worker.Select(publisherID).Where(x => x.AllowDataEntry == true).ToList();
            //cbPublication.DisplayMemberPath = "PublicationCode";
            //cbPublication.SelectedValuePath = "PublicationID";
        }
        private void LoadDataExports()
        {
            //bind cbDataExports with a list of data exports that we do
            //this should be fed from a reports table
            //have IsExport flag from table set to true
            //have See GoodDonor Report structure

        }
        private void cbPublisher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //int publisherID = 0;
            //int.TryParse(cbPublisher.SelectedValue.ToString(), out publisherID);
            //if (publisherID > 0)
            //    LoadPublication(publisherID);
        }
        private void cbPublication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            //myPublisher = (FrameworkCirculation.Entity.Publisher)cbPublisher.SelectedItem;
            //myPublication = (FrameworkCirculation.Entity.Publication)cbPublication.SelectedItem;

            //btnLock.IsEnabled = true;
        }

        private void btnLock_Click(object sender, RoutedEventArgs e)
        {
            //FrameworkCirculation.BusinessLogic.Batch worker = new FrameworkCirculation.BusinessLogic.Batch();
            ////Check if Batch opened before locking and Exporting
            //List<FrameworkCirculation.Entity.Batch> batches = worker.Select().Where(x => x.IsActive == true).ToList();
            //if (batches.Count > 0)
            //{
            //    //Display User that have open batch.
            //    List<int> batchUsers = (from x in batches where x.PublicationID == myPublication.PublicationID select x.UserID).ToList();
            //    KMPlatform.BusinessLogic.User u = new User();
            //    List<KMPlatform.Entity.User> users = u.Select().Where(x => batchUsers.Contains(x.UserID)).ToList();
            //    string userNames = String.Join(", ", users.Select(x => x.FirstName + " " + x.LastName));
            //    Core_AMS.Utilities.WPF.Message("User(s): " + userNames + " currently have an open batch on this Publication that needs to be closed before export.", MessageBoxButton.OK, MessageBoxImage.Information, "Action Not Available");
            //}
            //else
            //{
            //    FrameworkCirculation.BusinessLogic.Publication pubWorker = new FrameworkCirculation.BusinessLogic.Publication();
            //    myPublication.AllowDataEntry = false;
            //    myPublication.DateUpdated = DateTime.Now;
            //    myPublication.UpdatedByUserID = Home.myAppData.AuthorziedUser.User.UserID;
            //    pubWorker.Save(myPublication);
            //}
        }

        private void cbDataExport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //btnExport.IsEnabled = true;
            //load any user controls into spExportParams           
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            //execute selected sproc
            //export data to TAB DELIM CSV file
            //Core_AMS.Utilities.WPF.MessageDataExportComplete();
        }
    }
}

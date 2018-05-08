using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for SingleDataMapTransformation.xaml
    /// </summary>
    public partial class DataMap : UserControl
    {
        public Dictionary<int, string> myList;
        private FrameworkUAS.Entity.TransformDataMap myDataMap;
        public FrameworkUAS.Object.AppData myAppData;
        public KMPlatform.Entity.Client myClient;
        public Dictionary<int, string> pubCode;
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformDataMap> tdmData = FrameworkServices.ServiceClient.UAS_TransformDataMapClient();        

        public DataMap(FrameworkUAS.Object.AppData appData, KMPlatform.Entity.Client client, Dictionary<int,string> list, Dictionary<int, string> allPubs, FrameworkUAS.Entity.TransformDataMap m = null)
        {
            InitializeComponent();
            pubCode = allPubs;
            if (m == null)
                myDataMap = new FrameworkUAS.Entity.TransformDataMap();
            else
                myDataMap = m;

            myAppData = appData;
            myClient = client;
            myList = list;
            myDataMap = m;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            var bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {

            };
            bw.RunWorkerCompleted += (o, ea) =>
            {                                             
                foreach (FrameworkUAD_Lookup.Enums.MatchTypes pt in (FrameworkUAD_Lookup.Enums.MatchTypes[])Enum.GetValues(typeof(FrameworkUAD_Lookup.Enums.MatchTypes)))
                {
                    cbMatch.Items.Add(pt.ToString().Replace("_", " "));
                }

                lstPubCode.ItemsSource = myList.OrderBy(x => x.Value);
                lstPubCode.SelectedMemberPath = "Key";
                lstPubCode.DisplayMemberPath = "Value";

                foreach (KeyValuePair<int, string> c in lstPubCode.Items)
                {
                    if (c.Key > 0)
                        lstPubCode.SelectedItems.Add(c);
                }
                lstPubCode.IsEnabled = false;

                if (myDataMap != null && myDataMap.TransformDataMapID > 0)
                {
                    cbMatch.SelectedValue = myDataMap.MatchType;
                    tbSource.Text = myDataMap.SourceData;
                    tbDesire.Text = myDataMap.DesiredData;
                    btnDelete.Tag = myDataMap.TransformDataMapID;
                }
                busyIcon.IsBusy = false;
            };
            bw.RunWorkerAsync(); 
        }

        public string MatchType
        {
            get { return cbMatch.Text; }
            set { cbMatch.Text = value; }
        }
        public string SourceData
        {
            get { return tbSource.Text; }
            set { tbSource.Text = value; }
        }
        public string DesireData
        {
            get { return tbDesire.Text; }
            set { tbDesire.Text = value; }
        }
        public string ButtonTag
        {
            get
            {
                if (btnDelete.Tag != null)
                    return btnDelete.Tag.ToString();
                else
                    return "";
            }
            set { btnDelete.Tag = value; }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (btnDelete.Tag == null || string.IsNullOrEmpty(btnDelete.Tag.ToString()))
            {
                var sp = (StackPanel)this.Parent;
                sp.Children.Remove(this);
            }
            else
            {
                int TransformDataMapID = 0;
                int.TryParse(btnDelete.Tag.ToString(), out TransformDataMapID);
                tdmData.Proxy.Delete(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, TransformDataMapID);
                Core_AMS.Utilities.WPF.Message("Deleted change value row.", MessageBoxButton.OK, MessageBoxImage.Information, "Deletion Complete");

                var sp = (StackPanel)this.Parent;
                sp.Children.Remove(this);
            }
        }        
    }
}

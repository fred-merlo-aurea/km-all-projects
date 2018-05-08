using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using DQM.Helpers.DataLoader;
using FrameworkUAS.Object;

namespace DQM.Modules
{
    /// <summary>
    /// Interaction logic for DataLoader.xaml
    /// </summary>
    public partial class DataLoader : UserControl
    {
        public static string databaseTarget;
        public static string tableTarget;

        public DataLoader()
        {
            InitializeComponent();
            LoadDatabases();
        }
        private void LoadDatabases()
        {
            FrameworkServices.ServiceClient<UAD_WS.Interface.IDatabases> dbWorker = FrameworkServices.ServiceClient.UAD_DatabasesClient();
            List<FrameworkUAD.Entity.Databases> dbList = new List<FrameworkUAD.Entity.Databases>();
            dbList = dbWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
            List<string> allowed = new List<string>();

            foreach (FrameworkUAD.Entity.Databases s in dbList)
            {
                foreach(var c in AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients)
                {
                    if (c.ClientLiveDBConnectionString.ToLower().Contains(s.DatabaseName.ToLower()))
                    { 
                        allowed.Add(s.DatabaseName);
                        break;
                    }
                }
            }
            //Style s = this.FindResource("RadComboBoxItem") as Style;
            //cbDataBase.ItemContainerStyle = s;
            cbDataBase.ItemsSource = allowed;
        }

        private void StartDataLoad()
        {
            string filePath, fileName, circFiles;

            /*filePath = @"\\sapdev\data\PSD\BriefMedia\Staging\Loading\";
            fileName = "CHME13.csv";
            databaseTarget = "BriefMediaDB";
            tableTarget = "Briefmedia_Staging";
            circFiles = "false";*/
            //Console.Out.Write("reading file");

            FileInfo fi = new FileInfo(tbFile.Text);
            filePath = fi.FullName;//args[0];
            fileName = fi.Name;//args[1];
            databaseTarget = cbDataBase.SelectedValue.ToString();//args[2];
            tableTarget = tbTableTarget.Text;//args[3];
            circFiles = cbIsCirc.SelectedValue.ToString();//args[4];

            //FileInfo fi = new FileInfo(fileName);
            //Console.Out.Write(fileName);

            if (fi.Extension.ToLower() == ".txt" && circFiles.Equals("false"))
            {
                TextParser.textFileRead(filePath, fileName);
            }
            else if (fi.Extension.ToLower() == ".csv" && circFiles.Equals("false"))
            {
                TextParser.textAsCSVRead(filePath, fileName, true);
            }
            else if ((fi.Extension.ToLower() == ".xls" || fi.Extension.ToLower() == ".xlsx") && circFiles.Equals("false"))
            {
                ExcelParser.xlsReader(filePath, fileName);
            }
        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.FileName = "Document"; // Default file name
            //dlg.DefaultExt = ".txt"; // Default file extension
            //dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                tbFile.Text = filename;
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbFile.Text))
            {
                if (!string.IsNullOrEmpty(tbTableTarget.Text))
                {
                    if (cbDataBase.SelectedItem != null)
                    {
                        if (cbIsCirc.SelectedItem != null)
                            StartDataLoad();
                        else
                            Core_AMS.Utilities.WPF.Message("Please select if this is a Circ file or not.", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        Core_AMS.Utilities.WPF.Message("Please select a database.", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    Core_AMS.Utilities.WPF.Message("Please enter a table name. Typically 'CustomerName_Staging'.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                Core_AMS.Utilities.WPF.Message("Please select a file.", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using KM.Common.Import;
using KM.Common;

namespace DQM.Modules
{
    /// <summary>
    /// Interaction logic for UAD_FileViewer.xaml
    /// </summary>
    public partial class UAD_FileViewer : UserControl
    {
        public UAD_FileViewer()
        {
            InitializeComponent();
            foreach (Enums.ColumnDelimiter dl in (Enums.ColumnDelimiter[])Enum.GetValues(typeof(Enums.ColumnDelimiter)))
            {
                cbDelimiter.Items.Add(dl.ToString().Replace("_", " "));
            }

            cbTextQualifier.Items.Add("True");
            cbTextQualifier.Items.Add("False");
        }

        string myFile;
        FileConfiguration myFileConfig;
        bool needsFileConfig;

        private void ButtonSelectFile_Click(object sender, RoutedEventArgs e)
        {            
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name            
            dlg.Filter = "Recognized Files(*.txt;*.csv;*.xls;*.xlsx;*.dbf)|*.txt;*.csv;*.xls;*.xlsx;*.dbf"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                string fileName = dlg.FileName;
                TextboxFile.Text = System.IO.Path.GetFileName(fileName);                

                myFile = fileName;
                OpenFileConfiguration(myFile);
                ResetFileConfig();
            }
            else
                return;
        }

        private void ButtonRunViewer_Click(object sender, RoutedEventArgs e)
        {
            if(!isFileUploaded(new FileInfo(myFile)))            
                return;            
        }

        #region CustomMethods
        private bool isFileUploaded(FileInfo file)
        {
            bool canOpen;
            canOpen = false;

            try
            {
                using (File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    canOpen = true;
                }
            }
            catch
            {
                canOpen = false;
                Core_AMS.Utilities.WPF.MessageError(System.IO.Path.GetFileName(file.FullName) + " could not be opened. Is the file currently open? If so, please close and try again.");
            }

            return canOpen;
        }
        private void OpenFileConfiguration(string fileName)
        {
            if (System.IO.Path.GetExtension(fileName).ToLower() == ".csv" || System.IO.Path.GetExtension(fileName).ToLower() == ".txt")
            {
                LabelDelimiter.Visibility = Visibility.Visible;
                LabelTextQualifier.Visibility = Visibility.Visible;
                cbDelimiter.Visibility = Visibility.Visible;
                cbTextQualifier.Visibility = Visibility.Visible;
                needsFileConfig = true;
            }
            else
            {
                LabelDelimiter.Visibility = Visibility.Collapsed;
                LabelTextQualifier.Visibility = Visibility.Collapsed;
                cbDelimiter.Visibility = Visibility.Collapsed;
                cbTextQualifier.Visibility = Visibility.Collapsed;
                needsFileConfig = false;
            }
        }
        private void ResetFileConfig()
        {
            cbDelimiter.SelectedItem = -1;
            cbTextQualifier.SelectedItem = -1;
            myFileConfig = null;
        }
        #endregion
    }
}

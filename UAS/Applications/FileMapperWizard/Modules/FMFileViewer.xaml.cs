using Core_AMS.Utilities;
using FrameworkUAD.BusinessLogic;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using KM.Common.Import;
using CommonEnums = KM.Common.Enums;

namespace FileMapperWizard.Modules
{
    /// <summary>
    /// Interaction logic for FMFileViewer.xaml
    /// </summary>       

    public partial class FMFileViewer : UserControl
    {
        FrameworkServices.ServiceClient<UAD_WS.Interface.IImportVessel> ivBL = FrameworkServices.ServiceClient.UAD_ImportVesselClient();
        private ImportVessel iv = new ImportVessel();

        public FMFileViewer(FrameworkUAS.Object.AppData appData)
        {
            InitializeComponent();
            myAppData = appData;
            foreach (CommonEnums.ColumnDelimiter dl in (CommonEnums.ColumnDelimiter[])Enum.GetValues(typeof(CommonEnums.ColumnDelimiter)))
            {                
                cbDelimiter.Items.Add(dl.ToString().Replace("_", " "));
            }

            cbTextQualifier.Items.Add("True");
            cbTextQualifier.Items.Add("False");
        }

        FrameworkUAS.Object.AppData myAppData;
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
            myFileConfig = new FileConfiguration();
            if (System.IO.Path.GetExtension(myFile).ToLower() == ".csv" || System.IO.Path.GetExtension(myFile).ToLower() == ".txt")
            {
                if (cbDelimiter.SelectedValue != null)
                    myFileConfig.FileColumnDelimiter = cbDelimiter.SelectedValue.ToString();
                else
                {
                    Core_AMS.Utilities.WPF.MessageError("Missing Delimiter. Please select.");
                    return;
                }

                if (cbTextQualifier.SelectedValue != null)
                {
                    bool isQuote = false;
                    bool.TryParse(cbTextQualifier.SelectedValue.ToString(), out isQuote);
                    myFileConfig.IsQuoteEncapsulated = isQuote;
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageError("Missing extension. Please select.");
                    return;
                }

            }
            if (!isFileUploaded(new FileInfo(myFile)))
                return;
            else
            {
                busyIcon.IsBusy = true;
                BackgroundWorker bw = new BackgroundWorker();
                object data = new object();
                bw.DoWork += (o, ea) =>
                {                    
                    data = GetFile(new FileInfo(myFile), myFileConfig);
                };
                bw.RunWorkerCompleted += (o, ea) =>
                {
                    gridviewFileData.ItemsSource = data;
                    busyIcon.IsBusy = false;
                };
                bw.RunWorkerAsync();
                
            }
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
        /*
         * This will run through batching and insert records into the radgrid (appends data)        
         */
        private object GetFile(FileInfo importFile, FileConfiguration fileConfig)
        {            
            object a = new object();
            FrameworkUAD.Object.ImportVessel dataIV;
            FileWorker fileWorker = new FileWorker();
            int fileRowProcessedCount = 0;
            int fileRowBatch = 1000;
            int fileTotalRowCount = fileWorker.GetRowCount(importFile);
            while (fileRowProcessedCount < fileTotalRowCount)
            {
                int endRow = fileRowProcessedCount + fileRowBatch;
                if (endRow > fileTotalRowCount)
                    endRow = fileTotalRowCount;
                int startRow = fileRowProcessedCount + 1;

                dataIV = iv.GetImportVessel(importFile, startRow, fileRowBatch, fileConfig);
                fileRowProcessedCount += dataIV.TotalRowCount;

                a = dataIV.DataOriginal.AsDataView();

                dataIV.DataOriginal.Dispose();
            }
            return a;
        }
        #endregion
    }
}

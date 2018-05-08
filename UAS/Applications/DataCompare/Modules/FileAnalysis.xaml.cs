using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace DataCompare.Modules
{
    /// <summary>
    /// Interaction logic for FileAnalysis.xaml
    /// </summary>
    public partial class FileAnalysis : UserControl
    {
        public DataTable FileDataTable { get; set; }
        public FileAnalysis()
        {
            InitializeComponent();
        }

        #region Export
        void btnExport_Click(object sender, RoutedEventArgs e)
        {
            Telerik.Windows.Controls.RadButton button = (Telerik.Windows.Controls.RadButton)sender;
            switch (button.CommandParameter.ToString())
            {
                case "Excel":
                    ExcelExport();
                    break;
                case "CSV":
                    CsvExport();
                    break;
                case "TXT":
                    TxtExport();
                    break;
                case "PDF":
                    PdfExport();
                    break;
            }
        }
        void ExcelExport()
        {
            string extension = "xlsx";
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog()
            {
                DefaultExt = extension,
                Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, "Excel"),
                FilterIndex = 1
            };
            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    gridFile.ExportToXlsx(stream,
                     new GridViewDocumentExportOptions()
                     {
                         ShowColumnHeaders = true,
                         ShowColumnFooters = true,
                         ShowGroupFooters = false,
                     });
                }
            }
        }
        void CsvExport()
        {
            string extension = "csv";
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog()
            {
                DefaultExt = extension,
                Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, "Text"),
                FilterIndex = 1
            };
            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    gridFile.Export(stream,
                     new GridViewExportOptions()
                     {
                         Format = ExportFormat.Csv,
                         ShowColumnHeaders = true,
                         ShowColumnFooters = true,
                         ShowGroupFooters = false,
                     });
                }
            }
            
        }
        void TxtExport()
        {
            string extension = "txt";
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog()
            {
                DefaultExt = extension,
                Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, "Text"),
                FilterIndex = 1
            };
            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    gridFile.Export(stream,
                     new GridViewExportOptions()
                     {
                         Format = ExportFormat.Text,
                         ShowColumnHeaders = true,
                         ShowColumnFooters = true,
                         ShowGroupFooters = false,
                     });
                }
            }

        }
        void PdfExport()
        {
            string extension = "pdf";
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog()
            {
                DefaultExt = extension,
                Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, "Text"),
                FilterIndex = 1
            };
            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    gridFile.Export(stream,
                     new GridViewExportOptions()
                     {
                         Format = ExportFormat.Csv,
                         ShowColumnHeaders = true,
                         ShowColumnFooters = true,
                         ShowGroupFooters = false,
                     });
                }
            }

        }
        #endregion

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name            
            dlg.Filter = "Recognized Files(*.txt;*.csv;*.xls;*.xlsx;*.dbf)|*.txt;*.csv;*.xls;*.xlsx;*.dbf"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            if (!string.IsNullOrEmpty(dlg.FileName))
            {
                string fileName = dlg.FileName;
                txtFileName.Text = fileName;
                FileInfo file = new FileInfo(fileName);
                if (Core_AMS.Utilities.FileFunctions.IsFileLocked(file) == false)
                {
                    // Process open file dialog box results 
                    if (result == true)
                    {
                        #region Reload UserAuthorization
                        RadBusy.IsBusy = true;
                        BackgroundWorker bw = new BackgroundWorker();
                        FileDataTable = new DataTable();
                        bw.DoWork += (o, ea) =>
                        {
                            Core_AMS.Utilities.FileWorker fw = new Core_AMS.Utilities.FileWorker();

                            FileDataTable = fw.GetData(file);
                            ea.Result = FileDataTable;
                        };
                        bw.RunWorkerCompleted += (o, ea) =>
                        {
                            gridFile.ItemsSource = ea.Result;
                            SetRecordCount();
                            RadBusy.IsBusy = false;
                        };
                        bw.RunWorkerAsync();
                        #endregion
                    }
                }
                else
                    Core_AMS.Utilities.WPF.MessageFileOpenWarning();
            }
            else
                return;
        }
        private void SetRecordCount()
        {
            lbRecordCount.Content = "Record Count: " + gridFile.Items.Count.ToString("#,##0");
        }
        //private void gridFile_AutoGeneratingColumn(object sender, GridViewAutoGeneratingColumnEventArgs e)
        //{
        //    //if (addedCountFooter == false)
        //    //{
        //    //    addedCountFooter = true;

        //    //    CountFunction f = new CountFunction();
        //    //    f.Caption = "Total Records: ";
        //    //    e.Column.AggregateFunctions.Add(f);
        //    //}
        //}

        private void gridFile_Filtered(object sender, Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs e)
        {
            SetRecordCount();
        }
    }
}

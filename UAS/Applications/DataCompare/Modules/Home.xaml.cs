using FileMapperWizard.Modules;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Core_AMS.Utilities;
using DQM.Modules;
using FileMapperWizard.Windows;
using FrameworkUAS.Object;
using KMPlatform.BusinessLogic;
using Enums = KMPlatform.BusinessLogic.Enums;

namespace DataCompare.Modules
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        private const string SpModuleDockPanelChildName = "spModule";
        private const string EditExistingFileWindowTitle = "Edit Existing File";
        private const string NewMappingWindowTitle = "New Mapping";
        private const string EditFileWindowTitle = "Edit File";

        public Home(FrameworkUAS.Object.AppData appData)
        {
            InitializeComponent();
        }
        private void tile_MouseEnter(object sender, MouseEventArgs e)
        {
            Border me = sender as Border;
            //Type thisType = this.GetType();
            Rectangle rect = (Rectangle)this.FindName(me.Tag.ToString());
            rect.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#063E72"));
            rect.StrokeThickness = 4;
        }

        private void tile_MouseLeave(object sender, MouseEventArgs e)
        {
            Border me = sender as Border;
            Rectangle rect = (Rectangle)this.FindName(me.Tag.ToString());
            rect.StrokeThickness = 0;
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            DisplayWindow(border?.Tag.ToString());
        }

        private void Tile_Open(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DisplayWindow(button?.Tag.ToString());
        }

        private void DisplayWindow(string windowTag)
        {
            switch (windowTag)
            {
                //FileMappingWizard
                case "newMappingTile":
                    DisplayNewMappingTileWindow();
                    break;
                //FileMappingWizard
                case "editExistingTile":
                    DisplayEditExistingTileWindow();
                    break;
                case "uploadTile":
                    AddChildren<ADMS_FTP_FileUpload>();
                    break;
                case "viewTile":
                    AddChildren<CompareViewer>();
                    break;
                case "fileAnalysis":
                    AddChildren<FileAnalysis>();
                    break;
            }
        }

        private DockPanel FindChild()
        {
            var wMain = WPF.GetMainWindow();
            var spMain = WPF.FindChild<DockPanel>(wMain, SpModuleDockPanelChildName);
            spMain.Children.Clear();
            return spMain;
        }

        private void AddChildren<T>()
        {
            try
            {
                var spMain = FindChild();

                if (typeof(T) == typeof(CompareViewer))
                {
                    spMain.Children.Add(new CompareViewer());
                }
                else if (typeof(T) == typeof(ADMS_FTP_FileUpload))
                {
                    spMain.Children.Add(new ADMS_FTP_FileUpload());
                }
                else if (typeof(T) == typeof(FileAnalysis))
                {
                    spMain.Children.Add(new FileAnalysis());
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                LogCriticalError(ex);
            }
        }

        private void DisplayEditExistingTileWindow()
        {
            try
            {
                var newMappingWindow = new FMWindow(EditExistingFileWindowTitle);
                var stackPanel = new StackPanel();
                stackPanel.Children.Clear();
                var editFile = new FMUniversal(true, false, null, null, string.Empty, true);
                stackPanel.Children.Add(editFile);
                newMappingWindow.ReplaceContent(stackPanel);
                newMappingWindow.Title = EditFileWindowTitle;
                newMappingWindow.SizeToContent = SizeToContent.WidthAndHeight;
                newMappingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                newMappingWindow.ShowDialog();
                newMappingWindow.Activate();
            }
            catch (Exception ex)
            {
                LogCriticalError(ex);
            }
        }

        private void DisplayNewMappingTileWindow()
        {
            try
            {
                var newMappingWindow = new FMWindow(NewMappingWindowTitle);
                var stackPanel = new StackPanel();
                stackPanel.Children.Clear();
                var fmLite = new FMUniversal(false, false, null, null, string.Empty, true);
                stackPanel.Children.Add(fmLite);
                newMappingWindow.ReplaceContent(stackPanel);
                newMappingWindow.Title = NewMappingWindowTitle;
                newMappingWindow.SizeToContent = SizeToContent.WidthAndHeight;
                newMappingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                newMappingWindow.Activate();
                newMappingWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                LogCriticalError(ex);
            }
        }

        private void LogCriticalError(Exception ex)
        {
            var app = Enums.Applications.Data_Compare;

            if (AppData.myAppData != null
                && AppData.myAppData.CurrentApp != null)
            { 
                app = Enums.GetApplication(AppData.myAppData.CurrentApp
                    .ApplicationName);
            }

            var alWorker = new ApplicationLog();
            var formatException = StringFunctions.FormatException(ex);
            alWorker.LogCriticalError(formatException, GetType().Name + ".Border_MouseUp", app,
                string.Empty);
        }
    }
}

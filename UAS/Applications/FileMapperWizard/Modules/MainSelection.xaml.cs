using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace FileMapperWizard.Modules
{
    /// <summary>
    /// Interaction logic for MainSelection.xaml
    /// </summary>
    public partial class MainSelection : UserControl
    {
        #region Variables
        public static FrameworkUAS.Object.AppData myAppData { get; set; }        
        #endregion

        public MainSelection(FrameworkUAS.Object.AppData appData)
        {
            InitializeComponent();
            myAppData = appData;                                              
            InitialLoad();                
        }

        #region Control Loads
        public void InitialLoad()
        {
            grdCustomTiles.Visibility = Visibility.Visible;      
        }                    
        #endregion      
        
        #region FMLite
        private void tlFMLite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tile tl = (Tile)e.AddedItems[0] as Tile;
            
            if(tl.Name.Equals("tileNewMap"))
            {
                //FMWindow newMappingWindow = new FMWindow();
                Windows.FMWindow newMappingWindow = new Windows.FMWindow("New Mapping");
                StackPanel stackPanel = new StackPanel();
                stackPanel.Children.Clear();
                FMUniversal fmLite = new FMUniversal(false);
                stackPanel.Children.Add(fmLite);
                newMappingWindow.spContent.Children.Add(stackPanel);
                newMappingWindow.Title = "New Mapping";
                newMappingWindow.SizeToContent = SizeToContent.WidthAndHeight;
                newMappingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                newMappingWindow.ShowDialog();
                newMappingWindow.Activate();
            }
            else if(tl.Name.Equals("tileEditMap"))
            {
                
            }
            else if(tl.Name.Equals("tileView"))
            {
                Windows.FMWindow newMappingWindow2 = new Windows.FMWindow("View File");
                StackPanel stackPanel2 = new StackPanel();
                stackPanel2.Children.Clear();
                FMFileViewer fmLite2 = new FMFileViewer(myAppData);
                stackPanel2.Children.Add(fmLite2);
                newMappingWindow2.spContent.Children.Add(stackPanel2);
                newMappingWindow2.Title = "View File";
                newMappingWindow2.SizeToContent = SizeToContent.WidthAndHeight;
                newMappingWindow2.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                newMappingWindow2.ShowDialog();
                newMappingWindow2.Activate();
            }
        }

        private void tile_MouseEnter(object sender, MouseEventArgs e)
        {
            Border me = sender as Border;
            Type thisType = this.GetType();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button me = sender as Button;
            Windows.FMWindow newMappingWindow = new Windows.FMWindow("New Mapping");
            StackPanel stackPanel = new StackPanel();
            switch (me.Tag.ToString())
            {

                case "newMappingTile":
                    newMappingWindow = new Windows.FMWindow("New Mapping");
                    stackPanel = new StackPanel();
                    stackPanel.Children.Clear();
                    FMUniversal fmLite = new FMUniversal(false);
                    stackPanel.Children.Add(fmLite);
                    newMappingWindow.spContent.Children.Add(stackPanel);
                    newMappingWindow.Title = "New Mapping";
                    newMappingWindow.SizeToContent = SizeToContent.WidthAndHeight;
                    newMappingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    newMappingWindow.ShowDialog();
                    newMappingWindow.Activate();
                    break;
                case "viewTile":
                    newMappingWindow = new Windows.FMWindow("View File");
                    stackPanel = new StackPanel();
                    stackPanel.Children.Clear();
                    FMFileViewer fmViewer = new FMFileViewer(myAppData);
                    stackPanel.Children.Add(fmViewer);
                    newMappingWindow.spContent.Children.Add(stackPanel);
                    newMappingWindow.Title = "View File";
                    newMappingWindow.SizeToContent = SizeToContent.WidthAndHeight;
                    newMappingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    newMappingWindow.ShowDialog();
                    newMappingWindow.Activate();
                    break;
                case "editExistingTile":
                    newMappingWindow = new Windows.FMWindow("Edit Existing File");
                    stackPanel = new StackPanel();
                    stackPanel.Children.Clear();
                    //EditExistingFile editFile = new EditExistingFile(myAppData);
                    FMUniversal editFile = new FMUniversal(true);
                    stackPanel.Children.Add(editFile);
                    newMappingWindow.spContent.Children.Add(stackPanel);
                    newMappingWindow.Title = "Edit File";
                    newMappingWindow.SizeToContent = SizeToContent.WidthAndHeight;
                    newMappingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    newMappingWindow.ShowDialog();
                    newMappingWindow.Activate();
                    break;
                case "transformTile":
                    newMappingWindow = new Windows.FMWindow("Edit Existing Transformation");
                    stackPanel = new StackPanel();
                    stackPanel.Children.Clear();
                    EditTransformation editTransformation = new EditTransformation(myAppData);
                    stackPanel.Children.Add(editTransformation);
                    newMappingWindow.spContent.Children.Add(stackPanel);
                    newMappingWindow.Title = "View Transformations";
                    newMappingWindow.SizeToContent = SizeToContent.WidthAndHeight;
                    newMappingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    newMappingWindow.ShowDialog();
                    newMappingWindow.Activate();
                    break;
                case "validateTile":
                    newMappingWindow = new Windows.FMWindow("Validate File");
                    stackPanel = new StackPanel();
                    stackPanel.Children.Clear();
                    FMValidator validate = new FMValidator(myAppData);
                    stackPanel.Children.Add(validate);
                    newMappingWindow.spContent.Children.Add(stackPanel);
                    newMappingWindow.Title = "Validate File";
                    newMappingWindow.SizeToContent = SizeToContent.WidthAndHeight;
                    newMappingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    newMappingWindow.ShowDialog();
                    newMappingWindow.Activate();
                    break;
                case "statusTile":
                    newMappingWindow = new Windows.FMWindow("File Status");
                    stackPanel = new StackPanel();
                    stackPanel.Children.Clear();
                    FileStatus status = new FileStatus(myAppData);
                    stackPanel.Children.Add(status);
                    newMappingWindow.spContent.Children.Add(stackPanel);
                    newMappingWindow.Title = "File Status";
                    newMappingWindow.SizeToContent = SizeToContent.WidthAndHeight;
                    newMappingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    newMappingWindow.ShowDialog();
                    newMappingWindow.Activate();
                    break;
                case "fileAuditTile":
                    newMappingWindow = new Windows.FMWindow("File Audit");
                    stackPanel = new StackPanel();
                    stackPanel.Children.Clear();
                    FileAudit audit = new FileAudit(myAppData);
                    stackPanel.Children.Add(audit);
                    newMappingWindow.spContent.Children.Add(stackPanel);
                    newMappingWindow.Title = "File Audit";
                    newMappingWindow.SizeToContent = SizeToContent.WidthAndHeight;
                    newMappingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    newMappingWindow.ShowDialog();
                    newMappingWindow.Activate();
                    break;
                case "fileLogViewerTile":
                    newMappingWindow = new Windows.FMWindow("File Log Viewer");
                    stackPanel = new StackPanel();
                    stackPanel.Children.Clear();
                    FileLogViewer fileLog = new FileLogViewer(myAppData);
                    stackPanel.Children.Add(fileLog);
                    newMappingWindow.spContent.Children.Add(stackPanel);
                    newMappingWindow.Title = "File Log Viewer";
                    newMappingWindow.SizeToContent = SizeToContent.WidthAndHeight;
                    newMappingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    newMappingWindow.ShowDialog();
                    newMappingWindow.Activate();
                    break;
            }
        }
        #endregion
    }
}

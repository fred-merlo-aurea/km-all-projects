using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace FileMapperWizard.Windows
{
    /// <summary>
    /// Interaction logic for FMLiteWindow.xaml
    /// </summary>
    public partial class FMWindow : Window
    {
        public FMWindow(string tile)
        {
            InitializeComponent();
            
            switch(tile)
            {
                case "New Mapping":
                    headerImg.Source = new BitmapImage(new Uri("/ImageLibrary;Component/Images/TileList/MapIcon.png", UriKind.Relative));
                    txtTitle.Text = "Map a New File: ";
                    txtSub.Text = "Step by Step Instruction";
                    break;
                case "View File":
                    headerImg.Source = new BitmapImage(new Uri("/ImageLibrary;Component/Images/TileList/ViewFile.png", UriKind.Relative)); 
                    txtTitle.Text = "View File: ";
                    txtSub.Text = "View & Sort File Contents";
                    break;
                case "Edit Existing File":
                    headerImg.Source = new BitmapImage(new Uri("/ImageLibrary;Component/Images/TileList/EditExitingIcon.png", UriKind.Relative)); 
                    txtTitle.Text = "Edit File: ";
                    txtSub.Text = "Edit Existing File Mapping";
                    break;
                case "Edit Existing Transformation":
                    headerImg.Source = new BitmapImage(new Uri("/ImageLibrary;Component/Images/TileList/EditExistingTransformation.png", UriKind.Relative)); 
                    txtTitle.Text = "Transformations: ";
                    txtSub.Text = "View & Delete Existing Transformations";
                    break;
                case "Validate File":
                    headerImg.Source = new BitmapImage(new Uri("/ImageLibrary;Component/Images/TileList/Validating.png", UriKind.Relative));
                    txtTitle.Text = "Validate File: ";
                    txtSub.Text = "Validate Existing File";
                    break;
                case "File Status":
                    headerImg.Source = new BitmapImage(new Uri("/ImageLibrary;Component/Images/TileList/FileStatus.png", UriKind.Relative));
                    txtTitle.Text = "File Status ";
                    txtSub.Text = "";
                    break;
                case "Data Compare":
                    headerImg.Source = new BitmapImage(new Uri("/ImageLibrary;Component/Images/Compare/compare-72.png", UriKind.Relative));
                    txtTitle.Text = "Data Compare";
                    txtSub.Text = "";
                    break;
                case "File Audit":
                    headerImg.Source = new BitmapImage(new Uri("/ImageLibrary;Component/Images/Search/SearchWhite-72.png", UriKind.Relative));
                    txtTitle.Text = "File Audit";
                    txtSub.Text = "";
                    break;
                case "File Log Viewer":
                    headerImg.Source = new BitmapImage(new Uri("/ImageLibrary;Component/Images/Data/View_Details-128.png", UriKind.Relative));
                    txtTitle.Text = "File Log Viewer";
                    txtSub.Text = "";
                    break;

            }
            
        }
        public void ReplaceContent(StackPanel content)
        {
            spContent.Children.Clear();
            spContent.Children.Add(content);
        }
        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

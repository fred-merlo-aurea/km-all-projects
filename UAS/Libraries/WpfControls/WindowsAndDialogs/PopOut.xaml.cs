using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Interop;

namespace WpfControls.WindowsAndDialogs
{
    /// <summary>
    /// Interaction logic for PopOut.xaml
    /// </summary>
    public partial class PopOut : Window
    {
        private UserControl control;
        public PopOut(KMPlatform.BusinessLogic.Enums.Applications app, UserControl uc)
        {
            InitializeComponent();

            InitializeComponent();
            control = uc;
            control.Name = "mainControl";

            bannerSetUp(app);
            spModule.Children.Add(control);   
        }

        #region ButtonMethods
        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool max;
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
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

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        #endregion

        private void bannerSetUp(KMPlatform.BusinessLogic.Enums.Applications app)
        {
            spModule.Children.Clear();
            spModule.Visibility = Visibility.Visible;

            if (app == KMPlatform.BusinessLogic.Enums.Applications.Circulation)
                this.SystemBanner.Source = new BitmapImage(new Uri(@"/ImageLibrary;component/Images/Banners/CirculationRevised.jpg", UriKind.RelativeOrAbsolute));
            else if (app == KMPlatform.BusinessLogic.Enums.Applications.UAD)
                this.SystemBanner.Source = new BitmapImage(new Uri(@"/ImageLibrary;component/Images/Banners/UADAMSRevised.jpg", UriKind.RelativeOrAbsolute));
            else
                this.SystemBanner.Source = new BitmapImage(new Uri(@"/ImageLibrary;component/Images/Banners/TempSolidBlue.jpg", UriKind.RelativeOrAbsolute));

            this.Width = 1090;
            this.Height = 780;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            //this.Left = ()
            //PointFromScreen()
            if (System.Windows.Forms.SystemInformation.MonitorCount > 1)
            {
                System.Drawing.Rectangle workingArea = GetScreenFrom(this);
                this.Left = (workingArea.Width - this.Width) / 2 + workingArea.Left;
                this.Top = (workingArea.Height - this.Height) / 2 + workingArea.Top;
            }
            else
            {
                this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2 + SystemParameters.WorkArea.Left;
                this.Top = (SystemParameters.WorkArea.Height - this.Height) / 2 + SystemParameters.WorkArea.Top;
            }

            TitleBarImage.Visibility = Visibility.Visible;
            TitleBarImage.Source = new BitmapImage(new Uri(@"/ImageLibrary;component/Images/Banners/TempSolidBlue.jpg", UriKind.RelativeOrAbsolute));
        }

        public static System.Drawing.Rectangle GetScreenFrom(Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
            return screen.WorkingArea;
        }
    }
}

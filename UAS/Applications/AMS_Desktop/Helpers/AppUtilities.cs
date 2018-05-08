using System;
using System.Linq;
using System.Windows;

namespace AMS_Desktop.Helpers
{
    class AppUtilities
    {
        #region Main Application Window
        public static Windows.Home GetMainWindow(DependencyObject depObj)
        {
            Window w = Window.GetWindow(depObj);//MainWindow
            Windows.Home mw = (Windows.Home)w;//Application.Current.MainWindow;
            return mw;
        }
        public static Windows.Home GetMainWindow()
        {
            Windows.Home mw = (Windows.Home)Application.Current.MainWindow;
            return mw;
        }
        #endregion

        private static FrameworkUAS.Object.AppData _appDataCirc;
        public static FrameworkUAS.Object.AppData myAppDataCirc
        {
            get
            {
                if (_appDataCirc == null)
                {
                    FrameworkUAS.Object.AppData ad = new FrameworkUAS.Object.AppData();
                    _appDataCirc = ad;
                    return _appDataCirc;
                }
                else
                    return _appDataCirc;
            }
            set
            {
                _appDataCirc = value;
            }
        }
    }
}

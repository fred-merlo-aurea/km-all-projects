using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;

namespace AMS_Desktop.Windows
{
    /// <summary>
    /// Interaction logic for ThemeSelector.xaml
    /// </summary>
    public partial class ThemeSelector : Window
    {
        System.Configuration.Configuration config;

        public ThemeSelector()
        {
            InitializeComponent();
            
        }

        private void cbThemes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string theme = cbThemes.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
            config.AppSettings.Settings["UserSelectedTheme"].Value = theme;
            config.Save(ConfigurationSaveMode.Full);


            string uri = "/Themes/" + theme + ".xaml";

            Application.Current.Resources.Source = new Uri(uri, UriKind.RelativeOrAbsolute);
        }
    }
}

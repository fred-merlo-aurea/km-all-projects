using System;
using System.Linq;
using System.Windows;

namespace UAD_Explorer.Windows
{
    /// <summary>
    /// Interaction logic for FileLogMenuOptions.xaml
    /// </summary>
    public partial class FileLogMenuOptions : Window
    {
        public FileLogMenuOptions(string tag)
        {
            InitializeComponent();
            LoadData(tag);
        }

        public event Action<string> Check;

        private void LoadData(string tag)
        {
            switch(tag)
            {
                case "Search":
                    gridSearch.Visibility = System.Windows.Visibility.Visible;
                    break;
                case "Save":
                    break;
            }
        }

        private void rBtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxSearch.Text != null && txtBoxSearch.Text != "")
                Check(txtBoxSearch.Text);
            this.Close();
        }

        private void rBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

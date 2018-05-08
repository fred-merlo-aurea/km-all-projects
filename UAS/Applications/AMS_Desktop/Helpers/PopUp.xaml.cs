using System;
using System.Linq;
using System.Windows;

namespace AMS_Desktop.Helpers
{
    /// <summary>
    /// Interaction logic for PopUp.xaml
    /// </summary>
    public partial class PopUp : Window
    {
        public PopUp(string content)
        {
            InitializeComponent();
            txtContent.Text = content;
        }
    }
}

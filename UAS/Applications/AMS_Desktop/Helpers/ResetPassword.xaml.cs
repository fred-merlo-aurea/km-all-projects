using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AMS_Desktop.Helpers
{
    /// <summary>
    /// Interaction logic for ResetPassword.xaml
    /// </summary>
    public partial class ResetPassword : Page
    {
        public ResetPassword(string navigateToString)
        {
            InitializeComponent();
            //wbResetPw.NavigateToString(navigateToString);
            wbResetPw.Navigate(navigateToString);
        }
    }
}

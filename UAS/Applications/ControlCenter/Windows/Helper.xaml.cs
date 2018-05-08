using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ControlCenter.Windows
{
    /// <summary>
    /// Interaction logic for Helper.xaml
    /// </summary>
    public partial class Helper : Window
    {
        public Helper()
        {
            InitializeComponent();
        }

        public void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}

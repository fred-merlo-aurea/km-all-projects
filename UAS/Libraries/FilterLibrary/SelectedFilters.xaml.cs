using System;
using System.Linq;
using System.Windows.Controls;

namespace FilterControls
{
    /// <summary>
    /// Interaction logic for SelectedFilters.xaml
    /// </summary>
    public partial class SelectedFilters : UserControl
    {
        public SelectedFilters()
        {
            InitializeComponent();
        }

        private void btnFilters_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Framework.FilterObject fo = btn.DataContext as Framework.FilterObject;
            fo.RemoveSelection();
        }
    }
}

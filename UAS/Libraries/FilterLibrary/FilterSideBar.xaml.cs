using FilterControls.Framework;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FilterControls
{
    /// <summary>
    /// Interaction logic for FilterSideBar.xaml
    /// </summary>
    public partial class FilterSideBar : UserControl
    {
        #region Classes
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(Framework.Filters), typeof(FilterSideBar), new UIPropertyMetadata(null));
        public Framework.Filters Filter
        {
            get { return (Framework.Filters)GetValue(FilterProperty); }
            set
            {
                SetValue(FilterProperty, value);
            }
        }
        #endregion
        public FilterSideBar()
        {
            InitializeComponent();
        }
        private void Tab_Selected(object sender, RoutedEventArgs e)
        {
            //Button btn = sender as Button;
            //IFilters tab = btn.DataContext as IFilters;
            //if(tab != null)
            //{
            //    this.Filter = tab;
            //}
        }
        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowFilter(sender);
        }
        private void Button_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl)) && Keyboard.IsKeyDown(Key.S))
            {
                ShowFilter(sender);
            }

            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl)) && Keyboard.IsKeyDown(Key.D))
            {
                Detach(sender);   
            }
        }
        private void ShowFilter(object sender)
        {
            Button btn = sender as Button;
            Filters tab = btn.DataContext as Filters;
            if (tab != null)
            {
                this.Filter = tab;
            }
        }
        private void Detach(object sender)
        {
            Button btn = sender as Button;
            Filters tab = btn.DataContext as Filters;
            if (tab != null)
            {
                this.Filter = tab;
            }

            if (this.Filter != null)
            {
                this.Filter.Detached = true;
                PopFilterWindow pop = new PopFilterWindow(this.Filter);
                pop.Closed += (o, i) =>
                {
                    pop.Filter.Detached = false;
                };
                pop.Show();
                this.Filter = null;
            }
        }
        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Detach(sender);   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowFilter(sender);
        }

        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }
        
    }
}

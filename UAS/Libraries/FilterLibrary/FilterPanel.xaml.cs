using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FilterControls
{
    /// <summary>
    /// Interaction logic for FilterPanel.xaml
    /// </summary>
    public partial class FilterPanel : UserControl
    {      
        //We attached a new DependencyProperty called Filter so we could Bind to it on the UI. When the user leaves the Panel, we null the property and in turn, hide the Panel on the UI thanks to the binding.
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(Framework.Filters), typeof(FilterPanel), new UIPropertyMetadata(null));
        public Framework.Filters Filter
        {
            get { return (Framework.Filters)GetValue(FilterProperty); }
            set
            {
                SetValue(FilterProperty, value);
            }
        }
        public FilterPanel()
        {
            InitializeComponent();
        }
        Action emptyDelegate = delegate { };
        public void SetExpandersByHeader(Dictionary<string, bool> headerIsExpanded)
        {
            List<Expander> newExpanders = Core_AMS.Utilities.WPF.FindVisualChildren<Expander>(this).ToList();
            foreach (Expander ex in newExpanders)
            {
                if (ex.HasHeader == true)
                    if (headerIsExpanded.ContainsKey(ex.Header.ToString()))
                        ex.IsExpanded = headerIsExpanded[ex.Header.ToString()];
            }
            this.Dispatcher.Invoke(emptyDelegate, System.Windows.Threading.DispatcherPriority.Render);
        }
        private void Button_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Filter = null;
        }
        public void Detach()
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
        private void Button_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl)) && Keyboard.IsKeyDown(Key.D))
            {
                Detach();
            }

            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl)) && Keyboard.IsKeyDown(Key.H))
            {
                this.Filter = null;
            }
        }

        private void Button_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Detach();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Filter = null;
        }
        private void Button_MouseRightButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Detach();
        }
    }
}

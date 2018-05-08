using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ControlCenter.Windows
{
    /// <summary>
    /// Interaction logic for CodeSheetEditResponses.xaml
    /// </summary>
    public partial class CodeSheetEditResponses : Window
    {
        public CodeSheetEditResponses(string question, System.Windows.Controls.UserControl parent)
        {
            InitializeComponent();
            LoadData(question, parent);
        }

        private System.Windows.Controls.UserControl parentWindow;

        private void LoadData(string question, System.Windows.Controls.UserControl parent)
        {
            lbQuestion.Content = question;
            parentWindow = parent;
        }

        public void Window_Closing(object sender, CancelEventArgs e)
        {
            DoubleAnimation animationHeight = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(.4)
            };
            DoubleAnimation fadeIn = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(.4)
            };
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = animationHeight;
            anim.Completed += (s, _) =>
            {
                this.Close();
            };
            this.BeginAnimation(Rectangle.HeightProperty, animationHeight);
            parentWindow.BeginAnimation(Window.OpacityProperty, fadeIn);
        }

        private void rbCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_Response(object sender, RoutedEventArgs e)
        {
            StackPanel spBoxes = new StackPanel();
            spBoxes.Name = "spBox";
            spBoxes.Orientation = System.Windows.Controls.Orientation.Horizontal;
            System.Windows.Controls.TextBox tbCode = new System.Windows.Controls.TextBox();
            Thickness margin = tbCode.Margin;
            margin.Top = 5;
            margin.Left = 5;
            tbCode.Margin = margin;
            tbCode.Width = 90;
            System.Windows.Controls.TextBox tbName = new System.Windows.Controls.TextBox();
            tbName.Width = 150;
            tbName.Margin = margin;
            spBoxes.Children.Add(tbCode);
            spBoxes.Children.Add(tbName);
            lbResponses.Items.Add(spBoxes);
            lbResponses.ScrollIntoView(lbResponses.Items.Count - 1);
        }

        private void Change_Display_Order(object sender, RoutedEventArgs e)
        {
            if (btnDisplayOrder.Content.Equals("Change display order"))
            {
                btnDisplayOrder.Content = "Done";
                rbUp.Visibility = System.Windows.Visibility.Visible;
                rbDown.Visibility = System.Windows.Visibility.Visible;
                rbAdd.IsEnabled = false;
                lbResponses.SetResourceReference(Telerik.Windows.Controls.RadListBox.ItemContainerStyleProperty, "DraggableListBoxItem");
                Thickness margin = new Thickness();
                margin.Left = 0;
                margin.Top = 5;
                foreach (StackPanel sp in lbResponses.Items.OfType<StackPanel>())
                {
                    foreach (System.Windows.Controls.TextBox box in sp.Children.OfType<System.Windows.Controls.TextBox>())
                    {
                        box.Margin = margin;
                        box.IsReadOnly = true;
                    }
                }
            }
            else
            {
                btnDisplayOrder.Content = "Change display order";
                rbUp.Visibility = System.Windows.Visibility.Collapsed;
                rbDown.Visibility = System.Windows.Visibility.Collapsed;
                rbAdd.IsEnabled = true;
                Thickness margin = new Thickness();
                margin.Left = 5;
                margin.Top = 5;
                foreach (StackPanel sp in lbResponses.Items.OfType<StackPanel>())
                {
                    foreach (System.Windows.Controls.TextBox box in sp.Children.OfType<System.Windows.Controls.TextBox>())
                    {
                        box.Margin = margin;
                        box.IsReadOnly = false;
                    }
                }
            }
        }

        private void Response_Up(object sender, RoutedEventArgs e)
        {
            int index = lbResponses.SelectedIndex;
            if (index != 0 && index != -1)
            {
                StackPanel response = (StackPanel)lbResponses.SelectedItem;
                lbResponses.Items.RemoveAt(index);
                lbResponses.Items.Insert(index - 1, response);
                lbResponses.SelectedIndex = index - 1;
                lbResponses.ScrollIntoView(index - 1);
            }
            else
                return;
        }

        public void Response_Down(object sender, RoutedEventArgs e)
        {
            int index = lbResponses.SelectedIndex;
            if (index != lbResponses.Items.Count - 1 && index != -1)
            {
                StackPanel response = (StackPanel)lbResponses.SelectedItem;
                lbResponses.Items.RemoveAt(index);
                lbResponses.Items.Insert(index + 1, response);
                lbResponses.SelectedIndex = index + 1;
                lbResponses.ScrollIntoView(index + 1);
            }
            else
                return;
        }

        private void lbResponses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //StackPanel response = (StackPanel)lbResponses.SelectedItem;
            //var temp = lbResponses.Items.CurrentItem;
        }

        #region ArrowButtonChanges
        private void rbUp_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var uri = new Uri("pack://application:,,,/ImageLibrary;Component/Images/24/upSelect-24.png");
            var bitmap = new BitmapImage(uri);
            rbUp.Source = bitmap;
            Image im = (Image)sender;
            im.MouseLeave += (s, _) =>
            {
                var uri2 = new Uri("pack://application:,,,/ImageLibrary;Component/Images/24/up-24.png");
                var bitmap2 = new BitmapImage(uri2);
                rbUp.Source = bitmap2;
            };
        }

        private void rbDown_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var uri = new Uri("pack://application:,,,/ImageLibrary;Component/Images/24/downSelect-24.png");
            var bitmap = new BitmapImage(uri);
            rbDown.Source = bitmap;

            Image im = (Image)sender;
            im.MouseLeave += (s, _) =>
            {
                var uri2 = new Uri("pack://application:,,,/ImageLibrary;Component/Images/24/down-24.png");
                var bitmap2 = new BitmapImage(uri2);
                rbDown.Source = bitmap2;
            };
        }
        #endregion
    }
}

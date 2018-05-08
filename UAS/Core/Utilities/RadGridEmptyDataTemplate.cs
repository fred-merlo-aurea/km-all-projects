using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Telerik.Windows.Controls;
using System.Linq;

namespace Core_AMS.Utilities
{
    public class RadGridEmptyDataTemplate : Behavior<RadGridView>
    {
        ContentPresenter contentPresenter = new ContentPresenter();

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.LayoutUpdated += new EventHandler(AssociatedObject_LayoutUpdated);
        }

        public DataTemplate EmptyDataTemplate { get; set; }

        void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
        {
            Grid rootGrid = this.AssociatedObject.ChildrenOfType<Grid>().FirstOrDefault();

            if (rootGrid != null)
            {
                this.AssociatedObject.LayoutUpdated -= new EventHandler(AssociatedObject_LayoutUpdated);
                this.LoadTemplateIntoGridView(this.AssociatedObject);
                this.AssociatedObject.Items.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Items_CollectionChanged);
                SetVisibility();
            }
        }

        void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetVisibility();
        }

        private void SetVisibility()
        {
            if (this.AssociatedObject.ItemsSource != null)
            {
                if (this.AssociatedObject.Items.Count == 0)
                    this.contentPresenter.Visibility = Visibility.Visible;
                else
                    this.contentPresenter.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.contentPresenter.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadTemplateIntoGridView(RadGridView gridView)
        {
            contentPresenter.IsHitTestVisible = false;
            contentPresenter.DataContext = this;
            contentPresenter.ContentTemplate = this.EmptyDataTemplate;
            Grid rootGrid = gridView.FindChildByType<Grid>();//.ChildrenOfType<Grid>()[0];

            contentPresenter.SetValue(Grid.RowProperty, 2);
            contentPresenter.SetValue(Grid.RowSpanProperty, 2);
            contentPresenter.SetValue(Grid.ColumnSpanProperty, 2);
            contentPresenter.SetValue(Border.MarginProperty, new Thickness(0, 27, 0, 0));
            rootGrid.Children.Add(contentPresenter);
        }
    }
}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FilterControls.Framework
{
    //FilterObjectSelect determines which template to use when Binding collections of FilterObjects in XAML to an ItemsControl. This allows us to have control over how each type of derived FilterObject
    //appears on the UI. Make sure to add a new DataTemplate here for each new derived FilterObject you create.
    public class FilterObjectSelector : DataTemplateSelector
    {
        public DataTemplate ListObjectTemplate { get; set; }
        public DataTemplate ComboObjectTemplate { get; set; }
        public DataTemplate BoolObjectTemplate { get; set; }
        public DataTemplate RangeObjectTemplate { get; set; }
        public DataTemplate AdHocRangeObjectTemplate { get; set; }
        public DataTemplate AdHocStandardObjectTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Framework.ListFilterObject)
                return this.ListObjectTemplate;
            else if (item is Framework.ComboFilterObject)
                return this.ComboObjectTemplate;
            else if (item is Framework.BoolFilterObject)
                return this.BoolObjectTemplate;
            else if (item is Framework.RangeFilterObject)
                return this.RangeObjectTemplate;
            else if (item is Framework.AdHocStandardFilterObject)
                return this.AdHocStandardObjectTemplate;
            else if (item is Framework.AdHocRangeFilterObject)
                return this.AdHocRangeObjectTemplate;            
            return base.SelectTemplate(item, container);
        }
    }
}

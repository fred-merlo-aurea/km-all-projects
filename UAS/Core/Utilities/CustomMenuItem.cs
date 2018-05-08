using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Core_AMS.Utilities
{
    public class CustomMenuItem
    {
        public CustomMenuItem()
        {
            this.SubItems = new ObservableCollection<CustomMenuItem>();
        }
        public string Text
        {
            get;
            set;
        }
        public ObservableCollection<CustomMenuItem> SubItems
        {
            get;
            set;
        }
    }
}

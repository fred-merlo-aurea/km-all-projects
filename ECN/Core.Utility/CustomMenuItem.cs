using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities
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

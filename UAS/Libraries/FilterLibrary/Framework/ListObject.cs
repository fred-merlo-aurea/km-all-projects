using System;
using System.ComponentModel;
using System.Linq;

namespace FilterControls.Framework
{
    [Serializable]
    public class ListObject : INotifyPropertyChanged
    {
        private bool _selected = false;
        public string Value { get; set; }
        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    OnPropertyChanged("Selected");
                }
            }
        }
        public string DisplayValue { get; set; }
        public string ParentValue { get; set; }
        public ListObject() { }
        public ListObject(string display, string value, string parentValue = "")
        {
            this.DisplayValue = display;
            this.Value = value;
            this.Selected = false;
            this.ParentValue = parentValue;
        }
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}

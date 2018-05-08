using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace FilterControls.Framework
{
    public abstract class Filters : INotifyPropertyChanged
    {
        //Filters is the base class for representing a category of Filters (Standard, Demographics, etc).
        private bool _detached;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public abstract Framework.Enums.Filters FilterType { get; set; }
        public abstract string Title { get; set; }
        public abstract ObservableCollection<FilterObject> Objects { get; set; }
        //Detached property is set to True when we detach a Filter from the SideBar. When set to True, the UI hides the tab from the SideBar. No need to redefine in the derived classes as it has no other
        //special implementation.
        public bool Detached
        {
            get { return _detached; }
            set
            {
                _detached = value;
                OnPropertyChanged("Detached");
            }
        }
    }
}

using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FilterControls.Framework
{
    //FilterObject is the base abstract class for the various types of Filters the user can select. For every new derived FilterObject class you add, you must also add the XmlInclude tag as shown below.
    //This allows an XML Reader to serialize and deserialize the derived class (we need this functionality for saving and loading filters).
    [XmlInclude(typeof(ListFilterObject))]
    [XmlInclude(typeof(ComboFilterObject))]
    [XmlInclude(typeof(AdHocRangeFilterObject))]
    [XmlInclude(typeof(AdHocStandardFilterObject))]
    [XmlInclude(typeof(RangeFilterObject))]
    [XmlInclude(typeof(BoolFilterObject))]
    [Serializable]
    public abstract class FilterObject : INotifyPropertyChanged
    {
        //PropertyChanged is defined once in the abstract class. All derived classes call the base class implementation. It does not need to be defined in the derived class unless there is special
        //code that needs to be run for that particular derived class. Same goes for FilterType and Name.
        private bool _active;
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
            this.Active = CheckChanges();
        }
        public Framework.Enums.Filters FilterType { get; set; }
        public Framework.Enums.FilterObjects Name { get; set; }
        public string DisplayName { get; set; }
        //An Active filter is added to the ViewModel's ActiveFilters ObservableCollection. The ViewModel will listen to changes to filters to correctly add and remove filters from ActiveFilters.
        [IgnoreDataMember]
        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    _active = value;
                    OnPropertyChanged("Active");
                }
            }
        }
        //SelectedCriteria is a calculated field that is implemented by the derived classes. It returns the current options/text selected for your filter.
        [IgnoreDataMember]
        public abstract string SelectedCriteria { get; }
        //CheckChanges is implemented by the derived classes. It checks to see if a filter is considered Active.
        public abstract bool CheckChanges();
        //SetSelection is implemented by the derived classes. It sets filter criteria as defined by the FilterObject it takes in.
        public abstract void SetSelection(FilterObject fo, bool isLoad = false);
        //RemoveSelection is implemented by the derived classes. It removes all filter criteria currently selected.
        public abstract void RemoveSelection();
        //GetSelectionXml is implemented by the derviced classes. It returns the selected criteria in a light-weight XML format that is sent to SQL Server to return lists of subscribers.
        public abstract string GetSelectionXml();
        //We need an empty constructor for this base class and every derived class from this base class. It is a requirement for XML serializing/deserializing.
        public FilterObject() {}
    }
}

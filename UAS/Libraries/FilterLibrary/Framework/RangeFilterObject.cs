using System;
using System.Linq;

namespace FilterControls.Framework
{
    [Serializable]
    public class RangeFilterObject : FilterObject
    {
        private string _fromOption;
        private string _toOption;
        public string FromOption
        {
            get { return _fromOption; }
            set
            {
                _fromOption = value;
                OnPropertyChanged("FromOption");
                OnPropertyChanged("SelectedCriteria");
            }
        }
        public string ToOption
        {
            get { return _toOption; }
            set
            {
                _toOption = value;
                OnPropertyChanged("ToOption");
                OnPropertyChanged("SelectedCriteria");
            }
        }
        public RangeFilterObject(Enums.Filters type, Enums.FilterObjects name, string display)
        {
            this.Name = name;
            this.FilterType = type;
            this.DisplayName = display;
        }
        public RangeFilterObject() { }
        public override bool CheckChanges()
        {
            if (!string.IsNullOrEmpty(FromOption) && !string.IsNullOrEmpty(ToOption))
                return true;
            return false;
        }
        public override string SelectedCriteria
        {
            get
            {
                if (!string.IsNullOrEmpty(FromOption) && !string.IsNullOrEmpty(ToOption))
                    return FromOption + " To " + ToOption;
                return "";
            }
        }
        public override void SetSelection(FilterObject fo, bool isLoad = false)
        {
            if(fo is RangeFilterObject)
            {
                RangeFilterObject rfo = (RangeFilterObject)fo;
                this.FromOption = rfo.FromOption;
                this.ToOption = rfo.ToOption;
            }
        }
        public override void RemoveSelection()
        {
            this.FromOption = "";
            this.ToOption = "";
        }
        public override string GetSelectionXml()
        {
            return "<" + this.Name + "From>" + this.FromOption + "</" + this.Name + "From>" + "<" + this.Name + "To>" + this.ToOption.ToString() + "</" + this.Name + "To>";
        }
    }
}

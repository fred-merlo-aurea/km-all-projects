using System;
using System.Collections.Generic;
using System.Linq;

namespace FilterControls.Framework
{
    [Serializable]
    public class ComboFilterObject : FilterObject
    {
        private List<ListObject> _options = new List<ListObject>();
        private string _selection;
        public string Selection
        {
            get { return _selection; }
            set
            {
                _selection = value;
                OnPropertyChanged("Selection");
                OnPropertyChanged("SelectedCriteria");
            }
        }
        public override string SelectedCriteria
        {
            get { return _selection; }
        }
        public List<ListObject> Options { get { return _options; } }

        public ComboFilterObject(Enums.Filters type, Enums.FilterObjects name, string display, List<ListObject> list)
        {
            this.FilterType = type;
            this.Name = name;
            this.DisplayName = display;
            _options = list;
        }
        public ComboFilterObject() {}

        public override bool CheckChanges()
        {
 	        if(!string.IsNullOrEmpty(_selection))
                return true;
            else
                return false;
        }

        public override void SetSelection(FilterObject fo, bool isLoad = false)
        {
 	        if(fo is ComboFilterObject)
            {
                ComboFilterObject cfo = fo as ComboFilterObject;
                this.Selection = cfo.Selection;
            }
        }

        public override void RemoveSelection()
        {
 	        this.Selection = "";
        }

        public override string GetSelectionXml()
        {
 	        return "<" + this.Name + ">" + this.Selection + "</" + this.Name + ">";
        }
    }
}

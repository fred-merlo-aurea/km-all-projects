using System;
using System.Collections.Generic;
using System.Linq;

namespace FilterControls.Framework
{
    [Serializable]
    public class BoolFilterObject : FilterObject
    {
        private bool? _option;
        private List<string> _options;
        public bool? Option
        {
            get { return _option; }
            set
            {
                _option = value;
                OnPropertyChanged("Option");
                OnPropertyChanged("SelectedCriteria");
            }
        }
        public List<string> Options
        {
            get
            {
                if(_options == null)
                {
                    _options = new List<string>();
                    _options.Add("");
                    _options.Add("Yes");
                    _options.Add("No");
                }
                return _options;
            }
        }
        public override string SelectedCriteria
        {
            get
            {
                if (_option != null)
                    return _option.ToString();
                else
                    return "";
            }
        }

        public BoolFilterObject(Framework.Enums.Filters type, Enums.FilterObjects name, string display)
        {
            this.FilterType = type;
            this.Name = name;
            this.DisplayName = display;
        }
        public BoolFilterObject() {}
        public override bool CheckChanges()
        {
            if (_option != null)
                return true;
            else
                return false;
        }
        public override void SetSelection(FilterObject fo, bool isLoad = false)
        {
            if (fo is BoolFilterObject)
            {
                BoolFilterObject bfo = (BoolFilterObject)fo;
                this.Option = bfo.Option;
            }
        }
        public override void RemoveSelection()
        {
            this.Option = null;
        }
        public override string GetSelectionXml()
        {
            return "<" + this.Name + ">" + this.Option.ToString() + "</" + this.Name + ">";
        }
    }
}

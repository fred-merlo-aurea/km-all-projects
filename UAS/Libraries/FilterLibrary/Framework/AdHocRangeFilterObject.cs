using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FilterControls.Framework
{
    [Serializable]
    public class AdHocRangeFilterObject : FilterObject
    {
        private string _selection;
        private string _fromValue;
        private string _toValue;
        [IgnoreDataMember]
        public List<string> MatchCriteria { get; set; }
        public Enums.AdHocType AdHocType { get; set; }
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
        public string FromValue
        {
            get { return _fromValue; }
            set
            {
                _fromValue = value;
                OnPropertyChanged("FromValue");
                OnPropertyChanged("SelectedCriteria");
            }
        }
        public string ToValue
        {
            get { return _toValue; }
            set
            {
                _toValue = value;
                OnPropertyChanged("ToValue");
                OnPropertyChanged("SelectedCriteria");
            }
        }

        public AdHocRangeFilterObject(Enums.Filters type, string name, Enums.AdHocType adhocType)
        {
            this.FilterType = type;
            this.AdHocType = adhocType;
            this.Name = Enums.FilterObjects.AdHoc;
            this.DisplayName = name.ToString();
            this.MatchCriteria = new List<string>() { "RANGE", "EQUAL", "GREATER THAN", "LESSER THAN" };
        }
        public AdHocRangeFilterObject() { }
        public override bool CheckChanges()
        {
            if ((!string.IsNullOrEmpty(_fromValue) || !string.IsNullOrEmpty(_toValue)) && !string.IsNullOrEmpty(_selection))
                return true;
            else
                return false;
        }
        public override string SelectedCriteria
        {
            get
            {
                if (!string.IsNullOrEmpty(_fromValue) && !string.IsNullOrEmpty(_toValue) && !string.IsNullOrEmpty(_selection))
                    return _selection + " : " + _fromValue + " To " + _toValue;
                else if (!string.IsNullOrEmpty(_fromValue)  && !string.IsNullOrEmpty(_selection))
                    return _selection + " : " + _fromValue;
                else
                    return "";
            }
        }
        public override void SetSelection(FilterObject fo, bool isLoad = false)
        {
            if(fo is AdHocRangeFilterObject)
            {
                AdHocRangeFilterObject arfo = fo as AdHocRangeFilterObject;
                this.FromValue = arfo.FromValue;
                this.ToValue = arfo.ToValue;
                this.Selection = arfo.Selection;
            }
        }
        public override void RemoveSelection()
        {
            this.Selection = "";
            this.FromValue = "";
            this.ToValue = "";
        }
        public override string GetSelectionXml()
        {
            string xml = "<FilterDetail><FilterField>" + this.DisplayName + "</FilterField>" + "<SearchCondition>" + this.Selection + "</SearchCondition>" +
                "<FilterObjectType>" + this.AdHocType.ToString() + "</FilterObjectType>" + "<AdHocToField>" + this.ToValue + "</AdHocToField>" + "<AdHocFromField>" + this.FromValue + "</AdHocFromField>" +
                "</FilterDetail>";

            return xml;
        }
    }
}

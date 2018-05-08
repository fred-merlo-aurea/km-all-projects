using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FilterControls.Framework
{
    [Serializable]
    public class AdHocStandardFilterObject : FilterObject
    {
        private string _selection;
        private string _searchText;
        [IgnoreDataMember]
        public List<string> MatchCriteria { get; set; }
        public string Selection
        {
            get { return _selection; }
            set
            {                
                _selection = value;
                if (_selection.Equals("NO RESPONSE", StringComparison.CurrentCultureIgnoreCase) || _selection.Equals("ANY RESPONSE", StringComparison.CurrentCultureIgnoreCase))
                {
                    _searchText = "";
                    OnPropertyChanged("SearchText");
                    OnPropertyChanged("SelectedCriteria");
                }
                OnPropertyChanged("Selection");
                OnPropertyChanged("SelectedCriteria");
            }
        }
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
                OnPropertyChanged("SelectedCriteria");
            }
        }
        public Enums.AdHocType AdHocType { get; set; }
        public override string SelectedCriteria
        {
            get
            {
                if ((!string.IsNullOrEmpty(_selection) && !string.IsNullOrEmpty(_searchText)) || _selection.Equals("NO RESPONSE", StringComparison.CurrentCultureIgnoreCase) || _selection.Equals("ANY RESPONSE", StringComparison.CurrentCultureIgnoreCase))
                    return _selection + " : " + _searchText;                
                else
                    return "";
            }
        }

        public AdHocStandardFilterObject(Enums.Filters type, string name, Enums.AdHocType adhocType)
        {
            this.FilterType = type;
            this.AdHocType = adhocType;
            this.Name = Enums.FilterObjects.AdHoc;
            this.DisplayName = name.ToString();
            this.MatchCriteria = new List<string>() { "CONTAINS", "EQUAL", "START WITH", "END WITH", "DOES NOT CONTAIN", "NO RESPONSE", "ANY RESPONSE" }; 
        }

        public AdHocStandardFilterObject() {}

        public override bool CheckChanges()
        {
            if ((!string.IsNullOrEmpty(_selection) && !string.IsNullOrEmpty(_searchText)) || _selection.Equals("NO RESPONSE", StringComparison.CurrentCultureIgnoreCase) || _selection.Equals("ANY RESPONSE", StringComparison.CurrentCultureIgnoreCase))
                return true;
            else
                return false;
        }
        public override void SetSelection(FilterObject fo, bool isLoad = false)
        {
            if (fo is AdHocStandardFilterObject)
            {
                AdHocStandardFilterObject newObj = fo as AdHocStandardFilterObject;
                this.Selection = newObj.Selection;
                this.SearchText = newObj.SearchText;
            }
        }
        public override void RemoveSelection()
        {
            this.Selection = "";
            this.SearchText = "";
        }
        public override string GetSelectionXml()
        {
            string xml = "<FilterDetail><FilterField>" + this.DisplayName + "</FilterField>" + "<SearchCondition>" + this.Selection + "</SearchCondition>" +
                "<FilterObjectType>" + this.AdHocType.ToString() + "</FilterObjectType>" + "<AdHocFieldValue>" + this.SearchText + "</AdHocFieldValue></FilterDetail>";

            return xml;
        }
    }
}

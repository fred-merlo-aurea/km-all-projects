using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace FilterControls.Framework
{
    [Serializable]
    public class ListFilterObject : FilterObject
    {
        [IgnoreDataMember]
        public ObservableCollection<ListObject> Options { get; set; }
        private List<ListObject> _selectedOptions = new List<ListObject>();
        public List<ListObject> SelectedOptions { get { return _selectedOptions; } set { _selectedOptions = value; } }
        [IgnoreDataMember]
        public override string SelectedCriteria
        {
            get
            {
                string rtn = "";
                if (Options.Where(x => x.Selected) != null && Options.Where(x => x.Selected).Count() > 0)
                {
                    foreach (ListObject lo in Options.Where(x => x.Selected))
                        rtn += lo.DisplayValue + System.Environment.NewLine;
                    rtn = rtn.TrimEnd(System.Environment.NewLine.ToCharArray());
                }
                return rtn;
            }
        }
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ListObject newItem in e.NewItems)
                {
                    //Add listener for each item on PropertyChanged event
                    newItem.PropertyChanged += this.OnItemPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (ListObject oldItem in e.OldItems)
                {
                    oldItem.PropertyChanged -= this.OnItemPropertyChanged;
                }
            }
            OnPropertyChanged("SelectedCriteria");
        }
        void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ListObject lo = sender as ListObject;
            if (lo.Selected)
                SelectedOptions.Add(lo);
            else if (SelectedOptions.Contains(lo))
                SelectedOptions.Remove(lo);

            OnPropertyChanged("SelectedCriteria");
        }

        public ListFilterObject(Framework.Enums.Filters type, Enums.FilterObjects name, string displayName, List<ListObject> options)
        {
            this.FilterType = type;
            this.Name = name;
            this.DisplayName = displayName;
            this.Options = new ObservableCollection<ListObject>();
            this.Options.CollectionChanged += OnCollectionChanged;
            options.ForEach(x => this.Options.Add(x));
        }
        public ListFilterObject() { }
        public override bool CheckChanges()
        {
            if (Options.Where(x => x.Selected) != null && Options.Where(x => x.Selected).Count() > 0)
                return true;
            else
                return false;
        }
        public override void SetSelection(FilterObject fo, bool isLoad)
        {
            if (isLoad)
            {
                ListFilterObject lfo = (ListFilterObject)fo;
                var resultAll = from t in this.Options                             
                             select t;
                var resultSelected = from t in this.Options
                             join x in lfo.SelectedOptions on t.Value equals x.Value
                             select t;
                var result = resultAll.Except(resultSelected);

                result.ToList().ForEach(x => x.Selected = false);
            }
            if(fo is ListFilterObject)
            {
                ListFilterObject lfo = (ListFilterObject)fo;
                var result = from t in this.Options
                             join x in lfo.SelectedOptions on t.Value equals x.Value
                             select t;

                result.ToList().ForEach(x => x.Selected = true);
            }
        }
        public override void RemoveSelection()
        {
            foreach (ListObject lo in this.Options)
            {
                if (lo.Selected != false)
                    lo.Selected = false;
            }
        }
        public override string GetSelectionXml()
        {
            if (this.FilterType != Enums.Filters.Demographic)
            {
                string xml = "<" + this.Name + ">";
                this.Options.Where(x => x.Selected == true).ToList().ForEach(x => xml += x.Value + ",");
                xml = xml.TrimEnd(',');
                xml += "</" + this.Name + ">";
                return xml;
            }
            else
            {
                string xml = "";
                this.Options.Where(x => x.Selected == true).ToList().ForEach(x => xml += x.ParentValue + "_" + x.Value + ",");
                xml = xml.TrimEnd(',');
                return xml;
            }
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FilterControls.Framework
{
    public class ContactFilters : Filters
    {
        private Framework.Enums.Filters _filterType;
        public override Framework.Enums.Filters FilterType
        {
            get { return _filterType; }
            set
            {
                _filterType = value;
                OnPropertyChanged("FilterType");
            }
        }
        public override ObservableCollection<FilterObject> Objects { get; set; }
        public override string Title { get; set; }
        public ContactFilters()
        {
            this.FilterType = Framework.Enums.Filters.Contact_Fields;
            this.Title = this.FilterType.ToString().Replace("_"," ");
            Objects = new ObservableCollection<FilterObject>();
            Objects.Add(new BoolFilterObject(_filterType, Enums.FilterObjects.Email, "Has Email Data"));
            Objects.Add(new BoolFilterObject(_filterType, Enums.FilterObjects.Phone, "Has Phone Data"));
            Objects.Add(new BoolFilterObject(_filterType, Enums.FilterObjects.Mobile, "Has Mobile Data"));
            Objects.Add(new BoolFilterObject(_filterType, Enums.FilterObjects.Fax, "Has Fax Data"));
        }
    }
}

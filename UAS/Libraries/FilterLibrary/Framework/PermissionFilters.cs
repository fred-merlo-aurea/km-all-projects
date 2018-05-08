using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FilterControls.Framework
{
    public class PermissionFilters : Filters
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
        public PermissionFilters()
        {
            this.FilterType = Framework.Enums.Filters.Permission;
            this.Title = this.FilterType.ToString();
            Objects = new ObservableCollection<FilterObject>();
            //this.FilterType, Enums.FilterObjects.Media, "Media", options
            List<ListObject> mailOptions = new List<ListObject>();
            mailOptions.Add(new ListObject("Yes", "1"));
            mailOptions.Add(new ListObject("No", "0"));
            mailOptions.Add(new ListObject("Blank", "NULL"));

            //deep clone of list. Otherwise selecting one values selects it in every list
            List<ListObject> faxOptions = mailOptions.Select(x => new ListObject(x.DisplayValue, x.Value, x.ParentValue)).ToList();
            List<ListObject> phoneOptions = mailOptions.Select(x => new ListObject(x.DisplayValue, x.Value, x.ParentValue)).ToList();
            List<ListObject> otherProductsOptions = mailOptions.Select(x => new ListObject(x.DisplayValue, x.Value, x.ParentValue)).ToList();
            List<ListObject> thirdPartyOptions = mailOptions.Select(x => new ListObject(x.DisplayValue, x.Value, x.ParentValue)).ToList();
            List<ListObject> emailRenewOptions = mailOptions.Select(x => new ListObject(x.DisplayValue, x.Value, x.ParentValue)).ToList();

            Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.MailPermission, "Mail Permission", mailOptions));
            Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.FaxPermission, "Fax Permission", faxOptions));
            Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.PhonePermission, "Phone Permission", phoneOptions));
            Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.OtherProductsPermission, "Other Products Permission", otherProductsOptions));
            Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.ThirdPartyPermission, "Third Party Permission", thirdPartyOptions));
            Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.EmailRenewPermission, "Email Renew Permission", emailRenewOptions));
        }
    }
}

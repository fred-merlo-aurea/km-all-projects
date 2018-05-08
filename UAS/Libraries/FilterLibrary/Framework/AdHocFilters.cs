using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FilterControls.Framework
{
    public class AdHocFilters : Filters
    {
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> pubSubscriptionW = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
        private FrameworkUAS.Service.Response<List<string>> adHocResponse = new FrameworkUAS.Service.Response<List<string>>();
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

        public AdHocFilters(int pubID)
        {
            this.FilterType = Framework.Enums.Filters.AdHoc;
            this.Title = this.FilterType.ToString();
            Objects = new ObservableCollection<FilterObject>();
            LoadStandardProperties();
            LoadAdHocProperties(pubID);
        }

        public AdHocFilters()
        {
            this.FilterType = Framework.Enums.Filters.AdHoc;
            this.Title = this.FilterType.ToString();
            Objects = new ObservableCollection<FilterObject>();
            LoadStandardProperties();
        }

        private void LoadStandardProperties()
        {
            Objects.Add(new AdHocStandardFilterObject(_filterType, "Address1", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "Address2", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "Address3", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "City", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "Company", Enums.AdHocType.Standard));
            Objects.Add(new AdHocRangeFilterObject(_filterType, "Copies", Enums.AdHocType.Range));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "Country", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "County", Enums.AdHocType.Standard));
            //Objects.Add(new AdHocStandardFilterObject(_filterType, "Demo7", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "Email", Enums.AdHocType.Standard));
            Objects.Add(new AdHocRangeFilterObject(_filterType, "EmailID", Enums.AdHocType.Range));
            Objects.Add(new AdHocRangeFilterObject(_filterType, "EmailStatusID", Enums.AdHocType.Range));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "Fax", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "FirstName", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "LastName", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "MemberGroup", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "Mobile", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "OnBehalfOf", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "OrigsSrc", Enums.AdHocType.Standard));
            Objects.Add(new AdHocRangeFilterObject(_filterType, "Par3CID", Enums.AdHocType.Range));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "Phone", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "PhoneExt", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "Plus4", Enums.AdHocType.Standard));
            Objects.Add(new AdHocRangeFilterObject(_filterType, "PubSubscriptionID", Enums.AdHocType.Range));
            Objects.Add(new AdHocRangeFilterObject(_filterType, "ReqFlag", Enums.AdHocType.Range));
            Objects.Add(new AdHocRangeFilterObject(_filterType, "SequenceID", Enums.AdHocType.Range));
            Objects.Add(new AdHocRangeFilterObject(_filterType, "SubscriptionID", Enums.AdHocType.Range));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "SubscriberSourceCode", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "Title", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "Verify", Enums.AdHocType.Standard));
            Objects.Add(new AdHocRangeFilterObject(_filterType, "WaveMailingID", Enums.AdHocType.Range));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "Website", Enums.AdHocType.Standard));
            Objects.Add(new AdHocStandardFilterObject(_filterType, "ZipCode", Enums.AdHocType.Standard));

        }
        private void LoadAdHocProperties(int pubID)
        {
            adHocResponse = pubSubscriptionW.Proxy.Get_AdHocs(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, pubID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);

            if (Common.CheckResponse(adHocResponse.Result, adHocResponse.Status))
            {
                adHocResponse.Result.ForEach(x => this.Objects.Add(new AdHocStandardFilterObject(this.FilterType, x, Enums.AdHocType.AdHoc)));
            }
        }
    }
}

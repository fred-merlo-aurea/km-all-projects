using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace WpfControls.Helpers
{
    public class Common
    {
        public static bool CheckResponse<T>(List<T> response, FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes status)
        {
            if (response != null && status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                return true;
            else
            {
                Core_AMS.Utilities.WPF.MessageError("An unexpected error occured during a service request, please try again.  If the problem persists please contact Customer Support.");
                return false;
            }
        }

        public static bool CheckResponse(int response, FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes status)
        {
            if (response > 0 && status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                return true;
            else
            {
                Core_AMS.Utilities.WPF.MessageError("An unexpected error occured during a service request, please try again.  If the problem persists please contact Customer Support.");
                return false;
            }
        }

        public class FilterCriteria : INotifyPropertyChanged
        {
            public FilterCriteria(string name, Helpers.FilterOperations.FilterContainer fc, ObservableCollection<Helpers.FilterOperations.DisplayedFilterDetail> details, List<int> subscriberIDs)
            {
                this.FilterName = name;
                this.RecordCount = subscriberIDs.Count;
                this.FilterContainer = fc;
                this.FilterDetails = details;
                this.SubscriberIDs = subscriberIDs;
            }
            private string _FilterName;
            public string FilterName
            {
                get { return _FilterName; }
                set
                {
                    _FilterName = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FilterName"));
                    }
                }
            }
            public int FilterCount { get; set; }
            public int RecordCount { get; set; }
            public Helpers.FilterOperations.FilterContainer FilterContainer { get; set; }
            public ObservableCollection<Helpers.FilterOperations.DisplayedFilterDetail> FilterDetails { get; set; }
            public List<int> SubscriberIDs { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}

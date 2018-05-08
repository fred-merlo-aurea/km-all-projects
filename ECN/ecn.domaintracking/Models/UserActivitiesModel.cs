using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECN_Framework_Entities.DomainTracker;

namespace ecn.domaintracking.Models
{
    public class UserActivitiesModel
    {
        public List<DomainTrackerActivity> Activities { get; set; }
        public List<FieldsValuePair> FieldsValuePairList { get; set; }
    }
}
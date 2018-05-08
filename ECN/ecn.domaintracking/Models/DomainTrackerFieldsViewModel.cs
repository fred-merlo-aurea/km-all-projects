using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

using System.Data.Entity;
using System.Globalization;
using System.Web.Security;


namespace ecn.domaintracking.Models
{
    public class DomainTrackerFieldsViewModel
    {
        #region " Properties "
        public ECN_Framework_Entities.DomainTracker.DomainTracker DomainTracker { get; set; }
        public List<ECN_Framework_Entities.DomainTracker.DomainTrackerFields> DomainTrackerFields { get; set; }
        public ECN_Framework_Entities.DomainTracker.DomainTrackerFields DomainTrackerField { get; set; }
        public DataTable DTF { get; set; }
        public string ErrorMessage { get; set; }
        //[RegularExpression(@"^(([a-zA-Z0-9]+\.)([a-zA-Z0-9]+\.).{2,3})$", ErrorMessage = "Invalid domain")]
        [Required(ErrorMessage = "Domain is required")]
        public string Domain { get; set; }

        public string FieldName { get; set; }
        public string Source { get; set; }
        public string SourceID { get; set; }
        public int Page { get; set; }

        #endregion

        #region " Constructor "
        public DomainTrackerFieldsViewModel()
        {
            DomainTracker = new ECN_Framework_Entities.DomainTracker.DomainTracker();
            DomainTrackerFields = new List<ECN_Framework_Entities.DomainTracker.DomainTrackerFields>();
            DomainTrackerField = new ECN_Framework_Entities.DomainTracker.DomainTrackerFields();
            DTF = new DataTable();
            ErrorMessage = "";
            Domain = "";
            FieldName = "";
            Source = "";
            SourceID = "";
        }
        #endregion


    }
}
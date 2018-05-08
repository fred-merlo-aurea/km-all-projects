using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace ECN_Framework.Accounts.Entity
{
    [Serializable]
    public class CustomerInquiry
    {
        public CustomerInquiry() { }
        #region Properties
        public int CustomerInquirieID { get; set; }
        public int CustomerID { get; set; }
        public int LicenseID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfInquirie { get; set; }
        public string Notes { get; set; }
        public int CustomerServiceStaffID { get; set; }
        #endregion
        #region Data
        #region Select

        #endregion
        #region CRUD

        #endregion
        #endregion
    }
}

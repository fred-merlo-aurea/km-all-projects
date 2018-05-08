using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace ECN_Framework.Accounts.Entity
{
    [Serializable]
    public class CustomerContact
    {
        public CustomerContact() { }
        #region Properties
        public int ContactID { get; set; }
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Createdby { get; set; }
        public string Updatedby { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateUpdated { get; set; }
        #endregion
        #region Data
        #region Select

        #endregion
        #region CRUD

        #endregion
        #endregion
    }
}

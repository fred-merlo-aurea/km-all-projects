using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace ECN_Framework.Accounts.Entity
{
    [Serializable]
    public class CustomerIP
    {
        public CustomerIP() { }
        #region Properties
        public int CustomerID { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        #endregion
        #region Data
        #region Select

        #endregion
        #region CRUD

        #endregion
        #endregion
    }
}

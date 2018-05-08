using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace ECN_Framework.Accounts.Entity
{
    [Serializable]
    public class CustomerDiskUsage
    {
        public CustomerDiskUsage() { }
        #region Properties
        public int UsageID { get; set; }
        public int ChannelID { get; set; }
        public int CustomerID { get; set; }
        public string SizeInBytes { get; set; }
        public DateTime DateMonitored { get; set; }
        #endregion
        #region Data
        #region Select

        #endregion
        #region CRUD

        #endregion
        #endregion
    }
}

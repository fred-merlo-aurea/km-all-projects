using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace ECN_Framework.Accounts.Entity
{
    [Serializable]
    public class Wizard_EmailRate
    {
        public Wizard_EmailRate() { }
        #region Properties
        public int EmailRateID { get; set; }
        public int BaseChannelID { get; set; }
        public decimal Basefee { get; set; }
        public int EmailRangeStart { get; set; }
        public int EmailRangeEnd { get; set; }
        public decimal EmailRate { get; set; }
        #endregion
        #region Data
        #region Select

        #endregion
        #region CRUD

        #endregion
        #endregion
    }
}

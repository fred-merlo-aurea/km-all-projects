using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace ECN_Framework.Accounts.Entity
{
    [Serializable]
    public class Wizard_BaseFee
    {
        public Wizard_BaseFee() { }
        #region Properties
        public int BaseFeeID { get; set; }
        public int BaseChannelID { get; set; }
        public bool ChargeCrCard { get; set; }//char
        public bool DetailReports { get; set; }//char
        #endregion
        #region Data
        #region Select

        #endregion
        #region CRUD

        #endregion
        #endregion
    }
}

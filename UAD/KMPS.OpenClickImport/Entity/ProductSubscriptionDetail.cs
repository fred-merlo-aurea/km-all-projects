using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace KMPS.ActivityImport.Entity
{
    public class ProductSubscriptionDetail
    {
        public ProductSubscriptionDetail() { }
        #region Properties
        public int PubSubscriptionDetailID { get; set; }
        public int PubSubscriptionID { get; set; }
        public int SubscriptionID { get; set; }
        public int CodesheetID { get; set; }
        #endregion
    }
}

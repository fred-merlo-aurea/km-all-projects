using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Activity
{
    [Serializable]
    public class SuppressedCodes
    {
        public static bool Exists()
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Activity.SuppressedCodes.Exists();
                scope.Complete();
            }
            return exists;
        }

        public static List<ECN_Framework_Entities.Activity.SuppressedCodes> GetAll()
        {
            return ECN_Framework_DataLayer.Activity.SuppressedCodes.GetAll();
        }
    }
}

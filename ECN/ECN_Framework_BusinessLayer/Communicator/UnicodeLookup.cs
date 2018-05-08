using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using ECN_Framework_Common.Objects;


namespace ECN_Framework_BusinessLayer.Communicator
{
    public class UnicodeLookup
    {
        public static ECN_Framework_Entities.Communicator.UnicodeLookup GetByUnicodeNumber(int num)
        {
            ECN_Framework_Entities.Communicator.UnicodeLookup ul = new ECN_Framework_Entities.Communicator.UnicodeLookup();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ul = ECN_Framework_DataLayer.Communicator.UnicodeLookup.GetByUnicodeNum(num);
                scope.Complete();
            }
            return ul;
        }

        public static ECN_Framework_Entities.Communicator.UnicodeLookup GetById(int Id)
        {
            ECN_Framework_Entities.Communicator.UnicodeLookup ul = new ECN_Framework_Entities.Communicator.UnicodeLookup();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ul = ECN_Framework_DataLayer.Communicator.UnicodeLookup.GetByID(Id);
                scope.Complete();
            }
            return ul;
        }

        public static List<ECN_Framework_Entities.Communicator.UnicodeLookup> GetAllActive()
        {
            List<ECN_Framework_Entities.Communicator.UnicodeLookup> ul = new List<ECN_Framework_Entities.Communicator.UnicodeLookup>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ul = ECN_Framework_DataLayer.Communicator.UnicodeLookup.GetAllActive();
                scope.Complete();
            }

            return ul;
        }
    }
}

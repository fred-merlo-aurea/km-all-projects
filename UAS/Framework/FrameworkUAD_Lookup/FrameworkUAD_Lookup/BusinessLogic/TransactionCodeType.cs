using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD_Lookup.BusinessLogic
{
    public class TransactionCodeType
    {
        public bool Exists(string transactionCodeTypeName)
        {
            List<Entity.TransactionCodeType> all = Select().ToList();
            if (all.Exists(x => x.TransactionCodeTypeName.Equals(transactionCodeTypeName, StringComparison.CurrentCultureIgnoreCase)))
                return true;
            else
                return false;
        }
        public Entity.TransactionCodeType Select(Enums.TransactionCodeType transactionCodeTypeName)
        {
            Entity.TransactionCodeType tct = null;
            List<Entity.TransactionCodeType> all = Select().ToList();
            if (all.Exists(x => x.TransactionCodeTypeName.Equals(transactionCodeTypeName.ToString(), StringComparison.CurrentCultureIgnoreCase)))
                tct = all.SingleOrDefault(x => x.TransactionCodeTypeName.Equals(transactionCodeTypeName.ToString(), StringComparison.CurrentCultureIgnoreCase));
            return tct;
        }
        public Entity.TransactionCodeType Select(string transactionCodeTypeName)
        {
            Entity.TransactionCodeType tct = null;
            List<Entity.TransactionCodeType> all = Select().ToList();
            if (all.Exists(x => x.TransactionCodeTypeName.Equals(transactionCodeTypeName, StringComparison.CurrentCultureIgnoreCase)))
                tct = all.SingleOrDefault(x => x.TransactionCodeTypeName.Equals(transactionCodeTypeName, StringComparison.CurrentCultureIgnoreCase));
            return tct;
        }
        public List<Entity.TransactionCodeType> Select()
        {
            List<Entity.TransactionCodeType> retList = null;
            retList = DataAccess.TransactionCodeType.Select();
            return retList;
        }
        public List<Entity.TransactionCodeType> Select(bool isFree)
        {
            List<Entity.TransactionCodeType> retList = null;
            retList = DataAccess.TransactionCodeType.Select(isFree);
            return retList;
        }
        
        public int Save(Entity.TransactionCodeType x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.TransactionCodeTypeID = DataAccess.TransactionCodeType.Save(x);
                scope.Complete();
            }

            return x.TransactionCodeTypeID;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;

namespace FrameworkUAD_Lookup.BusinessLogic
{
    public class TransactionCode
    {
        public bool Exists(int transactionCodeTypeID, int transactionCodeValue)
        {
            List<Entity.TransactionCode> all = Select().ToList();
            if (all.Exists(x => x.TransactionCodeTypeID == transactionCodeTypeID && x.TransactionCodeValue == transactionCodeValue))
                return true;
            else
                return false;
        }
        public List<Entity.TransactionCode> SelectActiveIsFree(bool isFree)
        {
            List<Entity.TransactionCode> retList = null;
            retList = DataAccess.TransactionCode.SelectActiveIsFree(isFree);
            return retList;
        }
        public Entity.TransactionCode SelectTransactionCodeValue(int transactionCodeValue)
        {
            Entity.TransactionCode transCodeId = null;
            List<Entity.TransactionCode> all = Select().ToList();
            if (all.Exists(x => x.TransactionCodeValue == transactionCodeValue))
                transCodeId = all.SingleOrDefault(x => x.TransactionCodeValue == transactionCodeValue);
            return transCodeId;
        }

        public Entity.TransactionCode Select(int transactionCodeTypeID, int transactionCodeValue)
        {
            Entity.TransactionCode tc = null;
            List<Entity.TransactionCode> all = Select().ToList();
            if (all.Exists(x => x.TransactionCodeTypeID == transactionCodeTypeID && x.TransactionCodeValue == transactionCodeValue))
                tc = all.SingleOrDefault(x => x.TransactionCodeTypeID == transactionCodeTypeID && x.TransactionCodeValue == transactionCodeValue);
            return tc;
        }
        public List<Entity.TransactionCode> Select()
        {
            List<Entity.TransactionCode> retList = null;
            retList = DataAccess.TransactionCode.Select();
            return retList;
        }
       
        public int Save(Entity.TransactionCode x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.TransactionCodeID = DataAccess.TransactionCode.Save(x);
                scope.Complete();
            }

            return x.TransactionCodeID;
        }

        public static IEnumerable<SelectListItem> GetTransactionCodesforSelectList()
        {
            return DataAccess.TransactionCode.Select().Select(i => new SelectListItem()
            {
                Text = i.TransactionCodeName,
                Value = i.TransactionCodeID.ToString()
            });
        }
    }
}

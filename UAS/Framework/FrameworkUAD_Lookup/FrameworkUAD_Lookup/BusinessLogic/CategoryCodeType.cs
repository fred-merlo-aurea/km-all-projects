using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD_Lookup.BusinessLogic
{
    public class CategoryCodeType
    {
        public bool Exists(string categoryCodeTypeName)
        {
            List<Entity.CategoryCodeType> all = Select().ToList();
            if (all.Exists(x => x.CategoryCodeTypeName.Equals(categoryCodeTypeName, StringComparison.CurrentCultureIgnoreCase)))
                return true;
            else
                return false;
        }
        public Entity.CategoryCodeType Select(Enums.CategoryCodeType categoryCodeTypeName)
        {
            Entity.CategoryCodeType cct = null;
            List<Entity.CategoryCodeType> all = Select().ToList();
            if (all.Exists(x => x.CategoryCodeTypeName.Equals(categoryCodeTypeName.ToString(), StringComparison.CurrentCultureIgnoreCase)))
                cct = all.SingleOrDefault(x => x.CategoryCodeTypeName.Equals(categoryCodeTypeName.ToString(), StringComparison.CurrentCultureIgnoreCase));
            return cct;
        }
        public Entity.CategoryCodeType Select(string categoryCodeTypeName)
        {
            Entity.CategoryCodeType cct = null;
            List<Entity.CategoryCodeType> all = Select().ToList();
            if (all.Exists(x => x.CategoryCodeTypeName.Equals(categoryCodeTypeName, StringComparison.CurrentCultureIgnoreCase)))
                cct = all.SingleOrDefault(x => x.CategoryCodeTypeName.Equals(categoryCodeTypeName, StringComparison.CurrentCultureIgnoreCase));
            return cct;
        }
        public List<Entity.CategoryCodeType> Select()
        {
            List<Entity.CategoryCodeType> retList = null;
            retList = DataAccess.CategoryCodeType.Select();
            return retList;
        }
        public List<Entity.CategoryCodeType> Select(bool isFree)
        {
            List<Entity.CategoryCodeType> retList = null;
            retList = DataAccess.CategoryCodeType.Select(isFree);
            return retList;
        }
        
        public int Save(Entity.CategoryCodeType x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.CategoryCodeTypeID = DataAccess.CategoryCodeType.Save(x);
                scope.Complete();
            }
            return x.CategoryCodeTypeID;
        }
    }
}

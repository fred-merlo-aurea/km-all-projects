using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;

namespace FrameworkUAD_Lookup.BusinessLogic
{
    public class CategoryCode
    {
        public bool Exists(int categoryCodeTypeID, int categoryCodeValue)
        {
            List<Entity.CategoryCode> all = Select().ToList();
            if (all.Exists(x => x.CategoryCodeTypeID == categoryCodeTypeID && x.CategoryCodeValue == categoryCodeValue))
                return true;
            else
                return false;
        }
        public List<Entity.CategoryCode> SelectActiveIsFree(bool isFree)
        {
            List<Entity.CategoryCode> retList = null;
            retList = DataAccess.CategoryCode.SelectActiveIsFree(isFree);
            return retList;
        }
        public Entity.CategoryCode Select(int categoryCodeTypeID, int categoryCodeValue)
        {
            Entity.CategoryCode cc = null;
            List<Entity.CategoryCode> all = Select().ToList();
            if (all.Exists(x => x.CategoryCodeTypeID == categoryCodeTypeID && x.CategoryCodeValue == categoryCodeValue))
                cc = all.SingleOrDefault(x => x.CategoryCodeTypeID == categoryCodeTypeID && x.CategoryCodeValue == categoryCodeValue);
            return cc;
        }
        public List<Entity.CategoryCode> Select()
        {
            List<Entity.CategoryCode> retList = null;
            retList = DataAccess.CategoryCode.Select();
            return retList;
        }
        
        public int Save(Entity.CategoryCode x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.CategoryCodeID = DataAccess.CategoryCode.Save(x);
                scope.Complete();
            }
            return x.CategoryCodeID;
        }
        public static IEnumerable<SelectListItem> GetCategoryCodesforSelectList()
        {
            return DataAccess.CategoryCode.Select().Select(i => new SelectListItem()
            {
                Text = i.CategoryCodeName,
                Value = i.CategoryCodeID.ToString()
            });
        }
    }
}

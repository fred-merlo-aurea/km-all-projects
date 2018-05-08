using System;
using System.Linq;
using System.Collections.Generic;
using System.Transactions;

namespace FrameworkUAD_Lookup.BusinessLogic
{
    public class CodeType
    {
        public List<Entity.CodeType> Select()
        {
            List<Entity.CodeType> x = null;
            x = DataAccess.CodeType.Select().ToList();
            return x;
        }
        public Entity.CodeType Select(int codeTypeId)
        {
            Entity.CodeType x = null;
            x = DataAccess.CodeType.Select(codeTypeId);
            return x;
        }
        public Entity.CodeType Select(Enums.CodeType codeType)
        {
            Entity.CodeType x = null;
            x = DataAccess.CodeType.Select(codeType);
            return x;
        }
        public int Save(Entity.CodeType x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.CodeTypeId = DataAccess.CodeType.Save(x);
                scope.Complete();
            }

            return x.CodeTypeId;
        }
    }
}

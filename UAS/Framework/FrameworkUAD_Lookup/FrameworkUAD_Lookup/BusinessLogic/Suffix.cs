using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD_Lookup.BusinessLogic
{
    public class Suffix
    {
        public List<Entity.Suffix> Select()
        {
            List<Entity.Suffix> x = null;
            x = DataAccess.Suffix.Select();

            return x;
        }

        public List<Entity.Suffix> Select(int SuffixCodeTypeID)
        {
            List<Entity.Suffix> x = null;
            x = DataAccess.Suffix.Select(SuffixCodeTypeID);

            return x;
        }

        public int Save(Entity.Suffix x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.SuffixID = DataAccess.Suffix.Save(x);
                scope.Complete();
            }

            return x.SuffixID;
        }
    }
}

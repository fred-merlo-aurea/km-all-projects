using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class TransformJoin
    {
        public  List<Entity.TransformJoin> Select()
        {
            List<Entity.TransformJoin> x = null;
            x = DataAccess.TransformJoin.Select().ToList();

            return x;
        }

        public  List<Entity.TransformJoin> Select(int TransformationID)
        {
            List<Entity.TransformJoin> x = null;
            x = DataAccess.TransformJoin.Select(TransformationID).ToList();

            return x;
        }
        public  List<Entity.TransformJoin> SelectSourceFileID(int sourceFileID)
        {
            List<Entity.TransformJoin> x = null;
            x = DataAccess.TransformJoin.SelectSourceFileID(sourceFileID).ToList();

            return x;
        }
    
        public  int Save(Entity.TransformJoin x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.TransformationID = DataAccess.TransformJoin.Save(x);
                scope.Complete();
            }

            return x.TransformationID;
        }
    }
}

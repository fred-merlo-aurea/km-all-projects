using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class TransformAssign
    {
        public  List<Entity.TransformAssign> Select()
        {
            List<Entity.TransformAssign> x = null;
            x = DataAccess.TransformAssign.Select().ToList();

            return x;
        }

        public  List<Entity.TransformAssign> Select(int TransformationID)
        {
            List<Entity.TransformAssign> x = null;
            x = DataAccess.TransformAssign.Select(TransformationID).ToList();

            return x;
        }
        public  List<Entity.TransformAssign> SelectSourceFileID(int sourceFileID)
        {
            List<Entity.TransformAssign> x = null;
            x = DataAccess.TransformAssign.SelectSourceFileID(sourceFileID).ToList();

            return x;
        }
       
        public  int Save(Entity.TransformAssign x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.TransformationID = DataAccess.TransformAssign.Save(x);
                scope.Complete();
            }

            return x.TransformationID;
        }

        public int Delete(int TransformDataMapID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                TransformDataMapID = DataAccess.TransformAssign.Delete(TransformDataMapID);
                scope.Complete();
            }
            return TransformDataMapID;
        }
    }
}

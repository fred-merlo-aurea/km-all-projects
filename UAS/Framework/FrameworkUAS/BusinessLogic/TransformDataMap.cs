using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class TransformDataMap
    {
        public  List<Entity.TransformDataMap> Select()
        {
            List<Entity.TransformDataMap> x = null;
            x = DataAccess.TransformDataMap.Select().ToList();

            return x;
        }

        public  List<Entity.TransformDataMap> SelectSourceFileID(int sourceFileID)
        {
            List<Entity.TransformDataMap> x = null;
            x = DataAccess.TransformDataMap.SelectSourceFileID(sourceFileID).ToList();

            return x;
        }
        public  List<Entity.TransformDataMap> Select(int TransformationID)
        {
            List<Entity.TransformDataMap> x = null;
            x = DataAccess.TransformDataMap.Select(TransformationID).ToList();

            return x;
        }

        public  List<Entity.TransformDataMap> Delete(int TransformDataMapID)
        {
            List<Entity.TransformDataMap> x = null;

            using (TransactionScope scope = new TransactionScope())
            {
                x = DataAccess.TransformDataMap.Delete(TransformDataMapID).ToList();
                scope.Complete();
            }
            return x;
        }

        public  int Save(Entity.TransformDataMap x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.TransformationID = DataAccess.TransformDataMap.Save(x);
                scope.Complete();
            }

            return x.TransformationID;
        }
    }
}

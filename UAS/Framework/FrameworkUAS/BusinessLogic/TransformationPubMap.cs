using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class TransformationPubMap
    {
        public  List<Entity.TransformationPubMap> Select()
        {
            List<Entity.TransformationPubMap> x = null;
            x = DataAccess.TransformationPubMap.Select().ToList();

            return x;
        }

        public  List<Entity.TransformationPubMap> Select(int transformationID)
        {
            List<Entity.TransformationPubMap> x = null;
            x = DataAccess.TransformationPubMap.Select(transformationID).ToList();

            return x;
        }
        public HashSet<Entity.TransformationPubMap> HashSetTransformationId(int transformationID)
        {
            var list = DataAccess.TransformationPubMap.Select(transformationID);
            HashSet<Entity.TransformationPubMap> x = new HashSet<Entity.TransformationPubMap>(list);
            return x;
        }
        public  int Save(Entity.TransformationPubMap x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.TransformationPubMapID = DataAccess.TransformationPubMap.Save(x);
                scope.Complete();
            }

            return x.TransformationPubMapID;
        }

        public  int Delete(int TransformationID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.TransformationPubMap.Delete(TransformationID);
                scope.Complete();
            }

            return res;
        }

        public  int Delete(int TransformationID, int PubID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.TransformationPubMap.Delete(TransformationID, PubID);
                scope.Complete();
            }

            return res;
        }
    }
}

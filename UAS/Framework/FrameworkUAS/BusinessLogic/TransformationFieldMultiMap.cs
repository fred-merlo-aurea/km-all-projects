using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class TransformationFieldMultiMap
    {
        public List<Entity.TransformationFieldMultiMap> Select()
        {
            List<Entity.TransformationFieldMultiMap> x = null;
            x = DataAccess.TransformationFieldMultiMap.Select().ToList();

            return x;
        }

        public List<Entity.TransformationFieldMultiMap> SelectTransformationID(int transformationID)
        {
            List<Entity.TransformationFieldMultiMap> x = null;
            x = DataAccess.TransformationFieldMultiMap.SelectTransformationID(transformationID).ToList();

            return x;
        }

        public int Save(Entity.TransformationFieldMultiMap x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.TransformationFieldMultiMapID = DataAccess.TransformationFieldMultiMap.Save(x);
                scope.Complete();
            }

            return x.TransformationFieldMultiMapID;
        }

        public int DeleteByFieldMultiMapID(int FieldMultiMapID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.TransformationFieldMultiMap.DeleteByFieldMultiMapID(FieldMultiMapID);
                scope.Complete();
            }

            return res;
        }
        public int DeleteBySourceFileID(int SourceFileID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.TransformationFieldMultiMap.DeleteBySourceFileID(SourceFileID);
                scope.Complete();
            }

            return res;
        }
        public int DeleteBySourceFileIDAndFieldMultiMapID(int SourceFileID, int FieldMultiMapID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.TransformationFieldMultiMap.DeleteBySourceFileIDAndFieldMultiMapID(SourceFileID, FieldMultiMapID);
                scope.Complete();
            }

            return res;
        }
        public int DeleteByFieldMappingID(int FieldMappingID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.TransformationFieldMultiMap.DeleteByFieldMappingID(FieldMappingID);
                scope.Complete();
            }

            return res;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class FieldMultiMap
    {
        public List<Entity.FieldMultiMap> Select()
        {
            List<Entity.FieldMultiMap> x = null;
            x = DataAccess.FieldMultiMap.Select().ToList();

            return x;
        }
        public List<Entity.FieldMultiMap> SelectFieldMappingID(int fieldMappingID)
        {
            List<Entity.FieldMultiMap> x = null;
            x = DataAccess.FieldMultiMap.SelectFieldMappingID(fieldMappingID);

            return x;
        }
        public HashSet<Entity.FieldMultiMap> HashSetFieldMappingID(int fieldMappingID)
        {
            var list = DataAccess.FieldMultiMap.SelectFieldMappingID(fieldMappingID);
            HashSet<Entity.FieldMultiMap> x = new HashSet<Entity.FieldMultiMap>(list);
            return x;
        }
        public Entity.FieldMultiMap SelectFieldMultiMapID(int fieldMultiMapID)
        {
            Entity.FieldMultiMap x = null;
            x = DataAccess.FieldMultiMap.SelectFieldMultiMapID(fieldMultiMapID);

            return x;
        }        

       
        public int Save(Entity.FieldMultiMap x)
        {
            using (



                TransactionScope scope = new TransactionScope())
            {
                x.FieldMultiMapID = DataAccess.FieldMultiMap.Save(x);
                scope.Complete();
            }

            return x.FieldMultiMapID;
        }
        public int DeleteBySourceFileID(int SourceFileID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.FieldMultiMap.DeleteBySourceFileID(SourceFileID);
                scope.Complete();
            }

            return res;
        }
        public int DeleteByFieldMappingID(int FieldMappingID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.FieldMultiMap.DeleteByFieldMappingID(FieldMappingID);
                scope.Complete();
            }

            return res;
        }
        public int DeleteByFieldMultiMapID(int FieldMultiMapID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.FieldMultiMap.DeleteByFieldMultiMapID(FieldMultiMapID);
                scope.Complete();
            }

            return res;
        }
    }
}

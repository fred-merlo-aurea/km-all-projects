using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class TransformationFieldMap
    {
        public  List<Entity.TransformationFieldMap> Select()
        {
            List<Entity.TransformationFieldMap> x = null;
            x = DataAccess.TransformationFieldMap.Select().ToList();

            return x;
        }

        public List<Entity.TransformationFieldMap> Select(int SourceFileID)
        {
            List<Entity.TransformationFieldMap> x = null;
            x = DataAccess.TransformationFieldMap.Select(SourceFileID).ToList();

            return x;
        }

        public  List<Entity.TransformationFieldMap> SelectTransformationID(int transformationID)
        {
            List<Entity.TransformationFieldMap> x = null;
            x = DataAccess.TransformationFieldMap.SelectTransformationID(transformationID).ToList();

            return x;
        }
        public HashSet<Entity.TransformationFieldMap> HashSetTransformationID(int transformationID)
        {
            var list = DataAccess.TransformationFieldMap.SelectTransformationID(transformationID);
            HashSet<Entity.TransformationFieldMap> x = new HashSet<Entity.TransformationFieldMap>(list);
            return x;
        }
        public  int Save(Entity.TransformationFieldMap x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.TransformationFieldMapID = DataAccess.TransformationFieldMap.Save(x);
                scope.Complete();
            }

            return x.TransformationFieldMapID;
        }

        public  int Delete(string TransformationName, int clientID, string ColumnName)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.TransformationFieldMap.Delete(TransformationName, clientID, ColumnName);
                scope.Complete();
            }

            return res;
        }

        public  int DeleteSourceFileID(int SourceFileID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.TransformationFieldMap.DeleteSourceFileID(SourceFileID);
                scope.Complete();
            }

            return res;
        }

        public  int DeleteFieldMappingID(int FieldMappingID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.TransformationFieldMap.DeleteFieldMappingID(FieldMappingID);
                scope.Complete();
            }

            return res;
        }

        public int DeleteTransformationFieldMapID(int TransformationFieldMapID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.TransformationFieldMap.DeleteTransformationFieldMapID(TransformationFieldMapID);
                scope.Complete();
            }

            return res;
        }        

        public  int DeleteFieldMappingID(string TransformationName, int clientID, int FieldMappingID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.TransformationFieldMap.DeleteFieldMappingID(TransformationName, clientID, FieldMappingID);
                scope.Complete();
            }

            return res;
        }
    }
}

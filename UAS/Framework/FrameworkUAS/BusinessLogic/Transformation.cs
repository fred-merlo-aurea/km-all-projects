using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class Transformation
    {
        public Entity.Transformation SelectTransformationByID(int transformationID)
        {
            Entity.Transformation x = null;
            x = DataAccess.Transformation.SelectTransformationByID(transformationID);

            return x;
        }

        public  List<Entity.Transformation> Select()
        {
            List<Entity.Transformation> x = null;
            x = DataAccess.Transformation.Select().ToList();

            return x;
        }

        public  List<Entity.Transformation> Select(int transformationID)
        {
            List<Entity.Transformation> x = null;
            x = DataAccess.Transformation.Select(transformationID).ToList();

            return x;
        }

        public  List<Entity.Transformation> SelectClient(int clientID, bool includeCustomProperties = false)
        {
            List<Entity.Transformation> x = null;
            x = DataAccess.Transformation.SelectClient(clientID).ToList();

            if (includeCustomProperties)
            {
                foreach (var c in x)
                {
                    TransformationFieldMap tfm = new TransformationFieldMap();
                    TransformationPubMap tpm = new TransformationPubMap();
                    c.FieldMap = tfm.HashSetTransformationID(c.TransformationID);
                    c.PubMap = tpm.HashSetTransformationId(c.TransformationID);
                }

            }
            return x;
        }
        public  List<Entity.Transformation> Select(int clientID, int sourceFileID, bool includeCustomProperties = false)
        {
            List<Entity.Transformation> x = null;
            x = DataAccess.Transformation.Select(clientID,sourceFileID).ToList();

            if (includeCustomProperties)
            {
                foreach (var c in x)
                {
                    TransformationFieldMap tfm = new TransformationFieldMap();
                    TransformationPubMap tpm = new TransformationPubMap();
                    c.FieldMap = tfm.HashSetTransformationID(c.TransformationID);
                    c.PubMap = tpm.HashSetTransformationId(c.TransformationID);
                }

            }
            return x;
        }

        //public  List<Entity.Transformation> Select(string clientName)
        //{
        //    List<Entity.Transformation> x = null;
        //    x = DataAccess.Transformation.Select(clientName).ToList();

        //    return x;
        //}

        public  List<Entity.Transformation> SelectAssigned(string fileName)
        {
            List<Entity.Transformation> x = null;
            x = DataAccess.Transformation.SelectAssigned(fileName).ToList();

            return x;
        }

        public  List<Entity.Transformation> SelectAssigned(int fieldMappingID)
        {
            List<Entity.Transformation> x = null;
            x = DataAccess.Transformation.SelectAssigned(fieldMappingID).ToList();

            return x;
        }

        public  List<Entity.Transformation> SelectTransformationID(string transformationName, string clientName)
        {
            List<Entity.Transformation> x = null;
            x = DataAccess.Transformation.SelectTransformationID(transformationName, clientName).ToList();

            return x;
        }

        public  int Save(Entity.Transformation x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.TransformationID = DataAccess.Transformation.Save(x);
                scope.Complete();
            }

            return x.TransformationID;
        }

        public int Delete(int transformationID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                transformationID = DataAccess.Transformation.Delete(transformationID);
                scope.Complete();
            }

            return transformationID;
        }

        private FrameworkUAS.Entity.Transformation GetCustomProperties(FrameworkUAS.Entity.Transformation transformation)
        {
            TransformationFieldMap tfm = new TransformationFieldMap();
            TransformationPubMap tpm = new TransformationPubMap();
            transformation.FieldMap = tfm.HashSetTransformationID(transformation.TransformationID);
            transformation.PubMap = tpm.HashSetTransformationId(transformation.TransformationID);

            return transformation;
        }

        public int SelectPagingCount(int clientID, bool isTemplate, bool isActive, int TransformationTypeId, bool ignoreAdminTransformationTypes, int adminTransformId)
        {
            int res = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.Transformation.SelectPagingCount(clientID, isTemplate, isActive, TransformationTypeId, ignoreAdminTransformationTypes, adminTransformId);
                scope.Complete();
            }

            return res;
        }
        public List<Entity.Transformation> SelectPaging(int clientID, int currentPage, int pageSize, bool isTemplate, bool isActive, int TransformationTypeId, bool ignoreAdminTransformationTypes, int adminTransformId, string sortField, string sortDirection)
        {
            List<Entity.Transformation> y = null;

            y = DataAccess.Transformation.SelectPaging(clientID, currentPage, pageSize, isTemplate, isActive, TransformationTypeId, ignoreAdminTransformationTypes, adminTransformId, sortField, sortDirection).ToList();
            return y;
        }
    }
}

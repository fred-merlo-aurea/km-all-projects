using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class FieldMapping
    {
        public List<Entity.FieldMapping> Select(bool includeCustomProperties = false)
        {
            List<Entity.FieldMapping> x = null;
            x = DataAccess.FieldMapping.Select().ToList();

            if (includeCustomProperties)
            {
                foreach (var c in x)
                {
                    FieldMultiMap fmm = new FieldMultiMap();
                    c.FieldMultiMappings = fmm.HashSetFieldMappingID(c.FieldMappingID);
                }
            }
            return x;
        }
        public HashSet<Entity.FieldMapping> HashSetSelect(int sourceFileID, bool includeCustomProperties = false)
        {
            var list = Select(sourceFileID, includeCustomProperties);
            HashSet<Entity.FieldMapping> x = new HashSet<Entity.FieldMapping>(list);
            return x;
        }
        public List<Entity.FieldMapping> Select(int sourceFileID, bool includeCustomProperties = false)
        {
            List<Entity.FieldMapping> x = null;
            x = DataAccess.FieldMapping.Select(sourceFileID).ToList();

            if (includeCustomProperties)
            {
                foreach (var c in x)
                {
                    if (c.HasMultiMapping)
                    {
                        FieldMultiMap fmm = new FieldMultiMap();
                        c.FieldMultiMappings = fmm.HashSetFieldMappingID(c.FieldMappingID);
                    }

                }
            }
            return x;
        }
        public Entity.FieldMapping SelectFieldMappingID(int fieldMappingID, bool includeCustomProperties = false)
        {
            Entity.FieldMapping x = null;
            x = DataAccess.FieldMapping.SelectFieldMappingID(fieldMappingID);

            if (includeCustomProperties)
            {
                x = GetCustomProperties(x);                
            }
            return x;
        }
        public List<Entity.FieldMapping> Select(string clientName, bool includeCustomProperties = false)
        {
            List<Entity.FieldMapping> x = null;
            x = DataAccess.FieldMapping.Select(clientName).ToList();

            if (includeCustomProperties)
            {
                foreach (var c in x)
                {
                    FieldMultiMap fmm = new FieldMultiMap();
                    c.FieldMultiMappings = fmm.HashSetFieldMappingID(c.FieldMappingID);
                }
            }
            return x;
        }
        public List<Entity.FieldMapping> Select(int clientID, string fileName, bool includeCustomProperties = true)
        {
            List<Entity.FieldMapping> x = null;
            x = DataAccess.FieldMapping.Select(clientID, fileName).ToList();

            if (includeCustomProperties)
            {
                foreach (var c in x)
                {
                    FieldMultiMap fmm = new FieldMultiMap();
                    c.FieldMultiMappings = fmm.HashSetFieldMappingID(c.FieldMappingID);
                }
            }
            return x;
        }

        public bool ColumnReorder(int SourceFileID)
        {
            bool res = false;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.FieldMapping.ColumnReorder(SourceFileID);
                scope.Complete();
            }

            return res;
        }
       
        public  int Save(Entity.FieldMapping x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                if (x.IsNonFileColumn == true && x.FieldMappingID == 0)
                    x.IncomingField = Core_AMS.Utilities.StringFunctions.RandomAlphaString(5) + "_" + x.IncomingField;

                x.FieldMappingID = DataAccess.FieldMapping.Save(x);
                scope.Complete();
            }

            return x.FieldMappingID;
        }
        public  int Delete(int SourceFileID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.FieldMapping.Delete(SourceFileID);
                scope.Complete();
            }

            return res;
        }
        public  int DeleteMapping(int FieldMappingID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.FieldMapping.DeleteMapping(FieldMappingID);
                scope.Complete();
            }

            return res;
        }
        private FrameworkUAS.Entity.FieldMapping GetCustomProperties(FrameworkUAS.Entity.FieldMapping mapping)
        {
            FieldMultiMap fmm = new FieldMultiMap();
            mapping.FieldMultiMappings = fmm.HashSetFieldMappingID(mapping.FieldMappingID);
            return mapping;
        }
    }
}

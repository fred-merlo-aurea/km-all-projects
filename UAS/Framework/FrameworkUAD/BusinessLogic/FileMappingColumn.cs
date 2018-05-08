using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class FileMappingColumn
    {
        public List<Object.FileMappingColumn> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Object.FileMappingColumn> retList = null;
            retList = DataAccess.FileMappingColumn.Select(client);
            return retList;
        }
        public List<Object.FileMappingColumn> GetMappingColumns(int clientId, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.FileMappingColumn.GetMappingColumns(clientId, client);
        }
        public List<Object.FileMappingColumn> GetMappingColumns(int clientId, int pubId, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.FileMappingColumn.GetMappingColumns(clientId, pubId, client);
        }


        public List<Object.FileMappingColumnValue> GetMappingValues(int clientId, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.FileMappingColumn.GetMappingValues(clientId, client);
        }
        public List<Object.FileMappingColumnValue> GetMappingValues(int clientId, int pubId, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.FileMappingColumn.GetMappingValues(clientId, pubId, client);
        }



        #region Model
        public List<FrameworkUAS.Model.Field> GetFields(KMPlatform.Object.ClientConnections client)
        {
            return ConvertToFields(DataAccess.FileMappingColumn.Select(client));
        }
        public List<FrameworkUAS.Model.Field> GetFields(int clientId, KMPlatform.Object.ClientConnections client)
        {
            return ConvertToFields(DataAccess.FileMappingColumn.GetMappingColumns(clientId, client));
        }
        public List<FrameworkUAS.Model.Field> GetFields(int clientId, int pubId, KMPlatform.Object.ClientConnections client)
        {
            return ConvertToFields(DataAccess.FileMappingColumn.GetMappingColumns(clientId, pubId, client));
        }
        private List<FrameworkUAS.Model.Field> ConvertToFields(List<Object.FileMappingColumn> list)
        {
            List<FrameworkUAS.Model.Field> fieldList = new List<FrameworkUAS.Model.Field>();
            list.ForEach(x =>
            {
                FrameworkUAS.Model.Field f = new FrameworkUAS.Model.Field(x.Id, x.DataTable, x.TablePrefix, x.ColumnName, x.DataType, x.IsDemographic, x.IsDemographicOther, x.IsMultiSelect, x.ClientId, x.UxControl);
                fieldList.Add(f);
            });
            return fieldList;
        }


        public List<FrameworkUAS.Model.FieldValue> GetFieldValues(int clientId, KMPlatform.Object.ClientConnections client)
        {
            return ConvertToFieldValues(DataAccess.FileMappingColumn.GetMappingValues(clientId, client));
        }
        public List<FrameworkUAS.Model.FieldValue> GetFieldValues(int clientId, int pubId, KMPlatform.Object.ClientConnections client)
        {
            return ConvertToFieldValues(DataAccess.FileMappingColumn.GetMappingValues(clientId, pubId, client));
        }
        private List<FrameworkUAS.Model.FieldValue> ConvertToFieldValues(List<Object.FileMappingColumnValue> list)
        {
            List<FrameworkUAS.Model.FieldValue> fieldList = new List<FrameworkUAS.Model.FieldValue>();
            list.ForEach(x =>
            {
                FrameworkUAS.Model.FieldValue fv  = new FrameworkUAS.Model.FieldValue(x.Id, x.ItemText, x.ItemValue, x.ItemOrder);
            });
            return fieldList;
        }
        #endregion
    }
}

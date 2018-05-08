using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;

namespace Core.ADMS
{
    public class TransformerFunctions
    {
        #region StandardTransformations
        public DataTable RenameColumns(DataTable inDataTable, List<FrameworkUAS.Entity.FieldMapping> mapping, int ignTypeID, bool doRemoveColumns = false)
        {
            if (doRemoveColumns)
                inDataTable = RemoveColumns(inDataTable, mapping);

            string currentColumnName;
            string newColumnName;            
            int ignoredTypeID = ignTypeID;
           
            for (int i = 0; i < inDataTable.Columns.Count; i++)
            {
                //Get Current column name
                currentColumnName = inDataTable.Columns[i].ColumnName;
                newColumnName = string.Empty;
                //Determine if the currentColumn was mapped to ignore                

                var map = mapping.SingleOrDefault(x => x.IncomingField.Equals(currentColumnName, StringComparison.CurrentCultureIgnoreCase));//.SingleOrDefault(s => s.IncomingField == currentColumnName);

                if (map != null && map.FieldMappingID > 0)
                {
                    if (map != null && map.FieldMappingTypeID != ignoredTypeID) //map.MAFField != "Ignore")
                    {
                        if (map.MAFField.Equals("DEMO", StringComparison.CurrentCultureIgnoreCase))
                        {
                            newColumnName = map.MAFField + map.PubNumber.ToString();
                        }
                        else
                        {
                            newColumnName = map.MAFField;
                        }

                        try
                        {
                            ////if(!string.Equals(currentColumnName, newColumnName, StringComparison.CurrentCultureIgnoreCase) && inDataTable.Columns.Contains(newColumnName)
                            //// && (mapping.SingleOrDefault(s => s.IncomingField == newColumnName)).MAFField == "Ignore")
                            ////inDataTable.Columns.Remove(newColumnName);
                            if (inDataTable.Columns[newColumnName] == null && map.IsNonFileColumn == false)
                                inDataTable.Columns[i].ColumnName = newColumnName;
                            else
                            {
                                var map2 = mapping.SingleOrDefault(x => x.IncomingField.Equals(newColumnName, StringComparison.CurrentCultureIgnoreCase));
                                if (map2 != null)
                                {
                                    if (map2.FieldMappingTypeID == ignoredTypeID)
                                    {
                                        inDataTable.Columns[newColumnName].ColumnName = newColumnName + "_IGNORE";
                                        inDataTable.Columns[i].ColumnName = newColumnName;
                                    }
                                }
                            }

                            //if (inDataTable.Columns.Contains(newColumnName) == false)
                            //    inDataTable.Columns[i].ColumnName = newColumnName;
                            ////else
                            ////{
                            ////    inDataTable.Columns[newColumnName].ColumnName = newColumnName + i.ToString();
                            ////    inDataTable.Columns[i].ColumnName = newColumnName;
                            ////}
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return inDataTable;
        }

        public DataTable RemoveColumns(DataTable inDataTable, List<FrameworkUAS.Entity.FieldMapping> mapping)
        {
            //FrameworkUAS.BusinessLogic.FieldMappingType fmtData = new FrameworkUAS.BusinessLogic.FieldMappingType();
            int ignoredTypeID = 1;// fmtData.IgnoredTypeID();
            for (int i = 0; i < inDataTable.Columns.Count; i++)
            {
                string currentColumnName = inDataTable.Columns[i].ColumnName;
                var x = mapping.SingleOrDefault(s => s.IncomingField == currentColumnName);

                if (x != null && x.FieldMappingTypeID == ignoredTypeID)
                {
                    inDataTable = RemoveColumn(inDataTable, currentColumnName);
                    i--;
                }
            }
            inDataTable.AcceptChanges();
            return inDataTable;
        }
        public DataTable RemoveColumn(DataTable inDataTable, string columnName)
        {
            inDataTable.Columns.Remove(columnName);
            return inDataTable;
        }
        #endregion

        #region DataMappingTransformations
        public DataTable PerformDataMapping(DataTable inDataTable, int SourceFileID)
        {
            DataTable updated = inDataTable;

            //Get the Data Mappings for the SourceFileID
            //Get FieldMappingIDs from FM, if FMIDS in TPM get TID and apply those in DM based on PubID
            foreach (DataRow dr in updated.Rows)
            {
                dr["PUBCODE"] = "Changed";
            }
            return updated;
        }
        #endregion

        #region AssignValueTransformations
        public DataRow PerformAssignValue(DataRow row, HashSet<FrameworkUAS.Entity.TransformAssign> transAssignNoPubID, HashSet<FrameworkUAS.Entity.FieldMapping> allFieldMappings, HashSet<FrameworkUAS.Entity.Transformation> allTrans)
        {
            foreach (FrameworkUAS.Entity.TransformAssign ta in transAssignNoPubID)
            {
                FrameworkUAS.Entity.Transformation tran = allTrans.Single(x => x.TransformationID == ta.TransformationID);

                foreach (FrameworkUAS.Entity.TransformationFieldMap tfm in tran.FieldMap)
                {
                    FrameworkUAS.Entity.FieldMapping fm = allFieldMappings.SingleOrDefault(x => x.FieldMappingID == tfm.FieldMappingID && tfm.SourceFileID == x.SourceFileID);
                    if(fm != null)
                        row[fm.IncomingField] = ta.Value.Trim();//fm.IncomingField
                }
            }

            return row;
        }
        public StringDictionary AssignValue(StringDictionary row, HashSet<FrameworkUAS.Entity.TransformAssign> transAssignNoPubID, HashSet<FrameworkUAS.Entity.FieldMapping> allFieldMappings, HashSet<FrameworkUAS.Entity.Transformation> allTrans)
        {
            foreach (FrameworkUAS.Entity.TransformAssign ta in transAssignNoPubID)
            {
                FrameworkUAS.Entity.Transformation tran = allTrans.Single(x => x.TransformationID == ta.TransformationID);

                foreach (FrameworkUAS.Entity.TransformationFieldMap tfm in tran.FieldMap)
                {
                    FrameworkUAS.Entity.FieldMapping fm = allFieldMappings.SingleOrDefault(x => x.FieldMappingID == tfm.FieldMappingID && tfm.SourceFileID == x.SourceFileID);
                    if (fm != null)
                        row[fm.IncomingField] = ta.Value.Trim();//fm.IncomingField
                }
            }

            return row;
        }
        #endregion
    }
}

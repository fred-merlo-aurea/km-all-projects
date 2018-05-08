using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Core_AMS.Utilities;
using FrameworkUAD.BusinessLogic.Transformations;
using FrameworkUAD.Object;
using FrameworkUAS.Entity;
using KMPlatform.Entity;
using Enums = FrameworkUAD_Lookup.Enums;
using CommonEnums = KM.Common.Enums;

namespace Core.ADMS
{
    public delegate void LogExceptionDelegate(
        Exception ex,
        int sourceFileId,
        string processCode,
        int clientId,
        int rowNumber,
        StringDictionary rowData);

    public static class FileValidationTransformationHelper
    {
        public const string PubCodeFieldName = "PubCode";
        public const string OriginalImportRowFieldName = "OriginalImportRow";

        public static bool ProcessDataMappingForField(
            SourceFile sourceFile,
            IEnumerable<TransformationFieldMap> transformationFieldMaps,
            StringDictionary myRow,
            TransformDataMap tdmx)
        {
            var dmRan = false;
            foreach (var tfm in transformationFieldMaps)
            {
                var fieldMapping = sourceFile.FieldMappings.SingleOrDefault(x => x.FieldMappingID == tfm.FieldMappingID);
                if (fieldMapping == null ||
                    !myRow.ContainsKey(fieldMapping.IncomingField))
                {
                    continue;
                }

                var matchType = Enums.GetMatchTypeName(tdmx.MatchType);
                if (!string.IsNullOrWhiteSpace(myRow[fieldMapping.IncomingField]))
                {
                    switch (matchType)
                    {
                        case Enums.MatchTypes.Any_Character:
                            if (IsFieldContainsValue(myRow, tdmx, fieldMapping))
                            {
                                myRow[fieldMapping.IncomingField] = tdmx.DesiredData.Trim();
                                dmRan = true;
                            }

                            break;
                        case Enums.MatchTypes.Equals:
                            if (IsFieldEqualsWithValue(myRow, tdmx, fieldMapping))
                            {
                                myRow[fieldMapping.IncomingField] = tdmx.DesiredData.Trim();
                                dmRan = true;
                            }

                            break;
                        case Enums.MatchTypes.Like:
                            if (IsFieldContainsValue(myRow, tdmx, fieldMapping))
                            {
                                myRow[fieldMapping.IncomingField] = tdmx.DesiredData.Trim();
                                dmRan = true;
                            }

                            break;
                        case Enums.MatchTypes.Not_Equals:
                            if (!IsFieldEqualsWithValue(myRow, tdmx, fieldMapping))
                            {
                                myRow[fieldMapping.IncomingField] = tdmx.DesiredData.Trim();
                                dmRan = true;
                            }

                            break;
                        case Enums.MatchTypes.Has_Data:
                            if (!string.IsNullOrWhiteSpace(myRow[fieldMapping.IncomingField]))
                            {
                                myRow[fieldMapping.IncomingField] = tdmx.DesiredData.Trim();
                                dmRan = true;
                            }

                            break;
                        case Enums.MatchTypes.Is_Null_or_Empty:
                            if (string.IsNullOrWhiteSpace(myRow[fieldMapping.IncomingField]))
                            {
                                myRow[fieldMapping.IncomingField] = tdmx.DesiredData.Trim();
                                dmRan = true;
                            }

                            break;
                        case Enums.MatchTypes.Find_and_Replace:
                            if (!IsFieldEqualsWithValue(myRow, tdmx, fieldMapping))
                            {
                                myRow[fieldMapping.IncomingField] = myRow[fieldMapping.IncomingField]
                                    .Replace(tdmx.SourceData, tdmx.DesiredData).Trim();
                                dmRan = true;
                            }

                            break;
                        default:
                            if (matchType.Equals(Enums.MatchTypes.Default))
                            {
                                {
                                    myRow[fieldMapping.IncomingField] = tdmx.DesiredData.Trim();
                                    dmRan = true;
                                }
                            }

                            break;
                    }
                }
                else if (matchType.Equals(Enums.MatchTypes.Is_Null_or_Empty))
                {
                    if (string.IsNullOrWhiteSpace(myRow[fieldMapping.IncomingField.ToUpper()]))
                    {
                        myRow[fieldMapping.IncomingField] = tdmx.DesiredData.Trim();
                        dmRan = true;
                    }
                }
            }

            return dmRan;
        }

        public static void ProcessPubCodeSpecialSplitTransformationsForRow(
            SourceFile sourceFile,
            ImportFile importFile,
            TransformationDefinitionsGrouping transformConfigs,
            StringDictionary myRow,
            IList<Transformation> splitList,
            ICollection<int> deleteKeys,
            Dictionary<int, StringDictionary> addRows,
            int key)
        {
            var performedFirstSplitIntoRow = false;
            foreach (var transformation in splitList)
            {
                foreach (var tsx in transformConfigs.GetTransformSplits(transformation.TransformationID))
                {
                    var tranx = transformConfigs.GetTransformation(tsx.TransformationID);
                    if (tranx == null)
                    {
                        continue;
                    }

                    performedFirstSplitIntoRow = tranx.FieldMap.Where(x => x.SourceFileID == importFile.SourceFileId)
                        .Select(tfm => sourceFile.FieldMappings.SingleOrDefault(x => x.FieldMappingID == tfm.FieldMappingID))
                        .Where(fm => fm != null)
                        .Aggregate(performedFirstSplitIntoRow, (current, fm) => PerformSplitForField(importFile, myRow, deleteKeys, addRows, key, current, fm, tsx));
                }
            }
        }

        public static void ProcessPubCodeDataMapTransformationsForRow(
            SourceFile sourceFile,
            ImportFile importFile,
            TransformationDefinitionsGrouping transformConfigs,
            StringDictionary myRow,
            KeyValuePair<int, string> kvp,
            IEnumerable<Transformation> dmList)
        {
            foreach (var transformation in dmList.Where(t => !t.LastStepDataMap))
            {
                foreach (var tdmx in transformConfigs.GetDataMaps(transformation.TransformationID, kvp.Key))
                {
                    var tranx = transformConfigs.GetTransformation(tdmx.TransformationID);
                    if (tranx == null)
                    {
                        continue;
                    }

                    var dmRan = false;
                    foreach (var fm in tranx.FieldMap.Where(x => x.SourceFileID == importFile.SourceFileId)
                        .Select(tfm => sourceFile.FieldMappings.SingleOrDefault(x => x.FieldMappingID == tfm.FieldMappingID))
                        .Where(fm => fm != null && myRow.ContainsKey(fm.IncomingField.ToUpper())))
                    {
                        dmRan = ApplyPubCodeDataMapTransformationForField(myRow, tdmx, fm);
                    }
                    if (dmRan)
                    {
                        break;
                    }
                }
            }
        }

        public static void ProcessPubCodeAssignTransformationsForRow(
            SourceFile sourceFile,
            ImportFile importFile,
            TransformationDefinitionsGrouping transformConfigs,
            StringDictionary myRow,
            IEnumerable<Transformation> assignList)
        {
            foreach (var t in assignList)
            {
                foreach (var tax in transformConfigs.GetTransAssigns(t.TransformationID))
                {
                    var tranx = transformConfigs.GetTransformation(tax.TransformationID);
                    if (tranx != null)
                    {
                        var fieldMappings =
                        (from tfm in tranx.FieldMap.Where(x => x.SourceFileID == importFile.SourceFileId)
                         select sourceFile.FieldMappings.SingleOrDefault(x => x.FieldMappingID == tfm.FieldMappingID)
                            into fm
                         where fm != null
                         where myRow.ContainsKey(fm.IncomingField.ToUpper())
                         where !string.IsNullOrWhiteSpace(myRow[fm.IncomingField.ToUpper()])
                         select fm);
                        foreach (var fieldMapping in fieldMappings)
                        {
                            myRow[fieldMapping.IncomingField.ToUpper()] = tax.Value.Trim();
                        }
                    }
                }
            }
        }

        public static void ProcessDataMapTransformsWithMapsPubCode(
            SourceFile sourceFile,
            ImportFile importFile,
            ServiceFeature serviceFeature,
            Dictionary<int, string> clientPubCodes,
            TransformationDefinitionsGrouping transformConfigs,
            FieldMapping fmPubCode,
            LogExceptionDelegate logExceptionFunc)
        {
            var dmSetPubCodeList = transformConfigs.GetDataMapsActive();
            transformConfigs.RemoveDataMaps(dmSetPubCodeList);
            if (dmSetPubCodeList.Any())
            {
                foreach (var key in importFile.DataTransformed.Keys)
                {
                    var myRow = importFile.DataTransformed[key];
                    try
                    {
                        var pubCodeId = -1;
                        if (KMPlatform.BusinessLogic.Enums.GetUADFeature(serviceFeature.SFName) != KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare &&
                            !string.IsNullOrWhiteSpace(myRow[fmPubCode.IncomingField]) &&
                            clientPubCodes.ContainsValue(myRow[fmPubCode.IncomingField]))
                        {
                            pubCodeId = clientPubCodes
                                .Single(x => x.Value.Equals(myRow[fmPubCode.IncomingField].ToString())).Key;
                        }

                        if (pubCodeId == -1)
                        {
                            foreach (var t in dmSetPubCodeList)
                            {
                                foreach (var tdmx in transformConfigs.GetDataMaps(t.TransformationID))
                                {
                                    var tranx = transformConfigs.GetTransformation(tdmx.TransformationID);
                                    if (tranx == null)
                                    {
                                        continue;
                                    }

                                    var tranFieldMap = tranx.FieldMap.Where(x => x.SourceFileID == importFile.SourceFileId).ToList();
                                    if (tranFieldMap.Any())
                                    {
                                        foreach (var transformationFieldMap in tranx.FieldMap)
                                        {
                                            ApplyTransformationForPubCode(sourceFile, transformationFieldMap, myRow, tdmx);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logExceptionFunc(ex, importFile.SourceFileId, importFile.ProcessCode, importFile.ClientId, key, myRow);
                    }
                }
            }
        }

        public static void DiscoverTransformAssignsWithoutPubId(
            SourceFile sourceFile,
            ImportFile importFile,
            TransformationDefinitionsGrouping transformConfigs,
            LogExceptionDelegate logExceptionFunc)
        {
            var transAssignNoPubId = new HashSet<TransformAssign>(transformConfigs.GetAllTransAssigns().Where(x => x.HasPubID == false));
            foreach (var originalRow in importFile.DataOriginal)
            {
                try
                {
                    var newRow = new TransformerFunctions().AssignValue(
                        originalRow.Value,
                        transAssignNoPubId,
                        sourceFile.FieldMappings,
                        transformConfigs.AllTrans);
                    importFile.DataTransformed.Add(originalRow.Key, newRow);
                }
                catch (Exception ex)
                {
                    logExceptionFunc(ex, importFile.SourceFileId, importFile.ProcessCode, importFile.ClientId, originalRow.Key, originalRow.Value);
                }
            }
        }

        public static void ProcessSplitTransformations(
            SourceFile sourceFile,
            ImportFile importFile,
            TransformationDefinitionsGrouping transformConfigs,
            LogExceptionDelegate logExceptionFunc)
        {
            if (transformConfigs.TransSplit.Count > 0)
            {
                ProcessSplitBefore(importFile, transformConfigs, logExceptionFunc);

                ProcessSplitDataMapAndAfter(importFile, transformConfigs, logExceptionFunc);
            }
        }

        public static void DiscoverColumns(SourceFile sourceFile, ImportFile importFile, ICollection<FieldMultiMap> allMultiMappings, StringDictionary multiMapToFieldMap)
        {
            foreach (var fm in sourceFile.FieldMappings)
            {
                if (fm.IsNonFileColumn)
                {
                    var column = fm.MAFField.Equals(PubCodeFieldName, StringComparison.CurrentCultureIgnoreCase)
                        ? fm.MAFField
                        : fm.IncomingField;

                    if (!importFile.HeadersOriginal.ContainsKey(column))
                    {
                        importFile.HeadersOriginal.Add(column, (importFile.HeadersOriginal.Count + 1).ToString());
                    }
                }
            }

            foreach (var fm in sourceFile.FieldMappings)
            {
                foreach (var fmm in fm.FieldMultiMappings)
                {
                    allMultiMappings.Add(fmm);
                    var column = (fm.MAFField.Equals(PubCodeFieldName, StringComparison.CurrentCultureIgnoreCase))
                        ? fm.MAFField
                        : fm.IncomingField;

                    if (!importFile.HeadersOriginal.ContainsKey(column))
                    {
                        if (!multiMapToFieldMap.ContainsKey(fmm.MAFField))
                        {
                            multiMapToFieldMap.Add(fmm.MAFField, fm.IncomingField);
                        }

                        importFile.HeadersOriginal.Add(column, (importFile.HeadersOriginal.Count + 1).ToString());
                    }
                }
            }

            if (!importFile.HeadersOriginal.ContainsValue("OriginalImportRow"))
            {
                importFile.HeadersOriginal.Add("OriginalImportRow", (importFile.HeadersOriginal.Count + 1).ToString());
            }

            var origRowCount = 1;
            foreach (var key in importFile.DataOriginal.Keys)
            {
                var myRow = importFile.DataOriginal[key];
                if (!myRow.ContainsKey("OriginalImportRow"))
                {
                    myRow.Add("OriginalImportRow", origRowCount.ToString());
                }

                origRowCount++;
            }
        }

        public static void ProcessOriginalImportRowColumn(ImportFile importFile)
        {
            foreach (var key in importFile.DataTransformed.Keys)
            {
                var myRow = importFile.DataTransformed[key];
                int orig;
                int.TryParse(myRow[OriginalImportRowFieldName], out orig);
                importFile.TransformedRowToOriginalRowMap.Add(key, orig);
            }
        }

        public static void FillImportFileHeaderTransformedColumns(ImportFile importFile, SourceFile sourceFile, List<FieldMultiMap> allMultiMappings)
        {
            foreach (string column in importFile.HeadersOriginal.Keys)
            {
                if (column.Equals(PubCodeFieldName, StringComparison.CurrentCultureIgnoreCase))
                {
                    var incomingCol = sourceFile.FieldMappings
                        .SingleOrDefault(x => x.MAFField.Equals(column, StringComparison.CurrentCultureIgnoreCase))?.MAFField
                        .ToUpper();
                    AddImportFileHeaderTransformed(importFile, incomingCol, column);
                }
                else if (sourceFile.FieldMappings.Any(x =>
                    x.IncomingField.Equals(column, StringComparison.CurrentCultureIgnoreCase)))
                {
                    var incomingCol = sourceFile.FieldMappings
                        .Single(x => x.IncomingField.Equals(column, StringComparison.CurrentCultureIgnoreCase))
                        .IncomingField.ToUpper();
                    AddImportFileHeaderTransformed(importFile, incomingCol, column);
                }
                else if (allMultiMappings.Exists(x => x.MAFField.Equals(column, StringComparison.CurrentCultureIgnoreCase)))
                {
                    var incomingCol = allMultiMappings
                        .Single(x => x.MAFField.Equals(column, StringComparison.CurrentCultureIgnoreCase)).MAFField
                        .ToUpper();
                    AddImportFileHeaderTransformed(importFile, incomingCol, column);
                }
            }

            if (!importFile.HeadersTransformed.ContainsKey("OriginalImportRow"))
            {
                importFile.HeadersTransformed.Add("OriginalImportRow", (importFile.HeadersTransformed.Count + 1).ToString());
            }
        }

        public static bool IsFieldEqualsWithValue(StringDictionary myRow, TransformDataMap tdmx, FieldMapping fieldMapping)
        {
            return myRow[fieldMapping.IncomingField].Trim().Equals(tdmx.SourceData, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool IsFieldContainsValue(StringDictionary myRow, TransformDataMap tdmx, FieldMapping fieldMapping)
        {
            return myRow[fieldMapping.IncomingField].Trim().IndexOf(tdmx.SourceData, StringComparison.OrdinalIgnoreCase)>=0;
        }

        private static void ProcessSplitBefore(ImportFile importFile,
            TransformationDefinitionsGrouping transformConfigs,
            LogExceptionDelegate logExceptionFunc)
        {
            var removeKeys = new List<int>();
            var addrows = new Dictionary<int, StringDictionary>();

            foreach (var key in importFile.DataTransformed.Keys)
            {
                var myRow = importFile.DataTransformed[key];
                try
                {
                    foreach (var transformation in transformConfigs.TransSplit)
                    {
                        var transformSplitTrans = transformConfigs.GetGetSplitTrans(transformation.TransformationID);
                        if (transformSplitTrans == null)
                        {
                            continue;
                        }

                        if (transformSplitTrans.SplitBeforeID != 0)
                        {
                            ApplySplitBeforeTransformation(
                                importFile,
                                transformConfigs,
                                transformSplitTrans,
                                myRow,
                                removeKeys,
                                key,
                                addrows);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logExceptionFunc(ex, importFile.SourceFileId, importFile.ProcessCode, importFile.ClientId, key, myRow);
                }
            }

            ApplyAddRemoveRows(importFile, addrows, removeKeys);
        }

        private static void ProcessSplitDataMapAndAfter(ImportFile importFile,
            TransformationDefinitionsGrouping transformConfigs,
            LogExceptionDelegate logExceptionFunc)
        {
            var removeKeys = new List<int>();
            var addrows = new Dictionary<int, StringDictionary>();

            foreach (var key in importFile.DataTransformed.Keys)
            {
                var myRow = importFile.DataTransformed[key];
                try
                {
                    foreach (var transformation in transformConfigs.TransSplit)
                    {
                        var transformSplitTrans = transformConfigs.GetGetSplitTrans(transformation.TransformationID);
                        if (transformSplitTrans == null)
                        {
                            continue;
                        }

                        foreach (var tdm in transformConfigs.GetEveryTransDataMaps(transformSplitTrans.DataMapID))
                        {
                            ApplyEveryTransDataMapping(myRow, transformSplitTrans, tdm);
                        }

                        if (transformSplitTrans.SplitAfterID != 0)
                        {
                            ApplySplitAfterTransformation(
                                importFile,
                                transformConfigs,
                                transformSplitTrans,
                                myRow,
                                removeKeys,
                                key,
                                addrows);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logExceptionFunc(ex, importFile.SourceFileId, importFile.ProcessCode, importFile.ClientId, key, myRow);
                }
            }

            ApplyAddRemoveRows(importFile, addrows, removeKeys);
        }

        private static void ApplyAddRemoveRows(ImportFile importFile, Dictionary<int, StringDictionary> addrows, List<int> removeKeys)
        {
            foreach (var row in addrows)
            {
                importFile.DataTransformed.Add(row.Key, row.Value);
            }

            foreach (var key in removeKeys)
            {
                importFile.DataTransformed.Remove(key);
            }

            //reset key values on Transformed Dictionary
            var newKey = 1;
            var keysCopy = new List<int>(importFile.DataTransformed.Keys);
            foreach (var key in keysCopy)
            {
                importFile.DataTransformed.ChangeKey(key, newKey);
                newKey++;
            }
        }

        private static void ApplySplitAfterTransformation(
           ImportFile importFile,
           TransformationDefinitionsGrouping transformConfigs,
           TransformSplitTrans transformSplitTrans,
           StringDictionary myRow,
           List<int> removeKeys,
           int key,
           Dictionary<int, StringDictionary> addrows)
        {
            var data = new List<string>();
            var ts = transformConfigs.GetEveryTransSplitMap(transformSplitTrans.SplitAfterID);
            if (ts != null &&
                myRow.ContainsKey(transformSplitTrans.Column) &&
                !string.IsNullOrWhiteSpace(myRow[transformSplitTrans.Column]))
            {
                var del = CommonEnums.GetDelimiterSymbol(ts.Delimiter);
                if (del.HasValue)
                {
                    data = myRow[transformSplitTrans.Column].Trim().Split(del.Value).ToList();
                }
            }

            if (data.Count > 0)
            {
                removeKeys.Add(key);
            }

            foreach (var s in data)
            {
                myRow[transformSplitTrans.Column] = s.Trim();
                addrows.Add(importFile.DataTransformed.Count + addrows.Count + 1, myRow);
            }
        }

        private static void AddImportFileHeaderTransformed(ImportFile importFile, string incomingCol, string column)
        {
            if (!string.IsNullOrWhiteSpace(incomingCol) && !importFile.HeadersTransformed.ContainsKey(incomingCol))
            {
                importFile.HeadersTransformed.Add(incomingCol, (importFile.HeadersTransformed.Count + 1).ToString());
            }
            else
            {
                if (!importFile.HeadersTransformed.ContainsKey(column))
                {
                    importFile.HeadersTransformed.Add(column, (importFile.HeadersTransformed.Count + 1).ToString());
                }
            }
        }

        private static void ApplyEveryTransDataMapping(StringDictionary myRow, TransformSplitTrans transformSplitTrans,
            TransformDataMap tdm)
        {
            if (!myRow.ContainsKey(transformSplitTrans.Column) ||
                string.IsNullOrWhiteSpace(myRow[transformSplitTrans.Column]))
            {
                return;
            }

            var matchType = tdm.MatchType.Replace(" ", "_");
            if (matchType.Equals(Enums.MatchTypes.Any_Character.ToString()))
            {
                if (myRow[transformSplitTrans.Column].ToLower().Contains(tdm.SourceData.ToLower()))
                {
                    myRow[transformSplitTrans.Column] = tdm.DesiredData.Trim();
                }
            }
            else if (matchType.Equals(Enums.MatchTypes.Equals.ToString()))
            {
                if (myRow[transformSplitTrans.Column].Equals(tdm.SourceData, StringComparison.CurrentCultureIgnoreCase))
                {
                    myRow[transformSplitTrans.Column] = tdm.DesiredData.Trim();
                }
            }
            else if (matchType.Equals(Enums.MatchTypes.Like.ToString()))
            {
                if (myRow[transformSplitTrans.Column].ToLower().Contains(tdm.SourceData.ToLower()))
                {
                    myRow[transformSplitTrans.Column] = tdm.DesiredData.Trim();
                }
            }
            else if (matchType.Equals(Enums.MatchTypes.Not_Equals.ToString()))
            {
                if (!myRow[transformSplitTrans.Column].Equals(tdm.SourceData, StringComparison.CurrentCultureIgnoreCase))
                {
                    myRow[transformSplitTrans.Column] = tdm.DesiredData.Trim();
                }
            }
            else if (matchType.Equals(Enums.MatchTypes.Has_Data.ToString()))
            {
                if (!string.IsNullOrWhiteSpace(myRow[transformSplitTrans.Column]))
                {
                    myRow[transformSplitTrans.Column] = tdm.DesiredData.Trim();
                }
            }
            else if (matchType.Equals(Enums.MatchTypes.Is_Null_or_Empty.ToString()))
            {
                if (string.IsNullOrWhiteSpace(myRow[transformSplitTrans.Column]))
                {
                    myRow[transformSplitTrans.Column] = tdm.DesiredData.Trim();
                }
            }
            else if (matchType.Equals(Enums.MatchTypes.Find_and_Replace.ToString()))
            {
                if (!myRow[transformSplitTrans.Column].Equals(tdm.SourceData, StringComparison.CurrentCultureIgnoreCase))
                {
                    myRow[transformSplitTrans.Column] = myRow[transformSplitTrans.Column].Replace(tdm.SourceData, tdm.DesiredData).Trim();
                }
            }
            else if (matchType.Equals(Enums.MatchTypes.Default.ToString()))
            {
                myRow[transformSplitTrans.Column] = tdm.DesiredData.Trim();
            }
        }

        private static void ApplySplitBeforeTransformation(
            ImportFile importFile,
            TransformationDefinitionsGrouping transformConfigs,
            TransformSplitTrans transformSplitTrans,
            StringDictionary myRow,
            ICollection<int> removeKeys,
            int key,
            IDictionary<int, StringDictionary> addrows)
        {
            var data = new List<string>();
            var transformSplit =transformConfigs.EveryTransSplitMap.SingleOrDefault(x => x.TransformationID == transformSplitTrans.SplitBeforeID);
            if (transformSplit == null ||
                !myRow.ContainsKey(transformSplitTrans.Column) ||
                string.IsNullOrWhiteSpace(myRow[transformSplitTrans.Column]))
            {
                return;
            }

            var del = CommonEnums.GetDelimiterSymbol(transformSplit.Delimiter);
            if (del.HasValue)
            {
                data = myRow[transformSplitTrans.Column].Trim().Split(del.Value).ToList();
            }

            if (data.Count > 0)
            {
                removeKeys.Add(key);
            }

            foreach (var token in data)
            {
                var sdNew = new StringDictionary();
                foreach (string modifiedKey in myRow.Keys)
                {
                    sdNew.Add(modifiedKey, myRow[modifiedKey]);
                }

                sdNew[transformSplitTrans.Column] = token.Trim();
                addrows.Add(importFile.DataTransformed.Count + addrows.Count + 1, sdNew);
            }
        }

        private static bool ApplyPubCodeDataMapTransformationForField(
            StringDictionary myRow,
            TransformDataMap tdmx,
            FieldMapping fm)
        {
            var dmRan = false;
            var matchType = tdmx.MatchType.Replace(" ", "_");
            if (!string.IsNullOrWhiteSpace(myRow[fm.IncomingField.ToUpper()]))
            {
                if (matchType.Equals(Enums.MatchTypes.Any_Character.ToString()))
                {
                    if (IsFieldContainsValue(myRow, tdmx, fm))
                    {
                        myRow[fm.IncomingField.ToUpper()] = tdmx.DesiredData.Trim();
                        dmRan = true;
                    }
                }
                else if (matchType.Equals(Enums.MatchTypes.Equals.ToString()))
                {
                    if (IsFieldEqualsWithValue(myRow, tdmx, fm))
                    {
                        myRow[fm.IncomingField.ToUpper()] = tdmx.DesiredData.Trim();
                        dmRan = true;
                    }
                }
                else if (matchType.Equals(Enums.MatchTypes.Like.ToString()))
                {
                    if (IsFieldContainsValue(myRow, tdmx, fm))
                    {
                        myRow[fm.IncomingField.ToUpper()] = tdmx.DesiredData.Trim();
                        dmRan = true;
                    }
                }
                else if (matchType.Equals(Enums.MatchTypes.Not_Equals.ToString()))
                {
                    if (!IsFieldEqualsWithValue(myRow, tdmx, fm))
                    {
                        myRow[fm.IncomingField.ToUpper()] = tdmx.DesiredData.Trim();
                        dmRan = true;
                    }
                }
                else if (matchType.Equals(Enums.MatchTypes.Has_Data.ToString()))
                {
                    if (!string.IsNullOrWhiteSpace(myRow[fm.IncomingField.ToUpper()]))
                    {
                        myRow[fm.IncomingField.ToUpper()] = tdmx.DesiredData.Trim();
                        dmRan = true;
                    }
                }
                else if (matchType.Equals(Enums.MatchTypes.Is_Null_or_Empty.ToString()))
                {
                    if (string.IsNullOrWhiteSpace(myRow[fm.IncomingField.ToUpper()]))
                    {
                        myRow[fm.IncomingField.ToUpper()] = tdmx.DesiredData.Trim();
                        dmRan = true;
                    }
                }
                else if (matchType.Equals(Enums.MatchTypes.Find_and_Replace.ToString()))
                {
                    if (!IsFieldEqualsWithValue(myRow, tdmx, fm))
                    {
                        myRow[fm.IncomingField.ToUpper()] = myRow[fm.IncomingField].Replace(tdmx.SourceData, tdmx.DesiredData).Trim();
                        dmRan = true;
                    }
                }
                else if (matchType.Equals(Enums.MatchTypes.Default.ToString()))
                {
                    {
                        myRow[fm.IncomingField.ToUpper()] = tdmx.DesiredData.Trim();
                        dmRan = true;
                    }
                }
            }
            else if (matchType.Equals(Enums.MatchTypes.Is_Null_or_Empty.ToString()))
            {
                if (string.IsNullOrWhiteSpace(myRow[fm.IncomingField.ToUpper()]))
                {
                    myRow[fm.IncomingField.ToUpper()] = tdmx.DesiredData.Trim();
                    dmRan = true;
                }
            }

            return dmRan;
        }

        private static bool PerformSplitForField(
            ImportFile importFile,
            StringDictionary myRow,
            ICollection<int> deleteKeys,
            IDictionary<int, StringDictionary> addRows,
            int key,
            bool performedFirstSplitIntoRow,
            FieldMapping fieldMapping,
            TransformSplit trasformSplit)
        {
            if (!performedFirstSplitIntoRow)
            {
                var data = new List<string>();
                if (myRow.ContainsKey(fieldMapping.IncomingField.ToUpper()) &&
                    !string.IsNullOrWhiteSpace(
                        myRow[fieldMapping.IncomingField.ToUpper()]))
                {
                    var del = CommonEnums.GetDelimiterSymbol(trasformSplit.Delimiter);
                    if (del.HasValue)
                    {
                        var token = myRow[fieldMapping.IncomingField.ToUpper()].Trim();
                        if (del == ':')
                        {
                            token = token.Replace("\"", "");
                        }

                        data = token.Split(del.Value).ToList();
                    }
                }

                if (data.Count > 0)
                {
                    deleteKeys.Add(key);
                }

                foreach (var token in data)
                {
                    var sdNew = new StringDictionary();
                    foreach (string modifiedKey in myRow.Keys)
                    {
                        sdNew.Add(modifiedKey, myRow[modifiedKey]);
                    }

                    sdNew[fieldMapping.IncomingField.ToUpper()] = token.Trim();

                    var addCount = addRows.Count;
                    addRows.Add(importFile.DataTransformed.Count + addCount + 1, sdNew);
                }
            }
            else
            {
                var data = new List<string>();
                if (!string.IsNullOrWhiteSpace(myRow[fieldMapping.IncomingField.ToUpper()]))
                {
                    var del = CommonEnums.GetDelimiterSymbol(trasformSplit.Delimiter);
                    if (del.HasValue)
                    {
                        data = myRow[fieldMapping.IncomingField.ToUpper()].Trim().Split(del.Value).ToList();
                    }
                }

                foreach (var token in data)
                {
                    var sdNew = new StringDictionary();
                    foreach (string modifiedKey in myRow.Keys)
                    {
                        sdNew.Add(modifiedKey, myRow[modifiedKey]);
                    }

                    sdNew[fieldMapping.IncomingField.ToUpper()] = token.Trim();
                    addRows.Add(importFile.DataTransformed.Count + addRows.Count + 1, sdNew);
                }
            }

            return true;
        }

        private static void ApplyTransformationForPubCode(
            SourceFile sourceFile,
            TransformationFieldMap transformationFieldMap,
            StringDictionary myRow,
            TransformDataMap tdmx)
        {
            var fieldMapping = sourceFile.FieldMappings.SingleOrDefault(x =>
                x.FieldMappingID == transformationFieldMap.FieldMappingID &&
                x.MAFField.Equals(PubCodeFieldName, StringComparison.CurrentCultureIgnoreCase));
            if (fieldMapping == null ||
                !myRow.ContainsKey(fieldMapping.IncomingField.ToUpper()) ||
                string.IsNullOrWhiteSpace(myRow[fieldMapping.IncomingField.ToUpper()]))
            {
                return;
            }

            var matchType = tdmx.MatchType.Replace(" ", "_");
            if (matchType.Equals(Enums.MatchTypes.Any_Character.ToString()) ||
                matchType.Equals(Enums.MatchTypes.Like.ToString()))
            {
                if (IsFieldContainsValue(myRow, tdmx, fieldMapping))
                {
                    myRow[fieldMapping.IncomingField.ToUpper()] = tdmx.DesiredData;
                }
            }
            else if (matchType.Equals(Enums.MatchTypes.Equals.ToString()))
            {
                if (IsFieldEqualsWithValue(myRow, tdmx, fieldMapping))
                {
                    myRow[fieldMapping.IncomingField.ToUpper()] = tdmx.DesiredData;
                }
            }
            else if (matchType.Equals(Enums.MatchTypes.Not_Equals.ToString()))
            {
                if (!IsFieldEqualsWithValue(myRow, tdmx, fieldMapping))
                {
                    myRow[fieldMapping.IncomingField.ToUpper()] = tdmx.DesiredData;
                }
            }
            else if (matchType.Equals(Enums.MatchTypes.Find_and_Replace.ToString()))
            {
                if (!IsFieldEqualsWithValue(myRow, tdmx, fieldMapping))
                {
                    myRow[fieldMapping.IncomingField.ToUpper()] =
                        myRow[fieldMapping.IncomingField.ToUpper()]
                            .Replace(tdmx.SourceData, tdmx.DesiredData).Trim();
                }
            }
        }
    }
}

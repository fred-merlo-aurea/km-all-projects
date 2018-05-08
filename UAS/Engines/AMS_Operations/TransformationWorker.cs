using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Core.ADMS;
using Core_AMS.Utilities;
using FrameworkUAD.BusinessLogic.Transformations;
using FrameworkUAD.Object;
using FrameworkUAD_Lookup.BusinessLogic;
using FrameworkUAS.Entity;
using KMPlatform.Entity;
using Client = KMPlatform.Entity.Client;
using Enums = FrameworkUAD_Lookup.Enums;
using Transformation = FrameworkUAS.BusinessLogic.Transformation;
using TransformAssign = FrameworkUAS.BusinessLogic.TransformAssign;
using TransformDataMap = FrameworkUAS.BusinessLogic.TransformDataMap;
using TransformJoin = FrameworkUAS.BusinessLogic.TransformJoin;
using TransformSplit = FrameworkUAS.BusinessLogic.TransformSplit;
using TransformSplitTrans = FrameworkUAS.BusinessLogic.TransformSplitTrans;
using BusinessEnums = KMPlatform.BusinessLogic.Enums;

namespace AMS_Operations
{
    public delegate bool ValidationErrorDelegate(string errorMsg, int rowNumber = 0, StringDictionary rowData = null);

    public class TransformationWorker
    {
        private const string MethodLogNameExceptionMessage = "DQM.Helpers.Validation.FileValidator.ProcessFileAsObject - Unhandled Exception";
        private readonly Client _client;
        private readonly Dictionary<int, string> _clientPubCodes;
        private readonly Code _codeWorker;
        private readonly ImportFile _dataIv;
        private readonly List<string> _errorMessages;
        private readonly LogExceptionDelegate _logException;
        private readonly ServiceFeature _serviceFeature;
        private readonly SourceFile _sourceFile;
        private readonly ValidationErrorDelegate _validationError;

        public TransformationWorker(
            ImportFile dataIv,
            ValidationErrorDelegate validateErrorFunc,
            List<string> errorMessages,
            Client client,
            SourceFile sourceFile,
            Code codeWorker,
            LogExceptionDelegate logExceptionFunc,
            ServiceFeature serviceFeature,
            Dictionary<int, string> clientPubCodes)
        {
            _dataIv = dataIv;
            _validationError = validateErrorFunc;
            _errorMessages = errorMessages;
            _client = client;
            _sourceFile = sourceFile;
            _codeWorker = codeWorker;
            _logException = logExceptionFunc;
            _serviceFeature = serviceFeature;
            _clientPubCodes = clientPubCodes;
        }

        public void TransformImportFileData()
        {
            try
            {
                var multiMapToFieldMap = new StringDictionary();
                var allMultiMappings = new List<FieldMultiMap>();

                Console.WriteLine("Transformations");
                var transformationConfig = PopulateTransformationDefinitionGroups();
                DiscoverMappingFields(allMultiMappings, multiMapToFieldMap);
                AddMissingHeadersTransformed(allMultiMappings);

                var fmPubCode = _sourceFile.FieldMappings.SingleOrDefault(x =>
                    x.MAFField.Equals("PubCode", StringComparison.CurrentCultureIgnoreCase));

                FileValidationTransformationHelper.DiscoverTransformAssignsWithoutPubId(_sourceFile, _dataIv, transformationConfig, _logException);
                FileValidationTransformationHelper.ProcessSplitTransformations(_sourceFile, _dataIv, transformationConfig, _logException);
                FileValidationTransformationHelper.ProcessDataMapTransformsWithMapsPubCode(_sourceFile, _dataIv, _serviceFeature, _clientPubCodes, transformationConfig, fmPubCode, _logException);
                ProcessTransformationsByPubCode(transformationConfig, fmPubCode);
                ProcessJoinTransforms(transformationConfig, fmPubCode);
                ProcessDataMapTransforms(transformationConfig, fmPubCode);
                ProcessMultimappingsSetValueFromFieldMapping(multiMapToFieldMap);

                FileValidationTransformationHelper.ProcessOriginalImportRowColumn(_dataIv);
                _dataIv.TransformedRowCount = _dataIv.DataTransformed.Count;
            }
            catch (Exception ex)
            {
                var sbDetail = new StringBuilder();
                sbDetail.AppendLine(MethodLogNameExceptionMessage);
                sbDetail.AppendLine(Environment.NewLine);

                sbDetail.AppendLine(StringFunctions.FormatException(ex));
                _validationError(sbDetail.ToString());
                _errorMessages.Add(sbDetail.ToString());
            }
        }

        private void AddMissingHeadersTransformed(List<FieldMultiMap> allMultiMappings)
        {
            foreach (string column in _dataIv.HeadersOriginal.Keys)
            {
                if (column.Equals(FileValidationTransformationHelper.PubCodeFieldName, StringComparison.CurrentCultureIgnoreCase))
                {
                    var incomingCol = _sourceFile.FieldMappings.Single(x => x.MAFField.Equals(column, StringComparison.CurrentCultureIgnoreCase)).MAFField.ToUpper();
                    if (!string.IsNullOrWhiteSpace(incomingCol) && !_dataIv.HeadersTransformed.ContainsKey(incomingCol))
                    {
                        _dataIv.HeadersTransformed.Add(incomingCol, (_dataIv.HeadersTransformed.Count + 1).ToString());
                    }
                    else
                    {
                        if (!_dataIv.HeadersTransformed.ContainsKey(column))
                        {
                            _dataIv.HeadersTransformed.Add(column, (_dataIv.HeadersTransformed.Count + 1).ToString());
                        }
                    }
                }
                else if (_sourceFile.FieldMappings.Any(x =>x.IncomingField.Equals(column, StringComparison.CurrentCultureIgnoreCase)))
                {
                    var incomingCol = _sourceFile.FieldMappings
                        .Single(x => x.IncomingField.Equals(column, StringComparison.CurrentCultureIgnoreCase))
                        .IncomingField.ToUpper();
                    if (!string.IsNullOrWhiteSpace(incomingCol) && !_dataIv.HeadersTransformed.ContainsKey(incomingCol))
                    {
                        _dataIv.HeadersTransformed.Add(incomingCol, (_dataIv.HeadersTransformed.Count + 1).ToString());
                    }
                    else
                    {
                        if (!_dataIv.HeadersTransformed.ContainsKey(column))
                        {
                            _dataIv.HeadersTransformed.Add(column, (_dataIv.HeadersTransformed.Count + 1).ToString());
                        }
                    }
                }
                else if (allMultiMappings.Exists(x => x.MAFField.Equals(column, StringComparison.CurrentCultureIgnoreCase)))
                {
                    var incomingCol = allMultiMappings
                        .Single(x => x.MAFField.Equals(column, StringComparison.CurrentCultureIgnoreCase))
                        .MAFField.ToUpper();
                    if (!string.IsNullOrWhiteSpace(incomingCol) && !_dataIv.HeadersTransformed.ContainsKey(incomingCol))
                    {
                        _dataIv.HeadersTransformed.Add(incomingCol, (_dataIv.HeadersTransformed.Count + 1).ToString());
                    }
                    else if (!_dataIv.HeadersTransformed.ContainsKey(column))
                    {
                        _dataIv.HeadersTransformed.Add(column, (_dataIv.HeadersTransformed.Count + 1).ToString());
                    }
                }
            }

            if (!_dataIv.HeadersTransformed.ContainsKey(FileValidationTransformationHelper.OriginalImportRowFieldName))
            {
                _dataIv.HeadersTransformed.Add(FileValidationTransformationHelper.OriginalImportRowFieldName, (_dataIv.HeadersTransformed.Count + 1).ToString());
            }
        }

        private void DiscoverMappingFields(List<FieldMultiMap> allMultiMappings, StringDictionary multiMapToFieldMap)
        {
            foreach (var fm in _sourceFile.FieldMappings)
            {
                if (!fm.IsNonFileColumn)
                {
                    continue;
                }

                var column = fm.MAFField.Equals("PubCode", StringComparison.CurrentCultureIgnoreCase) ? fm.MAFField : fm.IncomingField;

                if (_dataIv.HeadersOriginal.ContainsKey(column))
                {
                    continue;
                }

                _dataIv.HeadersOriginal.Add(column, (_dataIv.HeadersOriginal.Count + 1).ToString());
                foreach (var key in _dataIv.DataOriginal.Keys)
                {
                    var myRow = _dataIv.DataOriginal[key];
                    if (!myRow.ContainsKey(column))
                    {
                        myRow.Add(column, string.Empty);
                    }
                }
            }

            foreach (var fm in _sourceFile.FieldMappings)
            {
                if (fm.FieldMultiMappings.Count == 0)
                {
                    continue;
                }

                foreach (var fmm in fm.FieldMultiMappings)
                {
                    allMultiMappings.Add(fmm);
                    var column = fm.MAFField.Equals(FileValidationTransformationHelper.PubCodeFieldName, StringComparison.CurrentCultureIgnoreCase) ? fm.MAFField : fm.IncomingField;

                    if (_dataIv.HeadersOriginal.ContainsKey(column))
                    {
                        continue;
                    }

                    _dataIv.HeadersOriginal.Add(column, (_dataIv.HeadersOriginal.Count + 1).ToString());
                    if (!multiMapToFieldMap.ContainsKey(fmm.MAFField))
                    {
                        multiMapToFieldMap.Add(fmm.MAFField, fm.IncomingField);
                    }

                    foreach (var key in _dataIv.DataOriginal.Keys)
                    {
                        var myRow = _dataIv.DataOriginal[key];
                        if (!myRow.ContainsKey(column))
                        {
                            myRow.Add(column, string.Empty);
                        }
                    }
                }
            }

            if (!_dataIv.HeadersOriginal.ContainsValue(FileValidationTransformationHelper.OriginalImportRowFieldName))
            {
                _dataIv.HeadersOriginal.Add(FileValidationTransformationHelper.OriginalImportRowFieldName, (_dataIv.HeadersOriginal.Count + 1).ToString());
            }

            var origRowCount = 1;
            foreach (var key in _dataIv.DataOriginal.Keys)
            {
                var myRow = _dataIv.DataOriginal[key];
                if (!myRow.ContainsKey(FileValidationTransformationHelper.OriginalImportRowFieldName))
                {
                    myRow.Add(FileValidationTransformationHelper.OriginalImportRowFieldName, origRowCount.ToString());
                }

                origRowCount++;
            }
        }

        private void ProcessMultimappingsSetValueFromFieldMapping(StringDictionary multiMapToFieldMap)
        {
            if (multiMapToFieldMap.Count == 0)
            {
                return;
            }

            foreach (var key in _dataIv.DataTransformed.Keys)
            {
                var myRow = _dataIv.DataTransformed[key];
                foreach (DictionaryEntry kvp in multiMapToFieldMap)
                {
                    if (myRow.ContainsKey(kvp.Key.ToString()))
                    {
                        myRow[kvp.Key.ToString()] = myRow[kvp.Value.ToString()];
                    }
                }
            }
        }

        private void ProcessDataMapTransforms(TransformationDefinitionsGrouping transformConfigs, FieldMapping fmPubCode)
        {
            if (transformConfigs.DataMapTrans.Count == 0 ||
                BusinessEnums.GetUADFeature(_serviceFeature.SFName) == BusinessEnums.UADFeatures.Data_Compare)
            {
                return;
            }

            foreach (var key in _dataIv.DataTransformed.Keys)
            {
                var myRow = _dataIv.DataTransformed[key];
                try
                {
                    if (string.IsNullOrWhiteSpace(myRow[fmPubCode.IncomingField]) ||
                        !_clientPubCodes.ContainsValue(myRow[fmPubCode.IncomingField]))
                    {
                        continue;
                    }

                    var pubCodeId = _clientPubCodes.Single(x => x.Value.Equals(myRow[fmPubCode.IncomingField])).Key;
                    if (pubCodeId > 0)
                    {
                        ApplyDataMapsForRow(transformConfigs, pubCodeId, myRow);
                    }
                }
                catch (Exception ex)
                {
                    _logException(ex, _dataIv.SourceFileId, _dataIv.ProcessCode, _dataIv.ClientId, key, myRow);
                }
            }

            transformConfigs.DataMapTrans.Clear();
            transformConfigs.DataMapTrans.TrimExcess();
        }

        private void ApplyDataMapsForRow(
            TransformationDefinitionsGrouping transformConfigs,
            int pubCodeId,
            StringDictionary myRow)
        {
            foreach (var transformation in transformConfigs.GetDataMapTransformsForPubCode(pubCodeId))
            {
                if (!transformation.LastStepDataMap)
                {
                    continue;
                }

                var dmRan = false;
                foreach (var tdmx in transformConfigs.GetDataMaps(transformation.TransformationID, pubCodeId))
                {
                    if (dmRan)
                    {
                        continue;
                    }

                    var tranFieldMap = transformConfigs.GetTransformation(tdmx.TransformationID)?.FieldMap.Where(x => x.SourceFileID == _dataIv.SourceFileId).ToList();
                    if (tranFieldMap?.Count > 0)
                    {
                        dmRan = FileValidationTransformationHelper.ProcessDataMappingForField(_sourceFile, tranFieldMap, myRow, tdmx);
                    }
                }
            }
        }

        private void ProcessJoinTransforms(TransformationDefinitionsGrouping transformConfigs, FieldMapping fmPubCode)
        {
            if (transformConfigs.JoinTrans.Count == 0 ||
            BusinessEnums.GetUADFeature(_serviceFeature.SFName) == BusinessEnums.UADFeatures.Data_Compare)
            {
                return;
            }

            foreach (var key in _dataIv.DataTransformed.Keys)
            {
                var myRow = _dataIv.DataTransformed[key];
                try
                {
                    var pubCodeId = -1;
                    if (!string.IsNullOrWhiteSpace(myRow[fmPubCode.IncomingField].Trim()) &&
                        _clientPubCodes.ContainsValue(myRow[fmPubCode.IncomingField].Trim()))
                    {
                        pubCodeId = _clientPubCodes.Single(x =>x.Value.Equals(myRow[fmPubCode.IncomingField].ToString().Trim())).Key;
                    }

                    if (pubCodeId > 0)
                    {
                        foreach (var transformation in transformConfigs.GetJoinTransformsForPubCode(pubCodeId))
                        {
                            foreach (var tjx in transformConfigs.GetTransJoinMap(transformation.TransformationID))
                            {
                                var tranx = transformConfigs.GetTransformation(tjx.TransformationID);
                                if (tranx == null)
                                {
                                    continue;
                                }

                                ProcessTransformationJoin(tranx, tjx, myRow);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logException(ex, _dataIv.SourceFileId, _dataIv.ProcessCode, _dataIv.ClientId, key, myRow);
                }
            }

            //Clearing lists after their last use
            transformConfigs.JoinTrans.Clear();
            transformConfigs.JoinTrans.TrimExcess();
        }

        private void ProcessTransformationJoin(FrameworkUAS.Entity.Transformation tranx, FrameworkUAS.Entity.TransformJoin tjx, StringDictionary myRow)
        {
            foreach (var tfm in tranx.FieldMap.Where(y => y.SourceFileID == _dataIv.SourceFileId))
            {
                var fm = _sourceFile.FieldMappings.SingleOrDefault(x => x.FieldMappingID == tfm.FieldMappingID && x.SourceFileID == _dataIv.SourceFileId);
                var splitter = Convert.ToChar(tjx.Delimiter);

                var columns = tjx.ColumnsToJoin.Split(splitter).ToList();

                var newValue = new StringBuilder();
                foreach (var s in columns)
                {
                    var col = s.Trim();
                    var colConversion = _sourceFile.FieldMappings.SingleOrDefault(x => x.SourceFileID == _dataIv.SourceFileId && x.IncomingField.Equals(col, StringComparison.CurrentCultureIgnoreCase));
                    if (colConversion != null)
                    {
                        var fieldName = colConversion.IncomingField.ToUpper();
                        if (myRow.ContainsKey(fieldName) && !string.IsNullOrWhiteSpace(myRow[fieldName]))
                        {
                            newValue.Append($"{myRow[fieldName].Trim()}{splitter}");
                        }
                    }
                }

                if (fm != null)
                {
                    if (myRow.ContainsKey(fm.IncomingField.ToUpper()))
                    {
                        myRow[fm.IncomingField.ToUpper()] = newValue.ToString().TrimEnd(splitter).Trim();
                    }
                }
            }
        }

        private void ProcessTransformationsByPubCode(TransformationDefinitionsGrouping transformConfigs, FieldMapping fmPubCode)
        {
            if (BusinessEnums.GetUADFeature(_serviceFeature.SFName) == BusinessEnums.UADFeatures.Data_Compare)
            {
                return;
            }

            if (transformConfigs.DataMapTrans.Count > 0 ||
                transformConfigs.AssignTrans.Count > 0 ||
                (transformConfigs.SplitTrans.Count > 0 &&
                BusinessEnums.GetUADFeature(_serviceFeature.SFName) != BusinessEnums.UADFeatures.Special_Split))
            {
                var distinctPubCodes = GetDistinctPubCodes(fmPubCode);
                var deleteKeys = new List<int>();
                var addRows = new Dictionary<int, StringDictionary>();
                foreach (var kvp in distinctPubCodes)
                {
                    var dmList = transformConfigs.GetDataMapTransformsForPubCode(kvp.Key);
                    var splitList = transformConfigs.GetSplitTransformsForPubCode(kvp.Key);
                    var assignList = transformConfigs.GetAssignTransformsForPubCode(kvp.Key);

                    if (!dmList.Any() && !splitList.Any() && !assignList.Any())
                    {
                        continue;
                    }

                    var sdList = _dataIv.DataTransformed.Values.Where(x => x[fmPubCode.IncomingField].Equals(kvp.Value, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    foreach (var myRow in sdList)
                    {
                        ProcessRowTransformationsForPubCode(transformConfigs, myRow, assignList, dmList, kvp, splitList, deleteKeys, addRows);
                    }
                }

                ChangeKeys(addRows, deleteKeys);
                transformConfigs.SplitTrans.Clear();
                transformConfigs.SplitTrans.TrimExcess();
                transformConfigs.AssignTrans.Clear();
                transformConfigs.AssignTrans.TrimExcess();
                transformConfigs.TransSplit.Clear();
                transformConfigs.TransSplit.TrimExcess();
            }
        }

        private void ProcessRowTransformationsForPubCode(
            TransformationDefinitionsGrouping transformConfigs,
            StringDictionary myRow,
            IList<FrameworkUAS.Entity.Transformation> assignList,
            IList<FrameworkUAS.Entity.Transformation> dmList,
            KeyValuePair<int, string> kvp,
            IList<FrameworkUAS.Entity.Transformation> splitList,
            List<int> deleteKeys,
            Dictionary<int, StringDictionary> addRows)
        {
            var key = _dataIv.DataTransformed.Single(x => x.Value == myRow).Key;
            try
            {
                if (assignList.Any())
                {
                    FileValidationTransformationHelper.ProcessPubCodeAssignTransformationsForRow(_sourceFile, _dataIv, transformConfigs, myRow, assignList);
                }

                if (dmList.Any())
                {
                    FileValidationTransformationHelper.ProcessPubCodeDataMapTransformationsForRow(_sourceFile, _dataIv, transformConfigs, myRow, kvp, dmList);
                }

                if (splitList.Any() &&
                    BusinessEnums.GetUADFeature(_serviceFeature.SFName) != BusinessEnums.UADFeatures.Special_Split)
                {
                    FileValidationTransformationHelper.ProcessPubCodeSpecialSplitTransformationsForRow(_sourceFile, _dataIv, transformConfigs, myRow, splitList, deleteKeys, addRows, key);
                }
            }
            catch (Exception ex)
            {
                _logException(
                    ex,
                    _dataIv.SourceFileId,
                    _dataIv.ProcessCode,
                    _dataIv.ClientId,
                    key,
                    myRow);
            }
        }

        private void ChangeKeys(Dictionary<int, StringDictionary> addRows, List<int> deleteKeys)
        {
            foreach (var k in addRows)
            {
                _dataIv.DataTransformed.Add(k.Key, k.Value);
            }

            foreach (var k in deleteKeys)
            {
                _dataIv.DataTransformed.Remove(k);
            }

            var newKey = 1;
            var keysCopy = new List<int>(_dataIv.DataTransformed.Keys);
            foreach (var key in keysCopy)
            {
                _dataIv.DataTransformed.ChangeKey(key, newKey++);
            }
        }

        private Dictionary<int, string> GetDistinctPubCodes(FieldMapping fmPubCode)
        {
            var distinctPubCodes = new Dictionary<int, string>();
            foreach (var key in _dataIv.DataTransformed.Keys)
            {
                var myRow = _dataIv.DataTransformed[key];
                var pubCodeID = -1;
                if (!string.IsNullOrWhiteSpace(myRow[fmPubCode.IncomingField]) &&
                    _clientPubCodes.ContainsValue(myRow[fmPubCode.IncomingField]))
                {
                    pubCodeID = _clientPubCodes.Single(x =>
                            x.Value.Equals(myRow[fmPubCode.IncomingField].ToString()))
                        .Key;
                    var pub = new KeyValuePair<int, string>(pubCodeID, myRow[fmPubCode.IncomingField]);
                    if (!distinctPubCodes.Contains(pub))
                    {
                        distinctPubCodes.Add(pubCodeID, myRow[fmPubCode.IncomingField]);
                    }
                }
            }

            return distinctPubCodes;
        }

        private TransformationDefinitionsGrouping PopulateTransformationDefinitionGroups()
        {
            var tranWorker = new Transformation();
            var allTrans = new HashSet<FrameworkUAS.Entity.Transformation>(
                tranWorker.Select(_client.ClientID, _sourceFile.SourceFileID, true).Where(x => x.IsActive));

            foreach (var transformation in allTrans)
            {
                transformation.FieldMap =
                    new HashSet<TransformationFieldMap>(
                        transformation.FieldMap.Where(x => x.SourceFileID == _dataIv.SourceFileId).ToList());
            }

            var transTypes = _codeWorker.Select(Enums.CodeType.Transformation);
            var dataMappingId = transTypes.Single(x =>
                    x.CodeName.Equals(Enums.TransformationTypes.Data_Mapping.ToString().Replace("_", " "))).CodeId;
            var joinColumnsId = transTypes.Single(x =>
                    x.CodeName.Equals(Enums.TransformationTypes.Join_Columns.ToString().Replace("_", " "))).CodeId;
            var splitIntoRowsId = transTypes.Single(x =>
                    x.CodeName.Equals(Enums.TransformationTypes.Split_Into_Rows.ToString().Replace("_", " "))).CodeId;
            var assignValueId = transTypes.Single(x =>
                    x.CodeName.Equals(Enums.TransformationTypes.Assign_Value.ToString().Replace("_", " "))).CodeId;
            var splitTransformId = transTypes.Single(x =>
                    x.CodeName.Equals(Enums.TransformationTypes.Split_Transform.ToString().Replace("_", " "))).CodeId;

            var taResp = new TransformAssign().SelectSourceFileID(_sourceFile.SourceFileID);
            var tdmWorker = new TransformDataMap();
            var tdmResp = tdmWorker.SelectSourceFileID(_sourceFile.SourceFileID);
            var tdmEveryResp = tdmWorker.Select();

            var tsWorker = new TransformSplit();
            var tsResp = tsWorker.SelectSourceFileID(_sourceFile.SourceFileID);
            var tsEveryResp = tsWorker.Select();
            var tjResp = new TransformJoin().SelectSourceFileID(_sourceFile.SourceFileID);
            var tstResp = new TransformSplitTrans().SelectSourceFileID(_sourceFile.SourceFileID);

            return new TransformationDefinitionsGrouping()
            {
                AllTrans = allTrans,
                DataMapTrans = new HashSet<FrameworkUAS.Entity.Transformation>(
                    allTrans.Where(x => x.TransformationTypeID == dataMappingId).ToList()),
                JoinTrans = new HashSet<FrameworkUAS.Entity.Transformation>(
                    allTrans.Where(x => x.TransformationTypeID == joinColumnsId).ToList()),
                SplitTrans = new HashSet<FrameworkUAS.Entity.Transformation>(
                    allTrans.Where(x => x.TransformationTypeID == splitIntoRowsId).ToList()),
                AssignTrans = new HashSet<FrameworkUAS.Entity.Transformation>(
                    allTrans.Where(x => x.TransformationTypeID == assignValueId).ToList()),
                TransSplit = new HashSet<FrameworkUAS.Entity.Transformation>(
                    allTrans.Where(x => x.TransformationTypeID == splitTransformId).ToList()),
                AllTransAssign = new HashSet<FrameworkUAS.Entity.TransformAssign>((taResp??Enumerable.Empty<FrameworkUAS.Entity.TransformAssign>()).Where(x=> x.IsActive)),
                AllTransDataMap = new HashSet<FrameworkUAS.Entity.TransformDataMap>((tdmResp ?? Enumerable.Empty<FrameworkUAS.Entity.TransformDataMap>()).Where(x => x.IsActive)),
                AllTransSplitMap = new HashSet<FrameworkUAS.Entity.TransformSplit>((tsResp ?? Enumerable.Empty<FrameworkUAS.Entity.TransformSplit>()).Where(x => x.IsActive)),
                AllTransJoinMap = new HashSet<FrameworkUAS.Entity.TransformJoin>((tjResp ?? Enumerable.Empty<FrameworkUAS.Entity.TransformJoin>()).Where(x => x.IsActive)),
                AllSplitTrans = new HashSet<FrameworkUAS.Entity.TransformSplitTrans>((tstResp ?? Enumerable.Empty<FrameworkUAS.Entity.TransformSplitTrans>()).Where(x => x.IsActive)),
                EveryTransDataMap = new HashSet<FrameworkUAS.Entity.TransformDataMap>((tdmEveryResp ?? Enumerable.Empty<FrameworkUAS.Entity.TransformDataMap>()).Where(x => x.IsActive)),
                EveryTransSplitMap = new HashSet<FrameworkUAS.Entity.TransformSplit>((tsEveryResp ?? Enumerable.Empty<FrameworkUAS.Entity.TransformSplit>()).Where(x => x.IsActive)),
            };
        }
    }
}

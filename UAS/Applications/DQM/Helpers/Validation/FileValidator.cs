using Core.ADMS;
using Core_AMS.Utilities;
using FrameworkServices;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using FrameworkUAD.BusinessLogic.Helpers;
using FrameworkUAD.BusinessLogic.Transformations;
using BusinessEnums = KMPlatform.BusinessLogic.Enums;
using Enums = FrameworkUAD_Lookup.Enums;

namespace DQM.Helpers.Validation
{
    public partial class FileValidator : FileValidatorBase
    {
        private const string CompanySurveyDimGroup = "Scranton_Company_Survey";
        private const string DomainsSurveyDimGroup = "Scranton_Domains_Survey";
        private const string PubCodeFieldName = "PubCode";
        private const string PBG300Code = "PBG300";
        private const string Value = "DQM.Helpers.Validation.FileValidator.ProcessFileAsObject - Unhandled Exception";
        private const string BDCG300Code = "BDCG300";
        #region class variables
        public System.Windows.Controls.TextBox overallProgress = new System.Windows.Controls.TextBox();
        public System.Windows.Controls.TextBox currentProgress = new System.Windows.Controls.TextBox();
        public System.Windows.Controls.TextBox currentOperation = new System.Windows.Controls.TextBox();        

        Dictionary<int, string> clientPubCodes;
        KMPlatform.Entity.Service service;                
        KMPlatform.Entity.Client client;
        FileInfo checkFile;
        FrameworkUAD.Object.ValidationResult validationResult;
        string processCode;
        List<FrameworkUAD.Object.ImportErrorSummary> listIES;        
        FrameworkUAD_Lookup.Entity.Code dbFileType;
        Guid accessKey;

        bool isValidFileType;
        bool isFileSchemaValid;
        private bool IsKnownCustomerFileName;

        FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeWorker;
        #endregion

        // Delegates to enable async calls for setting controls properties
        private delegate void SetTextCallback(System.Windows.Controls.TextBox control, string text);

        // Thread safe updating of control's text property
        private void SetText(System.Windows.Controls.TextBox control, string prog)
        {
            if (control.Dispatcher.CheckAccess())
            {
                control.Text = prog;
                //control.T
            }
            else
            {
                SetTextCallback d = new SetTextCallback(SetText);
                control.Dispatcher.Invoke(d, new object[] { control, prog });
            }
        }


        private void ValidateData()
        {
            List<SubscriberTransformed> listSubscriberTransformed;
            List<SubscriberInvalid> listSubscriberInvalid;
            List<SubscriberTransformed> listValidSt;

            var isDataCompare = BusinessEnums.GetUADFeature(serviceFeature.SFName) == BusinessEnums.UADFeatures.Data_Compare;
            var listSubscriberOriginal = sourceFile.IsDQMReady 
                ? InsertOriginalSubscribers()
                : new List<SubscriberOriginal>();
            var subOriginalDictionary = listSubscriberOriginal.ToDictionary(so => so.ImportRowNumber, so => so.SORecordIdentifier);

            ValidateDataRow(listSubscriberOriginal, isDataCompare, subOriginalDictionary, out listSubscriberTransformed, out listSubscriberInvalid, out listValidSt);
            var dedupedTransList = DeDupeTransformed(listValidSt);
            var dedupedInvalidList = DeDupeInvalid(listSubscriberInvalid);

            if (client.FtpFolder.Equals("Scranton", StringComparison.OrdinalIgnoreCase))
            {
                dedupedTransList = Scranton_CompanySurvey(dedupedTransList, clientPubCodes, client.ClientID);
            }

            if (sourceFile.IsDQMReady)
            {
                SaveTransformedAndInvalid(isDataCompare, dedupedTransList, listSubscriberTransformed, dedupedInvalidList, listSubscriberInvalid);
            }

            dataIV.ImportedRowCount = dedupedTransList.Count;
        }

        private void SaveTransformedAndInvalid(
            bool isDataCompare,
            List<SubscriberTransformed> dedupedTransList,
            List<SubscriberTransformed> listSubscriberTransformed,
            List<SubscriberInvalid> dedupedInvalidList,
            List<SubscriberInvalid> listSubscriberInvalid)
        {
            if (dedupedTransList == null)
            {
                throw new ArgumentNullException(nameof(dedupedTransList));
            }

            if (listSubscriberTransformed == null)
            {
                throw new ArgumentNullException(nameof(listSubscriberTransformed));
            }

            if (dedupedInvalidList == null)
            {
                throw new ArgumentNullException(nameof(dedupedInvalidList));
            }

            if (listSubscriberInvalid == null)
            {
                throw new ArgumentNullException(nameof(listSubscriberInvalid));
            }

            if (dedupedTransList.Count > 0)
            {
                var resp = ServiceClient.UAD_SubscriberTransformedClient()
                    .Proxy
                    .SaveBulkSqlInsert(accessKey, dedupedTransList, client.ClientConnections, isDataCompare);
                if (resp.Status != FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success || !resp.Result)
                {
                    LogError("Transformed", "DemoTransformed");
                    listSubscriberTransformed.Clear();
                }
            }
            else
            {
                ErrorMessages.Add($"NO VALID Transformed data to insert: {DateTime.Now.TimeOfDay}");
            }

            if (dedupedInvalidList.Any())
            {
                var resp = ServiceClient.UAD_SubscriberInvalidClient()
                    .Proxy
                    .SaveBulkSqlInsert(accessKey, dedupedInvalidList, client.ClientConnections);
                if (resp.Status != FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success || !resp.Result)
                {
                    LogError("Invalid", "DemoInvalid");
                    listSubscriberInvalid.Clear();
                }
            }
        }

        private void ValidateDataRow(
            List<SubscriberOriginal> listSubscriberOriginal,
            bool isDataCompare,
            Dictionary<int, Guid> subOriginalDictionary,
            out List<SubscriberTransformed> listSubscriberTransformed,
            out List<SubscriberInvalid> listSubscriberInvalid,
            out List<SubscriberTransformed> listValidSt)
        {
            if (listSubscriberOriginal == null)
            {
                throw new ArgumentNullException(nameof(listSubscriberOriginal));
            }

            if (subOriginalDictionary == null)
            {
                throw new ArgumentNullException(nameof(subOriginalDictionary));
            }

            listSubscriberInvalid = new List<SubscriberInvalid>();
            listValidSt = new List<SubscriberTransformed>();
            listSubscriberTransformed = new List<SubscriberTransformed>();
            if (!listSubscriberOriginal.Any())
            {
                return;
            }

            List<Transformation> splitTrans;
            List<TransformationFieldMap> splitTranFieldMappings;
            List<AdHocDimensionGroup> ahdGroups;
            List<TransformSplit> allTransformSplit;
            if (!LoadMetadata(out allTransformSplit, out ahdGroups, out splitTranFieldMappings, out splitTrans))
            {
                return;
            }

            CreateSubscribersTransformed(isDataCompare, subOriginalDictionary, listSubscriberTransformed, allTransformSplit, ahdGroups, splitTranFieldMappings, splitTrans);
            if (isDataCompare)
            {
                listValidSt.AddRange(listSubscriberTransformed);
            }
            else
            {
                List<SubscriberTransformed> listPubCodeValid;
                CheckPubCode(out listPubCodeValid, listSubscriberTransformed);
                CheckProfile(ref listValidSt, listPubCodeValid);

                var listInvalidValidSt = new List<SubscriberTransformed>();
                if (dbFileType != null &&
                    Enums.GetDatabaseFileType(dbFileType.CodeName) == Enums.FileTypes.Web_Forms &&
                    listPubCodeValid.Any())
                {
                    //File Should can contain multiple pubcodes. This is checked before calling the ProcessFileAsObject Method
                    if (listPubCodeValid.Count > 0)
                    {
                        CheckWebFormsCodesheetFilled(listValidSt, listPubCodeValid, listInvalidValidSt);
                    }
                }

                var validList = listValidSt;
                listInvalidValidSt = listSubscriberTransformed
                    .Where(st => validList.All(x => x.STRecordIdentifier != st.STRecordIdentifier))
                    .ToList();
                listSubscriberInvalid = ConvertTransformedToInvalid(listInvalidValidSt);
            }
        }

        private void CheckWebFormsCodesheetFilled(List<SubscriberTransformed> listValidSt, List<SubscriberTransformed> listPubCodeValid, List<SubscriberTransformed> listInvalidValidSt)
        {
            var requiredResponse = new List<string>();
            foreach (var pub in listPubCodeValid.Select(x => x.PubCode).Distinct())
            {
                if (!clientPubCodes.ContainsValue(pub.ToUpper()))
                {
                    continue;
                }

                var pubId = clientPubCodes.FirstOrDefault(x => x.Value.Equals(pub, StringComparison.CurrentCultureIgnoreCase)).Key;
                if (pubId <= 0)
                {
                    continue;
                }

                var rgList = ServiceClient.UAD_ResponseGroupClient()
                    .Proxy
                    .Select(accessKey, client.ClientConnections, pubId)
                    .Result;
                if (rgList == null || !rgList.Any())
                {
                    continue;
                }

                requiredResponse.AddRange(rgList.Where(x => x.IsRequired == true).Select(x => x.ResponseGroupName.ToLower()));
                foreach (var subscriberTransformed in listValidSt.Where(x => x.PubCode.Equals(pub, StringComparison.CurrentCultureIgnoreCase)))
                {
                    CheckDemographicsListFilledIn(listInvalidValidSt, subscriberTransformed, requiredResponse);
                }

                listValidSt.RemoveAll(x => listInvalidValidSt.Contains(x));
            }
        }

        private void CheckDemographicsListFilledIn(List<SubscriberTransformed> listInvalidValidSt, SubscriberTransformed st, List<string> requiredResponse)
        {
            var isSubTranValid = true;
            var distinctMAFFields = st.DemographicTransformedList
                .Select(x => x.MAFField.ToLower())
                .Distinct()
                .ToList();
            foreach (var val in requiredResponse)
            {
                if (distinctMAFFields.Contains(val))
                {
                    continue;
                }

                isSubTranValid = false;
                var myRow = dataIV.DataTransformed[st.ImportRowNumber];
                var msg = $"WEB FORM: Missing/Blank Required Codesheet: {val} @ Row {st.ImportRowNumber}";
                StringFunctions.WriteLineRepeater(msg, ConsoleColor.Red);
                ValidationError(msg, st.ImportRowNumber, myRow);
                ErrorMessages.Add(msg);
            }

            if (isSubTranValid)
            {
                foreach (var sdt in st.DemographicTransformedList.Where(x =>
                    requiredResponse.Contains(x.MAFField.ToLower()) &&
                    string.IsNullOrEmpty(x.Value.Trim())))
                {
                    isSubTranValid = false;
                    var myRow = dataIV.DataTransformed[st.ImportRowNumber];
                    var msg =
                        $"WEB FORM: Blank Required Codesheet: {sdt.MAFField} value is null/empty @ Row {st.ImportRowNumber}";
                    StringFunctions.WriteLineRepeater(msg, ConsoleColor.Red);
                    ValidationError(msg, st.ImportRowNumber, myRow);
                    ErrorMessages.Add(msg);
                }
            }

            if (!isSubTranValid)
            {
                listInvalidValidSt.Add(st);
            }
        }

        private void CheckProfile(ref List<SubscriberTransformed> listValidSt, List<SubscriberTransformed> listPubCodeValid)
        {
            var listEmailValid = new List<SubscriberTransformed>();
            listEmailValid.AddRange(listPubCodeValid.Where(x =>!string.IsNullOrEmpty(x.Email) && x.Email.Contains("@")));

            listPubCodeValid.RemoveAll(x => listEmailValid.Contains(x));
            var listQualProfileValid = HasQualifiedProfile(listPubCodeValid);
            listValidSt.AddRange(listEmailValid);
            listValidSt.AddRange(listQualProfileValid);
            listValidSt = listValidSt.Distinct().ToList();
        }

        private void CheckPubCode(out List<SubscriberTransformed> listPubCodeValid, List<SubscriberTransformed> listSubscriberTransformed)
        {
            listPubCodeValid = listSubscriberTransformed.Where(x => !string.IsNullOrEmpty(x.PubCode)).ToList();
            var listNoPubCode = listSubscriberTransformed.Where(x => string.IsNullOrEmpty(x.PubCode)).ToList();
            foreach (var subscriber in listNoPubCode)
            {
                var myRow = dataIV.DataTransformed[subscriber.ImportRowNumber];
                var msg = $"Blank PUBCODE @ Row {subscriber.ImportRowNumber}";
                StringFunctions.WriteLineRepeater(msg, ConsoleColor.Red);
                ValidationError(msg, subscriber.ImportRowNumber, myRow);
                ErrorMessages.Add(msg);
            }

            //not exist PubCode
            var notExistPubCode = 
                (from t in listSubscriberTransformed
                 where clientPubCodes.All(x =>
                 string.Compare(x.Value, t.PubCode, StringComparison.OrdinalIgnoreCase) != 0)
                 select t)
                .ToList();
            foreach (var subscriber in notExistPubCode)
            {
                var myRow = dataIV.DataTransformed[subscriber.ImportRowNumber];
                var msg = $"PUBCODE not found in UAD @ Row {subscriber.ImportRowNumber}";
                StringFunctions.WriteLineRepeater(msg, ConsoleColor.Red);
                ValidationError(msg, subscriber.ImportRowNumber, myRow);
                ErrorMessages.Add(msg);
            }
        }

        private void CreateSubscribersTransformed(
            bool isDataCompare,
            Dictionary<int, Guid> subOriginalDictionary,
            List<SubscriberTransformed> listSubscriberTransformed,
            List<TransformSplit> allTransformSplit,
            List<AdHocDimensionGroup> ahdGroups,
            List<TransformationFieldMap> splitTranFieldMappings,
            List<Transformation> splitTrans)
        {
            var fieldMapping = sourceFile.FieldMappings.SingleOrDefault(x =>
                x.MAFField.Equals(PubCodeFieldName, StringComparison.CurrentCultureIgnoreCase));
            foreach (var key in dataIV.DataTransformed.Keys)
            {
                var myRow = dataIV.DataTransformed[key];
                var msg = $"Creating Transformed Subscriber: {key} of {dataIV.DataTransformed.Count}";
                StringFunctions.WriteLineRepeater(msg, ConsoleColor.Green);
                var pubCodeId = -1;
                if (!isDataCompare)
                {
                    if (fieldMapping != null &&
                        myRow[fieldMapping.IncomingField] != null &&
                        !string.IsNullOrWhiteSpace(myRow[fieldMapping.IncomingField]))
                    {
                        var pubCodeValue = myRow[fieldMapping.IncomingField];
                        int.TryParse(clientPubCodes
                                .SingleOrDefault(x => x.Value.Equals(pubCodeValue.ToUpper()))
                                .Key.ToString(),
                            out pubCodeId);
                    }
                }

                var originalRowNumber = 0;
                if (dataIV.TransformedRowToOriginalRowMap.ContainsKey(key))
                {
                    originalRowNumber = dataIV.TransformedRowToOriginalRowMap[key];
                }

                try
                {
                    listSubscriberTransformed.Add(
                        CreateTransformedSubscriber(allTransformSplit,
                            myRow,
                            pubCodeId,
                            originalRowNumber,
                            key,
                            subOriginalDictionary,
                            ahdGroups,
                            splitTranFieldMappings,
                            splitTrans)
                    );
                }
                catch (Exception ex)
                {
                    var sbDetail = new StringBuilder();
                    sbDetail.AppendLine(
                        "DQM.Helpers.Validation.FileValidator.ProcessFileAsObject - CreateTransformedSubscriber");
                    sbDetail.AppendLine(Environment.NewLine);
                    sbDetail.AppendLine(StringFunctions.FormatException(ex));
                    //BUG: Should we put it somewhere isn't it?
                }
            }
        }

        private bool LoadMetadata(
            out List<TransformSplit> allTransformSplit,
            out List<AdHocDimensionGroup> ahdGroups,
            out List<TransformationFieldMap> splitTranFieldMappings,
            out List<Transformation> splitTrans)
        {
            allTransformSplit = null;
            ahdGroups = null;
            splitTranFieldMappings = null;
            splitTrans = null;

            var respTrans = ServiceClient.UAS_TransformationClient()
                .Proxy
                .Select(
                    accessKey,
                    client.ClientID,
                    sourceFile.SourceFileID,
                    true);
            if (respTrans.Result == null ||
                respTrans.Status != Enums.ServiceResponseStatusTypes.Success)
            {
                return false;
            }

            var allTransformations = respTrans.Result.Where(x => x.IsActive).ToList();

            var respTransformSplit = ServiceClient.UAS_TransformSplitClient()
                                        .Proxy
                                        .SelectForSourceFile(accessKey, sourceFile.SourceFileID);
            allTransformSplit = (respTransformSplit.Result != null &&
                                 respTransformSplit.Status == Enums.ServiceResponseStatusTypes.Success)
                ? respTransformSplit.Result.Where(x => x.IsActive).ToList()
                : new List<TransformSplit>();

            var ahdgResp = ServiceClient.UAS_AdHocDimensionGroupClient()
                .Proxy
                .Select(accessKey, client.ClientID);
            ahdGroups = (ahdgResp.Result != null &&
                         ahdgResp.Status == Enums.ServiceResponseStatusTypes.Success)
                ? ahdgResp.Result.Where(x => x.IsActive).OrderBy(y => y.OrderOfOperation).ToList()
                : new List<AdHocDimensionGroup>();

            var transTypes = codeWorker
                .Proxy
                .Select(accessKey, Enums.CodeType.Transformation)
                .Result;
            var splitTranFieldMappingsData = new List<TransformationFieldMap>();

            splitTrans = new List<Transformation>();
            if (transTypes != null)
            {
                var splitIntoRowsId = transTypes.Single(x =>
                        x.CodeName.Equals(Enums.TransformationTypes.Split_Into_Rows
                            .ToString()
                            .Replace("_", " ")))
                    .CodeId;
                if (allTransformations.Exists(x => x.TransformationTypeID == splitIntoRowsId))
                {
                    splitTrans = allTransformations
                        .Where(x => x.TransformationTypeID == splitIntoRowsId && x.IsActive)
                        .ToList();
                }

                splitTranFieldMappingsData.AddRange(splitTrans.SelectMany(t => t.FieldMap)
                    .Where(x =>
                        x.SourceFileID == sourceFile.SourceFileID &&
                        splitTranFieldMappingsData.All(stfm =>
                            stfm.TransformationFieldMapID != x.TransformationFieldMapID)));
                splitTranFieldMappings = splitTranFieldMappingsData;
            }

            return true;
        }

        private void LogError(params string[] tables)
        {
            if (tables == null)
            {
                throw new ArgumentNullException(nameof(tables));
            }

            if (!tables.Any())
            {
                throw new ArgumentException(nameof(tables));
            }

            var msg = new StringBuilder();
            msg.AppendLine($"ERROR: Insert to {tables.First()} Bulk Data Insert {DateTime.Now.TimeOfDay}");
            msg.AppendLine($"An unexplained error occurred while inserting records into {string.Join(" or ", tables)} tables.<br/>");
            msg.AppendLine("The details of this error have been logged in the FileLog table.<br/>");
            msg.AppendLine("Please run this query.<br/>");
            msg.AppendLine($"Select * From FileLog With(NoLock) Where SourceFileID = {sourceFile.SourceFileID} AND ProcessCode = '{processCode}' ORDER BY LogDate, LogTime; GO");
            ValidationError(msg.ToString());
            ErrorMessages.Add(msg.ToString());
        }

        #region Apply Transformations

        private void TransformImportFileData()
        {
            try
            {
                accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                var multiMapToFieldMap = new StringDictionary();
                var allMultiMappings = new List<FieldMultiMap>();
                var transformationsGrouping = GetTransformationsGrouped(accessKey);
                FileValidationTransformationHelper.DiscoverColumns(sourceFile, dataIV, allMultiMappings, multiMapToFieldMap);
                FileValidationTransformationHelper.FillImportFileHeaderTransformedColumns(dataIV, sourceFile, allMultiMappings);

                SetText(currentOperation, $"Tran count original: {dataIV.DataTransformed.Count}");
                SetText(currentProgress, "5");

                var fmPubCode = sourceFile.FieldMappings.SingleOrDefault(x => x.MAFField.Equals(PubCodeFieldName, StringComparison.CurrentCultureIgnoreCase));
                FileValidationTransformationHelper.DiscoverTransformAssignsWithoutPubId(sourceFile, dataIV, transformationsGrouping, LogException);
                SetText(currentOperation, $"Tran count after Step 1: {dataIV.DataTransformed.Count}");
                SetText(currentProgress, "15");

                FileValidationTransformationHelper.ProcessSplitTransformations(sourceFile, dataIV, transformationsGrouping, LogException);
                SetText(currentOperation, $"Tran count after Step 2: {dataIV.DataTransformed.Count}");
                SetText(currentProgress, "25");

                FileValidationTransformationHelper.ProcessDataMapTransformsWithMapsPubCode(sourceFile, dataIV, serviceFeature, clientPubCodes, transformationsGrouping, fmPubCode, LogException);
                SetText(currentOperation, $"Tran count after Step 3: {dataIV.DataTransformed.Count}");
                SetText(currentProgress, "35");

                ProcessTransformationsByPubCode(transformationsGrouping, fmPubCode);
                SetText(currentOperation, $"Tran count after Step 4: {dataIV.DataTransformed.Count}");
                SetText(currentProgress, "65");

                ProcessJoinTransforms(transformationsGrouping, fmPubCode);
                SetText(currentOperation, $"Tran count after Step 5: {dataIV.DataTransformed.Count}");
                SetText(currentProgress, "75");

                ProcessDataMapTransforms(transformationsGrouping, fmPubCode);
                SetText(currentOperation, $"Tran count after Step 6: {dataIV.DataTransformed.Count}");
                SetText(currentProgress, "85");

                ProcessMultimappingsSetValueFromFieldMapping(multiMapToFieldMap);
                SetText(currentOperation, $"Tran count after Step 7: {dataIV.DataTransformed.Count}");
                SetText(currentProgress, "95");

                FileValidationTransformationHelper.ProcessOriginalImportRowColumn(dataIV); 
                dataIV.TransformedRowCount = dataIV.DataTransformed.Count;
                SetText(currentOperation, $"Tran count final: {dataIV.DataTransformed.Count}");
                SetText(currentProgress, "100");
            }
            catch (Exception ex)
            {
                var sbDetail = new StringBuilder();
                sbDetail.AppendLine(Value);
                sbDetail.AppendLine(Environment.NewLine);

                sbDetail.AppendLine(StringFunctions.FormatException(ex));
                ValidationError(sbDetail.ToString());
                ErrorMessages.Add(sbDetail.ToString());
            }
        }

        private void ProcessMultimappingsSetValueFromFieldMapping(StringDictionary multiMapToFieldMap)
        {
            if (multiMapToFieldMap.Count == 0)
            {
                return;
            }

            foreach (var key in dataIV.DataTransformed.Keys)
            {
                var myRow = dataIV.DataTransformed[key];
                foreach (DictionaryEntry kvp in multiMapToFieldMap)
                {
                    if (myRow.ContainsKey(kvp.Key.ToString()))
                    {
                        myRow[kvp.Key.ToString()] = myRow[kvp.Value.ToString()];
                    }
                }
            }
        }

        private void ProcessDataMapTransforms(
            TransformationDefinitionsGrouping transformConfigs,
            FieldMapping fmPubCode)
        {
            if (transformConfigs.DataMapTrans.Count == 0 ||
                BusinessEnums.GetUADFeature(serviceFeature.SFName) == BusinessEnums.UADFeatures.Data_Compare)
            {
                return;
            }

            foreach (var key in dataIV.DataTransformed.Keys)
            {
                var myRow = dataIV.DataTransformed[key];
                try
                {
                    if (!string.IsNullOrWhiteSpace(myRow[fmPubCode.IncomingField]) &&
                        clientPubCodes.ContainsValue(myRow[fmPubCode.IncomingField]))
                    {
                        var pubCodeId = clientPubCodes
                            .Single(x => x.Value.Equals(myRow[fmPubCode.IncomingField].ToString()))
                            .Key;

                        if (pubCodeId > 0)
                        {
                            foreach (var transformation in transformConfigs.GetDataMapTransformsForPubCode(pubCodeId).Where(t => !t.LastStepDataMap))
                            {
                                foreach (var tdmx in transformConfigs.GetDataMaps(transformation.TransformationID, pubCodeId))
                                {
                                    var mappingTransformation = transformConfigs.GetTransformation(tdmx.TransformationID);
                                    var transformationFieldMaps = mappingTransformation?.FieldMap.Where(x => x.SourceFileID == dataIV.SourceFileId).ToList();
                                    if (transformationFieldMaps?.Count > 0 &&
                                        FileValidationTransformationHelper.ProcessDataMappingForField(sourceFile,transformationFieldMaps, myRow, tdmx))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogException(ex, dataIV.SourceFileId, dataIV.ProcessCode, dataIV.ClientId, key, myRow);
                }
            }

            //Clearing lists after their last use
            transformConfigs.DataMapTrans.Clear();
            transformConfigs.DataMapTrans.TrimExcess();
        }

        private void ProcessJoinTransforms(TransformationDefinitionsGrouping transformConfigs, FieldMapping fmPubCode)
        {
            if (transformConfigs.JoinTrans.Count <= 0 ||
                BusinessEnums.GetUADFeature(serviceFeature.SFName) == BusinessEnums.UADFeatures.Data_Compare)
            {
                return;
            }

            transformConfigs.JoinTrans.Clear();
            transformConfigs.JoinTrans.TrimExcess();
        }

        private void ProcessTransformationsByPubCode(
            TransformationDefinitionsGrouping transformConfigs,
            FieldMapping fmPubCode)
        {
            if (BusinessEnums.GetUADFeature(serviceFeature.SFName) == BusinessEnums.UADFeatures.Data_Compare ||
                (transformConfigs.DataMapTrans.Count == 0 &&
                transformConfigs.AssignTrans.Count == 0 &&
                (transformConfigs.SplitTrans.Count == 0 ||
                 BusinessEnums.GetUADFeature(serviceFeature.SFName) == BusinessEnums.UADFeatures.Special_Split)))
            {
                return;
            }

            var distinctPubCodes = GetDistinctPubCodes(fmPubCode);
            var deleteKeys = new List<int>();
            var addRows = new Dictionary<int, StringDictionary>();
            foreach (var kvp in distinctPubCodes)
            {
                var dmList = transformConfigs.GetDataMapTransformsForPubCode(kvp.Key);
                var splitList = transformConfigs.GetSplitTransformsForPubCode(kvp.Key); 
                var assignList = transformConfigs.GetAssignTransformsForPubCode(kvp.Key);

                SetText(currentOperation, $"dmList count for PubCode: {kvp.Value} - {dmList.Count}");
                SetText(currentOperation, $"splitList count for PubCode: {kvp.Value} - {splitList.Count}");
                SetText(currentOperation, $"assignList count for PubCode: {kvp.Value} - {assignList.Count}");

                if (!dmList.Any() && !splitList.Any() && !assignList.Any())
                {
                    continue;
                }

                var sdList = dataIV.DataTransformed.Values.Where(x =>
                        x[fmPubCode.IncomingField].Equals(kvp.Value, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();
                var counter = 1;
                foreach (var myRow in sdList)
                {
                    counter = ProcessPubCodeTransformationsForRow(transformConfigs, myRow, kvp, counter, assignList, dmList, splitList, deleteKeys, addRows);
                }
            }

            foreach (var k in addRows)
            {
                dataIV.DataTransformed.Add(k.Key, k.Value);
            }

            foreach (var k in deleteKeys)
            {
                dataIV.DataTransformed.Remove(k);
            }

            var newKey = 1;
            foreach (var key in dataIV.DataTransformed.Keys)
            {
                dataIV.DataTransformed.ChangeKey(key, newKey);
                newKey++;
            }

            transformConfigs.SplitTrans.Clear();
            transformConfigs.SplitTrans.TrimExcess();

            transformConfigs.AssignTrans.Clear();
            transformConfigs.AssignTrans.TrimExcess();

            transformConfigs.TransSplit.Clear();
            transformConfigs.TransSplit.TrimExcess();
        }

        private int ProcessPubCodeTransformationsForRow(
            TransformationDefinitionsGrouping transformConfigs,
            StringDictionary myRow,
            KeyValuePair<int, string> kvp, 
            int counter,
            IList<Transformation> assignList,
            IList<Transformation> dmList,
            IList<Transformation> splitList,
            ICollection<int> deleteKeys, 
            Dictionary<int, StringDictionary> addRows)
        {
            var key = dataIV.DataTransformed.Single(x => x.Value == myRow).Key;
            SetText(currentOperation, $"PubCode Transformations for {kvp.Value} : {counter} of ");
            try
            {
                if (assignList.Count > 0)
                {
                    FileValidationTransformationHelper.ProcessPubCodeAssignTransformationsForRow(sourceFile, dataIV, transformConfigs, myRow, assignList);
                }

                if (dmList.Count > 0)
                {
                    FileValidationTransformationHelper.ProcessPubCodeDataMapTransformationsForRow(sourceFile, dataIV, transformConfigs, myRow, kvp, dmList);
                }

                if (splitList.Count > 0)
                {
                    if (BusinessEnums.GetUADFeature(serviceFeature.SFName) != BusinessEnums.UADFeatures.Special_Split)
                    {
                        FileValidationTransformationHelper.ProcessPubCodeSpecialSplitTransformationsForRow(sourceFile, dataIV, transformConfigs, myRow, splitList, deleteKeys, addRows, key);
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex, dataIV.SourceFileId, dataIV.ProcessCode, dataIV.ClientId, key, myRow);
            }

            counter++;
            return counter;
        }

        private Dictionary<int, string> GetDistinctPubCodes(FieldMapping fmPubCode)
        {
            var distinctPubCodes = new Dictionary<int, string>();
            foreach (var key in dataIV.DataTransformed.Keys)
            {
                var myRow = dataIV.DataTransformed[key];
                if (!string.IsNullOrEmpty(myRow[fmPubCode.IncomingField]) &&
                    clientPubCodes.ContainsValue(myRow[fmPubCode.IncomingField]))
                {
                    var pubCodeId = clientPubCodes.Single(x => x.Value.Equals(myRow[fmPubCode.IncomingField].ToString())).Key;
                    var pub = new KeyValuePair<int, string>(pubCodeId, myRow[fmPubCode.IncomingField]);
                    if (!distinctPubCodes.Contains(pub))
                    {
                        distinctPubCodes.Add(pubCodeId, myRow[fmPubCode.IncomingField]);
                    }
                }
            }

            return distinctPubCodes;
        }

        private TransformationDefinitionsGrouping GetTransformationsGrouped(Guid accessKey)
        {
            var tData = ServiceClient.UAS_TransformationClient();
            var tdmData = ServiceClient.UAS_TransformDataMapClient();
            var cWorker = ServiceClient.UAD_Lookup_CodeClient();

            var allTrans = new HashSet<Transformation>(
                tData.Proxy.Select(accessKey, dataIV.ClientId, dataIV.SourceFileId, true)
                .Result.Where(x => x.IsActive).ToList());

            foreach (var transformation in allTrans)
            {
                transformation.FieldMap =
                    new HashSet<TransformationFieldMap>(
                        transformation.FieldMap.Where(x => x.SourceFileID == dataIV.SourceFileId).ToList());
            }

            var transTypes = new HashSet<FrameworkUAD_Lookup.Entity.Code>(
                cWorker.Proxy.Select(accessKey, Enums.CodeType.Transformation).Result.ToList());

            var dataMappingId = transTypes.Single(
                x => x.CodeName.Equals(Enums.TransformationTypes.Data_Mapping.ToString().Replace("_", " "))).CodeId;
            var joinColumnsId = transTypes.Single(
                x =>x.CodeName.Equals(Enums.TransformationTypes.Join_Columns.ToString().Replace("_", " "))).CodeId;
            var splitIntoRowsId = transTypes.Single(
                x =>x.CodeName.Equals(Enums.TransformationTypes.Split_Into_Rows.ToString().Replace("_", " "))).CodeId;
            var assignValueId = transTypes.Single(
                    x =>x.CodeName.Equals(Enums.TransformationTypes.Assign_Value.ToString().Replace("_", " "))).CodeId;
            var splitTransformId = transTypes.Single(
                    x =>x.CodeName.Equals(Enums.TransformationTypes.Split_Transform.ToString().Replace("_", " "))).CodeId;

            var tsData = ServiceClient.UAS_TransformSplitClient();
            var tjData = ServiceClient.UAS_TransformJoinClient();
            var tstData = ServiceClient.UAS_TransformSplitTransClient();

            return new TransformationDefinitionsGrouping()
            {
                AllTrans = allTrans,
                DataMapTrans = new HashSet<Transformation>(
                    allTrans.Where(x => x.TransformationTypeID == dataMappingId).ToList()),
                JoinTrans = new HashSet<Transformation>(
                    allTrans.Where(x => x.TransformationTypeID == joinColumnsId).ToList()),
                SplitTrans = new HashSet<Transformation>(
                    allTrans.Where(x => x.TransformationTypeID == splitIntoRowsId).ToList()),
                AssignTrans = new HashSet<Transformation>(
                    allTrans.Where(x => x.TransformationTypeID == assignValueId).ToList()),
                TransSplit = new HashSet<Transformation>(
                    allTrans.Where(x => x.TransformationTypeID == splitTransformId).ToList()),
                AllTransAssign = new HashSet<TransformAssign>(ServiceClient.UAS_TransformAssignClient().Proxy
                    .SelectForSourceFile(accessKey, dataIV.SourceFileId).Result.Where(x => x.IsActive).ToList()),
                AllTransDataMap = new HashSet<TransformDataMap>(tdmData.Proxy
                    .SelectForSourceFile(accessKey, dataIV.SourceFileId).Result.Where(x => x.IsActive).ToList()),
                AllTransSplitMap = new HashSet<TransformSplit>(tsData.Proxy
                    .SelectForSourceFile(accessKey, dataIV.SourceFileId).Result.Where(x => x.IsActive).ToList()),
                AllTransJoinMap = new HashSet<TransformJoin>(
                    tjData.Proxy.SelectForSourceFile(accessKey, dataIV.SourceFileId).Result.Where(x => x.IsActive).ToList()),
                AllSplitTrans = new HashSet<TransformSplitTrans>(
                    tstData.Proxy.SelectSourceFileID(accessKey, dataIV.SourceFileId).Result.Where(x => x.IsActive).ToList()),
                EveryTransDataMap = new HashSet<TransformDataMap>(
                    tdmData.Proxy.Select(accessKey).Result.Where(x => x.IsActive).ToList()),
                EveryTransSplitMap = new HashSet<TransformSplit>(
                    tsData.Proxy.Select(accessKey).Result.Where(x => x.IsActive).ToList())
            };
        }

        #endregion
        #region AdHocDimensions
        private StringDictionary SetAdHoc(FrameworkUAS.Entity.AdHocDimensionGroup adg, StringDictionary dr, List<FrameworkUAS.Entity.AdHocDimension> adList)
        {
            string sourceString = dr[adg.StandardField].ToString();
            int sourceInt = 0;
            int.TryParse(sourceString, out sourceInt);

            string generatedValue = string.Empty;
            string cd = adg.CreatedDimension.ToUpper();
            if (String.IsNullOrEmpty(dr[cd].ToString()))
                generatedValue = adg.DefaultValue.ToString();

            generatedValue = ValidatorMethods.SetGeneratedValue(adList, sourceInt, sourceString, generatedValue, false);

            if (generatedValue == null)
                generatedValue = string.Empty;

            if (String.IsNullOrEmpty(generatedValue))
                generatedValue = dr[cd].ToString();

            dr[cd] = generatedValue;
            return dr;
        }
        private void ApplyAdHocDimensions()
        {
            //first check if client has any AHD's
            FrameworkServices.ServiceClient<UAS_WS.Interface.IAdHocDimensionGroup> ahdgWorker = FrameworkServices.ServiceClient.UAS_AdHocDimensionGroupClient();
            FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.AdHocDimensionGroup>> ahdgResp = new FrameworkUAS.Service.Response<List<AdHocDimensionGroup>>();
            List<FrameworkUAS.Entity.AdHocDimensionGroup> ahdGroups = new List<AdHocDimensionGroup>();
            ahdgResp = ahdgWorker.Proxy.Select(accessKey, client.ClientID, true);
            if (ahdgResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success && ahdgResp.Result != null)
                ahdGroups = ahdgResp.Result.Where(x => x.IsActive == true).OrderBy(y => y.OrderOfOperation).ToList();

            FrameworkServices.ServiceClient<UAS_WS.Interface.IAdHocDimension> adWorker = FrameworkServices.ServiceClient.UAS_AdHocDimensionClient();
            FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.AdHocDimension>> adResp = new FrameworkUAS.Service.Response<List<AdHocDimension>>();

            if (ahdGroups.Count > 0)
            {
                //Grab codesheet values for the dimension group for below. Only want to run on invalid values.
                FrameworkServices.ServiceClient<UAD_WS.Interface.ICodeSheet> csWorker = FrameworkServices.ServiceClient.UAD_CodeSheetClient();
                FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>> csResp = new FrameworkUAS.Service.Response<List<CodeSheet>>();
                List<FrameworkUAD.Entity.CodeSheet> codeSheet = new List<CodeSheet>();
                csResp = csWorker.Proxy.Select(accessKey, client.ClientConnections);
                if (csResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success && csResp.Result != null)
                    codeSheet = csResp.Result;

                //Dictionary<int, string> clientPubCodes = FrameworkUAS.Object.DBWorker.GetPubIDAndCodesByClient(client);

                foreach (FrameworkUAS.Entity.AdHocDimensionGroup adg in ahdGroups)
                {
                    //so now create the dimension column OR set the column value
                    //first check if column exists
                    if (dataIV.HeadersTransformed.ContainsKey(adg.StandardField))
                    {
                        List<FrameworkUAS.Entity.AdHocDimension> adList = new List<AdHocDimension>();
                        adResp = adWorker.Proxy.Select(accessKey, adg.AdHocDimensionGroupId);
                        if (adResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success && adResp.Result != null)
                            adList = adResp.Result;

                        //our compare column is there now check for CreatedDimension column if not there create
                        //do a foreach loop instead of contains.
                        bool colExist = false;
                        if (dataIV.HeadersTransformed.ContainsKey(adg.CreatedDimension))
                            colExist = true;

                        if (colExist == false)
                        {
                            dataIV.HeadersTransformed.Add(adg.CreatedDimension, (dataIV.HeadersTransformed.Count + 1).ToString());
                            //add new column to each row with a blank value
                            foreach (var key in dataIV.DataTransformed.Keys)
                            {
                                StringDictionary myRow = dataIV.DataTransformed[key];
                                myRow.Add(adg.CreatedDimension.ToUpper(), string.Empty);
                            }
                        }
                        else
                        {
                            foreach (var key in dataIV.DataTransformed.Keys)
                            {
                                StringDictionary myRow = dataIV.DataTransformed[key];
                                if (!myRow.ContainsKey(adg.CreatedDimension.ToUpper()))
                                    myRow.Add(adg.CreatedDimension.ToUpper(), string.Empty);
                            }
                        }
                        if (adg.IsPubcodeSpecific == true)
                        {
                            foreach (var key in dataIV.DataTransformed.Keys)
                            {
                                StringDictionary myRow = dataIV.DataTransformed[key];
                                string pubCode = string.Empty;
                                if (myRow.ContainsKey(PubCodeFieldName))
                                    pubCode = myRow[PubCodeFieldName].ToString();

                                if (adg.DimensionGroupPubcodeMappings.Exists(x => x.Pubcode.Equals(pubCode, StringComparison.CurrentCultureIgnoreCase) && x.IsActive == true))
                                {
                                    //Get the Pubcode ID and codesheet values for the pubID and ResponseGroup = CreatedDimension
                                    //Check the codesheet values where current value = null meaning value not found/invalid and run the SetAdHoc
                                    var pubIDKey = clientPubCodes.FirstOrDefault(x => x.Value.Equals(pubCode, StringComparison.CurrentCultureIgnoreCase)).Key;
                                    int pubID = 0;
                                    int.TryParse(pubIDKey.ToString(), out pubID);
                                    List<FrameworkUAD.Entity.CodeSheet> pubCodeSheet = codeSheet.Where(x => x.PubID.Equals(pubID) && x.ResponseGroup.Equals(adg.CreatedDimension, StringComparison.CurrentCultureIgnoreCase)).ToList();
                                    int i = 0;
                                    string cd = adg.CreatedDimension.ToUpper();
                                    if (pubCodeSheet != null && pubCodeSheet.Count > 0)
                                    {
                                        if (int.TryParse(myRow[cd].ToString(), out i))
                                        {
                                            if (pubCodeSheet.FirstOrDefault(x => x.ResponseValue.Equals(myRow[cd].ToString().TrimStart('0'), StringComparison.CurrentCultureIgnoreCase)) == null)
                                            {
                                                SetAdHoc(adg, myRow, adList);
                                            }
                                        }
                                        else
                                        {
                                            if (pubCodeSheet.FirstOrDefault(x => x.ResponseValue.Equals(myRow[adg.CreatedDimension].ToString(), StringComparison.CurrentCultureIgnoreCase)) == null)
                                            {
                                                SetAdHoc(adg, myRow, adList);
                                            }
                                        }
                                    }
                                    else
                                        SetAdHoc(adg, myRow, adList);
                                }
                            }
                        }
                        else
                        {
                            foreach (var key in dataIV.DataTransformed.Keys)
                            {
                                StringDictionary myRow = dataIV.DataTransformed[key];
                                SetAdHoc(adg, myRow, adList);
                            }

                        }
                    }
                }
            }
        }
        #endregion
        #region Subscriber table Inserts - Original - Invalid - Transformed
        private List<FrameworkUAD.Entity.SubscriberOriginal> InsertOriginalSubscribers()
        {
            List<FrameworkUAD.Entity.SubscriberOriginal> listSubscriberOriginal = new List<SubscriberOriginal>();

            FrameworkServices.ServiceClient<UAS_WS.Interface.IAdHocDimensionGroup> ahdgWorker = FrameworkServices.ServiceClient.UAS_AdHocDimensionGroupClient();
            FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.AdHocDimensionGroup>> ahdgResp = new FrameworkUAS.Service.Response<List<AdHocDimensionGroup>>();
            List<FrameworkUAS.Entity.AdHocDimensionGroup> ahdGroups = new List<AdHocDimensionGroup>();
            ahdgResp = ahdgWorker.Proxy.Select(accessKey, client.ClientID, false);
            if (ahdgResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success && ahdgResp.Result != null)
                ahdGroups = ahdgResp.Result.Where(x => x.IsActive == true).OrderBy(y => y.OrderOfOperation).ToList();

            FrameworkUAS.Entity.FieldMapping fm = sourceFile.FieldMappings.SingleOrDefault(x => x.MAFField.Equals(PubCodeFieldName, StringComparison.CurrentCultureIgnoreCase));

            foreach (var key in dataIV.DataOriginal.Keys)
            {
                StringDictionary myRow = dataIV.DataOriginal[key];
                int pubCodeID = -1;
                if (KMPlatform.BusinessLogic.Enums.GetUADFeature(serviceFeature.SFName) != KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                {
                    if (fm != null && myRow[fm.IncomingField] != null)
                        if (!string.IsNullOrEmpty(myRow[fm.IncomingField].ToString()))
                        {
                            string pubCodeValue = myRow[fm.IncomingField].ToString();
                            int.TryParse(clientPubCodes.SingleOrDefault(x => x.Value.Equals(pubCodeValue)).Key.ToString(), out pubCodeID);
                        }
                }

                listSubscriberOriginal.Add(CreateSubscriber(myRow, pubCodeID, ahdGroups));
            }

            #region dedupe profiles
            //now due to transformations we may have duplicate profiles
            //can only do this after creating Subscriber objects - before insert
            List<FrameworkUAD.Entity.SubscriberOriginal> dedupedSubscriberOriginal = new List<FrameworkUAD.Entity.SubscriberOriginal>();
            dedupedSubscriberOriginal = listSubscriberOriginal.GroupBy(item => new { item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone }).Select(group => group.First()).ToList();

            List<FrameworkUAD.Entity.SubscriberOriginal> excluded = listSubscriberOriginal.Except(dedupedSubscriberOriginal).ToList();
            dataIV.OriginalDuplicateRecordCount = excluded.Count;
            if (excluded.Count > 0)
            {
                int rowCount = 1;
                foreach (FrameworkUAD.Entity.SubscriberOriginal exc in excluded)
                {
                    string msg = "Demo check: " + rowCount.ToString() + " of " + excluded.Count.ToString();
                    Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Green);

                    FrameworkUAD.Entity.SubscriberOriginal deDupeMatch = dedupedSubscriberOriginal.SingleOrDefault(x => (x.Address == exc.Address &&
                                                                                            x.PubCode == exc.PubCode &&
                                                                                            x.FName == exc.FName &&
                                                                                            x.LName == exc.LName &&
                                                                                            x.Company == exc.Company &&
                                                                                            x.Title == exc.Title &&
                                                                                            x.Email == exc.Email &&
                                                                                            x.Phone == exc.Phone) && x.SORecordIdentifier != exc.SORecordIdentifier);
                    if (deDupeMatch != null)
                    {
                        //this will always get all as differences cause SORecIdentifiers are always different so need to join on Maff and check for different values
                        //exc.DemographicOriginalList.Except(deDupeMatch.DemographicOriginalList).ToList();
                        List<FrameworkUAD.Entity.SubscriberDemographicOriginal> notMatch = (from excd in exc.DemographicOriginalList
                                                                                            join dd in deDupeMatch.DemographicOriginalList on excd.MAFField equals dd.MAFField
                                                                                            where excd.Value != dd.Value
                                                                                            select excd).Distinct().ToList();

                        List<FrameworkUAD.Entity.SubscriberDemographicOriginal> notExist = (from e in exc.DemographicOriginalList
                                                                                            where !deDupeMatch.DemographicOriginalList.Any(x => x.MAFField == e.MAFField)
                                                                                            select e).ToList();


                        List<FrameworkUAD.Entity.SubscriberDemographicOriginal> combined = new List<SubscriberDemographicOriginal>();
                        combined.AddRange(notMatch);
                        combined.AddRange(notExist);
                        combined.ForEach(x => x.SORecordIdentifier = deDupeMatch.SORecordIdentifier);
                        //deDupeMatch.DemographicOriginalList.AddRange(combined);
                        combined.ToList().ForEach(x => { deDupeMatch.DemographicOriginalList.Add(x); });
                        notMatch = null;
                        notExist = null;
                        combined = null;
                    }
                    deDupeMatch = null;

                    rowCount++;
                }
            }
            else
                dedupedSubscriberOriginal = listSubscriberOriginal;
            #endregion

            FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriberOriginal> soWorker = FrameworkServices.ServiceClient.UAD_SubscriberOriginalClient();
            FrameworkUAS.Service.Response<bool> soResp = new FrameworkUAS.Service.Response<bool>();
            bool success = false;
            soResp = soWorker.Proxy.SaveBulkSqlInsert(accessKey, dedupedSubscriberOriginal, client.ClientConnections);
            if (soResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success && soResp.Result != null)
                success = soResp.Result;

            if (success == false)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("ERROR: Insert to Original Bulk Data Insert " + DateTime.Now.TimeOfDay.ToString());
                msg.AppendLine("An unexplained error occurred while inserting records into Original or DemoOriginal tables.<br/>");
                msg.AppendLine("The details of this error have been logged in the FileLog table.<br/>");
                msg.AppendLine("Please run this query.<br/>");
                msg.AppendLine("Select * From FileLog With(NoLock) Where SourceFileID = " + sourceFile.SourceFileID.ToString() + " AND ProcessCode = '" + processCode.ToString() + "' ORDER BY LogDate, LogTime; GO");

                ValidationError(msg.ToString());
                ErrorMessages.Add(msg.ToString());

                dedupedSubscriberOriginal.Clear();
            }

            return dedupedSubscriberOriginal;
        }
        
        private List<SubscriberInvalid> ConvertTransformedToInvalid(IEnumerable<SubscriberTransformed> listSt)
        {
            return listSt.Select(st => new SubscriberInvalid(st)).ToList();
        }

        private List<FrameworkUAD.Entity.SubscriberTransformed> DeDupeTransformed(List<FrameworkUAD.Entity.SubscriberTransformed> listOrigST)
        {
            #region dedupe profiles
            listOrigST.OrderBy(x => x.FName).ThenBy(y => y.LName).ToList();
            //now due to transformations we may have duplicate profiles
            //can only do this after creating Subscriber objects - before insert
            List<FrameworkUAD.Entity.SubscriberTransformed> dedupedST = new List<SubscriberTransformed>();
            //group on PubCode, FirstName, LastName, Company, Title, Address, Email, Phone
            dedupedST = listOrigST.GroupBy(item => new { item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone }).Select(group => group.First()).ToList();
            List<FrameworkUAD.Entity.SubscriberTransformed> excluded = listOrigST.Except(dedupedST).ToList();
            dataIV.TransformedDuplicateRecordCount = excluded.Count;
            if (excluded.Count > 0)
            {
                int rowCount = 1;
                foreach (FrameworkUAD.Entity.SubscriberTransformed exc in excluded)
                {
                    string msg = "Demo check: " + rowCount.ToString() + " of " + excluded.Count.ToString();
                    Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Green);
                    SetText(currentOperation, msg);

                    FrameworkUAD.Entity.SubscriberTransformed deDupeMatch = dedupedST.SingleOrDefault(x => (x.Address == exc.Address &&
                                                                                            x.PubCode == exc.PubCode &&
                                                                                            x.FName == exc.FName &&
                                                                                            x.LName == exc.LName &&
                                                                                            x.Company == exc.Company &&
                                                                                            x.Title == exc.Title &&
                                                                                            x.Email == exc.Email &&
                                                                                            x.Phone == exc.Phone) && x.STRecordIdentifier != exc.STRecordIdentifier);

                    //var i = deDupeMatch.DemographicTransformedList.Select(x => new { x.MAFField, x.Value }).Distinct();
                    //int count = i.Count();

                    if (deDupeMatch != null)
                    {
                        List<FrameworkUAD.Entity.SubscriberDemographicTransformed> notMatch = (from excd in exc.DemographicTransformedList
                                                                                               join dd in deDupeMatch.DemographicTransformedList on excd.MAFField equals dd.MAFField
                                                                                               where excd.Value != dd.Value
                                                                                               select excd).Distinct().ToList();
                        //List<FrameworkUAD.Entity.SubscriberDemographicTransformed> notMatchNew = (from excd in exc.DemographicTransformedList
                        //                                                                          join dd in deDupeMatch.DemographicTransformedList.Select(x => new { x.MAFField, x.Value }).Distinct() on excd.MAFField equals dd.MAFField
                        //                                                                          where excd.Value != dd.Value
                        //                                                                          select excd).Distinct().ToList();

                        List<FrameworkUAD.Entity.SubscriberDemographicTransformed> notExist = (from e in exc.DemographicTransformedList
                                                                                               where !deDupeMatch.DemographicTransformedList.Any(x => x.MAFField == e.MAFField)
                                                                                               select e).ToList();


                        List<FrameworkUAD.Entity.SubscriberDemographicTransformed> combined = new List<SubscriberDemographicTransformed>();
                        combined.AddRange(notMatch);
                        combined.AddRange(notExist);
                        combined.ForEach(x => x.STRecordIdentifier = deDupeMatch.STRecordIdentifier);
                        combined.ForEach(x => x.SORecordIdentifier = deDupeMatch.SORecordIdentifier);
                        // deDupeMatch.DemographicTransformedList.AddRange(combined);
                        combined.ToList().ForEach(x => { deDupeMatch.DemographicTransformedList.Add(x); });
                        notMatch = null;
                        notExist = null;
                        combined = null;
                    }
                    deDupeMatch = null;
                    rowCount++;
                }
            }
            else
                dedupedST = listOrigST;
            #endregion

            return dedupedST;
        }
        private List<FrameworkUAD.Entity.SubscriberInvalid> DeDupeInvalid(List<FrameworkUAD.Entity.SubscriberInvalid> listOrig)
        {
            #region dedupe profiles
            //now due to transformations we may have duplicate profiles
            //can only do this after creating Subscriber objects - before insert
            List<FrameworkUAD.Entity.SubscriberInvalid> deduped = new List<SubscriberInvalid>();
            //group on PubCode, FirstName, LastName, Company, Title, Address, Email, Phone
            deduped = listOrig.GroupBy(item => new { item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone }).Select(group => group.First()).ToList();
            List<FrameworkUAD.Entity.SubscriberInvalid> excluded = listOrig.Except(deduped).ToList();
            if (excluded.Count > 0)
            {
                int rowCount = 1;
                foreach (FrameworkUAD.Entity.SubscriberInvalid exc in excluded)
                {
                    string msg = "Demo check: " + rowCount.ToString() + " of " + excluded.Count.ToString();
                    Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Green);

                    FrameworkUAD.Entity.SubscriberInvalid deDupeMatch = deduped.SingleOrDefault(x => (x.Address == exc.Address &&
                                                                                            x.PubCode == exc.PubCode &&
                                                                                            x.FName == exc.FName &&
                                                                                            x.LName == exc.LName &&
                                                                                            x.Company == exc.Company &&
                                                                                            x.Title == exc.Title &&
                                                                                            x.Email == exc.Email &&
                                                                                            x.Phone == exc.Phone) && x.SIRecordIdentifier != exc.SIRecordIdentifier);
                    if (deDupeMatch != null)
                    {
                        List<FrameworkUAD.Entity.SubscriberDemographicInvalid> notMatch = (from excd in exc.DemographicInvalidList
                                                                                           join dd in deDupeMatch.DemographicInvalidList on excd.MAFField equals dd.MAFField
                                                                                           where excd.Value != dd.Value
                                                                                           select excd).Distinct().ToList();

                        List<FrameworkUAD.Entity.SubscriberDemographicInvalid> notExist = (from e in exc.DemographicInvalidList
                                                                                           where !deDupeMatch.DemographicInvalidList.Any(x => x.MAFField == e.MAFField)
                                                                                           select e).ToList();
                        List<FrameworkUAD.Entity.SubscriberDemographicInvalid> combined = new List<SubscriberDemographicInvalid>();
                        combined.AddRange(notMatch);
                        combined.AddRange(notExist);
                        combined.ForEach(x => x.SIRecordIdentifier = deDupeMatch.SIRecordIdentifier);
                        combined.ForEach(x => x.SORecordIdentifier = deDupeMatch.SORecordIdentifier);
                        combined.ToList().ForEach(x => { deDupeMatch.DemographicInvalidList.Add(x); });

                        notMatch = null;
                        notExist = null;
                        combined = null;
                    }
                    deDupeMatch = null;
                    rowCount++;
                }
            }
            else
                deduped = listOrig;
            #endregion

            return deduped;
        }
        #endregion
        
        #region Exception Logging
        private void LogException(Exception ex, int sourceFileID, string processCode, int clientID, int rowNumber, StringDictionary rowData)
        {
            string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);

            StringBuilder sbDetail = new StringBuilder();
            sbDetail.AppendLine("ClientID: " + clientID.ToString());
            sbDetail.AppendLine("SourceFileID: " + sourceFileID.ToString());
            sbDetail.AppendLine("ProcessCode: " + processCode.ToString());
            sbDetail.AppendLine("Row Number: " + rowNumber.ToString());
            sbDetail.AppendLine(System.Environment.NewLine);
            sbDetail.AppendLine(message);
            ErrorMessages.Add(sbDetail.ToString());
            ValidationError(sbDetail.ToString(), rowNumber, rowData);
        }
        protected override bool ValidationError(string errorMsg, int rowNumber = 0, StringDictionary rowData = null)
        {
            bool exceedThreashold = false;
            FrameworkUAD.Entity.ImportError ie = new ImportError();
            StringBuilder sb = new StringBuilder();
            if (rowData != null)
            {
                String[] myKeys = new String[dataIV.BadDataOriginalHeaders.Count];
                dataIV.BadDataOriginalHeaders.Keys.CopyTo(myKeys, 0);
                foreach (string s in myKeys.OrderBy(x => x))
                {
                    if (s.Equals("originalimportrow", StringComparison.CurrentCultureIgnoreCase))
                        continue;

                    if (dataIV.BadDataOriginalHeaders.ContainsKey(s.ToString()))
                    {
                        if (rowData.ContainsKey(s))
                        {
                            sb.Append(rowData[s].ToString() + ",");
                        }
                        else
                        {
                            sb.Append(",");
                        }
                    }
                }
            }
            ie.BadDataRow = sb.ToString().Trim();//.TrimEnd(',');
            ie.ClientMessage = errorMsg;
            ie.RowNumber = rowNumber;

            if (dataIV.ImportErrors == null)
                dataIV.ImportErrors = new HashSet<ImportError>();
            dataIV.HasError = true;
            dataIV.ImportErrorCount++;
            dataIV.ImportErrors.Add(ie);

            int check = 0;
            if (dataIV.DataTransformed.Count > 0)
                check = (int)(((decimal)dataIV.ImportErrorCount / (decimal)dataIV.DataTransformed.Count) * 100);
            if (dataIV.ImportErrorCount >= 100000 || check >= 25)
            {
                //send some email that the processing of this file has stopped due to number of error encountered. 
                //100k errors or 25% of records in error state.
                exceedThreashold = true;
                ErrorMessages.Add("Rejecting file due to error threshold - will delete file from repository then shut down current engine and start new instance - " + DateTime.Now.TimeOfDay.ToString());
            }

            return exceedThreashold;
        }
        #endregion

        private List<SubscriberTransformed> Scranton_CompanySurvey(List<SubscriberTransformed> subscribers, Dictionary<int, string> clientPubCodes, int clientId)
        {
            var ahdgWorker = ServiceClient.UAS_AdHocDimensionGroupClient();
            var adgList = ahdgWorker.Proxy.Select(accessKey, clientId, true).Result;
            var fuzzySearch = new FuzzySearch();
            var matchCompanies = new HashSet<string>();
            var matchEmails = new HashSet<string>();
            List<AdHocDimension> domainList;
            int adgCompanyId;
            int adgDomainId;

            var demoUpdate = GetDemographicUpdateCode();
            InitCompanyAndDomains(adgList, out adgCompanyId, out adgDomainId, out domainList);
            InitMatchTokens(matchCompanies, matchEmails);

            foreach (var subscriberTransformed in subscribers)
            {
                var pubId = clientPubCodes.Single(x => x.Value.Equals(subscriberTransformed.PubCode, StringComparison.OrdinalIgnoreCase)).Key;
                var foundCompanyMatch = false;
                if (matchCompanies.Count > 0 && !string.IsNullOrWhiteSpace(subscriberTransformed.Company))
                {
                    foundCompanyMatch = SeachAndAddPBG300SurveyByCompany(matchCompanies, fuzzySearch, subscriberTransformed, adgList, adgCompanyId, pubId, demoUpdate);
                }

                if (!foundCompanyMatch && matchEmails.Count > 0)
                {
                    SearchAndAddPbg300SurveyByEmail(subscriberTransformed, matchEmails, adgList, adgCompanyId, pubId, demoUpdate);
                }

                if (domainList.Count > 0)
                {
                    SearchAndAddBdcg300ByEmailAndDomain(subscriberTransformed, domainList, adgList, adgDomainId, pubId, demoUpdate);
                }
            }

            return subscribers;
        }

        private static void SearchAndAddBdcg300ByEmailAndDomain(ITransformedSubscriber subscriberTransformed,
            List<AdHocDimension> domainList,
            List<AdHocDimensionGroup> adgList,
            int adgDomainId,
            int pubId,
            Code demoUpdate)
        {
            if (string.IsNullOrWhiteSpace(subscriberTransformed.Email) ||
                !domainList.Any(x => subscriberTransformed.Email.EndsWith(x.MatchValue, StringComparison.OrdinalIgnoreCase))) 
            {
                return;
            }

            var demographicTransformed = new SubscriberDemographicTransformed
            {
                CreatedByUserID = subscriberTransformed.CreatedByUserID,
                DateCreated = DateTime.Now,
                MAFField = adgList.Single(x => x.AdHocDimensionGroupId == adgDomainId).CreatedDimension,
                NotExists = false,
                PubID = pubId,
                SORecordIdentifier = subscriberTransformed.SORecordIdentifier,
                STRecordIdentifier = subscriberTransformed.STRecordIdentifier,
                Value = BDCG300Code,
                DemographicUpdateCodeId = demoUpdate.CodeId,
                IsAdhoc = true
            };
            subscriberTransformed.DemographicTransformedList.Add(demographicTransformed);
        }

        private static void SearchAndAddPbg300SurveyByEmail(
            SubscriberTransformed subscriberTransformed,
            HashSet<string> matchEmails,
            List<AdHocDimensionGroup> adgList,
            int adgCompanyId,
            int pubId,
            Code demoUpdate)
        {
            if (!IsCorporateEmail(subscriberTransformed))
            {
                return;
            }

            var profileEmailArray = subscriberTransformed.Email.Split('@');
            if (profileEmailArray.Length != 2 || string.IsNullOrWhiteSpace(profileEmailArray[1]))
            {
                return;
            }

            if (!matchEmails.Any(x => profileEmailArray[1].Equals(x, StringComparison.CurrentCultureIgnoreCase)))
            {
                return;
            }

            var createdDimension = adgList.Single(x => x.AdHocDimensionGroupId == adgCompanyId).CreatedDimension;
            var demographicTransformed = new SubscriberDemographicTransformed
            {
                CreatedByUserID = subscriberTransformed.CreatedByUserID,
                DateCreated = DateTime.Now,
                MAFField = createdDimension,
                NotExists = false,
                PubID = pubId,
                SORecordIdentifier = subscriberTransformed.SORecordIdentifier,
                STRecordIdentifier = subscriberTransformed.STRecordIdentifier,
                Value = PBG300Code,
                DemographicUpdateCodeId = demoUpdate.CodeId,
                IsAdhoc = true
            };
            subscriberTransformed.DemographicTransformedList.Add(demographicTransformed);
        }

        private static bool IsCorporateEmail(ISubscriber subscriberTransformed)
        {
            return !string.IsNullOrWhiteSpace(subscriberTransformed.Email) &&
                   (!subscriberTransformed.Email.EndsWith("gmail.com", StringComparison.OrdinalIgnoreCase) ||
                    !subscriberTransformed.Email.EndsWith("msn.com", StringComparison.OrdinalIgnoreCase) ||
                    !subscriberTransformed.Email.EndsWith("yahoo.com", StringComparison.OrdinalIgnoreCase) ||
                    !subscriberTransformed.Email.EndsWith("aol.com", StringComparison.OrdinalIgnoreCase) ||
                    !subscriberTransformed.Email.EndsWith("comcast.com", StringComparison.OrdinalIgnoreCase) ||
                    !subscriberTransformed.Email.EndsWith("hotmail.com", StringComparison.OrdinalIgnoreCase));
        }

        private static bool SeachAndAddPBG300SurveyByCompany(
            HashSet<string> matchCompanies,
            FuzzySearch fuzzySearch,
            SubscriberTransformed subscriberTransformed,
            List<AdHocDimensionGroup> adgList,
            int adgCompanyId,
            int pubId,
            Code demoUpdate)
        {
            if (matchCompanies.Any(x => fuzzySearch.Search(subscriberTransformed.Company, x) >= 80))
            {
                var mafField = adgList.Single(x => x.AdHocDimensionGroupId == adgCompanyId).CreatedDimension;
                var sdo = new SubscriberDemographicTransformed
                {
                    CreatedByUserID = subscriberTransformed.CreatedByUserID,
                    DateCreated = DateTime.Now,
                    MAFField = mafField,
                    NotExists = false,
                    PubID = pubId,
                    SORecordIdentifier = subscriberTransformed.SORecordIdentifier,
                    STRecordIdentifier = subscriberTransformed.STRecordIdentifier,
                    Value = PBG300Code,
                    DemographicUpdateCodeId = demoUpdate.CodeId,
                    IsAdhoc = true
                };

                subscriberTransformed.DemographicTransformedList.Add(sdo);
                return true;
            }

            return false;
        }

        private static void InitCompanyAndDomains(List<AdHocDimensionGroup> adgList, out int adgCompanyId, out int adgDomainId, out List<AdHocDimension> domainList)
        {
            var domainId = 0;
            adgCompanyId = 0;
            adgDomainId = 0;
            domainList = new List<AdHocDimension>();

            if (adgList == null)
            {
                return;
            }

            if (adgList.Any(x => x.AdHocDimensionGroupName.Equals(CompanySurveyDimGroup)))
            {
                adgCompanyId = adgList.First(x => x.AdHocDimensionGroupName.Equals(CompanySurveyDimGroup))
                    .AdHocDimensionGroupId;
            }

            if (adgList.Any(x => x.AdHocDimensionGroupName.Equals(DomainsSurveyDimGroup)))
            {
                adgDomainId = adgList.First(x => x.AdHocDimensionGroupName.Equals(DomainsSurveyDimGroup))
                    .AdHocDimensionGroupId;
            }

            if (domainId > 0)
            {
                domainList = adgList.Single(x => x.AdHocDimensionGroupId == domainId).AdHocDimensions;
            }
        }

        private static void InitMatchTokens(HashSet<string> matchCompanies, HashSet<string> matchEmails)
        {
            foreach (var ad in new List<AdHocDimension>())
            {
                var values = ad.MatchValue.Split(':').ToArray();
                if (values.Count() != 3)
                {
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(values[0]))
                {
                    matchCompanies.Add(values[0]);
                }

                if (!string.IsNullOrWhiteSpace(values[1]))
                {
                    matchEmails.Add(values[1]);
                }

                if (!string.IsNullOrWhiteSpace(values[2]))
                {
                    matchEmails.Add(values[2]);
                }
            }
        }

        private Code GetDemographicUpdateCode()
        {
            var demoUpdates = codeWorker.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update).Result;
            var demoUpdate = new Code();
            if (demoUpdates != null)
            {
                demoUpdate = demoUpdates.Single(x =>
                    x.CodeName.Equals(FrameworkUAD_Lookup.Enums.DemographicUpdate.Replace.ToString()));
            }

            return demoUpdate;
        }

        #region Validation Result Reports
        public StringBuilder GetReportBodyForSingleEmail(bool isKnownCustomerFileName, bool isValidFileType, bool isFileSchemaValid, FrameworkUAD.Object.ValidationResult vr)
        {
            string unexpectedColumns = vr.UnexpectedColumns != null && vr.UnexpectedColumns.Count > 0 ? string.Join(", ", vr.UnexpectedColumns) : null;
            string notFoundColumns = vr.NotFoundColumns != null && vr.NotFoundColumns.Count > 0 ? string.Join(", ", vr.NotFoundColumns) : null;
            string duplicateColumns = vr.DuplicateColumns != null && vr.DuplicateColumns.Count > 0 ? string.Join(", ", vr.DuplicateColumns) : null;
            StringBuilder clientReportToBeAppended = new StringBuilder();
            try
            {
                if (checkFile != null && vr != null && vr.ImportFile != null)
                {
                    #region valid file processed
                    FrameworkUAD.BusinessLogic.ValidationResult vrWorker = new FrameworkUAD.BusinessLogic.ValidationResult();
                    clientReportToBeAppended.Append("<b>Results for file processed.</b><br/><br/>");
                    clientReportToBeAppended.Append("Processed File Name: " + checkFile.Name + "<br/>");
                    clientReportToBeAppended.Append("Source File Name: " + sourceFile.FileName + "<br/>");
                    clientReportToBeAppended.Append("Source File Id: " + sourceFile.SourceFileID.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Is File Registered: " + BooleanExtensions.ToYesNoString(isKnownCustomerFileName) + "<br/>");
                    clientReportToBeAppended.AppendLine("Is File Type Valid: " + BooleanExtensions.ToYesNoString(isValidFileType) + "<br/>");
                    clientReportToBeAppended.AppendLine("Is File Schema Valid: " + BooleanExtensions.ToYesNoString(isFileSchemaValid) + "<br/>");
                    clientReportToBeAppended.AppendLine("Process Code: " + processCode.ToString() + "<br/><br/>");

                    //int profileIssues = eventMessage.ValidationResult.TransformedRowCount - eventMessage.ValidationResult.TotalRowCount;
                    int dimensionIssues = vr.DimensionImportErrorCount;
                    clientReportToBeAppended.AppendLine("<b>*** Import details for file ***</b><br/>");
                    clientReportToBeAppended.AppendLine("Total row count: " + vr.TotalRowCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Original row count: " + vr.OriginalRowCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Post transformations row count: " + vr.TransformedRowCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Inserted to Transformed: " + vr.ImportedRowCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Were errors encountered at the row level: " + vr.HasError.ToYesNoString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Record Error total: " + vr.RecordImportErrorCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Dimension Error total: " + vr.DimensionImportErrorCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Excluded records: " + vr.RecordImportErrorCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Dimension issues: " + dimensionIssues.ToString() + "<br/><br/>");

                    bool custErrorsAdded = false;

                    if (isKnownCustomerFileName == false)
                    {
                        clientReportToBeAppended.Append("<b>File Status: File is unknown</b><br/><br/>");
                        if (vr.HasError == true)
                        {
                            clientReportToBeAppended.AppendLine(vrWorker.GetCustomerErrorMessage(vr, sourceFile));
                            custErrorsAdded = true;
                        }
                    }

                    if (isValidFileType == false)
                    {
                        clientReportToBeAppended.Append("<b>File Status:</b> Extension type could not be processed<br/><br/>");
                        if (vr.HasError == true && custErrorsAdded == false)
                        {
                            clientReportToBeAppended.AppendLine(vrWorker.GetCustomerErrorMessage(vr, sourceFile));
                            custErrorsAdded = true;
                        }
                    }

                    if (isFileSchemaValid == false)
                    {
                        clientReportToBeAppended.Append("<b>File Status: Exceptions found</b><br/>");
                        if (!string.IsNullOrEmpty(unexpectedColumns))
                            clientReportToBeAppended.Append("<div style=\"margin-left:20px\">Unexpected Columns Found: " + unexpectedColumns + "</div><br/>");
                        if (!string.IsNullOrEmpty(notFoundColumns))
                            clientReportToBeAppended.Append("<div style=\"margin-left:20px\">Columns Not Found: " + notFoundColumns + "</div><br/>");
                        if (!string.IsNullOrEmpty(duplicateColumns))
                            clientReportToBeAppended.Append("<div style=\"margin-left:20px\">Duplicate Columns: " + duplicateColumns + "</div><br/>");
                        if (vr.HasError == true && custErrorsAdded == false)
                        {
                            clientReportToBeAppended.AppendLine(vrWorker.GetCustomerErrorMessage(vr, sourceFile));
                            custErrorsAdded = true;
                        }
                    }

                    if (vr.HasError == true && custErrorsAdded == false)
                    {
                        clientReportToBeAppended.AppendLine(vrWorker.GetCustomerErrorMessage(vr, sourceFile));
                        custErrorsAdded = true;
                    }
                    #endregion
                }
                else
                {
                    clientReportToBeAppended.Append("<b>An unrecognized error has occured.</b><br/><br/>");
                    clientReportToBeAppended.Append("<b>File Name:</b> " + checkFile.Name + "<br/>");
                    clientReportToBeAppended.AppendLine("<b>File Status:</b> " + BooleanExtensions.ToGoodBadString(isFileSchemaValid) + "<br/>");
                    clientReportToBeAppended.AppendLine("<b>Process Code:</b> " + processCode.ToString() + "<br/>");
                }
            }
            catch (Exception ex)
            {
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.DQM;
                if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
                    app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".GetReportBodyForSingleEmail", app, string.Empty);
            }
            return clientReportToBeAppended;
        }
        #endregion
    }
}

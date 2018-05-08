using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using ADMS.Services;
using ADMS.Services.Fakes;
using FrameworkUAD.Object;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.BusinessLogic;
using KMPlatform.Entity;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;
using Enums = FrameworkUAD_Lookup.Enums;
using UasEntity = FrameworkUAS.Entity;
using static UAS.UnitTests.ADMS.Services.Validator.Common.Constants;
using UAD_BusinessLogic = FrameworkUAD_Lookup.BusinessLogic;

namespace UAS.UnitTests.AMS_Operations
{
    /// <summary>
    ///     Test Data for <see cref="TransformImportFileData"/> Method
    /// </summary>
    public partial class FileValidatorTest
    {
        private ImportFile dataIV;
        private ServiceFeature serviceFeature;
        private Code dbFileType;
        private List<UasEntity.Transformation> allTransformations;

        private List<UasEntity.TransformAssign> assignMaps;
        private List<UasEntity.TransformDataMap> dataMaps;
        private List<UasEntity.TransformJoin> joinMaps;
        private List<UasEntity.TransformSplit> splitMaps;
        private List<UasEntity.TransformSplitTrans> splitTransMaps;

        private UasEntity.Transformation dataMapTransformation;
        private Dictionary<int, string> clientPubCodes;

        protected override List<UasEntity.TransformAssign> SelectSourceFileId(TransformAssign tr, int id)
        {
            base.SelectSourceFileId(tr, id);

            return assignMaps;
        }

        protected override List<UasEntity.TransformDataMap> SelectSourceFileId(TransformDataMap tr, int id)
        {
            base.SelectSourceFileId(tr, id);

            return dataMaps;
        }

        protected override List<UasEntity.TransformJoin> SelectSourceFileId(TransformJoin tr, int id)
        {
            base.SelectSourceFileId(tr, id);

            return joinMaps;
        }

        protected override List<UasEntity.TransformSplit> SelectSourceFileId(TransformSplit tr, int id)
        {
            base.SelectSourceFileId(tr, id);

            return splitMaps;
        }

        protected override List<UasEntity.TransformSplitTrans> SelectSourceFileId(TransformSplitTrans tr, int id)
        {
            base.SelectSourceFileId(tr, id);

            return splitTransMaps;
        }

        protected override List<UasEntity.TransformDataMap> Select(TransformDataMap tr)
        {
            base.Select(tr);

            return dataMaps;
        }

        protected override List<UasEntity.TransformSplit> Select(TransformSplit tr)
        {
            base.Select(tr);

            return splitMaps;
        }

        protected override List<Code> Select(FrameworkUAD_Lookup.BusinessLogic.Code businessCode,
            Enums.CodeType codeType)
        {
            base.Select(businessCode, codeType);

            return new List<Code>
            {
                new Code {CodeTypeId = (int) codeType, CodeName = "Data Mapping", CodeId = TransformDataMappingId},
                new Code {CodeTypeId = (int) codeType, CodeName = "Join Columns", CodeId = TransformJoinId},
                new Code {CodeTypeId = (int) codeType, CodeName = "Split Into Rows", CodeId = TransformSplitId},
                new Code {CodeTypeId = (int) codeType, CodeName = "Assign Value", CodeId = TransformAssignId},
                new Code {CodeTypeId = (int) codeType, CodeName = "Split Transform", CodeId = TransformSplitTransId}
            };
        }

        protected override List<UasEntity.Transformation> Select(Transformation transformation,
            int clientId, int sourceFileId, bool includeCustomProperties)
        {
            base.Select(transformation, clientId, sourceFileId, includeCustomProperties);

            return allTransformations;
        }

        private void Initialize()
        {
            ProcessCode = "process_code";

            client.ClientID = ClientId;
            dataIV = new ImportFile
            {
                ClientId = ClientId,
                ProcessCode = ProcessCode,
                SourceFileId = SourceFileId
            };

            clientPubCodes = new Dictionary<int, string>
            {
                {TransformDataMappingId, IncomingField},
                {TransformJoinId, "pubcode"},
                {TransformSplitId, "non-pub-code"},
                {TransformAssignId, "non-pub-code-incoming-field"},
                {TransformSplitTransId, "multi-pubcode"},
                {6, "multi-non-pub-code"}
            };

            SetAllTransformations();
            serviceFeature = typeof(ServiceFeature).CreateInstance();
            serviceFeature.SFName = "Data Compare";
            dbFileType = new Code();

            UasEntity.FieldMapping pubCodeFieldMapping = typeof(UasEntity.FieldMapping).CreateInstance();
            pubCodeFieldMapping.SourceFileID = SourceFileId;
            pubCodeFieldMapping.MAFField = "pubcode";
            pubCodeFieldMapping.IncomingField = "pub-code--incoming-field";
            pubCodeFieldMapping.FieldMappingID = FieldMappingId;
            pubCodeFieldMapping.FieldMappingTypeID = FieldMappingId;

            sourceFile = new UasEntity.SourceFile
            {
                ClientID = ClientId,
                SourceFileID = SourceFileId,
                FieldMappings = new HashSet<UasEntity.FieldMapping>(new List<UasEntity.FieldMapping>
                {
                    pubCodeFieldMapping
                })
            };

            ShimServiceBase.AllInstances.clientPubCodesGet = ClientPubCodesGet;
        }

        private void SetPrivateFields()
        {
            validator.SetField(nameof(dataIV), dataIV);
            validator.SetField(nameof(serviceFeature), serviceFeature);
            validator.SetField(nameof(sourceFile), sourceFile);
            validator.SetField(nameof(dbFileType), dbFileType);
            validator.SetField(nameof(client), client);
            validator.SetField("codeWorker", new UAD_BusinessLogic.Code());
            validator.SetField(nameof(clientPubCodes), clientPubCodes);
        }

        private void SetAllTransformations()
        {
            dataMapTransformation = typeof(UasEntity.Transformation).CreateInstance();
            UasEntity.TransformationFieldMap transformationFieldMap =
                typeof(UasEntity.TransformationFieldMap).CreateInstance();
            transformationFieldMap.IsActive = true;
            transformationFieldMap.SourceFileID = SourceFileId;

            dataMapTransformation.IsActive = true;
            dataMapTransformation.TransformationID = TransformDataMappingId;
            dataMapTransformation.TransformationTypeID = TransformDataMappingId;
            dataMapTransformation.FieldMap =
                new HashSet<UasEntity.TransformationFieldMap>(
                    new List<UasEntity.TransformationFieldMap> {transformationFieldMap});
            dataMapTransformation.MapsPubCode = true;
            dataMapTransformation.ClientID = ClientId;
            dataMapTransformation.LastStepDataMap = false;

            allTransformations = new List<UasEntity.Transformation>
            {
                new UasEntity.Transformation {TransformationID = TransformJoinId},
                new UasEntity.Transformation {TransformationID = TransformSplitId},
                new UasEntity.Transformation {TransformationID = TransformAssignId},
                new UasEntity.Transformation {TransformationID = TransformSplitTransId},
                new UasEntity.Transformation {TransformationID = 6}
            };

            for (var index = 0; index < allTransformations.Count; index++)
            {
                UasEntity.Transformation item = allTransformations[index];
                item.ClientID = ClientId;
                item.IsActive = true;
                item.TransformationTypeID = index + TransformJoinId;
                item.TransformationName = $"Name_{index}";
                item.TransformationDescription = $"Description_{index}";
                item.FieldMap =
                    new HashSet<UasEntity.TransformationFieldMap>(
                        new List<UasEntity.TransformationFieldMap> {transformationFieldMap});
            }

            allTransformations.Last().IsActive = false;
            allTransformations.Last().FieldMap = new HashSet<UasEntity.TransformationFieldMap>();
            assignMaps = new List<UasEntity.TransformAssign>();
            joinMaps = new List<UasEntity.TransformJoin>();
            splitTransMaps = new List<UasEntity.TransformSplitTrans>();
            splitMaps = new List<UasEntity.TransformSplit>();
            dataMaps = new List<UasEntity.TransformDataMap>();
        }

        private Dictionary<int, string> ClientPubCodesGet(ServiceBase serviceBase)
        {
            return clientPubCodes;
        }

        private void SetupForAssignTransform(string newRowValue)
        {
            UasEntity.Transformation transformation =
                allTransformations.First(x => x.TransformationTypeID == TransformAssignId);
            transformation.FieldMap.First().FieldMappingID = FieldMappingId;

            assignMaps.Add(new UasEntity.TransformAssign
            {
                TransformAssignID = TransformAssignId,
                TransformationID = TransformAssignId,
                IsActive = true,
                Value = newRowValue,
                HasPubID = false
            });
        }

        private void SetupFieldMapping(string mafField = Constants.PubCode, string incomingField = IncomingField,
            string multiMapField = "multi-map-field")
        {
            UasEntity.FieldMapping fieldMapping = sourceFile.FieldMappings.First();
            UasEntity.FieldMapping fieldMapping2 = typeof(UasEntity.FieldMapping).CreateInstance();

            fieldMapping.MAFField = mafField;
            fieldMapping.IncomingField = incomingField;
            fieldMapping.IsNonFileColumn = true;

            fieldMapping2.MAFField = $"{mafField}-2";
            fieldMapping2.IncomingField = $"{incomingField}-2";
            fieldMapping2.IsNonFileColumn = false;
            fieldMapping2.FieldMappingID = FieldMappingId + 1;

            sourceFile.FieldMappings.Add(fieldMapping2);

            UasEntity.FieldMultiMap multiFieldMap = typeof(UasEntity.FieldMultiMap).CreateInstance();
            UasEntity.FieldMultiMap multiFieldMap2 = typeof(UasEntity.FieldMultiMap).CreateInstance();

            multiFieldMap.MAFField = $"multi-{mafField}";
            multiFieldMap2.MAFField = multiMapField;

            fieldMapping.FieldMultiMappings = new HashSet<UasEntity.FieldMultiMap>(new List<UasEntity.FieldMultiMap>
            {
                multiFieldMap,
                multiFieldMap2
            });

            fieldMapping2.FieldMultiMappings = new HashSet<UasEntity.FieldMultiMap>
            {
                multiFieldMap
            };

            var dictionary = new StringDictionary();
            if (!string.IsNullOrEmpty(mafField))
            {
                dictionary.Add(mafField, mafField);
                dictionary.Add(multiFieldMap.MAFField, multiFieldMap.MAFField);
            }

            if (!string.IsNullOrEmpty(incomingField))
            {
                dictionary.Add(incomingField, incomingField);
            }

            dataIV.DataOriginal.Add(TransformDataMappingId, dictionary);
        }

        private void SetupForSplitTransformation(string delimiter, string matchType, string rowData, string desiredData)
        {
            UasEntity.Transformation transformation =
                allTransformations.First(x => x.TransformationTypeID == TransformSplitTransId);
            transformation.FieldMap.First().FieldMappingID = FieldMappingId;

            UasEntity.TransformSplitTrans transformSplitTran = typeof(UasEntity.TransformSplitTrans).CreateInstance();
            transformSplitTran.Column = Constants.PubCode;
            transformSplitTran.IsActive = true;
            transformSplitTran.TransformationID = TransformSplitTransId;

            UasEntity.TransformDataMap transformDataMap = typeof(UasEntity.TransformDataMap).CreateInstance();
            transformDataMap.IsActive = true;
            transformDataMap.MatchType = matchType;
            transformDataMap.TransformationID = transformSplitTran.DataMapID;
            transformDataMap.SourceData =
                matchType == Enums.MatchTypes.Find_and_Replace.ToString() ? $"{rowData}FAndR" : desiredData.Trim();
            transformDataMap.DesiredData = desiredData;

            splitMaps.Add(new UasEntity.TransformSplit
            {
                TransformationID = transformSplitTran.SplitBeforeID,
                Delimiter = delimiter,
                IsActive = true,
                TransformSplitID = TransformSplitId
            });

            splitMaps.Add(new UasEntity.TransformSplit
            {
                TransformationID = transformSplitTran.SplitAfterID,
                Delimiter = delimiter,
                IsActive = true,
                TransformSplitID = TransformSplitId
            });

            dataMaps.Add(transformDataMap);
            splitTransMaps.Add(transformSplitTran);
            dataIV.DataOriginal[TransformDataMappingId][Constants.PubCode] = rowData;
        }

        private void SetupForDataMapTransformation(string matchType, string rowData, string desiredData)
        {
            UasEntity.TransformationFieldMap map = dataMapTransformation.FieldMap.First();
            map.SourceFileID = SourceFileId;
            map.FieldMappingID = FieldMappingId;

            sourceFile.FieldMappings.First().MAFField = "pubcode";

            UasEntity.TransformDataMap transformDataMap = typeof(UasEntity.TransformDataMap).CreateInstance();
            transformDataMap.IsActive = true;
            transformDataMap.MatchType = matchType;
            transformDataMap.TransformationID = TransformDataMappingId;
            transformDataMap.SourceData =
                matchType == Enums.MatchTypes.Find_and_Replace.ToString() ? $"{rowData}FAndR" : desiredData.Trim();
            transformDataMap.DesiredData = desiredData;

            dataMaps.Add(transformDataMap);

            allTransformations.Add(dataMapTransformation);
            dataIV.DataOriginal[TransformDataMappingId][IncomingField] = rowData;
        }

        private void SetupForTransformByPubCode(string delimiter, string matchType, string rowData, string desiredData)
        {
            serviceFeature.SFName = string.Empty;
            SetupFieldMapping();
            int mapPubId = clientPubCodes.Keys.Count + 1;

            UasEntity.TransformationPubMap pubMap = SetupPubMap(mapPubId);

            dataMapTransformation.MapsPubCode = false;
            clientPubCodes.Add(mapPubId, rowData);
            SetupForAssignTransform(rowData);
            SetupForSplitTransformation(delimiter, matchType, rowData, desiredData);
            SetupForDataMapTransformation(matchType, rowData, desiredData);

            allTransformations.ForEach(x =>
                x.PubMap = new HashSet<UasEntity.TransformationPubMap>(
                    new List<UasEntity.TransformationPubMap> {pubMap}));

            assignMaps.ForEach(x => x.PubID = mapPubId);
            dataMaps.ForEach(x => x.PubID = mapPubId);
            splitMaps.ForEach(x => x.TransformationID = TransformSplitId);
        }

        private void SetupForJoinTransform(string matchType)
        {
            string rowData = "Numbers|1|2,3";
            string desiredData = " 1|2 ";
            string delimiter = "comma";
            serviceFeature.SFName = string.Empty;
            dataMapTransformation.LastStepDataMap = true;
            SetupFieldMapping();
            int mapPubId = clientPubCodes.Keys.Count + 1;

            UasEntity.TransformationPubMap pubMap = SetupPubMap(mapPubId);
            UasEntity.TransformationPubMap pubMap2 = SetupPubMap(mapPubId + 1);

            dataMapTransformation.MapsPubCode = false;
            clientPubCodes.Add(mapPubId, rowData);
            clientPubCodes.Add(mapPubId + 1, "Numbers|1|2");
            SetupForAssignTransform(rowData);
            SetupForSplitTransformation(delimiter, matchType, rowData, desiredData);
            SetupForDataMapTransformation(matchType, rowData, desiredData);

            dataIV.DataOriginal[TransformDataMappingId][IncomingField] = desiredData;

            UasEntity.TransformJoin transformJoin = typeof(UasEntity.TransformJoin).CreateInstance();
            transformJoin.TransformationID = TransformJoinId;
            transformJoin.IsActive = true;
            transformJoin.ColumnsToJoin = IncomingField;
            joinMaps.Add(transformJoin);

            allTransformations.ForEach(x =>
                x.PubMap = new HashSet<UasEntity.TransformationPubMap>(
                    new List<UasEntity.TransformationPubMap> {pubMap, pubMap2}));

            assignMaps.ForEach(x => x.PubID = mapPubId + 1);
            dataMaps.ForEach(x => x.PubID = mapPubId + 1);
            joinMaps.ForEach(x => x.Delimiter = ",");
            splitMaps.ForEach(x => x.TransformationID = TransformSplitId);
        }

        private UasEntity.TransformationPubMap SetupPubMap(int mapPubId)
        {
            UasEntity.TransformationPubMap pubMap = typeof(UasEntity.TransformationPubMap).CreateInstance();
            pubMap.PubID = mapPubId;
            pubMap.IsActive = true;
            pubMap.TransformationID = TransformDataMappingId;
            return pubMap;
        }
    }
}
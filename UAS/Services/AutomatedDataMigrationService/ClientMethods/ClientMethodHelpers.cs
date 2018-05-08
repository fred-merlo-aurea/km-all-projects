using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ADMS.ClientMethods.Common;
using Core.ADMS.Events;
using Core_AMS.Utilities;
using FrameworkUAS.Entity;
using KM.Common;
using KM.Common.Import;
using KMPlatform.Entity;
using BusinessAdHocDimensionGroup = FrameworkUAS.BusinessLogic.AdHocDimensionGroup;
using ClientEntity = KMPlatform.Entity.Client;
using FtpFunctions = KM.Common.Functions.FtpFunctions;
using ImportErrorEntity = FrameworkUAD.Entity.ImportError;
using ImportVesselBusiness = FrameworkUAD.BusinessLogic.ImportVessel;

namespace ADMS.ClientMethods
{

    internal class ClientMethodHelpers
    {
        private const int DefaultOperationOrder = 1;

        public static AdHocDimensionGroup ReadAndFillAgGroupAndTableReturnGroup(FillAgGroupAndTableArgs fillAgGroupAndTableArgs)
        {
            return IntReadAndFillAgGroupAndTable(fillAgGroupAndTableArgs)
                .Select(
                fillAgGroupAndTableArgs.Client.ClientID,
                fillAgGroupAndTableArgs.SourceFileId,
                fillAgGroupAndTableArgs.AdHocDimensionGroupName,
                true);
        }

        public static void ReadAndFillAgGroupAndTable(FillAgGroupAndTableArgs fillAgGroupAndTableArgs)
        {
            IntReadAndFillAgGroupAndTable(fillAgGroupAndTableArgs);
        }

        public static BusinessAdHocDimensionGroup FillAgGroupAndTable(
            FillAgGroupAndTableArgs fillAgGroupAndTableArgs)
        {
            var agWorker = new BusinessAdHocDimensionGroup();
            var listItems = FillAdHocDimensionGroup(fillAgGroupAndTableArgs, ref agWorker);

            fillAgGroupAndTableArgs.AhdData.SaveBulkSqlInsert(listItems);
            return new BusinessAdHocDimensionGroup();
        }

        public static List<AdHocDimension> FillAdHocDimensionGroup(
            FillAgGroupAndTableArgs fillAgGroupAndTableArgs,
            ref BusinessAdHocDimensionGroup agWorker)
        {
            var entityGroup = CreateDimensionGroup(fillAgGroupAndTableArgs, agWorker);
            var resultList = new List<AdHocDimension>();
            var rows = fillAgGroupAndTableArgs.Rows ??
                       fillAgGroupAndTableArgs.Dt.AsEnumerable().OfType<DataRow>();
            foreach (var dr in rows)
            {
                CreateDimensionValue(fillAgGroupAndTableArgs, entityGroup, resultList, dr);
            }

            return resultList;
        }

        public static void CreateDimensionValue(FillAgGroupAndTableArgs fillAgGroupAndTableArgs, AdHocDimensionGroup entityGroup, List<AdHocDimension> resultList, DataRow dr)
        {
            var ahd = new AdHocDimension
            {
                AdHocDimensionGroupId = entityGroup.AdHocDimensionGroupId,
                CreatedByUserID = 1,
                DateCreated = DateTime.Now,
                DateUpdated = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                DimensionValue = fillAgGroupAndTableArgs.DimensionValue
                                                 ?? dr[fillAgGroupAndTableArgs.DimensionValueField].ToString(),
                IsActive = fillAgGroupAndTableArgs.IsActive,
                MatchValue = fillAgGroupAndTableArgs.MatchValueFunc!=null
                                    ? fillAgGroupAndTableArgs.MatchValueFunc(dr)
                                    : dr[fillAgGroupAndTableArgs.MatchValueField].ToString(),
                Operator = fillAgGroupAndTableArgs.DimensionOperator,
                UpdateUAD = fillAgGroupAndTableArgs.UpdateUAD,
                UADLastUpdatedDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue
            };
            resultList.Add(ahd);
        }

        public static AdHocDimensionGroup CreateDimensionGroup(FillAgGroupAndTableArgs fillAgGroupAndTableArgs,
            BusinessAdHocDimensionGroup agWorker)
        {
            var entityGroup = agWorker.Select(
                fillAgGroupAndTableArgs.Client.ClientID,
                fillAgGroupAndTableArgs.SourceFileId,
                fillAgGroupAndTableArgs.AdHocDimensionGroupName,
                true);
            if (entityGroup != null)
            {
                return entityGroup;
            }

            entityGroup = new AdHocDimensionGroup
            {
                AdHocDimensionGroupName = fillAgGroupAndTableArgs.AdHocDimensionGroupName,
                ClientID = fillAgGroupAndTableArgs.Client.ClientID,
                SourceFileID = fillAgGroupAndTableArgs.SourceFileId,
                IsActive = fillAgGroupAndTableArgs.IsActive,
                OrderOfOperation = DefaultOperationOrder,
                StandardField = fillAgGroupAndTableArgs.StandardField,
                CreatedDimension = fillAgGroupAndTableArgs.CreatedDimension,
                IsPubcodeSpecific = fillAgGroupAndTableArgs.IsPubcodeSpecific
            };

            if (fillAgGroupAndTableArgs.DefaultValue != null)
            {
                entityGroup.DefaultValue = fillAgGroupAndTableArgs.DefaultValue;
            }

            agWorker.Save(entityGroup);
            entityGroup = agWorker.Select(
                fillAgGroupAndTableArgs.Client.ClientID,
                fillAgGroupAndTableArgs.SourceFileId,
                fillAgGroupAndTableArgs.AdHocDimensionGroupName,
                true);

            fillAgGroupAndTableArgs.AdditionalInitFunction?.Invoke(entityGroup);

            return entityGroup;
        }

        public static void SaveAdHocDimensions(
            DataTable dt, 
            AdHocDimensionGroup adgCode, 
            FrameworkUAS.BusinessLogic.AdHocDimension ahdData,
            string dimensionValueField,
            string matchValueField)
        {
            if (dt == null)
            {
                throw new ArgumentNullException(nameof(dt));
            }

            var codes = new List<AdHocDimension>();
            foreach (DataRow dr in dt.Rows)
            {
                var ahd = new AdHocDimension
                              {
                                  AdHocDimensionGroupId = adgCode.AdHocDimensionGroupId,
                                  CreatedByUserID = 1,
                                  DateCreated = DateTime.Now,
                                  DateUpdated = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                  DimensionValue = dr[dimensionValueField].ToString(),
                                  IsActive = true,
                                  MatchValue = dr[matchValueField].ToString(),
                                  Operator = ClientSpecialCommon.EqualOperation,
                                  UpdateUAD = true,
                                  UADLastUpdatedDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue
                              };
                codes.Add(ahd);
            }

            ahdData.SaveBulkSqlInsert(codes);
        }

        public static AdHocDimensionGroup CreateAdhocGroupCode(CreateAdhocGroupCodeArgs createAdhocGroupCodeArgs)
        {
            if (createAdhocGroupCodeArgs == null)
            {
                throw new ArgumentNullException(nameof(createAdhocGroupCodeArgs));
            }

            if (createAdhocGroupCodeArgs.AgWorker == null)
            {
                throw new ArgumentNullException(nameof(createAdhocGroupCodeArgs.AgWorker));
            }

            var adgCode = createAdhocGroupCodeArgs.AgWorker.Select(
                createAdhocGroupCodeArgs.ClientId, 
                createAdhocGroupCodeArgs.SourceFileId, 
                createAdhocGroupCodeArgs.AdHocDimensionGroupName, true);

            if (adgCode == null)
            {
                adgCode = new AdHocDimensionGroup
                              {
                                  AdHocDimensionGroupName = createAdhocGroupCodeArgs.AdHocDimensionGroupName,
                                  ClientID = createAdhocGroupCodeArgs.ClientId,
                                  SourceFileID = createAdhocGroupCodeArgs.SourceFileId,
                                  IsActive = true,
                                  OrderOfOperation = createAdhocGroupCodeArgs.OrderOfOperation,
                                  StandardField = createAdhocGroupCodeArgs.StandardField,
                                  CreatedDimension = createAdhocGroupCodeArgs.CreatedDimension,
                                  IsPubcodeSpecific = false
                              };
                createAdhocGroupCodeArgs.AgWorker.Save(adgCode);

                adgCode = createAdhocGroupCodeArgs.AgWorker.Select(
                    createAdhocGroupCodeArgs.ClientId,
                    createAdhocGroupCodeArgs.EvtSourceFileId,
                    createAdhocGroupCodeArgs.AdHocDimensionGroupName,
                    true);
            }

            return adgCode;
        }

        public static IList<FileInfo> GetFilesListWithExistenceCheck(string filePath, IEnumerable<string> fileNames)
        {
            var directoryInfo = new DirectoryInfo(filePath);
            if (!directoryInfo.Exists)
            {
                return new List<FileInfo>();
            }

            var fileList = directoryInfo.GetFiles().ToList();
            var filesExist = fileNames.Aggregate(
                true,
                (current, name) => current &&
                                   fileList.Exists(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)));

            return filesExist ? fileList : new List<FileInfo>();
        }

        private static bool IsSourceFileDefinedInplace(FillAgGroupAndTableArgs fillAgGroupAndTableArgs)
        {
            return fillAgGroupAndTableArgs.SourceFileId != 0 ||
                   fillAgGroupAndTableArgs.EventMessage == null ||
                   fillAgGroupAndTableArgs.EventMessage.SourceFile == null;
        }

        private static BusinessAdHocDimensionGroup IntReadAndFillAgGroupAndTable(FillAgGroupAndTableArgs fillAgGroupAndTableArgs)
        {
            if (fillAgGroupAndTableArgs == null)
            {
                throw new ArgumentNullException(nameof(fillAgGroupAndTableArgs));
            }

            //check for existing data in AdHocDimension - if data exist - delete - then insert new data
            var ahdData = new FrameworkUAS.BusinessLogic.AdHocDimension();
            ahdData.Delete(fillAgGroupAndTableArgs.SourceFileId);
            var fileWorker = new FileWorker();
            if (fillAgGroupAndTableArgs.Dt == null)
            {
                FileConfiguration fileConfiguration = null;
                if (!string.IsNullOrWhiteSpace(fillAgGroupAndTableArgs.FileExtension) ||
                    !string.IsNullOrWhiteSpace(fillAgGroupAndTableArgs.FileColumnDelimiter))
                {
                    fileConfiguration = new FileConfiguration()
                    {
                        FileExtension = fillAgGroupAndTableArgs.FileExtension,
                        FileColumnDelimiter = fillAgGroupAndTableArgs.FileColumnDelimiter
                    };
                }

                fillAgGroupAndTableArgs.Dt = fileWorker.GetData(fillAgGroupAndTableArgs.EventMessage.ImportFile, fileConfiguration);
            }

            fillAgGroupAndTableArgs.AhdData = fillAgGroupAndTableArgs.AhdData ?? ahdData;
            fillAgGroupAndTableArgs.Client = fillAgGroupAndTableArgs.Client ?? fillAgGroupAndTableArgs.EventMessage.Client;
            fillAgGroupAndTableArgs.SourceFileId = IsSourceFileDefinedInplace(fillAgGroupAndTableArgs)
                ? fillAgGroupAndTableArgs.SourceFileId
                : fillAgGroupAndTableArgs.EventMessage.SourceFile.SourceFileID;

            var tmp = FillAgGroupAndTable(fillAgGroupAndTableArgs);
            return tmp;
        }

        public static void AddRow(
            string pubCode, 
            int position, 
            IDictionary<int, DataRow> newRows, 
            DataTable workDataTable, 
            DataRow originalRows)
        {
            Guard.NotNull(newRows, nameof(newRows));
            Guard.NotNull(workDataTable, nameof(workDataTable));
            Guard.NotNull(originalRows, nameof(originalRows));

            var newRow = workDataTable.NewRow();
            newRow.ItemArray = originalRows.ItemArray;
            newRow.SetField(Consts.PubCodeKey, pubCode);
            newRows.Add(position, newRow);
        }

        public static void AddPubCode(string columnValue, IList<string> pubCodes, string value)
        {
            Guard.NotNull(pubCodes, nameof(pubCodes));

            if (columnValue.Trim() == Consts.YesCode)
            {
                pubCodes.Add(value);
            }
        }

        public static FileConfiguration CreateFileConfiguration(int? columnCount, string headers, string fileDelimiter, string fileExtension, bool isQuoteEncapsulated)
        {
            var fileConfiguration = new FileConfiguration
            {
                FileColumnDelimiter = fileDelimiter,
                FileExtension = fileExtension,
                IsQuoteEncapsulated = isQuoteEncapsulated
            };

            if (columnCount != null)
            {
                fileConfiguration.ColumnCount = columnCount.Value;
            }

            if (!string.IsNullOrWhiteSpace(headers))
            {
                fileConfiguration.ColumnHeaders = headers;
            }

            return fileConfiguration;
        }

        public static void DeleteExistingFiles(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static StringBuilder CreateEmailHeader(
            ClientEntity clientEntity, 
            FileConfiguration fileConfigurationAccess, 
            DataTable dataTableAccess)
        {
            Guard.NotNull(clientEntity, nameof(clientEntity));
            Guard.NotNull(fileConfigurationAccess, nameof(fileConfigurationAccess));
            Guard.NotNull(dataTableAccess, nameof(dataTableAccess));

            var sbDetail = new StringBuilder();
            var headers = new StringBuilder();

            foreach (DataColumn column in dataTableAccess.Columns)
            {
                headers.Append(fileConfigurationAccess.IsQuoteEncapsulated
                    ? $"{Consts.StartQuote}{column}{Consts.FinishQuote}"
                    : $"{column},");
            }

            return sbDetail.AppendLine(headers.ToString().Trim().TrimEnd(Consts.CommaSeparator));
        }

        public static string AppendImportErrors(
            FileConfiguration fileConfigurationAccess, 
            IList<ImportErrorEntity> importErrors, 
            StringBuilder sbDetail)
        {
            Guard.NotNull(fileConfigurationAccess, nameof(fileConfigurationAccess));
            Guard.NotNull(importErrors, nameof(importErrors));
            Guard.NotNull(sbDetail, nameof(sbDetail));

            var reportBody = new StringBuilder();
            foreach (var error in importErrors)
            {
                reportBody.Append($"<br /><b>Row: {error.RowNumber}</b><br />Message: {error.ClientMessage}");
                sbDetail.AppendLine(Environment.NewLine);

                if (!string.IsNullOrWhiteSpace(error.BadDataRow))
                {
                    error.BadDataRow = error.BadDataRow.Trim().TrimEnd(Consts.CommaSeparator);
                    if (fileConfigurationAccess.IsQuoteEncapsulated)
                    {
                        var data = new StringBuilder();
                        var quoteRow = error.BadDataRow.Split(Consts.CommaSeparator);
                        foreach (var quote in quoteRow)
                        {
                            data.Append($"{Consts.StartQuote}{quote}{Consts.FinishQuote}");
                        }

                        sbDetail.AppendLine(data.ToString().Trim().TrimEnd(Consts.CommaSeparator));
                        sbDetail.AppendLine(Environment.NewLine);
                    }
                    else
                    {
                        sbDetail.AppendLine(error.BadDataRow.Trim().TrimEnd(Consts.CommaSeparator));
                        sbDetail.AppendLine(Environment.NewLine);
                    }

                    sbDetail.AppendLine(Environment.NewLine);
                }
            }
            return reportBody.ToString();
        }

        public static void EmailSender(
            ClientEntity clientEntity,
            FileMoved eventMessage,
            StringBuilder sbDetail,
            string reportBody,
            string processCode)
        {
            Guard.NotNull(clientEntity, nameof(clientEntity));
            Guard.NotNull(eventMessage, nameof(eventMessage));
            Guard.NotNull(sbDetail, nameof(sbDetail));

            var mailer = new Services.Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.Null);
            mailer.SendEmail(mailer.GetMailMessage(clientEntity,
                    reportBody,
                    eventMessage.ImportFile,
                    true,
                    true,
                    "text/html",
                    processCode,
                    eventMessage.SourceFile.SourceFileID,
                    sbDetail.ToString()),
                clientEntity);
        }

        public static DataTable ProcessImportVesselData(FileInfo fileInfo, FileConfiguration fcAccess)
        {
            Guard.NotNull(fileInfo, nameof(fileInfo));
            Guard.NotNull(fcAccess, nameof(fcAccess));

            var importVesselData = new ImportVesselBusiness();
            var fileWorker = new FileWorker();
            var totalRowCount = fileWorker.GetRowCount(fileInfo);
            var rowProcessedCount = 0;
            var firstRun = true;
            DataTable dtAccess = null;

            if (totalRowCount > 0)
            {
                while (rowProcessedCount < totalRowCount)
                {
                    var endRow = rowProcessedCount + Consts.RowBatch;
                    if (endRow > totalRowCount)
                    {
                        endRow = totalRowCount;
                    }

                    var startRow = rowProcessedCount + 1;
                    var percent = (int)Math.Round((double)(100 * startRow) / totalRowCount);
                    Console.WriteLine($"{Consts.ProcessingStatus}{startRow} to {endRow} {DateTime.Now} {percent}%");

                    var importVessel = importVesselData.GetImportVessel(fileInfo, startRow, Consts.RowBatch, fcAccess);
                    rowProcessedCount += importVessel.TotalRowCount;

                    if (firstRun)
                    {
                        dtAccess = importVessel.DataOriginal.Copy();
                        firstRun = false;
                    }
                    else
                    {
                        dtAccess.Merge(importVessel.DataOriginal);
                    }

                    importVessel.DataOriginal.Dispose();
                }
            }
            return dtAccess;
        }

        public static void FinishingUploadToFtp(Client clientEntity, string remoteFile, string localFile)
        {
            Guard.NotNull(clientEntity, nameof(clientEntity));
            Console.WriteLine($"{Consts.MessageUploadingToFtp}{DateTime.Now}");

            var clientFtp = new ClientFTP();
            var businessLogicClientFtp = new FrameworkUAS.BusinessLogic.ClientFTP();
            clientFtp = businessLogicClientFtp.SelectClient(clientEntity.ClientID).First();

            var ftp = new FtpFunctions(clientFtp.Server, clientFtp.UserName, clientFtp.Password);
            ftp.Upload($"{clientFtp.Folder}{Consts.DoubleBackwardSlash}{remoteFile}", localFile);
            Console.WriteLine($"{Consts.MessageFinishedUpload}{DateTime.Now}");
        }
    }
}

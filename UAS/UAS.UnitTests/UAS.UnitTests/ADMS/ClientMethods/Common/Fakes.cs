using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Mail;
using System.Text;
using ADMS.ClientMethods;
using ADMS.Services.Emailer.Fakes;
using ADMS.Services.Fakes;
using Core.ADMS.Events;
using Core_AMS.Utilities.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using KM.Common;
using KM.Common.Import;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using ClientEntity = KMPlatform.Entity.Client;
using ImportErrorEntity = FrameworkUAD.Entity.ImportError;
using ShimFtpFunctions = KM.Common.Functions.Fakes.ShimFtpFunctions;

namespace UAS.UnitTests.ADMS.ClientMethods.Common
{
    [ExcludeFromCodeCoverage]
    public class Fakes
    {
        public static readonly int ClientId = 10;
        public static readonly char CommaSeparator = ',';
        public static readonly string DummyName = "DummyName";
        public static readonly string NoEmailKey = "noemail";
        public static readonly string NoAdvEmailKey = "noadvemail";
        public static readonly string NoTelemktg = "notelemktg";
        public static readonly string Noroweml = "noroweml";
        public static readonly string Noroweml3 = "noroweml3";
        public static readonly string SamplePubsCodes = "Sample1, Sample2, Sample3";
        public static readonly string SampleServer = "Server";
        public static readonly string SampleUsername = "Username";
        public static readonly string SamplePassword = "Password";
        public static readonly string SampleFolder = "Folder";
        public static readonly string PubCodeKey = "PubCode";
        public static readonly string PubCodeUpperKey = "PUBCODE";
        public static readonly string TransactionIdKey = "TRANSACTIONID";
        public static readonly string StartQuote = "\"";
        public static readonly string FinishQuote = "\",";
        public static readonly string YesCode = "Y";
        public static readonly string NoCode = "N";

        public static readonly string InitialTransId = "10";
        public static readonly string DefaultTransId = "38";

        public static readonly string SnwaeCode = "SNWAE";
        public static readonly string SnwauCode = "SNWAU";
        public static readonly string SnwohCode = "SNWOH";
        public static readonly string SnwtnaeCode = "SNWTNAE";
        public static readonly string SnwtnallCode = "SNWTNALL";
        public static readonly string SnwtnauCode = "SNWTNAU";
        public static readonly string SnwtnelCode = "SNWTNEL";
        public static readonly string SnwthdCode = "SNWTNHD";
        public static readonly string SnwtnofCode = "SNWTNOF";
        public static readonly string SaeroCode = "SAERO";
        public static readonly string SautoCode = "SAUTO";
        public static readonly string SdigautoCode = "SDIGAUTO";
        public static readonly string SofhCode = "SOFH";
        public static readonly string SsaeupDomCode = "SSAEUP-DOM";

        public static readonly string Update2014Key = "2014_Update";
        public static readonly string Update2013Key = "2013_Update";
        public static readonly string Update2012Key = "2012_Update";
        public static readonly string Update2011Key = "2011_Update";
        public static readonly string Update2010Key = "2010_Update";
        public static readonly string BinderKey = "Binder";
        public static readonly string SpiralKey = "Spiral";

        protected readonly SAETB Saetb = new SAETB();
        protected readonly BriefMedia AdmsBriefMedia = new BriefMedia();
        protected readonly Vcast Vcast = new Vcast();
        protected ClientCustomProcedure ClientCustomProcedure;
        protected ClientEntity ClientEntity;
        protected DataTable DtAccess;
        protected FileConfiguration FileConfigurationAccess;
        protected FileInfo FileInfo;
        protected FileMoved EventMessage;
        protected SourceFile ClientSpecialFile;
        protected StringBuilder SbDetail;
        protected StringBuilder SbDetailResult;
        protected List<string> PubCodes;
        protected List<ImportErrorEntity> ImportErrors;

        protected string ReportBody;
        protected string ReportBodyResult;

        private IDisposable _context;

        public void SetupFakes()
        {
            _context = ShimsContext.Create();
            DtAccess = new DataTable();
            ImportErrors = null;
            SbDetailResult = new StringBuilder();
            ReportBodyResult = string.Empty;
        }

        [TearDown]
        public void CleanUp() => _context?.Dispose();

        protected virtual void ShimForFileRows(int rows)
        {
            ShimFileWorker.AllInstances.GetRowCountFileInfo = (_, importFile) => rows;
        }

        protected virtual void ShimForDataTableAcceptChanges()
        {
            ShimDataTable.AllInstances.AcceptChanges = _ => { };
        }

        protected virtual void ShimForConsoleMessage()
        {
            ShimServiceBase.AllInstances.ConsoleMessageStringStringBooleanInt32Int32 =
                (_, message, processCode, createLog, sourceField, updateByUser) => { };
        }

        protected virtual void ShimForCreateCsvFile()
        {
            ShimFileFunctions.AllInstances.CreateCSVFromDataTableDataTableStringBoolean =
                (_, workTable, fileName, deleteExists) => { };
        }

        protected virtual void ShimForClientFtp()
        {
            ShimClientFTP.AllInstances.SelectClientInt32 = (_, clientId) =>
                new List<ClientFTP>
                {
                    new ClientFTP
                    {
                        ClientID = ClientId,
                        Server = SampleServer,
                        UserName = SampleUsername,
                        Password = SamplePassword,
                        Folder = SampleFolder
                    }
                };

            ShimFtpFunctions.AllInstances.UploadStringStringBoolean = (_, remoteFile, localFile, enableSsl) => true;
        }

        protected virtual void ShimsForMailMessages()
        {
            ShimEmailer.AllInstances.GetMailMessageClientStringFileInfoBooleanBooleanStringStringInt32StringMailPriorityBooleanBoolean
                = (emailer, client, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13)
                => new MailMessage();

            ShimEmailer.AllInstances.SendEmailMailMessageClient = (_, mailer, clientEntity) => { };
        }

        protected static FileInfo CreateFileInfo(string name)
        {
            return new FileInfo(name);
        }

        protected void CreateImportErrorsList()
        {
            ImportErrors = new List<ImportErrorEntity>
            {
                new ImportErrorEntity { BadDataRow = string.Empty },
                new ImportErrorEntity
                {
                    BadDataRow = "SampleError1, SampleError2, SampleError3"
                }
            };
        }

        protected void CreateDataTableAccessForDemo()
        {
            DtAccess.Columns.Add(NoEmailKey, typeof(string));
            DtAccess.Columns.Add(NoAdvEmailKey, typeof(string));
            DtAccess.Columns.Add(NoTelemktg, typeof(string));
            DtAccess.Columns.Add(PubCodeKey, typeof(string));
            DtAccess.Columns.Add(PubCodeUpperKey, typeof(string));

            var dataRow = DtAccess.NewRow();
            dataRow[NoEmailKey] = YesCode;
            dataRow[NoAdvEmailKey] = YesCode;
            dataRow[NoTelemktg] = YesCode;
            dataRow[PubCodeKey] = SamplePubsCodes;
            dataRow[PubCodeUpperKey] = SamplePubsCodes;
            DtAccess.Rows.Add(dataRow);
        }

        protected void CreateDataTableAccessForBmAutoGenAlgorithms()
        {
            DtAccess.Columns.Add(Update2014Key, typeof(string));
            DtAccess.Columns.Add(Update2013Key, typeof(string));
            DtAccess.Columns.Add(Update2012Key, typeof(string));
            DtAccess.Columns.Add(Update2011Key, typeof(string));
            DtAccess.Columns.Add(Update2010Key, typeof(string));
            DtAccess.Columns.Add(BinderKey, typeof(string));
            DtAccess.Columns.Add(SpiralKey, typeof(string));

            var dataRow = DtAccess.NewRow();
            dataRow[Update2014Key] = YesCode;
            dataRow[Update2013Key] = YesCode;
            dataRow[Update2012Key] = YesCode;
            dataRow[Update2011Key] = YesCode;
            dataRow[Update2010Key] = YesCode;
            dataRow[BinderKey] = YesCode;
            dataRow[SpiralKey] = YesCode;
            DtAccess.Rows.Add(dataRow);
        }

        protected StringBuilder SetEmailHeaders(DataTable dataTableAccess)
        {
            var headers = new StringBuilder();
            SbDetailResult = new StringBuilder();

            foreach (DataColumn column in dataTableAccess.Columns)
            {
                headers.Append(FileConfigurationAccess.IsQuoteEncapsulated
                    ? $"{StartQuote}{column}{FinishQuote}"
                    : $"{column},");
            }

            SbDetailResult.AppendLine(headers.ToString().Trim().TrimEnd(CommaSeparator));
            return SbDetailResult;
        }

        protected void SetImportErrorsResults(FileConfiguration fileConfigurationAccess, IList<ImportErrorEntity> importErrors)
        {
            Guard.NotNull(fileConfigurationAccess, nameof(fileConfigurationAccess));
            Guard.NotNull(importErrors, nameof(importErrors));

            foreach (var error in importErrors)
            {
                ReportBodyResult += $"<br /><b>Row: {error.RowNumber}</b><br />Message: {error.ClientMessage}";
                SbDetailResult.AppendLine(Environment.NewLine);

                if (!string.IsNullOrWhiteSpace(error.BadDataRow))
                {
                    error.BadDataRow = error.BadDataRow.Trim().TrimEnd(CommaSeparator);
                    if (fileConfigurationAccess.IsQuoteEncapsulated)
                    {
                        var data = new StringBuilder();
                        var quoteRow = error.BadDataRow.Split(CommaSeparator);
                        foreach (var quote in quoteRow)
                        {
                            data.Append($"{StartQuote}{quote}{FinishQuote}");
                        }

                        SbDetailResult.AppendLine(data.ToString().Trim().TrimEnd(CommaSeparator));
                        SbDetailResult.AppendLine(Environment.NewLine);
                    }
                    else
                    {
                        SbDetailResult.AppendLine(error.BadDataRow.Trim().TrimEnd(CommaSeparator));
                        SbDetailResult.AppendLine(Environment.NewLine);
                    }

                    SbDetailResult.AppendLine(Environment.NewLine);
                }
            }
        }
    }
}

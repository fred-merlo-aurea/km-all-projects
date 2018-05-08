using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration.Fakes;
using System.Data;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ADMS.Services.Fakes;
using ADMS.Services.UAD;
using Core.ADMS.Events;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Entity;
using FrameworkUAS.Entity.Fakes;
using KM.Common.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ADMS.Services.UAD
{
    /// <summary>
    /// Unit Test for <see cref="UADProcessor"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class UADProcessorTest
    {
        private const string SampleDemoColumn = "SampleDemoColumn";
        private const string ToDataTableSubscriberFinalMethodName = "ToDataTableSubscriberFinal";
        private const string TitleField = "Title";
        private const string TitleValue1 = "SampleTitle1";
        private const string TitleValue2 = "SampleTitle2";
        private const string AccountNumberField = "AccountNumber";
        private const string SampleAccountNumber = "Account98765";        
        private const string State = "state";                
        private const string Normal = "normal";
        private const string Other = "other";       
        private const int ClientId = 10;
        private const string Complimentary = "complimentary";
        private const string ParameterOther = "other";
        private const string DataCompare = "dataCompare";
        private const string Telemarketing = "telemarketing";
        private const string Web = "web";
        private const string TwoYear = "2yr";
        private const string ThreeYear = "3yr";
        private const string ListOther = "listOther";
        private const string QuickFill = "quickFill";
        private const string PaidTransaction = "paidTransaction";
        private const string FieldUpdate = "fieldUpdate";
        private const int Id = 10;
        private const int Count = 0;
        private const int One = 1;
        private const string FtpFolder = "FtpFolder";
        private const string FileName = "fileName";
        private const string IsDemo = "IsDemo";
        private const string DemoValue = "true";
        private const string StringOne = "1";

        private UADProcessor testEntity;
        private HashSet<SubscriberFinal> subList;
        private List<string> incomingColumns;
        private List<string> tmpDemoColumns;
        private List<FieldMapping> fieldMappings;
        private PrivateObject _privateObject;
        private List<Code> codeList;
        private List<SubscriberFinal> subscriberFinals;
        private IDisposable _shimsImport;
        private IDisposable _shims;

        /// <summary>
        /// Test SetUp
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            testEntity = new UADProcessor();
            subList = new HashSet<SubscriberFinal>();
            incomingColumns = new List<string>();
            tmpDemoColumns = new List<string> { SampleDemoColumn };
            fieldMappings = new List<FieldMapping>();
        }

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
            _shimsImport?.Dispose();
        }

        /// <summary>
        /// <see cref="UADProcessor.ToDataTable_SubscriberFinal"/>
        /// </summary>
        [Test]
        public void ToDataTable_SubscriberFinal_WithOutDemographicTransformedList_ReturnsDataTableWithDefaultValues()
        {
            // Arrange
            subList = new HashSet<SubscriberFinal>
            {
                new SubscriberFinal {AccountNumber = SampleAccountNumber }
            };
            SetTestData(fieldMappings, incomingColumns);
            var parameters = new object[]
            {
                subList,
                incomingColumns,
                tmpDemoColumns,
                fieldMappings
            };

            // Act
            var resultDataTable = (DataTable)ReflectionHelper.CallMethod(
                testEntity.GetType(),
                ToDataTableSubscriberFinalMethodName,
                parameters,
                testEntity);

            // Assert
            resultDataTable.ShouldNotBeNull();
            resultDataTable.Columns.Count.ShouldBe(TypeDescriptor.GetProperties(typeof(SubscriberFinal)).Count);
            resultDataTable.Rows.Count.ShouldBe(1);
            resultDataTable.Rows[0][AccountNumberField].ShouldBeSameAs(SampleAccountNumber);
        }

        /// <summary>
        /// <see cref="UADProcessor.ToDataTable_SubscriberFinal"/>
        /// </summary>
        [Test]
        public void ToDataTable_SubscriberFinal_WithDemographicTransformedList_ReturnsDataTableWithDefaultValues()
        {
            // Arrange
            subList = new HashSet<SubscriberFinal>
            {
                new SubscriberFinal
                {
                    AccountNumber = SampleAccountNumber,
                    DemographicFinalList = new HashSet<SubscriberDemographicFinal>
                    {
                        new SubscriberDemographicFinal
                        {
                            MAFField = TitleField,
                            Value = TitleValue1
                        },
                        new SubscriberDemographicFinal
                        {
                            MAFField = TitleField,
                            Value= TitleValue2
                        },
                    }
                }
            };
            SetTestData(fieldMappings, incomingColumns);
            var parameters = new object[]
            {
                subList,
                incomingColumns,
                tmpDemoColumns,
                fieldMappings
            };

            // Act
            var resultDataTable = (DataTable)ReflectionHelper.CallMethod(
                testEntity.GetType(),
                ToDataTableSubscriberFinalMethodName,
                parameters,
                testEntity);

            // Assert
            resultDataTable.ShouldNotBeNull();
            resultDataTable.Columns.Count.ShouldBe(TypeDescriptor.GetProperties(typeof(SubscriberFinal)).Count);
            resultDataTable.Rows.Count.ShouldBe(1);
            resultDataTable.Rows[0][AccountNumberField].ShouldBe(SampleAccountNumber);
            resultDataTable.Rows[0][TitleField].ShouldBe(string.Join(",", TitleValue1, TitleValue2));
        }

        /// <summary>
        /// <see cref="UADProcessor.ToDataTable_SubscriberFinal"/>
        /// </summary>
        [Test]
        public void ToDataTable_SubscriberFinal_WithSingleDemographicTransformedList_ReturnsDataTableWithDefaultValues()
        {
            // Arrange
            subList = new HashSet<SubscriberFinal>
            {
                new SubscriberFinal
                {
                    AccountNumber = SampleAccountNumber,
                    DemographicFinalList = new HashSet<SubscriberDemographicFinal>
                    {
                        new SubscriberDemographicFinal
                        {
                            MAFField = TitleField,
                            Value = TitleValue1
                        },
                    }
                }
            };
            SetTestData(fieldMappings, incomingColumns);
            var parameters = new object[]
            {
                subList,
                incomingColumns,
                tmpDemoColumns,
                fieldMappings
            };

            // Act
            var resultDataTable = (DataTable)ReflectionHelper.CallMethod(
                testEntity.GetType(),
                ToDataTableSubscriberFinalMethodName,
                parameters,
                testEntity);

            // Assert
            resultDataTable.ShouldNotBeNull();
            resultDataTable.Columns.Count.ShouldBe(TypeDescriptor.GetProperties(typeof(SubscriberFinal)).Count);
            resultDataTable.Rows.Count.ShouldBe(1);
            resultDataTable.Rows[0][AccountNumberField].ShouldBe(SampleAccountNumber);
            resultDataTable.Rows[0][TitleField].ShouldBe(TitleValue1);
        }

        [TestCase(FrameworkUAD_Lookup.Enums.FileTypes.Complimentary, Normal)]
        [TestCase(FrameworkUAD_Lookup.Enums.FileTypes.Other, Normal)]
        [TestCase(FrameworkUAD_Lookup.Enums.FileTypes.Data_Compare, Normal)]
        [TestCase(FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Long_Form, Normal)]
        [TestCase(FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms, Normal)]
        [TestCase(FrameworkUAD_Lookup.Enums.FileTypes.List_Source_2YR, Normal)]
        [TestCase(FrameworkUAD_Lookup.Enums.FileTypes.List_Source_3YR, Normal)]
        [TestCase(FrameworkUAD_Lookup.Enums.FileTypes.List_Source_Other, Normal)]
        [TestCase(FrameworkUAD_Lookup.Enums.FileTypes.QuickFill, Normal)]
        [TestCase(FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction, Normal)]
        [TestCase(FrameworkUAD_Lookup.Enums.FileTypes.Field_Update, Other)]
        public void ImportToUAD_ForFileTypes_ShouldImport(FrameworkUAD_Lookup.Enums.FileTypes fileTypes, string param)
        {
            // Arrange
            SetUpImport(param);
            var fileCleansed = InitializeImport();

            // Act
            testEntity.ImportToUAD(fileCleansed.Client, fileCleansed.AdmsLog, fileTypes, fileCleansed.SourceFile);

            // Assert
            fileCleansed.ShouldSatisfyAllConditions(
                () => fileCleansed.Client.ClientID.ShouldBe(ClientId),
                () => fileCleansed.Client.FtpFolder.ShouldBe(FtpFolder),
                () => fileCleansed.AdmsLog.ProcessCode.ShouldBe(string.Empty),
                () => fileCleansed.AdmsLog.SourceFileId.ShouldBe(One));
        }

        private FileCleansed InitializeImport()
        {
            var fileCleansed = new FileCleansed();
            var client = new Client()
            {
                ClientID = ClientId,
                FtpFolder = FtpFolder
            };
            var admsLog = new AdmsLog()
            {
                ProcessCode = String.Empty,
                SourceFileId = One
            };
            fileCleansed.Client = client;
            fileCleansed.AdmsLog = admsLog;
            fileCleansed.SourceFile = new SourceFile();
            return fileCleansed;
        }
      
        private void SetUpImport(string param)
        {
            _shimsImport = ShimsContext.Create();
            var nameValueCollection = new NameValueCollection();
            nameValueCollection.Add(IsDemo, DemoValue);
            if (param.Equals(Other))
            {
                var codeList = new List<Code>();
                var code = new Code();
                code.CodeName = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString();
                code.CodeId = One;
                codeList.Add(code);
                ShimCode.AllInstances.SelectEnumsCodeType = (x, y) => codeList;
                var subscriberFinal = new SubscriberFinal() { AccountNumber = SampleAccountNumber };
                var subscriberFinals = new List<SubscriberFinal>();
                subscriberFinals.Add(subscriberFinal);
                ShimSubscriberFinal.AllInstances.SelectForFieldUpdateStringClientConnectionsBoolean =
                (x, y, z, v) => subscriberFinals;
            }
            ShimConfigurationManager.AppSettingsGet = () => nameValueCollection;
            ShimSqlConnection.AllInstances.Open = (x) => {
                ReflectionHelper.SetField(x, State, ConnectionState.Open);
            };
            ShimSqlConnection.AllInstances.Close = (x) => {
                ReflectionHelper.SetField(x, State, ConnectionState.Closed);
            };
            ShimSqlCommand.AllInstances.ExecuteScalar = (x) => StringOne;
            ShimSqlCommand.AllInstances.ExecuteNonQuery = (x) => One;
            ShimSqlCommand.AllInstances.ExecuteReader = (x) => null;
            ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (x, y) => null;
            ShimSqlCommand.AllInstances.ConnectionGet = (x) => new ShimSqlConnection();
            ShimSqlConnection.AllInstances.StateGet = (x) => ConnectionState.Open;
            ShimDataFunctions.GetSqlConnectionString = (x) => new ShimSqlConnection();
            var admsResultCount = new AdmsResultCount();
            admsResultCount.FinalProfileCount = Count;
            admsResultCount.FinalDemoCount = Count;
            admsResultCount.MatchedRecordCount = Count;
            admsResultCount.UadConsensusCount = Count;
            ShimSubscriberFinal.AllInstances.SelectResultCountAfterProcessToLiveStringClientConnections = 
                (x, y, z) => admsResultCount;
        }

        [TestCase(Complimentary)]
        [TestCase(ParameterOther)]
        [TestCase(DataCompare)]
        [TestCase(Telemarketing)]
        [TestCase(Web)]
        [TestCase(TwoYear)]
        [TestCase(ThreeYear)]
        [TestCase(ListOther)]
        [TestCase(QuickFill)]
        [TestCase(PaidTransaction)]
        [TestCase(FieldUpdate)]
        public void HandleFileCleansed_ForFileTypeComplimentary_ShouldCleanseFile(string fileType)
        {
            // Arrange
            var fileClensed = Initialised(fileType);

            // Act
            testEntity.HandleFileCleansed(fileClensed);

            // Assert
            fileClensed.ShouldSatisfyAllConditions(
                () => fileClensed.Client.ClientID.ShouldBe(Id),
                () => fileClensed.AdmsLog.ProcessCode.ShouldBe(string.Empty));
        }

        private void SetTestData(List<FieldMapping> mappings, List<string> incomingColumns)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(SubscriberFinal));
            foreach (PropertyDescriptor prop in properties)
            {
                incomingColumns.Add(prop.DisplayName);
                mappings.Add(new FieldMapping { MAFField = prop.DisplayName, IncomingField = prop.DisplayName });
            }
        }

        private FileCleansed Initialised(String param)
        {
            SetUpCleansed(param);
            var fileClensed = new FileCleansed();
            var client = new Client();
            client.ClientID = Id;
            client.FtpFolder = FtpFolder;
            fileClensed.Client = client;
            fileClensed.SourceFile = new SourceFile();
            fileClensed.AdmsLog = new AdmsLog();
            if (param.Equals(FieldUpdate))
            {
                fileClensed.ImportFile = new FileInfo(FileName);
            }
            return fileClensed;
        }

        private void SetUpCleansed(string param)
        {
            _shims = ShimsContext.Create();
            var code = new Code();
            var admsResultCount = new AdmsResultCount();
            admsResultCount.FinalProfileCount = Count;
            admsResultCount.FinalDemoCount = Count;
            admsResultCount.MatchedRecordCount = Count;
            admsResultCount.UadConsensusCount = Count;
            if (param.Equals(Complimentary))
            {
                code.CodeId = Id;
                code.CodeName = FrameworkUAD_Lookup.Enums.FileTypes.Complimentary.ToString();
            }
            else if (param.Equals(ParameterOther))
            {
                code.CodeId = Id;
                code.CodeName = FrameworkUAD_Lookup.Enums.FileTypes.Other.ToString();
            }
            else if (param.Equals(DataCompare))
            {
                code.CodeId = Id;
                code.CodeName = FrameworkUAD_Lookup.Enums.FileTypes.Data_Compare.ToString();
            }
            else if (param.Equals(Telemarketing))
            {
                code.CodeId = Id;
                code.CodeName = FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Long_Form.ToString();
            }
            else if (param.Equals(Web))
            {
                code.CodeId = Id;
                code.CodeName = FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms.ToString();
            }
            else if (param.Equals(TwoYear))
            {
                code.CodeId = Id;
                code.CodeName = FrameworkUAD_Lookup.Enums.FileTypes.List_Source_2YR.ToString();
            }
            else if (param.Equals(ThreeYear))
            {
                code.CodeId = Id;
                code.CodeName = FrameworkUAD_Lookup.Enums.FileTypes.List_Source_3YR.ToString();
            }
            else if (param.Equals(ListOther))
            {
                code.CodeId = Id;
                code.CodeName = FrameworkUAD_Lookup.Enums.FileTypes.List_Source_Other.ToString();
            }
            else if (param.Equals(QuickFill))
            {
                code.CodeId = Id;
                code.CodeName = FrameworkUAD_Lookup.Enums.FileTypes.QuickFill.ToString();
            }
            else if (param.Equals(PaidTransaction))
            {
                code.CodeId = Id;
                code.CodeName = FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction.ToString();
            }
            else if (param.Equals(FieldUpdate))
            {
                code.CodeId = Id;
                code.CodeName = FrameworkUAD_Lookup.Enums.FileTypes.Field_Update.ToString();
                codeList = new List<Code>();
                var defCode = new Code();
                defCode.CodeName = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString();
                defCode.CodeId = One;
                codeList.Add(defCode);
                ShimCode.AllInstances.SelectEnumsCodeType = (x, y) => codeList;
                var subscriberFinal = new SubscriberFinal() { AccountNumber = SampleAccountNumber };
                subscriberFinals = new List<SubscriberFinal>();
                subscriberFinals.Add(subscriberFinal);
                ShimSubscriberFinal.AllInstances.SelectForFieldUpdateStringClientConnectionsBoolean = 
                    (x, y, z, v) => subscriberFinals;
                ShimServiceBase.AllInstances.clientGet = (x) => new Client();
            }
            ShimSubscriberFinal.AllInstances.SelectResultCountAfterProcessToLiveStringClientConnections =
                    (x, y, z) => admsResultCount;
            var nameValueCollection = new NameValueCollection();
            nameValueCollection.Add(IsDemo, DemoValue);
            ShimSqlConnection.AllInstances.Open = (x) => {
                ReflectionHelper.SetField(x, State, ConnectionState.Open);
            };
            ShimSqlConnection.AllInstances.Close = (x) => {
                ReflectionHelper.SetField(x, State, ConnectionState.Closed);
            };
            ShimSqlCommand.AllInstances.ExecuteScalar = (x) => StringOne;
            ShimSqlCommand.AllInstances.ExecuteNonQuery = (x) => One;
            ShimSqlCommand.AllInstances.ExecuteReader = (x) => null;
            ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (x, y) => null;
            ShimSqlCommand.AllInstances.ConnectionGet = (x) => new ShimSqlConnection();
            ShimSqlConnection.AllInstances.StateGet = (x) => ConnectionState.Open;
            ShimConfigurationManager.AppSettingsGet = () => nameValueCollection;
            ShimSourceFileBase.AllInstances.DatabaseFileTypeIdGet = (x) => Id;
            ShimCode.AllInstances.SelectCodeIdInt32 = (x, y) => code;
            ShimAdmsLog.AllInstances.ProcessCodeGet = (x) => String.Empty;
            ShimDataFunctions.GetSqlConnectionString = (x) => new ShimSqlConnection();
        }
    }
}

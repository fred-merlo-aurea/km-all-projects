using System;
using System.Collections;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using KM.Common.Entity.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECNExternalSrcDataImport.Tests
{
    /// <summary>
    ///     Unit tests for <see cref="ECNExternalSrcDataImport"/>
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public partial class DataImporterTest
    {
        private const string AssemblyName = "ECNExternalSrcDataImport";
        private const string ClassName = "ecn.communicator.engines.DataImporter";
        private const string FieldLogFile = "logFile";
        private const string FieldGroupId = "groupID";
        private const string FieldCustomerId = "customerID";
        private const string AppSettingKeyKMCommon_Application = "KMCommon_Application";
        private const string AppSettingValue1= "1";
        private const string DBColumnAction = "Action";
        private const string DBColumnCounts = "Counts";
        private const string DBValuet = "t";
        private const string DBValueT = "T";
        private const string DBValueI = "I";
        private const string DBValueU = "U";
        private const string DBValueD = "D";
        private const string DBValueS = "S";
        private const string DBValueE = "E";
        private const string DBValueM = "M";
        private const int DBValue1 = 1;
        private const int DBValue2 = 2;
        private const int DBValue3 = 3;
        private const string DBColumnuser_name = "user_name";
        private const string DBColumnSubscribeType= "subscribetype";
        private const string DBColumnEmailAddress = "emailaddress";
        private const string DBValueUser1 = "User1";
        private const string DBValueUser2 = "User2";
        private const string DBValueEmail1 = "User1@unittest.com";
        private const string DBValueEmail2 = "User1@unittest.com";
        private const string DBValueName = "name";

        private IDisposable _shimObject;
        private PrivateType _dataImporter;
        private string _exceptionMessage = string.Empty;
        private bool _anyException;

        [TestInitialize]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _dataImporter = new PrivateType(AssemblyName, ClassName);

            _sampleDataTable = new DataTable();
            _sampleDataTable.Clear();
            _sampleDataTable.Columns.Add(DBColumnuser_name);
            _sampleDataTable.Columns.Add(DBColumnSubscribeType);
            _sampleDataTable.Columns.Add(DBColumnEmailAddress);
            var sampleDataRow = _sampleDataTable.NewRow();
            sampleDataRow[DBColumnuser_name] = DBValueUser1;
            sampleDataRow[DBColumnSubscribeType] = DBValue1;
            sampleDataRow[DBColumnEmailAddress] = DBValueEmail1;
            _sampleDataTable.Rows.Add(sampleDataRow);
            sampleDataRow = _sampleDataTable.NewRow();
            sampleDataRow[DBColumnuser_name] = DBValueUser2;
            sampleDataRow[DBColumnSubscribeType] = DBValue2;
            sampleDataRow[DBColumnEmailAddress] = DBValueEmail2;
            _sampleDataTable.Rows.Add(sampleDataRow);

            ShimGroupDataFields.GetGroupUDFHashtable_NoAccessCheckInt32 = groupId => new Hashtable
            {
                {DBValue1, DBValueName}
            };

            ShimConfigurationManager.AppSettingsGet =
                () => new System.Collections.Specialized.NameValueCollection { { AppSettingKeyKMCommon_Application, AppSettingValue1 } };

            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, name, appId, note, gd, ec) =>
                {
                    _exceptionMessage = ex.Message;
                    _anyException = true;
                    return 0;
                };
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }
    }
}

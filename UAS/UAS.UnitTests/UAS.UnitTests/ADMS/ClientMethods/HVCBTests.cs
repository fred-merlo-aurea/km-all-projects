using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Fakes;
using ADMS.ClientMethods;
using ADMS.Services.Fakes;
using Core.ADMS.Events;
using Core_AMS.Utilities.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HVCBTests
    {
        private PrivateObject _hvcbPrivateObject;
        private readonly HVCB Hvcb = new HVCB();

        private DataTable _dataTableAgents;
        private IDictionary<int, DataRow> _newRows;
        private IDictionary<int, DataRow> _deleteRows;
        private IDictionary<string, string> _source;

        private const string SamplePk = "Id";
        private const string SampleValue1 = "Value 1";
        private const string SampleValue2 = "Value 2";
        private const string SampleValue3 = "Value 3";
        private const string SampleValue4 = "Value 4";
        private const string ClickAgentGuidField = "click_agent_guid";

        private const string AgentName = "SampleAgentName";
        private string _columnName;

        [SetUp]
        public void Initialize()
        {
            _dataTableAgents = new DataTable();
            _newRows = new Dictionary<int, DataRow>();
            _deleteRows = new Dictionary<int, DataRow>();
            _source = new Dictionary<string, string>();
            _columnName = string.Empty;
        }

        [Test]
        public void HVCBRelationalFiles_DirectoryNotExist_NoGetFilesCalled()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new HVCB();
                var client = new KMPlatform.Entity.Client();
                var getFilesCalled = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.GLM_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.GetFiles = info =>
                {
                    getFilesCalled = true;
                    return null;
                };

                // Act
                testObject.HVCBRelationalFiles(client, null, null, new FileMoved());

                // Assert
                getFilesCalled.ShouldBeFalse();
            }
        }

        [Test]
        public void HVCBRelationalFiles_DirectoryExists_NoFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new HVCB();
                var getFilesCalled = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.GLM_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new FileInfo[] { };
                };

                // Act
                testObject.HVCBRelationalFiles(client, null, null, new FileMoved());

                // Assert
                getFilesCalled.ShouldBeTrue();
            }
        }

        [Test]
        public void HVCBRelationalFiles_DirectoryExists_PartialFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new HVCB();
                var getFilesCalled = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.GLM_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new[]
                    {
                        new FileInfo("c2m_vw_Agents.txt"),
                        new FileInfo("c2m_Agents_Wholesalers.txt"),
                    };
                };

                // Act
                testObject.HVCBRelationalFiles(client, null, null, new FileMoved());

                // Assert
                getFilesCalled.ShouldBeTrue();
            }
        }

        [Test]
        public void HVCBRelationalFiles_DirectoryExists_AllFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new HVCB();
                var getFilesCalled = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.GLM_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new[]
                    {
                        new FileInfo("c2m_vw_Agents.txt"),
                        new FileInfo("c2m_Agents_Wholesalers.txt"),
                        new FileInfo("c2m_Agents_Consortia.txt"),
                        new FileInfo("c2m_Agent_Profile_Hds.txt"),
                        new FileInfo("c2m_Agents_HeardOfUsFromSource.txt"),
                        new FileInfo("c2m_Agents_Exams.txt"),
                        new FileInfo("c2m_Agents_Stays_Orders.txt"),
                    };
                };

                ShimServiceBase.AllInstances.LogErrorExceptionClientStringBooleanBoolean =
                    (_, __, ___, _4, _5, _6) => { };

                // Act
                testObject.HVCBRelationalFiles(client, null, null, new FileMoved());

                // Assert
                getFilesCalled.ShouldBeTrue();
            }
        }

        [Test]
        public void HVCBc2mAgentsAndSubscriptions_DirectoryNotExist_NoGetFilesCalled()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new HVCB();
                var client = new KMPlatform.Entity.Client();
                var getFilesCalled = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.GLM_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.GetFiles = info =>
                {
                    getFilesCalled = true;
                    return null;
                };

                // Act
                testObject.HVCBc2mAgentsAndSubscriptions(client, null, null, new FileMoved());

                // Assert
                getFilesCalled.ShouldBeFalse();
            }
        }

        [Test]
        public void HVCBc2mAgentsAndSubscriptions_DirectoryExists_NoFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new HVCB();
                var getFilesCalled = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.GLM_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new FileInfo[] { };
                };

                // Act
                testObject.HVCBc2mAgentsAndSubscriptions(client, null, null, new FileMoved());

                // Assert
                getFilesCalled.ShouldBeTrue();
            }
        }

        [Test]
        public void HVCBc2mAgentsAndSubscriptions_DirectoryExists_PartialFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new HVCB();
                var getFilesCalled = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.GLM_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new[]
                    {
                        new FileInfo("c2m_vw_Agents.txt"),
                        new FileInfo("c2m_Agents_Wholesalers.txt"),
                    };
                };

                // Act
                testObject.HVCBc2mAgentsAndSubscriptions(client, null, null, new FileMoved());

                // Assert
                getFilesCalled.ShouldBeTrue();
            }
        }

        [Test]
        public void HVCBc2mAgentsAndSubscriptions_DirectoryExists_AllFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new HVCB();
                var getFilesCalled = false;
                var errorLogged = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.GLM_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new[]
                    {
                        new FileInfo("c2m_vw_Agents.txt"),
                        new FileInfo("c2m_Agents_Subscriptions.txt"),
                    };
                };

                ShimServiceBase.AllInstances.LogErrorExceptionClientStringBooleanBoolean =
                    (_, __, ___, _4, _5, _6) => { errorLogged = true; };

                // Act
                testObject.HVCBc2mAgentsAndSubscriptions(client, null, null, new FileMoved());

                // Assert
                getFilesCalled.ShouldBeTrue();
                errorLogged.ShouldBeTrue();
            }
        }

        [Test]
        public void UpdateDataTableAgents_WhenDataTableAgentsIsNull_ThrowException()
        {
            // Arrange
            CreateHvbcPrivateObject();
            _dataTableAgents = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => _hvcbPrivateObject.Invoke("UpdateDataTableAgents", _dataTableAgents, _newRows, _deleteRows));
        }

        [Test]
        public void UpdateDataTableAgents_WhenNewRowsIsNull_ThrowException()
        {
            // Arrange
            CreateHvbcPrivateObject();
            _dataTableAgents = new DataTable();
            _newRows = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => _hvcbPrivateObject.Invoke("UpdateDataTableAgents", _dataTableAgents, _newRows, _deleteRows));
        }

        [Test]
        public void UpdateDataTableAgents_WhenDeleteRowsIsNull_ThrowException()
        {
            // Arrange
            CreateHvbcPrivateObject();
            _dataTableAgents = new DataTable();
            _newRows = new Dictionary<int, DataRow>();
            _deleteRows = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => _hvcbPrivateObject.Invoke("UpdateDataTableAgents", _dataTableAgents, _newRows, _deleteRows));
        }

        [Test]
        public void UpdateDataTableAgents_ShouldDeleteCorrectRows_UpdatesDataTableAgents()
        {
            // Arrange
            CreateHvbcPrivateObject();
            _dataTableAgents = CreateDataTableForHvcb();
            _deleteRows = SetRowToBeDeleted();
            _newRows = new Dictionary<int, DataRow>();

            var countRows = _dataTableAgents.Rows.Count - 1;

            // Act
            _hvcbPrivateObject.Invoke("UpdateDataTableAgents", _dataTableAgents, _newRows, _deleteRows);

            // Assert
            _dataTableAgents.ShouldSatisfyAllConditions(
                () => _dataTableAgents.ShouldNotBeNull(),
                () => _dataTableAgents.Rows.Count.ShouldBe(countRows));
        }

        [Test]
        public void UpdateDataTableAgents_ShouldAddCorrectRows_UpdatesDataTableAgents()
        {
            // Arrange
            CreateHvbcPrivateObject();
            _dataTableAgents = CreateDataTableForHvcb();
            _deleteRows = new Dictionary<int, DataRow>();
            _newRows = SetRowToBeAdded();

            var countRows = _dataTableAgents.Rows.Count + 1;

            // Act
            _hvcbPrivateObject.Invoke("UpdateDataTableAgents", _dataTableAgents, _newRows, _deleteRows);

            // Assert
            _dataTableAgents.ShouldSatisfyAllConditions(
                () => _dataTableAgents.ShouldNotBeNull(),
                () => _dataTableAgents.Rows.Count.ShouldBe(countRows));
        }

        [Test]
        public void AddSpecificRows_WhenDataTableAgentsIsNull_ThrowsException()
        {
            // Arrange
            CreateHvbcPrivateObject();
            _dataTableAgents = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => _hvcbPrivateObject.Invoke("AddSpecificRows", _dataTableAgents, _source, _columnName));
        }

        [Test]
        public void AddSpecificRows_WhenSourceIsNull_ThrowsException()
        {
            // Arrange
            CreateHvbcPrivateObject();
            _source = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => _hvcbPrivateObject.Invoke("AddSpecificRows", _dataTableAgents, _source, _columnName));
        }

        [Test]
        public void AddSpecificRows_WhenDataTableAgentsIsEmpty_ReturnsRowIndex()
        {
            // Arrange
            CreateHvbcPrivateObject();

            // Act
            var result = _hvcbPrivateObject.Invoke("AddSpecificRows", _dataTableAgents, _source, _columnName);

            // Assert
            result.ShouldBe(0);
        }

        [Test]
        public void AddSpecificRows_WhenSourceIsEmpty_ReturnsRowIndex()
        {
            // Arrange
            CreateHvbcPrivateObject();
            _dataTableAgents = CreateDataTableForHvcb();

            // Act
            var result = _hvcbPrivateObject.Invoke("AddSpecificRows", _dataTableAgents, _source, _columnName);

            // Assert
            result.ShouldBe(_dataTableAgents.Rows.Count);
        }

        [Test]
        public void AddSpecificRows_WhenSourceIsNotEmpty_ReturnsRowIndex()
        {
            // Arrange
            CreateHvbcPrivateObject();
            _dataTableAgents = CreateDataTableForHvcb();
            _source = CreateSourceDictionary();
            _columnName = ClickAgentGuidField;

            // Act
            var result = _hvcbPrivateObject.Invoke("AddSpecificRows", _dataTableAgents, _source, _columnName);

            // Assert
            result.ShouldBe(_dataTableAgents.Rows.Count - _source.Count);
        }

        [Test]
        public void ProcessAgents_WhenDataTableAgentsIsNull_ThrowsException()
        {
            // Arrange
            CreateHvbcPrivateObject();
            _dataTableAgents = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => _hvcbPrivateObject.Invoke("ProcessAgents", _dataTableAgents, AgentName, SamplePk));
        }

        [Test]
        public void ProcessAgents_WhenDataTableAgentsIsNotNull_ThrowsException()
        {
            // Arrange
            CreateHvbcPrivateObject();
            _dataTableAgents = CreateDataTableForHvcb();
            var agents = CreateProcessAgentsResult();

            // Act
            var result = _hvcbPrivateObject.Invoke("ProcessAgents", _dataTableAgents, AgentName, ClickAgentGuidField) as Dictionary<string, string>;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.Count.ShouldBe(_dataTableAgents.Rows.Count),
                () => result.ShouldBe(agents));
        }

        private void CreateHvbcPrivateObject()
        {
            _hvcbPrivateObject = new PrivateObject(Hvcb, new PrivateType(typeof(HVCB)));
        }

        private static DataTable CreateDataTableForHvcb()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(SamplePk, typeof(string));
            dataTable.Columns.Add(ClickAgentGuidField, typeof(string));

            var dataRow1 = dataTable.NewRow();
            dataRow1[SamplePk] = 1;
            dataRow1[ClickAgentGuidField] = SampleValue1;
            dataTable.Rows.Add(dataRow1);

            var dataRow2 = dataTable.NewRow();
            dataRow2[SamplePk] = 2;
            dataRow2[ClickAgentGuidField] = SampleValue2;
            dataTable.Rows.Add(dataRow2);

            var dataRow3 = dataTable.NewRow();
            dataRow3[SamplePk] = 3;
            dataRow3[ClickAgentGuidField] = SampleValue3;
            dataTable.Rows.Add(dataRow3);

            return dataTable;
        }
        private IDictionary<int, DataRow> SetRowToBeAdded()
        {
            var dataRow = _dataTableAgents.NewRow();
            dataRow[SamplePk] = 4;
            dataRow[ClickAgentGuidField] = SampleValue4;

            return new Dictionary<int, DataRow>
            {
                [4] = dataRow
            };
        }

        private IDictionary<int, DataRow> SetRowToBeDeleted()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(SamplePk, typeof(int));
            dataTable.Columns.Add(ClickAgentGuidField, typeof(string));

            var dataRow = dataTable.NewRow();
            dataRow[SamplePk] = 2;
            dataRow[ClickAgentGuidField] = SampleValue2;
            dataTable.Rows.Add(dataRow);

            return new Dictionary<int, DataRow>
            {
                [2] = dataRow
            };
        }

        private IDictionary<string, string> CreateSourceDictionary()
        {
            return new Dictionary<string, string>
            {
                [SampleValue1] = $"{SampleValue1}, {SampleValue2}",
                [SampleValue2] = $"{SampleValue3}, {SampleValue4}"
            };
        }

        private IDictionary<string, string> CreateProcessAgentsResult()
        {
            return new Dictionary<string, string>
            {
                [SampleValue1] = SampleValue1,
                [SampleValue2] = SampleValue2,
                [SampleValue3] = SampleValue3
            };
        }
    }
}

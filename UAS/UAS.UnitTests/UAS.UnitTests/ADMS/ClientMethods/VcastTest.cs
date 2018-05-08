using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using ADMS.Services.Fakes;
using Core_AMS.Utilities.Fakes;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.ADMS.ClientMethods.Common;
using ClientEntity = KMPlatform.Entity.Client;
using ImportVesselObject = FrameworkUAD.Object.ImportVessel;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class VcastTest : Fakes
    {
        private const string So2Key = "SO2";
        private const string So3Key = "SO3";
        private const string Sub02AnsKey = "SUB02ANS";
        private const string Sub03AnsKey = "SUB03ANS";
        private const string SubNumKey = "SubNum";
        private const string SubsCrbNumKey = "SUBSCRBNUM";

        private IDictionary<int, DataRow> _newRows;
        private DataTable _workDataTable;
        private DataRow _currentDataRow;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            _workDataTable = new DataTable();
            _newRows = new Dictionary<int, DataRow>();
        }

        [Test]
        public void AddNewRows_WhenWorkDataTableIsNull_ThrowsException()
        {
            // Arrange
            DataRow distinctMatch = null;
            _workDataTable = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => Vcast.AddNewRows(_workDataTable, _newRows, _currentDataRow, distinctMatch));
        }

        [Test]
        public void AddNewRows_WhenNewRowsTableIsNull_ThrowsException()
        {
            // Arrange
            DtAccess = CreateDataTableForVcast();
            _workDataTable = DtAccess.Clone();
            _newRows = null;
            DataRow distinctMatch = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => Vcast.AddNewRows(_workDataTable, _newRows, _currentDataRow, distinctMatch));
        }

        [Test]
        public void AddNewRows_WhenCurrentDataRowTableIsNull_ThrowsException()
        {
            // Arrange
            DtAccess = CreateDataTableForVcast();
            _workDataTable = DtAccess.Clone();
            _currentDataRow = null;
            DataRow distinctMatch = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => Vcast.AddNewRows(_workDataTable, _newRows, _currentDataRow, distinctMatch));
        }

        [Test]
        public void AddNewRows_WhenDistinctMatchIsNull_ShouldSetItemArray()
        {
            // Arrange
            DtAccess = CreateDataTableForVcast();

            _currentDataRow = DtAccess.Rows[0];
            DataRow distinctMatch = null;
            _workDataTable = DtAccess.Clone();
            var position = _newRows.Count + 1;

            // Act
            Vcast.AddNewRows(_workDataTable, _newRows, _currentDataRow, distinctMatch);

            // Assert
            _newRows.ShouldSatisfyAllConditions(
                () => _newRows.Count.ShouldBe(position),
                () => _newRows[1].ItemArray.ShouldBe(_currentDataRow.ItemArray));
        }

        [Test]
        public void AddNewRows_WhenDistinctMatchIsNotNull_ShouldSetCorrectColumns()
        {
            // Arrange
            DtAccess = CreateDataTableForVcast();

            _currentDataRow = DtAccess.Rows[0];
            var distinctValues = CreateDataTableForVcast();
            var distinctMatch = distinctValues.Rows.Find(_currentDataRow[SubsCrbNumKey].ToString());
            _workDataTable = DtAccess.Clone();
            var position = _newRows.Count + 1;

            // Act
            Vcast.AddNewRows(_workDataTable, _newRows, _currentDataRow, distinctMatch);

            // Assert
            _newRows.ShouldSatisfyAllConditions(
                () => _newRows.Count.ShouldBe(position),
                () => _newRows[1].ItemArray.ShouldBe(_currentDataRow.ItemArray));
        }

        [Test]
        public void CreateCsvForVcast_WhenDtAccessIsNull_ThrowsException()
        {
            // Arrange
            DtAccess = null;

            var distinctValues = CreateDataTableForVcast();

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => Vcast.CreateCsvForVcast(DtAccess, distinctValues, PubCodeKey, SampleFolder));
        }

        [Test]
        public void CreateCsvForVcast_WhenDistinctValuesIsNull_ThrowsException()
        {
            // Arrange
            DtAccess = CreateDataTableForVcast();
            DataTable distinctValues = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => Vcast.CreateCsvForVcast(DtAccess, distinctValues, PubCodeKey, SampleFolder));
        }

        [Test]
        public void CreateCsvForVcast_WhenDtAccessHasRows_ShouldCreateCsvFile()
        {
            // Arrange
            DtAccess = CreateDataTableForVcast();
            _currentDataRow = DtAccess.Rows[0];
            var distinctValues = CreateDataTableForVcast();
            _workDataTable = DtAccess.Clone();

            ShimForDataTableAcceptChanges();
            ShimForCreateCsvFile();
            ShimForConsoleMessage();

            // Act, Assert
            Vcast.CreateCsvForVcast(DtAccess, distinctValues, PubCodeKey, SampleFolder);
        }

        [Test]
        public void VcastMxBooks_ShouldThrowsExceptionAtProcessImportVesselData_CreateLogError()
        {
            // Arrange
            ClientEntity = new ClientEntity();

            ShimForConsoleMessage();
            ShimForClientFtp();
            ShimForDataTableAcceptChanges();
            ShimForFileRows(3);
            ShimImportVessel.AllInstances.GetImportVesselFileInfoInt32Int32FileConfiguration =
                (_, fileInfo, startRow, takeRowCount, fileConfig) => new ImportVesselObject
                {
                    TotalRowCount = 1,
                    DataOriginal = DtAccess
                };

            ShimFileFunctions.AllInstances.CreateCSVFromDataTableDataTableStringBoolean =
                (_, workTable, fileName, deleteExists) => throw new InvalidOperationException();

            ShimServiceBase.AllInstances.LogErrorExceptionClientStringBooleanBoolean =
                (_, exception, client, message, removeThread, removeQue) => { };

            ShimClientMethods.AllInstances.Vcast_CreateTempMXBooksTable = _ => true;
            ShimClientMethods.AllInstances.Vcast_DropTempMXBooksTable = _ => true;

            // Act, Assert
            Vcast.Vcast_MX_BOOKS(ClientEntity, FileInfo, ClientSpecialFile);
        }

        [Test]
        public void VcastMxBooks_WhenDataAccessIsNotNull_ShouldUploadFile()
        {
            // Arrange
            ClientEntity = new ClientEntity();
            DtAccess = CreateDataTableForVcast();
            _currentDataRow = DtAccess.Rows[0];
            _workDataTable = DtAccess.Clone();

            FileInfo = CreateFileInfo($"{DummyName}{Consts.CsvExtension}");

            ShimForConsoleMessage();
            ShimForClientFtp();
            ShimForDataTableAcceptChanges();
            ShimForFileRows(3);
            ShimImportVessel.AllInstances.GetImportVesselFileInfoInt32Int32FileConfiguration =
                (_, fileInfo, startRow, takeRowCount, fileConfig) => new ImportVesselObject
                {
                    TotalRowCount = 1,
                    DataOriginal = DtAccess
                };

            ShimFileFunctions.AllInstances.CreateCSVFromDataTableDataTableStringBoolean =
                (_, workTable, fileName, deleteExists) => { };

            ShimServiceBase.AllInstances.LogErrorExceptionClientStringBooleanBoolean =
                (_, exception, client, message, removeThread, removeQue) => { };

            ShimClientMethods.AllInstances.Vcast_CreateTempMXBooksTable = _ => true;
            ShimClientMethods.AllInstances.Vcast_Process_File_MX_BooksDataTable = (_, dataAccess) => true;
            ShimClientMethods.AllInstances.Vcase_Get_Distinct_MX_Books = _ => CreateDataTableForVcast();
            ShimClientMethods.AllInstances.Vcast_DropTempMXBooksTable = _ => true;

            // Act, Assert
            Vcast.Vcast_MX_BOOKS(ClientEntity, FileInfo, ClientSpecialFile);
        }

        [Test]
        public void VcastMxElanBook_ShouldThrowsExceptionAtProcessImportVesselData_CreateLogError()
        {
            // Arrange
            ClientEntity = new ClientEntity();

            ShimForConsoleMessage();
            ShimForClientFtp();
            ShimForDataTableAcceptChanges();
            ShimForFileRows(3);
            ShimImportVessel.AllInstances.GetImportVesselFileInfoInt32Int32FileConfiguration =
                (_, fileInfo, startRow, takeRowCount, fileConfig) => new ImportVesselObject
                {
                    TotalRowCount = 1,
                    DataOriginal = DtAccess
                };

            ShimFileFunctions.AllInstances.CreateCSVFromDataTableDataTableStringBoolean =
                (_, workTable, fileName, deleteExists) => throw new InvalidOperationException();

            ShimServiceBase.AllInstances.LogErrorExceptionClientStringBooleanBoolean =
                (_, exception, client, message, removeThread, removeQue) => { };

            ShimClientMethods.AllInstances.Vcast_CreateTempMXElanTable = _ => true;
            ShimClientMethods.AllInstances.Vcast_DropTempMXElanTable = _ => true;

            // Act, Assert
            Vcast.Vcast_MX_ELANBOOK(ClientEntity, FileInfo, ClientSpecialFile);
        }

        [Test]
        public void VcastMxElanBook_WhenDataAccessIsNotNull_ShouldUploadFile()
        {
            // Arrange
            ClientEntity = new ClientEntity();
            DtAccess = CreateDataTableForVcast();
            _currentDataRow = DtAccess.Rows[0];
            _workDataTable = DtAccess.Clone();

            FileInfo = CreateFileInfo($"{DummyName}{Consts.CsvExtension}");

            ShimForConsoleMessage();
            ShimForClientFtp();
            ShimForDataTableAcceptChanges();
            ShimForFileRows(3);
            ShimImportVessel.AllInstances.GetImportVesselFileInfoInt32Int32FileConfiguration =
                (_, fileInfo, startRow, takeRowCount, fileConfig) => new ImportVesselObject
                {
                    TotalRowCount = 1,
                    DataOriginal = DtAccess
                };

            ShimFileFunctions.AllInstances.CreateCSVFromDataTableDataTableStringBoolean =
                (_, workTable, fileName, deleteExists) => { };

            ShimServiceBase.AllInstances.LogErrorExceptionClientStringBooleanBoolean =
                (_, exception, client, message, removeThread, removeQue) => { };

            ShimClientMethods.AllInstances.Vcast_CreateTempMXElanTable = _ => true;
            ShimClientMethods.AllInstances.Vcast_Process_File_MX_ElanDataTable = (_, dataAccess) => true;
            ShimClientMethods.AllInstances.Vcase_Get_Distinct_MX_Elan = _ => CreateDataTableForVcast();
            ShimClientMethods.AllInstances.Vcast_DropTempMXElanTable = _ => true;

            // Act, Assert
            Vcast.Vcast_MX_ELANBOOK(ClientEntity, FileInfo, ClientSpecialFile);
        }

        private static DataTable CreateDataTableForVcast()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(SubNumKey, typeof(int));
            dataTable.Columns.Add(SubsCrbNumKey, typeof(int));
            dataTable.Columns.Add(So2Key, typeof(string));
            dataTable.Columns.Add(So3Key, typeof(string));
            dataTable.Columns.Add(Sub02AnsKey, typeof(string));
            dataTable.Columns.Add(Sub03AnsKey, typeof(string));

            dataTable.PrimaryKey = new[] { dataTable.Columns[SubNumKey] };

            var dataRow = dataTable.NewRow();
            dataRow[SubNumKey] = 1;
            dataRow[SubsCrbNumKey] = 1;
            dataRow[So2Key] = YesCode;
            dataRow[So3Key] = YesCode;
            dataRow[Sub02AnsKey] = YesCode;
            dataRow[Sub03AnsKey] = YesCode;
            dataTable.Rows.Add(dataRow);

            return dataTable;
        }
    }
}

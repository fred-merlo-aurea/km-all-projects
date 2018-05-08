using System;
using System.Data;
using System.Data.Fakes;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using FrameworkUAS.DataAccess;
using FrameworkUAS.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

using KMComonDataFunctions = KM.Common.DataFunctions;

using ShimKMCommonDataFunctions = KM.Common.Fakes.ShimDataFunctions;

namespace UAS.UnitTests.FrameworkUAS.DataAccess
{
    [TestFixture]
    public class DataFunctionsTest
    {
        private IDisposable context;

        [SetUp]
        public void SetUp()
        {
            context = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        [Test]
        public void GetDataTable_ConnectionString_DataWritten()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            CommandBehavior? actualCommandBehavior = null;
            const string connectionName = "MyConnection";
            const string commandText = "SELECT ID, Rating from Company";
            const string tableName = "Company";
            const int numberOfRows = 2;
            const int numberOfColumns = 2;

            AddSqlConnectionShims();

            ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (command, commandBehavior) =>
            {
                actualSqlCommand = command;
                actualCommandBehavior = commandBehavior;

                return new ShimSqlDataReader();
            };

            ShimDataTable.AllInstances.LoadIDataReader = (dataTable, dataReader) =>
            {
                AddRowsToTable(dataTable, tableName, numberOfRows, numberOfColumns);
            };

            // Act
            var resultDataTable = DataFunctions.GetDataTable(commandText, connectionName);

            // Assert
            actualSqlCommand.ShouldSatisfyAllConditions(
                () => actualSqlCommand.CommandText.ShouldBe(commandText),
                () => actualSqlCommand.CommandTimeout.ShouldBe(0),
                () => actualCommandBehavior.ShouldBe(CommandBehavior.CloseConnection));

            resultDataTable.ShouldSatisfyAllConditions(
                () => resultDataTable.ShouldNotBeNull(),
                () => resultDataTable.TableName.ShouldBe(tableName),
                () => resultDataTable.Rows.Count.ShouldBe(numberOfRows),
                () => resultDataTable.Columns.Count.ShouldBe(numberOfColumns),
                () => resultDataTable.Rows[0][0].ShouldBe("Row: 0 Col: 0"),
                () => resultDataTable.Rows[0][1].ShouldBe("Row: 0 Col: 1"),
                () => resultDataTable.Rows[1][0].ShouldBe("Row: 1 Col: 0"),
                () => resultDataTable.Rows[1][1].ShouldBe("Row: 1 Col: 1"));
        }

        [Test]
        public void GetDataTable_SqlConnection_DataWritten()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            CommandBehavior? actualCommandBehavior = null;
            var sqlConnection = new SqlConnection();
            const string commandText = "SELECT ID, Rating from Company";
            const string tableName = "Company";
            const int numberOfRows = 2;
            const int numberOfColumns = 2;

            AddSqlConnectionShims();

            ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (command, commandBehavior) =>
            {
                actualSqlCommand = command;
                actualCommandBehavior = commandBehavior;

                return new ShimSqlDataReader();
            };

            ShimDataTable.AllInstances.LoadIDataReader = (dataTable, dataReader) =>
            {
                AddRowsToTable(dataTable, tableName, numberOfRows, numberOfColumns);
            };

            // Act
            var resultDataTable = KMComonDataFunctions.GetDataTable(commandText, sqlConnection);

            // Assert
            actualSqlCommand.ShouldSatisfyAllConditions(
                () => actualSqlCommand.CommandText.ShouldBe(commandText),
                () => actualSqlCommand.CommandTimeout.ShouldBe(0),
                () => actualCommandBehavior.ShouldBe(CommandBehavior.CloseConnection));

            resultDataTable.ShouldSatisfyAllConditions(
                () => resultDataTable.ShouldNotBeNull(),
                () => resultDataTable.TableName.ShouldBe(tableName),
                () => resultDataTable.Rows.Count.ShouldBe(numberOfRows),
                () => resultDataTable.Columns.Count.ShouldBe(numberOfColumns),
                () => resultDataTable.Rows[0][0].ShouldBe("Row: 0 Col: 0"),
                () => resultDataTable.Rows[0][1].ShouldBe("Row: 0 Col: 1"),
                () => resultDataTable.Rows[1][0].ShouldBe("Row: 1 Col: 0"),
                () => resultDataTable.Rows[1][1].ShouldBe("Row: 1 Col: 1"));
        }

        [Test]
        public void GetDataTable_SqlCommandConnectionString_DataWritten()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            CommandBehavior? actualCommandBehavior = null;
            const string connectionName = "MyConnection";
            const string commandText = "SELECT ID, Rating from Company";
            const string tableName = "Company";
            const int numberOfRows = 2;
            const int numberOfColumns = 2;
            var sqlCommand = new SqlCommand(commandText);

            AddSqlConnectionShims();

            ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (command, commandBehavior) =>
            {
                actualSqlCommand = command;
                actualCommandBehavior = commandBehavior;

                return new ShimSqlDataReader();
            };

            ShimDataTable.AllInstances.LoadIDataReader = (dataTable, dataReader) =>
            {
                AddRowsToTable(dataTable, tableName, numberOfRows, numberOfColumns);
            };

            // Act
            var resultDataTable = KMComonDataFunctions.GetDataTable(sqlCommand, connectionName);

            // Assert
            actualSqlCommand.ShouldSatisfyAllConditions(
                () => actualSqlCommand.CommandText.ShouldBe(commandText),
                () => actualSqlCommand.CommandTimeout.ShouldBe(0),
                () => actualCommandBehavior.ShouldBe(CommandBehavior.CloseConnection));

            resultDataTable.ShouldSatisfyAllConditions(
                () => resultDataTable.ShouldNotBeNull(),
                () => resultDataTable.TableName.ShouldBe(tableName),
                () => resultDataTable.Rows.Count.ShouldBe(numberOfRows),
                () => resultDataTable.Columns.Count.ShouldBe(numberOfColumns),
                () => resultDataTable.Rows[0][0].ShouldBe("Row: 0 Col: 0"),
                () => resultDataTable.Rows[0][1].ShouldBe("Row: 0 Col: 1"),
                () => resultDataTable.Rows[1][0].ShouldBe("Row: 1 Col: 0"),
                () => resultDataTable.Rows[1][1].ShouldBe("Row: 1 Col: 1"));
        }

        [Test]
        public void GetDataTable_SqlCommandSqlConnection_DataWritten()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            CommandBehavior? actualCommandBehavior = null;
            var sqlConnection = new SqlConnection();
            const string commandText = "SELECT ID, Rating from Company";
            const string tableName = "Company";
            const int numberOfRows = 2;
            const int numberOfColumns = 2;
            var sqlCommand = new SqlCommand(commandText, sqlConnection);

            AddSqlConnectionShims();

            ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (command, commandBehavior) =>
            {
                actualSqlCommand = command;
                actualCommandBehavior = commandBehavior;

                return new ShimSqlDataReader();
            };

            ShimDataTable.AllInstances.LoadIDataReader = (dataTable, dataReader) =>
            {
                AddRowsToTable(dataTable, tableName, numberOfRows, numberOfColumns);
            };

            // Act
            var resultDataTable = KMComonDataFunctions.GetDataTable(sqlCommand);

            // Assert
            actualSqlCommand.ShouldSatisfyAllConditions(
                () => actualSqlCommand.CommandText.ShouldBe(commandText),
                () => actualSqlCommand.CommandTimeout.ShouldBe(0),
                () => actualCommandBehavior.ShouldBe(CommandBehavior.CloseConnection));

            resultDataTable.ShouldSatisfyAllConditions(
                () => resultDataTable.ShouldNotBeNull(),
                () => resultDataTable.TableName.ShouldBe(tableName),
                () => resultDataTable.Rows.Count.ShouldBe(numberOfRows),
                () => resultDataTable.Columns.Count.ShouldBe(numberOfColumns),
                () => resultDataTable.Rows[0][0].ShouldBe("Row: 0 Col: 0"),
                () => resultDataTable.Rows[0][1].ShouldBe("Row: 0 Col: 1"),
                () => resultDataTable.Rows[1][0].ShouldBe("Row: 1 Col: 0"),
                () => resultDataTable.Rows[1][1].ShouldBe("Row: 1 Col: 1"));
        }

        private static void AddRowsToTable(DataTable dataTable, string tableName, int numberOfRows, int numberOfColumns)
        {
            for (var columnIndex = 0; columnIndex < numberOfColumns; columnIndex++)
            {
                dataTable.Columns.Add($"Column_{columnIndex}");
            }

            for (var rowIndex = 0; rowIndex < numberOfRows; rowIndex++)
            {
                var dataRow = dataTable.NewRow();
                for (var columnIndex = 0; columnIndex < numberOfColumns; columnIndex++)
                {
                    dataRow[columnIndex] = $"Row: {rowIndex} Col: {columnIndex}";
                }

                dataTable.Rows.Add(dataRow);
            }

            dataTable.TableName = tableName;
        }

        private void AddSqlConnectionShims()
        {
            ShimKMCommonDataFunctions.GetSqlConnectionString = requestedConnectionName => new SqlConnection();

            ShimSqlConnection.AllInstances.Open = connection => { };
            ShimSqlConnection.AllInstances.StateGet = connection => ConnectionState.Open;
        }
    }
}

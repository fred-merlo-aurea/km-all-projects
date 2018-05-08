using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Fakes;
using System.Data;
using System.Data.Common.Fakes;
using System.Data.Fakes;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;

using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.SqlServer.Server;
using NUnit.Framework;
using Shouldly;

using KM.Common.Fakes;
using KM.Common.Data;

namespace KM.Common.Tests.Functions
{
    [TestFixture]
    public class DataFunctionsTest
    {
        private const string CleanStringDirty = "a' b’ c– “d“ ”e” f…";
        private const string CleanStringCleaned = "a'' b'' c- \"d\" \"e\" f...";

        private const string SampleSqlQuery = "SELECT 1";
        private const int SampleExecuteReturnValue = 1;
        private const string SampleConnectionStringName = "ConnStrName";
        private const string SampleDateTimeParameter = "SampleDateTime";
        private static readonly string SampleConnectionString241 = 
            string.Format("server=10.0.0.{0}", DataFunctions.IpPart241);
        private static readonly string SampleConnectionString251 =
            string.Format("server=10.0.0.{0}", DataFunctions.IpPart251);
        private static readonly string SampleConnectionString198 =
            string.Format("server=10.0.0.{0}", DataFunctions.IpPart198);
        private static readonly string SampleConnectionString21617 =
            string.Format("server=10.0.{0}", DataFunctions.IpPart21617);
        private static readonly string SampleConnectionString1010 =
            string.Format("server=10.0.{0}", DataFunctions.IpPart1010);
        private const string SampleConnectionTemplate = "server=localhost;Database={0}";
        private const string DbNameDev = "dev";
        private static readonly string SampleConnectionMaster = 
            string.Format(SampleConnectionTemplate, DataFunctions.DbNameMaster);
        private static readonly string SampleConnectionDev =
            string.Format(SampleConnectionTemplate, DbNameDev);

        private const string FieldId = "Id";
        private const string FieldDone = "Done";
        private const int FieldIdValue1 = 173;
        private const int FieldIdValue2 = 1949;
        private const bool FieldDoneValue1 = false;
        private const bool FieldDoneValue2 = true;
        private const string SampleTableName = "Products";
        private static readonly string SampleXmlKmPlatformEntity =
            string.Join(Environment.NewLine,
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>",
                "<PACKAGE xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/KMPlatform.Entity\">",
                "<HEADER>",
                "</HEADER>",
                "</PACKAGE>");
        private static readonly string SampleXmlFrameworkUas =
            string.Join(Environment.NewLine,
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>",
                "<PACKAGE xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/FrameworkUAS.Entity\">",
                "<HEADER>",
                "</HEADER>",
                "</PACKAGE>");

        private IDisposable _shims;

        private ShimSqlConnection _shimConn;
        private bool _calledConnOpen;
        private bool _calledConnClose;
        private SqlConnection _conn;
        private bool _calledMinCheckDate;
        private bool _calledMinCheckTime;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();

            _shimConn = new ShimSqlConnection();
            _calledConnOpen = false;
            _shimConn.Open = () => { _calledConnOpen = true; };
            _calledConnClose = false;
            _shimConn.Close = () => { _calledConnClose = true; };
            _conn = _shimConn;

            ShimDataFunctions.GetSqlConnection = () => _conn;
            ShimDataFunctions.GetSqlConnectionString = connStrName => _conn;
        }

        [TearDown]
        public void Teardown()
        {
            _shims?.Dispose();
        }

        [Test]
        public void GetDataSet_RecordsReturned_DataSetFilled()
        {
            // Arrange
            SetupMinDateTimeShim();

            var shimCommand = new ShimSqlCommand();
            SqlConnection calledConnection = null;
            shimCommand.ConnectionSetSqlConnection = connection =>
            {
                calledConnection = connection;
            };

            int? calledCommandTimeout = null;
            shimCommand.CommandTimeoutSetInt32 = commandTimeout =>
            {
                calledCommandTimeout = commandTimeout;
            };

            shimCommand.ConnectionGet = () => _conn;

            ShimDataFunctions.GetSqlConnectionString = connectionStringName => _conn;

            SqlCommand calledDataAdapterConstructorCommand = null;
            ShimSqlDataAdapter.ConstructorSqlCommand = (adapter, command) =>
                {
                    calledDataAdapterConstructorCommand = command;
                };
            
            ShimDbDataAdapter.AllInstances.FillDataSet = (adapter, dataSet) =>
            {
                FillDataSet(dataSet);
                return SampleExecuteReturnValue;
            };

            using (SqlCommand command = shimCommand)
            {
                // Act 
                var resultDataSet = DataFunctions.GetDataSet(command, ConnectionString.UAS.ToString());

                // Assert 
                resultDataSet.ShouldSatisfyAllConditions(
                    () => calledConnection.ShouldBeSameAs(_conn),
                    () => _calledMinCheckDate.ShouldBeTrue(),
                    () => _calledMinCheckTime.ShouldBeTrue(),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledCommandTimeout.ShouldBe(0),
                    () => calledConnection.ShouldBeSameAs(_conn),
                    () => calledDataAdapterConstructorCommand.ShouldBeSameAs(command),
                    () => resultDataSet.Tables[0].TableName.ShouldBe(SampleTableName)
                );
            }
        }

        [Test]
        public void GetDataTable_RecordsReturned_DataTableFilled()
        {
            // Arrange
            SetupMinDateTimeShim();

            var shimCommand = new ShimSqlCommand();

            int? calledCommandTimeout = null;
            shimCommand.CommandTimeoutSetInt32 = commandTimeout =>
            {
                calledCommandTimeout = commandTimeout;
            };

            shimCommand.ConnectionGet = () => _conn;

            ShimDataFunctions.GetSqlConnectionString = connectionStringName => _conn;

            CommandBehavior? commandBehavior = null;
            ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (command, behavior) =>
                {
                    commandBehavior = behavior;
                    return null;
                };

            var calledLoadDataReader = false;
            ShimDataTable.AllInstances.LoadIDataReader = (table, reader) =>
            {
                calledLoadDataReader = true;
                table.TableName = SampleTableName;
            };

            using (SqlCommand command = shimCommand)
            {
                // Act 
                var resultDataTable = DataFunctions.GetDataTable(command);

                // Assert 
                resultDataTable.ShouldSatisfyAllConditions(
                    () => _calledMinCheckDate.ShouldBeTrue(),
                    () => _calledMinCheckTime.ShouldBeTrue(),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => commandBehavior.ShouldBe(CommandBehavior.CloseConnection),
                    () => calledLoadDataReader.ShouldBeTrue(),
                    () => calledCommandTimeout.ShouldBe(0),
                    () => resultDataTable.TableName.ShouldBe(SampleTableName)
                );
            }
        }

        [Test]
        public void GetDataTableWithConn_RecordsReturned_DataTableFilled()
        {
            // Arrange
            SetupMinDateTimeShim();

            var shimCommand = new ShimSqlCommand();
            SqlConnection calledConnection = null;
            shimCommand.ConnectionSetSqlConnection = connection =>
            {
                calledConnection = connection;
            };

            int? calledCommandTimeout = null;
            shimCommand.CommandTimeoutSetInt32 = commandTimeout =>
            {
                calledCommandTimeout = commandTimeout;
            };

            shimCommand.ConnectionGet = () => _conn;

            ShimDataFunctions.GetSqlConnectionString = connectionStringName => _conn;

            CommandBehavior? commandBehavior = null;
            ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (command, behavior) =>
            {
                commandBehavior = behavior;
                return null;
            };

            var calledLoadDataReader = false;
            ShimDataTable.AllInstances.LoadIDataReader = (table, reader) =>
            {
                calledLoadDataReader = true;
                table.TableName = SampleTableName;
            };

            using (SqlCommand command = shimCommand)
            {
                // Act 
                var resultDataTable = DataFunctions.GetDataTable(command, ConnectionString.UAS.ToString());

                // Assert 
                resultDataTable.ShouldSatisfyAllConditions(
                    () => calledConnection.ShouldBeSameAs(_conn),
                    () => _calledMinCheckDate.ShouldBeTrue(),
                    () => _calledMinCheckTime.ShouldBeTrue(),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => commandBehavior.ShouldBe(CommandBehavior.CloseConnection),
                    () => calledLoadDataReader.ShouldBeTrue(),
                    () => calledCommandTimeout.ShouldBe(0),
                    () => resultDataTable.TableName.ShouldBe(SampleTableName)
                );
            }
        }

        [Test]
        public void GetDataTableViaAdapter_RecordsReturned_DataSetFilled()
        {
            // Arrange
            SetupMinDateTimeShim();

            var shimCommand = new ShimSqlCommand();
            SqlConnection calledConnection = null;
            shimCommand.ConnectionSetSqlConnection = connection =>
            {
                calledConnection = connection;
            };

            int? calledCommandTimeout = null;
            shimCommand.CommandTimeoutSetInt32 = commandTimeout =>
            {
                calledCommandTimeout = commandTimeout;
            };

            shimCommand.ConnectionGet = () => _conn;

            ShimDataFunctions.GetSqlConnectionString = connectionStringName => _conn;

            SqlCommand calledDataAdapterConstructorCommand = null;
            ShimSqlDataAdapter.ConstructorSqlCommand = (adapter, command) =>
            {
                calledDataAdapterConstructorCommand = command;
            };

            ShimDbDataAdapter.AllInstances.FillDataTable = (adapter, dataTable) =>
            {
                dataTable.TableName = SampleTableName;
                return SampleExecuteReturnValue;
            };

            using (SqlCommand command = shimCommand)
            {
                // Act 
                var resultDataTable = DataFunctions.GetDataTableViaAdapter(command);

                // Assert 
                resultDataTable.ShouldSatisfyAllConditions(
                    () => _calledMinCheckDate.ShouldBeTrue(),
                    () => _calledMinCheckTime.ShouldBeTrue(),
                    () => calledCommandTimeout.ShouldBe(0),
                    () => calledDataAdapterConstructorCommand.ShouldBeSameAs(command),
                    () => resultDataTable.TableName.ShouldBe(SampleTableName)
                );
            }
        }

        [Test]
        public void GetDataTableViaAdapterWithConnName_RecordsReturned_DataSetFilled()
        {
            // Arrange
            SetupMinDateTimeShim();

            var shimCommand = new ShimSqlCommand();
            SqlConnection calledConnection = null;
            shimCommand.ConnectionSetSqlConnection = connection =>
            {
                calledConnection = connection;
            };

            int? calledCommandTimeout = null;
            shimCommand.CommandTimeoutSetInt32 = commandTimeout =>
            {
                calledCommandTimeout = commandTimeout;
            };

            shimCommand.ConnectionGet = () => _conn;

            ShimDataFunctions.GetSqlConnectionString = connectionStringName => _conn;

            SqlCommand calledDataAdapterConstructorCommand = null;
            ShimSqlDataAdapter.ConstructorSqlCommand = (adapter, command) =>
            {
                calledDataAdapterConstructorCommand = command;
            };

            ShimDbDataAdapter.AllInstances.FillDataTable = (adapter, dataTable) =>
            {
                dataTable.TableName = SampleTableName;
                return SampleExecuteReturnValue;
            };

            using (SqlCommand command = shimCommand)
            {
                // Act 
                var resultDataTable = DataFunctions.GetDataTableViaAdapter(command, ConnectionString.UAS.ToString());

                // Assert 
                resultDataTable.ShouldSatisfyAllConditions(
                    () => calledConnection.ShouldBeSameAs(_conn),
                    () => _calledMinCheckDate.ShouldBeTrue(),
                    () => _calledMinCheckTime.ShouldBeTrue(),
                    () => calledCommandTimeout.ShouldBe(0),
                    () => calledDataAdapterConstructorCommand.ShouldBeSameAs(command),
                    () => resultDataTable.TableName.ShouldBe(SampleTableName)
                );
            }
        }

        [Test]
        public void GetDataTableSql_RecordsReturned_DataTableFilled()
        {
            // Arrange
            SetupMinDateTimeShim();

            var shimCommand = new ShimSqlCommand();

            int? calledCommandTimeout = null;
            ShimSqlCommand.AllInstances.CommandTimeoutSetInt32 = (command, commandTimeout) =>
            {
                calledCommandTimeout = commandTimeout;
            };

            shimCommand.ConnectionGet = () => _conn;

            ShimDataFunctions.GetSqlConnectionString = connectionStringName => _conn;

            CommandBehavior? commandBehavior = null;
            ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (command, behavior) =>
            {
                commandBehavior = behavior;
                return null;
            };

            var calledLoadDataReader = false;
            ShimDataTable.AllInstances.LoadIDataReader = (table, reader) =>
            {
                calledLoadDataReader = true;
                table.TableName = SampleTableName;
            };

            // Act 
            var resultDataTable = DataFunctions.GetDataTable(SampleSqlQuery, SampleConnectionStringName);

            // Assert 
            resultDataTable.ShouldSatisfyAllConditions(
                () => _calledConnOpen.ShouldBeTrue(),
                () => _calledConnClose.ShouldBeTrue(),
                () => commandBehavior.ShouldBe(CommandBehavior.CloseConnection),
                () => calledLoadDataReader.ShouldBeTrue(),
                () => calledCommandTimeout.ShouldBe(0),
                () => resultDataTable.TableName.ShouldBe(SampleTableName)
            );
        }

        [Test]
        public void GetDataTableQueryConn_RecordsReturned_DataTableFilled()
        {
            // Arrange
            SetupMinDateTimeShim();

            var shimCommand = new ShimSqlCommand();

            int? calledCommandTimeout = null;
            ShimSqlCommand.AllInstances.CommandTimeoutSetInt32 = (command, commandTimeout) =>
            {
                calledCommandTimeout = commandTimeout;
            };

            shimCommand.ConnectionGet = () => _conn;

            ShimDataFunctions.GetSqlConnectionString = connectionStringName => _conn;

            SqlConnection calledCommandConnection = null;
            CommandBehavior? commandBehavior = null;
            ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (command, behavior) =>
            {
                calledCommandConnection = command.Connection;
                commandBehavior = behavior;
                return null;
            };

            var calledLoadDataReader = false;
            ShimDataTable.AllInstances.LoadIDataReader = (table, reader) =>
            {
                calledLoadDataReader = true;
                table.TableName = SampleTableName;
            };

            // Act 
            var resultDataTable = DataFunctions.GetDataTable(SampleSqlQuery, _conn);

            // Assert 
            resultDataTable.ShouldSatisfyAllConditions(
                () => _calledConnOpen.ShouldBeTrue(),
                () => _calledConnClose.ShouldBeTrue(),
                () => calledCommandConnection.ShouldBeSameAs(_conn),
                () => commandBehavior.ShouldBe(CommandBehavior.CloseConnection),
                () => calledLoadDataReader.ShouldBeTrue(),
                () => calledCommandTimeout.ShouldBe(0),
                () => resultDataTable.TableName.ShouldBe(SampleTableName)
            );
        }

        [Test]
        public void Execute_SetQuery_CalledExecuteNonQuery()
        {
            // Arrange
            SetupMinDateTimeShim();

            ShimSqlCommand.AllInstances.ConnectionGet = command => _conn;
            int? calledCommandTimeout = null;
            ShimSqlCommand.AllInstances.CommandTimeoutSetInt32 = (command, commandTimeout) =>
            {
                calledCommandTimeout = commandTimeout;
            };

            ShimSqlCommand.AllInstances.ExecuteNonQuery = (command) => SampleExecuteReturnValue;

            using (SqlCommand command = new SqlCommand())
            {
                // Act
                var result = DataFunctions.Execute(SampleSqlQuery);

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBe(SampleExecuteReturnValue),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledCommandTimeout.ShouldBe(0)
                );
            }
        }

        [Test]
        public void Execute_SetCmd_CalledExecuteNonQuery()
        {
            // Arrange
            SetupMinDateTimeShim();

            ShimSqlCommand.AllInstances.ConnectionGet = command => _conn;
            int? calledCommandTimeout = null;
            ShimSqlCommand.AllInstances.CommandTimeoutSetInt32 = (command, commandTimeout) =>
            {
                calledCommandTimeout = commandTimeout;
            };

            ShimSqlCommand.AllInstances.ExecuteNonQuery = (command) => SampleExecuteReturnValue;

            using (SqlCommand command = new SqlCommand())
            {
                // Act
                var result = DataFunctions.Execute(command);

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBe(SampleExecuteReturnValue),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledCommandTimeout.ShouldBe(0)
                );
            }
        }

        [Test]
        public void Execute_SetCmdConn_Executed()
        {
            // Arrange
            SetupMinDateTimeShim();

            var shimSqlCommand = new ShimSqlCommand();
            SqlConnection calledSetConnection = null;
            shimSqlCommand.ConnectionSetSqlConnection = connection => { calledSetConnection = connection; };
            var calledExecuteNonQuery = false;
            shimSqlCommand.ExecuteNonQuery = () =>
            {
                calledExecuteNonQuery = true;
                return SampleExecuteReturnValue;
            };
            using (SqlCommand command = shimSqlCommand)
            {
                // Act
                var result = DataFunctions.Execute(command, _conn);

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBe(SampleExecuteReturnValue),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledExecuteNonQuery.ShouldBeTrue(),
                    () => calledSetConnection.ShouldBeSameAs(_conn)
                );
            }
        }

        [Test]
        public void Execute_SetQueryConnNameStr_Executed()
        {
            // Arrange
            SqlCommand calledCommand = null;
            ShimSqlCommand.AllInstances.ExecuteNonQuery = command =>
            {
                calledCommand = command;
                return SampleExecuteReturnValue;
            };

            try
            {
                // Act
                var result = DataFunctions.Execute(SampleSqlQuery, SampleConnectionStringName);

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBe(SampleExecuteReturnValue),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledCommand.CommandText.ShouldBe(SampleSqlQuery),
                    () => calledCommand.Connection.ShouldBeSameAs(_conn)
                );
            }
            finally
            {
                calledCommand?.Dispose();
            }
        }

        [Test]
        public void Execute_SetCmdConnNameStr_Executed()
        {
            // Arrange
            SetupMinDateTimeShim();

            ShimSqlCommand.AllInstances.ConnectionGet = command => _conn;
            int? calledCommandTimeout = null;
            ShimSqlCommand.AllInstances.CommandTimeoutSetInt32 = (command, commandTimeout) =>
            {
                calledCommandTimeout = commandTimeout;
            };

            ShimSqlCommand.AllInstances.ExecuteNonQuery = (command) => SampleExecuteReturnValue;

            SqlCommand calledCommand = null;
            ShimSqlCommand.AllInstances.ExecuteNonQuery = command =>
            {
                calledCommand = command;
                return SampleExecuteReturnValue;
            };

            using (SqlCommand command = new SqlCommand())
            {
                // Act
                var result = DataFunctions.Execute(command, SampleConnectionStringName);

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBe(SampleExecuteReturnValue),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledCommand.ShouldBeSameAs(command),
                    () => calledCommandTimeout.ShouldBe(0)
                );
            }
        }

        [Test]
        public void Execute_SetSqlConn_Executed()
        {
            // Arrange
            string calledQuery = null;
            SqlConnection calledConnection = null;
            SqlCommand sqlCommand = null;
            ShimSqlCommand.ConstructorStringSqlConnection = (command, query, connection) =>
            {
                sqlCommand = command;
                command.Connection = connection;
                calledQuery = query;
                calledConnection = connection;
            };

            var calledExecuteNonQuery = false;
            ShimSqlCommand.AllInstances.ExecuteNonQuery = command =>
            {
                calledExecuteNonQuery = true;
                return SampleExecuteReturnValue;
            };

            try
            {
                // Act
                var result = DataFunctions.Execute(SampleSqlQuery, _conn);

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBe(SampleExecuteReturnValue),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledExecuteNonQuery.ShouldBeTrue()
                );
            }
            finally
            {
                sqlCommand?.Dispose();
            }
        }

        [Test]
        public void ExecuteScalar_SetCmd_Executed()
        {
            // Arrange
            SetupMinDateTimeShim();

            var shimSqlCommand = new ShimSqlCommand();
            SqlConnection calledSetConnection = null;
            shimSqlCommand.ConnectionSetSqlConnection = connection => { calledSetConnection = connection; };
            shimSqlCommand.ConnectionGet = () => _conn;
            var calledExecuteScalar = false;
            shimSqlCommand.ExecuteScalar = () =>
            {
                calledExecuteScalar = true;
                return SampleExecuteReturnValue;
            };
            using (SqlCommand command = shimSqlCommand)
            {
                // Act
                var result = DataFunctions.ExecuteScalar(command);

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBe(SampleExecuteReturnValue),
                    () => _calledMinCheckDate.ShouldBeTrue(),
                    () => _calledMinCheckTime.ShouldBeTrue(),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledExecuteScalar.ShouldBeTrue(),
                    () => calledSetConnection.ShouldBeSameAs(_conn)
                );
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ExecuteScalar_SetCmd_Executed(bool setDefaultConnection)
        {
            // Arrange
            SetupMinDateTimeShim();

            var shimSqlCommand = new ShimSqlCommand();
            SqlConnection calledSetConnection = null;
            shimSqlCommand.ConnectionSetSqlConnection = connection => { calledSetConnection = connection; };
            shimSqlCommand.ConnectionGet = () => _conn;
            var calledExecuteScalar = false;
            shimSqlCommand.ExecuteScalar = () =>
            {
                calledExecuteScalar = true;
                return SampleExecuteReturnValue;
            };
            using (SqlCommand command = shimSqlCommand)
            {
                // Act
                var result = DataFunctions.ExecuteScalar(command, setDefaultConnection);

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBe(SampleExecuteReturnValue),
                    () => _calledMinCheckDate.ShouldBeTrue(),
                    () => _calledMinCheckTime.ShouldBeTrue(),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledExecuteScalar.ShouldBeTrue()
                );

                if (setDefaultConnection)
                {
                    calledSetConnection.ShouldBeSameAs(_conn);
                }
            }
        }

        [Test]
        public void ExecuteScalar_SetCmdConn_Executed()
        {
            // Arrange
            SetupMinDateTimeShim();

            var shimSqlCommand = new ShimSqlCommand();
            SqlConnection calledSetConnection = null;
            shimSqlCommand.ConnectionSetSqlConnection = connection => { calledSetConnection = connection; };
            shimSqlCommand.ConnectionGet = () => _conn;
            var calledExecuteScalar = false;
            shimSqlCommand.ExecuteScalar = () =>
            {
                calledExecuteScalar = true;
                return SampleExecuteReturnValue;
            };

            using (SqlCommand command = shimSqlCommand)
            {
                // Act
                var result = DataFunctions.ExecuteScalar(command, _conn);

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBe(SampleExecuteReturnValue),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledExecuteScalar.ShouldBeTrue(),
                    () => calledSetConnection.ShouldBeSameAs(_conn)
                );
            }
        }

        [Test]
        public void ExecuteScalar_SetCmdConnStrName_Executed()
        {
            // Arrange
            SetupMinDateTimeShim();

            var shimSqlCommand = new ShimSqlCommand();
            SqlConnection calledSetConnection = null;
            shimSqlCommand.ConnectionSetSqlConnection = connection => { calledSetConnection = connection; };
            shimSqlCommand.ConnectionGet = () => _conn;
            var calledExecuteScalar = false;
            shimSqlCommand.ExecuteScalar = () =>
            {
                calledExecuteScalar = true;
                return SampleExecuteReturnValue;
            };
            using (SqlCommand command = shimSqlCommand)
            {
                // Act
                var result = DataFunctions.ExecuteScalar(command, SampleConnectionStringName);

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBe(SampleExecuteReturnValue),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledExecuteScalar.ShouldBeTrue(),
                    () => calledSetConnection.ShouldBeSameAs(_conn)
                );
            }
        }

        [Test]
        public void ExecuteScalar_SetSqlConn_Executed()
        {
            // Arrange
            string calledQuery = null;
            SqlConnection calledConnection = null;
            SqlCommand sqlCommand = null;
            ShimSqlCommand.AllInstances.ConnectionSetSqlConnection = (command, connection) =>
            {
                calledConnection = connection;
            };

            ShimSqlCommand.AllInstances.ConnectionGet = command => { return calledConnection; };

            ShimSqlCommand.ConstructorString = (command, query) =>
            {
                calledQuery = query;
            };

            var calledExecuteScalar = false;
            ShimSqlCommand.AllInstances.ExecuteScalar = command =>
            {
                calledExecuteScalar = true;
                return SampleExecuteReturnValue;
            };

            // Act
            var result = DataFunctions.ExecuteScalar(SampleSqlQuery, _conn);

            // Assert
            result.ShouldBe(SampleExecuteReturnValue);
            _calledConnOpen.ShouldBeTrue();
            _calledConnClose.ShouldBeTrue();

            calledExecuteScalar.ShouldBeTrue();
            calledConnection.ShouldBeSameAs(_conn);
            calledQuery.ShouldBe(SampleSqlQuery);
        }

        [Test]
        public void ExecuteNonQuery_SetSqlConnName_Executed()
        {
            // Arrange
            SetupMinDateTimeShim();

            string calledSqlQuery = null;
            SqlCommand calledCommand = null;
            ShimSqlCommand.ConstructorString = (command, sqlQuery) =>
            {
                calledSqlQuery = sqlQuery;
                command.CommandText = calledSqlQuery;
                calledCommand = command;
            };

            var calledExecuteNonQuery = false;
            ShimSqlCommand.AllInstances.ExecuteNonQuery = command =>
            {
                calledCommand = command;
                calledExecuteNonQuery = true;

                return SampleExecuteReturnValue;
            };

            var calledGetSqlConnectionString = false;
            string calledConnStrName = null;
            ShimDataFunctions.GetSqlConnectionString = connStrName =>
            {
                calledConnStrName = connStrName;
                calledGetSqlConnectionString = true;
                return _conn;
            };

            try
            {
                // Act
                var result = DataFunctions.ExecuteNonQuery(SampleSqlQuery, SampleConnectionStringName);

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBe(true),
                    () => calledConnStrName.ShouldBe(SampleConnectionStringName),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledExecuteNonQuery.ShouldBeTrue(),
                    () => calledCommand.CommandText.ShouldBe(SampleSqlQuery)
                );
            }
            finally
            {
                calledCommand?.Dispose();
            }
        }

        [Test]
        public void ExecuteNonQuery_SetCmdConnName_Executed()
        {
            // Arrange
            SetupMinDateTimeShim();

            using (var sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = _conn;
                sqlCommand.Parameters.AddWithValue(FieldId, 1);

                var calledExecuteNonQuery = false;
                SqlCommand calledCommand = null;
                ShimSqlCommand.AllInstances.ExecuteNonQuery = command =>
                {
                    calledCommand = command;
                    calledExecuteNonQuery = true;
                    return SampleExecuteReturnValue;
                };

                var calledGetSqlConnectionString = false;
                string calledConnStrName = null;
                ShimDataFunctions.GetSqlConnectionString = connStrName =>
                {
                    calledConnStrName = connStrName;
                    calledGetSqlConnectionString = true;
                    return _conn;
                };

                // Act
                var result = DataFunctions.ExecuteNonQuery(sqlCommand, SampleConnectionStringName);

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBe(true),
                    () => calledConnStrName.ShouldBe(SampleConnectionStringName),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => _calledMinCheckDate.ShouldBeTrue(),
                    () => _calledMinCheckTime.ShouldBeTrue(),
                    () => calledExecuteNonQuery.ShouldBeTrue(),
                    () => calledCommand.ShouldBeSameAs(sqlCommand)
                ); 
            }
        }

        [Test]
        public void ExecuteNonQuery_SetCmd_Executed()
        {
            // Arrange
            using (var sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = _conn;
                sqlCommand.Parameters.AddWithValue(FieldId, 1);

                var calledExecuteNonQuery = false;
                SqlCommand calledCommand = null;
                ShimSqlCommand.AllInstances.ExecuteNonQuery = command =>
                {
                    calledCommand = command;
                    calledExecuteNonQuery = true;
                    return SampleExecuteReturnValue;
                };

                // Act
                var result = DataFunctions.ExecuteNonQuery(sqlCommand);

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBe(true),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledExecuteNonQuery.ShouldBeTrue(),
                    () => calledCommand.ShouldBeSameAs(sqlCommand)
                );
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ExecuteReader_SetSqlConnStrName_Executed(bool readerHasRows)
        {
            // Arrange
            string calledQuery = null;
            SqlConnection calledConnection = null;
            SqlCommand sqlCommand = null;
            ShimSqlCommand.ConstructorStringSqlConnection = (command, query, connection) =>
            {
                sqlCommand = command;
                command.Connection = connection;
                calledQuery = query;
                calledConnection = connection;
            };

            ShimSqlCommand.AllInstances.ConnectionSetSqlConnection = (command, connection) =>
            {
                calledConnection = connection;
            };

            ShimSqlCommand.AllInstances.ConnectionGet = command => calledConnection;

            ShimSqlCommand.ConstructorString = (command, query) =>
            {
                calledQuery = query;
            };

            var shimReader = new ShimSqlDataReader();
            shimReader.HasRowsGet = () => readerHasRows;
            SqlDataReader mockReader = shimReader;
            ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (command, behavior) =>
            {
                if (behavior == CommandBehavior.CloseConnection)
                {
                    _calledConnClose = true;
                }
                return mockReader;
            };

            var calledGetSqlConnectionString = false;
            string calledConnStrName = null;
            ShimDataFunctions.GetSqlConnectionString = connStrName =>
            {
                calledConnStrName = connStrName;
                calledGetSqlConnectionString = true;
                return _conn;
            };

            // Act
            var result = DataFunctions.ExecuteReader(SampleSqlQuery, SampleConnectionStringName);

            // Assert
            if (readerHasRows)
            {
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBeSameAs(mockReader),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledConnection.ShouldBeSameAs(_conn),
                    () => calledQuery.ShouldBe(SampleSqlQuery)
                );
            }
            else
            {
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBeNull(),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledConnection.ShouldBeSameAs(_conn),
                    () => calledQuery.ShouldBe(SampleSqlQuery)
                );
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ExecuteReader_SetCmdConnStrName_Executed(bool readerHasRows)
        {
            // Arrange
            SetupMinDateTimeShim();

            var shimSqlCommand = new ShimSqlCommand();
            SqlConnection calledSetConnection = null;
            shimSqlCommand.ConnectionGet = () => _conn;
            shimSqlCommand.ConnectionSetSqlConnection = connection => { calledSetConnection = connection; };
            shimSqlCommand.ExecuteScalar = () => SampleExecuteReturnValue;

            var shimReader = new ShimSqlDataReader();
            shimReader.HasRowsGet = () => readerHasRows;
            SqlDataReader mockReader = shimReader;
            var calledExecuteReader = false;
            ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (command, behavior) =>
            {
                calledExecuteReader = true;
                if (behavior == CommandBehavior.CloseConnection)
                {
                    _calledConnClose = true;
                }
                return mockReader;
            };

            using (SqlCommand command = shimSqlCommand)
            {
                // Act
                var result = DataFunctions.ExecuteReader(command, SampleConnectionStringName);

                // Assert
                if (readerHasRows)
                {
                    result.ShouldBeSameAs(mockReader);
                }
                else
                {
                    result.ShouldBeNull();
                }

                result.ShouldSatisfyAllConditions(
                    () => _calledMinCheckDate.ShouldBeTrue(),
                    () => _calledMinCheckTime.ShouldBeTrue(),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledExecuteReader.ShouldBeTrue()
                );
            }
        }

        [Test]
        public void ExecuteReader_SetCmdConn_Executed()
        {
            // Arrange
            var shimSqlCommand = new ShimSqlCommand();
            SqlConnection calledSetConnection = null;
            shimSqlCommand.ConnectionSetSqlConnection = connection =>
            {
                calledSetConnection = connection;
            };

            shimSqlCommand.ConnectionGet = () => calledSetConnection;

            var shimReader = new ShimSqlDataReader();
            SqlDataReader mockReader = shimReader;
            shimSqlCommand.ExecuteReaderCommandBehavior = behavior =>
            {
                if (behavior == CommandBehavior.CloseConnection)
                {
                    _calledConnClose = true;
                }
                return mockReader;
            };

            using (SqlCommand command = shimSqlCommand)
            {
                // Act
                var result = DataFunctions.ExecuteReader(command, _conn);

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldNotBeNull(),
                    () => result.ShouldBeSameAs(mockReader),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledSetConnection.ShouldBeSameAs(_conn)
                );
            }
        }

        [Test]
        public void ExecuteReader_SetCmd_Executed()
        {
            // Arrange
            SetupMinDateTimeShim();

            var shimSqlCommand = new ShimSqlCommand();
            SqlConnection calledSetConnection = null;
            shimSqlCommand.ConnectionSetSqlConnection = connection =>
            {
                calledSetConnection = connection;
            };

            shimSqlCommand.ConnectionGet = () => calledSetConnection;

            var shimReader = new ShimSqlDataReader();
            SqlDataReader mockReader = shimReader;
            shimSqlCommand.ExecuteReaderCommandBehavior = behavior =>
            {
                if (behavior == CommandBehavior.CloseConnection)
                {
                    _calledConnClose = true;
                }
                return mockReader;
            };

            using (SqlCommand command = shimSqlCommand)
            {
                // Act
                var result = DataFunctions.ExecuteReader(command);

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldNotBeNull(),
                    () => result.ShouldBeSameAs(mockReader),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledSetConnection.ShouldBeSameAs(_conn)
                );
            }
        }

        [Test]
        public void ExecuteReader_SetSqlConn_Executed()
        {
            // Arrange
            string calledQuery = null;
            SqlConnection calledConnection = null;
            SqlCommand sqlCommand = null;
            ShimSqlCommand.ConstructorStringSqlConnection = (command, query, connection) =>
            {
                sqlCommand = command;
                command.Connection = connection;
                calledQuery = query;
                calledConnection = connection;
            };

            ShimSqlCommand.AllInstances.ConnectionSetSqlConnection = (command, connection) =>
            {
                calledConnection = connection;
            };

            ShimSqlCommand.AllInstances.ConnectionGet = command => calledConnection;

            ShimSqlCommand.ConstructorString = (command, query) =>
            {
                calledQuery = query;
            };

            var shimReader = new ShimSqlDataReader();
            SqlDataReader mockReader = shimReader;
            ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (command, behavior) =>
            {
                if (behavior == CommandBehavior.CloseConnection)
                {
                    _calledConnClose = true;
                }
                return mockReader;
            };

            try
            {
                // Act
                var result = DataFunctions.ExecuteReader(SampleSqlQuery, _conn);

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldNotBeNull(),
                    () => result.ShouldBeSameAs(mockReader),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledConnection.ShouldBeSameAs(_conn),
                    () => calledQuery.ShouldBe(SampleSqlQuery)
                );
            }
            finally
            {
                mockReader?.Dispose();
                sqlCommand?.Dispose();
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ExecuteReaderNullIfEmpty_SetCmdConnStrName_Executed(bool readerHasRows)
        {
            // Arrange
            SetupMinDateTimeShim();

            var shimSqlCommand = new ShimSqlCommand();
            shimSqlCommand.ConnectionGet = () => _conn;

            var shimReader = new ShimSqlDataReader();
            shimReader.HasRowsGet = () => readerHasRows;
            SqlDataReader mockReader = shimReader;
            var calledExecuteReader = false;
            ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (command, behavior) =>
            {
                calledExecuteReader = true;
                if (behavior == CommandBehavior.CloseConnection)
                {
                    _calledConnClose = true;
                }
                return mockReader;
            };

            using (SqlCommand command = shimSqlCommand)
            {
                // Act
                var result = DataFunctions.ExecuteReaderNullIfEmpty(command);

                // Assert
                if (readerHasRows)
                {
                    result.ShouldBeSameAs(mockReader);
                }
                else
                {
                    result.ShouldBeNull();
                }

                result.ShouldSatisfyAllConditions(
                    () => _calledMinCheckDate.ShouldBeTrue(),
                    () => _calledMinCheckTime.ShouldBeTrue(),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledExecuteReader.ShouldBeTrue()
                );
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ExecuteReader_SetCmdConnStrNameDbName_Executed(bool readerHasRows)
        {
            // Arrange
            SetupMinDateTimeShim();

            ShimDataFunctions.GetSqlConnectionString = connStrName => new SqlConnection(SampleConnectionMaster);

            var shimSqlCommand = new ShimSqlCommand();
            shimSqlCommand.ConnectionGet = () => _conn;
            string calledConnectionStr = null;
            shimSqlCommand.ConnectionSetSqlConnection = connection =>
            {
                calledConnectionStr = connection.ConnectionString;
            };

            var shimReader = new ShimSqlDataReader();
            shimReader.HasRowsGet = () => readerHasRows;
            SqlDataReader mockReader = shimReader;
            var calledExecuteReader = false;
            
            ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (command, behavior) =>
            {
                calledExecuteReader = true;
                if (behavior == CommandBehavior.CloseConnection)
                {
                    _calledConnClose = true;
                }
                return mockReader;
            };

            using (SqlCommand command = shimSqlCommand)
            {
                // Act
                var result = DataFunctions.ExecuteReader(command, SampleConnectionStringName, DbNameDev);

                // Assert
                if (readerHasRows)
                {
                    result.ShouldBeSameAs(mockReader);
                }
                else
                {
                    result.ShouldBeNull();
                }

                result.ShouldSatisfyAllConditions(
                    () => _calledMinCheckDate.ShouldBeTrue(),
                    () => _calledMinCheckTime.ShouldBeTrue(),
                    () => _calledConnOpen.ShouldBeTrue(),
                    () => _calledConnClose.ShouldBeTrue(),
                    () => calledExecuteReader.ShouldBeTrue(),
                    () => calledConnectionStr.ShouldBe(SampleConnectionDev)
                );
            }
        }

        [Test]
        public void MinDateCheck_Cmd_Modified()
        {
            // Arrange
            var minDate = WebFunctions.GetMinDate();

            using (var sqlCommand = new SqlCommand())
            {
                sqlCommand.Parameters.AddWithValue(SampleDateTimeParameter, DateTime.MinValue);

                // Act
                var resultCommand = DataFunctions.MinDateCheck(sqlCommand);

                // Assert
                resultCommand.Parameters[SampleDateTimeParameter].Value.ShouldBe(minDate.ToString());
            }
        }

        [Test]
        public void MinTimeCheck_Cmd_Modified()
        {
            // Arrange
            var minTime = WebFunctions.GetMinTime();

            using (var sqlCommand = new SqlCommand())
            {
                sqlCommand.Parameters.AddWithValue(SampleDateTimeParameter, TimeSpan.MinValue);

                // Act
                var resultCommand = DataFunctions.MinTimeCheck(sqlCommand);

                // Assert
                resultCommand.Parameters[SampleDateTimeParameter].Value.ShouldBe(minTime.ToString());
            }
        }

        [Test]
        public void GetList_DataRecord_Record()
        {
            // Arrange
            var sqlCommand = new SqlCommand(SampleSqlQuery, _conn);

            var shimSqlDataReader = new ShimSqlDataReader();
            var readCnt = 2;
            shimSqlDataReader.Read = () => --readCnt >= 0;

            SqlDataReader sqlDataReader = shimSqlDataReader;

            ShimDataFunctions.ExecuteReaderSqlCommandString = (command, connectionStringName) => sqlDataReader;

            var shimBuilder = new ShimDynamicBuilder<Record>();
            shimBuilder.BuildIDataRecord = dataRecord =>
            {
                var newRecord = new Record();
                if (readCnt == 1)
                {
                    newRecord.Id = FieldIdValue1;
                    newRecord.Done = FieldDoneValue1;
                }
                else
                {
                    newRecord.Id = FieldIdValue2;
                    newRecord.Done = FieldDoneValue2;
                }
                return newRecord;
            };
            ShimDynamicBuilder<Record>.CreateBuilderIDataRecord = dataRecord => shimBuilder;

            try
            {
                // Act
                var resultRecords = DataFunctions.GetList<Record>(sqlCommand, ConnectionString.UAS.ToString());

                // Assert
                resultRecords.ShouldSatisfyAllConditions(
                    () => resultRecords.ShouldNotBeNull(),
                    () => resultRecords.Count.ShouldBe(2),
                    () => resultRecords[0].Id.ShouldBe(FieldIdValue1),
                    () => resultRecords[0].Done.ShouldBe(FieldDoneValue1),
                    () => resultRecords[1].Id.ShouldBe(FieldIdValue2),
                    () => resultRecords[1].Done.ShouldBe(FieldDoneValue2),
                    () => _calledConnOpen.ShouldBeFalse(),
                    () => _calledConnClose.ShouldBeTrue());
            }
            finally
            {
                sqlDataReader?.Dispose();
                sqlCommand?.Dispose();
            }
        }

        [Test]
        [TestCaseSource(nameof(ConnectionStringData))]
        public void GetSqlConnection_UADMaster_Replaced(
            string connectionStringName,
            string connectionString,
            bool isDemo,
            bool isNetworkDeployed,
            string expectedConnectionString)
        {
            // Arrange
            ShimConfigurationManager.ConnectionStringsGet = () =>
            {
                var connectionStrings = new ConnectionStringSettingsCollection();
                connectionStrings.Add(
                    new ConnectionStringSettings(
                        connectionStringName,
                        connectionString));
                return connectionStrings;
            };

            ShimConfigurationManager.AppSettingsGet = () =>
            {
                var settings = new NameValueCollection();

                settings.Add(DataFunctions.AppSettingIsDemo, isDemo.ToString());
                settings.Add(DataFunctions.AppSettingIsNetworkDeployed, isNetworkDeployed.ToString());

                return settings;
            };

            ShimDataFunctions.GetSqlConnectionString = null;

            // Act
            var connection = DataFunctions.GetSqlConnection(connectionStringName);

            // Assert
            connection.ConnectionString.ShouldBe(expectedConnectionString);
        }

        public static IEnumerable ConnectionStringData()
        {
            yield return new TestCaseData(
                ConnectionString.UAS.ToString(),
                SampleConnectionString241,
                false,
                false,
                SampleConnectionString251);

            yield return new TestCaseData(
                ConnectionString.UAD_Lookup.ToString(),
                SampleConnectionString241,
                false,
                false,
                SampleConnectionString198);

            yield return new TestCaseData(
                ConnectionString.UAD_Lookup.ToString(),
                SampleConnectionString241,
                true,
                false,
                SampleConnectionString241);

            yield return new TestCaseData(
                ConnectionString.UAD_Lookup.ToString(),
                SampleConnectionString21617,
                false,
                true,
                SampleConnectionString1010);
        }

        [Test]
        [TestCaseSource(nameof(CleanSerializedXmlSource))]
        public void CleanSerializedXML_XmlHasHeaderAndNs_Replaced(string xml, string ns)
        {
            // Arrange, Act
            var resultXml = DataFunctions.CleanSerializedXML(xml);

            // Assert
            resultXml.ShouldSatisfyAllConditions(
                () => resultXml.ShouldNotContain(DataFunctions.XmlHeader),
                () => resultXml.ShouldNotContain(ns)
            );
        }

        private static IEnumerable CleanSerializedXmlSource()
        {
            yield return new TestCaseData(SampleXmlKmPlatformEntity, DataFunctions.XmlNsKMPlatformEntity);
            yield return new TestCaseData(SampleXmlFrameworkUas, DataFunctions.XmlNsFrameworkUasEntity);
        }

        [Test]
        [TestCaseSource(nameof(CleanStringXmlSource))]
        public void CleanString_HasReplacement_Replaced(string dirtyString, string expected)
        {
            // Arrange, Act
            var result = DataFunctions.CleanString(dirtyString);

            // Assert
            result.ShouldBe(expected);
        }

        private static IEnumerable CleanStringXmlSource()
        {
            yield return new TestCaseData(CleanStringDirty, CleanStringCleaned);
        }

        private void SetupMinDateTimeShim()
        {
            _calledMinCheckDate = false;
            ShimDataFunctions.MinDateCheckSqlCommand = command =>
            {
                _calledMinCheckDate = true;
                return command;
            };

            _calledMinCheckTime = false;
            ShimDataFunctions.MinTimeCheckSqlCommand = command =>
            {
                _calledMinCheckTime = true;
                return command;
            };
        }

        private static void FillDataSet(DataSet dataSet)
        {
            var dataTable = new DataTable(SampleTableName);
            dataSet.Tables.Add(dataTable);
        }
    }
}

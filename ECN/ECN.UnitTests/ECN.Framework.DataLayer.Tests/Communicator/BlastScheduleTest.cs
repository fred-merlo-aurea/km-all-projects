using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_DataLayer;
using ECN_Framework_DataLayer.Communicator;
using Ecn.Framework.DataLayer.Interfaces;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.DataLayer.Tests
{
    [TestFixture]
    public class BlastScheduleTest
    {
        private const int ValueGreaterThanZero = 1;
        private const int ValueLessThanZero = -1;
        private string _connectionString = string.Empty;
        private Mock<IDatabaseAdapter> _database;

        [SetUp]
        public void SetUp()
        {
            _database = new Mock<IDatabaseAdapter>();
            _database.Setup(x => x.OpenConnection(It.IsAny<IDbConnection>()))
                .Callback<IDbConnection>((conn) => _connectionString = conn.ConnectionString);
            _database.Setup(x => x.CreateCommand(It.IsAny<IDbConnection>())).Returns(new SqlCommand());
            _database.Setup(x => x.AddParameterWithValue(It.IsAny<IDbCommand>(), It.IsAny<string>(), It.IsAny<object>()));
            _database.Setup(x => x.BeginTransaction(It.IsAny<IDbConnection>(), It.IsAny<string>()));
            _database.Setup(x => x.CommitTransaction(It.IsAny<IDbTransaction>()));
        }

        [Test]
        public void Delete_NoException_Executes3QueriesAndCallsAddParameterWithValue4Times()
        {
            // Arrange
            _database.Setup(x => x.ExecuteNonQuery(It.IsAny<IDbCommand>())).Returns(0);
            BlastSchedule.Initialize(_database.Object);

            // Act
            BlastSchedule.Delete(0);

            // Assert
            _connectionString.ShouldBe(ConfigurationManager.ConnectionStrings[DataFunctions.ConnectionString.Communicator.ToString()].ToString());
            _database.VerifyAll();
            _database.Verify(x => x.ExecuteNonQuery(It.IsAny<IDbCommand>()), Times.Exactly(3));
            _database.Verify(x => x.AddParameterWithValue(It.IsAny<IDbCommand>(), It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(4));
        }

        [Test]
        public void Delete_ExceptionThrownWhenExecuteNonQuery_CallsRollback()
        {
            // Arrange
            _database.Setup(x => x.ExecuteNonQuery(It.IsAny<IDbCommand>())).Throws(new Exception());
            _database.Setup(x => x.RollbackTransaction(It.IsAny<IDbTransaction>()));
            BlastSchedule.Initialize(_database.Object);

            // Act
            BlastSchedule.Delete(0);

            // Assert
            _connectionString.ShouldBe(ConfigurationManager.ConnectionStrings[DataFunctions.ConnectionString.Communicator.ToString()].ToString());
            _database.Verify(x => x.CommitTransaction(It.IsAny<IDbTransaction>()), Times.Never());

            _database.Verify(x => x.RollbackTransaction(It.IsAny<IDbTransaction>()), Times.Once());
            _database.Verify(x => x.OpenConnection(It.IsAny<IDbConnection>()), Times.Once());
            _database.Verify(x => x.CreateCommand(It.IsAny<IDbConnection>()), Times.Once());
            _database.Verify(x => x.BeginTransaction(It.IsAny<IDbConnection>(), It.IsAny<string>()), Times.Once());

            _database.Verify(x => x.AddParameterWithValue(It.IsAny<IDbCommand>(), It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(2));
        }

        [Test]
        public void Update_NoExceptionAndTwoBlastScheduleDays_Executes4QueriesAndCallsAddParameterWithValue20Times()
        {
            // Arrange
            _database.Setup(x => x.ExecuteScalar(It.IsAny<IDbCommand>())).Returns(ValueGreaterThanZero);
            _database.Setup(x => x.ExecuteNonQuery(It.IsAny<IDbCommand>())).Returns(0);
            BlastSchedule.Initialize(_database.Object);
            var blastSchedule = new ECN_Framework_Entities.Communicator.BlastSchedule()
            {
                DaysList = new List<ECN_Framework_Entities.Communicator.BlastScheduleDays>()
                {
                    new ECN_Framework_Entities.Communicator.BlastScheduleDays(),
                    new ECN_Framework_Entities.Communicator.BlastScheduleDays()
                }
            };

            // Act
            var actual = BlastSchedule.Update(blastSchedule, 0);

            // Assert
            actual.ShouldBe(ValueGreaterThanZero);
            _connectionString.ShouldBe(ConfigurationManager.ConnectionStrings[DataFunctions.ConnectionString.Communicator.ToString()].ToString());
            _database.VerifyAll();
            _database.Verify(x => x.ExecuteNonQuery(It.IsAny<IDbCommand>()), Times.Exactly(4));
            _database.Verify(x => x.AddParameterWithValue(It.IsAny<IDbCommand>(), It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(20));
        }

        [Test]
        public void Update_ExecuteScalarReturnedNegativeValue_CallsRollbackAndReturnsNegativeValue()
        {
            // Arrange
            _database.Setup(x => x.ExecuteScalar(It.IsAny<IDbCommand>())).Returns(ValueLessThanZero);
            _database.Setup(x => x.RollbackTransaction(It.IsAny<IDbTransaction>()));
            BlastSchedule.Initialize(_database.Object);

            // Act
            var actual = BlastSchedule.Update(
                new ECN_Framework_Entities.Communicator.BlastSchedule(),
                0);

            // Assert
            actual.ShouldBe(ValueLessThanZero);
            _connectionString.ShouldBe(ConfigurationManager.ConnectionStrings[DataFunctions.ConnectionString.Communicator.ToString()].ToString());
            _database.Verify(x => x.CommitTransaction(It.IsAny<IDbTransaction>()), Times.Never());

            _database.Verify(x => x.RollbackTransaction(It.IsAny<IDbTransaction>()), Times.Once());
            _database.Verify(x => x.OpenConnection(It.IsAny<IDbConnection>()), Times.Once());
            _database.Verify(x => x.CreateCommand(It.IsAny<IDbConnection>()), Times.Once());
            _database.Verify(x => x.BeginTransaction(It.IsAny<IDbConnection>(), It.IsAny<string>()), Times.Once());

            _database.Verify(x => x.AddParameterWithValue(It.IsAny<IDbCommand>(), It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(2));
        }

        [Test]
        public void Update_ExceptionThrownWhenExecuteNonQuery_CallsRollbackAndReturnsZero()
        {
            // Arrange
            _database.Setup(x => x.ExecuteScalar(It.IsAny<IDbCommand>())).Throws(new Exception());
            _database.Setup(x => x.RollbackTransaction(It.IsAny<IDbTransaction>()));
            BlastSchedule.Initialize(_database.Object);

            // Act
            var actual = BlastSchedule.Update(
                new ECN_Framework_Entities.Communicator.BlastSchedule(),
                0);

            // Assert
            actual.ShouldBe(0);
            _connectionString.ShouldBe(ConfigurationManager.ConnectionStrings[DataFunctions.ConnectionString.Communicator.ToString()].ToString());
            _database.Verify(x => x.CommitTransaction(It.IsAny<IDbTransaction>()), Times.Never());

            _database.Verify(x => x.RollbackTransaction(It.IsAny<IDbTransaction>()), Times.Once());
            _database.Verify(x => x.OpenConnection(It.IsAny<IDbConnection>()), Times.Once());
            _database.Verify(x => x.CreateCommand(It.IsAny<IDbConnection>()), Times.Once());
            _database.Verify(x => x.BeginTransaction(It.IsAny<IDbConnection>(), It.IsAny<string>()), Times.Once());

            _database.Verify(x => x.AddParameterWithValue(It.IsAny<IDbCommand>(), It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(2));
        }
    }
}

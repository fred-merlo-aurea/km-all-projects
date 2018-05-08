using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using FrameworkUAD.Object.Fakes;
using KMPlatform.Object;
using KMPS.MD.Objects.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Objects.Tests
{
    [TestFixture]
    public class FilterScheduleTests
    {
        private const string DefaultTestId = "1";

        private ClientConnections _connection = new ClientConnections();
        private IDisposable _shims;
        private ShimSqlDataReader _reader;
        private bool _isOpenConnectionCalled;
        private bool _isCloseConnectionCalled;
        private bool _isExecuteCommandCalled;
        private bool _isReaderCalled;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();

            _reader = new ShimSqlDataReader();

            ShimDataFunctions.GetClientSqlConnectionClientConnections = connections => new ShimSqlConnection();
            ShimSqlConnection.AllInstances.Open = sqlConnection => { _isOpenConnectionCalled = true; };
            ShimSqlConnection.AllInstances.Close = sqlConnection => { _isCloseConnectionCalled = true; };
            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                _isExecuteCommandCalled = true;
                return _reader;
            };
            
            _isOpenConnectionCalled = false;
            _isCloseConnectionCalled = false;
            _isExecuteCommandCalled = false;
            _isReaderCalled = false;
        }

        [TearDown]
        public void TearDown()
        {
            if (_shims != null)
            {
                _shims.Dispose();
            }
        }
        
        [Test]
        public void GetByBrandID_ReadRecord_ReturnsOneResult()
        {
            // Arrange
            var count = 0;
            _reader.Read = () =>
            {
                _isReaderCalled = true;
                count++;
                if (count == 1)
                {
                    _reader.ItemGetString = s => DefaultTestId;
                    return true;
                }
                else
                {
                    return false;
                }
            };
            
            // Act
            var result = FilterSchedule.GetByBrandID(_connection, 1, true);

            // Assert
            _isOpenConnectionCalled.ShouldBeTrue();
            _isCloseConnectionCalled.ShouldBeTrue();
            _isExecuteCommandCalled.ShouldBeTrue();
            _isReaderCalled.ShouldBeTrue();
            result.Count.ShouldBe(1);
        }

        [Test]
        public void GetByBrandID_ReadThrowsException_ExceptionIsThrown()
        {
            // Arrange
            _reader.Read = () =>
            {
                _isReaderCalled = true;
                throw new InvalidOperationException();
            };

            // Act
            Should.Throw<InvalidOperationException>(() => FilterSchedule.GetByBrandID(_connection, 1, true));

            // Assert
            _isOpenConnectionCalled.ShouldBeTrue();
            _isCloseConnectionCalled.ShouldBeTrue();
            _isExecuteCommandCalled.ShouldBeTrue();
            _isReaderCalled.ShouldBeTrue();
        }

        [Test]
        public void GetByBrandIDUserID_ReadRecord_ReturnsOneResult()
        {
            // Arrange
            var count = 0;
            _reader.Read = () =>
            {
                _isReaderCalled = true;
                count++;
                if (count == 1)
                {
                    _reader.ItemGetString = s => DefaultTestId;
                    return true;
                }
                else
                {
                    return false;
                }
            };
            
            // Act
            var result = FilterSchedule.GetByBrandIDUserID(_connection, 1, 1, true);

            // Assert
            _isOpenConnectionCalled.ShouldBeTrue();
            _isCloseConnectionCalled.ShouldBeTrue();
            _isExecuteCommandCalled.ShouldBeTrue();
            _isReaderCalled.ShouldBeTrue();
            result.Count.ShouldBe(1);
        }

        [Test]
        public void GetByBrandIDUserID_ReadThrowsException_ExceptionIsThrown()
        {
            // Arrange
            _reader.Read = () =>
            {
                _isReaderCalled = true;
                throw new InvalidOperationException();
            };

            // Act
            Should.Throw<InvalidOperationException>(() => FilterSchedule.GetByBrandIDUserID(_connection, 1, 1, true));

            // Assert
            _isOpenConnectionCalled.ShouldBeTrue();
            _isCloseConnectionCalled.ShouldBeTrue();
            _isExecuteCommandCalled.ShouldBeTrue();
            _isReaderCalled.ShouldBeTrue();
        }
    }
}

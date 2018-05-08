using System;
using System.Data.SqlClient.Fakes;
using ecn.communicator.classes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Tests.Communicator.Customers
{
    [TestFixture]
    public class CustomerIPTest
    {
        private const string ColumnNameIpId = "IPID";
        private const string ColumnNameMtaId = "MTAID";
        private const string ColumnNameIpAddress = "IPAddress";
        private const string ColumnNameHostName = "HostName";
        private const int SampleValueIpId = 1;
        private const int SampleValueMtaId = 2;
        private const string SampleValueIpAdress = "SampleIPAddress";
        private const string SampleValueHostName = "SampleHostName";

        private ShimSqlDataReader _reader;
        private IDisposable _shims;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
            _reader = new ShimSqlDataReader();
        }

        [TearDown]
        public void Teardown()
        {
            _shims?.Dispose();
        }

        [Test]
        public void ReadCustomerIp_ReaderIsNull_ThrowsArgumentException()
        {
            // Arrange, Act, Assert
            Should.Throw<ArgumentNullException>(() => CustomerIP.ReadCustomerIp(null));
        }

        [Test]
        public void ReadCustomerIp_FieldsNotDbNull_ReturnsNonEmpty()
        {
            // Arrange
            ShimSqlDataReader.AllInstances.ItemGetString = (sender, columnName) =>
            {
                if (columnName == ColumnNameIpId)
                {
                    return SampleValueIpId;
                }
                else if (columnName == ColumnNameMtaId)
                {
                    return SampleValueMtaId;
                }
                else if (columnName == ColumnNameIpAddress)
                {
                    return SampleValueIpAdress;
                }
                else if (columnName == ColumnNameHostName)
                {
                    return SampleValueHostName;
                }

                return null;
            };

            //Act
            var customerIpCreated = CustomerIP.ReadCustomerIp(_reader.Instance);

            // Assert
            customerIpCreated.ShouldSatisfyAllConditions(
                () => customerIpCreated.IPID.ShouldBe(SampleValueIpId),
                () => customerIpCreated.MTAID.ShouldBe(SampleValueMtaId),
                () => customerIpCreated.IPAddress.ShouldBe(SampleValueIpAdress),
                () => customerIpCreated.HostName.ShouldBe(SampleValueHostName));
        }

        [Test]
        public void ReadCustomerIp_FieldsDbNull_ReturnsDefaultObject()
        {
            // Arrange
            ShimSqlDataReader.AllInstances.ItemGetString = (sender, coloumnName) => { return DBNull.Value; };

            //Act
            var customerIpCreated = CustomerIP.ReadCustomerIp(_reader.Instance);

            // Assert
            customerIpCreated.ShouldSatisfyAllConditions(
                () => customerIpCreated.IPID.ShouldBe(default(int)),
                () => customerIpCreated.MTAID.ShouldBe(default(int)),
                () => customerIpCreated.IPAddress.ShouldBe(default(string)),
                () => customerIpCreated.HostName.ShouldBe(default(string)));
        }
    }
}
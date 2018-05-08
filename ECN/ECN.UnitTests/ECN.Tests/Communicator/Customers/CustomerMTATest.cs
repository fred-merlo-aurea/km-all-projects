using System;
using System.Collections.Generic;
using System.Data.SqlClient.Fakes;
using System.Reflection;
using ecn.communicator.classes;
using ecn.communicator.classes.Fakes;
using ECN.TestHelpers;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Tests.Communicator.Customers
{
    [TestFixture]
    public class CustomerMTATest
    {
        private const string AssemblyNameCustomerMta = "ECN";
        private const string TypeNameCustomerMta = "ecn.communicator.classes.CustomerMTA";
        private const string MethodNameReadCustomerMta = "ReadCustomerMta";
        private const string ColumnNameCustomerId = "CustomerID";
        private const string ColumnNameMtaId = "MTAID";
        private const string ColumnNameMtaName = "MTAName";
        private const string ColumnNameDomainName = "DomainName";
        private const string ColumnNameIsDefault = "IsDefault";
        private const string PropertyNameIsDefault = "CIP";
        private const int SampleValueCustomerId = 1;
        private const int SampleValueMtaId = 2;
        private const string SampleValueMtaName = "SampleMTAName";
        private const string SampleValueDomainName = "SampleDomainName";
        private const bool SampleValueIsDefault = true;
        private const int InvalidMtaIdValue = -1;

        private int _mtaIdCalled;
        private List<CustomerIP> _customerIpList;
        private PrivateObject _customerMta;
        private ShimSqlDataReader _reader;
        private IDisposable _shims;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
            _reader = new ShimSqlDataReader();
            _customerMta = new PrivateObject(AssemblyNameCustomerMta, TypeNameCustomerMta);
            _customerIpList = new List<CustomerIP>();
            _mtaIdCalled = InvalidMtaIdValue;
            ShimCustomerIP.GetIPsInt32 = mtaId =>
            {
                _mtaIdCalled = mtaId;
                return _customerIpList;
            };
        }

        [TearDown]
        public void Teardown()
        {
            _shims?.Dispose();
        }

        [Test]
        public void ReadCustomerMta_ReaderIsNull_ThrowsArgumentException()
        {
            // Arrange, Act, Assert
            Should.Throw<TargetInvocationException>(() =>
            _customerMta.RealType.CallMethod(MethodNameReadCustomerMta, new object[] { null }))
            .InnerException.GetType()
            .ShouldBe(typeof(ArgumentNullException));
        }

        [Test]
        public void ReadCustomerMta_FieldsNotDbNull_ReturnsNonEmpty()
        {
            // Arrange
            ShimSqlDataReader.AllInstances.ItemGetString = (sender, coloumnName) =>
            {
                if (coloumnName == ColumnNameCustomerId)
                {
                    return SampleValueCustomerId;
                }
                else if (coloumnName == ColumnNameMtaId)
                {
                    return SampleValueMtaId;
                }
                else if (coloumnName == ColumnNameMtaName)
                {
                    return SampleValueMtaName;
                }
                else if (coloumnName == ColumnNameDomainName)
                {
                    return SampleValueDomainName;
                }
                else if (coloumnName == ColumnNameIsDefault)
                {
                    return SampleValueIsDefault;
                }

                return null;
            };

            //Act
            var customerMtaCreated =
                _customerMta.RealType.CallMethod(MethodNameReadCustomerMta, new object[] {_reader.Instance});

            // Assert
            customerMtaCreated.ShouldSatisfyAllConditions(
                () => _mtaIdCalled.ShouldBe(SampleValueMtaId),
                () => customerMtaCreated.GetPropertyValue(PropertyNameIsDefault).ShouldBe(_customerIpList),
                () => customerMtaCreated.GetPropertyValue(ColumnNameCustomerId).ShouldBe(SampleValueCustomerId),
                () => customerMtaCreated.GetPropertyValue(ColumnNameMtaId).ShouldBe(SampleValueMtaId),
                () => customerMtaCreated.GetPropertyValue(ColumnNameMtaName).ShouldBe(SampleValueMtaName),
                () => customerMtaCreated.GetPropertyValue(ColumnNameDomainName).ShouldBe(SampleValueDomainName),
                () => customerMtaCreated.GetPropertyValue(ColumnNameIsDefault).ShouldBe(SampleValueIsDefault));
        }

        [Test]
        public void ReadCustomerMta_FieldsDbNull_ReturnsDefaultObject()
        {
            // Arrange
            ShimSqlDataReader.AllInstances.ItemGetString = (sender, coloumnName) => { return DBNull.Value; };

            //Act
            var customerMtaCreated =
                _customerMta.RealType.CallMethod(MethodNameReadCustomerMta, new object[] {_reader.Instance});

            // Assert
            customerMtaCreated.ShouldSatisfyAllConditions(
                () => _mtaIdCalled.ShouldBe(default(int)),
                () => customerMtaCreated.GetPropertyValue(PropertyNameIsDefault).ShouldBe(_customerIpList),
                () => customerMtaCreated.GetPropertyValue(ColumnNameCustomerId).ShouldBe(default(int)),
                () => customerMtaCreated.GetPropertyValue(ColumnNameMtaId).ShouldBe(default(int)),
                () => customerMtaCreated.GetPropertyValue(ColumnNameMtaName).ShouldBe(default(string)),
                () => customerMtaCreated.GetPropertyValue(ColumnNameDomainName).ShouldBe(default(string)),
                () => customerMtaCreated.GetPropertyValue(ColumnNameIsDefault).ShouldBe(default(bool)));
        }
    }
}
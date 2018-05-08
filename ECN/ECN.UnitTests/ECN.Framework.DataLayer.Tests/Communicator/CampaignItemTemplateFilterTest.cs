using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Communicator;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.DataLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CampaignItemTemplateFilterTest
    {
        private IDisposable _shimObject;
        private PrivateType _testedClass;
        private const string TestedClassName = "ECN_Framework_DataLayer.Communicator.CampaignItemTemplateFilter";
        private const string TestedClassAssemblyName = "ECN_Framework_DataLayer";
        private const string TestedMethod_GetList = "GetList";
        private int _maxRowCount;
        private int _counter;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _testedClass = new PrivateType(TestedClassAssemblyName, TestedClassName);
            SetupShims();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void GetList_ValidData_ReturnsList()
        {
            // Arrange
            var sqlCommand = new SqlCommand();
            _counter = 0;
            _maxRowCount = 5;

            // Act
            var resultList = _testedClass.InvokeStatic(TestedMethod_GetList, new[] { sqlCommand });

            // Assert
            resultList.ShouldBeOfType(typeof(List<CampaignItemBlastFilter>));
            var filterList = resultList as List<CampaignItemBlastFilter>;
            filterList.Count.ShouldBe(_maxRowCount);
        }

        private void SetupShims()
        {
            ShimDataFunctions.ExecuteReaderSqlCommandString = (_, __) => new ShimSqlDataReader();
            ShimSqlCommand.AllInstances.ConnectionGet = (_) => new ShimSqlConnection();
            ShimSqlConnection.AllInstances.Close = (_) => { };
            ShimSqlCommand.AllInstances.DisposeBoolean = (_, __) => { };

            ShimSqlDataReader.AllInstances.Read = (_) =>
            {
                _counter++;
                return _counter <= _maxRowCount;
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="DataCompareDimension"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DataCompareDimensionTest
    {
        private IDisposable _context;
        private IList<Entity.DataCompareDimension> _list;
        private Entity.DataCompareDimension _objWithRandomValues;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();

            _objWithRandomValues = typeof(Entity.DataCompareDimension).CreateInstance();

            _list = new List<Entity.DataCompareDimension>
            {
                _objWithRandomValues
            };
            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = DataCompareDimension.Get(new SqlCommand());

            // Assert
            result.ShouldBe(_objWithRandomValues);
        }

        [Test]
        public void GetList_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = DataCompareDimension.GetList(new SqlCommand());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        private void SetupFakes()
        {
            ShimSqlCommand.AllInstances.ConnectionGet = cmd => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd => _list.GetSqlDataReader();
        }
    }
}
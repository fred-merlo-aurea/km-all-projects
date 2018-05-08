using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using ECN.Framework.DataLayer.Tests.Communicator.Common;
using ECN_Framework_DataLayer.Communicator;
using ECN_Framework_DataLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.DataLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class LayoutPlansTest
    {
        private const int LayoutPlanId = 1;
        private const int UserId = 2;
        private const int LayoutId = 3;
        private const int CustomerId = 4;
        private IDisposable _shimObject;
        private SqlParameterCollection _parameterCollection;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void Delete_LayoutPlanId_UserId_ShouldFillAllParameters()
        {
            // Arrange
            var commandText = string.Empty;
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (command, conn) =>
            {
                commandText = command.CommandText;
                _parameterCollection = command.Parameters;
                return true;
            };

            // Act
            LayoutPlans.Delete(LayoutPlanId, UserId);

            // Assert
            _parameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureLayoutPlansDeleteByLPID),
                () => GetParameterValue(Consts.ParamLayoutPlanId).ShouldBe(LayoutPlanId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }

        [Test]
        public void Delete_Single_ShouldFillAllParameters()
        {
            // Arrange
            var commandText = string.Empty;
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (command, conn) =>
            {
                commandText = command.CommandText;
                _parameterCollection = command.Parameters;
                return true;
            };

            // Act
            LayoutPlans.Delete(LayoutId, LayoutPlanId, CustomerId, UserId);

            // Assert
            _parameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureLayoutPlansDeleteSingle),
                () => GetParameterValue(Consts.ParamLayoutId).ShouldBe(LayoutId.ToString()),
                () => GetParameterValue(Consts.ParamLayoutPlanId).ShouldBe(LayoutPlanId.ToString()),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }

        private string GetParameterValue(string key)
        {
            return _parameterCollection.Contains(key) ? _parameterCollection[key].Value.ToString() : null;
        }
    }
}

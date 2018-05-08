using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Linq;
using ecn.common.classes;
using ecn.common.classes.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Tests.Common
{
    public partial class BaseChannelTest
    {
        private const string InsertQuerySelectIdentify = "SELECT @@IDENTITY;";
        private const string InsertQueryStatement = "INSERT INTO BaseChannel";
        private const string InsertMethod = "Insert";

        private bool _insertCalledSqlCommandPerpare;
        private bool _insertCalledSqlCommandExecute;

        [Test]
        public void Insert_ValidData_QueryHasAllParameters()
        {
            // Arrange
            InitTest_Insert_QueryHasAllParameters();

            // Act
            _baseChannelPrivateObject.Invoke(InsertMethod, null);

            // Assert
            _sqlCommand.CommandText.ShouldSatisfyAllConditions(
                () => _insertCalledSqlCommandExecute.ShouldBeTrue(),
                () => _insertCalledSqlCommandPerpare.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldNotBeNullOrEmpty(),
                () => _sqlCommand.CommandText.ToLower().ShouldStartWith(InsertQueryStatement),
                () => _sqlCommand.CommandText.ToLower().ShouldEndWith(InsertQuerySelectIdentify),
                () => QueryParameters.All(x => _sqlCommand.CommandText.Contains(x)).ShouldBeTrue());
        }

        private void InitTest_Insert_QueryHasAllParameters()
        {
            _baseChannelPrivateObject = new PrivateObject(new BaseChannel(BaseChannelValue));

            ShimSqlConnection.AllInstances.Open = (_) => { };
            ShimSqlConnection.AllInstances.Close = (_) => { };
            ShimSqlConnection.AllInstances.DisposeBoolean = (_, __) => { };

            ShimSqlCommand.AllInstances.Prepare = (command) => _insertCalledSqlCommandPerpare = true;
            ShimSqlCommand.AllInstances.ExecuteScalar = (command) =>
            {
                _sqlCommand = command;
                _insertCalledSqlCommandExecute = true;
                return 1;
            };
        }
    }
}

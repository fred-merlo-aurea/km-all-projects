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
        private const string UpdateQueryFilterString = "Where BaseChannelID = @baseChannelID;";
        private const string UpdateQueryStatement = "Update BaseChannel";
        private const string UpdateMethod = "Update";

        private bool _updateCalledSqlCommandPerpare;
        private bool _updateCalledSqlCommandExecute;

        [Test]
        public void Update_ValidData_QueryHasAllParameters()
        {
            // Arrange
            InitTest_Update_QueryHasAllParameters();

            // Act
            _baseChannelPrivateObject.Invoke(UpdateMethod, null);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _updateCalledSqlCommandExecute.ShouldBeTrue(),
                () => _updateCalledSqlCommandPerpare.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldNotBeNullOrEmpty(),
                () => _sqlCommand.CommandText.ToLower().ShouldStartWith(UpdateQueryStatement),
                () => _sqlCommand.CommandText.ToLower().ShouldEndWith(UpdateQueryFilterString),
                () => QueryParameters.All(x => _sqlCommand.CommandText.Contains(x)).ShouldBeTrue());
        }

        private void InitTest_Update_QueryHasAllParameters()
        {
            _baseChannelPrivateObject = new PrivateObject(new BaseChannel(BaseChannelValue));

            ShimSqlConnection.AllInstances.Open = (_) => { };
            ShimSqlConnection.AllInstances.Close = (_) => { };
            ShimSqlConnection.AllInstances.DisposeBoolean = (_, __) => { };

            ShimSqlCommand.AllInstances.Prepare = (command) => _updateCalledSqlCommandPerpare = true;
            ShimSqlCommand.AllInstances.ExecuteNonQuery = (command) =>
            {
                _sqlCommand = command;
                _updateCalledSqlCommandExecute = true;
                return 1;
            };
        }
    }
}

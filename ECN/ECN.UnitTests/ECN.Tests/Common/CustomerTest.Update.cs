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
    public partial class CustomerTest
    {
        private const string UpdateQueryFilterString = "where customerid= @customerid";
        private const string UpdateQueryStatement = "update [customer]";
        private readonly string[] _updateQueryParameters = new string[]
            {
              "@baseChannelID,",
              "@customerName,",
              "@activeFlag,",
              "@address,",
              "@city,",
              "@state,",
              "@country,",
              "@zip,",
              "@salutation,",
              "@contactName,",
              "@firstName,",
              "@lastName,",
              "@contactTitle,",
              "@email,",
              "@phone,",
              "@fax,",
              "@webAddress,",
              "@techContact,",
              "@techEmail,",
              "@techPhone,",
              "@subscriptionsEmail,",
              "@accountsLevel,",
              "@communicatorLevel,",
              "@communicatorChannelID",
              "@collectorLevel,",
              "@collectorChannelID,",
              "@creatorLevel,",
              "@creatorChannelID,",
              "@publisherLevel,",
              "@publisherChannelID,",
              "@charityLevel,",
              "@charityChannelID,",
              "@CustomerType,",
              "@demoFlag,",
              "@IsStrategic,",
              "@AccountExecutiveID,",
              "@AccountManagerID,",
              "@customer_udf1,",
              "@customer_udf2,",
              "@customer_udf3,",
              "@customer_udf4,",
              "@customer_udf5,",
              "@UserID,"
            };
        private SqlCommand _updateSqlCommand;
        private bool _updateCalledSqlCommandPerpare;
        private bool _updateCalledSqlCommandExecute;
        private bool _updateCalledSqlCommandDispose;
        private bool _updateCalledMapCustomerParameters;

        [Test]
        public void Update_QueryHasAllParameters()
        {
            // Arrange
            InitTest_Update_QueryHasAllParameters();

            // Act
            _customerPrivateObject.Invoke("Update", new object[] { new SqlConnection(), 10 });

            // Assert
            var commandText = _updateSqlCommand.CommandText?.Trim();
            _updateSqlCommand.ShouldSatisfyAllConditions(
                () => _updateCalledMapCustomerParameters.ShouldBeTrue(),
                () => _updateCalledSqlCommandDispose.ShouldBeTrue(),
                () => _updateCalledSqlCommandExecute.ShouldBeTrue(),
                () => _updateCalledSqlCommandPerpare.ShouldBeTrue(),
                () => commandText.ShouldNotBeNullOrEmpty(),
                () => commandText.ToLower().ShouldStartWith(UpdateQueryStatement),
                () => commandText.ToLower().ShouldEndWith(UpdateQueryFilterString),
                () => _updateQueryParameters.All(x => commandText.Contains(x)).ShouldBeTrue());
        }

        private void InitTest_Update_QueryHasAllParameters()
        {
            _customerPrivateObject = new PrivateObject(new Customer(-1, -1));
            ShimSqlCommand.AllInstances.Prepare = (command) => _updateCalledSqlCommandPerpare = true;
            ShimCustomer.AllInstances.MapCustomerParametersInt32SqlCommand = (customer, id, command) =>
			 {
               _updateSqlCommand = command;
               command.Disposed += (o, e) => _updateCalledSqlCommandDispose = true;
               _updateCalledMapCustomerParameters = true;
               return command;
			 };
            ShimSqlCommand.AllInstances.ExecuteNonQuery = (command) =>
            {
                _updateCalledSqlCommandExecute = true;
                return 1;
            };
        }
    }
}

using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common.Fakes;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Web;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.includes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Includes
{
    [TestFixture]
    public class EmailProfileUDFHistoryTest
    {
        private const string GetFromQueryStringMethodName = "GetFromQueryString";
        private const string LoadHistoryDataMethodName = "LoadUDFHistoryData";
        private const string EmailIdQueryStringKey = "eID";
        private const string EmailId = "100";
        private const string EmailIdErrorMessage = "EmailId specified does not exist.";
        private const string EmailIdCommandParameterName = "@UDFEmailID";
        private const string GetCustomerIdMethodName = "GetCustomerId";
        private const string CustomerId = "200";
        private const string CustomerIdErrorMessage = "<br>ERROR: CustomerID does not Exist for the GroupID:WrongId. Please click on the 'Profile' link in the email message that you received";
        private const string CustomerIdCommandParameterName = "@CustomerID";
        private const string GroupIdQueryStringKey = "gID";
        private const string GroupId = "300";
        private const string GroupIdErrorMessage = "GroupId specified does not exist.";
        private const string GroupIdCommandParameterName = "@GroupID";
        private const string GetDataFieldSetNameMethodName = "GetDataFieldSetName";
        private const string DataFieldSetName = "test data field";
        private const string DataFieldSetNameErrorMessage = "<br>ERROR: DataFieldSetID specified does not Exist. Please click on the 'Profile' link in the email message that you received";
        private const string DataFieldSetIdQueryStringKey = "dfsID";
        private const string DataFieldSetId = "400";
        private const string DataFieldSetIdErrorMessage = "DataFieldSetId specified does not exist.";
        private const string DataFieldSetIdCommandParameterName = "@DataFieldSetID";
        private const string MessageLabelName = "MessageLabel";

        private emailProfile_UDFHistory _emailProfileUDFHistoryObject;
        private PrivateObject _emailProfileUDFHistoryPrivateObject;
        private IDisposable _shimsContext;
        private ShimHttpRequest _shimHttpRequest;
        private HttpRequest _httpRequest;
        private NameValueCollection _queryStringCollection;
        private Label _messageLabel;
        private ConnectionState _connectionState;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _shimsContext = ShimsContext.Create();
            ShimUserControl.AllInstances.RequestGet = (userControl) => { return _httpRequest; };
            ShimHttpRequest.AllInstances.QueryStringGet = (request) => { return _queryStringCollection; };
            ShimSqlConnection.Constructor = (sqlConnection) => { };
            ShimSqlConnection.ConstructorString = (sqlConnection, connString) => { };
            ShimSqlConnection.AllInstances.Open = (sqlConnection) => { _connectionState = ConnectionState.Open; };
            ShimSqlConnection.AllInstances.Close = (sqlConnection) => { _connectionState = ConnectionState.Closed; };
            ShimSqlConnection.AllInstances.StateGet = (sqlConnection) => { return _connectionState; };

            _shimHttpRequest = new ShimHttpRequest();
            _emailProfileUDFHistoryObject = new emailProfile_UDFHistory();
            _httpRequest = _shimHttpRequest.Instance;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _shimsContext.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _queryStringCollection = new NameValueCollection();
            _emailProfileUDFHistoryObject = new emailProfile_UDFHistory();
            _emailProfileUDFHistoryPrivateObject = new PrivateObject(_emailProfileUDFHistoryObject);

            _messageLabel = new Label();
            _emailProfileUDFHistoryPrivateObject.SetField(MessageLabelName, _messageLabel);
        }

        [TearDown]
        public void TearDown()
        {
            _messageLabel.Dispose();
        }

        [Test]
        public void GetFromQueryString_IfEmailIdProvidedInQueryString_ReturnsEmailId()
        {
            // Arrange
            _httpRequest.QueryString[EmailIdQueryStringKey] = EmailId;

            // Act
            var returnedEmailId = (string)_emailProfileUDFHistoryPrivateObject.Invoke(GetFromQueryStringMethodName, EmailIdQueryStringKey, EmailIdErrorMessage);

            // Assert
            returnedEmailId.ShouldBe(EmailId);
        }

        [Test]
        public void GetFromQueryString_IfDataFieldSetIdProvidedInQueryString_ReturnsDataFieldSetId()
        {
            // Arrange
            _httpRequest.QueryString[DataFieldSetIdQueryStringKey] = DataFieldSetId;

            // Act
            var returnedDataFieldSetId = (string)_emailProfileUDFHistoryPrivateObject.Invoke(GetFromQueryStringMethodName, DataFieldSetIdQueryStringKey, DataFieldSetIdErrorMessage);

            // Assert
            returnedDataFieldSetId.ShouldBe(DataFieldSetId);
        }

        [Test]
        public void GetFromQueryString_IfGroupIdProvidedInQueryString_ReturnsGroupId()
        {
            // Arrange
            _httpRequest.QueryString[GroupIdQueryStringKey] = GroupId;

            // Act
            var returnedGroupId = (string)_emailProfileUDFHistoryPrivateObject.Invoke(GetFromQueryStringMethodName, GroupIdQueryStringKey, GroupIdErrorMessage);

            // Assert
            returnedGroupId.ShouldBe(GroupId);
        }

        [Test]
        public void GetFromQueryString_IfExpectedKeyDoesNotExistInQueryString_ReturnsEmptyStringAndSetsMessageLabel()
        {
            // Arrange
            // set no query string to get error

            // Act
            var returnedValue = (string)_emailProfileUDFHistoryPrivateObject.Invoke(GetFromQueryStringMethodName, EmailIdQueryStringKey, EmailIdErrorMessage);

            // Assert
            returnedValue.ShouldBeEmpty();
            _messageLabel.ShouldSatisfyAllConditions(
                () => _messageLabel.Visible.ShouldBeTrue(),
                () => _messageLabel.Text.ShouldEndWith(EmailIdErrorMessage));
        }

        [Test]
        public void GetCustomerId_IfDataFetchedFromDatabase_ReturnsCustomerId()
        {
            // Arrange
            ShimSqlCommand.AllInstances.ExecuteScalar = (sqlCommand) =>
            {
                if (sqlCommand.CommandText.Contains(GroupId))
                {
                    return CustomerId;
                }
                else
                {
                    throw new Exception();
                }
            };

            // Act
            var returnedCustomerId = (string)_emailProfileUDFHistoryPrivateObject.Invoke(GetCustomerIdMethodName, GroupId);

            // Assert
            returnedCustomerId.ShouldBe(CustomerId);
        }

        [Test]
        public void GetCustomerId_IfExceptionThrownWhileFetchingData_ReturnsEmptyStringAndSetsMessageLabel()
        {
            // Arrange
            ShimSqlCommand.AllInstances.ExecuteScalar = (sqlCommand) =>
            {
                if (sqlCommand.CommandText.Contains(GroupId))
                {
                    return CustomerId;
                }
                else
                {
                    throw new Exception();
                }
            };

            // Act
            var returnedCustomerId = (string)_emailProfileUDFHistoryPrivateObject.Invoke(GetCustomerIdMethodName, "WrongId");

            // Assert
            returnedCustomerId.ShouldBe("0");
            _messageLabel.ShouldSatisfyAllConditions(
                () => _messageLabel.Visible.ShouldBeTrue(),
                () => _messageLabel.Text.ShouldBe(CustomerIdErrorMessage));
        }

        [Test]
        public void GetDataFieldSetName_IfDataFetchedFromDatabase_ReturnsDataFieldSetName()
        {
            // Arrange
            ShimSqlCommand.AllInstances.ExecuteScalar = (sqlCommand) =>
            {
                if (sqlCommand.CommandText.Contains(DataFieldSetId))
                {
                    return DataFieldSetName;
                }
                else
                {
                    throw new Exception();
                }
            };

            // Act
            var returnedDataFieldSetName = (string)_emailProfileUDFHistoryPrivateObject.Invoke(GetDataFieldSetNameMethodName, DataFieldSetId);

            // Assert
            returnedDataFieldSetName.ShouldBe(DataFieldSetName);
        }

        [Test]
        public void GetDataFieldSetName_IfExceptionThrownWhileFetchingData_ReturnsEmptyStringAndSetsMessageLabel()
        {
            // Arrange
            ShimSqlCommand.AllInstances.ExecuteScalar = (sqlCommand) =>
            {
                if (sqlCommand.CommandText.Contains(DataFieldSetId))
                {
                    return DataFieldSetName;
                }
                else
                {
                    throw new Exception();
                }
            };

            // Act
            var returnedDataFieldSetName = (string)_emailProfileUDFHistoryPrivateObject.Invoke(GetDataFieldSetNameMethodName, "WrongId");

            // Assert
            returnedDataFieldSetName.ShouldBeEmpty();
            _messageLabel.ShouldSatisfyAllConditions(
                () => _messageLabel.Visible.ShouldBeTrue(),
                () => _messageLabel.Text.ShouldBe(DataFieldSetNameErrorMessage));
        }

        [Test]
        public void LoadUDFHistoryData_WhenCalled_VerifyCommandParameters()
        {
            // Arrange
            var sqlCommand = default(SqlCommand);
            ShimDbDataAdapter.AllInstances.FillDataSetString = (dataAdapter, dataSet, commandText) =>
            {
                sqlCommand = (SqlCommand)dataAdapter.SelectCommand;
                return -1;
            };

            int numericGroupId;
            int.TryParse(GroupId, out numericGroupId);

            int numericDataFieldSetId;
            int.TryParse(DataFieldSetId, out numericDataFieldSetId);

            int numericCustomerId;
            int.TryParse(CustomerId, out numericCustomerId);

            // Act
            _emailProfileUDFHistoryPrivateObject.Invoke(LoadHistoryDataMethodName, EmailId, GroupId, DataFieldSetId, CustomerId);

            // Assert
            var emailIdParameter = sqlCommand.Parameters[EmailIdCommandParameterName];
            emailIdParameter.ShouldSatisfyAllConditions(
                () => emailIdParameter.ShouldNotBeNull(),
                () => emailIdParameter.SqlDbType.ShouldBe(SqlDbType.VarChar),
                () => emailIdParameter.Value.ShouldBe(EmailId));

            var groupIdParameter = sqlCommand.Parameters[GroupIdCommandParameterName];
            groupIdParameter.ShouldSatisfyAllConditions(
                () => groupIdParameter.ShouldNotBeNull(),
                () => groupIdParameter.SqlDbType.ShouldBe(SqlDbType.Int),
                () => groupIdParameter.Value.ShouldBe(numericGroupId));

            var customerIdParameter = sqlCommand.Parameters[CustomerIdCommandParameterName];
            customerIdParameter.ShouldSatisfyAllConditions(
                () => customerIdParameter.ShouldNotBeNull(),
                () => customerIdParameter.SqlDbType.ShouldBe(SqlDbType.Int),
                () => customerIdParameter.Value.ShouldBe(numericCustomerId));

            var dataFieldSetIdParameter = sqlCommand.Parameters[DataFieldSetIdCommandParameterName];
            dataFieldSetIdParameter.ShouldSatisfyAllConditions(
                () => dataFieldSetIdParameter.ShouldNotBeNull(),
                () => dataFieldSetIdParameter.SqlDbType.ShouldBe(SqlDbType.Int),
                () => dataFieldSetIdParameter.Value.ShouldBe(numericDataFieldSetId));
        }
    }
}
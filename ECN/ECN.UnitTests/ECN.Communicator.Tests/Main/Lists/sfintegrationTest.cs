using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Xml;
using ActiveUp.WebControls;
using ecn.communicator.classes;
using ecn.communicator.classes.Fakes;
using ecn.communicator.main.lists;
using ecn.communicator.main.lists.Fakes;
using ecn.communicator.SalesForcePartner;
using ecn.communicator.SalesForcePartner.Fakes;
using ecn.communicator.SalesForcePartner.Interfaces;
using Ecn.Communicator.Main.Lists.Interfaces;
using ECN.Communicator.Tests.Helpers;
using ECN.Communicator.Tests.Main.Salesforce.SF_Pages;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Lists
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.main.lists.sfintegration"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SfIntegrationTest
    {
        private const int Timeout = 900;
        private const int ValueNegativeOne = -1;
        private const int ValueNegativeTwo = -2;
        private const int ValueZero = 0;
        private const int ValueOne = 1;
        private const int PagerCurrentPageIndex = 0;
        private const int PagerContactsPageSize = 1;
        private const string GroupIdKey = "GroupId";
        private const string GroupNameKey = "GroupName";
        private const string GroupNameDummy = "DummyGroupName";
        private const string GroupNameValueAll = "- All -";
        private const string GroupNameValueSelectGroup = "- Select Group -";
        private const string GroupNameValueNewGroup = "- New Group -";
        private const string EmailAddressDummy = "dummy@email.com";
        private const string StartRowIndexParameter = "@startrowIndex";
        private const string MaximumRowsParameter = "@maximumRows";
        private const string CustomerIdParameter = "@customerID";
        private const string GroupIdParameter = "@groupID";
        private const string EmailAddressParameter = "@emailAddress";
        private const string EmailAddressKey = "EmailAddress";
        private const string FirstNameKey = "FirstName";
        private const string LastNameKey = "LastName";
        private const string AddressKey = "Address";
        private const string CityKey = "City";
        private const string StateKey = "State";
        private const string ZipKey = "Zip";
        private const string CountryKey = "Country";
        private const string CompanyKey = "Company";
        private const string TitleKey = "Title";
        private const string VoiceKey = "Voice";
        private const string MobileKey = "Mobile";
        private const string FaxKey = "Fax";
        private const string ID = "1";
        private const string LoggedInKey = "loggedIn";
        private const string BindingKey = "binding";
        private const string CustomerIdKey = "CustomerID";
        private const string AccountId = "AccountId";
        private const string GetECNContactsByCustomerIdQuery = "SELECT Count(e.EmailAddress) FROM Emails e WHERE e.CustomerID ={0}";
        private const string GetECNContactsByCustomerIdAndGroupIdQuery = 
            "SELECT Count(e.EmailAddress) FROM Emails e INNER JOIN Groups g ON g.CustomerID  = e.CustomerID WHERE e.CustomerID ={0} AND g.GroupID ={1}";
        private const string GetGroupsByCustomerIdQuery = "SELECT GroupID,GroupName FROM Groups WHERE CustomerID ={0} ORDER BY GroupName";
        private const int CustomerID = 1;
        private const int GroupID = 2;
        private List<SF_Contact> contacts;
        private Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject _testObject;
        private Page _testPage;
        private IDisposable _shimObject;
        private HttpSessionState _sessionState;

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
            contacts = new List<SF_Contact>
            {
                new SF_Contact()
            };
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void CreateSfContact_ValidSfContact_CallsSessionBindingCreate()
        {
            // Arrange
            var itemCollection = new SessionStateItemCollection();
            itemCollection[LoggedInKey] = true;
            var service = (Mock<ISforceService>)typeof(ISforceService).GetMock();
            var saveResults = new SaveResult[]
            {
                new SaveResult()
                {
                    id = string.Empty
                }
            };

            service.Setup(x => x.create(It.IsAny<sObject[]>())).Returns(saveResults);
            itemCollection[BindingKey] = service.Object;

            var integration = new sfintegration(GetSession(itemCollection));
            integration.SetProperty(LoggedInKey, true);

            // Act
            var actual = typeof(sfintegration).CallMethod("CreateSfContact", new object[] { contacts[0] }, integration) as string;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsEmpty(actual);
            service.Verify(x => x.create(It.IsAny<sObject[]>()), Times.Once());
        }

        [Test]
        public void CreateSfContacts_ValidSfContact_CallsSessionBindingCreate()
        {
            // Arrange
            var itemCollection = new SessionStateItemCollection();
            itemCollection[LoggedInKey] = true;
            var service = (Mock<ISforceService>)typeof(ISforceService).GetMock();
            var saveResults = new SaveResult[0];
            service.Setup(x => x.create(It.IsAny<sObject[]>())).Returns(saveResults);
            itemCollection[BindingKey] = service.Object;

            var integration = new sfintegration(GetSession(itemCollection));
            integration.SetProperty(LoggedInKey, true);

            // Act
            typeof(sfintegration).CallMethod("CreateSfContacts", new object[] { contacts }, integration);

            // Assert
            service.Verify(x => x.create(It.IsAny<sObject[]>()), Times.Once());
        }

        [Test]
        public void GetSFContacts_ValidSfContact_ReturnsListOfSFContact()
        {
            // Arrange
            var itemCollection = new SessionStateItemCollection();
            itemCollection[LoggedInKey] = true;
            var service = new Mock<ISforceService>();
            var queryResult = new QueryResult()
            {
                records = new sObject[]
                {
                    new sObject()
                }
            };

            var getSFContactsByAccountIdQuery = new StringBuilder();
            getSFContactsByAccountIdQuery.AppendLine("Select Id,AccountID,Email,Fax,FirstName,HomePhone,LastName,MailingCity,MailingState,MailingCountry,MailingPostalCode,MailingStreet,MobilePhone,Phone,Title ");
            getSFContactsByAccountIdQuery.AppendLine("From Contact Where IsDeleted = false and AccountId = '{0}'");

            var queryText = string.Format(getSFContactsByAccountIdQuery.ToString(), AccountId);
            service.Setup(x => x.query(queryText)).Returns(queryResult);
            itemCollection[BindingKey] = service.Object;

            var integration = new sfintegration(GetSession(itemCollection));
            integration.SetProperty(LoggedInKey, true);

            // Act
            var actual = typeof(sfintegration).CallMethod("GetSFContacts", new object[] { AccountId }, integration) as List<SF_Contact>;

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            CollectionAssert.AllItemsAreInstancesOfType(actual, typeof(SF_Contact));
        }

        [Test]
        public void GetSFContactsAll_NotNullSessionSFContacts_ReturnsListOfSFContact()
        {
            // Arrange
            var itemCollection = new SessionStateItemCollection();
            itemCollection[LoggedInKey] = true;
            var service = new Mock<ISforceService>();
            var queryResult = new QueryResult()
            {
                records = new sObject[]
                {
                    new sObject()
                }
            };
            var getAllSFContactsQuery = new StringBuilder();
            getAllSFContactsQuery.AppendLine("Select Id,AccountID,Email,Fax,FirstName,HomePhone,LastName,MailingCity,MailingState,MailingCountry,MailingPostalCode,MailingStreet,MobilePhone,Phone,Title ");
            getAllSFContactsQuery.AppendLine("From Contact Where IsDeleted = false");

            service.Setup(x => x.query(getAllSFContactsQuery.ToString())).Returns(queryResult);
            itemCollection[BindingKey] = service.Object;

            var integration = new sfintegration(GetSession(itemCollection));
            integration.SetProperty(LoggedInKey, true);

            // Act
            var actual = typeof(sfintegration).CallMethod("GetSFContactsAll", null, integration) as List<SF_Contact>;

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            CollectionAssert.AllItemsAreInstancesOfType(actual, typeof(SF_Contact));
        }

        [Test]
        public void LoadECNGroups_OneRecordToRead_BindsECNGroupsDropDownWithTwoGroups()
        {
            // Arrange
            var commandText = string.Format(GetGroupsByCustomerIdQuery, CustomerID);
            var ordinals = new KeyValuePair<string, int>[]
            {
                new KeyValuePair<string, int>(GroupIdKey, ValueZero),
                new KeyValuePair<string, int>(GroupNameKey, ValueOne)
            };
            var parameters = new KeyValuePair<int, object>[]
            {
                new KeyValuePair<int, object>(ValueZero, ValueZero),
                new KeyValuePair<int, object>(ValueOne, GroupNameDummy)
            };

            var adapter = CreateDatabaseAdapter(commandText, ordinals, parameters);

            var itemCollection = new SessionStateItemCollection();
            itemCollection[CustomerIdKey] = CustomerID;

            var integration = new sfintegration(GetSession(itemCollection), adapter.Object);
            var dropdownControlName = "ddlECNGroups";
            var dropdownField = InitializeIntegrationField(integration, dropdownControlName, new DropDownList());

            // Act
            typeof(sfintegration).CallMethod("LoadECN_Groups", null, integration);

            // Assert
            var dropdown = dropdownField.GetValue(integration) as DropDownList;
            Assert.IsNotNull(dropdown);
            Assert.IsInstanceOf(typeof(DropDownList), dropdown);
            Assert.AreEqual(GroupNameKey, dropdown.DataTextField);
            Assert.AreEqual(GroupIdKey, dropdown.DataValueField);

            var dropdownDatasource = dropdown.DataSource;
            Assert.IsInstanceOf(typeof(List<ECN_Group>), dropdownDatasource);
            var groups = new List<ECN_Group>()
            {
                new ECN_Group()
                {
                    GroupID = ValueNegativeOne,
                    GroupName = GroupNameValueAll
                },
                new ECN_Group()
                {
                    GroupID = ValueZero,
                    GroupName = GroupNameDummy
                }
            };

            CollectionAssert.AreEqual(groups, dropdownDatasource as List<ECN_Group>, new ECNGroupComparer());
        }

        [Test]
        public void LoadECNGroupForSF_OneRecordToRead_BindsECNGroupFromSFDropDownWithThreeGroups()
        {
            // Arrange
            var commandText = string.Format(GetGroupsByCustomerIdQuery, CustomerID);
            var ordinals = new KeyValuePair<string, int>[]
            {
                new KeyValuePair<string, int>(GroupIdKey, ValueZero),
                new KeyValuePair<string, int>(GroupNameKey, ValueOne)
            };
            var parameters = new KeyValuePair<int, object>[]
            {
                new KeyValuePair<int, object>(ValueZero, ValueZero),
                new KeyValuePair<int, object>(ValueOne, GroupNameDummy)
            };
            var adapter = CreateDatabaseAdapter(commandText, ordinals, parameters);

            var itemCollection = new SessionStateItemCollection();
            itemCollection[CustomerIdKey] = CustomerID;

            var integration = new sfintegration(GetSession(itemCollection), adapter.Object);
            var dropdownControlName = "ddlECNGroupFromSF";
            var dropdownField = InitializeIntegrationField(integration, dropdownControlName, new DropDownList());

            // Act
            typeof(sfintegration).CallMethod("LoadECNGroupForSF", null, integration);

            // Assert
            var dropdown = dropdownField.GetValue(integration) as DropDownList;
            Assert.IsNotNull(dropdown);
            Assert.IsInstanceOf(typeof(DropDownList), dropdown);
            Assert.AreEqual(GroupNameKey, dropdown.DataTextField);
            Assert.AreEqual(GroupIdKey, dropdown.DataValueField);

            var dropdownDatasource = dropdown.DataSource;
            Assert.IsInstanceOf(typeof(List<ECN_Group>), dropdownDatasource);
            var groups = new List<ECN_Group>()
            {
                new ECN_Group()
                {
                    GroupID = ValueNegativeTwo,
                    GroupName = GroupNameValueSelectGroup
                },
                new ECN_Group()
                {
                    GroupID = ValueNegativeOne,
                    GroupName = GroupNameValueNewGroup
                },
                new ECN_Group()
                {
                    GroupID = ValueZero,
                    GroupName = GroupNameDummy
                }
            };

            CollectionAssert.AreEqual(groups, dropdownDatasource as List<ECN_Group>, new ECNGroupComparer());
        }

        [Test]
        public void GetECNContactByEmailAddress_OneRecordToRead_ReturnsECNContact()
        {
            // Arrange
            var commandText = string.Format(GetGroupsByCustomerIdQuery, CustomerID);
            var ordinals = PrepareECNContactOrdinals();
            var parameters = new KeyValuePair<int, object>[]
            {
                new KeyValuePair<int, object>(ValueZero, EmailAddressDummy)
            };

            var adapter = CreateDatabaseAdapter(commandText, ordinals, parameters);
            var integration = new sfintegration(null, adapter.Object);

            // Act
            var contact = typeof(sfintegration).CallMethod(
                "GetECNContactByEmailAddress",
                new object[] { EmailAddressDummy },
                integration) as ECN_Contact;

            // Assert
            Assert.IsNotNull(contact);
            Assert.AreEqual(EmailAddressDummy, contact.EmailAddress);
            Assert.AreNotEqual(EmailAddressDummy, contact.GroupName);
            adapter.Verify(x => x.AddParameter(It.IsAny<IDbCommand>(), EmailAddressParameter, SqlDbType.VarChar, EmailAddressDummy));
        }

        [Test]
        public void GetECNContactsAll_OneRecordToRead_ReturnsECNContactListContainsOneContact()
        {
            // Arrange
            var commandText = string.Format(GetECNContactsByCustomerIdQuery, CustomerID);
            var ordinals = PrepareECNContactOrdinals();
            var parameters = new KeyValuePair<int, object>[]
            {
                new KeyValuePair<int, object>(ValueZero, EmailAddressDummy)
            };

            var itemCollection = new SessionStateItemCollection();
            itemCollection[CustomerIdKey] = CustomerID;

            var adapter = CreateDatabaseAdapter(commandText, ordinals, parameters);
            var integration = new sfintegration(GetSession(itemCollection), adapter.Object);
            integration.pagerCurrentPage = ValueZero;
            var pagerBuilder = new PagerBuilder()
            {
                PageSize = ValueOne
            };
            var pagerECNContactsField = InitializeIntegrationField(integration, "pagerECNContacts", pagerBuilder);

            // Act
            var contacts = typeof(sfintegration).CallMethod(
                "GetECNContactsAll",
                null,
                integration) as List<ECN_Contact>;

            // Assert
            Assert.IsNotNull(contacts);
            Assert.AreEqual(1, contacts.Count);
            Assert.AreEqual(EmailAddressDummy, contacts[0].EmailAddress);
            Assert.AreEqual(EmailAddressDummy, contacts[0].GroupName);
            adapter.Verify(x => x.AddParameter(It.IsAny<IDbCommand>(), StartRowIndexParameter, SqlDbType.Int, ValueZero));
            adapter.Verify(x => x.AddParameter(It.IsAny<IDbCommand>(), MaximumRowsParameter, SqlDbType.Int, ValueOne));
            adapter.Verify(x => x.AddParameter(It.IsAny<IDbCommand>(), CustomerIdParameter, SqlDbType.Int, CustomerID));
            Assert.AreEqual(ValueZero, integration.pagerRecordCount);
        }

        [Test]
        public void GetECNContacts_OneRecordToRead_ReturnsECNContactListContainsOneContact()
        {
            // Arrange
            var commandText = string.Format(GetECNContactsByCustomerIdAndGroupIdQuery, CustomerID, GroupID);
            var ordinals = PrepareECNContactOrdinals();
            var parameters = new KeyValuePair<int, object>[]
            {
                new KeyValuePair<int, object>(ValueZero, EmailAddressDummy)
            };

            var itemCollection = new SessionStateItemCollection();
            itemCollection[CustomerIdKey] = CustomerID;

            var adapter = CreateDatabaseAdapter(commandText, ordinals, parameters);
            var integration = new sfintegration(GetSession(itemCollection), adapter.Object);
            integration.pagerCurrentPage = ValueZero;
            var pagerBuilder = new PagerBuilder()
            {
                PageSize = ValueOne
            };
            var pagerECNContactsField = InitializeIntegrationField(integration, "pagerECNContacts", pagerBuilder);

            // Act
            var contacts = typeof(sfintegration).CallMethod(
                "GetECNContacts",
                new object[] { GroupID },
                integration) as List<ECN_Contact>;

            // Assert
            Assert.IsNotNull(contacts);
            Assert.AreEqual(1, contacts.Count);
            Assert.AreEqual(EmailAddressDummy, contacts[0].EmailAddress);
            Assert.AreEqual(EmailAddressDummy, contacts[0].GroupName);
            adapter.Verify(x => x.AddParameter(It.IsAny<IDbCommand>(), StartRowIndexParameter, SqlDbType.Int, ValueZero));
            adapter.Verify(x => x.AddParameter(It.IsAny<IDbCommand>(), MaximumRowsParameter, SqlDbType.Int, ValueOne));
            adapter.Verify(x => x.AddParameter(It.IsAny<IDbCommand>(), CustomerIdParameter, SqlDbType.Int, CustomerID));
            adapter.Verify(x => x.AddParameter(It.IsAny<IDbCommand>(), GroupIdParameter, SqlDbType.Int, GroupID));
            Assert.AreEqual(ValueZero, integration.pagerRecordCount);
        }

        [Test]
        public void GetSFLeads_ReturnsListOfSFLeads()
        {
            // Arrange
            InitilizeFakes();
            _sessionState[LoggedInKey] = true;
            _sessionState["SFLeads"] = null;
            ShimSforceService.AllInstances.queryString = (p1,p2) => new QueryResult { records = 
                new sObject[] { new sObject { Any = new XmlElement[] {
                    GetElement("AnnualRevenue", "1"),
                    GetElement("NumberOfEmployees", "1"),
                    GetElement("ConvertedDate", "2018.1.1"),
                    GetElement("EmailBouncedDate", "2018.1.1") } } } };     
            var integration = new sfintegration();
            integration.SetProperty(LoggedInKey, true);

            // Act
            var result = typeof(sfintegration).CallMethod("GetSFLeads", new object[] { }, integration) as List<SF_Lead>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Count.ShouldBe(2),
                () => result[1].AnnualRevenue.ShouldBe(1),
                () => result[1].NumberOfEmployees.ShouldBe(1));
        }

        [Test]
        public void GetSFLeads_NoConvertion_ReturnsListOfSFLeads()
        {
            // Arrange
            InitilizeFakes();
            _sessionState[LoggedInKey] = true;
            _sessionState["SFLeads"] = null;
            ShimSforceService.AllInstances.queryString = (p1, p2) => new QueryResult
            {
                records =
                new sObject[] { new sObject { Any = new XmlElement[] {
                    GetElement("AnnualRevenue", "*"),
                    GetElement("NumberOfEmployees", "*"),
                    GetElement("ConvertedDate", "*"),
                    GetElement("EmailBouncedDate", "*") } } }
            };
            var integration = new sfintegration();
            integration.SetProperty(LoggedInKey, true);

            // Act
            var result = typeof(sfintegration).CallMethod("GetSFLeads", new object[] { }, integration) as List<SF_Lead>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Count.ShouldBe(2),
                () => result[1].AnnualRevenue.ShouldBe(0),
                () => result[1].NumberOfEmployees.ShouldBe(0));
        }

        [Test]
        public void GetSFLeads_Default_ReturnsListOfSFLeads()
        {
            // Arrange
            InitilizeFakes();
            _sessionState[LoggedInKey] = true;
            _sessionState["SFLeads"] = new List<SF_Lead> { new SF_Lead { AnnualRevenue = 5, NumberOfEmployees = 10} };
            var integration = new sfintegration();

            // Act
            var result = typeof(sfintegration).CallMethod("GetSFLeads", new object[] { }, integration) as List<SF_Lead>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Count.ShouldBe(1),
                () => result[0].AnnualRevenue.ShouldBe(5),
                () => result[0].NumberOfEmployees.ShouldBe(10));
        }

        [Test]
        public void GetSFCampaignsAll_ReturnsListOfSFCampaign([Values(true, false)]bool isConverted)
        {
            // Arrange
            InitilizeFakes();
            _sessionState["SFCampaigns"] = null;
            _sessionState[BindingKey] = new SforceService();
            var sampleInt = isConverted ? "1" : string.Empty;
            var sampleDate = isConverted ? "2018.1.1" : string.Empty;
            ShimSforceService.AllInstances.queryString = (p1, p2) => new QueryResult
            {
                records =
                new sObject[] { new sObject { Any = new XmlElement[] {
                    GetElement("ActualCost", sampleInt),
                    GetElement("AmountAllOpportunities", sampleInt),
                    GetElement("AmountWonOpportunities", sampleInt),
                    GetElement("BudgetedCost", sampleInt),
                    GetElement("EndDate", sampleDate),
                    GetElement("ExpectedResponse", sampleInt),
                    GetElement("ExpectedRevenue", sampleInt),
                    GetElement("NumberOfContacts", sampleInt),
                    GetElement("NumberOfConvertedLeads", sampleInt),
                    GetElement("NumberOfLeads", sampleInt),
                    GetElement("NumberOfOpportunities", sampleInt),
                    GetElement("NumberOfResponses", sampleInt),
                    GetElement("NumberOfWonOpportunities", sampleInt),
                    GetElement("NumberSent", sampleInt),
                    GetElement("StartDate", sampleDate) } } }
            };
            var integration = new sfintegration();
            integration.SetProperty(LoggedInKey, true);

            // Act
            var result = typeof(sfintegration).CallMethod("GetSFCampaignsAll", new object[] { }, integration) as List<SF_Campaign>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Count.ShouldBe(3),
                () => result[2].ActualCost.ShouldBe(isConverted ? 1 : 0),
                () => result[2].AmountAllOpportunities.ShouldBe(isConverted ? 1 : 0));
        }

        [Test]
        public void GetSFCampaignsAll_Default_ReturnsListOfSFCampaign()
        {
            // Arrange
            InitilizeFakes();
            _sessionState["SFCampaigns"] = new List<SF_Campaign> { new SF_Campaign { ActualCost = 5, AmountAllOpportunities = 10} };
            var integration = new sfintegration();

            // Act
            var result = typeof(sfintegration).CallMethod("GetSFCampaignsAll", new object[] { }, integration) as List<SF_Campaign>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Count.ShouldBe(1),
                () => result[0].ActualCost.ShouldBe(5),
                () => result[0].AmountAllOpportunities.ShouldBe(10));
        }

        [Test]
        public void BtnCampOK_Click_Successs()
        {
            // Arrange
            InitilizeFakes();
            _sessionState[LoggedInKey] = false;
            CreateTestObjects();
            var tbCampStart = _testObject.GetFieldOrProperty("tbCampStart") as TextBox;
            tbCampStart.Text = "2018.1.1";
            var tbCampEnd = _testObject.GetFieldOrProperty("tbCampEnd") as TextBox;
            tbCampEnd.Text = "2018.1.1";
            ShimSforceService.AllInstances.createsObjectArray = (p1, p2) => new SaveResult[] { new SaveResult { success = true, id = "1"} };
            var lbMessage = _testObject.GetFieldOrProperty("lbMessage") as Label;
            _sessionState["SFCampaigns"] = new List<SF_Campaign>();
            ShimSforceService.AllInstances.queryString = (p1, p2) => new QueryResult
            {
                records = new sObject[] { new sObject { Any = new XmlElement[] {
                    GetElement("HasResponded", "True"),
                    GetElement("IsDefault", "True"),
                    GetElement("SortOrder", "1"),
                    GetElement("Id", "1"),
                    GetElement("Label", "bounce") } } }
            };

            // Act
            _testObject.Invoke("btnCampOK_Click", new object[] { null, null } );

            // Assert
            var resultList = _sessionState["SFCampaigns"] as List<SF_Campaign>;
            resultList.ShouldSatisfyAllConditions(
                () => resultList.ShouldNotBeNull(),
                () => resultList.Count.ShouldBe(1),
                () => resultList[0].CampaignId.ShouldBe("1"));
            lbMessage.Text.ShouldBe("A campaign was create with an id of: 1");
        }

        [Test]
        public void BtnCampOK_Click_Error()
        {
            // Arrange
            InitilizeFakes();
            _sessionState[LoggedInKey] = false;
            CreateTestObjects();
            var tbCampStart = _testObject.GetFieldOrProperty("tbCampStart") as TextBox;
            tbCampStart.Text = "2018.1.1";
            var tbCampEnd = _testObject.GetFieldOrProperty("tbCampEnd") as TextBox;
            tbCampEnd.Text = "2018.1.1";
            ShimSforceService.AllInstances.createsObjectArray = (p1, p2) => new SaveResult[] {
                new SaveResult { success = false, id = "1" , errors = new Error[] { new Error { message = "test error"} } } };
            var lbMessage = _testObject.GetFieldOrProperty("lbMessage") as Label;
            _sessionState["SFCampaigns"] = new List<SF_Campaign>();
            ShimSforceService.AllInstances.queryString = (p1, p2) => new QueryResult
            {
                records = new sObject[] { }
            };

            // Act
            _testObject.Invoke("btnCampOK_Click", new object[] { null, null });

            // Assert
            var resultList = _sessionState["SFCampaigns"] as List<SF_Campaign>;
            resultList.ShouldSatisfyAllConditions(
                () => resultList.ShouldNotBeNull(),
                () => resultList.Count.ShouldBe(0));
            lbMessage.Text.ShouldBe("Errors were found on item 0\r\nError code is: ALREADY_IN_PROCESS\r\nError message: test error\r\n");
        }

        [Test]
        public void BtnCampaignDataUpload_Click_Success()
        {
            // Arrange
            InitilizeFakes();
            _sessionState[LoggedInKey] = false;
            CreateTestObjects();
            var lbMessage = _testObject.GetFieldOrProperty("lbMessage") as Label;
            ShimSforceService.AllInstances.queryString = (p1, p2) => new QueryResult
            {
                records = new sObject[] { new sObject { } }
            };
            var ddlEcnEmailBlast = _testObject.GetFieldOrProperty("ddlEcnEmailBlast") as DropDownList;
            ddlEcnEmailBlast.Items.Add("1");
            ddlEcnEmailBlast.SelectedValue = "1";
            ShimECN_EmailActivityLog.GetByBlastIDInt32 = (p) => new List<ECN_EmailActivityLog> { new ECN_EmailActivityLog { } };
            Shimsfintegration.AllInstances.GetECNContactByEmailAddressString = (p1, p2) => new ECN_Contact { };
            Shimsfintegration.AllInstances.CreateSfContactSF_Contact = (p1, p2) => "1";
            ShimSforceService.AllInstances.createsObjectArray = (p1, p2) => new SaveResult[] {
                new SaveResult { success = false, id = "1" , errors = new Error[] { new Error { message = "test error"} } } };
            ShimSforceService.AllInstances.updatesObjectArray = (p1, p2) => new SaveResult[] {
                new SaveResult { success = false, id = "1" , errors = new Error[] { new Error { message = "test error"} } } };

            // Act
            _testObject.Invoke("btnCampaignDataUpload_Click", new object[] { null, null });

            // Assert
            lbMessage.Text.ShouldBe("Successfully Created/Updated 1 campaign member records. Per SalesForce please wait up to 10 minutes for data to be available.") ;
        }

        [Test]
        public void BtnCampaignDataUpload_Click_Error()
        {
            // Arrange
            InitilizeFakes();
            _sessionState[LoggedInKey] = false;
            CreateTestObjects();
            var lbMessage = _testObject.GetFieldOrProperty("lbMessage") as Label;
            ShimECN_EmailActivityLog.GetByBlastIDInt32 = (p) => new List<ECN_EmailActivityLog> {  };
            var ddlEcnEmailBlast = _testObject.GetFieldOrProperty("ddlEcnEmailBlast") as DropDownList;
            ddlEcnEmailBlast.Items.Add("1");
            ddlEcnEmailBlast.SelectedValue = "1";
            Shimsfintegration.AllInstances.GetSFContactsAll = (p) => new List<SF_Contact> { };

            // Act
            _testObject.Invoke("btnCampaignDataUpload_Click", new object[] { null, null });

            // Assert
            lbMessage.Text.ShouldBe("There is no data in the selected Campaign...Nothing will be sent to SalesForce");
        }

        [Test]
        public void EmailAddressToXML_ResturnEmailListXML()
        {
            // Arrange
            InitilizeFakes();
            CreateTestObjects();
            var emailList = new List<ECN_Contact> { new ECN_Contact { EmailAddress = "test@km.com"} };

            // Act
            var result = _testObject.Invoke("EmailAddressToXML", new object[] { emailList }) as XmlDocument;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.GetElementsByTagName("emailaddress").ShouldNotBeNull(),
                () => result.GetElementsByTagName("emailaddress")[0].InnerText.ShouldBe("test@km.com"));
        }

        [Test]
        public void btnSendToSF_Click_Success()
        {
            // Arrange
            InitilizeFakes();
            CreateTestObjects();
            _sessionState[LoggedInKey] = true;
            var emailList = new List<ECN_Contact> { new ECN_Contact { EmailAddress = "test@km.com" } };
            Shimsfintegration.AllInstances.GetECNContactsAll = (p) => new List<ECN_Contact> { new ECN_Contact { } };
            Shimsfintegration.AllInstances.GetSFAccounts = (p) => new List<SF_Account> { new SF_Account { AccountNumber = "ECN2010" } };
            ShimControl.AllInstances.FindControlString = (p1, p2) => new CheckBox { Checked = true };
            Shimsfintegration.AllInstances.GetSFAccountIDString = (p1, p2) => "1";
            Shimsfintegration.AllInstances.GetSFContactsAll = (p) => new List<SF_Contact> { new SF_Contact { } };
            Shimsfintegration.AllInstances.CreateSfContactsListOfSF_Contact = (p1, p2) => { };
            _testObject.Invoke("BindECNContactGrid", -1);
            var lbMessage = _testObject.GetFieldOrProperty("lbMessage") as Label;

            // Act
            _testObject.Invoke("btnSendToSF_Click", new object[] { null, null });

            // Assert     
            lbMessage.Text.ShouldBe("Successfully sent 1 contacts to SalesForce.com.");
        }

        [Test]
        public void GetSFAccountID_ReturnId()
        {
            // Arrange
            InitilizeFakes();
            CreateTestObjects();
            Shimsfintegration.AllInstances.GetSFAccounts = (p) => new List<SF_Account> { new SF_Account { Name = "company"} };
            ShimSforceService.AllInstances.createsObjectArray = (p1, p2) => new SaveResult[] {
                new SaveResult { success = true, id = "1" } };

            // Act
            var result  = _testObject.Invoke("GetSFAccountID", new object[] { "company" }) as string;

            // Assert     
            result.ShouldBe("1");
        }

        [Test]
        public void BtnImportToECN_Click_Success()
        {
            // Arrange
            InitilizeFakes();
            CreateTestObjects();
            _sessionState[LoggedInKey] = true;
            var emailList = new List<ECN_Contact> { new ECN_Contact { EmailAddress = "test@km.com" } };
            var gvSFContacts = _testObject.GetFieldOrProperty("gvSFContacts") as GridView;
            gvSFContacts.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = new CheckBox { ID = "cbRowItem", Checked = true} }
            });
            gvSFContacts.DataSource = new List<SF_Contact> { new SF_Contact { Email = "test@km.com" } };
            gvSFContacts.DataBind();
            var checkbox = gvSFContacts.Rows[0].FindControl("cbRowItem") as CheckBox;
            checkbox.Checked = true;
            var ddlECNGroupFromSF = _testObject.GetFieldOrProperty("ddlECNGroupFromSF") as DropDownList;
            ddlECNGroupFromSF.Items.Add("1");
            ddlECNGroupFromSF.SelectedValue = "1";
            var lbMessage = _testObject.GetFieldOrProperty("lbMessage") as Label;
            _testObject.SetFieldOrProperty("CustomerID", 1);
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString = (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) => null;

            // Act
            _testObject.Invoke("btnImportToECN_Click", new object[] { null, null });

            // Assert     
            lbMessage.Text.ShouldBe("Successfully imported 1 contacts from SalesForce.com");
        }

        [Test]
        public void AddNewGroup_Success()
        {
            // Arrange
            InitilizeFakes();
            CreateTestObjects();
            var ordinals = PrepareECNContactOrdinals();
            var parameters = new KeyValuePair<int, object>[]
            {
                new KeyValuePair<int, object>(ValueZero, EmailAddressDummy)
            };
            _testObject.SetFieldOrProperty("DBAdapter", CreateDatabaseAdapter("SELECT GroupID,GroupName FROM Groups WHERE CustomerID =1 AND GroupName=''",
                new KeyValuePair<string, int>[]
                {
                    new KeyValuePair<string, int>("GroupID", 0),
                    new KeyValuePair<string, int>("GroupName", 1)
                },
                new KeyValuePair<int, object>[]
                {
                    new KeyValuePair<int, object>(0, 1),
                    new KeyValuePair<int, object>(1, "TestGroup")
                }).Object);            
            var itemCollection = new SessionStateItemCollection();
            itemCollection[CustomerIdKey] = CustomerID;
            _testObject.SetFieldOrProperty("SessionCollection", GetSession(itemCollection));
            var ddlECNGroupFromSF = _testObject.GetFieldOrProperty("ddlECNGroupFromSF") as DropDownList;

            // Act
            _testObject.Invoke("AddNewGroup", new object[] { string.Empty });

            // Assert    
            ddlECNGroupFromSF.ShouldSatisfyAllConditions(
                () => ddlECNGroupFromSF.SelectedItem.Value.ShouldBe("1"),
                () => ddlECNGroupFromSF.SelectedItem.Text.ShouldBe("TestGroup"));
        }

        [Test]
        public void RblImportExport_SelectedIndexChanged_Success([Range(0,2)]int index)
        {
            // Arrange
            InitilizeFakes();
            CreateTestObjects();
            var rblImportExport = _testObject.GetFieldOrProperty("rblImportExport") as RadioButtonList;
            rblImportExport.Items.Add("0");
            rblImportExport.Items.Add("1");
            rblImportExport.Items.Add("2");
            rblImportExport.SelectedIndex = index;
            var itemCollection = new SessionStateItemCollection();
            itemCollection[CustomerIdKey] = CustomerID;
            _testObject.SetFieldOrProperty("SessionCollection", GetSession(itemCollection));
            Shimsfintegration.AllInstances.PopulateDropDownWithECNGroupsFromDBDropDownListListOfECN_Group = (p1, p2, p3) => { };
            Shimsfintegration.AllInstances.GetSFCampaignsAll = (p) => new List<SF_Campaign> { new SF_Campaign { } };
            ShimECN_Blasts.GetBlastByCustomerIDInt32 = (p) => new List<ECN_Blasts> { new ECN_Blasts { } };
            var pnlECNtoSF = _testObject.GetFieldOrProperty("pnlECNtoSF") as System.Web.UI.WebControls.Panel;
            var pnlSFtoECN = _testObject.GetFieldOrProperty("pnlSFtoECN") as System.Web.UI.WebControls.Panel;

            // Act
            _testObject.Invoke("rblImportExport_SelectedIndexChanged", new object[] { null,null });

            // Assert    
            _testObject.ShouldSatisfyAllConditions(
                () => pnlECNtoSF.Visible.ShouldBe(index == 0),
                () => pnlSFtoECN.Visible.ShouldBe(index == 1) );
        }

        [Test]
        public void CombineSfLeadsSfContacts_ReturnsListOfSF_Contact()
        {
            // Arrange
            InitilizeFakes();
            CreateTestObjects();
            Shimsfintegration.AllInstances.GetSFContactsAll = (p) => new List<SF_Contact> { new SF_Contact { Email = "test1" } };
            Shimsfintegration.AllInstances.GetSFLeads = (p) => new List<SF_Lead> { new SF_Lead { Email = "test2"} };

            // Act
            var result = _testObject.Invoke("CombineSfLeadsSfContacts", new object[] { }) as List<SF_Contact>;

            // Assert    
            _testObject.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Count.ShouldBe(2),
                () => result[0].Email.ShouldBe("test1"),
                () => result[1].Email.ShouldBe("test2"));
        }

        [Test]
        public void ConvertLeadToContact_ReturnsListOfSF_Contact()
        {
            // Arrange
            InitilizeFakes();
            CreateTestObjects();
            Shimsfintegration.AllInstances.GetSFLeads = (p) => new List<SF_Lead> { new SF_Lead { Email = "test" } };

            // Act
            var result = _testObject.Invoke("ConvertLeadToContact", new object[] { }) as List<SF_Contact>;

            // Assert    
            _testObject.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Count.ShouldBe(1),
                () => result[0].Email.ShouldBe("test"));
        }

        [Test]
        public void Page_Load_Success()
        {
            // Arrange
            InitilizeFakes();
            CreateTestObjects();
            _testPage.Session["LoggedIn"] = true;
            var pnlSFLogin = _testObject.GetFieldOrProperty("pnlSFLogin") as System.Web.UI.WebControls.Panel;

            // Act
            _testObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            pnlSFLogin.Visible.ShouldBeFalse();
        }

        [Test]
        public void Page_Load_WithoutLogin_Success()
        {
            // Arrange
            InitilizeFakes();
            CreateTestObjects();
            _testPage.Session["LoggedIn"] = false;
            var pnlSFLogin = _testObject.GetFieldOrProperty("pnlSFLogin") as System.Web.UI.WebControls.Panel;

            // Act
            _testObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            pnlSFLogin.Visible.ShouldBeTrue();
        }

        [Test]
        public void BtnCreateNewGroup_Click_Success([Values("testGroup","")]string groupName)
        {
            // Arrange
            InitilizeFakes();
            CreateTestObjects();
            var ordinals = PrepareECNContactOrdinals();
            var parameters = new KeyValuePair<int, object>[]
            {
                new KeyValuePair<int, object>(ValueZero, EmailAddressDummy)
            };
            var command = "INSERT INTO Groups (CustomerID,FolderID,GroupName,GroupDescription,OwnerTypeCode,PublicFolder,AllowUDFHistory,IsSeedList) \r\nVALUES(1,0,'testGroup','','customer',0,'N',0)\r\n";
            _testObject.SetFieldOrProperty("DBAdapter", CreateDatabaseAdapter(command, new KeyValuePair<string, int> []{ },  new KeyValuePair<int, object>[] { }).Object);
            var itemCollection = new SessionStateItemCollection();
            itemCollection[CustomerIdKey] = CustomerID;
            _testObject.SetFieldOrProperty("SessionCollection", GetSession(itemCollection));
            var tbNewGroupName = _testObject.GetFieldOrProperty("tbNewGroupName") as TextBox;
            tbNewGroupName.Text = groupName;
            var lbMessage = _testObject.GetFieldOrProperty("lbMessage") as Label;
            var addedGroup = string.Empty;
            Shimsfintegration.AllInstances.AddNewGroupString = (p1, p2) => addedGroup = p2;

            // Act
            _testObject.Invoke("btnCreateNewGroup_Click", new object[] { null, null });

            // Assert
            if (groupName.IsNullOrWhiteSpace())
            {
                lbMessage.Text.ShouldBe("Group Name can not be blank.");
            }
            else
            {
                _testObject.ShouldSatisfyAllConditions(
                    () => addedGroup.ShouldBe(groupName),
                    () => lbMessage.Text.ShouldBe("Group created successfully."));
            }            
        }

        private XmlElement GetElement(string xmlTag, string innerText)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<"+xmlTag+">" + innerText+ "</" + xmlTag + ">");
            return doc.DocumentElement;
        }

        private void InitilizeFakes()
        {
            var master = new ecn.communicator.MasterPages.Communicator();
            InitializeAllControls(master);
            ShimPage.AllInstances.MasterGet = (instance) => master;
            ShimECNSession.CurrentSession = () => {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new KMPlatform.Entity.User();
                session.CurrentCustomer = new ECN_Framework_Entities.Accounts.Customer();
                return session;
            };
            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                         new HttpStaticObjectsCollection(), 10, true,
                                         HttpCookieMode.AutoDetect,
                                         SessionStateMode.InProc, false);
            _sessionState = typeof(HttpSessionState).GetConstructor(
                                     BindingFlags.NonPublic | BindingFlags.Instance,
                                     null, CallingConventions.Standard,
                                     new[] { typeof(HttpSessionStateContainer) },
                                     null)
                                .Invoke(new object[] { sessionContainer }) as HttpSessionState;
            _sessionState[BindingKey] = new SforceService();
            ShimPage.AllInstances.SessionGet = (p) =>
            {
                return _sessionState;
            };
            ShimSforceService.AllInstances.loginStringString = (p1, p2, p3) => new LoginResult { serverUrl = "http://km.com" };
        }

        private void CreateTestObjects()
        {
            _testPage = new sfintegration();
            InitializeAllControls(_testPage);
            _testObject = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(_testPage);
        }
        private void InitializeAllControls(object page)
        {
            var fields = page.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.GetValue(page) == null)
                {
                    var constructor = field.FieldType.GetConstructor(new Type[0]);
                    if (constructor != null)
                    {
                        var obj = constructor.Invoke(new object[0]);
                        if (obj != null)
                        {
                            field.SetValue(page, obj);
                        }
                    }
                }
            }
        }

        private KeyValuePair<string, int>[] PrepareECNContactOrdinals()
        {
            return new KeyValuePair<string, int>[]
            {
                new KeyValuePair<string, int>(EmailAddressKey, ValueZero),
                new KeyValuePair<string, int>(FirstNameKey, ValueZero),
                new KeyValuePair<string, int>(LastNameKey, ValueZero),
                new KeyValuePair<string, int>(AddressKey, ValueZero),
                new KeyValuePair<string, int>(CityKey, ValueZero),
                new KeyValuePair<string, int>(StateKey, ValueZero),
                new KeyValuePair<string, int>(ZipKey, ValueZero),
                new KeyValuePair<string, int>(CountryKey, ValueZero),
                new KeyValuePair<string, int>(CompanyKey, ValueZero),
                new KeyValuePair<string, int>(TitleKey, ValueZero),
                new KeyValuePair<string, int>(VoiceKey, ValueZero),
                new KeyValuePair<string, int>(MobileKey, ValueZero),
                new KeyValuePair<string, int>(FaxKey, ValueZero)
            };
        }

        private static FieldInfo InitializeIntegrationField(
            sfintegration integration, 
            string fieldName,
            object valueToSet)
        {
            var fieldInfo = typeof(sfintegration).GetField(
                fieldName, 
                BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo.SetValue(integration, valueToSet);
            return fieldInfo;
        }

        private Mock<IDatabaseAdapter> CreateDatabaseAdapter(
            string commandText, 
            KeyValuePair<string, int>[] ordinals,
            KeyValuePair<int, object>[] parameters)
        {
            var dataReader = (Mock<IDataReader>)typeof(IDataReader).GetMock();
            dataReader.SetupSequence(x => x.Read()).Returns(true).Returns(false);
            dataReader.Setup(x => x.IsDBNull(It.IsAny<int>())).Returns(false);
            foreach (var pair in ordinals)
            {
                dataReader.Setup(x => x.GetOrdinal(pair.Key)).Returns(pair.Value);
            }

            foreach (var parameter in parameters)
            {
                dataReader.Setup(x => x[parameter.Key]).Returns(parameter.Value);
            }

            var command = (Mock<IDbCommand>)typeof(IDbCommand).GetMock();
            command.Setup(x => x.ExecuteReader()).Returns(dataReader.Object);
            command.Setup(x => x.ExecuteScalar()).Returns(ValueZero);

            var adapter = (Mock<IDatabaseAdapter>)typeof(IDatabaseAdapter).GetMock();

            var connection = (Mock<IDbConnection>)typeof(IDbConnection).GetMock();
            connection.Setup(x => x.Open());
            connection.Setup(x => x.Close());

            adapter.Setup(x => x.Connection).Returns(connection.Object);
            adapter.Setup(x => x.AddParameter(
                It.IsAny<IDbCommand>(), 
                It.IsAny<string>(),
                It.IsAny<SqlDbType>(), 
                It.IsAny<Object>()));
            adapter.Setup(x => x.CreateCommand()).Returns(command.Object);
            adapter.Setup(x => x.CreateCommand(commandText, It.IsAny<IDbConnection>())).Returns(command.Object);
            return adapter;
        }

        private HttpSessionState GetSession(SessionStateItemCollection itemCollection)
        {
            var state = (HttpSessionState)System.Runtime.Serialization
                .FormatterServices.GetUninitializedObject(typeof(HttpSessionState));

            var containerFld = typeof(HttpSessionState).GetField(
                "_container",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            containerFld.SetValue(
                state,
                new HttpSessionStateContainer(
                    ID,
                    itemCollection,
                    new HttpStaticObjectsCollection(),
                    Timeout,
                    true,
                    HttpCookieMode.UseCookies,
                    SessionStateMode.InProc,
                    false
                )
            );

            return state;
        }
    }
}

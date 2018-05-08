using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.Salesforce.Controls;
using ecn.communicator.main.Salesforce.Controls.Fakes;
using ecn.communicator.main.Salesforce.Entity;
using ecn.communicator.main.Salesforce.Entity.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Salesforce.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SF_SearchTest : PageHelper
    {
        private const string MethodGetQuery = "GetQuery";
        private const string MethodBindSearch = "BindSearch";
        private const string MethodDDLLogicIndexChanged = "ddlLogic_SelectedIndexChanged"; 
        private const string MethodGvSearchRowDataBound = "gvSearch_RowDataBound"; 
        private const string TestUser = "TestUser";
        private const string DummyString = "dummyString";
        private const string LayoutValueString = "1";
        private const string One = "1";
        private const string WhereString = "WHERE";
        private const string NameField = "Name";
        private const string AccountField = "AccountID";
        private const int MaxGvSearchRows = 2;
        private HttpSessionState _sessionState;
        private SF_Search _testEntity;
        private PrivateObject _privateTestObject;
        private IDisposable _context;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            _context = ShimsContext.Create();
            base.SetPageSessionContext();
            _testEntity = new SF_Search();
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            InitializeSessionFakes();
        }

        [TearDown]
        public void TearDwond()
        {
            _context.Dispose();
        }

        [TestCase("birthdate", "%")]
        [TestCase("postalCode", "%")]
        [TestCase("propertyInt", "%")]
        [TestCase("propertyDate", "%")]
        [TestCase("propertyString", "")]
        [TestCase("propertyDouble", "%")]
        [TestCase("propertyString", "%")]
        [TestCase("propertyString", "&")]
        [TestCase("propertyBoolean", "%")]
        public void GetQuery_WhenFieldIsNotAccountName_QueryStringDoesNotContainNameField(string fieldValue, string searchValue)
        {
            //Arrange 
            InitializeSessionVariables();
            ReflectionHelper.SetField(_testEntity, "gvSearch", InitializeGrid(fieldValue, searchValue));

            // Act
            var query = (string)ReflectionHelper.CallMethod(typeof(SF_Search), MethodGetQuery, null, _testEntity);

            // Assert
            query.ShouldSatisfyAllConditions(
                () => query.ShouldNotBeNullOrWhiteSpace(),
                () => query.ShouldContain(fieldValue),
                () => query.ShouldContain(DummyString),
                () => query.ShouldContain(WhereString),
                () => query.ShouldNotContain(NameField));
        }

        [TestCase("accountname", "%")]
        [TestCase("accountname", "&")]
        public void GetQuery_WhenFieldIsAccountNameAndIsAcccountIsFalse_QueryStringContainsAccountField(string fieldValue, string searchValue)
        {
            //Arrange 
            InitializeSessionVariables();
            ReflectionHelper.SetField(_testEntity, "gvSearch", InitializeGrid(fieldValue, searchValue));
            ReflectionHelper.SetField(_testEntity, "isAccount", false);
            ShimSF_Account.GetListStringString = (x, y) => new List<SF_Account> { ReflectionHelper.CreateInstance(typeof(SF_Account)) };

            // Act
            var query = (string)ReflectionHelper.CallMethod(typeof(SF_Search), MethodGetQuery, null, _testEntity);

            //Assert
            query.ShouldSatisfyAllConditions(
                () => query.ShouldNotBeNullOrWhiteSpace(),
                () => query.ShouldContain(DummyString),
                () => query.ShouldContain(WhereString),
                () => query.ShouldContain(AccountField),
                () => query.ShouldNotContain(NameField));
        }

        [TestCase("accountname", "%")]
        public void GetQuery_WhenFieldIsAccountNameAndIsAcccountIsTrue_QueryStringContainsNameField(string fieldValue, string searchValue)
        {
            //Arrange 
            InitializeSessionVariables();
            ReflectionHelper.SetField(_testEntity, "gvSearch", InitializeGrid(fieldValue, searchValue));
            ReflectionHelper.SetField(_testEntity, "isAccount", true);

            // Act
            var query = (string)ReflectionHelper.CallMethod(typeof(SF_Search), MethodGetQuery, null, _testEntity);

            //Assert
            query.ShouldSatisfyAllConditions(
                () => query.ShouldNotBeNullOrWhiteSpace(),
                () => query.ShouldContain(DummyString),
                () => query.ShouldContain(WhereString),
                () => query.ShouldNotContain(AccountField),
                () => query.ShouldContain(NameField));
        }

        [TestCase(SF_Utilities.SFObject.Lead)]
        [TestCase(SF_Utilities.SFObject.Account)]
        [TestCase(SF_Utilities.SFObject.Contact)]
        public void BindSearch_Success_SearchGridViewIsPopulated(SF_Utilities.SFObject sfObject)
        {
            //Arrange 
            InitializeSessionVariables();
            var methodArgs = new object[] { sfObject };

            // Act
            ReflectionHelper.CallMethod(typeof(SF_Search), MethodBindSearch, methodArgs, _testEntity);

            //Assert
            var searchGridView = ReflectionHelper.GetFieldValue(_testEntity, "gvSearch") as GridView;
            _testEntity.ShouldSatisfyAllConditions(
                () => searchGridView.ShouldNotBeNull(),
                () => searchGridView.DataSource.ShouldNotBeNull());
        }

        [TestCase("propertyString", "&")]
        [TestCase("propertyBoolean", "%")]
        [TestCase("accountname", "%")]
        public void DdlLogic_SelectedIndexChanged_WhenSelectedIndexIsGreaterThanZero_GiListContainsSelectedFiled(string fieldValue, string searchValue)
        {
            //Arrange 
            InitializeSessionVariables();
            var gridViewRowObject = GetGridViewRow(1);
            ReflectionHelper.SetField(_testEntity, "gvSearch", InitializeGrid(fieldValue, searchValue));
            var methodArgs = new object[] { gridViewRowObject.Cells[0].FindControl("ddlLogic"), EventArgs.Empty };

            // Act
            ReflectionHelper.CallMethod(typeof(SF_Search), MethodDDLLogicIndexChanged, methodArgs, _testEntity);

            //Assert
            var giList = _sessionState["listGI"] as List<GridItem>;

            _testEntity.ShouldSatisfyAllConditions(
                () => giList.ShouldNotBeNull(),
                () => giList.Count.ShouldBe(3),
                () => giList.Find(x => x.SelectedField == fieldValue).ShouldNotBeNull());
        }

        [TestCase("propertyString", "&")]
        [TestCase("propertyBoolean", "%")]
        [TestCase("accountname", "%")]
        public void DdlLogic_SelectedIndexChanged_WhenSelectedIndexIsZero_GiListContainsSelectedFiled(string fieldValue, string searchValue)
        {
            //Arrange 
            InitializeSessionVariables();
            var gridViewRowObject = GetGridViewRow(0);
            ReflectionHelper.SetField(_testEntity, "gvSearch", InitializeGrid(fieldValue, searchValue, 3));
            var methodArgs = new object[] { gridViewRowObject.Cells[0].FindControl("ddlLogic"), EventArgs.Empty };

            // Act
            ReflectionHelper.CallMethod(typeof(SF_Search), MethodDDLLogicIndexChanged, methodArgs, _testEntity);

            //Assert
            var giList = _sessionState["listGI"] as List<GridItem>;
            _testEntity.ShouldSatisfyAllConditions(
                () => giList.ShouldNotBeNull(),
                () => giList.Count.ShouldBe(2),
                () => giList.Find(x => x.SelectedField == fieldValue).ShouldNotBeNull());
        }

        [TestCase("propertyString", "&")]
        [TestCase("propertyBoolean", "%")]
        [TestCase("accountname", "%")]
        public void GvSearch_RowDataBound_Success_DropDownsAreInitialized(string fieldValue, string searchValue)
        {
            //Arrange 
            InitializeSessionVariables();
            var gridViewRowObject = InitializeGrid(fieldValue, searchValue).Rows[0];
            var gridItem = ReflectionHelper.CreateInstance(typeof(GridItem));
            gridItem.SelectedField = fieldValue;
            gridViewRowObject.DataItem = gridItem;
            ReflectionHelper.SetField(_testEntity, "gvSearch", InitializeGrid(fieldValue, searchValue));
            var methodArgs = new object[] { null, new GridViewRowEventArgs(gridViewRowObject) };
            var dropDownsInitialized = false;
            ShimSF_Search.AllInstances.GetLogicBoolean = (x, y) =>
            {
                dropDownsInitialized = true;
                return new List<string>();
            };

            // Act
            ReflectionHelper.CallMethod(typeof(SF_Search), MethodGvSearchRowDataBound, methodArgs, _testEntity);

            //Assert
            dropDownsInitialized.ShouldBeTrue();
        }

        private GridViewRow GetGridViewRow(int selectedIndex)
        {
            var gridViewRow = new GridViewRow(1, 1, DataControlRowType.DataRow, DataControlRowState.Insert);
            gridViewRow.DataItem = ReflectionHelper.CreateInstance(typeof(CampaignItemTemplateGroup));
            gridViewRow.Cells.Add(new TableCell());
            var logicDropDownList = new DropDownList
            {
                ID = "ddlLogic",
            };
            if (selectedIndex == 0)
            {
                logicDropDownList.Items.Add(new ListItem
                {
                    Selected = true,
                    Text = DummyString
                });
            }
            if (selectedIndex == 1)
            {
                logicDropDownList.Items.Add(new ListItem
                {
                    Selected = false,
                    Text = DummyString
                });
                logicDropDownList.Items.Add(new ListItem
                {
                    Selected = true,
                    Text = DummyString
                });
            }
            gridViewRow.Cells[0].Controls.Add(logicDropDownList);
            return gridViewRow;
        }

        private void InitializeSessionVariables()
        {
            var dummyPropertiesObject = new
            {
                propertyBoolean = false,
                propertyDouble = (double)1,
                propertyInt = 1,
                propertyString = string.Empty,
                postalCode = string.Empty,
                birthdate = DateTime.Now,
                propertyDate = DateTime.Now
            };
            var listFields = new List<PropertyInfo>
            {
                dummyPropertiesObject.GetType().GetProperty("propertyBoolean"),
                dummyPropertiesObject.GetType().GetProperty("propertyDouble"),
                dummyPropertiesObject.GetType().GetProperty("propertyInt"),
                dummyPropertiesObject.GetType().GetProperty("propertyString"),
                dummyPropertiesObject.GetType().GetProperty("postalCode"),
                dummyPropertiesObject.GetType().GetProperty("birthdate"),
                dummyPropertiesObject.GetType().GetProperty("propertyDate"),
            };
            _sessionState.Add("listFields", listFields);
            _sessionState.Add("listGI", new List<GridItem>
            {
                ReflectionHelper.CreateInstance(typeof(GridItem)),
                ReflectionHelper.CreateInstance(typeof(GridItem)),
                ReflectionHelper.CreateInstance(typeof(GridItem))
            });
        }

        private GridView InitializeGrid(string fieldValue, string searchValue, int numberOfRows = 2)
        {
            var dataTable = new DataTable("grdSearch");
            dataTable.Columns.Add("ddlField", typeof(string));
            dataTable.Columns.Add("ddlLogic", typeof(string));
            dataTable.Columns.Add("ddlBoolean", typeof(string));
            dataTable.Columns.Add("txtSearch", typeof(string));
            for (var i = 0; i < numberOfRows; i++)
            {
                dataTable.Rows.Add(One, One, One, One);
            }
            var gvSearch = new GridView();
            gvSearch.DataSource = dataTable;
            gvSearch.DataBind();
            for (var i = 0; i < numberOfRows; i++)
            {
                gvSearch.Rows[i].Cells[0].Controls.Add(new DropDownList
                {
                    ID = "ddlField",
                    Items =
                    {
                        new ListItem
                        {
                            Selected = true,
                            Text = fieldValue
                        }
                    }
                });
                gvSearch.Rows[i].Cells[1].Controls.Add(new DropDownList
                {
                    ID = "ddlLogic",
                    Items =
                    {
                        new ListItem
                        {
                            Selected = true,
                            Text = DummyString
                        }
                    }
                });
                gvSearch.Rows[i].Cells[2].Controls.Add(new DropDownList
                {
                    ID = "ddlBoolean",
                    Items =
                    {
                        new ListItem
                        {
                            Selected = true,
                            Text = DummyString
                        }
                    }
                });
                gvSearch.Rows[i].Cells[3].Controls.Add(new TextBox
                {
                    ID = "txtSearch",
                    Text = searchValue
                });
            }
            return gvSearch;
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            shimSession.Instance.CurrentUser = new User()
            {
                UserID = 1,
                UserName = TestUser,
                IsActive = true
            };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
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
            ShimPage.AllInstances.SessionGet = (p) =>
            {
                return _sessionState;
            };
            ShimUserControl.AllInstances.SessionGet = (c) =>
            {
                return _sessionState;
            };
            ReflectionHelper.CreateInstance(typeof(SF_Search));
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Fakes;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using ecn.communicator.listsmanager;
using ecn.communicator.listsmanager.Fakes;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Objects.Fakes;
using ECN_Framework_Entities.Application;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity.Fakes;
using ECNBusiness = ECN_Framework_BusinessLayer.Application;
using ECNMasterPage = ecn.communicator.MasterPages;
using ECNMasterPageFake = ecn.communicator.MasterPages.Fakes;
using KMCommon = KM.Common.Entity;

namespace ECN.Communicator.Tests.Main.Lists
{
    /// <summary>
    ///	 Unit Tests for <see cref="ecn.communicator.listsmanager.importDatafromFile"/>
    /// </summary>
    [ExcludeFromCodeCoverage]
	[TestFixture]
	public class ImportDataFromFileTest
	{
		private IDisposable _context;
        private PrivateObject _testObject;
        private importDatafromFile _importDataFromFile;
		private Type _importDataFromFileType;
		private DataTable _dataFile;
		private	Label _errlabel;
		private Label _lblGUID;
		private Button _importButton;
		private string _column1;
		private string _column2;
		private string _column3;
		private string _reqParam;
		private string _gid;
		private GridView _gvImport;
		private const int NumberOfRows = 110;
        private const string ignore = "Ignore";
        private const string boxName = "name";
        private const string colName = "test";

        [SetUp]
		public void SetUp()
		{
			_context = ShimsContext.Create();
            _testObject = new PrivateObject(new importDatafromFile());
        }

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}

        [Test]
        public void BuildColumnHeaderDropdowns_PassColumnWhichExist_ReturnSelectWithSelectedTextAndHideIgnored()
        {
            // Arrange
            _testObject.SetFieldOrProperty("_groupDataFields", new List<GroupDataFields>() { new GroupDataFields { ShortName = colName } });

            // Act
            var result = _testObject.Invoke("buildColumnHeaderDropdowns", boxName, colName) as HtmlSelect;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Items.FindByText(ignore).Selected.ShouldBeFalse(),
                () => result.Items.FindByText($"user_{colName}").Selected.ShouldBeTrue());
        }

        [Test]
        public void BuildColumnHeaderDropdowns_PassColumnWhichNotExist_ReturnSelectSelectedIgnored()
        {
            // Arrange
            _testObject.SetFieldOrProperty("_groupDataFields", new List<GroupDataFields>());

            // Act
            var result = _testObject.Invoke("buildColumnHeaderDropdowns", boxName, colName) as HtmlSelect;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Items.FindByText(ignore).Selected.ShouldBeTrue(),
                () => result.Items.FindByText(colName).ShouldBeNull(),
                () => result.Items.FindByText($"user_{colName}").ShouldBeNull());
        }

        [Test]
		public void ImportData_WhenDuplicateColumnsProvidedInRequestParams_DisplaysError()
		{
			// Arrange
			Initialize();
			_column1 = "same";
			_column2 = "same";
			_column3 = "ignore";
			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFile);

			// Assert
			_errlabel.Text.ShouldNotBeNullOrWhiteSpace();
			_errlabel.Visible.ShouldBeTrue();
		}

		[Test]
		public void ImportData_WhenRequestParamsAreEmpty_DisplaysError()
		{
			// Arrange
			Initialize();
			ShimHttpRequest.AllInstances.ParamsGet = (x) =>
			{
				return new NameValueCollection();
			};
			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFile);

			// Assert
			_errlabel.ShouldSatisfyAllConditions(
				() => _errlabel.Text.ShouldNotBeNullOrWhiteSpace(),
				() => _errlabel.Visible.ShouldBeTrue());
		}

		[Test]
		public void ImportData_WhenFailsGettingDataFromFile_ErrorIsLoggedAndDisplayed()
		{
			// Arrange
			var errorLogged = false;
			Initialize();
			ShimimportDatafromFile.AllInstances.GetDataTableByFileTypeStringStringInt32 = (a, b, c, maxRecordsToRetrieve) => 
			{
				if (maxRecordsToRetrieve == 0)
				{
					throw (Exception)CreateInstance(typeof(OleDbException));
				}
				else
				{
					return _dataFile;
				}
			};
			ShimApplicationLog.SaveApplicationLogRef = (ref KMCommon.ApplicationLog a) => 
			{
				errorLogged = true;
				return true;
			};

			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFile);

			// Assert
			errorLogged.ShouldBeTrue();
			_errlabel.Text.ShouldNotBeNullOrWhiteSpace();
			_errlabel.Visible.ShouldBeTrue();
		}

		[Test]
		public void ImportData_WhenEmailNotProvided_DisplaysError()
		{
			// Arrange
			Initialize();
			ShimImportMapper.AllInstances.HasEmailAddressGet = (x) => { return false; };
			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFile);

			// Assert
			_errlabel.Text.ShouldNotBeNullOrWhiteSpace();
			_errlabel.Visible.ShouldBeTrue();
		}

		[Test]
		public void ImportData_WhenFileDoesNotHaveRows_DisplaysError()
		{
			// Arrange
			Initialize();
			var methodArgs = new object[] { this, EventArgs.Empty };
			ShimimportDatafromFile.AllInstances.GetDataTableByFileTypeStringStringInt32 = (a, b, c, d) => { return new DataTable(); };
			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFile);

			// Assert
			_errlabel.Text.ShouldNotBeNullOrWhiteSpace();
			_errlabel.Visible.ShouldBeTrue();
		}
		[Test]
		public void ImportData_WhenFailed_DisplaysError()
		{
			// Arrange
			Initialize();
			var methodArgs = new object[] { this, EventArgs.Empty };
			ShimimportDatafromFile.AllInstances.GetDataTableByFileTypeStringStringInt32 = (a, b, c, d) =>
			{
				throw (Exception)CreateInstance(typeof(OleDbException));
			};
			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFile);

			// Assert
			_errlabel.Text.ShouldNotBeNullOrWhiteSpace();
			_errlabel.Visible.ShouldBeTrue();
		}

		[Test]
		public void ImportData_WithColumnsAndRowsSupplied_ResultContainsSuppliedColumnsAndRows()
		{
			//Arrage
			Initialize();
			_dataFile = new DataTable{ Columns = { "1", "2", "3", } };
			var cell1 = "cell1";
			var cell2 = "cell2";
			var cell3 = "cell3";
			_dataFile.Rows.Add(cell1, cell2, cell3);
			_column1 = "column1";
			_column2 = "column2";
			_column3 = "column3";
			var result = "";
			ShimimportDatafromFile.AllInstances.UpdateToDBInt32Int32StringStringBoolean = (a, b, c, xmlProfile, e, f) => 
			{
				result = xmlProfile;
			};
			var methodArgs = new object[] { this, EventArgs.Empty };
			
			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFile);

			// Assert
			result.ShouldSatisfyAllConditions(
				() => result.ShouldNotBeNullOrWhiteSpace(),
				() => result.ShouldContain(cell1),
				() => result.ShouldContain(cell2),
				() => result.ShouldContain(cell3),
				() => result.ShouldContain(_column1),
				() => result.ShouldContain(_column1),
				() => result.ShouldContain(_column1));
		}

		[TestCase("user_","user defined")]
		[TestCase("ignore", "ignored value")]
		public void ImportData_WhenRequestContainIgnoredColumnsOrUserColumns_ResultNotContainsIgnoredColumnsAndUserColumns(string columnName, string cellValue)
		{
			//Arrage
			Initialize();
			_dataFile = CreateDataTable("1", "2", "3");
			var cell1 = "cell1";
			var cell2 = cellValue;
			var cell3 = "cell3";
			_dataFile.Rows.Add(cell1, cell2, cell3);
			_column1 = "column1";
			_column2 = columnName;
			_column3 = "emailaddress";
			var result = "";
			ShimimportDatafromFile.AllInstances.UpdateToDBInt32Int32StringStringBoolean = (a, b, c, xmlProfile, e, f) =>
			{
				result = xmlProfile;
			};
			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFile);

			// Assert
			result.ShouldNotBeNullOrWhiteSpace();
			result.ShouldNotContain(_column2);
			result.ShouldNotContain(cell2);
		}

		[TestCase("emailaddress", "email@email.com")]
		public void ImportData_WhenRequestContainEmailColumn_ResultContainEmailColumns(string columnName, string cellValue)
		{
			//Arrage
			Initialize();
			_dataFile = CreateDataTable("1", "2", "3");
			var cell1 = "cell1";
			var cell2 = cellValue;
			var cell3 = "cell3";
			_dataFile.Rows.Add(cell1, cell2, cell3);
			_column1 = "column1";
			_column2 = columnName;
			_column3 = "column3";
			var result = "";
			ShimimportDatafromFile.AllInstances.UpdateToDBInt32Int32StringStringBoolean = (a, b, c, xmlProfile, e, f) =>
			{
				result = xmlProfile;
			};
			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFile);

			// Assert
			result.ShouldSatisfyAllConditions(
					() => result.ShouldNotBeNullOrWhiteSpace(),
					() => result.ShouldContain(_column2),
					() => result.ShouldContain(cell2)
				);
		}

		[Test]
		public void ImportData_MoreThan100Rows_NotificationAndProgressIsDisplayed()
		{
			//Arrage
			Initialize();
			_dataFile = CreateDataTable("1", "2", "3");
			var cell1 = "cell1";
			var cell2 = "users_defined";
			var cell3 = "cell3";
			for (var i = 0; i < NumberOfRows; i++)
			{
				_dataFile.Rows.Add(cell1, cell2, cell3);
			}
			_column1 = "column1";
			_column2 = "user_";
			_column3 = "emailaddress";
			var notificationShown = false;
			var progressShown = false;
			ShimimportDatafromFile.AllInstances.NotifyStringString = (x, y, z) =>
			{
				progressShown = true;
			};
			ShimimportDatafromFile.AllInstances.initNotifyString = (x, y) =>
			{
				notificationShown = true;
			};
			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFile);

			// Assert
			notificationShown.ShouldBeTrue();
			progressShown.ShouldBeTrue();
		}

		[Test]
		public void ImportData_WhenFileContainsValidData_DataIsSavedToDB()
		{
			//Arrage
			Initialize();
			_dataFile = CreateDataTable("1", "2", "3");
			var cell1 = "cell1";
			var cell2 = "emailaddress";
			var cell3 = "cell3";
			_dataFile.Rows.Add(cell1, cell2, cell3);
			_column1 = "column1";
			_column2 = "emailaddress";
			_column3 = "column3";
			var dataSaved = false;
			ShimimportDatafromFile.AllInstances.UpdateToDBInt32Int32StringStringBoolean = (a, b, c, xmlProfile, e, f) =>
			{
				dataSaved = true;
			};
			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFile);

			// Assert
			dataSaved.ShouldBeTrue();
		}

		[Test]
		public void ImportData_WhenValidDataGiven_SummaryOfDataIsShownInGrid()
		{
			//Arrage
			Initialize();
			_dataFile = CreateDataTable("1", "2", "3");
			var cell1 = "cell1";
			var cell2 = "emailaddress";
			var cell3 = "cell3";
			_dataFile.Rows.Add(cell1, cell2, cell3);
			_column1 = "column1";
			_column2 = "emailaddress";
			_column3 = "column3";
			ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString = (a, b, c, d, e, f, g, h, i, j) =>
			{
				var dtable = new DataTable() { Columns = { "ACTION", "Counts" } };
				dtable.Rows.Add("T", "2");
				dtable.Rows.Add("I", "2");
				dtable.Rows.Add("U", "2");
				dtable.Rows.Add("D", "2");
				dtable.Rows.Add("S", "2");
				dtable.Rows.Add("M", "2");
				dtable.Rows.Add("D", "2");
				dtable.Rows.Add("S", "2");
				dtable.Rows.Add("M", "2");
				return dtable;
			};
			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFile);

			// Assert
			var duplicates = _gvImport.Rows[3].Cells[3].Text;
			_gvImport.Visible.ShouldBeTrue();
			duplicates.ShouldBe("4");
		}

        [Test]
        public void BuildEmailImportForm_Success([Values("C", "S")]string fileType)
        {
            //Arrage
            var dataTable = new DataTable { Columns = { "1","2"},Rows = { {"1","2" } } };
            var mapper = new ImportMapper { };
            var testObject = new importDatafromFile();
            InitializeAllControls(testObject);
            var privateObject = new PrivateObject(testObject);
            privateObject.SetFieldOrProperty("fileType", fileType);
            var dataCollectionTable = privateObject.GetFieldOrProperty("dataCollectionTable") as HtmlTable;

            // Act
            privateObject.Invoke("BuildEmailImportForm", new object[] { dataTable, mapper });

            // Assert
            dataCollectionTable.Rows.Count.ShouldBe(2);
        }

        [Test]
        public void GvImport_Command_Success()
        {
            //Arrage
            CreateShims();
            var eventArgs = new GridViewCommandEventArgs(null, new CommandEventArgs(string.Empty,"U"));
            var testObject = new importDatafromFile();
            InitializeAllControls(testObject);
            var privateObject = new PrivateObject(testObject);
            ShimDirectory.ExistsString = p => false;
            ShimDirectory.CreateDirectoryString = p => null;
            ShimFile.ExistsString = p => true;
            ShimFile.DeleteString = p => { };
            ShimFile.AppendTextString = p => new ShimStreamWriter();
            ShimEmailGroup.ExportFromImportEmailsUserStringString = (p1, p2, p3) => new DataTable { Columns = { "1" }, Rows = { { "1" } } };
            var responseHeader = string.Empty;
            ShimHttpResponse.AllInstances.ContentTypeSetString = (p1, p2) => { };
            ShimHttpResponse.AllInstances.AddHeaderStringString = (p1, p2, p3) => responseHeader += p2 + " " + p3;
            ShimHttpResponse.AllInstances.WriteFileString = (p1, p2) => { };
            ShimHttpResponse.AllInstances.End = (p) => { };
            ShimPage.AllInstances.ResponseGet = (p) =>new System.Web.HttpResponse(TextWriter.Null);

            // Act
            privateObject.Invoke("gvImport_Command", new object[] { null, eventArgs });

            // Assert
            responseHeader.ShouldBe("content-disposition attachment; filename=U-.xls");
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

        private void CallMethod(Type type, string methodName, object[] parametersValues, object instance = null)
		{
			ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
		}

		private dynamic CreateInstance(Type type)
		{
			return ReflectionHelper.CreateInstance(type);
		}

		private void CreateShims()
		{
			ConfigurationManager.AppSettings["KMCommon_Application"] = "1";
			ShimAuthenticationTicket.getTicket = () =>
			{
				AuthenticationTicket authTkt = CreateInstance(typeof(AuthenticationTicket));
				SetField(authTkt, "CustomerID", 1);
				return authTkt;
			};
			ShimECNSession.AllInstances.RefreshSession = (item) => { };
			ShimECNSession.AllInstances.ClearSession = (itme) => { };
			ShimECNSession.CurrentSession = () =>
			{
				ECNBusiness.ECNSession ecnSession = CreateInstance(typeof(ECNBusiness.ECNSession));
				SetField(ecnSession, "CustomerID", 1);
				SetField(ecnSession, "BaseChannelID", 1);
				return ecnSession;
			};
			ECNMasterPageFake.ShimCommunicator.AllInstances.UserSessionGet = (x) => 
			{
				return CreateInstance(typeof(ECNBusiness.ECNSession));
			};
			HttpContext.Current = MockHelpers.FakeHttpContext();
			ShimPage.AllInstances.SessionGet = x => { return HttpContext.Current.Session; };
			ShimPage.AllInstances.RequestGet = (x) => { return HttpContext.Current.Request; };
			ShimPage.AllInstances.MasterGet = (x) => { return new MasterPage() { }; };
			ShimHttpRequest.AllInstances.ParamsGet = (x) =>
			{
				return new NameValueCollection()
				{
					{ _reqParam+"0", _column1 },
					{ _reqParam+"1", _column2 },
					{ _reqParam+"2", _column3 },
				};
			};
			ShimimportDatafromFile.AllInstances.MasterGet = (x) =>
			{
				return new ECNMasterPage.Communicator();
			};
			ShimimportDatafromFile.AllInstances.ResetImportMapper = (x, y) => { _importButton.Visible = true; };
			ShimImportMapper.AllInstances.HasEmailAddressGet = (x) => { return true; };
			ShimimportDatafromFile.AllInstances.GetDataTableByFileTypeStringStringInt32 = (a, b, c, d) => { return _dataFile; };
			ShimApplicationLog.SaveApplicationLogRef = (ref KMCommon.ApplicationLog a) => { return true; };
			ShimimportDatafromFile.AllInstances.GetGroupDataFieldsInt32 = (x, y) =>
			{
				Hashtable hshTbl = new Hashtable()
				{
					{"user_", "A-51"}
				};
				return hshTbl;
			};
			ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString = (a, b, c, d, e, f, g, h, i, j) =>
			{
				var dtable = new DataTable();
				return dtable;
			};
		}

		private void Initialize()
		{
			CreateShims();
			_importDataFromFileType = typeof(importDatafromFile);
			_importDataFromFile = CreateInstance(_importDataFromFileType);
			_dataFile = new DataTable();
			_errlabel = new Label();
			_importButton = new Button();
			_lblGUID = new Label();
			_gvImport = new GridView();
			SetDefaults();
		}
		private void SetDefaults()
		{
			_dataFile = new DataTable
			{
				Columns =
					{
						"TestColumn1",
						"TestColumn2",
						"TestColumn3",
					},
				TableName = "TestTable"
			};
			_dataFile.Rows.Add("row1", "row1", "row1");
			_reqParam = "$ContentPlaceHolder1$ColumnHeaderSelect";
			_column1 = "col1";
			_column2 = "col2";
			_column3 = "ignore";
			_gid = "22";
			_lblGUID.Text = "testString";
			SetField(_importDataFromFile, "errlabel", _errlabel);
			SetField(_importDataFromFile, "ImportButton", _importButton);
			SetField(_importDataFromFile, "gid", _gid);
			SetField(_importDataFromFile, "lblGUID", _lblGUID);
			SetField(_importDataFromFile, "gvImport", _gvImport);
		}

		private DataTable CreateDataTable(string colName1,string colName2,string colName3)
		{
			return new DataTable { Columns = { colName1, colName2, colName3, } };
		}

		private void SetField(dynamic obj, string fieldName, dynamic value)
		{
			ReflectionHelper.SetField(obj, fieldName, value);
		}

		private void SetSessionVariable(string name, object value)
		{
			HttpContext.Current.Session.Add(name, value);
		}
	}
}

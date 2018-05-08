using System;
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
using ecn.communicator.listsmanager;
using ecn.communicator.listsmanager.Fakes;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Objects.Fakes;
using ECN_Framework_Entities.Application;
using KM.Common.Entity.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using CommunicatorGroupDataFields = ECN_Framework_Entities.Communicator.GroupDataFields;
using ECNBusiness = ECN_Framework_BusinessLayer.Application;
using ECNMasterPage = ecn.communicator.MasterPages;
using ECNMasterPageFake = ecn.communicator.MasterPages.Fakes;
using FunctionFakes = ECN_Framework_Common.Functions.Fakes;
using KMCommon = KM.Common.Entity;

namespace ECN.Communicator.Tests.Main.Lists
{
    /// <summary>
    ///	 Unit Tests for <see cref="ecn.communicator.listsmanager.ATHBimportDatafromFile"/>
    /// </summary>
    [ExcludeFromCodeCoverage]
	[TestFixture]
	public class ATHBImportDataFromFileTest
	{
		private const string LineStart = "3";
		private const string QueryStringGID = "gid";
		private const string QueryStringGIDValue = "1";
		private IDisposable _context;
		private ATHBimportDatafromFile _importDataFromFileObject;
		private Type _importDataFromFileType;
		private DataTable _dataFile;
		private Label _errlabel;
		private Label _lblGUID;
		private Button _importButton;
		private string _column1;
		private string _column2;
		private string _column3;
		private string _reqParam;
		private string _gid;
		private GridView _gvImport;
		private const int NumberOfRows = 110;
		private HtmlTable _dataCollectionTable;
		private EmailTableColumnManager _columnManager;

		[SetUp]
		public void SetUp()
		{
			_context = ShimsContext.Create();
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
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
				_importDataFromFileObject);

			// Assert
			_errlabel.ShouldNotBeNull();
			_errlabel.ShouldSatisfyAllConditions(
				() => _errlabel.Text.ShouldNotBeNullOrWhiteSpace(),
				() => _errlabel.Visible.ShouldBeTrue());
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
				_importDataFromFileObject);

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
			ShimATHBimportDatafromFile.AllInstances.GetDataTableByFileTypeStringStringInt32 = (a, b, c, maxRecordsToRetrieve) =>
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
			ShimApplicationLog.SaveApplicationLogRef = (ref KMCommon.ApplicationLog a) => true;
			ShimATHBimportDatafromFile.AllInstances.ResetImportMapper = (x, y) =>
			{
				errorLogged = true;
			};

			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFileObject);

			// Assert
			_errlabel.ShouldSatisfyAllConditions(
				() => _errlabel.Text.ShouldNotBeNullOrWhiteSpace(),
				() => _errlabel.Visible.ShouldBeTrue(),
				() => errorLogged.ShouldBeTrue());
		}

		//[Test]
		public void ImportData_WhenEmailNotProvided_DisplaysError()
		{
			// Arrange
			Initialize();
			ShimImportMapper.AllInstances.HasEmailAddressGet = (x) => false;
			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFileObject);

			// Assert
			_errlabel.ShouldNotBeNull();
			_errlabel.ShouldSatisfyAllConditions(
				() => _errlabel.Text.ShouldNotBeNullOrWhiteSpace(),
				() => _errlabel.Visible.ShouldBeTrue());
		}

		[Test]
		public void ImportData_WhenFileDoesNotHaveRows_DisplaysError()
		{
			// Arrange
			Initialize();
			var methodArgs = new object[] { this, EventArgs.Empty };
			ShimATHBimportDatafromFile.AllInstances.GetDataTableByFileTypeStringStringInt32 = (a, b, c, d) => { return new DataTable(); };
			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFileObject);

			// Assert
			_errlabel.ShouldNotBeNull();
			_errlabel.ShouldSatisfyAllConditions(
				() => _errlabel.Text.ShouldNotBeNullOrWhiteSpace(),
				() => _errlabel.Visible.ShouldBeTrue());
		}
		[Test]
		public void ImportData_WhenFailed_DisplaysError()
		{
			// Arrange
			Initialize();
			var methodArgs = new object[] { this, EventArgs.Empty };
			ShimATHBimportDatafromFile.AllInstances.GetDataTableByFileTypeStringStringInt32 = (a, b, c, d) =>
			{
				_errlabel.Text = "dummyErrorLable";
				_errlabel.Visible = true;
				return null;
			};
			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFileObject);

			// Assert
			_errlabel.ShouldNotBeNull();
			_errlabel.ShouldSatisfyAllConditions(
				() => _errlabel.Text.ShouldNotBeNullOrWhiteSpace(),
				() => _errlabel.Visible.ShouldBeTrue());
		}

		[Test]
		public void ImportData_WithColumnsAndRowsSupplied_ResultContainsSuppliedColumnsAndRows()
		{
			//Arrage
			Initialize();
			_dataFile = new DataTable { Columns = { "1", "2", "3", } };
			var cell1 = "cell1";
			var cell2 = "cell2";
			var cell3 = "cell3";
			_dataFile.Rows.Add(cell1, cell2, cell3);
			_column1 = "column1";
			_column2 = "column2";
			_column3 = "column3";
			var result = "";
			ShimATHBimportDatafromFile.AllInstances.UpdateToDBInt32Int32StringStringBoolean = (a, b, c, xmlProfile, e, f) =>
			{
				result = xmlProfile;
			};
			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFileObject);

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

		[TestCase("user_", "user defined")]
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
			ShimATHBimportDatafromFile.AllInstances.UpdateToDBInt32Int32StringStringBoolean = (a, b, c, xmlProfile, e, f) =>
			{
				result = xmlProfile;
			};
			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFileObject);

			// Assert
			result.ShouldSatisfyAllConditions(
				() => result.ShouldNotBeNullOrWhiteSpace(),
				() => result.ShouldNotContain(_column2),
				() => result.ShouldNotContain(cell2));
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
			ShimATHBimportDatafromFile.AllInstances.UpdateToDBInt32Int32StringStringBoolean = (a, b, c, xmlProfile, e, f) =>
			{
				result = xmlProfile;
			};
			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFileObject);

			// Assert
			result.ShouldSatisfyAllConditions(
				() => result.ShouldNotBeNullOrWhiteSpace(),
				() => result.ShouldContain(_column2),
				() => result.ShouldContain(cell2));
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
			ShimATHBimportDatafromFile.AllInstances.NotifyStringString = (x, y, z) =>
			{
				progressShown = true;
			};
			ShimATHBimportDatafromFile.AllInstances.initNotifyString = (x, y) =>
			{
				notificationShown = true;
			};
			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFileObject);

			// Assert
			progressShown.ShouldSatisfyAllConditions(
				() => notificationShown.ShouldBeTrue(),
				() => progressShown.ShouldBeTrue());
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
			ShimATHBimportDatafromFile.AllInstances.UpdateToDBInt32Int32StringStringBoolean = (a, b, c, xmlProfile, e, f) =>
			{
				dataSaved = true;
			};
			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFileObject);

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
			ShimEmailGroup.ImportEmailsWithDupesUserInt32StringStringStringStringBooleanStringBooleanString = (a, b, c, d, e, f, g, h, i, j) => EmailDupesTable();
			var methodArgs = new object[] { this, EventArgs.Empty };

			// Act
			CallMethod(
				_importDataFromFileType,
				"ImportData",
				methodArgs,
				_importDataFromFileObject);

			// Assert
			var duplicates = _gvImport?.Rows[3]?.Cells[3]?.Text;
			duplicates.ShouldSatisfyAllConditions(
				() => _gvImport.Visible.ShouldBeTrue(),
				() => duplicates.ShouldBe("4"));
        }

        [Test]
        public void GvImport_Command_Success()
        {
            //Arrage
            var eventArgs = new GridViewCommandEventArgs(null, new CommandEventArgs(string.Empty, "U"));
            var testObject = new ATHBimportDatafromFile();
            _importDataFromFileObject = testObject;
            InitializeAllControls(testObject);
            CreateShims();
            var privateObject = new PrivateObject(testObject);
            ShimDirectory.ExistsString = p => false;
            ShimDirectory.CreateDirectoryString = p => null;
            ShimFile.ExistsString = p => true;
            ShimFile.DeleteString = p => { };
            ShimFile.AppendTextString = p => new ShimStreamWriter();
            var stateBag = new StateBag ();
            stateBag["SupressionGroups_DataTable"] = new DataTable { Columns = { "1" }, Rows = { { "1" } } }; ;
            ShimEmailGroup.ExportFromImportEmailsUserStringString = (p1, p2, p3) => new DataTable { Columns = { "1" }, Rows = { { "1" } } };
            ShimControl.AllInstances.ViewStateGet = (p) => stateBag;
            var responseHeader = string.Empty;
            ShimHttpResponse.AllInstances.ContentTypeSetString = (p1, p2) => { };
            ShimHttpResponse.AllInstances.AddHeaderStringString = (p1, p2, p3) => responseHeader += p2 + " " + p3;
            ShimHttpResponse.AllInstances.WriteFileString = (p1, p2) => { };
            ShimHttpResponse.AllInstances.End = (p) => { };
            ShimPage.AllInstances.ResponseGet = (p) => new HttpResponse(TextWriter.Null);

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
			ShimHttpRequest.AllInstances.QueryStringGet = (x) =>
			{
				return new NameValueCollection()
				{
					{QueryStringGID, QueryStringGIDValue }
				};
			};
			ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString = (a, b, c, d, e, f, g, h, i, j) =>
			{
				var dtable = new DataTable();
				return dtable;
			};
			ShimImportMapper.AllInstances.HasEmailAddressGet = (x) => { return true; };
			ShimATHBimportDatafromFile.AllInstances.MasterGet = (x) => new ECNMasterPage.Communicator();
			_dataCollectionTable = new HtmlTable();
			SetField(_importDataFromFileObject, "dataCollectionTable", _dataCollectionTable);
			_columnManager = CreateInstance(typeof(EmailTableColumnManager));
			ShimATHBimportDatafromFile.AllInstances.ColumnManagerGet = (x) => _columnManager;
			ShimATHBimportDatafromFile.AllInstances.getPhysicalPath = (x) => "dummyPhyiscalPath";
			FunctionFakes.ShimFileImporter.GetDataTableByFileTypeStringStringStringStringInt32Int32String = (a, b, c, d, e, f, g) => _dataFile;
			SetField(_importDataFromFileObject, "lineStart", LineStart);
			ShimEmailGroup.ImportEmailsWithDupesUserInt32StringStringStringStringBooleanStringBooleanString = (a, b, c, d, e, f, g, h, i, j) => EmailDupesTable();
			ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (x, y, z) =>
			{
				var groupDataFields = CreateInstance(typeof(CommunicatorGroupDataFields));
				SetProperty(groupDataFields, "ShortName", string.Empty);
				var groupDataFieldsList = new List<CommunicatorGroupDataFields>
				{
					groupDataFields
				};
				return groupDataFieldsList;
			};
		}

		private void Initialize()
		{
			_importDataFromFileType = typeof(ATHBimportDatafromFile);
			_importDataFromFileObject = CreateInstance(_importDataFromFileType);
			CreateShims();
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
			SetField(_importDataFromFileObject, "errlabel", _errlabel);
			SetField(_importDataFromFileObject, "ImportButton", _importButton);
			SetField(_importDataFromFileObject, "gid", _gid);
			SetField(_importDataFromFileObject, "lblGUID", _lblGUID);
			SetField(_importDataFromFileObject, "gvImport", _gvImport);
		}

		private DataTable EmailDupesTable()
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
		}

		private DataTable CreateDataTable(string colName1, string colName2, string colName3)
		{
			return new DataTable { Columns = { colName1, colName2, colName3, } };
		}

		private void SetField(dynamic obj, string fieldName, dynamic fieldValue)
		{
			ReflectionHelper.SetField(obj, fieldName, fieldValue);
		}

		private void SetProperty(dynamic obj, string fieldName, dynamic fieldValue)
		{
			ReflectionHelper.SetProperty(obj, fieldName, fieldValue);
		}

		private void SetSessionVariable(string name, object value)
		{
			HttpContext.Current.Session.Add(name, value);
		}
	}
}

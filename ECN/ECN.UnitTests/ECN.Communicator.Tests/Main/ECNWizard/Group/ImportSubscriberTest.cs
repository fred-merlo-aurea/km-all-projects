using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.QualityTools.Testing.Fakes;
using ECN_Framework_Common.Functions.Fakes;
using ecn.common.classes.Fakes;
using ecn.communicator.main.ECNWizard.Group;
using ecn.communicator.main.ECNWizard.Group.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects.Fakes;
using ECN_Framework_Entities.Application;
using ECN.TestHelpers;
using BasicShims = ECN.Communicator.Tests.Helpers.BasicShims;
using ECNBusiness = ECN_Framework_BusinessLayer.Application;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Group
{
	[ExcludeFromCodeCoverage]
	[TestFixture]
	public class ImportSubscriberTest
	{
		private const string _reqParam = "dummyClientId$ColumnHeaderSelect";
		private IDisposable _context;
		private Type _newGroupImportType;
		private newGroup_Import _newGroupImportObject;
		private object[] _methodArgs;
		private Button _btnImport;
		private DataTable _dataFile;
		private Label _lblfilename;
		private TextBox _txtSheetName;
		private TextBox _txtGroupName1;
		private RadioButton _rbImporttoExisting;
		private RadioButton _rbImporttoNew;
		private DropDownList _drpGroup2;
		private DropDownList _drpFolder3;
		private PlaceHolder _plImportCompleted;
		private PlaceHolder _plImportGroup;
		private string _colName1;
		private string _colName2;
		private string _colName3;
		private string _errorMessage;
		private HtmlTable _dataCollectionTable;
		private GridView _gvImport;
		private DataTable _uploadFileData;
		private Label lblGUID;
		private PlaceHolder phError;
		private Label lblErrorMessage;
		private PlaceHolder plUpload;

		[SetUp]
		public void Setup()
		{
			_context = ShimsContext.Create();
		}

		[TearDown]
		public void TearDwond()
		{
			_context.Dispose();
		}

		[Test]
		public void btnImport_Click_WhenErrorReadingFile_EmailImportFormIsBuilt()
		{
			// Arrange
			Initialize();
			_dataCollectionTable = null;
			var emailImportFormIsBuilt = false;
			ShimFileImporter.GetDataTableByFileTypeStringStringStringStringInt32Int32String = (a, b, c, d, e, maxRecordToRetreive, f) =>
			{
				if (maxRecordToRetreive > 1)
				{
					emailImportFormIsBuilt = true;
					return _dataFile;
				}
				else
				{
					throw new Exception();
				}
			};

			// Act
			CallMethod(
				_newGroupImportType,
				"btnImport_Click",
				_methodArgs,
				_newGroupImportObject);

			// Assert
			_dataCollectionTable = GetField(_newGroupImportObject, "dataCollectionTable");
			_dataCollectionTable.ShouldSatisfyAllConditions(
				() => _dataCollectionTable.ShouldNotBeNull(),
				() => emailImportFormIsBuilt.ShouldBeTrue());
		}

		[Test]
		public void btnImport_Click_WhenNoDataIsProvide_ErrorMessageIsDisplayed()
		{
			// Arrange
			Initialize();
			SetProperty(_newGroupImportObject, "ErrorMessage", string.Empty);
			_dataFile = new DataTable();
			ShimFileImporter.GetDataTableByFileTypeStringStringStringStringInt32Int32String = (a, b, c, d, e, f, g) => _dataFile;

			// Act
			CallMethod(
				_newGroupImportType,
				"btnImport_Click",
				_methodArgs,
				_newGroupImportObject);

			// Assert
			var errMsg = "ERROR - Email Address or Phone Number is required to import data.";
			_errorMessage.ShouldSatisfyAllConditions(
				() => _errorMessage.ShouldNotBeNullOrWhiteSpace(),
				() => _errorMessage.ShouldContain(errMsg));
		}

		[Test]
		public void btnImport_Click_WhenFileContainsDuplicateColumns_ErrorMessageIsDisplayed()
		{
			// Arrange
			Initialize();
			SetProperty(_newGroupImportObject, "ErrorMessage", string.Empty);
			var duplicatedColumns = "sameColumnName";
			ShimHttpRequest.AllInstances.ParamsGet = (x) =>
			{
				return new NameValueCollection()
				{
					{ _reqParam + "0", duplicatedColumns },
					{ _reqParam + "1", duplicatedColumns },
					{ _reqParam + "2", "ignore" },
				};
			};
			ShimFileImporter.GetDataTableByFileTypeStringStringStringStringInt32Int32String = (a, b, c, d, e, f, g) => _dataFile;
			ShimnewGroup_Import.AllInstances.throwECNExceptionString = (x, errorString) =>
			{
				_errorMessage = errorString;
			};
			// Act
			CallMethod(
				_newGroupImportType,
				"btnImport_Click",
				_methodArgs,
				_newGroupImportObject);

			// Assert
			var errMsg = "ERROR - You have selected duplicate field names.<BR><BR>";
			_errorMessage.ShouldSatisfyAllConditions(
				() => _errorMessage.ShouldNotBeNullOrWhiteSpace(),
				() => _errorMessage.ShouldContain(errMsg));
		}

		[Test]
		public void btnImport_Click_WhenPhoneNumberColumnNotFoundInFile_ErrorMessageIsDisplayed()
		{
			// Arrange
			Initialize();
			ShimFileImporter.GetDataTableByFileTypeStringStringStringStringInt32Int32String = (a, b, c, d, e, f, g) => _dataFile;
			ShimImportMapper.AllInstances.HasEmailAddressGet = (x) => false;
			ShimImportMapper.AllInstances.HasMobileNumberGet = (x) => false;

			// Act
			CallMethod(
				_newGroupImportType,
				"btnImport_Click",
				_methodArgs,
				_newGroupImportObject);

			// Assert
			var errMsg = "ERROR - Email Address or Phone Number is required to import data.";
			_errorMessage.ShouldSatisfyAllConditions(
				() => _errorMessage.ShouldNotBeNullOrWhiteSpace(),
				() => _errorMessage.ShouldContain(errMsg));
		}

		[Test]
		public void btnImport_Click_WhenImportToNewGroupIsSelectedAndFileContainsValidData_DataIsSavedToDB()
		{
			// Arrange
			ShimnewGroup_Import.AllInstances.GroupIDSetInt32 = (x, y) => { };
			Initialize();
			var sampleData = "sample string data";
			_dataFile = CreateDataTableWithData("col1", "col2", "col3", sampleData, sampleData, sampleData);
			var updatedData = string.Empty;
			var dataUpdated = false;
			_rbImporttoNew.Checked = true;
			_txtGroupName1.Text = "dummyGroupName";
			CreateDataShims();
			ShimnewGroup_Import.AllInstances.UpdateToDBStringStringBooleanBoolean = (a, dataToBeUpdated, b, c, d) =>
			{
				updatedData = dataToBeUpdated;
				dataUpdated = true;
			};

			// Act
			CallMethod(
				_newGroupImportType,
				"btnImport_Click",
				_methodArgs,
				_newGroupImportObject);

			// Assert
			updatedData.ShouldSatisfyAllConditions(
				() => dataUpdated.ShouldBeTrue(),
				() => updatedData.ShouldNotBeNullOrWhiteSpace(),
				() => updatedData.ShouldContain(sampleData));
		}

		[Test]
		public void btnImport_Click_WhenCertainColumnIsIgnored_IgnoredColumnDataAndUserDefinedDataIsNotSavedToDb()
		{
			// Arrange
			ShimnewGroup_Import.AllInstances.GroupIDSetInt32 = (x, y) => { };
			Initialize();
			var firstColumn = "emailaddress";
			var secondColumn = "user_";
			var thirdColumn = "ignore";
			var sampleData = "sample string data";
			var ignoredSampleData = "ignored sample string data";
			var userDefinedSampleData = "ignored sample string data";
			_dataFile = CreateDataTableWithData(firstColumn, secondColumn, thirdColumn, sampleData, userDefinedSampleData, ignoredSampleData);
			var updatedData = string.Empty;
			_rbImporttoNew.Checked = true;
			_txtGroupName1.Text = "dummyGroupName";
			CreateDataShims();
			ShimHttpRequest.AllInstances.ParamsGet = (x) =>
			{
				return new NameValueCollection()
				{
					{ _reqParam + "0", firstColumn },
					{ _reqParam + "1", secondColumn },
					{ _reqParam + "2", thirdColumn },
				};
			};
			ShimnewGroup_Import.AllInstances.UpdateToDBStringStringBooleanBoolean = (a, dataToBeUpdated, b, c, d) =>
			{
				updatedData = dataToBeUpdated;
			};

			// Act
			CallMethod(
				_newGroupImportType,
				"btnImport_Click",
				_methodArgs,
				_newGroupImportObject);

			// Assert
			updatedData.ShouldSatisfyAllConditions(
				() => updatedData.ShouldNotBeNullOrWhiteSpace(),
				() => updatedData.ShouldContain(sampleData),
				() => updatedData.ShouldNotContain(ignoredSampleData),
				() => updatedData.ShouldNotContain(userDefinedSampleData));
		}

		[Test]
		public void btnImport_Click_WhenEmailAddressColumnNotFound_MobileColumnsDataIsSavedToEmailColumn()
		{
			// Arrange
			ShimnewGroup_Import.AllInstances.GroupIDSetInt32 = (x, y) => { };
			Initialize();
			var mobileNumberAsEmail = false;
			var firstColumn = "col1";
			var secondColumn = "col2";
			var thirdColumn = "mobile";
			var sampleData = "sample string data";
			var sampleMobileNumber = "0123456789";
			_dataFile = CreateDataTableWithData(firstColumn, secondColumn, thirdColumn, sampleData, sampleData, sampleMobileNumber);
			var updatedData = string.Empty;
			_rbImporttoNew.Checked = true;
			_txtGroupName1.Text = "dummyGroupName";
			CreateDataShims();
			ShimImportMapper.AllInstances.HasEmailAddressGet = (x) => false;
			ShimHttpRequest.AllInstances.ParamsGet = (x) =>
			{
				return new NameValueCollection()
				{
					{ _reqParam + "0", firstColumn },
					{ _reqParam + "1", secondColumn },
					{ _reqParam + "2", thirdColumn },
				};
			};
			ShimnewGroup_Import.AllInstances.UpdateToDBStringStringBooleanBoolean = (a, dataToBeSaved, b, c, mobileNumbersOnly) =>
			{
				if (mobileNumbersOnly)
				{
					mobileNumberAsEmail = mobileNumbersOnly;
				}
				updatedData += dataToBeSaved;
			};

			// Act
			CallMethod(
				_newGroupImportType,
				"btnImport_Click",
				_methodArgs,
				_newGroupImportObject);

			// Assert
			updatedData.ShouldSatisfyAllConditions(
				() => mobileNumberAsEmail.ShouldBeTrue(),
				() => updatedData.ShouldNotBeNullOrWhiteSpace(),
				() => updatedData.ShouldContain(sampleData),
				() => updatedData.ShouldContain(sampleMobileNumber));
		}

		[Test]
		public void btnImport_Click_WhenGroupedFieldsFound_GroupedFieldsAreSavedInUsersGroup()
		{
			// Arrange
			ShimnewGroup_Import.AllInstances.GroupIDSetInt32 = (x, y) => { };
			Initialize();
			var updatedData = string.Empty;
			_rbImporttoNew.Checked = true;
			var firstColumn = "col1";
			var secondColumn = "emailaddress";
			var thirdColumn = "user_";
			var sampleGroupedField = "sample string data";
			_dataFile = CreateDataTableWithData(firstColumn, secondColumn, thirdColumn, sampleGroupedField, sampleGroupedField, sampleGroupedField);
			CreateDataShims();
			ShimnewGroup_Import.AllInstances.UpdateToDBStringStringBooleanBoolean = (a, dataToBeUpdated, b, c, d) =>
			{
				updatedData = dataToBeUpdated;
			};
			ShimHttpRequest.AllInstances.ParamsGet = (x) =>
			{
				return new NameValueCollection()
				{
					{ _reqParam + "0", firstColumn },
					{ _reqParam + "1", secondColumn },
					{ _reqParam + "2", thirdColumn },
				};
			};
			ShimDataFunctions.GetDataTableString = (x) => CreateDummyDataTable();

			// Act
			CallMethod(
				_newGroupImportType,
				"btnImport_Click",
				_methodArgs,
				_newGroupImportObject);

			// Assert
			updatedData.ShouldSatisfyAllConditions(
				() => updatedData.ShouldNotBeNullOrWhiteSpace(),
				() => updatedData.ShouldContain(sampleGroupedField));
		}

		[TestCase("Duplicate(s)", "D", "23")]
		[TestCase("Total Records in the File", "T", "2001")]
		[TestCase("New", "I", "12")]
		[TestCase("Changed", "U", "23")]
		[TestCase("Skipped", "S", "213")]
		[TestCase("Skipped (Emails in Master Suppression)", "M", "2")]
		public void btnImport_Click_WhenRecordsAreUpdate_SummaryIsShownInDataGrid(string actionType, string actionTypeShort, string actionValue)
		{
			// Arrange
			ShimnewGroup_Import.AllInstances.GroupIDSetInt32 = (x, y) => { };
			Initialize();
			_gvImport.DataSource = null;
			var updatedData = string.Empty;
			_rbImporttoNew.Checked = true;
			var firstColumn = "col1";
			var secondColumn = "emailaddress";
			var thirdColumn = "user_";
			var sampleGroupedField = "sample string data";
			_dataFile = CreateDataTableWithData(firstColumn, secondColumn, thirdColumn, sampleGroupedField, sampleGroupedField, sampleGroupedField);
			CreateDataShims();
			ShimDataFunctions.GetDataTableString = (x) => CreateDummyDataTable();
			ShimHttpRequest.AllInstances.ParamsGet = (x) =>
			{
				return new NameValueCollection()
				{
					{ _reqParam + "0", firstColumn },
					{ _reqParam + "1", secondColumn },
					{ _reqParam + "2", thirdColumn },
				};
			};
			ShimDataFunctions.GetDataTableSqlCommand = (x) =>
			{
				var table = CreateDummyDataTable();
				table.Rows[0]["Action"] = actionTypeShort;
				table.Rows[0]["Counts"] = actionValue;
				return table;
			};
			ShimnewGroup_Import.AllInstances.UpdateToDBStringStringBooleanBoolean = (thisObject, dataToBeUpdated, b, c, d) =>
			{
				updatedData = dataToBeUpdated;
				var table = CreateHashtable();
				Hashtable hashtable = new Hashtable();
				hashtable.Add(actionTypeShort, actionValue);
				SetField(thisObject, "hUpdatedRecords", hashtable);
			};

			// Act
			CallMethod(
				_newGroupImportType,
				"btnImport_Click",
				_methodArgs,
				_newGroupImportObject);

			// Assert
			var actionName = _gvImport.Rows[0].Cells[0].Text;
			var actionSummary = _gvImport.Rows[0].Cells[2].Text;
			_gvImport.ShouldSatisfyAllConditions(
				() => _gvImport.DataSource.ShouldNotBeNull(),
				() => actionName.ShouldContain(actionType),
				() => actionSummary.ShouldContain(actionValue));
		}

		[Test]
		public void btnImport_Click_WhenMoreThan100Records_WaitNotificationIsShown()
		{
			// Arrange
			ShimnewGroup_Import.AllInstances.GroupIDSetInt32 = (x, y) => { };
			Initialize();
			var maxRecords = 110;
			var progressIsFlashed = false;
			var updatedData = string.Empty;
			_rbImporttoNew.Checked = true;
			var firstColumn = "col1";
			var secondColumn = "emailaddress";
			var thirdColumn = "user_";
			var sampleData = "sample string data";
			_dataFile = CreateDataTableWithData(firstColumn, secondColumn, thirdColumn, sampleData, sampleData, sampleData);
			for (var i = 0; i < maxRecords; i++)
			{
				_dataFile.Rows.Add(sampleData, sampleData, sampleData);
			}
			CreateDataShims();
			ShimHttpRequest.AllInstances.ParamsGet = (x) =>
			{
				return new NameValueCollection()
				{
					{ _reqParam + "0", firstColumn },
					{ _reqParam + "1", secondColumn },
					{ _reqParam + "2", thirdColumn },
				};
			};
			ShimDataFunctions.GetDataTableString = (x) => CreateDummyDataTable();
			ShimnewGroup_Import.AllInstances.UpdateToDBStringStringBooleanBoolean = (a, dataToBeSaved, b, c, mobileNumbersOnly) =>
			{
				progressIsFlashed = true;
			};

			// Act
			CallMethod(
				_newGroupImportType,
				"btnImport_Click",
				_methodArgs,
				_newGroupImportObject);

			// Assert
			progressIsFlashed.ShouldBeTrue();
		}

		private void Initialize()
		{
			Helpers.BasicShims.CreateShims();
			CreateShims();
			lblGUID = new Label();
			lblErrorMessage = new Label();
			phError = new PlaceHolder();
			plUpload = new PlaceHolder();
			_newGroupImportType = typeof(newGroup_Import);
			_newGroupImportObject = CreateInstance(_newGroupImportType);
			_methodArgs = new object[] { null, EventArgs.Empty };
			_btnImport = new Button();
			_lblfilename = new Label();
			_txtSheetName = new TextBox();
			_txtGroupName1 = new TextBox();
			_rbImporttoExisting = new RadioButton();
			_rbImporttoNew = new RadioButton();
			_drpGroup2 = new DropDownList();
			_drpFolder3 = new DropDownList();
			_plImportCompleted = new PlaceHolder();
			_plImportGroup = new PlaceHolder();
			_dataCollectionTable = new HtmlTable();
			_gvImport = new GridView();
			_colName1 = "sample column 1";
			_colName2 = "user_";
			_colName3 = "emailaddress";
			_errorMessage = string.Empty;
			_dataFile = CreateDataTable(_colName1, _colName2, _colName3);
			_uploadFileData = new DataTable();
			SetDefaults();
		}

		private void SetDefaults()
		{
			_btnImport.Visible = false;
			_lblfilename.Text = "sampleFileName.txt";
			_txtSheetName.Text = "sampleFileName.xls";
			SetField(_newGroupImportObject, "btnImport", _btnImport);
			SetField(_newGroupImportObject, "lblfilename", _lblfilename);
			SetField(_newGroupImportObject, "txtSheetName", _txtSheetName);
			SetField(_newGroupImportObject, "txtGroupName1", _txtGroupName1);
			SetField(_newGroupImportObject, "drpGroup2", _drpGroup2);
			SetField(_newGroupImportObject, "drpFolder3", _drpFolder3);
			SetField(_newGroupImportObject, "rbImporttoExisting", _rbImporttoExisting);
			SetField(_newGroupImportObject, "rbImporttoNew", _rbImporttoNew);
			SetField(_newGroupImportObject, "dataCollectionTable", _dataCollectionTable);
			SetField(_newGroupImportObject, "plImportCompleted", _plImportCompleted);
			SetField(_newGroupImportObject, "plImportGroup", _plImportGroup);
			SetField(_newGroupImportObject, "gvImport", _gvImport);
			SetProperty(_newGroupImportObject, "ErrorMessage", _errorMessage);
			_drpGroup2.Items.Insert(0, new ListItem()
			{
				Text = "Group ID",
				Selected = true,
				Value = "1"
			});
			_drpFolder3.Items.Insert(0, new ListItem()
			{
				Text = "Folder ID",
				Selected = true,
				Value = "1"
			});
			SetField(_newGroupImportObject, "lblGUID", lblGUID);
			SetField(_newGroupImportObject, "phError", phError);
			SetField(_newGroupImportObject, "lblErrorMessage", lblErrorMessage);
			SetField(_newGroupImportObject, "plUpload", plUpload);
		}

		private void CreateShims()
		{
			BasicShims.CreateShims();
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
			HttpContext.Current = MockHelpers.FakeHttpContext();
			ShimPage.AllInstances.SessionGet = x => { return HttpContext.Current.Session; };
			ShimPage.AllInstances.RequestGet = (x) => { return HttpContext.Current.Request; };
			ShimPage.AllInstances.MasterGet = (x) => { return new MasterPage() { }; };
			ShimHttpRequest.AllInstances.ParamsGet = (x) =>
			{
				return new NameValueCollection()
				{
					{ _reqParam + "0", _colName1 },
					{ _reqParam + "1", _colName2 },
					{ _reqParam + "2", _colName3 },
				};
			};
			ShimControl.AllInstances.ParentGet = (control) =>
			{
				return control == _newGroupImportObject ? new Page() : null;
			};
			ShimImportMapper.AllInstances.HasEmailAddressGet = (x) => true;
			ShimImportMapper.AllInstances.HasMobileNumberGet = (x) => true;
			ShimDataFunctions.GetDataTableString = (x) => new DataTable();
			ShimDataFunctions.GetDataTableSqlCommand = (x) => new DataTable();
			ShimControl.AllInstances.ClientIDGet = (x) => "dummyClientId";
			ShimnewGroup_Import.AllInstances.UpdateToDBStringStringBooleanBoolean = (a, dataToBeUpdated, b, c, d) => { };
			ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (a, b, c) => true;
			ShimnewGroup_Import.AllInstances.buildColumnHeaderDropdownsString = (x, y) => new HtmlSelect();
			ShimnewGroup_Import.AllInstances.throwECNExceptionString = (x, errorString) =>
			{
				_errorMessage = errorString;
			};
		}

		private void CallMethod(Type type, string methodName, object[] parametersValues, object instance = null)
		{
			ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
		}

		private dynamic CreateInstance(Type type)
		{
			return ReflectionHelper.CreateInstance(type);
		}

		private void SetField(dynamic obj, string fieldName, dynamic value)
		{
			ReflectionHelper.SetField(obj, fieldName, value);
		}

		private dynamic GetField(dynamic obj, string fieldName)
		{
			return ReflectionHelper.GetFieldValue(obj, fieldName);
		}

		private void SetProperty(dynamic obj, string fieldName, dynamic value)
		{
			ReflectionHelper.SetProperty(obj, fieldName, value);
		}

		private dynamic GetProperty(dynamic obj, string fieldName)
		{
			return ReflectionHelper.GetPropertyValue(obj, fieldName);
		}

		private DataTable CreateDataTable(string colName1, string colName2, string colName3)
		{
			var sampleTable = new DataTable
			{
				Columns =
				{
					colName1,
					colName2,
					colName3
				}
			};
			var sampleRow = sampleTable.NewRow();
			sampleRow[colName1] = "sample text";
			sampleRow[colName2] = "sample text";
			sampleRow[colName3] = "sample text";
			sampleTable.Rows.Add(sampleRow);
			return sampleTable;
		}

		private DataTable CreateDataTableWithData(string col1Name, string col2Name, string col3Name, string row1SampleData, string row2SampleData, string row3SampleData)
		{
			var sampleTable = new DataTable
			{
				Columns =
				{
					col1Name,
					col2Name,
					col3Name,
				}
			};
			var sampleRow = sampleTable.NewRow();
			sampleRow[col1Name] = row1SampleData;
			sampleRow[col2Name] = row2SampleData;
			sampleRow[col3Name] = row3SampleData;
			sampleTable.Rows.Add(sampleRow);
			return sampleTable;
		}

		private void CreateDataShims()
		{
			ShimDataFunctions.ExecuteScalarSqlCommand = (x) => "1";
			ShimDataFunctions.ExecuteString = (x) => 0;
			ShimFileImporter.GetDataTableByFileTypeStringStringStringStringInt32Int32String = (a, b, c, d, e, f, g) => _dataFile;
			ShimDataFunctions.ExecuteScalarString = (sqlQuery) =>
			{
				return sqlQuery.Contains("INSERT INTO") ? "1" : "0";
			};
			ShimnewGroup_Import.AllInstances.GetGroupDataFields = (x) => CreateHashtable();
		}

		private DataTable CreateDummyDataTable()
		{
			DataTable table = new DataTable
			{
				Columns = {
					"GroupDataFieldsID",
					"GroupID",
					"ShortName",
					"LongName",
					"SurveyID",
					"IsPublic",
					"DatafieldSetID",
					"Action",
					"Counts"
				},
			};
			table.Rows.Add("1", "1", "", "sampleString", "1", "1", "1", "T", "333");
			return table;
		}

		private Hashtable CreateHashtable()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("GroupDataFieldsID", "1");
			hashtable.Add("GroupID", "1");
			hashtable.Add("ShortName", "");
			hashtable.Add("LongName", "sampleString");
			hashtable.Add("SurveyID", "1");
			hashtable.Add("IsPublic", "1");
			hashtable.Add("DatafieldSetID", "1");
			hashtable.Add("Action", "T");
			hashtable.Add("Counts", "333");
			hashtable.Add("user_", "1");
			return hashtable;
		}
	}
}


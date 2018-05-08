using System;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ecn.communicator.main.SMSWizard.Controls.Fakes;
using ecn.communicator.classes.ImportData.Fakes;
using ecn.common.classes.Fakes;
using ecn.communicator.main.SMSWizard.Controls;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Application;
using ECN_Framework_Common.Objects.Fakes;
using ECN.TestHelpers;
using ECNBusiness = ECN_Framework_BusinessLayer.Application;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.SMSWizard.Controls
{
	[ExcludeFromCodeCoverage]
	[TestFixture]
	public class GroupInfoTest
	{
		private const string _reqParam = "$ContentPlaceHolder1$ECNWizard$ColumnHeaderSelect";
		private IDisposable _context;
		private Type _groupInfoType;
		private GroupInfo _groupInfoObject;
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
		private DataGrid _dgImport;
		private DataTable _uploadFileData;

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

		[TestCase("Invalid File")]
		[TestCase("Error Reading File")]
		public void btnImport_Click_WhenInvalidFileOrErrorOnReadingFile_ErrorMessageIsDisplayed(string error)
		{
			// Arrange
			Initialize();
			SetProperty(_groupInfoObject, "ErrorMessage", string.Empty);
			if (error == "Invalid File")
			{
				ShimFileImporter.GetDataTableByFileTypeStringStringStringStringInt32Int32String = (a, b, c, d, e, f, g) => throw new ArgumentException();
			}
			else
			{
				ShimFileImporter.GetDataTableByFileTypeStringStringStringStringInt32Int32String = (a, b, c, d, e, f, g) => throw new Exception();
			}
			
			// Act
			CallMethod(
				_groupInfoType,
				"btnImport_Click",
				_methodArgs,
				_groupInfoObject);

			// Assert
			var errMsg = "ERROR - Phone Number is required to import data.";
			_errorMessage = GetProperty(_groupInfoObject, "ErrorMessage");
			_errorMessage.ShouldSatisfyAllConditions(
				() => _errorMessage.ShouldNotBeNullOrWhiteSpace(),
				() => _errorMessage.ShouldBe(errMsg));
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
				_groupInfoType,
				"btnImport_Click",
				_methodArgs,
				_groupInfoObject);

			// Assert
			_dataCollectionTable = GetField(_groupInfoObject, "dataCollectionTable");
			_dataCollectionTable.ShouldSatisfyAllConditions(
				() => _dataCollectionTable.ShouldNotBeNull(),
				() => emailImportFormIsBuilt.ShouldBeTrue());
		}

		[Test]
		public void btnImport_Click_WhenNoDataIsProvide_ErrorMessageIsDisplayed()
		{
			// Arrange
			Initialize();
			SetProperty(_groupInfoObject, "ErrorMessage", string.Empty);
			_dataFile = new DataTable();
			ShimFileImporter.GetDataTableByFileTypeStringStringStringStringInt32Int32String = (a, b, c, d, e, f, g) => _dataFile;

			// Act
			CallMethod(
				_groupInfoType,
				"btnImport_Click",
				_methodArgs,
				_groupInfoObject);

			// Assert
			var errMsg = "ERROR - Phone Number is required to import data.";
			_errorMessage = GetProperty(_groupInfoObject, "ErrorMessage");
			_errorMessage.ShouldSatisfyAllConditions(
				() => _errorMessage.ShouldNotBeNullOrWhiteSpace(),
				() => _errorMessage.ShouldContain(errMsg));
		}

		[Test]
		public void btnImport_Click_WhenFileContainsDuplicateColumns_ErrorMessageIsDisplayed()
		{
			// Arrange
			Initialize();
			SetProperty(_groupInfoObject, "ErrorMessage", string.Empty);
			var duplicatedColumns = "sameColumnName";
			ShimHttpRequest.AllInstances.ParamsGet = (x) =>
			{
				return new NameValueCollection()
				{
					{ _reqParam+"0", duplicatedColumns },
					{ _reqParam+"1", duplicatedColumns },
					{ _reqParam+"2", "ignore" },
				};
			};
			ShimFileImporter.GetDataTableByFileTypeStringStringStringStringInt32Int32String = (a, b, c, d, e, f, g) => _dataFile;

			// Act
			CallMethod(
				_groupInfoType,
				"btnImport_Click",
				_methodArgs,
				_groupInfoObject);

			// Assert
			var errMsg = "ERROR - You have selected duplicate field names.<BR><BR>";
			_errorMessage = GetProperty(_groupInfoObject, "ErrorMessage");
			_errorMessage.ShouldSatisfyAllConditions(
				() => _errorMessage.ShouldNotBeNullOrWhiteSpace(),
				() => _errorMessage.ShouldContain(errMsg));
		}

		[Test]
		public void btnImport_Click_WhenPhoneNumberColumnNotFoundInFile_ErrorMessageIsDisplayed()
		{
			// Arrange
			Initialize();
			SetProperty(_groupInfoObject, "ErrorMessage", string.Empty);
			ShimFileImporter.GetDataTableByFileTypeStringStringStringStringInt32Int32String = (a, b, c, d, e, f, g) => _dataFile;
			ShimImportMapper.AllInstances.HasEmailAddressGet = (x) => false;
			ShimImportMapper.AllInstances.HasMobileNumberGet = (x) => false;
			// Act
			CallMethod(
				_groupInfoType,
				"btnImport_Click",
				_methodArgs,
				_groupInfoObject);

			// Assert
			var errMsg = "ERROR - Phone Number is required to import data.";
			_errorMessage = GetProperty(_groupInfoObject, "ErrorMessage");
			_errorMessage.ShouldSatisfyAllConditions(
				() => _errorMessage.ShouldNotBeNullOrWhiteSpace(),
				() => _errorMessage.ShouldContain(errMsg));
		}

		[Test]
		public void btnImport_Click_WhenNoGroupIsSelected_ErrorMessageIsDisplayed()
		{
			// Arrange
			Initialize();
			SetProperty(_groupInfoObject, "ErrorMessage", string.Empty);
			ShimFileImporter.GetDataTableByFileTypeStringStringStringStringInt32Int32String = (a, b, c, d, e, f, g) => _dataFile;
			// Act
			CallMethod(
				_groupInfoType,
				"btnImport_Click",
				_methodArgs,
				_groupInfoObject);

			// Assert
			var errMsg = "Select existing group or new group";
			_errorMessage = GetProperty(_groupInfoObject, "ErrorMessage");
			_errorMessage.ShouldSatisfyAllConditions(
				() => _errorMessage.ShouldNotBeNullOrWhiteSpace(),
				() => _errorMessage.ShouldContain(errMsg));
		}

		[Test]
		public void btnImport_Click_WhenImportToNewGroupIsSelectedAndNewGroupNameDoesNotExist_NewGroupIsCreatedAndSaved()
		{
			// Arrange
			ShimGroupInfo.AllInstances.GroupIDSetInt32 = (x, y) => { };
			Initialize();
			_rbImporttoNew.Checked = true;
			_txtGroupName1.Text = "dummyGroupName";
			var newGroupId = 1;
			var groupCreatedAndSaved = false;
			ShimDataFunctions.ExecuteScalarString = (x) => "0";
			ShimDataFunctions.ExecuteScalarSqlCommand = (x) => newGroupId;
			ShimDataFunctions.ExecuteString = (x) =>
			{
				groupCreatedAndSaved = true;
				return 0;
			};
			ShimFileImporter.GetDataTableByFileTypeStringStringStringStringInt32Int32String = (a, b, c, d, e, f, g) => _dataFile;

			// Act
			CallMethod(
				_groupInfoType,
				"btnImport_Click",
				_methodArgs,
				_groupInfoObject);

			// Assert
			groupCreatedAndSaved.ShouldBeTrue();
		}

		[Test]
		public void btnImport_Click_WhenImportToNewGroupIsSelectedAndNewGroupAlreadyExist_ErrorIsDisplayed()
		{
			// Arrange
			ShimGroupInfo.AllInstances.GroupIDSetInt32 = (x, y) => { };
			Initialize();
			_rbImporttoNew.Checked = true;
			_txtGroupName1.Text = "dummyGroupName";
			var groupAlreadyExists = false;
			SetProperty(_groupInfoObject, "ErrorMessage", string.Empty);
			ShimDataFunctions.ExecuteScalarString = (x) =>
			{
				groupAlreadyExists = true;
				return "1";
			};
			ShimFileImporter.GetDataTableByFileTypeStringStringStringStringInt32Int32String = (a, b, c, d, e, f, g) => _dataFile;

			// Act
			CallMethod(
				_groupInfoType,
				"btnImport_Click",
				_methodArgs,
				_groupInfoObject);

			// Assert
			var errMsg = "already exists. Please enter a different name";
			_errorMessage = GetProperty(_groupInfoObject, "ErrorMessage");
			_errorMessage.ShouldSatisfyAllConditions(
				() => _errorMessage.ShouldNotBeNullOrWhiteSpace(),
				() => _errorMessage.ShouldContain(errMsg),
				() =>groupAlreadyExists.ShouldBeTrue());
		}

		[Test]
		public void btnImport_Click_WhenImportToNewGroupIsSelectedAndFileContainsValidData_DataIsSavedToDB()
		{
			// Arrange
			ShimGroupInfo.AllInstances.GroupIDSetInt32 = (x, y) => { };
			Initialize();
			var sampleData = "sample string data";
			_dataFile = CreateDataTableWithData("col1", "col2", "col3", sampleData, sampleData, sampleData);
			var updatedData = string.Empty;
			var dataUpdated = false;
			_rbImporttoNew.Checked = true;
			_txtGroupName1.Text = "dummyGroupName";
			CreateDataShims();
			ShimGroupInfo.AllInstances.UpdateToDBInt32Int32StringStringBooleanBoolean = (a, b, groupId, dataToBeUpdated, d, e, f) =>
			{
				updatedData = dataToBeUpdated;
				dataUpdated = true;
			};

			// Act
			CallMethod(
				_groupInfoType,
				"btnImport_Click",
				_methodArgs,
				_groupInfoObject);

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
			ShimGroupInfo.AllInstances.GroupIDSetInt32 = (x, y) => { };
			Initialize();
			var firstColumn = "col1";
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
					{ _reqParam+"0", firstColumn },
					{ _reqParam+"1", secondColumn },
					{ _reqParam+"2", thirdColumn },
				};
			};
			ShimGroupInfo.AllInstances.UpdateToDBInt32Int32StringStringBooleanBoolean = (a, b, c, dataToBeUpdated, d, e, f) =>
			{
				updatedData += dataToBeUpdated;
			};

			// Act
			CallMethod(
				_groupInfoType,
				"btnImport_Click",
				_methodArgs,
				_groupInfoObject);

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
			ShimGroupInfo.AllInstances.GroupIDSetInt32 = (x, y) => { };
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
					{ _reqParam+"0", firstColumn },
					{ _reqParam+"1", secondColumn },
					{ _reqParam+"2", thirdColumn },
				};
			};
			ShimGroupInfo.AllInstances.UpdateToDBInt32Int32StringStringBooleanBoolean = (a, b, c, dataToBeUpdated, d, e, mobileNumbersOnly) =>
			{
				if (mobileNumbersOnly)
				{
					mobileNumberAsEmail = mobileNumbersOnly;
				}
				updatedData += dataToBeUpdated;
			};

			// Act
			CallMethod(
				_groupInfoType,
				"btnImport_Click",
				_methodArgs,
				_groupInfoObject);

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
			ShimGroupInfo.AllInstances.GroupIDSetInt32 = (x, y) => { };
			Initialize();
			var updatedData = string.Empty;
			_rbImporttoNew.Checked = true;
			var firstColumn = "col1";
			var secondColumn = "emailaddress";
			var thirdColumn = "user_";
			var sampleGroupedField = "sample string data";
			_dataFile = CreateDataTableWithData(firstColumn, secondColumn, thirdColumn, sampleGroupedField, sampleGroupedField, sampleGroupedField);
			CreateDataShims();
			ShimGroupInfo.AllInstances.UpdateToDBInt32Int32StringStringBooleanBoolean = (a, b, c, dataToBeUpdated, d, e, mobileNumbersOnly) =>
			{
				updatedData += dataToBeUpdated;
			};
			ShimHttpRequest.AllInstances.ParamsGet = (x) =>
			{
				return new NameValueCollection()
				{
					{ _reqParam+"0", firstColumn },
					{ _reqParam+"1", secondColumn },
					{ _reqParam+"2", thirdColumn },
				};
			};
			ShimDataFunctions.GetDataTableString = (x) => CreateDummyDataTable();

			// Act
			CallMethod(
				_groupInfoType,
				"btnImport_Click",
				_methodArgs,
				_groupInfoObject);

			// Assert
			updatedData.ShouldSatisfyAllConditions(
				() => updatedData.ShouldNotBeNullOrWhiteSpace(),
				() => updatedData.ShouldContain(sampleGroupedField));
		}

		[TestCase("Duplicate(s)","D","23")]
		[TestCase("Total Records in the File", "T","2001")]
		[TestCase("New", "I","12")]
		[TestCase("Changed", "U","23")]
		[TestCase("Skipped", "S","213")]
		[TestCase("Skipped (Emails in Master Suppression)", "M","2")]
		public void btnImport_Click_WhenRecordsAreUpdate_SummaryIsShownInDataGrid(string actionType, string actionTypeShort, string actionValue)
		{
			// Arrange
			ShimGroupInfo.AllInstances.GroupIDSetInt32 = (x, y) => { };
			Initialize();
			_dgImport.DataSource = null;
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
					{ _reqParam+"0", firstColumn },
					{ _reqParam+"1", secondColumn },
					{ _reqParam+"2", thirdColumn },
				};
			};
			ShimDataFunctions.GetDataTableSqlCommand = (x) =>
			{
				var table = CreateDummyDataTable();
				table.Rows[0]["Action"] = actionTypeShort;
				table.Rows[0]["Counts"] = actionValue;
				return table;
			};

			// Act
			CallMethod(
				_groupInfoType,
				"btnImport_Click",
				_methodArgs,
				_groupInfoObject);

			// Assert
			var actionName = _dgImport.Items[0].Cells[0].Text;
			var actionSummary = _dgImport.Items[0].Cells[1].Text;
			_dgImport.ShouldSatisfyAllConditions(
				() => _dgImport.DataSource.ShouldNotBeNull(),
				() => actionName.ShouldContain(actionType),
				() => actionSummary.ShouldContain(actionValue));
		}

		[Test]
		public void btnImport_Click_WhenMoreThan100Records_WaitNotificationIsShown()
		{
			// Arrange
			ShimGroupInfo.AllInstances.GroupIDSetInt32 = (x, y) => { };
			Initialize();
			var maxRecords = 110;
			var notificationShown = false;
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
			ShimGroupInfo.AllInstances.UpdateToDBInt32Int32StringStringBooleanBoolean = (a, b, c, dataToBeUpdated, d, e, mobileNumbersOnly) =>
			{
				updatedData += dataToBeUpdated;
			};
			ShimHttpRequest.AllInstances.ParamsGet = (x) =>
			{
				return new NameValueCollection()
				{
					{ _reqParam+"0", firstColumn },
					{ _reqParam+"1", secondColumn },
					{ _reqParam+"2", thirdColumn },
				};
			};
			ShimDataFunctions.GetDataTableString = (x) => CreateDummyDataTable();
			ShimGroupInfo.AllInstances.initNotifyString = (x, y) => { };
			ShimGroupInfo.AllInstances.NotifyStringString = (x, y, z) => 
			{
				notificationShown = true;
			};

			// Act
			CallMethod(
				_groupInfoType,
				"btnImport_Click",
				_methodArgs,
				_groupInfoObject);

			// Assert
			notificationShown.ShouldBeTrue();
		}

		private void Initialize()
		{
			Helpers.BasicShims.CreateShims();
			CreateShims();
			_groupInfoType = typeof(GroupInfo);
			_groupInfoObject = CreateInstance(_groupInfoType);
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
			_dgImport = new DataGrid();
			_colName1 = "sample column 1";
			_colName2 = "sample column 2";
			_colName3 = "sample column 3";
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
			SetField(_groupInfoObject, "btnImport", _btnImport);
			SetField(_groupInfoObject, "lblfilename", _lblfilename);
			SetField(_groupInfoObject, "txtSheetName", _txtSheetName);
			SetField(_groupInfoObject, "txtGroupName1", _txtGroupName1);
			SetField(_groupInfoObject, "drpGroup2", _drpGroup2);
			SetField(_groupInfoObject, "drpFolder3", _drpFolder3);
			SetField(_groupInfoObject, "rbImporttoExisting", _rbImporttoExisting); 
			SetField(_groupInfoObject, "rbImporttoNew", _rbImporttoNew);
			SetField(_groupInfoObject, "dataCollectionTable", _dataCollectionTable); 
			SetField(_groupInfoObject, "plImportCompleted", _plImportCompleted); 
			SetField(_groupInfoObject, "plImportGroup", _plImportGroup);
			SetField(_groupInfoObject, "dgImport", _dgImport);
			SetProperty(_groupInfoObject, "ErrorMessage", _errorMessage);
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
		}

		private void CreateShims()
		{
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
					{ _reqParam+"0", _colName1 },
					{ _reqParam+"1", _colName2 },
					{ _reqParam+"2", _colName3 },
				};
			};
			ShimControl.AllInstances.ParentGet = (control) => 
			{
				return control == _groupInfoObject ? new Page() : null;
			};
			ShimImportMapper.AllInstances.HasEmailAddressGet = (x) => true;
			ShimImportMapper.AllInstances.HasMobileNumberGet = (x) => true;
			ShimDataFunctions.GetDataTableString = (x) => new DataTable();
			ShimDataFunctions.GetDataTableSqlCommand = (x) => new DataTable();
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
			var sampleTable = new DataTable { Columns = { colName1, colName2, colName3, } };
			var sampleRow = sampleTable.NewRow();
			sampleRow[colName1] = "sample text";
			sampleRow[colName2] = "sample text";
			sampleRow[colName3] = "sample text";
			sampleTable.Rows.Add(sampleRow);
			return sampleTable;
		}

		private DataTable CreateDataTableWithData(string col1Name, string col2Name, string col3Name, string row1SampleData,string row2SampleData,string row3SampleData)
		{
			var sampleTable = new DataTable { Columns = { col1Name, col2Name, col3Name, } };
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
				return sqlQuery.Contains("INSERT INTO") == true ? "1" : "0";
			};
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
			table.Rows.Add("1", "1", "", "sampleString", "1", "1", "1","T", "333");
			return table;
		}
	}
}

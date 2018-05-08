using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using AjaxControlToolkit;
using AjaxControlToolkit.Fakes;
using ecn.common.classes.Fakes;
using ecn.communicator.Customers;
using ecn.communicator.Customers.Fakes;
using ECN.Communicator.Tests.Helpers;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Customers
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class MTAEditorTests
    {
        private MTAEditor _globalInstance;
        private string _sqlQuery;
        private bool _testExecuted;        
        private IDisposable _shimObject;
        private object[] _methodParams;
        private Button _btnSaveMTA;
        private Button _btnSaveMTAIP;
        private Button _btnSaveMTACustomer;
        private DataTable _dummydataTable;
        private DropDownList _ddlMTA;        
        private DropDownList _ddlCustomer;
        private DropDownList _ddlIsDefault;
        private DropDownList _ddlMTACustomer;
        private DropDownList _ddlBlastConfig;
        private GridView _gridView;
        private Label _lblIPID;
        private Label _lblMTAID;
        private Label _lblEMessage;
        private Label _lblEMessage2;
        private Label _lblEMessage3;
        private Label _lblCustomerID;        
        private Label _lblCustomerMTAID;
        private TextBox _txtMTAName;
        private TextBox _txtSearchIP;
        private TextBox _txtHostName;
        private TextBox _txtIPAddress;
        private TextBox _txtSearchMTA;
        private TextBox _txtSearchHost;
        private TextBox _txtDomainName;
        private TextBox _txtSearchDomain;
        private TextBox _txtSearchCustomer;
        private ModalPopupExtender _mpeMTA;
        private ModalPopupExtender _mpeMTAIP;
        private ModalPopupExtender _mpeMTACustomer;
        private const string _dataTableName = "TEST";
        private const string _executeScalarReturnValue = "100";

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _methodParams = new object[] { };
            _testExecuted = false;
            _sqlQuery = string.Empty;

            InitializeUIControls();
            InititalizeModalPopupExtenders();
            InitializeMTAEditorShimConstructor();
            InitializeMTAEditorShims();
            InitializeDataFunctionShims();
        }

        private void InititalizeModalPopupExtenders()
        {            
            _mpeMTA = new ModalPopupExtender();
            _mpeMTAIP = new ModalPopupExtender();
            _mpeMTACustomer = new ModalPopupExtender();
        }

        private void InitializeUIControls()
        {
            InitializeWebUITextBoxes();
            InitializeWebUIButtons();
            InitializeWebUIGridViews();
            InitializeWebUIDropDownLists();
            InitializeWebUILabels();
        }

        private void InitializeWebUIDropDownLists()
        {
            _ddlCustomer = new DropDownList();
            _ddlCustomer.Items.Add("1");
            _ddlCustomer.SelectedValue = "1";
            _ddlCustomer.SelectedIndex = 0;

            _ddlMTACustomer = new DropDownList();
            _ddlMTACustomer.Items.Add("1");
            _ddlMTACustomer.SelectedValue = "1";
            _ddlMTACustomer.SelectedIndex = 0;

            _ddlIsDefault = new DropDownList();
            _ddlIsDefault.Items.Add("1");
            _ddlIsDefault.SelectedValue = "1";

            _ddlBlastConfig = new DropDownList();
            _ddlBlastConfig.Items.Add("1");
            _ddlBlastConfig.SelectedValue = "1";

            _ddlMTA = new DropDownList();
            _ddlMTA.Items.Add("1");
            _ddlMTA.SelectedValue = "1";
        }

        private void InitializeWebUILabels()
        {
            const string id = "111";

            _lblCustomerMTAID = new Label
            {
                Text = id
            };

            _lblCustomerID = new Label
            {
                Text = id
            };

            _lblMTAID = new Label
            {
                Text = id
            };

            _lblIPID = new Label
            {
                Text = id
            };

            _lblEMessage2 = new Label
            {
                Text = ""
            };

            _lblEMessage = new Label
            {
                Text = ""
            };

            _lblEMessage3 = new Label
            {
                Text = ""
            };
        }

        private void InitializeWebUIGridViews()
        {
            _gridView = new GridView();
        }

        private void InitializeWebUIButtons()
        {
            var text = "TEST";
            _btnSaveMTACustomer = new Button()
            {
                Text = text
            };

            _btnSaveMTA = new Button()
            {
                Text = text
            };

            _btnSaveMTAIP = new Button()
            {
                Text = text
            };
        }

        private void InitializeWebUITextBoxes()
        {
            _txtSearchCustomer = new TextBox()
            {
                Text = "TestCustomer"
            };

            _txtSearchMTA = new TextBox()
            {
                Text = "TestSearch"
            };

            _txtSearchDomain = new TextBox()
            {
                Text = "TestSearchDomain"
            };

            _txtSearchIP = new TextBox()
            {
                Text = "TestIPSearch"
            };

            _txtSearchHost = new TextBox()
            {
                Text = "TestHostName"
            };

            _txtMTAName = new TextBox()
            {
                Text = "TEST-MTAName"
            };

            _txtDomainName = new TextBox()
            {
                Text = "TEST-DomainName"
            };

            _txtIPAddress = new TextBox()
            {
                Text = "128.0.0.1"
            };

            _txtHostName = new TextBox()
            {
                Text = "TEST-HostName"
            };
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void GetParentDT_Test()
        {
            //Arrange
            var mtaEditor = new MTAEditor();
            var privateObject = new PrivateObject(mtaEditor);

            //Act
            var result = privateObject.Invoke("GetParentDT") as DataTable;

            //Assert
            result.TableName.ShouldBe(_dataTableName);
        }

        [Test]
        [TestCase("Add", false, "", "Add")]
        [TestCase("Add", true, "System.NullReferenceException", "Add")]
        [TestCase("Update", false, "", "Add")]
        [TestCase("Update", true, "System.NullReferenceException", "Add")]
        public void AddMTACustomer_Test(string buttonText, bool objectMTAIPNull, string errorString, string expectedButtonText)
        {
            //Arrange
            GetParentDTShim();
            GridViewShims();
            ModalPopupExtenderShim();
            CheckForDefaultMTAShim();

            _btnSaveMTACustomer.Text = buttonText;

            if (objectMTAIPNull)
            {
                _mpeMTACustomer = null;
            }

            var mtaEditor = new MTAEditor();
            var privateObject = new PrivateObject(mtaEditor);
            var resultError = false;

            //Act   
            try
            {
                privateObject.Invoke("AddMTACustomer");
            }
            catch (Exception ex)
            {
                resultError = true;
            }
            
            //Assert
            if (!resultError)
            {
                _btnSaveMTACustomer.Text.ShouldBe(expectedButtonText);
                _lblEMessage3.Text.ShouldBe(errorString);
            }
            else
            {
                _lblEMessage3.Text.ShouldContain(errorString);
            }
        }

        [Test]
        [TestCase("Add", false, "", "Add")]
        [TestCase("Add", true, "System.NullReferenceException", "Add")]
        [TestCase("Update", false, "", "Add")]
        [TestCase("Update", true, "System.NullReferenceException", "Add")]
        public void AddMTA_Test(string buttonText, bool objectMTAIPNull, string errorString, string expectedButtonText)
        {
            //Arrange
            GetParentDTShim();
            GridViewShims();
            ModalPopupExtenderShim();
            GetParentDTShim();

            _btnSaveMTA.Text = buttonText;

            if (objectMTAIPNull)
            {
                _mpeMTA = null;
            }

            var mtaEditor = new MTAEditor();
            var privateObject = new PrivateObject(mtaEditor);
            var resultError = false;

            //Act   
            try
            {
                privateObject.Invoke("AddMTA");
            }
            catch (Exception ex)
            {
                resultError = true;
            }

            //Assert
            if (!resultError)
            {
                _btnSaveMTA.Text.ShouldBe(expectedButtonText);
                _lblEMessage.Text.ShouldBe(errorString);
            }
            else
            {
                _lblEMessage.Text.ShouldContain(errorString);
            }
        }

        [Test]
        [TestCase("Add", false, "", "Add")]
        [TestCase("Add", true, "System.NullReferenceException", "Add")]
        [TestCase("Update", false, "", "Add")]
        [TestCase("Update", true, "System.NullReferenceException", "Add")]
        public void AddMTAIP_Test(string buttonText, bool objectMTAIPNull, string errorString, string expectedButtonText)
        {
            //Arrange            
            GetParentDTShim();
            GridViewShims();
            ModalPopupExtenderShim();
            GetParentDTShim();

            _btnSaveMTAIP.Text = buttonText;

            if (objectMTAIPNull)
            {
                _mpeMTAIP = null;
            }

            var mtaEditor = new MTAEditor();
            var privateObject = new PrivateObject(mtaEditor);
            var resultError = false;

            //Act   
            try
            {
                privateObject.Invoke("AddMTAIP");
            }
            catch (Exception ex)
            {
                resultError = true;
            }

            //Assert
            if (!resultError)
            {
                _btnSaveMTAIP.Text.ShouldBe(expectedButtonText);
                _lblEMessage2.Text.ShouldBe(errorString);
            }
            else
            {
                _lblEMessage2.Text.ShouldContain(errorString);
            }
        }

        [Test]
        public void CheckForDefaultMTA_Test()
        {
            //Arrange
            var mtaEditor = new MTAEditor();
            var privateObject = new PrivateObject(mtaEditor);
            _testExecuted = false;
            _methodParams = new object[] { 999 };

            //Act
            int result  = (int)(privateObject.Invoke("CheckForDefaultMTA", _methodParams));

            //Assert
            _testExecuted.ShouldBeTrue();
            result.ShouldBe(int.Parse(_executeScalarReturnValue));
        }


        [Test]
        [TestCase("Add", null,null, true, true)]
        [TestCase("Update", 1, 1, true, false)]
        public void SetupAddMTACustomer_Test(string btnText, int? mtaId, int? customerId, bool initial, bool expected)
        {
            try
            {
                //Arrange
                FillDataTableDummyData("MTAID", "CustomerID", "IsDefault");
                BaseDataBoundControlShims();

                _ddlMTACustomer = new DropDownList();
                InsertDropDownListItems(_ddlMTACustomer, "ddlMTACustomer");

                _ddlCustomer = new DropDownList();
                InsertDropDownListItems(_ddlCustomer, "ddlCustomer");

                var mtaEditor = new MTAEditor();
                _globalInstance = mtaEditor;

                var privateObject = new PrivateObject(mtaEditor);
                _methodParams = new object[] { mtaId, customerId };
                _ddlCustomer.Enabled = initial;

                //Act
                privateObject.Invoke("SetupAddMTACustomer", _methodParams);

                //Assert
                _ddlCustomer.Enabled.ShouldBe(expected);
                _btnSaveMTACustomer.Text.ShouldBe(btnText);
            }
            finally
            {
                _ddlMTACustomer.Dispose();
                _ddlCustomer.Dispose();
            }
        }

        [Test]
        [TestCase("Add", null, true, true)]
        [TestCase("Update", 1, true, false)]
        public void SetupAddMTA_Test(string btnText, int? mtaId, bool initial, bool expected)
        {
            try
            {
                //Arrange
                FillDataTableDummyData("MTAName", "DomainName", "BlastConfigID");
                BaseDataBoundControlShims();

                _ddlBlastConfig = new DropDownList();
                InsertDropDownListItems(_ddlBlastConfig, "ddlBlastConfig");

                var mtaEditor = new MTAEditor();
                _globalInstance = mtaEditor;

                var privateObject = new PrivateObject(mtaEditor);
                _methodParams = new object[] { mtaId };
                _ddlBlastConfig.Enabled = initial;

                //Act
                privateObject.Invoke("SetupAddMTA", _methodParams);

                //Assert
                _ddlBlastConfig.Enabled.ShouldBe(expected);
                _btnSaveMTA.Text.ShouldBe(btnText);
            }
            finally
            {
                _ddlBlastConfig.Dispose();
            }
        }

        [Test]
        [TestCase("Add", null)]
        [TestCase("Update", 1)]
        public void SetupAddMTAIP_Test(string btnText, int? mtaId)
        {
            try
            {
                //Arrange
                FillDataTableDummyData("MTAID", "IPAddress", "HostName");
                BaseDataBoundControlShims();

                _ddlMTA = new DropDownList();
                InsertDropDownListItems(_ddlMTA, "ddlMTA");

                var mtaEditor = new MTAEditor();
                _globalInstance = mtaEditor;

                var privateObject = new PrivateObject(mtaEditor);
                _methodParams = new object[] { mtaId };

                //Act
                privateObject.Invoke("SetupAddMTAIP", _methodParams);

                //Assert
                _btnSaveMTAIP.Text.ShouldBe(btnText);
            }
            finally
            {
                _ddlMTA.Dispose();
            }
        }

        [Test]
        [TestCase(1, 1, "true", "delete")]
        [TestCase(1, 1, "false", "update")]
        public void DeleteMTACustomer_Test(int mtaId, int customerID, string isDefault, string sql)
        {
            //Arrange
            FillDataTableDummyData("MTAID", "CustomerID", "IsDefault", isDefault);
            BaseDataBoundControlShims();
            GridViewShims();
            GetParentDTShim();

            var mtaEditor = new MTAEditor();            
            var privateObject = new PrivateObject(mtaEditor);
            _methodParams = new object[] { mtaId, customerID };

            //Act
            privateObject.Invoke("DeleteMTACustomer", _methodParams);

            //Assert
            _sqlQuery.ShouldContain(sql);
        }

        [Test]
        public void GetCustomersForMTA_Test()
        {
            //Arrange
            var mtaEditor = new MTAEditor();
            var privateObject = new PrivateObject(mtaEditor);
            
            //Act
            var dataTable = privateObject.Invoke("GetCustomersForMTA", 1) as DataTable;

            //Assert
            dataTable.TableName.ShouldBe(_dataTableName);
        }

        private void InsertDropDownListItems(DropDownList dropDownList, string id)
        {
            dropDownList.ID = id;
            dropDownList.Items.Insert(0, new ListItem("1","1"));
        }

        private void FillDataTableDummyData(string col1,string col2, string col3, string isDefault = "true")
        {
            _dummydataTable = new DataTable();
            _dummydataTable.TableName = _dataTableName;
            _dummydataTable.Columns.Add(col1, typeof(string));
            _dummydataTable.Columns.Add(col2, typeof(string));
            _dummydataTable.Columns.Add(col3, typeof(string));

            if (col3.Equals("BlastConfigID"))
            {
                _dummydataTable.Rows.Add("1", "1", "1");
            }
            else
            {
                _dummydataTable.Rows.Add("1", "1", isDefault);
            }
        }

        private void InitializeMTAEditorShims()
        {
            ShimMTAEditor.AllInstances.UpdateDefaultInt32Int32String = (MTAEditor instance, int cId, int mId, string dafaultVal) =>
            {
                //Do nothing
            };
            
            ShimMTAEditor.AllInstances.GetMTAList = (instance) =>
            {
                return GetDataTable();
            };

            ShimMTAEditor.AllInstances.GetCustomerList = (instace) =>
            {
                return GetDataTable();
            };

            ShimMTAEditor.AllInstances.GetMTAListForCustomerInt32 = (MTAEditor instance, int customerId) =>
            {
                return GetDataTable();
            };           
        }

        private DataTable GetDataTable()
        {
            if (_dummydataTable != null)
            {
                return _dummydataTable;
            }

            DataTable dataTable = new DataTable
            {
                TableName = _dataTableName
            };

            return dataTable;
        }

        private void InitializeDataFunctionShims()
        {
            ShimDataFunctions.GetDataTableSqlCommand = (instance) =>
            {
                return GetDataTable();
            };

            ShimDataFunctions.ExecuteSqlCommand = (instance) =>
            {
                _sqlQuery = instance.CommandText;
                return 1;
            };

            ShimDataFunctions.ExecuteString = (instance) =>
            {
                _sqlQuery = instance;
                return 1;
            };
           
            ShimDataFunctions.ExecuteScalarSqlCommand = (instance) =>
            {
                _testExecuted = true;
                return _executeScalarReturnValue;
            };
        }

        private static void CheckForDefaultMTAShim()
        {
            ShimMTAEditor.AllInstances.CheckForDefaultMTAInt32 = (MTAEditor instance, int customerId) =>
            {
                return 1;
            };
        }

        private void InitializeMTAEditorShimConstructor()
        {
            ShimMTAEditor.Constructor = (instance) =>
            {
                SetFields(instance);
                GetFields(instance);
                SetProperties(instance);
            };
        }

        private void SetFields(MTAEditor instance)
        {
            SetTextBoxesField(instance);                    
            SetDropDownListsField(instance);            
            SetButtonsField(instance);            
            SetLabelsField(instance);            
            SetGridViewsField(instance);            
            SetModalPopupExtendersField(instance);
        }

        private void SetModalPopupExtendersField(MTAEditor instance)
        {
            ReflectionHelper.SetField(instance, "mpeMTA", _mpeMTA);
            ReflectionHelper.SetField(instance, "mpeMTAIP", _mpeMTAIP);
            ReflectionHelper.SetField(instance, "mpeMTACustomer", _mpeMTACustomer);
        }

        private void SetGridViewsField(MTAEditor instance)
        {
            ReflectionHelper.SetField(instance, "GridView1", _gridView);
        }

        private void SetLabelsField(MTAEditor instance)
        {
            ReflectionHelper.SetField(instance, "lblIPID", _lblIPID);
            ReflectionHelper.SetField(instance, "lblMTAID", _lblMTAID);
            ReflectionHelper.SetField(instance, "lblEMessage", _lblEMessage);
            ReflectionHelper.SetField(instance, "lblEMessage2", _lblEMessage2);
            ReflectionHelper.SetField(instance, "lblEMessage3", _lblEMessage3);
            ReflectionHelper.SetField(instance, "lblCustomerID", _lblCustomerID);
            ReflectionHelper.SetField(instance, "lblCustomerMTAID", _lblCustomerMTAID);
        }

        private void SetButtonsField(MTAEditor instance)
        {
            ReflectionHelper.SetField(instance, "btnSaveMTA", _btnSaveMTA);
            ReflectionHelper.SetField(instance, "btnSaveMTAIP", _btnSaveMTAIP);
            ReflectionHelper.SetField(instance, "btnSaveMTACustomer", _btnSaveMTACustomer);
        }

        private void SetDropDownListsField(MTAEditor instance)
        {
            ReflectionHelper.SetField(instance, "ddlMTA", _ddlMTA);
            ReflectionHelper.SetField(instance, "ddlCustomer", _ddlCustomer);
            ReflectionHelper.SetField(instance, "ddlIsDefault", _ddlIsDefault);
            ReflectionHelper.SetField(instance, "ddlMTACustomer", _ddlMTACustomer);
            ReflectionHelper.SetField(instance, "ddlBlastConfig", _ddlBlastConfig);
        }

        private void SetTextBoxesField(MTAEditor instance)
        {
            ReflectionHelper.SetField(instance, "txtMTAName", _txtMTAName);
            ReflectionHelper.SetField(instance, "txtSearchIP", _txtSearchIP);
            ReflectionHelper.SetField(instance, "txtHostName", _txtHostName);
            ReflectionHelper.SetField(instance, "txtIPAddress", _txtIPAddress);
            ReflectionHelper.SetField(instance, "txtSearchMTA", _txtSearchMTA);
            ReflectionHelper.SetField(instance, "txtSearchHost", _txtSearchHost);
            ReflectionHelper.SetField(instance, "txtDomainName", _txtDomainName);
            ReflectionHelper.SetField(instance, "txtSearchDomain", _txtSearchDomain);
            ReflectionHelper.SetField(instance, "txtSearchCustomer", _txtSearchCustomer);
        }

        private void GetFields(MTAEditor instance)
        {
            GetButtonsField(instance);            
            GetDropDownListsField(instance);
        }

        private void GetDropDownListsField(MTAEditor instance)
        {
            _ddlCustomer = ReflectionHelper.GetField(instance, "ddlCustomer") as DropDownList;
            _ddlIsDefault = ReflectionHelper.GetField(instance, "ddlIsDefault") as DropDownList;
            _ddlMTACustomer = ReflectionHelper.GetField(instance, "ddlMTACustomer") as DropDownList;
            _ddlBlastConfig = ReflectionHelper.GetField(instance, "ddlBlastConfig") as DropDownList;
        }

        private void GetButtonsField(MTAEditor instance)
        {
            _btnSaveMTA = ReflectionHelper.GetField(instance, "btnSaveMTA") as Button;
            _btnSaveMTACustomer = ReflectionHelper.GetField(instance, "btnSaveMTACustomer") as Button;
        }

        private void SetProperties(MTAEditor instance)
        {
            ReflectionHelper.SetProperty(_ddlCustomer, "SelectedValue", 1);
            ReflectionHelper.SetProperty(_ddlIsDefault, "SelectedValue", 1);
            ReflectionHelper.SetProperty(_ddlMTACustomer, "SelectedValue", 1);
            ReflectionHelper.SetProperty(_ddlBlastConfig, "SelectedValue", 1);
        }

        private void GetParentDTShim()
        {
            ShimMTAEditor.AllInstances.GetParentDT = (instance) =>
            {
                DataTable dataTable = new DataTable
                {
                    TableName = _dataTableName
                };

                return dataTable;
            };
        }

        private void GridViewShims()
        {
            ShimGridView.AllInstances.DataBind = (instance) =>
            {
                //Do nothing
            };

            ShimModalPopupExtender.AllInstances.Hide = (instance) =>
             {
                 //Do nothing
             };
        }

        private void BaseDataBoundControlShims()
        {
            ShimBaseDataBoundControl.AllInstances.DataBind = (instance) =>
            {
                string cstId = string.Empty;

                if (instance.ID.Equals("ddlMTACustomer"))
                {
                    cstId = instance.ID;
                    InsertDropDownListItems(_ddlMTACustomer, cstId);

                    if (_globalInstance != null)
                    {
                        SetFields(_globalInstance);
                    }
                }
                else if (instance.ID.Equals("ddlCustomer"))
                {
                    cstId = instance.ID;
                    InsertDropDownListItems(_ddlCustomer, cstId);

                    if (_globalInstance != null)
                    {
                        SetFields(_globalInstance);
                    }
                }
                else if(instance.ID.Equals("ddlBlastConfig"))
                {
                    cstId = instance.ID;
                    InsertDropDownListItems(_ddlBlastConfig, cstId);

                    if (_globalInstance != null)
                    {
                        SetFields(_globalInstance);
                    }
                }
                else if (instance.ID.Equals("ddlBlastConfig"))
                {
                    cstId = instance.ID;
                    InsertDropDownListItems(_ddlBlastConfig, cstId);

                    if (_globalInstance != null)
                    {
                        SetFields(_globalInstance);
                    }
                }
                else if (instance.ID.Equals("ddlMTA"))
                {
                    cstId = instance.ID;
                    InsertDropDownListItems(_ddlMTA, cstId);

                    if (_globalInstance != null)
                    {
                        SetFields(_globalInstance);
                    }
                }
            };
        }

        private void ModalPopupExtenderShim()
        {
            ShimModalPopupExtender.AllInstances.Hide = (instance) =>
            {
                //Do nothing
            };
        }
    }
}
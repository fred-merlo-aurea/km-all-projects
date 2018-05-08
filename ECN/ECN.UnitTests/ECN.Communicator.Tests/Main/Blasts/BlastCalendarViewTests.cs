using System;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using ecn.communicator.main.blasts;
using ecn.communicator.main.blasts.Fakes;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Blasts
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class BlastCalendarViewTests
    {
        public object[] _parameters;
        private IDisposable _shimObject;
        private TabContainer _tabContainer1;
        private Calendar _monthCalendar;
        private Xml _xml1;
        private Xml _xmlWeekly;
        private BlastCalendarView _blastCalendar;
        private PrivateObject _privateObject;
        private const string _weeklyCalendarMethod = "WeeklyCalendar";
        private const string _dailyCalendarMethod = "DailyCalendar";
        private const string _monthCalendarDayRenderMethod = "MonthCalender_DayRender";
        private const string _startBlastDate = "04/13/2018";
        private DropDownList _drpCampaigns;
        private DataTable _dtBlastData;
        private DataTable _dummydataTable;
        private RadioButtonList _rbBlastCalType;
        private TextBox _txtDailyCalendar;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _parameters = new object[] { };
            _blastCalendar = new BlastCalendarView();

            InitializeWebControls();
        }

        private void InitializeWebControls()
        {
            InitializeRadioButtonList();

            _tabContainer1 = new TabContainer();
            _monthCalendar = new Calendar();
            _xml1 = new Xml();
            _xmlWeekly = new Xml();
            _dtBlastData = new DataTable();
            _dummydataTable = new DataTable();
            _txtDailyCalendar = new TextBox();

            _drpCampaigns = new DropDownList()
            {
                ID = "Test"
            };
            _drpCampaigns.Items.Insert(0, new ListItem("1", "1"));
        }

        private void InitializeRadioButtonList()
        {
            _rbBlastCalType = new RadioButtonList
            {
                Text = "TEST-RadioBtn"
            };
            _rbBlastCalType.Items.Add(new ListItem("SUM", "SUM"));
            _rbBlastCalType.Items.Add(new ListItem("DET", "DET"));
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
            _tabContainer1?.Dispose();
            _monthCalendar?.Dispose();
            _xml1?.Dispose();
            _xmlWeekly?.Dispose();
            _dtBlastData?.Dispose();
            _dummydataTable?.Dispose();
            _txtDailyCalendar?.Dispose();
            _drpCampaigns?.Dispose();
            _rbBlastCalType?.Dispose();
        }

        [Test]
        public void WeeklyCalendarTest()
        {
            try
            {
                //Arrange
                _blastCalendar = new BlastCalendarView();
                _privateObject = new PrivateObject(_blastCalendar);

                FillDataTableDummyData();
                _dtBlastData = _dummydataTable;
                SetFields(_blastCalendar);
                CreateShims();

                //Act            
                _privateObject.Invoke(_weeklyCalendarMethod, _startBlastDate);

                //Assert
                _xmlWeekly.DocumentContent.ShouldContain("<BlastWeekly><BlastWeek>");
            }
            finally
            {
                _blastCalendar.Dispose();
            }
        }

        [Test]
        public void DailyCalendarTest()
        {
            try
            {
                //Arrange
                _blastCalendar = new BlastCalendarView();
                _privateObject = new PrivateObject(_blastCalendar);

                FillDataTableDummyData();
                _dtBlastData = _dummydataTable;
                SetFields(_blastCalendar);
                CreateShims();

                //Act            
                _privateObject.Invoke(_dailyCalendarMethod);

                //Assert
                _xml1.DocumentContent.ShouldContain("<BlastDaily><BlastDate>");
            }
            finally
            {
                _blastCalendar.Dispose();
            }
        }

        [Test]
        [TestCase("DET", "13", "Blue")]
        [TestCase("SUM", "Active: 500", "0")]
        public void MonthCalender_DayRenderTest(string reportType, string outputText, string outputColor)
        {
            try
            {
                //Arrange
                _blastCalendar = new BlastCalendarView();
                _privateObject = new PrivateObject(_blastCalendar);
                _rbBlastCalType.SelectedValue = reportType;

                if (reportType.Equals("DET"))
                {
                    FillDataTableDummyData();
                }
                else
                {
                    FillDataTableDummyData("SendDate", "SendTime", "SentTotal", "Pending", "Active");
                }

                _dtBlastData = _dummydataTable;

                var tableCell = new TableCell
                {
                    Text = _startBlastDate
                };

                var date = Convert.ToDateTime(_startBlastDate);
                var calendarDay = new CalendarDay(date, false, true, true, false, "13");

                SetFields(_blastCalendar);
                CreateShims();

                _parameters = new object[] { new object(), new DayRenderEventArgs(tableCell, calendarDay) };

                //Act            
                _privateObject.Invoke(_monthCalendarDayRenderMethod, _parameters);

                var output = _parameters[1] as DayRenderEventArgs;

                //Assert
                output.ShouldSatisfyAllConditions(
                    () => output.Cell.Text.ShouldContain(outputText),
                    () => output.Cell.ForeColor.Name.ShouldBe(outputColor));
            }
            finally
            {
                _blastCalendar.Dispose();
            }
        }

        [Test]
        public void Page_Load_Test()
        {
            try
            {
                //Arrange
                _blastCalendar = new BlastCalendarView();
                _privateObject = new PrivateObject(_blastCalendar);

                CreateMasterPage();
                InitializeAllControls(_blastCalendar);
                InitializePageLoadShims();

                var expected = false;
                var rbBlastCalType = new RadioButtonList();
                rbBlastCalType.Items.Add(new ListItem("sum", "sum"));
                ReflectionHelper.SetField(_blastCalendar, "rbBlastCalType", rbBlastCalType);

                //Act
                _privateObject.Invoke("Page_Load", new object[] { null, null });

                if (_parameters.Length > 0)
                {
                    expected = (bool)_parameters[0];
                }

                //Assert
                expected.ShouldBeTrue();
            }
            finally
            {
                _blastCalendar.Dispose();
            }
        }

        [Test]
        [TestCase("04/09/2018", "04/09/2018")]
        [TestCase("04/10/2018", "04/09/2018")]
        [TestCase("04/11/2018", "04/09/2018")]
        [TestCase("04/12/2018", "04/09/2018")]
        [TestCase("04/13/2018", "04/09/2018")]
        [TestCase("04/14/2018", "04/09/2018")]
        [TestCase("04/15/2018", "04/09/2018")]
        public void CalculateWeekDays_Test(string date, string expected)
        {
            try
            {
                //Arrange
                _blastCalendar = new BlastCalendarView();
                _privateObject = new PrivateObject(_blastCalendar);

                var expectedDate = Convert.ToDateTime(expected);
                expected = expectedDate.ToShortDateString();

                //Act
                var result = (DateTime)_privateObject.Invoke("CalculateWeekDays", date);
                date = result.ToShortDateString();

                //Assert
                date.ShouldBe(expected);
            }
            finally
            {
                _blastCalendar.Dispose();
            }
        }

        [Test]
        [TestCase(0, "04/14/2018")]
        [TestCase(0, "123")]
        [TestCase(1, "")]
        [TestCase(2, "")]
        public void BtnSearch_Click_Test(int index, string date)
        {
            try
            {
                //Arrange
                _blastCalendar = new BlastCalendarView();
                _privateObject = new PrivateObject(_blastCalendar);
                _parameters = new object[] { null, EventArgs.Empty };

                _tabContainer1.ActiveTabIndex = index;
                _txtDailyCalendar.Text = date;

                InitializePageLoadShims();
                SetFields(_blastCalendar);

                //Act
                _privateObject.Invoke("btnSearch_Click", _parameters);

                //Assert
                _txtDailyCalendar.Text.ShouldBe("TEST");
            }
            finally
            {
                _blastCalendar.Dispose();
            }
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void TabContainer1_ActiveTabChanged_Test(int index)
        {
            try
            {
                //Arrange
                _blastCalendar = new BlastCalendarView();
                _privateObject = new PrivateObject(_blastCalendar);
                _parameters = new object[] { null, EventArgs.Empty };

                _tabContainer1.ActiveTabIndex = index;
                _txtDailyCalendar.Text = "";

                InitializePageLoadShims();
                SetFields(_blastCalendar);

                //Act
                _privateObject.Invoke("TabContainer1_ActiveTabChanged", _parameters);

                //Assert
                _txtDailyCalendar.Text.ShouldBe("TEST");
            }
            finally
            {
                _blastCalendar.Dispose();
            }
        }

        [Test]
        [TestCase("04/09/2018")]
        [TestCase("")]
        public void BtnDailyBlast_Click_Test(string date)
        {
            try
            {
                //Arrange
                _blastCalendar = new BlastCalendarView();
                _privateObject = new PrivateObject(_blastCalendar);
                _parameters = new object[] { null, EventArgs.Empty };

                _txtDailyCalendar.Text = date;

                InitializePageLoadShims();
                SetFields(_blastCalendar);

                //Act
                _privateObject.Invoke("btnDailyBlast_Click", _parameters);

                //Assert
                _txtDailyCalendar.Text.ShouldBe("TEST");
            }
            finally
            {
                _blastCalendar.Dispose();
            }
        }

        private void InitializePageLoadShims()
        {
            ShimBlastCalendarView.AllInstances.loadCampaignsDRInt32 = (BlastCalendarView instance, int val1) => { };

            ShimBlastCalendarView.AllInstances.LoadUserDD = (instance) => { };

            ShimBlastCalendarView.AllInstances.LoadFilterFromSession = (instance) => { };

            ShimBlastCalendarView.AllInstances.DailyCalendar = (instance) =>
            {
                _txtDailyCalendar.Text = "TEST";
            };

            ShimBlastCalendarView.AllInstances.WeeklyCalendarString = (BlastCalendarView instance, string val1) =>
            {
                _txtDailyCalendar.Text = "TEST";
            };

            ShimBlastCalendarView.AllInstances.BindMonthlyCalendar = (instance) =>
            {
                _txtDailyCalendar.Text = "TEST";
                _parameters = new object[] { true };
            };
        }

        private void FillDataTableDummyData(string col0 = "SendDate", string col1 = "SendTime",
            string col2 = "GroupName", string col3 = "EmailSubject"
            , string col4 = "sendtotal", string col5 = "StatusCode", string col6 = "BlastID")
        {
            _dummydataTable = new DataTable();
            _dummydataTable.TableName = "TEST";
            _dummydataTable.Columns.Add(col0, typeof(string));
            _dummydataTable.Columns.Add(col1, typeof(string));
            _dummydataTable.Columns.Add(col2, typeof(string));
            _dummydataTable.Columns.Add(col3, typeof(string));
            _dummydataTable.Columns.Add(col4, typeof(string));
            _dummydataTable.Columns.Add(col5, typeof(string));
            _dummydataTable.Columns.Add(col6, typeof(string));

            if (col2.Equals("SentTotal") && col3.Equals("Pending") && col4.Equals("Active"))
            {
                _dummydataTable.Rows.Add("04/13/2018", "04/13/2018 12:30:45", "1024", "500", "500", "TESTING", "1234");
            }
            else
            {
                _dummydataTable.Rows.Add("04/13/2018", "04/13/2018 12:30:45", "TestGroup", "TestEmail", "500", "TESTING", "1234");
                _dummydataTable.Rows.Add("04/14/2018", "04/14/2018 12:30:45", "TestGroup", "TestEmail", "500", "TESTING", "1234");
                _dummydataTable.Rows.Add("04/15/2018", "04/15/2018 12:30:45", "TestGroup", "TestEmail", "500", "TESTING", "1234");
            }

            _dummydataTable.DefaultView.Sort = "SendTime";
        }

        private void SetFields(BlastCalendarView instance)
        {
            ReflectionHelper.SetField(instance, "Xml1", _xml1);
            ReflectionHelper.SetField(instance, "XmlWeekly", _xmlWeekly);
            ReflectionHelper.SetField(instance, "TabContainer1", _tabContainer1);
            ReflectionHelper.SetField(instance, "MonthCalender", _monthCalendar);
            ReflectionHelper.SetField(instance, "dtBlastData", _dtBlastData);
            ReflectionHelper.SetField(instance, "rbBlastCalType", _rbBlastCalType);
            ReflectionHelper.SetField(instance, "txtDailyCalendar", _txtDailyCalendar);
        }

        private void CreateShims()
        {
            BasicShims.CreateShims();
            ShimPage.AllInstances.RequestGet = (x) => { return HttpContext.Current.Request; };
            BindWeeklyCalendarShim();
            BindDailyCalendarShim();
            CalculateWeekDaysShim();
        }

        private void CalculateWeekDaysShim()
        {
            ShimBlastCalendarView.AllInstances.CalculateWeekDaysString = (BlastCalendarView instance, string dateString) =>
            {
                return Convert.ToDateTime(_startBlastDate);
            };
        }

        private void BindWeeklyCalendarShim()
        {
            ShimBlastCalendarView.AllInstances.BindWeeklyCalendarString = (BlastCalendarView instance, string stringDate) =>
            {
                //Do nothing
            };
        }

        private void BindDailyCalendarShim()
        {
            ShimBlastCalendarView.AllInstances.BindDailyCalendar = (instance) =>
            {
                //Do nothing
            };
        }

        private void CreateMasterPage()
        {
            var master = new ecn.communicator.MasterPages.Communicator();
            InitializeAllControls(master);
            ShimPage.AllInstances.MasterGet = (instance) => master;
            ShimECNSession.CurrentSession = () => {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new User();
                session.CurrentCustomer = new Customer();
                session.CurrentBaseChannel = new BaseChannel();
                return session;
            };
            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                         new HttpStaticObjectsCollection(), 10, true,
                                         HttpCookieMode.AutoDetect,
                                         SessionStateMode.InProc, false);
            var sessionState = typeof(HttpSessionState).GetConstructor(
                                     BindingFlags.NonPublic | BindingFlags.Instance,
                                     null, CallingConventions.Standard,
                                     new[] { typeof(HttpSessionStateContainer) },
                                     null)
                                .Invoke(new object[] { sessionContainer }) as HttpSessionState;
            ShimUserControl.AllInstances.SessionGet = (p) => sessionState;
        }

        protected void InitializeAllControls(object page)
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
                            TryLinkFieldWithPage(obj, page);
                        }
                    }
                }
            }
        }

        private void TryLinkFieldWithPage(object field, object page)
        {
            if (page is Page)
            {
                var fieldType = field.GetType().GetField("_page", BindingFlags.Public |
                                                                  BindingFlags.NonPublic |
                                                                  BindingFlags.Static |
                                                                  BindingFlags.Instance);

                if (fieldType != null)
                {
                    try
                    {
                        fieldType.SetValue(field, page);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Error: {0}", ex);
                    }
                }
            }
        }
    }
}
using System;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KMPS.MD.Administration;
using KMPS.MD.Administration.Fakes;
using KMPS.MD.Administration.Models;
using Moq;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;
using WebTimer = System.Web.UI.Timer;

namespace KMPS.MD.Tests.Administration
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DataLoadTest : BasePageTests
    {
        private const int TimerInterval = 2000;
        private const byte ClassError = 20;
        private const byte ClassInfo = 2;
        private const int SampleLineNumber = 333;
        private const string ControlTimerName = "timer";
        private const string MethodButtonProdClick = "btnPrd_Click";
        private const string MethodButtonRefreshClick = "btnRefresh_Click";
        private const string MethodInitializeDataComponents = "InitializeDataComponents";
        private const string MethodPrd4OnInfoMessage = "Prd4_OnInfoMessage";
        private const string MethodRef4OnInfoMessage = "Ref4_OnInfoMessage";
        private const string MethodAdd = "Add";
        private const string TextSuccess = "successfully processed";
        private const string TextInProgress = "in progress";
        private const string TextDone = "Done";
        private const string SampleSqlError = "sample sql error";
        private const string SampleDbName = "db1";
        private const string FieldProdComponent = "_prodComponent";
        private const string FieldRefreshComponent = "_refreshComponent";
        private const string ErrorConnectionNotSet = "Database connection string could not be set";
        private static readonly string[] ProdDbDoneNames = {"prd1Done", "prd1Continue", "prd2Done", "prd3Done", "prd4Done"};
        private static readonly string[] RefDbDoneNames = {"ref1Done", "ref1Continue", "ref2Done", "ref3Done", "ref4Done"};
        private static readonly string[] ProdStatusNames = {"prd1Status", "prd2Status", "prd3Status", "prd4Status"};
        private static readonly string[] RefStatusNames = {"ref1Status", "ref2Status", "ref3Status", "ref4Status"};
        private static readonly string[] ControlNames = {"ddlPrd", "ddlRefresh", "btnPrd", "btnRefresh"};

        private DataLoad _testEntity;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var schedulerMock = new Mock<TaskScheduler>();
            ShimTaskScheduler.FromCurrentSynchronizationContext = () => schedulerMock.Object;

            _testEntity = new DataLoad();
            InitializePage(_testEntity);
            ProdStatusNames.ToList().ForEach(name => PrivateType.SetStaticFieldOrProperty(name, null));
            RefStatusNames.ToList().ForEach(name => PrivateType.SetStaticFieldOrProperty(name, null));
            PrivatePage.Invoke(MethodInitializeDataComponents);
        }

        [TestCase("prd1Done", false)]
        [TestCase("prd1Continue", false)]
        [TestCase("prd2Done", false)]
        [TestCase("prd3Done", false)]
        [TestCase("prd4Done", false)]
        [TestCase("ref1Done", false)]
        [TestCase("ref1Continue", false)]
        [TestCase("ref2Done", false)]
        [TestCase("ref3Done", false)]
        [TestCase("ref4Done", false)]
        [TestCase("EstimatedRestoreSeconds", 0)]
        public void StatusProperties_GetsDefaultValue_ReturnsDefaultValue(string propertyName, object expected)
        {
            // Arrange, Act, Assert
            PrivatePage.GetProperty(propertyName).ShouldBe(expected);
        }

        [TestCase("prd1Done", true)]
        [TestCase("prd1Continue", true)]
        [TestCase("prd2Done", true)]
        [TestCase("prd3Done", true)]
        [TestCase("prd4Done", true)]
        [TestCase("ref1Done", true)]
        [TestCase("ref1Continue", true)]
        [TestCase("ref2Done", true)]
        [TestCase("ref3Done", true)]
        [TestCase("ref4Done", true)]
        [TestCase("EstimatedRestoreSeconds", 20)]
        public void StatusProperties_SetsWithValues_UpdatesSessionStatus(string propertyName, object expected)
        {
            // Arrange, Act
            PrivatePage.SetProperty(propertyName, expected);

            // Assert
            PrivatePage.GetProperty(propertyName).ShouldBe(expected);
        }

        [Test]
        public void btnPrd_Click_GetConnection_SetsLabelsAndStatus()
        {
            // Arrange
            var jobDoneNames = ProdDbDoneNames.ToList();
            var jobStatusNames = ProdStatusNames.ToList();
            InitializeForButtonClick();
            ShimDataLoad.AllInstances.GetConnectionString = (load, dbString) => dbString;

            // Act
            PrivatePage.Invoke(MethodButtonProdClick, this, EventArgs.Empty);

            // Assert
            PrivatePage.GetProperty(jobDoneNames.First()).ShouldBe(true);
            jobDoneNames.RemoveAt(0);
            jobDoneNames.ShouldAllBe(name => !(bool)PrivatePage.GetProperty(name));
            PrivateType.GetStaticFieldOrProperty(jobStatusNames.First()).ToString().ShouldContain("Started");
            jobStatusNames.RemoveAt(0);
            jobStatusNames.ShouldAllBe(name => (string)PrivateType.GetStaticFieldOrProperty(name) == string.Empty);
            ControlNames.ShouldAllBe(name => !((WebControl)PrivatePage.GetField(name)).Enabled);
            VerifyStatusMessages(true, "Page controls will be disabled");
            VerifyTimer(true, TimerInterval);
        }

        [Test]
        public void btnPrd_Click_GetEmptyConnectionString_DisplaysError()
        {
            // Arrange
            InitializeForButtonClick();
            ShimDataLoad.AllInstances.GetConnectionString = (_, __) => string.Empty;

            // Act
            PrivatePage.Invoke(MethodButtonProdClick, this, EventArgs.Empty);

            // Assert
            ProdDbDoneNames.ShouldAllBe(name => !(bool)PrivatePage.GetProperty(name));
            ProdStatusNames.ShouldAllBe(name => (string)PrivateType.GetStaticFieldOrProperty(name) == string.Empty);
            ControlNames.ShouldAllBe(name => !((WebControl)PrivatePage.GetField(name)).Enabled);
            VerifyStatusMessages(true, ErrorConnectionNotSet);
            VerifyTimer(true, TimerInterval);
        }

        [Test]
        public void btnRefresh_Click_GetConnection_SetsLabelsAndStatus()
        {
            // Arrange
            var jobDoneNames = RefDbDoneNames.ToList();
            var jobStatusNames = RefStatusNames.ToList();
            InitializeForButtonClick();
            ShimDataLoad.AllInstances.GetConnectionString = (load, dbString) => dbString;
            ShimSqlConnection.ConstructorString = (_, __) => { };
            ShimSqlConnection.AllInstances.Open = _ => { };
            ShimDataLoad.AllInstances.RunCmdTaskSchedulerSqlConnectionString = (a, b, c, d) => { };

            // Act
            PrivatePage.Invoke(MethodButtonRefreshClick, this, EventArgs.Empty);

            // Assert
            jobDoneNames.ShouldAllBe(name => !(bool)PrivatePage.GetProperty(name));
            PrivateType.GetStaticFieldOrProperty(jobStatusNames.First()).ToString().ShouldContain("Started");
            jobStatusNames.RemoveAt(0);
            jobStatusNames.ShouldAllBe(name => (string)PrivateType.GetStaticFieldOrProperty(name) == string.Empty);
            ControlNames.ShouldAllBe(name => !((WebControl)PrivatePage.GetField(name)).Enabled);
            VerifyStatusMessages(true, "Page controls will be disabled");
            VerifyTimer(true, TimerInterval);
        }

        [Test]
        public void btnRefresh_Click_GetEmptyConnectionString_DisplaysError()
        {
            // Arrange
            InitializeForButtonClick();
            ShimDataLoad.AllInstances.GetConnectionString = (load, dbString) => string.Empty;

            // Act
            PrivatePage.Invoke(MethodButtonRefreshClick, this, EventArgs.Empty);

            // Assert
            RefDbDoneNames.ShouldAllBe(name => !(bool)PrivatePage.GetProperty(name));
            RefStatusNames.ShouldAllBe(name => (string)PrivateType.GetStaticFieldOrProperty(name) == string.Empty);
            ControlNames.ShouldAllBe(name => !((WebControl)PrivatePage.GetField(name)).Enabled);
            VerifyStatusMessages(true, ErrorConnectionNotSet);
            VerifyTimer(true, TimerInterval);
        }

        [Test]
        public void Prd4_OnInfoMessage_WithError_DisplaysError()
        {
            // Arrange
            var eventArg = GetSqlInfoMessageEventArgs(ClassError, SampleSqlError);

            // Act
            PrivatePage.Invoke(MethodPrd4OnInfoMessage, this, eventArg);

            // Assert
            VerifyStatusMessages(true, SampleSqlError);
        }

        [Test]
        public void Prd4_OnInfoMessage_WithSuccessMessage_DisplaysDone()
        {
            // Arrange
            var eventArg = GetSqlInfoMessageEventArgs(ClassInfo, TextSuccess);
            var component = (DataLoadComponent)PrivatePage.GetFieldOrProperty(FieldProdComponent);

            // Act
            PrivatePage.Invoke(MethodPrd4OnInfoMessage, this, eventArg);

            // Assert
            VerifyStatusMessages(true, string.Empty);
            component.ShouldSatisfyAllConditions(
                () => component.Step4Done.ShouldBeTrue(),
                () => component.Messages.Step4Status.ShouldBe(TextDone)
            );
        }

        [Test]
        public void Prd4_OnInfoMessage_WithInfoMessage_DisplaysMessage()
        {
            // Arrange
            var eventArg = GetSqlInfoMessageEventArgs(ClassInfo, TextInProgress);
            var component = (DataLoadComponent)PrivatePage.GetFieldOrProperty(FieldProdComponent);

            // Act
            PrivatePage.Invoke(MethodPrd4OnInfoMessage, this, eventArg);

            // Assert
            VerifyStatusMessages(true, string.Empty);
            component.ShouldSatisfyAllConditions(
                () => component.Step4Done.ShouldBeFalse(),
                () => component.Messages.Step4Status.ShouldBe(TextInProgress)
            );
        }

        [Test]
        public void Ref4_OnInfoMessage_WithError_DisplaysError()
        {
            // Arrange
            var eventArg = GetSqlInfoMessageEventArgs(ClassError, SampleSqlError);

            // Act
            PrivatePage.Invoke(MethodRef4OnInfoMessage, this, eventArg);

            // Assert
            VerifyStatusMessages(true, SampleSqlError);
        }

        [Test]
        public void Ref4_OnInfoMessage_WithSuccessMessage_DisplaysDone()
        {
            // Arrange
            var eventArg = GetSqlInfoMessageEventArgs(ClassInfo, TextSuccess);
            var component = (DataLoadComponent)PrivatePage.GetFieldOrProperty(FieldRefreshComponent);

            // Act
            PrivatePage.Invoke(MethodRef4OnInfoMessage, this, eventArg);

            // Assert
            VerifyStatusMessages(true, string.Empty);
            component.ShouldSatisfyAllConditions(
                () => component.Step4Done.ShouldBeTrue(),
                () => component.Messages.Step4Status.ShouldBe(TextDone)
            );
        }

        [Test]
        public void Ref4_OnInfoMessage_WithInfoMessage_DisplaysMessage()
        {
            // Arrange
            var eventArg = GetSqlInfoMessageEventArgs(ClassInfo, TextInProgress);
            var component = (DataLoadComponent)PrivatePage.GetFieldOrProperty(FieldRefreshComponent);

            // Act
            PrivatePage.Invoke(MethodRef4OnInfoMessage, this, eventArg);

            // Assert
            VerifyStatusMessages(true, string.Empty);
            component.ShouldSatisfyAllConditions(
                () => component.Step4Done.ShouldBeFalse(),
                () => component.Messages.Step4Status.ShouldBe(TextInProgress)
            );
        }

        private SqlInfoMessageEventArgs GetSqlInfoMessageEventArgs(byte errorClass, string message)
        {
            var error = ReflectionHelper.CreateInstance<SqlError>(0, errorClass, errorClass, string.Empty, message, string.Empty, SampleLineNumber);
            var collection = ReflectionHelper.CreateInstance<SqlErrorCollection>();
            ReflectionHelper.CallMethod(collection, MethodAdd, error);

            var exception = ReflectionHelper.CreateInstance<SqlException>(message, collection, new Exception(), Guid.Empty);
            return ReflectionHelper.CreateInstance<SqlInfoMessageEventArgs>(exception);
        }

        private void InitializeForButtonClick()
        {
            InitializeDropDownList("ddlPrd");
            InitializeDropDownList("ddlRefresh");
        }

        private void InitializeDropDownList(string fieldName)
        {
            var dropDownList = (DropDownList)PrivatePage.GetField(fieldName);
            dropDownList.Items.Add(string.Empty);
            dropDownList.Items.Add(SampleDbName);
            dropDownList.SelectedIndex = 1;
        }

        private void VerifyStatusMessages(bool visible, string message)
        {
            ((HtmlGenericControl)PrivatePage.GetField("divError")).Visible.ShouldBe(visible);
            ((Label)PrivatePage.GetField("lblErrorMessage")).Text.ShouldContain(message);
        }

        private void VerifyTimer(bool enabled, int interval)
        {
            var timer = (WebTimer)PrivatePage.GetField(ControlTimerName);
            timer.Enabled.ShouldBe(enabled);
            timer.Interval.ShouldBe(interval);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using ecn.collector.main.survey.UserControls;
using ecn.collector.main.survey.UserControls.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Collector.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using Assert = NUnit.Framework.Assert;
using BusinessLayerFakes = ECN_Framework_BusinessLayer.Communicator.Fakes;
using CollectorEntities = ECN_Framework_Entities.Collector;
using CollectorEntitiesFakes = ECN_Framework_Entities.Collector.Fakes;
using DataLayerFakes = ECN_Framework_DataLayer.Collector.Fakes;

namespace ECN.Collector.Tests.Main.Survey.UserControls
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.collector.main.survey.UserControls.DefineSurvey"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class DefineSurveyTest : PageHelper
    {
        private const string TxtActivationDateId = "txtActivationDate";
        private const string TxtDeActivationDateId = "txtDeActivationDate";
        private const string LblCopySurveyIDId = "lblCopySurveyID";
        private const string DrpSurveySelectedIndexChangedMethod = "drpSurvey_SelectedIndexChanged";
        private const string RbNewSurveyCheckedChangedMethod = "rbNewSurvey_CheckedChanged";
        private const string RbCopySurveyCheckedChangedMethod = "rbCopySurvey_CheckedChanged";
        private const string OnInitMethod = "OnInit";
        private const string DrpSurveyId = "drpSurvey";
        private const string PlCopySurveyId = "plCopySurvey";
        private PrivateObject _defineSurveyPrivateObject;
        private DefineSurvey _defineSurveyInstance;
        private ShimDefineSurvey _shimDefineSurvey;

        [Test]
        public void OnInit_NoError()
        {
            // Act
            var action = (TestDelegate)(() => _defineSurveyPrivateObject.Invoke(OnInitMethod, new object[] { EventArgs.Empty }));

            // Assert
            Assert.DoesNotThrow(action);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ServerSide_isDate_ArgValidIfValidData(bool isValidDate)
        {
            // Arrange
            var date = isValidDate
                        ? DateTime.Now.ToString()
                        : "notValid";
            var arg = new ServerValidateEventArgs(date, false);

            // Act
            _defineSurveyInstance.ServerSide_isDate(null, arg);

            // Assert
            arg.IsValid.ShouldBe(isValidDate);
        }

        [Test]
        public void RbNewSurvey_CheckedChanged_PlCopySurveyInvisible()
        {
            // Arrange
            var plCopySurvey = GetControl<PlaceHolder>(PlCopySurveyId);

            // Act
            _defineSurveyPrivateObject.Invoke(RbNewSurveyCheckedChangedMethod, new object[] { null, EventArgs.Empty });

            // Assert
            plCopySurvey.Visible.ShouldBeFalse();
        }

        [Test]
        public void RbCopySurvey_CheckedChanged_PlCopySurveyVisible()
        {
            // Arrange
            var plCopySurvey = GetControl<PlaceHolder>(PlCopySurveyId);
            plCopySurvey.Visible = false;
            DataLayerFakes.ShimSurvey.GetListSqlCommandInt32 = (cmd, id) => new List<CollectorEntities.Survey>();

            // Act
            _defineSurveyPrivateObject.Invoke(RbCopySurveyCheckedChangedMethod, new object[] { null, EventArgs.Empty });

            // Assert
            plCopySurvey.Visible.ShouldBeTrue();
        }

        [Test]
        public void Initialize_NoException_ControlsInitialized()
        {
            // Arrange
            _defineSurveyInstance.SurveyID = 10;
            var currentDate = DateTime.Now;
            ShimSurvey.GetBySurveyIDInt32User = (id, user) => new CollectorEntities.Survey()
            {
                SurveyTitle = "title",
                Description = "description",
                DisableDate = currentDate,
                EnableDate = currentDate
            };
            var txtActivationDate = GetControl<TextBox>(TxtActivationDateId);
            var txtDeActivationDate = GetControl<TextBox>(TxtDeActivationDateId);

            // Act
            _defineSurveyInstance.Initialize();

            // Assert
            _defineSurveyInstance.ErrorMessage.ShouldSatisfyAllConditions(
                () => _defineSurveyInstance.ErrorMessage.ShouldBeNullOrEmpty(),
                () => txtActivationDate.Text.ShouldBe(currentDate.ToString("MM/dd/yyyy")),
                () => txtDeActivationDate.Text.ShouldBe(currentDate.ToString("MM/dd/yyyy")));
        }

        [Test]
        public void Initialize_Exception_Error()
        {
            // Arrange
            _defineSurveyInstance.SurveyID = 10;
            ShimSurvey.GetBySurveyIDInt32User = (id, user) => throw new Exception();

            // Act
            _defineSurveyInstance.Initialize();

            // Assert
            _defineSurveyInstance.ErrorMessage.ShouldNotBeNullOrEmpty();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void DrpSurvey_SelectedIndexChanged_ControlsInitialized(bool noSurveyFound)
        {
            // Arrange
            var dropDownSurvey = GetControl<DropDownList>(DrpSurveyId);
            dropDownSurvey.Items.Add(new ListItem("1", "1") { Selected = true });
            var currentDate = DateTime.Now;
            ShimSurvey.GetBySurveyIDInt32User = (id, user) =>
            {
                return noSurveyFound
                        ? null
                        : new CollectorEntities.Survey()
                        {
                            EnableDate = currentDate,
                            DisableDate = currentDate
                        };
            };
            var txtActivationDate = GetControl<TextBox>(TxtActivationDateId);
            var txtDeActivationDate = GetControl<TextBox>(TxtDeActivationDateId);

            // Act
            _defineSurveyPrivateObject.Invoke(DrpSurveySelectedIndexChangedMethod, new object[] { null, EventArgs.Empty });

            // Assert
            txtActivationDate.ShouldSatisfyAllConditions(
                () => txtActivationDate.Text.ShouldBe(noSurveyFound
                        ? string.Empty
                        : currentDate.ToString("MM/dd/yyyy")),
                () => txtDeActivationDate.Text.ShouldBe(noSurveyFound
                        ? string.Empty
                        : currentDate.ToString("MM/dd/yyyy")));
        }

        [TestCase(0, false, true, "")]
        [TestCase(1, true, true, "0")]
        [TestCase(1, false, false, "1")]
        public void Save_NoException_ReturnTrue(int surveyId, bool noDates, bool withSurveyInitialValues, string copySurveryId)
        {
            // Arrange
            var survey = InitTestSaveNoException(surveyId, noDates, withSurveyInitialValues, copySurveryId);
            
            // Act
            var result = _defineSurveyInstance.Save();

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeTrue(),
                () => survey.EnableDate.HasValue.ShouldBe(!noDates && surveyId > 0),
                () => survey.DisableDate.HasValue.ShouldBe(!noDates && surveyId > 0));
        }

        [Test]
        public void Save_Exception_ReturnFalse()
        {
            // Arrange
            InitCommon();
            var ecnErrors = new List<ECNError>() { new ECNError() };
            ShimSurvey.SaveSurveyUser = (surveyObj, user) => throw new ECNException(ecnErrors);
            DataLayerFakes.ShimSurvey.GetSqlCommand = (cmd) => new CollectorEntities.Survey();

            // Act
            var result = _defineSurveyInstance.Save();

            // Assert
            result.ShouldBeFalse();
        }

        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _defineSurveyInstance = new DefineSurvey();
            _shimDefineSurvey = new ShimDefineSurvey(_defineSurveyInstance);
            _defineSurveyPrivateObject = new PrivateObject(_defineSurveyInstance);
            InitializeAllControls(_defineSurveyInstance);
            var shimEcnSession = new ShimECNSession();
            ShimECNSession.CurrentSession = () => shimEcnSession;
            ECNSession.CurrentSession().CurrentUser = new KMPlatform.Entity.User() { UserID = 10 };
        }

        private CollectorEntities.Survey InitTestSaveNoException(int surveyId, bool noDates, bool withSurveyInitialValues, string copySurveryId)
        {
            InitCommon();
            _defineSurveyInstance.SurveyID = surveyId;
            GetControl<Label>(LblCopySurveyIDId).Text = copySurveryId;
            if (!noDates)
            {
                GetControl<TextBox>(TxtActivationDateId).Text = DateTime.Now.ToString();
                GetControl<TextBox>(TxtDeActivationDateId).Text = DateTime.Now.ToString();
            }
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) =>
            {
                if (cmd.CommandText == "e_Survey_Exists_ByTitle")
                {
                    return 0;
                }
                return 1;
            };
            CollectorEntitiesFakes.ShimSurvey.AllInstances.SurveyTitleGet = (surveyObj) => "title";
            var survey = CreateSuvey(!withSurveyInitialValues);
            DataLayerFakes.ShimSurvey.GetSqlCommand = (cmd) => survey;
            return survey;
        }

        private CollectorEntities.Survey CreateSuvey(bool setDummyValues = false)
        {
            var survey = new CollectorEntities.Survey();
            if (setDummyValues)
            {
                survey.Description = "Description";
                survey.IntroHTML = "IntroHTML";
                survey.ThankYouHTML = "ThankYouHTML";
            }
            else
            {
                survey.Description = null;
                survey.IntroHTML = null;
                survey.ThankYouHTML = null;
            }
            return survey;
        }

        private void InitCommon()
        {
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (cmd, conn) => true;
            BusinessLayerFakes.ShimGroup.SaveGroupUser = (grp, user) => 1;
        }

        private T GetControl<T>(string name)
        {
            return Get<T>(_defineSurveyPrivateObject, name);
        }
    }
}

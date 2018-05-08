using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using ecn.collector.main.survey.UserControls;
using ecn.collector.main.survey.UserControls.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Collector.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Collector;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using CollectorEntities = ECN_Framework_Entities.Collector;
using CollectorEntitiesFakes = ECN_Framework_Entities.Collector.Fakes;
using DataLayerFakes = ECN_Framework_DataLayer.Collector.Fakes;
using ShimPage = System.Web.UI.Fakes.ShimPage;

namespace ECN.Collector.Tests.Main.Survey.UserControls
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.collector.main.survey.UserControls.DefineTemplate"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class DefineTemplateTest : PageHelper
    {
        private const string BtnResetClickMethod = "btnReset_Click";
        private const string BtnTemplateSaveClickMethod = "btnTemplateSave_Click";
        private const string PAlignId = "pAlign";
        private const string HalignId = "halign";
        private const string FalignId = "falign";
        private const string PhfontsizeId = "phfontsize";
        private const string PhboldId = "phbold";
        private const string PdfontsizeId = "pdfontsize";
        private const string PdboldId = "pdbold";
        private const string DrpShowQuestionNOId = "drpShowQuestionNO";
        private const string QfontsizeId = "qfontsize";
        private const string QboldId = "qbold";
        private const string AfontsizeId = "afontsize";
        private const string AboldId = "abold";
        private const string PfontId = "pfont";
        private const string PWidthId = "pWidth";
        private const string PborderId = "pborder";
        private const string PlToobarId = "plToobar";
        private const string PlTemplatesId = "plTemplates";
        private const string LblTemplateErrorMessageId = "lblTemplateErrorMessage";
        private IDisposable _shimContext;
        private PrivateObject _defineTemplatePrivateObject;
        private DefineTemplate _defineTemplateInstance;
        private ShimDefineTemplate _shimDefineQuestions;
        private DropDownList _drpShowQuestionNODdl;
        private DropDownList _qboldDdl;
        private DropDownList _aboldDdl;
        private DropDownList _pdboldDdl;
        private DropDownList _phboldDdl;
        private DropDownList _pborderDdl;
        private PlaceHolder _plToobar;
        private PlaceHolder _plTemplates;
        private Label _lblTemplateErrorMessage;

        [TestCase(true)]
        [TestCase(false)]
        public void BtnReset_Click_ControlsInitialized(bool showQuestionNo)
        {
            // Arrange
            _defineTemplateInstance.SurveyID = 10;
            var template = CreateSurveyStyles(showQuestionNo);
            DataLayerFakes.ShimSurveyStyles.GetSqlCommand = (cmd) => template;
            if (!showQuestionNo)
            {
                template.pBorder = true;
                template.phBold = true;
                template.pdbold = true;
                template.qbold = true;
                template.abold = true;
            }
            // Act
            _defineTemplatePrivateObject.Invoke(BtnResetClickMethod, new object[] { null, EventArgs.Empty });

            // Assert
            _drpShowQuestionNODdl.ShouldSatisfyAllConditions(
                () => _drpShowQuestionNODdl.SelectedValue.ShouldBe(showQuestionNo
                     ? "1"
                     : "0"),
                () => _qboldDdl.SelectedValue.ShouldBe(showQuestionNo 
                     ? "0" 
                     : "1" ),
                () => _aboldDdl.SelectedValue.ShouldBe(showQuestionNo
                     ? "0"
                     : "1"),
                () => _pdboldDdl.SelectedValue.ShouldBe(showQuestionNo
                     ? "0"
                     : "1"));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void DlTemplates_itemcommand_ControlsInitialized(bool showQuestionNo)
        {
            // Arrange
            var arg = new DataListCommandEventArgs(null, null, new CommandEventArgs("select", 10));
            _defineTemplateInstance.SurveyID = 10;
            var template = CreateTemplates(showQuestionNo);
            DataLayerFakes.ShimTemplates.GetSqlCommand = (cmd) => template;
            if (!showQuestionNo)
            {
                template.pBorder = true;
                template.phBold = true;
                template.pdbold = true;
                template.qbold = true;
                template.abold = true;
            }

            // Act
            _defineTemplateInstance.dlTemplates_itemcommand(null, arg);

            // Assert
            _drpShowQuestionNODdl.ShouldSatisfyAllConditions(
                () => _drpShowQuestionNODdl.SelectedValue.ShouldBe(showQuestionNo
                     ? "1"
                     : "0"),
                () => _qboldDdl.SelectedValue.ShouldBe(showQuestionNo
                     ? "0"
                     : "1"),
                () => _aboldDdl.SelectedValue.ShouldBe(showQuestionNo
                     ? "0"
                     : "1"),
                () => _pdboldDdl.SelectedValue.ShouldBe(showQuestionNo
                     ? "0"
                     : "1"));
        }

        [Test]
        public void Save_PageValid_Saved()
        {
            // Arrange
            ShimPage.AllInstances.IsValidGet = (p) => true;
            DataLayerFakes.ShimSurvey.GetSqlCommand = (cmd) => new CollectorEntities.Survey()
            {
                SurveyTitle = "SurveyTitle"
            };

            // Act
            var result = _defineTemplateInstance.Save();

            // Assert
            result.ShouldBeTrue();
        }

        [Test]
        public void Save_PageValidNullSurveyStyleProperties_Saved()
        {
            // Arrange
            ShimPage.AllInstances.IsValidGet = (p) => true;
            SetMarginControls();
            _phboldDdl.SelectedValue = "0";
            _pdboldDdl.SelectedValue = "0";
            _qboldDdl.SelectedValue = "0";
            _aboldDdl.SelectedValue = "0";
            _drpShowQuestionNODdl.SelectedValue = "0";
            _pborderDdl.SelectedValue = "0";
            DataLayerFakes.ShimSurvey.GetSqlCommand = (cmd) => new CollectorEntities.Survey()
            {
                SurveyTitle = "SurveyTitle"
            };
            SetSurveyStylePropertiesToNull();

            // Act
            var result = _defineTemplateInstance.Save();

            // Assert
            result.ShouldBeTrue();
        }

        [Test]
        public void Save_Exception_Error()
        {
            // Arrange
            ShimPage.AllInstances.IsValidGet = (p) => true;
            DataLayerFakes.ShimSurvey.GetSqlCommand = (cmd) => throw new Exception();

            // Act
            _defineTemplateInstance.Save();

            // Assert
            _defineTemplateInstance.ErrorMessage.ShouldNotBeNullOrEmpty();
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Initialize_ControlsInitialized(int responseCount)
        {
            // Arrange
            _defineTemplateInstance.SurveyID = 10;
            var loadStylesCalled = false;
            var loadTemplatedCalled = false;
            DataLayerFakes.ShimSurvey.GetSqlCommand = (cmd) => new CollectorEntities.Survey()
            {
                SurveyTitle = "SurveyTitle",
                ResponseCount = responseCount
            };
            ShimDefineTemplate.AllInstances.LoadStylesFromSurveyInt32 = (p, id) => { loadStylesCalled = true; };
            DataLayerFakes.ShimTemplates.GetListSqlCommandInt32 = (command, id) =>
            {
                loadTemplatedCalled = true;
                return new List<Templates>();
            };

            // Act
            _defineTemplateInstance.Initialize();

            // Assert
            loadStylesCalled.ShouldSatisfyAllConditions(
                () => loadStylesCalled.ShouldBeTrue(),
                () => _plToobar.Visible.ShouldBe(responseCount <= 0),
                () => _plTemplates.Visible.ShouldBe(responseCount <= 0),
                () => loadTemplatedCalled.ShouldBe(responseCount <= 0));
        }

        [Test]
        public void Initialize_GetSurveyException_Error()
        {
            // Arrange
            _defineTemplateInstance.SurveyID = 10;
            DataLayerFakes.ShimSurvey.GetSqlCommand = (cmd) => throw new Exception();

            // Act
            _defineTemplateInstance.Initialize();

            // Assert
            _defineTemplateInstance.ErrorMessage.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void BtnTemplateSave_Click_TemplateExist_Error()
        {
            // Act
            _defineTemplatePrivateObject.Invoke(BtnTemplateSaveClickMethod, new object[] { null, null });

            // Assert
            _lblTemplateErrorMessage.Visible.ShouldBeTrue();
        }

        [Test]
        public void BtnTemplateSave_Click_TemplateSaved()
        {
            // Arrange
            var loadStylesCalled = false;
            var loadTemplatedCalled = false;
            ShimTemplates.ExistsStringInt32User = (name, id, user) => false;
            ShimDefineTemplate.AllInstances.LoadStylesFromTemplateInt32 = (p, id) => { loadStylesCalled = true; };
            ShimDefineTemplate.AllInstances.LoadTemplates = (p) => { loadTemplatedCalled = true; };
            DataLayerFakes.ShimTemplates.GetListSqlCommandInt32 = (command, id) =>
            {
                loadTemplatedCalled = true;
                return new List<Templates>();
            };

            // Act
            _defineTemplatePrivateObject.Invoke(BtnTemplateSaveClickMethod, new object[] { null, null });

            // Assert
            _lblTemplateErrorMessage.ShouldSatisfyAllConditions(
                () => _lblTemplateErrorMessage.Visible.ShouldBeFalse(),
                () => loadStylesCalled.ShouldBeTrue(),
                () => loadTemplatedCalled.ShouldBeTrue());
        }

        [Test]
        public void BtnTemplateSave_Click_NullTemplateProperties_TemplateSaved()
        {
            // Arrange
            SetTemplatePropertiesToNull();
            SetMarginControls();
            var loadStylesCalled = false;
            var loadTemplatedCalled = false;
            _phboldDdl.SelectedValue = "0";
            _pdboldDdl.SelectedValue = "0";
            _qboldDdl.SelectedValue = "0";
            _aboldDdl.SelectedValue = "0";
            _drpShowQuestionNODdl.SelectedValue = "0";
            _pborderDdl.SelectedValue = "0";
            ShimTemplates.ExistsStringInt32User = (name, id, user) => false;
            ShimDefineTemplate.AllInstances.LoadStylesFromTemplateInt32 = (p, id) => { loadStylesCalled = true; };
            ShimDefineTemplate.AllInstances.LoadTemplates = (p) => { loadTemplatedCalled = true; };

            // Act
            _defineTemplatePrivateObject.Invoke(BtnTemplateSaveClickMethod, new object[] { null, null });

            // Assert
            _lblTemplateErrorMessage.ShouldSatisfyAllConditions(
                () => _lblTemplateErrorMessage.Visible.ShouldBeFalse(),
                () => loadStylesCalled.ShouldBeTrue(),
                () => loadTemplatedCalled.ShouldBeTrue());
        }
         
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _shimContext = ShimsContext.Create();
            _defineTemplateInstance = new DefineTemplate();
            _shimDefineQuestions = new ShimDefineTemplate(_defineTemplateInstance);
            _defineTemplatePrivateObject = new PrivateObject(_defineTemplateInstance);
            InitializeAllControls(_defineTemplateInstance);
            var shimEcnSession = new ShimECNSession();
            ShimECNSession.CurrentSession = () => shimEcnSession;
            ECNSession.CurrentSession().CurrentUser = new KMPlatform.Entity.User() { UserID = 10 };
            InitControls();
            InitCommonShims();
        }

        private void SetMarginControls()
        {
            GetControl<TextBox>("hleftmargin").Text = "margin";
            GetControl<TextBox>("htopmargin").Text = "margin";
            GetControl<TextBox>("hbottommargin").Text = "margin";
            GetControl<TextBox>("hrightmargin").Text = "margin";
            GetControl<TextBox>("fleftmargin").Text = "margin";
            GetControl<TextBox>("ftopmargin").Text = "margin";
            GetControl<TextBox>("frightmargin").Text = "margin";
            GetControl<TextBox>("fbottommargin").Text = "margin";
        }

        private void InitControls()
        {
            _drpShowQuestionNODdl = GetControl<DropDownList>(DrpShowQuestionNOId);
            _qboldDdl = GetControl<DropDownList>(QboldId);
            _aboldDdl = GetControl<DropDownList>(AboldId);
            _pdboldDdl = GetControl<DropDownList>(PdboldId);
            _plToobar = GetControl<PlaceHolder>(PlToobarId);
            _plTemplates = GetControl<PlaceHolder>(PlTemplatesId);
            _lblTemplateErrorMessage = GetControl<Label>(LblTemplateErrorMessageId);
            _lblTemplateErrorMessage.Visible = false;
            _phboldDdl = GetControl<DropDownList>(PhboldId);
            _pborderDdl = GetControl<DropDownList>(PborderId);
            var pAlignDdl = GetControl<DropDownList>(PAlignId);
            var hAlignDdl = GetControl<DropDownList>(HalignId);
            var fAlignDdl = GetControl<DropDownList>(FalignId);
            var phFontSizeDdl = GetControl<DropDownList>(PhfontsizeId);
            var pdFontsizeDdl = GetControl<DropDownList>(PdfontsizeId);
            var qfontsizeDdl = GetControl<DropDownList>(QfontsizeId);
            var afontsizeDdl = GetControl<DropDownList>(AfontsizeId);
            var pfontDdl = GetControl<DropDownList>(PfontId);
            var pWidthDdl = GetControl<DropDownList>(PWidthId);
            pfontDdl.Items.Add(new ListItem("pfontfamily", "pfontfamily") { Selected = true });
            pWidthDdl.Items.Add(new ListItem("pWidth", "pWidth") { Selected = true });
            pAlignDdl.Items.Add(new ListItem("pAlign", "pAlign") { Selected = true });
            hAlignDdl.Items.Add(new ListItem("hAlign", "hAlign") { Selected = true });
            fAlignDdl.Items.Add(new ListItem("fAlign", "fAlign") { Selected = true });
            phFontSizeDdl.Items.Add(new ListItem("phfontsize", "phfontsize") { Selected = true });
            _phboldDdl.Items.Add(new ListItem("0", "0"));
            _phboldDdl.Items.Add(new ListItem("1", "1") { Selected = true });
            pdFontsizeDdl.Items.Add(new ListItem("pdfontsize", "pdfontsize") { Selected = true });
            _pdboldDdl.Items.Add(new ListItem("1", "1") { Selected = true });
            _pdboldDdl.Items.Add(new ListItem("0", "0"));
            _drpShowQuestionNODdl.Items.Add(new ListItem("1", "1") { Selected = true });
            _drpShowQuestionNODdl.Items.Add(new ListItem("0", "0"));
            qfontsizeDdl.Items.Add(new ListItem("qfontsize", "qfontsize"));
            _qboldDdl.Items.Add(new ListItem("1", "1"));
            _qboldDdl.Items.Add(new ListItem("0", "0"));
            afontsizeDdl.Items.Add(new ListItem("afontsize", "afontsize") { Selected = true });
            _aboldDdl.Items.Add(new ListItem("1", "1") { Selected = true });
            _aboldDdl.Items.Add(new ListItem("0", "0"));
            _pborderDdl.Items.Add(new ListItem("1", "1") { Selected = true });
            _pborderDdl.Items.Add(new ListItem("0", "0"));
        }

        private void SetSurveyStylePropertiesToNull()
        {
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.pWidthGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.pbgcolorGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.pAlignGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.pBordercolorGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.pfontfamilyGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.hImageGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.hMarginGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.hAlignGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.hbgcolorGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.phbgcolorGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.phfontsizeGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.pdbgcolorGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.pdfontsizeGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.pdcolorGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.bbgcolorGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.qcolorGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.acolorGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.afontsizeGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.fImageGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.fAlignGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.fMarginGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.fbgcolorGet = (s) => null;
            CollectorEntitiesFakes.ShimSurveyStyles.AllInstances.fbgcolorGet = (s) => null;
        }

        private void SetTemplatePropertiesToNull()
        {
            CollectorEntitiesFakes.ShimTemplates.AllInstances.pWidthGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.pbgcolorGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.pAlignGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.pBordercolorGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.pfontfamilyGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.hImageGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.hMarginGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.hAlignGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.hbgcolorGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.phbgcolorGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.phfontsizeGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.pdbgcolorGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.pdfontsizeGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.pdcolorGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.bbgcolorGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.qcolorGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.acolorGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.afontsizeGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.fImageGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.fAlignGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.fMarginGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.fbgcolorGet = (s) => null;
            CollectorEntitiesFakes.ShimTemplates.AllInstances.fbgcolorGet = (s) => null;
        }

        private SurveyStyles CreateSurveyStyles(bool showQuestionNo)
        {
            var surveyStyles = new SurveyStyles();
            surveyStyles.hMargin = "m1 m2 m3 m4";
            surveyStyles.fMargin = "m1 m2 m3 m4";
            surveyStyles.ShowQuestionNo = showQuestionNo;
            surveyStyles.pfontfamily = "pfontfamily";
            surveyStyles.pWidth = "pWidth";
            surveyStyles.pAlign = "pAlign";
            surveyStyles.hAlign = "hAlign";
            surveyStyles.fAlign = "fAlign";
            surveyStyles.phfontsize = "phfontsize";
            surveyStyles.pdfontsize = "pdfontsize";
            surveyStyles.qfontsize = "qfontsize";
            surveyStyles.afontsize = "afontsize";
            return surveyStyles;
        }

        private Templates CreateTemplates(bool showQuestionNo)
        {
            var templates = new Templates();
            templates.hMargin = "m1 m2 m3 m4";
            templates.fMargin = "m1 m2 m3 m4";
            templates.ShowQuestionNo = showQuestionNo;
            templates.pfontfamily = "pfontfamily";
            templates.pWidth = "pWidth";
            templates.pAlign = "pAlign";
            templates.hAlign = "hAlign";
            templates.fAlign = "fAlign";
            templates.phfontsize = "phfontsize";
            templates.pdfontsize = "pdfontsize";
            templates.qfontsize = "qfontsize";
            templates.afontsize = "afontsize";
            return templates;
        }

        private T GetControl<T>(string controlName) where T : class
        {
            return Get<T>(_defineTemplatePrivateObject, controlName);
        }

        private void InitCommonShims()
        {
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (cmd, conn) => true;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) =>
            {
                if (cmd.CommandText == "e_Survey_Exists_ByTitle")
                {
                    return 0;
                }
                return 1;
            };
            ShimGroup.SaveGroupUser = (grp, user) => 10;
        }
    }
}

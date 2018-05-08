using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using ecn.communicator.main.blasts;
using ecn.communicator.main.blasts.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.Blasts
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.main.blasts.BlastEmailPreview"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class BlastEmailPreviewTest
    {
        private const string AppSettingsLitmusAPIKey = "LitmusAPIKey";
        private const string AppSettingsLitmusAPIPasswordKey = "LitmusAPIPassword";
        private const string AppSettingsLitmusAPIKeyValue = "ApiKey";
        private const string AppSettingsLitmusAPIPasswordValue = "Password";
        private const string AppSettingsKMCommonApplicationKey = "KMCommon_Application";
        private const string AppSettingsKMCommonApplicationValue = "10";
        private const string LbMessageId = "lbMessage";
        private const string MpeMessageId = "mpeMessage";
        private const string RPTSideBarId = "rptSideBar";
        private const string RPTSpamId = "rptSpam";
        private const string LitHtmlId = "litHtml";
        private const string PLPotentialProblemsId = "plPotentialProblems";
        private const string PLHtmlValidationId = "plHtmlValidation";
        private const string LblCodeAnalysisResultId = "lblCodeAnalysisResult";
        private const string RPTPotentialProblemsId = "rptPotentialProblems";
        private const string RPTCodeHtmlValidationId = "rptCodeHtmlValidation";
        private const string RPTLinkCheckId = "rptLinkCheck";
        private const string LblLinkCheckResultId = "lblLinkCheckResult";
        private const string ImageMap1Id = "ImageMap1";
        private const string LblPublicURLId = "lblPublicURL";
        private const string CodeAnalysisResultFieldName = "lcar";
        private Label _labelMessage;
        private Label _labelPublicURL;
        private Label _labelCodeAnalysisResult;
        private Label _labelLinkCheckResult;
        private Repeater _rptSideBar;
        private Repeater _rptSpam;
        private Repeater _rptPotentialProblems;
        private Repeater _rptCodeHtmlValidation;
        private Repeater _rptLinkCheck;
        private PlaceHolder _plHtmlValidation;
        private PlaceHolder _plPotentialProblems;
        private ModalPopupExtender _mpeMessage;
        private Literal _litHtml;
        private ImageMap _imageMap1;
        private IDisposable _shimContext;
        private PrivateObject _blastEmailPreviewPrivateObject;
        private BlastEmailPreview _blastEmailPreviewInstance;
        private ShimBlastEmailPreview _shimBlastEmailPreview;

        [SetUp]
        public void Setup()
        {
            _shimContext = ShimsContext.Create();
            _blastEmailPreviewInstance = new BlastEmailPreview();
            _shimBlastEmailPreview = new ShimBlastEmailPreview(_blastEmailPreviewInstance);
            _blastEmailPreviewPrivateObject = new PrivateObject(_blastEmailPreviewInstance);
        }

        [TearDown]
        public void TearDown()
        {
            _labelMessage?.Dispose();
            _mpeMessage?.Dispose();
            _rptSideBar?.Dispose();
            _rptSpam?.Dispose();
            _rptPotentialProblems?.Dispose();
            _rptCodeHtmlValidation?.Dispose();
            _rptLinkCheck?.Dispose();
            _labelCodeAnalysisResult?.Dispose();
            _labelLinkCheckResult?.Dispose();
            _litHtml?.Dispose();
            _plHtmlValidation?.Dispose();
            _plPotentialProblems?.Dispose();
            _imageMap1?.Dispose();
            _labelPublicURL?.Dispose();
            _shimContext.Dispose();
        }

        private T Get<T>(string fieldName)
        {
            var val = (T)_blastEmailPreviewPrivateObject.GetFieldOrProperty(fieldName);
            return val;
        }

        private void Set(string fieldName, object fieldValue)
        {
            _blastEmailPreviewPrivateObject.SetFieldOrProperty(fieldName, fieldValue);
        }

        private T InitField<T>(string fieldName, object fieldValue = null, bool createInstance = true) where T : new()
        {
            var obj = createInstance 
                ? new T() 
                : fieldValue;
            Set(fieldName, obj);
            return Get<T>(fieldName);
        }
    }
}

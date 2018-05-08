using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ecn.activityengines.engines;
using ecn.activityengines.Tests.Helpers;
using CommunicatorEmailPreview = ECN_Framework_Entities.Communicator.EmailPreview;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Entities = ECN_Framework_Entities.Communicator;
using EmailPreview;
using EmailPreview.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class BlastEmailPreviewTest: PageHelper
    {
        private BlastEmailPreview _testEntity;
        private PrivateObject _privateTestObject;
        private IDisposable _shimObject;
        private List<EmailResult> _emailResults;
        private CodeAnalysisTest _codeAnalysis;
        private LinkTest _linkTest;
        private List<TestingApplication> _testingApplication;
        private List<CommunicatorEmailPreview> _emailPreviews;
        private CommunicatorEmailPreview _defaultEmailPreview;
        private const int BlastID = 5;
        private const int CustomerID = 10;
        private const string ApiID = "ID_1";
        private const string ApiID_Name = "ID_1_Long_Name";
        private const string ApiID_2 = "ID_2";
        private const int DummyInt = 10;
        private const string BlastIDFieldName = "BlastID";
        private const string DummyStringValue = "DummyStringValue";
        private const string HTMLValue = "<html></html>";
        private const string ImageUrl = "http://images.ecn5.com/images/icon-tick1.gif";

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _testEntity = new BlastEmailPreview();
            InitializeAllControls(_testEntity);

            _privateTestObject = new PrivateObject(_testEntity);
            _privateTestObject.SetFieldOrProperty(BlastIDFieldName, BlastID);

            InitializeFakes();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        private void InitializeFakes()
        {
            CreateEmailPreviewsShims();

            CreatePreviewShims();

            CreateBlastShims();

            CreateLayoutShims();
        }

        private static void CreateLayoutShims()
        {
            ShimLayout.GetPreviewNoAccessCheckInt32EnumsContentTypeCodeBooleanInt32NullableOfInt32NullableOfInt32NullableOfInt32 =
                (layoutId, contentType, isMobile, customerId, emailId, groupId, blastId) => HTMLValue;
        }

        private void CreateBlastShims()
        {
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean =
                (blastID, getChildren) =>
                {
                    var blast = new Entities.BlastLayout()
                    {
                        LayoutID = DummyInt,
                        CustomerID = CustomerID
                    };
                    return blast;
                };
        }

        private void CreatePreviewShims()
        {
            _emailResults = new List<EmailResult>();
            _codeAnalysis = new CodeAnalysisTest();
            _linkTest = new LinkTest();
            _testingApplication = new List<TestingApplication>();

            ShimPreview.Constructor =
                (prev) =>
                {
                    new ShimPreview(prev)
                    {
                        GetExistingTestResultsInt32String = (emailId, zipFile) => _emailResults,
                        GetCodeAnalysisTestString = (html) => _codeAnalysis,
                        GetLinkTestResultsInt32 = (id) => _linkTest,
                        GetTestingApplication = () =>  _testingApplication
                    };
                };
        }

        private void CreateEmailPreviewsShims()
        {
            _emailPreviews = new List<CommunicatorEmailPreview>();
            _defaultEmailPreview = new CommunicatorEmailPreview()
            {
                DateCreated = DateTime.Now.Date,
                TimeCreated = DateTime.Now.TimeOfDay,
                EmailTestID = DummyInt,
                ZipFile = DummyStringValue,
                LinkTestID = DummyInt
            };
            _emailPreviews.Add(_defaultEmailPreview);
            ShimEmailPreview.GetByBlastIDInt32 = (blastId) => _emailPreviews;
        }
    }
}

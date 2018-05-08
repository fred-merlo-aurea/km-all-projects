using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.activityengines.Tests.Setup;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ecn.activityengines.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class publicPreviewTest
    {
        private const int Zero = 0;
        private const int One = 1;
        private const int NegativeOne = -1;
        private const string EmptyString = "";
        private publicPreview _publicPreview;
        private PrivateObject _publicPreviewPrivateObject;
        private MocksContext _mocksContext;
        private readonly Random _random = new Random();
        private IDisposable _shimsContext;
        private string _traceMessage;
        private int _applicationId;

        [SetUp]
        public void Setup()
        {
            _applicationId = GetAnyNumber();
            _mocksContext = new MocksContext();
            _mocksContext.AppSettings.Add(KMCommonApplicatoinKey, _applicationId.ToString());
            _publicPreview = new publicPreview();
            _publicPreviewPrivateObject = new PrivateObject(_publicPreview);
            InitializeLiteralPreview();            
            InitializeTraceContextShims();
        }

        private void InitializeTraceContextShims()
        {
            _shimsContext = ShimsContext.Create();

            ShimPage.AllInstances.TraceGet = page =>
            {
                return new ShimTraceContext()
                {
                    WarnStringStringException = (issueType, issueMessage, exception) =>
                    {
                        _traceMessage = string.Format(issueMessage, exception);
                    }
                };
            };
        }

        [TearDown]
        public void TearDown()
        {
            _mocksContext.Dispose();
            _shimsContext.Dispose();
        }

        private int GetAnyNumber(int? minimum = null, int? maximum = null)
        {
            const int DefaultMinimum = 1000;
            const int DefaultMaximum = DefaultMinimum * 10;
            return _random.Next(minimum ?? DefaultMinimum, maximum ?? DefaultMaximum);
        }

        private string GetUniqueString()
        {
            return Guid.NewGuid().ToString();
        }

        private void InitializeLiteralPreview()
        {
            const string FieldName = "literalPreview";
            var value = new Literal();
            _publicPreviewPrivateObject.SetField(FieldName, value);
        }
    }
}

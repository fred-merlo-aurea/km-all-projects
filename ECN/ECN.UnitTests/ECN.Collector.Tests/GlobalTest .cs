using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Collector.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public partial class GlobalTest : PageHelper
    {
        private const string ApplicationErrorMethodName = "Application_Error";
        private const string SampleHostPath = "http://www.sample.com/";
        private const string SampleHttpHost = "http://www.sample.com/home.aspx";
        private const string SampleHost = "SampleHost";
        private const string SampleUserAgent = "Chrome";

        private ecn.collector.Global _testEntity;
        private PrivateObject _privateObject;
        private Dictionary<Exception, string> _logDictionary = new Dictionary<Exception, string>();

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _logDictionary.Clear();
            _testEntity = new ecn.collector.Global();
            _privateObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
        }
    }
}
using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using ecn.activityengines.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ecn.activityengines.Tests
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.activityengines.SClick"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class SClickTest
    {
        private const string AppSettingsEngineAccessKey = "ECNEngineAccessKey";
        private const string AppSettingsEngineAccessKeyValue = "ECNEngineAccessKeyValue";
        private const string AppSettingsSocialDomainPathKey = "Social_DomainPath";
        private const string AppSettingsSocialPreviewKey = "SocialPreview";
        private const string AppSettingsSocialPreviewValue = "SocialPreview";
        private const string AppSettingsSocialDomainPathValue = "SocialDomainPath";
        private const string CacheUserKey = "cache_user_by_AccessKey_";
        private const string ErrorMsgPanelId = "errorMsgPanel";
        private Panel _errorMsgPanel;
        private bool _responseRedirectMethodCalled;
        private IDisposable _shimContext;
        private PrivateObject _sClicktPrivateObject;
        private SClick _sClicktInstance;
        private ShimSClick _shimSClick;

        [SetUp]
        public void Setup()
        {
            _shimContext = ShimsContext.Create();
            _sClicktInstance = new SClick();
            _shimSClick = new ShimSClick(_sClicktInstance);
            _sClicktPrivateObject = new PrivateObject(_sClicktInstance);
        }

        [TearDown]
        public void TearDown()
        {
            _errorMsgPanel?.Dispose();
            _shimContext.Dispose();
        }

        private T Get<T>(string fieldName)
        {
            var val = (T)_sClicktPrivateObject.GetFieldOrProperty(fieldName);
            return val;
        }
        private void Set(string fieldName, object fieldValue)
        {
            _sClicktPrivateObject.SetFieldOrProperty(fieldName, fieldValue);
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

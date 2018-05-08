using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using KMDbManagers.Fakes;
using KMManagers.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace KMManagers.Tests
{
    /// <summary>
    ///     Unit tests for <see cref="KMManagers.FormManager"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class FormManagerTest
    {
        private IDisposable _shimContext;
        private PrivateObject _formManagerPrivateObject;
        private FormManager _formManagerInstance;
        private ShimFormManager _shimFormManager;
        private int _groupDataFieldsSaveMethodCallCount;
        private int _formDbManagerSaveChangedMethodCallCount;
        private NameValueCollection _appSettings;

        [SetUp]
        public void Setup()
        {
            _appSettings = new NameValueCollection();
            _groupDataFieldsSaveMethodCallCount = 0;
            _formDbManagerSaveChangedMethodCallCount = 0;
            _shimContext = ShimsContext.Create();
            InitShims();
            _formManagerInstance = new FormManager();
            _shimFormManager = new ShimFormManager(_formManagerInstance);
            _formManagerPrivateObject = new PrivateObject(_formManagerInstance);
        }

        [TearDown]
        public void TearDown()
        {
            _shimContext.Dispose();
        }

        private void InitShims()
        {
            _appSettings.Add("aspNetMX.LicenseKey",
             "QENSR-6F3UP-Q2QKF-SNTGA-VVWE1-AEY7X-DJBCM-HYF1A-RCRU7-XDXK6-JY6JT-5NCYC-WLD5Z-6KC3L-SX");
            _appSettings.Add("MasterAccessKey", string.Empty);
            _appSettings.Add("ApiDomain", string.Empty);
            _appSettings.Add("ApiTimeout", "1");
            _appSettings.Add("aspNetMXLevel", "1");
            _appSettings.Add("BlockSubmitIfTimeout", "1");
            _appSettings.Add("CssDir", string.Empty);
            ShimCssFileDbManager.AllInstances.AddCssFileInt32 = (instance, cssFile, copyId) => { };
            ShimControlDbManager.AllInstances.AddControl = (instance, control) => { };
            ShimFormControlPropertyDbManager.AllInstances.AddFormControlProperty = (instance, Property) => { };
            ShimFormControlPropertyGridDbManager.AllInstances.AddFormControlPropertyGrid = (instacne, propertyGrid) =>
            {
            };
            ShimDbManagerBase.AllInstances.SaveChanges = instance => { };
            ShimUser.GetByAccessKeyStringBoolean = (accessKey, getChildren) =>
            {
                return new User();
            };
            ShimConfigurationManager.AppSettingsGet = () =>
            {
                var actualAppSetings = (ShimsContext.ExecuteWithoutShims(() => ConfigurationManager.AppSettings));
                var result = new NameValueCollection(_appSettings);
                result.Add(actualAppSetings);
                return result;
            };
            ShimDbManagerBase.AllInstances.SaveChanges = (dbm) => _formDbManagerSaveChangedMethodCallCount++;
            ShimFormManager.AllInstances.FMGet = (s) => new KMDbManagers.FormDbManager();
        }
    }
}

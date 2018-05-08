using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using ecn.communicator.main.ECNWizard;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard
{
    /// <summary>
    /// UT for <see cref="wizardSetup_Numbers"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class WizardSetupNumbersTest
    {
        private const string CampaignItemId = "CampaignItemID";
        private const string GetBlastType = "getBlastType";
        private const string EnableTabBar = "EnableTabBar";
        private const string BlastTypeName = "BlastType";
        private const string GetCampaignItemId = "getCampaignItemID";
        private const int TestValue = 10;
        private const string BlastType = "BlastType";
        private wizardSetup_Numbers _testObject;
        private PrivateObject _privateObject;
        private IDisposable _shimsContext;
        private NameValueCollection _queryString;

        [SetUp]
        public void InitTest()
        {
            _shimsContext = ShimsContext.Create();
            ShimPage.AllInstances.RequestGet = _ => new ShimHttpRequest();
            _queryString = new NameValueCollection();
            ShimHttpRequest.AllInstances.QueryStringGet = _ => _queryString;
            _testObject = new wizardSetup_Numbers();
            _privateObject = new PrivateObject(_testObject);
        }

        [TearDown]
        public void CleanUp()
        {
            _queryString = null;
            _testObject.Dispose();
            _privateObject = null;
            _shimsContext.Dispose();
        }

        [Test]
        public void CampaignItemId_DefaultValueSetValue_ReturnsZeroOrSetValue()
        {
            // Arrange
            var getCampaignItemIdValue = new Func<int>(() => (int) _privateObject.GetFieldOrProperty(CampaignItemId));

            // Act
            var defaultValue = getCampaignItemIdValue();
            _privateObject.SetFieldOrProperty(CampaignItemId, TestValue);

            // Assert
            _testObject.ShouldSatisfyAllConditions(
                () => defaultValue.ShouldBe(0),
                () => getCampaignItemIdValue().ShouldBe(TestValue));
        }

        [Test]
        public void GetBlastType_DefaultValue_ReturnsEmptyString()
        {
            // Arrange
            var getBlastType = new Func<string>(() => (string)_privateObject.Invoke(GetBlastType));

            // Act
            _queryString = null;

            //Assert
            getBlastType().ShouldBeEmpty();
        }

        [Test]
        public void GetBlastType_SetQueryString_ReturnsSetValue()
        {
            // Arrange
            var getBlastType = new Func<string>(() => (string)_privateObject.Invoke(GetBlastType));

            // Act
            _queryString.Add(BlastType, BlastType);

            //Assert
            getBlastType().ShouldBe(BlastType);
        }

        [Test]
        public void GetCampaignItemId_DefaultValue_ReturnsZero()
        {
            // Arrange
            var getCampaignItemId = new Func<int>(() => (int)_privateObject.Invoke(GetCampaignItemId));

            // Act
            _queryString = null;

            // Assert
            getCampaignItemId().ShouldBe(0);
        }

        [Test]
        public void GetCampaignItemId_SetQueryString_SetValue()
        {
            // Arrange
            var getCampaignItemId = new Func<int>(() => (int)_privateObject.Invoke(GetCampaignItemId));

            // Act
            _queryString.Add(CampaignItemId, TestValue.ToString());

            // Assert
            getCampaignItemId().ShouldBe(TestValue);
        }

        private T Get<T>(string fieldName)
        {
            var val = (T)_privateObject.GetFieldOrProperty(fieldName);
            return val;
        }
        private void Set(string fieldName, object fieldValue)
        {
            _privateObject.SetFieldOrProperty(fieldName, fieldValue);
        }
        private T InitField<T>(string fieldName, object fieldValue = null, bool createInstance = true) where T : new()
        {
            var obj = createInstance ? new T() : fieldValue;
            Set(fieldName, obj);
            return Get<T>(fieldName);
        }
        private void InitializeAllControls()
        {
            var fields = _testObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.GetValue(_testObject) == null)
                {
                    var constructor = field.FieldType.GetConstructor(new Type[0]);
                    if (constructor != null)
                    {
                        var obj = constructor.Invoke(new object[0]);
                        if (obj != null)
                        {
                            field.SetValue(_testObject, obj);
                            TryLinkFieldWithPage(obj, _testObject);
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
                        // ignored
                        Trace.TraceError($"Unable to set value as :{ex}");
                    }
                }
            }
        }
    }
}

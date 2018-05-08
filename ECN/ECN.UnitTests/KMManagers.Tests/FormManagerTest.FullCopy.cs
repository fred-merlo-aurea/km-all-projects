using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Fakes;
using System.Data.Entity.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using KMDbManagers.Fakes;
using KMEntities;
using KMEnums;
using KMManagers.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace KMManagers.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class FormManagerTestFullCopy
    {
        private IDisposable _shimsContext;
        private FormManager _formManager;
        private PrivateObject _formManagerPrivate;
        private NameValueCollection _appSettings;
        private List<GroupDataFields> _groupDataFieldsGetByGroupIdChildResult;
        private List<GroupDataFields> _groupDataFieldsGetByGroupIdParentResult;
        private ControlCategory _controlCategoryDbManagerGetByNameResult;
        private NewsletterGroup _newsletterGroupDbManagerAdded;
        private SubscriberLogin _subscriberLoginDbManagerAdded;
        private readonly Random _random = new Random();

        [SetUp]
        public void SetUp()
        {
            _appSettings = new NameValueCollection();
            _shimsContext = ShimsContext.Create();
            CommonShims();
            _formManager = new FormManager();
            _formManagerPrivate = new PrivateObject(_formManager);
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
        }

        [Test]
        public void FullCopy_ControlFieldsNull_ShouldCopyPrperties()
        {
            //Arrange
            var child = new Form();
            var parent = CreateFullForm();
            //Act
            CallFullCopy(child, parent);
            //Assert
            AssertPropertiesAreCopiedToChild(child, parent);
        }

        [Test]
        public void FullCopy_ControlFieldsNotNull_ShouldCopyProperties()
        {
            //Arrange
            var child = new Form();
            var controlField = new Dictionary<string, string>();
            SetControlField(controlField);
            var parent = CreateFullForm(controlField);
            //Act
            CallFullCopy(child, parent);
            //Assert
            AssertPropertiesAreCopiedToChild(child, parent);
        }

        [Test]
        public void FullCopy_GetByNameReturnNull_ShouldCopyPrperties()
        {
            //Arrange
            var child = new Form();
            var parent = CreateFullForm();
            _controlCategoryDbManagerGetByNameResult = null;
            //Act
            CallFullCopy(child, parent);
            //Assert
            AssertPropertiesAreCopiedToChild(child, parent);
            _newsletterGroupDbManagerAdded.ShouldNotBeNull();
            _newsletterGroupDbManagerAdded.ControlCategoryID.ShouldBeNull();
        }

        [Test]
        public void FullCopy_ControlCategoryIDNull_ShouldCopyPrperties()
        {
            //Arrange
            var child = new Form();
            var parent = CreateFullForm();
            foreach (var group in parent.Controls.SelectMany(control => control.NewsletterGroups))
            {
                group.ControlCategoryID = null;
            }
            //Act
            CallFullCopy(child, parent);
            //Assert
            AssertPropertiesAreCopiedToChild(child, parent);
            _newsletterGroupDbManagerAdded.ShouldNotBeNull();
            _newsletterGroupDbManagerAdded.ControlCategoryID.ShouldBeNull();
        }

        [Test]
        public void FullCopy_SubscriberLoginHasNoneNumericIdentification_ShouldCopyPrperties()
        {
            //Arrange
            var child = new Form();
            var parent = CreateFullForm();
            var identification = GetString();
            foreach (var login in parent.SubscriberLogins)
            {
                login.OtherIdentification = identification;
            }
            //Act
            CallFullCopy(child, parent);
            //Assert
            AssertPropertiesAreCopiedToChild(child, parent);
            _subscriberLoginDbManagerAdded.ShouldNotBeNull();
            _subscriberLoginDbManagerAdded.OtherIdentification.ShouldBe(identification);
        }

        private static void AssertPropertiesAreCopiedToChild(Form child, Form parent)
        {
            child.ShouldSatisfyAllConditions(
                () => child.OptInType.ShouldBe(parent.OptInType),
                () => child.SubmitButtonText.ShouldBe(parent.SubmitButtonText),
                () => child.Status.ShouldBe(FormStatus.Saved.ToString()),
                () => child.HeaderHTML.ShouldBe(parent.HeaderHTML),
                () => child.FooterHTML.ShouldBe(parent.FooterHTML),
                () => child.HeaderJs.ShouldBe(parent.HeaderJs),
                () => child.FooterJs.ShouldBe(parent.FooterJs),
                () => child.UpdatedBy.ShouldBe(parent.UpdatedBy),
                () => child.PublishAfter.ShouldBe(parent.PublishAfter),
                () => child.CssUri.ShouldBe(parent.CssUri),
                () => child.StylingType.ShouldBe(parent.StylingType),
                () => child.LanguageTranslationType.ShouldBe(parent.LanguageTranslationType),
                () => child.Iframe.ShouldBe(parent.Iframe),
                () => child.Delay.ShouldBe(parent.Delay));
        }

        private void CommonShims()
        {
            _appSettings.Add("aspNetMX.LicenseKey",
                "QENSR-6F3UP-Q2QKF-SNTGA-VVWE1-AEY7X-DJBCM-HYF1A-RCRU7-XDXK6-JY6JT-5NCYC-WLD5Z-6KC3L-SX");
            _appSettings.Add("MasterAccessKey", string.Empty);
            _appSettings.Add("ApiDomain", string.Empty);
            _appSettings.Add("ApiTimeout", "1");
            _appSettings.Add("aspNetMXLevel", "1");
            _appSettings.Add("BlockSubmitIfTimeout", "1");
            _appSettings.Add("CssDir", string.Empty);
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
            ShimCssFileDbManager.AllInstances.AddCssFileInt32 = (instance, cssFile, copyId) => { };
            ShimControlDbManager.AllInstances.AddControl = (instance, control) => { };
            ShimFormControlPropertyDbManager.AllInstances.AddFormControlProperty = (instance, Property) => { };
            ShimFormControlPropertyGridDbManager.AllInstances.AddFormControlPropertyGrid = (instacne, propertyGrid) =>
            {
            };
            ShimControlCategoryDbManager.AllInstances.AddControlCategory = (instance, controlCategory) => { };
            ShimControlCategoryDbManager.AllInstances.GetByIDInt32 = (instance, id) =>
            {
                return new ControlCategory();
            };
            _controlCategoryDbManagerGetByNameResult = new ControlCategory();
            ShimControlCategoryDbManager.AllInstances.GetByNameInt32String = (instance, id, name) =>
            {
                return _controlCategoryDbManagerGetByNameResult;
            };
            ShimNewsletterGroupUDFDbManager.AllInstances.RemoveAllInt32 = (instance, id) => { };
            ShimNewsletterGroupDbManager.AllInstances.AddNewsletterGroup = (instance, newsLetterGroup) =>
            {
                _newsletterGroupDbManagerAdded = newsLetterGroup;
            };
            ShimRuleDbManager.AllInstances.AddRuleRuleNullableOfInt32NullableOfInt32Int32DictionaryOfInt32Int32 =
                (instance, rule, formId, controlId, groupId, controls) => { };
            ShimConditionGroupDbManager.AllInstances.CopyGroupConditionGroupDictionaryOfInt32Int32 =
                (instance, conditionGroup, groups) =>
                {
                    return 100;
                };
            ShimNotificationDbManager.AllInstances.AddNotification = (instance, notification) => { };
            ShimFormResultDbManager.AllInstances.AddFormResult = (instance, formResult) => { };
            ShimSubscriberLoginDbManager.AllInstances.AddSubscriberLogin = (instance, subscriberLogin) =>
            {
                _subscriberLoginDbManagerAdded = subscriberLogin;
            };
            ShimDbManagerBase.AllInstances.SaveChanges = instance => { };
            ShimDbSet<Form>.AllInstances.AddT0 = (instance, form) => form;
            _groupDataFieldsGetByGroupIdParentResult = new List<GroupDataFields>();
            _groupDataFieldsGetByGroupIdChildResult = new List<GroupDataFields>();
            ShimGroupDataFields.GetByGroupID_NoAccessCheckInt32 = groupId =>
            {
                if (groupId > 0)
                {
                    return _groupDataFieldsGetByGroupIdParentResult;
                }
                else
                {
                    return _groupDataFieldsGetByGroupIdChildResult;
                }
            };
            ShimConditionManager.CopyAllByFormFormDictionaryOfInt32Int32DictionaryOfInt32Int32DictionaryOfInt32Int32 =
                (form, controls, groups, items) => { };
            ShimThirdPartyQueryValueManager.CopyAllByFRFormResultInt32DictionaryOfInt32Int32 =
                (formResult, newId, controlsList) => { };
        }

        private void SetControlField(Dictionary<string, string> value)
        {
            _formManagerPrivate.SetField("ControlField", value);
        }

        private Form CreateFullForm(Dictionary<string, string> controlField = null)
        {
            var result = new Form
            {
                GroupID = GetNumber()
            };
            FillProperties(result);
            foreach (var control in result.Controls)
            {
                var childShortName = GetString();
                _groupDataFieldsGetByGroupIdParentResult.Add(new GroupDataFields
                {
                    GroupDataFieldsID = control.FieldID.Value,
                    ShortName = childShortName
                });
                _groupDataFieldsGetByGroupIdChildResult.Add(new GroupDataFields
                {
                    ShortName = childShortName
                });
                if (controlField != null)
                {
                    controlField[control.FieldLabel] = childShortName;
                }
            }
            foreach (var subscriberLogin in result.SubscriberLogins)
            {
                var parentShortName = GetString();
                var fieldsId = 0;
                int.TryParse(subscriberLogin.OtherIdentification, out fieldsId);
                _groupDataFieldsGetByGroupIdParentResult.Add(new GroupDataFields
                {
                    GroupDataFieldsID = fieldsId,
                    ShortName = parentShortName
                });
                _groupDataFieldsGetByGroupIdChildResult.Add(new GroupDataFields
                {
                    ShortName = parentShortName
                });
                if (controlField != null)
                {
                    controlField["Subscriber Identification"] = parentShortName;
                }
            }
            return result;
        }

        private void FillProperties(object item, int level = 0)
        {
            if (level > 10)
            {
                return;
            }
            var properties = item.GetType().GetProperties();
            var collectionProperties = properties.Where(IsCollectionProperty);
            FillCollectionProperties(item, collectionProperties, ++level);
            var stringProperties = properties.Where(IsStringProperty);
            FillStringProperties(item, stringProperties);
            var nullableProperties = properties.Where(IsNullableProperty);
            FillNullableProperties(item, nullableProperties);
        }

        private void FillNullableProperties(object item, IEnumerable<PropertyInfo> nullableProperties)
        {
            foreach (var property in nullableProperties)
            {
                var type = Nullable.GetUnderlyingType(property.PropertyType);
                var value = Activator.CreateInstance(type);
                if (type.IsAssignableFrom(typeof(Int32)))
                {
                    value = GetNumber();
                }
                property.SetValue(item, value);
            }
        }

        private void FillStringProperties(object item, IEnumerable<PropertyInfo> stringProperties)
        {
            foreach (var property in stringProperties)
            {
                var value = GetString();
                property.SetValue(item, GetNumber().ToString());
            }
        }

        private void FillCollectionProperties(object item, IEnumerable<PropertyInfo> collectionProperties, int level)
        {
            foreach (var property in collectionProperties)
            {
                var type = property.PropertyType.GetGenericArguments()[0];
                var collection = property.GetValue(item);
                var value = CreateInstance(type);
                FillProperties(value, ++level);
                property.PropertyType
                    .GetMethod("Add")
                    .Invoke(collection, new object[] { value });
            }
        }

        private bool IsNullableProperty(PropertyInfo property)
        {
            return Nullable.GetUnderlyingType(property.PropertyType) != null;
        }

        private bool IsStringProperty(PropertyInfo property)
        {
            return property.PropertyType.IsAssignableFrom(typeof(string));
        }

        private bool IsCollectionProperty(PropertyInfo property)
        {
            var type = property.PropertyType;
            return type.IsGenericType &&
                type.GetGenericTypeDefinition()
                    .IsAssignableFrom(typeof(ICollection<>));
        }

        private object CreateInstance(Type type)
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return type
                .GetConstructor(flags, null, new Type[0], null)
                .Invoke(new object[0]);
        }

        private int CallFullCopy(Form child, Form parent)
        {
            return (int)_formManagerPrivate.Invoke("FullCopy", new object[] { child, parent });
        }

        private string GetString()
        {
            return Guid.NewGuid().ToString();
        }

        private int GetNumber()
        {
            return _random.Next(10, 1000);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration.Fakes;
using System.Collections.Specialized;
using AMS_Operations;
using FrameworkSubGen.Entity;
using FrameworkSubGen.BusinessLogic.Fakes;
using NUnit.Framework;
using KMPlatform.BusinessLogic.Fakes;
using FrameworkUAD.BusinessLogic.Fakes;
using Shouldly;
using SubGenAPI = FrameworkSubGen.BusinessLogic.API;
using SubGenApiFakes = FrameworkSubGen.BusinessLogic.API.Fakes;
using UADObjects = FrameworkUAD.Object;
using UADEntity = FrameworkUAD.Entity;
using FrameworkSubGenEntity = FrameworkSubGen.Entity;
using FrameworkSubGenBusinessLogic = FrameworkSubGen.BusinessLogic;

namespace UAS.UnitTests.AMS_Operations
{
    /// <summary>
    /// Unit Test for <see cref="Operations.HourlyDataSync"/>
    /// </summary>
    public partial class OperationTest
    {
        private IList<Bundle> _savedBundles;
        private IList<CustomField> _savedCustomFields;
        private List<Publication> _savedPublications;
        private List<User> _savedUser;
        private List<ValueOption> _savedValueOptions;
        private CustomField _createdCustomField;
        private string _exceptionMessage = string.Empty;

        [Test]
        public void HourlyDataSync_WithNoValueOptions_SavesDataWithoutValueOptions()
        {
            // Arrange 
            SetHourlyDataSyncFakes();
            SetSaveHourlyDataSyncFakes();

            // Act
            _privateOperationsObj.Invoke(HourlyDataSyncMethodName, null);

            // Assert
            _isUserSaved.ShouldBeTrue();
            _isCustomFieldSaved.ShouldBeTrue();
            _isPublicationSaved.ShouldBeTrue();
            _isBundleSaved.ShouldBeTrue();
            _isCustomFieldCreated.ShouldBeTrue();
            _isCustomFieldCreated.ShouldBeTrue();
            _savedBundles.ShouldNotBeNull();
            _savedBundles.ShouldNotBeEmpty();
            _savedCustomFields.ShouldNotBeEmpty();
            _savedUser.ShouldNotBeEmpty();
            _savedPublications.ShouldNotBeEmpty();
            _createdCustomField.ShouldNotBeNull();
        }

        [Test]
        public void HourlyDataSync_SaveDataThrowsException_LogsMessage()
        {
            // Arrange 
            SetHourlyDataSyncFakes();
            ShimBundle.AllInstances.SaveBulkXmlIListOfBundleInt32 =
                (_, __, ___) => throw new InvalidOperationException();

            // Act and Assert
            Should.Throw<InvalidOperationException>(() => _privateOperationsObj.Invoke(HourlyDataSyncMethodName, null));
            _isExpectionLogged.ShouldBeTrue();
            _exceptionMessage.ShouldNotBeEmpty();
            _exceptionMessage.ShouldContain("Operation is not valid due to the current state of the object.");
        }

        [Test]
        public void HourlyDataSync_CustomFieldMatchingNameExist_SavesDataWithOutValueOptionsandCustomFieldCreated()
        {
            // Arrange 
            SetHourlyDataSyncFakes(responseGroupName: $"{SamplePubCode} - {SampleResponseGroup}");
            SetSaveHourlyDataSyncFakes();

            // Act
            _privateOperationsObj.Invoke(HourlyDataSyncMethodName, null);

            // Assert
            _isUserSaved.ShouldBeTrue();
            _isCustomFieldSaved.ShouldBeTrue();
            _isPublicationSaved.ShouldBeTrue();
            _isBundleSaved.ShouldBeTrue();
            _isCustomFieldCreated.ShouldBeFalse();
            _isValueOptionSaved.ShouldBeFalse();
            _savedBundles.ShouldNotBeNull();
            _savedBundles.ShouldNotBeEmpty();
            _savedCustomFields.ShouldNotBeEmpty();
            _savedUser.ShouldNotBeEmpty();
            _savedPublications.ShouldNotBeEmpty();
            _savedValueOptions.ShouldBeNull();
            _createdCustomField.ShouldBeNull();
        }

        [Test]
        public void HourlyDataSync_CustomFieldNonMatchingValueOptionsField_Test()
        {
            // Arrange 
            SetHourlyDataSyncFakes(responseGroupName: $"{SamplePubCode} - {SampleResponseGroup}", fieldId: 1);
            SetSaveHourlyDataSyncFakes();

            // Act
            _privateOperationsObj.Invoke(HourlyDataSyncMethodName, null);

            // Assert
            _isUserSaved.ShouldBeTrue();
            _isCustomFieldSaved.ShouldBeTrue();
            _isPublicationSaved.ShouldBeTrue();
            _isBundleSaved.ShouldBeTrue();
            _isCustomFieldCreated.ShouldBeFalse();
            _isValueOptionSaved.ShouldBeTrue();
            _savedBundles.ShouldNotBeNull();
            _savedBundles.ShouldNotBeEmpty();
            _savedCustomFields.ShouldNotBeEmpty();
            _savedUser.ShouldNotBeEmpty();
            _savedPublications.ShouldNotBeEmpty();
            _savedValueOptions.ShouldNotBeNull();
            _createdCustomField.ShouldBeNull();
        }

        private void SetHourlyDataSyncFakes(string responseGroupName = SampleResponseGroup, int fieldId = 0)
        {
            ShimAccount.AllInstances.Select = a => new List<Account>
            {
                new Account
                {
                    active = true,
                    KMClientId = 1,
                    company_name = Enums.Client.KM_API_Testing.ToString(),
                    account_id = 9999
                }
            };
            SubGenApiFakes.ShimBundle.AllInstances.GetBundlesEnumsClientBooleanBooleanDoubleDoubleStringDoubleString =
                (c, q, t, y, i, a, s, g, h) =>
                {
                    return new List<Bundle>
                    {
                        new Bundle { }
                    };
                };

            SubGenApiFakes.ShimCustomField.AllInstances.GetCustomFieldsEnumsClient = (c, e) =>
            {
                return new List<CustomField>
                {
                    new CustomField { }
                };
            };
            ShimCustomField.AllInstances.SelectInt32 = (c, a) =>
            {
                return new List<CustomField>
                {
                    new CustomField
                    {
                        name = responseGroupName,
                        value_options = new List<ValueOption>
                        {
                            new ValueOption
                            {
                                field_id = fieldId,
                                value = SampleDimensionResponse,
                                display_as = OtherDimensionField
                            }
                        }
                    }
                };
            };
            ShimClient.AllInstances.SelectInt32Boolean = (c, a, b) => { return new KMPlatform.Entity.Client { }; };
            ShimObjects.AllInstances.GetDimensionsClientConnections = (o, c) =>
            {
                return new List<UADObjects.Dimension>
                {
                    new UADObjects.Dimension
                    {
                        ResponseGroupName = SampleResponseGroup,
                        SubGenResponseGroupName = SampleResponseGroup,
                        ProductCode = "SampleProductCode",
                        ProductId = 1,
                        Responsevalue = SampleDimensionResponse,
                        CodeSheetID = 1,
                    }
                };
            };
            SubGenApiFakes.ShimPublication.AllInstances.GetPublicationsEnumsClient = (p, c) =>
            {
                return new List<Publication>
                {
                    new Publication { }
                };
            };
            SubGenApiFakes.ShimUser.AllInstances.GetUsersEnumsClientBooleanInt32StringStringStringString =
                (u, e, c, l, v, o, p, n) => { return new List<User> {new User()}; };
            ShimConfigurationManager.AppSettingsGet = () =>
            {
                var nameValueCollection = new NameValueCollection();
                nameValueCollection.Add("KMtoSubGenActive", "true");
                return nameValueCollection;
            };
            ShimProduct.AllInstances.SelectClientConnectionsBoolean = (p, c, b) =>
            {
                return new List<UADEntity.Product>
                {
                    new UADEntity.Product
                    {
                        HasPaidRecords = true,
                        PubCode = SamplePubCode,
                        PubID = 1,
                        CodeSheets = new List<UADEntity.CodeSheet>
                        {
                            new UADEntity.CodeSheet {ResponseGroupID = 1, ResponseValue = SampleDimensionResponse}
                        },
                        ResponseGroups = new List<UADEntity.ResponseGroup>
                        {
                            new UADEntity.ResponseGroup
                            {
                                ResponseGroupID = 1,
                                IsMultipleValue = true,
                                ResponseGroupName = SampleResponseGroup
                            }
                        }
                    }
                };
            };
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (f, m, a, q, w, r, p) =>
                {
                    _exceptionMessage += m;
                    _isExpectionLogged = true;
                    return 1;
                };
        }

        private void SetSaveHourlyDataSyncFakes()
        {
            ShimBundle.AllInstances.SaveBulkXmlIListOfBundleInt32 = (x, bundles, a) =>
            {
                _isBundleSaved = true;
                _savedBundles = bundles;
                return _isBundleSaved;
            };
            ShimCustomField.AllInstances.SaveBulkXmlIListOfCustomFieldInt32 = (x, customFields, a) =>
            {
                _isCustomFieldSaved = true;
                _savedCustomFields = customFields;
                return _isCustomFieldSaved;
            };
            ShimPublication.AllInstances.SaveBulkXmlListOfPublicationInt32 = (x, publications, a) =>
            {
                _isPublicationSaved = true;
                _savedPublications = publications;
                return _isPublicationSaved;
            };
            ShimValueOption.AllInstances.SaveBulkXmlListOfValueOption = (x, valueOptions) =>
            {
                _isValueOptionSaved = true;
                _savedValueOptions = valueOptions;
                return _isValueOptionSaved;
            };
            FrameworkSubGenBusinessLogic.Fakes.ShimUser.AllInstances.SaveBulkXmlListOfUserInt32 = (x, users, c) =>
            {
                _isUserSaved = true;
                _savedUser = users;
                return _isUserSaved;
            };
            SubGenApiFakes.ShimCustomField.AllInstances.CreateEnumsClientCustomFieldRef =
                (SubGenAPI.CustomField cc, Enums.Client c, ref CustomField customField) =>
                {
                    customField.allow_other = true;
                    _isCustomFieldCreated = true;
                    _createdCustomField = customField;
                    return 1;
                };
        }
    }
}
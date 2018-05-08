using System;
using System.Collections.Generic;
using ecn.webservice;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using KM.Common.Entity.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Enums;
using BusinessLogicFakes = KMPlatform.BusinessLogic.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Accounts;
using EntitiesAccounts =ECN_Framework_Entities.Accounts;

namespace ECN.Webservice.Tests
{
    public partial class ListManagerTest
    {
        [Test]
        public void AddCustomField_OnException_LogErrorAndReturnErrorResponse()
        {
            // Arrange
            var exceptionLogged = false;
            ShimAPILogging.UpdateLogInt32NullableOfInt32 = (id, logid) =>
            {
                exceptionLogged = true;
            };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
               (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
               {
                   return 1;
               };
            var listManager = new ListManager();
            var listId = 1;
            var customFieldName = "customFieldName";
            var customFieldDesc = "customFieldDesc";
            var isPublic = "True";

            // Act	
            var actualResult = listManager.AddCustomField(
               _ECNAccesskey,
               listId,
               customFieldName,
               customFieldDesc,
               isPublic);

            // Assert
            actualResult.ShouldNotBeNull();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void AddCustomField_ECNAccessKeyIsNull_LogErrorAndReturnErrorResponse()
        {
            // Arrange
            var exceptionLogged = false;
            var excMsg = "INVALID ECN ACCESS KEY FORMAT";
            ShimAPILogging.UpdateLogInt32NullableOfInt32 = (id, logid) =>
            {
                exceptionLogged = true;
            };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
               (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
               {
                   return 1;
               };
            var listManager = new ListManager();
            var listId = 1;
            var customFieldName = "customFieldName";
            var customFieldDesc = "customFieldDesc";
            var isPublic = "True";

            // Act	
            var actualResult = listManager.AddCustomField(
               string.Empty,
               listId,
               customFieldName,
               customFieldDesc,
               isPublic);

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.ShouldContain(excMsg);
            exceptionLogged.ShouldBeFalse();
        }

        [Test]
        public void AddCustomField_OnECNException_LogErrorAndReturnErrorResponse()
        {
            // Arrange
            var exceptionLogged = false;
            ShimAPILogging.UpdateLogInt32NullableOfInt32 = (id, logid) =>
            {
                exceptionLogged = true;
            };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
               (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
               {
                   return 1;
               };
            ShimAPILogging.InsertAPILogging = (log) => 1;
            BusinessLogicFakes::ShimUser.AllInstances.LogInGuidBoolean = (obj, id, include) =>
            {
                throw new ECNException(new List<ECNError>(), ExceptionLayer.API);
            };
            var listManager = new ListManager();
            var listId = 1;
            var customFieldName = "customFieldName";
            var customFieldDesc = "customFieldDesc";
            var isPublic = "True";

            // Act	
            var actualResult = listManager.AddCustomField(
               _ECNAccesskey,
               listId,
               customFieldName,
               customFieldDesc,
               isPublic);

            // Assert
            actualResult.ShouldNotBeNull();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void AddCustomField_OnSecurityException_LogErrorAndReturnErrorResponse()
        {
            // Arrange
            var exceptionLogged = false;
            ShimAPILogging.UpdateLogInt32NullableOfInt32 = (id, logid) =>
            {
                exceptionLogged = true;
            };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
               (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
               {
                   return 1;
               };
            ShimAPILogging.InsertAPILogging = (log) => 1;
            BusinessLogicFakes::ShimUser.AllInstances.LogInGuidBoolean = (obj, id, include) =>
            {
                throw new SecurityException();
            };
            var listManager = new ListManager();
            var listId = 1;
            var customFieldName = "customFieldName";
            var customFieldDesc = "customFieldDesc";
            var isPublic = "True";

            // Act	
            var actualResult = listManager.AddCustomField(
               _ECNAccesskey,
               listId,
               customFieldName,
               customFieldDesc,
               isPublic);

            // Assert
            actualResult.ShouldNotBeNull();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void AddCustomField_OnUserLoginException_LogErrorAndReturnErrorResponse()
        {
            // Arrange
            var exceptionLogged = false;
            ShimAPILogging.UpdateLogInt32NullableOfInt32 = (id, logid) =>
            {
                exceptionLogged = true;
            };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
               (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
               {
                   return 1;
               };
            ShimAPILogging.InsertAPILogging = (log) => 1;
            BusinessLogicFakes::ShimUser.AllInstances.LogInGuidBoolean = (obj, id, include) =>
            {
                throw new UserLoginException();
            };
            var listManager = new ListManager();
            var listId = 1;
            var customFieldName = "customFieldName";
            var customFieldDesc = "customFieldDesc";
            var isPublic = "True";

            // Act	
            var actualResult = listManager.AddCustomField(
               _ECNAccesskey,
               listId,
               customFieldName,
               customFieldDesc,
               isPublic);

            // Assert
            actualResult.ShouldNotBeNull();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void AddCustomField_UserIsNull_LogErrorAndReturnErrorResponse()
        {
            // Arrange
            var exceptionLogged = false;
            var excMsg = "LOGIN AUTHENTICATION FAILED";
            ShimAPILogging.UpdateLogInt32NullableOfInt32 = (id, logid) =>
            {
                exceptionLogged = true;
            };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
               (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
               {
                   return 1;
               };
            ShimAPILogging.InsertAPILogging = (log) => 1;
            BusinessLogicFakes::ShimUser.AllInstances.LogInGuidBoolean = (obj, id, include) => null;
            var listManager = new ListManager();
            var listId = 1;
            var customFieldName = "customFieldName";
            var customFieldDesc = "customFieldDesc";
            var isPublic = "True";

            // Act	
            var actualResult = listManager.AddCustomField(
               _ECNAccesskey,
               listId,
               customFieldName,
               customFieldDesc,
               isPublic);

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.ShouldContain(excMsg);
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void AddCustomField_CustomerIsNotNull_LogErrorAndReturnErrorResponse()
        {
            // Arrange
            var exceptionLogged = false;
            var excMsg = "CUSTOM FIELD ALREADY EXISTS FOR GROUP";
            ShimAPILogging.UpdateLogInt32NullableOfInt32 = (id, logid) =>
            {
                exceptionLogged = true;
            };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
               (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
               {
                   return 1;
               };
            ShimAPILogging.InsertAPILogging = (log) => 1;
            BusinessLogicFakes::ShimUser.AllInstances.LogInGuidBoolean = (obj, id, include) =>
            {
                return new User
                {
                    DefaultClientID = 1,
                    CustomerID = 1,
                    UserID = 1
                };
            };
            ShimCustomer.GetByClientIDInt32Boolean = (id, child) => 
            {
                return new EntitiesAccounts::Customer
                {
                    CustomerID = 1
                };
            };
            ShimGroupDataFields.ExistsStringNullableOfInt32Int32Int32 =
                (name, fId, gId, custId) => true;
            var listManager = new ListManager();
            var listId = 1;
            var customFieldName = "customFieldName";
            var customFieldDesc = "customFieldDesc";
            var isPublic = "True";

            // Act	
            var actualResult = listManager.AddCustomField(
               _ECNAccesskey,
               listId,
               customFieldName,
               customFieldDesc,
               isPublic);

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.ShouldContain(excMsg);
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void AddCustomField_CustomerIsNull_LogErrorAndReturnErrorResponse()
        {
            // Arrange
            var exceptionLogged = false;
            ShimAPILogging.UpdateLogInt32NullableOfInt32 = (id, logid) =>
            {
                exceptionLogged = true;
            };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
               (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
               {
                   return 1;
               };
            ShimAPILogging.InsertAPILogging = (log) => 1;
            BusinessLogicFakes::ShimUser.AllInstances.LogInGuidBoolean = (obj, id, include) =>
            {
                return new User
                {
                    DefaultClientID = 1,
                    CustomerID = 1,
                    UserID = 1
                };
            };
            ShimCustomer.GetByClientIDInt32Boolean = (id, child) => null;
            ShimGroupDataFields.ExistsStringNullableOfInt32Int32Int32 =
                (name, fId, gId, custId) => true;
            var listManager = new ListManager();
            var listId = 1;
            var customFieldName = "customFieldName";
            var customFieldDesc = "customFieldDesc";
            var isPublic = "True";

            // Act	
            var actualResult = listManager.AddCustomField(
                _ECNAccesskey,
                listId,
                customFieldName,
                customFieldDesc,
                isPublic);

            // Assert
            actualResult.ShouldNotBeNull();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void AddCustomField_GroupDataFieldsNotExist_ReturnErrorResponse()
        {
            // Arrange
            var exceptionLogged = false;
            var excMsg = "CUSTOM FIELD ADDED";
            ShimAPILogging.UpdateLogInt32NullableOfInt32 = (id, logid) => { };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
               (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
               {
                   return 1;
               };
            ShimAPILogging.InsertAPILogging = (log) => 1;
            BusinessLogicFakes::ShimUser.AllInstances.LogInGuidBoolean = (obj, id, include) =>
            {
                return new User
                {
                    DefaultClientID = 1,
                    CustomerID = 1,
                    UserID = 1
                };
            };
            ShimCustomer.GetByClientIDInt32Boolean = (id, child) =>
            {
                return new EntitiesAccounts::Customer
                {
                    CustomerID = 1
                };
            };
            ShimGroupDataFields.ExistsStringNullableOfInt32Int32Int32 =
                (name, fId, gId, custId) => false;
            ShimGroupDataFields.SaveGroupDataFieldsUser = (field, user) => 1;
            var listManager = new ListManager();
            var listId = 1;
            var customFieldName = "customFieldName";
            var customFieldDesc = "customFieldDesc";
            var isPublic = "True";

            // Act	
            var actualResult = listManager.AddCustomField(
               _ECNAccesskey,
               listId,
               customFieldName,
               customFieldDesc,
               isPublic);

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.ShouldContain(excMsg);
            exceptionLogged.ShouldBeFalse();
        }
    }
}

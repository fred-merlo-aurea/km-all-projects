using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using Shouldly;
using AutoFixture;
using NUnit.Framework;
using Moq;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Salesforce.Helpers;

namespace ECN_Framework_Entities.Salesforce.Helpers
{
    [TestFixture]
    public class SFUtilitiesAdapterTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : DelegateMethod

        #region Method call test

        #region Method Call Test : SFUtilitiesAdapter => CreateQueryRequest (Return type :  WebRequest)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateQueryRequest_Method_4_Parameters_Simple_Call_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var query = Fixture.Create<string>();
            var method = Fixture.Create<SF_Utilities.Method>();
            var rt = Fixture.Create<SF_Utilities.ResponseType>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act, Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.CreateQueryRequest(accessToken, query, method, rt));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateQueryRequest_Method_4_Parameters_2_Calls_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var query = Fixture.Create<string>();
            var method = Fixture.Create<SF_Utilities.Method>();
            var rt = Fixture.Create<SF_Utilities.ResponseType>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<WebRequest> createQueryRequest = () => sFUtilitiesAdapter.CreateQueryRequest(accessToken, query, method, rt);

            // Assert
            Should.Throw<Exception>(() => createQueryRequest.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateQueryRequest_Method_With_4_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var query = Fixture.Create<string>();
            var method = Fixture.Create<SF_Utilities.Method>();
            var rt = Fixture.Create<SF_Utilities.ResponseType>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<WebRequest> createQueryRequest1 = () => sFUtilitiesAdapter.CreateQueryRequest(accessToken, query, method, rt);
            Func<WebRequest> createQueryRequest2 = () => sFUtilitiesAdapter.CreateQueryRequest(accessToken, query, method, rt);
            var target1 = createQueryRequest1.Target;
            var target2 = createQueryRequest2.Target;

            // Assert
            createQueryRequest1.ShouldNotBeNull();
            createQueryRequest2.ShouldNotBeNull();
            createQueryRequest1.ShouldNotBe(createQueryRequest2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => createQueryRequest1.Invoke());
            Should.Throw<Exception>(() => createQueryRequest2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateQueryRequest_Method_With_4_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var query = Fixture.Create<string>();
            var method = Fixture.Create<SF_Utilities.Method>();
            var rt = Fixture.Create<SF_Utilities.ResponseType>();
            Object[] parametersOfCreateQueryRequest = { accessToken, query, method, rt };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "CreateQueryRequest";

            // Act
            var createQueryRequestMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var createQueryRequestMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = createQueryRequestMethodInfo1.ReturnType;
            var returnType2 = createQueryRequestMethodInfo2.ReturnType;

            // Assert
            parametersOfCreateQueryRequest.ShouldNotBeNull();
            sFUtilitiesAdapter.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            createQueryRequestMethodInfo1.ShouldNotBeNull();
            createQueryRequestMethodInfo2.ShouldNotBeNull();
            createQueryRequestMethodInfo1.ShouldBe(createQueryRequestMethodInfo2);
            Should.Throw<Exception>(() => createQueryRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOfCreateQueryRequest));
            Should.Throw<Exception>(() => createQueryRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOfCreateQueryRequest));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateQueryRequest_Method_With_4_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var query = Fixture.Create<string>();
            var method = Fixture.Create<SF_Utilities.Method>();
            var rt = Fixture.Create<SF_Utilities.ResponseType>();
            Object[] parametersOutRanged = { accessToken, query, method, rt, null };
            Object[] parametersInDifferentNumber = { accessToken, query, method };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "CreateQueryRequest";

            // Act
            var createQueryRequestMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var createQueryRequestMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = createQueryRequestMethodInfo1.ReturnType;
            var returnType2 = createQueryRequestMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            sFUtilitiesAdapter.ShouldNotBeNull();
            createQueryRequestMethodInfo1.ShouldNotBeNull();
            createQueryRequestMethodInfo2.ShouldNotBeNull();
            createQueryRequestMethodInfo1.ShouldBe(createQueryRequestMethodInfo2);
            Should.Throw<Exception>(() => createQueryRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => createQueryRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => createQueryRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<Exception>(() => createQueryRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => createQueryRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => createQueryRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => createQueryRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => createQueryRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : SFUtilitiesAdapter => CreateUpdateRequest (Return type :  WebRequest)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateUpdateRequest_Method_5_Parameters_Simple_Call_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var json = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();
            var objectID = Fixture.Create<string>();
            var rt = Fixture.Create<SF_Utilities.ResponseType>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act, Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.CreateUpdateRequest(accessToken, json, sfObject, objectID, rt));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateUpdateRequest_Method_5_Parameters_2_Calls_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var json = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();
            var objectID = Fixture.Create<string>();
            var rt = Fixture.Create<SF_Utilities.ResponseType>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<WebRequest> createUpdateRequest = () => sFUtilitiesAdapter.CreateUpdateRequest(accessToken, json, sfObject, objectID, rt);

            // Assert
            Should.Throw<Exception>(() => createUpdateRequest.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateUpdateRequest_Method_With_5_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var json = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();
            var objectID = Fixture.Create<string>();
            var rt = Fixture.Create<SF_Utilities.ResponseType>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<WebRequest> createUpdateRequest1 = () => sFUtilitiesAdapter.CreateUpdateRequest(accessToken, json, sfObject, objectID, rt);
            Func<WebRequest> createUpdateRequest2 = () => sFUtilitiesAdapter.CreateUpdateRequest(accessToken, json, sfObject, objectID, rt);
            var target1 = createUpdateRequest1.Target;
            var target2 = createUpdateRequest2.Target;

            // Assert
            createUpdateRequest1.ShouldNotBeNull();
            createUpdateRequest2.ShouldNotBeNull();
            createUpdateRequest1.ShouldNotBe(createUpdateRequest2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => createUpdateRequest1.Invoke());
            Should.Throw<Exception>(() => createUpdateRequest2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateUpdateRequest_Method_With_5_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var json = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();
            var objectID = Fixture.Create<string>();
            var rt = Fixture.Create<SF_Utilities.ResponseType>();
            Object[] parametersOfCreateUpdateRequest = { accessToken, json, sfObject, objectID, rt };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "CreateUpdateRequest";

            // Act
            var createUpdateRequestMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var createUpdateRequestMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = createUpdateRequestMethodInfo1.ReturnType;
            var returnType2 = createUpdateRequestMethodInfo2.ReturnType;

            // Assert
            parametersOfCreateUpdateRequest.ShouldNotBeNull();
            sFUtilitiesAdapter.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            createUpdateRequestMethodInfo1.ShouldNotBeNull();
            createUpdateRequestMethodInfo2.ShouldNotBeNull();
            createUpdateRequestMethodInfo1.ShouldBe(createUpdateRequestMethodInfo2);
            Should.Throw<Exception>(() => createUpdateRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOfCreateUpdateRequest));
            Should.Throw<Exception>(() => createUpdateRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOfCreateUpdateRequest));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateUpdateRequest_Method_With_5_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var json = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();
            var objectID = Fixture.Create<string>();
            var rt = Fixture.Create<SF_Utilities.ResponseType>();
            Object[] parametersOutRanged = { accessToken, json, sfObject, objectID, rt, null };
            Object[] parametersInDifferentNumber = { accessToken, json, sfObject, objectID };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "CreateUpdateRequest";

            // Act
            var createUpdateRequestMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var createUpdateRequestMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = createUpdateRequestMethodInfo1.ReturnType;
            var returnType2 = createUpdateRequestMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            sFUtilitiesAdapter.ShouldNotBeNull();
            createUpdateRequestMethodInfo1.ShouldNotBeNull();
            createUpdateRequestMethodInfo2.ShouldNotBeNull();
            createUpdateRequestMethodInfo1.ShouldBe(createUpdateRequestMethodInfo2);
            Should.Throw<Exception>(() => createUpdateRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => createUpdateRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => createUpdateRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<Exception>(() => createUpdateRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => createUpdateRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => createUpdateRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => createUpdateRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => createUpdateRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : SFUtilitiesAdapter => GetNextURL (Return type :  StreamReader)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_GetNextURL_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
            var url = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act, Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.GetNextURL(url));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_GetNextURL_Method_1_Parameters_2_Calls_Test()
        {
            // Arrange
            var url = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<StreamReader> getNextURL = () => sFUtilitiesAdapter.GetNextURL(url);

            // Assert
            Should.Throw<Exception>(() => getNextURL.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_GetNextURL_Method_With_1_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var url = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<StreamReader> getNextURL1 = () => sFUtilitiesAdapter.GetNextURL(url);
            Func<StreamReader> getNextURL2 = () => sFUtilitiesAdapter.GetNextURL(url);
            var target1 = getNextURL1.Target;
            var target2 = getNextURL2.Target;

            // Assert
            getNextURL1.ShouldNotBeNull();
            getNextURL2.ShouldNotBeNull();
            getNextURL1.ShouldNotBe(getNextURL2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => getNextURL1.Invoke());
            Should.Throw<Exception>(() => getNextURL2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_GetNextURL_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var url = Fixture.Create<string>();
            Object[] parametersOfGetNextURL = { url };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "GetNextURL";

            // Act
            var getNextURLMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var getNextURLMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = getNextURLMethodInfo1.ReturnType;
            var returnType2 = getNextURLMethodInfo2.ReturnType;

            // Assert
            parametersOfGetNextURL.ShouldNotBeNull();
            sFUtilitiesAdapter.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getNextURLMethodInfo1.ShouldNotBeNull();
            getNextURLMethodInfo2.ShouldNotBeNull();
            getNextURLMethodInfo1.ShouldBe(getNextURLMethodInfo2);
            Should.Throw<Exception>(() => getNextURLMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOfGetNextURL));
            Should.Throw<Exception>(() => getNextURLMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOfGetNextURL));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_GetNextURL_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var url = Fixture.Create<string>();
            Object[] parametersOutRanged = { url, null };
            Object[] parametersInDifferentNumber = { };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "GetNextURL";

            // Act
            var getNextURLMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var getNextURLMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = getNextURLMethodInfo1.ReturnType;
            var returnType2 = getNextURLMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            sFUtilitiesAdapter.ShouldNotBeNull();
            getNextURLMethodInfo1.ShouldNotBeNull();
            getNextURLMethodInfo2.ShouldNotBeNull();
            getNextURLMethodInfo1.ShouldBe(getNextURLMethodInfo2);
            Should.Throw<Exception>(() => getNextURLMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => getNextURLMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => getNextURLMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<Exception>(() => getNextURLMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => getNextURLMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => getNextURLMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => getNextURLMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => getNextURLMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : SFUtilitiesAdapter => LogWebException (Return type :  void)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_LogWebException_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
            var ex = Fixture.Create<WebException>();
            var requestURL = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act, Assert
            Should.NotThrow(() => sFUtilitiesAdapter.LogWebException(ex, requestURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_LogWebException_Method_2_Parameters_2_Calls_Test()
        {
            // Arrange
            var ex = Fixture.Create<WebException>();
            var requestURL = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Action logWebException = () => sFUtilitiesAdapter.LogWebException(ex, requestURL);

            // Assert
            Should.NotThrow(() => logWebException.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_LogWebException_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var ex = Fixture.Create<WebException>();
            var requestURL = Fixture.Create<string>();
            Object[] parametersOfLogWebException = { ex, requestURL };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "LogWebException";

            // Act
            var logWebExceptionMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var logWebExceptionMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = logWebExceptionMethodInfo1.ReturnType;
            var returnType2 = logWebExceptionMethodInfo2.ReturnType;

            // Assert
            parametersOfLogWebException.ShouldNotBeNull();
            sFUtilitiesAdapter.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            logWebExceptionMethodInfo1.ShouldNotBeNull();
            logWebExceptionMethodInfo2.ShouldNotBeNull();
            logWebExceptionMethodInfo1.ShouldBe(logWebExceptionMethodInfo2);
            Should.NotThrow(() => logWebExceptionMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOfLogWebException));
            Should.NotThrow(() => logWebExceptionMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOfLogWebException));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_LogWebException_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var ex = Fixture.Create<WebException>();
            var requestURL = Fixture.Create<string>();
            Object[] parametersOutRanged = { ex, requestURL, null };
            Object[] parametersInDifferentNumber = { ex };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "LogWebException";

            // Act
            var logWebExceptionMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var logWebExceptionMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = logWebExceptionMethodInfo1.ReturnType;
            var returnType2 = logWebExceptionMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            sFUtilitiesAdapter.ShouldNotBeNull();
            logWebExceptionMethodInfo1.ShouldNotBeNull();
            logWebExceptionMethodInfo2.ShouldNotBeNull();
            logWebExceptionMethodInfo1.ShouldBe(logWebExceptionMethodInfo2);
            Should.Throw<Exception>(() => logWebExceptionMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => logWebExceptionMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => logWebExceptionMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<Exception>(() => logWebExceptionMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => logWebExceptionMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => logWebExceptionMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => logWebExceptionMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => logWebExceptionMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : SFUtilitiesAdapter => ProceedJobRequest (Return type :  string)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_ProceedJobRequest_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act, Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.ProceedJobRequest(null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_ProceedJobRequest_Method_1_Parameters_2_Calls_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<string> proceedJobRequest = () => sFUtilitiesAdapter.ProceedJobRequest(null);

            // Assert
            Should.Throw<Exception>(() => proceedJobRequest.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_ProceedJobRequest_Method_With_1_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<string> proceedJobRequest1 = () => sFUtilitiesAdapter.ProceedJobRequest(null);
            Func<string> proceedJobRequest2 = () => sFUtilitiesAdapter.ProceedJobRequest(null);
            var target1 = proceedJobRequest1.Target;
            var target2 = proceedJobRequest2.Target;

            // Assert
            proceedJobRequest1.ShouldNotBeNull();
            proceedJobRequest2.ShouldNotBeNull();
            proceedJobRequest1.ShouldNotBe(proceedJobRequest2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => proceedJobRequest1.Invoke());
            Should.Throw<Exception>(() => proceedJobRequest2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_ProceedJobRequest_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            Object[] parametersOfProceedJobRequest = { null };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "ProceedJobRequest";

            // Act
            var proceedJobRequestMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var proceedJobRequestMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = proceedJobRequestMethodInfo1.ReturnType;
            var returnType2 = proceedJobRequestMethodInfo2.ReturnType;

            // Assert
            parametersOfProceedJobRequest.ShouldNotBeNull();
            sFUtilitiesAdapter.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            proceedJobRequestMethodInfo1.ShouldNotBeNull();
            proceedJobRequestMethodInfo2.ShouldNotBeNull();
            proceedJobRequestMethodInfo1.ShouldBe(proceedJobRequestMethodInfo2);
            Should.Throw<Exception>(() => proceedJobRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOfProceedJobRequest));
            Should.Throw<Exception>(() => proceedJobRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOfProceedJobRequest));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_ProceedJobRequest_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            Object[] parametersOutRanged = { null, null };
            Object[] parametersInDifferentNumber = { };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "ProceedJobRequest";

            // Act
            var proceedJobRequestMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var proceedJobRequestMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = proceedJobRequestMethodInfo1.ReturnType;
            var returnType2 = proceedJobRequestMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            sFUtilitiesAdapter.ShouldNotBeNull();
            proceedJobRequestMethodInfo1.ShouldNotBeNull();
            proceedJobRequestMethodInfo2.ShouldNotBeNull();
            proceedJobRequestMethodInfo1.ShouldBe(proceedJobRequestMethodInfo2);
            Should.Throw<Exception>(() => proceedJobRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => proceedJobRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => proceedJobRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<Exception>(() => proceedJobRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => proceedJobRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => proceedJobRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => proceedJobRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => proceedJobRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : SFUtilitiesAdapter => Update (Return type :  bool)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_Update_Method_4_Parameters_Simple_Call_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var obj = Fixture.Create<SalesForceBase>();
            var objectType = Fixture.Create<SF_Utilities.SFObject>();
            var id = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act, Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.Update(accessToken, obj, objectType, id));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_Update_Method_4_Parameters_2_Calls_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var obj = Fixture.Create<SalesForceBase>();
            var objectType = Fixture.Create<SF_Utilities.SFObject>();
            var id = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<bool> update = () => sFUtilitiesAdapter.Update(accessToken, obj, objectType, id);

            // Assert
            Should.Throw<Exception>(() => update.Invoke());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_Update_Method_With_4_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var obj = Fixture.Create<SalesForceBase>();
            var objectType = Fixture.Create<SF_Utilities.SFObject>();
            var id = Fixture.Create<string>();
            Object[] parametersOutRanged = { accessToken, obj, objectType, id, null };
            Object[] parametersInDifferentNumber = { accessToken, obj, objectType };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "Update";

            // Act
            var updateMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var updateMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = updateMethodInfo1.ReturnType;
            var returnType2 = updateMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            sFUtilitiesAdapter.ShouldNotBeNull();
            updateMethodInfo1.ShouldNotBeNull();
            updateMethodInfo2.ShouldNotBeNull();
            updateMethodInfo1.ShouldBe(updateMethodInfo2);
            Should.Throw<Exception>(() => updateMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => updateMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => updateMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<Exception>(() => updateMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => updateMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => updateMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => updateMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => updateMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : SFUtilitiesAdapter => SafeExecute (Return type :  dynamic)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_SafeExecute_Static_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
            var func = Fixture.Create<Func<dynamic>>();
            var onException = Fixture.Create<Action<Exception>>();

            // Act, Assert
            Should.Throw<Exception>(() => SFUtilitiesAdapter.SafeExecute(func, onException));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_SafeExecute_Static_Method_2_Parameters_2_Calls_Test()
        {
            // Arrange
            var func = Fixture.Create<Func<dynamic>>();
            var onException = Fixture.Create<Action<Exception>>();

            // Act
            Func<dynamic> safeExecute = () => SFUtilitiesAdapter.SafeExecute(func, onException);

            // Assert
            Should.Throw<Exception>(() => safeExecute.Invoke());
            Should.Throw<Exception>(() => SFUtilitiesAdapter.SafeExecute(func, onException));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_SafeExecute_Static_Method_With_2_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var func = Fixture.Create<Func<dynamic>>();
            var onException = Fixture.Create<Action<Exception>>();

            // Act
            Func<dynamic> safeExecute1 = () => SFUtilitiesAdapter.SafeExecute(func, onException);
            Func<dynamic> safeExecute2 = () => SFUtilitiesAdapter.SafeExecute(func, onException);
            var target1 = safeExecute1.Target;
            var target2 = safeExecute2.Target;

            // Assert
            safeExecute1.ShouldNotBeNull();
            safeExecute2.ShouldNotBeNull();
            safeExecute1.ShouldNotBe(safeExecute2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => safeExecute1.Invoke());
            Should.Throw<Exception>(() => safeExecute2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_SafeExecute_Static_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var func = Fixture.Create<Func<dynamic>>();
            var onException = Fixture.Create<Action<Exception>>();
            Object[] parametersOutRanged = { func, onException, null };
            Object[] parametersInDifferentNumber = { func };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "SafeExecute";

            // Act
            var safeExecuteMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var safeExecuteMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = safeExecuteMethodInfo1.ReturnType;
            var returnType2 = safeExecuteMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            sFUtilitiesAdapter.ShouldNotBeNull();
            safeExecuteMethodInfo1.ShouldNotBeNull();
            safeExecuteMethodInfo2.ShouldNotBeNull();
            safeExecuteMethodInfo1.ShouldBe(safeExecuteMethodInfo2);
            Should.Throw<Exception>(() => safeExecuteMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => safeExecuteMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => safeExecuteMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<Exception>(() => safeExecuteMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : SFUtilitiesAdapter => CreateWebRequest (Return type :  WebRequest)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateWebRequest_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
            var url = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act, Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.CreateWebRequest(url));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateWebRequest_Method_1_Parameters_2_Calls_Test()
        {
            // Arrange
            var url = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<WebRequest> createWebRequest = () => sFUtilitiesAdapter.CreateWebRequest(url);

            // Assert
            Should.Throw<Exception>(() => createWebRequest.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateWebRequest_Method_With_1_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var url = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<WebRequest> createWebRequest1 = () => sFUtilitiesAdapter.CreateWebRequest(url);
            Func<WebRequest> createWebRequest2 = () => sFUtilitiesAdapter.CreateWebRequest(url);
            var target1 = createWebRequest1.Target;
            var target2 = createWebRequest2.Target;

            // Assert
            createWebRequest1.ShouldNotBeNull();
            createWebRequest2.ShouldNotBeNull();
            createWebRequest1.ShouldNotBe(createWebRequest2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => createWebRequest1.Invoke());
            Should.Throw<Exception>(() => createWebRequest2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateWebRequest_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var url = Fixture.Create<string>();
            Object[] parametersOfCreateWebRequest = { url };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "CreateWebRequest";

            // Act
            var createWebRequestMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var createWebRequestMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = createWebRequestMethodInfo1.ReturnType;
            var returnType2 = createWebRequestMethodInfo2.ReturnType;

            // Assert
            parametersOfCreateWebRequest.ShouldNotBeNull();
            sFUtilitiesAdapter.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            createWebRequestMethodInfo1.ShouldNotBeNull();
            createWebRequestMethodInfo2.ShouldNotBeNull();
            createWebRequestMethodInfo1.ShouldBe(createWebRequestMethodInfo2);
            Should.Throw<Exception>(() => createWebRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOfCreateWebRequest));
            Should.Throw<Exception>(() => createWebRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOfCreateWebRequest));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateWebRequest_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var url = Fixture.Create<string>();
            Object[] parametersOutRanged = { url, null };
            Object[] parametersInDifferentNumber = { };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "CreateWebRequest";

            // Act
            var createWebRequestMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var createWebRequestMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = createWebRequestMethodInfo1.ReturnType;
            var returnType2 = createWebRequestMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            sFUtilitiesAdapter.ShouldNotBeNull();
            createWebRequestMethodInfo1.ShouldNotBeNull();
            createWebRequestMethodInfo2.ShouldNotBeNull();
            createWebRequestMethodInfo1.ShouldBe(createWebRequestMethodInfo2);
            Should.Throw<Exception>(() => createWebRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => createWebRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => createWebRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<Exception>(() => createWebRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => createWebRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => createWebRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => createWebRequestMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => createWebRequestMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : SFUtilitiesAdapter => ReadToken (Return type :  dynamic)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_ReadToken_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act, Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.ReadToken<dynamic>(null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_ReadToken_Method_1_Parameters_2_Calls_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<dynamic> readToken = () => sFUtilitiesAdapter.ReadToken<dynamic>(null);

            // Assert
            Should.Throw<Exception>(() => readToken.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_ReadToken_Method_With_1_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<dynamic> readToken1 = () => sFUtilitiesAdapter.ReadToken<dynamic>(null);
            Func<dynamic> readToken2 = () => sFUtilitiesAdapter.ReadToken<dynamic>(null);
            var target1 = readToken1.Target;
            var target2 = readToken2.Target;

            // Assert
            readToken1.ShouldNotBeNull();
            readToken2.ShouldNotBeNull();
            readToken1.ShouldNotBe(readToken2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => readToken1.Invoke());
            Should.Throw<Exception>(() => readToken2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_ReadToken_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            Object[] parametersOutRanged = { null, null };
            Object[] parametersInDifferentNumber = { };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "ReadToken";

            // Act
            var readTokenMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var readTokenMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = readTokenMethodInfo1.ReturnType;
            var returnType2 = readTokenMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            sFUtilitiesAdapter.ShouldNotBeNull();
            readTokenMethodInfo1.ShouldNotBeNull();
            readTokenMethodInfo2.ShouldNotBeNull();
            readTokenMethodInfo1.ShouldBe(readTokenMethodInfo2);
            Should.Throw<Exception>(() => readTokenMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => readTokenMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => readTokenMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<Exception>(() => readTokenMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : SFUtilitiesAdapter => WriteToLog (Return type :  void)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_WriteToLog_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
            var text = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act, Assert
            Should.NotThrow(() => sFUtilitiesAdapter.WriteToLog(text));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_WriteToLog_Method_1_Parameters_2_Calls_Test()
        {
            // Arrange
            var text = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Action writeToLog = () => sFUtilitiesAdapter.WriteToLog(text);

            // Assert
            Should.NotThrow(() => writeToLog.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_WriteToLog_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var text = Fixture.Create<string>();
            Object[] parametersOfWriteToLog = { text };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "WriteToLog";

            // Act
            var writeToLogMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var writeToLogMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = writeToLogMethodInfo1.ReturnType;
            var returnType2 = writeToLogMethodInfo2.ReturnType;

            // Assert
            parametersOfWriteToLog.ShouldNotBeNull();
            sFUtilitiesAdapter.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            writeToLogMethodInfo1.ShouldNotBeNull();
            writeToLogMethodInfo2.ShouldNotBeNull();
            writeToLogMethodInfo1.ShouldBe(writeToLogMethodInfo2);
            Should.NotThrow(() => writeToLogMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOfWriteToLog));
            Should.NotThrow(() => writeToLogMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOfWriteToLog));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_WriteToLog_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var text = Fixture.Create<string>();
            Object[] parametersOutRanged = { text, null };
            Object[] parametersInDifferentNumber = { };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "WriteToLog";

            // Act
            var writeToLogMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var writeToLogMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = writeToLogMethodInfo1.ReturnType;
            var returnType2 = writeToLogMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            sFUtilitiesAdapter.ShouldNotBeNull();
            writeToLogMethodInfo1.ShouldNotBeNull();
            writeToLogMethodInfo2.ShouldNotBeNull();
            writeToLogMethodInfo1.ShouldBe(writeToLogMethodInfo2);
            Should.Throw<Exception>(() => writeToLogMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => writeToLogMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => writeToLogMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<Exception>(() => writeToLogMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => writeToLogMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => writeToLogMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => writeToLogMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => writeToLogMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : SFUtilitiesAdapter => GetBatchResults (Return type :  WebRequest)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_GetBatchResults_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act, Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.GetBatchResults(accessToken, jobId, batchId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_GetBatchResults_Method_3_Parameters_2_Calls_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<WebRequest> getBatchResults = () => sFUtilitiesAdapter.GetBatchResults(accessToken, jobId, batchId);

            // Assert
            Should.Throw<Exception>(() => getBatchResults.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_GetBatchResults_Method_With_3_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<WebRequest> getBatchResults1 = () => sFUtilitiesAdapter.GetBatchResults(accessToken, jobId, batchId);
            Func<WebRequest> getBatchResults2 = () => sFUtilitiesAdapter.GetBatchResults(accessToken, jobId, batchId);
            var target1 = getBatchResults1.Target;
            var target2 = getBatchResults2.Target;

            // Assert
            getBatchResults1.ShouldNotBeNull();
            getBatchResults2.ShouldNotBeNull();
            getBatchResults1.ShouldNotBe(getBatchResults2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => getBatchResults1.Invoke());
            Should.Throw<Exception>(() => getBatchResults2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_GetBatchResults_Method_With_3_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();
            Object[] parametersOfGetBatchResults = { accessToken, jobId, batchId };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "GetBatchResults";

            // Act
            var getBatchResultsMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var getBatchResultsMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = getBatchResultsMethodInfo1.ReturnType;
            var returnType2 = getBatchResultsMethodInfo2.ReturnType;

            // Assert
            parametersOfGetBatchResults.ShouldNotBeNull();
            sFUtilitiesAdapter.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getBatchResultsMethodInfo1.ShouldNotBeNull();
            getBatchResultsMethodInfo2.ShouldNotBeNull();
            getBatchResultsMethodInfo1.ShouldBe(getBatchResultsMethodInfo2);
            Should.Throw<Exception>(() => getBatchResultsMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOfGetBatchResults));
            Should.Throw<Exception>(() => getBatchResultsMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOfGetBatchResults));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_GetBatchResults_Method_With_3_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();
            Object[] parametersOutRanged = { accessToken, jobId, batchId, null };
            Object[] parametersInDifferentNumber = { accessToken, jobId };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "GetBatchResults";

            // Act
            var getBatchResultsMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var getBatchResultsMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = getBatchResultsMethodInfo1.ReturnType;
            var returnType2 = getBatchResultsMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            sFUtilitiesAdapter.ShouldNotBeNull();
            getBatchResultsMethodInfo1.ShouldNotBeNull();
            getBatchResultsMethodInfo2.ShouldNotBeNull();
            getBatchResultsMethodInfo1.ShouldBe(getBatchResultsMethodInfo2);
            Should.Throw<Exception>(() => getBatchResultsMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => getBatchResultsMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => getBatchResultsMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<Exception>(() => getBatchResultsMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => getBatchResultsMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => getBatchResultsMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => getBatchResultsMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => getBatchResultsMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : SFUtilitiesAdapter => CreateNewJob (Return type :  WebRequest)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateNewJob_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();
            var operation = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act, Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.CreateNewJob(accessToken, sfObject, operation));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateNewJob_Method_3_Parameters_2_Calls_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();
            var operation = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<WebRequest> createNewJob = () => sFUtilitiesAdapter.CreateNewJob(accessToken, sfObject, operation);

            // Assert
            Should.Throw<Exception>(() => createNewJob.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateNewJob_Method_With_3_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();
            var operation = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<WebRequest> createNewJob1 = () => sFUtilitiesAdapter.CreateNewJob(accessToken, sfObject, operation);
            Func<WebRequest> createNewJob2 = () => sFUtilitiesAdapter.CreateNewJob(accessToken, sfObject, operation);

            var target1 = createNewJob1.Target;
            var target2 = createNewJob2.Target;

            // Assert
            createNewJob1.ShouldNotBeNull();
            createNewJob2.ShouldNotBeNull();
            createNewJob1.ShouldNotBe(createNewJob2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => createNewJob1.Invoke());
            Should.Throw<Exception>(() => createNewJob2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateNewJob_Method_With_3_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();
            var operation = Fixture.Create<string>();
            Object[] parametersOfCreateNewJob = { accessToken, sfObject, operation };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "CreateNewJob";

            // Act
            var createNewJobMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var createNewJobMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = createNewJobMethodInfo1.ReturnType;
            var returnType2 = createNewJobMethodInfo2.ReturnType;

            // Assert
            parametersOfCreateNewJob.ShouldNotBeNull();
            sFUtilitiesAdapter.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            createNewJobMethodInfo1.ShouldNotBeNull();
            createNewJobMethodInfo2.ShouldNotBeNull();
            createNewJobMethodInfo1.ShouldBe(createNewJobMethodInfo2);
            Should.Throw<Exception>(() => createNewJobMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOfCreateNewJob));
            Should.Throw<Exception>(() => createNewJobMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOfCreateNewJob));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CreateNewJob_Method_With_3_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();
            var operation = Fixture.Create<string>();
            Object[] parametersOutRanged = { accessToken, sfObject, operation, null };
            Object[] parametersInDifferentNumber = { accessToken, sfObject };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "CreateNewJob";

            // Act
            var createNewJobMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var createNewJobMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = createNewJobMethodInfo1.ReturnType;
            var returnType2 = createNewJobMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            sFUtilitiesAdapter.ShouldNotBeNull();
            createNewJobMethodInfo1.ShouldNotBeNull();
            createNewJobMethodInfo2.ShouldNotBeNull();
            createNewJobMethodInfo1.ShouldBe(createNewJobMethodInfo2);
            Should.Throw<Exception>(() => createNewJobMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => createNewJobMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => createNewJobMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<Exception>(() => createNewJobMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => createNewJobMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => createNewJobMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => createNewJobMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => createNewJobMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : SFUtilitiesAdapter => GetBatchState (Return type :  WebRequest)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_GetBatchState_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act, Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.GetBatchState(accessToken, jobId, batchId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_GetBatchState_Method_3_Parameters_2_Calls_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<WebRequest> getBatchState = () => sFUtilitiesAdapter.GetBatchState(accessToken, jobId, batchId);

            // Assert
            Should.Throw<Exception>(() => getBatchState.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_GetBatchState_Method_With_3_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<WebRequest> getBatchState1 = () => sFUtilitiesAdapter.GetBatchState(accessToken, jobId, batchId);
            Func<WebRequest> getBatchState2 = () => sFUtilitiesAdapter.GetBatchState(accessToken, jobId, batchId);
            var target1 = getBatchState1.Target;
            var target2 = getBatchState2.Target;

            // Assert
            getBatchState1.ShouldNotBeNull();
            getBatchState2.ShouldNotBeNull();
            getBatchState1.ShouldNotBe(getBatchState2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => getBatchState1.Invoke());
            Should.Throw<Exception>(() => getBatchState2.Invoke());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_GetBatchState_Method_With_3_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();
            Object[] parametersOfGetBatchState = { accessToken, jobId, batchId };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "GetBatchState";

            // Act
            var getBatchStateMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var getBatchStateMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = getBatchStateMethodInfo1.ReturnType;
            var returnType2 = getBatchStateMethodInfo2.ReturnType;

            // Assert
            parametersOfGetBatchState.ShouldNotBeNull();
            sFUtilitiesAdapter.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getBatchStateMethodInfo1.ShouldNotBeNull();
            getBatchStateMethodInfo2.ShouldNotBeNull();
            getBatchStateMethodInfo1.ShouldBe(getBatchStateMethodInfo2);
            Should.Throw<Exception>(() => getBatchStateMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOfGetBatchState));
            Should.Throw<Exception>(() => getBatchStateMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOfGetBatchState));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_GetBatchState_Method_With_3_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();
            Object[] parametersOutRanged = { accessToken, jobId, batchId, null };
            Object[] parametersInDifferentNumber = { accessToken, jobId };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "GetBatchState";

            // Act
            var getBatchStateMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var getBatchStateMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = getBatchStateMethodInfo1.ReturnType;
            var returnType2 = getBatchStateMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            sFUtilitiesAdapter.ShouldNotBeNull();
            getBatchStateMethodInfo1.ShouldNotBeNull();
            getBatchStateMethodInfo2.ShouldNotBeNull();
            getBatchStateMethodInfo1.ShouldBe(getBatchStateMethodInfo2);
            Should.Throw<Exception>(() => getBatchStateMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => getBatchStateMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => getBatchStateMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<Exception>(() => getBatchStateMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => getBatchStateMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => getBatchStateMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => getBatchStateMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => getBatchStateMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : SFUtilitiesAdapter => AddBatchToJob (Return type :  WebRequest)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_AddBatchToJob_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var xmlString = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act, Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.AddBatchToJob(accessToken, jobId, xmlString));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_AddBatchToJob_Method_3_Parameters_2_Calls_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var xmlString = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<WebRequest> addBatchToJob = () => sFUtilitiesAdapter.AddBatchToJob(accessToken, jobId, xmlString);

            // Assert
            Should.Throw<Exception>(() => addBatchToJob.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_AddBatchToJob_Method_With_3_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var xmlString = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<WebRequest> addBatchToJob1 = () => sFUtilitiesAdapter.AddBatchToJob(accessToken, jobId, xmlString);
            Func<WebRequest> addBatchToJob2 = () => sFUtilitiesAdapter.AddBatchToJob(accessToken, jobId, xmlString);
            var target1 = addBatchToJob1.Target;
            var target2 = addBatchToJob2.Target;

            // Assert
            addBatchToJob1.ShouldNotBeNull();
            addBatchToJob2.ShouldNotBeNull();
            addBatchToJob1.ShouldNotBe(addBatchToJob2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => addBatchToJob1.Invoke());
            Should.Throw<Exception>(() => addBatchToJob2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_AddBatchToJob_Method_With_3_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var xmlString = Fixture.Create<string>();
            Object[] parametersOfAddBatchToJob = { accessToken, jobId, xmlString };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "AddBatchToJob";

            // Act
            var addBatchToJobMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var addBatchToJobMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = addBatchToJobMethodInfo1.ReturnType;
            var returnType2 = addBatchToJobMethodInfo2.ReturnType;

            // Assert
            parametersOfAddBatchToJob.ShouldNotBeNull();
            sFUtilitiesAdapter.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            addBatchToJobMethodInfo1.ShouldNotBeNull();
            addBatchToJobMethodInfo2.ShouldNotBeNull();
            addBatchToJobMethodInfo1.ShouldBe(addBatchToJobMethodInfo2);
            Should.Throw<Exception>(() => addBatchToJobMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOfAddBatchToJob));
            Should.Throw<Exception>(() => addBatchToJobMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOfAddBatchToJob));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_AddBatchToJob_Method_With_3_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var xmlString = Fixture.Create<string>();
            Object[] parametersOutRanged = { accessToken, jobId, xmlString, null };
            Object[] parametersInDifferentNumber = { accessToken, jobId };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "AddBatchToJob";

            // Act
            var addBatchToJobMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var addBatchToJobMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = addBatchToJobMethodInfo1.ReturnType;
            var returnType2 = addBatchToJobMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            sFUtilitiesAdapter.ShouldNotBeNull();
            addBatchToJobMethodInfo1.ShouldNotBeNull();
            addBatchToJobMethodInfo2.ShouldNotBeNull();
            addBatchToJobMethodInfo1.ShouldBe(addBatchToJobMethodInfo2);
            Should.Throw<Exception>(() => addBatchToJobMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => addBatchToJobMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => addBatchToJobMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<Exception>(() => addBatchToJobMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => addBatchToJobMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => addBatchToJobMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => addBatchToJobMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => addBatchToJobMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : SFUtilitiesAdapter => CloseJob (Return type :  WebRequest)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CloseJob_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act, Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.CloseJob(accessToken, jobId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CloseJob_Method_2_Parameters_2_Calls_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<WebRequest> closeJob = () => sFUtilitiesAdapter.CloseJob(accessToken, jobId);

            // Assert
            Should.Throw<Exception>(() => closeJob.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CloseJob_Method_With_2_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act
            Func<WebRequest> closeJob1 = () => sFUtilitiesAdapter.CloseJob(accessToken, jobId);
            Func<WebRequest> closeJob2 = () => sFUtilitiesAdapter.CloseJob(accessToken, jobId);
            var target1 = closeJob1.Target;
            var target2 = closeJob2.Target;

            // Assert
            closeJob1.ShouldNotBeNull();
            closeJob2.ShouldNotBeNull();
            closeJob1.ShouldNotBe(closeJob2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => closeJob1.Invoke());
            Should.Throw<Exception>(() => closeJob2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CloseJob_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            Object[] parametersOfCloseJob = { accessToken, jobId };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "CloseJob";

            // Act
            var closeJobMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var closeJobMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = closeJobMethodInfo1.ReturnType;
            var returnType2 = closeJobMethodInfo2.ReturnType;

            // Assert
            parametersOfCloseJob.ShouldNotBeNull();
            sFUtilitiesAdapter.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            closeJobMethodInfo1.ShouldNotBeNull();
            closeJobMethodInfo2.ShouldNotBeNull();
            closeJobMethodInfo1.ShouldBe(closeJobMethodInfo2);
            Should.Throw<Exception>(() => closeJobMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOfCloseJob));
            Should.Throw<Exception>(() => closeJobMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOfCloseJob));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void SFUtilitiesAdapter_CloseJob_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            Object[] parametersOutRanged = { accessToken, jobId, null };
            Object[] parametersInDifferentNumber = { accessToken };
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var methodName = "CloseJob";

            // Act
            var closeJobMethodInfo1 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var closeJobMethodInfo2 = sFUtilitiesAdapter.GetType().GetMethod(methodName);
            var returnType1 = closeJobMethodInfo1.ReturnType;
            var returnType2 = closeJobMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            sFUtilitiesAdapter.ShouldNotBeNull();
            closeJobMethodInfo1.ShouldNotBeNull();
            closeJobMethodInfo2.ShouldNotBeNull();
            closeJobMethodInfo1.ShouldBe(closeJobMethodInfo2);
            Should.Throw<Exception>(() => closeJobMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => closeJobMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<Exception>(() => closeJobMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<Exception>(() => closeJobMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => closeJobMethodInfo1.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => closeJobMethodInfo2.Invoke(sFUtilitiesAdapter, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => closeJobMethodInfo1.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => closeJobMethodInfo2.Invoke(sFUtilitiesAdapter, parametersInDifferentNumber));
        }

        #endregion

        #endregion


        #endregion
        #region Category : Constructor

        #region DelegateLikeMethods

        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_GetNextURL_Non_Static_DelegateMethod_With_Parameter_1_Throw_Exception_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var url = Fixture.Create<string>();

            // Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.GetNextURL(url));
            Should.Throw<Exception>(() => SF_Utilities.GetNextURL(url));
        }

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_GetNextURL_Non_Static_DelegateMethod_With_Parameter_1_No_Exception_Thrown_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var url = Fixture.Create<string>();

            // Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.GetNextURL(url));
            Should.Throw<Exception>(() => SF_Utilities.GetNextURL(url));
        }

        #endregion

        #region Non-Static DelegateMethod : Return type void test (SFUtilitiesAdapter => GetNextURL)

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_GetNextURL_Non_Static_DelegateMethod_With_1_Parameters_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var url = Fixture.Create<string>();

            // Act	
            Action result1 = () => sFUtilitiesAdapter.GetNextURL(url);
            Action result2 = () => SF_Utilities.GetNextURL(url);
            var target1 = result1.Target;
            var target2 = result2.Target;

            // Assert
            result1.ShouldNotBeNull();
            result2.ShouldNotBeNull();
            target1.ShouldBe(target2);
            result2.ShouldNotBe(result1);
            Should.Throw<Exception>(() => result1.Invoke());
            Should.Throw<Exception>(() => result2.Invoke());
        }

        #endregion


        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_LogWebException_Non_Static_DelegateMethod_With_Parameter_2_Throw_Exception_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var ex = Fixture.Create<WebException>();
            var requestURL = Fixture.Create<string>();

            // Act , Assert
            Should.NotThrow(() => sFUtilitiesAdapter.LogWebException(ex, requestURL));
            Should.NotThrow(() => SF_Utilities.LogWebException(ex, requestURL));
        }

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_LogWebException_Non_Static_DelegateMethod_With_Parameter_2_No_Exception_Thrown_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var ex = Fixture.Create<WebException>();
            var requestURL = Fixture.Create<string>();

            // Act , Assert
            Should.NotThrow(() => sFUtilitiesAdapter.LogWebException(ex, requestURL));
            Should.NotThrow(() => SF_Utilities.LogWebException(ex, requestURL));
        }

        #endregion

        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_ProceedJobRequest_Non_Static_DelegateMethod_With_Parameter_1_Throw_Exception_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.ProceedJobRequest(null));
        }

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_ProceedJobRequest_Non_Static_DelegateMethod_With_Parameter_1_No_Exception_Thrown_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();

            // Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.ProceedJobRequest(null));
        }

        #endregion

        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_CreateWebRequest_Non_Static_DelegateMethod_With_Parameter_1_Throw_Exception_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var url = Fixture.Create<string>();

            // Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.CreateWebRequest(url));
            Should.Throw<Exception>(() => WebRequest.Create(url));
        }

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_CreateWebRequest_Non_Static_DelegateMethod_With_Parameter_1_No_Exception_Thrown_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var url = Fixture.Create<string>();

            // Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.CreateWebRequest(url));
        }

        #endregion

        #region Non-Static DelegateMethod : Return type void test (SFUtilitiesAdapter => CreateWebRequest)

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_CreateWebRequest_Non_Static_DelegateMethod_With_1_Parameters_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var url = Fixture.Create<string>();

            // Act	
            Action result1 = () => sFUtilitiesAdapter.CreateWebRequest(url);
            Action result2 = () => WebRequest.Create(url);
            var target1 = result1.Target;
            var target2 = result2.Target;

            // Assert
            result1.ShouldNotBeNull();
            result2.ShouldNotBeNull();
            target1.ShouldBe(target2);
            result2.ShouldNotBe(result1);
            Should.Throw<Exception>(() => result1.Invoke());
            Should.Throw<Exception>(() => result2.Invoke());
        }

        #endregion


        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_WriteToLog_Non_Static_DelegateMethod_With_Parameter_1_No_Exception_Thrown_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var text = Fixture.Create<string>();

            // Act , Assert
            Should.NotThrow(() => sFUtilitiesAdapter.WriteToLog(text));
            Should.NotThrow(() => SF_Utilities.WriteToLog(text));
        }

        #endregion

        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_GetBatchResults_Non_Static_DelegateMethod_With_Parameter_3_Throw_Exception_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();

            // Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.GetBatchResults(accessToken, jobId, batchId));
            Should.Throw<Exception>(() => SF_Utilities.GetBatchResults(accessToken, jobId, batchId));
        }

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_GetBatchResults_Non_Static_DelegateMethod_With_Parameter_3_No_Exception_Thrown_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();

            // Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.GetBatchResults(accessToken, jobId, batchId));
            Should.Throw<Exception>(() => SF_Utilities.GetBatchResults(accessToken, jobId, batchId));
        }

        #endregion

        #region Non-Static DelegateMethod : Return type void test (SFUtilitiesAdapter => GetBatchResults)

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_GetBatchResults_Non_Static_DelegateMethod_With_3_Parameters_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();

            // Act	
            Action result1 = () => sFUtilitiesAdapter.GetBatchResults(accessToken, jobId, batchId);
            Action result2 = () => SF_Utilities.GetBatchResults(accessToken, jobId, batchId);
            var target1 = result1.Target;
            var target2 = result2.Target;

            // Assert
            result1.ShouldNotBeNull();
            result2.ShouldNotBeNull();
            target1.ShouldBe(target2);
            result2.ShouldNotBe(result1);
            Should.Throw<Exception>(() => result1.Invoke());
            Should.Throw<Exception>(() => result2.Invoke());
        }

        #endregion


        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_CreateNewJob_Non_Static_DelegateMethod_With_Parameter_3_Throw_Exception_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var accessToken = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();
            var operation = Fixture.Create<string>();

            // Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.CreateNewJob(accessToken, sfObject, operation));
            Should.Throw<Exception>(() => SF_Utilities.CreateNewJob(accessToken, sfObject, operation));
        }

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_CreateNewJob_Non_Static_DelegateMethod_With_Parameter_3_No_Exception_Thrown_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var accessToken = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();
            var operation = Fixture.Create<string>();

            // Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.CreateNewJob(accessToken, sfObject, operation));
            Should.Throw<Exception>(() => SF_Utilities.CreateNewJob(accessToken, sfObject, operation));
        }

        #endregion

        #region Non-Static DelegateMethod : Return type void test (SFUtilitiesAdapter => CreateNewJob)

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_CreateNewJob_Non_Static_DelegateMethod_With_3_Parameters_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var accessToken = Fixture.Create<string>();
            var sfObject = Fixture.Create<SF_Utilities.SFObject>();
            var operation = Fixture.Create<string>();

            // Act	
            Action result1 = () => sFUtilitiesAdapter.CreateNewJob(accessToken, sfObject, operation);
            Action result2 = () => SF_Utilities.CreateNewJob(accessToken, sfObject, operation);
            var target1 = result1.Target;
            var target2 = result2.Target;

            // Assert
            result1.ShouldNotBeNull();
            result2.ShouldNotBeNull();
            target1.ShouldBe(target2);
            result2.ShouldNotBe(result1);
            Should.Throw<Exception>(() => result1.Invoke());
            Should.Throw<Exception>(() => result2.Invoke());
        }

        #endregion


        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_GetBatchState_Non_Static_DelegateMethod_With_Parameter_3_Throw_Exception_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();

            // Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.GetBatchState(accessToken, jobId, batchId));
            Should.Throw<Exception>(() => SF_Utilities.GetBatchState(accessToken, jobId, batchId));
        }

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_GetBatchState_Non_Static_DelegateMethod_With_Parameter_3_No_Exception_Thrown_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();

            // Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.GetBatchState(accessToken, jobId, batchId));
            Should.Throw<Exception>(() => SF_Utilities.GetBatchState(accessToken, jobId, batchId));
        }

        #endregion

        #region Non-Static DelegateMethod : Return type void test (SFUtilitiesAdapter => GetBatchState)

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_GetBatchState_Non_Static_DelegateMethod_With_3_Parameters_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var batchId = Fixture.Create<string>();

            // Act	
            Action result1 = () => sFUtilitiesAdapter.GetBatchState(accessToken, jobId, batchId);
            Action result2 = () => SF_Utilities.GetBatchState(accessToken, jobId, batchId);
            var target1 = result1.Target;
            var target2 = result2.Target;

            // Assert
            result1.ShouldNotBeNull();
            result2.ShouldNotBeNull();
            target1.ShouldBe(target2);
            result2.ShouldNotBe(result1);
            Should.Throw<Exception>(() => result1.Invoke());
            Should.Throw<Exception>(() => result2.Invoke());
        }

        #endregion


        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_AddBatchToJob_Non_Static_DelegateMethod_With_Parameter_3_Throw_Exception_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var xmlString = Fixture.Create<string>();

            // Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.AddBatchToJob(accessToken, jobId, xmlString));
            Should.Throw<Exception>(() => SF_Utilities.AddBatchToJob(accessToken, jobId, xmlString));
        }

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_AddBatchToJob_Non_Static_DelegateMethod_With_Parameter_3_No_Exception_Thrown_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var xmlString = Fixture.Create<string>();

            // Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.AddBatchToJob(accessToken, jobId, xmlString));
            Should.Throw<Exception>(() => SF_Utilities.AddBatchToJob(accessToken, jobId, xmlString));
        }

        #endregion

        #region Non-Static DelegateMethod : Return type void test (SFUtilitiesAdapter => AddBatchToJob)

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_AddBatchToJob_Non_Static_DelegateMethod_With_3_Parameters_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();
            var xmlString = Fixture.Create<string>();

            // Act	
            Action result1 = () => sFUtilitiesAdapter.AddBatchToJob(accessToken, jobId, xmlString);
            Action result2 = () => SF_Utilities.AddBatchToJob(accessToken, jobId, xmlString);
            var target1 = result1.Target;
            var target2 = result2.Target;

            // Assert
            result1.ShouldNotBeNull();
            result2.ShouldNotBeNull();
            target1.ShouldBe(target2);
            result2.ShouldNotBe(result1);
            Should.Throw<Exception>(() => result1.Invoke());
            Should.Throw<Exception>(() => result2.Invoke());
        }

        #endregion


        #region Delegate Like Method

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_CloseJob_Non_Static_DelegateMethod_With_Parameter_2_Throw_Exception_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();

            // Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.CloseJob(accessToken, jobId));
            Should.Throw<Exception>(() => SF_Utilities.CloseJob(accessToken, jobId));
        }

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_CloseJob_Non_Static_DelegateMethod_With_Parameter_2_No_Exception_Thrown_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();

            // Act , Assert
            Should.Throw<Exception>(() => sFUtilitiesAdapter.CloseJob(accessToken, jobId));
            Should.Throw<Exception>(() => SF_Utilities.CloseJob(accessToken, jobId));
        }

        #endregion

        #region Non-Static DelegateMethod : Return type void test (SFUtilitiesAdapter => CloseJob)

        [Test]
        [Author("AUT. Bilal Shahzad")]
        [Category("AUT DelegateMethod")]
        public void SFUtilitiesAdapter_CloseJob_Non_Static_DelegateMethod_With_2_Parameters_Test()
        {
            // Arrange
            var sFUtilitiesAdapter = Fixture.Create<SFUtilitiesAdapter>();
            var accessToken = Fixture.Create<string>();
            var jobId = Fixture.Create<string>();

            // Act	
            Action result1 = () => sFUtilitiesAdapter.CloseJob(accessToken, jobId);
            Action result2 = () => SF_Utilities.CloseJob(accessToken, jobId);
            var target1 = result1.Target;
            var target2 = result2.Target;

            // Assert
            result1.ShouldNotBeNull();
            result2.ShouldNotBeNull();
            target1.ShouldBe(target2);
            result2.ShouldNotBe(result1);
            Should.Throw<Exception>(() => result1.Invoke());
            Should.Throw<Exception>(() => result2.Invoke());
        }

        #endregion


        #endregion


        #endregion
        #region Category : Constructor

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<SFUtilitiesAdapter>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion


        #endregion


        #endregion
    }
}
using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;
using Shouldly;
using AutoFixture;
using NUnit.Framework;
using Moq;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Salesforce.Helpers;
using ECN_Framework_Entities.Salesforce.Interfaces;

namespace ECN_Framework_Entities.Salesforce.Helpers
{
    [TestFixture]
    public class AuthenticationHelperTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : AnyMethodCall

        #region AnyMethodCall

        #region Method Call Test : AuthenticationHelper => InitUtilities (Return type :  void)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_InitUtilities_Static_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
            var utilities = Fixture.Create<ISFUtilities>();

            // Act, Assert
            Should.NotThrow(() => AuthenticationHelper.InitUtilities(utilities));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_InitUtilities_Static_Method_1_Parameters_2_Calls_Test()
        {
            // Arrange
            var utilities = Fixture.Create<ISFUtilities>();

            // Act
            Action initUtilities = () => AuthenticationHelper.InitUtilities(utilities);

            // Assert
            Should.NotThrow(() => initUtilities.Invoke());
            Should.NotThrow(() => AuthenticationHelper.InitUtilities(utilities));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_InitUtilities_Static_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var utilities = Fixture.Create<ISFUtilities>();
            Object[] parametersOfInitUtilities = { utilities };
            var authenticationHelper = Fixture.Create<AuthenticationHelper>();
            var methodName = "InitUtilities";

            // Act
            var initUtilitiesMethodInfo1 = authenticationHelper.GetType().GetMethod(methodName);
            var initUtilitiesMethodInfo2 = authenticationHelper.GetType().GetMethod(methodName);
            var returnType1 = initUtilitiesMethodInfo1.ReturnType;
            var returnType2 = initUtilitiesMethodInfo2.ReturnType;

            // Assert
            parametersOfInitUtilities.ShouldNotBeNull();
            authenticationHelper.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            initUtilitiesMethodInfo1.ShouldNotBeNull();
            initUtilitiesMethodInfo2.ShouldNotBeNull();
            initUtilitiesMethodInfo1.ShouldBe(initUtilitiesMethodInfo2);
            Should.NotThrow(() => initUtilitiesMethodInfo1.Invoke(authenticationHelper, parametersOfInitUtilities));
            Should.NotThrow(() => initUtilitiesMethodInfo2.Invoke(authenticationHelper, parametersOfInitUtilities));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_InitUtilities_Static_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var utilities = Fixture.Create<ISFUtilities>();
            Object[] parametersOutRanged = { utilities, null };
            Object[] parametersInDifferentNumber = { };
            var authenticationHelper = Fixture.Create<AuthenticationHelper>();
            var methodName = "InitUtilities";

            // Act
            var initUtilitiesMethodInfo1 = authenticationHelper.GetType().GetMethod(methodName);
            var initUtilitiesMethodInfo2 = authenticationHelper.GetType().GetMethod(methodName);
            var returnType1 = initUtilitiesMethodInfo1.ReturnType;
            var returnType2 = initUtilitiesMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            authenticationHelper.ShouldNotBeNull();
            initUtilitiesMethodInfo1.ShouldNotBeNull();
            initUtilitiesMethodInfo2.ShouldNotBeNull();
            initUtilitiesMethodInfo1.ShouldBe(initUtilitiesMethodInfo2);
            Should.Throw<Exception>(() => initUtilitiesMethodInfo1.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<Exception>(() => initUtilitiesMethodInfo2.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<Exception>(() => initUtilitiesMethodInfo1.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<Exception>(() => initUtilitiesMethodInfo2.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => initUtilitiesMethodInfo1.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => initUtilitiesMethodInfo2.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => initUtilitiesMethodInfo1.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => initUtilitiesMethodInfo2.Invoke(authenticationHelper, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : AuthenticationHelper => GetTokenUrl (Return type :  string)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetTokenUrl_Static_Method_5_Parameters_Simple_Call_Test()
        {
            // Arrange
            var endpoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();
            var redirectURL = Fixture.Create<string>();
            var authCode = Fixture.Create<string>();

            // Act, Assert
            Should.NotThrow(() => AuthenticationHelper.GetTokenUrl(endpoint, consumerKey, consumerSecret, redirectURL, authCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetTokenUrl_Static_Method_5_Parameters_2_Calls_Test()
        {
            // Arrange
            var endpoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();
            var redirectURL = Fixture.Create<string>();
            var authCode = Fixture.Create<string>();

            // Act
            Func<string> getTokenUrl = () => AuthenticationHelper.GetTokenUrl(endpoint, consumerKey, consumerSecret, redirectURL, authCode);

            // Assert
            Should.NotThrow(() => getTokenUrl.Invoke());
            Should.NotThrow(() => AuthenticationHelper.GetTokenUrl(endpoint, consumerKey, consumerSecret, redirectURL, authCode));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetTokenUrl_Static_Method_With_5_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var endpoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();
            var redirectURL = Fixture.Create<string>();
            var authCode = Fixture.Create<string>();

            // Act
            Func<string> getTokenUrl1 = () => AuthenticationHelper.GetTokenUrl(endpoint, consumerKey, consumerSecret, redirectURL, authCode);
            Func<string> getTokenUrl2 = () => AuthenticationHelper.GetTokenUrl(endpoint, consumerKey, consumerSecret, redirectURL, authCode);
            var result1 = getTokenUrl1();
            var result2 = getTokenUrl2();
            var target1 = getTokenUrl1.Target;
            var target2 = getTokenUrl2.Target;

            // Assert
            getTokenUrl1.ShouldNotBeNull();
            getTokenUrl2.ShouldNotBeNull();
            getTokenUrl1.ShouldNotBe(getTokenUrl2);
            target1.ShouldBe(target2);
            Should.NotThrow(() => getTokenUrl1.Invoke());
            Should.NotThrow(() => getTokenUrl2.Invoke());
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetTokenUrl_Static_Method_With_5_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var endpoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();
            var redirectURL = Fixture.Create<string>();
            var authCode = Fixture.Create<string>();
            Object[] parametersOfGetTokenUrl = { endpoint, consumerKey, consumerSecret, redirectURL, authCode };
            var authenticationHelper = Fixture.Create<AuthenticationHelper>();
            var methodName = "GetTokenUrl";

            // Act
            var getTokenUrlMethodInfo1 = authenticationHelper.GetType().GetMethod(methodName);
            var getTokenUrlMethodInfo2 = authenticationHelper.GetType().GetMethod(methodName);
            var returnType1 = getTokenUrlMethodInfo1.ReturnType;
            var returnType2 = getTokenUrlMethodInfo2.ReturnType;
            var result1 = getTokenUrlMethodInfo1.Invoke(authenticationHelper, parametersOfGetTokenUrl) as string;
            var result2 = getTokenUrlMethodInfo2.Invoke(authenticationHelper, parametersOfGetTokenUrl) as string;

            // Assert
            parametersOfGetTokenUrl.ShouldNotBeNull();
            authenticationHelper.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getTokenUrlMethodInfo1.ShouldNotBeNull();
            getTokenUrlMethodInfo2.ShouldNotBeNull();
            getTokenUrlMethodInfo1.ShouldBe(getTokenUrlMethodInfo2);
            Should.NotThrow(() => getTokenUrlMethodInfo1.Invoke(authenticationHelper, parametersOfGetTokenUrl));
            Should.NotThrow(() => getTokenUrlMethodInfo2.Invoke(authenticationHelper, parametersOfGetTokenUrl));
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetTokenUrl_Static_Method_With_5_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var endpoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();
            var redirectURL = Fixture.Create<string>();
            var authCode = Fixture.Create<string>();
            Object[] parametersOutRanged = { endpoint, consumerKey, consumerSecret, redirectURL, authCode, null };
            Object[] parametersInDifferentNumber = { endpoint, consumerKey, consumerSecret, redirectURL };
            var authenticationHelper = Fixture.Create<AuthenticationHelper>();
            var methodName = "GetTokenUrl";

            // Act
            var getTokenUrlMethodInfo1 = authenticationHelper.GetType().GetMethod(methodName);
            var getTokenUrlMethodInfo2 = authenticationHelper.GetType().GetMethod(methodName);
            var returnType1 = getTokenUrlMethodInfo1.ReturnType;
            var returnType2 = getTokenUrlMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            authenticationHelper.ShouldNotBeNull();
            getTokenUrlMethodInfo1.ShouldNotBeNull();
            getTokenUrlMethodInfo2.ShouldNotBeNull();
            getTokenUrlMethodInfo1.ShouldBe(getTokenUrlMethodInfo2);
            Should.Throw<Exception>(() => getTokenUrlMethodInfo1.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<Exception>(() => getTokenUrlMethodInfo2.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<Exception>(() => getTokenUrlMethodInfo1.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<Exception>(() => getTokenUrlMethodInfo2.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => getTokenUrlMethodInfo1.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => getTokenUrlMethodInfo2.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => getTokenUrlMethodInfo1.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => getTokenUrlMethodInfo2.Invoke(authenticationHelper, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : AuthenticationHelper => GetRefreshTokenUrl (Return type :  string)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetRefreshTokenUrl_Static_Method_4_Parameters_Simple_Call_Test()
        {
            // Arrange
            var endpoint = Fixture.Create<string>();
            var refreshToken = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();

            // Act, Assert
            Should.NotThrow(() => AuthenticationHelper.GetRefreshTokenUrl(endpoint, refreshToken, consumerKey, consumerSecret));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetRefreshTokenUrl_Static_Method_4_Parameters_2_Calls_Test()
        {
            // Arrange
            var endpoint = Fixture.Create<string>();
            var refreshToken = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();

            // Act
            Func<string> getRefreshTokenUrl = () => AuthenticationHelper.GetRefreshTokenUrl(endpoint, refreshToken, consumerKey, consumerSecret);

            // Assert
            Should.NotThrow(() => getRefreshTokenUrl.Invoke());
            Should.NotThrow(() => AuthenticationHelper.GetRefreshTokenUrl(endpoint, refreshToken, consumerKey, consumerSecret));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetRefreshTokenUrl_Static_Method_With_4_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var endpoint = Fixture.Create<string>();
            var refreshToken = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();

            // Act
            Func<string> getRefreshTokenUrl1 = () => AuthenticationHelper.GetRefreshTokenUrl(endpoint, refreshToken, consumerKey, consumerSecret);
            Func<string> getRefreshTokenUrl2 = () => AuthenticationHelper.GetRefreshTokenUrl(endpoint, refreshToken, consumerKey, consumerSecret);
            var result1 = getRefreshTokenUrl1();
            var result2 = getRefreshTokenUrl2();
            var target1 = getRefreshTokenUrl1.Target;
            var target2 = getRefreshTokenUrl2.Target;

            // Assert
            getRefreshTokenUrl1.ShouldNotBeNull();
            getRefreshTokenUrl2.ShouldNotBeNull();
            getRefreshTokenUrl1.ShouldNotBe(getRefreshTokenUrl2);
            target1.ShouldBe(target2);
            Should.NotThrow(() => getRefreshTokenUrl1.Invoke());
            Should.NotThrow(() => getRefreshTokenUrl2.Invoke());
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetRefreshTokenUrl_Static_Method_With_4_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var endpoint = Fixture.Create<string>();
            var refreshToken = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();
            Object[] parametersOfGetRefreshTokenUrl = { endpoint, refreshToken, consumerKey, consumerSecret };
            var authenticationHelper = Fixture.Create<AuthenticationHelper>();
            var methodName = "GetRefreshTokenUrl";

            // Act
            var getRefreshTokenUrlMethodInfo1 = authenticationHelper.GetType().GetMethod(methodName);
            var getRefreshTokenUrlMethodInfo2 = authenticationHelper.GetType().GetMethod(methodName);
            var returnType1 = getRefreshTokenUrlMethodInfo1.ReturnType;
            var returnType2 = getRefreshTokenUrlMethodInfo2.ReturnType;
            var result1 = getRefreshTokenUrlMethodInfo1.Invoke(authenticationHelper, parametersOfGetRefreshTokenUrl) as string;
            var result2 = getRefreshTokenUrlMethodInfo2.Invoke(authenticationHelper, parametersOfGetRefreshTokenUrl) as string;

            // Assert
            parametersOfGetRefreshTokenUrl.ShouldNotBeNull();
            authenticationHelper.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getRefreshTokenUrlMethodInfo1.ShouldNotBeNull();
            getRefreshTokenUrlMethodInfo2.ShouldNotBeNull();
            getRefreshTokenUrlMethodInfo1.ShouldBe(getRefreshTokenUrlMethodInfo2);
            Should.NotThrow(() => getRefreshTokenUrlMethodInfo1.Invoke(authenticationHelper, parametersOfGetRefreshTokenUrl));
            Should.NotThrow(() => getRefreshTokenUrlMethodInfo2.Invoke(authenticationHelper, parametersOfGetRefreshTokenUrl));
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetRefreshTokenUrl_Static_Method_With_4_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var endpoint = Fixture.Create<string>();
            var refreshToken = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();
            Object[] parametersOutRanged = { endpoint, refreshToken, consumerKey, consumerSecret, null };
            Object[] parametersInDifferentNumber = { endpoint, refreshToken, consumerKey };
            var authenticationHelper = Fixture.Create<AuthenticationHelper>();
            var methodName = "GetRefreshTokenUrl";

            // Act
            var getRefreshTokenUrlMethodInfo1 = authenticationHelper.GetType().GetMethod(methodName);
            var getRefreshTokenUrlMethodInfo2 = authenticationHelper.GetType().GetMethod(methodName);
            var returnType1 = getRefreshTokenUrlMethodInfo1.ReturnType;
            var returnType2 = getRefreshTokenUrlMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            authenticationHelper.ShouldNotBeNull();
            getRefreshTokenUrlMethodInfo1.ShouldNotBeNull();
            getRefreshTokenUrlMethodInfo2.ShouldNotBeNull();
            getRefreshTokenUrlMethodInfo1.ShouldBe(getRefreshTokenUrlMethodInfo2);
            Should.Throw<Exception>(() => getRefreshTokenUrlMethodInfo1.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<Exception>(() => getRefreshTokenUrlMethodInfo2.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<Exception>(() => getRefreshTokenUrlMethodInfo1.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<Exception>(() => getRefreshTokenUrlMethodInfo2.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => getRefreshTokenUrlMethodInfo1.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => getRefreshTokenUrlMethodInfo2.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => getRefreshTokenUrlMethodInfo1.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => getRefreshTokenUrlMethodInfo2.Invoke(authenticationHelper, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : AuthenticationHelper => GetLoginUrl (Return type :  string)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetLoginUrl_Static_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
            var endpoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var redirectUrl = Fixture.Create<string>();

            // Act, Assert
            Should.NotThrow(() => AuthenticationHelper.GetLoginUrl(endpoint, consumerKey, redirectUrl));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetLoginUrl_Static_Method_3_Parameters_2_Calls_Test()
        {
            // Arrange
            var endpoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var redirectUrl = Fixture.Create<string>();

            // Act
            Func<string> getLoginUrl = () => AuthenticationHelper.GetLoginUrl(endpoint, consumerKey, redirectUrl);

            // Assert
            Should.NotThrow(() => getLoginUrl.Invoke());
            Should.NotThrow(() => AuthenticationHelper.GetLoginUrl(endpoint, consumerKey, redirectUrl));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetLoginUrl_Static_Method_With_3_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var endpoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var redirectUrl = Fixture.Create<string>();

            // Act
            Func<string> getLoginUrl1 = () => AuthenticationHelper.GetLoginUrl(endpoint, consumerKey, redirectUrl);
            Func<string> getLoginUrl2 = () => AuthenticationHelper.GetLoginUrl(endpoint, consumerKey, redirectUrl);
            var result1 = getLoginUrl1();
            var result2 = getLoginUrl2();
            var target1 = getLoginUrl1.Target;
            var target2 = getLoginUrl2.Target;

            // Assert
            getLoginUrl1.ShouldNotBeNull();
            getLoginUrl2.ShouldNotBeNull();
            getLoginUrl1.ShouldNotBe(getLoginUrl2);
            target1.ShouldBe(target2);
            Should.NotThrow(() => getLoginUrl1.Invoke());
            Should.NotThrow(() => getLoginUrl2.Invoke());
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetLoginUrl_Static_Method_With_3_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var endpoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var redirectUrl = Fixture.Create<string>();
            Object[] parametersOfGetLoginUrl = { endpoint, consumerKey, redirectUrl };
            var authenticationHelper = Fixture.Create<AuthenticationHelper>();
            var methodName = "GetLoginUrl";

            // Act
            var getLoginUrlMethodInfo1 = authenticationHelper.GetType().GetMethod(methodName);
            var getLoginUrlMethodInfo2 = authenticationHelper.GetType().GetMethod(methodName);
            var returnType1 = getLoginUrlMethodInfo1.ReturnType;
            var returnType2 = getLoginUrlMethodInfo2.ReturnType;
            var result1 = getLoginUrlMethodInfo1.Invoke(authenticationHelper, parametersOfGetLoginUrl) as string;
            var result2 = getLoginUrlMethodInfo2.Invoke(authenticationHelper, parametersOfGetLoginUrl) as string;

            // Assert
            parametersOfGetLoginUrl.ShouldNotBeNull();
            authenticationHelper.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            getLoginUrlMethodInfo1.ShouldNotBeNull();
            getLoginUrlMethodInfo2.ShouldNotBeNull();
            getLoginUrlMethodInfo1.ShouldBe(getLoginUrlMethodInfo2);
            Should.NotThrow(() => getLoginUrlMethodInfo1.Invoke(authenticationHelper, parametersOfGetLoginUrl));
            Should.NotThrow(() => getLoginUrlMethodInfo2.Invoke(authenticationHelper, parametersOfGetLoginUrl));
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetLoginUrl_Static_Method_With_3_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var endpoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var redirectUrl = Fixture.Create<string>();
            Object[] parametersOutRanged = { endpoint, consumerKey, redirectUrl, null };
            Object[] parametersInDifferentNumber = { endpoint, consumerKey };
            var authenticationHelper = Fixture.Create<AuthenticationHelper>();
            var methodName = "GetLoginUrl";

            // Act
            var getLoginUrlMethodInfo1 = authenticationHelper.GetType().GetMethod(methodName);
            var getLoginUrlMethodInfo2 = authenticationHelper.GetType().GetMethod(methodName);
            var returnType1 = getLoginUrlMethodInfo1.ReturnType;
            var returnType2 = getLoginUrlMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            authenticationHelper.ShouldNotBeNull();
            getLoginUrlMethodInfo1.ShouldNotBeNull();
            getLoginUrlMethodInfo2.ShouldNotBeNull();
            getLoginUrlMethodInfo1.ShouldBe(getLoginUrlMethodInfo2);
            Should.Throw<Exception>(() => getLoginUrlMethodInfo1.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<Exception>(() => getLoginUrlMethodInfo2.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<Exception>(() => getLoginUrlMethodInfo1.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<Exception>(() => getLoginUrlMethodInfo2.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => getLoginUrlMethodInfo1.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => getLoginUrlMethodInfo2.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => getLoginUrlMethodInfo1.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => getLoginUrlMethodInfo2.Invoke(authenticationHelper, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : AuthenticationHelper => GetToken (Return type :  dynamic)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetToken_Static_Method_5_Parameters_Simple_Call_Test()
        {
            // Arrange
            var tokenEndPoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();
            var redirectURL = Fixture.Create<string>();
            var authCode = Fixture.Create<string>();

            // Act, Assert
            Should.Throw<Exception>(() => AuthenticationHelper.GetToken<dynamic>(tokenEndPoint, consumerKey, consumerSecret, redirectURL, authCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetToken_Static_Method_5_Parameters_2_Calls_Test()
        {
            // Arrange
            var tokenEndPoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();
            var redirectURL = Fixture.Create<string>();
            var authCode = Fixture.Create<string>();

            // Act
            Func<dynamic> getToken = () => AuthenticationHelper.GetToken<dynamic>(tokenEndPoint, consumerKey, consumerSecret, redirectURL, authCode);

            // Assert
            Should.Throw<Exception>(() => getToken.Invoke());
            Should.Throw<Exception>(() => AuthenticationHelper.GetToken<dynamic>(tokenEndPoint, consumerKey, consumerSecret, redirectURL, authCode));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetToken_Static_Method_With_5_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var tokenEndPoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();
            var redirectURL = Fixture.Create<string>();
            var authCode = Fixture.Create<string>();

            // Act
            Func<dynamic> getToken1 = () => AuthenticationHelper.GetToken<dynamic>(tokenEndPoint, consumerKey, consumerSecret, redirectURL, authCode);
            Func<dynamic> getToken2 = () => AuthenticationHelper.GetToken<dynamic>(tokenEndPoint, consumerKey, consumerSecret, redirectURL, authCode);
            var target1 = getToken1.Target;
            var target2 = getToken2.Target;

            // Assert
            getToken1.ShouldNotBeNull();
            getToken2.ShouldNotBeNull();
            getToken1.ShouldNotBe(getToken2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => getToken1.Invoke());
            Should.Throw<Exception>(() => getToken2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetToken_Static_Method_With_5_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var tokenEndPoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();
            var redirectURL = Fixture.Create<string>();
            var authCode = Fixture.Create<string>();
            Object[] parametersOutRanged = { tokenEndPoint, consumerKey, consumerSecret, redirectURL, authCode, null };
            Object[] parametersInDifferentNumber = { tokenEndPoint, consumerKey, consumerSecret, redirectURL };
            var authenticationHelper = Fixture.Create<AuthenticationHelper>();
            var methodName = "GetToken";

            // Act
            var getTokenMethodInfo1 = authenticationHelper.GetType().GetMethod(methodName);
            var getTokenMethodInfo2 = authenticationHelper.GetType().GetMethod(methodName);
            var returnType1 = getTokenMethodInfo1.ReturnType;
            var returnType2 = getTokenMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            authenticationHelper.ShouldNotBeNull();
            getTokenMethodInfo1.ShouldNotBeNull();
            getTokenMethodInfo2.ShouldNotBeNull();
            getTokenMethodInfo1.ShouldBe(getTokenMethodInfo2);
            Should.Throw<Exception>(() => getTokenMethodInfo1.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<Exception>(() => getTokenMethodInfo2.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<Exception>(() => getTokenMethodInfo1.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<Exception>(() => getTokenMethodInfo2.Invoke(authenticationHelper, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : AuthenticationHelper => GetRefreshToken (Return type :  dynamic)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetRefreshToken_Static_Method_4_Parameters_Simple_Call_Test()
        {
            // Arrange
            var tokenEndPoint = Fixture.Create<string>();
            var refreshToken = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();

            // Act, Assert
            Should.Throw<Exception>(() => AuthenticationHelper.GetRefreshToken<dynamic>(tokenEndPoint, refreshToken, consumerKey, consumerSecret));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetRefreshToken_Static_Method_4_Parameters_2_Calls_Test()
        {
            // Arrange
            var tokenEndPoint = Fixture.Create<string>();
            var refreshToken = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();

            // Act
            Func<dynamic> getRefreshToken = () => AuthenticationHelper.GetRefreshToken<dynamic>(tokenEndPoint, refreshToken, consumerKey, consumerSecret);

            // Assert
            Should.Throw<Exception>(() => getRefreshToken.Invoke());
            Should.Throw<Exception>(() => AuthenticationHelper.GetRefreshToken<dynamic>(tokenEndPoint, refreshToken, consumerKey, consumerSecret));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetRefreshToken_Static_Method_With_4_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var tokenEndPoint = Fixture.Create<string>();
            var refreshToken = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();

            // Act
            Func<dynamic> getRefreshToken1 = () => AuthenticationHelper.GetRefreshToken<dynamic>(tokenEndPoint, refreshToken, consumerKey, consumerSecret);
            Func<dynamic> getRefreshToken2 = () => AuthenticationHelper.GetRefreshToken<dynamic>(tokenEndPoint, refreshToken, consumerKey, consumerSecret);
            var target1 = getRefreshToken1.Target;
            var target2 = getRefreshToken2.Target;

            // Assert
            getRefreshToken1.ShouldNotBeNull();
            getRefreshToken2.ShouldNotBeNull();
            getRefreshToken1.ShouldNotBe(getRefreshToken2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => getRefreshToken1.Invoke());
            Should.Throw<Exception>(() => getRefreshToken2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_GetRefreshToken_Static_Method_With_4_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var tokenEndPoint = Fixture.Create<string>();
            var refreshToken = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();
            Object[] parametersOutRanged = { tokenEndPoint, refreshToken, consumerKey, consumerSecret, null };
            Object[] parametersInDifferentNumber = { tokenEndPoint, refreshToken, consumerKey };
            var authenticationHelper = Fixture.Create<AuthenticationHelper>();
            var methodName = "GetRefreshToken";

            // Act
            var getRefreshTokenMethodInfo1 = authenticationHelper.GetType().GetMethod(methodName);
            var getRefreshTokenMethodInfo2 = authenticationHelper.GetType().GetMethod(methodName);
            var returnType1 = getRefreshTokenMethodInfo1.ReturnType;
            var returnType2 = getRefreshTokenMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            authenticationHelper.ShouldNotBeNull();
            getRefreshTokenMethodInfo1.ShouldNotBeNull();
            getRefreshTokenMethodInfo2.ShouldNotBeNull();
            getRefreshTokenMethodInfo1.ShouldBe(getRefreshTokenMethodInfo2);
            Should.Throw<Exception>(() => getRefreshTokenMethodInfo1.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<Exception>(() => getRefreshTokenMethodInfo2.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<Exception>(() => getRefreshTokenMethodInfo1.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<Exception>(() => getRefreshTokenMethodInfo2.Invoke(authenticationHelper, parametersInDifferentNumber));
        }

        #endregion

        #region Method Call Test : AuthenticationHelper => Login (Return type :  string)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_Login_Static_Method_3_Parameters_Simple_Call_Test()
        {
            // Arrange
            var authorizeEndPoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var redirectUrl = Fixture.Create<string>();

            // Act, Assert
            Should.Throw<Exception>(() => AuthenticationHelper.Login(authorizeEndPoint, consumerKey, redirectUrl));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_Login_Static_Method_3_Parameters_2_Calls_Test()
        {
            // Arrange
            var authorizeEndPoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var redirectUrl = Fixture.Create<string>();

            // Act
            Func<string> login = () => AuthenticationHelper.Login(authorizeEndPoint, consumerKey, redirectUrl);

            // Assert
            Should.Throw<Exception>(() => login.Invoke());
            Should.Throw<Exception>(() => AuthenticationHelper.Login(authorizeEndPoint, consumerKey, redirectUrl));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_Login_Static_Method_With_3_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var authorizeEndPoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var redirectUrl = Fixture.Create<string>();

            // Act
            Func<string> login1 = () => AuthenticationHelper.Login(authorizeEndPoint, consumerKey, redirectUrl);
            Func<string> login2 = () => AuthenticationHelper.Login(authorizeEndPoint, consumerKey, redirectUrl);
            var target1 = login1.Target;
            var target2 = login2.Target;

            // Assert
            login1.ShouldNotBeNull();
            login2.ShouldNotBeNull();
            login1.ShouldNotBe(login2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => login1.Invoke());
            Should.Throw<Exception>(() => login2.Invoke());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_Login_Static_Method_With_3_Parameters_Call_With_Reflection_Test()
        {
            // Arrange
            var authorizeEndPoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var redirectUrl = Fixture.Create<string>();
            Object[] parametersOfLogin = { authorizeEndPoint, consumerKey, redirectUrl };
            var authenticationHelper = Fixture.Create<AuthenticationHelper>();
            var methodName = "Login";

            // Act
            var loginMethodInfo1 = authenticationHelper.GetType().GetMethod(methodName);
            var loginMethodInfo2 = authenticationHelper.GetType().GetMethod(methodName);
            var returnType1 = loginMethodInfo1.ReturnType;
            var returnType2 = loginMethodInfo2.ReturnType;

            // Assert
            parametersOfLogin.ShouldNotBeNull();
            authenticationHelper.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            loginMethodInfo1.ShouldNotBeNull();
            loginMethodInfo2.ShouldNotBeNull();
            loginMethodInfo1.ShouldBe(loginMethodInfo2);
            Should.Throw<Exception>(() => loginMethodInfo1.Invoke(authenticationHelper, parametersOfLogin));
            Should.Throw<Exception>(() => loginMethodInfo2.Invoke(authenticationHelper, parametersOfLogin));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void AuthenticationHelper_Login_Static_Method_With_3_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var authorizeEndPoint = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var redirectUrl = Fixture.Create<string>();
            Object[] parametersOutRanged = { authorizeEndPoint, consumerKey, redirectUrl, null };
            Object[] parametersInDifferentNumber = { authorizeEndPoint, consumerKey };
            var authenticationHelper = Fixture.Create<AuthenticationHelper>();
            var methodName = "Login";

            // Act
            var loginMethodInfo1 = authenticationHelper.GetType().GetMethod(methodName);
            var loginMethodInfo2 = authenticationHelper.GetType().GetMethod(methodName);
            var returnType1 = loginMethodInfo1.ReturnType;
            var returnType2 = loginMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            authenticationHelper.ShouldNotBeNull();
            loginMethodInfo1.ShouldNotBeNull();
            loginMethodInfo2.ShouldNotBeNull();
            loginMethodInfo1.ShouldBe(loginMethodInfo2);
            Should.Throw<Exception>(() => loginMethodInfo1.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<Exception>(() => loginMethodInfo2.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<Exception>(() => loginMethodInfo1.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<Exception>(() => loginMethodInfo2.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => loginMethodInfo1.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => loginMethodInfo2.Invoke(authenticationHelper, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(() => loginMethodInfo1.Invoke(authenticationHelper, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(() => loginMethodInfo2.Invoke(authenticationHelper, parametersInDifferentNumber));
        }

        #endregion

        #endregion


        #endregion


        #endregion
    }
}
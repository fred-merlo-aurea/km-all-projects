using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.UI;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using ECN_Framework_Common.Objects.Accounts;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.MasterPages
{
    [TestFixture]
    public class AccountsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (Accounts) => Method (HasAuthorized) (Return Type :  bool) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void Accounts_HasAuthorized_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
            var userId = Fixture.Create<int>();
            var clientId = Fixture.Create<int>();
            var accounts = new ecn.accounts.MasterPages.Accounts();

            // Act, Assert
            Should.Throw<System.Exception>(() => accounts.HasAuthorized(userId, clientId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void Accounts_HasAuthorized_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var userId = Fixture.Create<int>();
            var clientId = Fixture.Create<int>();
            Object[] parametersOfHasAuthorized = { userId, clientId };
            System.Exception exception, invokeException;
            var accounts = CreateAnalyzer.CreateOrReturnStaticInstance<ecn.accounts.MasterPages.Accounts>(Fixture, out exception);
            var methodName = "HasAuthorized";

            // Act
            var hasAuthorizedMethodInfo1 = accounts.GetType().GetMethod(methodName);
            var hasAuthorizedMethodInfo2 = accounts.GetType().GetMethod(methodName);
            var returnType1 = hasAuthorizedMethodInfo1.ReturnType;
            var returnType2 = hasAuthorizedMethodInfo2.ReturnType;

            // Assert
            parametersOfHasAuthorized.ShouldNotBeNull();
            accounts.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            hasAuthorizedMethodInfo1.ShouldNotBeNull();
            hasAuthorizedMethodInfo2.ShouldNotBeNull();
            hasAuthorizedMethodInfo1.ShouldBe(hasAuthorizedMethodInfo2);
            if (hasAuthorizedMethodInfo1.DoesInvokeThrow(accounts, out invokeException, parametersOfHasAuthorized))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => hasAuthorizedMethodInfo1.Invoke(accounts, parametersOfHasAuthorized), exceptionType: invokeException.GetType());
                Should.Throw(() => hasAuthorizedMethodInfo2.Invoke(accounts, parametersOfHasAuthorized), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => hasAuthorizedMethodInfo1.Invoke(accounts, parametersOfHasAuthorized));
                Should.NotThrow(() => hasAuthorizedMethodInfo2.Invoke(accounts, parametersOfHasAuthorized));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void Accounts_HasAuthorized_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var userId = Fixture.Create<int>();
            var clientId = Fixture.Create<int>();
            Object[] parametersOutRanged = { userId, clientId, null };
            Object[] parametersInDifferentNumber = { userId };
            System.Exception exception, exception1, exception2, exception3, exception4;
            var accounts = CreateAnalyzer.CreateOrReturnStaticInstance<ecn.accounts.MasterPages.Accounts>(Fixture, out exception);
            var methodName = "HasAuthorized";

            if (accounts != null)
            {
                // Act
                var hasAuthorizedMethodInfo1 = accounts.GetType().GetMethod(methodName);
                var hasAuthorizedMethodInfo2 = accounts.GetType().GetMethod(methodName);
                var returnType1 = hasAuthorizedMethodInfo1.ReturnType;
                var returnType2 = hasAuthorizedMethodInfo2.ReturnType;
                var result1 = hasAuthorizedMethodInfo1.GetResultMethodInfo<ecn.accounts.MasterPages.Accounts, bool>(accounts, out exception1, parametersOutRanged);
                var result2 = hasAuthorizedMethodInfo2.GetResultMethodInfo<ecn.accounts.MasterPages.Accounts, bool>(accounts, out exception2, parametersOutRanged);
                var result3 = hasAuthorizedMethodInfo1.GetResultMethodInfo<ecn.accounts.MasterPages.Accounts, bool>(accounts, out exception3, parametersInDifferentNumber);
                var result4 = hasAuthorizedMethodInfo2.GetResultMethodInfo<ecn.accounts.MasterPages.Accounts, bool>(accounts, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                accounts.ShouldNotBeNull();
                hasAuthorizedMethodInfo1.ShouldNotBeNull();
                hasAuthorizedMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if (exception1 != null)
                {
                    Should.Throw(() => hasAuthorizedMethodInfo1.Invoke(accounts, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => hasAuthorizedMethodInfo2.Invoke(accounts, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => hasAuthorizedMethodInfo1.Invoke(accounts, parametersOutRanged));
                    Should.NotThrow(() => hasAuthorizedMethodInfo2.Invoke(accounts, parametersOutRanged));
                }

                if (exception1 != null)
                {
                    Should.Throw(() => hasAuthorizedMethodInfo1.Invoke(accounts, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => hasAuthorizedMethodInfo2.Invoke(accounts, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => hasAuthorizedMethodInfo1.Invoke(accounts, parametersOutRanged));
                    Should.NotThrow(() => hasAuthorizedMethodInfo2.Invoke(accounts, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => hasAuthorizedMethodInfo1.Invoke(accounts, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => hasAuthorizedMethodInfo2.Invoke(accounts, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => hasAuthorizedMethodInfo1.Invoke(accounts, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => hasAuthorizedMethodInfo2.Invoke(accounts, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => hasAuthorizedMethodInfo1.Invoke(accounts, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => hasAuthorizedMethodInfo2.Invoke(accounts, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                accounts.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (Accounts) => Method (MasterRegisterButtonForPostBack) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void Accounts_MasterRegisterButtonForPostBack_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            Object[] parametersOfMasterRegisterButtonForPostBack = { null };
            System.Exception exception, invokeException;
            var accounts = CreateAnalyzer.CreateOrReturnStaticInstance<ecn.accounts.MasterPages.Accounts>(Fixture, out exception);
            var methodName = "MasterRegisterButtonForPostBack";

            // Act
            var masterRegisterButtonForPostBackMethodInfo1 = accounts.GetType().GetMethod(methodName);
            var masterRegisterButtonForPostBackMethodInfo2 = accounts.GetType().GetMethod(methodName);
            var returnType1 = masterRegisterButtonForPostBackMethodInfo1.ReturnType;
            var returnType2 = masterRegisterButtonForPostBackMethodInfo2.ReturnType;

            // Assert
            parametersOfMasterRegisterButtonForPostBack.ShouldNotBeNull();
            accounts.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            masterRegisterButtonForPostBackMethodInfo1.ShouldNotBeNull();
            masterRegisterButtonForPostBackMethodInfo2.ShouldNotBeNull();
            masterRegisterButtonForPostBackMethodInfo1.ShouldBe(masterRegisterButtonForPostBackMethodInfo2);
            if (masterRegisterButtonForPostBackMethodInfo1.DoesInvokeThrow(accounts, out invokeException, parametersOfMasterRegisterButtonForPostBack))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => masterRegisterButtonForPostBackMethodInfo1.Invoke(accounts, parametersOfMasterRegisterButtonForPostBack), exceptionType: invokeException.GetType());
                Should.Throw(() => masterRegisterButtonForPostBackMethodInfo2.Invoke(accounts, parametersOfMasterRegisterButtonForPostBack), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => masterRegisterButtonForPostBackMethodInfo1.Invoke(accounts, parametersOfMasterRegisterButtonForPostBack));
                Should.NotThrow(() => masterRegisterButtonForPostBackMethodInfo2.Invoke(accounts, parametersOfMasterRegisterButtonForPostBack));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void Accounts_MasterRegisterButtonForPostBack_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            Object[] parametersOutRanged = { null, null };
            Object[] parametersInDifferentNumber = { };
            System.Exception exception, exception1, exception2, exception3, exception4;
            var accounts = CreateAnalyzer.CreateOrReturnStaticInstance<ecn.accounts.MasterPages.Accounts>(Fixture, out exception);
            var methodName = "MasterRegisterButtonForPostBack";

            if (accounts != null)
            {
                // Act
                var masterRegisterButtonForPostBackMethodInfo1 = accounts.GetType().GetMethod(methodName);
                var masterRegisterButtonForPostBackMethodInfo2 = accounts.GetType().GetMethod(methodName);
                var returnType1 = masterRegisterButtonForPostBackMethodInfo1.ReturnType;
                var returnType2 = masterRegisterButtonForPostBackMethodInfo2.ReturnType;
                masterRegisterButtonForPostBackMethodInfo1.InvokeMethodInfo(accounts, out exception1, parametersOutRanged);
                masterRegisterButtonForPostBackMethodInfo2.InvokeMethodInfo(accounts, out exception2, parametersOutRanged);
                masterRegisterButtonForPostBackMethodInfo1.InvokeMethodInfo(accounts, out exception3, parametersInDifferentNumber);
                masterRegisterButtonForPostBackMethodInfo2.InvokeMethodInfo(accounts, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                accounts.ShouldNotBeNull();
                masterRegisterButtonForPostBackMethodInfo1.ShouldNotBeNull();
                masterRegisterButtonForPostBackMethodInfo2.ShouldNotBeNull();
                if (exception1 != null)
                {
                    Should.Throw(() => masterRegisterButtonForPostBackMethodInfo1.Invoke(accounts, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => masterRegisterButtonForPostBackMethodInfo2.Invoke(accounts, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => masterRegisterButtonForPostBackMethodInfo1.Invoke(accounts, parametersOutRanged));
                    Should.NotThrow(() => masterRegisterButtonForPostBackMethodInfo2.Invoke(accounts, parametersOutRanged));
                }

                if (exception1 != null)
                {
                    Should.Throw(() => masterRegisterButtonForPostBackMethodInfo1.Invoke(accounts, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => masterRegisterButtonForPostBackMethodInfo2.Invoke(accounts, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => masterRegisterButtonForPostBackMethodInfo1.Invoke(accounts, parametersOutRanged));
                    Should.NotThrow(() => masterRegisterButtonForPostBackMethodInfo2.Invoke(accounts, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => masterRegisterButtonForPostBackMethodInfo1.Invoke(accounts, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => masterRegisterButtonForPostBackMethodInfo2.Invoke(accounts, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => masterRegisterButtonForPostBackMethodInfo1.Invoke(accounts, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => masterRegisterButtonForPostBackMethodInfo2.Invoke(accounts, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => masterRegisterButtonForPostBackMethodInfo1.Invoke(accounts, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => masterRegisterButtonForPostBackMethodInfo2.Invoke(accounts, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                accounts.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Accounts) => Property (CurrentMenuCode) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Accounts_CurrentMenuCode_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCurrentMenuCode = "CurrentMenuCode";
            var accounts = new ecn.accounts.MasterPages.Accounts();
            var randomString = Fixture.Create<string>();
            var propertyInfo = accounts.GetType().GetProperty(propertyNameCurrentMenuCode);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(accounts, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Accounts) => Property (CurrentMenuCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Accounts_Class_Invalid_Property_CurrentMenuCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCurrentMenuCode = "CurrentMenuCodeNotPresent";
            var accounts = new ecn.accounts.MasterPages.Accounts();

            // Act , Assert
            Should.NotThrow(() => accounts.GetType().GetProperty(propertyNameCurrentMenuCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Accounts_CurrentMenuCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCurrentMenuCode = "CurrentMenuCode";
            var accounts = new ecn.accounts.MasterPages.Accounts();
            var propertyInfo = accounts.GetType().GetProperty(propertyNameCurrentMenuCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Accounts) => Property (es) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Accounts_es_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNamees = "es";
            var accounts = new ecn.accounts.MasterPages.Accounts();
            var randomString = Fixture.Create<string>();
            var propertyInfo = accounts.GetType().GetProperty(propertyNamees);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(accounts, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Accounts) => Property (es) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Accounts_Class_Invalid_Property_esNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamees = "esNotPresent";
            var accounts = new ecn.accounts.MasterPages.Accounts();

            // Act , Assert
            Should.NotThrow(() => accounts.GetType().GetProperty(propertyNamees));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Accounts_es_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamees = "es";
            var accounts = new ecn.accounts.MasterPages.Accounts();
            var propertyInfo = accounts.GetType().GetProperty(propertyNamees);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Accounts) => Property (UserSession) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Accounts_UserSession_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUserSession = "UserSession";
            var accounts = new ecn.accounts.MasterPages.Accounts();
            var randomString = Fixture.Create<string>();
            var propertyInfo = accounts.GetType().GetProperty(propertyNameUserSession);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(accounts, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Accounts) => Property (UserSession) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Accounts_Class_Invalid_Property_UserSessionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUserSession = "UserSessionNotPresent";
            var accounts = new ecn.accounts.MasterPages.Accounts();

            // Act , Assert
            Should.NotThrow(() => accounts.GetType().GetProperty(propertyNameUserSession));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Accounts_UserSession_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUserSession = "UserSession";
            var accounts = new ecn.accounts.MasterPages.Accounts();
            var propertyInfo = accounts.GetType().GetProperty(propertyNameUserSession);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Accounts) => Property (virtualPath) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Accounts_Class_Invalid_Property_virtualPathNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamevirtualPath = "virtualPathNotPresent";
            var accounts = new ecn.accounts.MasterPages.Accounts();

            // Act , Assert
            Should.NotThrow(() => accounts.GetType().GetProperty(propertyNamevirtualPath));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Accounts_virtualPath_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamevirtualPath = "virtualPath";
            var accounts = new ecn.accounts.MasterPages.Accounts();
            var propertyInfo = accounts.GetType().GetProperty(propertyNamevirtualPath);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #endregion
    }
}
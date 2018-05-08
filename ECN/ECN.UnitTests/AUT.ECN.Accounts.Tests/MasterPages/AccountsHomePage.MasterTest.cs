using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using ecn.accounts.MasterPages;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.MasterPages
{
    [TestFixture]
    public class AccountsHomePageTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (AccountsHomePage) => Method (HasAuthorized) (Return Type :  bool) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void AccountsHomePage_HasAuthorized_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var userId = Fixture.Create<int>();
            var clientId = Fixture.Create<int>();
            Object[] parametersOfHasAuthorized = {userId, clientId};
            System.Exception exception, invokeException;
            var accountsHomePage  = CreateAnalyzer.CreateOrReturnStaticInstance<AccountsHomePage>(Fixture, out exception);
            var methodName = "HasAuthorized";

            // Act
            var hasAuthorizedMethodInfo1 = accountsHomePage.GetType().GetMethod(methodName);
            var hasAuthorizedMethodInfo2 = accountsHomePage.GetType().GetMethod(methodName);
            var returnType1 = hasAuthorizedMethodInfo1.ReturnType;
            var returnType2 = hasAuthorizedMethodInfo2.ReturnType;

            // Assert
            parametersOfHasAuthorized.ShouldNotBeNull();
            accountsHomePage.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            hasAuthorizedMethodInfo1.ShouldNotBeNull();
            hasAuthorizedMethodInfo2.ShouldNotBeNull();
            hasAuthorizedMethodInfo1.ShouldBe(hasAuthorizedMethodInfo2);
            if(hasAuthorizedMethodInfo1.DoesInvokeThrow(accountsHomePage, out invokeException, parametersOfHasAuthorized))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => hasAuthorizedMethodInfo1.Invoke(accountsHomePage, parametersOfHasAuthorized), exceptionType: invokeException.GetType());
                Should.Throw(() => hasAuthorizedMethodInfo2.Invoke(accountsHomePage, parametersOfHasAuthorized), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => hasAuthorizedMethodInfo1.Invoke(accountsHomePage, parametersOfHasAuthorized));
                Should.NotThrow(() => hasAuthorizedMethodInfo2.Invoke(accountsHomePage, parametersOfHasAuthorized));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void AccountsHomePage_HasAuthorized_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var userId = Fixture.Create<int>();
            var clientId = Fixture.Create<int>();
            Object[] parametersOutRanged = {userId, clientId, null};
            Object[] parametersInDifferentNumber = {userId};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var accountsHomePage  = CreateAnalyzer.CreateOrReturnStaticInstance<AccountsHomePage>(Fixture, out exception);
            var methodName = "HasAuthorized";

            if(accountsHomePage != null)
            {
                // Act
                var hasAuthorizedMethodInfo1 = accountsHomePage.GetType().GetMethod(methodName);
                var hasAuthorizedMethodInfo2 = accountsHomePage.GetType().GetMethod(methodName);
                var returnType1 = hasAuthorizedMethodInfo1.ReturnType;
                var returnType2 = hasAuthorizedMethodInfo2.ReturnType;
                var result1 = hasAuthorizedMethodInfo1.GetResultMethodInfo<AccountsHomePage, bool>(accountsHomePage, out exception1, parametersOutRanged);
                var result2 = hasAuthorizedMethodInfo2.GetResultMethodInfo<AccountsHomePage, bool>(accountsHomePage, out exception2, parametersOutRanged);
                var result3 = hasAuthorizedMethodInfo1.GetResultMethodInfo<AccountsHomePage, bool>(accountsHomePage, out exception3, parametersInDifferentNumber);
                var result4 = hasAuthorizedMethodInfo2.GetResultMethodInfo<AccountsHomePage, bool>(accountsHomePage, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                accountsHomePage.ShouldNotBeNull();
                hasAuthorizedMethodInfo1.ShouldNotBeNull();
                hasAuthorizedMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => hasAuthorizedMethodInfo1.Invoke(accountsHomePage, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => hasAuthorizedMethodInfo2.Invoke(accountsHomePage, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => hasAuthorizedMethodInfo1.Invoke(accountsHomePage, parametersOutRanged));
                    Should.NotThrow(() => hasAuthorizedMethodInfo2.Invoke(accountsHomePage, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => hasAuthorizedMethodInfo1.Invoke(accountsHomePage, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => hasAuthorizedMethodInfo2.Invoke(accountsHomePage, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => hasAuthorizedMethodInfo1.Invoke(accountsHomePage, parametersOutRanged));
                    Should.NotThrow(() => hasAuthorizedMethodInfo2.Invoke(accountsHomePage, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => hasAuthorizedMethodInfo1.Invoke(accountsHomePage, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => hasAuthorizedMethodInfo2.Invoke(accountsHomePage, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => hasAuthorizedMethodInfo1.Invoke(accountsHomePage, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => hasAuthorizedMethodInfo2.Invoke(accountsHomePage, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => hasAuthorizedMethodInfo1.Invoke(accountsHomePage, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => hasAuthorizedMethodInfo2.Invoke(accountsHomePage, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                accountsHomePage.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #region Category : GetterSetter

        #region General Getters/Setters : Class (AccountsHomePage) => Property (CurrentMenuCode) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AccountsHomePage_CurrentMenuCode_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCurrentMenuCode = "CurrentMenuCode";
            var accountsHomePage = new AccountsHomePage();
            var randomString = Fixture.Create<string>();
            var propertyInfo = accountsHomePage.GetType().GetProperty(propertyNameCurrentMenuCode);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(accountsHomePage, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (AccountsHomePage) => Property (CurrentMenuCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AccountsHomePage_Class_Invalid_Property_CurrentMenuCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCurrentMenuCode = "CurrentMenuCodeNotPresent";
            var accountsHomePage  = new AccountsHomePage();

            // Act , Assert
            Should.NotThrow(() => accountsHomePage.GetType().GetProperty(propertyNameCurrentMenuCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AccountsHomePage_CurrentMenuCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCurrentMenuCode = "CurrentMenuCode";
            var accountsHomePage  = new AccountsHomePage();
            var propertyInfo  = accountsHomePage.GetType().GetProperty(propertyNameCurrentMenuCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AccountsHomePage) => Property (Heading) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AccountsHomePage_Class_Invalid_Property_HeadingNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHeading = "HeadingNotPresent";
            var accountsHomePage  = new AccountsHomePage();

            // Act , Assert
            Should.NotThrow(() => accountsHomePage.GetType().GetProperty(propertyNameHeading));
        }

        #endregion

        #region General Getters/Setters : Class (AccountsHomePage) => Property (HelpContent) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AccountsHomePage_Class_Invalid_Property_HelpContentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHelpContent = "HelpContentNotPresent";
            var accountsHomePage  = new AccountsHomePage();

            // Act , Assert
            Should.NotThrow(() => accountsHomePage.GetType().GetProperty(propertyNameHelpContent));
        }

        #endregion

        #region General Getters/Setters : Class (AccountsHomePage) => Property (HelpTitle) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AccountsHomePage_Class_Invalid_Property_HelpTitleNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHelpTitle = "HelpTitleNotPresent";
            var accountsHomePage  = new AccountsHomePage();

            // Act , Assert
            Should.NotThrow(() => accountsHomePage.GetType().GetProperty(propertyNameHelpTitle));
        }

        #endregion

        #region General Getters/Setters : Class (AccountsHomePage) => Property (SubMenu) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AccountsHomePage_SubMenu_Property_String_Type_Verify_Test()
        {
            // Arrange
            var accountsHomePage = new AccountsHomePage();
            accountsHomePage.SubMenu = Fixture.Create<string>();
            var stringType = accountsHomePage.SubMenu.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (AccountsHomePage) => Property (SubMenu) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AccountsHomePage_Class_Invalid_Property_SubMenuNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSubMenu = "SubMenuNotPresent";
            var accountsHomePage  = new AccountsHomePage();

            // Act , Assert
            Should.NotThrow(() => accountsHomePage.GetType().GetProperty(propertyNameSubMenu));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AccountsHomePage_SubMenu_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSubMenu = "SubMenu";
            var accountsHomePage  = new AccountsHomePage();
            var propertyInfo  = accountsHomePage.GetType().GetProperty(propertyNameSubMenu);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AccountsHomePage) => Property (UserSession) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AccountsHomePage_UserSession_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUserSession = "UserSession";
            var accountsHomePage = new AccountsHomePage();
            var randomString = Fixture.Create<string>();
            var propertyInfo = accountsHomePage.GetType().GetProperty(propertyNameUserSession);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(accountsHomePage, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (AccountsHomePage) => Property (UserSession) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AccountsHomePage_Class_Invalid_Property_UserSessionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUserSession = "UserSessionNotPresent";
            var accountsHomePage  = new AccountsHomePage();

            // Act , Assert
            Should.NotThrow(() => accountsHomePage.GetType().GetProperty(propertyNameUserSession));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AccountsHomePage_UserSession_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUserSession = "UserSession";
            var accountsHomePage  = new AccountsHomePage();
            var propertyInfo  = accountsHomePage.GetType().GetProperty(propertyNameUserSession);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AccountsHomePage) => Property (virtualPath) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AccountsHomePage_Class_Invalid_Property_virtualPathNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamevirtualPath = "virtualPathNotPresent";
            var accountsHomePage  = new AccountsHomePage();

            // Act , Assert
            Should.NotThrow(() => accountsHomePage.GetType().GetProperty(propertyNamevirtualPath));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AccountsHomePage_virtualPath_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamevirtualPath = "virtualPath";
            var accountsHomePage  = new AccountsHomePage();
            var propertyInfo  = accountsHomePage.GetType().GetProperty(propertyNamevirtualPath);

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
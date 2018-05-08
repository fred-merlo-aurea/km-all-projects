using System;
using System.Reflection;
using System.Web.UI.WebControls;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using ecn.accounts.MasterPages;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Objects.Accounts;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.MasterPages
{
    [TestFixture]
    public class AccountMenuFunctionsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (AccountMenuFunctions) => Method (MenuMenuItemDataBound) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void AccountMenuFunctions_MenuMenuItemDataBound_Static_Method_With_4_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var homePage = Fixture.Create<bool>();
            Object[] parametersOfMenuMenuItemDataBound = { null, homePage };
            System.Exception exception, invokeException;
            var accountMenuFunctions = CreateAnalyzer.CreateOrReturnStaticInstance<AccountMenuFunctions>(Fixture, out exception);
            var methodName = "MenuMenuItemDataBound";

            // Act
            var menuMenuItemDataBoundMethodInfo1 = accountMenuFunctions.GetType().GetMethod(methodName);
            var menuMenuItemDataBoundMethodInfo2 = accountMenuFunctions.GetType().GetMethod(methodName);
            var returnType1 = menuMenuItemDataBoundMethodInfo1.ReturnType;
            var returnType2 = menuMenuItemDataBoundMethodInfo2.ReturnType;

            // Assert
            parametersOfMenuMenuItemDataBound.ShouldNotBeNull();
            accountMenuFunctions.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            menuMenuItemDataBoundMethodInfo1.ShouldNotBeNull();
            menuMenuItemDataBoundMethodInfo2.ShouldNotBeNull();
            menuMenuItemDataBoundMethodInfo1.ShouldBe(menuMenuItemDataBoundMethodInfo2);
            if (menuMenuItemDataBoundMethodInfo1.DoesInvokeThrow(accountMenuFunctions, out invokeException, parametersOfMenuMenuItemDataBound))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => menuMenuItemDataBoundMethodInfo1.Invoke(accountMenuFunctions, parametersOfMenuMenuItemDataBound), exceptionType: invokeException.GetType());
                Should.Throw(() => menuMenuItemDataBoundMethodInfo2.Invoke(accountMenuFunctions, parametersOfMenuMenuItemDataBound), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => menuMenuItemDataBoundMethodInfo1.Invoke(accountMenuFunctions, parametersOfMenuMenuItemDataBound));
                Should.NotThrow(() => menuMenuItemDataBoundMethodInfo2.Invoke(accountMenuFunctions, parametersOfMenuMenuItemDataBound));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void AccountMenuFunctions_MenuMenuItemDataBound_Static_Method_With_4_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            System.Exception exception, exception1, exception2, exception3, exception4;
            var accountMenuFunctions = CreateAnalyzer.CreateOrReturnStaticInstance<AccountMenuFunctions>(Fixture, out exception);
            var methodName = "MenuMenuItemDataBound";

            if (accountMenuFunctions != null)
            {
                // Act
                var menuMenuItemDataBoundMethodInfo1 = accountMenuFunctions.GetType().GetMethod(methodName);
                var menuMenuItemDataBoundMethodInfo2 = accountMenuFunctions.GetType().GetMethod(methodName);
                var returnType1 = menuMenuItemDataBoundMethodInfo1.ReturnType;
                var returnType2 = menuMenuItemDataBoundMethodInfo2.ReturnType;
                menuMenuItemDataBoundMethodInfo1.InvokeMethodInfo(accountMenuFunctions, out exception1);
                menuMenuItemDataBoundMethodInfo2.InvokeMethodInfo(accountMenuFunctions, out exception2);
                menuMenuItemDataBoundMethodInfo1.InvokeMethodInfo(accountMenuFunctions, out exception3);
                menuMenuItemDataBoundMethodInfo2.InvokeMethodInfo(accountMenuFunctions, out exception4);

                // Assert

                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                accountMenuFunctions.ShouldNotBeNull();
                menuMenuItemDataBoundMethodInfo1.ShouldNotBeNull();
                menuMenuItemDataBoundMethodInfo2.ShouldNotBeNull();
                if (exception1 != null)
                {
                    Should.Throw(() => menuMenuItemDataBoundMethodInfo1.Invoke(accountMenuFunctions, null), exceptionType: exception1.GetType());
                    Should.Throw(() => menuMenuItemDataBoundMethodInfo2.Invoke(accountMenuFunctions, null), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => menuMenuItemDataBoundMethodInfo1.Invoke(accountMenuFunctions, null));
                    Should.NotThrow(() => menuMenuItemDataBoundMethodInfo2.Invoke(accountMenuFunctions, null));
                }

                if (exception1 != null)
                {
                    Should.Throw(() => menuMenuItemDataBoundMethodInfo1.Invoke(accountMenuFunctions, null), exceptionType: exception1.GetType());
                    Should.Throw(() => menuMenuItemDataBoundMethodInfo2.Invoke(accountMenuFunctions, null), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => menuMenuItemDataBoundMethodInfo1.Invoke(accountMenuFunctions, null));
                    Should.NotThrow(() => menuMenuItemDataBoundMethodInfo2.Invoke(accountMenuFunctions, null));
                }

                Should.Throw<System.Exception>(() => menuMenuItemDataBoundMethodInfo1.Invoke(accountMenuFunctions, null));
                Should.Throw<System.Exception>(() => menuMenuItemDataBoundMethodInfo2.Invoke(accountMenuFunctions, null));
                Should.Throw<TargetParameterCountException>(() => menuMenuItemDataBoundMethodInfo1.Invoke(accountMenuFunctions, null));
                Should.Throw<TargetParameterCountException>(() => menuMenuItemDataBoundMethodInfo2.Invoke(accountMenuFunctions, null));
                Should.Throw<TargetParameterCountException>(() => menuMenuItemDataBoundMethodInfo1.Invoke(accountMenuFunctions, null));
                Should.Throw<TargetParameterCountException>(() => menuMenuItemDataBoundMethodInfo2.Invoke(accountMenuFunctions, null));
            }
            else
            {
                // Act, Assert
                accountMenuFunctions.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (AccountMenuFunctions) => Method (RemoveMenuIfNonAuthorized) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void AccountMenuFunctions_RemoveMenuIfNonAuthorized_Static_Method_With_3_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            Object[] parametersOutRanged = { null, null, null, null };
            Object[] parametersInDifferentNumber = { null, null };
            System.Exception exception, exception1, exception2, exception3, exception4;
            var accountMenuFunctions = CreateAnalyzer.CreateOrReturnStaticInstance<AccountMenuFunctions>(Fixture, out exception);
            var methodName = "RemoveMenuIfNonAuthorized";

            if (accountMenuFunctions != null)
            {
                // Act
                var removeMenuIfNonAuthorizedMethodInfo1 = accountMenuFunctions.GetType().GetMethod(methodName);
                var removeMenuIfNonAuthorizedMethodInfo2 = accountMenuFunctions.GetType().GetMethod(methodName);
                var returnType1 = removeMenuIfNonAuthorizedMethodInfo1.ReturnType;
                var returnType2 = removeMenuIfNonAuthorizedMethodInfo2.ReturnType;
                removeMenuIfNonAuthorizedMethodInfo1.InvokeMethodInfo(accountMenuFunctions, out exception1, parametersOutRanged);
                removeMenuIfNonAuthorizedMethodInfo2.InvokeMethodInfo(accountMenuFunctions, out exception2, parametersOutRanged);
                removeMenuIfNonAuthorizedMethodInfo1.InvokeMethodInfo(accountMenuFunctions, out exception3, parametersInDifferentNumber);
                removeMenuIfNonAuthorizedMethodInfo2.InvokeMethodInfo(accountMenuFunctions, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                accountMenuFunctions.ShouldNotBeNull();
                removeMenuIfNonAuthorizedMethodInfo1.ShouldNotBeNull();
                removeMenuIfNonAuthorizedMethodInfo2.ShouldNotBeNull();
                if (exception1 != null)
                {
                    Should.Throw(() => removeMenuIfNonAuthorizedMethodInfo1.Invoke(accountMenuFunctions, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => removeMenuIfNonAuthorizedMethodInfo2.Invoke(accountMenuFunctions, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => removeMenuIfNonAuthorizedMethodInfo1.Invoke(accountMenuFunctions, parametersOutRanged));
                    Should.NotThrow(() => removeMenuIfNonAuthorizedMethodInfo2.Invoke(accountMenuFunctions, parametersOutRanged));
                }

                if (exception1 != null)
                {
                    Should.Throw(() => removeMenuIfNonAuthorizedMethodInfo1.Invoke(accountMenuFunctions, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => removeMenuIfNonAuthorizedMethodInfo2.Invoke(accountMenuFunctions, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => removeMenuIfNonAuthorizedMethodInfo1.Invoke(accountMenuFunctions, parametersOutRanged));
                    Should.NotThrow(() => removeMenuIfNonAuthorizedMethodInfo2.Invoke(accountMenuFunctions, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => removeMenuIfNonAuthorizedMethodInfo1.Invoke(accountMenuFunctions, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => removeMenuIfNonAuthorizedMethodInfo2.Invoke(accountMenuFunctions, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => removeMenuIfNonAuthorizedMethodInfo1.Invoke(accountMenuFunctions, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => removeMenuIfNonAuthorizedMethodInfo2.Invoke(accountMenuFunctions, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => removeMenuIfNonAuthorizedMethodInfo1.Invoke(accountMenuFunctions, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => removeMenuIfNonAuthorizedMethodInfo2.Invoke(accountMenuFunctions, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                accountMenuFunctions.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
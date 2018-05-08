using System;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using ecn.accounts;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests
{
    [TestFixture]
    public class AutoLoginTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (AutoLogin) => Method (ProcessLogin) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void AutoLogin_ProcessLogin_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var strUser = Fixture.Create<String>();
            var strPassword = Fixture.Create<String>();
            Object[] parametersOfProcessLogin = {strUser, strPassword};
            System.Exception exception, invokeException;
            var autoLogin  = CreateAnalyzer.CreateOrReturnStaticInstance<AutoLogin>(Fixture, out exception);
            var methodName = "ProcessLogin";

            // Act
            var processLoginMethodInfo1 = autoLogin.GetType().GetMethod(methodName);
            var processLoginMethodInfo2 = autoLogin.GetType().GetMethod(methodName);
            var returnType1 = processLoginMethodInfo1.ReturnType;
            var returnType2 = processLoginMethodInfo2.ReturnType;

            // Assert
            parametersOfProcessLogin.ShouldNotBeNull();
            autoLogin.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            processLoginMethodInfo1.ShouldNotBeNull();
            processLoginMethodInfo2.ShouldNotBeNull();
            processLoginMethodInfo1.ShouldBe(processLoginMethodInfo2);
            if(processLoginMethodInfo1.DoesInvokeThrow(autoLogin, out invokeException, parametersOfProcessLogin))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => processLoginMethodInfo1.Invoke(autoLogin, parametersOfProcessLogin), exceptionType: invokeException.GetType());
                Should.Throw(() => processLoginMethodInfo2.Invoke(autoLogin, parametersOfProcessLogin), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => processLoginMethodInfo1.Invoke(autoLogin, parametersOfProcessLogin));
                Should.NotThrow(() => processLoginMethodInfo2.Invoke(autoLogin, parametersOfProcessLogin));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void AutoLogin_ProcessLogin_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var strUser = Fixture.Create<String>();
            var strPassword = Fixture.Create<String>();
            Object[] parametersOutRanged = {strUser, strPassword, null};
            Object[] parametersInDifferentNumber = {strUser};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var autoLogin  = CreateAnalyzer.CreateOrReturnStaticInstance<AutoLogin>(Fixture, out exception);
            var methodName = "ProcessLogin";

            if(autoLogin != null)
            {
                // Act
                var processLoginMethodInfo1 = autoLogin.GetType().GetMethod(methodName);
                var processLoginMethodInfo2 = autoLogin.GetType().GetMethod(methodName);
                var returnType1 = processLoginMethodInfo1.ReturnType;
                var returnType2 = processLoginMethodInfo2.ReturnType;
                processLoginMethodInfo1.InvokeMethodInfo(autoLogin, out exception1, parametersOutRanged);
                processLoginMethodInfo2.InvokeMethodInfo(autoLogin, out exception2, parametersOutRanged);
                processLoginMethodInfo1.InvokeMethodInfo(autoLogin, out exception3, parametersInDifferentNumber);
                processLoginMethodInfo2.InvokeMethodInfo(autoLogin, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                autoLogin.ShouldNotBeNull();
                processLoginMethodInfo1.ShouldNotBeNull();
                processLoginMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => processLoginMethodInfo1.Invoke(autoLogin, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => processLoginMethodInfo2.Invoke(autoLogin, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => processLoginMethodInfo1.Invoke(autoLogin, parametersOutRanged));
                    Should.NotThrow(() => processLoginMethodInfo2.Invoke(autoLogin, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => processLoginMethodInfo1.Invoke(autoLogin, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => processLoginMethodInfo2.Invoke(autoLogin, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => processLoginMethodInfo1.Invoke(autoLogin, parametersOutRanged));
                    Should.NotThrow(() => processLoginMethodInfo2.Invoke(autoLogin, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => processLoginMethodInfo1.Invoke(autoLogin, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => processLoginMethodInfo2.Invoke(autoLogin, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => processLoginMethodInfo1.Invoke(autoLogin, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => processLoginMethodInfo2.Invoke(autoLogin, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => processLoginMethodInfo1.Invoke(autoLogin, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => processLoginMethodInfo2.Invoke(autoLogin, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                autoLogin.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
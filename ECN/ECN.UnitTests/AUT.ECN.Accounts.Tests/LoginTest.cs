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
    public class LoginTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (login) => Method (ProcessLogin) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void login_ProcessLogin_Method_With_4_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var strUser = Fixture.Create<String>();
            var strPassword = Fixture.Create<String>();
            var chkPersistLogin = Fixture.Create<bool>();
            var redirectApp = Fixture.Create<String>();
            Object[] parametersOfProcessLogin = {strUser, strPassword, chkPersistLogin, redirectApp};
            System.Exception exception, invokeException;
            var login  = CreateAnalyzer.CreateOrReturnStaticInstance<login>(Fixture, out exception);
            var methodName = "ProcessLogin";

            // Act
            var processLoginMethodInfo1 = login.GetType().GetMethod(methodName);
            var processLoginMethodInfo2 = login.GetType().GetMethod(methodName);
            var returnType1 = processLoginMethodInfo1.ReturnType;
            var returnType2 = processLoginMethodInfo2.ReturnType;

            // Assert
            parametersOfProcessLogin.ShouldNotBeNull();
            login.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            processLoginMethodInfo1.ShouldNotBeNull();
            processLoginMethodInfo2.ShouldNotBeNull();
            processLoginMethodInfo1.ShouldBe(processLoginMethodInfo2);
            if(processLoginMethodInfo1.DoesInvokeThrow(login, out invokeException, parametersOfProcessLogin))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => processLoginMethodInfo1.Invoke(login, parametersOfProcessLogin), exceptionType: invokeException.GetType());
                Should.Throw(() => processLoginMethodInfo2.Invoke(login, parametersOfProcessLogin), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => processLoginMethodInfo1.Invoke(login, parametersOfProcessLogin));
                Should.NotThrow(() => processLoginMethodInfo2.Invoke(login, parametersOfProcessLogin));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void login_ProcessLogin_Method_With_4_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var strUser = Fixture.Create<String>();
            var strPassword = Fixture.Create<String>();
            var chkPersistLogin = Fixture.Create<bool>();
            var redirectApp = Fixture.Create<String>();
            Object[] parametersOutRanged = {strUser, strPassword, chkPersistLogin, redirectApp, null};
            Object[] parametersInDifferentNumber = {strUser, strPassword, chkPersistLogin};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var login  = CreateAnalyzer.CreateOrReturnStaticInstance<login>(Fixture, out exception);
            var methodName = "ProcessLogin";

            if(login != null)
            {
                // Act
                var processLoginMethodInfo1 = login.GetType().GetMethod(methodName);
                var processLoginMethodInfo2 = login.GetType().GetMethod(methodName);
                var returnType1 = processLoginMethodInfo1.ReturnType;
                var returnType2 = processLoginMethodInfo2.ReturnType;
                processLoginMethodInfo1.InvokeMethodInfo(login, out exception1, parametersOutRanged);
                processLoginMethodInfo2.InvokeMethodInfo(login, out exception2, parametersOutRanged);
                processLoginMethodInfo1.InvokeMethodInfo(login, out exception3, parametersInDifferentNumber);
                processLoginMethodInfo2.InvokeMethodInfo(login, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                login.ShouldNotBeNull();
                processLoginMethodInfo1.ShouldNotBeNull();
                processLoginMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => processLoginMethodInfo1.Invoke(login, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => processLoginMethodInfo2.Invoke(login, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => processLoginMethodInfo1.Invoke(login, parametersOutRanged));
                    Should.NotThrow(() => processLoginMethodInfo2.Invoke(login, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => processLoginMethodInfo1.Invoke(login, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => processLoginMethodInfo2.Invoke(login, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => processLoginMethodInfo1.Invoke(login, parametersOutRanged));
                    Should.NotThrow(() => processLoginMethodInfo2.Invoke(login, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => processLoginMethodInfo1.Invoke(login, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => processLoginMethodInfo2.Invoke(login, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => processLoginMethodInfo1.Invoke(login, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => processLoginMethodInfo2.Invoke(login, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => processLoginMethodInfo1.Invoke(login, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => processLoginMethodInfo2.Invoke(login, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                login.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
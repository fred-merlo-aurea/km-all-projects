using System;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using ecn.accounts.usersmanager;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Users
{
    [TestFixture]
    public partial class UserDetailTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (userdetail) => Method (IsContentCreator) (Return Type :  bool) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void userdetail_IsContentCreator_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var userId = Fixture.Create<int>();
            Object[] parametersOfIsContentCreator = {userId};
            System.Exception exception, invokeException;
            var userdetail  = CreateAnalyzer.CreateOrReturnStaticInstance<userdetail>(Fixture, out exception);
            var methodName = "IsContentCreator";

            // Act
            var isContentCreatorMethodInfo1 = userdetail.GetType().GetMethod(methodName);
            var isContentCreatorMethodInfo2 = userdetail.GetType().GetMethod(methodName);
            var returnType1 = isContentCreatorMethodInfo1.ReturnType;
            var returnType2 = isContentCreatorMethodInfo2.ReturnType;

            // Assert
            parametersOfIsContentCreator.ShouldNotBeNull();
            userdetail.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            isContentCreatorMethodInfo1.ShouldNotBeNull();
            isContentCreatorMethodInfo2.ShouldNotBeNull();
            isContentCreatorMethodInfo1.ShouldBe(isContentCreatorMethodInfo2);
            if(isContentCreatorMethodInfo1.DoesInvokeThrow(userdetail, out invokeException, parametersOfIsContentCreator))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => isContentCreatorMethodInfo1.Invoke(userdetail, parametersOfIsContentCreator), exceptionType: invokeException.GetType());
                Should.Throw(() => isContentCreatorMethodInfo2.Invoke(userdetail, parametersOfIsContentCreator), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => isContentCreatorMethodInfo1.Invoke(userdetail, parametersOfIsContentCreator));
                Should.NotThrow(() => isContentCreatorMethodInfo2.Invoke(userdetail, parametersOfIsContentCreator));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void userdetail_IsContentCreator_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var userId = Fixture.Create<int>();
            Object[] parametersOutRanged = {userId, null};
            Object[] parametersInDifferentNumber = {};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var userdetail  = CreateAnalyzer.CreateOrReturnStaticInstance<userdetail>(Fixture, out exception);
            var methodName = "IsContentCreator";

            if(userdetail != null)
            {
                // Act
                var isContentCreatorMethodInfo1 = userdetail.GetType().GetMethod(methodName);
                var isContentCreatorMethodInfo2 = userdetail.GetType().GetMethod(methodName);
                var returnType1 = isContentCreatorMethodInfo1.ReturnType;
                var returnType2 = isContentCreatorMethodInfo2.ReturnType;
                var result1 = isContentCreatorMethodInfo1.GetResultMethodInfo<userdetail, bool>(userdetail, out exception1, parametersOutRanged);
                var result2 = isContentCreatorMethodInfo2.GetResultMethodInfo<userdetail, bool>(userdetail, out exception2, parametersOutRanged);
                var result3 = isContentCreatorMethodInfo1.GetResultMethodInfo<userdetail, bool>(userdetail, out exception3, parametersInDifferentNumber);
                var result4 = isContentCreatorMethodInfo2.GetResultMethodInfo<userdetail, bool>(userdetail, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                userdetail.ShouldNotBeNull();
                isContentCreatorMethodInfo1.ShouldNotBeNull();
                isContentCreatorMethodInfo2.ShouldNotBeNull();
                result1.ShouldBe(result2);
                result3.ShouldBe(result4);
                if(exception1 != null)
                {
                    Should.Throw(() => isContentCreatorMethodInfo1.Invoke(userdetail, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => isContentCreatorMethodInfo2.Invoke(userdetail, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => isContentCreatorMethodInfo1.Invoke(userdetail, parametersOutRanged));
                    Should.NotThrow(() => isContentCreatorMethodInfo2.Invoke(userdetail, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => isContentCreatorMethodInfo1.Invoke(userdetail, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => isContentCreatorMethodInfo2.Invoke(userdetail, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => isContentCreatorMethodInfo1.Invoke(userdetail, parametersOutRanged));
                    Should.NotThrow(() => isContentCreatorMethodInfo2.Invoke(userdetail, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => isContentCreatorMethodInfo1.Invoke(userdetail, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => isContentCreatorMethodInfo2.Invoke(userdetail, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => isContentCreatorMethodInfo1.Invoke(userdetail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => isContentCreatorMethodInfo2.Invoke(userdetail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => isContentCreatorMethodInfo1.Invoke(userdetail, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => isContentCreatorMethodInfo2.Invoke(userdetail, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                userdetail.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (userdetail) => Method (Save) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void userdetail_Save_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOfSave = {sender, e};
            System.Exception exception, invokeException;
            var userdetail  = CreateAnalyzer.CreateOrReturnStaticInstance<userdetail>(Fixture, out exception);
            var methodName = "Save";

            // Act
            var saveMethodInfo1 = userdetail.GetType().GetMethod(methodName);
            var saveMethodInfo2 = userdetail.GetType().GetMethod(methodName);
            var returnType1 = saveMethodInfo1.ReturnType;
            var returnType2 = saveMethodInfo2.ReturnType;

            // Assert
            parametersOfSave.ShouldNotBeNull();
            userdetail.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            saveMethodInfo1.ShouldNotBeNull();
            saveMethodInfo2.ShouldNotBeNull();
            saveMethodInfo1.ShouldBe(saveMethodInfo2);
            if(saveMethodInfo1.DoesInvokeThrow(userdetail, out invokeException, parametersOfSave))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => saveMethodInfo1.Invoke(userdetail, parametersOfSave), exceptionType: invokeException.GetType());
                Should.Throw(() => saveMethodInfo2.Invoke(userdetail, parametersOfSave), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => saveMethodInfo1.Invoke(userdetail, parametersOfSave));
                Should.NotThrow(() => saveMethodInfo2.Invoke(userdetail, parametersOfSave));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void userdetail_Save_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOutRanged = {sender, e, null};
            Object[] parametersInDifferentNumber = {sender};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var userdetail  = CreateAnalyzer.CreateOrReturnStaticInstance<userdetail>(Fixture, out exception);
            var methodName = "Save";

            if(userdetail != null)
            {
                // Act
                var saveMethodInfo1 = userdetail.GetType().GetMethod(methodName);
                var saveMethodInfo2 = userdetail.GetType().GetMethod(methodName);
                var returnType1 = saveMethodInfo1.ReturnType;
                var returnType2 = saveMethodInfo2.ReturnType;
                saveMethodInfo1.InvokeMethodInfo(userdetail, out exception1, parametersOutRanged);
                saveMethodInfo2.InvokeMethodInfo(userdetail, out exception2, parametersOutRanged);
                saveMethodInfo1.InvokeMethodInfo(userdetail, out exception3, parametersInDifferentNumber);
                saveMethodInfo2.InvokeMethodInfo(userdetail, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                userdetail.ShouldNotBeNull();
                saveMethodInfo1.ShouldNotBeNull();
                saveMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => saveMethodInfo1.Invoke(userdetail, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => saveMethodInfo2.Invoke(userdetail, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => saveMethodInfo1.Invoke(userdetail, parametersOutRanged));
                    Should.NotThrow(() => saveMethodInfo2.Invoke(userdetail, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => saveMethodInfo1.Invoke(userdetail, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => saveMethodInfo2.Invoke(userdetail, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => saveMethodInfo1.Invoke(userdetail, parametersOutRanged));
                    Should.NotThrow(() => saveMethodInfo2.Invoke(userdetail, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => saveMethodInfo1.Invoke(userdetail, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => saveMethodInfo2.Invoke(userdetail, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => saveMethodInfo1.Invoke(userdetail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => saveMethodInfo2.Invoke(userdetail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => saveMethodInfo1.Invoke(userdetail, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => saveMethodInfo2.Invoke(userdetail, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                userdetail.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
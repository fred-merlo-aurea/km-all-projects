using System;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using ecn.accounts.main.users;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Users
{
    [TestFixture]
    public  class EditUserProfileTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (EditUserProfile) => Method (SaveProfile) (Return Type :  string) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void EditUserProfile_SaveProfile_Method_With_No_Parameters_Call_With_Reflection_Reflection_Return_Data_Test()
        {
            // Arrange
            Object[] parametersOfSaveProfile = {};
            System.Exception exception, exception1, exception2;
            var editUserProfile  = CreateAnalyzer.CreateOrReturnStaticInstance<EditUserProfile>(Fixture, out exception);
            var methodName = "SaveProfile";

            if(editUserProfile != null)
            {
                // Act
                var saveProfileMethodInfo1 = editUserProfile.GetType().GetMethod(methodName);
                var saveProfileMethodInfo2 = editUserProfile.GetType().GetMethod(methodName);
                var returnType1 = saveProfileMethodInfo1.ReturnType;
                var returnType2 = saveProfileMethodInfo2.ReturnType;
                var result1 = saveProfileMethodInfo1.GetResultMethodInfo<EditUserProfile, string>(editUserProfile, out exception1, parametersOfSaveProfile);
                var result2 = saveProfileMethodInfo2.GetResultMethodInfo<EditUserProfile, string>(editUserProfile, out exception2, parametersOfSaveProfile);

                // Assert
                parametersOfSaveProfile.ShouldNotBeNull();
                editUserProfile.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                saveProfileMethodInfo1.ShouldNotBeNull();
                saveProfileMethodInfo2.ShouldNotBeNull();
                saveProfileMethodInfo1.ShouldBe(saveProfileMethodInfo2);
                if(exception1 == null)
                {
                    result1.ShouldBe(result2);
                    Should.NotThrow(() => saveProfileMethodInfo1.Invoke(editUserProfile, parametersOfSaveProfile));
                    Should.NotThrow(() => saveProfileMethodInfo2.Invoke(editUserProfile, parametersOfSaveProfile));
                }
                else
                {
                    result1.ShouldBeNull();
                    result2.ShouldBeNull();
                    exception1.ShouldNotBeNull();
                    exception2.ShouldNotBeNull();
                    Should.Throw(() => saveProfileMethodInfo1.Invoke(editUserProfile, parametersOfSaveProfile), exceptionType: exception1.GetType());
                    Should.Throw(() => saveProfileMethodInfo2.Invoke(editUserProfile, parametersOfSaveProfile), exceptionType: exception2.GetType());
                }
            }
            else
            {
                // Act, Assert
                editUserProfile.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void EditUserProfile_SaveProfile_Method_With_No_Parameters_Call_With_Reflection_Test()
        {
            // Arrange
            System.Exception exception, exception1, exception2;
            var editUserProfile  = CreateAnalyzer.CreateOrReturnStaticInstance<EditUserProfile>(Fixture, out exception);
            var methodName = "SaveProfile";

            if(editUserProfile != null)
            {
                // Act
                var saveProfileMethodInfo1 = editUserProfile.GetType().GetMethod(methodName);
                var saveProfileMethodInfo2 = editUserProfile.GetType().GetMethod(methodName);
                var returnType1 = saveProfileMethodInfo1.ReturnType;
                var returnType2 = saveProfileMethodInfo2.ReturnType;
                var result1 = saveProfileMethodInfo1.GetResultMethodInfo<EditUserProfile, string>(editUserProfile, out exception1);
                var result2 = saveProfileMethodInfo2.GetResultMethodInfo<EditUserProfile, string>(editUserProfile, out exception2);

                // Assert
                editUserProfile.ShouldNotBeNull();
                saveProfileMethodInfo1.ShouldNotBeNull();
                saveProfileMethodInfo2.ShouldNotBeNull();
                saveProfileMethodInfo1.ShouldBe(saveProfileMethodInfo2);
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                if(exception1 == null)
                {
                    result1.ShouldBe(result2);
                    Should.NotThrow(() => saveProfileMethodInfo1.Invoke(editUserProfile, null));
                    Should.NotThrow(() => saveProfileMethodInfo2.Invoke(editUserProfile, null));
                }
                else
                {
                    result1.ShouldBeNull();
                    result2.ShouldBeNull();
                    Should.Throw(() => saveProfileMethodInfo1.Invoke(editUserProfile, null), exceptionType: exception1.GetType());
                    Should.Throw(() => saveProfileMethodInfo2.Invoke(editUserProfile, null), exceptionType: exception2.GetType());
                }
            }
            else
            {
                // Act, Assert
                editUserProfile.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void EditUserProfile_SaveProfile_Method_With_No_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            Object[] parametersOutRanged = {null, null};
            System.Exception exception;
            var editUserProfile  = CreateAnalyzer.CreateOrReturnStaticInstance<EditUserProfile>(Fixture, out exception);
            var methodName = "SaveProfile";

            if(editUserProfile != null)
            {
                // Act
                var saveProfileMethodInfo1 = editUserProfile.GetType().GetMethod(methodName);
                var saveProfileMethodInfo2 = editUserProfile.GetType().GetMethod(methodName);
                var returnType1 = saveProfileMethodInfo1.ReturnType;
                var returnType2 = saveProfileMethodInfo2.ReturnType;

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                editUserProfile.ShouldNotBeNull();
                saveProfileMethodInfo1.ShouldNotBeNull();
                saveProfileMethodInfo2.ShouldNotBeNull();
                saveProfileMethodInfo1.ShouldBe(saveProfileMethodInfo2);
                Should.Throw<System.Exception>(() => saveProfileMethodInfo1.Invoke(editUserProfile, parametersOutRanged));
                Should.Throw<System.Exception>(() => saveProfileMethodInfo2.Invoke(editUserProfile, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => saveProfileMethodInfo1.Invoke(editUserProfile, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => saveProfileMethodInfo2.Invoke(editUserProfile, parametersOutRanged));
            }
            else
            {
                // Act, Assert
                editUserProfile.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (EditUserProfile) => Method (LoadUser) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void EditUserProfile_LoadUser_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var user = Fixture.Create<User>();
            Object[] parametersOfLoadUser = {user};
            System.Exception exception, invokeException;
            var editUserProfile  = CreateAnalyzer.CreateOrReturnStaticInstance<EditUserProfile>(Fixture, out exception);
            var methodName = "LoadUser";

            // Act
            var loadUserMethodInfo1 = editUserProfile.GetType().GetMethod(methodName);
            var loadUserMethodInfo2 = editUserProfile.GetType().GetMethod(methodName);
            var returnType1 = loadUserMethodInfo1.ReturnType;
            var returnType2 = loadUserMethodInfo2.ReturnType;

            // Assert
            parametersOfLoadUser.ShouldNotBeNull();
            editUserProfile.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            loadUserMethodInfo1.ShouldNotBeNull();
            loadUserMethodInfo2.ShouldNotBeNull();
            loadUserMethodInfo1.ShouldBe(loadUserMethodInfo2);
            if(loadUserMethodInfo1.DoesInvokeThrow(editUserProfile, out invokeException, parametersOfLoadUser))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => loadUserMethodInfo1.Invoke(editUserProfile, parametersOfLoadUser), exceptionType: invokeException.GetType());
                Should.Throw(() => loadUserMethodInfo2.Invoke(editUserProfile, parametersOfLoadUser), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => loadUserMethodInfo1.Invoke(editUserProfile, parametersOfLoadUser));
                Should.NotThrow(() => loadUserMethodInfo2.Invoke(editUserProfile, parametersOfLoadUser));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void EditUserProfile_LoadUser_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var user = Fixture.Create<User>();
            Object[] parametersOutRanged = {user, null};
            Object[] parametersInDifferentNumber = {};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var editUserProfile  = CreateAnalyzer.CreateOrReturnStaticInstance<EditUserProfile>(Fixture, out exception);
            var methodName = "LoadUser";

            if(editUserProfile != null)
            {
                // Act
                var loadUserMethodInfo1 = editUserProfile.GetType().GetMethod(methodName);
                var loadUserMethodInfo2 = editUserProfile.GetType().GetMethod(methodName);
                var returnType1 = loadUserMethodInfo1.ReturnType;
                var returnType2 = loadUserMethodInfo2.ReturnType;
                loadUserMethodInfo1.InvokeMethodInfo(editUserProfile, out exception1, parametersOutRanged);
                loadUserMethodInfo2.InvokeMethodInfo(editUserProfile, out exception2, parametersOutRanged);
                loadUserMethodInfo1.InvokeMethodInfo(editUserProfile, out exception3, parametersInDifferentNumber);
                loadUserMethodInfo2.InvokeMethodInfo(editUserProfile, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                editUserProfile.ShouldNotBeNull();
                loadUserMethodInfo1.ShouldNotBeNull();
                loadUserMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => loadUserMethodInfo1.Invoke(editUserProfile, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => loadUserMethodInfo2.Invoke(editUserProfile, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => loadUserMethodInfo1.Invoke(editUserProfile, parametersOutRanged));
                    Should.NotThrow(() => loadUserMethodInfo2.Invoke(editUserProfile, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => loadUserMethodInfo1.Invoke(editUserProfile, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => loadUserMethodInfo2.Invoke(editUserProfile, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => loadUserMethodInfo1.Invoke(editUserProfile, parametersOutRanged));
                    Should.NotThrow(() => loadUserMethodInfo2.Invoke(editUserProfile, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => loadUserMethodInfo1.Invoke(editUserProfile, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => loadUserMethodInfo2.Invoke(editUserProfile, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => loadUserMethodInfo1.Invoke(editUserProfile, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => loadUserMethodInfo2.Invoke(editUserProfile, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => loadUserMethodInfo1.Invoke(editUserProfile, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => loadUserMethodInfo2.Invoke(editUserProfile, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                editUserProfile.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
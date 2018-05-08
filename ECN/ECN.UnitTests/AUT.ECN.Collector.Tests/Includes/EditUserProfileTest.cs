using System;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.collector.includes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Collector.Tests.Includes
{
    [TestFixture]
    public class EditUserProfileTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void EditUserProfile_LoadUser_Method_With_1_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var user = Fixture.Create<User>();
            object[] parametersOutRanged = {user, null};
            object[] parametersInDifferentNumber = { };
            var editUserProfile  = new EditUserProfile();
            var methodName = "LoadUser";

            // Act
            var loadUserMethodInfo1 = editUserProfile.GetType().GetMethod(methodName);
            var loadUserMethodInfo2 = editUserProfile.GetType().GetMethod(methodName);
            var returnType1 = loadUserMethodInfo1.ReturnType;
            var returnType2 = loadUserMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            editUserProfile.ShouldNotBeNull();
            loadUserMethodInfo1.ShouldNotBeNull();
            loadUserMethodInfo2.ShouldNotBeNull();
            loadUserMethodInfo1.ShouldBe(loadUserMethodInfo2);
            Should.Throw<Exception>(actual: () => loadUserMethodInfo1.Invoke(editUserProfile, parametersOutRanged));
            Should.Throw<Exception>(actual: () => loadUserMethodInfo2.Invoke(editUserProfile, parametersOutRanged));
            Should.Throw<Exception>(actual: () => loadUserMethodInfo1.Invoke(editUserProfile, parametersInDifferentNumber));
            Should.Throw<Exception>(actual: () => loadUserMethodInfo2.Invoke(editUserProfile, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(actual: () => loadUserMethodInfo1.Invoke(editUserProfile, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => loadUserMethodInfo2.Invoke(editUserProfile, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => loadUserMethodInfo1.Invoke(editUserProfile, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(actual: () => loadUserMethodInfo2.Invoke(editUserProfile, parametersInDifferentNumber));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void EditUserProfile_LoadUser_Method_With_1_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            object[] parametersOfLoadUser = {null};
            var editUserProfile  = new EditUserProfile();
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
            Should.Throw<Exception>(actual: () => loadUserMethodInfo1.Invoke(editUserProfile, parametersOfLoadUser));
            Should.Throw<Exception>(actual: () => loadUserMethodInfo2.Invoke(editUserProfile, parametersOfLoadUser));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void EditUserProfile_SaveProfile_Method_With_No_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            object[] parametersOutRanged = {null, null};
            var editUserProfile  = new EditUserProfile();
            var methodName = "SaveProfile";

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
            Should.Throw<Exception>(actual: () => saveProfileMethodInfo1.Invoke(editUserProfile, parametersOutRanged));
            Should.Throw<Exception>(actual: () => saveProfileMethodInfo2.Invoke(editUserProfile, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => saveProfileMethodInfo1.Invoke(editUserProfile, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => saveProfileMethodInfo2.Invoke(editUserProfile, parametersOutRanged));
        }
    }
}
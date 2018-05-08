using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Collector.Tests.MasterPages
{
    [TestFixture]
    public class CollectorTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Collector_Class_Invalid_Property_UserSessionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUserSession = "UserSessionNotPresent";
            var collector  = new ecn.collector.MasterPages.Collector();

            // Act , Assert
            Should.NotThrow(action: () => collector.GetType().GetProperty(propertyNameUserSession));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Collector_Class_Invalid_Property_virtualPathNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamevirtualPath = "virtualPathNotPresent";
            var collector  = new ecn.collector.MasterPages.Collector();

            // Act , Assert
            Should.NotThrow(action: () => collector.GetType().GetProperty(propertyNamevirtualPath));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Collector_CurrentMenuCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCurrentMenuCode = "CurrentMenuCode";
            var collector  = new ecn.collector.MasterPages.Collector();
            var propertyInfo  = collector.GetType().GetProperty(propertyNameCurrentMenuCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void Collector_HasAuthorized_Method_2_Parameters_2_Calls_Test()
        {
            // Arrange
            var userId = Fixture.Create<int>();
            var clientId = Fixture.Create<int>();
            var collector  = new ecn.collector.MasterPages.Collector();

            // Assert
            Should.Throw<Exception>(actual: () => collector.HasAuthorized(userId, clientId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void Collector_HasAuthorized_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
            var userId = Fixture.Create<int>();
            var clientId = Fixture.Create<int>();
            var collector  = new ecn.collector.MasterPages.Collector();

            // Act, Assert
            Should.Throw<Exception>(actual: () => collector.HasAuthorized(userId, clientId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void Collector_HasAuthorized_Method_With_2_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var userId = Fixture.Create<int>();
            var clientId = Fixture.Create<int>();
            var collector  = new ecn.collector.MasterPages.Collector();

            // Act
            Func<bool> hasAuthorized1 = () => collector.HasAuthorized(userId, clientId);
            Func<bool> hasAuthorized2 = () => collector.HasAuthorized(userId, clientId);
            var target1 = hasAuthorized1.Target;
            var target2 = hasAuthorized2.Target;

            // Assert
            hasAuthorized1.ShouldNotBeNull();
            hasAuthorized2.ShouldNotBeNull();
            hasAuthorized1.ShouldNotBe(hasAuthorized2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(actual: () => hasAuthorized1.Invoke());
            Should.Throw<Exception>(actual: () => hasAuthorized2.Invoke());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void Collector_HasAuthorized_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var userId = Fixture.Create<int>();
            var clientId = Fixture.Create<int>();
            object[] parametersOutRanged = {userId, clientId, null};
            object[] parametersInDifferentNumber = {userId};
            var collector  = new ecn.collector.MasterPages.Collector();
            var methodName = "HasAuthorized";

            // Act
            var hasAuthorizedMethodInfo1 = collector.GetType().GetMethod(methodName);
            var hasAuthorizedMethodInfo2 = collector.GetType().GetMethod(methodName);
            var returnType1 = hasAuthorizedMethodInfo1.ReturnType;
            var returnType2 = hasAuthorizedMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            collector.ShouldNotBeNull();
            hasAuthorizedMethodInfo1.ShouldNotBeNull();
            hasAuthorizedMethodInfo2.ShouldNotBeNull();
            hasAuthorizedMethodInfo1.ShouldBe(hasAuthorizedMethodInfo2);
            Should.Throw<Exception>(actual: () => hasAuthorizedMethodInfo1.Invoke(collector, parametersOutRanged));
            Should.Throw<Exception>(actual: () => hasAuthorizedMethodInfo2.Invoke(collector, parametersOutRanged));
            Should.Throw<Exception>(actual: () => hasAuthorizedMethodInfo1.Invoke(collector, parametersInDifferentNumber));
            Should.Throw<Exception>(actual: () => hasAuthorizedMethodInfo2.Invoke(collector, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(actual: () => hasAuthorizedMethodInfo1.Invoke(collector, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => hasAuthorizedMethodInfo2.Invoke(collector, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => hasAuthorizedMethodInfo1.Invoke(collector, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(actual: () => hasAuthorizedMethodInfo2.Invoke(collector, parametersInDifferentNumber));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void Collector_HasAuthorized_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var userId = Fixture.Create<int>();
            var clientId = Fixture.Create<int>();
            object[] parametersOfHasAuthorized = {userId, clientId};
            var collector  = new ecn.collector.MasterPages.Collector();
            var methodName = "HasAuthorized";

            // Act
            var hasAuthorizedMethodInfo1 = collector.GetType().GetMethod(methodName);
            var hasAuthorizedMethodInfo2 = collector.GetType().GetMethod(methodName);
            var returnType1 = hasAuthorizedMethodInfo1.ReturnType;
            var returnType2 = hasAuthorizedMethodInfo2.ReturnType;

            // Assert
            parametersOfHasAuthorized.ShouldNotBeNull();
            collector.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            hasAuthorizedMethodInfo1.ShouldNotBeNull();
            hasAuthorizedMethodInfo2.ShouldNotBeNull();
            hasAuthorizedMethodInfo1.ShouldBe(hasAuthorizedMethodInfo2);
            Should.Throw<Exception>(actual: () => hasAuthorizedMethodInfo1.Invoke(collector, parametersOfHasAuthorized));
            Should.Throw<Exception>(actual: () => hasAuthorizedMethodInfo2.Invoke(collector, parametersOfHasAuthorized));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Collector_UserSession_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUserSession = "UserSession";
            var collector  = new ecn.collector.MasterPages.Collector();
            var propertyInfo  = collector.GetType().GetProperty(propertyNameUserSession);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Collector_UserSession_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUserSession = "UserSession";
            var collector = new ecn.collector.MasterPages.Collector();
            var randomString = Fixture.Create<string>();
            var propertyInfo = collector.GetType().GetProperty(propertyNameUserSession);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(collector, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Collector_virtualPath_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamevirtualPath = "virtualPath";
            var collector  = new ecn.collector.MasterPages.Collector();
            var propertyInfo  = collector.GetType().GetProperty(propertyNamevirtualPath);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }
    }
}
using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Shouldly;
using AutoFixture;
using Castle.Components.DictionaryAdapter;
using NUnit.Framework;
using Moq;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Salesforce.Helpers;

namespace ECN_Framework_Entities.Salesforce.Helpers
{
    [TestFixture]
    public class PropertyHelperTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : AnyMethodCall

        #region Method call test

        #region Method Call Test : PropertyHelper => GetPropertyName (Return type :  string)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void PropertyHelper_GetPropertyName_Static_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var propertyExp = Fixture.Create<Expression<Func<dynamic, dynamic>>>();

            // Act, Assert
        	Should.Throw<Exception>(() => PropertyHelper.GetPropertyName<dynamic, dynamic>(propertyExp));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void PropertyHelper_GetPropertyName_Static_Method_1_Parameters_2_Calls_Test()
        {
            // Arrange
            var propertyExp = Fixture.Create<Expression<Func<dynamic, dynamic>>>();

            // Act
            Func<string> getPropertyName = () => PropertyHelper.GetPropertyName<dynamic, dynamic>(propertyExp);

            // Assert
            Should.Throw<Exception>(() => getPropertyName.Invoke());
            Should.Throw<Exception>(() => PropertyHelper.GetPropertyName<dynamic, dynamic>(propertyExp));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void PropertyHelper_GetPropertyName_Static_Method_With_1_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var propertyExp = Fixture.Create<Expression<Func<dynamic, dynamic>>>();

            // Act
            Func<string> getPropertyName1 = () => PropertyHelper.GetPropertyName<dynamic, dynamic>(propertyExp);
            Func<string> getPropertyName2 = () => PropertyHelper.GetPropertyName<dynamic, dynamic>(propertyExp);
            var target1 = getPropertyName1.Target;
            var target2 = getPropertyName2.Target;

            // Assert
            getPropertyName1.ShouldNotBeNull();
            getPropertyName2.ShouldNotBeNull();
            getPropertyName1.ShouldNotBe(getPropertyName2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => getPropertyName1.Invoke());
            Should.Throw<Exception>(() => getPropertyName2.Invoke());
        }

        #endregion

        #region Method Call Test : PropertyHelper => GetProperty (Return type :  PropertyDescriptor)

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void PropertyHelper_GetProperty_Static_Method_1_Parameters_Simple_Call_Test()
        {
            // Arrange
        	var propertyName = Fixture.Create<string>();

            // Act, Assert
        	Should.NotThrow(() => PropertyHelper.GetProperty<dynamic>(propertyName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void PropertyHelper_GetProperty_Static_Method_1_Parameters_2_Calls_Test()
        {
            // Arrange
            var propertyName = Fixture.Create<string>();

            // Act
            Func<dynamic> getProperty = () => PropertyHelper.GetProperty<dynamic>(propertyName);

            // Assert
            Should.NotThrow(() => getProperty.Invoke());
            Should.NotThrow(() => PropertyHelper.GetProperty<dynamic>(propertyName));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT AnyMethodCall")]
        public void PropertyHelper_GetProperty_Static_Method_With_1_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var propertyName = Fixture.Create<string>();

            // Act
            Func<dynamic> getProperty1 = () => PropertyHelper.GetProperty<dynamic>(propertyName);
            Func<dynamic> getProperty2 = () => PropertyHelper.GetProperty<dynamic>(propertyName);
            var target1 = getProperty1.Target;
            var target2 = getProperty2.Target;

            // Assert
            getProperty1.ShouldNotBeNull();
            getProperty2.ShouldNotBeNull();
            getProperty1.ShouldNotBe(getProperty2);
            target1.ShouldBe(target2);
            Should.Throw<Exception>(() => getProperty1.Invoke());
            Should.Throw<Exception>(() => getProperty2.Invoke());
        }

        #endregion

        #endregion

        #endregion

        #endregion
    }
}
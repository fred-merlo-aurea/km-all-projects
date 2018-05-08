using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;
using Shouldly;
using AutoFixture;
using NUnit.Framework;
using Moq;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_Entities.Activity.Report
{
    [TestFixture]
    public class UnsubscribeReasonTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (UnsubscribeReason) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReason_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var unsubscribeReason = Fixture.Create<UnsubscribeReason>();
            var selectedReason = Fixture.Create<string>();
            var uniqueCount = Fixture.Create<int>();
            var totalCount = Fixture.Create<int>();

            // Act
            unsubscribeReason.SelectedReason = selectedReason;
            unsubscribeReason.UniqueCount = uniqueCount;
            unsubscribeReason.TotalCount = totalCount;

            // Assert
            unsubscribeReason.SelectedReason.ShouldBe(selectedReason);
            unsubscribeReason.UniqueCount.ShouldBe(uniqueCount);
            unsubscribeReason.TotalCount.ShouldBe(totalCount);
        }

        #endregion

        #region General Getters/Setters : Class (UnsubscribeReason) => Property (SelectedReason) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReason_SelectedReason_Property_String_Type_Verify_Test()
        {
            // Arrange
            var unsubscribeReason = Fixture.Create<UnsubscribeReason>();
            unsubscribeReason.SelectedReason = Fixture.Create<string>();
            var stringType = unsubscribeReason.SelectedReason.GetType();

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

        #region General Getters/Setters : Class (UnsubscribeReason) => Property (SelectedReason) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReason_Class_Invalid_Property_SelectedReasonNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSelectedReason = "SelectedReasonNotPresent";
            var unsubscribeReason  = Fixture.Create<UnsubscribeReason>();

            // Act , Assert
            Should.NotThrow(() => unsubscribeReason.GetType().GetProperty(propertyNameSelectedReason));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReason_SelectedReason_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSelectedReason = "SelectedReason";
            var unsubscribeReason  = Fixture.Create<UnsubscribeReason>();
            var propertyInfo  = unsubscribeReason.GetType().GetProperty(propertyNameSelectedReason);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (UnsubscribeReason) => Property (TotalCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReason_TotalCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var unsubscribeReason = Fixture.Create<UnsubscribeReason>();
            unsubscribeReason.TotalCount = Fixture.Create<int>();
            var intType = unsubscribeReason.TotalCount.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (UnsubscribeReason) => Property (TotalCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReason_Class_Invalid_Property_TotalCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalCount = "TotalCountNotPresent";
            var unsubscribeReason  = Fixture.Create<UnsubscribeReason>();

            // Act , Assert
            Should.NotThrow(() => unsubscribeReason.GetType().GetProperty(propertyNameTotalCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReason_TotalCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalCount = "TotalCount";
            var unsubscribeReason  = Fixture.Create<UnsubscribeReason>();
            var propertyInfo  = unsubscribeReason.GetType().GetProperty(propertyNameTotalCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (UnsubscribeReason) => Property (UniqueCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReason_UniqueCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var unsubscribeReason = Fixture.Create<UnsubscribeReason>();
            unsubscribeReason.UniqueCount = Fixture.Create<int>();
            var intType = unsubscribeReason.UniqueCount.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (UnsubscribeReason) => Property (UniqueCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReason_Class_Invalid_Property_UniqueCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueCount = "UniqueCountNotPresent";
            var unsubscribeReason  = Fixture.Create<UnsubscribeReason>();

            // Act , Assert
            Should.NotThrow(() => unsubscribeReason.GetType().GetProperty(propertyNameUniqueCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReason_UniqueCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUniqueCount = "UniqueCount";
            var unsubscribeReason  = Fixture.Create<UnsubscribeReason>();
            var propertyInfo  = unsubscribeReason.GetType().GetProperty(propertyNameUniqueCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (UnsubscribeReason) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_UnsubscribeReason_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new UnsubscribeReason());
        }

        #endregion

        #region General Constructor : Class (UnsubscribeReason) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_UnsubscribeReason_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfUnsubscribeReason = Fixture.CreateMany<UnsubscribeReason>(2).ToList();
            var firstUnsubscribeReason = instancesOfUnsubscribeReason.FirstOrDefault();
            var lastUnsubscribeReason = instancesOfUnsubscribeReason.Last();

            // Act, Assert
            firstUnsubscribeReason.ShouldNotBeNull();
            lastUnsubscribeReason.ShouldNotBeNull();
            firstUnsubscribeReason.ShouldNotBeSameAs(lastUnsubscribeReason);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_UnsubscribeReason_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstUnsubscribeReason = new UnsubscribeReason();
            var secondUnsubscribeReason = new UnsubscribeReason();
            var thirdUnsubscribeReason = new UnsubscribeReason();
            var fourthUnsubscribeReason = new UnsubscribeReason();
            var fifthUnsubscribeReason = new UnsubscribeReason();
            var sixthUnsubscribeReason = new UnsubscribeReason();

            // Act, Assert
            firstUnsubscribeReason.ShouldNotBeNull();
            secondUnsubscribeReason.ShouldNotBeNull();
            thirdUnsubscribeReason.ShouldNotBeNull();
            fourthUnsubscribeReason.ShouldNotBeNull();
            fifthUnsubscribeReason.ShouldNotBeNull();
            sixthUnsubscribeReason.ShouldNotBeNull();
            firstUnsubscribeReason.ShouldNotBeSameAs(secondUnsubscribeReason);
            thirdUnsubscribeReason.ShouldNotBeSameAs(firstUnsubscribeReason);
            fourthUnsubscribeReason.ShouldNotBeSameAs(firstUnsubscribeReason);
            fifthUnsubscribeReason.ShouldNotBeSameAs(firstUnsubscribeReason);
            sixthUnsubscribeReason.ShouldNotBeSameAs(firstUnsubscribeReason);
            sixthUnsubscribeReason.ShouldNotBeSameAs(fourthUnsubscribeReason);
        }

        #endregion

        #endregion

        #endregion
    }
}
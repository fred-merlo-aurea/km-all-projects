using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class GroupDataFieldsDefaultTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (GroupDataFieldsDefault) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFieldsDefault_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var groupDataFieldsDefault = Fixture.Create<GroupDataFieldsDefault>();
            var gDFId = Fixture.Create<int>();
            var dataValue = Fixture.Create<string>();
            var systemValue = Fixture.Create<string>();

            // Act
            groupDataFieldsDefault.GDFID = gDFId;
            groupDataFieldsDefault.DataValue = dataValue;
            groupDataFieldsDefault.SystemValue = systemValue;

            // Assert
            groupDataFieldsDefault.GDFID.ShouldBe(gDFId);
            groupDataFieldsDefault.DataValue.ShouldBe(dataValue);
            groupDataFieldsDefault.SystemValue.ShouldBe(systemValue);
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFieldsDefault) => Property (DataValue) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFieldsDefault_DataValue_Property_String_Type_Verify_Test()
        {
            // Arrange
            var groupDataFieldsDefault = Fixture.Create<GroupDataFieldsDefault>();
            groupDataFieldsDefault.DataValue = Fixture.Create<string>();
            var stringType = groupDataFieldsDefault.DataValue.GetType();

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

        #region General Getters/Setters : Class (GroupDataFieldsDefault) => Property (DataValue) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFieldsDefault_Class_Invalid_Property_DataValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDataValue = "DataValueNotPresent";
            var groupDataFieldsDefault  = Fixture.Create<GroupDataFieldsDefault>();

            // Act , Assert
            Should.NotThrow(() => groupDataFieldsDefault.GetType().GetProperty(propertyNameDataValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFieldsDefault_DataValue_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDataValue = "DataValue";
            var groupDataFieldsDefault  = Fixture.Create<GroupDataFieldsDefault>();
            var propertyInfo  = groupDataFieldsDefault.GetType().GetProperty(propertyNameDataValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFieldsDefault) => Property (GDFID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFieldsDefault_GDFID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var groupDataFieldsDefault = Fixture.Create<GroupDataFieldsDefault>();
            groupDataFieldsDefault.GDFID = Fixture.Create<int>();
            var intType = groupDataFieldsDefault.GDFID.GetType();

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

        #region General Getters/Setters : Class (GroupDataFieldsDefault) => Property (GDFID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFieldsDefault_Class_Invalid_Property_GDFIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGDFID = "GDFIDNotPresent";
            var groupDataFieldsDefault  = Fixture.Create<GroupDataFieldsDefault>();

            // Act , Assert
            Should.NotThrow(() => groupDataFieldsDefault.GetType().GetProperty(propertyNameGDFID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFieldsDefault_GDFID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGDFID = "GDFID";
            var groupDataFieldsDefault  = Fixture.Create<GroupDataFieldsDefault>();
            var propertyInfo  = groupDataFieldsDefault.GetType().GetProperty(propertyNameGDFID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFieldsDefault) => Property (SystemValue) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFieldsDefault_SystemValue_Property_String_Type_Verify_Test()
        {
            // Arrange
            var groupDataFieldsDefault = Fixture.Create<GroupDataFieldsDefault>();
            groupDataFieldsDefault.SystemValue = Fixture.Create<string>();
            var stringType = groupDataFieldsDefault.SystemValue.GetType();

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

        #region General Getters/Setters : Class (GroupDataFieldsDefault) => Property (SystemValue) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFieldsDefault_Class_Invalid_Property_SystemValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSystemValue = "SystemValueNotPresent";
            var groupDataFieldsDefault  = Fixture.Create<GroupDataFieldsDefault>();

            // Act , Assert
            Should.NotThrow(() => groupDataFieldsDefault.GetType().GetProperty(propertyNameSystemValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFieldsDefault_SystemValue_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSystemValue = "SystemValue";
            var groupDataFieldsDefault  = Fixture.Create<GroupDataFieldsDefault>();
            var propertyInfo  = groupDataFieldsDefault.GetType().GetProperty(propertyNameSystemValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (GroupDataFieldsDefault) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GroupDataFieldsDefault_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new GroupDataFieldsDefault());
        }

        #endregion

        #region General Constructor : Class (GroupDataFieldsDefault) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GroupDataFieldsDefault_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfGroupDataFieldsDefault = Fixture.CreateMany<GroupDataFieldsDefault>(2).ToList();
            var firstGroupDataFieldsDefault = instancesOfGroupDataFieldsDefault.FirstOrDefault();
            var lastGroupDataFieldsDefault = instancesOfGroupDataFieldsDefault.Last();

            // Act, Assert
            firstGroupDataFieldsDefault.ShouldNotBeNull();
            lastGroupDataFieldsDefault.ShouldNotBeNull();
            firstGroupDataFieldsDefault.ShouldNotBeSameAs(lastGroupDataFieldsDefault);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GroupDataFieldsDefault_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstGroupDataFieldsDefault = new GroupDataFieldsDefault();
            var secondGroupDataFieldsDefault = new GroupDataFieldsDefault();
            var thirdGroupDataFieldsDefault = new GroupDataFieldsDefault();
            var fourthGroupDataFieldsDefault = new GroupDataFieldsDefault();
            var fifthGroupDataFieldsDefault = new GroupDataFieldsDefault();
            var sixthGroupDataFieldsDefault = new GroupDataFieldsDefault();

            // Act, Assert
            firstGroupDataFieldsDefault.ShouldNotBeNull();
            secondGroupDataFieldsDefault.ShouldNotBeNull();
            thirdGroupDataFieldsDefault.ShouldNotBeNull();
            fourthGroupDataFieldsDefault.ShouldNotBeNull();
            fifthGroupDataFieldsDefault.ShouldNotBeNull();
            sixthGroupDataFieldsDefault.ShouldNotBeNull();
            firstGroupDataFieldsDefault.ShouldNotBeSameAs(secondGroupDataFieldsDefault);
            thirdGroupDataFieldsDefault.ShouldNotBeSameAs(firstGroupDataFieldsDefault);
            fourthGroupDataFieldsDefault.ShouldNotBeSameAs(firstGroupDataFieldsDefault);
            fifthGroupDataFieldsDefault.ShouldNotBeSameAs(firstGroupDataFieldsDefault);
            sixthGroupDataFieldsDefault.ShouldNotBeSameAs(firstGroupDataFieldsDefault);
            sixthGroupDataFieldsDefault.ShouldNotBeSameAs(fourthGroupDataFieldsDefault);
        }

        #endregion

        #region General Constructor : Class (GroupDataFieldsDefault) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GroupDataFieldsDefault_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var gDFId = -1;
            var dataValue = string.Empty;
            var systemValue = string.Empty;

            // Act
            var groupDataFieldsDefault = new GroupDataFieldsDefault();

            // Assert
            groupDataFieldsDefault.GDFID.ShouldBe(gDFId);
            groupDataFieldsDefault.DataValue.ShouldBe(dataValue);
            groupDataFieldsDefault.SystemValue.ShouldBe(systemValue);
        }

        #endregion

        #endregion

        #endregion
    }
}
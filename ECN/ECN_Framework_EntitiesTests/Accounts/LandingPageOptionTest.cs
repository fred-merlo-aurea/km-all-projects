using System.Linq;
using Shouldly;
using NUnit.Framework;
using AutoFixture;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Accounts;

namespace ECN_Framework_EntitiesTests.Accounts
{
    [TestFixture]
    public class LandingPageOptionTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageOption_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var landingPageOption  = new LandingPageOption();
            var lPOID = Fixture.Create<int>();
            var lPID = Fixture.Create<int?>();
            var name = Fixture.Create<string>();
            var description = Fixture.Create<string>();
            var isActive = Fixture.Create<bool?>();

            // Act
            landingPageOption.LPOID = lPOID;
            landingPageOption.LPID = lPID;
            landingPageOption.Name = name;
            landingPageOption.Description = description;
            landingPageOption.IsActive = isActive;

            // Assert
            landingPageOption.LPOID.ShouldBe(lPOID);
            landingPageOption.LPID.ShouldBe(lPID);
            landingPageOption.Name.ShouldBe(name);
            landingPageOption.Description.ShouldBe(description);
            landingPageOption.IsActive.ShouldBe(isActive);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Nullable Property Test : LandingPageOption => IsActive

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsActive_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageOption = Fixture.Create<LandingPageOption>();
            var random = Fixture.Create<bool>();

            // Act , Set
            landingPageOption.IsActive = random;

            // Assert
            landingPageOption.IsActive.ShouldBe(random);
            landingPageOption.IsActive.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsActive_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageOption = Fixture.Create<LandingPageOption>();    

            // Act , Set
            landingPageOption.IsActive = null;

            // Assert
            landingPageOption.IsActive.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsActive_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsActive = "IsActive";
            var landingPageOption = Fixture.Create<LandingPageOption>();
            var propertyInfo = landingPageOption.GetType().GetProperty(constIsActive);

            // Act , Set
            propertyInfo.SetValue(landingPageOption, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageOption.IsActive.ShouldBeNull();
            landingPageOption.IsActive.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageOption_Class_Invalid_Property_IsActive_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsActive = "IsActive";
            var landingPageOption  = Fixture.Create<LandingPageOption>();

            // Act , Assert
            Should.NotThrow(() => landingPageOption.GetType().GetProperty(constIsActive));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsActive_Is_Present_In_LandingPageOption_Class_As_Public_Test()
        {
            // Arrange
            const string constIsActive = "IsActive";
            var landingPageOption  = Fixture.Create<LandingPageOption>();
            var propertyInfo  = landingPageOption.GetType().GetProperty(constIsActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : LandingPageOption => LPOID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPOID_Int_Type_Verify_Test()
        {
            // Arrange
            var landingPageOption = Fixture.Create<LandingPageOption>();
            var intType = landingPageOption.LPOID.GetType();

            // Act
            var isTypeInt = typeof(int).Equals(intType);    
            var isTypeNullableInt = typeof(int?).Equals(intType);
            var isTypeString = typeof(string).Equals(intType);
            var isTypeDecimal = typeof(decimal).Equals(intType);
            var isTypeLong = typeof(long).Equals(intType);
            var isTypeBool = typeof(bool).Equals(intType);
            var isTypeDouble = typeof(double).Equals(intType);
            var isTypeFloat = typeof(float).Equals(intType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(intType);
            var isTypeLongNullable = typeof(long?).Equals(intType);
            var isTypeBoolNullable = typeof(bool?).Equals(intType);
            var isTypeDoubleNullable = typeof(double?).Equals(intType);
            var isTypeFloatNullable = typeof(float?).Equals(intType);


            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageOption_Class_Invalid_Property_LPOID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constLPOID = "LPOID";
            var landingPageOption  = Fixture.Create<LandingPageOption>();

            // Act , Assert
            Should.NotThrow(() => landingPageOption.GetType().GetProperty(constLPOID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPOID_Is_Present_In_LandingPageOption_Class_As_Public_Test()
        {
            // Arrange
            const string constLPOID = "LPOID";
            var landingPageOption  = Fixture.Create<LandingPageOption>();
            var propertyInfo  = landingPageOption.GetType().GetProperty(constLPOID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : LandingPageOption => LPID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPID_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageOption = Fixture.Create<LandingPageOption>();
            var random = Fixture.Create<int>();

            // Act , Set
            landingPageOption.LPID = random;

            // Assert
            landingPageOption.LPID.ShouldBe(random);
            landingPageOption.LPID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPID_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageOption = Fixture.Create<LandingPageOption>();    

            // Act , Set
            landingPageOption.LPID = null;

            // Assert
            landingPageOption.LPID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constLPID = "LPID";
            var landingPageOption = Fixture.Create<LandingPageOption>();
            var propertyInfo = landingPageOption.GetType().GetProperty(constLPID);

            // Act , Set
            propertyInfo.SetValue(landingPageOption, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageOption.LPID.ShouldBeNull();
            landingPageOption.LPID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageOption_Class_Invalid_Property_LPID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constLPID = "LPID";
            var landingPageOption  = Fixture.Create<LandingPageOption>();

            // Act , Assert
            Should.NotThrow(() => landingPageOption.GetType().GetProperty(constLPID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPID_Is_Present_In_LandingPageOption_Class_As_Public_Test()
        {
            // Arrange
            const string constLPID = "LPID";
            var landingPageOption  = Fixture.Create<LandingPageOption>();
            var propertyInfo  = landingPageOption.GetType().GetProperty(constLPID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : LandingPageOption => Description

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Description_String_Type_Verify_Test()
        {
            // Arrange
            var landingPageOption = Fixture.Create<LandingPageOption>();
            var stringType = landingPageOption.Description.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageOption_Class_Invalid_Property_Description_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constDescription = "Description";
            var landingPageOption  = Fixture.Create<LandingPageOption>();

            // Act , Assert
            Should.NotThrow(() => landingPageOption.GetType().GetProperty(constDescription));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Description_Is_Present_In_LandingPageOption_Class_As_Public_Test()
        {
            // Arrange
            const string constDescription = "Description";
            var landingPageOption  = Fixture.Create<LandingPageOption>();
            var propertyInfo  = landingPageOption.GetType().GetProperty(constDescription);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : LandingPageOption => Name

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Name_String_Type_Verify_Test()
        {
            // Arrange
            var landingPageOption = Fixture.Create<LandingPageOption>();
            var stringType = landingPageOption.Name.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageOption_Class_Invalid_Property_Name_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constName = "Name";
            var landingPageOption  = Fixture.Create<LandingPageOption>();

            // Act , Assert
            Should.NotThrow(() => landingPageOption.GetType().GetProperty(constName));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Name_Is_Present_In_LandingPageOption_Class_As_Public_Test()
        {
            // Arrange
            const string constName = "Name";
            var landingPageOption  = Fixture.Create<LandingPageOption>();
            var propertyInfo  = landingPageOption.GetType().GetProperty(constName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region General Category : General

        #region Category : Contructor

        #region General Constructor Pattern : create and expect no exception.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new LandingPageOption());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<LandingPageOption>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion

        #region General Constructor Pattern : Default Assignment Test

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_With_Default_Assignments_NoChange_DefaultValues()
        {
            // Arrange
            var lPOID = -1;
            int? lPID = null;
            var name = string.Empty;
            var description = string.Empty;
            bool? isActive = null;    

            // Act
            var landingPageOption = new LandingPageOption();    

            // Assert
            landingPageOption.LPOID.ShouldBe(lPOID);
            landingPageOption.LPID.ShouldBeNull();
            landingPageOption.Name.ShouldBe(name);
            landingPageOption.Description.ShouldBe(description);
            landingPageOption.IsActive.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}
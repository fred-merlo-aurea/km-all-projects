using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ECN_Framework_Common.Objects.Accounts;
using Shouldly;
using NUnit.Framework;
using AutoFixture;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Accounts;

namespace ECN_Framework_EntitiesTests.Accounts
{
    [TestFixture]
    public class LicenseTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void License_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var license  = new License();
            var licenseOption = Fixture.Create<Enums.LicenseOption>();
            var allowed = Fixture.Create<int>();
            var available = Fixture.Create<int>();
            var used = Fixture.Create<int>();

            // Act
            license.LicenseOption = licenseOption;
            license.Allowed = allowed;
            license.Available = available;
            license.Used = used;

            // Assert
            license.LicenseOption.ShouldBe(licenseOption);
            license.Allowed.ShouldBe(allowed);
            license.Available.ShouldBe(available);
            license.Used.ShouldBe(used);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Non-String Property Type Test : License => LicenseOption

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LicenseOption_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constLicenseOption = "LicenseOption";
            var license = Fixture.Create<License>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = license.GetType().GetProperty(constLicenseOption);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(license, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void License_Class_Invalid_Property_LicenseOption_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constLicenseOption = "LicenseOption";
            var license  = Fixture.Create<License>();

            // Act , Assert
            Should.NotThrow(() => license.GetType().GetProperty(constLicenseOption));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LicenseOption_Is_Present_In_License_Class_As_Public_Test()
        {
            // Arrange
            const string constLicenseOption = "LicenseOption";
            var license  = Fixture.Create<License>();
            var propertyInfo  = license.GetType().GetProperty(constLicenseOption);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : License => Allowed

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Allowed_Int_Type_Verify_Test()
        {
            // Arrange
            var license = Fixture.Create<License>();
            var intType = license.Allowed.GetType();

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
        public void License_Class_Invalid_Property_Allowed_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constAllowed = "Allowed";
            var license  = Fixture.Create<License>();

            // Act , Assert
            Should.NotThrow(() => license.GetType().GetProperty(constAllowed));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Allowed_Is_Present_In_License_Class_As_Public_Test()
        {
            // Arrange
            const string constAllowed = "Allowed";
            var license  = Fixture.Create<License>();
            var propertyInfo  = license.GetType().GetProperty(constAllowed);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : License => Available

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Available_Int_Type_Verify_Test()
        {
            // Arrange
            var license = Fixture.Create<License>();
            var intType = license.Available.GetType();

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
        public void License_Class_Invalid_Property_Available_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constAvailable = "Available";
            var license  = Fixture.Create<License>();

            // Act , Assert
            Should.NotThrow(() => license.GetType().GetProperty(constAvailable));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Available_Is_Present_In_License_Class_As_Public_Test()
        {
            // Arrange
            const string constAvailable = "Available";
            var license  = Fixture.Create<License>();
            var propertyInfo  = license.GetType().GetProperty(constAvailable);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : License => Used

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Used_Int_Type_Verify_Test()
        {
            // Arrange
            var license = Fixture.Create<License>();
            var intType = license.Used.GetType();

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
        public void License_Class_Invalid_Property_Used_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUsed = "Used";
            var license  = Fixture.Create<License>();

            // Act , Assert
            Should.NotThrow(() => license.GetType().GetProperty(constUsed));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Used_Is_Present_In_License_Class_As_Public_Test()
        {
            // Arrange
            const string constUsed = "Used";
            var license  = Fixture.Create<License>();
            var propertyInfo  = license.GetType().GetProperty(constUsed);

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
            Should.NotThrow(() => new License());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<License>(2).ToList();
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
            var licenseOption = Enums.LicenseOption.notavailable;
            var allowed = 0;
            var available = 0;
            var used = 0;    

            // Act
            var license = new License();    

            // Assert
            license.LicenseOption.ShouldBe(licenseOption);
            license.Allowed.ShouldBe(allowed);
            license.Available.ShouldBe(available);
            license.Used.ShouldBe(used);
        }

        #endregion

        #endregion

        #endregion
    }
}
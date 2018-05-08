using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Accounts
{
    [TestFixture]
    public class LandingPageTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (LandingPage) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var landingPage = Fixture.Create<LandingPage>();
            var lPId = Fixture.Create<int>();
            var name = Fixture.Create<string>();
            var description = Fixture.Create<string>();
            var isActive = Fixture.Create<bool?>();
            var baseChannel = Fixture.Create<bool>();
            var customer = Fixture.Create<bool>();

            // Act
            landingPage.LPID = lPId;
            landingPage.Name = name;
            landingPage.Description = description;
            landingPage.IsActive = isActive;
            landingPage.BaseChannel = baseChannel;
            landingPage.Customer = customer;

            // Assert
            landingPage.LPID.ShouldBe(lPId);
            landingPage.Name.ShouldBe(name);
            landingPage.Description.ShouldBe(description);
            landingPage.IsActive.ShouldBe(isActive);
            landingPage.BaseChannel.ShouldBe(baseChannel);
            landingPage.Customer.ShouldBe(customer);
        }

        #endregion

        #region General Getters/Setters : Class (LandingPage) => Property (BaseChannel) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_BaseChannel_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var landingPage = Fixture.Create<LandingPage>();
            landingPage.BaseChannel = Fixture.Create<bool>();
            var boolType = landingPage.BaseChannel.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (LandingPage) => Property (BaseChannel) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_Class_Invalid_Property_BaseChannelNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannel = "BaseChannelNotPresent";
            var landingPage  = Fixture.Create<LandingPage>();

            // Act , Assert
            Should.NotThrow(() => landingPage.GetType().GetProperty(propertyNameBaseChannel));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_BaseChannel_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannel = "BaseChannel";
            var landingPage  = Fixture.Create<LandingPage>();
            var propertyInfo  = landingPage.GetType().GetProperty(propertyNameBaseChannel);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LandingPage) => Property (Customer) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_Customer_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var landingPage = Fixture.Create<LandingPage>();
            landingPage.Customer = Fixture.Create<bool>();
            var boolType = landingPage.Customer.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (LandingPage) => Property (Customer) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_Class_Invalid_Property_CustomerNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomer = "CustomerNotPresent";
            var landingPage  = Fixture.Create<LandingPage>();

            // Act , Assert
            Should.NotThrow(() => landingPage.GetType().GetProperty(propertyNameCustomer));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_Customer_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomer = "Customer";
            var landingPage  = Fixture.Create<LandingPage>();
            var propertyInfo  = landingPage.GetType().GetProperty(propertyNameCustomer);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LandingPage) => Property (Description) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_Description_Property_String_Type_Verify_Test()
        {
            // Arrange
            var landingPage = Fixture.Create<LandingPage>();
            landingPage.Description = Fixture.Create<string>();
            var stringType = landingPage.Description.GetType();

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

        #region General Getters/Setters : Class (LandingPage) => Property (Description) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_Class_Invalid_Property_DescriptionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDescription = "DescriptionNotPresent";
            var landingPage  = Fixture.Create<LandingPage>();

            // Act , Assert
            Should.NotThrow(() => landingPage.GetType().GetProperty(propertyNameDescription));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_Description_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDescription = "Description";
            var landingPage  = Fixture.Create<LandingPage>();
            var propertyInfo  = landingPage.GetType().GetProperty(propertyNameDescription);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LandingPage) => Property (IsActive) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_IsActive_Property_Data_Without_Null_Test()
        {
            // Arrange
            var landingPage = Fixture.Create<LandingPage>();
            var random = Fixture.Create<bool>();

            // Act , Set
            landingPage.IsActive = random;

            // Assert
            landingPage.IsActive.ShouldBe(random);
            landingPage.IsActive.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_IsActive_Property_Only_Null_Data_Test()
        {
            // Arrange
            var landingPage = Fixture.Create<LandingPage>();    

            // Act , Set
            landingPage.IsActive = null;

            // Assert
            landingPage.IsActive.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_IsActive_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsActive = "IsActive";
            var landingPage = Fixture.Create<LandingPage>();
            var propertyInfo = landingPage.GetType().GetProperty(propertyNameIsActive);

            // Act , Set
            propertyInfo.SetValue(landingPage, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPage.IsActive.ShouldBeNull();
            landingPage.IsActive.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LandingPage) => Property (IsActive) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_Class_Invalid_Property_IsActiveNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActiveNotPresent";
            var landingPage  = Fixture.Create<LandingPage>();

            // Act , Assert
            Should.NotThrow(() => landingPage.GetType().GetProperty(propertyNameIsActive));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_IsActive_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActive";
            var landingPage  = Fixture.Create<LandingPage>();
            var propertyInfo  = landingPage.GetType().GetProperty(propertyNameIsActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LandingPage) => Property (LPID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_LPID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var landingPage = Fixture.Create<LandingPage>();
            landingPage.LPID = Fixture.Create<int>();
            var intType = landingPage.LPID.GetType();

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

        #region General Getters/Setters : Class (LandingPage) => Property (LPID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_Class_Invalid_Property_LPIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLPID = "LPIDNotPresent";
            var landingPage  = Fixture.Create<LandingPage>();

            // Act , Assert
            Should.NotThrow(() => landingPage.GetType().GetProperty(propertyNameLPID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_LPID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLPID = "LPID";
            var landingPage  = Fixture.Create<LandingPage>();
            var propertyInfo  = landingPage.GetType().GetProperty(propertyNameLPID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LandingPage) => Property (Name) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_Name_Property_String_Type_Verify_Test()
        {
            // Arrange
            var landingPage = Fixture.Create<LandingPage>();
            landingPage.Name = Fixture.Create<string>();
            var stringType = landingPage.Name.GetType();

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

        #region General Getters/Setters : Class (LandingPage) => Property (Name) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_Class_Invalid_Property_NameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameName = "NameNotPresent";
            var landingPage  = Fixture.Create<LandingPage>();

            // Act , Assert
            Should.NotThrow(() => landingPage.GetType().GetProperty(propertyNameName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPage_Name_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameName = "Name";
            var landingPage  = Fixture.Create<LandingPage>();
            var propertyInfo  = landingPage.GetType().GetProperty(propertyNameName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (LandingPage) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LandingPage_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new LandingPage());
        }

        #endregion

        #region General Constructor : Class (LandingPage) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LandingPage_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfLandingPage = Fixture.CreateMany<LandingPage>(2).ToList();
            var firstLandingPage = instancesOfLandingPage.FirstOrDefault();
            var lastLandingPage = instancesOfLandingPage.Last();

            // Act, Assert
            firstLandingPage.ShouldNotBeNull();
            lastLandingPage.ShouldNotBeNull();
            firstLandingPage.ShouldNotBeSameAs(lastLandingPage);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LandingPage_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstLandingPage = new LandingPage();
            var secondLandingPage = new LandingPage();
            var thirdLandingPage = new LandingPage();
            var fourthLandingPage = new LandingPage();
            var fifthLandingPage = new LandingPage();
            var sixthLandingPage = new LandingPage();

            // Act, Assert
            firstLandingPage.ShouldNotBeNull();
            secondLandingPage.ShouldNotBeNull();
            thirdLandingPage.ShouldNotBeNull();
            fourthLandingPage.ShouldNotBeNull();
            fifthLandingPage.ShouldNotBeNull();
            sixthLandingPage.ShouldNotBeNull();
            firstLandingPage.ShouldNotBeSameAs(secondLandingPage);
            thirdLandingPage.ShouldNotBeSameAs(firstLandingPage);
            fourthLandingPage.ShouldNotBeSameAs(firstLandingPage);
            fifthLandingPage.ShouldNotBeSameAs(firstLandingPage);
            sixthLandingPage.ShouldNotBeSameAs(firstLandingPage);
            sixthLandingPage.ShouldNotBeSameAs(fourthLandingPage);
        }

        #endregion

        #region General Constructor : Class (LandingPage) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LandingPage_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var lPId = -1;
            var name = string.Empty;
            var description = string.Empty;
            var baseChannel = true;
            var customer = true;

            // Act
            var landingPage = new LandingPage();

            // Assert
            landingPage.LPID.ShouldBe(lPId);
            landingPage.Name.ShouldBe(name);
            landingPage.Description.ShouldBe(description);
            landingPage.IsActive.ShouldBeNull();
            landingPage.BaseChannel.ShouldBeTrue();
            landingPage.Customer.ShouldBeTrue();
        }

        #endregion

        #endregion

        #endregion
    }
}
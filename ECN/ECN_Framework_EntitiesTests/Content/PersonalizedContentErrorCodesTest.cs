using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Content;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Content
{
    [TestFixture]
    public class PersonalizedContentErrorCodesTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (PersonalizedContentErrorCodes) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContentErrorCodes_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var personalizedContentErrorCodes = Fixture.Create<PersonalizedContentErrorCodes>();
            var errorCode = Fixture.Create<int>();
            var errorMessage = Fixture.Create<string>();

            // Act
            personalizedContentErrorCodes.ErrorCode = errorCode;
            personalizedContentErrorCodes.ErrorMessage = errorMessage;

            // Assert
            personalizedContentErrorCodes.ErrorCode.ShouldBe(errorCode);
            personalizedContentErrorCodes.ErrorMessage.ShouldBe(errorMessage);
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContentErrorCodes) => Property (ErrorCode) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContentErrorCodes_ErrorCode_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var personalizedContentErrorCodes = Fixture.Create<PersonalizedContentErrorCodes>();
            personalizedContentErrorCodes.ErrorCode = Fixture.Create<int>();
            var intType = personalizedContentErrorCodes.ErrorCode.GetType();

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

        #region General Getters/Setters : Class (PersonalizedContentErrorCodes) => Property (ErrorCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContentErrorCodes_Class_Invalid_Property_ErrorCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameErrorCode = "ErrorCodeNotPresent";
            var personalizedContentErrorCodes  = Fixture.Create<PersonalizedContentErrorCodes>();

            // Act , Assert
            Should.NotThrow(() => personalizedContentErrorCodes.GetType().GetProperty(propertyNameErrorCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContentErrorCodes_ErrorCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameErrorCode = "ErrorCode";
            var personalizedContentErrorCodes  = Fixture.Create<PersonalizedContentErrorCodes>();
            var propertyInfo  = personalizedContentErrorCodes.GetType().GetProperty(propertyNameErrorCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContentErrorCodes) => Property (ErrorMessage) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContentErrorCodes_ErrorMessage_Property_String_Type_Verify_Test()
        {
            // Arrange
            var personalizedContentErrorCodes = Fixture.Create<PersonalizedContentErrorCodes>();
            personalizedContentErrorCodes.ErrorMessage = Fixture.Create<string>();
            var stringType = personalizedContentErrorCodes.ErrorMessage.GetType();

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

        #region General Getters/Setters : Class (PersonalizedContentErrorCodes) => Property (ErrorMessage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContentErrorCodes_Class_Invalid_Property_ErrorMessageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameErrorMessage = "ErrorMessageNotPresent";
            var personalizedContentErrorCodes  = Fixture.Create<PersonalizedContentErrorCodes>();

            // Act , Assert
            Should.NotThrow(() => personalizedContentErrorCodes.GetType().GetProperty(propertyNameErrorMessage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContentErrorCodes_ErrorMessage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameErrorMessage = "ErrorMessage";
            var personalizedContentErrorCodes  = Fixture.Create<PersonalizedContentErrorCodes>();
            var propertyInfo  = personalizedContentErrorCodes.GetType().GetProperty(propertyNameErrorMessage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (PersonalizedContentErrorCodes) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_PersonalizedContentErrorCodes_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new PersonalizedContentErrorCodes());
        }

        #endregion

        #region General Constructor : Class (PersonalizedContentErrorCodes) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_PersonalizedContentErrorCodes_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfPersonalizedContentErrorCodes = Fixture.CreateMany<PersonalizedContentErrorCodes>(2).ToList();
            var firstPersonalizedContentErrorCodes = instancesOfPersonalizedContentErrorCodes.FirstOrDefault();
            var lastPersonalizedContentErrorCodes = instancesOfPersonalizedContentErrorCodes.Last();

            // Act, Assert
            firstPersonalizedContentErrorCodes.ShouldNotBeNull();
            lastPersonalizedContentErrorCodes.ShouldNotBeNull();
            firstPersonalizedContentErrorCodes.ShouldNotBeSameAs(lastPersonalizedContentErrorCodes);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_PersonalizedContentErrorCodes_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstPersonalizedContentErrorCodes = new PersonalizedContentErrorCodes();
            var secondPersonalizedContentErrorCodes = new PersonalizedContentErrorCodes();
            var thirdPersonalizedContentErrorCodes = new PersonalizedContentErrorCodes();
            var fourthPersonalizedContentErrorCodes = new PersonalizedContentErrorCodes();
            var fifthPersonalizedContentErrorCodes = new PersonalizedContentErrorCodes();
            var sixthPersonalizedContentErrorCodes = new PersonalizedContentErrorCodes();

            // Act, Assert
            firstPersonalizedContentErrorCodes.ShouldNotBeNull();
            secondPersonalizedContentErrorCodes.ShouldNotBeNull();
            thirdPersonalizedContentErrorCodes.ShouldNotBeNull();
            fourthPersonalizedContentErrorCodes.ShouldNotBeNull();
            fifthPersonalizedContentErrorCodes.ShouldNotBeNull();
            sixthPersonalizedContentErrorCodes.ShouldNotBeNull();
            firstPersonalizedContentErrorCodes.ShouldNotBeSameAs(secondPersonalizedContentErrorCodes);
            thirdPersonalizedContentErrorCodes.ShouldNotBeSameAs(firstPersonalizedContentErrorCodes);
            fourthPersonalizedContentErrorCodes.ShouldNotBeSameAs(firstPersonalizedContentErrorCodes);
            fifthPersonalizedContentErrorCodes.ShouldNotBeSameAs(firstPersonalizedContentErrorCodes);
            sixthPersonalizedContentErrorCodes.ShouldNotBeSameAs(firstPersonalizedContentErrorCodes);
            sixthPersonalizedContentErrorCodes.ShouldNotBeSameAs(fourthPersonalizedContentErrorCodes);
        }

        #endregion

        #endregion

        #endregion
    }
}
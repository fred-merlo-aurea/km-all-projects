using System.Linq;
using AutoFixture;
using ECN_Framework_EntitiesTests.ConfigureProject;
using EmailMarketing.API.Models;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class PersonalizationContentErrorCodesTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (PersonalizationContentErrorCodes) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizationContentErrorCodes_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var personalizationContentErrorCodes = Fixture.Create<PersonalizationContentErrorCodes>();
            var errorCode = Fixture.Create<int>();
            var errorMessage = Fixture.Create<string>();

            // Act
            personalizationContentErrorCodes.ErrorCode = errorCode;
            personalizationContentErrorCodes.ErrorMessage = errorMessage;

            // Assert
            personalizationContentErrorCodes.ErrorCode.ShouldBe(errorCode);
            personalizationContentErrorCodes.ErrorMessage.ShouldBe(errorMessage);
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizationContentErrorCodes) => Property (ErrorCode) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizationContentErrorCodes_ErrorCode_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var personalizationContentErrorCodes = Fixture.Create<PersonalizationContentErrorCodes>();
            personalizationContentErrorCodes.ErrorCode = Fixture.Create<int>();
            var intType = personalizationContentErrorCodes.ErrorCode.GetType();

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

        #region General Getters/Setters : Class (PersonalizationContentErrorCodes) => Property (ErrorCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizationContentErrorCodes_Class_Invalid_Property_ErrorCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameErrorCode = "ErrorCodeNotPresent";
            var personalizationContentErrorCodes  = Fixture.Create<PersonalizationContentErrorCodes>();

            // Act , Assert
            Should.NotThrow(() => personalizationContentErrorCodes.GetType().GetProperty(propertyNameErrorCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizationContentErrorCodes_ErrorCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameErrorCode = "ErrorCode";
            var personalizationContentErrorCodes  = Fixture.Create<PersonalizationContentErrorCodes>();
            var propertyInfo  = personalizationContentErrorCodes.GetType().GetProperty(propertyNameErrorCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizationContentErrorCodes) => Property (ErrorMessage) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizationContentErrorCodes_ErrorMessage_Property_String_Type_Verify_Test()
        {
            // Arrange
            var personalizationContentErrorCodes = Fixture.Create<PersonalizationContentErrorCodes>();
            personalizationContentErrorCodes.ErrorMessage = Fixture.Create<string>();
            var stringType = personalizationContentErrorCodes.ErrorMessage.GetType();

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

        #region General Getters/Setters : Class (PersonalizationContentErrorCodes) => Property (ErrorMessage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizationContentErrorCodes_Class_Invalid_Property_ErrorMessageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameErrorMessage = "ErrorMessageNotPresent";
            var personalizationContentErrorCodes  = Fixture.Create<PersonalizationContentErrorCodes>();

            // Act , Assert
            Should.NotThrow(() => personalizationContentErrorCodes.GetType().GetProperty(propertyNameErrorMessage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizationContentErrorCodes_ErrorMessage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameErrorMessage = "ErrorMessage";
            var personalizationContentErrorCodes  = Fixture.Create<PersonalizationContentErrorCodes>();
            var propertyInfo  = personalizationContentErrorCodes.GetType().GetProperty(propertyNameErrorMessage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (PersonalizationContentErrorCodes) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_PersonalizationContentErrorCodes_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new PersonalizationContentErrorCodes());
        }

        #endregion

        #region General Constructor : Class (PersonalizationContentErrorCodes) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_PersonalizationContentErrorCodes_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfPersonalizationContentErrorCodes = Fixture.CreateMany<PersonalizationContentErrorCodes>(2).ToList();
            var firstPersonalizationContentErrorCodes = instancesOfPersonalizationContentErrorCodes.FirstOrDefault();
            var lastPersonalizationContentErrorCodes = instancesOfPersonalizationContentErrorCodes.Last();

            // Act, Assert
            firstPersonalizationContentErrorCodes.ShouldNotBeNull();
            lastPersonalizationContentErrorCodes.ShouldNotBeNull();
            firstPersonalizationContentErrorCodes.ShouldNotBeSameAs(lastPersonalizationContentErrorCodes);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_PersonalizationContentErrorCodes_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstPersonalizationContentErrorCodes = new PersonalizationContentErrorCodes();
            var secondPersonalizationContentErrorCodes = new PersonalizationContentErrorCodes();
            var thirdPersonalizationContentErrorCodes = new PersonalizationContentErrorCodes();
            var fourthPersonalizationContentErrorCodes = new PersonalizationContentErrorCodes();
            var fifthPersonalizationContentErrorCodes = new PersonalizationContentErrorCodes();
            var sixthPersonalizationContentErrorCodes = new PersonalizationContentErrorCodes();

            // Act, Assert
            firstPersonalizationContentErrorCodes.ShouldNotBeNull();
            secondPersonalizationContentErrorCodes.ShouldNotBeNull();
            thirdPersonalizationContentErrorCodes.ShouldNotBeNull();
            fourthPersonalizationContentErrorCodes.ShouldNotBeNull();
            fifthPersonalizationContentErrorCodes.ShouldNotBeNull();
            sixthPersonalizationContentErrorCodes.ShouldNotBeNull();
            firstPersonalizationContentErrorCodes.ShouldNotBeSameAs(secondPersonalizationContentErrorCodes);
            thirdPersonalizationContentErrorCodes.ShouldNotBeSameAs(firstPersonalizationContentErrorCodes);
            fourthPersonalizationContentErrorCodes.ShouldNotBeSameAs(firstPersonalizationContentErrorCodes);
            fifthPersonalizationContentErrorCodes.ShouldNotBeSameAs(firstPersonalizationContentErrorCodes);
            sixthPersonalizationContentErrorCodes.ShouldNotBeSameAs(firstPersonalizationContentErrorCodes);
            sixthPersonalizationContentErrorCodes.ShouldNotBeSameAs(fourthPersonalizationContentErrorCodes);
        }

        #endregion

        #endregion

        #endregion
    }
}
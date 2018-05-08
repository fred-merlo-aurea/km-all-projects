using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.accounts.customersmanager;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Customers
{
    [TestFixture]
    public class CustomerEntryResultTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (CustomerEntryResult) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerEntryResult_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var customerEntryResult = Fixture.Create<CustomerEntryResult>();
            var userId = Fixture.Create<int>();
            var clientId = Fixture.Create<int>();
            var baseChannelId = Fixture.Create<int>();

            // Act
            customerEntryResult.UserId = userId;
            customerEntryResult.ClientId = clientId;
            customerEntryResult.BaseChannelId = baseChannelId;

            // Assert
            customerEntryResult.UserId.ShouldBe(userId);
            customerEntryResult.ClientId.ShouldBe(clientId);
            customerEntryResult.BaseChannelId.ShouldBe(baseChannelId);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerEntryResult) => Property (BaseChannelId) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerEntryResult_BaseChannelId_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var customerEntryResult = Fixture.Create<CustomerEntryResult>();
            customerEntryResult.BaseChannelId = Fixture.Create<int>();
            var intType = customerEntryResult.BaseChannelId.GetType();

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

        #region General Getters/Setters : Class (CustomerEntryResult) => Property (BaseChannelId) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerEntryResult_Class_Invalid_Property_BaseChannelIdNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelId = "BaseChannelIdNotPresent";
            var customerEntryResult  = Fixture.Create<CustomerEntryResult>();

            // Act , Assert
            Should.NotThrow(() => customerEntryResult.GetType().GetProperty(propertyNameBaseChannelId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerEntryResult_BaseChannelId_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelId = "BaseChannelId";
            var customerEntryResult  = Fixture.Create<CustomerEntryResult>();
            var propertyInfo  = customerEntryResult.GetType().GetProperty(propertyNameBaseChannelId);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerEntryResult) => Property (ClientId) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerEntryResult_ClientId_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var customerEntryResult = Fixture.Create<CustomerEntryResult>();
            customerEntryResult.ClientId = Fixture.Create<int>();
            var intType = customerEntryResult.ClientId.GetType();

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

        #region General Getters/Setters : Class (CustomerEntryResult) => Property (ClientId) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerEntryResult_Class_Invalid_Property_ClientIdNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClientId = "ClientIdNotPresent";
            var customerEntryResult  = Fixture.Create<CustomerEntryResult>();

            // Act , Assert
            Should.NotThrow(() => customerEntryResult.GetType().GetProperty(propertyNameClientId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerEntryResult_ClientId_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClientId = "ClientId";
            var customerEntryResult  = Fixture.Create<CustomerEntryResult>();
            var propertyInfo  = customerEntryResult.GetType().GetProperty(propertyNameClientId);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerEntryResult) => Property (UserId) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerEntryResult_UserId_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var customerEntryResult = Fixture.Create<CustomerEntryResult>();
            customerEntryResult.UserId = Fixture.Create<int>();
            var intType = customerEntryResult.UserId.GetType();

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

        #region General Getters/Setters : Class (CustomerEntryResult) => Property (UserId) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerEntryResult_Class_Invalid_Property_UserIdNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUserId = "UserIdNotPresent";
            var customerEntryResult  = Fixture.Create<CustomerEntryResult>();

            // Act , Assert
            Should.NotThrow(() => customerEntryResult.GetType().GetProperty(propertyNameUserId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerEntryResult_UserId_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUserId = "UserId";
            var customerEntryResult  = Fixture.Create<CustomerEntryResult>();
            var propertyInfo  = customerEntryResult.GetType().GetProperty(propertyNameUserId);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #endregion
    }
}
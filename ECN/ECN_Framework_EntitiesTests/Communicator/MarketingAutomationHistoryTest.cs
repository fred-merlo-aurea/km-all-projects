using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class MarketingAutomationHistoryTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (MarketingAutomationHistory) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomationHistory_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var marketingAutomationHistory = Fixture.Create<MarketingAutomationHistory>();
            var marketingAutomationHistoryId = Fixture.Create<int>();
            var marketingAutomationId = Fixture.Create<int>();
            var userId = Fixture.Create<int>();
            var action = Fixture.Create<string>();
            var historyDate = Fixture.Create<DateTime>();

            // Act
            marketingAutomationHistory.MarketingAutomationHistoryID = marketingAutomationHistoryId;
            marketingAutomationHistory.MarketingAutomationID = marketingAutomationId;
            marketingAutomationHistory.UserID = userId;
            marketingAutomationHistory.Action = action;
            marketingAutomationHistory.HistoryDate = historyDate;

            // Assert
            marketingAutomationHistory.MarketingAutomationHistoryID.ShouldBe(marketingAutomationHistoryId);
            marketingAutomationHistory.MarketingAutomationID.ShouldBe(marketingAutomationId);
            marketingAutomationHistory.UserID.ShouldBe(userId);
            marketingAutomationHistory.Action.ShouldBe(action);
            marketingAutomationHistory.HistoryDate.ShouldBe(historyDate);
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomationHistory) => Property (Action) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomationHistory_Action_Property_String_Type_Verify_Test()
        {
            // Arrange
            var marketingAutomationHistory = Fixture.Create<MarketingAutomationHistory>();
            marketingAutomationHistory.Action = Fixture.Create<string>();
            var stringType = marketingAutomationHistory.Action.GetType();

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

        #region General Getters/Setters : Class (MarketingAutomationHistory) => Property (Action) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomationHistory_Class_Invalid_Property_ActionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAction = "ActionNotPresent";
            var marketingAutomationHistory  = Fixture.Create<MarketingAutomationHistory>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomationHistory.GetType().GetProperty(propertyNameAction));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomationHistory_Action_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAction = "Action";
            var marketingAutomationHistory  = Fixture.Create<MarketingAutomationHistory>();
            var propertyInfo  = marketingAutomationHistory.GetType().GetProperty(propertyNameAction);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomationHistory) => Property (HistoryDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomationHistory_HistoryDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameHistoryDate = "HistoryDate";
            var marketingAutomationHistory = Fixture.Create<MarketingAutomationHistory>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = marketingAutomationHistory.GetType().GetProperty(propertyNameHistoryDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(marketingAutomationHistory, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomationHistory) => Property (HistoryDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomationHistory_Class_Invalid_Property_HistoryDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHistoryDate = "HistoryDateNotPresent";
            var marketingAutomationHistory  = Fixture.Create<MarketingAutomationHistory>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomationHistory.GetType().GetProperty(propertyNameHistoryDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomationHistory_HistoryDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameHistoryDate = "HistoryDate";
            var marketingAutomationHistory  = Fixture.Create<MarketingAutomationHistory>();
            var propertyInfo  = marketingAutomationHistory.GetType().GetProperty(propertyNameHistoryDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomationHistory) => Property (MarketingAutomationHistoryID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomationHistory_MarketingAutomationHistoryID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var marketingAutomationHistory = Fixture.Create<MarketingAutomationHistory>();
            marketingAutomationHistory.MarketingAutomationHistoryID = Fixture.Create<int>();
            var intType = marketingAutomationHistory.MarketingAutomationHistoryID.GetType();

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

        #region General Getters/Setters : Class (MarketingAutomationHistory) => Property (MarketingAutomationHistoryID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomationHistory_Class_Invalid_Property_MarketingAutomationHistoryIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMarketingAutomationHistoryID = "MarketingAutomationHistoryIDNotPresent";
            var marketingAutomationHistory  = Fixture.Create<MarketingAutomationHistory>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomationHistory.GetType().GetProperty(propertyNameMarketingAutomationHistoryID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomationHistory_MarketingAutomationHistoryID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMarketingAutomationHistoryID = "MarketingAutomationHistoryID";
            var marketingAutomationHistory  = Fixture.Create<MarketingAutomationHistory>();
            var propertyInfo  = marketingAutomationHistory.GetType().GetProperty(propertyNameMarketingAutomationHistoryID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomationHistory) => Property (MarketingAutomationID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomationHistory_MarketingAutomationID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var marketingAutomationHistory = Fixture.Create<MarketingAutomationHistory>();
            marketingAutomationHistory.MarketingAutomationID = Fixture.Create<int>();
            var intType = marketingAutomationHistory.MarketingAutomationID.GetType();

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

        #region General Getters/Setters : Class (MarketingAutomationHistory) => Property (MarketingAutomationID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomationHistory_Class_Invalid_Property_MarketingAutomationIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMarketingAutomationID = "MarketingAutomationIDNotPresent";
            var marketingAutomationHistory  = Fixture.Create<MarketingAutomationHistory>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomationHistory.GetType().GetProperty(propertyNameMarketingAutomationID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomationHistory_MarketingAutomationID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMarketingAutomationID = "MarketingAutomationID";
            var marketingAutomationHistory  = Fixture.Create<MarketingAutomationHistory>();
            var propertyInfo  = marketingAutomationHistory.GetType().GetProperty(propertyNameMarketingAutomationID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomationHistory) => Property (UserID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomationHistory_UserID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var marketingAutomationHistory = Fixture.Create<MarketingAutomationHistory>();
            marketingAutomationHistory.UserID = Fixture.Create<int>();
            var intType = marketingAutomationHistory.UserID.GetType();

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

        #region General Getters/Setters : Class (MarketingAutomationHistory) => Property (UserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomationHistory_Class_Invalid_Property_UserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUserID = "UserIDNotPresent";
            var marketingAutomationHistory  = Fixture.Create<MarketingAutomationHistory>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomationHistory.GetType().GetProperty(propertyNameUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomationHistory_UserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUserID = "UserID";
            var marketingAutomationHistory  = Fixture.Create<MarketingAutomationHistory>();
            var propertyInfo  = marketingAutomationHistory.GetType().GetProperty(propertyNameUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (MarketingAutomationHistory) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MarketingAutomationHistory_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new MarketingAutomationHistory());
        }

        #endregion

        #region General Constructor : Class (MarketingAutomationHistory) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MarketingAutomationHistory_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfMarketingAutomationHistory = Fixture.CreateMany<MarketingAutomationHistory>(2).ToList();
            var firstMarketingAutomationHistory = instancesOfMarketingAutomationHistory.FirstOrDefault();
            var lastMarketingAutomationHistory = instancesOfMarketingAutomationHistory.Last();

            // Act, Assert
            firstMarketingAutomationHistory.ShouldNotBeNull();
            lastMarketingAutomationHistory.ShouldNotBeNull();
            firstMarketingAutomationHistory.ShouldNotBeSameAs(lastMarketingAutomationHistory);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MarketingAutomationHistory_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstMarketingAutomationHistory = new MarketingAutomationHistory();
            var secondMarketingAutomationHistory = new MarketingAutomationHistory();
            var thirdMarketingAutomationHistory = new MarketingAutomationHistory();
            var fourthMarketingAutomationHistory = new MarketingAutomationHistory();
            var fifthMarketingAutomationHistory = new MarketingAutomationHistory();
            var sixthMarketingAutomationHistory = new MarketingAutomationHistory();

            // Act, Assert
            firstMarketingAutomationHistory.ShouldNotBeNull();
            secondMarketingAutomationHistory.ShouldNotBeNull();
            thirdMarketingAutomationHistory.ShouldNotBeNull();
            fourthMarketingAutomationHistory.ShouldNotBeNull();
            fifthMarketingAutomationHistory.ShouldNotBeNull();
            sixthMarketingAutomationHistory.ShouldNotBeNull();
            firstMarketingAutomationHistory.ShouldNotBeSameAs(secondMarketingAutomationHistory);
            thirdMarketingAutomationHistory.ShouldNotBeSameAs(firstMarketingAutomationHistory);
            fourthMarketingAutomationHistory.ShouldNotBeSameAs(firstMarketingAutomationHistory);
            fifthMarketingAutomationHistory.ShouldNotBeSameAs(firstMarketingAutomationHistory);
            sixthMarketingAutomationHistory.ShouldNotBeSameAs(firstMarketingAutomationHistory);
            sixthMarketingAutomationHistory.ShouldNotBeSameAs(fourthMarketingAutomationHistory);
        }

        #endregion

        #region General Constructor : Class (MarketingAutomationHistory) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MarketingAutomationHistory_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var marketingAutomationHistoryId = -1;
            var marketingAutomationId = -1;
            var userId = -1;
            var action = "";

            // Act
            var marketingAutomationHistory = new MarketingAutomationHistory();

            // Assert
            marketingAutomationHistory.MarketingAutomationHistoryID.ShouldBe(marketingAutomationHistoryId);
            marketingAutomationHistory.MarketingAutomationID.ShouldBe(marketingAutomationId);
            marketingAutomationHistory.UserID.ShouldBe(userId);
            marketingAutomationHistory.Action.ShouldBe(action);
            marketingAutomationHistory.HistoryDate.ShouldNotBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}
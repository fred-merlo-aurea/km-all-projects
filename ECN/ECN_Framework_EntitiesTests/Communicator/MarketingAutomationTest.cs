using System;
using System.Collections.Generic;
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
    public class MarketingAutomationTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (MarketingAutomation) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            var marketingAutomationId = Fixture.Create<int>();
            var name = Fixture.Create<string>();
            var customerId = Fixture.Create<int>();
            var state = Fixture.Create<ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool>();
            var goal = Fixture.Create<string>();
            var startDate = Fixture.Create<DateTime?>();
            var endDate = Fixture.Create<DateTime?>();
            var lastPublishedDate = Fixture.Create<DateTime?>();
            var jSONDiagram = Fixture.Create<string>();
            var type = Fixture.Create<ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationType>();
            var controls = Fixture.Create<List<ECN_Framework_Entities.Communicator.MAControl>>();
            var connectors = Fixture.Create<List<ECN_Framework_Entities.Communicator.MAConnector>>();

            // Act
            marketingAutomation.MarketingAutomationID = marketingAutomationId;
            marketingAutomation.Name = name;
            marketingAutomation.CustomerID = customerId;
            marketingAutomation.State = state;
            marketingAutomation.CreatedDate = createdDate;
            marketingAutomation.UpdatedDate = updatedDate;
            marketingAutomation.CreatedUserID = createdUserId;
            marketingAutomation.UpdatedUserID = updatedUserId;
            marketingAutomation.IsDeleted = isDeleted;
            marketingAutomation.Goal = goal;
            marketingAutomation.StartDate = startDate;
            marketingAutomation.EndDate = endDate;
            marketingAutomation.LastPublishedDate = lastPublishedDate;
            marketingAutomation.JSONDiagram = jSONDiagram;
            marketingAutomation.Type = type;
            marketingAutomation.Controls = controls;
            marketingAutomation.Connectors = connectors;

            // Assert
            marketingAutomation.MarketingAutomationID.ShouldBe(marketingAutomationId);
            marketingAutomation.Name.ShouldBe(name);
            marketingAutomation.CustomerID.ShouldBe(customerId);
            marketingAutomation.State.ShouldBe(state);
            marketingAutomation.CreatedDate.ShouldBe(createdDate);
            marketingAutomation.UpdatedDate.ShouldBe(updatedDate);
            marketingAutomation.CreatedUserID.ShouldBe(createdUserId);
            marketingAutomation.UpdatedUserID.ShouldBe(updatedUserId);
            marketingAutomation.IsDeleted.ShouldBe(isDeleted);
            marketingAutomation.Goal.ShouldBe(goal);
            marketingAutomation.StartDate.ShouldBe(startDate);
            marketingAutomation.EndDate.ShouldBe(endDate);
            marketingAutomation.LastPublishedDate.ShouldBe(lastPublishedDate);
            marketingAutomation.JSONDiagram.ShouldBe(jSONDiagram);
            marketingAutomation.Type.ShouldBe(type);
            marketingAutomation.Controls.ShouldBe(controls);
            marketingAutomation.Connectors.ShouldBe(connectors);
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (Connectors) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_ConnectorsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConnectors = "ConnectorsNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameConnectors));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Connectors_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConnectors = "Connectors";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameConnectors);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (Controls) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_ControlsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameControls = "ControlsNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameControls));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Controls_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameControls = "Controls";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameControls);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = marketingAutomation.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(marketingAutomation, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            var random = Fixture.Create<int>();

            // Act , Set
            marketingAutomation.CreatedUserID = random;

            // Assert
            marketingAutomation.CreatedUserID.ShouldBe(random);
            marketingAutomation.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var marketingAutomation = Fixture.Create<MarketingAutomation>();

            // Act , Set
            marketingAutomation.CreatedUserID = null;

            // Assert
            marketingAutomation.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            var propertyInfo = marketingAutomation.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(marketingAutomation, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            marketingAutomation.CreatedUserID.ShouldBeNull();
            marketingAutomation.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            marketingAutomation.CustomerID = Fixture.Create<int>();
            var intType = marketingAutomation.CustomerID.GetType();

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

        #region General Getters/Setters : Class (MarketingAutomation) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (EndDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_EndDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameEndDate = "EndDate";
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = marketingAutomation.GetType().GetProperty(propertyNameEndDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(marketingAutomation, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (EndDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_EndDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEndDate = "EndDateNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameEndDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_EndDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEndDate = "EndDate";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameEndDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (Goal) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Goal_Property_String_Type_Verify_Test()
        {
            // Arrange
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            marketingAutomation.Goal = Fixture.Create<string>();
            var stringType = marketingAutomation.Goal.GetType();

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

        #region General Getters/Setters : Class (MarketingAutomation) => Property (Goal) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_GoalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGoal = "GoalNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameGoal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Goal_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGoal = "Goal";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameGoal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (IsDeleted) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_IsDeleted_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            marketingAutomation.IsDeleted = Fixture.Create<bool>();
            var boolType = marketingAutomation.IsDeleted.GetType();

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

        #region General Getters/Setters : Class (MarketingAutomation) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (JSONDiagram) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_JSONDiagram_Property_String_Type_Verify_Test()
        {
            // Arrange
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            marketingAutomation.JSONDiagram = Fixture.Create<string>();
            var stringType = marketingAutomation.JSONDiagram.GetType();

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

        #region General Getters/Setters : Class (MarketingAutomation) => Property (JSONDiagram) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_JSONDiagramNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameJSONDiagram = "JSONDiagramNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameJSONDiagram));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_JSONDiagram_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameJSONDiagram = "JSONDiagram";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameJSONDiagram);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (LastPublishedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_LastPublishedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameLastPublishedDate = "LastPublishedDate";
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = marketingAutomation.GetType().GetProperty(propertyNameLastPublishedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(marketingAutomation, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (LastPublishedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_LastPublishedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLastPublishedDate = "LastPublishedDateNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameLastPublishedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_LastPublishedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLastPublishedDate = "LastPublishedDate";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameLastPublishedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (MarketingAutomationID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_MarketingAutomationID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            marketingAutomation.MarketingAutomationID = Fixture.Create<int>();
            var intType = marketingAutomation.MarketingAutomationID.GetType();

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

        #region General Getters/Setters : Class (MarketingAutomation) => Property (MarketingAutomationID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_MarketingAutomationIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMarketingAutomationID = "MarketingAutomationIDNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameMarketingAutomationID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_MarketingAutomationID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMarketingAutomationID = "MarketingAutomationID";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameMarketingAutomationID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (Name) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Name_Property_String_Type_Verify_Test()
        {
            // Arrange
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            marketingAutomation.Name = Fixture.Create<string>();
            var stringType = marketingAutomation.Name.GetType();

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

        #region General Getters/Setters : Class (MarketingAutomation) => Property (Name) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_NameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameName = "NameNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Name_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameName = "Name";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (StartDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_StartDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameStartDate = "StartDate";
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = marketingAutomation.GetType().GetProperty(propertyNameStartDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(marketingAutomation, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (StartDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_StartDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStartDate = "StartDateNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameStartDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_StartDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStartDate = "StartDate";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameStartDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (State) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_State_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameState = "State";
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = marketingAutomation.GetType().GetProperty(propertyNameState);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(marketingAutomation, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (State) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_StateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameState = "StateNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameState));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_State_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameState = "State";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameState);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (Type) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Type_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameType = "Type";
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = marketingAutomation.GetType().GetProperty(propertyNameType);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(marketingAutomation, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (Type) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_TypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameType = "TypeNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Type_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameType = "Type";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = marketingAutomation.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(marketingAutomation, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            var random = Fixture.Create<int>();

            // Act , Set
            marketingAutomation.UpdatedUserID = random;

            // Assert
            marketingAutomation.UpdatedUserID.ShouldBe(random);
            marketingAutomation.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var marketingAutomation = Fixture.Create<MarketingAutomation>();

            // Act , Set
            marketingAutomation.UpdatedUserID = null;

            // Assert
            marketingAutomation.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var marketingAutomation = Fixture.Create<MarketingAutomation>();
            var propertyInfo = marketingAutomation.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(marketingAutomation, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            marketingAutomation.UpdatedUserID.ShouldBeNull();
            marketingAutomation.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (MarketingAutomation) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();

            // Act , Assert
            Should.NotThrow(() => marketingAutomation.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MarketingAutomation_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var marketingAutomation  = Fixture.Create<MarketingAutomation>();
            var propertyInfo  = marketingAutomation.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (MarketingAutomation) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MarketingAutomation_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new MarketingAutomation());
        }

        #endregion

        #region General Constructor : Class (MarketingAutomation) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MarketingAutomation_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfMarketingAutomation = Fixture.CreateMany<MarketingAutomation>(2).ToList();
            var firstMarketingAutomation = instancesOfMarketingAutomation.FirstOrDefault();
            var lastMarketingAutomation = instancesOfMarketingAutomation.Last();

            // Act, Assert
            firstMarketingAutomation.ShouldNotBeNull();
            lastMarketingAutomation.ShouldNotBeNull();
            firstMarketingAutomation.ShouldNotBeSameAs(lastMarketingAutomation);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MarketingAutomation_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstMarketingAutomation = new MarketingAutomation();
            var secondMarketingAutomation = new MarketingAutomation();
            var thirdMarketingAutomation = new MarketingAutomation();
            var fourthMarketingAutomation = new MarketingAutomation();
            var fifthMarketingAutomation = new MarketingAutomation();
            var sixthMarketingAutomation = new MarketingAutomation();

            // Act, Assert
            firstMarketingAutomation.ShouldNotBeNull();
            secondMarketingAutomation.ShouldNotBeNull();
            thirdMarketingAutomation.ShouldNotBeNull();
            fourthMarketingAutomation.ShouldNotBeNull();
            fifthMarketingAutomation.ShouldNotBeNull();
            sixthMarketingAutomation.ShouldNotBeNull();
            firstMarketingAutomation.ShouldNotBeSameAs(secondMarketingAutomation);
            thirdMarketingAutomation.ShouldNotBeSameAs(firstMarketingAutomation);
            fourthMarketingAutomation.ShouldNotBeSameAs(firstMarketingAutomation);
            fifthMarketingAutomation.ShouldNotBeSameAs(firstMarketingAutomation);
            sixthMarketingAutomation.ShouldNotBeSameAs(firstMarketingAutomation);
            sixthMarketingAutomation.ShouldNotBeSameAs(fourthMarketingAutomation);
        }

        #endregion

        #region General Constructor : Class (MarketingAutomation) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MarketingAutomation_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var marketingAutomationId = -1;
            var name = "";
            var state = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus.Saved;
            var customerId = -1;
            var goal = "";
            var jSONDiagram = "";
            var isDeleted = false;
            var type = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationType.Simple;

            // Act
            var marketingAutomation = new MarketingAutomation();

            // Assert
            marketingAutomation.MarketingAutomationID.ShouldBe(marketingAutomationId);
            marketingAutomation.Name.ShouldBe(name);
            marketingAutomation.State.ShouldBe(state);
            marketingAutomation.CustomerID.ShouldBe(customerId);
            marketingAutomation.CreatedDate.ShouldBeNull();
            marketingAutomation.UpdatedDate.ShouldBeNull();
            marketingAutomation.CreatedUserID.ShouldBeNull();
            marketingAutomation.UpdatedUserID.ShouldBeNull();
            marketingAutomation.LastPublishedDate.ShouldBeNull();
            marketingAutomation.Goal.ShouldBe(goal);
            marketingAutomation.StartDate.ShouldBeNull();
            marketingAutomation.EndDate.ShouldBeNull();
            marketingAutomation.JSONDiagram.ShouldBe(jSONDiagram);
            marketingAutomation.IsDeleted.ShouldBeFalse();
            marketingAutomation.Controls.ShouldBeEmpty();
            marketingAutomation.Connectors.ShouldBeEmpty();
            marketingAutomation.Type.ShouldBe(type);
        }

        #endregion

        #endregion

        #endregion
    }
}
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
    public class RuleTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Rule) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var rule = Fixture.Create<Rule>();
            var ruleId = Fixture.Create<int>();
            var ruleName = Fixture.Create<string>();
            var conditionConnector = Fixture.Create<string>();
            var whereClause = Fixture.Create<string>();
            var customerId = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();
            var ruleConditionsList = Fixture.Create<List<RuleCondition>>();

            // Act
            rule.RuleID = ruleId;
            rule.RuleName = ruleName;
            rule.ConditionConnector = conditionConnector;
            rule.WhereClause = whereClause;
            rule.CustomerID = customerId;
            rule.CreatedUserID = createdUserId;
            rule.CreatedDate = createdDate;
            rule.UpdatedUserID = updatedUserId;
            rule.UpdatedDate = updatedDate;
            rule.IsDeleted = isDeleted;
            rule.RuleConditionsList = ruleConditionsList;

            // Assert
            rule.RuleID.ShouldBe(ruleId);
            rule.RuleName.ShouldBe(ruleName);
            rule.ConditionConnector.ShouldBe(conditionConnector);
            rule.WhereClause.ShouldBe(whereClause);
            rule.CustomerID.ShouldBe(customerId);
            rule.CreatedUserID.ShouldBe(createdUserId);
            rule.CreatedDate.ShouldBe(createdDate);
            rule.UpdatedUserID.ShouldBe(updatedUserId);
            rule.UpdatedDate.ShouldBe(updatedDate);
            rule.IsDeleted.ShouldBe(isDeleted);
            rule.RuleConditionsList.ShouldBe(ruleConditionsList);
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (ConditionConnector) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_ConditionConnector_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rule = Fixture.Create<Rule>();
            rule.ConditionConnector = Fixture.Create<string>();
            var stringType = rule.ConditionConnector.GetType();

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

        #region General Getters/Setters : Class (Rule) => Property (ConditionConnector) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_Class_Invalid_Property_ConditionConnectorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConditionConnector = "ConditionConnectorNotPresent";
            var rule  = Fixture.Create<Rule>();

            // Act , Assert
            Should.NotThrow(() => rule.GetType().GetProperty(propertyNameConditionConnector));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_ConditionConnector_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConditionConnector = "ConditionConnector";
            var rule  = Fixture.Create<Rule>();
            var propertyInfo  = rule.GetType().GetProperty(propertyNameConditionConnector);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var rule = Fixture.Create<Rule>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rule.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(rule, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var rule  = Fixture.Create<Rule>();

            // Act , Assert
            Should.NotThrow(() => rule.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var rule  = Fixture.Create<Rule>();
            var propertyInfo  = rule.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var rule = Fixture.Create<Rule>();
            var random = Fixture.Create<int>();

            // Act , Set
            rule.CreatedUserID = random;

            // Assert
            rule.CreatedUserID.ShouldBe(random);
            rule.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var rule = Fixture.Create<Rule>();

            // Act , Set
            rule.CreatedUserID = null;

            // Assert
            rule.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var rule = Fixture.Create<Rule>();
            var propertyInfo = rule.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(rule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            rule.CreatedUserID.ShouldBeNull();
            rule.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var rule  = Fixture.Create<Rule>();

            // Act , Assert
            Should.NotThrow(() => rule.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var rule  = Fixture.Create<Rule>();
            var propertyInfo  = rule.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var rule = Fixture.Create<Rule>();
            var random = Fixture.Create<int>();

            // Act , Set
            rule.CustomerID = random;

            // Assert
            rule.CustomerID.ShouldBe(random);
            rule.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var rule = Fixture.Create<Rule>();

            // Act , Set
            rule.CustomerID = null;

            // Assert
            rule.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var rule = Fixture.Create<Rule>();
            var propertyInfo = rule.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(rule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            rule.CustomerID.ShouldBeNull();
            rule.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var rule  = Fixture.Create<Rule>();

            // Act , Assert
            Should.NotThrow(() => rule.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var rule  = Fixture.Create<Rule>();
            var propertyInfo  = rule.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var rule = Fixture.Create<Rule>();
            var random = Fixture.Create<bool>();

            // Act , Set
            rule.IsDeleted = random;

            // Assert
            rule.IsDeleted.ShouldBe(random);
            rule.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var rule = Fixture.Create<Rule>();

            // Act , Set
            rule.IsDeleted = null;

            // Assert
            rule.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var rule = Fixture.Create<Rule>();
            var propertyInfo = rule.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(rule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            rule.IsDeleted.ShouldBeNull();
            rule.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var rule  = Fixture.Create<Rule>();

            // Act , Assert
            Should.NotThrow(() => rule.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var rule  = Fixture.Create<Rule>();
            var propertyInfo  = rule.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (RuleConditionsList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_Class_Invalid_Property_RuleConditionsListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRuleConditionsList = "RuleConditionsListNotPresent";
            var rule  = Fixture.Create<Rule>();

            // Act , Assert
            Should.NotThrow(() => rule.GetType().GetProperty(propertyNameRuleConditionsList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_RuleConditionsList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRuleConditionsList = "RuleConditionsList";
            var rule  = Fixture.Create<Rule>();
            var propertyInfo  = rule.GetType().GetProperty(propertyNameRuleConditionsList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (RuleID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_RuleID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var rule = Fixture.Create<Rule>();
            rule.RuleID = Fixture.Create<int>();
            var intType = rule.RuleID.GetType();

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

        #region General Getters/Setters : Class (Rule) => Property (RuleID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_Class_Invalid_Property_RuleIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRuleID = "RuleIDNotPresent";
            var rule  = Fixture.Create<Rule>();

            // Act , Assert
            Should.NotThrow(() => rule.GetType().GetProperty(propertyNameRuleID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_RuleID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRuleID = "RuleID";
            var rule  = Fixture.Create<Rule>();
            var propertyInfo  = rule.GetType().GetProperty(propertyNameRuleID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (RuleName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_RuleName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rule = Fixture.Create<Rule>();
            rule.RuleName = Fixture.Create<string>();
            var stringType = rule.RuleName.GetType();

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

        #region General Getters/Setters : Class (Rule) => Property (RuleName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_Class_Invalid_Property_RuleNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRuleName = "RuleNameNotPresent";
            var rule  = Fixture.Create<Rule>();

            // Act , Assert
            Should.NotThrow(() => rule.GetType().GetProperty(propertyNameRuleName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_RuleName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRuleName = "RuleName";
            var rule  = Fixture.Create<Rule>();
            var propertyInfo  = rule.GetType().GetProperty(propertyNameRuleName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var rule = Fixture.Create<Rule>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rule.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(rule, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var rule  = Fixture.Create<Rule>();

            // Act , Assert
            Should.NotThrow(() => rule.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var rule  = Fixture.Create<Rule>();
            var propertyInfo  = rule.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var rule = Fixture.Create<Rule>();
            var random = Fixture.Create<int>();

            // Act , Set
            rule.UpdatedUserID = random;

            // Assert
            rule.UpdatedUserID.ShouldBe(random);
            rule.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var rule = Fixture.Create<Rule>();

            // Act , Set
            rule.UpdatedUserID = null;

            // Assert
            rule.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var rule = Fixture.Create<Rule>();
            var propertyInfo = rule.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(rule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            rule.UpdatedUserID.ShouldBeNull();
            rule.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var rule  = Fixture.Create<Rule>();

            // Act , Assert
            Should.NotThrow(() => rule.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var rule  = Fixture.Create<Rule>();
            var propertyInfo  = rule.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Rule) => Property (WhereClause) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_WhereClause_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rule = Fixture.Create<Rule>();
            rule.WhereClause = Fixture.Create<string>();
            var stringType = rule.WhereClause.GetType();

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

        #region General Getters/Setters : Class (Rule) => Property (WhereClause) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_Class_Invalid_Property_WhereClauseNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameWhereClause = "WhereClauseNotPresent";
            var rule  = Fixture.Create<Rule>();

            // Act , Assert
            Should.NotThrow(() => rule.GetType().GetProperty(propertyNameWhereClause));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Rule_WhereClause_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameWhereClause = "WhereClause";
            var rule  = Fixture.Create<Rule>();
            var propertyInfo  = rule.GetType().GetProperty(propertyNameWhereClause);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Rule) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Rule_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Rule());
        }

        #endregion

        #region General Constructor : Class (Rule) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Rule_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfRule = Fixture.CreateMany<Rule>(2).ToList();
            var firstRule = instancesOfRule.FirstOrDefault();
            var lastRule = instancesOfRule.Last();

            // Act, Assert
            firstRule.ShouldNotBeNull();
            lastRule.ShouldNotBeNull();
            firstRule.ShouldNotBeSameAs(lastRule);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Rule_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstRule = new Rule();
            var secondRule = new Rule();
            var thirdRule = new Rule();
            var fourthRule = new Rule();
            var fifthRule = new Rule();
            var sixthRule = new Rule();

            // Act, Assert
            firstRule.ShouldNotBeNull();
            secondRule.ShouldNotBeNull();
            thirdRule.ShouldNotBeNull();
            fourthRule.ShouldNotBeNull();
            fifthRule.ShouldNotBeNull();
            sixthRule.ShouldNotBeNull();
            firstRule.ShouldNotBeSameAs(secondRule);
            thirdRule.ShouldNotBeSameAs(firstRule);
            fourthRule.ShouldNotBeSameAs(firstRule);
            fifthRule.ShouldNotBeSameAs(firstRule);
            sixthRule.ShouldNotBeSameAs(firstRule);
            sixthRule.ShouldNotBeSameAs(fourthRule);
        }

        #endregion

        #region General Constructor : Class (Rule) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Rule_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var ruleId = -1;

            // Act
            var rule = new Rule();

            // Assert
            rule.RuleID.ShouldBe(ruleId);
            rule.RuleName.ShouldBeNull();
            rule.ConditionConnector.ShouldBeNull();
            rule.WhereClause.ShouldBeNull();
            rule.CustomerID.ShouldBeNull();
            rule.RuleConditionsList.ShouldBeNull();
            rule.CreatedUserID.ShouldBeNull();
            rule.CreatedDate.ShouldBeNull();
            rule.UpdatedUserID.ShouldBeNull();
            rule.UpdatedDate.ShouldBeNull();
            rule.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}
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
    public class DynamicTagRuleTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (DynamicTagRule) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var dynamicTagRuleId = Fixture.Create<int>();
            var ruleId = Fixture.Create<int?>();
            var dynamicTagId = Fixture.Create<int?>();
            var contentId = Fixture.Create<int?>();
            var priority = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var rule = Fixture.Create<ECN_Framework_Entities.Communicator.Rule>();

            // Act
            dynamicTagRule.DynamicTagRuleID = dynamicTagRuleId;
            dynamicTagRule.RuleID = ruleId;
            dynamicTagRule.DynamicTagID = dynamicTagId;
            dynamicTagRule.ContentID = contentId;
            dynamicTagRule.Priority = priority;
            dynamicTagRule.CreatedUserID = createdUserId;
            dynamicTagRule.UpdatedUserID = updatedUserId;
            dynamicTagRule.IsDeleted = isDeleted;
            dynamicTagRule.Rule = rule;

            // Assert
            dynamicTagRule.DynamicTagRuleID.ShouldBe(dynamicTagRuleId);
            dynamicTagRule.RuleID.ShouldBe(ruleId);
            dynamicTagRule.DynamicTagID.ShouldBe(dynamicTagId);
            dynamicTagRule.ContentID.ShouldBe(contentId);
            dynamicTagRule.Priority.ShouldBe(priority);
            dynamicTagRule.CreatedUserID.ShouldBe(createdUserId);
            dynamicTagRule.CreatedDate.ShouldBeNull();
            dynamicTagRule.UpdatedUserID.ShouldBe(updatedUserId);
            dynamicTagRule.UpdatedDate.ShouldBeNull();
            dynamicTagRule.IsDeleted.ShouldBe(isDeleted);
            dynamicTagRule.Rule.ShouldBe(rule);
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (ContentID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_ContentID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var random = Fixture.Create<int>();

            // Act , Set
            dynamicTagRule.ContentID = random;

            // Assert
            dynamicTagRule.ContentID.ShouldBe(random);
            dynamicTagRule.ContentID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_ContentID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();

            // Act , Set
            dynamicTagRule.ContentID = null;

            // Assert
            dynamicTagRule.ContentID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_ContentID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameContentID = "ContentID";
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var propertyInfo = dynamicTagRule.GetType().GetProperty(propertyNameContentID);

            // Act , Set
            propertyInfo.SetValue(dynamicTagRule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            dynamicTagRule.ContentID.ShouldBeNull();
            dynamicTagRule.ContentID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (ContentID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Class_Invalid_Property_ContentIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentID = "ContentIDNotPresent";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();

            // Act , Assert
            Should.NotThrow(() => dynamicTagRule.GetType().GetProperty(propertyNameContentID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_ContentID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentID = "ContentID";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();
            var propertyInfo  = dynamicTagRule.GetType().GetProperty(propertyNameContentID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = dynamicTagRule.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(dynamicTagRule, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();

            // Act , Assert
            Should.NotThrow(() => dynamicTagRule.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();
            var propertyInfo  = dynamicTagRule.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var random = Fixture.Create<int>();

            // Act , Set
            dynamicTagRule.CreatedUserID = random;

            // Assert
            dynamicTagRule.CreatedUserID.ShouldBe(random);
            dynamicTagRule.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();

            // Act , Set
            dynamicTagRule.CreatedUserID = null;

            // Assert
            dynamicTagRule.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var propertyInfo = dynamicTagRule.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(dynamicTagRule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            dynamicTagRule.CreatedUserID.ShouldBeNull();
            dynamicTagRule.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();

            // Act , Assert
            Should.NotThrow(() => dynamicTagRule.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();
            var propertyInfo  = dynamicTagRule.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (DynamicTagID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_DynamicTagID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var random = Fixture.Create<int>();

            // Act , Set
            dynamicTagRule.DynamicTagID = random;

            // Assert
            dynamicTagRule.DynamicTagID.ShouldBe(random);
            dynamicTagRule.DynamicTagID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_DynamicTagID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();

            // Act , Set
            dynamicTagRule.DynamicTagID = null;

            // Assert
            dynamicTagRule.DynamicTagID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_DynamicTagID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameDynamicTagID = "DynamicTagID";
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var propertyInfo = dynamicTagRule.GetType().GetProperty(propertyNameDynamicTagID);

            // Act , Set
            propertyInfo.SetValue(dynamicTagRule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            dynamicTagRule.DynamicTagID.ShouldBeNull();
            dynamicTagRule.DynamicTagID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (DynamicTagID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Class_Invalid_Property_DynamicTagIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDynamicTagID = "DynamicTagIDNotPresent";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();

            // Act , Assert
            Should.NotThrow(() => dynamicTagRule.GetType().GetProperty(propertyNameDynamicTagID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_DynamicTagID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDynamicTagID = "DynamicTagID";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();
            var propertyInfo  = dynamicTagRule.GetType().GetProperty(propertyNameDynamicTagID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (DynamicTagRuleID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_DynamicTagRuleID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            dynamicTagRule.DynamicTagRuleID = Fixture.Create<int>();
            var intType = dynamicTagRule.DynamicTagRuleID.GetType();

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

        #region General Getters/Setters : Class (DynamicTagRule) => Property (DynamicTagRuleID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Class_Invalid_Property_DynamicTagRuleIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDynamicTagRuleID = "DynamicTagRuleIDNotPresent";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();

            // Act , Assert
            Should.NotThrow(() => dynamicTagRule.GetType().GetProperty(propertyNameDynamicTagRuleID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_DynamicTagRuleID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDynamicTagRuleID = "DynamicTagRuleID";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();
            var propertyInfo  = dynamicTagRule.GetType().GetProperty(propertyNameDynamicTagRuleID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var random = Fixture.Create<bool>();

            // Act , Set
            dynamicTagRule.IsDeleted = random;

            // Assert
            dynamicTagRule.IsDeleted.ShouldBe(random);
            dynamicTagRule.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();

            // Act , Set
            dynamicTagRule.IsDeleted = null;

            // Assert
            dynamicTagRule.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var propertyInfo = dynamicTagRule.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(dynamicTagRule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            dynamicTagRule.IsDeleted.ShouldBeNull();
            dynamicTagRule.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();

            // Act , Assert
            Should.NotThrow(() => dynamicTagRule.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();
            var propertyInfo  = dynamicTagRule.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (Priority) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Priority_Property_Data_Without_Null_Test()
        {
            // Arrange
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var random = Fixture.Create<int>();

            // Act , Set
            dynamicTagRule.Priority = random;

            // Assert
            dynamicTagRule.Priority.ShouldBe(random);
            dynamicTagRule.Priority.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Priority_Property_Only_Null_Data_Test()
        {
            // Arrange
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();

            // Act , Set
            dynamicTagRule.Priority = null;

            // Assert
            dynamicTagRule.Priority.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Priority_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNamePriority = "Priority";
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var propertyInfo = dynamicTagRule.GetType().GetProperty(propertyNamePriority);

            // Act , Set
            propertyInfo.SetValue(dynamicTagRule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            dynamicTagRule.Priority.ShouldBeNull();
            dynamicTagRule.Priority.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (Priority) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Class_Invalid_Property_PriorityNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePriority = "PriorityNotPresent";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();

            // Act , Assert
            Should.NotThrow(() => dynamicTagRule.GetType().GetProperty(propertyNamePriority));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Priority_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePriority = "Priority";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();
            var propertyInfo  = dynamicTagRule.GetType().GetProperty(propertyNamePriority);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (Rule) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Rule_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameRule = "Rule";
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = dynamicTagRule.GetType().GetProperty(propertyNameRule);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(dynamicTagRule, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (Rule) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Class_Invalid_Property_RuleNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRule = "RuleNotPresent";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();

            // Act , Assert
            Should.NotThrow(() => dynamicTagRule.GetType().GetProperty(propertyNameRule));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Rule_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRule = "Rule";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();
            var propertyInfo  = dynamicTagRule.GetType().GetProperty(propertyNameRule);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (RuleID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_RuleID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var random = Fixture.Create<int>();

            // Act , Set
            dynamicTagRule.RuleID = random;

            // Assert
            dynamicTagRule.RuleID.ShouldBe(random);
            dynamicTagRule.RuleID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_RuleID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();

            // Act , Set
            dynamicTagRule.RuleID = null;

            // Assert
            dynamicTagRule.RuleID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_RuleID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameRuleID = "RuleID";
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var propertyInfo = dynamicTagRule.GetType().GetProperty(propertyNameRuleID);

            // Act , Set
            propertyInfo.SetValue(dynamicTagRule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            dynamicTagRule.RuleID.ShouldBeNull();
            dynamicTagRule.RuleID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (RuleID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Class_Invalid_Property_RuleIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRuleID = "RuleIDNotPresent";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();

            // Act , Assert
            Should.NotThrow(() => dynamicTagRule.GetType().GetProperty(propertyNameRuleID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_RuleID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRuleID = "RuleID";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();
            var propertyInfo  = dynamicTagRule.GetType().GetProperty(propertyNameRuleID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = dynamicTagRule.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(dynamicTagRule, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();

            // Act , Assert
            Should.NotThrow(() => dynamicTagRule.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();
            var propertyInfo  = dynamicTagRule.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var random = Fixture.Create<int>();

            // Act , Set
            dynamicTagRule.UpdatedUserID = random;

            // Assert
            dynamicTagRule.UpdatedUserID.ShouldBe(random);
            dynamicTagRule.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();

            // Act , Set
            dynamicTagRule.UpdatedUserID = null;

            // Assert
            dynamicTagRule.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var dynamicTagRule = Fixture.Create<DynamicTagRule>();
            var propertyInfo = dynamicTagRule.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(dynamicTagRule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            dynamicTagRule.UpdatedUserID.ShouldBeNull();
            dynamicTagRule.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTagRule) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();

            // Act , Assert
            Should.NotThrow(() => dynamicTagRule.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTagRule_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var dynamicTagRule  = Fixture.Create<DynamicTagRule>();
            var propertyInfo  = dynamicTagRule.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (DynamicTagRule) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DynamicTagRule_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new DynamicTagRule());
        }

        #endregion

        #region General Constructor : Class (DynamicTagRule) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DynamicTagRule_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfDynamicTagRule = Fixture.CreateMany<DynamicTagRule>(2).ToList();
            var firstDynamicTagRule = instancesOfDynamicTagRule.FirstOrDefault();
            var lastDynamicTagRule = instancesOfDynamicTagRule.Last();

            // Act, Assert
            firstDynamicTagRule.ShouldNotBeNull();
            lastDynamicTagRule.ShouldNotBeNull();
            firstDynamicTagRule.ShouldNotBeSameAs(lastDynamicTagRule);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DynamicTagRule_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstDynamicTagRule = new DynamicTagRule();
            var secondDynamicTagRule = new DynamicTagRule();
            var thirdDynamicTagRule = new DynamicTagRule();
            var fourthDynamicTagRule = new DynamicTagRule();
            var fifthDynamicTagRule = new DynamicTagRule();
            var sixthDynamicTagRule = new DynamicTagRule();

            // Act, Assert
            firstDynamicTagRule.ShouldNotBeNull();
            secondDynamicTagRule.ShouldNotBeNull();
            thirdDynamicTagRule.ShouldNotBeNull();
            fourthDynamicTagRule.ShouldNotBeNull();
            fifthDynamicTagRule.ShouldNotBeNull();
            sixthDynamicTagRule.ShouldNotBeNull();
            firstDynamicTagRule.ShouldNotBeSameAs(secondDynamicTagRule);
            thirdDynamicTagRule.ShouldNotBeSameAs(firstDynamicTagRule);
            fourthDynamicTagRule.ShouldNotBeSameAs(firstDynamicTagRule);
            fifthDynamicTagRule.ShouldNotBeSameAs(firstDynamicTagRule);
            sixthDynamicTagRule.ShouldNotBeSameAs(firstDynamicTagRule);
            sixthDynamicTagRule.ShouldNotBeSameAs(fourthDynamicTagRule);
        }

        #endregion

        #region General Constructor : Class (DynamicTagRule) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DynamicTagRule_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var dynamicTagRuleId = -1;

            // Act
            var dynamicTagRule = new DynamicTagRule();

            // Assert
            dynamicTagRule.DynamicTagRuleID.ShouldBe(dynamicTagRuleId);
            dynamicTagRule.RuleID.ShouldBeNull();
            dynamicTagRule.DynamicTagID.ShouldBeNull();
            dynamicTagRule.ContentID.ShouldBeNull();
            dynamicTagRule.Priority.ShouldBeNull();
            dynamicTagRule.CreatedUserID.ShouldBeNull();
            dynamicTagRule.CreatedDate.ShouldBeNull();
            dynamicTagRule.UpdatedUserID.ShouldBeNull();
            dynamicTagRule.UpdatedDate.ShouldBeNull();
            dynamicTagRule.IsDeleted.ShouldBeNull();
            dynamicTagRule.Rule.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}
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
    public class BlastPlansTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastPlans) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();
            var blastPlanId = Fixture.Create<int>();
            var blastId = Fixture.Create<int?>();
            var customerId = Fixture.Create<int?>();
            var groupId = Fixture.Create<int?>();
            var eventType = Fixture.Create<string>();
            var period = Fixture.Create<decimal?>();
            var blastDay = Fixture.Create<int?>();
            var planType = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            blastPlans.BlastPlanID = blastPlanId;
            blastPlans.BlastID = blastId;
            blastPlans.CustomerID = customerId;
            blastPlans.GroupID = groupId;
            blastPlans.EventType = eventType;
            blastPlans.Period = period;
            blastPlans.BlastDay = blastDay;
            blastPlans.PlanType = planType;
            blastPlans.CreatedUserID = createdUserId;
            blastPlans.CreatedDate = createdDate;
            blastPlans.UpdatedUserID = updatedUserId;
            blastPlans.UpdatedDate = updatedDate;
            blastPlans.IsDeleted = isDeleted;

            // Assert
            blastPlans.BlastPlanID.ShouldBe(blastPlanId);
            blastPlans.BlastID.ShouldBe(blastId);
            blastPlans.CustomerID.ShouldBe(customerId);
            blastPlans.GroupID.ShouldBe(groupId);
            blastPlans.EventType.ShouldBe(eventType);
            blastPlans.Period.ShouldBe(period);
            blastPlans.BlastDay.ShouldBe(blastDay);
            blastPlans.PlanType.ShouldBe(planType);
            blastPlans.CreatedUserID.ShouldBe(createdUserId);
            blastPlans.CreatedDate.ShouldBe(createdDate);
            blastPlans.UpdatedUserID.ShouldBe(updatedUserId);
            blastPlans.UpdatedDate.ShouldBe(updatedDate);
            blastPlans.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (BlastDay) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_BlastDay_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastPlans.BlastDay = random;

            // Assert
            blastPlans.BlastDay.ShouldBe(random);
            blastPlans.BlastDay.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_BlastDay_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();    

            // Act , Set
            blastPlans.BlastDay = null;

            // Assert
            blastPlans.BlastDay.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_BlastDay_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastDay = "BlastDay";
            var blastPlans = Fixture.Create<BlastPlans>();
            var propertyInfo = blastPlans.GetType().GetProperty(propertyNameBlastDay);

            // Act , Set
            propertyInfo.SetValue(blastPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastPlans.BlastDay.ShouldBeNull();
            blastPlans.BlastDay.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (BlastDay) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Class_Invalid_Property_BlastDayNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastDay = "BlastDayNotPresent";
            var blastPlans  = Fixture.Create<BlastPlans>();

            // Act , Assert
            Should.NotThrow(() => blastPlans.GetType().GetProperty(propertyNameBlastDay));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_BlastDay_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastDay = "BlastDay";
            var blastPlans  = Fixture.Create<BlastPlans>();
            var propertyInfo  = blastPlans.GetType().GetProperty(propertyNameBlastDay);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (BlastID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_BlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastPlans.BlastID = random;

            // Assert
            blastPlans.BlastID.ShouldBe(random);
            blastPlans.BlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_BlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();    

            // Act , Set
            blastPlans.BlastID = null;

            // Assert
            blastPlans.BlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_BlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastID = "BlastID";
            var blastPlans = Fixture.Create<BlastPlans>();
            var propertyInfo = blastPlans.GetType().GetProperty(propertyNameBlastID);

            // Act , Set
            propertyInfo.SetValue(blastPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastPlans.BlastID.ShouldBeNull();
            blastPlans.BlastID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastPlans  = Fixture.Create<BlastPlans>();

            // Act , Assert
            Should.NotThrow(() => blastPlans.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastPlans  = Fixture.Create<BlastPlans>();
            var propertyInfo  = blastPlans.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (BlastPlanID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_BlastPlanID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();
            blastPlans.BlastPlanID = Fixture.Create<int>();
            var intType = blastPlans.BlastPlanID.GetType();

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

        #region General Getters/Setters : Class (BlastPlans) => Property (BlastPlanID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Class_Invalid_Property_BlastPlanIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastPlanID = "BlastPlanIDNotPresent";
            var blastPlans  = Fixture.Create<BlastPlans>();

            // Act , Assert
            Should.NotThrow(() => blastPlans.GetType().GetProperty(propertyNameBlastPlanID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_BlastPlanID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastPlanID = "BlastPlanID";
            var blastPlans  = Fixture.Create<BlastPlans>();
            var propertyInfo  = blastPlans.GetType().GetProperty(propertyNameBlastPlanID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var blastPlans = Fixture.Create<BlastPlans>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastPlans.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastPlans, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var blastPlans  = Fixture.Create<BlastPlans>();

            // Act , Assert
            Should.NotThrow(() => blastPlans.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var blastPlans  = Fixture.Create<BlastPlans>();
            var propertyInfo  = blastPlans.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastPlans.CreatedUserID = random;

            // Assert
            blastPlans.CreatedUserID.ShouldBe(random);
            blastPlans.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();    

            // Act , Set
            blastPlans.CreatedUserID = null;

            // Assert
            blastPlans.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var blastPlans = Fixture.Create<BlastPlans>();
            var propertyInfo = blastPlans.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(blastPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastPlans.CreatedUserID.ShouldBeNull();
            blastPlans.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var blastPlans  = Fixture.Create<BlastPlans>();

            // Act , Assert
            Should.NotThrow(() => blastPlans.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var blastPlans  = Fixture.Create<BlastPlans>();
            var propertyInfo  = blastPlans.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastPlans.CustomerID = random;

            // Assert
            blastPlans.CustomerID.ShouldBe(random);
            blastPlans.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();    

            // Act , Set
            blastPlans.CustomerID = null;

            // Assert
            blastPlans.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var blastPlans = Fixture.Create<BlastPlans>();
            var propertyInfo = blastPlans.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(blastPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastPlans.CustomerID.ShouldBeNull();
            blastPlans.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var blastPlans  = Fixture.Create<BlastPlans>();

            // Act , Assert
            Should.NotThrow(() => blastPlans.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var blastPlans  = Fixture.Create<BlastPlans>();
            var propertyInfo  = blastPlans.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (EventType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_EventType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();
            blastPlans.EventType = Fixture.Create<string>();
            var stringType = blastPlans.EventType.GetType();

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

        #region General Getters/Setters : Class (BlastPlans) => Property (EventType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Class_Invalid_Property_EventTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEventType = "EventTypeNotPresent";
            var blastPlans  = Fixture.Create<BlastPlans>();

            // Act , Assert
            Should.NotThrow(() => blastPlans.GetType().GetProperty(propertyNameEventType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_EventType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEventType = "EventType";
            var blastPlans  = Fixture.Create<BlastPlans>();
            var propertyInfo  = blastPlans.GetType().GetProperty(propertyNameEventType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (GroupID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_GroupID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastPlans.GroupID = random;

            // Assert
            blastPlans.GroupID.ShouldBe(random);
            blastPlans.GroupID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_GroupID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();    

            // Act , Set
            blastPlans.GroupID = null;

            // Assert
            blastPlans.GroupID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_GroupID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameGroupID = "GroupID";
            var blastPlans = Fixture.Create<BlastPlans>();
            var propertyInfo = blastPlans.GetType().GetProperty(propertyNameGroupID);

            // Act , Set
            propertyInfo.SetValue(blastPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastPlans.GroupID.ShouldBeNull();
            blastPlans.GroupID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (GroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Class_Invalid_Property_GroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupIDNotPresent";
            var blastPlans  = Fixture.Create<BlastPlans>();

            // Act , Assert
            Should.NotThrow(() => blastPlans.GetType().GetProperty(propertyNameGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_GroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupID";
            var blastPlans  = Fixture.Create<BlastPlans>();
            var propertyInfo  = blastPlans.GetType().GetProperty(propertyNameGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();
            var random = Fixture.Create<bool>();

            // Act , Set
            blastPlans.IsDeleted = random;

            // Assert
            blastPlans.IsDeleted.ShouldBe(random);
            blastPlans.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();    

            // Act , Set
            blastPlans.IsDeleted = null;

            // Assert
            blastPlans.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var blastPlans = Fixture.Create<BlastPlans>();
            var propertyInfo = blastPlans.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(blastPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastPlans.IsDeleted.ShouldBeNull();
            blastPlans.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var blastPlans  = Fixture.Create<BlastPlans>();

            // Act , Assert
            Should.NotThrow(() => blastPlans.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var blastPlans  = Fixture.Create<BlastPlans>();
            var propertyInfo  = blastPlans.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (Period) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Period_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();
            var random = Fixture.Create<decimal>();

            // Act , Set
            blastPlans.Period = random;

            // Assert
            blastPlans.Period.ShouldBe(random);
            blastPlans.Period.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Period_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();    

            // Act , Set
            blastPlans.Period = null;

            // Assert
            blastPlans.Period.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Period_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNamePeriod = "Period";
            var blastPlans = Fixture.Create<BlastPlans>();
            var propertyInfo = blastPlans.GetType().GetProperty(propertyNamePeriod);

            // Act , Set
            propertyInfo.SetValue(blastPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastPlans.Period.ShouldBeNull();
            blastPlans.Period.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (Period) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Class_Invalid_Property_PeriodNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePeriod = "PeriodNotPresent";
            var blastPlans  = Fixture.Create<BlastPlans>();

            // Act , Assert
            Should.NotThrow(() => blastPlans.GetType().GetProperty(propertyNamePeriod));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Period_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePeriod = "Period";
            var blastPlans  = Fixture.Create<BlastPlans>();
            var propertyInfo  = blastPlans.GetType().GetProperty(propertyNamePeriod);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (PlanType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_PlanType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();
            blastPlans.PlanType = Fixture.Create<string>();
            var stringType = blastPlans.PlanType.GetType();

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

        #region General Getters/Setters : Class (BlastPlans) => Property (PlanType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Class_Invalid_Property_PlanTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePlanType = "PlanTypeNotPresent";
            var blastPlans  = Fixture.Create<BlastPlans>();

            // Act , Assert
            Should.NotThrow(() => blastPlans.GetType().GetProperty(propertyNamePlanType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_PlanType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePlanType = "PlanType";
            var blastPlans  = Fixture.Create<BlastPlans>();
            var propertyInfo  = blastPlans.GetType().GetProperty(propertyNamePlanType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var blastPlans = Fixture.Create<BlastPlans>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastPlans.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastPlans, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var blastPlans  = Fixture.Create<BlastPlans>();

            // Act , Assert
            Should.NotThrow(() => blastPlans.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var blastPlans  = Fixture.Create<BlastPlans>();
            var propertyInfo  = blastPlans.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastPlans.UpdatedUserID = random;

            // Assert
            blastPlans.UpdatedUserID.ShouldBe(random);
            blastPlans.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastPlans = Fixture.Create<BlastPlans>();    

            // Act , Set
            blastPlans.UpdatedUserID = null;

            // Assert
            blastPlans.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var blastPlans = Fixture.Create<BlastPlans>();
            var propertyInfo = blastPlans.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(blastPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastPlans.UpdatedUserID.ShouldBeNull();
            blastPlans.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastPlans) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var blastPlans  = Fixture.Create<BlastPlans>();

            // Act , Assert
            Should.NotThrow(() => blastPlans.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastPlans_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var blastPlans  = Fixture.Create<BlastPlans>();
            var propertyInfo  = blastPlans.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastPlans) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastPlans_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastPlans());
        }

        #endregion

        #region General Constructor : Class (BlastPlans) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastPlans_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastPlans = Fixture.CreateMany<BlastPlans>(2).ToList();
            var firstBlastPlans = instancesOfBlastPlans.FirstOrDefault();
            var lastBlastPlans = instancesOfBlastPlans.Last();

            // Act, Assert
            firstBlastPlans.ShouldNotBeNull();
            lastBlastPlans.ShouldNotBeNull();
            firstBlastPlans.ShouldNotBeSameAs(lastBlastPlans);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastPlans_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastPlans = new BlastPlans();
            var secondBlastPlans = new BlastPlans();
            var thirdBlastPlans = new BlastPlans();
            var fourthBlastPlans = new BlastPlans();
            var fifthBlastPlans = new BlastPlans();
            var sixthBlastPlans = new BlastPlans();

            // Act, Assert
            firstBlastPlans.ShouldNotBeNull();
            secondBlastPlans.ShouldNotBeNull();
            thirdBlastPlans.ShouldNotBeNull();
            fourthBlastPlans.ShouldNotBeNull();
            fifthBlastPlans.ShouldNotBeNull();
            sixthBlastPlans.ShouldNotBeNull();
            firstBlastPlans.ShouldNotBeSameAs(secondBlastPlans);
            thirdBlastPlans.ShouldNotBeSameAs(firstBlastPlans);
            fourthBlastPlans.ShouldNotBeSameAs(firstBlastPlans);
            fifthBlastPlans.ShouldNotBeSameAs(firstBlastPlans);
            sixthBlastPlans.ShouldNotBeSameAs(firstBlastPlans);
            sixthBlastPlans.ShouldNotBeSameAs(fourthBlastPlans);
        }

        #endregion

        #region General Constructor : Class (BlastPlans) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastPlans_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var blastPlanId = -1;
            var eventType = string.Empty;
            var planType = string.Empty;

            // Act
            var blastPlans = new BlastPlans();

            // Assert
            blastPlans.BlastPlanID.ShouldBe(blastPlanId);
            blastPlans.BlastID.ShouldBeNull();
            blastPlans.CustomerID.ShouldBeNull();
            blastPlans.GroupID.ShouldBeNull();
            blastPlans.EventType.ShouldBe(eventType);
            blastPlans.Period.ShouldBeNull();
            blastPlans.BlastDay.ShouldBeNull();
            blastPlans.PlanType.ShouldBe(planType);
            blastPlans.CreatedUserID.ShouldBeNull();
            blastPlans.CreatedDate.ShouldBeNull();
            blastPlans.UpdatedUserID.ShouldBeNull();
            blastPlans.UpdatedDate.ShouldBeNull();
            blastPlans.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}
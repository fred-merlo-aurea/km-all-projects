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
    public class LayoutPlansTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (LayoutPlans) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var layoutPlanId = Fixture.Create<int>();
            var layoutId = Fixture.Create<int?>();
            var eventType = Fixture.Create<string>();
            var blastId = Fixture.Create<int?>();
            var period = Fixture.Create<decimal?>();
            var criteria = Fixture.Create<string>();
            var customerId = Fixture.Create<int?>();
            var actionName = Fixture.Create<string>();
            var groupId = Fixture.Create<int?>();
            var status = Fixture.Create<string>();
            var smartFormId = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();
            var campaignItemId = Fixture.Create<int?>();
            var tokenUId = Fixture.Create<Guid?>();

            // Act
            layoutPlans.LayoutPlanID = layoutPlanId;
            layoutPlans.LayoutID = layoutId;
            layoutPlans.EventType = eventType;
            layoutPlans.BlastID = blastId;
            layoutPlans.Period = period;
            layoutPlans.Criteria = criteria;
            layoutPlans.CustomerID = customerId;
            layoutPlans.ActionName = actionName;
            layoutPlans.GroupID = groupId;
            layoutPlans.Status = status;
            layoutPlans.SmartFormID = smartFormId;
            layoutPlans.CreatedUserID = createdUserId;
            layoutPlans.CreatedDate = createdDate;
            layoutPlans.UpdatedUserID = updatedUserId;
            layoutPlans.UpdatedDate = updatedDate;
            layoutPlans.IsDeleted = isDeleted;
            layoutPlans.CampaignItemID = campaignItemId;
            layoutPlans.TokenUID = tokenUId;

            // Assert
            layoutPlans.HasValidID.ShouldBeTrue();
            layoutPlans.LayoutPlanID.ShouldBe(layoutPlanId);
            layoutPlans.LayoutID.ShouldBe(layoutId);
            layoutPlans.EventType.ShouldBe(eventType);
            layoutPlans.BlastID.ShouldBe(blastId);
            layoutPlans.Period.ShouldBe(period);
            layoutPlans.Criteria.ShouldBe(criteria);
            layoutPlans.CustomerID.ShouldBe(customerId);
            layoutPlans.ActionName.ShouldBe(actionName);
            layoutPlans.GroupID.ShouldBe(groupId);
            layoutPlans.Status.ShouldBe(status);
            layoutPlans.SmartFormID.ShouldBe(smartFormId);
            layoutPlans.CreatedUserID.ShouldBe(createdUserId);
            layoutPlans.CreatedDate.ShouldBe(createdDate);
            layoutPlans.UpdatedUserID.ShouldBe(updatedUserId);
            layoutPlans.UpdatedDate.ShouldBe(updatedDate);
            layoutPlans.IsDeleted.ShouldBe(isDeleted);
            layoutPlans.CampaignItemID.ShouldBe(campaignItemId);
            layoutPlans.TokenUID.ShouldBe(tokenUId);
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (ActionName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_ActionName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            layoutPlans.ActionName = Fixture.Create<string>();
            var stringType = layoutPlans.ActionName.GetType();

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

        #region General Getters/Setters : Class (LayoutPlans) => Property (ActionName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_ActionNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionName = "ActionNameNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameActionName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_ActionName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionName = "ActionName";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameActionName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (BlastID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_BlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var random = Fixture.Create<int>();

            // Act , Set
            layoutPlans.BlastID = random;

            // Assert
            layoutPlans.BlastID.ShouldBe(random);
            layoutPlans.BlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_BlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();

            // Act , Set
            layoutPlans.BlastID = null;

            // Assert
            layoutPlans.BlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_BlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastID = "BlastID";
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var propertyInfo = layoutPlans.GetType().GetProperty(propertyNameBlastID);

            // Act , Set
            propertyInfo.SetValue(layoutPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layoutPlans.BlastID.ShouldBeNull();
            layoutPlans.BlastID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (CampaignItemID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_CampaignItemID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var random = Fixture.Create<int>();

            // Act , Set
            layoutPlans.CampaignItemID = random;

            // Assert
            layoutPlans.CampaignItemID.ShouldBe(random);
            layoutPlans.CampaignItemID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_CampaignItemID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();

            // Act , Set
            layoutPlans.CampaignItemID = null;

            // Assert
            layoutPlans.CampaignItemID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_CampaignItemID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCampaignItemID = "CampaignItemID";
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var propertyInfo = layoutPlans.GetType().GetProperty(propertyNameCampaignItemID);

            // Act , Set
            propertyInfo.SetValue(layoutPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layoutPlans.CampaignItemID.ShouldBeNull();
            layoutPlans.CampaignItemID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (CampaignItemID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_CampaignItemIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemID = "CampaignItemIDNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameCampaignItemID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_CampaignItemID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemID = "CampaignItemID";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameCampaignItemID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layoutPlans.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layoutPlans, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var random = Fixture.Create<int>();

            // Act , Set
            layoutPlans.CreatedUserID = random;

            // Assert
            layoutPlans.CreatedUserID.ShouldBe(random);
            layoutPlans.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();

            // Act , Set
            layoutPlans.CreatedUserID = null;

            // Assert
            layoutPlans.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var propertyInfo = layoutPlans.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(layoutPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layoutPlans.CreatedUserID.ShouldBeNull();
            layoutPlans.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (Criteria) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Criteria_Property_String_Type_Verify_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            layoutPlans.Criteria = Fixture.Create<string>();
            var stringType = layoutPlans.Criteria.GetType();

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

        #region General Getters/Setters : Class (LayoutPlans) => Property (Criteria) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_CriteriaNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCriteria = "CriteriaNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameCriteria));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Criteria_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCriteria = "Criteria";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameCriteria);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var random = Fixture.Create<int>();

            // Act , Set
            layoutPlans.CustomerID = random;

            // Assert
            layoutPlans.CustomerID.ShouldBe(random);
            layoutPlans.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();

            // Act , Set
            layoutPlans.CustomerID = null;

            // Assert
            layoutPlans.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var propertyInfo = layoutPlans.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(layoutPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layoutPlans.CustomerID.ShouldBeNull();
            layoutPlans.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (EventType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_EventType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            layoutPlans.EventType = Fixture.Create<string>();
            var stringType = layoutPlans.EventType.GetType();

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

        #region General Getters/Setters : Class (LayoutPlans) => Property (EventType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_EventTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEventType = "EventTypeNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameEventType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_EventType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEventType = "EventType";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameEventType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (GroupID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_GroupID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var random = Fixture.Create<int>();

            // Act , Set
            layoutPlans.GroupID = random;

            // Assert
            layoutPlans.GroupID.ShouldBe(random);
            layoutPlans.GroupID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_GroupID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();

            // Act , Set
            layoutPlans.GroupID = null;

            // Assert
            layoutPlans.GroupID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_GroupID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameGroupID = "GroupID";
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var propertyInfo = layoutPlans.GetType().GetProperty(propertyNameGroupID);

            // Act , Set
            propertyInfo.SetValue(layoutPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layoutPlans.GroupID.ShouldBeNull();
            layoutPlans.GroupID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (GroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_GroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupIDNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_GroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupID";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (HasValidID) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_HasValidID_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var boolType = layoutPlans.HasValidID.GetType();

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

        #region General Getters/Setters : Class (LayoutPlans) => Property (HasValidID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_HasValidIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHasValidID = "HasValidIDNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameHasValidID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_HasValidID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameHasValidID = "HasValidID";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameHasValidID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var random = Fixture.Create<bool>();

            // Act , Set
            layoutPlans.IsDeleted = random;

            // Assert
            layoutPlans.IsDeleted.ShouldBe(random);
            layoutPlans.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();

            // Act , Set
            layoutPlans.IsDeleted = null;

            // Assert
            layoutPlans.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var propertyInfo = layoutPlans.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(layoutPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layoutPlans.IsDeleted.ShouldBeNull();
            layoutPlans.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (LayoutID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_LayoutID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var random = Fixture.Create<int>();

            // Act , Set
            layoutPlans.LayoutID = random;

            // Assert
            layoutPlans.LayoutID.ShouldBe(random);
            layoutPlans.LayoutID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_LayoutID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();

            // Act , Set
            layoutPlans.LayoutID = null;

            // Assert
            layoutPlans.LayoutID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_LayoutID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameLayoutID = "LayoutID";
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var propertyInfo = layoutPlans.GetType().GetProperty(propertyNameLayoutID);

            // Act , Set
            propertyInfo.SetValue(layoutPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layoutPlans.LayoutID.ShouldBeNull();
            layoutPlans.LayoutID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (LayoutID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_LayoutIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLayoutID = "LayoutIDNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameLayoutID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_LayoutID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLayoutID = "LayoutID";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameLayoutID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (LayoutPlanID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_LayoutPlanID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            layoutPlans.LayoutPlanID = Fixture.Create<int>();
            var intType = layoutPlans.LayoutPlanID.GetType();

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

        #region General Getters/Setters : Class (LayoutPlans) => Property (LayoutPlanID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_LayoutPlanIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLayoutPlanID = "LayoutPlanIDNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameLayoutPlanID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_LayoutPlanID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLayoutPlanID = "LayoutPlanID";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameLayoutPlanID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (Period) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Period_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var random = Fixture.Create<decimal>();

            // Act , Set
            layoutPlans.Period = random;

            // Assert
            layoutPlans.Period.ShouldBe(random);
            layoutPlans.Period.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Period_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();

            // Act , Set
            layoutPlans.Period = null;

            // Assert
            layoutPlans.Period.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Period_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNamePeriod = "Period";
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var propertyInfo = layoutPlans.GetType().GetProperty(propertyNamePeriod);

            // Act , Set
            propertyInfo.SetValue(layoutPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layoutPlans.Period.ShouldBeNull();
            layoutPlans.Period.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (Period) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_PeriodNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePeriod = "PeriodNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNamePeriod));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Period_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePeriod = "Period";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNamePeriod);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (SmartFormID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_SmartFormID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var random = Fixture.Create<int>();

            // Act , Set
            layoutPlans.SmartFormID = random;

            // Assert
            layoutPlans.SmartFormID.ShouldBe(random);
            layoutPlans.SmartFormID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_SmartFormID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();

            // Act , Set
            layoutPlans.SmartFormID = null;

            // Assert
            layoutPlans.SmartFormID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_SmartFormID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSmartFormID = "SmartFormID";
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var propertyInfo = layoutPlans.GetType().GetProperty(propertyNameSmartFormID);

            // Act , Set
            propertyInfo.SetValue(layoutPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layoutPlans.SmartFormID.ShouldBeNull();
            layoutPlans.SmartFormID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (SmartFormID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_SmartFormIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSmartFormID = "SmartFormIDNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameSmartFormID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_SmartFormID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSmartFormID = "SmartFormID";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameSmartFormID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (Status) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Status_Property_String_Type_Verify_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            layoutPlans.Status = Fixture.Create<string>();
            var stringType = layoutPlans.Status.GetType();

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

        #region General Getters/Setters : Class (LayoutPlans) => Property (Status) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_StatusNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStatus = "StatusNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameStatus));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Status_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStatus = "Status";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameStatus);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (TokenUID) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_TokenUID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameTokenUID = "TokenUID";
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layoutPlans.GetType().GetProperty(propertyNameTokenUID);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layoutPlans, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (TokenUID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_TokenUIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTokenUID = "TokenUIDNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameTokenUID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_TokenUID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTokenUID = "TokenUID";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameTokenUID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = layoutPlans.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(layoutPlans, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var random = Fixture.Create<int>();

            // Act , Set
            layoutPlans.UpdatedUserID = random;

            // Assert
            layoutPlans.UpdatedUserID.ShouldBe(random);
            layoutPlans.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var layoutPlans = Fixture.Create<LayoutPlans>();

            // Act , Set
            layoutPlans.UpdatedUserID = null;

            // Assert
            layoutPlans.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var layoutPlans = Fixture.Create<LayoutPlans>();
            var propertyInfo = layoutPlans.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(layoutPlans, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            layoutPlans.UpdatedUserID.ShouldBeNull();
            layoutPlans.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlans) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var layoutPlans  = Fixture.Create<LayoutPlans>();

            // Act , Assert
            Should.NotThrow(() => layoutPlans.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlans_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var layoutPlans  = Fixture.Create<LayoutPlans>();
            var propertyInfo  = layoutPlans.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (LayoutPlans) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LayoutPlans_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new LayoutPlans());
        }

        #endregion

        #region General Constructor : Class (LayoutPlans) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LayoutPlans_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfLayoutPlans = Fixture.CreateMany<LayoutPlans>(2).ToList();
            var firstLayoutPlans = instancesOfLayoutPlans.FirstOrDefault();
            var lastLayoutPlans = instancesOfLayoutPlans.Last();

            // Act, Assert
            firstLayoutPlans.ShouldNotBeNull();
            lastLayoutPlans.ShouldNotBeNull();
            firstLayoutPlans.ShouldNotBeSameAs(lastLayoutPlans);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LayoutPlans_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstLayoutPlans = new LayoutPlans();
            var secondLayoutPlans = new LayoutPlans();
            var thirdLayoutPlans = new LayoutPlans();
            var fourthLayoutPlans = new LayoutPlans();
            var fifthLayoutPlans = new LayoutPlans();
            var sixthLayoutPlans = new LayoutPlans();

            // Act, Assert
            firstLayoutPlans.ShouldNotBeNull();
            secondLayoutPlans.ShouldNotBeNull();
            thirdLayoutPlans.ShouldNotBeNull();
            fourthLayoutPlans.ShouldNotBeNull();
            fifthLayoutPlans.ShouldNotBeNull();
            sixthLayoutPlans.ShouldNotBeNull();
            firstLayoutPlans.ShouldNotBeSameAs(secondLayoutPlans);
            thirdLayoutPlans.ShouldNotBeSameAs(firstLayoutPlans);
            fourthLayoutPlans.ShouldNotBeSameAs(firstLayoutPlans);
            fifthLayoutPlans.ShouldNotBeSameAs(firstLayoutPlans);
            sixthLayoutPlans.ShouldNotBeSameAs(firstLayoutPlans);
            sixthLayoutPlans.ShouldNotBeSameAs(fourthLayoutPlans);
        }

        #endregion

        #region General Constructor : Class (LayoutPlans) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LayoutPlans_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var layoutPlanId = -1;
            var eventType = string.Empty;
            var criteria = string.Empty;
            var actionName = string.Empty;
            var status = string.Empty;

            // Act
            var layoutPlans = new LayoutPlans();

            // Assert
            layoutPlans.LayoutPlanID.ShouldBe(layoutPlanId);
            layoutPlans.LayoutID.ShouldBeNull();
            layoutPlans.EventType.ShouldBe(eventType);
            layoutPlans.CustomerID.ShouldBeNull();
            layoutPlans.BlastID.ShouldBeNull();
            layoutPlans.GroupID.ShouldBeNull();
            layoutPlans.Period.ShouldBeNull();
            layoutPlans.Criteria.ShouldBe(criteria);
            layoutPlans.ActionName.ShouldBe(actionName);
            layoutPlans.Status.ShouldBe(status);
            layoutPlans.SmartFormID.ShouldBeNull();
            layoutPlans.CreatedUserID.ShouldBeNull();
            layoutPlans.CreatedDate.ShouldBeNull();
            layoutPlans.UpdatedUserID.ShouldBeNull();
            layoutPlans.UpdatedDate.ShouldBeNull();
            layoutPlans.IsDeleted.ShouldBeNull();
            layoutPlans.TokenUID.ShouldBeNull();
            layoutPlans.CampaignItemID.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}
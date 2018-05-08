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
    public class BlastSingleTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastSingle) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();
            var blastSingleId = Fixture.Create<int>();
            var blastId = Fixture.Create<int?>();
            var emailId = Fixture.Create<int?>();
            var sendTime = Fixture.Create<DateTime?>();
            var processed = Fixture.Create<string>();
            var layoutPlanId = Fixture.Create<int?>();
            var refBlastId = Fixture.Create<int?>();
            var customerId = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();
            var startTime = Fixture.Create<DateTime?>();
            var endTime = Fixture.Create<DateTime?>();

            // Act
            blastSingle.BlastSingleID = blastSingleId;
            blastSingle.BlastID = blastId;
            blastSingle.EmailID = emailId;
            blastSingle.SendTime = sendTime;
            blastSingle.Processed = processed;
            blastSingle.LayoutPlanID = layoutPlanId;
            blastSingle.RefBlastID = refBlastId;
            blastSingle.CustomerID = customerId;
            blastSingle.CreatedUserID = createdUserId;
            blastSingle.CreatedDate = createdDate;
            blastSingle.UpdatedUserID = updatedUserId;
            blastSingle.UpdatedDate = updatedDate;
            blastSingle.IsDeleted = isDeleted;
            blastSingle.StartTime = startTime;
            blastSingle.EndTime = endTime;

            // Assert
            blastSingle.BlastSingleID.ShouldBe(blastSingleId);
            blastSingle.BlastID.ShouldBe(blastId);
            blastSingle.EmailID.ShouldBe(emailId);
            blastSingle.SendTime.ShouldBe(sendTime);
            blastSingle.Processed.ShouldBe(processed);
            blastSingle.LayoutPlanID.ShouldBe(layoutPlanId);
            blastSingle.RefBlastID.ShouldBe(refBlastId);
            blastSingle.CustomerID.ShouldBe(customerId);
            blastSingle.CreatedUserID.ShouldBe(createdUserId);
            blastSingle.CreatedDate.ShouldBe(createdDate);
            blastSingle.UpdatedUserID.ShouldBe(updatedUserId);
            blastSingle.UpdatedDate.ShouldBe(updatedDate);
            blastSingle.IsDeleted.ShouldBe(isDeleted);
            blastSingle.StartTime.ShouldBe(startTime);
            blastSingle.EndTime.ShouldBe(endTime);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (BlastID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_BlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastSingle.BlastID = random;

            // Assert
            blastSingle.BlastID.ShouldBe(random);
            blastSingle.BlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_BlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();    

            // Act , Set
            blastSingle.BlastID = null;

            // Assert
            blastSingle.BlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_BlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastID = "BlastID";
            var blastSingle = Fixture.Create<BlastSingle>();
            var propertyInfo = blastSingle.GetType().GetProperty(propertyNameBlastID);

            // Act , Set
            propertyInfo.SetValue(blastSingle, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastSingle.BlastID.ShouldBeNull();
            blastSingle.BlastID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastSingle  = Fixture.Create<BlastSingle>();

            // Act , Assert
            Should.NotThrow(() => blastSingle.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastSingle  = Fixture.Create<BlastSingle>();
            var propertyInfo  = blastSingle.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (BlastSingleID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_BlastSingleID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();
            blastSingle.BlastSingleID = Fixture.Create<int>();
            var intType = blastSingle.BlastSingleID.GetType();

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

        #region General Getters/Setters : Class (BlastSingle) => Property (BlastSingleID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Class_Invalid_Property_BlastSingleIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastSingleID = "BlastSingleIDNotPresent";
            var blastSingle  = Fixture.Create<BlastSingle>();

            // Act , Assert
            Should.NotThrow(() => blastSingle.GetType().GetProperty(propertyNameBlastSingleID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_BlastSingleID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastSingleID = "BlastSingleID";
            var blastSingle  = Fixture.Create<BlastSingle>();
            var propertyInfo  = blastSingle.GetType().GetProperty(propertyNameBlastSingleID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var blastSingle = Fixture.Create<BlastSingle>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastSingle.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastSingle, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var blastSingle  = Fixture.Create<BlastSingle>();

            // Act , Assert
            Should.NotThrow(() => blastSingle.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var blastSingle  = Fixture.Create<BlastSingle>();
            var propertyInfo  = blastSingle.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastSingle.CreatedUserID = random;

            // Assert
            blastSingle.CreatedUserID.ShouldBe(random);
            blastSingle.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();    

            // Act , Set
            blastSingle.CreatedUserID = null;

            // Assert
            blastSingle.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var blastSingle = Fixture.Create<BlastSingle>();
            var propertyInfo = blastSingle.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(blastSingle, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastSingle.CreatedUserID.ShouldBeNull();
            blastSingle.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var blastSingle  = Fixture.Create<BlastSingle>();

            // Act , Assert
            Should.NotThrow(() => blastSingle.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var blastSingle  = Fixture.Create<BlastSingle>();
            var propertyInfo  = blastSingle.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastSingle.CustomerID = random;

            // Assert
            blastSingle.CustomerID.ShouldBe(random);
            blastSingle.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();    

            // Act , Set
            blastSingle.CustomerID = null;

            // Assert
            blastSingle.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var blastSingle = Fixture.Create<BlastSingle>();
            var propertyInfo = blastSingle.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(blastSingle, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastSingle.CustomerID.ShouldBeNull();
            blastSingle.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var blastSingle  = Fixture.Create<BlastSingle>();

            // Act , Assert
            Should.NotThrow(() => blastSingle.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var blastSingle  = Fixture.Create<BlastSingle>();
            var propertyInfo  = blastSingle.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (EmailID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_EmailID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastSingle.EmailID = random;

            // Assert
            blastSingle.EmailID.ShouldBe(random);
            blastSingle.EmailID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_EmailID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();    

            // Act , Set
            blastSingle.EmailID = null;

            // Assert
            blastSingle.EmailID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_EmailID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameEmailID = "EmailID";
            var blastSingle = Fixture.Create<BlastSingle>();
            var propertyInfo = blastSingle.GetType().GetProperty(propertyNameEmailID);

            // Act , Set
            propertyInfo.SetValue(blastSingle, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastSingle.EmailID.ShouldBeNull();
            blastSingle.EmailID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var blastSingle  = Fixture.Create<BlastSingle>();

            // Act , Assert
            Should.NotThrow(() => blastSingle.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var blastSingle  = Fixture.Create<BlastSingle>();
            var propertyInfo  = blastSingle.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (EndTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_EndTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameEndTime = "EndTime";
            var blastSingle = Fixture.Create<BlastSingle>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastSingle.GetType().GetProperty(propertyNameEndTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastSingle, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (EndTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Class_Invalid_Property_EndTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEndTime = "EndTimeNotPresent";
            var blastSingle  = Fixture.Create<BlastSingle>();

            // Act , Assert
            Should.NotThrow(() => blastSingle.GetType().GetProperty(propertyNameEndTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_EndTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEndTime = "EndTime";
            var blastSingle  = Fixture.Create<BlastSingle>();
            var propertyInfo  = blastSingle.GetType().GetProperty(propertyNameEndTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();
            var random = Fixture.Create<bool>();

            // Act , Set
            blastSingle.IsDeleted = random;

            // Assert
            blastSingle.IsDeleted.ShouldBe(random);
            blastSingle.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();    

            // Act , Set
            blastSingle.IsDeleted = null;

            // Assert
            blastSingle.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var blastSingle = Fixture.Create<BlastSingle>();
            var propertyInfo = blastSingle.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(blastSingle, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastSingle.IsDeleted.ShouldBeNull();
            blastSingle.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var blastSingle  = Fixture.Create<BlastSingle>();

            // Act , Assert
            Should.NotThrow(() => blastSingle.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var blastSingle  = Fixture.Create<BlastSingle>();
            var propertyInfo  = blastSingle.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (LayoutPlanID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_LayoutPlanID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastSingle.LayoutPlanID = random;

            // Assert
            blastSingle.LayoutPlanID.ShouldBe(random);
            blastSingle.LayoutPlanID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_LayoutPlanID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();    

            // Act , Set
            blastSingle.LayoutPlanID = null;

            // Assert
            blastSingle.LayoutPlanID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_LayoutPlanID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameLayoutPlanID = "LayoutPlanID";
            var blastSingle = Fixture.Create<BlastSingle>();
            var propertyInfo = blastSingle.GetType().GetProperty(propertyNameLayoutPlanID);

            // Act , Set
            propertyInfo.SetValue(blastSingle, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastSingle.LayoutPlanID.ShouldBeNull();
            blastSingle.LayoutPlanID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (LayoutPlanID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Class_Invalid_Property_LayoutPlanIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLayoutPlanID = "LayoutPlanIDNotPresent";
            var blastSingle  = Fixture.Create<BlastSingle>();

            // Act , Assert
            Should.NotThrow(() => blastSingle.GetType().GetProperty(propertyNameLayoutPlanID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_LayoutPlanID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLayoutPlanID = "LayoutPlanID";
            var blastSingle  = Fixture.Create<BlastSingle>();
            var propertyInfo  = blastSingle.GetType().GetProperty(propertyNameLayoutPlanID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (Processed) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Processed_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();
            blastSingle.Processed = Fixture.Create<string>();
            var stringType = blastSingle.Processed.GetType();

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

        #region General Getters/Setters : Class (BlastSingle) => Property (Processed) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Class_Invalid_Property_ProcessedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProcessed = "ProcessedNotPresent";
            var blastSingle  = Fixture.Create<BlastSingle>();

            // Act , Assert
            Should.NotThrow(() => blastSingle.GetType().GetProperty(propertyNameProcessed));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Processed_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProcessed = "Processed";
            var blastSingle  = Fixture.Create<BlastSingle>();
            var propertyInfo  = blastSingle.GetType().GetProperty(propertyNameProcessed);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (RefBlastID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_RefBlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastSingle.RefBlastID = random;

            // Assert
            blastSingle.RefBlastID.ShouldBe(random);
            blastSingle.RefBlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_RefBlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();    

            // Act , Set
            blastSingle.RefBlastID = null;

            // Assert
            blastSingle.RefBlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_RefBlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameRefBlastID = "RefBlastID";
            var blastSingle = Fixture.Create<BlastSingle>();
            var propertyInfo = blastSingle.GetType().GetProperty(propertyNameRefBlastID);

            // Act , Set
            propertyInfo.SetValue(blastSingle, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastSingle.RefBlastID.ShouldBeNull();
            blastSingle.RefBlastID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (RefBlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Class_Invalid_Property_RefBlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRefBlastID = "RefBlastIDNotPresent";
            var blastSingle  = Fixture.Create<BlastSingle>();

            // Act , Assert
            Should.NotThrow(() => blastSingle.GetType().GetProperty(propertyNameRefBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_RefBlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRefBlastID = "RefBlastID";
            var blastSingle  = Fixture.Create<BlastSingle>();
            var propertyInfo  = blastSingle.GetType().GetProperty(propertyNameRefBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (SendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_SendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var blastSingle = Fixture.Create<BlastSingle>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastSingle.GetType().GetProperty(propertyNameSendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastSingle, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (SendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Class_Invalid_Property_SendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTimeNotPresent";
            var blastSingle  = Fixture.Create<BlastSingle>();

            // Act , Assert
            Should.NotThrow(() => blastSingle.GetType().GetProperty(propertyNameSendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_SendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var blastSingle  = Fixture.Create<BlastSingle>();
            var propertyInfo  = blastSingle.GetType().GetProperty(propertyNameSendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (StartTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_StartTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameStartTime = "StartTime";
            var blastSingle = Fixture.Create<BlastSingle>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastSingle.GetType().GetProperty(propertyNameStartTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastSingle, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (StartTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Class_Invalid_Property_StartTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStartTime = "StartTimeNotPresent";
            var blastSingle  = Fixture.Create<BlastSingle>();

            // Act , Assert
            Should.NotThrow(() => blastSingle.GetType().GetProperty(propertyNameStartTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_StartTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStartTime = "StartTime";
            var blastSingle  = Fixture.Create<BlastSingle>();
            var propertyInfo  = blastSingle.GetType().GetProperty(propertyNameStartTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var blastSingle = Fixture.Create<BlastSingle>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastSingle.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastSingle, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var blastSingle  = Fixture.Create<BlastSingle>();

            // Act , Assert
            Should.NotThrow(() => blastSingle.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var blastSingle  = Fixture.Create<BlastSingle>();
            var propertyInfo  = blastSingle.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastSingle.UpdatedUserID = random;

            // Assert
            blastSingle.UpdatedUserID.ShouldBe(random);
            blastSingle.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastSingle = Fixture.Create<BlastSingle>();    

            // Act , Set
            blastSingle.UpdatedUserID = null;

            // Assert
            blastSingle.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var blastSingle = Fixture.Create<BlastSingle>();
            var propertyInfo = blastSingle.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(blastSingle, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastSingle.UpdatedUserID.ShouldBeNull();
            blastSingle.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSingle) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var blastSingle  = Fixture.Create<BlastSingle>();

            // Act , Assert
            Should.NotThrow(() => blastSingle.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSingle_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var blastSingle  = Fixture.Create<BlastSingle>();
            var propertyInfo  = blastSingle.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastSingle) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSingle_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastSingle());
        }

        #endregion

        #region General Constructor : Class (BlastSingle) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSingle_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastSingle = Fixture.CreateMany<BlastSingle>(2).ToList();
            var firstBlastSingle = instancesOfBlastSingle.FirstOrDefault();
            var lastBlastSingle = instancesOfBlastSingle.Last();

            // Act, Assert
            firstBlastSingle.ShouldNotBeNull();
            lastBlastSingle.ShouldNotBeNull();
            firstBlastSingle.ShouldNotBeSameAs(lastBlastSingle);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSingle_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastSingle = new BlastSingle();
            var secondBlastSingle = new BlastSingle();
            var thirdBlastSingle = new BlastSingle();
            var fourthBlastSingle = new BlastSingle();
            var fifthBlastSingle = new BlastSingle();
            var sixthBlastSingle = new BlastSingle();

            // Act, Assert
            firstBlastSingle.ShouldNotBeNull();
            secondBlastSingle.ShouldNotBeNull();
            thirdBlastSingle.ShouldNotBeNull();
            fourthBlastSingle.ShouldNotBeNull();
            fifthBlastSingle.ShouldNotBeNull();
            sixthBlastSingle.ShouldNotBeNull();
            firstBlastSingle.ShouldNotBeSameAs(secondBlastSingle);
            thirdBlastSingle.ShouldNotBeSameAs(firstBlastSingle);
            fourthBlastSingle.ShouldNotBeSameAs(firstBlastSingle);
            fifthBlastSingle.ShouldNotBeSameAs(firstBlastSingle);
            sixthBlastSingle.ShouldNotBeSameAs(firstBlastSingle);
            sixthBlastSingle.ShouldNotBeSameAs(fourthBlastSingle);
        }

        #endregion

        #region General Constructor : Class (BlastSingle) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSingle_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var blastSingleId = -1;
            var processed = string.Empty;

            // Act
            var blastSingle = new BlastSingle();

            // Assert
            blastSingle.BlastSingleID.ShouldBe(blastSingleId);
            blastSingle.BlastID.ShouldBeNull();
            blastSingle.EmailID.ShouldBeNull();
            blastSingle.SendTime.ShouldBeNull();
            blastSingle.Processed.ShouldBe(processed);
            blastSingle.LayoutPlanID.ShouldBeNull();
            blastSingle.RefBlastID.ShouldBeNull();
            blastSingle.CustomerID.ShouldBeNull();
            blastSingle.CreatedUserID.ShouldBeNull();
            blastSingle.CreatedDate.ShouldBeNull();
            blastSingle.UpdatedUserID.ShouldBeNull();
            blastSingle.UpdatedDate.ShouldBeNull();
            blastSingle.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}
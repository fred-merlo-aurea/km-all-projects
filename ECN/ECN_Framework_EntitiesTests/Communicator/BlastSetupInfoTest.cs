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
    public class BlastSetupInfoTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastSetupInfo) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();
            var blastScheduleId = Fixture.Create<int?>();
            var scheduleType = Fixture.Create<string>();
            var isTestBlast = Fixture.Create<bool?>();
            var sendNowIsAmount = Fixture.Create<bool?>();
            var sendNowAmount = Fixture.Create<int?>();
            var sendTime = Fixture.Create<DateTime?>();
            var blastFrequency = Fixture.Create<string>();
            var blastType = Fixture.Create<string>();
            var sendTextTestBlast = Fixture.Create<bool?>();

            // Act
            blastSetupInfo.BlastScheduleID = blastScheduleId;
            blastSetupInfo.ScheduleType = scheduleType;
            blastSetupInfo.IsTestBlast = isTestBlast;
            blastSetupInfo.SendNowIsAmount = sendNowIsAmount;
            blastSetupInfo.SendNowAmount = sendNowAmount;
            blastSetupInfo.SendTime = sendTime;
            blastSetupInfo.BlastFrequency = blastFrequency;
            blastSetupInfo.BlastType = blastType;
            blastSetupInfo.SendTextTestBlast = sendTextTestBlast;

            // Assert
            blastSetupInfo.BlastScheduleID.ShouldBe(blastScheduleId);
            blastSetupInfo.ScheduleType.ShouldBe(scheduleType);
            blastSetupInfo.IsTestBlast.ShouldBe(isTestBlast);
            blastSetupInfo.SendNowIsAmount.ShouldBe(sendNowIsAmount);
            blastSetupInfo.SendNowAmount.ShouldBe(sendNowAmount);
            blastSetupInfo.SendTime.ShouldBe(sendTime);
            blastSetupInfo.BlastFrequency.ShouldBe(blastFrequency);
            blastSetupInfo.BlastType.ShouldBe(blastType);
            blastSetupInfo.SendTextTestBlast.ShouldBe(sendTextTestBlast);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (BlastFrequency) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_BlastFrequency_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();
            blastSetupInfo.BlastFrequency = Fixture.Create<string>();
            var stringType = blastSetupInfo.BlastFrequency.GetType();

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

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (BlastFrequency) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_Class_Invalid_Property_BlastFrequencyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastFrequency = "BlastFrequencyNotPresent";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();

            // Act , Assert
            Should.NotThrow(() => blastSetupInfo.GetType().GetProperty(propertyNameBlastFrequency));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_BlastFrequency_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastFrequency = "BlastFrequency";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();
            var propertyInfo  = blastSetupInfo.GetType().GetProperty(propertyNameBlastFrequency);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (BlastScheduleID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_BlastScheduleID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastSetupInfo.BlastScheduleID = random;

            // Assert
            blastSetupInfo.BlastScheduleID.ShouldBe(random);
            blastSetupInfo.BlastScheduleID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_BlastScheduleID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();    

            // Act , Set
            blastSetupInfo.BlastScheduleID = null;

            // Assert
            blastSetupInfo.BlastScheduleID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_BlastScheduleID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastScheduleID = "BlastScheduleID";
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();
            var propertyInfo = blastSetupInfo.GetType().GetProperty(propertyNameBlastScheduleID);

            // Act , Set
            propertyInfo.SetValue(blastSetupInfo, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastSetupInfo.BlastScheduleID.ShouldBeNull();
            blastSetupInfo.BlastScheduleID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (BlastScheduleID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_Class_Invalid_Property_BlastScheduleIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastScheduleID = "BlastScheduleIDNotPresent";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();

            // Act , Assert
            Should.NotThrow(() => blastSetupInfo.GetType().GetProperty(propertyNameBlastScheduleID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_BlastScheduleID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastScheduleID = "BlastScheduleID";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();
            var propertyInfo  = blastSetupInfo.GetType().GetProperty(propertyNameBlastScheduleID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (BlastType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_BlastType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();
            blastSetupInfo.BlastType = Fixture.Create<string>();
            var stringType = blastSetupInfo.BlastType.GetType();

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

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (BlastType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_Class_Invalid_Property_BlastTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastType = "BlastTypeNotPresent";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();

            // Act , Assert
            Should.NotThrow(() => blastSetupInfo.GetType().GetProperty(propertyNameBlastType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_BlastType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastType = "BlastType";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();
            var propertyInfo  = blastSetupInfo.GetType().GetProperty(propertyNameBlastType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (IsTestBlast) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_IsTestBlast_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();
            var random = Fixture.Create<bool>();

            // Act , Set
            blastSetupInfo.IsTestBlast = random;

            // Assert
            blastSetupInfo.IsTestBlast.ShouldBe(random);
            blastSetupInfo.IsTestBlast.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_IsTestBlast_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();    

            // Act , Set
            blastSetupInfo.IsTestBlast = null;

            // Assert
            blastSetupInfo.IsTestBlast.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_IsTestBlast_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsTestBlast = "IsTestBlast";
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();
            var propertyInfo = blastSetupInfo.GetType().GetProperty(propertyNameIsTestBlast);

            // Act , Set
            propertyInfo.SetValue(blastSetupInfo, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastSetupInfo.IsTestBlast.ShouldBeNull();
            blastSetupInfo.IsTestBlast.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (IsTestBlast) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_Class_Invalid_Property_IsTestBlastNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsTestBlast = "IsTestBlastNotPresent";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();

            // Act , Assert
            Should.NotThrow(() => blastSetupInfo.GetType().GetProperty(propertyNameIsTestBlast));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_IsTestBlast_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsTestBlast = "IsTestBlast";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();
            var propertyInfo  = blastSetupInfo.GetType().GetProperty(propertyNameIsTestBlast);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (ScheduleType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_ScheduleType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();
            blastSetupInfo.ScheduleType = Fixture.Create<string>();
            var stringType = blastSetupInfo.ScheduleType.GetType();

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

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (ScheduleType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_Class_Invalid_Property_ScheduleTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameScheduleType = "ScheduleTypeNotPresent";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();

            // Act , Assert
            Should.NotThrow(() => blastSetupInfo.GetType().GetProperty(propertyNameScheduleType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_ScheduleType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameScheduleType = "ScheduleType";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();
            var propertyInfo  = blastSetupInfo.GetType().GetProperty(propertyNameScheduleType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (SendNowAmount) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_SendNowAmount_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastSetupInfo.SendNowAmount = random;

            // Assert
            blastSetupInfo.SendNowAmount.ShouldBe(random);
            blastSetupInfo.SendNowAmount.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_SendNowAmount_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();    

            // Act , Set
            blastSetupInfo.SendNowAmount = null;

            // Assert
            blastSetupInfo.SendNowAmount.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_SendNowAmount_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSendNowAmount = "SendNowAmount";
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();
            var propertyInfo = blastSetupInfo.GetType().GetProperty(propertyNameSendNowAmount);

            // Act , Set
            propertyInfo.SetValue(blastSetupInfo, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastSetupInfo.SendNowAmount.ShouldBeNull();
            blastSetupInfo.SendNowAmount.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (SendNowAmount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_Class_Invalid_Property_SendNowAmountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendNowAmount = "SendNowAmountNotPresent";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();

            // Act , Assert
            Should.NotThrow(() => blastSetupInfo.GetType().GetProperty(propertyNameSendNowAmount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_SendNowAmount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendNowAmount = "SendNowAmount";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();
            var propertyInfo  = blastSetupInfo.GetType().GetProperty(propertyNameSendNowAmount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (SendNowIsAmount) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_SendNowIsAmount_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();
            var random = Fixture.Create<bool>();

            // Act , Set
            blastSetupInfo.SendNowIsAmount = random;

            // Assert
            blastSetupInfo.SendNowIsAmount.ShouldBe(random);
            blastSetupInfo.SendNowIsAmount.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_SendNowIsAmount_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();    

            // Act , Set
            blastSetupInfo.SendNowIsAmount = null;

            // Assert
            blastSetupInfo.SendNowIsAmount.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_SendNowIsAmount_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSendNowIsAmount = "SendNowIsAmount";
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();
            var propertyInfo = blastSetupInfo.GetType().GetProperty(propertyNameSendNowIsAmount);

            // Act , Set
            propertyInfo.SetValue(blastSetupInfo, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastSetupInfo.SendNowIsAmount.ShouldBeNull();
            blastSetupInfo.SendNowIsAmount.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (SendNowIsAmount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_Class_Invalid_Property_SendNowIsAmountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendNowIsAmount = "SendNowIsAmountNotPresent";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();

            // Act , Assert
            Should.NotThrow(() => blastSetupInfo.GetType().GetProperty(propertyNameSendNowIsAmount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_SendNowIsAmount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendNowIsAmount = "SendNowIsAmount";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();
            var propertyInfo  = blastSetupInfo.GetType().GetProperty(propertyNameSendNowIsAmount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (SendTextTestBlast) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_SendTextTestBlast_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();
            var random = Fixture.Create<bool>();

            // Act , Set
            blastSetupInfo.SendTextTestBlast = random;

            // Assert
            blastSetupInfo.SendTextTestBlast.ShouldBe(random);
            blastSetupInfo.SendTextTestBlast.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_SendTextTestBlast_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();    

            // Act , Set
            blastSetupInfo.SendTextTestBlast = null;

            // Assert
            blastSetupInfo.SendTextTestBlast.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_SendTextTestBlast_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSendTextTestBlast = "SendTextTestBlast";
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();
            var propertyInfo = blastSetupInfo.GetType().GetProperty(propertyNameSendTextTestBlast);

            // Act , Set
            propertyInfo.SetValue(blastSetupInfo, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastSetupInfo.SendTextTestBlast.ShouldBeNull();
            blastSetupInfo.SendTextTestBlast.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (SendTextTestBlast) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_Class_Invalid_Property_SendTextTestBlastNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTextTestBlast = "SendTextTestBlastNotPresent";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();

            // Act , Assert
            Should.NotThrow(() => blastSetupInfo.GetType().GetProperty(propertyNameSendTextTestBlast));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_SendTextTestBlast_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTextTestBlast = "SendTextTestBlast";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();
            var propertyInfo  = blastSetupInfo.GetType().GetProperty(propertyNameSendTextTestBlast);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (SendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_SendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var blastSetupInfo = Fixture.Create<BlastSetupInfo>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastSetupInfo.GetType().GetProperty(propertyNameSendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastSetupInfo, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastSetupInfo) => Property (SendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_Class_Invalid_Property_SendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTimeNotPresent";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();

            // Act , Assert
            Should.NotThrow(() => blastSetupInfo.GetType().GetProperty(propertyNameSendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSetupInfo_SendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var blastSetupInfo  = Fixture.Create<BlastSetupInfo>();
            var propertyInfo  = blastSetupInfo.GetType().GetProperty(propertyNameSendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastSetupInfo) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSetupInfo_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastSetupInfo());
        }

        #endregion

        #region General Constructor : Class (BlastSetupInfo) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSetupInfo_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastSetupInfo = Fixture.CreateMany<BlastSetupInfo>(2).ToList();
            var firstBlastSetupInfo = instancesOfBlastSetupInfo.FirstOrDefault();
            var lastBlastSetupInfo = instancesOfBlastSetupInfo.Last();

            // Act, Assert
            firstBlastSetupInfo.ShouldNotBeNull();
            lastBlastSetupInfo.ShouldNotBeNull();
            firstBlastSetupInfo.ShouldNotBeSameAs(lastBlastSetupInfo);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSetupInfo_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastSetupInfo = new BlastSetupInfo();
            var secondBlastSetupInfo = new BlastSetupInfo();
            var thirdBlastSetupInfo = new BlastSetupInfo();
            var fourthBlastSetupInfo = new BlastSetupInfo();
            var fifthBlastSetupInfo = new BlastSetupInfo();
            var sixthBlastSetupInfo = new BlastSetupInfo();

            // Act, Assert
            firstBlastSetupInfo.ShouldNotBeNull();
            secondBlastSetupInfo.ShouldNotBeNull();
            thirdBlastSetupInfo.ShouldNotBeNull();
            fourthBlastSetupInfo.ShouldNotBeNull();
            fifthBlastSetupInfo.ShouldNotBeNull();
            sixthBlastSetupInfo.ShouldNotBeNull();
            firstBlastSetupInfo.ShouldNotBeSameAs(secondBlastSetupInfo);
            thirdBlastSetupInfo.ShouldNotBeSameAs(firstBlastSetupInfo);
            fourthBlastSetupInfo.ShouldNotBeSameAs(firstBlastSetupInfo);
            fifthBlastSetupInfo.ShouldNotBeSameAs(firstBlastSetupInfo);
            sixthBlastSetupInfo.ShouldNotBeSameAs(firstBlastSetupInfo);
            sixthBlastSetupInfo.ShouldNotBeSameAs(fourthBlastSetupInfo);
        }

        #endregion

        #region General Constructor : Class (BlastSetupInfo) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSetupInfo_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var scheduleType = string.Empty;
            var blastFrequency = string.Empty;
            var blastType = string.Empty;
            var sendTextTestBlast = false;

            // Act
            var blastSetupInfo = new BlastSetupInfo();

            // Assert
            blastSetupInfo.BlastScheduleID.ShouldBeNull();
            blastSetupInfo.ScheduleType.ShouldBe(scheduleType);
            blastSetupInfo.IsTestBlast.ShouldBeNull();
            blastSetupInfo.SendNowIsAmount.ShouldBeNull();
            blastSetupInfo.SendNowAmount.ShouldBeNull();
            blastSetupInfo.SendTime.ShouldBeNull();
            blastSetupInfo.BlastFrequency.ShouldBe(blastFrequency);
            blastSetupInfo.BlastType.ShouldBe(blastType);
            blastSetupInfo.SendTextTestBlast.ShouldBe(sendTextTestBlast);
        }

        #endregion

        #endregion

        #endregion
    }
}
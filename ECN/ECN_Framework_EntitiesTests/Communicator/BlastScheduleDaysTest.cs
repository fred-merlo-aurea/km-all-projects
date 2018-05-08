using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class BlastScheduleDaysTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastScheduleDays) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();
            var blastScheduleDaysId = Fixture.Create<int?>();
            var blastScheduleId = Fixture.Create<int?>();
            var dayToSend = Fixture.Create<int?>();
            var isAmount = Fixture.Create<bool?>();
            var total = Fixture.Create<int?>();
            var weeks = Fixture.Create<int?>();
            var errorList = Fixture.Create<List<ECNError>>();

            // Act
            blastScheduleDays.BlastScheduleDaysID = blastScheduleDaysId;
            blastScheduleDays.BlastScheduleID = blastScheduleId;
            blastScheduleDays.DayToSend = dayToSend;
            blastScheduleDays.IsAmount = isAmount;
            blastScheduleDays.Total = total;
            blastScheduleDays.Weeks = weeks;
            blastScheduleDays.ErrorList = errorList;

            // Assert
            blastScheduleDays.BlastScheduleDaysID.ShouldBe(blastScheduleDaysId);
            blastScheduleDays.BlastScheduleID.ShouldBe(blastScheduleId);
            blastScheduleDays.DayToSend.ShouldBe(dayToSend);
            blastScheduleDays.IsAmount.ShouldBe(isAmount);
            blastScheduleDays.Total.ShouldBe(total);
            blastScheduleDays.Weeks.ShouldBe(weeks);
            blastScheduleDays.ErrorList.ShouldBe(errorList);
        }

        #endregion

        #region General Getters/Setters : Class (BlastScheduleDays) => Property (BlastScheduleDaysID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_BlastScheduleDaysID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastScheduleDays.BlastScheduleDaysID = random;

            // Assert
            blastScheduleDays.BlastScheduleDaysID.ShouldBe(random);
            blastScheduleDays.BlastScheduleDaysID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_BlastScheduleDaysID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();    

            // Act , Set
            blastScheduleDays.BlastScheduleDaysID = null;

            // Assert
            blastScheduleDays.BlastScheduleDaysID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_BlastScheduleDaysID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastScheduleDaysID = "BlastScheduleDaysID";
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();
            var propertyInfo = blastScheduleDays.GetType().GetProperty(propertyNameBlastScheduleDaysID);

            // Act , Set
            propertyInfo.SetValue(blastScheduleDays, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastScheduleDays.BlastScheduleDaysID.ShouldBeNull();
            blastScheduleDays.BlastScheduleDaysID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastScheduleDays) => Property (BlastScheduleDaysID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_Class_Invalid_Property_BlastScheduleDaysIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastScheduleDaysID = "BlastScheduleDaysIDNotPresent";
            var blastScheduleDays  = Fixture.Create<BlastScheduleDays>();

            // Act , Assert
            Should.NotThrow(() => blastScheduleDays.GetType().GetProperty(propertyNameBlastScheduleDaysID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_BlastScheduleDaysID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastScheduleDaysID = "BlastScheduleDaysID";
            var blastScheduleDays  = Fixture.Create<BlastScheduleDays>();
            var propertyInfo  = blastScheduleDays.GetType().GetProperty(propertyNameBlastScheduleDaysID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastScheduleDays) => Property (BlastScheduleID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_BlastScheduleID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastScheduleDays.BlastScheduleID = random;

            // Assert
            blastScheduleDays.BlastScheduleID.ShouldBe(random);
            blastScheduleDays.BlastScheduleID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_BlastScheduleID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();    

            // Act , Set
            blastScheduleDays.BlastScheduleID = null;

            // Assert
            blastScheduleDays.BlastScheduleID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_BlastScheduleID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastScheduleID = "BlastScheduleID";
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();
            var propertyInfo = blastScheduleDays.GetType().GetProperty(propertyNameBlastScheduleID);

            // Act , Set
            propertyInfo.SetValue(blastScheduleDays, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastScheduleDays.BlastScheduleID.ShouldBeNull();
            blastScheduleDays.BlastScheduleID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastScheduleDays) => Property (BlastScheduleID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_Class_Invalid_Property_BlastScheduleIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastScheduleID = "BlastScheduleIDNotPresent";
            var blastScheduleDays  = Fixture.Create<BlastScheduleDays>();

            // Act , Assert
            Should.NotThrow(() => blastScheduleDays.GetType().GetProperty(propertyNameBlastScheduleID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_BlastScheduleID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastScheduleID = "BlastScheduleID";
            var blastScheduleDays  = Fixture.Create<BlastScheduleDays>();
            var propertyInfo  = blastScheduleDays.GetType().GetProperty(propertyNameBlastScheduleID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastScheduleDays) => Property (DayToSend) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_DayToSend_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastScheduleDays.DayToSend = random;

            // Assert
            blastScheduleDays.DayToSend.ShouldBe(random);
            blastScheduleDays.DayToSend.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_DayToSend_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();    

            // Act , Set
            blastScheduleDays.DayToSend = null;

            // Assert
            blastScheduleDays.DayToSend.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_DayToSend_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameDayToSend = "DayToSend";
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();
            var propertyInfo = blastScheduleDays.GetType().GetProperty(propertyNameDayToSend);

            // Act , Set
            propertyInfo.SetValue(blastScheduleDays, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastScheduleDays.DayToSend.ShouldBeNull();
            blastScheduleDays.DayToSend.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastScheduleDays) => Property (DayToSend) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_Class_Invalid_Property_DayToSendNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDayToSend = "DayToSendNotPresent";
            var blastScheduleDays  = Fixture.Create<BlastScheduleDays>();

            // Act , Assert
            Should.NotThrow(() => blastScheduleDays.GetType().GetProperty(propertyNameDayToSend));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_DayToSend_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDayToSend = "DayToSend";
            var blastScheduleDays  = Fixture.Create<BlastScheduleDays>();
            var propertyInfo  = blastScheduleDays.GetType().GetProperty(propertyNameDayToSend);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastScheduleDays) => Property (ErrorList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_Class_Invalid_Property_ErrorListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameErrorList = "ErrorListNotPresent";
            var blastScheduleDays  = Fixture.Create<BlastScheduleDays>();

            // Act , Assert
            Should.NotThrow(() => blastScheduleDays.GetType().GetProperty(propertyNameErrorList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_ErrorList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameErrorList = "ErrorList";
            var blastScheduleDays  = Fixture.Create<BlastScheduleDays>();
            var propertyInfo  = blastScheduleDays.GetType().GetProperty(propertyNameErrorList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastScheduleDays) => Property (IsAmount) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_IsAmount_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();
            var random = Fixture.Create<bool>();

            // Act , Set
            blastScheduleDays.IsAmount = random;

            // Assert
            blastScheduleDays.IsAmount.ShouldBe(random);
            blastScheduleDays.IsAmount.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_IsAmount_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();    

            // Act , Set
            blastScheduleDays.IsAmount = null;

            // Assert
            blastScheduleDays.IsAmount.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_IsAmount_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsAmount = "IsAmount";
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();
            var propertyInfo = blastScheduleDays.GetType().GetProperty(propertyNameIsAmount);

            // Act , Set
            propertyInfo.SetValue(blastScheduleDays, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastScheduleDays.IsAmount.ShouldBeNull();
            blastScheduleDays.IsAmount.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastScheduleDays) => Property (IsAmount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_Class_Invalid_Property_IsAmountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsAmount = "IsAmountNotPresent";
            var blastScheduleDays  = Fixture.Create<BlastScheduleDays>();

            // Act , Assert
            Should.NotThrow(() => blastScheduleDays.GetType().GetProperty(propertyNameIsAmount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_IsAmount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsAmount = "IsAmount";
            var blastScheduleDays  = Fixture.Create<BlastScheduleDays>();
            var propertyInfo  = blastScheduleDays.GetType().GetProperty(propertyNameIsAmount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastScheduleDays) => Property (Total) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_Total_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastScheduleDays.Total = random;

            // Assert
            blastScheduleDays.Total.ShouldBe(random);
            blastScheduleDays.Total.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_Total_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();    

            // Act , Set
            blastScheduleDays.Total = null;

            // Assert
            blastScheduleDays.Total.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_Total_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameTotal = "Total";
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();
            var propertyInfo = blastScheduleDays.GetType().GetProperty(propertyNameTotal);

            // Act , Set
            propertyInfo.SetValue(blastScheduleDays, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastScheduleDays.Total.ShouldBeNull();
            blastScheduleDays.Total.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastScheduleDays) => Property (Total) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_Class_Invalid_Property_TotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotal = "TotalNotPresent";
            var blastScheduleDays  = Fixture.Create<BlastScheduleDays>();

            // Act , Assert
            Should.NotThrow(() => blastScheduleDays.GetType().GetProperty(propertyNameTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_Total_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotal = "Total";
            var blastScheduleDays  = Fixture.Create<BlastScheduleDays>();
            var propertyInfo  = blastScheduleDays.GetType().GetProperty(propertyNameTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastScheduleDays) => Property (Weeks) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_Weeks_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastScheduleDays.Weeks = random;

            // Assert
            blastScheduleDays.Weeks.ShouldBe(random);
            blastScheduleDays.Weeks.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_Weeks_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();    

            // Act , Set
            blastScheduleDays.Weeks = null;

            // Assert
            blastScheduleDays.Weeks.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_Weeks_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameWeeks = "Weeks";
            var blastScheduleDays = Fixture.Create<BlastScheduleDays>();
            var propertyInfo = blastScheduleDays.GetType().GetProperty(propertyNameWeeks);

            // Act , Set
            propertyInfo.SetValue(blastScheduleDays, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastScheduleDays.Weeks.ShouldBeNull();
            blastScheduleDays.Weeks.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastScheduleDays) => Property (Weeks) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_Class_Invalid_Property_WeeksNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameWeeks = "WeeksNotPresent";
            var blastScheduleDays  = Fixture.Create<BlastScheduleDays>();

            // Act , Assert
            Should.NotThrow(() => blastScheduleDays.GetType().GetProperty(propertyNameWeeks));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastScheduleDays_Weeks_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameWeeks = "Weeks";
            var blastScheduleDays  = Fixture.Create<BlastScheduleDays>();
            var propertyInfo  = blastScheduleDays.GetType().GetProperty(propertyNameWeeks);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastScheduleDays) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastScheduleDays_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastScheduleDays());
        }

        #endregion

        #region General Constructor : Class (BlastScheduleDays) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastScheduleDays_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastScheduleDays = Fixture.CreateMany<BlastScheduleDays>(2).ToList();
            var firstBlastScheduleDays = instancesOfBlastScheduleDays.FirstOrDefault();
            var lastBlastScheduleDays = instancesOfBlastScheduleDays.Last();

            // Act, Assert
            firstBlastScheduleDays.ShouldNotBeNull();
            lastBlastScheduleDays.ShouldNotBeNull();
            firstBlastScheduleDays.ShouldNotBeSameAs(lastBlastScheduleDays);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastScheduleDays_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastScheduleDays = new BlastScheduleDays();
            var secondBlastScheduleDays = new BlastScheduleDays();
            var thirdBlastScheduleDays = new BlastScheduleDays();
            var fourthBlastScheduleDays = new BlastScheduleDays();
            var fifthBlastScheduleDays = new BlastScheduleDays();
            var sixthBlastScheduleDays = new BlastScheduleDays();

            // Act, Assert
            firstBlastScheduleDays.ShouldNotBeNull();
            secondBlastScheduleDays.ShouldNotBeNull();
            thirdBlastScheduleDays.ShouldNotBeNull();
            fourthBlastScheduleDays.ShouldNotBeNull();
            fifthBlastScheduleDays.ShouldNotBeNull();
            sixthBlastScheduleDays.ShouldNotBeNull();
            firstBlastScheduleDays.ShouldNotBeSameAs(secondBlastScheduleDays);
            thirdBlastScheduleDays.ShouldNotBeSameAs(firstBlastScheduleDays);
            fourthBlastScheduleDays.ShouldNotBeSameAs(firstBlastScheduleDays);
            fifthBlastScheduleDays.ShouldNotBeSameAs(firstBlastScheduleDays);
            sixthBlastScheduleDays.ShouldNotBeSameAs(firstBlastScheduleDays);
            sixthBlastScheduleDays.ShouldNotBeSameAs(fourthBlastScheduleDays);
        }

        #endregion

        #region General Constructor : Class (BlastScheduleDays) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastScheduleDays_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var errorList = new List<ECNError>();

            // Act
            var blastScheduleDays = new BlastScheduleDays();

            // Assert
            blastScheduleDays.BlastScheduleDaysID.ShouldBeNull();
            blastScheduleDays.BlastScheduleID.ShouldBeNull();
            blastScheduleDays.DayToSend.ShouldBeNull();
            blastScheduleDays.IsAmount.ShouldBeNull();
            blastScheduleDays.Total.ShouldBeNull();
            blastScheduleDays.Weeks.ShouldBeNull();
            blastScheduleDays.ErrorList.ShouldBe(errorList);
        }

        #endregion

        #endregion

        #endregion
    }
}
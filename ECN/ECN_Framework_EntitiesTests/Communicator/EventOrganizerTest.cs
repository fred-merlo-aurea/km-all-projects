using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class EventOrganizerTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (EventOrganizer) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EventOrganizer_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var eventOrganizer = Fixture.Create<EventOrganizer>();
            var customerId = Fixture.Create<int>();

            // Act
            eventOrganizer.CustomerID = customerId;

            // Assert
            eventOrganizer.CustomerID.ShouldBe(customerId);
        }

        #endregion

        #region General Getters/Setters : Class (EventOrganizer) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EventOrganizer_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var eventOrganizer = Fixture.Create<EventOrganizer>();
            eventOrganizer.CustomerID = Fixture.Create<int>();
            var intType = eventOrganizer.CustomerID.GetType();

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

        #region General Getters/Setters : Class (EventOrganizer) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EventOrganizer_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var eventOrganizer  = Fixture.Create<EventOrganizer>();

            // Act , Assert
            Should.NotThrow(() => eventOrganizer.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EventOrganizer_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var eventOrganizer  = Fixture.Create<EventOrganizer>();
            var propertyInfo  = eventOrganizer.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (EventOrganizer) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EventOrganizer_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new EventOrganizer());
        }

        #endregion

        #region General Constructor : Class (EventOrganizer) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EventOrganizer_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfEventOrganizer = Fixture.CreateMany<EventOrganizer>(2).ToList();
            var firstEventOrganizer = instancesOfEventOrganizer.FirstOrDefault();
            var lastEventOrganizer = instancesOfEventOrganizer.Last();

            // Act, Assert
            firstEventOrganizer.ShouldNotBeNull();
            lastEventOrganizer.ShouldNotBeNull();
            firstEventOrganizer.ShouldNotBeSameAs(lastEventOrganizer);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EventOrganizer_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEventOrganizer = new EventOrganizer();
            var secondEventOrganizer = new EventOrganizer();
            var thirdEventOrganizer = new EventOrganizer();
            var fourthEventOrganizer = new EventOrganizer();
            var fifthEventOrganizer = new EventOrganizer();
            var sixthEventOrganizer = new EventOrganizer();

            // Act, Assert
            firstEventOrganizer.ShouldNotBeNull();
            secondEventOrganizer.ShouldNotBeNull();
            thirdEventOrganizer.ShouldNotBeNull();
            fourthEventOrganizer.ShouldNotBeNull();
            fifthEventOrganizer.ShouldNotBeNull();
            sixthEventOrganizer.ShouldNotBeNull();
            firstEventOrganizer.ShouldNotBeSameAs(secondEventOrganizer);
            thirdEventOrganizer.ShouldNotBeSameAs(firstEventOrganizer);
            fourthEventOrganizer.ShouldNotBeSameAs(firstEventOrganizer);
            fifthEventOrganizer.ShouldNotBeSameAs(firstEventOrganizer);
            sixthEventOrganizer.ShouldNotBeSameAs(firstEventOrganizer);
            sixthEventOrganizer.ShouldNotBeSameAs(fourthEventOrganizer);
        }

        #endregion

        #endregion

        #endregion
    }
}
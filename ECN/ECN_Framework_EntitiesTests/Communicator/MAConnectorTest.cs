using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class MAConnectorTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (MAConnector) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAConnector_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var mAConnector = Fixture.Create<MAConnector>();
            var connectorId = Fixture.Create<int>();
            var from = Fixture.Create<string>();
            var to = Fixture.Create<string>();
            var marketingAutomationId = Fixture.Create<int>();
            var controlId = Fixture.Create<string>();

            // Act
            mAConnector.ConnectorID = connectorId;
            mAConnector.From = from;
            mAConnector.To = to;
            mAConnector.MarketingAutomationID = marketingAutomationId;
            mAConnector.ControlID = controlId;

            // Assert
            mAConnector.ConnectorID.ShouldBe(connectorId);
            mAConnector.From.ShouldBe(from);
            mAConnector.To.ShouldBe(to);
            mAConnector.MarketingAutomationID.ShouldBe(marketingAutomationId);
            mAConnector.ControlID.ShouldBe(controlId);
        }

        #endregion

        #region General Getters/Setters : Class (MAConnector) => Property (ConnectorID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAConnector_ConnectorID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var mAConnector = Fixture.Create<MAConnector>();
            mAConnector.ConnectorID = Fixture.Create<int>();
            var intType = mAConnector.ConnectorID.GetType();

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

        #region General Getters/Setters : Class (MAConnector) => Property (ConnectorID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAConnector_Class_Invalid_Property_ConnectorIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConnectorID = "ConnectorIDNotPresent";
            var mAConnector  = Fixture.Create<MAConnector>();

            // Act , Assert
            Should.NotThrow(() => mAConnector.GetType().GetProperty(propertyNameConnectorID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAConnector_ConnectorID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConnectorID = "ConnectorID";
            var mAConnector  = Fixture.Create<MAConnector>();
            var propertyInfo  = mAConnector.GetType().GetProperty(propertyNameConnectorID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MAConnector) => Property (ControlID) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAConnector_ControlID_Property_String_Type_Verify_Test()
        {
            // Arrange
            var mAConnector = Fixture.Create<MAConnector>();
            mAConnector.ControlID = Fixture.Create<string>();
            var stringType = mAConnector.ControlID.GetType();

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

        #region General Getters/Setters : Class (MAConnector) => Property (ControlID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAConnector_Class_Invalid_Property_ControlIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameControlID = "ControlIDNotPresent";
            var mAConnector  = Fixture.Create<MAConnector>();

            // Act , Assert
            Should.NotThrow(() => mAConnector.GetType().GetProperty(propertyNameControlID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAConnector_ControlID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameControlID = "ControlID";
            var mAConnector  = Fixture.Create<MAConnector>();
            var propertyInfo  = mAConnector.GetType().GetProperty(propertyNameControlID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MAConnector) => Property (From) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAConnector_From_Property_String_Type_Verify_Test()
        {
            // Arrange
            var mAConnector = Fixture.Create<MAConnector>();
            mAConnector.From = Fixture.Create<string>();
            var stringType = mAConnector.From.GetType();

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

        #region General Getters/Setters : Class (MAConnector) => Property (From) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAConnector_Class_Invalid_Property_FromNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFrom = "FromNotPresent";
            var mAConnector  = Fixture.Create<MAConnector>();

            // Act , Assert
            Should.NotThrow(() => mAConnector.GetType().GetProperty(propertyNameFrom));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAConnector_From_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFrom = "From";
            var mAConnector  = Fixture.Create<MAConnector>();
            var propertyInfo  = mAConnector.GetType().GetProperty(propertyNameFrom);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MAConnector) => Property (MarketingAutomationID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAConnector_MarketingAutomationID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var mAConnector = Fixture.Create<MAConnector>();
            mAConnector.MarketingAutomationID = Fixture.Create<int>();
            var intType = mAConnector.MarketingAutomationID.GetType();

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

        #region General Getters/Setters : Class (MAConnector) => Property (MarketingAutomationID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAConnector_Class_Invalid_Property_MarketingAutomationIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMarketingAutomationID = "MarketingAutomationIDNotPresent";
            var mAConnector  = Fixture.Create<MAConnector>();

            // Act , Assert
            Should.NotThrow(() => mAConnector.GetType().GetProperty(propertyNameMarketingAutomationID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAConnector_MarketingAutomationID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMarketingAutomationID = "MarketingAutomationID";
            var mAConnector  = Fixture.Create<MAConnector>();
            var propertyInfo  = mAConnector.GetType().GetProperty(propertyNameMarketingAutomationID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MAConnector) => Property (To) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAConnector_To_Property_String_Type_Verify_Test()
        {
            // Arrange
            var mAConnector = Fixture.Create<MAConnector>();
            mAConnector.To = Fixture.Create<string>();
            var stringType = mAConnector.To.GetType();

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

        #region General Getters/Setters : Class (MAConnector) => Property (To) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAConnector_Class_Invalid_Property_ToNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTo = "ToNotPresent";
            var mAConnector  = Fixture.Create<MAConnector>();

            // Act , Assert
            Should.NotThrow(() => mAConnector.GetType().GetProperty(propertyNameTo));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAConnector_To_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTo = "To";
            var mAConnector  = Fixture.Create<MAConnector>();
            var propertyInfo  = mAConnector.GetType().GetProperty(propertyNameTo);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (MAConnector) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MAConnector_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new MAConnector());
        }

        #endregion

        #region General Constructor : Class (MAConnector) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MAConnector_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfMAConnector = Fixture.CreateMany<MAConnector>(2).ToList();
            var firstMAConnector = instancesOfMAConnector.FirstOrDefault();
            var lastMAConnector = instancesOfMAConnector.Last();

            // Act, Assert
            firstMAConnector.ShouldNotBeNull();
            lastMAConnector.ShouldNotBeNull();
            firstMAConnector.ShouldNotBeSameAs(lastMAConnector);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MAConnector_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstMAConnector = new MAConnector();
            var secondMAConnector = new MAConnector();
            var thirdMAConnector = new MAConnector();
            var fourthMAConnector = new MAConnector();
            var fifthMAConnector = new MAConnector();
            var sixthMAConnector = new MAConnector();

            // Act, Assert
            firstMAConnector.ShouldNotBeNull();
            secondMAConnector.ShouldNotBeNull();
            thirdMAConnector.ShouldNotBeNull();
            fourthMAConnector.ShouldNotBeNull();
            fifthMAConnector.ShouldNotBeNull();
            sixthMAConnector.ShouldNotBeNull();
            firstMAConnector.ShouldNotBeSameAs(secondMAConnector);
            thirdMAConnector.ShouldNotBeSameAs(firstMAConnector);
            fourthMAConnector.ShouldNotBeSameAs(firstMAConnector);
            fifthMAConnector.ShouldNotBeSameAs(firstMAConnector);
            sixthMAConnector.ShouldNotBeSameAs(firstMAConnector);
            sixthMAConnector.ShouldNotBeSameAs(fourthMAConnector);
        }

        #endregion

        #region General Constructor : Class (MAConnector) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MAConnector_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var connectorId = -1;
            var from = "";
            var to = "";
            var marketingAutomationId = -1;
            var controlId = "";

            // Act
            var mAConnector = new MAConnector();

            // Assert
            mAConnector.ConnectorID.ShouldBe(connectorId);
            mAConnector.From.ShouldBe(from);
            mAConnector.To.ShouldBe(to);
            mAConnector.MarketingAutomationID.ShouldBe(marketingAutomationId);
            mAConnector.ControlID.ShouldBe(controlId);
        }

        #endregion

        #endregion

        #endregion
    }
}
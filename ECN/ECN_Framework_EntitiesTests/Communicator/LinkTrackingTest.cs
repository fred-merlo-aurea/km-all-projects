using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class LinkTrackingTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (LinkTracking) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTracking_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var linkTracking = Fixture.Create<LinkTracking>();
            var lTId = Fixture.Create<int>();
            var displayName = Fixture.Create<string>();
            var isActive = Fixture.Create<bool?>();

            // Act
            linkTracking.LTID = lTId;
            linkTracking.DisplayName = displayName;
            linkTracking.IsActive = isActive;

            // Assert
            linkTracking.LTID.ShouldBe(lTId);
            linkTracking.DisplayName.ShouldBe(displayName);
            linkTracking.IsActive.ShouldBe(isActive);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTracking) => Property (DisplayName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTracking_DisplayName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkTracking = Fixture.Create<LinkTracking>();
            linkTracking.DisplayName = Fixture.Create<string>();
            var stringType = linkTracking.DisplayName.GetType();

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

        #region General Getters/Setters : Class (LinkTracking) => Property (DisplayName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTracking_Class_Invalid_Property_DisplayNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayNameNotPresent";
            var linkTracking  = Fixture.Create<LinkTracking>();

            // Act , Assert
            Should.NotThrow(() => linkTracking.GetType().GetProperty(propertyNameDisplayName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTracking_DisplayName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayName";
            var linkTracking  = Fixture.Create<LinkTracking>();
            var propertyInfo  = linkTracking.GetType().GetProperty(propertyNameDisplayName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTracking) => Property (IsActive) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTracking_IsActive_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTracking = Fixture.Create<LinkTracking>();
            var random = Fixture.Create<bool>();

            // Act , Set
            linkTracking.IsActive = random;

            // Assert
            linkTracking.IsActive.ShouldBe(random);
            linkTracking.IsActive.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTracking_IsActive_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTracking = Fixture.Create<LinkTracking>();

            // Act , Set
            linkTracking.IsActive = null;

            // Assert
            linkTracking.IsActive.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTracking_IsActive_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsActive = "IsActive";
            var linkTracking = Fixture.Create<LinkTracking>();
            var propertyInfo = linkTracking.GetType().GetProperty(propertyNameIsActive);

            // Act , Set
            propertyInfo.SetValue(linkTracking, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTracking.IsActive.ShouldBeNull();
            linkTracking.IsActive.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTracking) => Property (IsActive) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTracking_Class_Invalid_Property_IsActiveNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActiveNotPresent";
            var linkTracking  = Fixture.Create<LinkTracking>();

            // Act , Assert
            Should.NotThrow(() => linkTracking.GetType().GetProperty(propertyNameIsActive));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTracking_IsActive_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActive";
            var linkTracking  = Fixture.Create<LinkTracking>();
            var propertyInfo  = linkTracking.GetType().GetProperty(propertyNameIsActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTracking) => Property (LTID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTracking_LTID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkTracking = Fixture.Create<LinkTracking>();
            linkTracking.LTID = Fixture.Create<int>();
            var intType = linkTracking.LTID.GetType();

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

        #region General Getters/Setters : Class (LinkTracking) => Property (LTID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTracking_Class_Invalid_Property_LTIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLTID = "LTIDNotPresent";
            var linkTracking  = Fixture.Create<LinkTracking>();

            // Act , Assert
            Should.NotThrow(() => linkTracking.GetType().GetProperty(propertyNameLTID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTracking_LTID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLTID = "LTID";
            var linkTracking  = Fixture.Create<LinkTracking>();
            var propertyInfo  = linkTracking.GetType().GetProperty(propertyNameLTID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (LinkTracking) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTracking_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new LinkTracking());
        }

        #endregion

        #region General Constructor : Class (LinkTracking) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTracking_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfLinkTracking = Fixture.CreateMany<LinkTracking>(2).ToList();
            var firstLinkTracking = instancesOfLinkTracking.FirstOrDefault();
            var lastLinkTracking = instancesOfLinkTracking.Last();

            // Act, Assert
            firstLinkTracking.ShouldNotBeNull();
            lastLinkTracking.ShouldNotBeNull();
            firstLinkTracking.ShouldNotBeSameAs(lastLinkTracking);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTracking_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstLinkTracking = new LinkTracking();
            var secondLinkTracking = new LinkTracking();
            var thirdLinkTracking = new LinkTracking();
            var fourthLinkTracking = new LinkTracking();
            var fifthLinkTracking = new LinkTracking();
            var sixthLinkTracking = new LinkTracking();

            // Act, Assert
            firstLinkTracking.ShouldNotBeNull();
            secondLinkTracking.ShouldNotBeNull();
            thirdLinkTracking.ShouldNotBeNull();
            fourthLinkTracking.ShouldNotBeNull();
            fifthLinkTracking.ShouldNotBeNull();
            sixthLinkTracking.ShouldNotBeNull();
            firstLinkTracking.ShouldNotBeSameAs(secondLinkTracking);
            thirdLinkTracking.ShouldNotBeSameAs(firstLinkTracking);
            fourthLinkTracking.ShouldNotBeSameAs(firstLinkTracking);
            fifthLinkTracking.ShouldNotBeSameAs(firstLinkTracking);
            sixthLinkTracking.ShouldNotBeSameAs(firstLinkTracking);
            sixthLinkTracking.ShouldNotBeSameAs(fourthLinkTracking);
        }

        #endregion

        #region General Constructor : Class (LinkTracking) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTracking_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var lTId = -1;
            var displayName = string.Empty;

            // Act
            var linkTracking = new LinkTracking();

            // Assert
            linkTracking.LTID.ShouldBe(lTId);
            linkTracking.DisplayName.ShouldBe(displayName);
            linkTracking.IsActive.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}
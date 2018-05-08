using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class LinkTrackingParamTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (LinkTrackingParam) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var linkTrackingParam = Fixture.Create<LinkTrackingParam>();
            var lTPId = Fixture.Create<int>();
            var lTId = Fixture.Create<int?>();
            var displayName = Fixture.Create<string>();
            var isActive = Fixture.Create<bool?>();

            // Act
            linkTrackingParam.LTPID = lTPId;
            linkTrackingParam.LTID = lTId;
            linkTrackingParam.DisplayName = displayName;
            linkTrackingParam.IsActive = isActive;

            // Assert
            linkTrackingParam.LTPID.ShouldBe(lTPId);
            linkTrackingParam.LTID.ShouldBe(lTId);
            linkTrackingParam.DisplayName.ShouldBe(displayName);
            linkTrackingParam.IsActive.ShouldBe(isActive);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParam) => Property (DisplayName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_DisplayName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingParam = Fixture.Create<LinkTrackingParam>();
            linkTrackingParam.DisplayName = Fixture.Create<string>();
            var stringType = linkTrackingParam.DisplayName.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingParam) => Property (DisplayName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_Class_Invalid_Property_DisplayNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayNameNotPresent";
            var linkTrackingParam  = Fixture.Create<LinkTrackingParam>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParam.GetType().GetProperty(propertyNameDisplayName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_DisplayName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayName";
            var linkTrackingParam  = Fixture.Create<LinkTrackingParam>();
            var propertyInfo  = linkTrackingParam.GetType().GetProperty(propertyNameDisplayName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParam) => Property (IsActive) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_IsActive_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingParam = Fixture.Create<LinkTrackingParam>();
            var random = Fixture.Create<bool>();

            // Act , Set
            linkTrackingParam.IsActive = random;

            // Assert
            linkTrackingParam.IsActive.ShouldBe(random);
            linkTrackingParam.IsActive.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_IsActive_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingParam = Fixture.Create<LinkTrackingParam>();

            // Act , Set
            linkTrackingParam.IsActive = null;

            // Assert
            linkTrackingParam.IsActive.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_IsActive_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsActive = "IsActive";
            var linkTrackingParam = Fixture.Create<LinkTrackingParam>();
            var propertyInfo = linkTrackingParam.GetType().GetProperty(propertyNameIsActive);

            // Act , Set
            propertyInfo.SetValue(linkTrackingParam, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingParam.IsActive.ShouldBeNull();
            linkTrackingParam.IsActive.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParam) => Property (IsActive) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_Class_Invalid_Property_IsActiveNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActiveNotPresent";
            var linkTrackingParam  = Fixture.Create<LinkTrackingParam>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParam.GetType().GetProperty(propertyNameIsActive));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_IsActive_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActive";
            var linkTrackingParam  = Fixture.Create<LinkTrackingParam>();
            var propertyInfo  = linkTrackingParam.GetType().GetProperty(propertyNameIsActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParam) => Property (LTID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_LTID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingParam = Fixture.Create<LinkTrackingParam>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkTrackingParam.LTID = random;

            // Assert
            linkTrackingParam.LTID.ShouldBe(random);
            linkTrackingParam.LTID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_LTID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingParam = Fixture.Create<LinkTrackingParam>();

            // Act , Set
            linkTrackingParam.LTID = null;

            // Assert
            linkTrackingParam.LTID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_LTID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameLTID = "LTID";
            var linkTrackingParam = Fixture.Create<LinkTrackingParam>();
            var propertyInfo = linkTrackingParam.GetType().GetProperty(propertyNameLTID);

            // Act , Set
            propertyInfo.SetValue(linkTrackingParam, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingParam.LTID.ShouldBeNull();
            linkTrackingParam.LTID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParam) => Property (LTID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_Class_Invalid_Property_LTIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLTID = "LTIDNotPresent";
            var linkTrackingParam  = Fixture.Create<LinkTrackingParam>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParam.GetType().GetProperty(propertyNameLTID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_LTID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLTID = "LTID";
            var linkTrackingParam  = Fixture.Create<LinkTrackingParam>();
            var propertyInfo  = linkTrackingParam.GetType().GetProperty(propertyNameLTID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParam) => Property (LTPID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_LTPID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingParam = Fixture.Create<LinkTrackingParam>();
            linkTrackingParam.LTPID = Fixture.Create<int>();
            var intType = linkTrackingParam.LTPID.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingParam) => Property (LTPID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_Class_Invalid_Property_LTPIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLTPID = "LTPIDNotPresent";
            var linkTrackingParam  = Fixture.Create<LinkTrackingParam>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParam.GetType().GetProperty(propertyNameLTPID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParam_LTPID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLTPID = "LTPID";
            var linkTrackingParam  = Fixture.Create<LinkTrackingParam>();
            var propertyInfo  = linkTrackingParam.GetType().GetProperty(propertyNameLTPID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (LinkTrackingParam) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingParam_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new LinkTrackingParam());
        }

        #endregion

        #region General Constructor : Class (LinkTrackingParam) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingParam_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfLinkTrackingParam = Fixture.CreateMany<LinkTrackingParam>(2).ToList();
            var firstLinkTrackingParam = instancesOfLinkTrackingParam.FirstOrDefault();
            var lastLinkTrackingParam = instancesOfLinkTrackingParam.Last();

            // Act, Assert
            firstLinkTrackingParam.ShouldNotBeNull();
            lastLinkTrackingParam.ShouldNotBeNull();
            firstLinkTrackingParam.ShouldNotBeSameAs(lastLinkTrackingParam);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingParam_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstLinkTrackingParam = new LinkTrackingParam();
            var secondLinkTrackingParam = new LinkTrackingParam();
            var thirdLinkTrackingParam = new LinkTrackingParam();
            var fourthLinkTrackingParam = new LinkTrackingParam();
            var fifthLinkTrackingParam = new LinkTrackingParam();
            var sixthLinkTrackingParam = new LinkTrackingParam();

            // Act, Assert
            firstLinkTrackingParam.ShouldNotBeNull();
            secondLinkTrackingParam.ShouldNotBeNull();
            thirdLinkTrackingParam.ShouldNotBeNull();
            fourthLinkTrackingParam.ShouldNotBeNull();
            fifthLinkTrackingParam.ShouldNotBeNull();
            sixthLinkTrackingParam.ShouldNotBeNull();
            firstLinkTrackingParam.ShouldNotBeSameAs(secondLinkTrackingParam);
            thirdLinkTrackingParam.ShouldNotBeSameAs(firstLinkTrackingParam);
            fourthLinkTrackingParam.ShouldNotBeSameAs(firstLinkTrackingParam);
            fifthLinkTrackingParam.ShouldNotBeSameAs(firstLinkTrackingParam);
            sixthLinkTrackingParam.ShouldNotBeSameAs(firstLinkTrackingParam);
            sixthLinkTrackingParam.ShouldNotBeSameAs(fourthLinkTrackingParam);
        }

        #endregion

        #region General Constructor : Class (LinkTrackingParam) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingParam_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var lTPId = -1;
            var displayName = string.Empty;

            // Act
            var linkTrackingParam = new LinkTrackingParam();

            // Assert
            linkTrackingParam.LTPID.ShouldBe(lTPId);
            linkTrackingParam.LTID.ShouldBeNull();
            linkTrackingParam.DisplayName.ShouldBe(displayName);
            linkTrackingParam.IsActive.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}
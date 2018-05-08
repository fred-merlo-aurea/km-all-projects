using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class BlastLinkTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastLink) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastLink_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastLink = Fixture.Create<BlastLink>();
            var blastLinkId = Fixture.Create<int>();
            var blastId = Fixture.Create<int>();
            var linkURL = Fixture.Create<string>();

            // Act
            blastLink.BlastLinkID = blastLinkId;
            blastLink.BlastID = blastId;
            blastLink.LinkURL = linkURL;

            // Assert
            blastLink.BlastLinkID.ShouldBe(blastLinkId);
            blastLink.BlastID.ShouldBe(blastId);
            blastLink.LinkURL.ShouldBe(linkURL);
        }

        #endregion

        #region General Getters/Setters : Class (BlastLink) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastLink_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastLink = Fixture.Create<BlastLink>();
            blastLink.BlastID = Fixture.Create<int>();
            var intType = blastLink.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastLink) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastLink_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastLink  = Fixture.Create<BlastLink>();

            // Act , Assert
            Should.NotThrow(() => blastLink.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastLink_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastLink  = Fixture.Create<BlastLink>();
            var propertyInfo  = blastLink.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastLink) => Property (BlastLinkID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastLink_BlastLinkID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastLink = Fixture.Create<BlastLink>();
            blastLink.BlastLinkID = Fixture.Create<int>();
            var intType = blastLink.BlastLinkID.GetType();

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

        #region General Getters/Setters : Class (BlastLink) => Property (BlastLinkID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastLink_Class_Invalid_Property_BlastLinkIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastLinkID = "BlastLinkIDNotPresent";
            var blastLink  = Fixture.Create<BlastLink>();

            // Act , Assert
            Should.NotThrow(() => blastLink.GetType().GetProperty(propertyNameBlastLinkID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastLink_BlastLinkID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastLinkID = "BlastLinkID";
            var blastLink  = Fixture.Create<BlastLink>();
            var propertyInfo  = blastLink.GetType().GetProperty(propertyNameBlastLinkID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastLink) => Property (LinkURL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastLink_LinkURL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastLink = Fixture.Create<BlastLink>();
            blastLink.LinkURL = Fixture.Create<string>();
            var stringType = blastLink.LinkURL.GetType();

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

        #region General Getters/Setters : Class (BlastLink) => Property (LinkURL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastLink_Class_Invalid_Property_LinkURLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLinkURL = "LinkURLNotPresent";
            var blastLink  = Fixture.Create<BlastLink>();

            // Act , Assert
            Should.NotThrow(() => blastLink.GetType().GetProperty(propertyNameLinkURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastLink_LinkURL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLinkURL = "LinkURL";
            var blastLink  = Fixture.Create<BlastLink>();
            var propertyInfo  = blastLink.GetType().GetProperty(propertyNameLinkURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastLink) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastLink_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastLink());
        }

        #endregion

        #region General Constructor : Class (BlastLink) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastLink_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastLink = Fixture.CreateMany<BlastLink>(2).ToList();
            var firstBlastLink = instancesOfBlastLink.FirstOrDefault();
            var lastBlastLink = instancesOfBlastLink.Last();

            // Act, Assert
            firstBlastLink.ShouldNotBeNull();
            lastBlastLink.ShouldNotBeNull();
            firstBlastLink.ShouldNotBeSameAs(lastBlastLink);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastLink_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastLink = new BlastLink();
            var secondBlastLink = new BlastLink();
            var thirdBlastLink = new BlastLink();
            var fourthBlastLink = new BlastLink();
            var fifthBlastLink = new BlastLink();
            var sixthBlastLink = new BlastLink();

            // Act, Assert
            firstBlastLink.ShouldNotBeNull();
            secondBlastLink.ShouldNotBeNull();
            thirdBlastLink.ShouldNotBeNull();
            fourthBlastLink.ShouldNotBeNull();
            fifthBlastLink.ShouldNotBeNull();
            sixthBlastLink.ShouldNotBeNull();
            firstBlastLink.ShouldNotBeSameAs(secondBlastLink);
            thirdBlastLink.ShouldNotBeSameAs(firstBlastLink);
            fourthBlastLink.ShouldNotBeSameAs(firstBlastLink);
            fifthBlastLink.ShouldNotBeSameAs(firstBlastLink);
            sixthBlastLink.ShouldNotBeSameAs(firstBlastLink);
            sixthBlastLink.ShouldNotBeSameAs(fourthBlastLink);
        }

        #endregion

        #region General Constructor : Class (BlastLink) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastLink_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var blastLinkId = -1;
            var blastId = -1;
            var linkURL = string.Empty;

            // Act
            var blastLink = new BlastLink();

            // Assert
            blastLink.BlastLinkID.ShouldBe(blastLinkId);
            blastLink.BlastID.ShouldBe(blastId);
            blastLink.LinkURL.ShouldBe(linkURL);
        }

        #endregion

        #endregion

        #endregion
    }
}
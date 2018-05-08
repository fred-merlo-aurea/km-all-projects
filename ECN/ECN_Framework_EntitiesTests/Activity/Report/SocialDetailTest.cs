using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;
using Shouldly;
using AutoFixture;
using NUnit.Framework;
using Moq;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_Entities.Activity.Report
{
    [TestFixture]
    public class SocialDetailTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (SocialDetail) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var socialDetail = Fixture.Create<SocialDetail>();
            var blastId = Fixture.Create<int?>();
            var displayName = Fixture.Create<string>();
            var click = Fixture.Create<int?>();
            var emailAddress = Fixture.Create<string>();
            var socialMediaId = Fixture.Create<int?>();

            // Act
            socialDetail.BlastID = blastId;
            socialDetail.DisplayName = displayName;
            socialDetail.Click = click;
            socialDetail.EmailAddress = emailAddress;
            socialDetail.SocialMediaID = socialMediaId;

            // Assert
            socialDetail.BlastID.ShouldBe(blastId);
            socialDetail.DisplayName.ShouldBe(displayName);
            socialDetail.Click.ShouldBe(click);
            socialDetail.EmailAddress.ShouldBe(emailAddress);
            socialDetail.SocialMediaID.ShouldBe(socialMediaId);
        }

        #endregion

        #region General Getters/Setters : Class (SocialDetail) => Property (BlastID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_BlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var socialDetail = Fixture.Create<SocialDetail>();
            var random = Fixture.Create<int>();

            // Act , Set
            socialDetail.BlastID = random;

            // Assert
            socialDetail.BlastID.ShouldBe(random);
            socialDetail.BlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_BlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var socialDetail = Fixture.Create<SocialDetail>();    

            // Act , Set
            socialDetail.BlastID = null;

            // Assert
            socialDetail.BlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_BlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastID = "BlastID";
            var socialDetail = Fixture.Create<SocialDetail>();
            var propertyInfo = socialDetail.GetType().GetProperty(propertyNameBlastID);

            // Act , Set
            propertyInfo.SetValue(socialDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            socialDetail.BlastID.ShouldBeNull();
            socialDetail.BlastID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (SocialDetail) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var socialDetail  = Fixture.Create<SocialDetail>();

            // Act , Assert
            Should.NotThrow(() => socialDetail.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var socialDetail  = Fixture.Create<SocialDetail>();
            var propertyInfo  = socialDetail.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SocialDetail) => Property (Click) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_Click_Property_Data_Without_Null_Test()
        {
            // Arrange
            var socialDetail = Fixture.Create<SocialDetail>();
            var random = Fixture.Create<int>();

            // Act , Set
            socialDetail.Click = random;

            // Assert
            socialDetail.Click.ShouldBe(random);
            socialDetail.Click.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_Click_Property_Only_Null_Data_Test()
        {
            // Arrange
            var socialDetail = Fixture.Create<SocialDetail>();    

            // Act , Set
            socialDetail.Click = null;

            // Assert
            socialDetail.Click.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_Click_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameClick = "Click";
            var socialDetail = Fixture.Create<SocialDetail>();
            var propertyInfo = socialDetail.GetType().GetProperty(propertyNameClick);

            // Act , Set
            propertyInfo.SetValue(socialDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            socialDetail.Click.ShouldBeNull();
            socialDetail.Click.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (SocialDetail) => Property (Click) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_Class_Invalid_Property_ClickNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClick = "ClickNotPresent";
            var socialDetail  = Fixture.Create<SocialDetail>();

            // Act , Assert
            Should.NotThrow(() => socialDetail.GetType().GetProperty(propertyNameClick));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_Click_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClick = "Click";
            var socialDetail  = Fixture.Create<SocialDetail>();
            var propertyInfo  = socialDetail.GetType().GetProperty(propertyNameClick);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SocialDetail) => Property (DisplayName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_DisplayName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var socialDetail = Fixture.Create<SocialDetail>();
            socialDetail.DisplayName = Fixture.Create<string>();
            var stringType = socialDetail.DisplayName.GetType();

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

        #region General Getters/Setters : Class (SocialDetail) => Property (DisplayName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_Class_Invalid_Property_DisplayNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayNameNotPresent";
            var socialDetail  = Fixture.Create<SocialDetail>();

            // Act , Assert
            Should.NotThrow(() => socialDetail.GetType().GetProperty(propertyNameDisplayName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_DisplayName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayName";
            var socialDetail  = Fixture.Create<SocialDetail>();
            var propertyInfo  = socialDetail.GetType().GetProperty(propertyNameDisplayName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SocialDetail) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var socialDetail = Fixture.Create<SocialDetail>();
            socialDetail.EmailAddress = Fixture.Create<string>();
            var stringType = socialDetail.EmailAddress.GetType();

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

        #region General Getters/Setters : Class (SocialDetail) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var socialDetail  = Fixture.Create<SocialDetail>();

            // Act , Assert
            Should.NotThrow(() => socialDetail.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var socialDetail  = Fixture.Create<SocialDetail>();
            var propertyInfo  = socialDetail.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SocialDetail) => Property (SocialMediaID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_SocialMediaID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var socialDetail = Fixture.Create<SocialDetail>();
            var random = Fixture.Create<int>();

            // Act , Set
            socialDetail.SocialMediaID = random;

            // Assert
            socialDetail.SocialMediaID.ShouldBe(random);
            socialDetail.SocialMediaID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_SocialMediaID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var socialDetail = Fixture.Create<SocialDetail>();    

            // Act , Set
            socialDetail.SocialMediaID = null;

            // Assert
            socialDetail.SocialMediaID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_SocialMediaID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSocialMediaID = "SocialMediaID";
            var socialDetail = Fixture.Create<SocialDetail>();
            var propertyInfo = socialDetail.GetType().GetProperty(propertyNameSocialMediaID);

            // Act , Set
            propertyInfo.SetValue(socialDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            socialDetail.SocialMediaID.ShouldBeNull();
            socialDetail.SocialMediaID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (SocialDetail) => Property (SocialMediaID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_Class_Invalid_Property_SocialMediaIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSocialMediaID = "SocialMediaIDNotPresent";
            var socialDetail  = Fixture.Create<SocialDetail>();

            // Act , Assert
            Should.NotThrow(() => socialDetail.GetType().GetProperty(propertyNameSocialMediaID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialDetail_SocialMediaID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSocialMediaID = "SocialMediaID";
            var socialDetail  = Fixture.Create<SocialDetail>();
            var propertyInfo  = socialDetail.GetType().GetProperty(propertyNameSocialMediaID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (SocialDetail) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SocialDetail_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new SocialDetail());
        }

        #endregion

        #region General Constructor : Class (SocialDetail) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SocialDetail_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfSocialDetail = Fixture.CreateMany<SocialDetail>(2).ToList();
            var firstSocialDetail = instancesOfSocialDetail.FirstOrDefault();
            var lastSocialDetail = instancesOfSocialDetail.Last();

            // Act, Assert
            firstSocialDetail.ShouldNotBeNull();
            lastSocialDetail.ShouldNotBeNull();
            firstSocialDetail.ShouldNotBeSameAs(lastSocialDetail);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SocialDetail_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstSocialDetail = new SocialDetail();
            var secondSocialDetail = new SocialDetail();
            var thirdSocialDetail = new SocialDetail();
            var fourthSocialDetail = new SocialDetail();
            var fifthSocialDetail = new SocialDetail();
            var sixthSocialDetail = new SocialDetail();

            // Act, Assert
            firstSocialDetail.ShouldNotBeNull();
            secondSocialDetail.ShouldNotBeNull();
            thirdSocialDetail.ShouldNotBeNull();
            fourthSocialDetail.ShouldNotBeNull();
            fifthSocialDetail.ShouldNotBeNull();
            sixthSocialDetail.ShouldNotBeNull();
            firstSocialDetail.ShouldNotBeSameAs(secondSocialDetail);
            thirdSocialDetail.ShouldNotBeSameAs(firstSocialDetail);
            fourthSocialDetail.ShouldNotBeSameAs(firstSocialDetail);
            fifthSocialDetail.ShouldNotBeSameAs(firstSocialDetail);
            sixthSocialDetail.ShouldNotBeSameAs(firstSocialDetail);
            sixthSocialDetail.ShouldNotBeSameAs(fourthSocialDetail);
        }

        #endregion

        #region General Constructor : Class (SocialDetail) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SocialDetail_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var emailAddress = string.Empty;
            var displayName = string.Empty;

            // Act
            var socialDetail = new SocialDetail();

            // Assert
            socialDetail.BlastID.ShouldBeNull();
            socialDetail.EmailAddress.ShouldBe(emailAddress);
            socialDetail.Click.ShouldBeNull();
            socialDetail.DisplayName.ShouldBe(displayName);
        }

        #endregion

        #endregion

        #endregion
    }
}
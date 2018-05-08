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
using ECN_Framework_Entities.Activity;

namespace ECN_Framework_Entities.Activity
{
    [TestFixture]
    public class BlastActivitySocialTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastActivitySocial) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            var socialId = Fixture.Create<int?>();
            var blastId = Fixture.Create<int?>();
            var emailId = Fixture.Create<int?>();
            var refEmailId = Fixture.Create<int?>();
            var socialActivityCodeId = Fixture.Create<int?>();
            var socialActivityCode = Fixture.Create<string>();
            var actionDate = Fixture.Create<DateTime?>();
            var uRL = Fixture.Create<string>();
            var socialMediaId = Fixture.Create<int?>();
            var socialMedia = Fixture.Create<string>();

            // Act
            blastActivitySocial.SocialID = socialId;
            blastActivitySocial.BlastID = blastId;
            blastActivitySocial.EmailID = emailId;
            blastActivitySocial.RefEmailID = refEmailId;
            blastActivitySocial.SocialActivityCodeID = socialActivityCodeId;
            blastActivitySocial.SocialActivityCode = socialActivityCode;
            blastActivitySocial.ActionDate = actionDate;
            blastActivitySocial.URL = uRL;
            blastActivitySocial.SocialMediaID = socialMediaId;
            blastActivitySocial.SocialMedia = socialMedia;

            // Assert
            blastActivitySocial.SocialID.ShouldBe(socialId);
            blastActivitySocial.BlastID.ShouldBe(blastId);
            blastActivitySocial.EmailID.ShouldBe(emailId);
            blastActivitySocial.RefEmailID.ShouldBe(refEmailId);
            blastActivitySocial.SocialActivityCodeID.ShouldBe(socialActivityCodeId);
            blastActivitySocial.SocialActivityCode.ShouldBe(socialActivityCode);
            blastActivitySocial.ActionDate.ShouldBe(actionDate);
            blastActivitySocial.URL.ShouldBe(uRL);
            blastActivitySocial.SocialMediaID.ShouldBe(socialMediaId);
            blastActivitySocial.SocialMedia.ShouldBe(socialMedia);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (ActionDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_ActionDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDate";
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastActivitySocial.GetType().GetProperty(propertyNameActionDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastActivitySocial, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (ActionDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_Class_Invalid_Property_ActionDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDateNotPresent";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySocial.GetType().GetProperty(propertyNameActionDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_ActionDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDate";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();
            var propertyInfo  = blastActivitySocial.GetType().GetProperty(propertyNameActionDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (BlastID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_BlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastActivitySocial.BlastID = random;

            // Assert
            blastActivitySocial.BlastID.ShouldBe(random);
            blastActivitySocial.BlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_BlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();    

            // Act , Set
            blastActivitySocial.BlastID = null;

            // Assert
            blastActivitySocial.BlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_BlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastID = "BlastID";
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            var propertyInfo = blastActivitySocial.GetType().GetProperty(propertyNameBlastID);

            // Act , Set
            propertyInfo.SetValue(blastActivitySocial, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastActivitySocial.BlastID.ShouldBeNull();
            blastActivitySocial.BlastID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySocial.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();
            var propertyInfo  = blastActivitySocial.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (EmailID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_EmailID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastActivitySocial.EmailID = random;

            // Assert
            blastActivitySocial.EmailID.ShouldBe(random);
            blastActivitySocial.EmailID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_EmailID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();    

            // Act , Set
            blastActivitySocial.EmailID = null;

            // Assert
            blastActivitySocial.EmailID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_EmailID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameEmailID = "EmailID";
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            var propertyInfo = blastActivitySocial.GetType().GetProperty(propertyNameEmailID);

            // Act , Set
            propertyInfo.SetValue(blastActivitySocial, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastActivitySocial.EmailID.ShouldBeNull();
            blastActivitySocial.EmailID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySocial.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();
            var propertyInfo  = blastActivitySocial.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (RefEmailID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_RefEmailID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastActivitySocial.RefEmailID = random;

            // Assert
            blastActivitySocial.RefEmailID.ShouldBe(random);
            blastActivitySocial.RefEmailID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_RefEmailID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();    

            // Act , Set
            blastActivitySocial.RefEmailID = null;

            // Assert
            blastActivitySocial.RefEmailID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_RefEmailID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameRefEmailID = "RefEmailID";
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            var propertyInfo = blastActivitySocial.GetType().GetProperty(propertyNameRefEmailID);

            // Act , Set
            propertyInfo.SetValue(blastActivitySocial, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastActivitySocial.RefEmailID.ShouldBeNull();
            blastActivitySocial.RefEmailID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (RefEmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_Class_Invalid_Property_RefEmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRefEmailID = "RefEmailIDNotPresent";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySocial.GetType().GetProperty(propertyNameRefEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_RefEmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRefEmailID = "RefEmailID";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();
            var propertyInfo  = blastActivitySocial.GetType().GetProperty(propertyNameRefEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (SocialActivityCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_SocialActivityCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            blastActivitySocial.SocialActivityCode = Fixture.Create<string>();
            var stringType = blastActivitySocial.SocialActivityCode.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (SocialActivityCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_Class_Invalid_Property_SocialActivityCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSocialActivityCode = "SocialActivityCodeNotPresent";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySocial.GetType().GetProperty(propertyNameSocialActivityCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_SocialActivityCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSocialActivityCode = "SocialActivityCode";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();
            var propertyInfo  = blastActivitySocial.GetType().GetProperty(propertyNameSocialActivityCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (SocialActivityCodeID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_SocialActivityCodeID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastActivitySocial.SocialActivityCodeID = random;

            // Assert
            blastActivitySocial.SocialActivityCodeID.ShouldBe(random);
            blastActivitySocial.SocialActivityCodeID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_SocialActivityCodeID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();    

            // Act , Set
            blastActivitySocial.SocialActivityCodeID = null;

            // Assert
            blastActivitySocial.SocialActivityCodeID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_SocialActivityCodeID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSocialActivityCodeID = "SocialActivityCodeID";
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            var propertyInfo = blastActivitySocial.GetType().GetProperty(propertyNameSocialActivityCodeID);

            // Act , Set
            propertyInfo.SetValue(blastActivitySocial, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastActivitySocial.SocialActivityCodeID.ShouldBeNull();
            blastActivitySocial.SocialActivityCodeID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (SocialActivityCodeID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_Class_Invalid_Property_SocialActivityCodeIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSocialActivityCodeID = "SocialActivityCodeIDNotPresent";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySocial.GetType().GetProperty(propertyNameSocialActivityCodeID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_SocialActivityCodeID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSocialActivityCodeID = "SocialActivityCodeID";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();
            var propertyInfo  = blastActivitySocial.GetType().GetProperty(propertyNameSocialActivityCodeID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (SocialID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_SocialID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastActivitySocial.SocialID = random;

            // Assert
            blastActivitySocial.SocialID.ShouldBe(random);
            blastActivitySocial.SocialID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_SocialID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();    

            // Act , Set
            blastActivitySocial.SocialID = null;

            // Assert
            blastActivitySocial.SocialID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_SocialID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSocialID = "SocialID";
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            var propertyInfo = blastActivitySocial.GetType().GetProperty(propertyNameSocialID);

            // Act , Set
            propertyInfo.SetValue(blastActivitySocial, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastActivitySocial.SocialID.ShouldBeNull();
            blastActivitySocial.SocialID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (SocialID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_Class_Invalid_Property_SocialIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSocialID = "SocialIDNotPresent";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySocial.GetType().GetProperty(propertyNameSocialID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_SocialID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSocialID = "SocialID";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();
            var propertyInfo  = blastActivitySocial.GetType().GetProperty(propertyNameSocialID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (SocialMedia) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_SocialMedia_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            blastActivitySocial.SocialMedia = Fixture.Create<string>();
            var stringType = blastActivitySocial.SocialMedia.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (SocialMedia) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_Class_Invalid_Property_SocialMediaNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSocialMedia = "SocialMediaNotPresent";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySocial.GetType().GetProperty(propertyNameSocialMedia));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_SocialMedia_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSocialMedia = "SocialMedia";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();
            var propertyInfo  = blastActivitySocial.GetType().GetProperty(propertyNameSocialMedia);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (SocialMediaID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_SocialMediaID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastActivitySocial.SocialMediaID = random;

            // Assert
            blastActivitySocial.SocialMediaID.ShouldBe(random);
            blastActivitySocial.SocialMediaID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_SocialMediaID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();    

            // Act , Set
            blastActivitySocial.SocialMediaID = null;

            // Assert
            blastActivitySocial.SocialMediaID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_SocialMediaID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSocialMediaID = "SocialMediaID";
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            var propertyInfo = blastActivitySocial.GetType().GetProperty(propertyNameSocialMediaID);

            // Act , Set
            propertyInfo.SetValue(blastActivitySocial, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastActivitySocial.SocialMediaID.ShouldBeNull();
            blastActivitySocial.SocialMediaID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (SocialMediaID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_Class_Invalid_Property_SocialMediaIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSocialMediaID = "SocialMediaIDNotPresent";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySocial.GetType().GetProperty(propertyNameSocialMediaID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_SocialMediaID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSocialMediaID = "SocialMediaID";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();
            var propertyInfo  = blastActivitySocial.GetType().GetProperty(propertyNameSocialMediaID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (URL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_URL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySocial = Fixture.Create<BlastActivitySocial>();
            blastActivitySocial.URL = Fixture.Create<string>();
            var stringType = blastActivitySocial.URL.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySocial) => Property (URL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_Class_Invalid_Property_URLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameURL = "URLNotPresent";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySocial.GetType().GetProperty(propertyNameURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySocial_URL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameURL = "URL";
            var blastActivitySocial  = Fixture.Create<BlastActivitySocial>();
            var propertyInfo  = blastActivitySocial.GetType().GetProperty(propertyNameURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastActivitySocial) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivitySocial_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastActivitySocial());
        }

        #endregion

        #region General Constructor : Class (BlastActivitySocial) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivitySocial_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastActivitySocial = Fixture.CreateMany<BlastActivitySocial>(2).ToList();
            var firstBlastActivitySocial = instancesOfBlastActivitySocial.FirstOrDefault();
            var lastBlastActivitySocial = instancesOfBlastActivitySocial.Last();

            // Act, Assert
            firstBlastActivitySocial.ShouldNotBeNull();
            lastBlastActivitySocial.ShouldNotBeNull();
            firstBlastActivitySocial.ShouldNotBeSameAs(lastBlastActivitySocial);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivitySocial_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastActivitySocial = new BlastActivitySocial();
            var secondBlastActivitySocial = new BlastActivitySocial();
            var thirdBlastActivitySocial = new BlastActivitySocial();
            var fourthBlastActivitySocial = new BlastActivitySocial();
            var fifthBlastActivitySocial = new BlastActivitySocial();
            var sixthBlastActivitySocial = new BlastActivitySocial();

            // Act, Assert
            firstBlastActivitySocial.ShouldNotBeNull();
            secondBlastActivitySocial.ShouldNotBeNull();
            thirdBlastActivitySocial.ShouldNotBeNull();
            fourthBlastActivitySocial.ShouldNotBeNull();
            fifthBlastActivitySocial.ShouldNotBeNull();
            sixthBlastActivitySocial.ShouldNotBeNull();
            firstBlastActivitySocial.ShouldNotBeSameAs(secondBlastActivitySocial);
            thirdBlastActivitySocial.ShouldNotBeSameAs(firstBlastActivitySocial);
            fourthBlastActivitySocial.ShouldNotBeSameAs(firstBlastActivitySocial);
            fifthBlastActivitySocial.ShouldNotBeSameAs(firstBlastActivitySocial);
            sixthBlastActivitySocial.ShouldNotBeSameAs(firstBlastActivitySocial);
            sixthBlastActivitySocial.ShouldNotBeSameAs(fourthBlastActivitySocial);
        }

        #endregion

        #region General Constructor : Class (BlastActivitySocial) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivitySocial_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var socialActivityCode = string.Empty;
            var uRL = string.Empty;
            var socialMedia = string.Empty;

            // Act
            var blastActivitySocial = new BlastActivitySocial();

            // Assert
            blastActivitySocial.SocialID.ShouldBeNull();
            blastActivitySocial.BlastID.ShouldBeNull();
            blastActivitySocial.EmailID.ShouldBeNull();
            blastActivitySocial.RefEmailID.ShouldBeNull();
            blastActivitySocial.SocialActivityCodeID.ShouldBeNull();
            blastActivitySocial.SocialActivityCode.ShouldBe(socialActivityCode);
            blastActivitySocial.ActionDate.ShouldBeNull();
            blastActivitySocial.URL.ShouldBe(uRL);
            blastActivitySocial.SocialMediaID.ShouldBeNull();
            blastActivitySocial.SocialMedia.ShouldBe(socialMedia);
        }

        #endregion

        #endregion

        #endregion
    }
}
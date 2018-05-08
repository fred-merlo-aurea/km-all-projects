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
    public class SocialSummaryTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (SocialSummary) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var socialSummary = Fixture.Create<SocialSummary>();
            var id = Fixture.Create<int?>();
            var reportImagePath = Fixture.Create<string>();
            var share = Fixture.Create<int?>();
            var click = Fixture.Create<int?>();
            var isBlastGroup = Fixture.Create<bool?>();
            var reportPath = Fixture.Create<string>();
            var socialMediaId = Fixture.Create<int>();

            // Act
            socialSummary.ID = id;
            socialSummary.ReportImagePath = reportImagePath;
            socialSummary.Share = share;
            socialSummary.Click = click;
            socialSummary.IsBlastGroup = isBlastGroup;
            socialSummary.ReportPath = reportPath;
            socialSummary.SocialMediaID = socialMediaId;

            // Assert
            socialSummary.ID.ShouldBe(id);
            socialSummary.ReportImagePath.ShouldBe(reportImagePath);
            socialSummary.Share.ShouldBe(share);
            socialSummary.Click.ShouldBe(click);
            socialSummary.IsBlastGroup.ShouldBe(isBlastGroup);
            socialSummary.ReportPath.ShouldBe(reportPath);
            socialSummary.SocialMediaID.ShouldBe(socialMediaId);
        }

        #endregion

        #region General Getters/Setters : Class (SocialSummary) => Property (Click) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_Click_Property_Data_Without_Null_Test()
        {
            // Arrange
            var socialSummary = Fixture.Create<SocialSummary>();
            var random = Fixture.Create<int>();

            // Act , Set
            socialSummary.Click = random;

            // Assert
            socialSummary.Click.ShouldBe(random);
            socialSummary.Click.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_Click_Property_Only_Null_Data_Test()
        {
            // Arrange
            var socialSummary = Fixture.Create<SocialSummary>();    

            // Act , Set
            socialSummary.Click = null;

            // Assert
            socialSummary.Click.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_Click_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameClick = "Click";
            var socialSummary = Fixture.Create<SocialSummary>();
            var propertyInfo = socialSummary.GetType().GetProperty(propertyNameClick);

            // Act , Set
            propertyInfo.SetValue(socialSummary, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            socialSummary.Click.ShouldBeNull();
            socialSummary.Click.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (SocialSummary) => Property (Click) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_Class_Invalid_Property_ClickNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClick = "ClickNotPresent";
            var socialSummary  = Fixture.Create<SocialSummary>();

            // Act , Assert
            Should.NotThrow(() => socialSummary.GetType().GetProperty(propertyNameClick));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_Click_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClick = "Click";
            var socialSummary  = Fixture.Create<SocialSummary>();
            var propertyInfo  = socialSummary.GetType().GetProperty(propertyNameClick);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SocialSummary) => Property (ID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_ID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var socialSummary = Fixture.Create<SocialSummary>();
            var random = Fixture.Create<int>();

            // Act , Set
            socialSummary.ID = random;

            // Assert
            socialSummary.ID.ShouldBe(random);
            socialSummary.ID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_ID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var socialSummary = Fixture.Create<SocialSummary>();    

            // Act , Set
            socialSummary.ID = null;

            // Assert
            socialSummary.ID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_ID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameID = "ID";
            var socialSummary = Fixture.Create<SocialSummary>();
            var propertyInfo = socialSummary.GetType().GetProperty(propertyNameID);

            // Act , Set
            propertyInfo.SetValue(socialSummary, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            socialSummary.ID.ShouldBeNull();
            socialSummary.ID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (SocialSummary) => Property (ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_Class_Invalid_Property_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameID = "IDNotPresent";
            var socialSummary  = Fixture.Create<SocialSummary>();

            // Act , Assert
            Should.NotThrow(() => socialSummary.GetType().GetProperty(propertyNameID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameID = "ID";
            var socialSummary  = Fixture.Create<SocialSummary>();
            var propertyInfo  = socialSummary.GetType().GetProperty(propertyNameID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SocialSummary) => Property (IsBlastGroup) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_IsBlastGroup_Property_Data_Without_Null_Test()
        {
            // Arrange
            var socialSummary = Fixture.Create<SocialSummary>();
            var random = Fixture.Create<bool>();

            // Act , Set
            socialSummary.IsBlastGroup = random;

            // Assert
            socialSummary.IsBlastGroup.ShouldBe(random);
            socialSummary.IsBlastGroup.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_IsBlastGroup_Property_Only_Null_Data_Test()
        {
            // Arrange
            var socialSummary = Fixture.Create<SocialSummary>();    

            // Act , Set
            socialSummary.IsBlastGroup = null;

            // Assert
            socialSummary.IsBlastGroup.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_IsBlastGroup_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsBlastGroup = "IsBlastGroup";
            var socialSummary = Fixture.Create<SocialSummary>();
            var propertyInfo = socialSummary.GetType().GetProperty(propertyNameIsBlastGroup);

            // Act , Set
            propertyInfo.SetValue(socialSummary, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            socialSummary.IsBlastGroup.ShouldBeNull();
            socialSummary.IsBlastGroup.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (SocialSummary) => Property (IsBlastGroup) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_Class_Invalid_Property_IsBlastGroupNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsBlastGroup = "IsBlastGroupNotPresent";
            var socialSummary  = Fixture.Create<SocialSummary>();

            // Act , Assert
            Should.NotThrow(() => socialSummary.GetType().GetProperty(propertyNameIsBlastGroup));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_IsBlastGroup_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsBlastGroup = "IsBlastGroup";
            var socialSummary  = Fixture.Create<SocialSummary>();
            var propertyInfo  = socialSummary.GetType().GetProperty(propertyNameIsBlastGroup);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SocialSummary) => Property (ReportImagePath) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_ReportImagePath_Property_String_Type_Verify_Test()
        {
            // Arrange
            var socialSummary = Fixture.Create<SocialSummary>();
            socialSummary.ReportImagePath = Fixture.Create<string>();
            var stringType = socialSummary.ReportImagePath.GetType();

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

        #region General Getters/Setters : Class (SocialSummary) => Property (ReportImagePath) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_Class_Invalid_Property_ReportImagePathNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReportImagePath = "ReportImagePathNotPresent";
            var socialSummary  = Fixture.Create<SocialSummary>();

            // Act , Assert
            Should.NotThrow(() => socialSummary.GetType().GetProperty(propertyNameReportImagePath));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_ReportImagePath_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReportImagePath = "ReportImagePath";
            var socialSummary  = Fixture.Create<SocialSummary>();
            var propertyInfo  = socialSummary.GetType().GetProperty(propertyNameReportImagePath);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SocialSummary) => Property (ReportPath) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_ReportPath_Property_String_Type_Verify_Test()
        {
            // Arrange
            var socialSummary = Fixture.Create<SocialSummary>();
            socialSummary.ReportPath = Fixture.Create<string>();
            var stringType = socialSummary.ReportPath.GetType();

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

        #region General Getters/Setters : Class (SocialSummary) => Property (ReportPath) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_Class_Invalid_Property_ReportPathNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReportPath = "ReportPathNotPresent";
            var socialSummary  = Fixture.Create<SocialSummary>();

            // Act , Assert
            Should.NotThrow(() => socialSummary.GetType().GetProperty(propertyNameReportPath));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_ReportPath_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReportPath = "ReportPath";
            var socialSummary  = Fixture.Create<SocialSummary>();
            var propertyInfo  = socialSummary.GetType().GetProperty(propertyNameReportPath);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SocialSummary) => Property (Share) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_Share_Property_Data_Without_Null_Test()
        {
            // Arrange
            var socialSummary = Fixture.Create<SocialSummary>();
            var random = Fixture.Create<int>();

            // Act , Set
            socialSummary.Share = random;

            // Assert
            socialSummary.Share.ShouldBe(random);
            socialSummary.Share.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_Share_Property_Only_Null_Data_Test()
        {
            // Arrange
            var socialSummary = Fixture.Create<SocialSummary>();    

            // Act , Set
            socialSummary.Share = null;

            // Assert
            socialSummary.Share.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_Share_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameShare = "Share";
            var socialSummary = Fixture.Create<SocialSummary>();
            var propertyInfo = socialSummary.GetType().GetProperty(propertyNameShare);

            // Act , Set
            propertyInfo.SetValue(socialSummary, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            socialSummary.Share.ShouldBeNull();
            socialSummary.Share.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (SocialSummary) => Property (Share) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_Class_Invalid_Property_ShareNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameShare = "ShareNotPresent";
            var socialSummary  = Fixture.Create<SocialSummary>();

            // Act , Assert
            Should.NotThrow(() => socialSummary.GetType().GetProperty(propertyNameShare));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_Share_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameShare = "Share";
            var socialSummary  = Fixture.Create<SocialSummary>();
            var propertyInfo  = socialSummary.GetType().GetProperty(propertyNameShare);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SocialSummary) => Property (SocialMediaID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_SocialMediaID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var socialSummary = Fixture.Create<SocialSummary>();
            socialSummary.SocialMediaID = Fixture.Create<int>();
            var intType = socialSummary.SocialMediaID.GetType();

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

        #region General Getters/Setters : Class (SocialSummary) => Property (SocialMediaID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_Class_Invalid_Property_SocialMediaIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSocialMediaID = "SocialMediaIDNotPresent";
            var socialSummary  = Fixture.Create<SocialSummary>();

            // Act , Assert
            Should.NotThrow(() => socialSummary.GetType().GetProperty(propertyNameSocialMediaID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialSummary_SocialMediaID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSocialMediaID = "SocialMediaID";
            var socialSummary  = Fixture.Create<SocialSummary>();
            var propertyInfo  = socialSummary.GetType().GetProperty(propertyNameSocialMediaID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (SocialSummary) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SocialSummary_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new SocialSummary());
        }

        #endregion

        #region General Constructor : Class (SocialSummary) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SocialSummary_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfSocialSummary = Fixture.CreateMany<SocialSummary>(2).ToList();
            var firstSocialSummary = instancesOfSocialSummary.FirstOrDefault();
            var lastSocialSummary = instancesOfSocialSummary.Last();

            // Act, Assert
            firstSocialSummary.ShouldNotBeNull();
            lastSocialSummary.ShouldNotBeNull();
            firstSocialSummary.ShouldNotBeSameAs(lastSocialSummary);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SocialSummary_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstSocialSummary = new SocialSummary();
            var secondSocialSummary = new SocialSummary();
            var thirdSocialSummary = new SocialSummary();
            var fourthSocialSummary = new SocialSummary();
            var fifthSocialSummary = new SocialSummary();
            var sixthSocialSummary = new SocialSummary();

            // Act, Assert
            firstSocialSummary.ShouldNotBeNull();
            secondSocialSummary.ShouldNotBeNull();
            thirdSocialSummary.ShouldNotBeNull();
            fourthSocialSummary.ShouldNotBeNull();
            fifthSocialSummary.ShouldNotBeNull();
            sixthSocialSummary.ShouldNotBeNull();
            firstSocialSummary.ShouldNotBeSameAs(secondSocialSummary);
            thirdSocialSummary.ShouldNotBeSameAs(firstSocialSummary);
            fourthSocialSummary.ShouldNotBeSameAs(firstSocialSummary);
            fifthSocialSummary.ShouldNotBeSameAs(firstSocialSummary);
            sixthSocialSummary.ShouldNotBeSameAs(firstSocialSummary);
            sixthSocialSummary.ShouldNotBeSameAs(fourthSocialSummary);
        }

        #endregion

        #region General Constructor : Class (SocialSummary) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SocialSummary_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var reportImagePath = string.Empty;
            var reportPath = string.Empty;

            // Act
            var socialSummary = new SocialSummary();

            // Assert
            socialSummary.ReportImagePath.ShouldBe(reportImagePath);
            socialSummary.Share.ShouldBeNull();
            socialSummary.Click.ShouldBeNull();
            socialSummary.ID.ShouldBeNull();
            socialSummary.IsBlastGroup.ShouldBeNull();
            socialSummary.ReportPath.ShouldBe(reportPath);
        }

        #endregion

        #endregion

        #endregion
    }
}
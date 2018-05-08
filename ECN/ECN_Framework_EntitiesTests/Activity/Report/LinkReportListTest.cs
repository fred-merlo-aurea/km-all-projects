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
    public class LinkReportListTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (LinkReportList) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var linkReportList = Fixture.Create<LinkReportList>();
            var blastId = Fixture.Create<int>();
            var emailsubject = Fixture.Create<string>();
            var blastcategory = Fixture.Create<string>();
            var actionDate = Fixture.Create<DateTime>();
            var linkownerIndexId = Fixture.Create<int>();
            var linkownername = Fixture.Create<string>();
            var linktype = Fixture.Create<string>();
            var alias = Fixture.Create<string>();
            var link = Fixture.Create<string>();
            var actionFrom = Fixture.Create<string>();
            var clickcount = Fixture.Create<int>();
            var uclickcount = Fixture.Create<int>();
            var viewcount = Fixture.Create<int>();
            var sendcount = Fixture.Create<int>();

            // Act
            linkReportList.BlastID = blastId;
            linkReportList.emailsubject = emailsubject;
            linkReportList.blastcategory = blastcategory;
            linkReportList.ActionDate = actionDate;
            linkReportList.linkownerIndexID = linkownerIndexId;
            linkReportList.linkownername = linkownername;
            linkReportList.linktype = linktype;
            linkReportList.Alias = alias;
            linkReportList.Link = link;
            linkReportList.ActionFrom = actionFrom;
            linkReportList.clickcount = clickcount;
            linkReportList.Uclickcount = uclickcount;
            linkReportList.viewcount = viewcount;
            linkReportList.sendcount = sendcount;

            // Assert
            linkReportList.BlastID.ShouldBe(blastId);
            linkReportList.emailsubject.ShouldBe(emailsubject);
            linkReportList.blastcategory.ShouldBe(blastcategory);
            linkReportList.ActionDate.ShouldBe(actionDate);
            linkReportList.linkownerIndexID.ShouldBe(linkownerIndexId);
            linkReportList.linkownername.ShouldBe(linkownername);
            linkReportList.linktype.ShouldBe(linktype);
            linkReportList.Alias.ShouldBe(alias);
            linkReportList.Link.ShouldBe(link);
            linkReportList.ActionFrom.ShouldBe(actionFrom);
            linkReportList.clickcount.ShouldBe(clickcount);
            linkReportList.Uclickcount.ShouldBe(uclickcount);
            linkReportList.viewcount.ShouldBe(viewcount);
            linkReportList.sendcount.ShouldBe(sendcount);
        }

        #endregion

        #region General Getters/Setters : Class (LinkReportList) => Property (ActionDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_ActionDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDate";
            var linkReportList = Fixture.Create<LinkReportList>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = linkReportList.GetType().GetProperty(propertyNameActionDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(linkReportList, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LinkReportList) => Property (ActionDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Class_Invalid_Property_ActionDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDateNotPresent";
            var linkReportList  = Fixture.Create<LinkReportList>();

            // Act , Assert
            Should.NotThrow(() => linkReportList.GetType().GetProperty(propertyNameActionDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_ActionDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDate";
            var linkReportList  = Fixture.Create<LinkReportList>();
            var propertyInfo  = linkReportList.GetType().GetProperty(propertyNameActionDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkReportList) => Property (ActionFrom) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_ActionFrom_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkReportList = Fixture.Create<LinkReportList>();
            linkReportList.ActionFrom = Fixture.Create<string>();
            var stringType = linkReportList.ActionFrom.GetType();

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

        #region General Getters/Setters : Class (LinkReportList) => Property (ActionFrom) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Class_Invalid_Property_ActionFromNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionFrom = "ActionFromNotPresent";
            var linkReportList  = Fixture.Create<LinkReportList>();

            // Act , Assert
            Should.NotThrow(() => linkReportList.GetType().GetProperty(propertyNameActionFrom));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_ActionFrom_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionFrom = "ActionFrom";
            var linkReportList  = Fixture.Create<LinkReportList>();
            var propertyInfo  = linkReportList.GetType().GetProperty(propertyNameActionFrom);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkReportList) => Property (Alias) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Alias_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkReportList = Fixture.Create<LinkReportList>();
            linkReportList.Alias = Fixture.Create<string>();
            var stringType = linkReportList.Alias.GetType();

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

        #region General Getters/Setters : Class (LinkReportList) => Property (Alias) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Class_Invalid_Property_AliasNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAlias = "AliasNotPresent";
            var linkReportList  = Fixture.Create<LinkReportList>();

            // Act , Assert
            Should.NotThrow(() => linkReportList.GetType().GetProperty(propertyNameAlias));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Alias_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAlias = "Alias";
            var linkReportList  = Fixture.Create<LinkReportList>();
            var propertyInfo  = linkReportList.GetType().GetProperty(propertyNameAlias);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkReportList) => Property (blastcategory) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_blastcategory_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkReportList = Fixture.Create<LinkReportList>();
            linkReportList.blastcategory = Fixture.Create<string>();
            var stringType = linkReportList.blastcategory.GetType();

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

        #region General Getters/Setters : Class (LinkReportList) => Property (blastcategory) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Class_Invalid_Property_blastcategoryNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameblastcategory = "blastcategoryNotPresent";
            var linkReportList  = Fixture.Create<LinkReportList>();

            // Act , Assert
            Should.NotThrow(() => linkReportList.GetType().GetProperty(propertyNameblastcategory));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_blastcategory_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameblastcategory = "blastcategory";
            var linkReportList  = Fixture.Create<LinkReportList>();
            var propertyInfo  = linkReportList.GetType().GetProperty(propertyNameblastcategory);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkReportList) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkReportList = Fixture.Create<LinkReportList>();
            linkReportList.BlastID = Fixture.Create<int>();
            var intType = linkReportList.BlastID.GetType();

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

        #region General Getters/Setters : Class (LinkReportList) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var linkReportList  = Fixture.Create<LinkReportList>();

            // Act , Assert
            Should.NotThrow(() => linkReportList.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var linkReportList  = Fixture.Create<LinkReportList>();
            var propertyInfo  = linkReportList.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkReportList) => Property (clickcount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_clickcount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkReportList = Fixture.Create<LinkReportList>();
            linkReportList.clickcount = Fixture.Create<int>();
            var intType = linkReportList.clickcount.GetType();

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

        #region General Getters/Setters : Class (LinkReportList) => Property (clickcount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Class_Invalid_Property_clickcountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameclickcount = "clickcountNotPresent";
            var linkReportList  = Fixture.Create<LinkReportList>();

            // Act , Assert
            Should.NotThrow(() => linkReportList.GetType().GetProperty(propertyNameclickcount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_clickcount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameclickcount = "clickcount";
            var linkReportList  = Fixture.Create<LinkReportList>();
            var propertyInfo  = linkReportList.GetType().GetProperty(propertyNameclickcount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkReportList) => Property (emailsubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_emailsubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkReportList = Fixture.Create<LinkReportList>();
            linkReportList.emailsubject = Fixture.Create<string>();
            var stringType = linkReportList.emailsubject.GetType();

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

        #region General Getters/Setters : Class (LinkReportList) => Property (emailsubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Class_Invalid_Property_emailsubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameemailsubject = "emailsubjectNotPresent";
            var linkReportList  = Fixture.Create<LinkReportList>();

            // Act , Assert
            Should.NotThrow(() => linkReportList.GetType().GetProperty(propertyNameemailsubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_emailsubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameemailsubject = "emailsubject";
            var linkReportList  = Fixture.Create<LinkReportList>();
            var propertyInfo  = linkReportList.GetType().GetProperty(propertyNameemailsubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkReportList) => Property (Link) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Link_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkReportList = Fixture.Create<LinkReportList>();
            linkReportList.Link = Fixture.Create<string>();
            var stringType = linkReportList.Link.GetType();

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

        #region General Getters/Setters : Class (LinkReportList) => Property (Link) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Class_Invalid_Property_LinkNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLink = "LinkNotPresent";
            var linkReportList  = Fixture.Create<LinkReportList>();

            // Act , Assert
            Should.NotThrow(() => linkReportList.GetType().GetProperty(propertyNameLink));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Link_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLink = "Link";
            var linkReportList  = Fixture.Create<LinkReportList>();
            var propertyInfo  = linkReportList.GetType().GetProperty(propertyNameLink);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkReportList) => Property (linkownerIndexID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_linkownerIndexID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkReportList = Fixture.Create<LinkReportList>();
            linkReportList.linkownerIndexID = Fixture.Create<int>();
            var intType = linkReportList.linkownerIndexID.GetType();

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

        #region General Getters/Setters : Class (LinkReportList) => Property (linkownerIndexID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Class_Invalid_Property_linkownerIndexIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamelinkownerIndexID = "linkownerIndexIDNotPresent";
            var linkReportList  = Fixture.Create<LinkReportList>();

            // Act , Assert
            Should.NotThrow(() => linkReportList.GetType().GetProperty(propertyNamelinkownerIndexID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_linkownerIndexID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamelinkownerIndexID = "linkownerIndexID";
            var linkReportList  = Fixture.Create<LinkReportList>();
            var propertyInfo  = linkReportList.GetType().GetProperty(propertyNamelinkownerIndexID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkReportList) => Property (linkownername) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_linkownername_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkReportList = Fixture.Create<LinkReportList>();
            linkReportList.linkownername = Fixture.Create<string>();
            var stringType = linkReportList.linkownername.GetType();

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

        #region General Getters/Setters : Class (LinkReportList) => Property (linkownername) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Class_Invalid_Property_linkownernameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamelinkownername = "linkownernameNotPresent";
            var linkReportList  = Fixture.Create<LinkReportList>();

            // Act , Assert
            Should.NotThrow(() => linkReportList.GetType().GetProperty(propertyNamelinkownername));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_linkownername_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamelinkownername = "linkownername";
            var linkReportList  = Fixture.Create<LinkReportList>();
            var propertyInfo  = linkReportList.GetType().GetProperty(propertyNamelinkownername);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkReportList) => Property (linktype) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_linktype_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkReportList = Fixture.Create<LinkReportList>();
            linkReportList.linktype = Fixture.Create<string>();
            var stringType = linkReportList.linktype.GetType();

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

        #region General Getters/Setters : Class (LinkReportList) => Property (linktype) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Class_Invalid_Property_linktypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamelinktype = "linktypeNotPresent";
            var linkReportList  = Fixture.Create<LinkReportList>();

            // Act , Assert
            Should.NotThrow(() => linkReportList.GetType().GetProperty(propertyNamelinktype));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_linktype_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamelinktype = "linktype";
            var linkReportList  = Fixture.Create<LinkReportList>();
            var propertyInfo  = linkReportList.GetType().GetProperty(propertyNamelinktype);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkReportList) => Property (sendcount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_sendcount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkReportList = Fixture.Create<LinkReportList>();
            linkReportList.sendcount = Fixture.Create<int>();
            var intType = linkReportList.sendcount.GetType();

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

        #region General Getters/Setters : Class (LinkReportList) => Property (sendcount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Class_Invalid_Property_sendcountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamesendcount = "sendcountNotPresent";
            var linkReportList  = Fixture.Create<LinkReportList>();

            // Act , Assert
            Should.NotThrow(() => linkReportList.GetType().GetProperty(propertyNamesendcount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_sendcount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamesendcount = "sendcount";
            var linkReportList  = Fixture.Create<LinkReportList>();
            var propertyInfo  = linkReportList.GetType().GetProperty(propertyNamesendcount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkReportList) => Property (Uclickcount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Uclickcount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkReportList = Fixture.Create<LinkReportList>();
            linkReportList.Uclickcount = Fixture.Create<int>();
            var intType = linkReportList.Uclickcount.GetType();

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

        #region General Getters/Setters : Class (LinkReportList) => Property (Uclickcount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Class_Invalid_Property_UclickcountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUclickcount = "UclickcountNotPresent";
            var linkReportList  = Fixture.Create<LinkReportList>();

            // Act , Assert
            Should.NotThrow(() => linkReportList.GetType().GetProperty(propertyNameUclickcount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Uclickcount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUclickcount = "Uclickcount";
            var linkReportList  = Fixture.Create<LinkReportList>();
            var propertyInfo  = linkReportList.GetType().GetProperty(propertyNameUclickcount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkReportList) => Property (viewcount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_viewcount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkReportList = Fixture.Create<LinkReportList>();
            linkReportList.viewcount = Fixture.Create<int>();
            var intType = linkReportList.viewcount.GetType();

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

        #region General Getters/Setters : Class (LinkReportList) => Property (viewcount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_Class_Invalid_Property_viewcountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameviewcount = "viewcountNotPresent";
            var linkReportList  = Fixture.Create<LinkReportList>();

            // Act , Assert
            Should.NotThrow(() => linkReportList.GetType().GetProperty(propertyNameviewcount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkReportList_viewcount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameviewcount = "viewcount";
            var linkReportList  = Fixture.Create<LinkReportList>();
            var propertyInfo  = linkReportList.GetType().GetProperty(propertyNameviewcount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (LinkReportList) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkReportList_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new LinkReportList());
        }

        #endregion

        #region General Constructor : Class (LinkReportList) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkReportList_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfLinkReportList = Fixture.CreateMany<LinkReportList>(2).ToList();
            var firstLinkReportList = instancesOfLinkReportList.FirstOrDefault();
            var lastLinkReportList = instancesOfLinkReportList.Last();

            // Act, Assert
            firstLinkReportList.ShouldNotBeNull();
            lastLinkReportList.ShouldNotBeNull();
            firstLinkReportList.ShouldNotBeSameAs(lastLinkReportList);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkReportList_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstLinkReportList = new LinkReportList();
            var secondLinkReportList = new LinkReportList();
            var thirdLinkReportList = new LinkReportList();
            var fourthLinkReportList = new LinkReportList();
            var fifthLinkReportList = new LinkReportList();
            var sixthLinkReportList = new LinkReportList();

            // Act, Assert
            firstLinkReportList.ShouldNotBeNull();
            secondLinkReportList.ShouldNotBeNull();
            thirdLinkReportList.ShouldNotBeNull();
            fourthLinkReportList.ShouldNotBeNull();
            fifthLinkReportList.ShouldNotBeNull();
            sixthLinkReportList.ShouldNotBeNull();
            firstLinkReportList.ShouldNotBeSameAs(secondLinkReportList);
            thirdLinkReportList.ShouldNotBeSameAs(firstLinkReportList);
            fourthLinkReportList.ShouldNotBeSameAs(firstLinkReportList);
            fifthLinkReportList.ShouldNotBeSameAs(firstLinkReportList);
            sixthLinkReportList.ShouldNotBeSameAs(firstLinkReportList);
            sixthLinkReportList.ShouldNotBeSameAs(fourthLinkReportList);
        }

        #endregion

        #endregion

        #endregion
    }
}
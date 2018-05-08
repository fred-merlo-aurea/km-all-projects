using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class CampaignItemBlastTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (CampaignItemBlast) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            var campaignItemBlastId = Fixture.Create<int>();
            var campaignItemId = Fixture.Create<int?>();
            var emailSubject = Fixture.Create<string>();
            var dynamicFromName = Fixture.Create<string>();
            var dynamicFromEmail = Fixture.Create<string>();
            var dynamicReplyTo = Fixture.Create<string>();
            var layoutId = Fixture.Create<int?>();
            var groupId = Fixture.Create<int?>();
            var socialMediaId = Fixture.Create<int?>();
            var addOptOuts_to_MS = Fixture.Create<bool?>();
            var blastId = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var customerId = Fixture.Create<int?>();
            var emailFrom = Fixture.Create<string>();
            var replyTo = Fixture.Create<string>();
            var fromName = Fixture.Create<string>();
            var refBlastList = Fixture.Create<List<CampaignItemBlastRefBlast>>();
            var filters = Fixture.Create<List<CampaignItemBlastFilter>>();

            // Act
            campaignItemBlast.CampaignItemBlastID = campaignItemBlastId;
            campaignItemBlast.CampaignItemID = campaignItemId;
            campaignItemBlast.EmailSubject = emailSubject;
            campaignItemBlast.DynamicFromName = dynamicFromName;
            campaignItemBlast.DynamicFromEmail = dynamicFromEmail;
            campaignItemBlast.DynamicReplyTo = dynamicReplyTo;
            campaignItemBlast.LayoutID = layoutId;
            campaignItemBlast.GroupID = groupId;
            campaignItemBlast.SocialMediaID = socialMediaId;
            campaignItemBlast.AddOptOuts_to_MS = addOptOuts_to_MS;
            campaignItemBlast.BlastID = blastId;
            campaignItemBlast.CreatedUserID = createdUserId;
            campaignItemBlast.UpdatedUserID = updatedUserId;
            campaignItemBlast.IsDeleted = isDeleted;
            campaignItemBlast.CustomerID = customerId;
            campaignItemBlast.EmailFrom = emailFrom;
            campaignItemBlast.ReplyTo = replyTo;
            campaignItemBlast.FromName = fromName;
            campaignItemBlast.RefBlastList = refBlastList;
            campaignItemBlast.Filters = filters;

            // Assert
            campaignItemBlast.CampaignItemBlastID.ShouldBe(campaignItemBlastId);
            campaignItemBlast.CampaignItemID.ShouldBe(campaignItemId);
            campaignItemBlast.EmailSubject.ShouldBe(emailSubject);
            campaignItemBlast.DynamicFromName.ShouldBe(dynamicFromName);
            campaignItemBlast.DynamicFromEmail.ShouldBe(dynamicFromEmail);
            campaignItemBlast.DynamicReplyTo.ShouldBe(dynamicReplyTo);
            campaignItemBlast.LayoutID.ShouldBe(layoutId);
            campaignItemBlast.GroupID.ShouldBe(groupId);
            campaignItemBlast.SocialMediaID.ShouldBe(socialMediaId);
            campaignItemBlast.AddOptOuts_to_MS.ShouldBe(addOptOuts_to_MS);
            campaignItemBlast.BlastID.ShouldBe(blastId);
            campaignItemBlast.CreatedUserID.ShouldBe(createdUserId);
            campaignItemBlast.CreatedDate.ShouldBeNull();
            campaignItemBlast.UpdatedUserID.ShouldBe(updatedUserId);
            campaignItemBlast.UpdatedDate.ShouldBeNull();
            campaignItemBlast.IsDeleted.ShouldBe(isDeleted);
            campaignItemBlast.CustomerID.ShouldBe(customerId);
            campaignItemBlast.EmailFrom.ShouldBe(emailFrom);
            campaignItemBlast.ReplyTo.ShouldBe(replyTo);
            campaignItemBlast.FromName.ShouldBe(fromName);
            campaignItemBlast.Blast.ShouldBeNull();
            campaignItemBlast.RefBlastList.ShouldBe(refBlastList);
            campaignItemBlast.Filters.ShouldBe(filters);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (AddOptOuts_to_MS) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_AddOptOuts_to_MS_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            var random = Fixture.Create<bool>();

            // Act , Set
            campaignItemBlast.AddOptOuts_to_MS = random;

            // Assert
            campaignItemBlast.AddOptOuts_to_MS.ShouldBe(random);
            campaignItemBlast.AddOptOuts_to_MS.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_AddOptOuts_to_MS_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();    

            // Act , Set
            campaignItemBlast.AddOptOuts_to_MS = null;

            // Assert
            campaignItemBlast.AddOptOuts_to_MS.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_AddOptOuts_to_MS_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameAddOptOuts_to_MS = "AddOptOuts_to_MS";
            var campaignItemBlast = new CampaignItemBlast();
            var propertyInfo = campaignItemBlast.GetType().GetProperty(propertyNameAddOptOuts_to_MS);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlast, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlast.AddOptOuts_to_MS.ShouldBeNull();
            campaignItemBlast.AddOptOuts_to_MS.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (AddOptOuts_to_MS) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_AddOptOuts_to_MSNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAddOptOuts_to_MS = "AddOptOuts_to_MSNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameAddOptOuts_to_MS));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_AddOptOuts_to_MS_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAddOptOuts_to_MS = "AddOptOuts_to_MS";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameAddOptOuts_to_MS);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (Blast) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Blast_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameBlast = "Blast";
            var campaignItemBlast = new CampaignItemBlast();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignItemBlast.GetType().GetProperty(propertyNameBlast);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignItemBlast, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (Blast) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_BlastNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlast = "BlastNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameBlast));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Blast_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlast = "Blast";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameBlast);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (BlastID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_BlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlast.BlastID = random;

            // Assert
            campaignItemBlast.BlastID.ShouldBe(random);
            campaignItemBlast.BlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_BlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();    

            // Act , Set
            campaignItemBlast.BlastID = null;

            // Assert
            campaignItemBlast.BlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_BlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastID = "BlastID";
            var campaignItemBlast = new CampaignItemBlast();
            var propertyInfo = campaignItemBlast.GetType().GetProperty(propertyNameBlastID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlast, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlast.BlastID.ShouldBeNull();
            campaignItemBlast.BlastID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (CampaignItemBlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_CampaignItemBlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            campaignItemBlast.CampaignItemBlastID = Fixture.Create<int>();
            var intType = campaignItemBlast.CampaignItemBlastID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (CampaignItemBlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_CampaignItemBlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemBlastID = "CampaignItemBlastIDNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameCampaignItemBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_CampaignItemBlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemBlastID = "CampaignItemBlastID";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameCampaignItemBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (CampaignItemID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_CampaignItemID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlast.CampaignItemID = random;

            // Assert
            campaignItemBlast.CampaignItemID.ShouldBe(random);
            campaignItemBlast.CampaignItemID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_CampaignItemID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();    

            // Act , Set
            campaignItemBlast.CampaignItemID = null;

            // Assert
            campaignItemBlast.CampaignItemID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_CampaignItemID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCampaignItemID = "CampaignItemID";
            var campaignItemBlast = new CampaignItemBlast();
            var propertyInfo = campaignItemBlast.GetType().GetProperty(propertyNameCampaignItemID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlast, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlast.CampaignItemID.ShouldBeNull();
            campaignItemBlast.CampaignItemID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (CampaignItemID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_CampaignItemIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemID = "CampaignItemIDNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameCampaignItemID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_CampaignItemID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemID = "CampaignItemID";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameCampaignItemID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var campaignItemBlast = new CampaignItemBlast();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignItemBlast.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignItemBlast, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlast.CreatedUserID = random;

            // Assert
            campaignItemBlast.CreatedUserID.ShouldBe(random);
            campaignItemBlast.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();    

            // Act , Set
            campaignItemBlast.CreatedUserID = null;

            // Assert
            campaignItemBlast.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var campaignItemBlast = new CampaignItemBlast();
            var propertyInfo = campaignItemBlast.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlast, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlast.CreatedUserID.ShouldBeNull();
            campaignItemBlast.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlast.CustomerID = random;

            // Assert
            campaignItemBlast.CustomerID.ShouldBe(random);
            campaignItemBlast.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();    

            // Act , Set
            campaignItemBlast.CustomerID = null;

            // Assert
            campaignItemBlast.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var campaignItemBlast = new CampaignItemBlast();
            var propertyInfo = campaignItemBlast.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlast, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlast.CustomerID.ShouldBeNull();
            campaignItemBlast.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (DynamicFromEmail) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_DynamicFromEmail_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            campaignItemBlast.DynamicFromEmail = Fixture.Create<string>();
            var stringType = campaignItemBlast.DynamicFromEmail.GetType();

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

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (DynamicFromEmail) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_DynamicFromEmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDynamicFromEmail = "DynamicFromEmailNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameDynamicFromEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_DynamicFromEmail_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDynamicFromEmail = "DynamicFromEmail";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameDynamicFromEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (DynamicFromName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_DynamicFromName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            campaignItemBlast.DynamicFromName = Fixture.Create<string>();
            var stringType = campaignItemBlast.DynamicFromName.GetType();

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

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (DynamicFromName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_DynamicFromNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDynamicFromName = "DynamicFromNameNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameDynamicFromName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_DynamicFromName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDynamicFromName = "DynamicFromName";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameDynamicFromName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (DynamicReplyTo) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_DynamicReplyTo_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            campaignItemBlast.DynamicReplyTo = Fixture.Create<string>();
            var stringType = campaignItemBlast.DynamicReplyTo.GetType();

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

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (DynamicReplyTo) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_DynamicReplyToNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDynamicReplyTo = "DynamicReplyToNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameDynamicReplyTo));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_DynamicReplyTo_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDynamicReplyTo = "DynamicReplyTo";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameDynamicReplyTo);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (EmailFrom) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_EmailFrom_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            campaignItemBlast.EmailFrom = Fixture.Create<string>();
            var stringType = campaignItemBlast.EmailFrom.GetType();

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

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (EmailFrom) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_EmailFromNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailFrom = "EmailFromNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameEmailFrom));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_EmailFrom_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailFrom = "EmailFrom";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameEmailFrom);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            campaignItemBlast.EmailSubject = Fixture.Create<string>();
            var stringType = campaignItemBlast.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (Filters) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_FiltersNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilters = "FiltersNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameFilters));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Filters_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilters = "Filters";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameFilters);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (FromName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_FromName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            campaignItemBlast.FromName = Fixture.Create<string>();
            var stringType = campaignItemBlast.FromName.GetType();

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

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (FromName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_FromNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFromName = "FromNameNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameFromName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_FromName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFromName = "FromName";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameFromName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (GroupID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_GroupID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlast.GroupID = random;

            // Assert
            campaignItemBlast.GroupID.ShouldBe(random);
            campaignItemBlast.GroupID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_GroupID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();    

            // Act , Set
            campaignItemBlast.GroupID = null;

            // Assert
            campaignItemBlast.GroupID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_GroupID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameGroupID = "GroupID";
            var campaignItemBlast = new CampaignItemBlast();
            var propertyInfo = campaignItemBlast.GetType().GetProperty(propertyNameGroupID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlast, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlast.GroupID.ShouldBeNull();
            campaignItemBlast.GroupID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (GroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_GroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupIDNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_GroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupID";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            var random = Fixture.Create<bool>();

            // Act , Set
            campaignItemBlast.IsDeleted = random;

            // Assert
            campaignItemBlast.IsDeleted.ShouldBe(random);
            campaignItemBlast.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();    

            // Act , Set
            campaignItemBlast.IsDeleted = null;

            // Assert
            campaignItemBlast.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var campaignItemBlast = new CampaignItemBlast();
            var propertyInfo = campaignItemBlast.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlast, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlast.IsDeleted.ShouldBeNull();
            campaignItemBlast.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (LayoutID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_LayoutID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlast.LayoutID = random;

            // Assert
            campaignItemBlast.LayoutID.ShouldBe(random);
            campaignItemBlast.LayoutID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_LayoutID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();    

            // Act , Set
            campaignItemBlast.LayoutID = null;

            // Assert
            campaignItemBlast.LayoutID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_LayoutID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameLayoutID = "LayoutID";
            var campaignItemBlast = new CampaignItemBlast();
            var propertyInfo = campaignItemBlast.GetType().GetProperty(propertyNameLayoutID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlast, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlast.LayoutID.ShouldBeNull();
            campaignItemBlast.LayoutID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (LayoutID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_LayoutIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLayoutID = "LayoutIDNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameLayoutID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_LayoutID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLayoutID = "LayoutID";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameLayoutID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (RefBlastList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_RefBlastListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRefBlastList = "RefBlastListNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameRefBlastList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_RefBlastList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRefBlastList = "RefBlastList";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameRefBlastList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (ReplyTo) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_ReplyTo_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            campaignItemBlast.ReplyTo = Fixture.Create<string>();
            var stringType = campaignItemBlast.ReplyTo.GetType();

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

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (ReplyTo) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_ReplyToNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReplyTo = "ReplyToNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameReplyTo));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_ReplyTo_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReplyTo = "ReplyTo";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameReplyTo);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (SocialMediaID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_SocialMediaID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlast.SocialMediaID = random;

            // Assert
            campaignItemBlast.SocialMediaID.ShouldBe(random);
            campaignItemBlast.SocialMediaID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_SocialMediaID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();    

            // Act , Set
            campaignItemBlast.SocialMediaID = null;

            // Assert
            campaignItemBlast.SocialMediaID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_SocialMediaID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSocialMediaID = "SocialMediaID";
            var campaignItemBlast = new CampaignItemBlast();
            var propertyInfo = campaignItemBlast.GetType().GetProperty(propertyNameSocialMediaID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlast, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlast.SocialMediaID.ShouldBeNull();
            campaignItemBlast.SocialMediaID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (SocialMediaID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_SocialMediaIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSocialMediaID = "SocialMediaIDNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameSocialMediaID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_SocialMediaID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSocialMediaID = "SocialMediaID";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameSocialMediaID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var campaignItemBlast = new CampaignItemBlast();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignItemBlast.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignItemBlast, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlast.UpdatedUserID = random;

            // Assert
            campaignItemBlast.UpdatedUserID.ShouldBe(random);
            campaignItemBlast.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlast = new CampaignItemBlast();    

            // Act , Set
            campaignItemBlast.UpdatedUserID = null;

            // Assert
            campaignItemBlast.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var campaignItemBlast = new CampaignItemBlast();
            var propertyInfo = campaignItemBlast.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlast, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlast.UpdatedUserID.ShouldBeNull();
            campaignItemBlast.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlast) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var campaignItemBlast  = new CampaignItemBlast();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlast.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlast_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var campaignItemBlast  = new CampaignItemBlast();
            var propertyInfo  = campaignItemBlast.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (CampaignItemBlast) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemBlast_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CampaignItemBlast());
        }

        #endregion

        #region General Constructor : Class (CampaignItemBlast) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemBlast_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCampaignItemBlast = new CampaignItemBlast();
            var secondCampaignItemBlast = new CampaignItemBlast();
            var thirdCampaignItemBlast = new CampaignItemBlast();
            var fourthCampaignItemBlast = new CampaignItemBlast();
            var fifthCampaignItemBlast = new CampaignItemBlast();
            var sixthCampaignItemBlast = new CampaignItemBlast();

            // Act, Assert
            firstCampaignItemBlast.ShouldNotBeNull();
            secondCampaignItemBlast.ShouldNotBeNull();
            thirdCampaignItemBlast.ShouldNotBeNull();
            fourthCampaignItemBlast.ShouldNotBeNull();
            fifthCampaignItemBlast.ShouldNotBeNull();
            sixthCampaignItemBlast.ShouldNotBeNull();
            firstCampaignItemBlast.ShouldNotBeSameAs(secondCampaignItemBlast);
            thirdCampaignItemBlast.ShouldNotBeSameAs(firstCampaignItemBlast);
            fourthCampaignItemBlast.ShouldNotBeSameAs(firstCampaignItemBlast);
            fifthCampaignItemBlast.ShouldNotBeSameAs(firstCampaignItemBlast);
            sixthCampaignItemBlast.ShouldNotBeSameAs(firstCampaignItemBlast);
            sixthCampaignItemBlast.ShouldNotBeSameAs(fourthCampaignItemBlast);
        }

        #endregion

        #region General Constructor : Class (CampaignItemBlast) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemBlast_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var campaignItemBlastId = -1;
            var emailSubject = string.Empty;
            var dynamicFromName = string.Empty;
            var dynamicFromEmail = string.Empty;
            var dynamicReplyTo = string.Empty;
            var filters = new List<CampaignItemBlastFilter>();

            // Act
            var campaignItemBlast = new CampaignItemBlast();

            // Assert
            campaignItemBlast.CampaignItemBlastID.ShouldBe(campaignItemBlastId);
            campaignItemBlast.CampaignItemID.ShouldBeNull();
            campaignItemBlast.EmailSubject.ShouldBe(emailSubject);
            campaignItemBlast.DynamicFromName.ShouldBe(dynamicFromName);
            campaignItemBlast.DynamicFromEmail.ShouldBe(dynamicFromEmail);
            campaignItemBlast.DynamicReplyTo.ShouldBe(dynamicReplyTo);
            campaignItemBlast.LayoutID.ShouldBeNull();
            campaignItemBlast.GroupID.ShouldBeNull();
            campaignItemBlast.SocialMediaID.ShouldBeNull();
            campaignItemBlast.BlastID.ShouldBeNull();
            campaignItemBlast.AddOptOuts_to_MS.ShouldBeNull();
            campaignItemBlast.CreatedUserID.ShouldBeNull();
            campaignItemBlast.CreatedDate.ShouldBeNull();
            campaignItemBlast.UpdatedUserID.ShouldBeNull();
            campaignItemBlast.UpdatedDate.ShouldBeNull();
            campaignItemBlast.IsDeleted.ShouldBeNull();
            campaignItemBlast.CustomerID.ShouldBeNull();
            campaignItemBlast.Blast.ShouldBeNull();
            campaignItemBlast.RefBlastList.ShouldBeNull();
            campaignItemBlast.Filters.ShouldBe(filters);
        }

        #endregion

        #endregion

        #endregion
    }
}
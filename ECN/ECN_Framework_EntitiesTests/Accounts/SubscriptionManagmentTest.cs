using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Shouldly;
using NUnit.Framework;
using AutoFixture;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_EntitiesTests.Accounts
{
    [TestFixture]
    public class SubscriptionManagementTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var subscriptionManagement  = new SubscriptionManagement();
            var subscriptionManagementID = Fixture.Create<int>();
            var baseChannelID = Fixture.Create<int>();
            var name = Fixture.Create<string>();
            var header = Fixture.Create<string>();
            var footer = Fixture.Create<string>();
            var emailHeader = Fixture.Create<string>();
            var emailFooter = Fixture.Create<string>();
            var adminEmail = Fixture.Create<string>();
            var mSMessage = Fixture.Create<string>();
            var includeMSGroups = Fixture.Create<bool?>();
            var createdUserID = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserID = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();
            var useReasonDropDown = Fixture.Create<bool?>();
            var reasonLabel = Fixture.Create<string>();
            var thankYouLabel = Fixture.Create<string>();
            var reasonVisible = Fixture.Create<bool?>();
            var useThankYou = Fixture.Create<bool?>();
            var useRedirect = Fixture.Create<bool?>();
            var redirectURL = Fixture.Create<string>();
            var redirectDelay = Fixture.Create<int>();

            // Act
            subscriptionManagement.SubscriptionManagementID = subscriptionManagementID;
            subscriptionManagement.BaseChannelID = baseChannelID;
            subscriptionManagement.Name = name;
            subscriptionManagement.Header = header;
            subscriptionManagement.Footer = footer;
            subscriptionManagement.EmailHeader = emailHeader;
            subscriptionManagement.EmailFooter = emailFooter;
            subscriptionManagement.AdminEmail = adminEmail;
            subscriptionManagement.MSMessage = mSMessage;
            subscriptionManagement.IncludeMSGroups = includeMSGroups;
            subscriptionManagement.CreatedUserID = createdUserID;
            subscriptionManagement.CreatedDate = createdDate;
            subscriptionManagement.UpdatedUserID = updatedUserID;
            subscriptionManagement.UpdatedDate = updatedDate;
            subscriptionManagement.IsDeleted = isDeleted;
            subscriptionManagement.UseReasonDropDown = useReasonDropDown;
            subscriptionManagement.ReasonLabel = reasonLabel;
            subscriptionManagement.ThankYouLabel = thankYouLabel;
            subscriptionManagement.ReasonVisible = reasonVisible;
            subscriptionManagement.UseThankYou = useThankYou;
            subscriptionManagement.UseRedirect = useRedirect;
            subscriptionManagement.RedirectURL = redirectURL;
            subscriptionManagement.RedirectDelay = redirectDelay;

            // Assert
            subscriptionManagement.SubscriptionManagementID.ShouldBe(subscriptionManagementID);
            subscriptionManagement.BaseChannelID.ShouldBe(baseChannelID);
            subscriptionManagement.Name.ShouldBe(name);
            subscriptionManagement.Header.ShouldBe(header);
            subscriptionManagement.Footer.ShouldBe(footer);
            subscriptionManagement.EmailHeader.ShouldBe(emailHeader);
            subscriptionManagement.EmailFooter.ShouldBe(emailFooter);
            subscriptionManagement.AdminEmail.ShouldBe(adminEmail);
            subscriptionManagement.MSMessage.ShouldBe(mSMessage);
            subscriptionManagement.IncludeMSGroups.ShouldBe(includeMSGroups);
            subscriptionManagement.CreatedUserID.ShouldBe(createdUserID);
            subscriptionManagement.CreatedDate.ShouldBe(createdDate);
            subscriptionManagement.UpdatedUserID.ShouldBe(updatedUserID);
            subscriptionManagement.UpdatedDate.ShouldBe(updatedDate);
            subscriptionManagement.IsDeleted.ShouldBe(isDeleted);
            subscriptionManagement.UseReasonDropDown.ShouldBe(useReasonDropDown);
            subscriptionManagement.ReasonLabel.ShouldBe(reasonLabel);
            subscriptionManagement.ThankYouLabel.ShouldBe(thankYouLabel);
            subscriptionManagement.ReasonVisible.ShouldBe(reasonVisible);
            subscriptionManagement.UseThankYou.ShouldBe(useThankYou);
            subscriptionManagement.UseRedirect.ShouldBe(useRedirect);
            subscriptionManagement.RedirectURL.ShouldBe(redirectURL);
            subscriptionManagement.RedirectDelay.ShouldBe(redirectDelay);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Nullable Property Test : SubscriptionManagement => IncludeMSGroups

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IncludeMSGroups_Data_Without_Null_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var random = Fixture.Create<bool>();

            // Act , Set
            subscriptionManagement.IncludeMSGroups = random;

            // Assert
            subscriptionManagement.IncludeMSGroups.ShouldBe(random);
            subscriptionManagement.IncludeMSGroups.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IncludeMSGroups_Only_Null_Data_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();    

            // Act , Set
            subscriptionManagement.IncludeMSGroups = null;

            // Assert
            subscriptionManagement.IncludeMSGroups.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IncludeMSGroups_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIncludeMSGroups = "IncludeMSGroups";
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var propertyInfo = subscriptionManagement.GetType().GetProperty(constIncludeMSGroups);

            // Act , Set
            propertyInfo.SetValue(subscriptionManagement, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            subscriptionManagement.IncludeMSGroups.ShouldBeNull();
            subscriptionManagement.IncludeMSGroups.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_IncludeMSGroups_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIncludeMSGroups = "IncludeMSGroups";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constIncludeMSGroups));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IncludeMSGroups_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constIncludeMSGroups = "IncludeMSGroups";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constIncludeMSGroups);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SubscriptionManagement => IsDeleted

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Data_Without_Null_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var random = Fixture.Create<bool>();

            // Act , Set
            subscriptionManagement.IsDeleted = random;

            // Assert
            subscriptionManagement.IsDeleted.ShouldBe(random);
            subscriptionManagement.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Only_Null_Data_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();    

            // Act , Set
            subscriptionManagement.IsDeleted = null;

            // Assert
            subscriptionManagement.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsDeleted = "IsDeleted";
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var propertyInfo = subscriptionManagement.GetType().GetProperty(constIsDeleted);

            // Act , Set
            propertyInfo.SetValue(subscriptionManagement, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            subscriptionManagement.IsDeleted.ShouldBeNull();
            subscriptionManagement.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_IsDeleted_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constIsDeleted));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SubscriptionManagement => ReasonVisible

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ReasonVisible_Data_Without_Null_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var random = Fixture.Create<bool>();

            // Act , Set
            subscriptionManagement.ReasonVisible = random;

            // Assert
            subscriptionManagement.ReasonVisible.ShouldBe(random);
            subscriptionManagement.ReasonVisible.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ReasonVisible_Only_Null_Data_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();    

            // Act , Set
            subscriptionManagement.ReasonVisible = null;

            // Assert
            subscriptionManagement.ReasonVisible.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ReasonVisible_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constReasonVisible = "ReasonVisible";
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var propertyInfo = subscriptionManagement.GetType().GetProperty(constReasonVisible);

            // Act , Set
            propertyInfo.SetValue(subscriptionManagement, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            subscriptionManagement.ReasonVisible.ShouldBeNull();
            subscriptionManagement.ReasonVisible.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_ReasonVisible_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constReasonVisible = "ReasonVisible";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constReasonVisible));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ReasonVisible_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constReasonVisible = "ReasonVisible";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constReasonVisible);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SubscriptionManagement => UseReasonDropDown

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UseReasonDropDown_Data_Without_Null_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var random = Fixture.Create<bool>();

            // Act , Set
            subscriptionManagement.UseReasonDropDown = random;

            // Assert
            subscriptionManagement.UseReasonDropDown.ShouldBe(random);
            subscriptionManagement.UseReasonDropDown.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UseReasonDropDown_Only_Null_Data_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();    

            // Act , Set
            subscriptionManagement.UseReasonDropDown = null;

            // Assert
            subscriptionManagement.UseReasonDropDown.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UseReasonDropDown_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUseReasonDropDown = "UseReasonDropDown";
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var propertyInfo = subscriptionManagement.GetType().GetProperty(constUseReasonDropDown);

            // Act , Set
            propertyInfo.SetValue(subscriptionManagement, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            subscriptionManagement.UseReasonDropDown.ShouldBeNull();
            subscriptionManagement.UseReasonDropDown.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_UseReasonDropDown_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUseReasonDropDown = "UseReasonDropDown";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constUseReasonDropDown));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UseReasonDropDown_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constUseReasonDropDown = "UseReasonDropDown";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constUseReasonDropDown);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SubscriptionManagement => UseRedirect

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UseRedirect_Data_Without_Null_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var random = Fixture.Create<bool>();

            // Act , Set
            subscriptionManagement.UseRedirect = random;

            // Assert
            subscriptionManagement.UseRedirect.ShouldBe(random);
            subscriptionManagement.UseRedirect.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UseRedirect_Only_Null_Data_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();    

            // Act , Set
            subscriptionManagement.UseRedirect = null;

            // Assert
            subscriptionManagement.UseRedirect.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UseRedirect_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUseRedirect = "UseRedirect";
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var propertyInfo = subscriptionManagement.GetType().GetProperty(constUseRedirect);

            // Act , Set
            propertyInfo.SetValue(subscriptionManagement, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            subscriptionManagement.UseRedirect.ShouldBeNull();
            subscriptionManagement.UseRedirect.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_UseRedirect_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUseRedirect = "UseRedirect";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constUseRedirect));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UseRedirect_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constUseRedirect = "UseRedirect";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constUseRedirect);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SubscriptionManagement => UseThankYou

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UseThankYou_Data_Without_Null_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var random = Fixture.Create<bool>();

            // Act , Set
            subscriptionManagement.UseThankYou = random;

            // Assert
            subscriptionManagement.UseThankYou.ShouldBe(random);
            subscriptionManagement.UseThankYou.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UseThankYou_Only_Null_Data_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();    

            // Act , Set
            subscriptionManagement.UseThankYou = null;

            // Assert
            subscriptionManagement.UseThankYou.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UseThankYou_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUseThankYou = "UseThankYou";
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var propertyInfo = subscriptionManagement.GetType().GetProperty(constUseThankYou);

            // Act , Set
            propertyInfo.SetValue(subscriptionManagement, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            subscriptionManagement.UseThankYou.ShouldBeNull();
            subscriptionManagement.UseThankYou.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_UseThankYou_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUseThankYou = "UseThankYou";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constUseThankYou));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UseThankYou_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constUseThankYou = "UseThankYou";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constUseThankYou);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : SubscriptionManagement => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = subscriptionManagement.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(subscriptionManagement, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : SubscriptionManagement => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = subscriptionManagement.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(subscriptionManagement, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : SubscriptionManagement => BaseChannelID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelID_Int_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var intType = subscriptionManagement.BaseChannelID.GetType();

            // Act
            var isTypeInt = typeof(int).Equals(intType);    
            var isTypeNullableInt = typeof(int?).Equals(intType);
            var isTypeString = typeof(string).Equals(intType);
            var isTypeDecimal = typeof(decimal).Equals(intType);
            var isTypeLong = typeof(long).Equals(intType);
            var isTypeBool = typeof(bool).Equals(intType);
            var isTypeDouble = typeof(double).Equals(intType);
            var isTypeFloat = typeof(float).Equals(intType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(intType);
            var isTypeLongNullable = typeof(long?).Equals(intType);
            var isTypeBoolNullable = typeof(bool?).Equals(intType);
            var isTypeDoubleNullable = typeof(double?).Equals(intType);
            var isTypeFloatNullable = typeof(float?).Equals(intType);


            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_BaseChannelID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constBaseChannelID = "BaseChannelID";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constBaseChannelID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelID_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constBaseChannelID = "BaseChannelID";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : SubscriptionManagement => RedirectDelay

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_RedirectDelay_Int_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var intType = subscriptionManagement.RedirectDelay.GetType();

            // Act
            var isTypeInt = typeof(int).Equals(intType);    
            var isTypeNullableInt = typeof(int?).Equals(intType);
            var isTypeString = typeof(string).Equals(intType);
            var isTypeDecimal = typeof(decimal).Equals(intType);
            var isTypeLong = typeof(long).Equals(intType);
            var isTypeBool = typeof(bool).Equals(intType);
            var isTypeDouble = typeof(double).Equals(intType);
            var isTypeFloat = typeof(float).Equals(intType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(intType);
            var isTypeLongNullable = typeof(long?).Equals(intType);
            var isTypeBoolNullable = typeof(bool?).Equals(intType);
            var isTypeDoubleNullable = typeof(double?).Equals(intType);
            var isTypeFloatNullable = typeof(float?).Equals(intType);


            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_RedirectDelay_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constRedirectDelay = "RedirectDelay";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constRedirectDelay));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_RedirectDelay_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constRedirectDelay = "RedirectDelay";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constRedirectDelay);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : SubscriptionManagement => SubscriptionManagementID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SubscriptionManagementID_Int_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var intType = subscriptionManagement.SubscriptionManagementID.GetType();

            // Act
            var isTypeInt = typeof(int).Equals(intType);    
            var isTypeNullableInt = typeof(int?).Equals(intType);
            var isTypeString = typeof(string).Equals(intType);
            var isTypeDecimal = typeof(decimal).Equals(intType);
            var isTypeLong = typeof(long).Equals(intType);
            var isTypeBool = typeof(bool).Equals(intType);
            var isTypeDouble = typeof(double).Equals(intType);
            var isTypeFloat = typeof(float).Equals(intType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(intType);
            var isTypeLongNullable = typeof(long?).Equals(intType);
            var isTypeBoolNullable = typeof(bool?).Equals(intType);
            var isTypeDoubleNullable = typeof(double?).Equals(intType);
            var isTypeFloatNullable = typeof(float?).Equals(intType);


            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_SubscriptionManagementID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constSubscriptionManagementID = "SubscriptionManagementID";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constSubscriptionManagementID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SubscriptionManagementID_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constSubscriptionManagementID = "SubscriptionManagementID";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constSubscriptionManagementID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SubscriptionManagement => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var random = Fixture.Create<int>();

            // Act , Set
            subscriptionManagement.CreatedUserID = random;

            // Assert
            subscriptionManagement.CreatedUserID.ShouldBe(random);
            subscriptionManagement.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();    

            // Act , Set
            subscriptionManagement.CreatedUserID = null;

            // Assert
            subscriptionManagement.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var propertyInfo = subscriptionManagement.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(subscriptionManagement, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            subscriptionManagement.CreatedUserID.ShouldBeNull();
            subscriptionManagement.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SubscriptionManagement => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var random = Fixture.Create<int>();

            // Act , Set
            subscriptionManagement.UpdatedUserID = random;

            // Assert
            subscriptionManagement.UpdatedUserID.ShouldBe(random);
            subscriptionManagement.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();    

            // Act , Set
            subscriptionManagement.UpdatedUserID = null;

            // Assert
            subscriptionManagement.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var propertyInfo = subscriptionManagement.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(subscriptionManagement, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            subscriptionManagement.UpdatedUserID.ShouldBeNull();
            subscriptionManagement.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SubscriptionManagement => AdminEmail

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_AdminEmail_String_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var stringType = subscriptionManagement.AdminEmail.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_AdminEmail_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constAdminEmail = "AdminEmail";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constAdminEmail));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_AdminEmail_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constAdminEmail = "AdminEmail";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constAdminEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SubscriptionManagement => EmailFooter

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_EmailFooter_String_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var stringType = subscriptionManagement.EmailFooter.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_EmailFooter_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constEmailFooter = "EmailFooter";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constEmailFooter));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_EmailFooter_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constEmailFooter = "EmailFooter";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constEmailFooter);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SubscriptionManagement => EmailHeader

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_EmailHeader_String_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var stringType = subscriptionManagement.EmailHeader.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_EmailHeader_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constEmailHeader = "EmailHeader";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constEmailHeader));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_EmailHeader_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constEmailHeader = "EmailHeader";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constEmailHeader);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SubscriptionManagement => Footer

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Footer_String_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var stringType = subscriptionManagement.Footer.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_Footer_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constFooter = "Footer";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constFooter));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Footer_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constFooter = "Footer";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constFooter);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SubscriptionManagement => Header

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Header_String_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var stringType = subscriptionManagement.Header.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_Header_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constHeader = "Header";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constHeader));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Header_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constHeader = "Header";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constHeader);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SubscriptionManagement => MSMessage

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_MSMessage_String_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var stringType = subscriptionManagement.MSMessage.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_MSMessage_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constMSMessage = "MSMessage";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constMSMessage));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_MSMessage_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constMSMessage = "MSMessage";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constMSMessage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SubscriptionManagement => Name

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Name_String_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var stringType = subscriptionManagement.Name.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_Name_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constName = "Name";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constName));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Name_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constName = "Name";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SubscriptionManagement => ReasonLabel

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ReasonLabel_String_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var stringType = subscriptionManagement.ReasonLabel.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_ReasonLabel_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constReasonLabel = "ReasonLabel";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constReasonLabel));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ReasonLabel_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constReasonLabel = "ReasonLabel";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constReasonLabel);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SubscriptionManagement => RedirectURL

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_RedirectURL_String_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var stringType = subscriptionManagement.RedirectURL.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_RedirectURL_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constRedirectURL = "RedirectURL";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constRedirectURL));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_RedirectURL_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constRedirectURL = "RedirectURL";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constRedirectURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SubscriptionManagement => ThankYouLabel

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ThankYouLabel_String_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagement = Fixture.Create<SubscriptionManagement>();
            var stringType = subscriptionManagement.ThankYouLabel.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagement_Class_Invalid_Property_ThankYouLabel_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constThankYouLabel = "ThankYouLabel";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagement.GetType().GetProperty(constThankYouLabel));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ThankYouLabel_Is_Present_In_SubscriptionManagement_Class_As_Public_Test()
        {
            // Arrange
            const string constThankYouLabel = "ThankYouLabel";
            var subscriptionManagement  = Fixture.Create<SubscriptionManagement>();
            var propertyInfo  = subscriptionManagement.GetType().GetProperty(constThankYouLabel);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region General Category : General

        #region Category : Contructor

        #region General Constructor Pattern : create and expect no exception.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new SubscriptionManagement());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<SubscriptionManagement>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion

        #region General Constructor Pattern : Default Assignment Test

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_With_Default_Assignments_NoChange_DefaultValues()
        {
            // Arrange
            var subscriptionManagementID = -1;
            var baseChannelID = -1;
            var name = string.Empty;
            var header = string.Empty;
            var footer = string.Empty;
            var emailHeader = string.Empty;
            var emailFooter = string.Empty;
            var adminEmail = string.Empty;
            var mSMessage = string.Empty;
            var includeMSGroups = false;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            var isDeleted = false;
            var useReasonDropDown = false;
            var reasonVisible = false;
            var useThankYou = false;
            var useRedirect = false;
            var thankYouLabel = string.Empty;
            var redirectURL = string.Empty;
            var redirectDelay = 0;    

            // Act
            var subscriptionManagement = new SubscriptionManagement();    

            // Assert
            subscriptionManagement.SubscriptionManagementID.ShouldBe(subscriptionManagementID);
            subscriptionManagement.BaseChannelID.ShouldBe(baseChannelID);
            subscriptionManagement.Name.ShouldBe(name);
            subscriptionManagement.Header.ShouldBe(header);
            subscriptionManagement.Footer.ShouldBe(footer);
            subscriptionManagement.EmailHeader.ShouldBe(emailHeader);
            subscriptionManagement.EmailFooter.ShouldBe(emailFooter);
            subscriptionManagement.AdminEmail.ShouldBe(adminEmail);
            subscriptionManagement.MSMessage.ShouldBe(mSMessage);
            subscriptionManagement.IncludeMSGroups.ShouldBe(includeMSGroups);
            subscriptionManagement.CreatedUserID.ShouldBeNull();
            subscriptionManagement.CreatedDate.ShouldBeNull();
            subscriptionManagement.UpdatedUserID.ShouldBeNull();
            subscriptionManagement.UpdatedDate.ShouldBeNull();
            subscriptionManagement.IsDeleted.ShouldBe(isDeleted);
            subscriptionManagement.UseReasonDropDown.ShouldBe(useReasonDropDown);
            subscriptionManagement.ReasonVisible.ShouldBe(reasonVisible);
            subscriptionManagement.UseThankYou.ShouldBe(useThankYou);
            subscriptionManagement.UseRedirect.ShouldBe(useRedirect);
            subscriptionManagement.ThankYouLabel.ShouldBe(thankYouLabel);
            subscriptionManagement.RedirectURL.ShouldBe(redirectURL);
            subscriptionManagement.RedirectDelay.ShouldBe(redirectDelay);
        }

        #endregion

        #endregion

        #endregion
    }
}
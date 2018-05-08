using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class CampaignItemBlastFilterTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            var campaignItemBlastFilterId = Fixture.Create<int>();
            var campaignItemBlastId = Fixture.Create<int?>();
            var campaignItemSuppresionId = Fixture.Create<int?>();
            var suppressionGroupId = Fixture.Create<int?>();
            var campaignItemTestBlastId = Fixture.Create<int?>();
            var smartSegmentId = Fixture.Create<int?>();
            var refBlastIDs = Fixture.Create<string>();
            var filterId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            campaignItemBlastFilter.CampaignItemBlastFilterID = campaignItemBlastFilterId;
            campaignItemBlastFilter.CampaignItemBlastID = campaignItemBlastId;
            campaignItemBlastFilter.CampaignItemSuppresionID = campaignItemSuppresionId;
            campaignItemBlastFilter.SuppressionGroupID = suppressionGroupId;
            campaignItemBlastFilter.CampaignItemTestBlastID = campaignItemTestBlastId;
            campaignItemBlastFilter.SmartSegmentID = smartSegmentId;
            campaignItemBlastFilter.RefBlastIDs = refBlastIDs;
            campaignItemBlastFilter.FilterID = filterId;
            campaignItemBlastFilter.IsDeleted = isDeleted;

            // Assert
            campaignItemBlastFilter.CampaignItemBlastFilterID.ShouldBe(campaignItemBlastFilterId);
            campaignItemBlastFilter.CampaignItemBlastID.ShouldBe(campaignItemBlastId);
            campaignItemBlastFilter.CampaignItemSuppresionID.ShouldBe(campaignItemSuppresionId);
            campaignItemBlastFilter.SuppressionGroupID.ShouldBe(suppressionGroupId);
            campaignItemBlastFilter.CampaignItemTestBlastID.ShouldBe(campaignItemTestBlastId);
            campaignItemBlastFilter.SmartSegmentID.ShouldBe(smartSegmentId);
            campaignItemBlastFilter.RefBlastIDs.ShouldBe(refBlastIDs);
            campaignItemBlastFilter.FilterID.ShouldBe(filterId);
            campaignItemBlastFilter.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (CampaignItemBlastFilterID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_CampaignItemBlastFilterID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            campaignItemBlastFilter.CampaignItemBlastFilterID = Fixture.Create<int>();
            var intType = campaignItemBlastFilter.CampaignItemBlastFilterID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (CampaignItemBlastFilterID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_Class_Invalid_Property_CampaignItemBlastFilterIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemBlastFilterID = "CampaignItemBlastFilterIDNotPresent";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastFilter.GetType().GetProperty(propertyNameCampaignItemBlastFilterID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_CampaignItemBlastFilterID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemBlastFilterID = "CampaignItemBlastFilterID";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();
            var propertyInfo  = campaignItemBlastFilter.GetType().GetProperty(propertyNameCampaignItemBlastFilterID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (CampaignItemBlastID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_CampaignItemBlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlastFilter.CampaignItemBlastID = random;

            // Assert
            campaignItemBlastFilter.CampaignItemBlastID.ShouldBe(random);
            campaignItemBlastFilter.CampaignItemBlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_CampaignItemBlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();    

            // Act , Set
            campaignItemBlastFilter.CampaignItemBlastID = null;

            // Assert
            campaignItemBlastFilter.CampaignItemBlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_CampaignItemBlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCampaignItemBlastID = "CampaignItemBlastID";
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            var propertyInfo = campaignItemBlastFilter.GetType().GetProperty(propertyNameCampaignItemBlastID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlastFilter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlastFilter.CampaignItemBlastID.ShouldBeNull();
            campaignItemBlastFilter.CampaignItemBlastID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (CampaignItemBlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_Class_Invalid_Property_CampaignItemBlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemBlastID = "CampaignItemBlastIDNotPresent";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastFilter.GetType().GetProperty(propertyNameCampaignItemBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_CampaignItemBlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemBlastID = "CampaignItemBlastID";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();
            var propertyInfo  = campaignItemBlastFilter.GetType().GetProperty(propertyNameCampaignItemBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (CampaignItemSuppresionID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_CampaignItemSuppresionID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlastFilter.CampaignItemSuppresionID = random;

            // Assert
            campaignItemBlastFilter.CampaignItemSuppresionID.ShouldBe(random);
            campaignItemBlastFilter.CampaignItemSuppresionID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_CampaignItemSuppresionID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();    

            // Act , Set
            campaignItemBlastFilter.CampaignItemSuppresionID = null;

            // Assert
            campaignItemBlastFilter.CampaignItemSuppresionID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_CampaignItemSuppresionID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCampaignItemSuppresionID = "CampaignItemSuppresionID";
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            var propertyInfo = campaignItemBlastFilter.GetType().GetProperty(propertyNameCampaignItemSuppresionID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlastFilter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlastFilter.CampaignItemSuppresionID.ShouldBeNull();
            campaignItemBlastFilter.CampaignItemSuppresionID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (CampaignItemSuppresionID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_Class_Invalid_Property_CampaignItemSuppresionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemSuppresionID = "CampaignItemSuppresionIDNotPresent";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastFilter.GetType().GetProperty(propertyNameCampaignItemSuppresionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_CampaignItemSuppresionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemSuppresionID = "CampaignItemSuppresionID";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();
            var propertyInfo  = campaignItemBlastFilter.GetType().GetProperty(propertyNameCampaignItemSuppresionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (CampaignItemTestBlastID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_CampaignItemTestBlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlastFilter.CampaignItemTestBlastID = random;

            // Assert
            campaignItemBlastFilter.CampaignItemTestBlastID.ShouldBe(random);
            campaignItemBlastFilter.CampaignItemTestBlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_CampaignItemTestBlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();    

            // Act , Set
            campaignItemBlastFilter.CampaignItemTestBlastID = null;

            // Assert
            campaignItemBlastFilter.CampaignItemTestBlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_CampaignItemTestBlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCampaignItemTestBlastID = "CampaignItemTestBlastID";
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            var propertyInfo = campaignItemBlastFilter.GetType().GetProperty(propertyNameCampaignItemTestBlastID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlastFilter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlastFilter.CampaignItemTestBlastID.ShouldBeNull();
            campaignItemBlastFilter.CampaignItemTestBlastID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (CampaignItemTestBlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_Class_Invalid_Property_CampaignItemTestBlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemTestBlastID = "CampaignItemTestBlastIDNotPresent";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastFilter.GetType().GetProperty(propertyNameCampaignItemTestBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_CampaignItemTestBlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemTestBlastID = "CampaignItemTestBlastID";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();
            var propertyInfo  = campaignItemBlastFilter.GetType().GetProperty(propertyNameCampaignItemTestBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (FilterID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_FilterID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlastFilter.FilterID = random;

            // Assert
            campaignItemBlastFilter.FilterID.ShouldBe(random);
            campaignItemBlastFilter.FilterID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_FilterID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();    

            // Act , Set
            campaignItemBlastFilter.FilterID = null;

            // Assert
            campaignItemBlastFilter.FilterID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_FilterID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameFilterID = "FilterID";
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            var propertyInfo = campaignItemBlastFilter.GetType().GetProperty(propertyNameFilterID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlastFilter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlastFilter.FilterID.ShouldBeNull();
            campaignItemBlastFilter.FilterID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (FilterID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_Class_Invalid_Property_FilterIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterID = "FilterIDNotPresent";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastFilter.GetType().GetProperty(propertyNameFilterID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_FilterID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterID = "FilterID";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();
            var propertyInfo  = campaignItemBlastFilter.GetType().GetProperty(propertyNameFilterID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            var random = Fixture.Create<bool>();

            // Act , Set
            campaignItemBlastFilter.IsDeleted = random;

            // Assert
            campaignItemBlastFilter.IsDeleted.ShouldBe(random);
            campaignItemBlastFilter.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();    

            // Act , Set
            campaignItemBlastFilter.IsDeleted = null;

            // Assert
            campaignItemBlastFilter.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            var propertyInfo = campaignItemBlastFilter.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlastFilter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlastFilter.IsDeleted.ShouldBeNull();
            campaignItemBlastFilter.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastFilter.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();
            var propertyInfo  = campaignItemBlastFilter.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (RefBlastIDs) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_RefBlastIDs_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            campaignItemBlastFilter.RefBlastIDs = Fixture.Create<string>();
            var stringType = campaignItemBlastFilter.RefBlastIDs.GetType();

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

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (RefBlastIDs) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_Class_Invalid_Property_RefBlastIDsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRefBlastIDs = "RefBlastIDsNotPresent";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastFilter.GetType().GetProperty(propertyNameRefBlastIDs));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_RefBlastIDs_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRefBlastIDs = "RefBlastIDs";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();
            var propertyInfo  = campaignItemBlastFilter.GetType().GetProperty(propertyNameRefBlastIDs);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (SmartSegmentID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_SmartSegmentID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlastFilter.SmartSegmentID = random;

            // Assert
            campaignItemBlastFilter.SmartSegmentID.ShouldBe(random);
            campaignItemBlastFilter.SmartSegmentID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_SmartSegmentID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();    

            // Act , Set
            campaignItemBlastFilter.SmartSegmentID = null;

            // Assert
            campaignItemBlastFilter.SmartSegmentID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_SmartSegmentID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSmartSegmentID = "SmartSegmentID";
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            var propertyInfo = campaignItemBlastFilter.GetType().GetProperty(propertyNameSmartSegmentID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlastFilter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlastFilter.SmartSegmentID.ShouldBeNull();
            campaignItemBlastFilter.SmartSegmentID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (SmartSegmentID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_Class_Invalid_Property_SmartSegmentIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSmartSegmentID = "SmartSegmentIDNotPresent";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastFilter.GetType().GetProperty(propertyNameSmartSegmentID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_SmartSegmentID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSmartSegmentID = "SmartSegmentID";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();
            var propertyInfo  = campaignItemBlastFilter.GetType().GetProperty(propertyNameSmartSegmentID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (SuppressionGroupID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_SuppressionGroupID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlastFilter.SuppressionGroupID = random;

            // Assert
            campaignItemBlastFilter.SuppressionGroupID.ShouldBe(random);
            campaignItemBlastFilter.SuppressionGroupID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_SuppressionGroupID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();    

            // Act , Set
            campaignItemBlastFilter.SuppressionGroupID = null;

            // Assert
            campaignItemBlastFilter.SuppressionGroupID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_SuppressionGroupID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSuppressionGroupID = "SuppressionGroupID";
            var campaignItemBlastFilter = Fixture.Create<CampaignItemBlastFilter>();
            var propertyInfo = campaignItemBlastFilter.GetType().GetProperty(propertyNameSuppressionGroupID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlastFilter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlastFilter.SuppressionGroupID.ShouldBeNull();
            campaignItemBlastFilter.SuppressionGroupID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastFilter) => Property (SuppressionGroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_Class_Invalid_Property_SuppressionGroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressionGroupID = "SuppressionGroupIDNotPresent";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastFilter.GetType().GetProperty(propertyNameSuppressionGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastFilter_SuppressionGroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuppressionGroupID = "SuppressionGroupID";
            var campaignItemBlastFilter  = Fixture.Create<CampaignItemBlastFilter>();
            var propertyInfo  = campaignItemBlastFilter.GetType().GetProperty(propertyNameSuppressionGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (CampaignItemBlastFilter) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemBlastFilter_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CampaignItemBlastFilter());
        }

        #endregion

        #region General Constructor : Class (CampaignItemBlastFilter) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemBlastFilter_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCampaignItemBlastFilter = Fixture.CreateMany<CampaignItemBlastFilter>(2).ToList();
            var firstCampaignItemBlastFilter = instancesOfCampaignItemBlastFilter.FirstOrDefault();
            var lastCampaignItemBlastFilter = instancesOfCampaignItemBlastFilter.Last();

            // Act, Assert
            firstCampaignItemBlastFilter.ShouldNotBeNull();
            lastCampaignItemBlastFilter.ShouldNotBeNull();
            firstCampaignItemBlastFilter.ShouldNotBeSameAs(lastCampaignItemBlastFilter);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemBlastFilter_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCampaignItemBlastFilter = new CampaignItemBlastFilter();
            var secondCampaignItemBlastFilter = new CampaignItemBlastFilter();
            var thirdCampaignItemBlastFilter = new CampaignItemBlastFilter();
            var fourthCampaignItemBlastFilter = new CampaignItemBlastFilter();
            var fifthCampaignItemBlastFilter = new CampaignItemBlastFilter();
            var sixthCampaignItemBlastFilter = new CampaignItemBlastFilter();

            // Act, Assert
            firstCampaignItemBlastFilter.ShouldNotBeNull();
            secondCampaignItemBlastFilter.ShouldNotBeNull();
            thirdCampaignItemBlastFilter.ShouldNotBeNull();
            fourthCampaignItemBlastFilter.ShouldNotBeNull();
            fifthCampaignItemBlastFilter.ShouldNotBeNull();
            sixthCampaignItemBlastFilter.ShouldNotBeNull();
            firstCampaignItemBlastFilter.ShouldNotBeSameAs(secondCampaignItemBlastFilter);
            thirdCampaignItemBlastFilter.ShouldNotBeSameAs(firstCampaignItemBlastFilter);
            fourthCampaignItemBlastFilter.ShouldNotBeSameAs(firstCampaignItemBlastFilter);
            fifthCampaignItemBlastFilter.ShouldNotBeSameAs(firstCampaignItemBlastFilter);
            sixthCampaignItemBlastFilter.ShouldNotBeSameAs(firstCampaignItemBlastFilter);
            sixthCampaignItemBlastFilter.ShouldNotBeSameAs(fourthCampaignItemBlastFilter);
        }

        #endregion

        #endregion

        #endregion
    }
}
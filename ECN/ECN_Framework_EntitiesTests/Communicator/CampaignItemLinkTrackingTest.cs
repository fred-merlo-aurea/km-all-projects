using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class CampaignItemLinkTrackingTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (CampaignItemLinkTracking) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var campaignItemLinkTracking = Fixture.Create<CampaignItemLinkTracking>();
            var cILTId = Fixture.Create<int>();
            var campaignItemId = Fixture.Create<int?>();
            var lTPId = Fixture.Create<int?>();
            var lTPOId = Fixture.Create<int?>();
            var customValue = Fixture.Create<string>();

            // Act
            campaignItemLinkTracking.CILTID = cILTId;
            campaignItemLinkTracking.CampaignItemID = campaignItemId;
            campaignItemLinkTracking.LTPID = lTPId;
            campaignItemLinkTracking.LTPOID = lTPOId;
            campaignItemLinkTracking.CustomValue = customValue;

            // Assert
            campaignItemLinkTracking.CILTID.ShouldBe(cILTId);
            campaignItemLinkTracking.CampaignItemID.ShouldBe(campaignItemId);
            campaignItemLinkTracking.LTPID.ShouldBe(lTPId);
            campaignItemLinkTracking.LTPOID.ShouldBe(lTPOId);
            campaignItemLinkTracking.CustomValue.ShouldBe(customValue);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemLinkTracking) => Property (CampaignItemID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_CampaignItemID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemLinkTracking = Fixture.Create<CampaignItemLinkTracking>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemLinkTracking.CampaignItemID = random;

            // Assert
            campaignItemLinkTracking.CampaignItemID.ShouldBe(random);
            campaignItemLinkTracking.CampaignItemID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_CampaignItemID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemLinkTracking = Fixture.Create<CampaignItemLinkTracking>();    

            // Act , Set
            campaignItemLinkTracking.CampaignItemID = null;

            // Assert
            campaignItemLinkTracking.CampaignItemID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_CampaignItemID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCampaignItemID = "CampaignItemID";
            var campaignItemLinkTracking = Fixture.Create<CampaignItemLinkTracking>();
            var propertyInfo = campaignItemLinkTracking.GetType().GetProperty(propertyNameCampaignItemID);

            // Act , Set
            propertyInfo.SetValue(campaignItemLinkTracking, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemLinkTracking.CampaignItemID.ShouldBeNull();
            campaignItemLinkTracking.CampaignItemID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemLinkTracking) => Property (CampaignItemID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_Class_Invalid_Property_CampaignItemIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemID = "CampaignItemIDNotPresent";
            var campaignItemLinkTracking  = Fixture.Create<CampaignItemLinkTracking>();

            // Act , Assert
            Should.NotThrow(() => campaignItemLinkTracking.GetType().GetProperty(propertyNameCampaignItemID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_CampaignItemID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemID = "CampaignItemID";
            var campaignItemLinkTracking  = Fixture.Create<CampaignItemLinkTracking>();
            var propertyInfo  = campaignItemLinkTracking.GetType().GetProperty(propertyNameCampaignItemID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemLinkTracking) => Property (CILTID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_CILTID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignItemLinkTracking = Fixture.Create<CampaignItemLinkTracking>();
            campaignItemLinkTracking.CILTID = Fixture.Create<int>();
            var intType = campaignItemLinkTracking.CILTID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemLinkTracking) => Property (CILTID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_Class_Invalid_Property_CILTIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCILTID = "CILTIDNotPresent";
            var campaignItemLinkTracking  = Fixture.Create<CampaignItemLinkTracking>();

            // Act , Assert
            Should.NotThrow(() => campaignItemLinkTracking.GetType().GetProperty(propertyNameCILTID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_CILTID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCILTID = "CILTID";
            var campaignItemLinkTracking  = Fixture.Create<CampaignItemLinkTracking>();
            var propertyInfo  = campaignItemLinkTracking.GetType().GetProperty(propertyNameCILTID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemLinkTracking) => Property (CustomValue) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_CustomValue_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignItemLinkTracking = Fixture.Create<CampaignItemLinkTracking>();
            campaignItemLinkTracking.CustomValue = Fixture.Create<string>();
            var stringType = campaignItemLinkTracking.CustomValue.GetType();

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

        #region General Getters/Setters : Class (CampaignItemLinkTracking) => Property (CustomValue) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_Class_Invalid_Property_CustomValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomValue = "CustomValueNotPresent";
            var campaignItemLinkTracking  = Fixture.Create<CampaignItemLinkTracking>();

            // Act , Assert
            Should.NotThrow(() => campaignItemLinkTracking.GetType().GetProperty(propertyNameCustomValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_CustomValue_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomValue = "CustomValue";
            var campaignItemLinkTracking  = Fixture.Create<CampaignItemLinkTracking>();
            var propertyInfo  = campaignItemLinkTracking.GetType().GetProperty(propertyNameCustomValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemLinkTracking) => Property (LTPID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_LTPID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemLinkTracking = Fixture.Create<CampaignItemLinkTracking>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemLinkTracking.LTPID = random;

            // Assert
            campaignItemLinkTracking.LTPID.ShouldBe(random);
            campaignItemLinkTracking.LTPID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_LTPID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemLinkTracking = Fixture.Create<CampaignItemLinkTracking>();    

            // Act , Set
            campaignItemLinkTracking.LTPID = null;

            // Assert
            campaignItemLinkTracking.LTPID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_LTPID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameLTPID = "LTPID";
            var campaignItemLinkTracking = Fixture.Create<CampaignItemLinkTracking>();
            var propertyInfo = campaignItemLinkTracking.GetType().GetProperty(propertyNameLTPID);

            // Act , Set
            propertyInfo.SetValue(campaignItemLinkTracking, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemLinkTracking.LTPID.ShouldBeNull();
            campaignItemLinkTracking.LTPID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemLinkTracking) => Property (LTPID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_Class_Invalid_Property_LTPIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLTPID = "LTPIDNotPresent";
            var campaignItemLinkTracking  = Fixture.Create<CampaignItemLinkTracking>();

            // Act , Assert
            Should.NotThrow(() => campaignItemLinkTracking.GetType().GetProperty(propertyNameLTPID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_LTPID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLTPID = "LTPID";
            var campaignItemLinkTracking  = Fixture.Create<CampaignItemLinkTracking>();
            var propertyInfo  = campaignItemLinkTracking.GetType().GetProperty(propertyNameLTPID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemLinkTracking) => Property (LTPOID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_LTPOID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemLinkTracking = Fixture.Create<CampaignItemLinkTracking>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemLinkTracking.LTPOID = random;

            // Assert
            campaignItemLinkTracking.LTPOID.ShouldBe(random);
            campaignItemLinkTracking.LTPOID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_LTPOID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemLinkTracking = Fixture.Create<CampaignItemLinkTracking>();    

            // Act , Set
            campaignItemLinkTracking.LTPOID = null;

            // Assert
            campaignItemLinkTracking.LTPOID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_LTPOID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameLTPOID = "LTPOID";
            var campaignItemLinkTracking = Fixture.Create<CampaignItemLinkTracking>();
            var propertyInfo = campaignItemLinkTracking.GetType().GetProperty(propertyNameLTPOID);

            // Act , Set
            propertyInfo.SetValue(campaignItemLinkTracking, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemLinkTracking.LTPOID.ShouldBeNull();
            campaignItemLinkTracking.LTPOID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemLinkTracking) => Property (LTPOID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_Class_Invalid_Property_LTPOIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLTPOID = "LTPOIDNotPresent";
            var campaignItemLinkTracking  = Fixture.Create<CampaignItemLinkTracking>();

            // Act , Assert
            Should.NotThrow(() => campaignItemLinkTracking.GetType().GetProperty(propertyNameLTPOID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemLinkTracking_LTPOID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLTPOID = "LTPOID";
            var campaignItemLinkTracking  = Fixture.Create<CampaignItemLinkTracking>();
            var propertyInfo  = campaignItemLinkTracking.GetType().GetProperty(propertyNameLTPOID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (CampaignItemLinkTracking) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemLinkTracking_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CampaignItemLinkTracking());
        }

        #endregion

        #region General Constructor : Class (CampaignItemLinkTracking) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemLinkTracking_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCampaignItemLinkTracking = Fixture.CreateMany<CampaignItemLinkTracking>(2).ToList();
            var firstCampaignItemLinkTracking = instancesOfCampaignItemLinkTracking.FirstOrDefault();
            var lastCampaignItemLinkTracking = instancesOfCampaignItemLinkTracking.Last();

            // Act, Assert
            firstCampaignItemLinkTracking.ShouldNotBeNull();
            lastCampaignItemLinkTracking.ShouldNotBeNull();
            firstCampaignItemLinkTracking.ShouldNotBeSameAs(lastCampaignItemLinkTracking);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemLinkTracking_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCampaignItemLinkTracking = new CampaignItemLinkTracking();
            var secondCampaignItemLinkTracking = new CampaignItemLinkTracking();
            var thirdCampaignItemLinkTracking = new CampaignItemLinkTracking();
            var fourthCampaignItemLinkTracking = new CampaignItemLinkTracking();
            var fifthCampaignItemLinkTracking = new CampaignItemLinkTracking();
            var sixthCampaignItemLinkTracking = new CampaignItemLinkTracking();

            // Act, Assert
            firstCampaignItemLinkTracking.ShouldNotBeNull();
            secondCampaignItemLinkTracking.ShouldNotBeNull();
            thirdCampaignItemLinkTracking.ShouldNotBeNull();
            fourthCampaignItemLinkTracking.ShouldNotBeNull();
            fifthCampaignItemLinkTracking.ShouldNotBeNull();
            sixthCampaignItemLinkTracking.ShouldNotBeNull();
            firstCampaignItemLinkTracking.ShouldNotBeSameAs(secondCampaignItemLinkTracking);
            thirdCampaignItemLinkTracking.ShouldNotBeSameAs(firstCampaignItemLinkTracking);
            fourthCampaignItemLinkTracking.ShouldNotBeSameAs(firstCampaignItemLinkTracking);
            fifthCampaignItemLinkTracking.ShouldNotBeSameAs(firstCampaignItemLinkTracking);
            sixthCampaignItemLinkTracking.ShouldNotBeSameAs(firstCampaignItemLinkTracking);
            sixthCampaignItemLinkTracking.ShouldNotBeSameAs(fourthCampaignItemLinkTracking);
        }

        #endregion

        #region General Constructor : Class (CampaignItemLinkTracking) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemLinkTracking_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var cILTId = -1;
            var customValue = string.Empty;

            // Act
            var campaignItemLinkTracking = new CampaignItemLinkTracking();

            // Assert
            campaignItemLinkTracking.CILTID.ShouldBe(cILTId);
            campaignItemLinkTracking.CampaignItemID.ShouldBeNull();
            campaignItemLinkTracking.LTPID.ShouldBeNull();
            campaignItemLinkTracking.LTPOID.ShouldBeNull();
            campaignItemLinkTracking.CustomValue.ShouldBe(customValue);
        }

        #endregion

        #endregion

        #endregion
    }
}
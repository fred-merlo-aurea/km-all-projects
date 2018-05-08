using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class CustomerLinkTrackingTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (CustomerLinkTracking) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var customerLinkTracking = Fixture.Create<CustomerLinkTracking>();
            var cLTId = Fixture.Create<int>();
            var customerId = Fixture.Create<int?>();
            var lTPId = Fixture.Create<int?>();
            var lTPOId = Fixture.Create<int?>();
            var isActive = Fixture.Create<bool?>();

            // Act
            customerLinkTracking.CLTID = cLTId;
            customerLinkTracking.CustomerID = customerId;
            customerLinkTracking.LTPID = lTPId;
            customerLinkTracking.LTPOID = lTPOId;
            customerLinkTracking.IsActive = isActive;

            // Assert
            customerLinkTracking.CLTID.ShouldBe(cLTId);
            customerLinkTracking.CustomerID.ShouldBe(customerId);
            customerLinkTracking.LTPID.ShouldBe(lTPId);
            customerLinkTracking.LTPOID.ShouldBe(lTPOId);
            customerLinkTracking.IsActive.ShouldBe(isActive);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerLinkTracking) => Property (CLTID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_CLTID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var customerLinkTracking = Fixture.Create<CustomerLinkTracking>();
            customerLinkTracking.CLTID = Fixture.Create<int>();
            var intType = customerLinkTracking.CLTID.GetType();

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

        #region General Getters/Setters : Class (CustomerLinkTracking) => Property (CLTID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_Class_Invalid_Property_CLTIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCLTID = "CLTIDNotPresent";
            var customerLinkTracking  = Fixture.Create<CustomerLinkTracking>();

            // Act , Assert
            Should.NotThrow(() => customerLinkTracking.GetType().GetProperty(propertyNameCLTID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_CLTID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCLTID = "CLTID";
            var customerLinkTracking  = Fixture.Create<CustomerLinkTracking>();
            var propertyInfo  = customerLinkTracking.GetType().GetProperty(propertyNameCLTID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerLinkTracking) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerLinkTracking = Fixture.Create<CustomerLinkTracking>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerLinkTracking.CustomerID = random;

            // Assert
            customerLinkTracking.CustomerID.ShouldBe(random);
            customerLinkTracking.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerLinkTracking = Fixture.Create<CustomerLinkTracking>();    

            // Act , Set
            customerLinkTracking.CustomerID = null;

            // Assert
            customerLinkTracking.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var customerLinkTracking = Fixture.Create<CustomerLinkTracking>();
            var propertyInfo = customerLinkTracking.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(customerLinkTracking, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerLinkTracking.CustomerID.ShouldBeNull();
            customerLinkTracking.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerLinkTracking) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var customerLinkTracking  = Fixture.Create<CustomerLinkTracking>();

            // Act , Assert
            Should.NotThrow(() => customerLinkTracking.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var customerLinkTracking  = Fixture.Create<CustomerLinkTracking>();
            var propertyInfo  = customerLinkTracking.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerLinkTracking) => Property (IsActive) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_IsActive_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerLinkTracking = Fixture.Create<CustomerLinkTracking>();
            var random = Fixture.Create<bool>();

            // Act , Set
            customerLinkTracking.IsActive = random;

            // Assert
            customerLinkTracking.IsActive.ShouldBe(random);
            customerLinkTracking.IsActive.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_IsActive_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerLinkTracking = Fixture.Create<CustomerLinkTracking>();    

            // Act , Set
            customerLinkTracking.IsActive = null;

            // Assert
            customerLinkTracking.IsActive.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_IsActive_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsActive = "IsActive";
            var customerLinkTracking = Fixture.Create<CustomerLinkTracking>();
            var propertyInfo = customerLinkTracking.GetType().GetProperty(propertyNameIsActive);

            // Act , Set
            propertyInfo.SetValue(customerLinkTracking, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerLinkTracking.IsActive.ShouldBeNull();
            customerLinkTracking.IsActive.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerLinkTracking) => Property (IsActive) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_Class_Invalid_Property_IsActiveNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActiveNotPresent";
            var customerLinkTracking  = Fixture.Create<CustomerLinkTracking>();

            // Act , Assert
            Should.NotThrow(() => customerLinkTracking.GetType().GetProperty(propertyNameIsActive));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_IsActive_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActive";
            var customerLinkTracking  = Fixture.Create<CustomerLinkTracking>();
            var propertyInfo  = customerLinkTracking.GetType().GetProperty(propertyNameIsActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerLinkTracking) => Property (LTPID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_LTPID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerLinkTracking = Fixture.Create<CustomerLinkTracking>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerLinkTracking.LTPID = random;

            // Assert
            customerLinkTracking.LTPID.ShouldBe(random);
            customerLinkTracking.LTPID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_LTPID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerLinkTracking = Fixture.Create<CustomerLinkTracking>();    

            // Act , Set
            customerLinkTracking.LTPID = null;

            // Assert
            customerLinkTracking.LTPID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_LTPID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameLTPID = "LTPID";
            var customerLinkTracking = Fixture.Create<CustomerLinkTracking>();
            var propertyInfo = customerLinkTracking.GetType().GetProperty(propertyNameLTPID);

            // Act , Set
            propertyInfo.SetValue(customerLinkTracking, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerLinkTracking.LTPID.ShouldBeNull();
            customerLinkTracking.LTPID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerLinkTracking) => Property (LTPID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_Class_Invalid_Property_LTPIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLTPID = "LTPIDNotPresent";
            var customerLinkTracking  = Fixture.Create<CustomerLinkTracking>();

            // Act , Assert
            Should.NotThrow(() => customerLinkTracking.GetType().GetProperty(propertyNameLTPID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_LTPID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLTPID = "LTPID";
            var customerLinkTracking  = Fixture.Create<CustomerLinkTracking>();
            var propertyInfo  = customerLinkTracking.GetType().GetProperty(propertyNameLTPID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerLinkTracking) => Property (LTPOID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_LTPOID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerLinkTracking = Fixture.Create<CustomerLinkTracking>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerLinkTracking.LTPOID = random;

            // Assert
            customerLinkTracking.LTPOID.ShouldBe(random);
            customerLinkTracking.LTPOID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_LTPOID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerLinkTracking = Fixture.Create<CustomerLinkTracking>();    

            // Act , Set
            customerLinkTracking.LTPOID = null;

            // Assert
            customerLinkTracking.LTPOID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_LTPOID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameLTPOID = "LTPOID";
            var customerLinkTracking = Fixture.Create<CustomerLinkTracking>();
            var propertyInfo = customerLinkTracking.GetType().GetProperty(propertyNameLTPOID);

            // Act , Set
            propertyInfo.SetValue(customerLinkTracking, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerLinkTracking.LTPOID.ShouldBeNull();
            customerLinkTracking.LTPOID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerLinkTracking) => Property (LTPOID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_Class_Invalid_Property_LTPOIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLTPOID = "LTPOIDNotPresent";
            var customerLinkTracking  = Fixture.Create<CustomerLinkTracking>();

            // Act , Assert
            Should.NotThrow(() => customerLinkTracking.GetType().GetProperty(propertyNameLTPOID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLinkTracking_LTPOID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLTPOID = "LTPOID";
            var customerLinkTracking  = Fixture.Create<CustomerLinkTracking>();
            var propertyInfo  = customerLinkTracking.GetType().GetProperty(propertyNameLTPOID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (CustomerLinkTracking) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerLinkTracking_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CustomerLinkTracking());
        }

        #endregion

        #region General Constructor : Class (CustomerLinkTracking) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerLinkTracking_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCustomerLinkTracking = Fixture.CreateMany<CustomerLinkTracking>(2).ToList();
            var firstCustomerLinkTracking = instancesOfCustomerLinkTracking.FirstOrDefault();
            var lastCustomerLinkTracking = instancesOfCustomerLinkTracking.Last();

            // Act, Assert
            firstCustomerLinkTracking.ShouldNotBeNull();
            lastCustomerLinkTracking.ShouldNotBeNull();
            firstCustomerLinkTracking.ShouldNotBeSameAs(lastCustomerLinkTracking);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerLinkTracking_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCustomerLinkTracking = new CustomerLinkTracking();
            var secondCustomerLinkTracking = new CustomerLinkTracking();
            var thirdCustomerLinkTracking = new CustomerLinkTracking();
            var fourthCustomerLinkTracking = new CustomerLinkTracking();
            var fifthCustomerLinkTracking = new CustomerLinkTracking();
            var sixthCustomerLinkTracking = new CustomerLinkTracking();

            // Act, Assert
            firstCustomerLinkTracking.ShouldNotBeNull();
            secondCustomerLinkTracking.ShouldNotBeNull();
            thirdCustomerLinkTracking.ShouldNotBeNull();
            fourthCustomerLinkTracking.ShouldNotBeNull();
            fifthCustomerLinkTracking.ShouldNotBeNull();
            sixthCustomerLinkTracking.ShouldNotBeNull();
            firstCustomerLinkTracking.ShouldNotBeSameAs(secondCustomerLinkTracking);
            thirdCustomerLinkTracking.ShouldNotBeSameAs(firstCustomerLinkTracking);
            fourthCustomerLinkTracking.ShouldNotBeSameAs(firstCustomerLinkTracking);
            fifthCustomerLinkTracking.ShouldNotBeSameAs(firstCustomerLinkTracking);
            sixthCustomerLinkTracking.ShouldNotBeSameAs(firstCustomerLinkTracking);
            sixthCustomerLinkTracking.ShouldNotBeSameAs(fourthCustomerLinkTracking);
        }

        #endregion

        #region General Constructor : Class (CustomerLinkTracking) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerLinkTracking_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var cLTId = -1;

            // Act
            var customerLinkTracking = new CustomerLinkTracking();

            // Assert
            customerLinkTracking.CLTID.ShouldBe(cLTId);
            customerLinkTracking.CustomerID.ShouldBeNull();
            customerLinkTracking.LTPID.ShouldBeNull();
            customerLinkTracking.LTPOID.ShouldBeNull();
            customerLinkTracking.IsActive.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}
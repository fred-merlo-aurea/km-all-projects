using System.Collections.Generic;
using System.Linq;
using Shouldly;
using NUnit.Framework;
using AutoFixture;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Accounts;

namespace ECN_Framework_EntitiesTests.Accounts
{
    [TestFixture]
    public class CustomerDepartmentTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDepartment_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var customerDepartment  = new CustomerDepartment();
            var departmentID = Fixture.Create<int>();
            var customerID = Fixture.Create<int?>();
            var departmentName = Fixture.Create<string>();
            var departmentDesc = Fixture.Create<string>();
            var udList = Fixture.Create<List<UserDepartment>>();

            // Act
            customerDepartment.DepartmentID = departmentID;
            customerDepartment.CustomerID = customerID;
            customerDepartment.DepartmentName = departmentName;
            customerDepartment.DepartmentDesc = departmentDesc;
            customerDepartment.udList = udList;

            // Assert
            customerDepartment.DepartmentID.ShouldBe(departmentID);
            customerDepartment.CustomerID.ShouldBe(customerID);
            customerDepartment.DepartmentName.ShouldBe(departmentName);
            customerDepartment.DepartmentDesc.ShouldBe(departmentDesc);
            customerDepartment.udList.ShouldBe(udList);
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region int property type test : CustomerDepartment => DepartmentID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_DepartmentID_Int_Type_Verify_Test()
        {
            // Arrange
            var customerDepartment = Fixture.Create<CustomerDepartment>();
            var intType = customerDepartment.DepartmentID.GetType();

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
        public void CustomerDepartment_Class_Invalid_Property_DepartmentID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constDepartmentID = "DepartmentID";
            var customerDepartment  = Fixture.Create<CustomerDepartment>();

            // Act , Assert
            Should.NotThrow(() => customerDepartment.GetType().GetProperty(constDepartmentID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_DepartmentID_Is_Present_In_CustomerDepartment_Class_As_Public_Test()
        {
            // Arrange
            const string constDepartmentID = "DepartmentID";
            var customerDepartment  = Fixture.Create<CustomerDepartment>();
            var propertyInfo  = customerDepartment.GetType().GetProperty(constDepartmentID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : CustomerDepartment => CustomerID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Data_Without_Null_Test()
        {
            // Arrange
            var customerDepartment = Fixture.Create<CustomerDepartment>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerDepartment.CustomerID = random;

            // Assert
            customerDepartment.CustomerID.ShouldBe(random);
            customerDepartment.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Only_Null_Data_Test()
        {
            // Arrange
            var customerDepartment = Fixture.Create<CustomerDepartment>();    

            // Act , Set
            customerDepartment.CustomerID = null;

            // Assert
            customerDepartment.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCustomerID = "CustomerID";
            var customerDepartment = Fixture.Create<CustomerDepartment>();
            var propertyInfo = customerDepartment.GetType().GetProperty(constCustomerID);

            // Act , Set
            propertyInfo.SetValue(customerDepartment, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerDepartment.CustomerID.ShouldBeNull();
            customerDepartment.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDepartment_Class_Invalid_Property_CustomerID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var customerDepartment  = Fixture.Create<CustomerDepartment>();

            // Act , Assert
            Should.NotThrow(() => customerDepartment.GetType().GetProperty(constCustomerID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Is_Present_In_CustomerDepartment_Class_As_Public_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var customerDepartment  = Fixture.Create<CustomerDepartment>();
            var propertyInfo  = customerDepartment.GetType().GetProperty(constCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDepartment_Class_Invalid_Property_ErrorList_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constErrorList = "ErrorList";
            var customerDepartment  = Fixture.Create<CustomerDepartment>();

            // Act , Assert
            Should.NotThrow(() => customerDepartment.GetType().GetProperty(constErrorList));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ErrorList_Is_Present_In_CustomerDepartment_Class_As_Public_Test()
        {
            // Arrange
            const string constErrorList = "ErrorList";
            var customerDepartment  = Fixture.Create<CustomerDepartment>();
            var propertyInfo  = customerDepartment.GetType().GetProperty(constErrorList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDepartment_Class_Invalid_Property_udList_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constudList = "udList";
            var customerDepartment  = Fixture.Create<CustomerDepartment>();

            // Act , Assert
            Should.NotThrow(() => customerDepartment.GetType().GetProperty(constudList));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_udList_Is_Present_In_CustomerDepartment_Class_As_Public_Test()
        {
            // Arrange
            const string constudList = "udList";
            var customerDepartment  = Fixture.Create<CustomerDepartment>();
            var propertyInfo  = customerDepartment.GetType().GetProperty(constudList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : CustomerDepartment => DepartmentDesc

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_DepartmentDesc_String_Type_Verify_Test()
        {
            // Arrange
            var customerDepartment = Fixture.Create<CustomerDepartment>();
            var stringType = customerDepartment.DepartmentDesc.GetType();

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
        public void CustomerDepartment_Class_Invalid_Property_DepartmentDesc_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constDepartmentDesc = "DepartmentDesc";
            var customerDepartment  = Fixture.Create<CustomerDepartment>();

            // Act , Assert
            Should.NotThrow(() => customerDepartment.GetType().GetProperty(constDepartmentDesc));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_DepartmentDesc_Is_Present_In_CustomerDepartment_Class_As_Public_Test()
        {
            // Arrange
            const string constDepartmentDesc = "DepartmentDesc";
            var customerDepartment  = Fixture.Create<CustomerDepartment>();
            var propertyInfo  = customerDepartment.GetType().GetProperty(constDepartmentDesc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : CustomerDepartment => DepartmentName

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_DepartmentName_String_Type_Verify_Test()
        {
            // Arrange
            var customerDepartment = Fixture.Create<CustomerDepartment>();
            var stringType = customerDepartment.DepartmentName.GetType();

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
        public void CustomerDepartment_Class_Invalid_Property_DepartmentName_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constDepartmentName = "DepartmentName";
            var customerDepartment  = Fixture.Create<CustomerDepartment>();

            // Act , Assert
            Should.NotThrow(() => customerDepartment.GetType().GetProperty(constDepartmentName));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_DepartmentName_Is_Present_In_CustomerDepartment_Class_As_Public_Test()
        {
            // Arrange
            const string constDepartmentName = "DepartmentName";
            var customerDepartment  = Fixture.Create<CustomerDepartment>();
            var propertyInfo  = customerDepartment.GetType().GetProperty(constDepartmentName);

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
            Should.NotThrow(() => new CustomerDepartment());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<CustomerDepartment>(2).ToList();
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
            var departmentID = -1;
            int? customerID = null;
            var departmentName = string.Empty;
            var departmentDesc = string.Empty;
            List<UserDepartment> udList = null;

            // Act
            var customerDepartment = new CustomerDepartment();    

            // Assert
            customerDepartment.DepartmentID.ShouldBe(departmentID);
            customerDepartment.CustomerID.ShouldBeNull();
            customerDepartment.DepartmentName.ShouldBe(departmentName);
            customerDepartment.DepartmentDesc.ShouldBe(departmentDesc);
            customerDepartment.udList.ShouldBe(udList);
            customerDepartment.ErrorList.ShouldBeEmpty();
        }

        #endregion

        #endregion

        #endregion
    }
}
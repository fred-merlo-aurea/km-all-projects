using System;
using System.Collections.Generic;
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
    public class BillTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Bill_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var bill  = new Bill();
            var billID = Fixture.Create<int>();
            var customerID = Fixture.Create<int>();
            var quoteID = Fixture.Create<int>();
            var createDate = Fixture.Create<DateTime>();
            var source = Fixture.Create<int>();
            var isSyncToQB = Fixture.Create<bool>();
            var errorList = Fixture.Create<List<ECNError>>();

            // Act
            bill.BillID = billID;
            bill.CustomerID = customerID;
            bill.QuoteID = quoteID;
            bill.CreateDate = createDate;
            bill.Source = source;
            bill.IsSyncToQB = isSyncToQB;
            bill.ErrorList = errorList;

            // Assert
            bill.BillID.ShouldBe(billID);
            bill.CustomerID.ShouldBe(customerID);
            bill.QuoteID.ShouldBe(quoteID);
            bill.CreateDate.ShouldBe(createDate);
            bill.Source.ShouldBe(source);
            bill.IsSyncToQB.ShouldBe(isSyncToQB);
            bill.ErrorList.ShouldBe(errorList);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region bool property type test : Bill => IsSyncToQB

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsSyncToQB_Bool_Type_Verify_Test()
        {
            // Arrange
            var bill = Fixture.Create<Bill>();
            var boolType = bill.IsSyncToQB.GetType();

            // Act
            var isTypeBool = typeof(bool).Equals(boolType);    
            var isTypeNullableBool = typeof(bool?).Equals(boolType);
            var isTypeString = typeof(string).Equals(boolType);
            var isTypeInt = typeof(int).Equals(boolType);
            var isTypeDecimal = typeof(decimal).Equals(boolType);
            var isTypeLong = typeof(long).Equals(boolType);
            var isTypeDouble = typeof(double).Equals(boolType);
            var isTypeFloat = typeof(float).Equals(boolType);
            var isTypeIntNullable = typeof(int?).Equals(boolType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(boolType);
            var isTypeLongNullable = typeof(long?).Equals(boolType);
            var isTypeDoubleNullable = typeof(double?).Equals(boolType);
            var isTypeFloatNullable = typeof(float?).Equals(boolType);


            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Bill_Class_Invalid_Property_IsSyncToQB_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsSyncToQB = "IsSyncToQB";
            var bill  = Fixture.Create<Bill>();

            // Act , Assert
            Should.NotThrow(() => bill.GetType().GetProperty(constIsSyncToQB));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsSyncToQB_Is_Present_In_Bill_Class_As_Public_Test()
        {
            // Arrange
            const string constIsSyncToQB = "IsSyncToQB";
            var bill  = Fixture.Create<Bill>();
            var propertyInfo  = bill.GetType().GetProperty(constIsSyncToQB);

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
        public void Bill_Class_Invalid_Property_CreateDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreateDate = "CreateDate";
            var bill  = Fixture.Create<Bill>();

            // Act , Assert
            Should.NotThrow(() => bill.GetType().GetProperty(constCreateDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreateDate_Is_Present_In_Bill_Class_As_Public_Test()
        {
            // Arrange
            const string constCreateDate = "CreateDate";
            var bill  = Fixture.Create<Bill>();
            var propertyInfo  = bill.GetType().GetProperty(constCreateDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Bill => BillID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BillID_Int_Type_Verify_Test()
        {
            // Arrange
            var bill = Fixture.Create<Bill>();
            var intType = bill.BillID.GetType();

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
        public void Bill_Class_Invalid_Property_BillID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constBillID = "BillID";
            var bill  = Fixture.Create<Bill>();

            // Act , Assert
            Should.NotThrow(() => bill.GetType().GetProperty(constBillID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BillID_Is_Present_In_Bill_Class_As_Public_Test()
        {
            // Arrange
            const string constBillID = "BillID";
            var bill  = Fixture.Create<Bill>();
            var propertyInfo  = bill.GetType().GetProperty(constBillID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Bill => CustomerID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Int_Type_Verify_Test()
        {
            // Arrange
            var bill = Fixture.Create<Bill>();
            var intType = bill.CustomerID.GetType();

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
        public void Bill_Class_Invalid_Property_CustomerID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var bill  = Fixture.Create<Bill>();

            // Act , Assert
            Should.NotThrow(() => bill.GetType().GetProperty(constCustomerID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Is_Present_In_Bill_Class_As_Public_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var bill  = Fixture.Create<Bill>();
            var propertyInfo  = bill.GetType().GetProperty(constCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Bill => QuoteID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_QuoteID_Int_Type_Verify_Test()
        {
            // Arrange
            var bill = Fixture.Create<Bill>();
            var intType = bill.QuoteID.GetType();

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
        public void Bill_Class_Invalid_Property_QuoteID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constQuoteID = "QuoteID";
            var bill  = Fixture.Create<Bill>();

            // Act , Assert
            Should.NotThrow(() => bill.GetType().GetProperty(constQuoteID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_QuoteID_Is_Present_In_Bill_Class_As_Public_Test()
        {
            // Arrange
            const string constQuoteID = "QuoteID";
            var bill  = Fixture.Create<Bill>();
            var propertyInfo  = bill.GetType().GetProperty(constQuoteID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Bill => Source

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Source_Int_Type_Verify_Test()
        {
            // Arrange
            var bill = Fixture.Create<Bill>();
            var intType = bill.Source.GetType();

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
        public void Bill_Class_Invalid_Property_Source_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constSource = "Source";
            var bill  = Fixture.Create<Bill>();

            // Act , Assert
            Should.NotThrow(() => bill.GetType().GetProperty(constSource));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Source_Is_Present_In_Bill_Class_As_Public_Test()
        {
            // Arrange
            const string constSource = "Source";
            var bill  = Fixture.Create<Bill>();
            var propertyInfo  = bill.GetType().GetProperty(constSource);

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
        public void Bill_Class_Invalid_Property_ErrorList_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constErrorList = "ErrorList";
            var bill  = Fixture.Create<Bill>();

            // Act , Assert
            Should.NotThrow(() => bill.GetType().GetProperty(constErrorList));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ErrorList_Is_Present_In_Bill_Class_As_Public_Test()
        {
            // Arrange
            const string constErrorList = "ErrorList";
            var bill  = Fixture.Create<Bill>();
            var propertyInfo  = bill.GetType().GetProperty(constErrorList);

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
            Should.NotThrow(() => new Bill());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<Bill>(2).ToList();
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
            var billID = -1;
            var customerID = -1;
            var quoteID = -1;
            var source = -1;
            var isSyncToQB = false;
            var errorList = new List<ECNError>();    

            // Act
            var bill = new Bill();    

            // Assert
            bill.BillID.ShouldBe(billID);
            bill.CustomerID.ShouldBe(customerID);
            bill.QuoteID.ShouldBe(quoteID);
            bill.Source.ShouldBe(source);
            bill.IsSyncToQB.ShouldBeFalse();
            bill.ErrorList.ShouldBeEmpty();
        }

        #endregion

        #endregion

        #endregion
    }
}
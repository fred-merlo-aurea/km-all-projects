using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Shouldly;
using NUnit.Framework;
using AutoFixture;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Accounts;

namespace ECN_Framework_EntitiesTests.Accounts
{
    [TestFixture]
    public class BillItemTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillItem_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var billItem  = new BillItem();
            var billItemID = Fixture.Create<int>();
            var billID = Fixture.Create<int>();
            var quoteItemID = Fixture.Create<int>();
            var changedDate = Fixture.Create<DateTime>();
            var quantity = Fixture.Create<int>();
            var rate = Fixture.Create<double>();
            var startDate = Fixture.Create<DateTime>();
            var endDate = Fixture.Create<DateTime>();
            var status = Fixture.Create<int>();
            var transactionID = Fixture.Create<string>();
            var isProcessedByLicenseEngine = Fixture.Create<bool>();

            // Act
            billItem.BillItemID = billItemID;
            billItem.BillID = billID;
            billItem.QuoteItemID = quoteItemID;
            billItem.ChangedDate = changedDate;
            billItem.Quantity = quantity;
            billItem.Rate = rate;
            billItem.StartDate = startDate;
            billItem.EndDate = endDate;
            billItem.Status = status;
            billItem.TransactionID = transactionID;
            billItem.IsProcessedByLicenseEngine = isProcessedByLicenseEngine;

            // Assert
            billItem.BillItemID.ShouldBe(billItemID);
            billItem.BillID.ShouldBe(billID);
            billItem.QuoteItemID.ShouldBe(quoteItemID);
            billItem.ChangedDate.ShouldBe(changedDate);
            billItem.Quantity.ShouldBe(quantity);
            billItem.Rate.ShouldBe(rate);
            billItem.StartDate.ShouldBe(startDate);
            billItem.EndDate.ShouldBe(endDate);
            billItem.Status.ShouldBe(status);
            billItem.TransactionID.ShouldBe(transactionID);
            billItem.IsProcessedByLicenseEngine.ShouldBe(isProcessedByLicenseEngine);
            billItem.ErrorList.ShouldBeEmpty();
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region bool property type test : BillItem => IsProcessedByLicenseEngine

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsProcessedByLicenseEngine_Bool_Type_Verify_Test()
        {
            // Arrange
            var billItem = Fixture.Create<BillItem>();
            var boolType = billItem.IsProcessedByLicenseEngine.GetType();

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
        public void BillItem_Class_Invalid_Property_IsProcessedByLicenseEngine_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsProcessedByLicenseEngine = "IsProcessedByLicenseEngine";
            var billItem  = Fixture.Create<BillItem>();

            // Act , Assert
            Should.NotThrow(() => billItem.GetType().GetProperty(constIsProcessedByLicenseEngine));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsProcessedByLicenseEngine_Is_Present_In_BillItem_Class_As_Public_Test()
        {
            // Arrange
            const string constIsProcessedByLicenseEngine = "IsProcessedByLicenseEngine";
            var billItem  = Fixture.Create<BillItem>();
            var propertyInfo  = billItem.GetType().GetProperty(constIsProcessedByLicenseEngine);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : BillItem => ChangedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ChangedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constChangedDate = "ChangedDate";
            var billItem = Fixture.Create<BillItem>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = billItem.GetType().GetProperty(constChangedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(billItem, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillItem_Class_Invalid_Property_ChangedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constChangedDate = "ChangedDate";
            var billItem  = Fixture.Create<BillItem>();

            // Act , Assert
            Should.NotThrow(() => billItem.GetType().GetProperty(constChangedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ChangedDate_Is_Present_In_BillItem_Class_As_Public_Test()
        {
            // Arrange
            const string constChangedDate = "ChangedDate";
            var billItem  = Fixture.Create<BillItem>();
            var propertyInfo  = billItem.GetType().GetProperty(constChangedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : BillItem => EndDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_EndDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constEndDate = "EndDate";
            var billItem = Fixture.Create<BillItem>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = billItem.GetType().GetProperty(constEndDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(billItem, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillItem_Class_Invalid_Property_EndDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constEndDate = "EndDate";
            var billItem  = Fixture.Create<BillItem>();

            // Act , Assert
            Should.NotThrow(() => billItem.GetType().GetProperty(constEndDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_EndDate_Is_Present_In_BillItem_Class_As_Public_Test()
        {
            // Arrange
            const string constEndDate = "EndDate";
            var billItem  = Fixture.Create<BillItem>();
            var propertyInfo  = billItem.GetType().GetProperty(constEndDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : BillItem => StartDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_StartDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constStartDate = "StartDate";
            var billItem = Fixture.Create<BillItem>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = billItem.GetType().GetProperty(constStartDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(billItem, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillItem_Class_Invalid_Property_StartDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constStartDate = "StartDate";
            var billItem  = Fixture.Create<BillItem>();

            // Act , Assert
            Should.NotThrow(() => billItem.GetType().GetProperty(constStartDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_StartDate_Is_Present_In_BillItem_Class_As_Public_Test()
        {
            // Arrange
            const string constStartDate = "StartDate";
            var billItem  = Fixture.Create<BillItem>();
            var propertyInfo  = billItem.GetType().GetProperty(constStartDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region double property type test : BillItem => Rate

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Rate_Double_Type_Verify_Test()
        {
            // Arrange
            var billItem = Fixture.Create<BillItem>();
            var doubleType = billItem.Rate.GetType();

            // Act
            var isTypeDouble = typeof(double).Equals(doubleType);    
            var isTypeNullableDouble = typeof(double?).Equals(doubleType);
            var isTypeString = typeof(string).Equals(doubleType);
            var isTypeInt = typeof(int).Equals(doubleType);
            var isTypeDecimal = typeof(decimal).Equals(doubleType);
            var isTypeLong = typeof(long).Equals(doubleType);
            var isTypeBool = typeof(bool).Equals(doubleType);
            var isTypeFloat = typeof(float).Equals(doubleType);
            var isTypeIntNullable = typeof(int?).Equals(doubleType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(doubleType);
            var isTypeLongNullable = typeof(long?).Equals(doubleType);
            var isTypeBoolNullable = typeof(bool?).Equals(doubleType);
            var isTypeFloatNullable = typeof(float?).Equals(doubleType);


            // Assert
            isTypeDouble.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillItem_Class_Invalid_Property_Rate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constRate = "Rate";
            var billItem  = Fixture.Create<BillItem>();

            // Act , Assert
            Should.NotThrow(() => billItem.GetType().GetProperty(constRate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Rate_Is_Present_In_BillItem_Class_As_Public_Test()
        {
            // Arrange
            const string constRate = "Rate";
            var billItem  = Fixture.Create<BillItem>();
            var propertyInfo  = billItem.GetType().GetProperty(constRate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : BillItem => BillID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BillID_Int_Type_Verify_Test()
        {
            // Arrange
            var billItem = Fixture.Create<BillItem>();
            var intType = billItem.BillID.GetType();

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
        public void BillItem_Class_Invalid_Property_BillID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constBillID = "BillID";
            var billItem  = Fixture.Create<BillItem>();

            // Act , Assert
            Should.NotThrow(() => billItem.GetType().GetProperty(constBillID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BillID_Is_Present_In_BillItem_Class_As_Public_Test()
        {
            // Arrange
            const string constBillID = "BillID";
            var billItem  = Fixture.Create<BillItem>();
            var propertyInfo  = billItem.GetType().GetProperty(constBillID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : BillItem => BillItemID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BillItemID_Int_Type_Verify_Test()
        {
            // Arrange
            var billItem = Fixture.Create<BillItem>();
            var intType = billItem.BillItemID.GetType();

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
        public void BillItem_Class_Invalid_Property_BillItemID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constBillItemID = "BillItemID";
            var billItem  = Fixture.Create<BillItem>();

            // Act , Assert
            Should.NotThrow(() => billItem.GetType().GetProperty(constBillItemID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BillItemID_Is_Present_In_BillItem_Class_As_Public_Test()
        {
            // Arrange
            const string constBillItemID = "BillItemID";
            var billItem  = Fixture.Create<BillItem>();
            var propertyInfo  = billItem.GetType().GetProperty(constBillItemID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : BillItem => Quantity

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Quantity_Int_Type_Verify_Test()
        {
            // Arrange
            var billItem = Fixture.Create<BillItem>();
            var intType = billItem.Quantity.GetType();

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
        public void BillItem_Class_Invalid_Property_Quantity_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constQuantity = "Quantity";
            var billItem  = Fixture.Create<BillItem>();

            // Act , Assert
            Should.NotThrow(() => billItem.GetType().GetProperty(constQuantity));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Quantity_Is_Present_In_BillItem_Class_As_Public_Test()
        {
            // Arrange
            const string constQuantity = "Quantity";
            var billItem  = Fixture.Create<BillItem>();
            var propertyInfo  = billItem.GetType().GetProperty(constQuantity);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : BillItem => QuoteItemID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_QuoteItemID_Int_Type_Verify_Test()
        {
            // Arrange
            var billItem = Fixture.Create<BillItem>();
            var intType = billItem.QuoteItemID.GetType();

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
        public void BillItem_Class_Invalid_Property_QuoteItemID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constQuoteItemID = "QuoteItemID";
            var billItem  = Fixture.Create<BillItem>();

            // Act , Assert
            Should.NotThrow(() => billItem.GetType().GetProperty(constQuoteItemID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_QuoteItemID_Is_Present_In_BillItem_Class_As_Public_Test()
        {
            // Arrange
            const string constQuoteItemID = "QuoteItemID";
            var billItem  = Fixture.Create<BillItem>();
            var propertyInfo  = billItem.GetType().GetProperty(constQuoteItemID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : BillItem => Status

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Status_Int_Type_Verify_Test()
        {
            // Arrange
            var billItem = Fixture.Create<BillItem>();
            var intType = billItem.Status.GetType();

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
        public void BillItem_Class_Invalid_Property_Status_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constStatus = "Status";
            var billItem  = Fixture.Create<BillItem>();

            // Act , Assert
            Should.NotThrow(() => billItem.GetType().GetProperty(constStatus));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Status_Is_Present_In_BillItem_Class_As_Public_Test()
        {
            // Arrange
            const string constStatus = "Status";
            var billItem  = Fixture.Create<BillItem>();
            var propertyInfo  = billItem.GetType().GetProperty(constStatus);

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
        public void BillItem_Class_Invalid_Property_ErrorList_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constErrorList = "ErrorList";
            var billItem  = Fixture.Create<BillItem>();

            // Act , Assert
            Should.NotThrow(() => billItem.GetType().GetProperty(constErrorList));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ErrorList_Is_Present_In_BillItem_Class_As_Public_Test()
        {
            // Arrange
            const string constErrorList = "ErrorList";
            var billItem  = Fixture.Create<BillItem>();
            var propertyInfo  = billItem.GetType().GetProperty(constErrorList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : BillItem => TransactionID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_TransactionID_String_Type_Verify_Test()
        {
            // Arrange
            var billItem = Fixture.Create<BillItem>();
            var stringType = billItem.TransactionID.GetType();

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
        public void BillItem_Class_Invalid_Property_TransactionID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constTransactionID = "TransactionID";
            var billItem  = Fixture.Create<BillItem>();

            // Act , Assert
            Should.NotThrow(() => billItem.GetType().GetProperty(constTransactionID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_TransactionID_Is_Present_In_BillItem_Class_As_Public_Test()
        {
            // Arrange
            const string constTransactionID = "TransactionID";
            var billItem  = Fixture.Create<BillItem>();
            var propertyInfo  = billItem.GetType().GetProperty(constTransactionID);

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
            Should.NotThrow(() => new BillItem());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<BillItem>(2).ToList();
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
            var billItemID = -1;
            var billID = -1;
            var quoteItemID = -1;
            var quantity = -1;
            var rate = -1;
            var status = -1;
            var transactionID = string.Empty;
            var isProcessedByLicenseEngine = false;

            // Act
            var billItem = new BillItem();    

            // Assert
            billItem.BillItemID.ShouldBe(billItemID);
            billItem.BillID.ShouldBe(billID);
            billItem.QuoteItemID.ShouldBe(quoteItemID);
            billItem.Quantity.ShouldBe(quantity);
            billItem.Rate.ShouldBe(rate);
            billItem.Status.ShouldBe(status);
            billItem.TransactionID.ShouldBe(transactionID);
            billItem.IsProcessedByLicenseEngine.ShouldBeFalse();
            billItem.ErrorList.ShouldBeEmpty();
        }

        #endregion

        #endregion

        #endregion
    }
}
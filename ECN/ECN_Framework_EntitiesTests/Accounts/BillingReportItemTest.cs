using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Accounts
{
    [TestFixture]
    public class BillingReportItemTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BillingReportItem) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            var baseChannelId = Fixture.Create<int>();
            var baseChannelName = Fixture.Create<string>();
            var customerId = Fixture.Create<int>();
            var customerName = Fixture.Create<string>();
            var blastId = Fixture.Create<int?>();
            var billingItemId = Fixture.Create<int>();
            var billingReportId = Fixture.Create<int>();
            var isFlatRateItem = Fixture.Create<bool>();
            var isMasterFile = Fixture.Create<bool>();
            var isFulfillment = Fixture.Create<bool>();
            var amountOfItems = Fixture.Create<int?>();
            var invoiceText = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool>();
            var amount = Fixture.Create<decimal?>();
            var sendTime = Fixture.Create<DateTime?>();
            var blastField1 = Fixture.Create<string>();
            var blastField2 = Fixture.Create<string>();
            var blastField3 = Fixture.Create<string>();
            var blastField4 = Fixture.Create<string>();
            var blastField5 = Fixture.Create<string>();
            var emailSubject = Fixture.Create<string>();
            var fromName = Fixture.Create<string>();
            var fromEmail = Fixture.Create<string>();
            var groupName = Fixture.Create<string>();

            // Act
            billingReportItem.BaseChannelID = baseChannelId;
            billingReportItem.BaseChannelName = baseChannelName;
            billingReportItem.CustomerID = customerId;
            billingReportItem.CustomerName = customerName;
            billingReportItem.BlastID = blastId;
            billingReportItem.BillingItemID = billingItemId;
            billingReportItem.BillingReportID = billingReportId;
            billingReportItem.IsFlatRateItem = isFlatRateItem;
            billingReportItem.IsMasterFile = isMasterFile;
            billingReportItem.IsFulfillment = isFulfillment;
            billingReportItem.AmountOfItems = amountOfItems;
            billingReportItem.InvoiceText = invoiceText;
            billingReportItem.CreatedUserID = createdUserId;
            billingReportItem.CreatedDate = createdDate;
            billingReportItem.UpdatedUserID = updatedUserId;
            billingReportItem.UpdatedDate = updatedDate;
            billingReportItem.IsDeleted = isDeleted;
            billingReportItem.Amount = amount;
            billingReportItem.SendTime = sendTime;
            billingReportItem.BlastField1 = blastField1;
            billingReportItem.BlastField2 = blastField2;
            billingReportItem.BlastField3 = blastField3;
            billingReportItem.BlastField4 = blastField4;
            billingReportItem.BlastField5 = blastField5;
            billingReportItem.EmailSubject = emailSubject;
            billingReportItem.FromName = fromName;
            billingReportItem.FromEmail = fromEmail;
            billingReportItem.GroupName = groupName;

            // Assert
            billingReportItem.BaseChannelID.ShouldBe(baseChannelId);
            billingReportItem.BaseChannelName.ShouldBe(baseChannelName);
            billingReportItem.CustomerID.ShouldBe(customerId);
            billingReportItem.CustomerName.ShouldBe(customerName);
            billingReportItem.BlastID.ShouldBe(blastId);
            billingReportItem.BillingItemID.ShouldBe(billingItemId);
            billingReportItem.BillingReportID.ShouldBe(billingReportId);
            billingReportItem.IsFlatRateItem.ShouldBe(isFlatRateItem);
            billingReportItem.IsMasterFile.ShouldBe(isMasterFile);
            billingReportItem.IsFulfillment.ShouldBe(isFulfillment);
            billingReportItem.AmountOfItems.ShouldBe(amountOfItems);
            billingReportItem.InvoiceText.ShouldBe(invoiceText);
            billingReportItem.CreatedUserID.ShouldBe(createdUserId);
            billingReportItem.CreatedDate.ShouldBe(createdDate);
            billingReportItem.UpdatedUserID.ShouldBe(updatedUserId);
            billingReportItem.UpdatedDate.ShouldBe(updatedDate);
            billingReportItem.IsDeleted.ShouldBe(isDeleted);
            billingReportItem.Amount.ShouldBe(amount);
            billingReportItem.SendTime.ShouldBe(sendTime);
            billingReportItem.BlastField1.ShouldBe(blastField1);
            billingReportItem.BlastField2.ShouldBe(blastField2);
            billingReportItem.BlastField3.ShouldBe(blastField3);
            billingReportItem.BlastField4.ShouldBe(blastField4);
            billingReportItem.BlastField5.ShouldBe(blastField5);
            billingReportItem.EmailSubject.ShouldBe(emailSubject);
            billingReportItem.FromName.ShouldBe(fromName);
            billingReportItem.FromEmail.ShouldBe(fromEmail);
            billingReportItem.GroupName.ShouldBe(groupName);
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (Amount) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Amount_Property_Data_Without_Null_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            var random = Fixture.Create<decimal>();

            // Act , Set
            billingReportItem.Amount = random;

            // Assert
            billingReportItem.Amount.ShouldBe(random);
            billingReportItem.Amount.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Amount_Property_Only_Null_Data_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();    

            // Act , Set
            billingReportItem.Amount = null;

            // Assert
            billingReportItem.Amount.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Amount_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameAmount = "Amount";
            var billingReportItem = Fixture.Create<BillingReportItem>();
            var propertyInfo = billingReportItem.GetType().GetProperty(propertyNameAmount);

            // Act , Set
            propertyInfo.SetValue(billingReportItem, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            billingReportItem.Amount.ShouldBeNull();
            billingReportItem.Amount.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (Amount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_AmountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAmount = "AmountNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameAmount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Amount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAmount = "Amount";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameAmount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (AmountOfItems) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_AmountOfItems_Property_Data_Without_Null_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            var random = Fixture.Create<int>();

            // Act , Set
            billingReportItem.AmountOfItems = random;

            // Assert
            billingReportItem.AmountOfItems.ShouldBe(random);
            billingReportItem.AmountOfItems.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_AmountOfItems_Property_Only_Null_Data_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();    

            // Act , Set
            billingReportItem.AmountOfItems = null;

            // Assert
            billingReportItem.AmountOfItems.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_AmountOfItems_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameAmountOfItems = "AmountOfItems";
            var billingReportItem = Fixture.Create<BillingReportItem>();
            var propertyInfo = billingReportItem.GetType().GetProperty(propertyNameAmountOfItems);

            // Act , Set
            propertyInfo.SetValue(billingReportItem, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            billingReportItem.AmountOfItems.ShouldBeNull();
            billingReportItem.AmountOfItems.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (AmountOfItems) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_AmountOfItemsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAmountOfItems = "AmountOfItemsNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameAmountOfItems));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_AmountOfItems_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAmountOfItems = "AmountOfItems";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameAmountOfItems);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (BaseChannelID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BaseChannelID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.BaseChannelID = Fixture.Create<int>();
            var intType = billingReportItem.BaseChannelID.GetType();

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

        #region General Getters/Setters : Class (BillingReportItem) => Property (BaseChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_BaseChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelIDNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameBaseChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BaseChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelID";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (BaseChannelName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BaseChannelName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.BaseChannelName = Fixture.Create<string>();
            var stringType = billingReportItem.BaseChannelName.GetType();

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

        #region General Getters/Setters : Class (BillingReportItem) => Property (BaseChannelName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_BaseChannelNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelName = "BaseChannelNameNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameBaseChannelName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BaseChannelName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelName = "BaseChannelName";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameBaseChannelName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (BillingItemID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BillingItemID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.BillingItemID = Fixture.Create<int>();
            var intType = billingReportItem.BillingItemID.GetType();

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

        #region General Getters/Setters : Class (BillingReportItem) => Property (BillingItemID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_BillingItemIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBillingItemID = "BillingItemIDNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameBillingItemID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BillingItemID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBillingItemID = "BillingItemID";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameBillingItemID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (BillingReportID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BillingReportID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.BillingReportID = Fixture.Create<int>();
            var intType = billingReportItem.BillingReportID.GetType();

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

        #region General Getters/Setters : Class (BillingReportItem) => Property (BillingReportID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_BillingReportIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBillingReportID = "BillingReportIDNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameBillingReportID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BillingReportID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBillingReportID = "BillingReportID";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameBillingReportID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (BlastField1) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BlastField1_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.BlastField1 = Fixture.Create<string>();
            var stringType = billingReportItem.BlastField1.GetType();

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

        #region General Getters/Setters : Class (BillingReportItem) => Property (BlastField1) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_BlastField1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastField1 = "BlastField1NotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameBlastField1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BlastField1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastField1 = "BlastField1";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameBlastField1);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (BlastField2) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BlastField2_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.BlastField2 = Fixture.Create<string>();
            var stringType = billingReportItem.BlastField2.GetType();

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

        #region General Getters/Setters : Class (BillingReportItem) => Property (BlastField2) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_BlastField2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastField2 = "BlastField2NotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameBlastField2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BlastField2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastField2 = "BlastField2";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameBlastField2);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (BlastField3) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BlastField3_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.BlastField3 = Fixture.Create<string>();
            var stringType = billingReportItem.BlastField3.GetType();

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

        #region General Getters/Setters : Class (BillingReportItem) => Property (BlastField3) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_BlastField3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastField3 = "BlastField3NotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameBlastField3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BlastField3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastField3 = "BlastField3";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameBlastField3);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (BlastField4) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BlastField4_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.BlastField4 = Fixture.Create<string>();
            var stringType = billingReportItem.BlastField4.GetType();

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

        #region General Getters/Setters : Class (BillingReportItem) => Property (BlastField4) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_BlastField4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastField4 = "BlastField4NotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameBlastField4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BlastField4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastField4 = "BlastField4";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameBlastField4);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (BlastField5) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BlastField5_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.BlastField5 = Fixture.Create<string>();
            var stringType = billingReportItem.BlastField5.GetType();

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

        #region General Getters/Setters : Class (BillingReportItem) => Property (BlastField5) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_BlastField5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastField5 = "BlastField5NotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameBlastField5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BlastField5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastField5 = "BlastField5";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameBlastField5);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (BlastID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            var random = Fixture.Create<int>();

            // Act , Set
            billingReportItem.BlastID = random;

            // Assert
            billingReportItem.BlastID.ShouldBe(random);
            billingReportItem.BlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();    

            // Act , Set
            billingReportItem.BlastID = null;

            // Assert
            billingReportItem.BlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastID = "BlastID";
            var billingReportItem = Fixture.Create<BillingReportItem>();
            var propertyInfo = billingReportItem.GetType().GetProperty(propertyNameBlastID);

            // Act , Set
            propertyInfo.SetValue(billingReportItem, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            billingReportItem.BlastID.ShouldBeNull();
            billingReportItem.BlastID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var billingReportItem = Fixture.Create<BillingReportItem>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = billingReportItem.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(billingReportItem, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            var random = Fixture.Create<int>();

            // Act , Set
            billingReportItem.CreatedUserID = random;

            // Assert
            billingReportItem.CreatedUserID.ShouldBe(random);
            billingReportItem.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();    

            // Act , Set
            billingReportItem.CreatedUserID = null;

            // Assert
            billingReportItem.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var billingReportItem = Fixture.Create<BillingReportItem>();
            var propertyInfo = billingReportItem.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(billingReportItem, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            billingReportItem.CreatedUserID.ShouldBeNull();
            billingReportItem.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.CustomerID = Fixture.Create<int>();
            var intType = billingReportItem.CustomerID.GetType();

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

        #region General Getters/Setters : Class (BillingReportItem) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (CustomerName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_CustomerName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.CustomerName = Fixture.Create<string>();
            var stringType = billingReportItem.CustomerName.GetType();

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

        #region General Getters/Setters : Class (BillingReportItem) => Property (CustomerName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_CustomerNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerNameNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameCustomerName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_CustomerName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerName";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameCustomerName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.EmailSubject = Fixture.Create<string>();
            var stringType = billingReportItem.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (BillingReportItem) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (FromEmail) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_FromEmail_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.FromEmail = Fixture.Create<string>();
            var stringType = billingReportItem.FromEmail.GetType();

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

        #region General Getters/Setters : Class (BillingReportItem) => Property (FromEmail) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_FromEmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFromEmail = "FromEmailNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameFromEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_FromEmail_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFromEmail = "FromEmail";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameFromEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (FromName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_FromName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.FromName = Fixture.Create<string>();
            var stringType = billingReportItem.FromName.GetType();

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

        #region General Getters/Setters : Class (BillingReportItem) => Property (FromName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_FromNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFromName = "FromNameNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameFromName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_FromName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFromName = "FromName";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameFromName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (GroupName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_GroupName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.GroupName = Fixture.Create<string>();
            var stringType = billingReportItem.GroupName.GetType();

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

        #region General Getters/Setters : Class (BillingReportItem) => Property (GroupName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_GroupNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupNameNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameGroupName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_GroupName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupName";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameGroupName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (InvoiceText) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_InvoiceText_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.InvoiceText = Fixture.Create<string>();
            var stringType = billingReportItem.InvoiceText.GetType();

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

        #region General Getters/Setters : Class (BillingReportItem) => Property (InvoiceText) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_InvoiceTextNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameInvoiceText = "InvoiceTextNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameInvoiceText));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_InvoiceText_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameInvoiceText = "InvoiceText";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameInvoiceText);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (IsDeleted) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_IsDeleted_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.IsDeleted = Fixture.Create<bool>();
            var boolType = billingReportItem.IsDeleted.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
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

        #region General Getters/Setters : Class (BillingReportItem) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (IsFlatRateItem) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_IsFlatRateItem_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.IsFlatRateItem = Fixture.Create<bool>();
            var boolType = billingReportItem.IsFlatRateItem.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
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

        #region General Getters/Setters : Class (BillingReportItem) => Property (IsFlatRateItem) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_IsFlatRateItemNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsFlatRateItem = "IsFlatRateItemNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameIsFlatRateItem));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_IsFlatRateItem_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsFlatRateItem = "IsFlatRateItem";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameIsFlatRateItem);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (IsFulfillment) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_IsFulfillment_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.IsFulfillment = Fixture.Create<bool>();
            var boolType = billingReportItem.IsFulfillment.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
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

        #region General Getters/Setters : Class (BillingReportItem) => Property (IsFulfillment) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_IsFulfillmentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsFulfillment = "IsFulfillmentNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameIsFulfillment));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_IsFulfillment_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsFulfillment = "IsFulfillment";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameIsFulfillment);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (IsMasterFile) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_IsMasterFile_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            billingReportItem.IsMasterFile = Fixture.Create<bool>();
            var boolType = billingReportItem.IsMasterFile.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
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

        #region General Getters/Setters : Class (BillingReportItem) => Property (IsMasterFile) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_IsMasterFileNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsMasterFile = "IsMasterFileNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameIsMasterFile));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_IsMasterFile_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsMasterFile = "IsMasterFile";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameIsMasterFile);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (SendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_SendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var billingReportItem = Fixture.Create<BillingReportItem>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = billingReportItem.GetType().GetProperty(propertyNameSendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(billingReportItem, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (SendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_SendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTimeNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameSendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_SendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameSendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var billingReportItem = Fixture.Create<BillingReportItem>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = billingReportItem.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(billingReportItem, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();
            var random = Fixture.Create<int>();

            // Act , Set
            billingReportItem.UpdatedUserID = random;

            // Assert
            billingReportItem.UpdatedUserID.ShouldBe(random);
            billingReportItem.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var billingReportItem = Fixture.Create<BillingReportItem>();    

            // Act , Set
            billingReportItem.UpdatedUserID = null;

            // Assert
            billingReportItem.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var billingReportItem = Fixture.Create<BillingReportItem>();
            var propertyInfo = billingReportItem.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(billingReportItem, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            billingReportItem.UpdatedUserID.ShouldBeNull();
            billingReportItem.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BillingReportItem) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var billingReportItem  = Fixture.Create<BillingReportItem>();

            // Act , Assert
            Should.NotThrow(() => billingReportItem.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReportItem_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var billingReportItem  = Fixture.Create<BillingReportItem>();
            var propertyInfo  = billingReportItem.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BillingReportItem) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BillingReportItem_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BillingReportItem());
        }

        #endregion

        #region General Constructor : Class (BillingReportItem) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BillingReportItem_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBillingReportItem = Fixture.CreateMany<BillingReportItem>(2).ToList();
            var firstBillingReportItem = instancesOfBillingReportItem.FirstOrDefault();
            var lastBillingReportItem = instancesOfBillingReportItem.Last();

            // Act, Assert
            firstBillingReportItem.ShouldNotBeNull();
            lastBillingReportItem.ShouldNotBeNull();
            firstBillingReportItem.ShouldNotBeSameAs(lastBillingReportItem);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BillingReportItem_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBillingReportItem = new BillingReportItem();
            var secondBillingReportItem = new BillingReportItem();
            var thirdBillingReportItem = new BillingReportItem();
            var fourthBillingReportItem = new BillingReportItem();
            var fifthBillingReportItem = new BillingReportItem();
            var sixthBillingReportItem = new BillingReportItem();

            // Act, Assert
            firstBillingReportItem.ShouldNotBeNull();
            secondBillingReportItem.ShouldNotBeNull();
            thirdBillingReportItem.ShouldNotBeNull();
            fourthBillingReportItem.ShouldNotBeNull();
            fifthBillingReportItem.ShouldNotBeNull();
            sixthBillingReportItem.ShouldNotBeNull();
            firstBillingReportItem.ShouldNotBeSameAs(secondBillingReportItem);
            thirdBillingReportItem.ShouldNotBeSameAs(firstBillingReportItem);
            fourthBillingReportItem.ShouldNotBeSameAs(firstBillingReportItem);
            fifthBillingReportItem.ShouldNotBeSameAs(firstBillingReportItem);
            sixthBillingReportItem.ShouldNotBeSameAs(firstBillingReportItem);
            sixthBillingReportItem.ShouldNotBeSameAs(fourthBillingReportItem);
        }

        #endregion

        #region General Constructor : Class (BillingReportItem) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BillingReportItem_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var billingItemId = -1;
            var billingReportId = -1;
            var baseChannelId = -1;
            var baseChannelName = string.Empty;
            var customerId = -1;
            var customerName = string.Empty;
            var isFlatRateItem = false;
            var isMasterFile = false;
            var isFulfillment = false;
            var invoiceText = string.Empty;
            var isDeleted = false;
            var groupName = string.Empty;

            // Act
            var billingReportItem = new BillingReportItem();

            // Assert
            billingReportItem.BillingItemID.ShouldBe(billingItemId);
            billingReportItem.BillingReportID.ShouldBe(billingReportId);
            billingReportItem.BaseChannelID.ShouldBe(baseChannelId);
            billingReportItem.BaseChannelName.ShouldBe(baseChannelName);
            billingReportItem.BlastID.ShouldBeNull();
            billingReportItem.CustomerID.ShouldBe(customerId);
            billingReportItem.CustomerName.ShouldBe(customerName);
            billingReportItem.IsFlatRateItem.ShouldBe(isFlatRateItem);
            billingReportItem.IsMasterFile.ShouldBe(isMasterFile);
            billingReportItem.IsFulfillment.ShouldBe(isFulfillment);
            billingReportItem.AmountOfItems.ShouldBeNull();
            billingReportItem.InvoiceText.ShouldBe(invoiceText);
            billingReportItem.CreatedDate.ShouldBeNull();
            billingReportItem.CreatedUserID.ShouldBeNull();
            billingReportItem.UpdatedDate.ShouldBeNull();
            billingReportItem.UpdatedUserID.ShouldBeNull();
            billingReportItem.IsDeleted.ShouldBe(isDeleted);
            billingReportItem.Amount.ShouldBeNull();
            billingReportItem.SendTime.ShouldBeNull();
            billingReportItem.BlastField1.ShouldBeNull();
            billingReportItem.GroupName.ShouldBe(groupName);
        }

        #endregion

        #endregion

        #endregion
    }
}
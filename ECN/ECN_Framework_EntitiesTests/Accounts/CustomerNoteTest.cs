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
    public class CustomerNoteTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerNote_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var customerNote  = new CustomerNote();
            var noteID = Fixture.Create<int>();
            var customerID = Fixture.Create<int>();
            var notes = Fixture.Create<string>();
            var isBillingNotes = Fixture.Create<bool?>();
            var createdBy = Fixture.Create<string>();
            var updatedBy = Fixture.Create<string>();
            var createdUserID = Fixture.Create<int?>();
            var updatedUserID = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            customerNote.NoteID = noteID;
            customerNote.CustomerID = customerID;
            customerNote.Notes = notes;
            customerNote.IsBillingNotes = isBillingNotes;
            customerNote.CreatedBy = createdBy;
            customerNote.UpdatedBy = updatedBy;
            customerNote.CreatedUserID = createdUserID;
            customerNote.UpdatedUserID = updatedUserID;
            customerNote.CreatedDate = createdDate;
            customerNote.UpdatedDate = updatedDate;
            customerNote.IsDeleted = isDeleted;

            // Assert
            customerNote.NoteID.ShouldBe(noteID);
            customerNote.CustomerID.ShouldBe(customerID);
            customerNote.Notes.ShouldBe(notes);
            customerNote.IsBillingNotes.ShouldBe(isBillingNotes);
            customerNote.CreatedBy.ShouldBe(createdBy);
            customerNote.UpdatedBy.ShouldBe(updatedBy);
            customerNote.CreatedUserID.ShouldBe(createdUserID);
            customerNote.UpdatedUserID.ShouldBe(updatedUserID);
            customerNote.CreatedDate.ShouldBe(createdDate);
            customerNote.UpdatedDate.ShouldBe(updatedDate);
            customerNote.IsDeleted.ShouldBe(isDeleted);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Nullable Property Test : CustomerNote => IsBillingNotes

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsBillingNotes_Data_Without_Null_Test()
        {
            // Arrange
            var customerNote = Fixture.Create<CustomerNote>();
            var random = Fixture.Create<bool>();

            // Act , Set
            customerNote.IsBillingNotes = random;

            // Assert
            customerNote.IsBillingNotes.ShouldBe(random);
            customerNote.IsBillingNotes.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsBillingNotes_Only_Null_Data_Test()
        {
            // Arrange
            var customerNote = Fixture.Create<CustomerNote>();    

            // Act , Set
            customerNote.IsBillingNotes = null;

            // Assert
            customerNote.IsBillingNotes.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsBillingNotes_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsBillingNotes = "IsBillingNotes";
            var customerNote = Fixture.Create<CustomerNote>();
            var propertyInfo = customerNote.GetType().GetProperty(constIsBillingNotes);

            // Act , Set
            propertyInfo.SetValue(customerNote, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerNote.IsBillingNotes.ShouldBeNull();
            customerNote.IsBillingNotes.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerNote_Class_Invalid_Property_IsBillingNotes_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsBillingNotes = "IsBillingNotes";
            var customerNote  = Fixture.Create<CustomerNote>();

            // Act , Assert
            Should.NotThrow(() => customerNote.GetType().GetProperty(constIsBillingNotes));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsBillingNotes_Is_Present_In_CustomerNote_Class_As_Public_Test()
        {
            // Arrange
            const string constIsBillingNotes = "IsBillingNotes";
            var customerNote  = Fixture.Create<CustomerNote>();
            var propertyInfo  = customerNote.GetType().GetProperty(constIsBillingNotes);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : CustomerNote => IsDeleted

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Data_Without_Null_Test()
        {
            // Arrange
            var customerNote = Fixture.Create<CustomerNote>();
            var random = Fixture.Create<bool>();

            // Act , Set
            customerNote.IsDeleted = random;

            // Assert
            customerNote.IsDeleted.ShouldBe(random);
            customerNote.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Only_Null_Data_Test()
        {
            // Arrange
            var customerNote = Fixture.Create<CustomerNote>();    

            // Act , Set
            customerNote.IsDeleted = null;

            // Assert
            customerNote.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsDeleted = "IsDeleted";
            var customerNote = Fixture.Create<CustomerNote>();
            var propertyInfo = customerNote.GetType().GetProperty(constIsDeleted);

            // Act , Set
            propertyInfo.SetValue(customerNote, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerNote.IsDeleted.ShouldBeNull();
            customerNote.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerNote_Class_Invalid_Property_IsDeleted_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var customerNote  = Fixture.Create<CustomerNote>();

            // Act , Assert
            Should.NotThrow(() => customerNote.GetType().GetProperty(constIsDeleted));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Is_Present_In_CustomerNote_Class_As_Public_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var customerNote  = Fixture.Create<CustomerNote>();
            var propertyInfo  = customerNote.GetType().GetProperty(constIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : CustomerNote => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var customerNote = Fixture.Create<CustomerNote>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerNote.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerNote, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerNote_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var customerNote  = Fixture.Create<CustomerNote>();

            // Act , Assert
            Should.NotThrow(() => customerNote.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_CustomerNote_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var customerNote  = Fixture.Create<CustomerNote>();
            var propertyInfo  = customerNote.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : CustomerNote => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var customerNote = Fixture.Create<CustomerNote>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerNote.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerNote, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerNote_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var customerNote  = Fixture.Create<CustomerNote>();

            // Act , Assert
            Should.NotThrow(() => customerNote.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_CustomerNote_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var customerNote  = Fixture.Create<CustomerNote>();
            var propertyInfo  = customerNote.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : CustomerNote => CustomerID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Int_Type_Verify_Test()
        {
            // Arrange
            var customerNote = Fixture.Create<CustomerNote>();
            var intType = customerNote.CustomerID.GetType();

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
        public void CustomerNote_Class_Invalid_Property_CustomerID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var customerNote  = Fixture.Create<CustomerNote>();

            // Act , Assert
            Should.NotThrow(() => customerNote.GetType().GetProperty(constCustomerID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Is_Present_In_CustomerNote_Class_As_Public_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var customerNote  = Fixture.Create<CustomerNote>();
            var propertyInfo  = customerNote.GetType().GetProperty(constCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : CustomerNote => NoteID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_NoteID_Int_Type_Verify_Test()
        {
            // Arrange
            var customerNote = Fixture.Create<CustomerNote>();
            var intType = customerNote.NoteID.GetType();

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
        public void CustomerNote_Class_Invalid_Property_NoteID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constNoteID = "NoteID";
            var customerNote  = Fixture.Create<CustomerNote>();

            // Act , Assert
            Should.NotThrow(() => customerNote.GetType().GetProperty(constNoteID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_NoteID_Is_Present_In_CustomerNote_Class_As_Public_Test()
        {
            // Arrange
            const string constNoteID = "NoteID";
            var customerNote  = Fixture.Create<CustomerNote>();
            var propertyInfo  = customerNote.GetType().GetProperty(constNoteID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : CustomerNote => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var customerNote = Fixture.Create<CustomerNote>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerNote.CreatedUserID = random;

            // Assert
            customerNote.CreatedUserID.ShouldBe(random);
            customerNote.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var customerNote = Fixture.Create<CustomerNote>();    

            // Act , Set
            customerNote.CreatedUserID = null;

            // Assert
            customerNote.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var customerNote = Fixture.Create<CustomerNote>();
            var propertyInfo = customerNote.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(customerNote, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerNote.CreatedUserID.ShouldBeNull();
            customerNote.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerNote_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var customerNote  = Fixture.Create<CustomerNote>();

            // Act , Assert
            Should.NotThrow(() => customerNote.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_CustomerNote_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var customerNote  = Fixture.Create<CustomerNote>();
            var propertyInfo  = customerNote.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : CustomerNote => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var customerNote = Fixture.Create<CustomerNote>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerNote.UpdatedUserID = random;

            // Assert
            customerNote.UpdatedUserID.ShouldBe(random);
            customerNote.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var customerNote = Fixture.Create<CustomerNote>();    

            // Act , Set
            customerNote.UpdatedUserID = null;

            // Assert
            customerNote.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var customerNote = Fixture.Create<CustomerNote>();
            var propertyInfo = customerNote.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(customerNote, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerNote.UpdatedUserID.ShouldBeNull();
            customerNote.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerNote_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var customerNote  = Fixture.Create<CustomerNote>();

            // Act , Assert
            Should.NotThrow(() => customerNote.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_CustomerNote_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var customerNote  = Fixture.Create<CustomerNote>();
            var propertyInfo  = customerNote.GetType().GetProperty(constUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : CustomerNote => CreatedBy

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedBy_String_Type_Verify_Test()
        {
            // Arrange
            var customerNote = Fixture.Create<CustomerNote>();
            var stringType = customerNote.CreatedBy.GetType();

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
        public void CustomerNote_Class_Invalid_Property_CreatedBy_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedBy = "CreatedBy";
            var customerNote  = Fixture.Create<CustomerNote>();

            // Act , Assert
            Should.NotThrow(() => customerNote.GetType().GetProperty(constCreatedBy));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedBy_Is_Present_In_CustomerNote_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedBy = "CreatedBy";
            var customerNote  = Fixture.Create<CustomerNote>();
            var propertyInfo  = customerNote.GetType().GetProperty(constCreatedBy);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : CustomerNote => Notes

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Notes_String_Type_Verify_Test()
        {
            // Arrange
            var customerNote = Fixture.Create<CustomerNote>();
            var stringType = customerNote.Notes.GetType();

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
        public void CustomerNote_Class_Invalid_Property_Notes_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constNotes = "Notes";
            var customerNote  = Fixture.Create<CustomerNote>();

            // Act , Assert
            Should.NotThrow(() => customerNote.GetType().GetProperty(constNotes));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Notes_Is_Present_In_CustomerNote_Class_As_Public_Test()
        {
            // Arrange
            const string constNotes = "Notes";
            var customerNote  = Fixture.Create<CustomerNote>();
            var propertyInfo  = customerNote.GetType().GetProperty(constNotes);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : CustomerNote => UpdatedBy

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedBy_String_Type_Verify_Test()
        {
            // Arrange
            var customerNote = Fixture.Create<CustomerNote>();
            var stringType = customerNote.UpdatedBy.GetType();

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
        public void CustomerNote_Class_Invalid_Property_UpdatedBy_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedBy = "UpdatedBy";
            var customerNote  = Fixture.Create<CustomerNote>();

            // Act , Assert
            Should.NotThrow(() => customerNote.GetType().GetProperty(constUpdatedBy));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedBy_Is_Present_In_CustomerNote_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedBy = "UpdatedBy";
            var customerNote  = Fixture.Create<CustomerNote>();
            var propertyInfo  = customerNote.GetType().GetProperty(constUpdatedBy);

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
            Should.NotThrow(() => new CustomerNote());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<CustomerNote>(2).ToList();
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
            var noteID = -1;
            var customerID = -1;
            var notes = string.Empty;
            bool? isBillingNotes = null;
            var createdBy = string.Empty;
            var updatedBy = string.Empty;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            bool? isDeleted = null;    

            // Act
            var customerNote = new CustomerNote();    

            // Assert
            customerNote.NoteID.ShouldBe(noteID);
            customerNote.CustomerID.ShouldBe(customerID);
            customerNote.Notes.ShouldBe(notes);
            customerNote.IsBillingNotes.ShouldBeNull();
            customerNote.CreatedBy.ShouldBe(createdBy);
            customerNote.UpdatedBy.ShouldBe(updatedBy);
            customerNote.CreatedUserID.ShouldBeNull();
            customerNote.CreatedDate.ShouldBeNull();
            customerNote.UpdatedUserID.ShouldBeNull();
            customerNote.UpdatedDate.ShouldBeNull();
            customerNote.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}
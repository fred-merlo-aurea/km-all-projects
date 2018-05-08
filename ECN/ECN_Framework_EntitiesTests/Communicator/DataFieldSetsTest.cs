using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class DataFieldSetsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (DataFieldSets) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataFieldSets_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var dataFieldSets = Fixture.Create<DataFieldSets>();
            var dataFieldSetId = Fixture.Create<int>();
            var groupId = Fixture.Create<int>();
            var multivaluedYN = Fixture.Create<string>();
            var name = Fixture.Create<string>();
            var errorList = Fixture.Create<List<ECNError>>();

            // Act
            dataFieldSets.DataFieldSetID = dataFieldSetId;
            dataFieldSets.GroupID = groupId;
            dataFieldSets.MultivaluedYN = multivaluedYN;
            dataFieldSets.Name = name;
            dataFieldSets.ErrorList = errorList;

            // Assert
            dataFieldSets.DataFieldSetID.ShouldBe(dataFieldSetId);
            dataFieldSets.GroupID.ShouldBe(groupId);
            dataFieldSets.MultivaluedYN.ShouldBe(multivaluedYN);
            dataFieldSets.Name.ShouldBe(name);
            dataFieldSets.ErrorList.ShouldBe(errorList);
        }

        #endregion

        #region General Getters/Setters : Class (DataFieldSets) => Property (DataFieldSetID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataFieldSets_DataFieldSetID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataFieldSets = Fixture.Create<DataFieldSets>();
            dataFieldSets.DataFieldSetID = Fixture.Create<int>();
            var intType = dataFieldSets.DataFieldSetID.GetType();

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

        #region General Getters/Setters : Class (DataFieldSets) => Property (DataFieldSetID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataFieldSets_Class_Invalid_Property_DataFieldSetIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDataFieldSetID = "DataFieldSetIDNotPresent";
            var dataFieldSets  = Fixture.Create<DataFieldSets>();

            // Act , Assert
            Should.NotThrow(() => dataFieldSets.GetType().GetProperty(propertyNameDataFieldSetID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataFieldSets_DataFieldSetID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDataFieldSetID = "DataFieldSetID";
            var dataFieldSets  = Fixture.Create<DataFieldSets>();
            var propertyInfo  = dataFieldSets.GetType().GetProperty(propertyNameDataFieldSetID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataFieldSets) => Property (ErrorList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataFieldSets_Class_Invalid_Property_ErrorListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameErrorList = "ErrorListNotPresent";
            var dataFieldSets  = Fixture.Create<DataFieldSets>();

            // Act , Assert
            Should.NotThrow(() => dataFieldSets.GetType().GetProperty(propertyNameErrorList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataFieldSets_ErrorList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameErrorList = "ErrorList";
            var dataFieldSets  = Fixture.Create<DataFieldSets>();
            var propertyInfo  = dataFieldSets.GetType().GetProperty(propertyNameErrorList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataFieldSets) => Property (GroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataFieldSets_GroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataFieldSets = Fixture.Create<DataFieldSets>();
            dataFieldSets.GroupID = Fixture.Create<int>();
            var intType = dataFieldSets.GroupID.GetType();

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

        #region General Getters/Setters : Class (DataFieldSets) => Property (GroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataFieldSets_Class_Invalid_Property_GroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupIDNotPresent";
            var dataFieldSets  = Fixture.Create<DataFieldSets>();

            // Act , Assert
            Should.NotThrow(() => dataFieldSets.GetType().GetProperty(propertyNameGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataFieldSets_GroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupID";
            var dataFieldSets  = Fixture.Create<DataFieldSets>();
            var propertyInfo  = dataFieldSets.GetType().GetProperty(propertyNameGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataFieldSets) => Property (MultivaluedYN) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataFieldSets_MultivaluedYN_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataFieldSets = Fixture.Create<DataFieldSets>();
            dataFieldSets.MultivaluedYN = Fixture.Create<string>();
            var stringType = dataFieldSets.MultivaluedYN.GetType();

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

        #region General Getters/Setters : Class (DataFieldSets) => Property (MultivaluedYN) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataFieldSets_Class_Invalid_Property_MultivaluedYNNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMultivaluedYN = "MultivaluedYNNotPresent";
            var dataFieldSets  = Fixture.Create<DataFieldSets>();

            // Act , Assert
            Should.NotThrow(() => dataFieldSets.GetType().GetProperty(propertyNameMultivaluedYN));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataFieldSets_MultivaluedYN_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMultivaluedYN = "MultivaluedYN";
            var dataFieldSets  = Fixture.Create<DataFieldSets>();
            var propertyInfo  = dataFieldSets.GetType().GetProperty(propertyNameMultivaluedYN);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataFieldSets) => Property (Name) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataFieldSets_Name_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataFieldSets = Fixture.Create<DataFieldSets>();
            dataFieldSets.Name = Fixture.Create<string>();
            var stringType = dataFieldSets.Name.GetType();

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

        #region General Getters/Setters : Class (DataFieldSets) => Property (Name) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataFieldSets_Class_Invalid_Property_NameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameName = "NameNotPresent";
            var dataFieldSets  = Fixture.Create<DataFieldSets>();

            // Act , Assert
            Should.NotThrow(() => dataFieldSets.GetType().GetProperty(propertyNameName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataFieldSets_Name_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameName = "Name";
            var dataFieldSets  = Fixture.Create<DataFieldSets>();
            var propertyInfo  = dataFieldSets.GetType().GetProperty(propertyNameName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (DataFieldSets) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DataFieldSets_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new DataFieldSets());
        }

        #endregion

        #region General Constructor : Class (DataFieldSets) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DataFieldSets_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfDataFieldSets = Fixture.CreateMany<DataFieldSets>(2).ToList();
            var firstDataFieldSets = instancesOfDataFieldSets.FirstOrDefault();
            var lastDataFieldSets = instancesOfDataFieldSets.Last();

            // Act, Assert
            firstDataFieldSets.ShouldNotBeNull();
            lastDataFieldSets.ShouldNotBeNull();
            firstDataFieldSets.ShouldNotBeSameAs(lastDataFieldSets);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DataFieldSets_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstDataFieldSets = new DataFieldSets();
            var secondDataFieldSets = new DataFieldSets();
            var thirdDataFieldSets = new DataFieldSets();
            var fourthDataFieldSets = new DataFieldSets();
            var fifthDataFieldSets = new DataFieldSets();
            var sixthDataFieldSets = new DataFieldSets();

            // Act, Assert
            firstDataFieldSets.ShouldNotBeNull();
            secondDataFieldSets.ShouldNotBeNull();
            thirdDataFieldSets.ShouldNotBeNull();
            fourthDataFieldSets.ShouldNotBeNull();
            fifthDataFieldSets.ShouldNotBeNull();
            sixthDataFieldSets.ShouldNotBeNull();
            firstDataFieldSets.ShouldNotBeSameAs(secondDataFieldSets);
            thirdDataFieldSets.ShouldNotBeSameAs(firstDataFieldSets);
            fourthDataFieldSets.ShouldNotBeSameAs(firstDataFieldSets);
            fifthDataFieldSets.ShouldNotBeSameAs(firstDataFieldSets);
            sixthDataFieldSets.ShouldNotBeSameAs(firstDataFieldSets);
            sixthDataFieldSets.ShouldNotBeSameAs(fourthDataFieldSets);
        }

        #endregion

        #region General Constructor : Class (DataFieldSets) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DataFieldSets_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var dataFieldSetId = -1;
            var groupId = -1;
            var multivaluedYN = string.Empty;
            var name = string.Empty;
            var errorList = new List<ECNError>();

            // Act
            var dataFieldSets = new DataFieldSets();

            // Assert
            dataFieldSets.DataFieldSetID.ShouldBe(dataFieldSetId);
            dataFieldSets.GroupID.ShouldBe(groupId);
            dataFieldSets.MultivaluedYN.ShouldBe(multivaluedYN);
            dataFieldSets.Name.ShouldBe(name);
            dataFieldSets.ErrorList.ShouldBeEmpty();
        }

        #endregion

        #endregion

        #endregion
    }
}
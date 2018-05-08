using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture]
    public class SmartFormsPrePopFieldsTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int prePopFieldID = -1;
            string profileFieldName = string.Empty;
            string displayName = string.Empty;
            string dataType = string.Empty;
            string controlType = string.Empty;
            string dataValues = string.Empty;
            string required = string.Empty;
            string prePopulate = string.Empty;        

            // Act
            SmartFormsPrePopFields smartFormsPrePopFields = new SmartFormsPrePopFields();    

            // Assert
            smartFormsPrePopFields.PrePopFieldID.ShouldBe(prePopFieldID);
            smartFormsPrePopFields.SFID.ShouldBeNull();
            smartFormsPrePopFields.ProfileFieldName.ShouldBe(profileFieldName);
            smartFormsPrePopFields.DisplayName.ShouldBe(displayName);
            smartFormsPrePopFields.DataType.ShouldBe(dataType);
            smartFormsPrePopFields.ControlType.ShouldBe(controlType);
            smartFormsPrePopFields.DataValues.ShouldBe(dataValues);
            smartFormsPrePopFields.Required.ShouldBe(required);
            smartFormsPrePopFields.PrePopulate.ShouldBe(prePopulate);
            smartFormsPrePopFields.SortOrder.ShouldBeNull();
            smartFormsPrePopFields.CreatedUserID.ShouldBeNull();
            smartFormsPrePopFields.CreatedDate.ShouldBeNull();
            smartFormsPrePopFields.UpdatedUserID.ShouldBeNull();
            smartFormsPrePopFields.UpdatedDate.ShouldBeNull();
            smartFormsPrePopFields.IsDeleted.ShouldBeNull();
            smartFormsPrePopFields.CustomerID.ShouldBeNull();
        }
    }
}
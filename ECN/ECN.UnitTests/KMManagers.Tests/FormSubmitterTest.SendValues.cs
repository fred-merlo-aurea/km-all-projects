using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using KMEntities;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KMManagers.Tests
{
    [TestFixture]
    public partial class FormSubmitterTest
    {
        private const string TestedMethodName_SendValues = "SendValues";
        private const int SendValues_ReturnSuccessCode = 200;
        private const int SendValues_ReturnErrorCode = 500;

        [Test]
        public void SendValue_WithFormData_MatchingControlAndValue_ReturnsCode200()
        {
            //Arrange            
            _form.Controls = CreateControls(controlID: ControlID, mainTypeId: HtmlControltypeAsNewsLetter, typeSeqID: TypeSeqIDForMailingPassword);
            _values.Add(ControlID, DummyStringValue);

            //Act
            var returnResult = (int)_formSubmitter.Invoke(TestedMethodName_SendValues);

            //Assert
            returnResult.ShouldBe(SendValues_ReturnSuccessCode);
        }

        [Test]
        public void SendValue_WithFormData_MatchingControlAndValue_ExceptionCaught_ReturnsCode500()
        {
            //Arrange            
            _form.Controls = CreateControls(controlID: ControlID, mainTypeId: HtmlControltypeAsNewsLetter, typeSeqID: TypeSeqIDForMailingPassword);
            _values.Add(ControlID, DummyStringValue);            
            ShimLayoutPlans.GetByFormTokenUID_NoAccessCheckGuid = null;

            //Act
            var returnResult = (int)_formSubmitter.Invoke(TestedMethodName_SendValues);

            //Assert
            returnResult.ShouldBe(SendValues_ReturnErrorCode);
        }

        [Test]
        public void SendValue_WithFormData_MatchingNewslterrGroupsAndControl_WithoutValue_ExceptionCaught_ReturnsCode500()
        {
            //Arrange            
            var controls= CreateControls(controlID: ControlID, mainTypeId: HtmlControltypeAsNewsLetter, typeSeqID: TypeSeqIDForNewsletter);
            ChangeControlToAddNewsletterGroup(controls[0]);
            _form.Controls = controls;

            //Act
            var returnResult = (int)_formSubmitter.Invoke(TestedMethodName_SendValues);

            //Assert
            returnResult.ShouldBe(SendValues_ReturnErrorCode);
        }

        [Test]
        public void SendValue_WithFormData_MatchingNewsletterGroupsWithControlAndValue_FormControl_ReturnsCode200()
        {
            //Arrange                    
            var controls = CreateControls(controlID: ControlID, mainTypeId: null, typeSeqID: TypeSeqIDForNewsletter, fieldID: GroupFieldID);
            ChangeControlToAddNewsletterGroup(controls[0]);

            var controlToAdd = CreateControl(controlID: ControlID_2, mainTypeId: HtmlControltypeAsNewsLetter, typeSeqID: TypeSeqIDForMailingPassword);
            controls.Add(controlToAdd);
            controls.First().FormControlProperties = CreateFormControlProperties();
            _form.Controls = controls;

            _groupDataFields.Add(CreateGroupDataField());

            _values.Add(ControlID, DummyStringValue);
            _values.Add(ControlID_2, DummyStringValue);

            //Act
            var returnResult = (int)_formSubmitter.Invoke(TestedMethodName_SendValues);

            //Assert
            returnResult.ShouldBe(SendValues_ReturnSuccessCode);
        }

        [Test]
        public void SendValue_WithFormData_MatchingNewsletterGroupsWithControlAndValue_FormData_ReturnsCode200()
        {
            //Arrange                    
            var controls = CreateControls(controlID: ControlID, mainTypeId: null, typeSeqID: TypeSeqIDForNewsletter, fieldID: GroupFieldID);
            ChangeControlToAddNewsletterGroup(controls[0]);

            var controlToAdd = CreateControl(controlID: ControlID_2, mainTypeId: HtmlControltypeAsNewsLetter, typeSeqID: TypeSeqIDForMailingPassword);
            controls.Add(controlToAdd);
            controls.First().FormControlProperties = CreateFormControlProperties();
            _form.Controls = controls;

            _groupDataFields.Add(CreateGroupDataField());

            _values.Add(ControlID, DummyStringValue);
            _values.Add(ControlID_2, DummyStringValue);

            //Act
            var returnResult = (int)_formSubmitter.Invoke(TestedMethodName_SendValues);

            //Assert
            returnResult.ShouldBe(SendValues_ReturnSuccessCode);
        }

        [Test]
        public void SendValue_WithFormData_MatchingNewsletterGroupsWithControlAndValue_DuplicatedParameters_ReturnsCode500()
        {
            //Arrange                    
            var controls = CreateControls(controlID: ControlID, mainTypeId: null, typeSeqID: TypeSeqIDForNewsletter, fieldID: GroupFieldID);
            ChangeControlToAddNewsletterGroup(controls[0]);

            var controlToAdd = CreateControl(controlID: ControlID_2, mainTypeId: HtmlControltypeAsNewsLetter, typeSeqID: TypeSeqIDForNewsletter);
            ChangeControlToAddNewsletterGroup(controlToAdd);            
            controlToAdd.FormControlProperties = CreateFormControlProperties();
            controls.Add(controlToAdd);

            _form.Controls = controls;           
            _groupDataFields.Add(CreateGroupDataField());

            _values.Add(ControlID, DummyStringValue);
            _values.Add(ControlID_2, DummyStringValue);

            //Act
            var returnResult = (int)_formSubmitter.Invoke(TestedMethodName_SendValues);

            //Assert
            returnResult.ShouldBe(SendValues_ReturnErrorCode);
        }
        
        [Test]
        public void SendValue_WithFormData_MatchingControlAndValue_WithStateControl_ExceptionCaught_ReturnsCode500()
        {
            //Arrange            
            _form.Controls = CreateControls(controlID: ControlID, mainTypeId: HtmlControltypeAsNewsLetter, typeSeqID: TypeSeqIDForStates); 

            _values.Add(ControlID, DummyIntValue);           

            //Act
            var returnResult = (int)_formSubmitter.Invoke(TestedMethodName_SendValues);

            //Assert
            returnResult.ShouldBe(SendValues_ReturnErrorCode);
        }

        [Test]
        public void SendValue_WithFormData_MatchingControlAndValue_WithStateCountry_ExceptionCaught_ReturnsCode500()
        {
            //Arrange            
            _form.Controls = CreateControls(controlID: ControlID, mainTypeId: HtmlControltypeAsNewsLetter, typeSeqID: TypeSeqIDForCountries); 

            _values.Add(ControlID, DummyIntValue);

            //Act
            var returnResult = (int)_formSubmitter.Invoke(TestedMethodName_SendValues);

            //Assert
            returnResult.ShouldBe(SendValues_ReturnErrorCode);
        }

        [Test]
        public void SendValue_WithFormData_MatchingControlAndValue_DuplicatedFieldNames_ExceptionCaught_ReturnsCode500()
        {
            //Arrange     
            _form.Controls = CreateControls(controlID: ControlID, mainTypeId: null, typeSeqID: TypeSeqIDForMailing, fieldID: GroupFieldID);
            _form.Controls.Add(CreateControl(controlID: ControlID_2, mainTypeId: null, typeSeqID: TypeSeqIDForMailing, fieldID: GroupFieldID));

            _values.Add(ControlID, DummyStringValue);
            _values.Add(ControlID_2, MailingProfilePswdColumnValue);

            _groupDataFields.Add(CreateGroupDataField());

            //Act
            var returnResult = (int)_formSubmitter.Invoke(TestedMethodName_SendValues);

            //Assert
            returnResult.ShouldBe(SendValues_ReturnErrorCode);
        }

        [Test]
        public void SendValue_WithFormData_MatchingControlAndValue_WithoutMatchingEmail_ExceptionCaught_ReturnsCode500()
        {
            //Arrange     
            var controls = CreateControls(controlID: ControlID, mainTypeId: HtmlControltypeAsNewsLetter, typeSeqID: TypeSeqIDForNewsletter);
            ChangeControlToAddNewsletterGroup(controls[0]);

            _form.Controls = controls;
            _values.Add(ControlID, DummyStringValue);

            ShimEmailGroup.ExistsStringInt32Int32 =
                (emailAddres, groupID, customerID) =>
                {
                    return false;
                };

            //Act
            var returnResult = (int)_formSubmitter.Invoke(TestedMethodName_SendValues);

            //Assert
            returnResult.ShouldBe(SendValues_ReturnErrorCode);
        }

        private List<FormControlProperty> CreateFormControlProperties(string controlValue = DummyStringValue)
        {
            var formControlProperties = new List<FormControlProperty>();
            formControlProperties.Add(new FormControlProperty()
            {
                ControlProperty_ID = ControlPropertySeqID,
                Value = controlValue
            });
            return formControlProperties;
        }
    }
}

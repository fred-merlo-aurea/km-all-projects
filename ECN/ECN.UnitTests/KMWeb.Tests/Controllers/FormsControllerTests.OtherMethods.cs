using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Web.Mvc;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Activity.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_BusinessLayer.FormDesigner.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMEntities;
using KMManagers.Fakes;
using KMModels.PostModels;
using KMModels;
using ShimKMUser = KMPlatform.BusinessLogic.Fakes.ShimUser;
using KMPlatform.Entity;
using KMSite;
using KMWeb.Controllers.Fakes;
using NUnit.Framework;
using Shouldly;
using KMWeb.Models.Forms;
using ECN_Framework_Common.Objects;
using Kendo.Mvc.UI;
using System.Data;
using KMManagers;
using KMModels.ViewModels;
using KMEnums;

namespace KMWeb.Tests.Controllers
{
    [TestFixture]    
    public partial class FormsControllerTests
    {
        private const string DummyString = "DummyString";
        private const string DummyURL = "http://www.url.com";
        private const string EmptyString = "";
        private const string MasterAccessKey = "MasterAccessKey";
        private const string CustomerAcessKey = "XYZABCD";
        private const string ResponseHeaderSaved = "Saved";
        private const string InvalidLinkUrl = "www.google.com";
        private const string ValidHtmlHrefUrl = "<a href='http://www.google.com'>Google</a>";
        private const string ValidHtmlInput = "<input id='dummyInput'>";
        private const string InvalidHtmlHrefUrl = "<a href='www.google.com'>Google</a>";
        private const string InvalidHtmlInput = "<input id='__viewstate'>";
        private const string ValidEmailAddress = "dummy@email.com";
        private const string InvalidEmailAddress = "email!dummy@email.com,";
        private const string InternalUserNotificationModelMessage = "SubmissionErrorNotification";
        private const string SuccessResponse = "200";
        private const string ErrorResponse = "500";
        private const string NotFoundErrorResponse = "404";
        private const string NewEmailAddress = "newEmailAddress@email.com";
        private const string OldEmailAddress = "oldEmailAddress@email.com";        
        private const int DefaultNumber = 0;
        private const int HttpStatusOK = 200;
        private const int DummyInt = 10;
        private const int GroupId = 1;                
        private const int SuppressValueNone = 0;
        private const int SuppressValueReceiveOnlyThis = 1;
        private const int SuppressValueReceiveAll = 2;
        private const int SuppressValueSuppressed = 3;

        [Test]
        public void PublicFormChangeEmail_SupressValueNone_WithSuccess()
        {
            // Arrange            
            InitializeShims();
            ShimEmailGroup.GetByEmailAddressGroupIDStringInt32User = (email, groupID, user) => null;

            // Act
            var response = _testEntity.PublicFormChangeEmail(NewEmailAddress, OldEmailAddress, GroupId, DummyInt, SuppressValueNone) as JsonResult;

            // Assert
            response.ShouldNotBeNull();
            var responseData = response.Data as List<string>;
            responseData.ShouldContain(SuccessResponse);
        }

        [Test]
        public void PublicFormChangeEmail_SupressValueReceiveAll_WithSuccess()
        {
            // Arrange            
            InitializeShims();
            // Act
            var response = _testEntity.PublicFormChangeEmail(NewEmailAddress, OldEmailAddress, GroupId, DummyInt, SuppressValueReceiveAll) as JsonResult;

            // Assert
            response.ShouldNotBeNull();
            var responseData = response.Data as List<string>;
            responseData.ShouldContain(SuccessResponse);
        }

        [Test]
        public void PublicFormChangeEmail_SupressValueReceiveOnlyThis_WithSuccess()
        {
            // Arrange            
            InitializeShims();
            // Act
            var response = _testEntity.PublicFormChangeEmail(NewEmailAddress, OldEmailAddress, GroupId, DummyInt, SuppressValueReceiveOnlyThis) as JsonResult;

            // Assert
            response.ShouldNotBeNull();
            var responseData = response.Data as List<string>;
            responseData.ShouldContain(SuccessResponse);
        }

        [Test]
        public void PublicFormChangeEmail_SupressValueNone_WithTreatedError()
        {
            // Arrange            
            InitializeShims();
            // Act
            var response = _testEntity.PublicFormChangeEmail(NewEmailAddress, OldEmailAddress, GroupId, DummyInt, SuppressValueNone) as JsonResult;

            // Assert
            response.ShouldNotBeNull();
            var responseData = response.Data as List<string>;
            responseData.ShouldContain(ErrorResponse);
        }

        [Test]
        public void PublicFormChangeEmail_ExceptionCaught_WithTreatedError()
        {
            // Arrange    
            InitializeShims();
            ShimChannelMasterSuppressionList.GetByEmailAddressInt32StringUser = (channelID, email, user) => null;

            // Act
            var response = _testEntity.PublicFormChangeEmail(NewEmailAddress, OldEmailAddress, GroupId, DummyInt, SuppressValueReceiveAll) as JsonResult;

            // Assert
            response.ShouldNotBeNull();
            var responseData = response.Data as List<string>;
            responseData.ShouldContain(ErrorResponse);
        }

        [Test]
        [TestCase(KMEnums.ResultType.Message, DummyString)]
        [TestCase(KMEnums.ResultType.Message, EmptyString)]
        [TestCase(KMEnums.ResultType.Message, InvalidHtmlHrefUrl)]
        [TestCase(KMEnums.ResultType.Message, InvalidHtmlInput)]
        [TestCase(KMEnums.ResultType.URL, EmptyString)]
        [TestCase(KMEnums.ResultType.MessageAndURL, EmptyString)]
        [TestCase(KMEnums.ResultType.MessageAndURL, InvalidHtmlHrefUrl)]
        [TestCase(KMEnums.ResultType.MessageAndURL, InvalidHtmlInput)]
        public void SaveProperties_WithModelError(KMEnums.ResultType resultType, string pageMessage)
        {
            // Arrange     
            InitializeShims();
            var model = new FormPropertiesPostModel()
            {
                Id = DummyInt,
                ConfirmationPageType = resultType,
                ConfirmationPageMessage = pageMessage,
                ConfirmationPageMAUMessage = pageMessage,
                ConfirmationPageMAUUrl = pageMessage,
                ConfirmationPageUrl = pageMessage,
                InactiveRedirectType = resultType,
                InactiveRedirectMessage = pageMessage,
                InactiveRedirectUrl = pageMessage
            };

            ShimFormManager.AllInstances.CheckNameIsUniqueInt32StringInt32 = (fm, baseChannelId, name, id) => false;

            // Act
            var response = _testEntity.SaveProperties(model) as PartialViewResult;
            var controls = response.ViewBag.Controls as List<ControlModel>;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => controls.ShouldNotBeNull(),
                () => controls.Count.ShouldBe(1),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeFalse());
        }

        [TestCase(KMEnums.ResultType.Message, DummyString)]
        [TestCase(KMEnums.ResultType.Message, ValidHtmlHrefUrl)]
        [TestCase(KMEnums.ResultType.Message, ValidHtmlInput)]
        [TestCase(KMEnums.ResultType.URL, DummyURL)]
        [TestCase(KMEnums.ResultType.MessageAndURL, DummyURL)]
        [TestCase(KMEnums.ResultType.MessageAndURL, ValidHtmlHrefUrl)]
        [TestCase(KMEnums.ResultType.MessageAndURL, ValidHtmlInput)]
        public void SaveProperties_WitSuccess(KMEnums.ResultType resultType, string pageMessage)
        {
            // Arrange     
            InitializeShims();
            var model = new FormPropertiesPostModel()
            {
                Id = DummyInt,
                ConfirmationPageType = resultType,
                ConfirmationPageMessage = pageMessage,
                ConfirmationPageMAUMessage = pageMessage,
                ConfirmationPageMAUUrl = pageMessage,
                ConfirmationPageUrl = pageMessage,
                InactiveRedirectType = resultType,
                InactiveRedirectMessage = pageMessage,
                InactiveRedirectUrl = pageMessage
            };

            // Act
            var response = _testEntity.SaveProperties(model) as PartialViewResult;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue());
        }

        [Test]
        [TestCase(null)]
        [TestCase(EmptyString)]
        [TestCase(InvalidHtmlHrefUrl)]
        public void SaveSubscriberLogin_WithModelError(string modelValue)
        {
            // Arrange       
            InitializeShims();
            var model = new FormSubscriberLoginPostModel()
            {
                FormID = DummyInt,
                LoginModalHTML = modelValue,
                ForgotPasswordMessageHTML = modelValue,
                ForgotPasswordNotificationHTML = modelValue,
                EmailAddressQuerystringName = modelValue ?? InvalidEmailAddress
            };

            // Act
            var response = _testEntity.SaveSubscriberLogin(model) as PartialViewResult;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeFalse());
        }

        [Test]
        [TestCase(EmptyString)]
        [TestCase(InvalidEmailAddress)]
        public void SaveSubscriberLogin_WithOtherIdentificationAndPasswordRequired_WithModelError(string modelValueToValidate)
        {
            // Arrange    
            InitializeShims();
            var model = new FormSubscriberLoginPostModel()
            {
                FormID = DummyInt,
                OtherIdentificationSelection = true,
                OtherQuerystringName = modelValueToValidate,
                PasswordRequired = true,
                AutoLoginAllowed = true,
                EmailAddressQuerystringName = modelValueToValidate ?? InvalidEmailAddress,
                PasswordQuerystringName = modelValueToValidate,
                ForgotPasswordFromName = modelValueToValidate,
                ForgotPasswordSubject = modelValueToValidate
            };

            // Act
            var response = _testEntity.SaveSubscriberLogin(model) as PartialViewResult;
            var fields = _testEntity.ViewBag.Fields as List<GroupDataFields>;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeFalse(),
                () => fields.ShouldNotBeNull(),
                () => fields.Count.ShouldBeGreaterThan(1));
        }

        [Test]
        public void SaveSubscriberLogin_WithSuccess()
        {
            // Arrange     
            InitializeShims();
            var model = new FormSubscriberLoginPostModel()
            {
                FormID = DummyInt,
                LoginModalHTML = ValidHtmlHrefUrl,
                ForgotPasswordMessageHTML = ValidHtmlHrefUrl,
                ForgotPasswordNotificationHTML = ValidHtmlHrefUrl,
                EmailAddressQuerystringName = DummyString,
                OtherIdentificationSelection = true,
                OtherQuerystringName = DummyString,
                PasswordRequired = true,
                AutoLoginAllowed = true,
                PasswordQuerystringName = DummyString,
                ForgotPasswordFromName = DummyString,
                ForgotPasswordSubject = DummyString
            };

            // Act
            var response = _testEntity.SaveSubscriberLogin(model) as PartialViewResult;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue());
        }

        [Test]
        public void GetSubscriberLogin_WithSuccess()
        {
            // Arrange   
            InitializeShims();
            // Act
            var response = _testEntity.GetSubscriberLogin(DefaultNumber) as PartialViewResult;
            var responseModel = response.Model as FormSubscriberLoginPostModel;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => responseModel.ShouldNotBeNull(),
                () => responseModel.ForgotPasswordMessageHTML.ShouldNotBeNullOrWhiteSpace(),
                () => responseModel.LoginModalHTML.ShouldNotBeNullOrWhiteSpace());
        }

        [Test]
        public void ChangeGroupContent_WithSuccess()
        {
            // Arrange            
            InitializeShims();
            const bool changeFormGroup = true;

            // Act
            var response = _testEntity.ChangeGroupContent(DummyInt, DummyInt, GroupId, changeFormGroup) as PartialViewResult;
            var fields = _testEntity.ViewBag.Fields as List<FieldModel>;
            var responseModel = response.Model as ChangeGroupPostModel;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => fields.ShouldNotBeNull(),
                () => fields.Count.ShouldBeGreaterThan(0),
                () => responseModel.ShouldNotBeNull(),
                () => responseModel.ChangeFormGroup.ShouldBe(changeFormGroup),
                () => responseModel.Fields.ShouldNotBeNull());
        }

        [Test]
        [TestCase(InternalUserNotificationModelMessage)]
        [TestCase(DummyString)]
        public void GetNotifications_WithSuccess(string notificationMessage)
        {
            // Arrange            
            InitializeShims();
            ShimNotificationManager.AllInstances.GetAllUserNotificationsByFormIDInt32 = (nm, id) => new List<InternalUserNotificationModel>()
            {
                new InternalUserNotificationModel()
                {
                    CustomerID = DummyInt,
                    Id = DummyInt,
                    Message = notificationMessage,
                },
                new InternalUserNotificationModel()
                {
                    CustomerID = DummyInt,
                    Id = DummyInt +1,
                    Message = notificationMessage,
                }
            };

            // Act
            var response = _testEntity.GetNotifications(DummyInt) as PartialViewResult;
            var responseModel = response.Model as FormNotificationsPostModel;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => responseModel.ShouldNotBeNull(),
                () => responseModel.InternalUserNotifications.ShouldNotBeNull(),
                () => responseModel.SubscriberNotifications.ShouldNotBeNull());
        }

        [Test]
        public void SaveNotifications_WithSuccess()
        {
            // Arrange    
            InitializeShims();
            var model = new FormNotificationsPostModel()
            {
                Id = DummyInt,
                InternalUserNotifications = new List<InternalUserNotificationModel>()
                {
                    new InternalUserNotificationModel()
                    {
                        CustomerID = DummyInt,
                        Id = DummyInt,
                        ToEmail = ValidEmailAddress,
                        Conditions = new List<ConditionModel>()
                        {
                            new ConditionModel()
                            {
                                Type = KMEnums.ConditionType.And
                            }
                        }
                    }
                },
                SubscriberNotifications = new List<SubscriberNotificationModel>()
                {
                    new SubscriberNotificationModel()
                    {
                        CustomerID = DummyInt,
                        Id = DummyInt,
                        Message = DummyString,
                        Conditions = new List<ConditionModel>()
                        {
                            new ConditionModel()
                            {
                                Type = KMEnums.ConditionType.And
                            }
                        }
                    }
                }
            };

            // Act
            var response = _testEntity.SaveNotifications(model) as PartialViewResult;
            var responseModel = response.Model as FormNotificationsPostModel;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => responseModel.ShouldBe(model));
        }

        [Test]
        public void SaveNotifications_WithModelError()
        {
            // Arrange  
            InitializeShims();
            var model = new FormNotificationsPostModel()
            {
                Id = DummyInt,
                InternalUserNotifications = new List<InternalUserNotificationModel>()
                {
                    new InternalUserNotificationModel()
                    {
                        CustomerID = DummyInt,
                        Id = DummyInt,
                        ToEmail = InvalidEmailAddress,
                        Conditions = new List<ConditionModel>()
                        {
                            new ConditionModel()
                            {
                                Type = KMEnums.ConditionType.And
                            }
                        }
                    }
                },
                SubscriberNotifications = new List<SubscriberNotificationModel>()
                {
                    new SubscriberNotificationModel()
                    {
                        CustomerID = DummyInt,
                        Id = DummyInt,
                        Message = EmptyString,
                        Conditions = new List<ConditionModel>()
                        {
                            new ConditionModel()
                            {
                                Type = KMEnums.ConditionType.And
                            }
                        }
                    }
                }
            };

            // Act
            var response = _testEntity.SaveNotifications(model) as PartialViewResult;
            var responseModel = response.Model as FormNotificationsPostModel;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeFalse(),
                () => responseModel.ShouldBe(model));
        }

        [Test]
        public void Create_ValidState_WithSuccess()
        {
            // Arrange            
            InitializeShims();
            var model = new FormPostModel()
            {
                GroupId = GroupId,
                CustomerId = DummyInt,
                Name = DummyString
            };

            // Act
            var response = _testEntity.Create(model) as JavaScriptRedirectResult;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => response.RouteValues.ToString().ShouldContain(DummyInt.ToString()),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue());
        }

        [Test]
        public void Create_InvalidState_WithSuccess()
        {
            // Arrange            
            InitializeShims();
            var model = new FormPostModel()
            {
                GroupId = GroupId,
                CustomerId = DummyInt,
                Name = DummyString
            };
            ShimFormManager.AllInstances.CheckNameIsUniqueInt32String = (fm, baseChannelId, name) => false;

            // Act
            var response = _testEntity.Create(model) as PartialViewResult;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeFalse());
        }


        [Test]
        public void Copy_ValidState_WithSuccess()
        {
            // Arrange            
            InitializeShims();
            var model = new FormPostModel()
            {
                GroupId = GroupId,
                CustomerId = DummyInt,
                Name = DummyString
            };

            // Act
            var response = _testEntity.Copy(model) as JavaScriptRedirectResult;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue());
        }

        [Test]
        public void Copy_InvalidState_WithSuccess()
        {
            // Arrange            
            InitializeShims();
            var model = new FormPostModel()
            {
                GroupId = GroupId,
                CustomerId = DummyInt,
                Name = DummyString
            };
            ShimFormManager.AllInstances.CheckNameIsUniqueInt32String = (fm, baseChannelId, name) => false;

            // Act
            var response = _testEntity.Copy(model) as PartialViewResult;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeFalse());
        }

        [Test]
        public void UpdateRules_WithSuccess()
        {
            // Arrange    
            InitializeShims();
            var model = new FormControlsPostModel()
            {
                Id = DummyInt,
                OldGrids = new List<string>()
                {
                    "state",
                   $"{DummyString},{DummyString}"
                }
            };

            // Act
            var response = _testEntity.UpdateRules(model) as HttpStatusCodeResult;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => response.StatusCode.ShouldBe(HttpStatusOK),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue());
        }

        [Test]
        public void GetRules_WithSuccess()
        {
            // Arrange               
            InitializeShims();
            // Act
            var response = _testEntity.GetRules(DummyInt) as PartialViewResult;
            var responseModel = response.Model as FormRulesPostModel;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => responseModel.ShouldNotBeNull(),
                () => responseModel.Rules.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue());
        }

        [Test]
        public void PublicFormSignUp_WithSuccess()
        {
            // Arrange            
            InitializeShims();

            // Act
            var response = _testEntity.PublicFormSignUp(ValidEmailAddress,GroupId) as JsonResult;

            // Assert
            response.ShouldNotBeNull();
            var responseData = response.Data as List<string>;
            responseData.ShouldContain(SuccessResponse);
            responseData.ShouldNotContain(ErrorResponse);
        }

        [Test]
        public void PublicFormSignUp_WithErrorResponse()
        {
            // Arrange            
            InitializeShims();
            ShimEmailGroup.GetByEmailAddressGroupID_NoAccessCheckStringInt32 = (emailAddress, groupID) => throw new Exception();

            // Act
            var response = _testEntity.PublicFormSignUp(ValidEmailAddress, GroupId) as JsonResult;

            // Assert
            response.ShouldNotBeNull();
            var responseData = response.Data as List<string>;
            responseData.ShouldContain(ErrorResponse);            
        }

        [Test]
        public void PublicFormSignUp_WithNotFoundErrorResponse()
        {
            // Arrange            
            InitializeShims();
            ShimEmailGroup.GetByEmailAddressGroupID_NoAccessCheckStringInt32 = (emailAddress, groupID) => null;

            // Act
            var response = _testEntity.PublicFormSignUp(ValidEmailAddress, GroupId) as JsonResult;

            // Assert
            response.ShouldNotBeNull();
            var responseData = response.Data as List<string>;
            responseData.ShouldContain(NotFoundErrorResponse);
        }

        [Test]
        [TestCase(KMEnums.FormActive.Active, true)]        
        [TestCase(KMEnums.FormActive.UseActivationDates, true)]
        [TestCase(KMEnums.FormActive.UseActivationDates, false)]        
        public void ChangeActiveState_WithErrorResponse(KMEnums.FormActive state, bool isArchived)
        {
            // Arrange               
            InitializeShims();
            var model = new ActivateFormModel()
            {
                Id = DummyInt,
                State = state
            };
            ShimFormManager.AllInstances.GetGroupByFormIDInt32Int32 = (fm, baseChannelID, formID) => new Group()
            {
                Archived = isArchived
            };

            // Act
            var response = _testEntity.ChangeActiveState(model) as PartialViewResult;
            var responseModel = response.Model as ActivateFormModel;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),                
                () => responseModel.State.ShouldBe(KMEnums.FormActive.Inactive),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeFalse());
        }

        [Test]
        public void ChangeActiveState_WithSuccess()
        {
            // Arrange               
            InitializeShims();
            var model = new ActivateFormModel()
            {
                Id = DummyInt,
                State = KMEnums.FormActive.Inactive
            };            

            // Act
            var response = _testEntity.ChangeActiveState(model) as JavaScriptRedirectResult;            

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),                
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue());
        }

        [Test]
        public void UpdateKMPSEmail_WithSuccess()
        {
            // Arrange               
            InitializeShims();
            ShimEmail.ExistsStringInt32 = (newemail, customerID) => false;

            // Act
            var response = _testEntity.UpdateKMPSEmail(ValidEmailAddress, ValidEmailAddress, GroupId, true, DummyInt) as JsonResult;
            var responseData = response.Data as List<string>;
            
            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => responseData.ShouldContain(SuccessResponse));
        }

        [Test]
        public void UpdateKMPSEmail_WithErrorResponse()
        {
            // Arrange               
            InitializeShims();
            ShimEmail.ExistsStringInt32 = (newemail, customerID) => true;

            // Act
            var response = _testEntity.UpdateKMPSEmail(ValidEmailAddress, ValidEmailAddress, GroupId, true, DummyInt) as JsonResult;
            var responseData = response.Data as List<string>;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => responseData.ShouldContain(ErrorResponse));
        }

        [Test]
        public void UpdateKMPSEmail_WithExceptionCaughtAndErrorResponse()
        {
            // Arrange               
            InitializeShims();
            ShimEmail.ExistsStringInt32 = (newemail, customerID) => throw new Exception();

            // Act
            var response = _testEntity.UpdateKMPSEmail(ValidEmailAddress, ValidEmailAddress, GroupId, true, DummyInt) as JsonResult;
            var responseData = response.Data as List<string>;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => responseData.ShouldContain(ErrorResponse));
        }

        [Test]
        public void AddGroup_WithSuccess()
        {
            // Arrange               
            InitializeShims();
            const string expectedResult = "success = true";
            var model = new AddGroupModel()
            {
                GroupName = DummyString,
                FolderId = DummyInt,
                CustomerId = DummyInt
            };

            // Act
            var response = _testEntity.AddGroup(model) as JsonResult;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => response.Data.ToString().ShouldContain(expectedResult));
        }

        [Test]
        public void AddGroup_WithErrorResponse()
        {
            // Arrange               
            InitializeShims();            
            var model = new AddGroupModel()
            {
                GroupName = DummyString,
                FolderId = DummyInt,
                CustomerId = DummyInt
            };
            var errorList = new List<ECNError>()
            {
                new ECNError()
                {
                    ErrorMessage = ErrorResponse
                }
            };
            ShimGroup.SaveGroupUser = (g, currentUser) => throw new ECNException(errorList);

            // Act
            var response = _testEntity.AddGroup(model) as PartialViewResult;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeFalse());
        }

        [Test]
        public void LoadForm_WithSuccess()
        {
            // Arrange               
            InitializeShims();            

            // Act
            var response = _testEntity.LoadForm(DummyInt) as PartialViewResult;            
            var groupIds = response.ViewBag.nlGroupIds as List<int>;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => response.Model.ShouldNotBeNull(),
                () => groupIds.ShouldNotBeNull(),
                () => groupIds.Count.ShouldBeGreaterThan(0));
        }

        [Test]
        public void LoadForm_WithoutEnoughDataSuccess()
        {
            // Arrange               
            InitializeShims();

            // Act
            var response = _testEntity.LoadForm(null) as PartialViewResult;
            var groupIds = response.ViewBag.nlGroupIds as List<int>;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => response.Model.ShouldNotBeNull(),
                () => groupIds.ShouldNotBeNull(),
                () => groupIds.Count.ShouldBe(0));
        }

        [Test]
        public void ActiveFormsReadToGrid_WithSuccess()
        {
            // Arrange               
            InitializeShims();            
            var request = new DataSourceRequest()
            {                
                PageSize = DummyInt
            };

            // Act
            var response = _testEntity.ActiveFormsReadToGrid(request) as JsonResult;
            var responseData = response.Data as DataSourceResult;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => responseData.Total.ShouldBe(DummyInt));
        }

        [Test]
        public void InactiveFormsReadToGrid_WithSuccess()
        {
            // Arrange               
            InitializeShims();
            var request = new DataSourceRequest()
            {
                PageSize = DummyInt
            };

            // Act
            var response = _testEntity.InactiveFormsReadToGrid(request) as JsonResult;
            var responseData = response.Data as DataSourceResult;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => responseData.Total.ShouldBe(DummyInt));
        }

        [Test]
        public void SaveDoubleOptIn_WithSuccess()
        {
            // Arrange               
            InitializeShims();
            var model = new FormDoubleOptInPostModel()
            {
                Notification = new DOINotificationModel()
                {
                    Message = InvalidLinkUrl + HTMLGenerator.urlMacros                    
                },
                Page = string.Empty
            };

            // Act
            var response = _testEntity.SaveDoubleOptIn(model) as PartialViewResult;            

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => response.Model.ShouldNotBeNull());
        }

        [Test]
        [TestCase(" ")]
        [TestCase(DummyString)]
        public void SaveDoubleOptIn_WithResponseError(string message)
        {
            // Arrange               
            InitializeShims();
            var model = new FormDoubleOptInPostModel()
            {
                Notification = new DOINotificationModel()
                {
                    Message = message
                },
                Page = message
            };

            // Act
            var response = _testEntity.SaveDoubleOptIn(model) as PartialViewResult;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeFalse(),
                () => response.Model.ShouldNotBeNull());
        }

        [Test]        
        public void Edit_StatusUpdated_WithSuccess()
        {
            // Arrange           
            const int childId = 1;
            InitializeShims();
            var form = new FormViewModel()
            {
                Status = FormStatus.Updated.ToString(),
                Child = new FormViewModel() { Id = childId }
            };
            ShimFormManager.AllInstances.GetByIDOf1Int32Int32<FormViewModel>((fm, baseChannelID, modelId) => form);

            // Act
            var response = _testEntity.Edit(DummyInt) as RedirectToRouteResult;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => response.RouteValues.ShouldNotBeNull(),
                () => response.RouteValues.Values.ShouldContain(childId));
        }

        [Test]
        public void Edit_StatusPublished_WithSuccess()
        {
            // Arrange               
            InitializeShims();
            var form = new FormViewModel()
            {
                Status = FormStatus.Published.ToString()
            };
            ShimFormManager.AllInstances.GetByIDOf1Int32Int32<FormViewModel>((fm, baseChannelID, modelId) => form);

            // Act
            var response = _testEntity.Edit(DummyInt) as ViewResult;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => response.Model.ShouldNotBeNull(),
                () => response.Model.ShouldBe(DummyInt));
        }

        [Test]
        public void SaveControls_WithErrorResponse()
        {
            // Arrange               
            InitializeShims();
            var request = new FormControlsPostModel()
            { 
                Email = new KMModels.Controls.Standard.Common.Email()
            };

            // Act
            var response = _testEntity.SaveControls(request) as JsonResult;
            var responseData = response.Data as List<string>;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => responseData.ShouldNotBeNull(),
                () => responseData[0].ShouldContain("505"));
        }

        [Test]
        public void AddField_WithModelError()
        {
            // Arrange               
            InitializeShims();
            var request = new AddFieldModel()
            {
                GroupId = GroupId,
                CustomerId = DummyInt,
                FieldName = DummyString
            };

            ShimGroupDataFields.SaveGroupDataFieldsUser = (gf, user) => 
            {
                var errorList = new List<ECNError>()
                {
                    new ECNError()
                    {
                       ErrorMessage = DummyString
                    }
                };
                throw new ECNException(errorList);
            };

            // Act
            var response = _testEntity.AddField(request) as PartialViewResult;            

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeFalse());
        }

        [Test]
        public void AddField_WithSuccess()
        {
            // Arrange               
            const string expectedResult = "success = true";
            InitializeShims();
            var request = new AddFieldModel()
            {
                GroupId = GroupId,
                CustomerId = DummyInt,
                FieldName = DummyString
            };

            ShimGroupDataFields.SaveGroupDataFieldsUser = (gf, user) => DummyInt;

            // Act
            var response = _testEntity.AddField(request) as JsonResult;
            var responseData = response.Data;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => responseData.ShouldNotBeNull(),
                () => responseData.ToString().ShouldContain(expectedResult));
        }

        [Test]
        public void GetProperties_WithSuccess()
        {
            // Arrange               
            InitializeShims();

            // Act
            var response = _testEntity.GetProperties(DummyInt) as PartialViewResult;
            var model = response.Model;
            var groupIds = response.ViewBag.nlGroupIds as List<int>;

            // Assert
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.ShouldNotBeNull(),
                () => _testEntity.ViewData.ModelState.IsValid.ShouldBeTrue(),
                () => groupIds.Count.ShouldBeGreaterThan(0),
                () => model.ShouldNotBeNull());
        }

        private void InitializeShims()
        {
            CreateAppSettingsShim();
            CreateGroupShims();
            CreateEmailsShims();
            CreateUserShim();
            CreateCustomerShim();
            CreateFMShims();
            CreateCMShims();
            CreateSLShims();
            CreateNMShims();
            CreateRMShims();

            ShimAPIRunnerBase.StaticConstructor = () => { };
            ShimChannelMasterSuppressionList.GetByEmailAddressInt32StringUser = (channelID, email, user) => new List<ChannelMasterSuppressionList>();
            ShimForm.GetByFormID_NoAccessCheckInt32 = (formID) => null;
            ShimECNSession.CurrentSession = () => new ShimECNSession()
            {
                BaseChannelIDGet = () => DummyInt
            };
            ShimFormsController.AllInstances.CurrentUserGet = (form) => new User()
            {
                CustomerID = DummyInt,
                UserID = DummyInt
            };
        }

        private void CreateRMShims()
        {
            ShimRuleManager.AllInstances.GetAllByFormIDOf1Int32((rm, id) => new List<RuleModel>()
            {
                new RuleModel()
                {
                    ConditionGroup = new List<ConditionGroupModel>()
                    {
                        new ConditionGroupModel()
                        {
                            Conditions = new List<ConditionModel>()
                            {
                                new ConditionModel()
                                {
                                    ControlId = DummyInt,
                                    Value = DummyString
                                }
                            }
                        }
                    }
                }
            });

            ShimRuleManager.AllInstances.SaveUserInt32FormRulesPostModel = (rm, currentUser, baseChannelID, rulesModel) => { };
        }

        private void CreateNMShims()
        {
            ShimNotificationManager.AllInstances.GetAllSubscriberNotificationsByFormIDInt32 = (nm, id) => new List<SubscriberNotificationModel>()
            {
                new SubscriberNotificationModel()
                {
                    CustomerID = DummyInt,
                    Id = DummyInt
                }
            };

            ShimNotificationManager.AllInstances.SaveUserInt32FormNotificationsPostModel = (nm, currentUser, baseChannelId, model) => { };
            ShimNotificationManager.AllInstances.SaveFormDoubleOptInPostModel = (nm, model) => { };
        }

        private void CreateSLShims()
        {
            ShimSubscriberLoginManager.AllInstances.SaveUserInt32FormSubscriberLoginPostModel = (sl, currentUser, baseChannelId, model) => { };
            ShimSubscriberLoginManager.AllInstances.GetByIDOf1Int32<FormSubscriberLoginPostModel>((sl, formIde) =>
            {
                if (formIde == DefaultNumber)
                {
                    return null;
                }
                else
                {
                    return new FormSubscriberLoginPostModel()
                    {
                        OtherIdentificationSelection = true,
                        OtherIdentification = DummyInt.ToString(),
                        ForgotPasswordMessageHTML = "ForgotPasswordNotificationHTML %%Email%%",
                        ForgotPasswordNotificationHTML = EmptyString
                    };
                }
            });
        }

        private void CreateCMShims()
        {
            var controls = new List<ControlModel>()
            {
                new ControlModel()
                {
                    Id = DummyInt,
                    DataType = KMEnums.DataType.Selection,
                    SelectableItems = new SelectableItem[]{ new SelectableItem() { Id = DummyInt, Label = DummyString} },
                    FieldID = DummyInt,
                    Control_Type = new ControlTypeModel()
                    {
                        Name = DummyString
                    }
                }
            };
            ShimControlManager.AllInstances.GetAllValuedByFormIDOf1Int32((shim, id) => controls);
            ShimControlManager.AllInstances.FillControlsFormControlsPostModelInt32User = (cm, allControls, baseChannelID, currentUser) => 
            {
                allControls.NewsLetter.Add(new KMModels.Controls.NewsLetter()
                {
                    Groups = new List<GroupModel>()
                    {
                        new GroupModel()
                        {
                            GroupID = GroupId
                        }
                    }
                });
            };
            ShimControlManager.AllInstances.GetAllCustomWithFieldByFormIDWithFieldNamesInt32StringInt32 = (cm, baseChannelID, apiKey, formId) => new List<ControlFieldModel>()
            {
                new ControlFieldModel()
                {
                    FieldName = DummyString + DummyString,
                    ControlId = DummyInt,
                    ControlLabel = DummyString
                }
            };
            ShimControlManager.AllInstances.GetAllVisibleByFormIDOf1Int32<ControlModel>((cm, id) => controls);
            ShimControlManager.AllInstances.GetPageBreaksByFormIDOf1Int32<ControlModel>((cm, id) => controls);
            ShimControlManager.AllInstances.GetAllRequestQueryByFormIDOf1Int32<ControlModel>((cm, id) => controls);

            ShimControlTypeManager.AllInstances.GetPaidQueryStringByNameOf1String<ControlTypeModel>((ctm, name) => new ControlTypeModel()
            {
                KMPaidQueryString = DummyString,
                Name = DummyString
            });
        }

        private void CreateFMShims()
        {
            var form = new Form()
            {
                FormType = Enum.GetName(typeof(KMEnums.FormType), KMEnums.FormType.AutoSubmit)
            };
            ShimFormManager.AllInstances.GetByIDInt32Int32 = (fm, baseChannelID, modelId) => form;
            ShimFormManager.AllInstances.CheckNameIsUniqueInt32StringInt32 = (fm, baseChannelId, name, id) => true;
            ShimFormManager.AllInstances.CheckNameIsUniqueInt32String = (fm, baseChannelId, name) => true;
            ShimFormManager.AllInstances.SaveUserInt32FormPropertiesPostModel = (fm, currentUser, baseChannelId, model) => { };

            var groupFields = new List<GroupDataFields>()
            {
                new GroupDataFields()
                {
                    GroupDataFieldsID = DummyInt,
                    ShortName = DummyString
                }
            };
            ShimFormManager.AllInstances.GetFieldsByFormIDInt32Int32User = (fm, formID, baseChannelID, currentUser) => groupFields;
            ShimFormManager.AllInstances.GetGroupByFormIDInt32Int32 = (fm, baseChannelID, formID) => new Group();
            ShimFormManager.AllInstances.GetFieldsByGroupIDInt32 = (fm, groupId) => groupFields;
            ShimFormManager.AllInstances.FullCopyInt32Int32FormPostModelStringDictionaryOfStringString =
                (fm, userID, baseChannelID, model, apiKey, controlField) => DummyInt;
            ShimFormManager.AllInstances.FullCopyByIDInt32Int32Int32 =
                (fm, userID, baseChannelID, id) => DummyInt;
            ShimFormManager.AllInstances.SaveInt32FormPostModelStringControlManager = (fm, userId, model, ApiKey, CM) => DummyInt;
            ShimFormManager.AllInstances.ChangeActiveStateByIDInt32Int32FormActiveNullableOfDateTimeNullableOfDateTime =
                (fm, baseChannelID, modelId, modelState, modelFrom, modelTo) => { };
                        
            ShimForm.GetBySearchStringPagingInt32Int32StringStringStringStringInt32Int32Int32StringString 
                = (baseChannelID, customerID, formType, formStatus, formName, searchCriterion, active, pageNumber, pageSize, sortDirection, sortColumn) =>
                {
                    var dataSet = new DataSet();
                    var dataTable = new DataTable(DummyString);
                    dataTable.Columns.Add("TotalCount");
                    dataTable.Columns.Add("Form_Seq_ID");
                    dataTable.Rows.Add(DummyInt, DummyInt);

                    dataSet.Tables.Add(dataTable);
                    return dataSet;
                };
        }

        private static void CreateCustomerShim()
        {
            ShimCustomer.GetByCustomerIDInt32Boolean = (customerID, getChildren) => new Customer()
            {
                BaseChannelID = DummyInt
            };
        }

        private static void CreateUserShim()
        {
            var userInstance = new KMPlatform.Entity.User();
            ShimKMUser.GetByAccessKeyStringBoolean = (key, getChildren) => userInstance;
        }

        private static void CreateGroupShims()
        {
            var group = new Group()
            {
                CustomerID = DummyInt,
                GroupID = GroupId,
                Archived = true
            };

            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (groupId) => group;
            ShimGroup.GetMasterSuppressionGroupInt32User = (customerID, user) => group;
            ShimGroupDataFields.GetByIDInt32User = (fID, currentUser) => new GroupDataFields()
            {
                ShortName = DummyString
            };
            ShimGroup.SaveGroupUser = (g, currentUser) => DummyInt;
        }

        private static void CreateEmailsShims()
        {
            var emailGroup = new EmailGroup()
            {
                CustomerID = DummyInt
            };
            ShimEmailGroup.GetByEmailAddressGroupIDStringInt32User = (email, groupID, user) => emailGroup;
            ShimEmailGroup.GetByEmailAddressGroupID_NoAccessCheckStringInt32 = (emailAddress, groupID) => emailGroup;
            ShimEmailGroup.GetByEmailIDInt32User = (emailID, user) => new List<EmailGroup>();
            ShimEmailGroup.DeleteFromMasterSuppressionGroupInt32Int32User = (groupID, emailID, user) => { };

            ShimEmailActivityUpdate.UpdateEmailActivity_NoAccessCheckStringStringInt32Int32Int32StringUser =
                (oldEmailAddress, newEmailAddress, groupID, customerID, formID, comments, user) => { };

            ShimEmail.ExistsByGroupStringInt32 = (email, groupId) => false;
            ShimEmail.UpdateEmailAddressInt32Int32StringStringUserString =
                (groupID, customerID, newEmailAddress, oldEmailAddress, user, source) => { };
            var emailInstance = new Email()
            {
                EmailID = DummyInt
            };
            ShimEmail.GetByEmailAddressStringInt32 = (newEmailAddress, customerID) => emailInstance;
            ShimEmail.GetByEmailID_NoAccessCheckInt32 = (emailID) => emailInstance;
            ShimEmail.SaveEmail = (email) => { };
        }

        public static void CreateAppSettingsShim()
        {
            ShimConfigurationManager.AppSettingsGet =
               () => new NameValueCollection
               {
                    {MasterAccessKey,  CustomerAcessKey }
               };
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using KMEnums;

using KMEntities;

namespace KMModels.PostModels
{
    public class FormSubscriberLoginPostModel: PostModelBase
    {
        [GetFromField("FormID")]
        public int FormID { get; set; }

        [GetFromField("LoginRequired")]
        public bool LoginRequired { get; set; }

        public bool OtherIdentificationSelection { get; set; }

        [GetFromField("OtherIdentification")]
        public string OtherIdentification { get; set; }

        [GetFromField("PasswordRequired")]
        public bool PasswordRequired { get; set; }

        [GetFromField("AutoLoginAllowed")]
        public bool AutoLoginAllowed { get; set; }

        [GetFromField("LoginModalHTML")]
        public string LoginModalHTML { get; set; }

        [GetFromField("LoginButtonText")]
        [Required(ErrorMessage = "Login Button Text Required")]
        [MaxLength(100, ErrorMessage = "Button Text is too long")]
        public string LoginButtonText { get; set; }

        [GetFromField("SignUpButtonText")]
        [Required(ErrorMessage = "Sign Up Button Text Required")]
        [MaxLength(100, ErrorMessage = "Button Text is too long")]
        public string SignUpButtonText { get; set; }

        [GetFromField("ForgotPasswordButtonText")]
        [Required(ErrorMessage = "Forgot Password Button Text Required")]
        [MaxLength(100, ErrorMessage = "Button Text is too long")]
        public string ForgotPasswordButtonText { get; set; }

        [GetFromField("NewSubscriberButtonText")]
        [Required(ErrorMessage = "New Subscriber Button Text Required")]
        [MaxLength(100, ErrorMessage = "Button Text is too long")]
        public string NewSubscriberButtonText { get; set; }

        [GetFromField("ExistingSubscriberButtonText")]
        [Required(ErrorMessage = "Existing Subscriber Button Text Required")]
        [MaxLength(100, ErrorMessage = "Button Text is too long")]
        public string ExistingSubscriberButtonText { get; set; }

        [GetFromField("ForgotPasswordMessageHTML")]
        public string ForgotPasswordMessageHTML { get; set; }

        [GetFromField("ForgotPasswordNotificationHTML")]
        public string ForgotPasswordNotificationHTML { get; set; }

        [GetFromField("ForgotPasswordFromName")]
        [MaxLength(100, ErrorMessage = "From Name is too long")]
        public string ForgotPasswordFromName { get; set; }

        [GetFromField("ForgotPasswordSubject")]
        [MaxLength(100, ErrorMessage = "Subject Line is too long")]
        public string ForgotPasswordSubject { get; set; }

        public int GroupID { get; set; }

        [GetFromField("EmailAddressQuerystringName")]
        [Required(ErrorMessage = "Email Address Querystring Name Required")]
        [MaxLength(50, ErrorMessage = "Email Address Querystring Name is too long")]
        public string EmailAddressQuerystringName { get; set; }

        [GetFromField("OtherQuerystringName")]
        [MaxLength(50, ErrorMessage = "Other Querystring Name is too long")]
        public string OtherQuerystringName { get; set; }

        [GetFromField("PasswordQuerystringName")]
        [MaxLength(50, ErrorMessage = "Password Querystring Name is too long")]
        public string PasswordQuerystringName { get; set; }

        public string SubIdLabel { get; set; }

        public override void FillData(object entity)
        {
            base.FillData(entity);
            var sl = (SubscriberLogin)entity;
            OtherIdentificationSelection = sl.OtherIdentification != "" ? true : false;
        }
    }  
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using KMEnums;
using KMEntities;
using KMModels.Attributes;

namespace KMModels.PostModels
{
    public class FormPropertiesPostModel : FormPostModelBase
    {
        public bool Iframe { get; set; }        

        public ResultType? ConfirmationPageType { get; set; }
        [RequiredIf("ConfirmationPageType", ResultType.URL, "Confirmation URL Required")]
        public string ConfirmationPageUrl { get; set; }
        [RequiredIf("ConfirmationPageType", ResultType.Message, "Confirmation Message Required")]
        public string ConfirmationPageMessage { get; set; }
        public string ConfirmationPageJsMessage { get; set; }

        [RequiredIf("ConfirmationPageType", ResultType.MessageAndURL, "Confirmation URL Required")]
        public string ConfirmationPageMAUUrl { get; set; }
        [RequiredIf("ConfirmationPageType", ResultType.MessageAndURL, "Confirmation Message Required")]
        public string ConfirmationPageMAUMessage { get; set; }
        public string ConfirmationPageJsMAUMessage { get; set; }
        [RequiredIf("ConfirmationPageType", ResultType.MessageAndURL, "Delay Time Required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int Delay { get; set; }

        public ResultType? InactiveRedirectType { get; set; }
        [RequiredIf("InactiveRedirectType", ResultType.URL, "Inactive Redirect Required")]
        public string InactiveRedirectUrl { get; set; }
        [RequiredIf("InactiveRedirectType", ResultType.Message, "Inactive Redirect Required")]
        public string InactiveRedirectMessage { get; set; }

        public OptInType OptInType { get; set; }

        public bool LanguageTranslationType { get; set; }

        [Required(ErrorMessage = "Submit Button Text Required")]
        public string SubmitButtonText { get; set; }

        [GetFromField("ParentForm_ID")]
        public int? parentId { get; private set; }

        public bool HasParent { get { return parentId.HasValue; } }

        public override void FillData(object entity)
        {
            base.FillData(entity);

            var form = (Form)entity;

            var results = form.FormResults;
            foreach (var result in results) 
            {
                var formResultType = (FormResultType)result.ResultType;
                if(formResultType == FormResultType.ConfirmationPage)
                {
                    if (result.URL != null && result.Message != null)
                    {
                        ConfirmationPageType = ResultType.MessageAndURL;
                        ConfirmationPageMAUUrl = result.URL;
                        ConfirmationPageMAUMessage = result.Message;
                        if (result.JsMessage != null)
                            ConfirmationPageJsMAUMessage = result.JsMessage;
                    }
                    else if (result.URL != null)
                    {
                        ConfirmationPageType = ResultType.URL;
                        ConfirmationPageUrl = result.URL;
                    }
                    else if(result.Message != null)
                    {
                        ConfirmationPageType = ResultType.Message;
                        ConfirmationPageMessage = result.Message;
                        if (result.JsMessage != null)
                            ConfirmationPageJsMessage = result.JsMessage;
                    }
                }
                if (formResultType == FormResultType.InactiveRedirect) 
                {
                    if (result.URL != null)
                    {
                        InactiveRedirectType = ResultType.URL;
                        InactiveRedirectUrl = result.URL;
                    }
                    if (result.Message != null)
                    {
                        InactiveRedirectType = ResultType.Message;
                        InactiveRedirectMessage = result.Message;
                    }
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class Gateway : GatewayBase
    {
        public Gateway()
        {
            GatewayID = -1;
            CustomerID = -1;
            Name = string.Empty;
            PubCode = string.Empty;
            TypeCode = string.Empty;
            GroupID = -1;
            Header = string.Empty;
            Footer = string.Empty;
            ShowForgotPassword = false;
            ForgotPasswordText = string.Empty;
            ShowSignup = false;
            SignupText = string.Empty;
            SignupURL = string.Empty;
            SubmitText = string.Empty;
            UseStyleFrom = string.Empty;
            Style = string.Empty;
            UseConfirmation = false;
            ConfirmationMessage = string.Empty;
            ConfirmationText = string.Empty;
            UseRedirect = false;
            RedirectURL = string.Empty;
            RedirectDelay = 0;
            LoginOrCapture = string.Empty;
            ValidateEmail = false;
            ValidatePassword = false;
            ValidateCustom = false;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = false;
            
            GatewayValues = new List<GatewayValue>();
        }

        #region properties

        [DataMember]
        public int CustomerID { get; set; }
       
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string PubCode { get; set; }

        [DataMember]
        public string TypeCode { get; set; }

        [DataMember]
        public int GroupID { get; set; }

        [DataMember]
        public string Header { get; set; }

        [DataMember]
        public string Footer { get; set; }

        [DataMember]
        public bool ShowForgotPassword { get; set; }

        [DataMember]
        public string ForgotPasswordText { get; set; }

        [DataMember]
        public bool ShowSignup { get; set; }

        [DataMember]
        public string SignupText { get; set; }

        [DataMember]
        public string SignupURL { get; set; }

        [DataMember]
        public string SubmitText { get; set; }

        [DataMember]
        public string UseStyleFrom { get; set; }

        [DataMember]
        public string Style { get; set; }

        [DataMember]
        public bool UseConfirmation { get; set; }

        [DataMember]
        public string ConfirmationMessage { get; set; }

        [DataMember]
        public string ConfirmationText{get;set;}

        [DataMember]
        public bool UseRedirect { get; set; }

        [DataMember]
        public string RedirectURL { get; set; }

        [DataMember]
        public int RedirectDelay { get; set; }

        [DataMember]
        public string LoginOrCapture { get; set; }

        [DataMember]
        public bool ValidateEmail { get; set; }

        [DataMember]
        public bool ValidatePassword { get; set; }

        [DataMember]
        public bool ValidateCustom { get; set; }

        [DataMember]
        public List<GatewayValue> GatewayValues { get; set; }
        #endregion
    }
}

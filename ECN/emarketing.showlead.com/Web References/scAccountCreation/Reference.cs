﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18052.
// 
#pragma warning disable 1591

namespace ecn.showcare.wizard.scAccountCreation {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="AccountCreationSoap", Namespace="http://showcare.ecn5.com/ecn.showcare.webservice/KM/AccountCreation.asmx")]
    public partial class AccountCreation : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback setCustomerContactInformationOperationCompleted;
        
        private System.Threading.SendOrPostCallback setupCustomerAccountOperationCompleted;
        
        private System.Threading.SendOrPostCallback setupUserAccountOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public AccountCreation() {
            this.Url = "http://showcare.ecn5.com/ecn.showcare.webservice/KM/AccountCreation.asmx";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event setCustomerContactInformationCompletedEventHandler setCustomerContactInformationCompleted;
        
        /// <remarks/>
        public event setupCustomerAccountCompletedEventHandler setupCustomerAccountCompleted;
        
        /// <remarks/>
        public event setupUserAccountCompletedEventHandler setupUserAccountCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://showcare.ecn5.com/ecn.showcare.webservice/KM/AccountCreation.asmx/setCusto" +
            "merContactInformation", RequestNamespace="http://showcare.ecn5.com/ecn.showcare.webservice/KM/AccountCreation.asmx", ResponseNamespace="http://showcare.ecn5.com/ecn.showcare.webservice/KM/AccountCreation.asmx", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public Contact setCustomerContactInformation(string customerContactFirstName, string customerContactLastName, string customerContactAddress, string customerContactCity, string customerContactState, string customerContactCountry, string customerContactZip, string customerContactPhone, string customerContactFax, string customerContactEmailAddress, string customerContactUID) {
            object[] results = this.Invoke("setCustomerContactInformation", new object[] {
                        customerContactFirstName,
                        customerContactLastName,
                        customerContactAddress,
                        customerContactCity,
                        customerContactState,
                        customerContactCountry,
                        customerContactZip,
                        customerContactPhone,
                        customerContactFax,
                        customerContactEmailAddress,
                        customerContactUID});
            return ((Contact)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginsetCustomerContactInformation(string customerContactFirstName, string customerContactLastName, string customerContactAddress, string customerContactCity, string customerContactState, string customerContactCountry, string customerContactZip, string customerContactPhone, string customerContactFax, string customerContactEmailAddress, string customerContactUID, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("setCustomerContactInformation", new object[] {
                        customerContactFirstName,
                        customerContactLastName,
                        customerContactAddress,
                        customerContactCity,
                        customerContactState,
                        customerContactCountry,
                        customerContactZip,
                        customerContactPhone,
                        customerContactFax,
                        customerContactEmailAddress,
                        customerContactUID}, callback, asyncState);
        }
        
        /// <remarks/>
        public Contact EndsetCustomerContactInformation(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((Contact)(results[0]));
        }
        
        /// <remarks/>
        public void setCustomerContactInformationAsync(string customerContactFirstName, string customerContactLastName, string customerContactAddress, string customerContactCity, string customerContactState, string customerContactCountry, string customerContactZip, string customerContactPhone, string customerContactFax, string customerContactEmailAddress, string customerContactUID) {
            this.setCustomerContactInformationAsync(customerContactFirstName, customerContactLastName, customerContactAddress, customerContactCity, customerContactState, customerContactCountry, customerContactZip, customerContactPhone, customerContactFax, customerContactEmailAddress, customerContactUID, null);
        }
        
        /// <remarks/>
        public void setCustomerContactInformationAsync(string customerContactFirstName, string customerContactLastName, string customerContactAddress, string customerContactCity, string customerContactState, string customerContactCountry, string customerContactZip, string customerContactPhone, string customerContactFax, string customerContactEmailAddress, string customerContactUID, object userState) {
            if ((this.setCustomerContactInformationOperationCompleted == null)) {
                this.setCustomerContactInformationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnsetCustomerContactInformationOperationCompleted);
            }
            this.InvokeAsync("setCustomerContactInformation", new object[] {
                        customerContactFirstName,
                        customerContactLastName,
                        customerContactAddress,
                        customerContactCity,
                        customerContactState,
                        customerContactCountry,
                        customerContactZip,
                        customerContactPhone,
                        customerContactFax,
                        customerContactEmailAddress,
                        customerContactUID}, this.setCustomerContactInformationOperationCompleted, userState);
        }
        
        private void OnsetCustomerContactInformationOperationCompleted(object arg) {
            if ((this.setCustomerContactInformationCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.setCustomerContactInformationCompleted(this, new setCustomerContactInformationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://showcare.ecn5.com/ecn.showcare.webservice/KM/AccountCreation.asmx/setupCus" +
            "tomerAccount", RequestNamespace="http://showcare.ecn5.com/ecn.showcare.webservice/KM/AccountCreation.asmx", ResponseNamespace="http://showcare.ecn5.com/ecn.showcare.webservice/KM/AccountCreation.asmx", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int setupCustomerAccount(Contact customerContactInformation, string customerName) {
            object[] results = this.Invoke("setupCustomerAccount", new object[] {
                        customerContactInformation,
                        customerName});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginsetupCustomerAccount(Contact customerContactInformation, string customerName, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("setupCustomerAccount", new object[] {
                        customerContactInformation,
                        customerName}, callback, asyncState);
        }
        
        /// <remarks/>
        public int EndsetupCustomerAccount(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void setupCustomerAccountAsync(Contact customerContactInformation, string customerName) {
            this.setupCustomerAccountAsync(customerContactInformation, customerName, null);
        }
        
        /// <remarks/>
        public void setupCustomerAccountAsync(Contact customerContactInformation, string customerName, object userState) {
            if ((this.setupCustomerAccountOperationCompleted == null)) {
                this.setupCustomerAccountOperationCompleted = new System.Threading.SendOrPostCallback(this.OnsetupCustomerAccountOperationCompleted);
            }
            this.InvokeAsync("setupCustomerAccount", new object[] {
                        customerContactInformation,
                        customerName}, this.setupCustomerAccountOperationCompleted, userState);
        }
        
        private void OnsetupCustomerAccountOperationCompleted(object arg) {
            if ((this.setupCustomerAccountCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.setupCustomerAccountCompleted(this, new setupCustomerAccountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://showcare.ecn5.com/ecn.showcare.webservice/KM/AccountCreation.asmx/setupUse" +
            "rAccount", RequestNamespace="http://showcare.ecn5.com/ecn.showcare.webservice/KM/AccountCreation.asmx", ResponseNamespace="http://showcare.ecn5.com/ecn.showcare.webservice/KM/AccountCreation.asmx", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int setupUserAccount(string userName, string password, int customerID) {
            object[] results = this.Invoke("setupUserAccount", new object[] {
                        userName,
                        password,
                        customerID});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginsetupUserAccount(string userName, string password, int customerID, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("setupUserAccount", new object[] {
                        userName,
                        password,
                        customerID}, callback, asyncState);
        }
        
        /// <remarks/>
        public int EndsetupUserAccount(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void setupUserAccountAsync(string userName, string password, int customerID) {
            this.setupUserAccountAsync(userName, password, customerID, null);
        }
        
        /// <remarks/>
        public void setupUserAccountAsync(string userName, string password, int customerID, object userState) {
            if ((this.setupUserAccountOperationCompleted == null)) {
                this.setupUserAccountOperationCompleted = new System.Threading.SendOrPostCallback(this.OnsetupUserAccountOperationCompleted);
            }
            this.InvokeAsync("setupUserAccount", new object[] {
                        userName,
                        password,
                        customerID}, this.setupUserAccountOperationCompleted, userState);
        }
        
        private void OnsetupUserAccountOperationCompleted(object arg) {
            if ((this.setupUserAccountCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.setupUserAccountCompleted(this, new setupUserAccountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18054")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://showcare.ecn5.com/ecn.showcare.webservice/KM/AccountCreation.asmx")]
    public partial class Contact {
        
        private string salutationField;
        
        private string firstNameField;
        
        private string lastNameField;
        
        private string contactNameField;
        
        private string contactTitleField;
        
        private string phoneField;
        
        private string faxField;
        
        private string emailField;
        
        private string streetAddressField;
        
        private string cityField;
        
        private string stateField;
        
        private string countryField;
        
        private string zipField;
        
        private bool isTheSameAsBillingContactField;
        
        private bool isTheSameAsTechContactField;
        
        /// <remarks/>
        public string Salutation {
            get {
                return this.salutationField;
            }
            set {
                this.salutationField = value;
            }
        }
        
        /// <remarks/>
        public string FirstName {
            get {
                return this.firstNameField;
            }
            set {
                this.firstNameField = value;
            }
        }
        
        /// <remarks/>
        public string LastName {
            get {
                return this.lastNameField;
            }
            set {
                this.lastNameField = value;
            }
        }
        
        /// <remarks/>
        public string ContactName {
            get {
                return this.contactNameField;
            }
            set {
                this.contactNameField = value;
            }
        }
        
        /// <remarks/>
        public string ContactTitle {
            get {
                return this.contactTitleField;
            }
            set {
                this.contactTitleField = value;
            }
        }
        
        /// <remarks/>
        public string Phone {
            get {
                return this.phoneField;
            }
            set {
                this.phoneField = value;
            }
        }
        
        /// <remarks/>
        public string Fax {
            get {
                return this.faxField;
            }
            set {
                this.faxField = value;
            }
        }
        
        /// <remarks/>
        public string Email {
            get {
                return this.emailField;
            }
            set {
                this.emailField = value;
            }
        }
        
        /// <remarks/>
        public string StreetAddress {
            get {
                return this.streetAddressField;
            }
            set {
                this.streetAddressField = value;
            }
        }
        
        /// <remarks/>
        public string City {
            get {
                return this.cityField;
            }
            set {
                this.cityField = value;
            }
        }
        
        /// <remarks/>
        public string State {
            get {
                return this.stateField;
            }
            set {
                this.stateField = value;
            }
        }
        
        /// <remarks/>
        public string Country {
            get {
                return this.countryField;
            }
            set {
                this.countryField = value;
            }
        }
        
        /// <remarks/>
        public string Zip {
            get {
                return this.zipField;
            }
            set {
                this.zipField = value;
            }
        }
        
        /// <remarks/>
        public bool IsTheSameAsBillingContact {
            get {
                return this.isTheSameAsBillingContactField;
            }
            set {
                this.isTheSameAsBillingContactField = value;
            }
        }
        
        /// <remarks/>
        public bool IsTheSameAsTechContact {
            get {
                return this.isTheSameAsTechContactField;
            }
            set {
                this.isTheSameAsTechContactField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void setCustomerContactInformationCompletedEventHandler(object sender, setCustomerContactInformationCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class setCustomerContactInformationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal setCustomerContactInformationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Contact Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Contact)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void setupCustomerAccountCompletedEventHandler(object sender, setupCustomerAccountCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class setupCustomerAccountCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal setupCustomerAccountCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void setupUserAccountCompletedEventHandler(object sender, setupUserAccountCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class setupUserAccountCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal setupUserAccountCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591
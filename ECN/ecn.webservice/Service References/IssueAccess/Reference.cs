﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ecn.webservice.IssueAccess {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ThirdPartyRequest", Namespace="http://schemas.datacontract.org/2004/07/MetaPress.API")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ecn.webservice.IssueAccess.ThirdPartyTokenRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ecn.webservice.IssueAccess.ThirdPartyTokenUpdateRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ecn.webservice.IssueAccess.ThirdPartyEditionRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ecn.webservice.IssueAccess.ThirdPartyEditionTokenRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ecn.webservice.IssueAccess.ThirdPartyEditionUrlRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ecn.webservice.IssueAccess.ThirdPartyEditionTokenizedUrlRequest))]
    public partial class ThirdPartyRequest : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ThirdPartyIDField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ThirdPartyID {
            get {
                return this.ThirdPartyIDField;
            }
            set {
                if ((object.ReferenceEquals(this.ThirdPartyIDField, value) != true)) {
                    this.ThirdPartyIDField = value;
                    this.RaisePropertyChanged("ThirdPartyID");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ThirdPartyTokenRequest", Namespace="http://schemas.datacontract.org/2004/07/MetaPress.API")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ecn.webservice.IssueAccess.ThirdPartyTokenUpdateRequest))]
    public partial class ThirdPartyTokenRequest : ecn.webservice.IssueAccess.ThirdPartyRequest {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TokenField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Token {
            get {
                return this.TokenField;
            }
            set {
                if ((object.ReferenceEquals(this.TokenField, value) != true)) {
                    this.TokenField = value;
                    this.RaisePropertyChanged("Token");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ThirdPartyTokenUpdateRequest", Namespace="http://schemas.datacontract.org/2004/07/MetaPress.API")]
    [System.SerializableAttribute()]
    public partial class ThirdPartyTokenUpdateRequest : ecn.webservice.IssueAccess.ThirdPartyTokenRequest {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DataBagField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> ExpirationDateField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DataBag {
            get {
                return this.DataBagField;
            }
            set {
                if ((object.ReferenceEquals(this.DataBagField, value) != true)) {
                    this.DataBagField = value;
                    this.RaisePropertyChanged("DataBag");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> ExpirationDate {
            get {
                return this.ExpirationDateField;
            }
            set {
                if ((this.ExpirationDateField.Equals(value) != true)) {
                    this.ExpirationDateField = value;
                    this.RaisePropertyChanged("ExpirationDate");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ThirdPartyEditionRequest", Namespace="http://schemas.datacontract.org/2004/07/MetaPress.API")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ecn.webservice.IssueAccess.ThirdPartyEditionTokenRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ecn.webservice.IssueAccess.ThirdPartyEditionUrlRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ecn.webservice.IssueAccess.ThirdPartyEditionTokenizedUrlRequest))]
    public partial class ThirdPartyEditionRequest : ecn.webservice.IssueAccess.ThirdPartyRequest {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ThirdPartyEditionIDField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ThirdPartyEditionID {
            get {
                return this.ThirdPartyEditionIDField;
            }
            set {
                if ((object.ReferenceEquals(this.ThirdPartyEditionIDField, value) != true)) {
                    this.ThirdPartyEditionIDField = value;
                    this.RaisePropertyChanged("ThirdPartyEditionID");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ThirdPartyEditionTokenRequest", Namespace="http://schemas.datacontract.org/2004/07/MetaPress.API")]
    [System.SerializableAttribute()]
    public partial class ThirdPartyEditionTokenRequest : ecn.webservice.IssueAccess.ThirdPartyEditionRequest {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> MaxNumberOfTokenUsesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int NumberOfTokensRequestedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TokenDataBagField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> TokenExpirationDateField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> MaxNumberOfTokenUses {
            get {
                return this.MaxNumberOfTokenUsesField;
            }
            set {
                if ((this.MaxNumberOfTokenUsesField.Equals(value) != true)) {
                    this.MaxNumberOfTokenUsesField = value;
                    this.RaisePropertyChanged("MaxNumberOfTokenUses");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int NumberOfTokensRequested {
            get {
                return this.NumberOfTokensRequestedField;
            }
            set {
                if ((this.NumberOfTokensRequestedField.Equals(value) != true)) {
                    this.NumberOfTokensRequestedField = value;
                    this.RaisePropertyChanged("NumberOfTokensRequested");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TokenDataBag {
            get {
                return this.TokenDataBagField;
            }
            set {
                if ((object.ReferenceEquals(this.TokenDataBagField, value) != true)) {
                    this.TokenDataBagField = value;
                    this.RaisePropertyChanged("TokenDataBag");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> TokenExpirationDate {
            get {
                return this.TokenExpirationDateField;
            }
            set {
                if ((this.TokenExpirationDateField.Equals(value) != true)) {
                    this.TokenExpirationDateField = value;
                    this.RaisePropertyChanged("TokenExpirationDate");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ThirdPartyEditionUrlRequest", Namespace="http://schemas.datacontract.org/2004/07/MetaPress.API")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ecn.webservice.IssueAccess.ThirdPartyEditionTokenizedUrlRequest))]
    public partial class ThirdPartyEditionUrlRequest : ecn.webservice.IssueAccess.ThirdPartyEditionRequest {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> PageIndexField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> PageSetIndexField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TokenPlaceholderField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> PageIndex {
            get {
                return this.PageIndexField;
            }
            set {
                if ((this.PageIndexField.Equals(value) != true)) {
                    this.PageIndexField = value;
                    this.RaisePropertyChanged("PageIndex");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> PageSetIndex {
            get {
                return this.PageSetIndexField;
            }
            set {
                if ((this.PageSetIndexField.Equals(value) != true)) {
                    this.PageSetIndexField = value;
                    this.RaisePropertyChanged("PageSetIndex");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TokenPlaceholder {
            get {
                return this.TokenPlaceholderField;
            }
            set {
                if ((object.ReferenceEquals(this.TokenPlaceholderField, value) != true)) {
                    this.TokenPlaceholderField = value;
                    this.RaisePropertyChanged("TokenPlaceholder");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ThirdPartyEditionTokenizedUrlRequest", Namespace="http://schemas.datacontract.org/2004/07/MetaPress.API")]
    [System.SerializableAttribute()]
    public partial class ThirdPartyEditionTokenizedUrlRequest : ecn.webservice.IssueAccess.ThirdPartyEditionUrlRequest {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> MaxNumberOfTokenUsesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TokenDataBagField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> TokenExpirationDateField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> MaxNumberOfTokenUses {
            get {
                return this.MaxNumberOfTokenUsesField;
            }
            set {
                if ((this.MaxNumberOfTokenUsesField.Equals(value) != true)) {
                    this.MaxNumberOfTokenUsesField = value;
                    this.RaisePropertyChanged("MaxNumberOfTokenUses");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TokenDataBag {
            get {
                return this.TokenDataBagField;
            }
            set {
                if ((object.ReferenceEquals(this.TokenDataBagField, value) != true)) {
                    this.TokenDataBagField = value;
                    this.RaisePropertyChanged("TokenDataBag");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> TokenExpirationDate {
            get {
                return this.TokenExpirationDateField;
            }
            set {
                if ((this.TokenExpirationDateField.Equals(value) != true)) {
                    this.TokenExpirationDateField = value;
                    this.RaisePropertyChanged("TokenExpirationDate");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EditionUrl", Namespace="http://schemas.datacontract.org/2004/07/MetaPress.API")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ecn.webservice.IssueAccess.TokenizedUrl))]
    public partial class EditionUrl : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UrlField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UrlWithPlaceholderField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Url {
            get {
                return this.UrlField;
            }
            set {
                if ((object.ReferenceEquals(this.UrlField, value) != true)) {
                    this.UrlField = value;
                    this.RaisePropertyChanged("Url");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UrlWithPlaceholder {
            get {
                return this.UrlWithPlaceholderField;
            }
            set {
                if ((object.ReferenceEquals(this.UrlWithPlaceholderField, value) != true)) {
                    this.UrlWithPlaceholderField = value;
                    this.RaisePropertyChanged("UrlWithPlaceholder");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TokenizedUrl", Namespace="http://schemas.datacontract.org/2004/07/MetaPress.API")]
    [System.SerializableAttribute()]
    public partial class TokenizedUrl : ecn.webservice.IssueAccess.EditionUrl {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TokenField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TokenizedEditionUrlField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Token {
            get {
                return this.TokenField;
            }
            set {
                if ((object.ReferenceEquals(this.TokenField, value) != true)) {
                    this.TokenField = value;
                    this.RaisePropertyChanged("Token");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TokenizedEditionUrl {
            get {
                return this.TokenizedEditionUrlField;
            }
            set {
                if ((object.ReferenceEquals(this.TokenizedEditionUrlField, value) != true)) {
                    this.TokenizedEditionUrlField = value;
                    this.RaisePropertyChanged("TokenizedEditionUrl");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TokenMetadata", Namespace="http://schemas.datacontract.org/2004/07/MetaPress.API")]
    [System.SerializableAttribute()]
    public partial class TokenMetadata : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DataBlobField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> ExpirationDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TokenField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DataBlob {
            get {
                return this.DataBlobField;
            }
            set {
                if ((object.ReferenceEquals(this.DataBlobField, value) != true)) {
                    this.DataBlobField = value;
                    this.RaisePropertyChanged("DataBlob");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> ExpirationDate {
            get {
                return this.ExpirationDateField;
            }
            set {
                if ((this.ExpirationDateField.Equals(value) != true)) {
                    this.ExpirationDateField = value;
                    this.RaisePropertyChanged("ExpirationDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Token {
            get {
                return this.TokenField;
            }
            set {
                if ((object.ReferenceEquals(this.TokenField, value) != true)) {
                    this.TokenField = value;
                    this.RaisePropertyChanged("Token");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="IssueAccess.IIssueAccess")]
    public interface IIssueAccess {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IIssueAccess/GetEditionTokenAndUrl", ReplyAction="http://tempuri.org/IIssueAccess/GetEditionTokenAndUrlResponse")]
        ecn.webservice.IssueAccess.TokenizedUrl GetEditionTokenAndUrl(ecn.webservice.IssueAccess.ThirdPartyEditionTokenizedUrlRequest tokenizedUrlRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IIssueAccess/GetEditionUrl", ReplyAction="http://tempuri.org/IIssueAccess/GetEditionUrlResponse")]
        ecn.webservice.IssueAccess.EditionUrl GetEditionUrl(ecn.webservice.IssueAccess.ThirdPartyEditionUrlRequest urlRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IIssueAccess/GetTokens", ReplyAction="http://tempuri.org/IIssueAccess/GetTokensResponse")]
        string[] GetTokens(ecn.webservice.IssueAccess.ThirdPartyEditionTokenRequest editionTokenRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IIssueAccess/GetTokenMetadata", ReplyAction="http://tempuri.org/IIssueAccess/GetTokenMetadataResponse")]
        ecn.webservice.IssueAccess.TokenMetadata GetTokenMetadata(ecn.webservice.IssueAccess.ThirdPartyTokenRequest tokenRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IIssueAccess/UpdateTokenMetadata", ReplyAction="http://tempuri.org/IIssueAccess/UpdateTokenMetadataResponse")]
        bool UpdateTokenMetadata(ecn.webservice.IssueAccess.ThirdPartyTokenUpdateRequest databagUpdateRequest);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IIssueAccessChannel : ecn.webservice.IssueAccess.IIssueAccess, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IssueAccessClient : System.ServiceModel.ClientBase<ecn.webservice.IssueAccess.IIssueAccess>, ecn.webservice.IssueAccess.IIssueAccess {
        
        public IssueAccessClient() {
        }
        
        public IssueAccessClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IssueAccessClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IssueAccessClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IssueAccessClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public ecn.webservice.IssueAccess.TokenizedUrl GetEditionTokenAndUrl(ecn.webservice.IssueAccess.ThirdPartyEditionTokenizedUrlRequest tokenizedUrlRequest) {
            return base.Channel.GetEditionTokenAndUrl(tokenizedUrlRequest);
        }
        
        public ecn.webservice.IssueAccess.EditionUrl GetEditionUrl(ecn.webservice.IssueAccess.ThirdPartyEditionUrlRequest urlRequest) {
            return base.Channel.GetEditionUrl(urlRequest);
        }
        
        public string[] GetTokens(ecn.webservice.IssueAccess.ThirdPartyEditionTokenRequest editionTokenRequest) {
            return base.Channel.GetTokens(editionTokenRequest);
        }
        
        public ecn.webservice.IssueAccess.TokenMetadata GetTokenMetadata(ecn.webservice.IssueAccess.ThirdPartyTokenRequest tokenRequest) {
            return base.Channel.GetTokenMetadata(tokenRequest);
        }
        
        public bool UpdateTokenMetadata(ecn.webservice.IssueAccess.ThirdPartyTokenUpdateRequest databagUpdateRequest) {
            return base.Channel.UpdateTokenMetadata(databagUpdateRequest);
        }
    }
}
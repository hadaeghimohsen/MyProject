﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace System.MessageBroadcast.iNotiSmsService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="iNotiSMSSoap", Namespace="http://tempuri.org/")]
    public partial class iNotiSMS : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback SendSingleSMSOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendBatchSMSOperationCompleted;
        
        private System.Threading.SendOrPostCallback DeliverSMSOperationCompleted;
        
        private System.Threading.SendOrPostCallback DeliverBatchSMSOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetChargeRemainingOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendP2PSMSOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public iNotiSMS() {
            this.Url = global::System.MessageBroadcast.Properties.Settings.Default.System_MessageBroadcast_iNotiSmsService_iNotiSMS;
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
        public event SendSingleSMSCompletedEventHandler SendSingleSMSCompleted;
        
        /// <remarks/>
        public event SendBatchSMSCompletedEventHandler SendBatchSMSCompleted;
        
        /// <remarks/>
        public event DeliverSMSCompletedEventHandler DeliverSMSCompleted;
        
        /// <remarks/>
        public event DeliverBatchSMSCompletedEventHandler DeliverBatchSMSCompleted;
        
        /// <remarks/>
        public event GetChargeRemainingCompletedEventHandler GetChargeRemainingCompleted;
        
        /// <remarks/>
        public event SendP2PSMSCompletedEventHandler SendP2PSMSCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendSingleSMS", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public long SendSingleSMS(string UserName, string Password, string LineNo, string MobileNumber, string Message) {
            object[] results = this.Invoke("SendSingleSMS", new object[] {
                        UserName,
                        Password,
                        LineNo,
                        MobileNumber,
                        Message});
            return ((long)(results[0]));
        }
        
        /// <remarks/>
        public void SendSingleSMSAsync(string UserName, string Password, string LineNo, string MobileNumber, string Message) {
            this.SendSingleSMSAsync(UserName, Password, LineNo, MobileNumber, Message, null);
        }
        
        /// <remarks/>
        public void SendSingleSMSAsync(string UserName, string Password, string LineNo, string MobileNumber, string Message, object userState) {
            if ((this.SendSingleSMSOperationCompleted == null)) {
                this.SendSingleSMSOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendSingleSMSOperationCompleted);
            }
            this.InvokeAsync("SendSingleSMS", new object[] {
                        UserName,
                        Password,
                        LineNo,
                        MobileNumber,
                        Message}, this.SendSingleSMSOperationCompleted, userState);
        }
        
        private void OnSendSingleSMSOperationCompleted(object arg) {
            if ((this.SendSingleSMSCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendSingleSMSCompleted(this, new SendSingleSMSCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendBatchSMS", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public long SendBatchSMS(string UserName, string Password, string LineNo, string[] MobileNumbers, string Message) {
            object[] results = this.Invoke("SendBatchSMS", new object[] {
                        UserName,
                        Password,
                        LineNo,
                        MobileNumbers,
                        Message});
            return ((long)(results[0]));
        }
        
        /// <remarks/>
        public void SendBatchSMSAsync(string UserName, string Password, string LineNo, string[] MobileNumbers, string Message) {
            this.SendBatchSMSAsync(UserName, Password, LineNo, MobileNumbers, Message, null);
        }
        
        /// <remarks/>
        public void SendBatchSMSAsync(string UserName, string Password, string LineNo, string[] MobileNumbers, string Message, object userState) {
            if ((this.SendBatchSMSOperationCompleted == null)) {
                this.SendBatchSMSOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendBatchSMSOperationCompleted);
            }
            this.InvokeAsync("SendBatchSMS", new object[] {
                        UserName,
                        Password,
                        LineNo,
                        MobileNumbers,
                        Message}, this.SendBatchSMSOperationCompleted, userState);
        }
        
        private void OnSendBatchSMSOperationCompleted(object arg) {
            if ((this.SendBatchSMSCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendBatchSMSCompleted(this, new SendBatchSMSCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/DeliverSMS", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public long DeliverSMS(string UserName, string Password, string LineNo, string MobileNumber, string BulkID) {
            object[] results = this.Invoke("DeliverSMS", new object[] {
                        UserName,
                        Password,
                        LineNo,
                        MobileNumber,
                        BulkID});
            return ((long)(results[0]));
        }
        
        /// <remarks/>
        public void DeliverSMSAsync(string UserName, string Password, string LineNo, string MobileNumber, string BulkID) {
            this.DeliverSMSAsync(UserName, Password, LineNo, MobileNumber, BulkID, null);
        }
        
        /// <remarks/>
        public void DeliverSMSAsync(string UserName, string Password, string LineNo, string MobileNumber, string BulkID, object userState) {
            if ((this.DeliverSMSOperationCompleted == null)) {
                this.DeliverSMSOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeliverSMSOperationCompleted);
            }
            this.InvokeAsync("DeliverSMS", new object[] {
                        UserName,
                        Password,
                        LineNo,
                        MobileNumber,
                        BulkID}, this.DeliverSMSOperationCompleted, userState);
        }
        
        private void OnDeliverSMSOperationCompleted(object arg) {
            if ((this.DeliverSMSCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeliverSMSCompleted(this, new DeliverSMSCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/DeliverBatchSMS", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public long DeliverBatchSMS(string UserName, string Password, string LineNo, string BulkID, [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)] out DeliveryList[] ResultList) {
            object[] results = this.Invoke("DeliverBatchSMS", new object[] {
                        UserName,
                        Password,
                        LineNo,
                        BulkID});
            ResultList = ((DeliveryList[])(results[1]));
            return ((long)(results[0]));
        }
        
        /// <remarks/>
        public void DeliverBatchSMSAsync(string UserName, string Password, string LineNo, string BulkID) {
            this.DeliverBatchSMSAsync(UserName, Password, LineNo, BulkID, null);
        }
        
        /// <remarks/>
        public void DeliverBatchSMSAsync(string UserName, string Password, string LineNo, string BulkID, object userState) {
            if ((this.DeliverBatchSMSOperationCompleted == null)) {
                this.DeliverBatchSMSOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeliverBatchSMSOperationCompleted);
            }
            this.InvokeAsync("DeliverBatchSMS", new object[] {
                        UserName,
                        Password,
                        LineNo,
                        BulkID}, this.DeliverBatchSMSOperationCompleted, userState);
        }
        
        private void OnDeliverBatchSMSOperationCompleted(object arg) {
            if ((this.DeliverBatchSMSCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeliverBatchSMSCompleted(this, new DeliverBatchSMSCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetChargeRemaining", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public float GetChargeRemaining(string UserName, string Password) {
            object[] results = this.Invoke("GetChargeRemaining", new object[] {
                        UserName,
                        Password});
            return ((float)(results[0]));
        }
        
        /// <remarks/>
        public void GetChargeRemainingAsync(string UserName, string Password) {
            this.GetChargeRemainingAsync(UserName, Password, null);
        }
        
        /// <remarks/>
        public void GetChargeRemainingAsync(string UserName, string Password, object userState) {
            if ((this.GetChargeRemainingOperationCompleted == null)) {
                this.GetChargeRemainingOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetChargeRemainingOperationCompleted);
            }
            this.InvokeAsync("GetChargeRemaining", new object[] {
                        UserName,
                        Password}, this.GetChargeRemainingOperationCompleted, userState);
        }
        
        private void OnGetChargeRemainingOperationCompleted(object arg) {
            if ((this.GetChargeRemainingCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetChargeRemainingCompleted(this, new GetChargeRemainingCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendP2PSMS", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public long SendP2PSMS(string UserName, string Password, string LineNo, string[] MobileNumbers, string[] Message, long TotalPart, out string[] SMSIDs) {
            object[] results = this.Invoke("SendP2PSMS", new object[] {
                        UserName,
                        Password,
                        LineNo,
                        MobileNumbers,
                        Message,
                        TotalPart});
            SMSIDs = ((string[])(results[1]));
            return ((long)(results[0]));
        }
        
        /// <remarks/>
        public void SendP2PSMSAsync(string UserName, string Password, string LineNo, string[] MobileNumbers, string[] Message, long TotalPart) {
            this.SendP2PSMSAsync(UserName, Password, LineNo, MobileNumbers, Message, TotalPart, null);
        }
        
        /// <remarks/>
        public void SendP2PSMSAsync(string UserName, string Password, string LineNo, string[] MobileNumbers, string[] Message, long TotalPart, object userState) {
            if ((this.SendP2PSMSOperationCompleted == null)) {
                this.SendP2PSMSOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendP2PSMSOperationCompleted);
            }
            this.InvokeAsync("SendP2PSMS", new object[] {
                        UserName,
                        Password,
                        LineNo,
                        MobileNumbers,
                        Message,
                        TotalPart}, this.SendP2PSMSOperationCompleted, userState);
        }
        
        private void OnSendP2PSMSOperationCompleted(object arg) {
            if ((this.SendP2PSMSCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendP2PSMSCompleted(this, new SendP2PSMSCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class DeliveryList {
        
        private string mobileField;
        
        private long statusField;
        
        private System.DateTime lastDeliverDateTimeField;
        
        /// <remarks/>
        public string Mobile {
            get {
                return this.mobileField;
            }
            set {
                this.mobileField = value;
            }
        }
        
        /// <remarks/>
        public long Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime LastDeliverDateTime {
            get {
                return this.lastDeliverDateTimeField;
            }
            set {
                this.lastDeliverDateTimeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void SendSingleSMSCompletedEventHandler(object sender, SendSingleSMSCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendSingleSMSCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendSingleSMSCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public long Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((long)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void SendBatchSMSCompletedEventHandler(object sender, SendBatchSMSCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendBatchSMSCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendBatchSMSCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public long Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((long)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void DeliverSMSCompletedEventHandler(object sender, DeliverSMSCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeliverSMSCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DeliverSMSCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public long Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((long)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void DeliverBatchSMSCompletedEventHandler(object sender, DeliverBatchSMSCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeliverBatchSMSCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DeliverBatchSMSCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public long Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((long)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public DeliveryList[] ResultList {
            get {
                this.RaiseExceptionIfNecessary();
                return ((DeliveryList[])(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void GetChargeRemainingCompletedEventHandler(object sender, GetChargeRemainingCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetChargeRemainingCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetChargeRemainingCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public float Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((float)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void SendP2PSMSCompletedEventHandler(object sender, SendP2PSMSCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendP2PSMSCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendP2PSMSCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public long Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((long)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string[] SMSIDs {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[1]));
            }
        }
    }
}

#pragma warning restore 1591
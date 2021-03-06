﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PMQuanLy.ServiceAppCust {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:ServiceCustomerControllerwsdl", ConfigurationName="ServiceAppCust.ServiceCustomerControllerPortType")]
    public interface ServiceCustomerControllerPortType {
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ServiceCustomerControllerwsdl#Login", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        string Login(string ten_dang_nhap, string mat_khau);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ServiceCustomerControllerwsdl#Login", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        System.Threading.Tasks.Task<string> LoginAsync(string ten_dang_nhap, string mat_khau);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ServiceCustomerControllerwsdl#listProduct", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        string listProduct(int id_khach_hang);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ServiceCustomerControllerwsdl#listProduct", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        System.Threading.Tasks.Task<string> listProductAsync(int id_khach_hang);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ServiceCustomerControllerwsdl#getGoodsInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        string getGoodsInfo(string qr_code);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ServiceCustomerControllerwsdl#getGoodsInfo", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        System.Threading.Tasks.Task<string> getGoodsInfoAsync(string qr_code);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ServiceCustomerControllerwsdl#checkVersion", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        int checkVersion(int @object, int id_khach_hang);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ServiceCustomerControllerwsdl#checkVersion", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        System.Threading.Tasks.Task<int> checkVersionAsync(int @object, int id_khach_hang);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ServiceCustomerControllerwsdl#syncTrans", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        int syncTrans(int id_khach_hang, int verion, string xml);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ServiceCustomerControllerwsdl#syncTrans", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        System.Threading.Tasks.Task<int> syncTransAsync(int id_khach_hang, int verion, string xml);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServiceCustomerControllerPortTypeChannel : PMQuanLy.ServiceAppCust.ServiceCustomerControllerPortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceCustomerControllerPortTypeClient : System.ServiceModel.ClientBase<PMQuanLy.ServiceAppCust.ServiceCustomerControllerPortType>, PMQuanLy.ServiceAppCust.ServiceCustomerControllerPortType {
        
        public ServiceCustomerControllerPortTypeClient() {
        }
        
        public ServiceCustomerControllerPortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceCustomerControllerPortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceCustomerControllerPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceCustomerControllerPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Login(string ten_dang_nhap, string mat_khau) {
            return base.Channel.Login(ten_dang_nhap, mat_khau);
        }
        
        public System.Threading.Tasks.Task<string> LoginAsync(string ten_dang_nhap, string mat_khau) {
            return base.Channel.LoginAsync(ten_dang_nhap, mat_khau);
        }
        
        public string listProduct(int id_khach_hang) {
            return base.Channel.listProduct(id_khach_hang);
        }
        
        public System.Threading.Tasks.Task<string> listProductAsync(int id_khach_hang) {
            return base.Channel.listProductAsync(id_khach_hang);
        }
        
        public string getGoodsInfo(string qr_code) {
            return base.Channel.getGoodsInfo(qr_code);
        }
        
        public System.Threading.Tasks.Task<string> getGoodsInfoAsync(string qr_code) {
            return base.Channel.getGoodsInfoAsync(qr_code);
        }
        
        public int checkVersion(int @object, int id_khach_hang) {
            return base.Channel.checkVersion(@object, id_khach_hang);
        }
        
        public System.Threading.Tasks.Task<int> checkVersionAsync(int @object, int id_khach_hang) {
            return base.Channel.checkVersionAsync(@object, id_khach_hang);
        }
        
        public int syncTrans(int id_khach_hang, int verion, string xml) {
            return base.Channel.syncTrans(id_khach_hang, verion, xml);
        }
        
        public System.Threading.Tasks.Task<int> syncTransAsync(int id_khach_hang, int verion, string xml) {
            return base.Channel.syncTransAsync(id_khach_hang, verion, xml);
        }
    }
}

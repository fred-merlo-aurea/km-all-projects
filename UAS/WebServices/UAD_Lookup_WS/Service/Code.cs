using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using UAD_Lookup_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD_Lookup.BusinessLogic.Code;
using EntityCode = FrameworkUAD_Lookup.Entity.Code;
using EnumCodeType = FrameworkUAD_Lookup.Enums.CodeType;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;

namespace UAD_Lookup_WS.Service
{
    public class Code : FrameworkServiceBase, ICode
    {
        private const string EntityName = "Code";
        private const string MethodSave = "Save";
        private const string MethodCodeValueExist = "CodeValueExist";
        private const string MethodCodeExist = "CodeExist";
        private const string MethodSelectCodeValue = "SelectCodeValue";
        private const string MethodSelectCodeName = "SelectCodeName";
        private const string MethodSelectCodeId = "SelectCodeId";
        private const string MethodSelectChildren = "SelectChildren";
        private const string MethodSelectForDemographicAttribute = "SelectForDemographicAttribute";
        private const string MethodSelect = "Select";

        /// <summary>
        /// Selects a list of Code objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of Code objects</returns>
        public Response<List<EntityCode>> Select(Guid accessKey)
        {
            var model = new ServiceRequestModel<List<EntityCode>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ =>  new BusinessLogicWorker().Select()
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Code objects based on the code type ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="codeTypeId">the code type ID</param>
        /// <returns>response.result will contain a list of Code objects</returns>
        public Response<List<EntityCode>> Select(Guid accessKey, int codeTypeId)
        {
            var param = $"CodeTypeId: {codeTypeId}";
            var model = new ServiceRequestModel<List<EntityCode>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ =>  new BusinessLogicWorker().Select(codeTypeId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Code objects based on the code type enum
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="codeType">the code type enum (ACS_Type, ACS_Action, Action, Address, Address_Update_Source, Authorization_Mode, Configuration, Credit_Card, 
        /// Database_Destination, Database_File, Data_Compare_Target, Demographic_Custom_Attributes, Demographic_Premium_Attributes, Demographic_Standard_Attributes,
        /// Execution_Points, Export, Export_Format, Field_Mapping, File_Snippet, File_Status, File_Recurrence, Filter_Type, Match, Operators, Payment, Postal_Service,
        /// Procedure, Profile_Premium_Attributes, Profile_Standard_Attributes, Qualification_Source, Recurrence, Report, Response_Group, Service_Response_Status,
        /// Special_File_Result, Subscriber_Source, Transformation, User_Log, Issue_Permission, Filter_Group, Report_Expression)</param>
        /// <returns>response.result will contain a list of Code objects</returns>
        public Response<List<EntityCode>> Select(Guid accessKey, EnumCodeType codeType)
        {
            var param = $"CodeType: {codeType}";
            var model = new ServiceRequestModel<List<EntityCode>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ =>  new BusinessLogicWorker().Select(codeType)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Code objects for the demographic attributes using the data compare result que ID and the code type enum
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="codeType">the code type enum (ACS_Type, ACS_Action, Action, Address, Address_Update_Source, Authorization_Mode, Configuration, Credit_Card, 
        /// Database_Destination, Database_File, Data_Compare_Target, Demographic_Custom_Attributes, Demographic_Premium_Attributes, Demographic_Standard_Attributes,
        /// Execution_Points, Export, Export_Format, Field_Mapping, File_Snippet, File_Status, File_Recurrence, Filter_Type, Match, Operators, Payment, Postal_Service,
        /// Procedure, Profile_Premium_Attributes, Profile_Standard_Attributes, Qualification_Source, Recurrence, Report, Response_Group, Service_Response_Status,
        /// Special_File_Result, Subscriber_Source, Transformation, User_Log, Issue_Permission, Filter_Group, Report_Expression)</param>
        /// <param name="dataCompareResultQueId">the data compare result que ID</param>
        /// <returns>response.result will contain a list of Code objects</returns>
        public Response<List<EntityCode>> SelectForDemographicAttribute(Guid accessKey, EnumCodeType codeType, int dataCompareResultQueId, string ftpFolder)
        {
            var param = $"CodeType: {codeType} DataCompareResultQueId: {dataCompareResultQueId}";
            var model = new ServiceRequestModel<List<EntityCode>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForDemographicAttribute,
                WorkerFunc = _ =>  new BusinessLogicWorker().SelectForDemographicAttribute(codeType, dataCompareResultQueId, ftpFolder)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects the children of the Code objects based on the parent code ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="parentCodeId">the parent code ID</param>
        /// <returns>response.result will contain a list of Code objects</returns>
        public Response<List<EntityCode>> SelectChildren(Guid accessKey, int parentCodeId)
        {
            var param = $"parentCodeID: {parentCodeId}";
            var model = new ServiceRequestModel<List<EntityCode>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectChildren,
                WorkerFunc = _ =>  new BusinessLogicWorker().SelectChildren(parentCodeId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a Code object based on the code ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="codeId">the code ID</param>
        /// <returns>response.result will contain a Code object</returns>
        public Response<EntityCode> SelectCodeId(Guid accessKey, int codeId)
        {
            var param = $"codeId: {codeId}";
            var model = new ServiceRequestModel<EntityCode>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectCodeId,
                WorkerFunc = _ =>  new BusinessLogicWorker().SelectCodeId(codeId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a Code object based on the code name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="codeType">the code type enum (ACS_Type, ACS_Action, Action, Address, Address_Update_Source, Authorization_Mode, Configuration, Credit_Card, 
        /// Database_Destination, Database_File, Data_Compare_Target, Demographic_Custom_Attributes, Demographic_Premium_Attributes, Demographic_Standard_Attributes,
        /// Execution_Points, Export, Export_Format, Field_Mapping, File_Snippet, File_Status, File_Recurrence, Filter_Type, Match, Operators, Payment, Postal_Service,
        /// Procedure, Profile_Premium_Attributes, Profile_Standard_Attributes, Qualification_Source, Recurrence, Report, Response_Group, Service_Response_Status,
        /// Special_File_Result, Subscriber_Source, Transformation, User_Log, Issue_Permission, Filter_Group, Report_Expression)</param>
        /// <param name="codeName">the code name</param>
        /// <returns>response.result will contain a Code object</returns>
        public Response<EntityCode> SelectCodeName(Guid accessKey, EnumCodeType codeType, string codeName)
        {
            var param = $"CodeType:{codeType} codeName: {codeName}";
            var model = new ServiceRequestModel<EntityCode>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectCodeName,
                WorkerFunc = _ =>  new BusinessLogicWorker().SelectCodeName(codeType, codeName)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a Code object based on the code value
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="codeType">the code type enum (ACS_Type, ACS_Action, Action, Address, Address_Update_Source, Authorization_Mode, Configuration, Credit_Card, 
        /// Database_Destination, Database_File, Data_Compare_Target, Demographic_Custom_Attributes, Demographic_Premium_Attributes, Demographic_Standard_Attributes,
        /// Execution_Points, Export, Export_Format, Field_Mapping, File_Snippet, File_Status, File_Recurrence, Filter_Type, Match, Operators, Payment, Postal_Service,
        /// Procedure, Profile_Premium_Attributes, Profile_Standard_Attributes, Qualification_Source, Recurrence, Report, Response_Group, Service_Response_Status,
        /// Special_File_Result, Subscriber_Source, Transformation, User_Log, Issue_Permission, Filter_Group, Report_Expression)</param>
        /// <param name="codeValue">the code value</param>
        /// <returns>response.result will contain a Code object</returns>
        public Response<EntityCode> SelectCodeValue(Guid accessKey, EnumCodeType codeType, string codeValue)
        {
            var param = $"CodeType:{codeType} codeValue: {codeValue}";
            var model = new ServiceRequestModel<EntityCode>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectCodeValue,
                WorkerFunc = _ =>  new BusinessLogicWorker().SelectCodeValue(codeType, codeValue)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Checks to see if the Code object exists using the code name and the code type ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="codeName">the code name</param>
        /// <param name="codeTypeId">the code type ID</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> CodeExist(Guid accessKey, string codeName, int codeTypeId)
        {
            var param = $"codeName: {codeName} codeTypeID: {codeTypeId}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodCodeExist,
                WorkerFunc = _ =>  new BusinessLogicWorker().CodeExist(codeName, codeTypeId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Checks to see if the Code object exists using the code name and the code type enum
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="codeName">the code name</param>
        /// <param name="codeType">the code type enum (ACS_Type, ACS_Action, Action, Address, Address_Update_Source, Authorization_Mode, Configuration, Credit_Card, 
        /// Database_Destination, Database_File, Data_Compare_Target, Demographic_Custom_Attributes, Demographic_Premium_Attributes, Demographic_Standard_Attributes,
        /// Execution_Points, Export, Export_Format, Field_Mapping, File_Snippet, File_Status, File_Recurrence, Filter_Type, Match, Operators, Payment, Postal_Service,
        /// Procedure, Profile_Premium_Attributes, Profile_Standard_Attributes, Qualification_Source, Recurrence, Report, Response_Group, Service_Response_Status,
        /// Special_File_Result, Subscriber_Source, Transformation, User_Log, Issue_Permission, Filter_Group, Report_Expression)</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> CodeExist(Guid accessKey, string codeName, EnumCodeType codeType)
        {
            var param = $"codeName: {codeName} codeType: {codeType}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodCodeExist,
                WorkerFunc = _ =>  new BusinessLogicWorker().CodeExist(codeName, codeType)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Checks to see if a code value exists using the code type ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="codeValue">the code value</param>
        /// <param name="codeTypeId">the code type ID</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> CodeValueExist(Guid accessKey, string codeValue, int codeTypeId)
        {
            var param = $"codeValue: {codeValue} codeTypeID: {codeTypeId}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodCodeValueExist,
                WorkerFunc = _ =>  new BusinessLogicWorker().CodeValueExist(codeValue, codeTypeId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Checks to see if a code value exists using the code type enum
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="codeValue">the code value</param>
        /// <param name="codeType">the code type enum (ACS_Type, ACS_Action, Action, Address, Address_Update_Source, Authorization_Mode, Configuration, Credit_Card, 
        /// Database_Destination, Database_File, Data_Compare_Target, Demographic_Custom_Attributes, Demographic_Premium_Attributes, Demographic_Standard_Attributes,
        /// Execution_Points, Export, Export_Format, Field_Mapping, File_Snippet, File_Status, File_Recurrence, Filter_Type, Match, Operators, Payment, Postal_Service,
        /// Procedure, Profile_Premium_Attributes, Profile_Standard_Attributes, Qualification_Source, Recurrence, Report, Response_Group, Service_Response_Status,
        /// Special_File_Result, Subscriber_Source, Transformation, User_Log, Issue_Permission, Filter_Group, Report_Expression)</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> CodeValueExist(Guid accessKey, string codeValue, EnumCodeType codeType)
        {
            var param = $"codeValue: {codeValue} codeType: {codeType}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodCodeValueExist,
                WorkerFunc = _ =>  new BusinessLogicWorker().CodeValueExist(codeValue, codeType)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves a Code object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the Code object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.Code x)
        {
            var param = new UtilityJsonFunctions().ToJson(x);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Save(x);
                    request.Succeeded = result > 0;
                    return result;
                }
            };

            return GetResponse(model);
        }
    }
}

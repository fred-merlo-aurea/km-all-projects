using System;
using System.Collections.Generic;
using System.Linq;
using UAD_Lookup_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_Lookup_WS.Service
{
    public class CodeType : ServiceBase, ICodeType
    {
        /// <summary>
        /// Selects a list of CodeType objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of CodeType objects</returns>
        public Response<List<FrameworkUAD_Lookup.Entity.CodeType>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAD_Lookup.Entity.CodeType>> response = new Response<List<FrameworkUAD_Lookup.Entity.CodeType>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, this.GetType().Name.ToString(), "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.CodeType worker = new FrameworkUAD_Lookup.BusinessLogic.CodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select();
                    if (response.Result != null)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a CodeType object based on the code type ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="codeTypeId">the code type ID</param>
        /// <returns>response.result will contain a CodeType object</returns>
        public Response<FrameworkUAD_Lookup.Entity.CodeType> Select(Guid accessKey, int codeTypeId)
        {
            Response<FrameworkUAD_Lookup.Entity.CodeType> response = new Response<FrameworkUAD_Lookup.Entity.CodeType>();
            try
            {
                string param = "CodeTypeId: " + codeTypeId.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, this.GetType().Name.ToString(), "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.CodeType worker = new FrameworkUAD_Lookup.BusinessLogic.CodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(codeTypeId);
                    if (response.Result != null)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a CodeType object based on the code type enum
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="codeType">the code type enum (ACS_Type, ACS_Action, Action, Address, Address_Update_Source, Authorization_Mode, Configuration, Credit_Card, 
        /// Database_Destination, Database_File, Data_Compare_Target, Demographic_Custom_Attributes, Demographic_Premium_Attributes, Demographic_Standard_Attributes,
        /// Execution_Points, Export, Export_Format, Field_Mapping, File_Snippet, File_Status, File_Recurrence, Filter_Type, Match, Operators, Payment, Postal_Service,
        /// Procedure, Profile_Premium_Attributes, Profile_Standard_Attributes, Qualification_Source, Recurrence, Report, Response_Group, Service_Response_Status,
        /// Special_File_Result, Subscriber_Source, Transformation, User_Log, Issue_Permission, Filter_Group, Report_Expression)</param>
        /// <returns>response.result will contain a CodeType object</returns>
        public Response<FrameworkUAD_Lookup.Entity.CodeType> Select(Guid accessKey, FrameworkUAD_Lookup.Enums.CodeType codeType)
        {
            Response<FrameworkUAD_Lookup.Entity.CodeType> response = new Response<FrameworkUAD_Lookup.Entity.CodeType>();
            try
            {
                string param = "codeType: " + codeType.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, this.GetType().Name.ToString(), "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.CodeType worker = new FrameworkUAD_Lookup.BusinessLogic.CodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(codeType);
                    if (response.Result != null)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Saves a CodeType object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the CodeType object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.CodeType x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD_Lookup.Entity.CodeType>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "CodeType", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.CodeType worker = new FrameworkUAD_Lookup.BusinessLogic.CodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
                    if (response.Result >= 0)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}

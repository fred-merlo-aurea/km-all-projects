using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using System.Data;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class Table : ServiceBase, ITable
    {
        /// <summary>
        /// Selects a list of Table objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of Table objects</returns>
        public Response<List<FrameworkUAS.Object.Table>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAS.Object.Table>> response = new Response<List<FrameworkUAS.Object.Table>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "Tables", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.Table worker = new FrameworkUAS.BusinessLogic.Table();
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
                LogError(accessKey, ex, "Table", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a DataTable object based on the specified table, client and file
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="table">the specific table</param>
        /// <param name="client">the specific client</param>
        /// <param name="file">the specific file</param>
        /// <returns>response.result will contain a DataTable object</returns>
        public Response<DataTable> SelectDataTable(Guid accessKey, string table, int client, int file)
        {
            Response<DataTable> response = new Response<DataTable>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "Tables", "SelectDataTable");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.Table worker = new FrameworkUAS.BusinessLogic.Table();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(table, client, file);
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
                LogError(accessKey, ex, "Table", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}

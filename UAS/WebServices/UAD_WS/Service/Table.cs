using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;
using System.Data;

namespace UAD_WS.Service
{
    public class Table : ServiceBase, ITable
    {
        /// <summary>
        /// Selects a list of Tables based on the client and database name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="dbName">the database name</param>
        /// <returns>response.result will contain a list of Table objects</returns>
        public Response<List<FrameworkUAD.Object.Table>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client, string dbName)
        {
            Response<List<FrameworkUAD.Object.Table>> response = new Response<List<FrameworkUAD.Object.Table>>();
            try
            {
                string param = "dbName:" + dbName;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Table", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Table worker = new FrameworkUAD.BusinessLogic.Table();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(client).ToList();
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
        /// Selects a DataTable object based on the database name, the table, the client and the pub code
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="dbName">the database name</param>
        /// <param name="table">the table</param>
        /// <param name="pubCode">the pub code</param>
        /// <returns>response.result will contain a DataTable object</returns>
        public Response<DataTable> SelectDataTable(Guid accessKey, KMPlatform.Object.ClientConnections client, string dbName, string table, string pubCode)
        {
            Response<DataTable> response = new Response<DataTable>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "Tables", "SelectDataTable");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Table worker = new FrameworkUAD.BusinessLogic.Table();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(client, dbName, table, pubCode);
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
    }
}

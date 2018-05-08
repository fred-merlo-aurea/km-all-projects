using System;
using System.Collections.Generic;
using System.Linq;
using UAS_WS.Interface;
using FrameworkUAS.Service;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class Reports : ServiceBase, IReports
    {
        /// <summary>
        /// Gets a list of ClientFileLog object based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// /// <param name="clientName">the start date</param>
        /// <returns>response.result will contain a list of ClientFileLog objects</returns>
        public Response<List<FrameworkUAS.Report.ClientFileLog>> GetClientFileLog(Guid accessKey, int clientID, string clientName)
        {

            Response<List<FrameworkUAS.Report.ClientFileLog>> response = new Response<List<FrameworkUAS.Report.ClientFileLog>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ClientID:" + clientID.ToString(), "Report.ClientFileLog", "GetClientFileLog");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.Reports worker = new FrameworkUAS.BusinessLogic.Reports();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetClientFileLog(clientID, clientName);
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
        /// Gets a list of ClientFileLog objects based on the client ID and the log date of the file
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// /// <param name="clientName">the start date</param>
        /// <param name="logDate">the log date</param>
        /// <returns>response.result will contain a list of ClientFileLog objects</returns>
        public Response<List<FrameworkUAS.Report.ClientFileLog>> GetClientFileLog(Guid accessKey, int clientID, string clientName, DateTime logDate)
        {
            Response<List<FrameworkUAS.Report.ClientFileLog>> response = new Response<List<FrameworkUAS.Report.ClientFileLog>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ClientID:" + clientID.ToString() + " LogDate:" + logDate.ToShortDateString(), "Report.ClientFileLog", "GetClientFileLog");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.Reports worker = new FrameworkUAS.BusinessLogic.Reports();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetClientFileLog(clientID, clientName, logDate);
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
        /// Gets a list of ClientFileLog objects based on the client ID and the start and end dates of the log
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="startDate">the start date</param>
        /// <param name="clientName">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <returns>response.result will contain a list of ClientFileLog objects</returns>
        public Response<List<FrameworkUAS.Report.ClientFileLog>> GetClientFileLog(Guid accessKey, int clientID, string clientName, DateTime startDate, DateTime endDate)
        {
            Response<List<FrameworkUAS.Report.ClientFileLog>> response = new Response<List<FrameworkUAS.Report.ClientFileLog>>();
            try
            {
                string param = "ClientID:" + clientID.ToString() + " startDate:" + startDate.ToShortDateString() + " endDate:" + endDate.ToShortDateString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Report.ClientFileLog", "GetClientFileLog");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.Reports worker = new FrameworkUAS.BusinessLogic.Reports();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetClientFileLog(clientID, clientName, startDate, endDate);
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
        /// Gets a list of FileCount objects based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// /// <param name="clientName">the start date</param>
        /// <returns>response.result will contain a list of FileCount objects</returns>
        public Response<List<FrameworkUAS.Report.FileCount>> GetFileCount(Guid accessKey, int clientID, string clientName)
        {
            Response<List<FrameworkUAS.Report.FileCount>> response = new Response<List<FrameworkUAS.Report.FileCount>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ClientID:" + clientID.ToString(), "Report.FileCount", "GetFileCount");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.Reports worker = new FrameworkUAS.BusinessLogic.Reports();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetFileCount(clientID, clientName);
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
        /// Gets a list of FileCount objects based on the client ID, and the start and end dates of the file
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// /// <param name="clientName">the start date</param>
        /// <param name="startDate">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <returns>response.result will contain a list of FileCount objects</returns>
        public Response<List<FrameworkUAS.Report.FileCount>> GetFileCount(Guid accessKey, int clientID, string clientName, DateTime startDate, DateTime endDate)
        {
            Response<List<FrameworkUAS.Report.FileCount>> response = new Response<List<FrameworkUAS.Report.FileCount>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ClientID:" + clientID.ToString() + " StartDate:" + startDate.ToShortDateString() + " EndDate:" + endDate.ToShortDateString(), "Report.FileCount", "GetFileCount");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.Reports worker = new FrameworkUAS.BusinessLogic.Reports();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetFileCount(clientID, clientName, startDate, endDate);
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
        /// Gets a list of FileCount objects based on the start and end dates of the file
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="startDate">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <returns>response.result will contain a list of FileCount objects</returns>
        public Response<List<FrameworkUAS.Report.FileCount>> GetFileCount(Guid accessKey, DateTime startDate, DateTime endDate)
        {
            Response<List<FrameworkUAS.Report.FileCount>> response = new Response<List<FrameworkUAS.Report.FileCount>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "StartDate:" + startDate.ToShortDateString() + " EndDate:" + endDate.ToShortDateString(), "Report.FileCount", "GetFileCount");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.Reports worker = new FrameworkUAS.BusinessLogic.Reports();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetFileCount(startDate, endDate);
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

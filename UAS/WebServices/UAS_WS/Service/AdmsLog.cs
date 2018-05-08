using System;
using System.Collections.Generic;
using System.Linq;
using UAS_WS.Interface;
using FrameworkUAS.Service;

namespace UAS_WS.Service
{
    public class AdmsLog : ServiceBase, IAdmsLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="sourceFileId"></param>
        /// <returns></returns>
        public Response<List<FrameworkUAS.Entity.AdmsLog>> Select(Guid accessKey, int clientID, int sourceFileId)
        {
            Response<List<FrameworkUAS.Entity.AdmsLog>> response = new Response<List<FrameworkUAS.Entity.AdmsLog>>();
            //try
            //{
            //    string param = "ClientId:" + clientId.ToString() + " Engine:" + engine.ToString() + " IsRunning:" + isRunning.ToString();
            //    FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "EngineLog", "SelectIsRunning");
            //    response.Status = auth.Status;

            //    if (auth.IsAuthenticated == true)
            //    {
            //        FrameworkUAS.BusinessLogic.EngineLog wrk = new FrameworkUAS.BusinessLogic.EngineLog();
            //        response.Message = "AccessKey Validated";
            //        response.Result = wrk.Select(isRunning).ToList();
            //        if (response.Result != null)
            //        {
            //            response.Message = "Success";
            //            response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
            //        }
            //        else
            //        {
            //            response.Message = "Error";
            //            response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
            //    LogError(accessKey, ex, this.GetType().Name.ToString());
            //    response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            //}
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="sourceFileId"></param>
        /// <param name="fileStart"></param>
        /// <returns></returns>
        public Response<List<FrameworkUAS.Entity.AdmsLog>> SelectFileStartDate(Guid accessKey, int clientID, int sourceFileId, DateTime fileStart)
        {
            Response<List<FrameworkUAS.Entity.AdmsLog>> response = new Response<List<FrameworkUAS.Entity.AdmsLog>>();
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="fileNameExact"></param>
        /// <returns></returns>
        public Response<List<FrameworkUAS.Entity.AdmsLog>> SelectFileExactName(Guid accessKey, int clientID, string fileNameExact)
        {
            Response<List<FrameworkUAS.Entity.AdmsLog>> response = new Response<List<FrameworkUAS.Entity.AdmsLog>>();
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="processCode"></param>
        /// <returns></returns>
        public Response<FrameworkUAS.Entity.AdmsLog> SelectProcessCode(Guid accessKey, string processCode)
        {
            Response<FrameworkUAS.Entity.AdmsLog> response = new Response<FrameworkUAS.Entity.AdmsLog>();
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public Response<bool> Save(Guid accessKey, FrameworkUAS.Entity.AdmsLog x)
        {
            Response<bool> response = new Response<bool>();
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="processCode"></param>
        /// <param name="fileStatus"></param>
        /// <param name="userId"></param>
        /// <param name="createLog"></param>
        /// <param name="sourceFileId"></param>
        /// <returns></returns>
        public Response<bool> UpdateFileStatus(Guid accessKey, string processCode, FrameworkUAD_Lookup.Enums.FileStatusType fileStatus, int userId, bool createLog = true, int sourceFileId = -1)
        {
            Response<bool> response = new Response<bool>();
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="processCode"></param>
        /// <param name="step"></param>
        /// <param name="userId"></param>
        /// <param name="createLog"></param>
        /// <param name="sourceFileId"></param>
        /// <returns></returns>
        public Response<bool> UpdateCurrentStep(Guid accessKey, string processCode, FrameworkUAD_Lookup.Enums.ADMS_StepType step, int userId, bool createLog = true, int sourceFileId = -1)
        {
            Response<bool> response = new Response<bool>();
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="processCode"></param>
        /// <param name="status"></param>
        /// <param name="userId"></param>
        /// <param name="createLog"></param>
        /// <param name="sourceFileId"></param>
        /// <returns></returns>
        public Response<bool> UpdateProcessingStatus(Guid accessKey, string processCode, FrameworkUAD_Lookup.Enums.FileStatusType status, int userId, bool createLog = false, int sourceFileId = -1)
        {
            Response<bool> response = new Response<bool>();
            return response;

        }
    }
}

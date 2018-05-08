using System;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;
using System.IO;
using Core_AMS.Utilities;
using KM.Common.Import;
using WebServiceFramework;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.ImportVessel;
using EntityImportVessel = FrameworkUAD.Object.ImportVessel;

namespace UAD_WS.Service
{
    public class ImportVessel : FrameworkServiceBase, IImportVessel
    {
        private const string EntityName = "ImportVessel";
        private const string MethodGetBadData = "GetBadData";
        private const string MethodGetTransformedData = "GetTransformedData";
        private const string MethodGetImportVessel = "GetImportVessel";
        private const string MethodGetImportVesselExcel = "GetImportVesselExcel";
        private const string MethodGetImportVesselDbf = "GetImportVesselDbf";
        private const string MethodGetImportVesselText = "GetImportVesselText";
        private const string MethodLoadFileImportVessel = "LoadFileImportVessel";

        /// <summary>
        /// Gets the customers error message
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="iv">the ImportVessel object</param>
        /// <returns>response.result will contain a string</returns>
        public Response<string> GetCustomerErrorMessage(Guid accessKey, FrameworkUAD.Object.ImportVessel iv)
        {
            Response<string> response = new Response<string>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Object.ImportVessel>(iv);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ImportVessel", "GetCustomerErrorMessage");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    response.Message = "AccessKey Validated";
                    response.Result = FrameworkUAD.BusinessLogic.ImportVessel.GetCustomerErrorMessage(iv);
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
        /// Gets the bad data from the ImportVessels bad data table
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="iv">the ImportVessel object</param>
        /// <returns>response.result will contain a string</returns>
        public Response<string> GetBadData(Guid accessKey, FrameworkUAD.Object.ImportVessel iv)
        {
            var param = new UtilityJsonFunctions().ToJson(iv);
            var model = new ServiceRequestModel<string>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetBadData,
                WorkerFunc = request => BusinessLogicWorker.GetBadData(iv)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Gets the original data from the ImportVessels original data table
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="iv">the ImportVessel object</param>
        /// <returns>response.result will contain a string</returns>
        public Response<string> GetCleanOriginalData(Guid accessKey, FrameworkUAD.Object.ImportVessel iv)
        {
            Response<string> response = new Response<string>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Object.ImportVessel>(iv);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ImportVessel", "GetCleanOriginalData");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    response.Message = "AccessKey Validated";
                    response.Result = FrameworkUAD.BusinessLogic.ImportVessel.GetCleanOriginalData(iv);
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
        /// Get the Transformed data form the ImportVessels transformed data table
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="iv">the ImportVessel object</param>
        /// <returns>response.result will contain a string</returns>
        public Response<string> GetTransformedData(Guid accessKey, FrameworkUAD.Object.ImportVessel iv)
        {
            var param = new UtilityJsonFunctions().ToJson(iv);
            var model = new ServiceRequestModel<string>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetTransformedData,
                WorkerFunc = request => BusinessLogicWorker.GetTransformedData(iv)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Gets the ImportVessel object from the given file info and configuration file objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="fileInfo">the FileInfo object</param>
        /// <param name="fileConfig">the FileConfiguration object</param>
        /// <returns>response.result will contain a ImportVessel object</returns>
        public Response<FrameworkUAD.Object.ImportVessel> GetImportVessel(Guid accessKey, FileInfo fileInfo, FileConfiguration fileConfig = null)
        {
            Response<FrameworkUAD.Object.ImportVessel> response = new Response<FrameworkUAD.Object.ImportVessel>();
            try
            {
                string param = "FileInfo: " + fileInfo.Name;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ImportVessel", "GetImportVessel");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ImportVessel worker = new FrameworkUAD.BusinessLogic.ImportVessel();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetImportVessel(fileInfo, fileConfig);
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
        /// Gets the ImportVessel object from a certain amount of rows of the given file info and configuration file objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="fileInfo">the FileInfo object</param>
        /// <param name="startRow">the row to start at</param>
        /// <param name="takeRowCount">the amount of rows to take</param>
        /// <param name="fileConfig">the FileConfiguration object</param>
        /// <returns>response.result will contain a ImportVessel object</returns>
        public Response<FrameworkUAD.Object.ImportVessel> GetImportVessel(Guid accessKey, FileInfo fileInfo, int startRow, int takeRowCount, FileConfiguration fileConfig = null)
        {
            var param = $"FileInfo: {fileInfo.Name} startRow: {startRow} takeRowCount: {takeRowCount}";
            var model = new ServiceRequestModel<EntityImportVessel>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetImportVessel,
                WorkerFunc = request => new BusinessLogicWorker().GetImportVessel(fileInfo, startRow, takeRowCount, fileConfig)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Gets the ImportVessel object from the excel file
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="fileInfo">the FileInfo object</param>
        /// <returns>response.result will contain a ImportVessel object</returns>
        public Response<FrameworkUAD.Object.ImportVessel> GetImportVesselExcel(Guid accessKey, FileInfo fileInfo)
        {
            var param = $"FileInfo: {fileInfo.Name}";
            var model = new ServiceRequestModel<EntityImportVessel>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetImportVesselExcel,
                WorkerFunc = request => new BusinessLogicWorker().GetImportVesselExcel(fileInfo)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Gets the ImportVessel object from a Dbf file
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="fileInfo">the FileInfo object</param>
        /// <returns>response.result will contain a ImportVessel object</returns>
        public Response<FrameworkUAD.Object.ImportVessel> GetImportVesselDbf(Guid accessKey, FileInfo fileInfo)
        {
            Response<FrameworkUAD.Object.ImportVessel> response = new Response<FrameworkUAD.Object.ImportVessel>();
            try
            {
                string param = "FileInfo: " + fileInfo.Name;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ImportVessel", "GetImportVesselDbf");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ImportVessel worker = new FrameworkUAD.BusinessLogic.ImportVessel();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetImportVesselDbf(fileInfo);
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
        /// Gets the ImportVessel object from a Dbf file for a certain amount of rows
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="fileInfo">the FileInfo object</param>
        /// <param name="startRow">the row to start at</param>
        /// <param name="takeRowCount">the amount of rows to take</param>
        /// <returns>response.result will contain a ImportVessel object</returns>
        public Response<FrameworkUAD.Object.ImportVessel> GetImportVesselDbf(Guid accessKey, FileInfo fileInfo, int startRow, int takeRowCount)
        {
            var param = $"FileInfo: {fileInfo.Name} startRow: {startRow} takeRowCount: {takeRowCount}";
            var model = new ServiceRequestModel<EntityImportVessel>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetImportVesselDbf,
                WorkerFunc = request => new BusinessLogicWorker().GetImportVesselDbf(fileInfo, startRow, takeRowCount)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Gets the ImportVessel object from a text file
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="fileInfo">the FileInfo object</param>
        /// <param name="fileConfig">the FileConfiguration object</param>
        /// <returns>response.result will contain a ImportVessel object</returns>
        public Response<FrameworkUAD.Object.ImportVessel> GetImportVesselText(Guid accessKey, FileInfo fileInfo, FileConfiguration fileConfig)
        {
            var param = $"FileInfo: {fileInfo.Name}";
            var model = new ServiceRequestModel<EntityImportVessel>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetImportVesselText,
                WorkerFunc = request => new BusinessLogicWorker().GetImportVesselText(fileInfo, fileConfig)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Gets the ImportVessel object from a text file for a certain amount of rows
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="fileInfo">the FileInfo object</param>
        /// <param name="startRow">the row to start at</param>
        /// <param name="takeRowCount">the amount of rows to take</param>
        /// <param name="fileConfig">the FileConfiguration object</param>
        /// <returns>response.result will contain a ImportVessel object</returns>
        public Response<FrameworkUAD.Object.ImportVessel> GetImportVesselText(Guid accessKey, FileInfo fileInfo, int startRow, int takeRowCount, FileConfiguration fileConfig)
        {
            var param = $"FileInfo: {fileInfo.Name} startRow: {startRow} takeRowCount: {takeRowCount}";
            var model = new ServiceRequestModel<EntityImportVessel>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetImportVesselText,
                WorkerFunc = request => new BusinessLogicWorker().GetImportVesselText(fileInfo, startRow, takeRowCount, fileConfig)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Loads the file as an ImportVessel object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="file">the FileInfo object</param>
        /// <param name="fileConfig">the FileConfiguration object</param>
        /// <returns>response.result will contain a ImportVessel object</returns>
        public Response<FrameworkUAD.Object.ImportVessel> LoadFileImportVessel(Guid accessKey, FileInfo file, FileConfiguration fileConfig)
        {
            Response<FrameworkUAD.Object.ImportVessel> response = new Response<FrameworkUAD.Object.ImportVessel>();
            try
            {
                string param = "FileInfo: " + file.Name;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ImportVessel", "LoadFileImportVessel");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ImportVessel worker = new FrameworkUAD.BusinessLogic.ImportVessel();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.LoadFileImportVessel(file, fileConfig);
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
        /// Loads a certain amount of the files rows as an ImportVessel object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="file">the FileInfo object</param>
        /// <param name="startRow">the row to start at</param>
        /// <param name="takeRowCount">the amount of rows to take</param>
        /// <param name="fileConfig">the FileConfiguration object</param>
        /// <returns>response.result will contain a ImportVessel object</returns>
        public Response<FrameworkUAD.Object.ImportVessel> LoadFileImportVessel(Guid accessKey, FileInfo file, int startRow, int takeRowCount, FileConfiguration fileConfig)
        {
            var param = $"FileInfo: {file.Name} startRow: {startRow} takeRowCount: {takeRowCount}";
            var model = new ServiceRequestModel<EntityImportVessel>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodLoadFileImportVessel,
                WorkerFunc = request => new BusinessLogicWorker().LoadFileImportVessel(file, startRow, takeRowCount, fileConfig)
            };

            return GetResponse(model);
        }
    }
}

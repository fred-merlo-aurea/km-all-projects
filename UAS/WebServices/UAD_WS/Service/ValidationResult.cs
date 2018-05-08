using System;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class ValidationResult : ServiceBase, IValidationResult
    {
        /// <summary>
        /// Gets a customer error message from the ValidationResul object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="vr">the ValidationResult object</param>
        /// <param name="isTextQualifier">boolean for text qualifier</param>
        /// <returns>response.result will contain a string</returns>
        public Response<string> GetCustomerErrorMessage(Guid accessKey, FrameworkUAD.Object.ValidationResult vr, bool isTextQualifier)
        {
            Response<string> response = new Response<string>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "ValidationResult", "GetCustomerErrorMessage");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ValidationResult worker = new FrameworkUAD.BusinessLogic.ValidationResult();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetCustomerErrorMessage(vr, isTextQualifier);
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
        /// Gets bad data
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="vr">the ValidationResult object</param>
        /// <param name="isTextQualifier">boolean for text qualifier</param>
        /// <returns>response.result will contain a string</returns>
        public Response<string> GetBadData(Guid accessKey, FrameworkUAD.Object.ValidationResult vr, bool isTextQualifier)
        {
            Response<string> response = new Response<string>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "ValidationResult", "GetBadData");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ValidationResult worker = new FrameworkUAD.BusinessLogic.ValidationResult();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetBadData(vr, isTextQualifier);
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
        /// Gets bad data
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="vr">the ValidationResult object</param>
        /// <param name="fileName">the file name</param>
        /// <param name="isTextQualifier">boolean for text qualifier</param>
        /// <returns>response.result will contain a string</returns>
        public Response<string> GetBadDataFileValidator(Guid accessKey, FrameworkUAD.Object.ValidationResult vr, string fileName, bool isTextQualifier)
        {
            Response<string> response = new Response<string>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "ValidationResult", "GetBadDataFileValidator");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ValidationResult worker = new FrameworkUAD.BusinessLogic.ValidationResult();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetBadDataFileValidator(vr, fileName, isTextQualifier);
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

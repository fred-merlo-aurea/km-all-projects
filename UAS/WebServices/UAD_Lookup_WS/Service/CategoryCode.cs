using UAD_Lookup_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_Lookup_WS.Service
{
    public class CategoryCode : ServiceBase, ICategoryCode
    {
        /// <summary>
        /// Checks to see if a CategoryCode object exists based on the category code type ID and the category code value
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="categoryCodeTypeID">the category code type ID</param>
        /// <param name="categoryCodeValue">the category code value</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Exists(Guid accessKey, int categoryCodeTypeID, int categoryCodeValue)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                string param = "CategoryCodeTypeID:" + categoryCodeTypeID.ToString() + " CategoryCodeValue:" + categoryCodeValue.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "CategoryCode", "Exists");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.CategoryCode worker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Exists(categoryCodeTypeID, categoryCodeValue);
                    if (response.Result == true || response.Result == false)
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
        /// Selects an active CategoryCode object based on if it's code type is free or not
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="isFree">boolean to look for the free or not free code types</param>
        /// <returns>response.result will contain a list of CategoryCode objects</returns>
        public Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> SelectActiveIsFree(Guid accessKey, bool isFree)
        {
            Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> response = new Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>>();
            try
            {
                string param = "IsFree:" + isFree.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "CategoryCode", "SelectActiveIsFree");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.CategoryCode worker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectActiveIsFree(isFree);
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
        /// Selects a CategoryCode object based on the category code type ID and the category code value
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="categoryCodeTypeID">the category code type ID</param>
        /// <param name="categoryCodeValue">the category code value</param>
        /// <returns>response.resutl will contain a CategoryCode object</returns>
        public Response<FrameworkUAD_Lookup.Entity.CategoryCode> Select(Guid accessKey, int categoryCodeTypeID, int categoryCodeValue)
        {
            Response<FrameworkUAD_Lookup.Entity.CategoryCode> response = new Response<FrameworkUAD_Lookup.Entity.CategoryCode>();
            try
            {
                string param = "categoryCodeTypeID:" + categoryCodeTypeID.ToString() + " CategoryCodeValue:" + categoryCodeValue.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "CategoryCode", "SelectActiveIsFree");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.CategoryCode worker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(categoryCodeTypeID, categoryCodeValue);
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
        /// Selects a list of CategoryCode objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of CategoryCode objects</returns>
        public Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> response = new Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "CategoryCode", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.CategoryCode worker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode();
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
        /// Saves the given CategoryCode object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the CategoryCode object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.CategoryCode x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD_Lookup.Entity.CategoryCode>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "CategoryCode", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.CategoryCode worker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
                    if (response.Result > 0)
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

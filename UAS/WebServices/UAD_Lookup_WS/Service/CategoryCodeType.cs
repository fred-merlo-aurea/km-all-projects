using UAD_Lookup_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_Lookup_WS.Service
{
    public class CategoryCodeType : ServiceBase, ICategoryCodeType
    {
        /// <summary>
        /// Checks to see if a CategoryCodeType objects based on the given name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="categoryCodeTypeName">the category code type name</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Exists(Guid accessKey, string categoryCodeTypeName)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                string param = "CategoryCodeTypeName:" + categoryCodeTypeName.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "CategoryCodeType", "Exists");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType worker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Exists(categoryCodeTypeName);
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
        /// Selects a CategoryCodeType object based on the given CategoryCodeType enum
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="categoryCodeTypeName">the CategoryCodeType enum (Qualified_Free, NonQualified_Free, Qualified_Paid, NonQualified_Paid)</param>
        /// <returns>response.result will contain a CategoryCodeType object</returns>
        public Response<FrameworkUAD_Lookup.Entity.CategoryCodeType> Select(Guid accessKey, FrameworkUAD_Lookup.Enums.CategoryCodeType categoryCodeTypeName)
        {
            Response<FrameworkUAD_Lookup.Entity.CategoryCodeType> response = new Response<FrameworkUAD_Lookup.Entity.CategoryCodeType>();
            try
            {
                string param = "CategoryCodeTypeName:" + categoryCodeTypeName.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "CategoryCodeType", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType worker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(categoryCodeTypeName);
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
        /// Selects a CategoryCodeType object based on the category code type name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="categoryCodeTypeName">the category code type name</param>
        /// <returns>response.result will contain a CategoryCodeType object</returns>
        public Response<FrameworkUAD_Lookup.Entity.CategoryCodeType> Select(Guid accessKey, string categoryCodeTypeName)
        {
            Response<FrameworkUAD_Lookup.Entity.CategoryCodeType> response = new Response<FrameworkUAD_Lookup.Entity.CategoryCodeType>();
            try
            {
                string param = "CategoryCodeTypeName:" + categoryCodeTypeName.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "CategoryCodeType", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType worker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(categoryCodeTypeName);
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
        /// Selects a list of CategoryCodeType objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of CategoryCodeType objects</returns>
        public Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>> response = new Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "CategoryCodeType", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType worker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType();
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
        /// Selects a list of CategoryCodeType objects that are free or not
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="isFree">boolean to look for the free or not free code types</param>
        /// <returns>resposne.result will contain a list of CategoryCodeType objects</returns>
        public Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>> Select(Guid accessKey, bool isFree)
        {
            Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>> response = new Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>>();
            try
            {
                string param = "IsFree:" + isFree.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "CategoryCodeType", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType worker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(isFree);
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
        /// Saves the given CategoryCodeType object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the CategoryCodeType object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.CategoryCodeType x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD_Lookup.Entity.CategoryCodeType>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "CategoryCodeType", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType worker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType();
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

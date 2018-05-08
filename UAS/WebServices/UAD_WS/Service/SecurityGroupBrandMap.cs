using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class SecurityGroupBrandMap : ServiceBase, ISecurityGroupBrandMap
    {
        /// <summary>
        /// Selects a list of SecurityGroupBrandMap objects based on the brand ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="brandID">the brand ID</param>
        /// <returns>response.result will contain a list of SecurityGroupBrandMap objects</returns>
        public Response<List<FrameworkUAD.Entity.SecurityGroupBrandMap>> SelectForBrand(Guid accessKey, KMPlatform.Object.ClientConnections client, int brandID)
        {
            Response<List<FrameworkUAD.Entity.SecurityGroupBrandMap>> response = new Response<List<FrameworkUAD.Entity.SecurityGroupBrandMap>>();
            try
            {
                string param = " BrandID:" + brandID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SecurityGroupBrandMap", "SelectForBrand");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SecurityGroupBrandMap worker = new FrameworkUAD.BusinessLogic.SecurityGroupBrandMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForBrand(client, brandID).ToList();
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
        /// Selects a list of SecurityGroupBrandMap objects based on the client and security group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="securityGroupID">the security group ID</param>
        /// <returns>response.result will contain a list of SecurityGroupBrandMap objects</returns>
        public Response<List<FrameworkUAD.Entity.SecurityGroupBrandMap>> SelectForSecurityGroup(Guid accessKey, KMPlatform.Object.ClientConnections client, int securityGroupID)
        {
            Response<List<FrameworkUAD.Entity.SecurityGroupBrandMap>> response = new Response<List<FrameworkUAD.Entity.SecurityGroupBrandMap>>();
            try
            {
                string param = " SecurityGroupID:" + securityGroupID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SecurityGroupBrandMap", "SelectForSecurityGroup");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SecurityGroupBrandMap worker = new FrameworkUAD.BusinessLogic.SecurityGroupBrandMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForSecurityGroup(client, securityGroupID).ToList();
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
        /// Saves a SecurityGroupBrandMap object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="x">the SecurityGroupBrandMap object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, KMPlatform.Object.ClientConnections client, FrameworkUAD.Entity.SecurityGroupBrandMap x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.SecurityGroupBrandMap>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SecurityGroupBrandMap", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SecurityGroupBrandMap worker = new FrameworkUAD.BusinessLogic.SecurityGroupBrandMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(client, x);
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

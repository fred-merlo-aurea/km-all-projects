using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class SecurityGroupProductMap : ServiceBase, ISecurityGroupProductMap
    {
        /// <summary>
        /// Selects a list of SecurityGroupProductMap objects based on the client and the product ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="productID">the product ID</param>
        /// <returns>response.result will contain a list of SecurityGroupProductMap objects</returns>
        public Response<List<FrameworkUAD.Entity.SecurityGroupProductMap>> SelectForProduct(Guid accessKey, KMPlatform.Object.ClientConnections client, int productID)
        {
            Response<List<FrameworkUAD.Entity.SecurityGroupProductMap>> response = new Response<List<FrameworkUAD.Entity.SecurityGroupProductMap>>();
            try
            {
                string param = " productID:" + productID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SecurityGroupProductMap", "SelectForProduct");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SecurityGroupProductMap worker = new FrameworkUAD.BusinessLogic.SecurityGroupProductMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForProduct(client, productID).ToList();
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
        /// Selects a list of SecurityGroupProductMap objects based on the client and security group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="securityGroupID">the security group ID</param>
        /// <returns>response.result will contain a list of SecurityGroupProductMap objects</returns>
        public Response<List<FrameworkUAD.Entity.SecurityGroupProductMap>> SelectForSecurityGroup(Guid accessKey, KMPlatform.Object.ClientConnections client, int securityGroupID)
        {
            Response<List<FrameworkUAD.Entity.SecurityGroupProductMap>> response = new Response<List<FrameworkUAD.Entity.SecurityGroupProductMap>>();
            try
            {
                string param = " securityGroupID:" + securityGroupID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SecurityGroupProductMap", "SelectForSecurityGroup");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SecurityGroupProductMap worker = new FrameworkUAD.BusinessLogic.SecurityGroupProductMap();
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
        /// Saves the SecurityGroupProductMap object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="x">the SecurityGroupProductMap object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, KMPlatform.Object.ClientConnections client, FrameworkUAD.Entity.SecurityGroupProductMap x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.SecurityGroupProductMap>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SecurityGroupProductMap", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SecurityGroupProductMap worker = new FrameworkUAD.BusinessLogic.SecurityGroupProductMap();
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

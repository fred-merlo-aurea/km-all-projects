using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;
using System.Data;

namespace UAD_WS.Service
{
    public class Subscription : ServiceBase, ISubscription
    {
        /// <summary>
        /// Selects a list of Subscription objects based on the client and whether to include the available custom properties
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="includeCustomProperties">boolean to include the available custom properties</param>
        /// <returns>response.result will contain a list of Subscription objects</returns>
        public Response<List<FrameworkUAD.Entity.Subscription>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client, bool includeCustomProperties = false)
        {
            Response<List<FrameworkUAD.Entity.Subscription>> response = new Response<List<FrameworkUAD.Entity.Subscription>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "Subscription", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Subscription worker = new FrameworkUAD.BusinessLogic.Subscription();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(client, auth.Client.DisplayName, includeCustomProperties).ToList();
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
        /// Selects a list of Subscription objects based on the email, the client and whether to include the available custom properties
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="email">the email address</param>
        /// <param name="client">the client object</param>
        /// <param name="includeCustomProperties">boolean to include the available custom properties</param>
        /// <returns>response.result will contain a list of Subscription objects</returns>
        public Response<List<FrameworkUAD.Entity.Subscription>> Select(Guid accessKey, string email, KMPlatform.Object.ClientConnections client, bool includeCustomProperties = false)
        {
            Response<List<FrameworkUAD.Entity.Subscription>> response = new Response<List<FrameworkUAD.Entity.Subscription>>();
            try
            {
                string param = " email:" + email;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Subscription", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Subscription worker = new FrameworkUAD.BusinessLogic.Subscription();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(email, client, auth.Client.DisplayName, includeCustomProperties);
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
        /// Selects a Subscription object based on the subscription ID, the client and whether to include the available custom properties
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionID">the subscription ID</param>
        /// <param name="client">the client object</param>
        /// <param name="includeCustomProperties">boolean to include the available custom properties</param>
        /// <returns>response.result will contain a Subscription object</returns>
        public Response<FrameworkUAD.Entity.Subscription> Select(Guid accessKey, int subscriptionID, KMPlatform.Object.ClientConnections client, bool includeCustomProperties = false)
        {
            Response<FrameworkUAD.Entity.Subscription> response = new Response<FrameworkUAD.Entity.Subscription>();
            try
            {
                string param = " subscriptionID:" + subscriptionID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Subscription", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Subscription worker = new FrameworkUAD.BusinessLogic.Subscription();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(subscriptionID, client, auth.Client.DisplayName, includeCustomProperties);
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
        /// Selects a list of IDs from the subscription objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer list</returns>
        public Response<List<int>> SelectIDs(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            Response<List<int>> response = new Response<List<int>>();
            try
            {
                string param = string.Empty;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Subscription", "SelectIDs");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Subscription worker = new FrameworkUAD.BusinessLogic.Subscription();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectIDs(client);
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
                response.Message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            }
            return response;
        }

        /// <summary>
        /// Selects a list of Subscription objects based on the process code, the source file ID, start date, end date and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="startDate">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of Subscription objects</returns>
        public Response<List<FrameworkUAD.Entity.Subscription>> SelectForFileAudit(Guid accessKey, string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
	        Response<List<FrameworkUAD.Entity.Subscription>> response = new Response<List<FrameworkUAD.Entity.Subscription>>();
	        try
	        {
                string sd = "NULL";
                string ed = "NULL";
                if (startDate != null && startDate.Value != null)
                    sd = startDate.Value.ToString();

                if (endDate != null && endDate.Value != null)
                    ed = endDate.Value.ToString();

                string param = " processCode:" + processCode + " sourceFileID:" + sourceFileID.ToString() + " startDate:" + sd + " endDate:" + ed;		        
		        FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Subscription", "SelectForFileAudit");
		        response.Status = auth.Status;

		        if (auth.IsAuthenticated == true)
		        {
			        FrameworkUAD.BusinessLogic.Subscription worker = new FrameworkUAD.BusinessLogic.Subscription();
			        response.Message = "AccessKey Validated";
			        response.Result = worker.SelectForFileAudit(processCode, sourceFileID, startDate, endDate, client).ToList();
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
        /// Saves the Subscription object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the Subscription object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD.Entity.Subscription x, KMPlatform.Object.ClientConnections client)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.Subscription>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Subscription", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Subscription worker = new FrameworkUAD.BusinessLogic.Subscription();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x, client);
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
        /// Gets a list of Subscribers based on the email address
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="emailAddress">the email address</param>
        /// <returns>response.result will contain a list of Subscription objects</returns>
        public Response<List<FrameworkUAD.Entity.Subscription>> GetSubscriber(Guid accessKey, string emailAddress)
        {
            Response<List<FrameworkUAD.Entity.Subscription>> response = new Response<List<FrameworkUAD.Entity.Subscription>>();
            try
            {
                string param = "EmailAddress:" + emailAddress;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Subscription", "GetSubscriber");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Subscription worker = new FrameworkUAD.BusinessLogic.Subscription();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(emailAddress, auth.Client.ClientConnections, auth.Client.DisplayName, true).ToList();
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
        /// Searches the database for Subscription objects based on the address and zip code for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="address1">the address</param>
        /// <param name="zipCode">the zip code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of Subscription objects</returns>
        public Response<List<FrameworkUAD.Entity.Subscription>> SearchAddressZip(Guid accessKey, string address1, string zipCode, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.Subscription>> response = new Response<List<FrameworkUAD.Entity.Subscription>>();
            try
            {
                string param = "address1: " + address1 + " zipCode:" + zipCode;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Subscriber", "SearchAddressZip");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Subscription worker = new FrameworkUAD.BusinessLogic.Subscription();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SearchAddressZip(address1, zipCode, client);
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
                LogError(accessKey, ex, this.GetType().Name.ToString()); response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
        public Response<DataTable> FindMatches(Guid accessKey, int productID, string fname, string lname, string company, string address, string state, string zip, string phone, string email, string title, KMPlatform.Object.ClientConnections client)
        {
            Response<DataTable> response = new Response<DataTable>();
            try
            {
                string param = "productId: " + productID.ToString() + " fname: " + fname + " lname: " + lname + " company: " + company + " address: " + address + " state: " + state + " zip: " + zip + " phone: " + phone + " email: " + email + " title: " + title;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Subscriber", "SearchAddressZip");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Subscription worker = new FrameworkUAD.BusinessLogic.Subscription();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.FindMatches(productID, fname, lname, company, address, state, zip, phone, email, title, client);
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
                LogError(accessKey, ex, this.GetType().Name.ToString()); response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}

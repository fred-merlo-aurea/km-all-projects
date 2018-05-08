using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace KM.Integration.OAuth2
{
    public class Authentication
    {
        readonly string baseUrl = string.Empty;
        readonly string clientId;
        readonly string clientSecret;

        public Authentication(string baseUrl, string clientId, string clientSecret)
        {
	        this.baseUrl = baseUrl;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        public Token getToken()
        {

			Token token = null;
			try
			{
				using (var client = new HttpClient())
				{
					var apiAddress = buildAutthenticationAddress();

					var task = client.GetAsync(apiAddress).ContinueWith((taskwithResponse) =>
					{
						try
						{
							var response = taskwithResponse.Result;
							var jsonString = response.Content.ReadAsStringAsync();
							jsonString.Wait();
							token = JsonConvert.DeserializeObject<Token>(jsonString.Result);
						}
						catch (Exception ex)
						{
							throw ex;
						}

					});
					task.Wait();
				}
				return token;

			}
			catch (Exception ex)
			{
				throw ex;
			}
        }

	    private string buildAutthenticationAddress()
	    {
		    var apiAddress = baseUrl;
		    var authenticationAddressTag = "/identity/oauth/token";
		    var content = "?grant_type=client_credentials&client_id=" + clientId + "&client_secret=" + clientSecret;
		    return apiAddress + authenticationAddressTag + content;
	    }
    }   
}

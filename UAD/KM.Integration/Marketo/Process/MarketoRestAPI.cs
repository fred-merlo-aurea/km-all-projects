using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using KM.Integration.OAuth2;
using MoreLinq;
using Newtonsoft.Json;

namespace KM.Integration.Marketo.Process
{
    public class MarketoRestAPIProcess
    {
        const int batchsize = 200;
	    readonly string baseUrl = string.Empty;
        readonly string clientId;
        readonly string clientSecret;

        private Token token = null;

        public MarketoRestAPIProcess(string baseUrl, string clientId, string clientSecret)
        {
            this.baseUrl = baseUrl;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        public List<Result> CreateUpdateLeads(List<Dictionary<string, string>> leads, string lookupField, string Partition, int? groupId)
        {
			int ThresholdCounter = 0;  //MARKETO ERROR : 606 / Max rate limit '100' exceeded with in '20' secs

			List<Result> lresults = new List<Result>();
            List<Result> leadresults = new List<Result>();
            List<Result> listresults = new List<Result>();

			Authentication auth = new Authentication(baseUrl, clientId, clientSecret);

			foreach (var leadslist in leads.Batch(batchsize))
			{
				ThresholdCounter++;

				if (ThresholdCounter == 99)
				{
					System.Threading.Thread.Sleep(2000);
					ThresholdCounter = 0;
				}

				using (var client = new HttpClient())
				{
					if (token == null || token.isExpired)
						token = auth.getToken();

					var getMultipleListsUrlAddress = baseUrl + "/rest/v1/leads.json?access_token=" + token.access_token;

					var getMultipleListsContent = bodyBuilder(leadslist, lookupField, Partition);

					var multipleListsRequest = new HttpRequestMessage
					{
						RequestUri = new Uri(getMultipleListsUrlAddress),
						Method = HttpMethod.Post,
						Content = new StringContent(getMultipleListsContent, Encoding.UTF8, "application/json")
					};
					multipleListsRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					var getMultipleListsTask = client.SendAsync(multipleListsRequest).ContinueWith((taskwithResponse) =>
					{
                        try
                        {
                            string message = string.Empty;
                            var response = taskwithResponse.Result;
                            var jsonString = response.Content.ReadAsStringAsync();
                            jsonString.Wait();

                            var addLeadsToListResponse = JsonConvert.DeserializeObject<AddLeadsToListResponse>(jsonString.Result);

                            if (addLeadsToListResponse.Success == false)
                            {
                                throw new Exception(addLeadsToListResponse.Errors[0].message);
                            }
                            else
                            {
                                lresults = addLeadsToListResponse.Result.ToList();
                                lresults.ForEach(x => x.type = Enums.Results.Lead.ToString());

                                if (groupId != null && groupId > 0)
                                {
                                    listresults.AddRange(lresults);
                                    listresults.AddRange(AddLeadsToList(lresults, groupId, auth));
                                }
                                else
                                    leadresults.AddRange(lresults);
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
					});

                    try
                    {
                        getMultipleListsTask.Wait();
                    }
                    catch(AggregateException ae)
                    {
                        throw ae.InnerException;
                    }
				}
			}
            if (groupId != null && groupId > 0)
                return listresults;
            else
                return leadresults;
        }

        public List<Result> AddLeadsToList(List<Result> leadresults, int? groupId, Authentication auth)
	    {
           List<Result> listResults = new List<Result>();

		   // TODO 300 results count

            var ids = leadresults.Select(x => Convert.ToInt32((string)x.id));

			if (token == null || token.isExpired)
				token = auth.getToken();

		    using (var client = new HttpClient())
		    {

			    var addLeadsToListurlAddress = addLeadsToListAddress(baseUrl, groupId, token.access_token);

			    var addLeadsToListContent = bodyBuilder(ids);

			    var request = new HttpRequestMessage
			    {
				    RequestUri = new Uri(addLeadsToListurlAddress),
				    Method = HttpMethod.Post,
				    Content = new StringContent(addLeadsToListContent, Encoding.UTF8, "application/json")
			    };
			    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			    var task = client.SendAsync(request).ContinueWith((taskwithResponse) =>
			    {
				    try
				    {
					    var response = taskwithResponse.Result;
					    var jsonString = response.Content.ReadAsStringAsync();
					    jsonString.Wait();
					    var addLeadsToListResponse = JsonConvert.DeserializeObject<AddLeadsToListResponse>(jsonString.Result);
                        listResults.AddRange(addLeadsToListResponse.Result);
                        listResults.ForEach(x => x.type = Enums.Results.List.ToString());
				    }
				    catch (Exception ex)
				    {
					    throw ex;
				    }
			    });

			    task.Wait();
		    }
            return listResults;
	    }

	    private String bodyBuilder(IEnumerable<int> ids)
		{
			Dictionary<String, Object> parent = new Dictionary<string, object>();
			List<Dictionary<String, int>> input = new List<Dictionary<string, int>>();
			foreach (int lead in ids)
			{
				Dictionary<string, int> leadObject = new Dictionary<string, int>();
				leadObject.Add("id", lead);
				input.Add(leadObject);
			}
			parent.Add("input", input);
			return JsonConvert.SerializeObject(parent);
		}

	    private string addLeadsToListAddress(string url, int? id, string accessToken)
	    {
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder = stringBuilder.Append(url);
			stringBuilder = stringBuilder.AppendFormat("/rest/v1/lists/{0}/leads.json?access_token={1}", id, accessToken);
		    return stringBuilder.ToString();
	    }


	    private String bodyBuilder(IEnumerable<Dictionary<string, string>> input, string lookupField, string partitionName)
        {
            //Create a new Dict as the parent
            Dictionary<String, Object> body = new Dictionary<string, object>();

            foreach (var item in input)
            {
                item.Where(x => x.Value.Equals("")).ToArray().ForEach(x => item.Remove(x.Key));
            }

            //Append optional fields
            if (lookupField != null)
            {
                body.Add("lookupField", lookupField);
            }

            if (partitionName != null)
            {
                body.Add("partionName", partitionName);
            }

            //Add the list of leads into the input member
            body.Add("input", input);
            //serialize the body object into JSON

            String json = JsonConvert.SerializeObject(body); 
            return json;
        }
		


	    public List<MarketoListResponse> GetMarketoLists(Token token, string[] ids, string[] names, int batchSize = 0,string nextPageToken = "")
	    {
		    List<MarketoListResponse> marketoiListResponses = new List<MarketoListResponse>();

		    using (var client = new HttpClient())
		    {
			    var urlAddress = getMarketoListUrlAddress(ids, names, batchSize, nextPageToken,baseUrl, token.access_token);

			    var request = new HttpRequestMessage
			    {
				    RequestUri = new Uri(urlAddress),
				    Method = HttpMethod.Get
			    };
				
			    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);

			    var task = client.SendAsync(request).ContinueWith((taskwithResponse) =>
			    {
				    try
				    {
					    var response = taskwithResponse.Result;
					    var jsonString = response.Content.ReadAsStringAsync();
					    jsonString.Wait();
					    var marketoListResponse = JsonConvert.DeserializeObject<MarketoListResponse>(jsonString.Result);
						marketoiListResponses.Add(marketoListResponse);
					}
				    catch (Exception ex)
				    {
					    throw ex;
				    }
			    });

			    task.Wait();
		    }
		    return marketoiListResponses;
	    }


	    private string getMarketoListUrlAddress(string[] ids, string[] names, int batchSize, string nextPageToken, string url, string accessToken)
	    {
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder = stringBuilder.Append(url);
			stringBuilder = stringBuilder.AppendFormat("/rest/v1/lists.json?access_token={0}",accessToken);
		    if (ids != null)
		    {
				stringBuilder = stringBuilder.AppendFormat("&id={0}", ids);    
		    }
		    if (names != null)
		    {
				stringBuilder = stringBuilder.AppendFormat("&name={0}", names);
		    }
		    if (batchSize > 0 && batchSize < 300)
		    {
				stringBuilder = stringBuilder.AppendFormat("&batchSize={0}", batchSize);
		    }
		    if (!String.IsNullOrWhiteSpace(nextPageToken))
		    {
				stringBuilder = stringBuilder.AppendFormat("&nextpagetoken={0}", nextPageToken);    
		    }
			return stringBuilder.ToString();
	    }
    }
}

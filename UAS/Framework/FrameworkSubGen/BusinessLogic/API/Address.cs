using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace FrameworkSubGen.BusinessLogic.API
{
    public class Address
    {
        private const string AddressCityStateAndSubscriberIdAreRequired = "address, city, state and subscriber_id are required";
        private const string AddressUriExtension = "addresses/";
        private const string UpdateError = "error";
        private const string ClassName = "FrameworkSubGen.BusinessLogic.API.Address";
        private const string UpdateMethod = "Update";
        private const string CreateMethod = "Create";
        private const string AddressParameterKey = "address";
        private const string AddressLine2ParameterKey = "address_line_2";
        private const string LastNameParameterKey = "last_name";
        private const string FirstNameParameterKey = "first_name";
        private const string CountryParameterKey = "country";
        private const string CompanyParameterKey = "company";
        private const string ZipCodeParameterKey = "zip_code";
        private const string SubscriberIdParameterKey = "subscriber_id";
        private const string StateParameterKey = "state";
        private const string CityParameterKey = "city";

        public List<Entity.Address> GetAddresses(Entity.Enums.Client client, string city = "", string company = "", string country = "", string first_name = "", string last_name = "",
            string state = "", int subscriber_id = 0, string zip_code = "")
        {
            Response.Address resp = new Response.Address();
            try
            {
                //GET https://api.knowledgemarketing.com/2/addresses/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                if (!string.IsNullOrEmpty(city))
                    webClient.QueryString.Add("city", city.ToString());
                if (!string.IsNullOrEmpty(company))
                    webClient.QueryString.Add("company", company.ToString());
                if (!string.IsNullOrEmpty(country))
                    webClient.QueryString.Add("country", country.ToString());
                if (!string.IsNullOrEmpty(first_name))
                    webClient.QueryString.Add("first_name", first_name.ToString());
                if (!string.IsNullOrEmpty(last_name))
                    webClient.QueryString.Add("last_name", last_name.ToString());
                if (!string.IsNullOrEmpty(state))
                    webClient.QueryString.Add("state", state.ToString());
                if (subscriber_id > 0)
                    webClient.QueryString.Add("subscriber_id", subscriber_id.ToString());
                if (!string.IsNullOrEmpty(zip_code))
                    webClient.QueryString.Add("zip_code", zip_code.ToString());
                string json = webClient.DownloadString(auth.BaseUri + AddressUriExtension);

                if (!string.IsNullOrEmpty(json) && json != "null")
                {
                    resp = jf.FromJson<Response.Address>(json);
                }
                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, "FrameworkSubGen.BusinessLogic.API.Address", "GetAddresses");
            }
            return resp.addresses;
        }
        public Entity.Address GetAddress(Entity.Enums.Client client, int address_id)
        {
            Entity.Address item = new Entity.Address();
            try
            {
                //GET https://api.knowledgemarketing.com/2/addresses/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                //webClient.QueryString.Add("address_id", address_id.ToString());
                string json = webClient.DownloadString(auth.BaseUri + AddressUriExtension + address_id.ToString());
                if (!string.IsNullOrEmpty(json) && json != "null")
                {
                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    item = jf.FromJson<Entity.Address>(json);
                }
                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, "FrameworkSubGen.BusinessLogic.API.Address", "GetAddress");
            }
            return item;
        }

        public int Create(Entity.Enums.Client client, Entity.Address address)
        {
            if (address == null || !AddressEntityIsValid(address))
            {
                return 0;
            }

            var item = 0;
            try
            {
                if (string.IsNullOrWhiteSpace(address.country))
                {
                    address.country = GetCountryFromAddressEntity(address);
                }

                // POST https://api.knowledgemarketing.com/2/addresses/
                var authentication = new Authentication();
                using (var webClient = authentication.GetClient(client))
                {
                    var requestParameters = new NameValueCollection();

                    AddToRequestParameter(requestParameters, AddressParameterKey, address.address, false);
                    AddToRequestParameter(requestParameters, AddressLine2ParameterKey, address.address_line_2, false);
                    AddToRequestParameter(requestParameters, CityParameterKey, address.city);
                    AddToRequestParameter(requestParameters, StateParameterKey, address.state);
                    AddToRequestParameter(requestParameters, SubscriberIdParameterKey, address.subscriber_id.ToString());
                    AddToRequestParameter(requestParameters, ZipCodeParameterKey, address.zip_code);
                    AddToRequestParameter(requestParameters, CompanyParameterKey, address.company, false);
                    AddToRequestParameter(requestParameters, CountryParameterKey, address.country, false);
                    AddToRequestParameter(requestParameters, FirstNameParameterKey, address.first_name, false);
                    AddToRequestParameter(requestParameters, LastNameParameterKey, address.last_name, false);

                    var responseBytes = webClient.UploadValues(
                        authentication.BaseUri + AddressUriExtension,
                        Entity.Enums.HttpMethod.POST.ToString(),
                        requestParameters);

                    if (responseBytes != null)
                    {
                        var json = Encoding.UTF8.GetString(responseBytes).Split(':');
                        if (json.Count() > 1)
                        {
                            var jsonFunctions = new Core_AMS.Utilities.JsonFunctions();
                            item = jsonFunctions.FromJson<int>(json[1].TrimEnd('}'));
                        }
                    }

                    webClient.Dispose();
                }
            }
            catch (Exception exception) when (exception is WebException
                                              || exception is ArgumentNullException
                                              || exception is DecoderFallbackException
                                              || exception is ArgumentException
                                              || exception is OverflowException)
            {
                Authentication.SaveApiLog(exception, ClassName, CreateMethod);
            }

            return item;
        }

        public string Update(Entity.Enums.Client client, Entity.Address address)
        {
            if (address == null 
                || address.address_id <= 0 
                || !AddressEntityIsValid(address))
            {
                return AddressCityStateAndSubscriberIdAreRequired;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(address.country))
                {
                    address.country = GetCountryFromAddressEntity(address);
                }

                // POST https://api.knowledgemarketing.com/2/addresses/{address_id}
                var authentication = new Authentication();
                byte[] responseBytes;
                using (var webClient = authentication.GetClient(client))
                {
                    var reqparm = new NameValueCollection();
                    AddToRequestParameter(reqparm, AddressParameterKey, address.address);
                    AddToRequestParameter(reqparm, AddressLine2ParameterKey, address.address_line_2);
                    AddToRequestParameter(reqparm, CityParameterKey, address.city);
                    AddToRequestParameter(reqparm, StateParameterKey, address.state);
                    AddToRequestParameter(reqparm, SubscriberIdParameterKey, address.subscriber_id.ToString());
                    AddToRequestParameter(reqparm, ZipCodeParameterKey, address.zip_code);
                    AddToRequestParameter(reqparm, CompanyParameterKey, address.company, false);
                    AddToRequestParameter(reqparm, CountryParameterKey, address.country, false);
                    AddToRequestParameter(reqparm, FirstNameParameterKey, address.first_name, false);
                    AddToRequestParameter(reqparm, LastNameParameterKey, address.last_name, false);

                    responseBytes = webClient.UploadValues(
                        $"{authentication.BaseUri}{AddressUriExtension}{address.address_id}",
                        Entity.Enums.HttpMethod.POST.ToString(),
                        reqparm);
                }

                return Encoding.UTF8.GetString(responseBytes);
            }
            catch (Exception exception) when (exception is WebException 
                                       || exception is ArgumentNullException
                                       || exception is DecoderFallbackException
                                       || exception is ArgumentException)
            {
                Authentication.SaveApiLog(exception, ClassName, UpdateMethod);
                return UpdateError;
            }
        }

        private static string GetCountryFromAddressEntity(Entity.Address address)
        {
            if (!string.IsNullOrWhiteSpace(address.country_name))
            {
                return address.country_name;
            }

            if (!string.IsNullOrWhiteSpace(address.country_abbreviation))
            {
                return address.country_abbreviation;
            }

            return address.country;
        }

        private static void AddToRequestParameter(NameValueCollection requestParameters, string name, string value, bool allowEmpty = true)
        {
            if (allowEmpty || !string.IsNullOrWhiteSpace(value))
            {
                requestParameters.Add(name, value);
            }
        }

        private static bool AddressEntityIsValid(Entity.Address address)
        {
            return !string.IsNullOrWhiteSpace(address.address) 
                   && !string.IsNullOrWhiteSpace(address.city)
                   && !string.IsNullOrWhiteSpace(address.state)
                   && !string.IsNullOrWhiteSpace(address.zip_code)
                   && address.subscriber_id > 0;
        }
    }
}

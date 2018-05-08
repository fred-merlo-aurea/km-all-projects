using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Data;

namespace DQM.Helpers.Validation
{
    class AddressCleaner
    {
        static string YAHOO = "http://api.local.yahoo.com/MapsService/V1/geocode";
        static string BING = "http://dev.virtualearth.net/REST/v1/Locations/US/adminDistrict/postalCode/locality/addressLine?output=xml";
        static string BING_Key = "key=Al8sOe7CZ0GwR1p3arrayhzyfds0kyGs_oQVPoQeeTSTnHai4tZWgMdSS31TX4tS";
        static string GOOGLE = "https://maps.googleapis.com/maps/api/geocode/xml?";
        //static string GOOGLE_Key = "ABQIAAAAZOehRZQZ8Fc0ih-0QPaiVhSc1oGDelTNiEdrJ-ykOsaqvvkA6BSApQFx32-8AUIJ_DEAX9Yxfjy0SA";

        private static List<FrameworkUAD.Object.AddressLocation> ValidateExisting(List<FrameworkUAD.Object.AddressLocation> locations)
        {
            foreach (FrameworkUAD.Object.AddressLocation al in locations)
            {
                FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscription> worker = FrameworkServices.ServiceClient.UAD_SubscriptionClient();
                List<FrameworkUAD.Entity.Subscription> subscribers = worker.Proxy.SearchAddressZip(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, al.Street, al.PostalCode, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                if (subscribers.Count > 0)
                {
                    FrameworkUAD.Entity.Subscription sub = subscribers.First();
                    al.Latitude = sub.Latitude;
                    al.Longitude = sub.Longitude;
                    al.IsAddressValidated = true;
                    al.AddressValidationMessage = "Success - Good Address - Existing Record " + DateTime.Now.ToString();
                    al.AddressValidationSource = "Existing Record";
                    al.AddressValidationDate = DateTime.Now;
                    al.UpdatedByUserID = 1;
                }
                else
                {
                    al.Latitude = 0;
                    al.Longitude = 0;
                    al.IsAddressValidated = false;
                    al.AddressValidationMessage += "\n" + "\n" + "Invalid from Existing " + DateTime.Now.ToString();
                    al.AddressValidationSource = "Existing Record";
                    al.AddressValidationDate = DateTime.Now;
                    al.UpdatedByUserID = 1;
                }
            }
            return locations;
        }
        private static FrameworkUAD.Object.AddressLocation ValidateExisting(FrameworkUAD.Object.AddressLocation location)
        {
            FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscription> worker = FrameworkServices.ServiceClient.UAD_SubscriptionClient();
            List<FrameworkUAD.Entity.Subscription> subscribers = worker.Proxy.SearchAddressZip(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, location.Street, location.PostalCode, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
            if (subscribers.Count > 0)
            {
                FrameworkUAD.Entity.Subscription sub = subscribers.First();
                location.Latitude = sub.Latitude;
                location.Longitude = sub.Longitude;
                location.IsAddressValidated = true;
                location.AddressValidationMessage = "Success - Good Address - Existing Record " + DateTime.Now.ToString();
                location.AddressValidationSource = "Existing Record";
                location.AddressValidationDate = DateTime.Now;
                location.UpdatedByUserID = 1;
            }
            else
            {
                location.Latitude = 0;
                location.Longitude = 0;
                location.IsAddressValidated = false;
                location.AddressValidationMessage += "\n" + "\n" + "Invalid from Existing " + DateTime.Now.ToString();
                location.AddressValidationSource = "Existing Record";
                location.AddressValidationDate = DateTime.Now;
                location.UpdatedByUserID = 1;
            }
            return location;
        }
        
        public static List<FrameworkUAD.Object.AddressLocation> ValidateAddress_IncludeMapPoint(List<FrameworkUAD.Object.AddressLocation> Locations, bool validateExisting, bool useMapPoint = false)
        {
            if (validateExisting == true)
                Locations = ValidateExisting(Locations);
            if(useMapPoint == true)
                Locations = MapPointValidate(Locations);

            foreach (FrameworkUAD.Object.AddressLocation loc in Locations)
            {
                if (loc.IsAddressValidated == false)
                    ValidateBingAddress(loc);

                if (loc.IsAddressValidated == false)
                    ValidateGoogleAddress(loc);
            }
            return Locations;
        }
        public static FrameworkUAD.Object.AddressLocation ValidateAddress(FrameworkUAD.Object.AddressLocation location, bool validateExisting, bool useMapPoint = false)
        {
            if(validateExisting == true)
                location = ValidateExisting(location);

            if (location.IsAddressValidated == false && useMapPoint == true)
                location = ValidateMapPoint(location);

            if (location.IsAddressValidated == false)
                location = ValidateBingAddress(location);

            if (location.IsAddressValidated == false)
                location = ValidateGoogleAddress(location);

            return location;
        }
        
        public static List<FrameworkUAD.Object.AddressLocation> MapPointValidate(List<FrameworkUAD.Object.AddressLocation> locations)
        {
            try
            {
                MapPoint.Application mapApp = new MapPoint.Application();
                MapPoint.Map map = mapApp.NewMap();
                foreach (FrameworkUAD.Object.AddressLocation loc in locations)
                {
                    try
                    {
                        MapPoint.FindResults listFR;

                        listFR = map.FindAddressResults(loc.Street, loc.City, null, loc.Region, loc.PostalCode, null);

                        if (listFR.ResultsQuality == MapPoint.GeoFindResultsQuality.geoAllResultsValid || listFR.ResultsQuality == MapPoint.GeoFindResultsQuality.geoFirstResultGood)// || listFR.ResultsQuality == MapPoint.GeoFindResultsQuality.geoNoGoodResult)
                        {
                            foreach (object o in listFR)
                            {
                                if (o is MapPoint.Location)
                                {
                                    MapPoint.Location found = (MapPoint.Location)o;

                                    loc.Latitude = Convert.ToDecimal(found.Latitude);
                                    loc.Longitude = Convert.ToDecimal(found.Longitude);
                                    loc.Region = found.StreetAddress.Region;
                                    loc.City = found.StreetAddress.City;
                                    loc.Country = found.StreetAddress.Country.ToString();
                                    loc.PostalCode = found.StreetAddress.PostalCode;

                                    if (string.IsNullOrEmpty(loc.PostalCode))
                                        loc.PostalCode = found.StreetAddress.PostalCode;
                                    if (string.IsNullOrWhiteSpace(loc.PostalCode))
                                        loc.PostalCode = found.StreetAddress.PostalCode;
                                    if (loc.PostalCode.Length < 5)
                                        loc.PostalCode = found.StreetAddress.PostalCode;

                                    loc.AddressValidationMessage = "Success - Good Address - MapPoint " + DateTime.Now.ToString();
                                    loc.AddressValidationDate = DateTime.Now;
                                    loc.AddressValidationSource = "MapPoint";
                                    loc.IsAddressValidated = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            loc.IsAddressValidated = false;
                            loc.AddressValidationMessage += "\n" + "\n" + "Invalid from MapPoint " + DateTime.Now.ToString();
                            loc.AddressValidationDate = DateTime.Now;
                            loc.AddressValidationSource = "MapPoint";
                        }
                    }
                    catch { }
                }

                DestroyMapPoint(mapApp);
            }
            catch { }
            return locations;
        }
        public static FrameworkUAD.Object.AddressLocation ValidateMapPoint(FrameworkUAD.Object.AddressLocation loc)
        {
            try
            {
                MapPoint.Application mapApp = new MapPoint.Application();
                MapPoint.Map map = mapApp.NewMap();
                MapPoint.FindResults listFR;

                listFR = map.FindAddressResults(loc.Street, loc.City, null, loc.Region, loc.PostalCode, null);

                if (listFR.ResultsQuality == MapPoint.GeoFindResultsQuality.geoAllResultsValid || listFR.ResultsQuality == MapPoint.GeoFindResultsQuality.geoFirstResultGood)// || listFR.ResultsQuality == MapPoint.GeoFindResultsQuality.geoNoGoodResult)
                {
                    foreach (object o in listFR)
                    {
                        if (o is MapPoint.Location)
                        {
                            MapPoint.Location found = (MapPoint.Location)o;

                            loc.Latitude = Convert.ToDecimal(found.Latitude);
                            loc.Longitude = Convert.ToDecimal(found.Longitude);
                            loc.Region = found.StreetAddress.Region;
                            loc.City = found.StreetAddress.City;
                            loc.Country = found.StreetAddress.Country.ToString();
                            loc.PostalCode = found.StreetAddress.PostalCode;

                            if (string.IsNullOrEmpty(loc.PostalCode))
                                loc.PostalCode = found.StreetAddress.PostalCode;
                            if (string.IsNullOrWhiteSpace(loc.PostalCode))
                                loc.PostalCode = found.StreetAddress.PostalCode;
                            if (loc.PostalCode.Length < 5)
                                loc.PostalCode = found.StreetAddress.PostalCode;

                            loc.AddressValidationMessage = "Success - Good Address - MapPoint " + DateTime.Now.ToString();
                            loc.AddressValidationDate = DateTime.Now;
                            loc.AddressValidationSource = "MapPoint";
                            loc.IsAddressValidated = true;
                            break;
                        }
                    }
                }
                else
                {
                    loc.IsAddressValidated = false;
                    loc.AddressValidationMessage += "\n" + "\n" + "Invalid from MapPoint " + DateTime.Now.ToString();
                    loc.AddressValidationDate = DateTime.Now;
                    loc.AddressValidationSource = "MapPoint";
                }

                DestroyMapPoint(mapApp);
            }
            catch { }
            return loc;
        }
        public static FrameworkUAD.Object.AddressLocation ValidateYahooAddress(FrameworkUAD.Object.AddressLocation loc)
        {
            loc.AddressValidationSource = "Yahoo";
            loc.AddressValidationDate = DateTime.Now;
            loc.UpdatedByUserID = 1;

            if (loc.City == null)
                loc.City = string.Empty;
            if (loc.Region == null)
                loc.Region = string.Empty;
            if (loc.PostalCode == null)
                loc.PostalCode = string.Empty;
            if (loc.Street == null)
                loc.Street = string.Empty;

            string address = loc.Street + "," + loc.City.Replace(" ", "+") + "," + loc.Region.Trim().Replace(" ", "+") + "," + loc.PostalCode;
            string postParams = "?appid=dnk&location=" + address + "&Geocode=Geocode";

            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(YAHOO + postParams);

                httpRequest.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();

                DataSet ds = new DataSet();
                ds.ReadXml(response.GetResponseStream());
                DataTable dt = ds.Tables[0];

                foreach (DataRow xmlDR in dt.Rows)
                {
                    if (xmlDR["precision"].ToString().ToLower().Equals("address"))
                    {
                        loc.IsAddressValidated = true;
                        loc.AddressValidationMessage = "Success - Good Address - Yahoo " + DateTime.Now.ToString();

                        loc.Latitude = Convert.ToDecimal(xmlDR["Latitude"].ToString());
                        loc.Longitude = Convert.ToDecimal(xmlDR["Longitude"].ToString());
                        loc.PostalCode = xmlDR["Zip"].ToString();
                    }
                    else
                    {
                        loc.IsAddressValidated = false;
                        loc.AddressValidationMessage += "\n" + "\n" + "YAHOO NO ADDRESS - PRECISION: " + xmlDR["precision"].ToString() + DateTime.Now.ToString();
                    }
                    break;
                }
                response.Close();
            }
            catch (Exception ex)
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alClient.Proxy.LogCriticalError(accessKey, formatException, "DQM.Helpers.Validation.AddressCleaner.ValidateYahooAddress", app, string.Empty, logClientId);

                if (ex.ToString().Contains("(403) Forbidden"))
                {
                    loc.IsAddressValidated = false;
                    loc.AddressValidationMessage += "\n" + "\n" + "ERROR: YAHOO BLOCKED [STATUS CODE: (403) Forbidden] " + DateTime.Now.ToString();
                }
            }

            return loc;
        }
        public static FrameworkUAD.Object.AddressLocation ValidateBingAddress(FrameworkUAD.Object.AddressLocation loc)
        {
            loc.AddressValidationSource = "Bing";
            loc.AddressValidationDate = DateTime.Now;
            loc.UpdatedByUserID = 1;

            if (loc.City == null)
                loc.City = string.Empty;
            if (loc.Region == null)
                loc.Region = string.Empty;
            if (loc.PostalCode == null)
                loc.PostalCode = string.Empty;
            if (loc.Street == null)
                loc.Street = string.Empty;

            string postParams = BING;
            string bingKey = BING_Key;
            postParams += "&" + bingKey;

            postParams = postParams.Replace("adminDistrict", loc.Region);
            postParams = postParams.Replace("postalCode", loc.PostalCode);
            postParams = postParams.Replace("locality", loc.City);
            postParams = postParams.Replace("addressLine", loc.Street);
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(postParams);

                httpRequest.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();

                XmlDocument master = new XmlDocument();
                master.Load(response.GetResponseStream());
                XmlNode root = master.DocumentElement;
                XmlNode sc = FindNode(root.ChildNodes, "StatusCode");
                XmlNode confNode = FindNode(root.ChildNodes, "Confidence");
                XmlNode latNode = FindNode(root.ChildNodes, "Latitude");
                XmlNode lonNode = FindNode(root.ChildNodes, "Longitude");
                XmlNode postalCode = FindNode(root.ChildNodes, "PostalCode");

                decimal latitude = 0;
                decimal longitude = 0;

                if (sc.InnerText.Equals("200"))
                {
                    if (confNode.InnerText.Equals("High"))
                    {
                        latitude = Convert.ToDecimal(latNode.InnerText);
                        longitude = Convert.ToDecimal(lonNode.InnerText);
                    }
                }

                if (latitude > 0 && longitude < 0)
                {

                    loc.IsAddressValidated = true;
                    loc.AddressValidationMessage = "Success - Good Address - Bing " + DateTime.Now.ToString();
                    loc.Latitude = latitude;
                    loc.Longitude = longitude;  
                    loc.PostalCode = postalCode.InnerText;
                }
                else
                {
                    //400, 401, 500, 503
                    loc.IsAddressValidated = false;
                    loc.AddressValidationMessage += "\n" + "\n" + "BING NO ADDRESS - Status: " + sc.InnerText.ToString() + " Confidence: " + confNode.InnerText.ToString() + " Lat:" + latitude.ToString() + " Long:" + longitude.ToString() + "\n" + DateTime.Now.ToString();
                }

                response.Close();
            }
            catch { }

            return loc;
        }
        public static FrameworkUAD.Object.AddressLocation ValidateGoogleAddress(FrameworkUAD.Object.AddressLocation loc)
        {
            loc.AddressValidationSource = "Google";
            loc.AddressValidationDate = DateTime.Now;
            loc.UpdatedByUserID = 1;

            if (loc.City == null)
                loc.City = string.Empty;
            if (loc.Region == null)
                loc.Region = string.Empty;
            if (loc.PostalCode == null)
                loc.PostalCode = string.Empty;
            if (loc.Street == null)
                loc.Street = string.Empty;

            string address = loc.Street + "," + loc.City.Replace(" ", "+") + "," + loc.Region.Replace(" ", "+") + "," + loc.PostalCode.Trim();
            string postParams = "address=" + address + "&sensor=false";
            HttpWebResponse response = null;

            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(GOOGLE + postParams);

                httpRequest.Credentials = CredentialCache.DefaultCredentials;
                response = (HttpWebResponse)httpRequest.GetResponse();

                XmlDocument master = new XmlDocument();
                master.Load(response.GetResponseStream());
                XmlNode root = master.DocumentElement;
                XmlNode sc = FindNode(root.ChildNodes, "status");
                XmlNode latNode = FindNode(root.ChildNodes, "lat");
                XmlNode lonNode = FindNode(root.ChildNodes, "lng");
                XmlNode locTypeNode = FindNode(root.ChildNodes, "location_type");
                XmlNode formAddress = FindNode(root.ChildNodes, "formatted_address");
                //1600 Amphitheatre Pkwy, Mountain View, CA 94043, USA
                string[] longAddress = formAddress.InnerText.Split(',');
                string[] stateZip = longAddress[2].Split(' ');
                string zip = stateZip[2];

                //**************Location Type
                //"ROOFTOP" indicates that the returned result is a precise geocode for which we have location information accurate down to street address precision.
                //"RANGE_INTERPOLATED" indicates that the returned result reflects an approximation (usually on a road) interpolated between two precise points (such as intersections). Interpolated results are generally returned when rooftop geocodes are unavailable for a street address.
                //"GEOMETRIC_CENTER" indicates that the returned result is the geometric center of a result such as a polyline (for example, a street) or polygon (region).
                //"APPROXIMATE" indicates that the returned result is approximate.

                //***************STATUS
                //"OK" indicates that no errors occurred; the address was successfully parsed and at least one geocode was returned.
                //"ZERO_RESULTS" indicates that the geocode was successful but returned no results. This may occur if the geocode was passed a non-existent address or a latlng in a remote location.
                //"OVER_QUERY_LIMIT" indicates that you are over your quota.
                //"REQUEST_DENIED" indicates that your request was denied, generally because of lack of a sensor parameter.
                //"INVALID_REQUEST" generally indicates that the query (address or latlng) is missing.

                decimal latitude = 0;
                decimal longitude = 0;

                if (sc.InnerText.Equals("OK") && locTypeNode.InnerText.Equals("ROOFTOP"))
                {
                    latitude = Convert.ToDecimal(latNode.InnerText);
                    longitude = Convert.ToDecimal(lonNode.InnerText);

                    loc.IsAddressValidated = true;
                    loc.AddressValidationMessage = "Success - Good Address - Google " + DateTime.Now.ToString();
                    loc.Latitude = latitude;
                    loc.Longitude = longitude;
                    loc.PostalCode = zip;
                }
                else
                {
                    loc.IsAddressValidated = false;
                    loc.AddressValidationMessage += "\n" + "\n" + "GOOGLE NO ADDRESS - Status:  " + sc.InnerText.ToString() + "] " + DateTime.Now.ToString();
                }
            }
            catch
            {

            }
            response.Close();
            return loc;
        }

        private static void DestroyMapPoint(MapPoint.Application mapApp)
        {
            mapApp.ActiveMap.Saved = true;
            ((MapPoint._Application)mapApp).Quit();
            mapApp = null;
        }
        private static XmlNode FindNode(XmlNodeList list, string nodeName)
        {
            if (list.Count > 0)
            {
                foreach (XmlNode node in list)
                {
                    if (node.Name.Equals(nodeName))
                        return node;
                    if (node.HasChildNodes)
                    {
                        XmlNode nodeFound = FindNode(node.ChildNodes, nodeName);
                        if (nodeFound != null)
                            return nodeFound;
                    }
                }
            }
            return null;
        }
        private static string CleanAddress(string address)
        {
            address = address.Replace(",", string.Empty).Replace("#", string.Empty).Replace(".", string.Empty);
            #region remove duplicates
            string[] parseAdd = address.Split(' ');
            List<string> distList = new List<string>();
            foreach (string s in parseAdd)
            {
                distList.Add(s);
            }

            distList = distList.Distinct().ToList();

            StringBuilder sbReturn = new StringBuilder();
            foreach (string s in distList)
            {
                sbReturn.Append(s + " ");
            }
            #endregion

            return sbReturn.ToString().Trim();
        }
    }
}

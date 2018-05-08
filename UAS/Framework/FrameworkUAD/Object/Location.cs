using System;
using System.Configuration;
using System.Data;
using System.Net;
using System.Xml;

namespace FrameworkUAD.Object
{
    public class Location
    {
        public Location() { }
        public Location(string _Street = "", string _City = "", string _Region = "", string _PostalCode = "", double _Latitude = 0, double _Longitude = 0, bool _IsValid = false,
                        string _ValidationMessage = "")
        {
            Street = _Street;
            City = _City;
            Region = _Region;
            PostalCode = _PostalCode;
            Latitude = _Latitude;
            Longitude = _Longitude;
            IsValid = _IsValid;
            ValidationMessage = _ValidationMessage;
            DateUpdated = DateTime.Now;
            ModifiedByID = 1;
        }
        #region properties

        public string Street { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsValid { get; set; }

        public string ValidationMessage { get; set; }

        public DateTime DateUpdated { get; set; }

        public int ModifiedByID { get; set; }

        #endregion

        #region Data
        //Service at http://demo.gooddonor.org/WebServices/AddressValidationService/json/?address={ADDRESS1}&city={CITY}&state={STATE}&zipcode={ZIPCODE}
        //Service at http://demo.gooddonor.org/WebServices/AddressValidationService/xml/?address={ADDRESS1}&city={CITY}&state={STATE}&zipcode={ZIPCODE}

        #endregion

        static string YAHOO = "http://api.local.yahoo.com/MapsService/V1/geocode";
        static string GOOGLE = "https://maps.googleapis.com/maps/api/geocode/xml?";
        //static string GOOGLE_Key = "ABQIAAAAZOehRZQZ8Fc0ih-0QPaiVhSc1oGDelTNiEdrJ-ykOsaqvvkA6BSApQFx32-8AUIJ_DEAX9Yxfjy0SA";


        public static Location ValidateYahooAddress(Location loc)
        {
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
                HttpWebRequest httpRequest = (HttpWebRequest) WebRequest.Create(YAHOO + postParams);

                httpRequest.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse response = (HttpWebResponse) httpRequest.GetResponse();

                DataSet ds = new DataSet();
                ds.ReadXml(response.GetResponseStream());
                DataTable dt = ds.Tables[0];

                foreach (DataRow xmlDR in dt.Rows)
                {
                    if (xmlDR["precision"].ToString().ToLower().Equals("address"))
                    {
                        loc.IsValid = true;
                        loc.ValidationMessage = "Success - Good Address - Yahoo " + DateTime.Now.ToString();
                        loc.Latitude = Convert.ToDouble(xmlDR["Latitude"].ToString());
                        loc.Longitude = Convert.ToDouble(xmlDR["Longitude"].ToString());
                        //if (string.IsNullOrEmpty(loc.PostalCode))
                        loc.PostalCode = xmlDR["Zip"].ToString();
                    }
                    else
                    {
                        loc.IsValid = false;
                        loc.ValidationMessage += "\n" + "\n" + "YAHOO NO ADDRESS - PRECISION: " + xmlDR["precision"].ToString() + DateTime.Now.ToString();
                    }
                    break;
                }
                response.Close();
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("(403) Forbidden"))
                {
                    loc.IsValid = false;
                    loc.ValidationMessage += "\n" + "\n" + "ERROR: YAHOO BLOCKED [STATUS CODE: (403) Forbidden] " + DateTime.Now.ToString();
                }
            }

            return loc;
        }

        public static Location ValidateBingAddress(Location loc, string country)
        {
            string BING = string.Empty;

            if (country.Equals("UNITED STATES", StringComparison.OrdinalIgnoreCase))
                BING = "http://dev.virtualearth.net/REST/v1/Locations/US/adminDistrict/postalCode/locality/addressLine?output=xml";
            else
                BING = "http://dev.virtualearth.net/REST/v1/Locations/CA/adminDistrict/postalCode/locality/addressLine?output=xml";


            if (loc.City == null)
                loc.City = string.Empty;
            if (loc.Region == null)
                loc.Region = string.Empty;
            if (loc.PostalCode == null)
                loc.PostalCode = string.Empty;
            if (loc.Street == null)
                loc.Street = string.Empty;

            string postParams = BING;
            string bingKey = ConfigurationManager.AppSettings["BingKey"];
            postParams += "&" + bingKey;

            //adminDistrict/postalCode/locality/addressLine
            postParams = postParams.Replace("adminDistrict", loc.Region);
            postParams = postParams.Replace("postalCode", loc.PostalCode);
            postParams = postParams.Replace("locality", loc.City);
            postParams = postParams.Replace("addressLine", loc.Street);
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest) WebRequest.Create(postParams);

                httpRequest.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse response = (HttpWebResponse) httpRequest.GetResponse();

                XmlDocument master = new XmlDocument();
                master.Load(response.GetResponseStream());
                XmlNode root = master.DocumentElement;
                XmlNode sc = FindNode(root.ChildNodes, "StatusCode");
                XmlNode confNode = FindNode(root.ChildNodes, "Confidence");

                int tries = 1;
                while (confNode == null && tries < 10)
                {
                    tries++;
                    httpRequest = (HttpWebRequest) WebRequest.Create(postParams);
                    httpRequest.Credentials = CredentialCache.DefaultCredentials;
                    response = (HttpWebResponse) httpRequest.GetResponse();

                    master = new XmlDocument();
                    master.Load(response.GetResponseStream());
                    root = master.DocumentElement;
                    sc = FindNode(root.ChildNodes, "StatusCode");
                    confNode = FindNode(root.ChildNodes, "Confidence");
                }

                XmlNode latNode = FindNode(root.ChildNodes, "Latitude");
                XmlNode lonNode = FindNode(root.ChildNodes, "Longitude");
                XmlNode postalCode = FindNode(root.ChildNodes, "PostalCode");

                double latitude = 0;
                double longitude = 0;

                if (sc.InnerText.Equals("200"))
                {
                    if (confNode.InnerText.Equals("High"))//!confNode.InnerText.Equals("Low"))
                    {
                        latitude = Convert.ToDouble(latNode.InnerText);
                        longitude = Convert.ToDouble(lonNode.InnerText);
                    }
                }

                if (latitude > 0 && longitude < 0)
                {
                    loc.IsValid = true;
                    loc.ValidationMessage = "Success - Good Address - Bing " + DateTime.Now.ToString();
                    loc.Latitude = latitude;
                    loc.Longitude = longitude;
                    //if (string.IsNullOrEmpty(loc.PostalCode))
                    loc.PostalCode = postalCode.InnerText;
                }
                else
                {
                    //400, 401, 500, 503
                    loc.IsValid = false;
                    loc.ValidationMessage += "\n" + "\n" + "BING NO ADDRESS - Status: " + sc.InnerText.ToString() + " Confidence: " + confNode.InnerText.ToString() + " Lat:" + latitude.ToString() + " Long:" + longitude.ToString() + "\n" + DateTime.Now.ToString();
                }

                response.Close();
            }
            catch
            {

            }

            return loc;
        }

        public static Location ValidateGoogleAddress(Location loc)
        {
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
                HttpWebRequest httpRequest = (HttpWebRequest) WebRequest.Create(GOOGLE + postParams);

                httpRequest.Credentials = CredentialCache.DefaultCredentials;
                response = (HttpWebResponse) httpRequest.GetResponse();

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

                double latitude = 0;
                double longitude = 0;

                if (sc.InnerText.Equals("OK") && locTypeNode.InnerText.Equals("ROOFTOP"))
                {
                    latitude = Convert.ToDouble(latNode.InnerText);
                    longitude = Convert.ToDouble(lonNode.InnerText);

                    loc.IsValid = true;
                    loc.ValidationMessage = "Success - Good Address - Google " + DateTime.Now.ToString();
                    loc.Latitude = latitude;
                    loc.Longitude = longitude;
                    //if (string.IsNullOrEmpty(loc.PostalCode))
                    loc.PostalCode = zip;
                }
                else
                {
                    loc.IsValid = false;
                    loc.ValidationMessage += "\n" + "\n" + "GOOGLE NO ADDRESS - Status:  " + sc.InnerText.ToString() + "] " + DateTime.Now.ToString();
                }
            }
            catch
            {

            }
            response.Close();
            return loc;
        }

        private static XmlNode FindNode(XmlNodeList list, string nodeName)
        {
            if (list.Count > 0)
            {
                foreach (XmlNode node in list)
                {
                    if (node.Name.Equals(nodeName)) return node;
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
    }
}

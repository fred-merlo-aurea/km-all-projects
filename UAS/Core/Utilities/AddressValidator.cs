using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml;
using MapPoint;

namespace Core_AMS.Utilities
{
    public class AddressValidator
    {
        private const string StreetIdentifierVariation1 = "PO BOX";
        private const string StreetIdentifierVariation2 = "P O BOX";
        private const string StreetIdentifierVariation3 = "BOX ";
        private const string StreetIdentifierVariation4 = "PO ";
        private const string UnitedStatesCountryIdentifier = "united";
        private const string AmericaCountryIdentifier = "america";
        private const string UnitedStatesCountry = "United States";
        private const string UnitedStatesMisspelledCountry = "unitedstates";
        private const string CanadaCountry = "Canada";
        private const string GeoCountryPrefix = "geoCountry";
        private const string AmpersandChar = "&";
        private const string EmptySpace = " ";
        private const string PlusChar = "+";
        private const string CommaChar = ",";
        private const string InvalidMapPointMessageFormat = "Invalid from MapPoint {0}";
        private const string RooftopGoodAddressMessageFormat = "Rooftop - Good Address - MapPoint {0}";
        private const string RooftopGoogleAddressMessageFormat = "Rooftop - Good Address - Google {0}";
        private const string StreetGoogleAddressMessageFormat = "Street Level - Good Address - Google {0}";
        private const string ZipCodeGoodAddressMessageFormat = "Zipcode Only - Good Address - MapPoint {0}";
        private const string InvalidAddressMessageFormat = "Invalid Address {0}";
        private const string InvalidGoogleAddressMessageFormat = "{0}{0}GOOGLE NO ADDRESS - Status: {1}] {2}";
        private const string LocationErrorMessage = "The RPC server is unavailable. (Exception from HRESULT: 0x800706BA)";
        private const string GoogleApiPostParamFormat = "address={0}&sensor=false";
        private const string GoogleApiStatusFieldName = "status";
        private const string GoogleApiLatitudeFieldName = "lat";
        private const string GoogleApiLongitudeFieldName = "lng";
        private const string GoogleApiLocationTypeFieldName = "location_type";
        private const string GoogleApiAddressFieldName = "formatted_address";
        private const string HttpStatusOk = "OK";
        private const string LocationTypeRooftop = "ROOFTOP";
        private const string LocationTypeRange = "RANGE_INTERPOLATED";
        private const string LocationTypeApproximate = "APPROXIMATE";
        private const int MinPostalCodeLength = 5;
        private const int MaxPostalCodeLength = 7;
        private const int MaxPostalCodePlusFourLength = 4;

        private readonly string GoogleApiEndpoint = "https://maps.googleapis.com/maps/api/geocode/xml?";
        private readonly IList<string> _usRegions = new List<string>
        {
            "AL",
            "AK",
            "AZ",
            "AR",
            "CA",
            "CO",
            "CT",
            "DC",
            "DE",
            "FL",
            "GA",
            "HI",
            "ID",
            "IL",
            "IN",
            "IA",
            "KS",
            "KY",
            "LA",
            "ME",
            "MD",
            "MA",
            "MI",
            "MN",
            "MS",
            "MO",
            "MT",
            "NE",
            "NV",
            "NH",
            "NJ",
            "NM",
            "NY",
            "NC",
            "ND",
            "OH",
            "OK",
            "OR",
            "PA",
            "RI",
            "SC",
            "SD",
            "TN",
            "TX",
            "UT",
            "VT",
            "VA",
            "WA",
            "WV",
            "WI",
            "WY"
        };
        private readonly IList<string> _canadaRegions = new List<string>
        {
            "AB",
            "BC",
            "MB",
            "NB",
            "NL",
            "NS",
            "NT",
            "NU",
            "ON",
            "PE",
            "QC",
            "SK",
            "YT"
        };

        public IList<string> GetUsRegions()
        {
            return _usRegions;
        }

        public IList<string> GetCanadaRegions()
        {
            return _canadaRegions;
        }

        public string CleanAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return string.Empty;
            }

            address = StringFunctions.Allow_Numbers_Letters_Spaces_Dashes(address);
            var addressParts = address.Split(EmptySpace.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var distinctAddressParts = addressParts.Distinct();

            return string.Join(EmptySpace, distinctAddressParts);
        }

        public void ValidateAddress(ref AddressLocation location, bool useMapPoint)
        {
            if (location == null)
            {
                return;
            }

            var locations = new List<AddressLocation> { location };
            ValidateAddresses(locations, true, useMapPoint);
        }

        public IList<AddressLocation> ValidateAddresses(
            IList<AddressLocation> locations,
            bool validateExisting,
            bool useMapPoint = false,
            bool useBingGoogle = true)
        {
            if (locations == null)
            {
                return new List<AddressLocation>();
            }

            if (useMapPoint)
            {
                ValidateAddressesUsingMapPoints(locations);
            }

            var done = 0;
            var total = locations.Count;
            var validatedAddresses = new List<AddressLocation>();
            for (int i = 0; i < locations.Count; i++)
            {
                var location = locations[i];
                if (useBingGoogle)
                {
                    DisplayTraceMessage($"Bing-Google Validate: {++done} of {total}");
                    ValidateGoogle(location);
                }

                if (!location.IsValid && string.IsNullOrWhiteSpace(location.ValidationMessage))
                {
                    location.ValidationMessage = string.Format(InvalidAddressMessageFormat, DateTime.Now);
                }

                validatedAddresses.Add(location);
            }

            return validatedAddresses;
        }

        private void ValidateAddressesUsingMapPoints(IList<AddressLocation> locations)
        {
            GuardNotNull(locations, nameof(locations));

            try
            {
                var mapPointService = new MapPointService();
                using (var mapApplication = mapPointService.CreateApplication())
                {
                    var map = GetMap(mapApplication);
                    if (map == null)
                    {
                        return;
                    }

                    var done = 0;
                    var total = locations.Count;
                    foreach (var location in locations)
                    {
                        DisplayTraceMessage($"MapPoint {++done} of {total}");
                        try
                        {
                            UpdateAddressLocation(location, map);
                            if (!location.IsValid)
                            {
                                UpdateAddressLocationZipOnly(location, map);
                            }
                        }
                        // All exceptions must be caught and NOT interrupt the iteration flow
                        catch (Exception exception)
                        {
                            location.ErrorOccured = true;
                            if (exception.Message != null &&
                                !exception.Message.Equals(
                                    LocationErrorMessage, 
                                    StringComparison.OrdinalIgnoreCase))
                            {
                                LogException(exception);
                            }
                        }
                    }
                }
            }
            // All exceptions should be intercepted and prohibited from propagating 
            // since the validation process is non-breaking
            catch (Exception exception)
            {
                LogException(exception);
            }
        }

        private void UpdateAddressLocation(AddressLocation location, Map map)
        {
            GuardNotNull(location, nameof(location));
            GuardNotNull(map, nameof(map));

            if (!string.IsNullOrWhiteSpace(location.Street))
            {
                if (location.Street.IndexOf(StreetIdentifierVariation1, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    location.Street.IndexOf(StreetIdentifierVariation2, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    location.Street.IndexOf(StreetIdentifierVariation3, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    location.Street.IndexOf(StreetIdentifierVariation4, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    location.Street.IndexOf(AmpersandChar, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    location.Street = string.Empty;
                }
            }

            if (!string.IsNullOrWhiteSpace(location.Region))
            {
                if (!_usRegions.Any(x => x.Equals(location.Region, StringComparison.OrdinalIgnoreCase)) &&
                    !_canadaRegions.Any(x => x.Equals(location.Region, StringComparison.OrdinalIgnoreCase)))
                {
                    location.Region = string.Empty;
                }
            }

            var addressLocationsFound = FindAddressLocations(location, map);
            if (addressLocationsFound != null &&
                (addressLocationsFound.ResultsQuality == GeoFindResultsQuality.geoAllResultsValid ||
                 addressLocationsFound.ResultsQuality == GeoFindResultsQuality.geoFirstResultGood))
            {
                UpdateRooftopAddressLocation(addressLocationsFound, location);
            }
            else if (addressLocationsFound != null && !string.IsNullOrEmpty(location.PostalCode))
            {
                UpdateZipCodeAddressLocation(addressLocationsFound, location);
            }
            else
            {
                location.IsValid = false;
                location.ValidationMessage = string.Format(InvalidMapPointMessageFormat, DateTime.Now);
            }
        }

        private static void UpdateAddressLocationZipOnly(AddressLocation location, Map map)
        {
            GuardNotNull(location, nameof(location));
            GuardNotNull(map, nameof(map));

            var addressLocationsFound = FindAddressLocationsByZipCode(location, map);
            if (addressLocationsFound != null && !string.IsNullOrEmpty(location.PostalCode))
            {
                UpdateZipCodeAddressLocation(addressLocationsFound, location);
            }
            else
            {
                location.IsValid = false;
                location.ValidationMessage = string.Format(InvalidMapPointMessageFormat, DateTime.Now);
            }
        }

        private static void UpdateRooftopAddressLocation(FindResults addressResults, AddressLocation location)
        {
            GuardNotNull(location, nameof(location));
            if (addressResults == null)
            {
                return;
            }

            foreach (var address in addressResults)
            {
                var addressResult = address as Location;
                if (addressResult?.StreetAddress == null)
                {
                    continue;
                }

                location.Latitude = addressResult.Latitude;
                location.Longitude = addressResult.Longitude;
                location.Region = addressResult.StreetAddress.Region;
                location.City = addressResult.StreetAddress.City;
                location.IsValid = true;

                if (!string.IsNullOrWhiteSpace(addressResult.StreetAddress.PostalCode))
                {
                    var postalCodePlusFour = string.Empty;
                    var postalCode = GetPostalCode(addressResult.StreetAddress.PostalCode, out postalCodePlusFour);
                    location.PostalCode = postalCode ?? location.PostalCode;
                    location.PostalCodePlusFour = postalCodePlusFour ?? location.PostalCodePlusFour;
                }
                else if (string.IsNullOrWhiteSpace(location.PostalCode))
                {
                    location.PostalCode = addressResult.StreetAddress.PostalCode;
                }

                var country = addressResult.StreetAddress.Country.ToString();
                var geoCountryIndex = country.IndexOf(GeoCountryPrefix, StringComparison.OrdinalIgnoreCase);
                if (geoCountryIndex != -1)
                {
                    location.Country = country.Substring(geoCountryIndex);
                }

                if (location.Country.Equals(UnitedStatesMisspelledCountry, StringComparison.OrdinalIgnoreCase))
                {
                    location.Country = UnitedStatesCountry;
                }

                location.ValidationMessage = string.Format(RooftopGoodAddressMessageFormat, DateTime.Now);
                break;
            }
        }

        private static void UpdateZipCodeAddressLocation(FindResults addressResults, AddressLocation location)
        {
            GuardNotNull(location, nameof(location));
            if (addressResults == null)
            {
                return;
            }

            foreach (var address in addressResults)
            {
                var addressResult = address as Location;
                if (addressResult == null)
                {
                    continue;
                }

                location.Latitude = addressResult.Latitude;
                location.Longitude = addressResult.Longitude;
                location.IsValid = true;

                location.ValidationMessage = string.Format(ZipCodeGoodAddressMessageFormat, DateTime.Now);
                break;
            }
        }

        private static FindResults FindAddressLocations(AddressLocation location, Map map)
        {
            GuardNotNull(location, nameof(location));
            GuardNotNull(map, nameof(map));

            var country = GetGeoCountry(location.Country);
            if (country.HasValue && country.Value == GeoCountry.geoCountryDefault)
            {
                return null;
            }

            var postalCode = location.PostalCode;
            if (string.IsNullOrWhiteSpace(postalCode) || postalCode.Length < 5)
            {
                postalCode = null;
            }

            FindResults addressLocationsFound;
            try
            {
                addressLocationsFound = map.FindAddressResults(
                    location.Street,
                    location.City,
                    null,
                    location.Region,
                    postalCode,
                    country);
            }
            catch
            {
                addressLocationsFound = map.FindAddressResults(
                    string.Empty,
                    location.City,
                    null,
                    location.Region,
                    postalCode, 
                    country);
            }

            return addressLocationsFound;
        }

        private static FindResults FindAddressLocationsByZipCode(AddressLocation location, Map map)
        {
            GuardNotNull(location, nameof(location));
            GuardNotNull(map, nameof(map));

            var country = GetGeoCountry(location.Country);
            if (country.HasValue && country.Value == GeoCountry.geoCountryDefault)
            {
                return null;
            }

            FindResults addressLocationsFound = null;
            if (location.PostalCode.Length >= MinPostalCodeLength)
            {
                addressLocationsFound = map.FindAddressResults(
                    string.Empty,
                    string.Empty,
                    null,
                    string.Empty,
                    location.PostalCode,
                    country);
            }

            return addressLocationsFound;
        }

        private static GeoCountry? GetGeoCountry(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
            {
                return null;
            }

            if (country.Equals(UnitedStatesCountry, StringComparison.OrdinalIgnoreCase) ||
                (country.IndexOf(UnitedStatesCountryIdentifier, StringComparison.OrdinalIgnoreCase) >= 0 &&
                 country.IndexOf(AmericaCountryIdentifier, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                return GeoCountry.geoCountryUnitedStates;
            }

            if (country.Equals(CanadaCountry, StringComparison.OrdinalIgnoreCase))
            {
                return GeoCountry.geoCountryCanada;
            }

            return GeoCountry.geoCountryDefault;
        }

        private static string GetPostalCode(string postalCode, out string postalCodePlusFour)
        {
            postalCodePlusFour = null;
            if (string.IsNullOrWhiteSpace(postalCode))
            {
                return null;
            }

            var postalCodeLength = postalCode.Length;
            if (postalCodeLength >= MinPostalCodeLength || postalCodeLength <= MaxPostalCodeLength)
            {
                return postalCode;
            }
            else if (postalCodeLength > MaxPostalCodeLength)
            {
                var postalCodePlusFourLength = postalCodeLength - MaxPostalCodeLength;
                if (postalCodePlusFourLength > MaxPostalCodePlusFourLength)
                {
                    postalCodePlusFourLength = MaxPostalCodePlusFourLength;
                }

                postalCodePlusFour = postalCode.Substring(MinPostalCodeLength, postalCodePlusFourLength);
                return postalCode.Substring(0, MinPostalCodeLength);
            }

            return null;
        }

        public AddressLocation ValidateGoogle(AddressLocation location)
        {
            GuardNotNull(location, nameof(location));
            if (location.IsValid)
            {
                return location;
            }

            location.City = location.City ?? string.Empty;
            location.Region = location.Region ?? string.Empty;
            location.PostalCode = location.PostalCode ?? string.Empty;
            location.Street = location.Street ?? string.Empty;
            try
            {
                var master = GetGoogleAddress(location);
                var root = master.DocumentElement;
                var statusNode = FindNode(root.ChildNodes, GoogleApiStatusFieldName);
                var latitudeNode = FindNode(root.ChildNodes, GoogleApiLatitudeFieldName);
                var longitudeNode = FindNode(root.ChildNodes, GoogleApiLongitudeFieldName);
                var locationTypeNode = FindNode(root.ChildNodes, GoogleApiLocationTypeFieldName);
                var formattedAddressNode = FindNode(root.ChildNodes, GoogleApiAddressFieldName);

                //1600 Amphitheatre Pkwy, Mountain View, CA 94043, USA
                var validAddressParts = 4;
                var addressParts = formattedAddressNode.InnerText.Split(CommaChar.ToCharArray());
                if (addressParts.Length < validAddressParts)
                {
                    location.ErrorOccured = true;
                    return location;
                }

                var validStateZipParts = 2;
                if (addressParts.Length < validStateZipParts)
                {
                    location.ErrorOccured = true;
                    return location;
                }
                var stateZip = addressParts[2].Trim().Split(EmptySpace.ToCharArray());
                var state = stateZip[0];
                var zip = stateZip[1];
                var googleAddress = addressParts[0];
                var city = addressParts[1];
                var country = addressParts[3];

                UpdateGoogleAddressLocation(location,
                    statusNode,
                    locationTypeNode,
                    latitudeNode,
                    longitudeNode,
                    zip,
                    googleAddress,
                    city,
                    country,
                    state);
            }
            // All exceptions should be intercepted and prohibited from propagating 
            // since the validation process is non-breaking
            catch (Exception ex)
            {
                location.ErrorOccured = true;
            }

            return location;
        }

        private static void UpdateGoogleAddressLocation(
            AddressLocation location,
            XmlNode statusNode,
            XmlNode locationTypeNode,
            XmlNode latitudeNode,
            XmlNode longitudeNode,
            string zip,
            string googleAddress,
            string city,
            string country,
            string state)
        {
            GuardNotNull(location, nameof(location));

            if (statusNode.InnerText.Equals(HttpStatusOk, StringComparison.OrdinalIgnoreCase))
            {
                double latitude;
                double longitude;

                Double.TryParse(latitudeNode.InnerText, out latitude);
                Double.TryParse(longitudeNode.InnerText, out longitude);
                location.Latitude = latitude;
                location.Longitude = longitude;
                location.IsValid = true;

                if (locationTypeNode.InnerText.Equals(LocationTypeRooftop, StringComparison.OrdinalIgnoreCase))
                {
                    var postalCodePlusFour = string.Empty;
                    var postalCode = GetPostalCode(zip, out postalCodePlusFour);
                    location.PostalCode = postalCode != null ? location.PostalCode : zip;
                    location.PostalCodePlusFour = postalCodePlusFour ?? location.PostalCodePlusFour;
                    location.Street = googleAddress;
                    location.City = city;
                    location.Country = country;
                    location.Region = state;
                    location.ValidationMessage = string.Format(RooftopGoogleAddressMessageFormat, DateTime.Now);
                }

                if (locationTypeNode.InnerText.Equals(LocationTypeRange) ||
                    locationTypeNode.InnerText.Equals(LocationTypeApproximate))
                {
                    location.ValidationMessage = string.Format(StreetGoogleAddressMessageFormat, DateTime.Now);
                }
            }
            else
            {
                location.IsValid = false;
                location.ValidationMessage +=
                    string.Format(
                        InvalidGoogleAddressMessageFormat,
                        Environment.NewLine,
                        statusNode.InnerText,
                        DateTime.Now);
            }
        }

        private static string BuildGoogleApiPostParam(AddressLocation location)
        {
            GuardNotNull(location, nameof(location));

            var street = location.Street ?? string.Empty;
            var city = location.City?.Replace(EmptySpace, PlusChar) ?? string.Empty;
            var region = location.Region?.Replace(EmptySpace, PlusChar) ?? string.Empty;
            var postalCode = location.PostalCode?.Trim() ?? string.Empty;
            if (postalCode.Length < MinPostalCodeLength)
            {
                postalCode = string.Empty;
            }

            var address = $"{street},{city},{region},{postalCode}";
            return string.Format(GoogleApiPostParamFormat, address);
        }

        private XmlDocument GetGoogleAddress(AddressLocation location)
        {
            GuardNotNull(location, nameof(location));

            var postParams = BuildGoogleApiPostParam(location);
            var httpRequest = (HttpWebRequest)WebRequest.Create(GoogleApiEndpoint + postParams);
            httpRequest.Timeout = (int)TimeSpan.FromSeconds(30).TotalSeconds;
            httpRequest.Credentials = CredentialCache.DefaultCredentials;

            var xmlResult = new XmlDocument();
            using (var response = (HttpWebResponse)httpRequest.GetResponse())
            {
                var responseStream = response.GetResponseStream();
                xmlResult.Load(responseStream);
            }

            return xmlResult;
        }

        private XmlNode FindNode(XmlNodeList nodes, string nodeName)
        {
            if (nodes?.Count > 0)
            {
                foreach (var node in nodes.OfType<XmlNode>())
                {
                    if (node.Name.Equals(nodeName, StringComparison.OrdinalIgnoreCase))
                    {
                        return node;
                    }

                    if (node.HasChildNodes)
                    {
                        var nodeFound = FindNode(node.ChildNodes, nodeName);
                        if (nodeFound != null)
                        {
                            return nodeFound;
                        }
                    }
                }
            }

            return null;
        }

        private Map GetMap(MapApplication mapApplication)
        {
            var app = mapApplication?.Application;
            return app?.NewMap();
        }

        private void DisplayTraceMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
        }

        private void LogException(Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            var message = StringFunctions.FormatException(exception);
            DisplayTraceMessage(message);
        }

        private static void GuardNotNull(object item, string itemName)
        {
            if (item == null)
            {
                throw new ArgumentNullException(itemName);
            }
        }
    }
}

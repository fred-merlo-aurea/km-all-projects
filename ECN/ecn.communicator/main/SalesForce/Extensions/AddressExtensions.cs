using AddressValidator;

namespace ecn.communicator.main.Salesforce.Extensions
{
    public static class AddressExtensions
    {
        public static bool IsSameGeoLocation(this AddressLocation first, AddressLocation second)
        {
            return first.Latitude == second.Latitude && first.Longitude == second.Longitude;
        }

        public static bool IsSameStreet(this AddressLocation first, AddressLocation second)
        {
            return first.Street == second.Street;
        }

        public static AddressLocation GetValidAddress(this AddressLocation location)
        {
            if (!string.IsNullOrEmpty(location.Street))
            {
                try
                {
                    return Validator.ValidateAddress_NoMapPoint(location);
                }
                catch { }
            }

            return null;
        }
    }
}
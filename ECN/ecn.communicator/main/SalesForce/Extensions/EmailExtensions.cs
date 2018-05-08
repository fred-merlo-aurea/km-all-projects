using AddressValidator;
using ECN_Framework_Entities.Communicator;

namespace ecn.communicator.main.Salesforce.Extensions
{
    public static class EmailExtensions
    {
        public static string FullAddress(this Email email)
        {
            return $"{email.Address} {email.Address2}";
        }

        public static AddressLocation BuildAddressLocation(this Email email)
        {
            return new AddressLocation
            {
                City = email.City,
                IsValid = false,
                Latitude = 0,
                Longitude = 0,
                PostalCode = email.Zip,
                Region = email.State,
                Street = email.FullAddress(),
                ValidationMessage = string.Empty
            };
        }
    }
}
using System.Text;
using AddressValidator;
using ecn.communicator.main.Salesforce.SF_Pages;

namespace ecn.communicator.main.Salesforce.Extensions
{
    public static class LocationEntityExtensions
    {
        public static AddressLocation BuildAddressLocation(this LocationEntity entity)
        {
            return new AddressLocation
            {
                City = entity.City,
                IsValid = false,
                Latitude = 0,
                Longitude = 0,
                PostalCode = entity.PostalCode,
                Region = entity.State,
                Street = entity.Street,
                ValidationMessage = string.Empty
            };
        }

        public static string ToXml(this LocationEntity entity)
        {
            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<Emails>");
            xmlBuilder.Append($"<emailaddress>{entity.Email.Trim()}</emailaddress>");
            xmlBuilder.Append($"<firstname>{entity.FirstName.Trim()}</firstname>");
            xmlBuilder.Append($"<lastname>{entity.LastName.Trim()}</lastname>");
            xmlBuilder.Append($"<fullname>{entity.Name.Trim()}</fullname>");
            xmlBuilder.Append($"<city>{entity.City.Trim()}</city>");
            xmlBuilder.Append($"<state>{entity.State.Trim()}</state>");
            xmlBuilder.Append($"<zip>{entity.PostalCode.Trim()}</zip>");
            xmlBuilder.Append($"<address>{entity.Street.Trim().Replace("\\r", " ").Replace("\\n", " ").Replace("\"", " ")}</address>");
            xmlBuilder.Append($"<voice>{entity.Phone.Trim()}</voice>");
            xmlBuilder.Append($"<mobile>{entity.MobilePhone.Trim()}</mobile>");
            xmlBuilder.Append("</Emails>");

            return xmlBuilder.ToString();
        }
    }
}
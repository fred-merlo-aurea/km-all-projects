using ecn.communicator.main.Salesforce.Entity;
using ecn.communicator.main.Salesforce.SF_Pages;

namespace ecn.communicator.main.Salesforce.Extensions
{
    public static class SalesForceEntityExtensions
    {
        public static LocationEntity ToEntity(this SF_Lead lead)
        {
            return new LocationEntity
            {
                Id = lead.Id,
                Name = lead.Name,
                FirstName = lead.FirstName,
                LastName = lead.LastName,
                Email = lead.Email,
                Phone = lead.Phone,
                MobilePhone = lead.MobilePhone,
                City = lead.City,
                PostalCode = lead.PostalCode,
                State = lead.State,
                Street = lead.Street
            };
        }

        public static LocationEntity ToEntity(this SF_Contact contact)
        {
            return new LocationEntity
            {
                Id = contact.Id,
                Name = contact.Name,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                Phone = contact.Phone,
                MobilePhone = contact.MobilePhone,
                City = contact.MailingCity,
                PostalCode = contact.MailingPostalCode,
                State = contact.MailingState,
                Street = contact.MailingStreet
            };
        }
    }
}
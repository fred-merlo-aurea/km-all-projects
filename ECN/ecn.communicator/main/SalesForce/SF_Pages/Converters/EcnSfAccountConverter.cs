using AddressValidator;
using ecn.communicator.main.Salesforce.Entity;
using ecn.communicator.main.Salesforce.Extensions;
using ECN_Framework_Entities.Accounts;
using System;

namespace ecn.communicator.main.Salesforce.SF_Pages.Converters
{
    public class EcnSfAccountConverter : EcnSfConverterBase
    {
        public AccountEcnSfViewModel Convert(Customer customer, SF_Account entity, AddressLocation ecnAddress, AddressLocation sfAddress)
        {
            if (customer == null)
            {
                throw new ArgumentNullException($"The parameter {nameof(customer)} shouldn't be null");
            }

            if (entity == null)
            {
                throw new ArgumentNullException($"The parameter {nameof(entity)} shouldn't be null");
            }

            var model = new AccountEcnSfViewModel(customer.CustomerID, entity.Id)
            {
                Address = new ItemViewModel(customer.Address, entity.BillingStreet),
                City = new ItemViewModel(customer.City, entity.BillingCity),
                Country = new ItemViewModel(customer.Country, entity.BillingCountry),
                Fax = new ItemViewModel(customer.Fax, entity.Fax),
                Name = new ItemViewModel(customer.CustomerName, entity.Name),
                Phone = new ItemViewModel(customer.Phone, entity.Phone),
                State = new ItemViewModel(customer.State, entity.BillingState),
                Zip = new ItemViewModel(customer.Zip, entity.BillingPostalCode)
            };

            UpdateModelByLocation(model, ecnAddress, sfAddress);

            return model;
        }

        protected override void UpdateModelByLocation(EcnSfViewModel model, AddressLocation ecnAddress, AddressLocation sfAddress)
        {
            var accountModel = model as AccountEcnSfViewModel;

            if (accountModel != null
                && sfAddress != null
                && ecnAddress != null
                && (sfAddress.IsValid || ecnAddress.IsValid)
                && ecnAddress.IsSameGeoLocation(sfAddress))
            {
                if (ecnAddress.IsValid && !sfAddress.IsValid)
                {
                    SetColorNames(accountModel, ColorName.BlueLight, ColorName.GreyDark);
                    accountModel.Country.Visible = true;
                }
                else if (!ecnAddress.IsValid && sfAddress.IsValid)
                {
                    SetColorNames(accountModel, ColorName.GreyDark, ColorName.BlueLight);
                    accountModel.Country.Visible = true;
                }
                else
                {
                    SetBlueLight(accountModel);
                    accountModel.Country.SetColor(ColorName.BlueLight);
                    accountModel.Country.Visible = false;
                }
            }
        }
    }
}
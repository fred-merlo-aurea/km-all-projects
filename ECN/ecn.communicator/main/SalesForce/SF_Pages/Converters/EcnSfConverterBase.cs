using AddressValidator;
using ecn.communicator.main.Salesforce.Extensions;
using ECN_Framework_Entities.Communicator;

namespace ecn.communicator.main.Salesforce.SF_Pages.Converters
{
    public abstract class EcnSfConverterBase
    {
        public EcnSfViewModel Convert(Email email, LocationEntity entity, AddressLocation ecnLocation, AddressLocation sfLocation)
        {
            var model = new EcnSfViewModel(email.EmailID, entity.Id)
            {
                CellPhone = new ItemViewModel(email.Mobile, entity.MobilePhone),
                Phone = new ItemViewModel(email.Voice, entity.Phone),
                FirstName = new ItemViewModel(email.FirstName, entity.FirstName),
                LastName = new ItemViewModel(email.LastName, entity.LastName),
                Email = new ItemViewModel(email.EmailAddress, entity.Email),
                Address = GetAddress(email, entity),
                City = new ItemViewModel(email.City, entity.City),
                State = new ItemViewModel(email.State, entity.State),
                Zip = new ItemViewModel(email.Zip, entity.PostalCode)
            };

            UpdateModelByLocation(model, ecnLocation, sfLocation);
            return model;
        }

        protected virtual ItemViewModel GetAddress(Email email, LocationEntity entity)
        {
            return new ItemViewModel(email.FullAddress(), entity.Street);
        }

        protected abstract void UpdateModelByLocation(EcnSfViewModel model, AddressLocation ecnAddress, AddressLocation sfAddress);

        protected void SetColorNames(EcnSfViewModel model, ColorName ecnColor, ColorName sfColor)
        {
            model.Address.EcnColor = ecnColor;
            model.Address.SfColor = sfColor;
            model.Address.Visible = true;
            model.City.Visible = true;
            model.State.Visible = true;
            model.Zip.Visible = true;
        }

        protected void SetBlueLight(EcnSfViewModel model)
        {
            model.Address.SetColor(ColorName.BlueLight);
            model.City.SetColor(ColorName.BlueLight);
            model.State.SetColor(ColorName.BlueLight);
            model.Zip.SetColor(ColorName.BlueLight);
            model.Address.Visible = false;
            model.City.Visible = false;
            model.State.Visible = false;
            model.Zip.Visible = false;
        }
    }
}
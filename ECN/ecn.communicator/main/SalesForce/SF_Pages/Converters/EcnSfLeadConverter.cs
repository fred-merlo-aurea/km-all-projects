using AddressValidator;
using ecn.communicator.main.Salesforce.Extensions;
using ECN_Framework_Entities.Communicator;

namespace ecn.communicator.main.Salesforce.SF_Pages.Converters
{
    public class EcnSfLeadConverter : EcnSfConverterBase
    {
        protected override ItemViewModel GetAddress(Email email, LocationEntity entity)
        {
            var address = new ItemViewModel(email.Address, entity.Street);
            address.EcnText = email.FullAddress();

            return address;
        }
        protected override void UpdateModelByLocation(EcnSfViewModel model, AddressLocation ecnAddress, AddressLocation sfAddress)
        {
            if (sfAddress != null && ecnAddress != null && (sfAddress.IsValid || ecnAddress.IsValid))
            {
                if (ecnAddress.IsValid && !sfAddress.IsValid)
                {
                    SetColorNames(model, ColorName.BlueLight, ColorName.GreyDark);
                }
                else if (!ecnAddress.IsValid && sfAddress.IsValid)
                {
                    SetColorNames(model, ColorName.GreyDark, ColorName.BlueLight);
                }
                else if (ecnAddress.IsValid && sfAddress.IsValid && ecnAddress.IsSameStreet(sfAddress))
                {
                    SetBlueLight(model);
                }
                else
                {
                    model.Address.SetColor(ColorName.GreyDark);
                    model.Address.Visible = true;
                }
            }
        }
    }
}
using AddressValidator;
using ecn.communicator.main.Salesforce.Extensions;

namespace ecn.communicator.main.Salesforce.SF_Pages.Converters
{
    public class EcnSfContactConverter : EcnSfConverterBase
    {
        protected override void UpdateModelByLocation(EcnSfViewModel model, AddressLocation ecnAddress, AddressLocation sfAddress)
        {
            if (sfAddress != null 
                && ecnAddress != null 
                && (sfAddress.IsValid || ecnAddress.IsValid) 
                && ecnAddress.IsSameGeoLocation(sfAddress))
            {
                if (ecnAddress.IsValid && !sfAddress.IsValid)
                {
                    SetColorNames(model, ColorName.BlueLight, ColorName.GreyDark);
                }
                else if (!ecnAddress.IsValid && sfAddress.IsValid)
                {
                    SetColorNames(model, ColorName.GreyDark, ColorName.BlueLight);
                }
                else
                {
                    SetBlueLight(model);
                }
            }
        }
    }
}
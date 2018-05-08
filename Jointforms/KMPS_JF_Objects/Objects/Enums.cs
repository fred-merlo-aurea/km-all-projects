using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KMPS_JF_Objects.Objects
{
    public class Enums
    {
        public enum CCProcessors
        {
            Paypal, AuthorizeDotNet, PaypalRedirect, Paypalflow
        }

        public enum Region
        { 
            US, CANADA, INTERNATIONAL, ISLAND
        }

        public enum DiscountType
        {
            FLATAMOUNT, PERCENTAGE
        }      
    }
}

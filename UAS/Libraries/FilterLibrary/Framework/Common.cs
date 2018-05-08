using System;
using System.Collections.Generic;
using System.Linq;

namespace FilterControls.Framework
{
    public class Common
    {
        public static bool CheckResponse<T>(List<T> response, FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes status, bool showServiceError = true)
        {
            if (response != null && status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                return true;
            else
            {
                if (showServiceError == true)
                    Core_AMS.Utilities.WPF.MessageServiceError();
                return false;
            }
        }
    }
}

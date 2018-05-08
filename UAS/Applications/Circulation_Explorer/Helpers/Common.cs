using System;
using System.Collections.Generic;
using System.Linq;

namespace Circulation_Explorer.Helpers
{
    public class Common
    {
        public static bool CheckResponse<T>(List<T> response, FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes status)
        {
            if (response != null && status == FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Success)
                return true;
            else
            {
                Core_AMS.Utilities.WPF.MessageServiceError();
                return false;
            }
        }
    }
}

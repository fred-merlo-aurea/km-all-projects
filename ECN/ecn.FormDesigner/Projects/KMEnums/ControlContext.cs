using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KMEnums
{
    public enum ControlContext
    {
        [EnumMember(Value = "Rules")]
        Rules,
        [EnumMember(Value = "Notifications")]
        Notifications,
        [EnumMember(Value = "Output")]
        Output,
        [EnumMember(Value = "Notification Templates")]
        NotificationTemplates
    }
}

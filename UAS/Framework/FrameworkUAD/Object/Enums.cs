using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.Object
{
    [Serializable]
    public partial class Enums
    {
        public enum Entity
        {
            Products,
            CustomField,
            Adhoc,
            PubCustomField,
            MasterGroup,
            MasterCodeSheet,
            ProductType,
            ResponseGroup,
            ProductCustomField,
            CodeSheet,
            Product,
            Subscription,
            AdhocCategory,
            ReportGroup
        }

        public enum Method
        {
            Validate,
            Save,
            Delete,
            None
        }

        public enum ExceptionLayer
        {
            Business,
            WebSite,
            API
        }

        public enum ErrorMessage
        {
            ValidationError,
            SecurityError,
            HardError,
            InvalidLink,
            PageNotFound,
            Timeout,
            Unknown
        }
    }
}

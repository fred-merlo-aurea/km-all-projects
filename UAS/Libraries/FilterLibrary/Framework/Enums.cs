using System;
using System.Linq;

namespace FilterControls.Framework
{
    public class Enums
    {
        public enum Filters
        {
            Standard,
            AdHoc,
            Demographic,
            Permission,
            Contact_Fields,
            Default
        }

        public enum AdHocType
        {
            AdHoc,
            Standard,
            Range
        }

        public enum FilterObjects
        {
            #region Standard
            CategoryCodeType,
            CategoryCode,
            TransactionCodeType,
            TransactionCode,
            QualificationSource,
            RegionCode,
            CountryRegions,
            Country,
            Responses,
            Year,
            QualificationDate,
            WaveMail,
            AdHoc,
            Media,
            #endregion
            #region Contact
            Email,
            Phone,
            Mobile,
            Fax,
            #endregion
            #region Permission
            MailPermission,
            FaxPermission,
            PhonePermission,
            OtherProductsPermission,
            ThirdPartyPermission,
            EmailRenewPermission
            #endregion
        }
    }
}

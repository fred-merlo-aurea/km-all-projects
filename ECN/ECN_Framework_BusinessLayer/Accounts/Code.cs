using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class Code
    {
        public static bool Exists()
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Accounts.Code.Exists();
                scope.Complete();
            }
            return exists;
        }

        public static List<ECN_Framework_Entities.Accounts.Code> GetAll()
        {
            List<ECN_Framework_Entities.Accounts.Code> codeList = ECN_Framework_DataLayer.Accounts.Code.GetAll();

            foreach (ECN_Framework_Entities.Accounts.Code code in codeList)
            {
                code.CodeTypeCode = GetCodeTypeCode(code.CodeType);
            }

            return codeList;
        }

        public static List<ECN_Framework_Entities.Accounts.Code> GetByCodeType(ECN_Framework_Common.Objects.Accounts.Enums.CodeType codeType, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Accounts.Code> lCode = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lCode = ECN_Framework_DataLayer.Accounts.Code.GetByCodeType(codeType);
                scope.Complete();
            }

            foreach (ECN_Framework_Entities.Accounts.Code code in lCode)
            {
                code.CodeTypeCode = GetCodeTypeCode(code.CodeType);
            } 

            return lCode;
        }


        private static ECN_Framework_Common.Objects.Accounts.Enums.CodeType GetCodeTypeCode(string codeType)
        {
            ECN_Framework_Common.Objects.Accounts.Enums.CodeType returnType;

            switch (codeType.Trim().ToLower())
            {
                case "ChannelType":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.CodeType.ChannelType;
                    break;
                case "ContentType":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.CodeType.ContentType;
                    break;
                case "CustomerSecurity":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.CodeType.CustomerSecurity;
                    break;
                case "CustomerType":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.CodeType.CustomerType;
                    break;
                case "FolderType":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.CodeType.FolderType;
                    break;
                case "FormatType":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.CodeType.FormatType;
                    break;
                case "LicenseType":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.CodeType.LicenseType;
                    break;
                case "OwnerType":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.CodeType.OwnerType;
                    break;
                case "ProfileType":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.CodeType.ProfileType;
                    break;
                case "SubscribeType":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.CodeType.SubscribeType;
                    break;
                case "TemplateStyle":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.CodeType.TemplateStyle;
                    break;
                case "TemplateType":
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.CodeType.TemplateType;
                    break;
                default:
                    returnType = ECN_Framework_Common.Objects.Accounts.Enums.CodeType.Unknown;
                    break;
            }
            return returnType;
        }

    }
}


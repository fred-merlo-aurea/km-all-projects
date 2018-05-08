using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_BusinessLayer.Creator
{
    [Serializable]
    public class Code
    {
        public static List<ECN_Framework_Entities.Creator.Code> GetByCodeType(string codeType, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Creator.Code> lCode = ECN_Framework_DataLayer.Creator.Code.GetByCodeType(codeType);

            foreach (ECN_Framework_Entities.Creator.Code code in lCode)
            {
                code.CodeTypeCode = GetCodeTypeCode(code.CodeType);
            }

            return lCode;
        }


        private static ECN_Framework_Common.Objects.Creator.Enums.CodeType GetCodeTypeCode(string codeType)
        {
            ECN_Framework_Common.Objects.Creator.Enums.CodeType returnType;

            switch (codeType.Trim().ToLower())
            {
                case "EventType":
                    returnType = ECN_Framework_Common.Objects.Creator.Enums.CodeType.EventType;
                    break;
                case "ItemType":
                    returnType = ECN_Framework_Common.Objects.Creator.Enums.CodeType.ItemType;
                    break;
                case "MediaFields":
                    returnType = ECN_Framework_Common.Objects.Creator.Enums.CodeType.MediaFields;
                    break;
                case "MediaType":
                    returnType = ECN_Framework_Common.Objects.Creator.Enums.CodeType.MediaType;
                    break;
                case "MenuType":
                    returnType = ECN_Framework_Common.Objects.Creator.Enums.CodeType.MenuType;
                    break;
                case "PageType":
                    returnType = ECN_Framework_Common.Objects.Creator.Enums.CodeType.PageType;
                    break;
                case "PointerType":
                    returnType = ECN_Framework_Common.Objects.Creator.Enums.CodeType.PointerType;
                    break;
                case "TemplateType":
                    returnType = ECN_Framework_Common.Objects.Creator.Enums.CodeType.TemplateType;
                    break;
                default:
                    returnType = ECN_Framework_Common.Objects.Creator.Enums.CodeType.Unknown;
                    break;
            }
            return returnType;
        }
    }
}

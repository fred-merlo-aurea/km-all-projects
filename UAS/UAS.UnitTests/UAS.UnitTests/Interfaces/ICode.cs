using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using FrameworkUAD_Lookup;
using FrameworkUAD_Lookup.Model;

namespace UAS.UnitTests.Interfaces
{
    public interface ICode
    {
        bool CodeExist(string codeName, int codeTypeID);
        bool CodeExist(string codeName, Enums.CodeType codeType);
        bool CodeValueExist(string codeValue, int codeTypeID);
        bool CodeValueExist(string codeValue, Enums.CodeType codeType);
        DataTable dtGetCode(Enums.CodeType codeType);
        List<SelectListItem> GetDropDownList(Enums.CodeType codeType);
        List<SelectListItem> GetDropDownList(Enums.CodeType parentCodeType, string parentCode);
        List<Operator> GetOperators();
        int Save(FrameworkUAD_Lookup.Entity.Code x);
        List<FrameworkUAD_Lookup.Entity.Code> Select();
        List<FrameworkUAD_Lookup.Entity.Code> Select(int codeTypeId);
        List<FrameworkUAD_Lookup.Entity.Code> Select(Enums.CodeType codeType);
        List<FrameworkUAD_Lookup.Entity.Code> SelectChildren(int parentCodeID);
        List<FrameworkUAD_Lookup.Entity.Code> SelectChildren(Enums.CodeType parentCodeType, string parentCode);
        FrameworkUAD_Lookup.Entity.Code SelectCodeId(int codeId);
        FrameworkUAD_Lookup.Entity.Code SelectCodeName(Enums.CodeType codeType, string codeName);
        FrameworkUAD_Lookup.Entity.Code SelectCodeValue(Enums.CodeType codeType, string codeValue);
        List<FrameworkUAD_Lookup.Entity.Code> SelectForDemographicAttribute(Enums.CodeType codeType, int dataCompareResultQueId, string ftpFolder);
    }
}
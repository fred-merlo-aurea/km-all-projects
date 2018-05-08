using System.Collections.Generic;

namespace UAS.UnitTests.Interfaces
{
    public interface ICategoryCode
    {
        bool Exists(int categoryCodeTypeID, int categoryCodeValue);
        int Save(FrameworkUAD_Lookup.Entity.CategoryCode x);
        List<FrameworkUAD_Lookup.Entity.CategoryCode> Select();
        FrameworkUAD_Lookup.Entity.CategoryCode Select(int categoryCodeTypeID, int categoryCodeValue);
        List<FrameworkUAD_Lookup.Entity.CategoryCode> SelectActiveIsFree(bool isFree);
    }
}
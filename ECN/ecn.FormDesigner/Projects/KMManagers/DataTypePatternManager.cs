using System;
using KMEnums;

namespace KMManagers
{
    public class DataTypePatternManager : ManagerBase
    {
        public string GetPatternByType(TextboxDataTypes type)
        {
            return DB.DataTypePatternDbManager.GetPatternByType(type);
        }
    }
}
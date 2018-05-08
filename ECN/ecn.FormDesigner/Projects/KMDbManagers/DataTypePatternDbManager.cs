using System;
using System.Linq;
using KMEntities;
using KMEnums;

namespace KMDbManagers
{
    public class DataTypePatternDbManager : DbManagerBase
    {
        public string GetPatternByType(TextboxDataTypes type)
        {
            DataTypePattern pattern = KM.DataTypePatterns.SingleOrDefault(x => x.DataTypePattern_Seq_ID == (int)type);

            return pattern == null ? null : pattern.Pattern;
        }
    }
}
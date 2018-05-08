using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KM.Common.Utilities.Enums.EnumAttributes
{
    public class ColumnValueAttribute : Attribute
    {
        public string ColumnStringValue { get; private set; }

        public ColumnValueAttribute(string columnValue)
        {
            ColumnStringValue = columnValue;
        }
    }
}

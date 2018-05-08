using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KM.Common.Utilities.Enums.EnumAttributes
{
    public class CodeValueAttribute : Attribute
    {
        public string CodeStringValue { get; private set; }
        public char CodeCharValue { get; private set; }

        public CodeValueAttribute(string codeValue)
        {
            CodeStringValue = codeValue;
        }

        public CodeValueAttribute(char codeValue)
        {
            CodeCharValue = codeValue;
        }
    }
}

using System.Collections.Generic;

namespace Core_AMS.Utilities
{
    public class LogicalOperators
    {
        public static Dictionary<string,string> GetOperators()
        {
            Dictionary<string, string> ops = new Dictionary<string, string>();
            ops.Add("multiply", "*");
            ops.Add("divide", "/");
            ops.Add("modulus", "%");
            ops.Add("add", "+");
            ops.Add("subtract", "-");
            ops.Add("greater than", ">");
            ops.Add("less than", "<");
            ops.Add("greater than or equal to", ">=");
            ops.Add("less than or equal to", "<=");
            ops.Add("is not less than", "!<");
            ops.Add("is not greater than", "!>");
            ops.Add("equal", "=");
            ops.Add("not equal", "!=");
            ops.Add("and", "and");
            ops.Add("or", "or");
            ops.Add("contains", "contains");
            ops.Add("starts with", "starts with");
            ops.Add("ends with", "ends with");
            ops.Add("in", "in");
            ops.Add("not in", "not in");
            return ops;
        }
    }
}

using System;
using CommonStringFunctions = KM.Common.StringFunctions;

namespace ecn.common.classes
{
	public class StringFunctions
	{
        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string Replace(string strText, string strFind, string strReplace)
		{
		    return CommonStringFunctions.Replace(strText, strFind, strReplace);
		}

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string DBString(object obj, string tacit)
		{
		    return CommonStringFunctions.DbTypeToString(obj, tacit);
		}

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string Remove(string strText, string removeSet)
		{
		    return CommonStringFunctions.Remove(strText, removeSet);
		}

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string NonDomain()
		{
		    return CommonStringFunctions.GetNonDomainCharacters();
		}
	}
}

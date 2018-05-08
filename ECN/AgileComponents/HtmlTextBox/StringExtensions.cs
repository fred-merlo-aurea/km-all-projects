// The namespace has been intentionately set to System, 
// in order for this extension method to automatically be visible in any class
// without having to add a "using" clause (which can be added only if you know about 
// the existence of this class).
namespace System
{
    public static class StringExtensions
    {
        // .NET 2.0 does not have such a method.
        public static bool IsNullOrWhiteSpace(this string value)
        {
            if (value == null)
            {
                return true;
            }

            for (var i = 0; i < value.Length; i++)
            {
                if (!Char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
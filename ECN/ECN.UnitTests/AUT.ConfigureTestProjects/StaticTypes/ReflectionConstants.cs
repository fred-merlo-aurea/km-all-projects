using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AUT.ConfigureTestProjects.StaticTypes
{
    [ExcludeFromCodeCoverage]
    public static class ReflectionConstants
    {
        public const BindingFlags NonPublicInstanceFlag = BindingFlags.Instance | BindingFlags.NonPublic;
        public const BindingFlags NonPublicStaticFlag = BindingFlags.Static | BindingFlags.NonPublic;
        public const BindingFlags InvokeMethodFlag = BindingFlags.InvokeMethod;
        public const BindingFlags PublicFlag = BindingFlags.Public;
        public const BindingFlags PublicInstanceFlag = BindingFlags.Public | BindingFlags.Instance;
        public const BindingFlags PublicStaticFlag = BindingFlags.Public | BindingFlags.Static;
        public const BindingFlags DeclaredNonInheritingMember = BindingFlags.DeclaredOnly;
    }
}
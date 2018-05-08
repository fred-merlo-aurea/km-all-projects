namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// .Net Framework 2.0 does not support extension method.
    /// With mimic of ExtenstionAttribute, compilation will succeed 
    /// even when targeting .NET 2.0 framework
    /// </summary>
    [AttributeUsage(AttributeTargets.Method |
                    AttributeTargets.Class | AttributeTargets.Assembly)]
    public sealed class ExtensionAttribute : Attribute
    {
    }
}

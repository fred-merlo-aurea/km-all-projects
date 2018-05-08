using KM.Common;
using static ECN_Framework_Common.Objects.Enums;

namespace ECN_Framework_Common.Objects
{
    // This class is inheriting from an abstract base class,
    // and it's purpose is to eliminate the genericity of the base class,
    // by allowing clients to call it from a non-generic context.
    public class QueryStringParameters : QueryStringParameter<ParameterTypes>
    {
    }
}

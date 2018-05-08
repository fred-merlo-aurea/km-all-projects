namespace KM.Common
{
    // This class is inheriting from an abstract base class,
    // and it's purpose is to eliminate the genericity of the base class,
    // by allowing clients to call it from a non-generic context.
    public class QueryStringParameters : QueryStringParameter<ECNParameterTypes>
    {
    }
}
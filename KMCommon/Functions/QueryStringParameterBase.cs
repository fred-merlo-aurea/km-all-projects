namespace KM.Common
{
    public abstract class QueryStringParameter<TEnum> 
        where TEnum : struct
    {
        public TEnum Parameter { get; set; }

        public string ParameterValue { get; set; }
    }
}
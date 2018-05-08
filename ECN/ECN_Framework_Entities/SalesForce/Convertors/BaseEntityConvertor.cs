using System;
using System.Collections.Generic;
using ECN_Framework_Common.Functions;
using ECN_Framework_Entities.Salesforce.Helpers;

namespace ECN_Framework_Entities.Salesforce.Convertors
{
    public abstract class EntityConverterBase
    {
        private Dictionary<string, string> _propertyNameMapping;
        private Dictionary<Type, Func<string, object>> _typeConverter;
        private Dictionary<string, Func<string, object>> _propertyNameConverter;

        public EntityConverterBase()
        {
            _propertyNameMapping = new Dictionary<string, string>();
            _typeConverter = new Dictionary<Type, Func<string, object>>();
            _propertyNameConverter = new Dictionary<string, Func<string, object>>();

            InitDefaultTypeConverters();
        }

        protected abstract string LastPropertyName { get; }

        public void AddPropertyMapping(string originalName, string mappedName)
        {
            _propertyNameMapping.Add(mappedName, originalName);
        }

        public void AddPropertyConverter(string propertyName, Func<string, object> action)
        {
            _propertyNameConverter.Add(propertyName, action);
        }

        public virtual IList<T> Convert<T>(IEnumerable<string> json) where T : new()
        {
            var list = new List<T>();
            var entity = new T();

            foreach (var element in json)
            {
                var parse = JsonFunctions.CleanJsonText(element);
                var propertyName = GetPropertyName(parse);
                string propertyValue = JsonFunctions.GetPropertyValue(parse);

                SetPropertyValue(entity, propertyName, propertyValue);
                if (propertyName == LastPropertyName)
                {
                    list.Add(entity);
                    entity = new T();
                }
            }

            return list;
        }

        private string GetPropertyName(string[] parse)
        {
            var name = JsonFunctions.GetPropertyName(parse);
            if (_propertyNameMapping.ContainsKey(name))
            {
                return _propertyNameMapping[name];
            }
            return name;
        }

        private void SetPropertyValue<T>(T obj, string propertyName, string value)
        {
            var property = PropertyHelper.GetProperty<T>(propertyName);
            if (property != null)
            {
                var convertedValue = Convert(propertyName, property.PropertyType, value);
                property.SetValue(obj, convertedValue);
            }
        }

        private object Convert(string propertyName, Type propertyType, string value)
        {
            var convert = _propertyNameConverter.ContainsKey(propertyName)
                ? _propertyNameConverter[propertyName]
                : _typeConverter[propertyType];

            return convert(value);
        }

        private void InitDefaultTypeConverters()
        {
            _typeConverter.Add(typeof(string), s => JsonFunctions.GetStringValue(s));
            _typeConverter.Add(typeof(bool), s => JsonFunctions.GetBooleanValue(s));
            _typeConverter.Add(typeof(double), s => JsonFunctions.GetDoubleValue(s));
            _typeConverter.Add(typeof(int), s => JsonFunctions.GetIntValue(s));
            _typeConverter.Add(typeof(DateTime), s => JsonFunctions.GetDateFromJson(s));
            _typeConverter.Add(typeof(decimal), s => System.Convert.ToDecimal(s));
        }
    }
}

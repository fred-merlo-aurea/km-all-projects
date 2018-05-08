using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Core.Utilities
{
    public class ConditionalDataContractSurrogate : IDataContractSurrogate
    {
        private readonly IDataContractSurrogate _baseSerializer;

        public ConditionalDataContractSurrogate(IDataContractSurrogate baseSerializer)
        {
            _baseSerializer = baseSerializer;
        }

        public Type GetDataContractType(Type type)
        {
            return _baseSerializer != null ? _baseSerializer.GetDataContractType(type) : type;
        }

        private bool IsVisible(string isClientServiceVisible)
        {
            bool isVisible = true;
            bool.TryParse(isClientServiceVisible, out isVisible);
            return isVisible;
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            if (obj == null) return null;

            var type = obj.GetType();
            type.GetProperties().ToList()
              .ForEach(prop =>
              {
                  try
                  {
                      var attr = prop.GetCustomAttributes(typeof(ConditionalDataMemberAttribute), false);
                      if (attr.Any())
                      {
                          bool isVisible = true;
                          if (((ConditionalDataMemberAttribute)attr[0]).IsClientServiceVisible != null)
                              isVisible = ((ConditionalDataMemberAttribute)attr[0]).IsClientServiceVisible;
                          //Is the user authorized
                          if (isVisible == true)
                          {
                              var proptype = prop.PropertyType;
                              prop.GetSetMethod().Invoke(obj,
                                                         new[] { proptype.IsValueType ? Activator.CreateInstance(proptype) : null });
                          }
                          else
                          {
                              prop = null;
                          }

                      }
                  }
                  catch { }
              });
            
            return _baseSerializer != null ? _baseSerializer.GetObjectToSerialize(obj, targetType) : obj;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            return _baseSerializer != null ? _baseSerializer.GetDeserializedObject(obj, targetType) : obj;
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            return _baseSerializer != null ? _baseSerializer.GetCustomDataToExport(memberInfo, dataContractType) : null;
        }

        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            return _baseSerializer != null ? _baseSerializer.GetCustomDataToExport(clrType, dataContractType) : null;
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
            if (_baseSerializer != null) _baseSerializer.GetKnownCustomDataTypes(customDataTypes);
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            return _baseSerializer != null ? _baseSerializer.GetReferencedTypeOnImport(typeName, typeNamespace, customData) : null;
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            return _baseSerializer != null ? _baseSerializer.ProcessImportedType(typeDeclaration, compileUnit) : typeDeclaration;
        }
    }
}

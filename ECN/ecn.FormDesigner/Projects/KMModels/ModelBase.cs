using System;
using System.Reflection;

namespace KMModels
{
    public abstract class ModelBase
    {
        public virtual void FillData(object entity)
        {
            if (entity != null)
            {
                Type T = entity.GetType();
                PropertyInfo[] fields = GetType().GetProperties();
                foreach (var f in fields)
                {
                    string name = f.Name;
                    Attribute attr = f.GetCustomAttribute(typeof(GetFromFieldAttribute));
                    if (attr != null)
                    {
                        name = ((GetFromFieldAttribute)attr).FieldName;
                    }

                    PropertyInfo pi = T.GetProperty(name);
                    if (pi != null)
                    {
                        object value = pi.GetValue(entity);
                        if (f.GetValue(this) == null || value != null)
                        {
                            if (f.PropertyType.IsEnum && value is bool)
                            {
                                value = (bool)value ? 1 : 0;
                            }
                            try
                            {
                                f.SetValue(this, value);
                            }
                            catch
                            { }
                        }
                    }
                }
            }
        }
    }
}
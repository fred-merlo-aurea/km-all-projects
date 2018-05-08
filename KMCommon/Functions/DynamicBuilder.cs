using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

namespace KM.Common
{
    [Serializable]
    public class DynamicBuilder<T>
    {
        protected static readonly MethodInfo getValueMethod =
            typeof(IDataRecord).GetMethod("get_Item", new Type[] { typeof(int) });
        protected static readonly MethodInfo isDBNullMethod =
            typeof(IDataRecord).GetMethod("IsDBNull", new Type[] { typeof(int) });
        private delegate T Load(IDataRecord dataRecord);
        private Load handler;
        private IDictionary<Type, Type> NullableTypes;
        public DynamicBuilderConfiguration Configuration { get; set; } = new DynamicBuilderConfiguration();

        protected DynamicBuilder()
        {
        }
        private void InitNullableFields()
        {
            NullableTypes = new Dictionary<Type, Type>
            {
                [typeof(bool)] = typeof(bool?),
                [typeof(byte)] = typeof(byte?),
                [typeof(DateTime)] = typeof(DateTime?),
                [typeof(decimal)] = typeof(decimal?),
                [typeof(double)] = typeof(double?),
                [typeof(float)] = typeof(float?),
                [typeof(Guid)] = typeof(Guid?),
                [typeof(short)] = typeof(short?),
                [typeof(int)] = typeof(int?),
                [typeof(long)] = typeof(long?),
            };
            if (Configuration.TimeSpanFieldNullable)
            {
                NullableTypes.Add(typeof(TimeSpan), typeof(TimeSpan?));
            }
        }

        public T Build(IDataRecord dataRecord)
        {
            return handler(dataRecord);
        }

        public static DynamicBuilder<T> CreateBuilder(IDataRecord dataRecord)
        {
            var dynamicBuilder = new DynamicBuilder<T>();
            InitBuilder(dynamicBuilder, dataRecord);
            return dynamicBuilder;
        }

        public static DynamicBuilder<T> CreateBuilder(IDataRecord dataRecord, DynamicBuilderConfiguration config = null)
        {
            var dynamicBuilder = new DynamicBuilder<T>();
            InitBuilder(dynamicBuilder, dataRecord, config);
            return dynamicBuilder;
        }

        public static void InitBuilder(DynamicBuilder<T> dynamicBuilder, IDataRecord dataRecord, DynamicBuilderConfiguration config = null)
        {
            if (dataRecord == null)
            {
                throw new ArgumentNullException(nameof(dataRecord));
            }

            if (config != null)
            {
                dynamicBuilder.Configuration = config;
            }
            dynamicBuilder.InitNullableFields();
            dynamicBuilder.ProcessBuild(dataRecord);
        }


        private void ProcessBuild(IDataRecord dataRecord)
        {
            var method =
                new DynamicMethod("DynamicCreate", typeof(T), new Type[] { typeof(IDataRecord) }, typeof(T), true);
            var generator = method.GetILGenerator();

            var result = generator.DeclareLocal(typeof(T));
            generator.Emit(OpCodes.Newobj, typeof(T).GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);

            for (var i = 0; i < dataRecord.FieldCount; i++)
            {
                ProcessForEachFieldAction(dataRecord, generator, i, result);
            }

            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);

            handler = (Load)method.CreateDelegate(typeof(Load));
        }
        private void ProcessForEachFieldAction(IDataRecord dataRecord, ILGenerator generator, int index, LocalBuilder result)
        {
            var recordName = dataRecord.GetName(index);

            var propertyInfo = typeof(T).GetProperty(recordName, BindingFlags.SetProperty | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            var endIfLabel = generator.DefineLabel();

            var isNullable = false;

            if (propertyInfo != null)
            {
                if ((propertyInfo.PropertyType.Name.IndexOf("nullable", StringComparison.OrdinalIgnoreCase)) >= 0)
                {
                    isNullable = true;
                }

                if (propertyInfo.GetSetMethod() != null)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, index);
                    generator.Emit(OpCodes.Callvirt, isDBNullMethod);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);

                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, index);
                    generator.Emit(OpCodes.Callvirt, getValueMethod);

                    Type _type = dataRecord.GetFieldType(index);

                    if (isNullable)
                    {
                        generator.Emit(OpCodes.Unbox_Any, GetNullableType(_type));
                    }
                    else
                    {
                        generator.Emit(OpCodes.Unbox_Any, _type);
                    }

                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());

                    generator.MarkLabel(endIfLabel);
                }
            }
        }

        private Type GetNullableType(Type type)
        {
            Type result;
            NullableTypes.TryGetValue(type, out result);
            return result;
        }
    }

    public class DynamicBuilderConfiguration
    {
        public bool TimeSpanFieldNullable { get; set; }
    }
}

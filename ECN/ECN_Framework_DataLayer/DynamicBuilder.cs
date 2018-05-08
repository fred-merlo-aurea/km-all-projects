using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using ECN_Framework_Common.Functions;
using KM.Common;

namespace ECN_Framework_DataLayer
{
    [Serializable]
    public class DynamicBuilder<T>: KM.Common.DynamicBuilder<T>
    {
        public static DynamicBuilder<T> CreateBuilder(IDataRecord dataRecord)
        {
            DynamicBuilder<T> dynamicBuilder = new DynamicBuilder<T>();
            DynamicBuilderConfiguration config = new DynamicBuilderConfiguration();
            config.TimeSpanFieldNullable = true;
            dynamicBuilder.Configuration = config;
            DynamicBuilder<T>.InitBuilder(dynamicBuilder, dataRecord);
            return dynamicBuilder;
        }

    }
}

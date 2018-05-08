using System;
using System.Collections.Generic;
using FrameworkSubGen.Entity;

namespace UAS.UnitTests.FrameworkSubGen.DataAccess
{
    public interface IBulkDataInsertTestData<T> where T : IEntity
    {
        string TestClassName { get; }

        string TestMethodName { get; }

        string TableName { get; }

        IEnumerable<string> ColumnNames { get; }

        Action<IEnumerable<T>, int> TestAction { get; }

        T CreateEntity();
    }
}
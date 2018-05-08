using System;
using System.Collections.Generic;
using FrameworkSubGen.Entity;

namespace UAS.UnitTests.FrameworkSubGen.DataAccess
{
    public class BulkDataInsertTestData<T> : IBulkDataInsertTestData<IEntity> where T : IEntity, new()
    {
        public BulkDataInsertTestData(string tableName, 
                                  IEnumerable<string> columnNames, 
                                  Action<IEnumerable<IEntity>, int> testAction, 
                                  string testClass = BulkDataInsertTest.BulkDataInsertClassName,
                                  string testMethodName = null)
        {
            TableName = tableName;
            ColumnNames = columnNames;
            TestAction = testAction;
            TestClassName = testClass;

            TestMethodName = testMethodName ?? tableName;
        }

        public string TestClassName { get; }

        public string TestMethodName { get; }

        public string TableName { get; }

        public IEnumerable<string> ColumnNames { get; }

        public Action<IEnumerable<IEntity>, int> TestAction { get; }

        public IEntity CreateEntity()
        {
            return new T() { account_id = BulkDataInsertTest.AccountId };
        }
    }
}
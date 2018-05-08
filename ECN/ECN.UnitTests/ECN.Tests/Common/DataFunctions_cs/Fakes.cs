using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient.Fakes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.common.classes;
using Microsoft.QualityTools.Testing.Fakes;

namespace ECN.Tests.Common.DataFunctions_cs
{
    public class Fakes
    {
        protected static readonly int SqlSuccessCode = 0;

        protected static readonly string TableName = "spresult";

        protected const string DbAccounts = "accounts";
        protected const string DbCollector = "collector";
        protected const string DbCreator = "creator";
        protected const string DbCommunicator = "communicator";
        protected const string DbCharity = "charity";
        protected const string DbPublisher = "publisher";
        protected const string DbMisc = "misc";
        protected const string DbActivity = "activity";

        private const string ConnStrDataSource = @"Server=server;Database=";
        private const string ConnStrDataSourcePostfix = @";Trusted_Connection=True;";

        protected const string ConnStrConnStr = ConnStrDataSource + "connString" + ConnStrDataSourcePostfix;
        protected const string ConnStrAccountsdb = ConnStrDataSource + "accountsdb" + ConnStrDataSourcePostfix;
        protected const string ConnStrCommunicatordb = ConnStrDataSource + "communicatordb" + ConnStrDataSourcePostfix;
        protected const string ConnStrConCreator = ConnStrDataSource + "cre" + ConnStrDataSourcePostfix;
        protected const string ConnStrConCollector = ConnStrDataSource + "col" + ConnStrDataSourcePostfix;
        protected const string ConnStrConCommunicator = ConnStrDataSource + "com" + ConnStrDataSourcePostfix;
        protected const string ConnStrConAccounts = ConnStrDataSource + "act" + ConnStrDataSourcePostfix;
        protected const string ConnStrConCharity = ConnStrDataSource + "chr" + ConnStrDataSourcePostfix;
        protected const string ConnStrConPublisher = ConnStrDataSource + "pub" + ConnStrDataSourcePostfix;
        protected const string ConnStrConMisc = ConnStrDataSource + "misc" + ConnStrDataSourcePostfix;
        protected const string ConnStrConActivity = ConnStrDataSource + "activity" + ConnStrDataSourcePostfix;
        protected const string ConnStrProductName = ConnStrDataSource + "product_name" + ConnStrDataSourcePostfix;

        private IDisposable _shims;

        protected bool _connOpened;
        protected bool _connClosed;

        protected void SetupFakes()
        {
            DataFunctions.connStr = ConnStrConnStr;
            DataFunctions.accountsdb = ConnStrAccountsdb;
            DataFunctions.communicatordb = ConnStrCommunicatordb;
            DataFunctions.con_creator = ConnStrConCreator;
            DataFunctions.con_collector = ConnStrConCollector;
            DataFunctions.con_communicator = ConnStrConCommunicator;
            DataFunctions.con_accounts = ConnStrConAccounts;
            DataFunctions.con_charity = ConnStrConCharity;
            DataFunctions.con_publisher = ConnStrConPublisher;
            DataFunctions.con_misc = ConnStrConMisc;
            DataFunctions.con_activity = ConnStrConActivity;
            DataFunctions.product_name = ConnStrProductName;

            _shims = ShimsContext.Create();

            ShimSqlConnection.AllInstances.Open = connection => { _connOpened = true; };
            ShimSqlConnection.AllInstances.Close = connection => { _connClosed = true; };
        }

        protected void ReleaseFakes()
        {
            _shims?.Dispose();
        }
    }
}

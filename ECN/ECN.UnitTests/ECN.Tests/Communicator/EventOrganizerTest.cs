using System;
using System.Diagnostics.CodeAnalysis;
using System.Data;
using System.Configuration.Fakes;
using System.Collections.Specialized;
using NUnit.Framework;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ecn.common.classes.Fakes;
using ecn.communicator.classes.Fakes;
using ecn.communicator.classes;

namespace ECN.Tests.Communicator
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class EventOrganizerTest
    {
        private IDisposable _shimObject;
        private PrivateObject _testedClass;
        private DataTable _blastDataTable;
        private string _eventType;
        private const string AssemblyName = "ECN";
        private const string ClassName = "ecn.communicator.classes.EventOrganizer";
        private const string LayoutStatusValue = "Y";
        private const string DummyValue = "DummyValue";
        private const string DummyNumericValue = "0";
        private const string EventTypeUnsubscribe = "unsubscribe";
        private const string EventTypeSubscribe = "subscribe";
        private const string EmptyValue = "";
        private const string DummyEmailAddress = "dummy.email@email.com";
        private const string BlastIDColumn = "BlastID";
        private const string PeriodColumn = "Period";
        private const string TriggerPlanIDColumn = "TriggerPlanID";
        private const string ConnectionStringKey = "com";
        private const int LayoutPlanID = 1;
        private const double PeriodValue = 7;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _testedClass = new PrivateObject(AssemblyName, ClassName);
            _eventType = String.Empty;
            _blastDataTable = new DataTable();
            _blastDataTable.Columns.Add(BlastIDColumn);
            _blastDataTable.Columns.Add(PeriodColumn);
            _blastDataTable.Columns.Add(TriggerPlanIDColumn);
            _blastDataTable.Rows.Add(BlastIDColumn, PeriodValue, TriggerPlanIDColumn);

            ShimGroups.Constructor =
                (groups) =>
                {
                    var shimGroups = new ShimGroups(groups)
                    {
                        WhatEmailString =
                        (email) =>
                        {
                            return new Emails();
                        }
                    };
                };

            ShimEmails.Constructor =
                (emails) =>
                {
                    var shimEmails = new ShimEmails(emails)
                    {
                        EmailAddress = () => DummyEmailAddress
                    };
                };

            ShimBlasts.Constructor =
                (blasts) =>
                {
                    var shimBlast = new ShimBlasts(blasts);
                };

            ShimLayoutPlans.ConstructorInt32 =
                (layoutPlan,id) =>
                {
                    var shimLayout = new ShimLayoutPlans(layoutPlan);                   
                    shimLayout.StatusGet = () => LayoutStatusValue;
                    shimLayout.Blast = () => new Blasts();
                    shimLayout.EventType = () => _eventType;
                    shimLayout.Criteria = () => EmptyValue;
                    shimLayout.Group = () => new Groups();
                    shimLayout.Period = () => PeriodValue;
                };

            ShimDatabaseAccessor.AllInstances.ID = (id) => LayoutPlanID;

            ShimEmailActivityLog.Constructor =
                (logEmail) =>
                {
                    var shimLog = new ShimEmailActivityLog(logEmail)
                    {
                        ActionValue = () => DummyValue,
                        Email = () => new Emails()
                    };
                };

            ShimDataFunctions.ExecuteScalarStringString = (sqlArgs, sql) => DummyNumericValue;
            ShimDataFunctions.GetDataTableStringString = (sqlArgs, sqlCmd) => _blastDataTable;

            ShimConfigurationManager.AppSettingsGet =
               () => new NameValueCollection
               {
                    {ConnectionStringKey,  ConnectionStringKey }
               };            
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }
    }
}

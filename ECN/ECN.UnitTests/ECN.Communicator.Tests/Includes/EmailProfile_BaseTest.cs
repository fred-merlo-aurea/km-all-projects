using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using ecn.communicator.includes;
using ecn.communicator.listsmanager.Fakes;
using ECN.Tests.Helpers;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Includes
{
    /// <summary>
    /// Unit Tests for <see cref="emailProfile_base.UpdateEmail"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class EmailProfile_BaseTest : BasePageTests
    {
        private IDisposable _context;
        private emailProfile_base _page;
        private SqlCommand _updateCommand;
        private SqlCommand _emailGrpUpdateCommand;

        [SetUp]
        public void SetUp()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(DefaultCulture);
            _context = ShimsContext.Create();
            _page = new emailProfile_base();
            _privateObject = new PrivateObject(_page);

            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        private void SetupFakes()
        {
            Shimemaileditor.AllInstances.isEmailSuppressedString = (_, email) =>
            {
                email.ShouldBe(EmailAddress);
                return false;
            };

            SetupSqlFakes();
        }

        private void SetupSqlFakes()
        {
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection
            {
                [ConnString] = TestConnectionString
            };

            ShimSqlConnection.ConstructorString = (_, __) => { };
            ShimSqlCommand.ConstructorStringSqlConnection = (_, __, ___) => { };

            var connection = new ShimSqlConnection
            {
                Open = () => { },
                Close = () => { }
            };

            ShimSqlCommand.AllInstances.ConnectionGet = _ => connection.Instance;

            ShimSqlCommand.AllInstances.ExecuteNonQuery = command =>
            {
                if (command.CommandText.StartsWith("update emails", StringComparison.OrdinalIgnoreCase))
                    {
                    _updateCommand = command;
                }
                else if(command.CommandText.StartsWith("update emailgroups", StringComparison.OrdinalIgnoreCase))
                {
                    _emailGrpUpdateCommand = command;
                }

                return -1;
            };
        }
    }
}
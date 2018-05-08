using System.Diagnostics.CodeAnalysis;
using ecn.communicator.classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class FilterTest
    {
        private PrivateType _privateFilterType;

        [SetUp]
        public void SetUp()
        {
            _privateFilterType = new PrivateType(typeof(Filter));
        }

        private static string[] NonCommonSwitchCaseConstants => new string[]
        {
            "Birthdate",
            "UserEvent1Date",
            "UserEvent2Date",
            "CreatedOn",
            "LastChanged",
        };

        private static string[] CommonSwitchCaseConstants() => new string[]
        {
            "EmailAddress",
            "FormatTypeCode",
            "SubscribeTypeCode",
            "Title",
            "FirstName",
            "LastName",
            "FullName",
            "Company",
            "Occupation",
            "Address",
            "Address2",
            "City",
            "State",
            "Zip",
            "Country",
            "Voice",
            "Mobile",
            "Fax",
            "Website",
            "Age",
            "Income",
            "Gender",
            "UserEvent1",
            "UserEvent2",
            "Notes"
        };
    }
}
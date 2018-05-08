using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using ecn.communicator.main.Salesforce.Entity;
using ECN.Communicator.Tests.Helpers;
using Moq;
using NUnit.Framework;
using static ecn.communicator.main.Salesforce.Entity.SF_Utilities;

namespace ECN.Communicator.Tests.Main.Salesforce.Entity
{
    [TestFixture]
    public partial class SF_UtilitiesTest
    {
        [Test]
        public void GetStateAbbr_NotFoundState_ReturnString()
        {
            // Arrange
            var state = "Teststing";

            // Act
            var actualRes = GetStateAbbr(state);

            // Assert
            StringAssert.Contains(state, actualRes);
        }

        [Test]
        [TestCase("alabama", "AL")]
        [TestCase("alaska", "AK")]
        [TestCase("new brunswick", "NB")]
        [TestCase("ontario", "ON")]
        [TestCase("alberta", "AB")]
        [TestCase("quebec", "QC")]
        [TestCase("manitoba", "MB")]
        [TestCase("Washington", "WA")]
        [TestCase("texas", "TX")]
        [TestCase("tennessee", "TN")]
        [TestCase("arizona", "AZ")]
        [TestCase("arkansas", "AR")]
        [TestCase("california", "CA")]
        [TestCase("colorado", "CO")]
        [TestCase("connecticut", "CT")]
        [TestCase("delaware", "DE")]
        [TestCase("florida", "FL")]
        [TestCase("georgia", "FA")]
        [TestCase("hawaii", "HI")]
        [TestCase("idaho", "ID")]
        [TestCase("illinois", "IL")]
        [TestCase("indiana", "IN")]
        [TestCase("iowa", "IA")]
        [TestCase("kansas", "KS")]
        [TestCase("kentucky", "KY")]
        [TestCase("louisiana", "LA")]
        [TestCase("maine", "ME")]
        [TestCase("maryland", "MD")]
        [TestCase("massachusetts", "MA")]
        [TestCase("michigan", "MI")]
        [TestCase("minnesota", "MN")]
        [TestCase("mississippi", "MS")]
        [TestCase("missouri", "MO")]
        [TestCase("montana", "MT")]
        [TestCase("nebraska", "NE")]
        [TestCase("nevada", "NV")]
        [TestCase("new hampshire", "NH")]
        [TestCase("new jersey", "NJ")]
        [TestCase("new mexico", "NM")]
        [TestCase("new york", "NY")]
        [TestCase("north carolina", "NC")]
        [TestCase("north dakota", "ND")]
        [TestCase("ohio", "OH")]
        [TestCase("oklahoma", "OK")]
        [TestCase("oregon", "OR")]
        [TestCase("pennsylvania", "PA")]
        [TestCase("rhode island", "RI")]
        [TestCase("south carolina", "SC")]
        [TestCase("south dakota", "SD")]
        [TestCase("utah", "UT")]
        [TestCase("vermont", "VT")]
        [TestCase("virginia", "VA")]
        [TestCase("west virginia", "WV")]
        [TestCase("wisconsin", "WI")]
        [TestCase("wyoming", "WY")]
        [TestCase("british columbia", "BC")]
        [TestCase("newfoundland and labrador", "NL")]
        [TestCase("nova scotia", "NS")]
        [TestCase("northwest territories", "NT")]
        [TestCase("prince edward island", "PE")]
        [TestCase("saskatchewan", "SK")]
        [TestCase("yukon", "YT")]
        [TestCase("nunavut", "NU")]
        public void GetStateAbbr_ValidCall_ReturnString(string state, string stateAbbr)
        {
            // Arrange - Act
            var actualRes = GetStateAbbr(state);

            // Assert
            StringAssert.Contains(stateAbbr, actualRes);
        }     
    }
}

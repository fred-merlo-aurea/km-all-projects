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
        public void InsertResult_Success_ValidCall_ReturnTrue()
        {
            // Arrange
            var resp = "{ success: true}";

            // Act
            var actualRes = InsertResult.Success(resp);

            // Assert
            Assert.IsTrue(actualRes);
        }

        [Test]
        public void InsertResult_SuccessId_ValidCall_ReturnTrue()
        {
            // Arrange
            var successId = "1";
            var resp = $"{{ id: {successId}}}";

            // Act
            var actualRes = InsertResult.SuccessId(resp);

            // Assert
            Assert.AreEqual(successId, actualRes);
        }
    }
}

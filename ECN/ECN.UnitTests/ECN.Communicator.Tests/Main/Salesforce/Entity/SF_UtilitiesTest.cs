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
    [ExcludeFromCodeCoverage]
    public partial class SF_UtilitiesTest
    {
        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = MockHelpers.FakeHttpContext();
        }

        [Test]
        public void GetInstance_ValidCall_ReturnString()
        {
            // Arrange
            HttpContext.Current.Session["SF_Token"] = "TestString";
            var instanceUrl = "TestString";
            SF_Authentication.Token = new SF_Token()
            {
                instance_url = instanceUrl
            };

            // Act
            var actualRes = GetInstance();
            
            // Assert
            Assert.AreEqual(instanceUrl, actualRes);
        }
        
        [Test]
        public void PrepareBulkOperation_ValidCall_ReturnString()
        {
            // Arrange
            var instanceUrl = "TestString";
            SF_Authentication.Token = new SF_Token()
            {
                instance_url = instanceUrl
            };

            // Act
            var actualRes = PrepareBulkOperation();

            // Assert
            StringAssert.Contains(instanceUrl, actualRes);
        }

        [Test]
        public void PrepareBatchAdd_ValidCall_ReturnString()
        {
            // Arrange
            var instanceUrl = "TestString";
            SF_Authentication.Token = new SF_Token()
            {
                instance_url = instanceUrl
            };
            var jobId = "1";

            // Act
            var actualRes = PrepareBatchAdd(jobId);

            // Assert
            StringAssert.Contains(instanceUrl, actualRes);
        }

        [Test]
        public void PrepareInsert_ValidCall_ReturnString()
        {
            // Arrange
            var instanceUrl = "TestString";
            SF_Authentication.Token = new SF_Token()
            {
                instance_url = instanceUrl
            };
            var sfObj = new SFObject();

            // Act
            var actualRes = PrepareInsert(sfObj);

            // Assert
            StringAssert.Contains(instanceUrl, actualRes);
        }

        [Test]
        public void PrepareUpdate_ValidCall_ReturnString()
        {
            // Arrange
            var instanceUrl = "TestString";
            SF_Authentication.Token = new SF_Token()
            {
                instance_url = instanceUrl
            };
            var sfObj = new SFObject();
            var objId = "1";

            // Act
            var actualRes = PrepareUpdate(sfObj, objId);
            
            // Assert
            StringAssert.Contains(instanceUrl, actualRes);
        }

        [Test]
        public void PrepareQuery_ValidCall_ReturnString()
        {
            // Arrange
            var instanceUrl = "TestString";
            SF_Authentication.Token = new SF_Token()
            {
                instance_url = instanceUrl
            };
            var query = "1";

            // Act
            var actualRes = PrepareQuery(query);
            
            // Assert
            StringAssert.Contains(instanceUrl, actualRes);
        }

        [Test]
        public void GetXMLForMasterSuppressJob_OnValidCall_ReturnString()
        {
            // Arrange
            var key = "TestKey";
            var value = "TestValue";
            var masterSuppress = new Dictionary<string, string>();
            masterSuppress.Add(key, value);

            // Act
            var actualRes = GetXMLForMasterSuppressJob(masterSuppress);

            // Assert
            StringAssert.Contains(key, actualRes);
        }

        [Test]
        public void GetXMLForOptOutJob__OnValidCall_ReturnString()
        {
            // Arrange
            var key = "TestKey";
            var value = "TestValue";
            var optOut = new Dictionary<string, string>();
            optOut.Add(key, value);

            // Act
            var actualRes = GetXMLForOptOutJob(optOut);

            // Assert
            StringAssert.Contains(key, actualRes);
        }

        [Test]
        public void SelectAllQuery_OnValidCall_ReturnString()
        {
            // Arrange
            var type = typeof(int);
            var strAssert = "%20FROM%20";

            // Act
            var actualRes = SelectAllQuery(type);

            // Assert
            StringAssert.Contains(strAssert, actualRes);
        }

        [Test]
        public void SelectWhere_OnValidCall_ReturnString()
        {
            // Arrange
            var type = typeof(int);
            var strAssert = "%20FROM%20";
            var where = "TestWhere";

            // Act
            var actualRes = SelectWhere(type, where);

            // Assert
            StringAssert.Contains(strAssert, actualRes);
            StringAssert.Contains(where, actualRes);
        }

        [Test]
        public void SelectWhere_EmptyWhere_ReturnString()
        {
            // Arrange
            var type = typeof(int);
            var strAssert = "%20FROM%20";
            var where = string.Empty;

            // Act
            var actualRes = SelectWhere(type, where);

            // Assert
            StringAssert.Contains(strAssert, actualRes);
        }

        [Test]
        public void ExcludedCharacters_ValidCall_ReturnListOfString()
        {
            // Arrange
            var expectedComma = ",";
            var expectedSemiColumn = ";";

            // Act
            var actualRes = ExcludedCharacters();

            // Assert
            Assert.IsTrue(actualRes.Contains(expectedComma));
            Assert.IsTrue(actualRes.Contains(expectedSemiColumn));
        }

        [Test]
        public void CleanStringSqlInjection_ValidCall_ReturnString()
        {
            // Arrange
            var cleanString = "TestString";
            var CommaString = "TestString,";

            // Act
            var actualRes = CleanStringSqlInjection(CommaString);

            // Assert
            Assert.AreEqual(cleanString, actualRes);
        }

        [Test]
        public void CleanStringSqlInjection_Empty_ReturnEmptyString()
        {
            // Arrange
            var cleanString = string.Empty;
            var CommaString = string.Empty;

            // Act
            var actualRes = CleanStringSqlInjection(CommaString);

            // Assert
            Assert.AreEqual(cleanString, actualRes);
        }
    }
}

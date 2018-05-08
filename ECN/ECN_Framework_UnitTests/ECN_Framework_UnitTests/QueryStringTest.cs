using KM.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ECN_Framework_UnitTests
{
    
    
    /// <summary>
    ///This is a test class for QueryStringTest and is intended
    ///to contain all QueryStringTest Unit Tests
    ///</summary>
    [TestClass()]
    public class QueryStringTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ParseEncryptedSSOQuerystring
        ///</summary>
        [TestMethod()]
        public void ParseEncryptedSSOQuerystringTest()
        {
            string encryptedQueryString = "XoOjli%2fUVVjAEnzC7fRbEcllO%2fQ7ZT2hVtCrwuH8xVQShOpBBl%2bt%2btOJyS0rafbV";
            int applicationID = 34; 
            string userNameValue = string.Empty; 
            string userNameValueNotExpected = string.Empty; 
            string passwordValue = string.Empty; 
            string passwordValueNotExpected = string.Empty; 
            QueryString.ParseEncryptedSSOQuerystring(encryptedQueryString, applicationID, out userNameValue, out passwordValue);
            Assert.IsFalse(string.IsNullOrEmpty(userNameValue));
            Assert.IsFalse(string.IsNullOrEmpty(passwordValue));
        }
    }
}

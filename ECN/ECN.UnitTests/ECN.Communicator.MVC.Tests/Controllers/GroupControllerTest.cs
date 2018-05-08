using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using ecn.communicator.mvc.Controllers;

namespace ECN.Communicator.MVC.Tests
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.mvc.Controllers.GroupController"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class GroupControllerTest
    {
        private const string UpdatedMessage = " rows updated/inserted";
        private const string TotalRecordsMessage = "Total Records in the File</td><td>";
        private const string NewRecordsMessage = "New</td><td>";
        private const string ChangedMessage = "Changed</td><td>";
        private const string DublicateMessage = "Duplicate(s)</td><td>";
        private const string SkippedMessage = "Skipped</td><td>";
        private const string TotalRecordKeyT = "T";
        private const string NewRecordKeyI = "I";
        private const string ChangedRecordKeyU = "U";
        private const string DublicateRecordKeyD = "D";
        private const string SkippedRecordKeyS = "S";

        private IDisposable _shimObject;
        private PrivateObject _testObject;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            InitPrivateObject();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        private void InitPrivateObject()
        {
            _testObject = new PrivateObject(new GroupController());
            _testObject.SetFieldOrProperty("_currentUser", new KMPlatform.Entity.User());
        }
    }
}

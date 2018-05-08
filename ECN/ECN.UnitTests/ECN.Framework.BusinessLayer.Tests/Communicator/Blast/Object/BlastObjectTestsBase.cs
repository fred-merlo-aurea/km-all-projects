using System;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    public abstract class BlastObjectTestsBase<T> where T : class, new()
    {
        protected internal const int BlastId = 1;
        protected internal const int CustomerId = 2;
        protected internal const int LayoutId = 3;
        protected internal const int GroupId = 4;
        protected internal const int CreatedUserId = 11;
        protected internal const int UpdatedUserId = 12;
        protected internal const string EmailFrom = "sender@example.com";
        protected internal const string EmailFromName = "Sender";
        protected internal const string EmailSubject = "Subject";
        protected internal const string ReplyTo = "replyto@example.com";
        protected internal const string TestBlastValueN = "N";
        protected internal const string TestBlastValueX = "X";
        protected internal const string EmailFromErrorMessage = "EmailFrom cannot be empty";
        protected internal const string EmailFromNameErrorMessage = "EmailFromName cannot be empty";
        protected internal const string StatusCodeErrorMessage = "StatusCode is invalid";
        protected internal const string BlastTypeErrorMessage = "BlastType is invalid";
        protected internal const string LayoutErrorMessage = "LayoutID is invalid";
        protected internal const string ReplyToErrorMessage = "ReplyTo cannot by empty";
        protected internal const string TestBlastErrorMessage = "TestBlast is invalid";
        protected internal const string EmailSubjectErrorMessage = "EmailSubject is invalid";

        protected T _testEntity;
        protected PrivateObject _testEntityPrivate;
        protected IDisposable _shimsContext;

        [SetUp]
        public virtual void SetUp()
        {
            _shimsContext = ShimsContext.Create();
            _testEntity = new T();
            _testEntityPrivate = new PrivateObject(_testEntity);
        }

        [TearDown]
        public virtual void TearDown()
        {
            _shimsContext.Dispose();
        }
    }
}

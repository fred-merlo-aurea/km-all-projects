using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using ECN_Framework_BusinessLayer.Communicator;
using Entities = ECN_Framework_Entities.Communicator;
using EntitiesFakes = ECN_Framework_Entities.Communicator.Fakes;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using KMPlatform.Entity;
using ECN.TestHelpers;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class BlastTest
    {
        private const string _MethodHTMLReplaceWithLinkID = "HTMLReplaceWithLinkID";

        [Test]
        public void HTMLReplaceWithLinkID_OnEmptyString_ReturnEmptyString()
        {
            // Arrange
            var strText = string.Empty;
            var blastId = 1;

            // Act	
            var actualResult = ReflectionHelper.CallMethod(typeof(Blast),
                _MethodHTMLReplaceWithLinkID,
                new object[] { strText, blastId }) as string;

            // Assert
            actualResult.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(-1, -1)]
        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        public void HTMLReplaceWithLinkID_OnNonEmptyString_ReturnString(int blastLinkID, int blastLinkIDWithECNID)
        {
            // Arrange
            var strText = 
                $"<a href=\"http://www.google.com\" &l=\"Test\" ecn_id=\"{Guid.NewGuid()}\">Test</a>";
            var blastId = 1;
            ShimBlastLink.GetByLinkURLInt32String = (id, url) =>
            {
                return new Entities::BlastLink
                {
                    BlastLinkID = blastLinkID
                };
            };
            ShimBlastLink.GetByLinkURL_ECNIDInt32StringString = (id, url, guid) =>
            {
                return new Entities::BlastLink
                {
                    BlastLinkID = blastLinkIDWithECNID
                };
            };
            ShimUniqueLink.GetByBlastID_UniqueIDInt32String= (id, guid)=>
            {
                return new Entities::UniqueLink
                {
                    UniqueID = "TestID"
                };
            };
            ShimUniqueLink.SaveUniqueLink = (link) => 1;
            ShimBlastLink.InsertBlastLink = (link) => 1;

            // Act	
            var actualResult = ReflectionHelper.CallMethod(typeof(Blast),
                _MethodHTMLReplaceWithLinkID,
                new object[] { strText, blastId }) as string;

            // Assert
            actualResult.ShouldNotBeNullOrWhiteSpace();
        }

        [Test]
        public void HTMLReplaceWithLinkID_ECNIDIsEmpty_ReturnString()
        {
            // Arrange
            var strText =
                $"<a href=\"http://www.google.com\" &l=\"Test\">Test</a>";
            var blastId = 1;
            var blastLink = new Entities::BlastLink
            {
                BlastLinkID = 1
            };
            ShimBlastLink.GetByLinkURLInt32String = (id, url) => blastLink;
            ShimBlastLink.GetByLinkURL_ECNIDInt32StringString = (id, url, guid) => null;
            ShimUniqueLink.GetByBlastID_UniqueIDInt32String = (id, guid) =>
            {
                return new Entities::UniqueLink
                {
                    UniqueID = Guid.NewGuid().ToString()
                };
            };
            ShimUniqueLink.SaveUniqueLink = (link) => 1;
            ShimBlastLink.InsertBlastLink = (link) => 1;

            // Act	
            var actualResult = ReflectionHelper.CallMethod(typeof(Blast),
                _MethodHTMLReplaceWithLinkID,
                new object[] { strText, blastId }) as string;

            // Assert
            actualResult.ShouldNotBeNullOrWhiteSpace();
        }

        [Test]
        public void HTMLReplaceWithLinkID_LinkGuidFound_ReturnString()
        {
            // Arrange
            var strText =
                $"<a href=\"http://www.google.com\" &l=\"Test\" ecn_id=\"{Guid.NewGuid()}\">Test</a>";
            var blastId = 1;
            var blastLink = new Entities::BlastLink
            {
                BlastLinkID = 1
            };
            ShimBlastLink.GetByLinkURLInt32String = (id, url) => blastLink;
            ShimBlastLink.GetByLinkURL_ECNIDInt32StringString = (id, url, guid) => null;
            ShimUniqueLink.GetByBlastID_UniqueIDInt32String = (id, guid) =>
            {
                return new Entities::UniqueLink
                {
                    UniqueID = Guid.NewGuid().ToString()
                };
            };
            ShimUniqueLink.SaveUniqueLink = (link) => 1;
            ShimBlastLink.InsertBlastLink = (link) => 1;

            // Act	
            var actualResult = ReflectionHelper.CallMethod(typeof(Blast),
                _MethodHTMLReplaceWithLinkID,
                new object[] { strText, blastId }) as string;

            // Assert
            actualResult.ShouldNotBeNullOrWhiteSpace();
        }
    }
}

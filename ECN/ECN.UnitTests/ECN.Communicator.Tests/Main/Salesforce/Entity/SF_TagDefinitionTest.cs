using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ecn.communicator.main.Salesforce.Entity;
using ECN.Communicator.Tests.Helpers;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.Salesforce.Entity
{
    /// <summary>
    ///     Unit test for <see cref="SF_TagDefinition"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SF_TagDefinitionTest
    {
        [Test]
        public void ConvertJsonList_PassNullStringList_ThrowNullReferenceException()
        {
            // Act
            var exp = Assert.Throws<TargetInvocationException>(() =>
                typeof(SF_TagDefinition).CallMethod("ConvertJsonList", new object[] {null}));

            // Assert
            Assert.That(exp.InnerException, Is.TypeOf<NullReferenceException>());
        }

        [Test]
        public void ConvertJsonList_PassEmptyStringList_ReturnEmptyTagDefinitionList()
        {
            // Arrange
            var json = new List<string>();

            // Act
            var result =
                (List<SF_TagDefinition>) typeof(SF_TagDefinition).CallMethod("ConvertJsonList", new object[] {json});

            // Assert
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNonNullValues_VerifyTagDefinitionList()
        {
            // Arrange
            List<string> json = SfTagDefinitionJsonWithNonNullValues;
            List<SF_TagDefinition> expectedList = SfTagDefinitionListWithNonNullValues;

            // Act
            var result =
                (List<SF_TagDefinition>) typeof(SF_TagDefinition).CallMethod("ConvertJsonList", new object[] {json});

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.True(expectedList.IsListContentMatched(result));
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNullValues_VerifyTagDefinitionList()
        {
            // Arrange
            List<string> json = GetSfTagDefinitionJsonWithNullValues();

            List<SF_TagDefinition> expectedList = SfTagDefinitionListWithNullValues;

            // Act
            var result =
                (List<SF_TagDefinition>) typeof(SF_TagDefinition).CallMethod("ConvertJsonList", new object[] {json});

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.True(expectedList.IsListContentMatched(result));
        }

        private static List<string> SfTagDefinitionJsonWithNonNullValues => new List<string>
        {
            " \"Name\": \"Name\" ,",
            " \"Type\": \"Type\" ,"
        };

        private static List<SF_TagDefinition> SfTagDefinitionListWithNonNullValues => new List<SF_TagDefinition>
        {
            new SF_TagDefinition
            {
                Name = "Name",
                Type = "Type"
            }
        };

        private static List<string> GetSfTagDefinitionJsonWithNullValues() => new List<string>
        {
            " \"Name\": \"null\" ,",
            " \"Type\": null"
        };

        private static List<SF_TagDefinition> SfTagDefinitionListWithNullValues => new List<SF_TagDefinition>
        {
            new SF_TagDefinition
            {
                Name = string.Empty,
                Type = string.Empty
            }
        };
    }
}
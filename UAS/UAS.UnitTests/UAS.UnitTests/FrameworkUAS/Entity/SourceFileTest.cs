using System;
using System.Collections.Generic;
using FrameworkUAS.Entity;
using NUnit.Framework;
using Shouldly;
using RuleSet = FrameworkUAS.Object.RuleSet;

namespace UAS.UnitTests.FrameworkUAS.Entity
{
    [TestFixture]
    public class SourceFileTest
    {
        [Test]
        public void SourceFile_SetDefaultParameter()
        {
            // Arrange, Act
            var sourceFile = new SourceFile();

            // Assert
            sourceFile.ShouldSatisfyAllConditions(
                () => sourceFile.FileName.ShouldBe(string.Empty),
                () => sourceFile.SourceFileID.ShouldBe(0),
                () => sourceFile.FileRecurrenceTypeId.ShouldBe(0),
                () => sourceFile.DatabaseFileTypeId.ShouldBe(0),
                () => sourceFile.FileName.ShouldBe(string.Empty),
                () => sourceFile.ClientID.ShouldBe(0),
                () => sourceFile.PublicationID.ShouldBe(0),
                () => sourceFile.IsDeleted.ShouldBeFalse(),
                () => sourceFile.IsIgnored.ShouldBeFalse(),
                () => sourceFile.FileSnippetID.ShouldBe(0),
                () => sourceFile.Extension.ShouldBe(string.Empty),
                () => sourceFile.IsDQMReady.ShouldBeTrue(),
                () => sourceFile.Delimiter.ShouldBe(string.Empty),
                () => sourceFile.IsTextQualifier.ShouldBeFalse(),
                () => sourceFile.ServiceID.ShouldBe(0),
                () => sourceFile.ServiceFeatureID.ShouldBe(0),
                () => sourceFile.MasterGroupID.ShouldBe(0),
                () => sourceFile.UseRealTimeGeocoding.ShouldBeFalse(),
                () => sourceFile.IsSpecialFile.ShouldBeFalse(),
                () => sourceFile.ClientCustomProcedureID.ShouldBe(0),
                () => sourceFile.SpecialFileResultID.ShouldBe(0),
                () => sourceFile.DateCreated.DayOfYear.ShouldBe(DateTime.Now.DayOfYear),
                () => sourceFile.DateUpdated.ShouldBeNull(),
                () => sourceFile.CreatedByUserID.ShouldBe(0),
                () => sourceFile.UpdatedByUserID.ShouldBeNull(),
                () => sourceFile.QDateFormat.ShouldBe(string.Empty),
                () => sourceFile.FieldMappings.ShouldBeOfType<HashSet<FieldMapping>>(),
                () => sourceFile.FieldMappings.Count.ShouldBe(0),
                () => sourceFile.BatchSize.ShouldBe(2500),
                () => sourceFile.IsPasswordProtected.ShouldBeFalse(),
                () => sourceFile.ProtectionPassword.ShouldBe(string.Empty),
                () => sourceFile.NotifyEmailList.ShouldBe(string.Empty),
                () => sourceFile.IsBillable.ShouldBeTrue(),
                () => sourceFile.Notes.ShouldBe(string.Empty),
                () => sourceFile.RuleSets.ShouldBeOfType<HashSet<RuleSet>>(),
                () => sourceFile.IsFullFile.ShouldBeFalse());
        }
    }
}

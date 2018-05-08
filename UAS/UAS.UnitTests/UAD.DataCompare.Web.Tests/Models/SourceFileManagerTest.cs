using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using UAD.DataCompare.Web.Models;

using SourceFile = UAD.DataCompare.Web.Models.SourceFile;

namespace UAD.DataCompare.Web.Tests.Models
{
    [TestFixture]
    public class SourceFileManagerTest
    {
        [Test]
        public void SourceFileManager_SourceFile_SetDefaultParameter()
        {
            // Arrange, Act
            var sourceFile = new SourceFile();

            // Assert
            sourceFile.ShouldSatisfyAllConditions(
                () => sourceFile.SourceFileID.ShouldBe(0),
                () => sourceFile.FileRecurrenceTypeId.ShouldBe(0),
                () => sourceFile.DatabaseFileTypeId.ShouldBe(0),
                () => sourceFile.FileName.ShouldBe(string.Empty),
                () => sourceFile.ClientID.ShouldBe(0),
                () => sourceFile.PublicationID.ShouldBe(0),
                () => sourceFile.IsDeleted.ShouldBe(false),
                () => sourceFile.IsIgnored.ShouldBe(false),
                () => sourceFile.FileSnippetID.ShouldBe(0),
                () => sourceFile.Extension.ShouldBe(string.Empty),
                () => sourceFile.IsDQMReady.ShouldBe(true),
                () => sourceFile.Delimiter.ShouldBe(string.Empty),
                () => sourceFile.IsTextQualifier.ShouldBe(false),
                () => sourceFile.ServiceID.ShouldBe(0),
                () => sourceFile.ServiceFeatureID.ShouldBe(0),
                () => sourceFile.MasterGroupID.ShouldBe(0),
                () => sourceFile.UseRealTimeGeocoding.ShouldBe(false),
                () => sourceFile.IsSpecialFile.ShouldBe(false),
                () => sourceFile.ClientCustomProcedureID.ShouldBe(0),
                () => sourceFile.SpecialFileResultID.ShouldBe(0),
                () => sourceFile.DateCreated.DayOfYear.ShouldBe(DateTime.Now.DayOfYear),
                () => sourceFile.DateUpdated.ShouldBe(null),
                () => sourceFile.CreatedByUserID.ShouldBe(0),
                () => sourceFile.UpdatedByUserID.ShouldBe(null),
                () => sourceFile.QDateFormat.ShouldBe(string.Empty),
                () => sourceFile.BatchSize.ShouldBe(2500),
                () => sourceFile.IsPasswordProtected.ShouldBe(false),
                () => sourceFile.ProtectionPassword.ShouldBe(string.Empty),
                () => sourceFile.FieldMapping.ShouldBe(null),
                () => sourceFile.ColumnMapping.ShouldBe(null),
                () => sourceFile.sourceFileUploadPath.ShouldBe(string.Empty),
                () => sourceFile.errorMessagesDict.ShouldBe(null));
        }   
    }      
}           

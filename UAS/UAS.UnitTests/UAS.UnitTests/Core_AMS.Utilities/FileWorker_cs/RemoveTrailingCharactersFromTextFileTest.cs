using System;
using System.IO;
using Core_AMS.Utilities;
using NUnit.Framework;
using Shouldly;
using CommonEnums = KM.Common.Enums;

namespace UAS.UnitTests.Core_AMS.Utilities.FileWorker_cs
{
    [TestFixture]
    public class RemoveTrailingCharactersFromTextFileTest : Fakes
    {
        [Test]
        public void RemoveTrailingCharactersFromTextFile_CommaDelim_FileParsed()
        {
            // Arrange
            FillFileRow1Col2Semicolon();

            var worker = new FileWorker();

            // Act
            worker.RemoveTrailingCharactersFromTextFile(FileInfo1, true, CommonEnums.ColumnDelimiter.semicolon);

            // Assert
            var content = File.ReadAllText(FileInfo1.FullName);
            content.ShouldBe(string.Format("\"{0}\";\"{1}\"{2}", ValueFile1, ValueFile2, Environment.NewLine));
        }
    }
}

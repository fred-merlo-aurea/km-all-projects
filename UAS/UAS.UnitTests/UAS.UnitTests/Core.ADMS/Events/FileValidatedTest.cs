using System.IO;
using Core.ADMS.Events;
using FrameworkUAD.Object;
using FrameworkUAS.Entity;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.Core.ADMS.Events
{
	[TestFixture]
	public class FileValidatedTest
	{
		private const string DummyFileName = "DummyFileName.pdf";

		[Test]
		public void FileValidated_SetAndGetValue_ReturnsDefaultValue()
		{
			// Arrange, Act
			var fileValidated = new FileValidated();

			// Assert
			fileValidated.ShouldSatisfyAllConditions(
				() => fileValidated.ImportFile.ShouldBeNull(),
				() => fileValidated.Client.ShouldBeNull(),
				() => fileValidated.IsKnownCustomerFileName.ShouldBeFalse(),
				() => fileValidated.IsValidFileType.ShouldBeFalse(),
				() => fileValidated.IsFileSchemaValid.ShouldBeFalse(),
				() => fileValidated.SourceFile.ShouldBeNull(),
				() => fileValidated.ValidationResult.ShouldBeNull(),
				() => fileValidated.AdmsLog.ShouldBeNull());
		}

		[Test]
		public void FileValidated_SetAndGetValue_ReturnsSetValue()
		{
			// Arrange, Act
			var fileValidated = new FileValidated
			{
				ImportFile = new FileInfo(DummyFileName),
				Client = new Client(),
				IsKnownCustomerFileName = true,
				IsValidFileType = true,
				IsFileSchemaValid = true,
				SourceFile = new SourceFile(),
				AdmsLog = new AdmsLog(),
				ValidationResult = new ValidationResult()
			};

			// Assert
			fileValidated.ShouldSatisfyAllConditions(
				() => fileValidated.ImportFile.ShouldNotBeNull(),
				() => fileValidated.Client.ShouldNotBeNull(),
				() => fileValidated.IsKnownCustomerFileName.ShouldBeTrue(),
				() => fileValidated.IsValidFileType.ShouldBeTrue(),
				() => fileValidated.IsFileSchemaValid.ShouldBeTrue(),
				() => fileValidated.SourceFile.ShouldNotBeNull(),
				() => fileValidated.ValidationResult.ShouldNotBeNull(),
				() => fileValidated.AdmsLog.ShouldNotBeNull());
		}
	}
}
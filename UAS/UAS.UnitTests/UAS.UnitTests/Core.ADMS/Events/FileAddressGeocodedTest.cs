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
	public class FileAddressGeocodedTest
	{
		private const string DummyFileName = "DummyFileName.pdf";

		[Test]
		public void FileAddressGeocoded_SetAndGetValue_ReturnsDefaultValue()
		{
			// Arrange, Act
			var fileAddressGeocoded = new FileAddressGeocoded();

			// Assert
			fileAddressGeocoded.ShouldSatisfyAllConditions(
				() => fileAddressGeocoded.ImportFile.ShouldBeNull(),
				() => fileAddressGeocoded.Client.ShouldBeNull(),
				() => fileAddressGeocoded.IsKnownCustomerFileName.ShouldBeFalse(),
				() => fileAddressGeocoded.IsValidFileType.ShouldBeFalse(),
				() => fileAddressGeocoded.IsFileSchemaValid.ShouldBeFalse(),
				() => fileAddressGeocoded.SourceFile.ShouldBeNull(),
				() => fileAddressGeocoded.ValidationResult.ShouldBeNull(),
				() => fileAddressGeocoded.AdmsLog.ShouldBeNull());
		}

		[Test]
		public void FileAddressGeocoded_SetAndGetValue_ReturnsSetValue()
		{
			// Arrange, Act
			var fileAddressGeocoded = new FileAddressGeocoded
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
			fileAddressGeocoded.ShouldSatisfyAllConditions(
				() => fileAddressGeocoded.ImportFile.ShouldNotBeNull(),
				() => fileAddressGeocoded.Client.ShouldNotBeNull(),
				() => fileAddressGeocoded.IsKnownCustomerFileName.ShouldBeTrue(),
				() => fileAddressGeocoded.IsValidFileType.ShouldBeTrue(),
				() => fileAddressGeocoded.IsFileSchemaValid.ShouldBeTrue(),
				() => fileAddressGeocoded.SourceFile.ShouldNotBeNull(),
				() => fileAddressGeocoded.ValidationResult.ShouldNotBeNull(),
				() => fileAddressGeocoded.AdmsLog.ShouldNotBeNull());
		}
	}
}
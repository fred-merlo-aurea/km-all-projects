using System;
using System.Diagnostics.CodeAnalysis;
using ecn.communicator.main.lists;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Lists
{
	/// <summary>
	/// UT for <see cref="download"/>
	/// </summary>
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public partial class DownloadTest : BaseListsTest<download>
	{
		private const string MethodDownload = "downloadFile";

		[Test]
		public void DownloadFile_SubscribeTypeDefault_SearchTypeDefault_DownloadTypeDefault_NoEmail()
		{
			// Arrange
			InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, string.Empty, string.Empty,
				string.Empty, string.Empty, SampleFilterId1, string.Empty);

			// Act
			privateObject.Invoke(MethodDownload, null, new EventArgs());

			// Assert
			testObject.ShouldSatisfyAllConditions(
				() => contentType.ShouldBeEmpty(),
				() => fileText.ShouldBeEmpty());
		}

		[Test]
		public void DownloadFile_SubscribeTypeDefault_SearchTypeDefault_DownloadTypeDefault()
		{
			// Arrange
			InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, string.Empty, string.Empty,
				string.Empty, SampleEmail, SampleFilterId1, string.Empty);

			// Act
			privateObject.Invoke(MethodDownload, null, new EventArgs());

			// Assert
			testObject.ShouldSatisfyAllConditions(
				() => contentType.ShouldBeEmpty(),
				() => fileText.ShouldBeEmpty());
		}

		[Test]
		public void DownloadFile_SubscribeTypeAll_SearchTypeLike_DownloadTypeXls()
		{
			// Arrange
			InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, DownloadTypeXls, SubscribeTypeAll,
				SearchTypeLike, SampleEmail, SampleFilterId1, string.Empty);

			// Act
			privateObject.Invoke(MethodDownload, null, new EventArgs());

			// Assert
			testObject.ShouldSatisfyAllConditions(
				() => contentType.ShouldBe(XlsContentType),
				() => fileText.ShouldBe(XlsFileText),
				() => responseHeader.ShouldContain(Emails + DownloadTypeXls),
				() => responseText.ShouldContain(Emails + DownloadTypeXls));
		}

		[Test]
		public void DownloadFile_SubscribeTypeDefault_SearchTypeEquals_DownloadTypeCsv()
		{
			// Arrange
			InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, DownloadTypeCsv, string.Empty,
				SearchTypeEquals, SampleEmail, SampleFilterId1, string.Empty);

			// Act
			privateObject.Invoke(MethodDownload, null, new EventArgs());

			// Assert
			testObject.ShouldSatisfyAllConditions(
				() => contentType.ShouldBe(CsvContentType),
				() => fileText.ShouldBe(TxtFileText),
				() => responseHeader.ShouldContain(Emails + DownloadTypeCsv),
				() => responseText.ShouldContain(Emails + DownloadTypeCsv));
		}

		[Test]
		public void DownloadFile_SubscribeTypeDefault_SearchTypeStarts_DownloadTypeXml()
		{
			// Arrange
			InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, DownloadTypeXml, string.Empty,
				SearchTypeStarts, SampleEmail, SampleFilterId1, string.Empty);

			// Act
			privateObject.Invoke(MethodDownload, null, new EventArgs());

			// Assert
			testObject.ShouldSatisfyAllConditions(
				() => contentType.ShouldBe(XmlContentType),
				() => fileText.ShouldBeEmpty(),
				() => responseHeader.ShouldContain(Emails + DownloadTypeXml),
				() => responseText.ShouldContain(Emails + DownloadTypeXml));
		}

		[Test]
		public void DownloadFile_SubscribeTypeDefault_SearchTypeEnds_DownloadTypeTxt()
		{
			// Arrange
			InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, DownloadTypeTxt, string.Empty,
				SearchTypeEnds, SampleEmail, SampleFilterId1, string.Empty);

			// Act
			privateObject.Invoke(MethodDownload, null, new EventArgs());

			// Assert
			testObject.ShouldSatisfyAllConditions(
				() => contentType.ShouldBe(TxtContentType),
				() => fileText.ShouldBe(XlsFileText),
				() => responseHeader.ShouldContain(Emails + DownloadTypeTxt),
				() => responseText.ShouldContain(Emails + DownloadTypeTxt));
		}

		protected override void InitializeFields(string chId, string custId, string grpId, string fileType, string subType,
			string srchType, string srchEm, string filterId, string profFilter)
		{
			QueryString.Add("chID", chId);
			QueryString.Add("custID", custId);
			QueryString.Add("grpID", grpId);
			QueryString.Add(nameof(srchEm), srchEm);
			QueryString.Add(nameof(subType), subType);
			QueryString.Add(nameof(srchType), srchType);
			QueryString.Add(nameof(fileType), fileType);
			QueryString.Add("FilterID", filterId);
			QueryString.Add(nameof(profFilter), profFilter);

		}
	}
}
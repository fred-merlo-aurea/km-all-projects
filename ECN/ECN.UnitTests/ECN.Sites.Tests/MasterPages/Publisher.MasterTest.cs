using System.Diagnostics.CodeAnalysis;
using ECN.TestHelpers;
using ecn.publisher.MasterPages;
using NUnit.Framework;

namespace ECN.Sites.Tests.MasterPages
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	// All the UTs for ecn.publisher.MasterPages.Publisher are implemented in generic class MasterPageExTestBase
	public class PublisherTest : MasterPageExTestBase<Publisher>
	{
	}
}
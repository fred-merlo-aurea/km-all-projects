using System.Diagnostics.CodeAnalysis;
using ECN.TestHelpers;
using ecn.collector.MasterPages;
using NUnit.Framework;

namespace ECN.Sites.Tests.MasterPages
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	// All the UTs for ecn.collector.MasterPages.Collector are implemented in generic class MasterPageExTestBase
	public class CollectorTest : MasterPageExTestBase<Collector>
	{
	}
}

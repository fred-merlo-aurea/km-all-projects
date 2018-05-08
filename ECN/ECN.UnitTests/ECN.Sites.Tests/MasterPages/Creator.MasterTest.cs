using System.Diagnostics.CodeAnalysis;
using ECN.TestHelpers;
using ecn.creator;
using NUnit.Framework;

namespace ECN.Sites.Tests.MasterPages
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	// All the UTs for ecn.creator.Creator are implemented in generic class MasterPageExTestBase
	public class CreatorTest : MasterPageExTestBase<Creator>
	{
	}
}
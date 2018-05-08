using System.Diagnostics.CodeAnalysis;
using ECN.TestHelpers;
using NUnit.Framework;

namespace ECN.Sites.Tests.MasterPages
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	// All the UTs for ecn.communicator.MasterPages.Communicator are implemented in generic class MasterPageExTestBase
	public class CommunicatorTest : MasterPageExTestBase<ecn.communicator.MasterPages.Communicator>
	{
	}
}
﻿using System.Diagnostics.CodeAnalysis;
using ECN.TestHelpers;
using ecn.accounts.MasterPages;
using NUnit.Framework;

namespace ECN.Sites.Tests.MasterPages
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	// All the UTs for ecn.accounts.MasterPages.Accounts are implemented in generic class MasterPageExTestBase
	public class AccountsTest : MasterPageExTestBase<Accounts>
	{
	}
}

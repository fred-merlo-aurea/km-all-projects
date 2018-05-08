using System;
using System.Diagnostics.CodeAnalysis;
using ecn.communicator.foldermanager;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.Folders
{
	/// <summary>
	///     Unit tests for <see cref="ecn.communicator.foldermanager.folderseditor"/>
	/// </summary>
	[TestFixture, ExcludeFromCodeCoverage]
	public partial class FoldersEditorTest
	{
		private IDisposable _shimContext;
		private PrivateObject _foldersEditorPrivateObject;

		[SetUp]
		public void Setup()
		{
			_shimContext = ShimsContext.Create();
			_foldersEditorPrivateObject = new PrivateObject(new folderseditor());
		}

		[TearDown]
		public void TearDown()
		{
			_shimContext.Dispose();
		}
	}
}

using System;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using ecn.collector.classes;
using ecn.common.classes;
using ecn.common.classes.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Tests.Collector
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ChannelBlockTest
    {
        [Test()]
        public void LoginBlock_ValidValues_ReturnsValidData()
        {
            // Arrange
            var userName = "User1";
            var errorMessage = "ErrorMessage1";
            var processPage = "ProcessPage1";

            // Act
            var channelBlock = new ChannelBlock();
            var result = channelBlock.LoginBlock(userName, errorMessage, processPage);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldContain($"<form action='{processPage}' method=post>"),
                () => result.ShouldContain($"<input name=user value='{userName}' size=15 maxlength=100 class=formfield type=text>"),
                () => result.ShouldContain($"<div id=ErrorMessage><br><b><font color=darkRed>{errorMessage}</font></b></div>"));
        }
    }
}

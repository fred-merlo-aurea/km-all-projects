using System;
using System.Data;
using System.Reflection;

using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.SqlServer.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace KM.Common.UnitTests.Functions
{
    [TestFixture]
    public class DynamicBuilderConfigurationTest
    {
        [Test]
        public void TimeSpanFieldNullable_DefaultValue_False()
        {
            // Arrange
            DynamicBuilderConfiguration config = new DynamicBuilderConfiguration();
            // Act
            bool defaultTimeSpanFieldNullable = config.TimeSpanFieldNullable;
            // Assert
            defaultTimeSpanFieldNullable.ShouldBeFalse();
        }
    }
}

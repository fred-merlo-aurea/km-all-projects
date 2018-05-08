using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADMS.ClientMethods;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ClientSpecialCommonTest
    {
        [Test]
        public void EventSwipeDataDefault_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new ClientSpecialCommon();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.EventSwipeDataDefault(f.Client, f),
                    new[] { "Market", "Pubcode" },
                    new[] { "test", "CodeSheetValue_Test" },
                    dimensionValue: "test",
                    matchValue: "CodeSheetValue_Test",
                    operatorExpected: "equal",
                    updateUADExpected: true);
            }
        }
    }
}

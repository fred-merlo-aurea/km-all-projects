using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ADMS.ClientMethods;
using ADMS.ClientMethods.Fakes;
using Core.ADMS.Events;
using FrameworkUAD.BusinessLogic.Fakes;
using KM.Common.Import.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Entity;
using Shouldly;
using Product = FrameworkUAD.Entity.Product;
using ShimProduct = FrameworkUAD.DataAccess.Fakes.ShimProduct;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TradePressTest
    {
        [Test]
        public void ProcessWebFiles_ExecuteWitValidAndInvalidFields_ReturnVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                List<global::FrameworkUAD.Entity.SubscriberInvalid> siListToCheck = null;
                List<global::FrameworkUAD.Entity.SubscriberTransformed> stListToCheck = null;
                List<global::FrameworkUAD.Entity.SubscriberOriginal> soListToCheck = null;

                var testObject = new TradePress();
                var eventMessage = new FileMoved();
                var client = new Client();

                eventMessage.AdmsLog = new AdmsLog();
                eventMessage.SourceFile = new SourceFile();
                eventMessage.SourceFile.FieldMappings = new HashSet<FieldMapping>()
                {
                    new FieldMapping()
                    {
                        IncomingField = "TEST",
                        MAFField = "T1",
                        DataType = string.Empty,
                        PreviewData = string.Empty
                    }
                };
                eventMessage.Client = client;

                ShimFileImporter.LoadFileFileInfoFileConfiguration = (_, __) =>
                {
                    var dateTable = new DataTable();
                    dateTable.Columns.Add("EmailAddress");
                    dateTable.Columns.Add("PUBCODE");
                    dateTable.Columns.Add("TEST");

                    dateTable.Rows.Add("test@test.com", "TST", "V1");
                    dateTable.Rows.Add("test2@test.com", "TST2", "V2");
                    return dateTable;
                };

                FrameworkUAD_Lookup.BusinessLogic.Fakes.ShimCode.AllInstances.SelectCodeNameEnumsCodeTypeString =
                    (_, __, ___) => new Code();
                ShimProduct.SelectClientConnections = connections => new List<Product>()
                {
                    new Product()
                    {
                        PubCode = "TST"
                    }
                };
                ShimSubscriberOriginal.AllInstances.SaveBulkSqlInsertListOfSubscriberOriginalClientConnections =
                    (original, soList, __) =>
                    {
                        soListToCheck = soList; 
                        return false;
                    };
                ShimSubscriberTransformed.AllInstances
                        .SaveBulkSqlInsertListOfSubscriberTransformedClientConnectionsBoolean =
                    (transformed, stList, __, ___) =>
                    {
                        stListToCheck = stList;
                        return true;
                    };
                ShimSubscriberInvalid.AllInstances.SaveBulkSqlInsertListOfSubscriberInvalidClientConnections =
                    (invalid, siList, __) =>
                    {
                        siListToCheck = siList;
                        return true;
                    };

                ShimCodeSheet.AllInstances.CodeSheetValidationInt32StringClientConnections =
                    (sheet, _, __, ___) => true;
                ShimOperations.AllInstances.QSourceValidationClientConnectionsInt32String = (_, __, ___, ____) => true;
                ShimTradePress.AllInstances.SelectImportErrorsFileMoved = (_, __) => null;
                ShimTradePress.AllInstances.GoToDQMFileMovedValidationResult = (_, __, ___) => { };

                // Act
                testObject.ProcessWebFiles(client, null, null, eventMessage);

                // Assert
                siListToCheck.ShouldNotBeNull();
                stListToCheck.ShouldNotBeNull();
                soListToCheck.ShouldNotBeNull();

                siListToCheck.Count.ShouldBe(1);
                stListToCheck.Count.ShouldBe(1);
                soListToCheck.Count.ShouldBe(2);

                siListToCheck.ShouldContain(x=> x.Email == "test2@test.com" && x.DemographicInvalidList.Count == 1);
                stListToCheck.ShouldContain(x => x.Email == "test@test.com" && x.DemographicTransformedList.Count == 1);

                siListToCheck.ShouldContain(x=> x.DemographicInvalidList.Any(di=>di.MAFField=="T1" && di.Value == "V2"));
                stListToCheck.ShouldContain(x => x.DemographicTransformedList.Any(di => di.MAFField == "T1" && di.Value == "V1"));
            }
        }
    }
}

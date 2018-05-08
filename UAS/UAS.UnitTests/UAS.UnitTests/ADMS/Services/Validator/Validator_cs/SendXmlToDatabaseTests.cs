using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using ADMS.Services.Emailer.Fakes;
using ADMS.Services.Fakes;
using Core.ADMS.Events;
using FrameworkSubGen.BusinessLogic.Fakes;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAS.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static UAS.UnitTests.ADMS.Services.Validator.Common.Constants;
using ADMS_Validator = ADMS.Services.Validator.Validator;
using ClientEntity = KMPlatform.Entity.Client;
using ShimSubscription = FrameworkUAD.BusinessLogic.Fakes.ShimSubscription;
using StringFunctions = KM.Common.StringFunctions;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    ///     Unit Tests for <see cref="ADMS_Validator"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SendXmlToDatabaseTests
    {
        private const int DefaultPubCode = 10;
        private const string PubcodeKey = "pubcode";

        private PrivateObject _validatorPrivateObject;
        private FileMoved _eventMessage;
        private DataTable _dtNcoa;
        private string _pubCode;
        private string _type;

        private IDisposable _context;

        [SetUp]
        public void Initialize()
        {
            _context = ShimsContext.Create();

            _validatorPrivateObject = new PrivateObject(typeof(ADMS_Validator));
            _eventMessage = new FileMoved();
            _dtNcoa = new DataTable();
        }

        [TearDown]
        public void DisposeContext()
        {
            _context.Dispose();
        }

        [Test]
        public void SendXmlToDatabase_WhenEventMessageIsNull_ThrowsException()
        {
            // Arrange
            _eventMessage = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SendXmlToDatabaseMethod, _eventMessage, _dtNcoa, _pubCode, _type));
        }

        [Test]
        public void SendXmlToDatabase_WhenDataTableNcoaIsNull_ThrowsException()
        {
            // Arrange
            _dtNcoa = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SendXmlToDatabaseMethod, _eventMessage, _dtNcoa, _pubCode, _type));
        }

        [Test]
        [TestCase("coa")]
        [TestCase("std")]
        public void SendXmlToDatabase_WhenDataTableNcoaIsNotNull_ShouldSendXmlToDatabase(string fieldType)
        {
            // Arrange
            _pubCode = DefaultPubCode.ToString();
            _dtNcoa = CreateNcoaDataTable(fieldType);
            _eventMessage = new FileMoved
            {
                AdmsLog = new AdmsLog { ProcessCode = "Process Code 1" },
                Client = new ClientEntity { ClientID = 30 },
                SourceFile = new SourceFile { SourceFileID = 40 }
            };

            ShimProduct.AllInstances.SelectClientConnectionsBoolean = (_, clientConnection, includeProperties) =>
                new List<Product>
                {
                    new Product { PubCode = "10" , PubID = 1 }
                };

            var xmlResult = string.Empty;
            ShimSubscription.AllInstances.NcoaUpdateAddressStringClientConnectionsInt32 = (_, xmlFile, conn, id) =>
            {
                xmlResult = xmlFile;
                return true;
            };

            ShimProductSubscription.AllInstances.SelectSequenceListOfStringClientConnections =
                (subscription, list, arg3) => null;

            ShimAddress.AllInstances.UpdateForNCOAIListOfProductSubscriptionIListOfNCOAInt32 =
                (address, list, arg3, arg4) => null;

            ShimEmailer.AllInstances.SubGenAddressesNotAbleToUpdateListOfProductSubscriptionClient =
                (emailer, list, arg3) => { };

            ShimServiceBase.AllInstances.ConsoleMessageStringStringBooleanInt32Int32 =
                (_, message, processCode, createLog, sourceField, updateByUser) => { };

            var pubList = CreatePubList(_dtNcoa.Rows[0], _eventMessage, fieldType);
            var xml = fieldType.Equals("std")
                ? NcoaUadGetXml(pubList)
                : NcoaGetXml(pubList, _eventMessage.Client);

            // Act
            _validatorPrivateObject.Invoke(SendXmlToDatabaseMethod, _eventMessage, _dtNcoa, _pubCode, fieldType);

            // Assert
            xml.ShouldBe(xmlResult);
        }

        private static DataTable CreateNcoaDataTable(string type)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(PubcodeKey, typeof(string));
            dataTable.Columns.Add("ncoaseq", typeof(string));
            dataTable.Columns.Add($"{type}_line1", typeof(string));
            dataTable.Columns.Add($"{type}_line2", typeof(string));
            dataTable.Columns.Add($"{type}_city", typeof(string));
            dataTable.Columns.Add($"{type}_state", typeof(string));
            dataTable.Columns.Add($"{type}_zip", typeof(string));
            dataTable.Columns.Add($"{type}_zip4", typeof(string));

            var dataRow1 = dataTable.NewRow();
            dataRow1[PubcodeKey] = 10;
            dataRow1["ncoaseq"] = 20;
            dataRow1[$"{type}_line1"] = "Address1";
            dataRow1[$"{type}_line2"] = "Address2";
            dataRow1[$"{type}_city"] = "City";
            dataRow1[$"{type}_state"] = "RegionCode";
            dataRow1[$"{type}_zip"] = "ZipCode";
            dataRow1[$"{type}_zip4"] = "Plus4";
            dataTable.Rows.Add(dataRow1);

            return dataTable;
        }

        private static IList<NCOA> CreatePubList(DataRow row, FileMoved eventMessage, string type)
        {
            var pubList = new List<NCOA>();
            int parseSequenceId;
            int.TryParse(row["ncoaseq"].ToString().Trim(), out parseSequenceId);

            pubList.Add(new NCOA
            {
                SequenceID = parseSequenceId,
                Address1 = row[$"{type}_line1"].ToString().Trim(),
                Address2 = row[$"{type}_line2"].ToString().Trim(),
                City = row[$"{type}_city"].ToString().Trim(),
                RegionCode = row[$"{type}_state"].ToString().Trim(),
                ZipCode = row[$"{type}_zip"].ToString().Trim(),
                Plus4 = row[$"{type}_zip4"].ToString().Trim(),
                ProductCode = row["pubcode"].ToString().Trim(),
                PublisherID = 0,
                PublicationID = 0,
                ProcessCode = eventMessage.AdmsLog.ProcessCode
            });

            return pubList;
        }

        private static string NcoaUadGetXml(IEnumerable<NCOA> pubList)
        {
            var result = new StringBuilder();
            result.AppendLine("<XML>");

            foreach (var node in pubList)
            {
                result.AppendLine("<NCOA>");
                result.AppendLine($"<SequenceID>{StringFunctions.CleanString(node.SequenceID.ToString())}</SequenceID>");
                result.AppendLine($"<Address1>{StringFunctions.CleanString(node.Address1)}</Address1>");
                result.AppendLine($"<Address2>{StringFunctions.CleanString(node.Address2)}</Address2>");
                result.AppendLine($"<City>{StringFunctions.CleanString(node.City)}</City>");
                result.AppendLine($"<RegionCode>{StringFunctions.CleanString(node.RegionCode)}</RegionCode>");
                result.AppendLine($"<ZipCode>{StringFunctions.CleanString(node.ZipCode)}</ZipCode>");
                result.AppendLine($"<Plus4>{StringFunctions.CleanString(node.Plus4)}</Plus4>");
                result.AppendLine($"<ProcessCode>{node.ProcessCode}</ProcessCode>");
                result.AppendLine("</NCOA>");
            }
            result.AppendLine("</XML>");

            return result.ToString();
        }

        private static string NcoaGetXml(IEnumerable<NCOA> pubList, ClientEntity client)
        {
            var result = new StringBuilder();
            result.AppendLine("<XML>");

            foreach (var node in pubList)
            {
                result.AppendLine("<NCOA>");
                result.AppendLine($"<SequenceID>{StringFunctions.CleanString(node.SequenceID.ToString())}</SequenceID>");
                result.AppendLine($"<Address1>{StringFunctions.CleanString(node.Address1)}</Address1>");
                result.AppendLine($"<Address2>{StringFunctions.CleanString(node.Address2)}</Address2>");
                result.AppendLine($"<City>{StringFunctions.CleanString(node.City)}</City>");
                result.AppendLine($"<RegionCode>{StringFunctions.CleanString(node.RegionCode)}</RegionCode>");
                result.AppendLine($"<ZipCode>{StringFunctions.CleanString(node.ZipCode)}</ZipCode>");
                result.AppendLine($"<Plus4>{StringFunctions.CleanString(node.Plus4)}</Plus4>");
                result.AppendLine($"<ProductCode>{StringFunctions.CleanString(node.ProductCode)}</ProductCode>");
                result.AppendLine($"<PublisherID>{client.ClientID.ToString().Trim()}</PublisherID>");
                result.AppendLine($"<PublicationID>1</PublicationID>");
                result.AppendLine($"<ProcessCode>{node.ProcessCode}</ProcessCode>");
                result.AppendLine("</NCOA>");
            }
            result.AppendLine("</XML>");

            return result.ToString();
        }
    }
}

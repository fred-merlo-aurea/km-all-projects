using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using PrivateObject = Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject;

namespace SCG_CDS_Import.Tests
{
    public partial class ProgramTest
    {
        private const string ImportToEcnMethodName = "ImportToEcn";
        private const string BatchCountConfigurationKey = "BatchCount";
        private const string SUBColumn = "SUB";
        private const string UPDColumn = "UPD";
        private const string UPTColumn = "UPT";
        private const string INMColumn = "INM";
        private const string CNTColumn = "CNT";
        private const string VDTColumn = "VDT";
        private const string ZipColumn = "ZIP";
        private const string ZipValue = "ZIP Value";
        private const string SUBValue = "SUB Value";
        private const string UPDValue = "UPD Value";
        private const string UPTValue = "UPT Value";
        private const string CombineFields = SUBColumn + "," + UPTColumn;
        private const string Email = "mail@someDomain";
        private const string ECNFieldDifferentValue = "Some different value";
        private string _fileFieldColumn;
        private string _fileFieldValue;
        private string _inmValue;
        private string _ecnField;
        private string _vdtValue;

        [SetUp]
        public void ImportToEcnSetup()
        {
            _fileFieldColumn = "File Field";
            _fileFieldValue = "File Field Value";
            _inmValue = "INM Value";
            _ecnField = "EcnField";
            _vdtValue = "VDT Value";
        }

        [Test]
        public void ImportToEcn_ProfileFields_WorkAsExpected()
        {
            //Arrange
            const int batchCount = 2;
            _fakedAppSettings.Add(BatchCountConfigurationKey, batchCount.ToString());
            var importFile = GetImportFile();
            var dataTable = GetDataTable();
            var expectedXmlProfile =
                "<Emails>" + Environment.NewLine +
                "<ecnfield>" + _fileFieldValue + "</ecnfield>" + Environment.NewLine +
                "<ecnfield>SUB Value,UPT Value</ecnfield>" + Environment.NewLine +
                "<notes><![CDATA[ [SCG_CDS-Import Engine] [" + _now.ToString() +
                "] ]]> </notes>" + Environment.NewLine +
                "</Emails>" + Environment.NewLine;
            var expectedXmlUDF =
                "<row>" + Environment.NewLine +
                "<ea>mail@someDomain</ea>" + Environment.NewLine +
                "</row>" + Environment.NewLine;
            var expectedImprtFileLog = $"{_now}-END--ImportToECN() File: {importFile.FileName} --";
            //Act
            CallImportToEcn(dataTable, importFile);

            //Assert
            _programTestContext.UpdateToDBXmlProfile.ShouldContain(expectedXmlProfile);
            _programTestContext.UpdateToDBXmlUDF.ShouldContain(expectedXmlUDF);
            _programTestContext.ImportFileLogs.ShouldContain(expectedImprtFileLog);
        }

        [Test]
        public void ImportToEcn_ProfileFieldsWithDifferenctBatchCount_WorkAsExpected()
        {
            //Arrange
            const int batchCount = 10;
            _fakedAppSettings.Add(BatchCountConfigurationKey, batchCount.ToString());
            var importFile = GetImportFile();
            var dataTable = GetDataTable();
            var expectedXmlProfile =
                "<Emails>" + Environment.NewLine +
                "<ecnfield>" + _fileFieldValue + "</ecnfield>" + Environment.NewLine +
                "<ecnfield>SUB Value,UPT Value</ecnfield>" + Environment.NewLine +
                "<notes><![CDATA[ [SCG_CDS-Import Engine] [" + _now.ToString() +
                "] ]]> </notes>" + Environment.NewLine +
                "</Emails>" + Environment.NewLine;
            var expectedXmlUDF =
                "<row>" + Environment.NewLine +
                "<ea>mail@someDomain</ea>" + Environment.NewLine +
                "</row>" + Environment.NewLine;
            var expectedImprtFileLog = $"{_now}-END--ImportToECN() File: {importFile.FileName} --";
            //Act
            CallImportToEcn(dataTable, importFile);

            //Assert
            _programTestContext.UpdateToDBXmlProfile.ShouldContain(expectedXmlProfile);
            _programTestContext.UpdateToDBXmlUDF.ShouldContain(expectedXmlUDF);
            _programTestContext.ImportFileLogs.ShouldContain(expectedImprtFileLog);
        }

        [Test]
        public void ImportToEcn_ProfileFieldsECNFieldFirstName_WorkAsExpected()
        {
            //Arrange
            const int batchCount = 2;
            const string firstName = "FirstName";
            _inmValue = $"{firstName} ";
            var dataTable = GetDataTable();
            _fileFieldColumn = string.Empty;
            _ecnField = "firstname";
            _fakedAppSettings.Add(BatchCountConfigurationKey, batchCount.ToString());
            var importFile = GetImportFile();
            var expectedXmlProfile = $"<{_ecnField}>{firstName}</{_ecnField}>";

            //Act
            CallImportToEcn(dataTable, importFile);

            //Assert
            _programTestContext.UpdateToDBXmlProfile
                .ShouldContain(xmlProfile => xmlProfile.Contains(expectedXmlProfile));
        }

        [Test]
        public void ImportToEcn_ProfileFieldsECNFieldLastName_WorkAsExpected()
        {
            //Arrange
            const int batchCount = 2;
            const string lastName = "Last Name";
            _inmValue = $"FirstName {lastName}";
            var dataTable = GetDataTable();
            _fileFieldColumn = string.Empty;
            _ecnField = "lastname";
            _fakedAppSettings.Add(BatchCountConfigurationKey, batchCount.ToString());
            var importFile = GetImportFile();
            var expectedXmlProfile = $"<{_ecnField}>{lastName}</{_ecnField}>";

            //Act
            CallImportToEcn(dataTable, importFile);

            //Assert
            _programTestContext.UpdateToDBXmlProfile
                .ShouldContain(xmlProfile => xmlProfile.Contains(expectedXmlProfile));
        }

        [Test]
        public void ImportToEcn_ProfileFieldsECNFieldVoiceFaxMobile_WorkAsExpected()
        {
            //Arrange
            const int batchCount = 2;
            var fieldNames = new[] { "voice", "fax", "mobile" };
            _fakedAppSettings.Add(BatchCountConfigurationKey, batchCount.ToString());
            foreach (var fieldName in fieldNames)
            {
                _ecnField = fieldName;
                var dataTable = GetDataTable();
                var importFile = GetImportFile();
                var expectedXmlProfile = $"<{_ecnField}>SUBValue,UPTValue</{_ecnField}>";

                //Act
                CallImportToEcn(dataTable, importFile);

                //Assert
                _programTestContext.UpdateToDBXmlProfile
                    .ShouldContain(xmlProfile => xmlProfile.Contains(expectedXmlProfile));
            }
        }

        [Test]
        public void ImportToEcn_ProfileFieldsECNFieldEmailAddress_WorkAsExpected()
        {
            //Arrange
            const int batchCount = 2;
            _fakedAppSettings.Add(BatchCountConfigurationKey, batchCount.ToString());
            _ecnField = "emailaddress";
            var dataTable = GetDataTable();
            var importFile = GetImportFile();
            var expectedXmlProfile = $"<{_ecnField}>{Email}</{_ecnField}>";

            //Act
            CallImportToEcn(dataTable, importFile);

            //Assert
            _programTestContext.UpdateToDBXmlProfile
                .ShouldContain(xmlProfile => xmlProfile.Contains(expectedXmlProfile));
        }

        [Test]
        public void ImportToEcn_ProfileFieldsECNFieldZipCode_WorkAsExpected()
        {
            //Arrange
            const int batchCount = 2;
            _fakedAppSettings.Add(BatchCountConfigurationKey, batchCount.ToString());
            _ecnField = "zip";
            var dataTable = GetDataTable();
            var importFile = GetImportFile();

            //Act
            CallImportToEcn(dataTable, importFile);

            //Assert
            _programTestContext.UpdateToDBXmlProfile.ShouldNotContain(xmlProfile => xmlProfile.Contains(_ecnField));
        }

        [Test]
        public void ImportToEcn_ProfileFieldsECNFieldSubscribeTypeCode_WorkAsExpected()
        {
            //Arrange
            const int batchCount = 2;
            const string transactionReportFieldName = "TransactionReport";
            _fakedAppSettings.Add(BatchCountConfigurationKey, batchCount.ToString());
            _ecnField = "subscribetypecode";
            var ecnFieldValues = new Dictionary<string, Func<TransactionType, bool>>
            {
                { "U", transactionType => transactionType.DeleteCount == 1 },
                { "D", transactionType => transactionType.DeleteCount == 1 },
                { "C", transactionType => transactionType.ChangeCount == 1 },
                { "A", transactionType => transactionType.AddCount == 1 },
            };

            foreach (var item in ecnFieldValues)
            {
                _fileFieldValue = item.Key;
                var dataTable = GetDataTable();
                var importFile = GetImportFile();
                var transactionReport = new List<TransactionType>();
                SetField(transactionReportFieldName, transactionReport);
                var predicate = item.Value;
                //Act
                CallImportToEcn(dataTable, importFile);

                //Assert
                transactionReport.ShouldNotBeEmpty();
                transactionReport.FirstOrDefault().ShouldNotBeNull();
                predicate(transactionReport.First()).ShouldBeTrue();
            }
        }

        [Test]
        public void ImportToEcn_UDFFields_WorkAsExpected()
        {
            //Arrange
            const int batchCount = 2;
            _fakedAppSettings.Add(BatchCountConfigurationKey, batchCount.ToString());
            const string dUDFFieldsFieldName = "dUDFFields";
            const string uniqueString = "C2331CB9826B4B84835C22C7A04BE57D";
            const int numberAboveZero = 10;
            _fileFieldValue = uniqueString;
            var inputCombinations = new[]
            {
                new
                {
                    ECNFieldName = "qdate",
                    ColumnName = VDTColumn,
                    Value = "220210",
                    ExpectedValue = "10/01/2202"
                },
                new
                {
                    ECNFieldName = "forzip",
                    ColumnName = CNTColumn,
                    Value = uniqueString,
                    ExpectedValue = ZipValue
                },
                new
                {
                    ECNFieldName = "cat",
                    ColumnName = _fileFieldColumn,
                    Value = "Q",
                    ExpectedValue = "10"
                },
                new
                {
                    ECNFieldName = "cat",
                    ColumnName = _fileFieldColumn,
                    Value = "E",
                    ExpectedValue = "10"
                },
                new
                {
                    ECNFieldName = "cat",
                    ColumnName = _fileFieldColumn,
                    Value = "R",
                    ExpectedValue = "70"
                },
                new
                {
                    ECNFieldName = "cat",
                    ColumnName = _fileFieldColumn,
                    Value = uniqueString,
                    ExpectedValue = "10"
                },
                new
                {
                    ECNFieldName = "xact",
                    ColumnName = _fileFieldColumn,
                    Value = "Q",
                    ExpectedValue = "10"
                },
                new
                {
                    ECNFieldName = "xact",
                    ColumnName = _fileFieldColumn,
                    Value = "E",
                    ExpectedValue = "10"
                },
                new
                {
                    ECNFieldName = "xact",
                    ColumnName = _fileFieldColumn,
                    Value = "R",
                    ExpectedValue = "38"
                },
                new
                {
                    ECNFieldName = "xact",
                    ColumnName = _fileFieldColumn,
                    Value = uniqueString,
                    ExpectedValue = "10"
                },
                new
                {
                    ECNFieldName = "demo7",
                    ColumnName = _fileFieldColumn,
                    Value = "D",
                    ExpectedValue = "B"
                },
                new
                {
                    ECNFieldName = "demo7",
                    ColumnName = _fileFieldColumn,
                    Value = "P",
                    ExpectedValue = "A"
                },
                new
                {
                    ECNFieldName = "demo7",
                    ColumnName = _fileFieldColumn,
                    Value = "B",
                    ExpectedValue = "C"
                },
                new
                {
                    ECNFieldName = "demo7",
                    ColumnName = _fileFieldColumn,
                    Value = uniqueString,
                    ExpectedValue = "A"
                },
                new
                {
                    ECNFieldName = uniqueString,
                    ColumnName = _fileFieldColumn,
                    Value = uniqueString,
                    ExpectedValue = uniqueString
                },
            };
            foreach (var inputs in inputCombinations)
            {
                _ecnField = inputs.ECNFieldName;
                var udfFields = new Dictionary<string, int>
                {
                    {_ecnField,  numberAboveZero},
                    {ECNFieldDifferentValue, numberAboveZero }
                };
                SetField(dUDFFieldsFieldName, udfFields);
                var importFile = GetImportFile(addUDF: true);
                var dataTable = GetDataTable();
                foreach (DataRow row in dataTable.Rows)
                {
                    row[inputs.ColumnName] = inputs.Value;
                }
                var expected = $"<udf id=\"{numberAboveZero}\"><v>{inputs.ExpectedValue}</v></udf>";
                //Act
                CallImportToEcn(dataTable, importFile);

                //Assert
                _programTestContext.UpdateToDBXmlUDF.Any(xml => xml.Contains(expected))
                    .ShouldBeTrue();
            }
        }

        [Test]
        public void ImportToEcn_WhenExceptionThrown_ShouldBeLoggedAndThrown()
        {
            //Arrange
            var dataTable = GetDataTable();
            var importFile = GetImportFile();
            var exception = new Exception();
            _programTestContext.AppSettingsExceptionToThrow = exception;
            var expectedFirstPartOfLog =
                "*****************" + Environment.NewLine +
                $"Exception - {_now}" + Environment.NewLine +
                "-- Message --" + Environment.NewLine +
                "Exception of type 'System.Exception' was thrown." + Environment.NewLine +
                "-- InnerException --" + Environment.NewLine +
                "-- Stack Trace --";
            var expectedSecondPartOfLog = "*****************" + Environment.NewLine;
            var expectedEmailSubject = "SCG_CDS-Import";
            var expecteDCriticalErrorSourceMethod = "SCG_CDS-Import.Program.ImportToECN";

            //Act, Assert
            Assert.That(() => CallImportToEcn(dataTable, importFile), Throws.Exception.InstanceOf<Exception>());
            _programTestContext.EmailFunctionsNotifyAdminSubject.ShouldBe(expectedEmailSubject);
            _programTestContext.EmailFunctionsNotifyAdminTextMessage.ShouldContain(expectedFirstPartOfLog);
            _programTestContext.EmailFunctionsNotifyAdminTextMessage.ShouldContain(expectedSecondPartOfLog);
            _programTestContext.LogCriticalErrorException.ShouldBe(exception);
            _programTestContext.LogCriticalErrorSourceMethod.ShouldBe(expecteDCriticalErrorSourceMethod);
            _programTestContext.ImportFileLogs.ShouldContain(log => log.Contains(expectedFirstPartOfLog));
            _programTestContext.ImportFileLogs.ShouldContain(log => log.Contains(expectedSecondPartOfLog));
        }

        private void CallImportToEcn(
            DataTable dataTableFile,
            ImportFile importFile)
        {
            var cdsData = new Dictionary<string, string>
            {
                {SUBValue,  Email}
            };
            var cdsLatestData = new Dictionary<string, string>
            {
                { SUBValue, $"{UPDValue} {UPTValue}" }
            };
            var caller = new PrivateObject(typeof(Program));
            caller.Invoke(ImportToEcnMethodName, BindingFlags.NonPublic | BindingFlags.Static,
                dataTableFile, importFile, cdsData, cdsLatestData);
        }

        private DataTable GetDataTable()
        {
            var result = new DataTable();
            result.Columns.Add(SUBColumn);
            result.Columns.Add(UPDColumn);
            result.Columns.Add(UPTColumn);
            result.Columns.Add(INMColumn);
            result.Columns.Add(CNTColumn);
            result.Columns.Add(VDTColumn);
            result.Columns.Add(ZipColumn);
            result.Columns.Add(_fileFieldColumn);
            var row = result.NewRow();
            row[SUBColumn] = SUBValue;
            row[UPDColumn] = UPDValue;
            row[UPTColumn] = UPTValue;
            row[INMColumn] = _inmValue;
            row[CNTColumn] = CNTColumn;
            row[VDTColumn] = _vdtValue;
            row[ZipColumn] = ZipValue;
            row[_fileFieldColumn] = _fileFieldValue;
            result.Rows.Add(row);
            return result;
        }

        private ImportFile GetImportFile(bool addUDF = false)
        {
            var result = new ImportFile
            {
                Fields = new List<Field>
                {
                    new Field
                    {
                        IsUDF = false,
                        Ignore = false,
                        ECNField = _ecnField,
                        FileField =_fileFieldColumn
                    },
                    new Field
                    {
                        IsUDF = false,
                        Ignore = false,
                        ECNField = _ecnField,
                        FileField = string.Empty,
                        CombineFields = CombineFields
                    }
                }
            };

            if (addUDF)
            {
                result.Fields.Add(new Field
                {
                    IsUDF = true,
                    Ignore = false,
                    ECNField = _ecnField,
                    FileField = _fileFieldColumn
                });
                result.Fields.Add(new Field
                {
                    IsUDF = true,
                    Ignore = false,
                    ECNField = ECNFieldDifferentValue,
                    FileField = string.Empty,
                    CombineFields = CombineFields
                });
            }
            return result;
        }
    }
}

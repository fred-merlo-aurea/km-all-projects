using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameworkUAD.BusinessLogic;
using NUnit.Framework;
using Shouldly;
using EntityImportError = FrameworkUAD.Entity.ImportError;
using ImportErrorEntity = FrameworkUAD.Entity.ImportError;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic
{
    public partial class ValidationResultTest
    {
        [Test]
        public void AppendImportErrors_WhenValidationResultIsNull_ThrowsException()
        {
            // Arrange
            validationResultObject.RecordImportErrors = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => { 
                ValidationResult.AppendImportErrors(
                    validationResultObject.RecordImportErrors, 
                    _sbDetail, 
                    _isTextQualifier,
                    _shouldClearSbDetailt,
                    string.Empty);
            });
        }

        [Test]
        public void AppendImportErrors_WhenSbDetailIsNull_ThrowsException()
        {
            // Arrange
            _sbDetail = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => { 
                ValidationResult.AppendImportErrors(
                    validationResultObject.RecordImportErrors, 
                    _sbDetail, 
                    _isTextQualifier,
                    _shouldClearSbDetailt,
                    string.Empty);
            });
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AppendImportErrors_WhenImportErrorsIsNotNull_ShouldAppendSbDetail(bool isTextQualifier)
        {
            // Arrange
            _shouldClearSbDetailt = true;
            var importErrors = new List<ImportErrorEntity>()
            {
                new ImportErrorEntity
                {
                    BadDataRow = "Sample1, Sample2, Sample3"
                }
            };

            validationResultObject.RecordImportErrors = new HashSet<ImportErrorEntity>(importErrors);

            // Act
            ValidationResult.AppendImportErrors(
                validationResultObject.RecordImportErrors, 
                _sbDetail, 
                isTextQualifier,
                _shouldClearSbDetailt,
                string.Empty);

            // Assert
            _sbDetail.ToString().ShouldBeNullOrWhiteSpace();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AppendImportErrors_WhenImportErrorsIsNotNullAndShouldClearSbDetail_ShouldAppendSbDetail(bool isTextQualifier)
        {
            // Arrange
            _shouldClearSbDetailt = true;
            var importErrors = CreateImportErrorsList();

            validationResultObject.RecordImportErrors = new HashSet<ImportErrorEntity>(importErrors);

            // Act
            ValidationResult.AppendImportErrors(
                validationResultObject.RecordImportErrors, 
                _sbDetail, 
                isTextQualifier,
                _shouldClearSbDetailt,
                string.Empty);

            // Assert
            _sbDetail.ToString().ShouldBeNullOrWhiteSpace();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AppendImportErrors_WhenImportErrorsIsNotNullAndShouldNotClearSbDetail_ShouldAppendSbDetail(bool isTextQualifier)
        {
            // Arrange
            _shouldClearSbDetailt = false;
            var importErrors = CreateImportErrorsList();

            validationResultObject.RecordImportErrors = new HashSet<ImportErrorEntity>(importErrors);
            var sbResult =
                CreateSbDetailResultForImportErrors(validationResultObject.RecordImportErrors, isTextQualifier);

            // Act
            ValidationResult.AppendImportErrors(
                validationResultObject.RecordImportErrors, 
                _sbDetail, 
                isTextQualifier,
                _shouldClearSbDetailt,
                string.Empty);

            // Assert
            _sbDetail.ToString().ShouldBe(sbResult.ToString());
        }

        private static IEnumerable<ImportErrorEntity> CreateImportErrorsList()
        {
            return new List<ImportErrorEntity>
            {
                new ImportErrorEntity
                {
                    BadDataRow = "Sample1, Sample2, Sample3"
                }
            };
        }

        private static StringBuilder CreateSbDetailResultForImportErrors(
            IEnumerable<EntityImportError> recordImportErrors, 
            bool isTextQualifier)
        {
            var temporarySb = new StringBuilder();
            temporarySb.AppendLine(Environment.NewLine);

            foreach (var error in recordImportErrors.Distinct().OrderBy(x => x.RowNumber))
            {
                if (!string.IsNullOrWhiteSpace(error.BadDataRow))
                {
                    error.BadDataRow = error.BadDataRow.Trim().TrimEnd(CommaSeparator);
                    if (isTextQualifier)
                    {
                        var data = new StringBuilder();
                        var quoteRow = error.BadDataRow.Split(',');

                        foreach (var quote in quoteRow)
                        {
                            data.Append($"\"{quote}\",");
                        }

                        temporarySb.AppendLine(data.ToString().Trim().TrimEnd(CommaSeparator));
                    }
                    else
                    {
                        temporarySb.AppendLine(error.BadDataRow.Trim().TrimEnd(CommaSeparator));
                    }
                }
            }

            return temporarySb;
        }
    }
}

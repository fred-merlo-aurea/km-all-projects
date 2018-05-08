using System;
using System.Collections.Generic;
using System.Linq;

namespace KM.Common.Export
{
    public class Helper
    {
        private const string InputNameScore = "SCORE";
        private const string InputNameAddress = "ADDRESS1";
        private const string InputNameRegionCode = "REGIONCODE";
        private const string InputNameZipCode = "ZIPCODE";
        private const string InputNamePubTransactionDate = "PUBTRANSACTIONDATE";
        private const string InputNameQualificationDate = "QUALIFICATIONDATE";
        private const string InputNameEmailStatus = "EMAILSTATUS";
        private const string InputNameCategoryId = "CATEGORYID";
        private const string InputNameTransactionStatus = "TRANSACTIONSTATUS";
        private const string InputNameTransactionId = "TRANSACTIONID";
        private const string InputNameQSourceId = "QSOURCEID";
        private const string InputNameParc3C = "PAR3C";
        private const string InputNameTransactionName = "TRANSACTIONNAME";
        private const string InputNameQSourceName = "QSOURCENAME";
        private const string InputNameLastOpenEdDate = "LASTOPENEDDATE";
        private const string InputNameLastOpenPubCode = "LASTOPENEDPUBCODE";
        private const string InputNameIgrpNo = "IGRP_NO";
        private const string InputNameCgRpNo = "CGRP_NO";
        private const string InputNameBatch = "BATCH";
        private const string InputNameFName = "FNAME";
        private const string InputNameLName = "LNAME";
        private const string InputNameLatLonValid = "ISLATLONVALID";
        private const char PipeSeparator = '|';
        private const string GroupSuffixDefault = "Default";
        private const string GroupSuffixNone = "None";
        private const string SubscriptionIdColumnProductView = "ps.SUBSCRIPTIONID|Default";
        private const string SubscriptionIdColumnNonProductView = "s.SUBSCRIPTIONID|Default";
        private const string SubscriptionIdColumnProductViewExpected = "ps.SubscriptionID|None";
        private const string SubscriptionIdColumnNonProductViewExpected = "s.SubscriptionID|None";
        private const string CommaSeparator = ",";
        private const string ExportTransactionName = "tc.TransactionCodeName as TransactionName";
        private const string ExportDisplayOrderAsParc3C = "cp.DisplayOrder as Par3C";
        private const string ExportQSourceId = "c.CodeValue as QSourceID";
        private const string ExportTransactionStatus = "tct.TransactionCodeTypename as Transactionstatus";
        private const string ExportCategoryId = "cc.CategoryCodeValue as CategoryID";
        private const string ExportEmailStatus = "es.Status as Emailstatus";
        private const string ExportQualifictionDate = "CONVERT(VARCHAR(10),ps.QualificationDate,1) as QDate";
        private const string ExportPubTransactionDate = "ps.PubTransactionDATE as TransactionDate";
        private const string ExportZipCode = "ps.ZipCode as Zip";
        private const string ExportRegionCode = "ps.RegionCode as State";
        private const string ExportAddress = "ps.Address1 as Address";
        private const string ExportScore = "s.Score";
        private const string ExportScoreGreaterZero = "bs.Score";
        private const string ExportBatch = "ps.Batch";
        private const string ExportQSourceName = "c.DisplayName  as QSourceName";
        private const string ExportTransactionId = "tc.TransactionCodeValue as TransactionID";
        private const string ExportFirstName = "s.FNAME as FirstName";
        private const string ExportLastName = "s.LNAME as LastName";
        private const string ExportLatLonValid = "s.IsLatLonValid as GeoLocated";
        private const string ExportParc3CNonProductView = "cp.CodeValue as Par3C";

        private static readonly string[] GroupExportColumnsProductViewDefault =
        {
            "ps.EMAIL",
            "ps.FIRSTNAME",
            "ps.LASTNAME",
            "ps.COMPANY",
            "ps.TITLE",
            "ps.ADDRESS1 as Address",
            "ps.CITY",
            "ps.REGIONCODE as State",
            "ps.COUNTRY"
        };

        private static readonly string[] GroupExportColumnsNonProductViewDefault =
        {
            "s.EMAIL",
            "s.FNAME as FirstName",
            "s.LNAME as LastName",
            "s.COMPANY",
            "s.TITLE",
            "s.ADDRESS",
            "s.CITY",
            "s.STATE",
            "s.ZIP",
            "s.COUNTRY"
        };

        private static readonly string[] GroupExportColumnsProductViewNone =
        {
            "ps.ZIPCODE as Zip",
            "ps.PHONE",
            "ps.FAX",
            "ps.MOBILE"
        };

        private static readonly string[] GroupExportColumnsNonProductViewNone =
        {
            "s.PHONE",
            "s.FAX",
            "s.MOBILE"
        };

        public static IList<string> GetStandardExportColumnFieldName(
            IList<string> columnList,
            bool productView,
            int brandId,
            bool isGroupExport,
            bool issueSplit = false)
        {
            if (columnList == null)
            {
                throw new ArgumentNullException(nameof(columnList));
            }

            var exportColumns = GetExportColums(columnList, productView, brandId);

            var columns = string.Empty;
            if (exportColumns.Count >= 0)
            {
                columns = string.Join(CommaSeparator, exportColumns.ToArray());
            }

            if (!(exportColumns.Contains(SubscriptionIdColumnNonProductViewExpected) || exportColumns.Contains(SubscriptionIdColumnProductViewExpected))
                && !issueSplit)
            {
                exportColumns.Add(
                    productView
                        ? SubscriptionIdColumnProductView
                        : SubscriptionIdColumnNonProductView);
            }

            // Add profile fields for Export to ECN if not selected
            if (isGroupExport)
            {
                var groupExportColumns = GetColumnsForGroupExport(columns, productView).ToList();

                if (groupExportColumns.Any())
                {
                    exportColumns.AddRange(groupExportColumns);
                }
            }

            return exportColumns;
        }

        private static List<string> GetExportColums(IEnumerable<string> columnList, bool productView, int brandId)
        {
            var exportColumns = new List<string>();

            foreach (var column in columnList)
            {
                var splitParts = column.Split(PipeSeparator);
                if (splitParts.Length != 2)
                {
                    continue;
                }

                var columnName = splitParts[0];
                var columnSuffix = splitParts[1];

                var exportColumn = productView
                                       ? GetExportColumnForProductView(columnName, columnSuffix, brandId)
                                       : GetExportColumnForNonProductView(columnName, columnSuffix, brandId);

                if (exportColumn != null)
                {
                    exportColumns.Add(exportColumn);
                }
            }

            return exportColumns;
        }

        private static string GetExportColumnForProductView(string column, string columnSuffix, int brandId)
        {
            switch (column.ToUpperInvariant())
            {
                case InputNameScore:
                    return brandId > 0
                               ? $"{ExportScoreGreaterZero}{PipeSeparator}{columnSuffix}"
                               : $"{ExportScore}{PipeSeparator}{columnSuffix}";
                case InputNameAddress:
                    return $"{ExportAddress}{PipeSeparator}{columnSuffix}";
                case InputNameRegionCode:
                    return $"{ExportRegionCode}{PipeSeparator}{columnSuffix}";
                case InputNameZipCode:
                    return $"{ExportZipCode}{PipeSeparator}{columnSuffix}";
                case InputNamePubTransactionDate:
                    return $"{ExportPubTransactionDate}{PipeSeparator}{columnSuffix}";
                case InputNameQualificationDate:
                    return $"{ExportQualifictionDate}{PipeSeparator}{columnSuffix}";
                case InputNameEmailStatus:
                    return $"{ExportEmailStatus}{PipeSeparator}{columnSuffix}";
                case InputNameCategoryId:
                    return $"{ExportCategoryId}{PipeSeparator}{columnSuffix}";
                case InputNameTransactionStatus:
                    return $"{ExportTransactionStatus}{PipeSeparator}{columnSuffix}";
                case InputNameTransactionId:
                    return $"{ExportTransactionId}{PipeSeparator}{columnSuffix}";
                case InputNameQSourceId:
                    return $"{ExportQSourceId}{PipeSeparator}{columnSuffix}";
                case InputNameParc3C:
                    return $"{ExportDisplayOrderAsParc3C}{PipeSeparator}{columnSuffix}";
                case InputNameTransactionName:
                    return $"{ExportTransactionName}{PipeSeparator}{columnSuffix}";
                case InputNameQSourceName:
                    return $"{ExportQSourceName}{PipeSeparator}{columnSuffix}";
                case InputNameLastOpenEdDate:
                case InputNameLastOpenPubCode:
                    return null;
                case InputNameIgrpNo:
                case InputNameCgRpNo:
                    return $"s.{column}{PipeSeparator}{columnSuffix}";
                case InputNameBatch:
                    return $"{ExportBatch}{PipeSeparator}{columnSuffix}";
                default:
                    return $"ps.{column}{PipeSeparator}{columnSuffix}";
            }
        }

        private static string GetExportColumnForNonProductView(string column, string columnSuffix, int brandId)
        {
            switch (column.ToUpperInvariant())
            {
                case InputNameScore:
                    return brandId > 0
                               ? $"{ExportScoreGreaterZero}{PipeSeparator}{columnSuffix}"
                               : $"{ExportScore}{PipeSeparator}{columnSuffix}";

                case InputNameFName:
                    return $"{ExportFirstName}{PipeSeparator}{columnSuffix}";
                case InputNameLName:
                    return $"{ExportLastName}{PipeSeparator}{columnSuffix}";
                case InputNameLatLonValid:
                    return $"{ExportLatLonValid}{PipeSeparator}{columnSuffix}";
                case InputNameCategoryId:
                    return $"{ExportCategoryId}{PipeSeparator}{columnSuffix}";
                case InputNameTransactionStatus:
                    return $"{ExportTransactionStatus}{PipeSeparator}{columnSuffix}";
                case InputNameTransactionId:
                    return $"{ExportTransactionId}{PipeSeparator}{columnSuffix}";
                case InputNameQSourceId:
                    return $"{ExportQSourceId}{PipeSeparator}{columnSuffix}";
                case InputNameParc3C:
                    return $"{ExportParc3CNonProductView}{PipeSeparator}{columnSuffix}";
                case InputNameTransactionName:
                    return $"{ExportTransactionName}{PipeSeparator}{columnSuffix}";
                case InputNameQSourceName:
                    return $"{ExportQSourceName}{PipeSeparator}{columnSuffix}";
                case InputNameLastOpenEdDate:
                case InputNameLastOpenPubCode:
                    return null;
                default:
                    return $"s.{column}{PipeSeparator}{columnSuffix}";
            }
        }

        private static IEnumerable<string> GetColumnsForGroupExport(string columns, bool isProductView)
        {
            if (isProductView)
            {
                return GetColumnsForGroupExport(
                    columns,
                    GroupExportColumnsProductViewDefault,
                    GroupExportColumnsProductViewNone);
            }

            return GetColumnsForGroupExport(
                columns,
                GroupExportColumnsNonProductViewDefault,
                GroupExportColumnsNonProductViewNone);
        }

        private static IEnumerable<string> GetColumnsForGroupExport(
            string columns,
            IEnumerable<string> exportColumnsWithDefaultSuffix,
            IEnumerable<string> exportColumnsWithNoneSuffixe)
        {
            var upperCaseColumns = columns.ToUpperInvariant();

            foreach (var groupExport in exportColumnsWithDefaultSuffix)
            {
                if (!upperCaseColumns.Contains(groupExport.ToUpperInvariant()))
                {
                    yield return $"{groupExport}{PipeSeparator}{GroupSuffixDefault}";
                }
            }

            foreach (var groupExport in exportColumnsWithNoneSuffixe)
            {
                if (!upperCaseColumns.Contains(groupExport.ToUpperInvariant()))
                {
                    yield return $"{groupExport}{PipeSeparator}{GroupSuffixNone}";
                }
            }
        }
    }
}

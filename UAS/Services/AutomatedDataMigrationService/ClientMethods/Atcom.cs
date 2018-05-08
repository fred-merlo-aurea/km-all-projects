using System.Collections.Generic;
using System.Data;
using Core.ADMS.Events;
using FrameworkUAS.Entity;
using Core_AMS.Utilities;
using KM.Common.Import;
using BusinessAdHocDimensionGroup = FrameworkUAS.BusinessLogic.AdHocDimensionGroup;
using BusinessAdHocDimension = FrameworkUAS.BusinessLogic.AdHocDimension;

namespace ADMS.ClientMethods
{
    public class Atcom : ClientSpecialCommon
    {
        private const string MatchFieldName = "MATCHFIELD";
        private const string CompanyTypeGroupName = "Atcom_Company_Type";
        private const string TypeDim = "TYPE";
        private const string TypeFieldName = "TYPE";
        private const string RegionGroupName = "Atcom_Company_Region";
        private const string RegionDim = "REGION";
        private const string OperatorGroupType = "Atcom_Company_Operator_Country";
        private const string OperatorCountryDim = "OPERATOR_COUNTRY";
        private const string CountryFieldName = "COUNTRY";

        public void AtcomCompanyAdHocImport(FileMoved eventMessage)
        {
            var ahdData = new BusinessAdHocDimension();
            ahdData.Delete(eventMessage.SourceFile.SourceFileID);

            var fileWorker = new FileWorker();
            var fileConfiguration = new FileConfiguration
            {
                FileExtension = ".xlsx"
            };
            var dataTable = fileWorker.GetData(eventMessage.ImportFile, fileConfiguration);
            var list = new List<AdHocDimension>();
            var newRows = new List<DataRow>();
            foreach (DataRow dr in dataTable.Rows)
            {
                if (dr[MatchFieldName].ToString().Contains(","))
                {
                    var strings = dr[MatchFieldName].ToString().Split(',');
                    foreach (var s in strings)
                    {
                        var newRow = dataTable.NewRow();
                        newRow.ItemArray = dr.ItemArray;
                        newRow.SetField(MatchFieldName, s);
                        newRows.Add(newRow);
                    }
                }
                else
                {
                    newRows.Add(dr);
                }
            }

            var agWorker =new BusinessAdHocDimensionGroup();
            var fillAgGroupAndTableArgsCompanyType = new FillAgGroupAndTableArgs()
            {
                Client = eventMessage.Client,
                EventMessage = eventMessage,
                AdHocDimensionGroupName = CompanyTypeGroupName,
                AhdData = ahdData,
                CreatedDimension = TypeDim,
                StandardField = CompanyStandardField,
                DimensionValueField = TypeFieldName,
                MatchValueField = MatchFieldName,
                Rows = newRows,
                DimensionOperator = EqualOperation
            };
            list.AddRange(
                ClientMethodHelpers.FillAdHocDimensionGroup(fillAgGroupAndTableArgsCompanyType, ref agWorker));

            var fillAgGroupAndTableArgsRegion = new FillAgGroupAndTableArgs()
            {
                Client = eventMessage.Client,
                EventMessage = eventMessage,
                AdHocDimensionGroupName = RegionGroupName,
                AhdData = ahdData,
                CreatedDimension = RegionDim,
                StandardField = CompanyStandardField,
                DimensionValueField = RegionDim,
                MatchValueField = MatchFieldName,
                Rows = newRows,
                DimensionOperator = EqualOperation
            };
            list.AddRange(
                ClientMethodHelpers.FillAdHocDimensionGroup(fillAgGroupAndTableArgsRegion, ref agWorker));

            var fillAgGroupAndTableArgsOperator = new FillAgGroupAndTableArgs()
            {
                Client = eventMessage.Client,
                EventMessage = eventMessage,
                AdHocDimensionGroupName = OperatorGroupType,
                AhdData = ahdData,
                CreatedDimension = OperatorCountryDim,
                StandardField = CompanyStandardField,
                DimensionValueField = CountryFieldName,
                MatchValueField = MatchFieldName,
                Rows = newRows,
                DimensionOperator = EqualOperation
            };
            list.AddRange(
                ClientMethodHelpers.FillAdHocDimensionGroup(fillAgGroupAndTableArgsOperator, ref agWorker));

            ahdData.SaveBulkSqlInsert(list);
        }
    }
}

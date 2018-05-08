using System;
using System.Collections.Generic;
using System.Data;
using Core.ADMS.Events;
using FrameworkUAS.Entity;
using KMPlatform.Entity;

namespace ADMS.ClientMethods
{
    internal class FillAgGroupAndTableArgs
    {
        public FillAgGroupAndTableArgs()
        {
            IsActive = true;
            UpdateUAD = true;
        }

        public Client Client { get; set; }
        public FileMoved EventMessage { get; set; }
        public int SourceFileId { get; set; }
        public DataTable Dt { get; set; }
        public IList<DataRow> Rows { get; set; }
        public FrameworkUAS.BusinessLogic.AdHocDimension AhdData { get; set; }
        public string StandardField { get; set; }
        public string AdHocDimensionGroupName { get; set; }
        public string CreatedDimension { get; set; }
        public string DimensionOperator { get; set; }
        public string MatchValueField { get; set; }
        public string DimensionValue { get; set; }
        public string DimensionValueField { get; set; }
        public string DefaultValue { get; set; }
        public bool IsPubcodeSpecific { get; set; }
        public Func<DataRow, string> MatchValueFunc { get; set; }
        public bool IsActive { get; set; }
        public bool UpdateUAD { get; set; }
        public string FileExtension { get; set; }
        public string FileColumnDelimiter { get; set; }
        public Action<AdHocDimensionGroup> AdditionalInitFunction { get; set; }
    }
}

using Core.ADMS.Events;
using FrameworkUAS.Entity;
using Core_AMS.Utilities;
using System.Data;
using System.Collections.Generic;
using System;

namespace ADMS.ClientMethods
{
    class SpecialtyFoods : ClientSpecialCommon
    {
        private const string AdHocDimensionGroupSpecialtyFoodsCompanyBrnFlag = "SpecialtyFoods_Company_BRNFLAG";
        private const string AdHocDimensionGroupNameSpecialtyFoodsCompanyKbFlag = "SpecialtyFoods_Company_KBFLAG";
        private const string DimensionBrnFlag = "BRNFLAG";
        private const string DimensionKbFlag = "KBFLAG";
        private const string MatchValueFieldCompany = "COMPANY";

        public void BRNFLAGAdHocImport(KMPlatform.Entity.Client client, FileMoved eventMessage)
        {
            var ahdData = new FrameworkUAS.BusinessLogic.AdHocDimension();
            ahdData.Delete(eventMessage.SourceFile.SourceFileID);

            var fileWorker = new FileWorker();
            var dt = fileWorker.GetData(eventMessage.ImportFile);

            var fillAgGroupAndTableArgs = new FillAgGroupAndTableArgs()
            {
                Client = client,
                EventMessage = eventMessage,
                SourceFileId = eventMessage.SourceFile.SourceFileID,
                Dt = dt,
                AhdData = ahdData,
                StandardField = StandardFieldCompany,
                AdHocDimensionGroupName = AdHocDimensionGroupSpecialtyFoodsCompanyBrnFlag,
                CreatedDimension = DimensionBrnFlag,
                DimensionOperator = ContainsOperation,
                MatchValueField = MatchValueFieldCompany,
                DimensionValue = "Y"
            };
            ClientMethodHelpers.FillAgGroupAndTable(fillAgGroupAndTableArgs);
        }

        public void KBFLAGAdHocImport(KMPlatform.Entity.Client client, FileMoved eventMessage)
        {
            FrameworkUAS.BusinessLogic.AdHocDimension ahdData = new FrameworkUAS.BusinessLogic.AdHocDimension();
            ahdData.Delete(eventMessage.SourceFile.SourceFileID);

            Core_AMS.Utilities.FileWorker fw = new FileWorker();
            DataTable dt = fw.GetData(eventMessage.ImportFile);
            List<FrameworkUAS.Entity.AdHocDimension> list = new List<AdHocDimension>();

            var fillAgGroupAndTableArgs = new FillAgGroupAndTableArgs()
            {
                Client = client,
                EventMessage = eventMessage,
                SourceFileId = eventMessage.SourceFile.SourceFileID,
                Dt = dt,
                AhdData = ahdData,
                StandardField = StandardFieldCompany,
                AdHocDimensionGroupName = AdHocDimensionGroupNameSpecialtyFoodsCompanyKbFlag,
                CreatedDimension = DimensionKbFlag,
                DimensionOperator = ContainsOperation,
                MatchValueField = MatchValueFieldCompany,
                DimensionValue = "Y"
            };

            ClientMethodHelpers.FillAgGroupAndTable(fillAgGroupAndTableArgs);
        }
    }
}

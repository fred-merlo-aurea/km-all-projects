using System.Diagnostics.CodeAnalysis;

namespace ECN.Framework.DataLayer.Tests.Communicator.Common
{
    [ExcludeFromCodeCoverage]
    public static class Consts
    {
        public static readonly string ParamAlias = "@Alias";
        public static readonly string ParamAliasId = "@AliasID";
        public static readonly string ParamArchived = "@Archived";
        public static readonly string ParamArchiveFilter = "@ArchiveFilter";
        public static readonly string ParamBaseChannelId = "@BaseChannelID";
        public static readonly string ParamBlastId = "@BlastID";
        public static readonly string ParamCampaignId = "@CampaignID";
        public static readonly string ParamCampaignItemId = "@CampaignItemID";
        public static readonly string ParamCampaignItemBlastId = "@CampaignItemBlastID";
        public static readonly string ParamContentId = "@ContentID";
        public static readonly string ParamContentTitle = "@ContentTitle";
        public static readonly string ParamCompositeKey = "@compositekey";
        public static readonly string ParamCurrentPage = "@CurrentPage";
        public static readonly string ParamCustomerId = "@CustomerID";
        public static readonly string ParamEmailId = "@EmailID";
        public static readonly string ParamEmailAddressOnly = "@EmailAddressOnly";
        public static readonly string ParamFdId = "@FDID";
        public static readonly string ParamFilterId = "@FilterID";
        public static readonly string ParamFilterGroupId = "@FilterGroupID";
        public static readonly string ParamFilterConditionId = "@FilterConditionID";
        public static readonly string ParamFilename = "@filename";
        public static readonly string ParamFolderId = "@FolderID";
        public static readonly string ParamFormatTypeCode = "@formattypecode";
        public static readonly string ParamGroupId = "@GroupID";
        public static readonly string ParamGroupDataFieldsId = "@GroupDataFieldsID";
        public static readonly string ParamLayoutId = "@LayoutID";
        public static readonly string ParamLayoutName = "@LayoutName";
        public static readonly string ParamLayoutPlanId = "@LayoutPlanID";
        public static readonly string ParamLink = "@Link";
        public static readonly string ParamLinkId = "@LinkID";
        public static readonly string ParamLinkName = "@LinkName";
        public static readonly string ParamLinkOwnerIndexId = "@LinkOwnerIndexID";
        public static readonly string ParamLinkTypeId = "@LinkTypeID";
        public static readonly string ParamOverwriteWithNull = "@overwritewithNULL";
        public static readonly string ParamOwnerId = "@OwnerID";
        public static readonly string ParamPageSize = "@PageSize";
        public static readonly string ParamSource = "@Source";
        public static readonly string ParamSecondarySource = "@source";
        public static readonly string ParamSortColumn = "@SortColumn";
        public static readonly string ParamSortDirection = "@SortDirection";
        public static readonly string ParamSubscribeTypeCode = "@subscribetypecode";
        public static readonly string ParamTemplateId = "@TemplateID";
        public static readonly string ParamUserId = "@UserID";
        public static readonly string ParamUpdatedUserId = "@UpdatedUserID";
        public static readonly string ParamUpdatedDateFrom = "@UpdatedDateFrom";
        public static readonly string ParamUpdatedDateTo = "@UpdatedDateTo";
        public static readonly string ParamXmlProfile = "@xmlProfile";
        public static readonly string ParamXmlUdf = "@xmlUDF";
        public static readonly string ParamValidatedOnly = "@ValidatedOnly";

        public static readonly string ProcedureBlastActiveOrSent = "e_Blast_ActiveOrSent";
        public static readonly string ProcedureBlastExistsByBlastId = "e_Blast_Exists_ByBlastID";
        public static readonly string ProcedureBlastSingleDeleteEmailId = "e_BlastSingle_Delete_EmailID";
        public static readonly string ProcedureCampaignItemBlastUpdateBlastId = "e_CampaignItemBlast_UpdateBlastID";
        public static readonly string ProcedureCampaignItemDeleteAll = "e_CampaignItem_Delete_All";
        public static readonly string ProcedureCampaignItemDeleteSingle = "e_CampaignItem_Delete_Single";
        public static readonly string ProcedureCampaignItemBlastDeleteAll = "e_CampaignItemBlast_Delete_All";
        public static readonly string ProcedureCampaignItemBlastDeleteSingle = "e_CampaignItemBlast_Delete_Single";
        public static readonly string ProcedureCampaignSelectCampaignId = "e_Campaign_Select_CampaignID";
        public static readonly string ProcedureCodeExistsLinkTypeId = "e_Code_Exists_LinkTypeID";
        public static readonly string ProcedureContentSelectTitle = "v_Content_Select_Title";
        public static readonly string ProcedureContentFilterDetailDeleteAll = "e_ContentFilterDetail_Delete_All";
        public static readonly string ProcedureContentFilterDetailDeleteSingle = "e_ContentFilterDetail_Delete_Single";
        public static readonly string ProcedureContentFilterDeleteAll = "e_ContentFilter_Delete_All";
        public static readonly string ProcedureContentFilterDeleteSingle = "e_ContentFilter_Delete_Single";
        public static readonly string ProcedureConversionLinksDeleteAll = "e_ConversionLinks_Delete_All";
        public static readonly string ProcedureConversionLinksExistsByName = "e_ConversionLinks_Exists_ByName";
        public static readonly string ProcedureEmailDataValuesDeleteAll = "e_EmailDataValues_Delete_All";
        public static readonly string ProcedureEmailDataValuesDeleteSingle = "e_EmailDataValues_Delete_Single";
        public static readonly string ProcedureEmailGroupDeleteGroupIdEmailId = "e_EmailGroup_Delete_GroupID_EmailID";
        public static readonly string ProcedureEmailGroupDeleteGroupId = "e_EmailGroup_Delete_GroupID";
        public static readonly string ProcedureEmailGroupImportEmails = "e_EmailGroup_ImportEmails";
        public static readonly string ProcedureEmailGroupImportMsEmails = "e_EmailGroup_ImportMSEmails";
        public static readonly string ProcedureEmailGroupImportEmailsWithDupes = "e_EmailGroup_ImportEmailsWithDupes";
        public static readonly string ProcedureConversionLinksDeleteSingle = "e_ConversionLinks_Delete_Single";
        public static readonly string ProcedureFilterDeleteFilterId = "e_Filter_Delete_FilterID";
        public static readonly string ProcedureFilterGroupDeleteFilterId = "e_FilterGroup_Delete_FilterID";
        public static readonly string ProcedureFilterGroupDeleteFilterGroupId = "e_FilterGroup_Delete_FilterGroupID";
        public static readonly string ProcedureFilterConditionDeleteFilterGroupId = "e_FilterCondition_Delete_FilterGroupID";
        public static readonly string ProceudreFilterConditionDeleteFilterConditionId = "e_FilterCondition_Delete_FilterConditionID";
        public static readonly string ProcedureGroupDataFieldsDeleteSingle = "e_GroupDataFields_Delete_Single";
        public static readonly string ProcedureGroupDataFieldsDeleteAll = "e_GroupDataFields_Delete_All";
        public static readonly string ProcedureGroupDataFieldsSelectDataFieldSetId = "e_GroupDataFields_Select_DataFieldSetID";
        public static readonly string ProcedureGroupDataFieldsSelectGroupId = "e_GroupDataFields_Select_GroupID";
        public static readonly string ProcedureGroupDataFieldsSelectGroupDataFieldsId = "e_GroupDataFields_Select_GroupDataFieldsID";
        public static readonly string ProcedureLayoutExistsByName = "e_Layout_Exists_ByName";
        public static readonly string ProcedureLayoutExistsContentId = "e_Layout_Exists_ContentID";
        public static readonly string ProcedureLayoutExistsTemplateId = "e_Layout_Exists_TemplateID";
        public static readonly string ProcedureLayoutSelectSearch = "e_Layout_Select_Search";
        public static readonly string ProcedureLayoutIsValidatedById = "e_Layout_IsValidated_ByID";
        public static readonly string ProcedureLinkAliasExistsByAlias = "e_LinkAlias_Exists_ByAlias";
        public static readonly string ProcedureLinkAliasDeleteAll = "e_LinkAlias_Delete_All";
        public static readonly string ProcedureLinkAliasSelectLink = "e_LinkAlias_Select_Link";
        public static readonly string ProcedureLinkAliasSelectOwnerId = "e_LinkAlias_Select_OwnerID";
        public static readonly string ProcedureLinkOwnerIndexSelectAll = "e_LinkOwnerIndex_Select_All";
        public static readonly string ProcedureLinkOwnerIndexSelectSingle = "e_LinkOwnerIndex_Select_Single";
        public static readonly string ProcedureLinkOwnerIndexDeleteAll = "e_LinkOwnerIndex_Delete_All";
        public static readonly string ProcedureLayoutPlansDeleteByLPID = "e_LayoutPlans_Delete_ByLPID";
        public static readonly string ProcedureLayoutPlansDeleteSingle = "e_LayoutPlans_Delete_Single";

        public static readonly string ProcedureBlastSingleExistsByBlastEmailLayoutPlan =
            "e_BlastSingle_Exists_ByBlastEmailLayoutPlan";

        public static readonly string ProcedureBlastSingleDeleteNoOpenFromAbandonEmailId =
            "e_BlastSingle_Delete_NoOpenFromAbandon_EmailID";

        public static readonly string ProcedureCampaignItemBlastSelectCampaignItemBlastId =
            "e_CampaignItemBlast_Select_CampaignItemBlastID";

        public static readonly string ProcedureCampaignItemBlastSelectCampaignItemId =
            "e_CampaignItemBlast_Select_CampaignItemID";

        public static readonly string ProcedureCampaignItemBlastSelectBlastId = 
            "e_CampaignItemBlast_Select_BlastID";

        public static readonly string ProcedureEmailGroupImportEmailsPreImportResults =
            "e_EmailGroup_ImportEmails_PreImportResults";
    }
}

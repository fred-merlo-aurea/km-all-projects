CREATE PROCEDURE [dbo].[e_Group_Exists_CampaignItemTemplate]
	@GroupID int,
	@CustomerID int
AS
	if exists (Select top 1 * From CampaignItemTemplateGroup c with(nolock)
							join CampaignItemTemplates cit with(nolock) on c.CampaignItemTemplateID = cit.CampaignItemTemplateID
							where c.GroupID = @GroupID and cit.CustomerID = @CustomerID and c.IsDeleted = 0 and cit.IsDeleted = 0)
		SELECT 1
	ELSE IF exists(Select top 1 * from CampaignItemTemplateSuppressionGroup c with(nolock)
								join CampaignItemTemplates cit with(nolock) on c.CampaignItemTemplateID = cit.CampaignItemTemplateID
								where c.GroupID = @GroupID and cit.CustomerID = @CustomerID and c.IsDeleted = 0 and cit.IsDeleted = 0)
		SELECT 1
	ELSE
		SELECT 0


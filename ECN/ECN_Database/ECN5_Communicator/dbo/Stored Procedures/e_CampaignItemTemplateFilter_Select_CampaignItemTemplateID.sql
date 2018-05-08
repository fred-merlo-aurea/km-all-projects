CREATE PROCEDURE [dbo].[e_CampaignItemTemplateFilter_Select_CampaignItemTemplateID]
	@CampaignItemTemplateID int,
	@GroupID int
AS
	SELECT * 
	FROM CampaignItemTemplateFilter citf with(nolock) 
	WHERE citf.CampaignItemTemplateID = @CampaignItemTemplateID and citf.GroupID = @GroupID and ISNULL(citf.IsDeleted, 0) = 0

CREATE PROCEDURE [dbo].[e_CampaignItemTemplateGroup_Select_CampaignItemTemplateID]
	@CampaignItemTemplateID int
AS
	SELECT * 
	FROM CampaignItemTemplateGroup citg with(nolock) 
	WHERE citg.CampaignItemTemplateID = @CampaignItemTemplateID and ISNULL(citg.IsDeleted, 0) = 0

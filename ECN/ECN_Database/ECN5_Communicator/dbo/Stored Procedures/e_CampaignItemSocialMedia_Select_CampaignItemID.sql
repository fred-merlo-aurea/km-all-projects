CREATE PROCEDURE [dbo].[e_CampaignItemSocialMedia_Select_CampaignItemID]
	@CampaignItemID int
AS
	SELECT * 
	FROM CampaignItemSocialMedia cism with(nolock)
	WHERE cism.CampaignItemID = @CampaignItemID

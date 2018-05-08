CREATE PROCEDURE [dbo].[e_CampaignItemMetaTag_Select_CampaignItemID]
	@CampaignItemID int
AS
	SELECT *
	FROM CampaignItemMetaTag cimt with(nolock)
	WHERE cimt.CampaignItemID = @CampaignItemID and cimt.IsDeleted = 0


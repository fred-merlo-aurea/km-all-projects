CREATE PROCEDURE [dbo].[e_SimpleShareDetail_Select_CampaignItemID]
	@CampaignItemID int
AS
	SELECT * 
	FROM SimpleShareDetail ssd with(nolock)
	WHERE ssd.CampaignItemID = @CampaignItemID and ssd.IsDeleted = 0

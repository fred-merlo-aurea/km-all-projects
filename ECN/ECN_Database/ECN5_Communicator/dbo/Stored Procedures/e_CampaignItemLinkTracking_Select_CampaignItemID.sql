CREATE PROCEDURE [dbo].[e_CampaignItemLinkTracking_Select_CampaignItemID]   
@CampaignItemID int
AS
	SELECT cilt.*
	FROM 
		CampaignItemLinkTracking cilt WITH (NOLOCK)
		JOIN CampaignItem ci WITH (NOLOCK) on cilt.CampaignItemID = ci.CampaignItemID
	WHERE cilt.CampaignItemID = @CampaignItemID and ci.IsDeleted = 0
CREATE PROCEDURE [dbo].[e_CampaignItem_Select_CampaignItemID]   
@CampaignItemID int
AS
	SELECT ci.*, c.CustomerID 
	FROM CampaignItem ci WITH (NOLOCK)
		JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID 
	WHERE ci.CampaignItemID = @CampaignItemID and ci.IsDeleted = 0 and c.IsDeleted = 0
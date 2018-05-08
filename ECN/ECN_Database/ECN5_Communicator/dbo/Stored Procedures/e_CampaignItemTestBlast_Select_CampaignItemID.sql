CREATE PROCEDURE [dbo].[e_CampaignItemTestBlast_Select_CampaignItemID]   
@CampaignItemID int
AS
	SELECT citb.*, c.CustomerID 
	FROM CampaignItemTestBlast citb WITH (NOLOCK) 
		JOIN CampaignItem ci WITH (NOLOCK) ON citb.CampaignItemID = ci.CampaignItemID
		JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID 
		WHERE ci.CampaignItemID = @CampaignItemID and citb.IsDeleted = 0 and ci.IsDeleted = 0 and c.IsDeleted = 0
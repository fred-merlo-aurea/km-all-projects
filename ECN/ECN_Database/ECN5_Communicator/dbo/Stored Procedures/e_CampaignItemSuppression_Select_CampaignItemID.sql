CREATE PROCEDURE [dbo].[e_CampaignItemSuppression_Select_CampaignItemID]   
@CampaignItemID int
AS
	SELECT cis.*, c.CustomerID 
	FROM CampaignItemSuppression cis WITH (NOLOCK) 
		JOIN CampaignItem ci WITH (NOLOCK) ON cis.CampaignItemID = ci.CampaignItemID
		JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID 
	WHERE ci.CampaignItemID = @CampaignItemID and cis.IsDeleted = 0 and ci.IsDeleted = 0 and c.IsDeleted = 0
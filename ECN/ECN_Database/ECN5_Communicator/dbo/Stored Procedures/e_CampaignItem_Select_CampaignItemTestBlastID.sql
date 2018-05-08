CREATE PROCEDURE [dbo].[e_CampaignItem_Select_CampaignItemTestBlastID]   
@CampaignItemTestBlastID int
AS
	SELECT ci.*, c.CustomerID 
	FROM CampaignItemTestBlast citb WITH (NOLOCK)
		JOIN CampaignItem ci WITH (NOLOCK) ON citb.CampaignItemID = ci.CampaignItemID
		JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID 
	WHERE citb.CampaignItemTestBlastID = @CampaignItemTestBlastID and citb.IsDeleted = 0 and ci.IsDeleted = 0 and c.IsDeleted = 0
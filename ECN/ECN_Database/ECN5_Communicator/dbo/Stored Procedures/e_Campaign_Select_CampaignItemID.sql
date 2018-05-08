CREATE PROCEDURE [dbo].[e_Campaign_Select_CampaignItemID]   
@CampaignItemID int
AS
	SELECT c.* FROM Campaign c WITH (NOLOCK) JOIN CampaignItem ci WITH (NOLOCK) ON c.CampaignID = ci.CampaignID WHERE ci.CampaignItemID = @CampaignItemID and c.IsDeleted = 0 and ci.IsDeleted = 0
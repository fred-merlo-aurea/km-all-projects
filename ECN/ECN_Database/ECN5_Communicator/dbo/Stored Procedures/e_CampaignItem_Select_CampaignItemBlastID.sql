CREATE PROCEDURE [dbo].[e_CampaignItem_Select_CampaignItemBlastID]   
@CampaignItemBlastID int
AS
	SELECT ci.*, c.CustomerID 
	FROM CampaignItemBlast cib WITH (NOLOCK)
		JOIN CampaignItem ci WITH (NOLOCK) ON cib.CampaignItemID = ci.CampaignItemID
		JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID 
	WHERE cib.CampaignItemBlastID = @CampaignItemBlastID and cib.IsDeleted = 0 and ci.IsDeleted = 0 and c.IsDeleted = 0
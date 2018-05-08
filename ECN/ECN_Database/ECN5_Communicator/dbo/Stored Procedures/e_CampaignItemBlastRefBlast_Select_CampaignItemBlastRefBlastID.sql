CREATE PROCEDURE [dbo].[e_CampaignItemBlastRefBlast_Select_CampaignItemBlastRefBlastID]   
@CampaignItemBlastRefBlastID int
AS
	SELECT cibrb.*, c.CustomerID 
	FROM CampaignItemBlastRefBlast cibrb WITH (NOLOCK)
		JOIN CampaignItemBlast cib WITH (NOLOCK) ON cibrb.CampaignItemBlastID = cib.CampaignItemBlastID
		JOIN CampaignItem ci WITH (NOLOCK) ON cib.CampaignItemID = ci.CampaignItemID
        JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID 
    WHERE cibrb.CampaignItemBlastRefBlastID = @CampaignItemBlastRefBlastID and cibrb.IsDeleted = 0 and cib.IsDeleted = 0 and ci.IsDeleted = 0 and c.IsDeleted = 0
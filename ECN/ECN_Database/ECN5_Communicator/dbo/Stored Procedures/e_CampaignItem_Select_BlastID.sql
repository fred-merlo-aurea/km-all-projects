CREATE PROCEDURE [dbo].[e_CampaignItem_Select_BlastID]   
@BlastID int
AS
	SELECT ci.*, c.CustomerID 
	FROM CampaignItemBlast cib WITH (NOLOCK)
		JOIN CampaignItem ci WITH (NOLOCK) ON cib.CampaignItemID = ci.CampaignItemID
		JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID 
	WHERE cib.BlastID = @BlastID and cib.IsDeleted = 0 and ci.IsDeleted = 0 and c.IsDeleted = 0
CREATE PROCEDURE [dbo].[e_Blast_Select_CampaignID]
@CampaignID int
AS
SELECT b.*
FROM 
	Blast b WITH(NOLOCK)
	JOIN CampaignItemBlast cib WITH (NOLOCK) ON b.BlastID = cib.BlastID
	JOIN CampaignItem ci WITH (NOLOCK) ON cib.CampaignItemID = ci.CampaignItemID
	JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID
WHERE 
	c.CampaignID = @CampaignID AND
	c.IsDeleted = 0 AND
	ci.IsDeleted = 0 AND
	cib.IsDeleted = 0 AND
	b.StatusCode <> 'Deleted'

CREATE PROCEDURE [dbo].[e_Blast_Select_CampaignItemID]
@CampaignItemID int
AS
SELECT b.*
FROM 
	Blast b WITH(NOLOCK)
	JOIN CampaignItemBlast cib WITH (NOLOCK) ON b.BlastID = cib.BlastID
WHERE 
	cib.CampaignItemID = @CampaignItemID AND
	cib.IsDeleted = 0 AND
	b.StatusCode <> 'Deleted'
UNION 
SELECT b.*
FROM 
	Blast b WITH(NOLOCK)
	JOIN CampaignItemTestBlast citb WITH (NOLOCK) ON b.BlastID = citb.BlastID
WHERE 
	citb.CampaignItemID = @CampaignItemID AND
	citb.IsDeleted = 0 AND
	b.StatusCode <> 'Deleted'
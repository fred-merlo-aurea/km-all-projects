CREATE PROCEDURE [dbo].[e_Blast_Select_CampaignItemTestBlastID]
@CampaignItemTestBlastID int
AS
SELECT b.*
FROM 
	Blast b WITH(NOLOCK)
	JOIN CampaignItemTestBlast citb WITH (NOLOCK) ON b.BlastID = citb.BlastID
WHERE 
	citb.CampaignItemTestBlastID = @CampaignItemTestBlastID AND
	citb.IsDeleted = 0 AND
	b.StatusCode <> 'Deleted'

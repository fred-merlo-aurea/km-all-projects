CREATE PROCEDURE [dbo].[e_Blast_Select_CampaignItemBlastID]
@CampaignItemBlastID int
AS
SELECT b.*
FROM 
	Blast b WITH(NOLOCK)
	JOIN CampaignItemBlast cib WITH (NOLOCK) ON b.BlastID = cib.BlastID
WHERE 
	cib.CampaignItemBlastID = @CampaignItemBlastID AND
	cib.IsDeleted = 0 AND
	b.StatusCode <> 'Deleted'

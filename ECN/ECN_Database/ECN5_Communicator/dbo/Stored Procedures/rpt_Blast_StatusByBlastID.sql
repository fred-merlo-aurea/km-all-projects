CREATE PROCEDURE [dbo].[rpt_Blast_StatusByBlastID]
	@BlastID int
AS
BEGIN
SELECT 
	b.*, ci.CampaignItemName, c.CustomerName, g.GroupName
FROM 
	Blast b WITH(NOLOCK)
	LEFT OUTER JOIN CampaignItemBlast cib WITH (NOLOCK) ON b.BlastID = cib.BlastID
	LEFT OUTER JOIN CampaignItemTestBlast citb WITH (NOLOCK) ON b.BlastID = citb.BlastID
	LEFT OUTER JOIN CampaignItem ci WITH (NOLOCK) ON ci.CampaignItemID = case when cib.CampaignItemID is null then citb.CampaignItemID else cib.CampaignItemID end
	JOIN ecn5_accounts..Customer c WITH (NOLOCK) ON b.CustomerID = c.CustomerID 
	JOIN Groups g WITH (NOLOCK) ON b.GroupID = g.groupID	
WHERE 
	b.BlastID = @BlastID AND
	b.StatusCode <> 'Deleted'
END
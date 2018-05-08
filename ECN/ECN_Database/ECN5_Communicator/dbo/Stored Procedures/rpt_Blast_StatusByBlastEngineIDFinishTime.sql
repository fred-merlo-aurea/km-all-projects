CREATE PROCEDURE [dbo].[rpt_Blast_StatusByBlastEngineIDFinishTime]
	@BlastEngineID int,
	@FinishTime datetime
AS
BEGIN
SELECT 
	top 1 b.*, ci.CampaignItemName, c.CustomerName, g.GroupName
FROM 
	Blast b WITH(NOLOCK)
	LEFT OUTER JOIN CampaignItemBlast cib WITH (NOLOCK) ON b.BlastID = cib.BlastID
	LEFT OUTER JOIN CampaignItem ci WITH (NOLOCK) ON cib.CampaignItemID = ci.CampaignItemID
	LEFT OUTER JOIN ecn5_accounts..Customer c WITH (NOLOCK) ON b.CustomerID = c.CustomerID 
	LEFT OUTER JOIN Groups g WITH (NOLOCK) ON b.GroupID = g.groupID
WHERE 
	BlastEngineID = @BlastEngineID and FinishTime < @FinishTime  and StatusCode <> 'Deleted' order by FinishTime desc
END
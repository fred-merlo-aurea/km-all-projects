CREATE PROCEDURE [dbo].[e_Campaign_Select_BlastID]   
@BlastID int
AS
	SELECT c.* 
	FROM 
		Campaign c WITH (NOLOCK) 
		JOIN CampaignItem ci WITH (NOLOCK) ON c.CampaignID = ci.CampaignID 
		JOIN CampaignItemBlast cib WITH (NOLOCK) ON ci.CampaignItemID = cib.CampaignItemID
	WHERE 
		cib.BlastID = @BlastID and 
		c.IsDeleted = 0 and 
		ci.IsDeleted = 0 and
		cib.IsDeleted = 0
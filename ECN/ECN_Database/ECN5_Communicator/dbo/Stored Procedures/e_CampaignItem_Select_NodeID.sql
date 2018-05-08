CREATE PROCEDURE [dbo].[e_CampaignItem_Select_NodeID]   
@NodeID varchar(100)
AS
	SELECT ci.*, c.CustomerID 
	FROM CampaignItem ci WITH (NOLOCK)
		JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID 
	WHERE ci.NodeID = @NodeID and ci.IsDeleted = 0 and c.IsDeleted = 0
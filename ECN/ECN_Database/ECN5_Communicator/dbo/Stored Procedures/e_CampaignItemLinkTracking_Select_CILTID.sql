CREATE PROCEDURE [dbo].[e_CampaignItemLinkTracking_Select_CILTID]   
@CILTID int
AS
	SELECT *
	FROM 
		CampaignItemLinkTracking WITH (NOLOCK)
	WHERE CILTID = @CILTID
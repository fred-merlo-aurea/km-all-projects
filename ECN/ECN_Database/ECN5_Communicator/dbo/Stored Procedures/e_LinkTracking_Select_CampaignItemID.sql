CREATE PROCEDURE [dbo].[e_LinkTracking_Select_CampaignItemID]   
@CampaignItemID int
AS


SELECT distinct lt.*
	FROM 
		CampaignItemLinkTracking cilt WITH (NOLOCK)
		JOIN CampaignItem ci WITH (NOLOCK) on cilt.CampaignItemID = ci.CampaignItemID
		JOIN LinkTrackingParam ltp WITH (NOLOCK) ON ltp.LTPID = cilt.LTPID 
		JOIN LinkTracking lt WITH (NOLOCK) ON lt.LTID = ltp.LTID 
	WHERE cilt.CampaignItemID = @CampaignItemID and ci.IsDeleted = 0 and lt.IsActive=1
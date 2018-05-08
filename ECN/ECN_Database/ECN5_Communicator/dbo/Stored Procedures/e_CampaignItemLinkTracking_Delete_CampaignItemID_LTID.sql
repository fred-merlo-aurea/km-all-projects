CREATE PROCEDURE [dbo].[e_CampaignItemLinkTracking_Delete_CampaignItemID_LTID]
@LTID int,
@CampaignItemID int,
@UserID int
AS	
	
	delete from CampaignItemLinkTracking where CampaignItemID=@CampaignItemID and LTPID in
	(select LTPID from LinkTrackingParam where LTID=@LTID)
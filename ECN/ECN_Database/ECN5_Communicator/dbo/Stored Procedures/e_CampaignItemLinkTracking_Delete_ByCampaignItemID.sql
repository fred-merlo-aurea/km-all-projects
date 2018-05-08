CREATE PROCEDURE [dbo].[e_CampaignItemLinkTracking_Delete_ByCampaignItemID]
@CampaignItemID int,
@CustomerID int,
@UserID int
AS	
	delete from CampaignItemLinkTracking where CampaignItemID=@CampaignItemID
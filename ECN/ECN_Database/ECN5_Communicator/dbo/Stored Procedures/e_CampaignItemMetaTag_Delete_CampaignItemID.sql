CREATE PROCEDURE [dbo].[e_CampaignItemMetaTag_Delete_CampaignItemID]
	@CampaignItemID int,
	@UpdatedUserID int
AS
	Update CampaignItemMetaTag 
	set IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UpdatedUserID
	WHERE CampaignItemID = @CampaignItemID and IsDeleted = 0
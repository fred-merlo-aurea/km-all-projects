CREATE PROCEDURE [dbo].[e_CampaignItemMetaTag_Delete_SocialMediaID_CampaignItemID]
	@SocialMediaID int,
	@CampaignItemID int,
	@UpdatedUserID int
AS
	Update CampaignItemMetaTag
	set IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UpdatedUserID
	where CampaignItemID = @CampaignItemID and SocialMediaID = @SocialMediaID and IsDeleted = 0

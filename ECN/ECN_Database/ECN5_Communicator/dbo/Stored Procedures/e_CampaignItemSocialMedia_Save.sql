CREATE PROCEDURE [dbo].[e_CampaignItemSocialMedia_Save]
	@CampaignItemSocialMediaID int = null,
	@CampaignItemID int,
	@SocialMediaID int,
	@SimpleShareDetailID int = null,
	@SocialMediaAuthID int = null,
	@Status varchar(10),
	@PageID varchar(MAX) = null,
	@PageAccessToken varchar(MAX) = null,
	@PostID varchar(MAX) = null,
	@LastErrorCode int = null
AS
	if(@CampaignItemSocialMediaID is null)
	BEGIN
		INSERT INTO CampaignItemSocialMedia(CampaignItemID, SocialMediaID, SimpleShareDetailID,SocialMediaAuthID,Status, StatusDate, PageID, PageAccessToken, PostID)
		VALUES(@CampaignItemID, @SocialMediaID, @SimpleShareDetailID,@SocialMediaAuthID, @Status, GetDate(), @PageID, @PageAccessToken, @PostID)
		SELECT @@IDENTITY;
	END
	ELSE
	BEGIN
		Update CampaignItemSocialMedia
		set CampaignItemID = @CampaignItemID, SocialMediaID = @SocialMediaID, SimpleShareDetailID = @SimpleShareDetailID, SocialMediaAuthID = @SocialMediaAuthID, Status = @Status, StatusDate = GetDate(), PageID = @PageID, PageAccessToken = @PageAccessToken, PostID = @PostID, LastErrorCode = @LastErrorCode
		WHERE CampaignItemSocialMediaID = @CampaignItemSocialMediaID
		SELECT @CampaignItemSocialMediaID
	END

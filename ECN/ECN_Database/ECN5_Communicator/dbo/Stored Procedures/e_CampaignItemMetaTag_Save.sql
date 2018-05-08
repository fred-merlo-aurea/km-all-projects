CREATE PROCEDURE [dbo].[e_CampaignItemMetaTag_Save]
	@CampaignItemMetaTagID int = null,
	@CampaignItemID int,
	@Property varchar(100),
	@Content varchar(MAX),
	@SocialMediaID int,
	@CreatedUserID int = null,
	@UpdatedUserID int = null,
	@IsDeleted bit
AS
	IF @CampaignItemMetaTagID is null
		BEGIN
			INSERT INTO CampaignItemMetaTag(CampaignItemID, Property,Content, SocialMediaID, CreatedDate,  CreatedUserID,  IsDeleted)
			VALUES(@CampaignItemID, @Property, @Content, @SocialMediaID, GETDATE(), @CreatedUserID, 0)
			SELECT @@IDENTITY;
		END
	ELSE
		BEGIN
			UPDATE CampaignItemMetaTag
			SET CampaignItemID = @CampaignItemID, Property = @Property, Content = @Content, SocialMediaID = @SocialMediaID, UpdatedDate = GETDATE(), UpdatedUserID = @UpdatedUserID, IsDeleted = @IsDeleted
			WHERE CampaignItemMetaTagID = @CampaignItemMetaTagID
			SELECT @CampaignItemMetaTagID
		END
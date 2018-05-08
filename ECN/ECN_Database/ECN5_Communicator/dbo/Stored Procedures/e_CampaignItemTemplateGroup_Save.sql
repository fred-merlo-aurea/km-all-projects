CREATE PROCEDURE [dbo].[e_CampaignItemTemplateGroup_Save]
	@CampaignItemTemplateGroupID int,
	@CampaignItemTemplateID int,
	@GroupID int,
	@IsDeleted bit,
	@UpdatedUserID int = null,
	@CreatedUserID int = null
AS
	if(@CampaignItemTemplateGroupID > 0)
	BEGIN
		UPDATE CampaignItemTemplateGroup
		SET IsDeleted = @IsDeleted, UpdatedDate = GETDATE(), UpdatedUserID = @UpdatedUserID
		WHERE CampaignItemTemplateGroupId = @CampaignItemTemplateGroupID
		SELECT @CampaignItemTemplateGroupID
	END
	ELSE
	BEGIN
		INSERT INTO CampaignItemTemplateGroup(CampaignItemTemplateID, GroupID, IsDeleted, CreatedDate, CreatedUserID)
		VALUES(@CampaignItemTemplateID, @GroupID, @IsDeleted, GETDATE(), @CreatedUserID)
		SELECT @@IDENTITY;
	END


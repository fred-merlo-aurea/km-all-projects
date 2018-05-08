CREATE PROCEDURE [dbo].[e_CampaignItemTemplateOptoutGroup_Save]
	@CampaignItemTemplateOptOutGroupId int,
	@CampaignItemTemplateID int,
	@GroupID int,
	@IsDeleted bit,
	@UpdatedUserID int = null,
	@CreatedUserID int = null
AS
	if(@CampaignItemTemplateOptOutGroupId > 0)
	BEGIN
		UPDATE CampaignItemTemplateOptOutGroup
		SET IsDeleted = @IsDeleted, UpdatedDate = GETDATE(), UpdatedUserID = @UpdatedUserID
		WHERE CampaignItemTemplateOptOutGroupId = @CampaignItemTemplateOptOutGroupId
		SELECT @CampaignItemTemplateOptOutGroupId
	END
	ELSE
	BEGIN
		INSERT INTO CampaignItemTemplateOptOutGroup(CampaignItemTemplateID, GroupID, IsDeleted, CreatedDate, CreatedUserID)
		VALUES(@CampaignItemTemplateID, @GroupID, @IsDeleted, GETDATE(), @CreatedUserID)
		SELECT @@IDENTITY;
	END


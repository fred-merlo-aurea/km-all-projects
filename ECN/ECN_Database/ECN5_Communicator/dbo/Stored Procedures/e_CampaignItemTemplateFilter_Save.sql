CREATE PROCEDURE [dbo].[e_CampaignItemTemplateFilter_Save]
	@CampaignItemTemplateID int,
	@GroupID int,
	@FilterID int,
	@IsDeleted bit,
	@UpdatedUserID int = null,
	@CreatedUserID int = null
AS
	--IF(@CampaignItemTemplateGroupID > 0)
	--BEGIN
	--	UPDATE CampaignItemTemplateFilter
	--	SET IsDeleted = @IsDeleted, UpdatedDate = GETDATE(), UpdatedUserID = @UpdatedUserID
	--	WHERE CampaignItemTemplateID = @CampaignItemTemplateID AND CampaignItemTemplateGroupId = @CampaignItemTemplateGroupID
	--	SELECT @CampaignItemTemplateGroupID
	--END
	--ELSE
	BEGIN
		INSERT INTO CampaignItemTemplateFilter(CampaignItemTemplateID, GroupID, FilterID, IsDeleted, CreatedDate, CreatedUserID)
		VALUES(@CampaignItemTemplateID, @GroupID, @FilterID, @IsDeleted, GETDATE(), @CreatedUserID)
		SELECT @@IDENTITY;
	END

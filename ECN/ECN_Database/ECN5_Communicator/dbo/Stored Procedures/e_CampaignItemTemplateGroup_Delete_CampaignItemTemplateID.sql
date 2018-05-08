CREATE PROCEDURE [dbo].[e_CampaignItemTemplateGroup_Delete_CampaignItemTemplateID]
	@CampaignItemTemplateID int,
	@UpdatedUserID int
AS
	UPDATE CampaignItemTemplateGroup
	SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UpdatedUserID
	WHERE CampaignItemTemplateID = @CampaignItemTemplateID and IsDeleted = 0

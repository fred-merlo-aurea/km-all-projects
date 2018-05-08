CREATE PROCEDURE [dbo].[e_CampaignItemTemplateOptoutGroup_Delete_CampaignItemTemplateID]
	@CampaignItemTemplateID int,
	@UpdatedUserID int
AS
	UPDATE CampaignItemTemplateOptOutGroup
	SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UpdatedUserID
	WHERE CampaignItemTemplateID = @CampaignItemTemplateID and IsDeleted = 0

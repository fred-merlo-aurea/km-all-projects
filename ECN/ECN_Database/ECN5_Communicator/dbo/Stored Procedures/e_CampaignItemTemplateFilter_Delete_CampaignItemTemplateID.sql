CREATE PROCEDURE [dbo].[e_CampaignItemTemplateFilter_Delete_CampaignItemTemplateID]
	@CampaignItemTemplateID int,
	@UpdatedUserID int
AS
	UPDATE CampaignItemTemplateFilter
	SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UpdatedUserID
	WHERE CampaignItemTemplateID = @CampaignItemTemplateID and IsDeleted = 0

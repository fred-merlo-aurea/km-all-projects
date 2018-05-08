CREATE  PROC dbo.e_CampaignItemTemplateSuppressionGroup_Delete_CampaignItemTemplateID 
(
	@CampaignItemTemplateID int,
    @UserID int
)
AS 
BEGIN
	UPDATE CampaignItemTemplateSuppressionGroup SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID
	WHERE CampaignItemTemplateID = @CampaignItemTemplateID 
END
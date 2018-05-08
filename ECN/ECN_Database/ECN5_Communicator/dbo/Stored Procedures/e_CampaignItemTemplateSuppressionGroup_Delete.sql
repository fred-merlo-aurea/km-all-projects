CREATE  PROC [dbo].[e_CampaignItemTemplateSuppressionGroup_Delete] 
(
    @CampaignItemTemplateSuppressionGroupID int,
    @UserID int
)
AS 
BEGIN
	UPDATE CampaignItemTemplateSuppressionGroup SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID
	WHERE CampaignItemTemplateSuppressionGroupID = @CampaignItemTemplateSuppressionGroupID 
END
CREATE  PROC [dbo].[e_CampaignItemOptOutGroup_Delete_CampaignItemID] 
(
	@CampaignItemID int,
    @UserID int
)
AS 
BEGIN
	UPDATE CampaignItemOptOutGroup SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID
	WHERE CampaignItemID = @CampaignItemID 
END
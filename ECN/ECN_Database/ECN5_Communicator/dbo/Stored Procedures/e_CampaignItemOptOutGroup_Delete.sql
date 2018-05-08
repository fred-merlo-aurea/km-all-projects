CREATE  PROC [dbo].[e_CampaignItemOptOutGroup_Delete] 
(
    @CampaignitemOptOutID int,
    @UserID int
)
AS 
BEGIN
	UPDATE CampaignItemOptOutGroup SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID
	WHERE CampaignitemOptOutID = @CampaignitemOptOutID 
END
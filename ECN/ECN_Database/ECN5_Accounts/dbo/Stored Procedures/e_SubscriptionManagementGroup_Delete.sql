CREATE PROCEDURE [dbo].[e_SubscriptionManagementGroup_Delete_SMID]
	@SMID int,
	@UserID int
AS
	UPDATE SubscriptionManagementGroup
	SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID
	WHERE SubscriptionManagementPageID = @SMID and IsDeleted = 0

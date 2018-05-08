CREATE PROCEDURE [dbo].[e_SubscriptionManagementGroup_Delete_SMID_SMGID]
	@SMID int,
	@SMGID int,
	@UserID int
AS
	UPDATE SubscriptionManagementGroup
	SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID
	WHERE SubscriptionManagementPageID = @SMID and SubscriptionManagementGroupID = @SMGID and IsDeleted = 0

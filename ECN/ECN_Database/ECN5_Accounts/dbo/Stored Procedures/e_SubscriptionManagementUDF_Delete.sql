CREATE PROCEDURE [dbo].[e_SubscriptionManagementUDF_Delete]
	@SMGID int,
	@UserID int,
	@SMGUDFID int = null
AS
	if (@SMGUDFID is null)
	BEGIN
		UPDATE SubscriptionManagementUDF
		SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID
		WHERE SubscriptionManagementGroupID = @SMGID and IsDeleted = 0
	END
	ELSE
	BEGIN
		UPDATE SubscriptionManagementUDF
		SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID
		WHERE SubscriptionManagementUDFID = @SMGUDFID and SubscriptionManagementGroupID = @SMGID and IsDeleted = 0
	END

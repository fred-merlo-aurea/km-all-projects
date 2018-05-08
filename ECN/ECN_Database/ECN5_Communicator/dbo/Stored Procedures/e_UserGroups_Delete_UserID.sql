CREATE PROCEDURE [dbo].[e_UserGroups_Delete_UserID]  	
	@UserID int,
	@LoggingUserID int
AS     
BEGIN     
	Update UserGroups SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @LoggingUserID WHERE UserID = @UserID
END
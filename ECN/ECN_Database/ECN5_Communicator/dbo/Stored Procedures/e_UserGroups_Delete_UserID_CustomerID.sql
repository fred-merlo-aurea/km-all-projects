CREATE PROCEDURE [dbo].[e_UserGroups_Delete_UserID_CustomerID]
	@UserID int,
	@CustomerID int,
	@LoggingUserID int
AS
	Update ug
	SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @LoggingUserID 
	FROM UserGroups ug
	JOIN Groups g with(nolock) on ug.GroupID = g.GroupID
	WHERE UserID = @UserID and g.CustomerID = @CustomerID and ISNULL(ug.IsDeleted,0) = 0
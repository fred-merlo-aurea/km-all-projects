CREATE  PROC [dbo].[e_User_Delete] 
(
	@UserID int = NULL,
    @LoggingUserID int = NULL,
    @CustomerID int = NULL
)
AS 
BEGIN
	Update Users SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @LoggingUserID WHERE UserID = @UserID AND CustomerID = @CustomerID
END
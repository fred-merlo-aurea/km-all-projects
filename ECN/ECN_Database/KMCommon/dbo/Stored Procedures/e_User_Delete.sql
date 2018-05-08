CREATE PROCEDURE [dbo].[e_User_Delete]
@UserID int = NULL
AS
BEGIN
	DELETE [User] WHERE UserID = @UserID
END

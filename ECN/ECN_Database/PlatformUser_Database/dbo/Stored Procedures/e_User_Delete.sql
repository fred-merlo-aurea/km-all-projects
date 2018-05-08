CREATE PROCEDURE [dbo].[e_User_Delete]
	@UserID int
AS
	Delete FROM [User]
	where UserID = @UserID
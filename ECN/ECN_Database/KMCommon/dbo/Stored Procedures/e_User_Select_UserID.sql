CREATE PROCEDURE [dbo].[e_User_Select_UserID]
@UserID int = NULL
AS
BEGIN
	SELECT * FROM [User] WHERE UserID = @UserID
END

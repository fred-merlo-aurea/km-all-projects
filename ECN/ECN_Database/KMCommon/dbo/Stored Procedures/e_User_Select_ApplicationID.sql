CREATE PROCEDURE [dbo].[e_User_Select_ApplicationID]
@ApplicationID int = NULL
AS
BEGIN
	SELECT u.* FROM [User] u JOIN ApplicationUser au ON u.UserID = au.UserID WHERE au.ApplicationID = @ApplicationID
END

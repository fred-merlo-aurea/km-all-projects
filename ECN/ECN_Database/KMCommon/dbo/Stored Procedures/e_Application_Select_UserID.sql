CREATE PROCEDURE [dbo].[e_Application_Select_UserID]
@UserID int = NULL
AS
BEGIN
	SELECT a.* FROM [Application] a JOIN ApplicationUser au ON a.ApplicationID = au.ApplicationID WHERE au.UserID = @UserID
END

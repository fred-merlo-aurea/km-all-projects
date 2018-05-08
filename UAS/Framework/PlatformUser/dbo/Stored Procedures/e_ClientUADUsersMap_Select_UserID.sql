CREATE PROCEDURE [dbo].[e_ClientUADUsersMap_Select_UserID]
@UserID int
AS
	SELECT *
	FROM ClientUADUsersMap With(NoLock)
	WHERE UserID = @UserID

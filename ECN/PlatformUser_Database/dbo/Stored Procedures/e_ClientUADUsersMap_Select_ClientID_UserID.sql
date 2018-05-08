CREATE PROCEDURE [dbo].[e_ClientUADUsersMap_Select_ClientID_UserID]
@ClientID int,
@UserID int
AS
	SELECT *
	FROM ClientUADUsersMap With(NoLock)
	WHERE ClientID = @ClientID AND UserID = @UserID
GO

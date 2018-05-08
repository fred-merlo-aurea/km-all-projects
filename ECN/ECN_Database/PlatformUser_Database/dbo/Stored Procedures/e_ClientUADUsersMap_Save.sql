CREATE PROCEDURE [dbo].[e_ClientUADUsersMap_Save]
@ClientID int,
@UserID int
AS
	IF NOT EXISTS(Select UserID From ClientUADUsersMap With(NoLock) Where ClientID = @ClientID AND UserID = @UserID)
		BEGIN
			INSERT INTO ClientUADUsersMap (ClientID,UserID)
			VALUES(@ClientID,@UserID)
		END
GO

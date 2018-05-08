CREATE PROCEDURE [dbo].[e_ClientUADUsersMap_Select_ClientID]
@ClientID int
AS
	SELECT *
	FROM ClientUADUsersMap With(NoLock)
	WHERE ClientID = @ClientID
GO

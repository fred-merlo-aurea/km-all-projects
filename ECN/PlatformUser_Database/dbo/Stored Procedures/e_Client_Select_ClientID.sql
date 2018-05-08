CREATE PROCEDURE [dbo].[e_Client_Select_ClientID]
@ClientID int
AS
	SELECT *
	FROM Client With(NoLock)
	WHERE ClientID = @ClientID
GO

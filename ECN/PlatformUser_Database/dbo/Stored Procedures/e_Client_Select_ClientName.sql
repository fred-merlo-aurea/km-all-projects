CREATE PROCEDURE [dbo].[e_Client_Select_ClientName] 
@ClientName varchar(100)
AS
	SELECT * 
	FROM Client With(NoLock)
	WHERE ClientName = @ClientName


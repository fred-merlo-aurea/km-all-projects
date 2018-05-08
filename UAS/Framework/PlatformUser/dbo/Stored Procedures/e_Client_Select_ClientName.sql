CREATE PROCEDURE [dbo].[e_Client_Select_ClientName] 
@ClientName varchar(50)
AS
	SELECT * 
	FROM UAS..Client With(NoLock)
	WHERE ClientName = @ClientName

CREATE PROCEDURE [dbo].[e_Client_Exists_ClientName]
	@ClientName varchar(100)
AS
	IF exists (Select top 1 * from Client c with(nolock) where c.ClientName = @ClientName)
	BEGIN
		SELECT 1
	END
	ELSE
	BEGIN
		SELECT 0
	END


CREATE PROCEDURE [dbo].[e_Client_Delete_ClientID]
	@ClientID int
AS
	if exists (Select top 1 * from Client c with(nolock) where c.ClientID = @ClientID)
	BEGIN
		DELETE FROM Client
		where ClientID = @ClientID
	END
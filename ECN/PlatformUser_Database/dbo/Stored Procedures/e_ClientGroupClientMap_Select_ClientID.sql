CREATE PROCEDURE [dbo].[e_ClientGroupClientMap_Select_ClientID]
@ClientID int 
AS
	select *
	from ClientGroupClientMap with(nolock)
	where ClientID = @ClientID
GO

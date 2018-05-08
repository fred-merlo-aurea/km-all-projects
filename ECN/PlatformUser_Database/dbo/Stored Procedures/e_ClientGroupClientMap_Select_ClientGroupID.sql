CREATE PROCEDURE [dbo].[e_ClientGroupClientMap_Select_ClientGroupID]
@ClientGroupID int 
AS
	select *
	from ClientGroupClientMap with(nolock)
	where ClientGroupID = @ClientGroupID
GO

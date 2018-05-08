CREATE PROCEDURE [dbo].[e_ClientConfigurationMap_Select_ClientID]
@ClientID int
as
	select *
	from ClientConfigurationMap with(nolock)
	where ClientID = @ClientID
	order by CodeTypeId,CodeId
go

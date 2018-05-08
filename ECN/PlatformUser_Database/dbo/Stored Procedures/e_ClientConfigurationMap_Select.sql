CREATE PROCEDURE [dbo].[e_ClientConfigurationMap_Select]
as
	select *
	from ClientConfigurationMap with(nolock)
	order by ClientID, CodeTypeId,CodeId
go

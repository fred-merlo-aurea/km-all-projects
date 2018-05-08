CREATE PROCEDURE [dbo].[e_ApplicationServiceMap_Select]
AS
	select *
	from ApplicationServiceMap with(nolock)
	order by ApplicationID
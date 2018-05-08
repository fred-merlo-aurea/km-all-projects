CREATE PROCEDURE [dbo].[e_ClientGroupClientMap_Select]
AS
	select *
	from ClientGroupClientMap with(nolock)
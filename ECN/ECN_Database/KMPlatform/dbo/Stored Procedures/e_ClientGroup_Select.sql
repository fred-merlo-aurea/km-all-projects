CREATE PROCEDURE [dbo].[e_ClientGroup_Select]
AS
	select *
	from ClientGroup with(nolock)
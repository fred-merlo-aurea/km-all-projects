/*
**	Drop an object from the dbo.dtproperties table
*/
create procedure dbo.dt_dropuserobjectbyid
	@id int
as
	set nocount on
	delete from dbo.dtproperties where objectid=@id

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dt_dropuserobjectbyid] TO PUBLIC
    AS [dbo];


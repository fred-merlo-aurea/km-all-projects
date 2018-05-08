/*
**	This procedure returns the version number of the stored
**    procedures used by legacy versions of the Microsoft
**	Visual Database Tools.  Version is 7.0.00.
*/
create procedure dbo.dt_verstamp006
as
	select 7000

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dt_verstamp006] TO PUBLIC
    AS [dbo];


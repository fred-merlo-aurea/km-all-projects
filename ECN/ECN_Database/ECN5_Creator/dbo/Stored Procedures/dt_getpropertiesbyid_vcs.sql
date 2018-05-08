create procedure dbo.dt_getpropertiesbyid_vcs
    @id       int,
    @property varchar(64),
    @value    varchar(255) = NULL OUT
as
    set nocount on
    select @value = (
        select value
                from dbo.dtproperties
                where @id=objectid and @property=property
                )

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dt_getpropertiesbyid_vcs] TO PUBLIC
    AS [dbo];


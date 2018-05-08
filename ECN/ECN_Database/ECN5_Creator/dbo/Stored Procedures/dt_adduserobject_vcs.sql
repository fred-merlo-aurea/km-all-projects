create procedure dbo.dt_adduserobject_vcs
    @vchProperty varchar(64)
as
set nocount on
declare @iReturn int
    /*
    ** Create the user object if it does not exist already
    */
    begin transaction
        select @iReturn = objectid from dbo.dtproperties where property = @vchProperty
        if @iReturn IS NULL
        begin
            insert dbo.dtproperties (property) VALUES (@vchProperty)
            update dbo.dtproperties set objectid=@@identity
                    where id=@@identity and property=@vchProperty
            select @iReturn = @@identity
        end
    commit
    return @iReturn

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dt_adduserobject_vcs] TO PUBLIC
    AS [dbo];


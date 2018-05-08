create proc dbo.dt_vcsenabled
as
set nocount on
declare @iObjectId int
select @iObjectId = 0
declare @VSSGUID varchar(100)
select @VSSGUID = 'SQLVersionControl.VCS_SQL'
    declare @iReturn int
    exec @iReturn = master.dbo.sp_OACreate @VSSGUID, @iObjectId OUT
    if @iReturn <> 0 raiserror('', 16, -1) /* Can't Load Helper DLLC */

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dt_vcsenabled] TO PUBLIC
    AS [dbo];


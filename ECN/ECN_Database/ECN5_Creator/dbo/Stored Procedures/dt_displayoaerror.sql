CREATE PROCEDURE dbo.dt_displayoaerror
    @iObject int,
    @iresult int
as
set nocount on
declare @vchOutput      varchar(255)
declare @hr             int
declare @vchSource      varchar(255)
declare @vchDescription varchar(255)
    exec @hr = master.dbo.sp_OAGetErrorInfo @iObject, @vchSource OUT, @vchDescription OUT
    select @vchOutput = @vchSource + ': ' + @vchDescription
    raiserror (@vchOutput,16,-1)
    return

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dt_displayoaerror] TO PUBLIC
    AS [dbo];


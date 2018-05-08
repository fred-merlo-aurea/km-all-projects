create procedure e_DQMQue_UpdateComplete
@ProcessCode varchar(50)
as
BEGIN

	set nocount on

	update DQMQue
	set IsCompleted=1,DateCompleted=GetDate()
	where ProcessCode = @ProcessCode

END
go

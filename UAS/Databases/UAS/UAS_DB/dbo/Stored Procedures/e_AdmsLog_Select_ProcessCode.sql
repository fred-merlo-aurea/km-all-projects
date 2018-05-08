create procedure e_AdmsLog_Select_ProcessCode
@ProcessCode varchar(50)
as
	begin
		set nocount on
		select * 
		from AdmsLog with(nolock)
		where ProcessCode = @ProcessCode
	end
go

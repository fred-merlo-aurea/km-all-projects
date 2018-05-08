create procedure e_EngineLog_Select
as
	begin
		set nocount on
		select * from EngineLog with(nolock)
	end
go

create procedure e_EngineLog_UpdateRefresh
@ClientId int,
@Engine varchar(50),
@CurrentStatus varchar(max)
as
	begin
		set nocount on
		update EngineLog
		set CurrentStatus = @CurrentStatus,
			LastRefreshDate = getdate(),
			LastRefreshTime = getdate(),
			DateUpdated = getdate()
		where ClientId = @ClientId and Engine = @Engine
	end
go

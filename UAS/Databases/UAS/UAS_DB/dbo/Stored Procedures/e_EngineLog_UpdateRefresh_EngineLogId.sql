create procedure e_EngineLog_UpdateRefresh_EngineLogId
@EngineLogId int,
@CurrentStatus varchar(max)
as
	begin
		set nocount on
		update EngineLog
		set CurrentStatus = @CurrentStatus,
			LastRefreshDate = getdate(),
			LastRefreshTime = getdate(),
			DateUpdated = getdate()
		where EngineLogId = @EngineLogId
	end
go


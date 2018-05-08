create procedure e_EngineLog_UpdateIsRunning_EngineLogId
@EngineLogId int,
@IsRunning bit,
@CurrentStatus varchar(max)
as
	begin
		set nocount on
		update EngineLog
		set CurrentStatus = @CurrentStatus,
			IsRunning = @IsRunning,
			LastRunningCheckDate = getdate(),
			LastRunningCheckTime = getdate(),
			DateUpdated = getdate()
		where EngineLogId = @EngineLogId
	end
go

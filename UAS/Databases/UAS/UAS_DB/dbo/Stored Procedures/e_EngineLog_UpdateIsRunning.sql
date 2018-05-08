create procedure e_EngineLog_UpdateIsRunning
@ClientId int,
@Engine varchar(50),
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
		where ClientId = @ClientId and Engine = @Engine
	end
go


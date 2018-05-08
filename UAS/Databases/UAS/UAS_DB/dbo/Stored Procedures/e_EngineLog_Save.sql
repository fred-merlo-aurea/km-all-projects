create procedure e_EngineLog_Save
@EngineLogId int,  
@ClientId int, 
@Engine varchar(50), 
@CurrentStatus varchar(max) = '',
@LastRefreshDate date,  
@LastRefreshTime time(7),  
@IsRunning bit,  
@LastRunningCheckDate date, 
@LastRunningCheckTime time(7), 
@DateUpdated datetime  
as
begin
		set nocount on
		
		if @EngineLogId > 0
		begin
			if @DateUpdated IS NULL
				begin
					set @DateUpdated = getdate();
				end
			
			update EngineLog
			set 
				CurrentStatus = @CurrentStatus,
				LastRefreshDate = @LastRefreshDate,
				LastRefreshTime = @LastRefreshTime,
				IsRunning = @IsRunning,
				LastRunningCheckDate = @LastRunningCheckDate,
				LastRunningCheckTime = @LastRunningCheckTime,
				DateUpdated = @DateUpdated
			where EngineLogId = @EngineLogId;
		
			select @EngineLogId;
		end
	else
		begin
			insert into EngineLog (ClientId,Engine,CurrentStatus,LastRefreshDate,LastRefreshTime,IsRunning,LastRunningCheckDate,LastRunningCheckTime,DateUpdated)
			values(@ClientId,@Engine,@CurrentStatus,@LastRefreshDate,@LastRefreshTime,@IsRunning,@LastRunningCheckDate,@LastRunningCheckTime,@DateUpdated);select @@IDENTITY;
		end
	end
go

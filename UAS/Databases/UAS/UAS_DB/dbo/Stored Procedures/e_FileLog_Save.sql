CREATE procedure e_FileLog_Save
@SourceFileID int,
@FileStatusTypeID int,
@Message varchar(max),
@LogDate date,
@LogTime time,
@ProcessCode varchar(50)
as
	begin
		set nocount on
		if @FileStatusTypeID > 0
			begin
				insert into FileLog (SourceFileID, FileStatusTypeID, Message, LogDate, LogTime, ProcessCode)
				values(@SourceFileID,@FileStatusTypeID,@Message,@LogDate,@LogTime,@ProcessCode)
			end
		else
			begin
				insert into FileLog (SourceFileID, FileStatusTypeID, Message, LogDate, LogTime, ProcessCode)
				values(@SourceFileID,@FileStatusTypeID,@Message,@LogDate,@LogTime,@ProcessCode)
				select @SourceFileID,isnull(FileStatusId,0),@Message,@LogDate,@LogTime,@ProcessCode
				from AdmsLog with(nolock)
				where ProcessCode = @ProcessCode
			end
	end
go
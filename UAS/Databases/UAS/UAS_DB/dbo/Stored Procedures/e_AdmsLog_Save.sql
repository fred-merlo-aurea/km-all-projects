create procedure e_AdmsLog_Save
@AdmsLogId int,
@ClientId int,
@SourceFileId int,
@FileNameExact varchar(260),
@FileStart datetime,
@FileEnd datetime = null,
@FileStatusId int,
@StatusMessage varchar(max),
@AdmsStepId int,
@ProcessingStatusId int,
@ExecutionPointId int,
@RecordSource varchar(50),
@ProcessCode varchar(50),
@OriginalRecordCount int,
@OriginalProfileCount int,
@OriginalDemoCount int,
@TransformedRecordCount int,
@TransformedProfileCount int,
@TransformedDemoCount int,
@DuplicateRecordCount int,
@DuplicateProfileCount int,
@DuplicateDemoCount int,
@FailedRecordCount int,
@FailedProfileCount int,
@FailedDemoCount int,
@FinalRecordCount int,
@FinalProfileCount int,
@FinalDemoCount int,
@DateCreated datetime,
@DateUpdated datetime = null,
@CreatedByUserID int,
@UpdatedByUserID int,
@MatchedRecordCount int = 0,
@UadConsensusCount int = 0
as
	begin
		set nocount on
		
		if(@RecordSource = '' or @RecordSource = 'ClientFtp')
			begin
				set @RecordSource = (select c.CodeName
									 from UAD_LOOKUP..Code c with(nolock)
									 join SourceFile s with(nolock) on c.CodeId = s.DatabaseFileTypeId
									 where s.SourceFileId = @SourceFileId)
			end

		if @AdmsLogId > 0
		begin
			if @DateUpdated IS NULL
				begin
					set @DateUpdated = getdate();
				end
			if @FileEnd is not null
				begin
					update AdmsLog
					set FileEnd = @FileEnd
					where AdmsLogId = @AdmsLogId;
				end
			if @ClientId > 0
				begin
					update AdmsLog
					set ClientId = @ClientId
					where AdmsLogId = @AdmsLogId;
				end
			if @SourceFileId > 0
				begin
					update AdmsLog
					set SourceFileId = @SourceFileId
					where AdmsLogId = @AdmsLogId;
				end
			if len(@FileNameExact) > 0
				begin
					update AdmsLog
					set FileNameExact = @FileNameExact
					where AdmsLogId = @AdmsLogId;
				end
			if len(@FileStart) > 0
				begin
					update AdmsLog
					set FileStart = @FileStart
					where AdmsLogId = @AdmsLogId;
				end
			if @SourceFileId > 0
				begin
					update AdmsLog
					set SourceFileId = @SourceFileId
					where AdmsLogId = @AdmsLogId;
				end
			if @FileStatusId > 0
				begin
					update AdmsLog
					set FileStatusId = @FileStatusId
					where AdmsLogId = @AdmsLogId;
				end
			if len(@StatusMessage) > 0
				begin
					update AdmsLog
					set StatusMessage = @StatusMessage
					where AdmsLogId = @AdmsLogId;
				end
			if @AdmsStepId > 0
				begin
					update AdmsLog
					set AdmsStepId = @AdmsStepId
					where AdmsLogId = @AdmsLogId;
				end
			if @AdmsStepId > 0
				begin
					update AdmsLog
					set AdmsStepId = @AdmsStepId
					where AdmsLogId = @AdmsLogId;
				end
			if @ProcessingStatusId > 0
				begin
					update AdmsLog
					set ProcessingStatusId = @ProcessingStatusId
					where AdmsLogId = @AdmsLogId;
				end
			if @OriginalRecordCount > 0
				begin
					update AdmsLog
					set OriginalRecordCount = @OriginalRecordCount
					where AdmsLogId = @AdmsLogId;
				end
			if @ExecutionPointId > 0
				begin
					update AdmsLog
					set ExecutionPointId = @ExecutionPointId
					where AdmsLogId = @AdmsLogId;
				end
			if len(@RecordSource) > 0
				begin
					update AdmsLog
					set RecordSource = @RecordSource
					where AdmsLogId = @AdmsLogId;
				end
			if @OriginalRecordCount > 0
				begin
					update AdmsLog
					set OriginalRecordCount = @OriginalRecordCount
					where AdmsLogId = @AdmsLogId;
				end

			if @OriginalProfileCount > 0
				begin
					update AdmsLog
					set OriginalProfileCount = @OriginalProfileCount
					where AdmsLogId = @AdmsLogId;
				end
			if @OriginalDemoCount > 0
				begin
					update AdmsLog
					set OriginalDemoCount = @OriginalDemoCount
					where AdmsLogId = @AdmsLogId;
				end
			if @TransformedRecordCount > 0
				begin
					update AdmsLog
					set TransformedRecordCount = @TransformedRecordCount
					where AdmsLogId = @AdmsLogId;
				end
			if @TransformedProfileCount > 0
				begin
					update AdmsLog
					set TransformedProfileCount = @TransformedProfileCount
					where AdmsLogId = @AdmsLogId;
				end
			if @TransformedDemoCount > 0
				begin
					update AdmsLog
					set TransformedDemoCount = @TransformedDemoCount
					where AdmsLogId = @AdmsLogId;
				end
			if @DuplicateRecordCount > 0
				begin
					update AdmsLog
					set DuplicateRecordCount = @DuplicateRecordCount
					where AdmsLogId = @AdmsLogId;
				end
			if @DuplicateProfileCount > 0
				begin
					update AdmsLog
					set DuplicateProfileCount = @DuplicateProfileCount
					where AdmsLogId = @AdmsLogId;
				end
			if @DuplicateDemoCount > 0
				begin
					update AdmsLog
					set DuplicateDemoCount = @DuplicateDemoCount
					where AdmsLogId = @AdmsLogId;
				end
			if @FailedRecordCount > 0
				begin
					update AdmsLog
					set FailedRecordCount = @FailedRecordCount
					where AdmsLogId = @AdmsLogId;
				end
			if @FailedProfileCount > 0
				begin
					update AdmsLog
					set FailedProfileCount = @FailedProfileCount
					where AdmsLogId = @AdmsLogId;
				end
			if @FailedDemoCount > 0
				begin
					update AdmsLog
					set FailedDemoCount = @FailedDemoCount
					where AdmsLogId = @AdmsLogId;
				end
			if @FinalRecordCount > 0
				begin
					update AdmsLog
					set FinalRecordCount = @FinalRecordCount
					where AdmsLogId = @AdmsLogId;
				end
			if @FinalProfileCount > 0
				begin
					update AdmsLog
					set FinalProfileCount = @FinalProfileCount
					where AdmsLogId = @AdmsLogId;
				end
			if @FinalDemoCount > 0
				begin
					update AdmsLog
					set FinalDemoCount = @FinalDemoCount
					where AdmsLogId = @AdmsLogId;
				end
			if @MatchedRecordCount > 0
				begin
					update AdmsLog
					set MatchedRecordCount = @MatchedRecordCount
					where AdmsLogId = @AdmsLogId;
				end
			if @UadConsensusCount > 0
				begin
					update AdmsLog
					set UadConsensusCount = @UadConsensusCount
					where AdmsLogId = @AdmsLogId;
				end

			update AdmsLog
			set 
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			where AdmsLogId = @AdmsLogId;
		
			select @AdmsLogId;
		end
	else
		begin
			if @DateCreated IS NULL
				begin
					set @DateCreated = getdate();
				end

			declare @fsid int = (select codeId 
							 from UAD_Lookup..Code with(nolock) 
							 where codetypeid=(select CodeTypeId 
												from UAD_Lookup..CodeType with(nolock) 
												where codetypename='File Status') 
									and codename='Detected')
			if not exists(select AdmsLogId from AdmsLog with(nolock) where ProcessCode = @ProcessCode)
				begin
					insert into AdmsLog (ClientId,SourceFileId,FileNameExact,FileStart,FileEnd,FileStatusId,StatusMessage,AdmsStepId,ProcessingStatusId,ExecutionPointId,RecordSource,
										ProcessCode,OriginalRecordCount,OriginalProfileCount,OriginalDemoCount,TransformedRecordCount,TransformedProfileCount,
										TransformedDemoCount,DuplicateRecordCount,DuplicateProfileCount,DuplicateDemoCount,FailedRecordCount,FailedProfileCount,
										FailedDemoCount,FinalRecordCount,FinalProfileCount,FinalDemoCount,DateCreated,CreatedByUserID,MatchedRecordCount,UadConsensusCount)
					values(@ClientId,@SourceFileId,@FileNameExact,@FileStart,@FileEnd,@fsid,@StatusMessage,@AdmsStepId,@ProcessingStatusId,@ExecutionPointId,@RecordSource,
							@ProcessCode,@OriginalRecordCount,@OriginalProfileCount,@OriginalDemoCount,@TransformedRecordCount,@TransformedProfileCount,
							@TransformedDemoCount,@DuplicateRecordCount,@DuplicateProfileCount,@DuplicateDemoCount,@FailedRecordCount,@FailedProfileCount,
							@FailedDemoCount,@FinalRecordCount,@FinalProfileCount,@FinalDemoCount,@DateCreated,@CreatedByUserID,@MatchedRecordCount,@UadConsensusCount);select @@IDENTITY;
				end
			else
				begin
					if @DateUpdated IS NULL
				begin
					set @DateUpdated = getdate();
				end
					if @FileEnd is not null
						begin
							update AdmsLog
							set FileEnd = @FileEnd
							where ProcessCode = @ProcessCode;
						end
					if @ClientId > 0
						begin
							update AdmsLog
							set ClientId = @ClientId
							where ProcessCode = @ProcessCode;
						end
					if @SourceFileId > 0
						begin
							update AdmsLog
							set SourceFileId = @SourceFileId
							where ProcessCode = @ProcessCode;
						end
					if len(@FileNameExact) > 0
						begin
							update AdmsLog
							set FileNameExact = @FileNameExact
							where ProcessCode = @ProcessCode;
						end
					if len(@FileStart) > 0
						begin
							update AdmsLog
							set FileStart = @FileStart
							where ProcessCode = @ProcessCode;
						end
					if @SourceFileId > 0
						begin
							update AdmsLog
							set SourceFileId = @SourceFileId
							where ProcessCode = @ProcessCode;
						end
					if @FileStatusId > 0
						begin
							update AdmsLog
							set FileStatusId = @FileStatusId
							where ProcessCode = @ProcessCode;
						end
					if len(@StatusMessage) > 0
						begin
							update AdmsLog
							set StatusMessage = @StatusMessage
							where ProcessCode = @ProcessCode;
						end
					if @AdmsStepId > 0
						begin
							update AdmsLog
							set AdmsStepId = @AdmsStepId
							where ProcessCode = @ProcessCode;
						end
					if @AdmsStepId > 0
						begin
							update AdmsLog
							set AdmsStepId = @AdmsStepId
							where ProcessCode = @ProcessCode;
						end
					if @ProcessingStatusId > 0
						begin
							update AdmsLog
							set ProcessingStatusId = @ProcessingStatusId
							where ProcessCode = @ProcessCode;
						end
					if @OriginalRecordCount > 0
						begin
							update AdmsLog
							set OriginalRecordCount = @OriginalRecordCount
							where ProcessCode = @ProcessCode;
						end
					if @ExecutionPointId > 0
						begin
							update AdmsLog
							set ExecutionPointId = @ExecutionPointId
							where ProcessCode = @ProcessCode;
						end
					if len(@RecordSource) > 0
						begin
							update AdmsLog
							set RecordSource = @RecordSource
							where ProcessCode = @ProcessCode;
						end
					if @OriginalRecordCount > 0
						begin
							update AdmsLog
							set OriginalRecordCount = @OriginalRecordCount
							where ProcessCode = @ProcessCode;
						end

					if @OriginalProfileCount > 0
						begin
							update AdmsLog
							set OriginalProfileCount = @OriginalProfileCount
							where ProcessCode = @ProcessCode;
						end
					if @OriginalDemoCount > 0
						begin
							update AdmsLog
							set OriginalDemoCount = @OriginalDemoCount
							where ProcessCode = @ProcessCode;
						end
					if @TransformedRecordCount > 0
						begin
							update AdmsLog
							set TransformedRecordCount = @TransformedRecordCount
							where ProcessCode = @ProcessCode;
						end
					if @TransformedProfileCount > 0
						begin
							update AdmsLog
							set TransformedProfileCount = @TransformedProfileCount
							where ProcessCode = @ProcessCode;
						end
					if @TransformedDemoCount > 0
						begin
							update AdmsLog
							set TransformedDemoCount = @TransformedDemoCount
							where ProcessCode = @ProcessCode;
						end
					if @DuplicateRecordCount > 0
						begin
							update AdmsLog
							set DuplicateRecordCount = @DuplicateRecordCount
							where ProcessCode = @ProcessCode;
						end
					if @DuplicateProfileCount > 0
						begin
							update AdmsLog
							set DuplicateProfileCount = @DuplicateProfileCount
							where ProcessCode = @ProcessCode;
						end
					if @DuplicateDemoCount > 0
						begin
							update AdmsLog
							set DuplicateDemoCount = @DuplicateDemoCount
							where ProcessCode = @ProcessCode;
						end
					if @FailedRecordCount > 0
						begin
							update AdmsLog
							set FailedRecordCount = @FailedRecordCount
							where ProcessCode = @ProcessCode;
						end
					if @FailedProfileCount > 0
						begin
							update AdmsLog
							set FailedProfileCount = @FailedProfileCount
							where ProcessCode = @ProcessCode;
						end
					if @FailedDemoCount > 0
						begin
							update AdmsLog
							set FailedDemoCount = @FailedDemoCount
							where ProcessCode = @ProcessCode;
						end
					if @FinalRecordCount > 0
						begin
							update AdmsLog
							set FinalRecordCount = @FinalRecordCount
							where ProcessCode = @ProcessCode;
						end
					if @FinalProfileCount > 0
						begin
							update AdmsLog
							set FinalProfileCount = @FinalProfileCount
							where ProcessCode = @ProcessCode;
						end
					if @FinalDemoCount > 0
						begin
							update AdmsLog
							set FinalDemoCount = @FinalDemoCount
							where ProcessCode = @ProcessCode;
						end
					if @MatchedRecordCount > 0
						begin
							update AdmsLog
							set MatchedRecordCount = @MatchedRecordCount
							where ProcessCode = @ProcessCode;
						end
					if @UadConsensusCount > 0
						begin
							update AdmsLog
							set UadConsensusCount = @UadConsensusCount
							where ProcessCode = @ProcessCode;
						end

					update AdmsLog
					set 
						DateUpdated = @DateUpdated,
						UpdatedByUserID = @UpdatedByUserID
					where ProcessCode = @ProcessCode;
		
					select @AdmsLogId;
				end
		end
	end
go

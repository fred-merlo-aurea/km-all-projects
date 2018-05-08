CREATE PROCEDURE e_AdmsLog_UpdateProcessingStatus
@ProcessCode varchar(50),
@ProcessingStatus varchar(50),
@UserId int,
@StatusMessage varchar(max)
as
	begin
		set nocount on
		declare @csid int = (select codeId 
							 from UAD_Lookup..Code with(nolock) 
							 where codetypeid=(select CodeTypeId 
												from UAD_Lookup..CodeType with(nolock) 
												where codetypename='Processing Status') 
									and codename=@ProcessingStatus)
		update AdmsLog
		set StatusMessage = @StatusMessage,
			ProcessingStatusId = @csid,
			DateUpdated = getdate(),
			UpdatedByUserID = @UserId
		where ProcessCode = @ProcessCode

		select @csid;
	end
go

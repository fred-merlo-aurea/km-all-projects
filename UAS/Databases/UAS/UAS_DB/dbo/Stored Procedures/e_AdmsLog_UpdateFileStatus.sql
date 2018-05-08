CREATE PROCEDURE e_AdmsLog_UpdateFileStatus
@ProcessCode varchar(50),
@FileStatus varchar(50),
@UserId int,
@StatusMessage varchar(max)
as
	begin
		set nocount on
		declare @fsid int = (select codeId 
							 from UAD_Lookup..Code with(nolock) 
							 where codetypeid=(select CodeTypeId 
												from UAD_Lookup..CodeType with(nolock) 
												where codetypename='File Status') 
									and codename=@FileStatus)
		update AdmsLog
		set StatusMessage = @StatusMessage,
			FileStatusId = @fsid,
			DateUpdated = getdate(),
			UpdatedByUserID = @UserId
		where ProcessCode = @ProcessCode

		select @fsid;
	end
go

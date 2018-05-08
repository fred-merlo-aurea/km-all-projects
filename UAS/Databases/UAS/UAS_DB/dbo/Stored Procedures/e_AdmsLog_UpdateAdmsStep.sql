create procedure e_AdmsLog_UpdateAdmsStep
@ProcessCode varchar(50),
@AdmsStep varchar(50),
@UserId int,
@StatusMessage varchar(max)
as
	begin
		set nocount on
		declare @csid int = (select codeId 
							 from UAD_Lookup..Code with(nolock) 
							 where codetypeid=(select CodeTypeId 
												from UAD_Lookup..CodeType with(nolock) 
												where codetypename='ADMS Step') 
									and codename=@AdmsStep)
		update AdmsLog
		set StatusMessage = @StatusMessage,
			AdmsStepId = @csid,
			DateUpdated = getdate(),
			UpdatedByUserID = @UserId
		where ProcessCode = @ProcessCode

		select @csid;
	end
go


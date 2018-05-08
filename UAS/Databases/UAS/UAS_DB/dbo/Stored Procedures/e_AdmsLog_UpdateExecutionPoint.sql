create procedure e_AdmsLog_UpdateExecutionPoint
@ProcessCode varchar(50),
@ExecutionPoint varchar(50),
@UserId int
as
	begin
		set nocount on
		declare @epid int = (select codeId 
							 from UAD_Lookup..Code with(nolock) 
							 where codetypeid=(select CodeTypeId 
												from UAD_Lookup..CodeType with(nolock) 
												where codetypename='Execution Points') 
									and codename=@ExecutionPoint)
		if(@epid > 0)
			begin
			update AdmsLog
			set ExecutionPointId = @epid,
				DateUpdated = getdate(),
				UpdatedByUserID = @UserId
			where ProcessCode = @ProcessCode

			select @epid;
		end
		else
			select 0;
	end
go


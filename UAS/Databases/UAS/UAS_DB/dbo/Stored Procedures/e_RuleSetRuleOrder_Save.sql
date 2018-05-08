create procedure e_RuleSetRuleOrder_Save
@RuleSetId	int,
@RuleId	int,
@ExecutionOrder	int = 1,
@RuleScript	varchar(max) = '',
@DateCreated datetime,
@DateUpdated datetime = null,
@CreatedByUserId int = 1,
@UpdatedByUserId int = null
as
	begin
		set nocount on
		if exists(select RuleSetId from RuleSetRuleOrder with(nolock) where RuleSetId = @RuleSetId and RuleId = @RuleId)
			begin
				if(@DateUpdated = null)
					set @DateUpdated = getdate()
				update RuleSetRuleOrder
				set ExecutionOrder = @ExecutionOrder,
					RuleScript = @RuleScript,
					DateUpdated = @DateUpdated,
					UpdatedByUserId = @UpdatedByUserId
				where RuleSetId = @RuleSetId and RuleId = @RuleId
			end
		else
			begin
				if(@DateCreated = null)
					set @DateCreated = getdate()
				insert into RuleSetRuleOrder (RuleSetId,RuleId,ExecutionOrder,RuleScript,DateCreated,CreatedByUserId)
				values(@RuleSetId,@RuleId,@ExecutionOrder,@RuleScript,@DateCreated,@CreatedByUserId)
			end
	end
go

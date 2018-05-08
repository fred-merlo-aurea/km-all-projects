create procedure e_RuleCondition_Save
@RuleId int,
@Line int,
@IsGrouped bit = 'false',
@GroupNumber int = 0,
@ChainId int,
@CompareField varchar(50),
@CompareFieldPrefix varchar(5),
@IsClientField bit = 'false',
@OperatorId int,
@CompareValue varchar(250),
@IsActive bit = 'true',
@CreatedDate datetime,
@CreatedByUserId int,
@UpdatedByUserId int
as
	begin
		set nocount on
		if exists(select RuleId from RuleCondition with(nolock) where RuleId = @RuleId and Line = @Line)
			begin
				update RuleCondition
				set  IsGrouped = @IsGrouped,
					 GroupNumber = @GroupNumber,
					 ChainId = @ChainId,
					 CompareField = @CompareField,
					 CompareFieldPrefix = @CompareFieldPrefix,
					 IsClientField = @IsClientField,
					 OperatorId = @OperatorId,
					 CompareValue = @CompareValue,
					 IsActive = @IsActive,
					 UpdatedByUserId = @UpdatedByUserId
				where RuleId = @RuleId and Line = @Line
			end
		else
			begin
				if(@CreatedDate is null)
					set @CreatedDate = getdate();
				insert into RuleCondition (RuleId,Line,IsGrouped,GroupNumber,ChainId,CompareField,CompareFieldPrefix,IsClientField,OperatorId,CompareValue,IsActive,CreatedDate,CreatedByUserId)
				values(@RuleId,@Line,@IsGrouped,@GroupNumber,@ChainId,@CompareField,@CompareFieldPrefix,@IsClientField,@OperatorId,@CompareValue,@IsActive,@CreatedDate,@CreatedByUserId);
			end
	end
go


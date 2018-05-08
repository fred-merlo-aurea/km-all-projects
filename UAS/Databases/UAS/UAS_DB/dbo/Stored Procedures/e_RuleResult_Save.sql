create procedure e_RuleResult_Save
@RuleResultId int,
@RuleId int,
@RuleFieldId int,
@UpdateField varchar(50),
@UpdateFieldPrefix varchar(5),
@UpdateFieldValue varchar(250),
@IsClientField bit,
@IsActive bit,
@CreatedDate datetime,
@CreatedByUserId int,
@UpdatedByUserId int
as
	begin
		set nocount on
		if(@RuleResultId > 0)
			begin
				update RuleResult
				set RuleId = @RuleId,
					RuleFieldId = @RuleFieldId,
					UpdateField = @UpdateField,
					UpdateFieldPrefix = @UpdateFieldPrefix,
					UpdateFieldValue = @UpdateFieldValue,
					IsClientField = @IsClientField,
					IsActive = @IsActive,
					UpdatedByUserId = @UpdatedByUserId
				where RuleResultId = @RuleResultId;

				select @RuleResultId;
			end
		else
			begin
				if(@CreatedDate is null)
					set @CreatedDate = getdate();
				insert into RuleResult (RuleId,RuleFieldId,UpdateField,UpdateFieldPrefix,UpdateFieldValue,IsClientField,IsActive,CreatedDate,CreatedByUserId,UpdatedByUserId)
				values(@RuleId,@RuleFieldId,@UpdateField,@UpdateFieldPrefix,@UpdateFieldValue,@IsClientField,@IsActive,@CreatedDate,@CreatedByUserId,@UpdatedByUserId);select @@IDENTITY;
			end
	end
go

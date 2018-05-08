create procedure e_Rule_Save
@RuleId int,
@RuleName varchar(250),
@DisplayName varchar(250),
@RuleDescription varchar(1250),
@IsDeleteRule bit = 'false',
@IsSystem bit = 'false',
@IsGlobal bit = 'false',
@ClientId int,
@IsActive bit = 'true',
@DateCreated datetime,
@DateUpdated datetime = null,
@CreatedByUserId int,
@UpdatedByUserId int = null,
@CustomImportRuleId int,
@RuleActionId int
as
	begin
		set nocount on
		if(@RuleId > 0)
			begin
				if(@DateUpdated is null)
					set @DateUpdated = getdate();
				update [Rule]
				set RuleName = @RuleName,
					DisplayName = @DisplayName,
					RuleDescription = @RuleDescription,
					IsDeleteRule = @IsDeleteRule,
					IsSystem = @IsSystem,
					IsGlobal = @IsGlobal,
					ClientId = @ClientId,
					IsActive = @IsActive,
					DateUpdated = @DateUpdated,
					UpdatedByUserId = @UpdatedByUserId,
					CustomImportRuleId = @CustomImportRuleId,
					RuleActionId = @RuleActionId
				where RuleId = @RuleId;

				select @RuleId;
			end
		else
			begin
				if(@DateCreated is null)
					set @DateCreated = getdate();
				insert into [Rule] (RuleName,DisplayName,RuleDescription,IsDeleteRule,IsSystem,IsGlobal,ClientId,IsActive,DateCreated,CreatedByUserId,CustomImportRuleId,RuleActionId)
				values(@RuleName,@DisplayName,@RuleDescription,@IsDeleteRule,@IsSystem,@IsGlobal,@ClientId,@IsActive,@DateCreated,@CreatedByUserId,@CustomImportRuleId,@RuleActionId);select @@IDENTITY;
			end
	end
go

create procedure e_RuleSet_Update_Name_IsGlobal
@RuleSetId int,
@RuleSetName varchar(250),
@IsGlobalRuleSet bit = 'false',
@UpdatedByUserId int
as
	begin
		update RuleSet
		set RuleSetName = @RuleSetName, IsGlobal = @IsGlobalRuleSet, UpdatedByUserId = @UpdatedByUserId, DateUpdated = GetDate() 
		where RuleSetId = @RuleSetId
	end
go


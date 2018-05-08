create procedure e_RuleSet_Save
@RuleSetId int,
@RuleSetName varchar(250),
@DisplayName varchar(250),
@RuleSetDescription varchar(1250) = '',
@IsActive bit = 'true',
@IsSystem bit = 'false',
@IsGlobal bit = 'false',
@ClientId int,
@IsDateSpecific bit = 'false',
@StartMonth int = null,
@EndMonth int = null,
@StartDay int = null,
@EndDay int = null,
@StartYear int = null,
@EndYear int = null,
@DateCreated datetime,
@DateUpdated datetime = null,
@CreatedByUserId int,
@UpdatedByUserId int = null,
@CustomImportRuleId int = 0
as
	begin
		set nocount on
		if(@CustomImportRuleId = 0)
			begin
				declare @ctCIR int = (select codeTypeId from UAD_Lookup..CodeType where CodeTypeName = 'Custom Import Rule')
				set @CustomImportRuleId = (select CodeId from UAD_Lookup..Code where CodeTypeid=@ctCIR and CodeName = 'ADMS')
			end

		if(@RuleSetId>0)
			begin
				if @DateUpdated IS NULL
				begin
					set @DateUpdated = getdate();
				end

				update RuleSet
				set 
					RuleSetName = @RuleSetName,
					DisplayName = @DisplayName,
					RuleSetDescription = @RuleSetDescription,
					IsActive = @IsActive,
					IsSystem = @IsSystem,
					IsGlobal = @IsGlobal,
					ClientId = @ClientId,
					IsDateSpecific = @IsDateSpecific,
					StartMonth = @StartMonth,
					EndMonth = @EndMonth,
					StartDay = @StartDay,
					EndDay = @EndDay,
					StartYear = @StartYear,
					EndYear = @EndYear,
					DateUpdated = @DateUpdated,
					UpdatedByUserId = @UpdatedByUserId,
					CustomImportRuleId = @CustomImportRuleId
				where RuleSetId = @RuleSetId

				select @RuleSetId;
			end
		else
			begin
				if @DateCreated IS NULL
				begin
					set @DateCreated = getdate();
				end
				insert into RuleSet (RuleSetName,DisplayName,RuleSetDescription,IsActive,IsSystem,IsGlobal,ClientId,IsDateSpecific,StartMonth,EndMonth,StartDay,EndDay,StartYear,EndYear,DateCreated,CreatedByUserId,CustomImportRuleId)
				values(@RuleSetName,@DisplayName,@RuleSetDescription,@IsActive,@IsSystem,@IsGlobal,@ClientId,@IsDateSpecific,@StartMonth,@EndMonth,@StartDay,@EndDay,@StartYear,@EndYear,@DateCreated,@CreatedByUserId,@CustomImportRuleId);
				select @@IDENTITY;
			end
	end
go


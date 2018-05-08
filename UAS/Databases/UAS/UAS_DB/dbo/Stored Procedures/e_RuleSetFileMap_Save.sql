create procedure e_RuleSetFileMap_Save
@ruleSetId int,
@sourceFileId int,
@fileTypeId int,
@isSystem bit = 'false',
@isGlobal bit = 'false',
@isActive bit = 'true',
@executionPointId int,
@executionOrder int = 0,
@whereClause varchar(max) = '',
@dateCreated datetime = null,
@dateUpdated datetime = null,
@createdByUserId int,
@updatedByUserId int
as
	begin
		set nocount on
		if exists(select RuleSetId from RuleSet_File_Map where RuleSetId = @ruleSetId and SourceFileId = @sourceFileId and FileTypeId = @fileTypeId)
			begin
				if(@dateUpdated is null)
					set @dateUpdated = getdate()

				update RuleSet_File_Map
				set IsSystem = @isSystem,
					IsGlobal = @isGlobal,
					IsActive = @isActive,
					ExecutionPointId = @executionPointId,
					ExecutionOrder = @executionOrder,
					WhereClause = @whereClause,
					DateUpdated = @dateUpdated,
					UpdatedByUserId = @updatedByUserId
				where RuleSetId = @ruleSetId and SourceFileId = @sourceFileId and FileTypeId = @fileTypeId
			end
		else
			begin
				if(@dateCreated is null)
					set @dateCreated = getdate()
				insert into RuleSet_File_Map (RuleSetId,SourceFileId,FileTypeId,IsSystem,IsGlobal,IsActive,ExecutionPointId,ExecutionOrder,WhereClause,DateCreated,CreatedByUserId)
				values(@ruleSetId,@sourceFileId,@fileTypeId,@isSystem,@isGlobal,@isActive,@executionPointId,@executionOrder,@whereClause,@dateCreated,@createdByUserId)
			end
	end
go


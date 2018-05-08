create procedure e_RuleSetFileMap_SaveFromFileMapperWizard
@ruleSetId int,
@sourceFileId int,
@userId int
as
	begin
		set nocount on
		--FileTypeId = CodeTypeId 7 = Database File --> SourceFile.DatabaseFileTypeId
		--ExecutionPointId = save CodeName='Custom Import Rule' CodeTypeId=8 CodetypeName='Execution Points'
		declare @ctId int = (select codeTypeId from UAD_Lookup..CodeType where CodeTypeName = 'Execution Points')
		declare @epId int = (select codeId from UAD_Lookup..Code where CodeTypeId = @ctId and CodeName='Custom Import Rule')
		declare @ftId int = (select sf.DatabaseFileTypeId from SourceFile sf with(nolock) where sf.SourceFileId = @sourceFileId)

		insert into RuleSet_File_Map (RuleSetId,SourceFileId,FileTypeId,IsSystem,IsGlobal,IsActive,ExecutionPointId,ExecutionOrder,DateCreated,CreatedByUserId)
		select rs.RuleSetId,@sourceFileId,@ftId,rs.IsSystem,rs.IsGlobal,rs.IsActive,@epId,0,getdate(),@userId 
		from RuleSet rs with(nolock)
		left join RuleSet_File_Map m with(nolock) on rs.RuleSetId = m.RuleSetId and m.SourceFileId = @sourceFileId
		where rs.RuleSetId = @ruleSetId and m.RuleSetId is null
	end
go

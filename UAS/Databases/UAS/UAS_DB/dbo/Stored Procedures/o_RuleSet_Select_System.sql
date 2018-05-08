create procedure o_RuleSet_Select_System
@isActive bit = 'true'
as
	begin
		set nocount on
		
		select  rs.RuleSetId, rs.RuleSetName, rs.DisplayName, rs.RuleSetDescription, fm.IsActive, fm.IsSystem,fm.IsGlobal,rs.ClientId,rs.IsDateSpecific,
				rs.StartMonth,rs.EndMonth,rs.StartDay,rs.EndDay,rs.StartYear,rs.EndYear,fm.SourceFileId,fm.FileTypeId,fm.ExecutionPointId,fm.ExecutionOrder,fm.WhereClause,
				isnull(ftC.CodeName,'NotSet') as '_fileTypeEnum',
				isnull(epC.CodeName,'NotSet') as '_executionPoint'
		from RuleSet rs with(nolock)
		join RuleSet_File_Map fm with(nolock) on rs.RuleSetId = fm.RuleSetId
		left join UAD_LOOKUP..Code ftC with(nolock) on fm.FileTypeId = ftC.CodeId
		left join UAD_LOOKUP..Code epC with(nolock) on fm.ExecutionPointId = epC.CodeId
		where rs.IsSystem = 'true' and rs.ClientId = 0 and rs.IsActive = @isActive
		and   fm.IsSystem = 'true' and fm.SourceFileId = 0 and fm.IsActive = @isActive
		order by rs.RuleSetId,fm.ExecutionOrder,ftC.CodeName
	end
GO

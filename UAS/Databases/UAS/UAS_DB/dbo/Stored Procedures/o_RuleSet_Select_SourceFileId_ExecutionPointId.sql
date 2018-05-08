create procedure o_RuleSet_Select_SourceFileId_ExecutionPointId
@sourceFileId int,
@executionPointId int,
@isActive bit = 'true'
as
	begin
		set nocount on

		select  rs.RuleSetId, rs.RuleSetName, rs.DisplayName, rs.RuleSetDescription, fm.IsActive, fm.IsSystem,fm.IsGlobal,
		case when rs.ClientId = 0 then sf.ClientID else rs.ClientId end as 'ClientId',
		rs.IsDateSpecific,
				rs.StartMonth,rs.EndMonth,rs.StartDay,rs.EndDay,rs.StartYear,rs.EndYear,fm.SourceFileId,fm.FileTypeId,fm.ExecutionPointId,fm.ExecutionOrder,fm.WhereClause,
				isnull(ftC.CodeName,'NotSet') as '_fileTypeEnum',
				isnull(epC.CodeName,'NotSet') as '_executionPoint'
		from RuleSet rs with(nolock)
		join RuleSet_File_Map fm with(nolock) on rs.RuleSetId = fm.RuleSetId
		left join UAD_LOOKUP..Code ftC with(nolock) on fm.FileTypeId = ftC.CodeId
		left join UAD_LOOKUP..Code epC with(nolock) on fm.ExecutionPointId = epC.CodeId
		left join SourceFile sf with(nolock) on fm.SourceFileId = sf.SourceFileID
		where rs.IsActive = @isActive
		and   fm.IsSystem = 'false' and fm.SourceFileId = @sourceFileId and fm.IsActive = @isActive and fm.ExecutionPointId = @executionPointId
		order by rs.RuleSetId,fm.ExecutionOrder,ftC.CodeName
	end
GO

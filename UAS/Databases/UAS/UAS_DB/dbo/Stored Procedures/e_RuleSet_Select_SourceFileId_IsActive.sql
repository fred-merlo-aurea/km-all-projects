create procedure e_RuleSet_Select_SourceFileId_IsActive
@sourceFileId int,
@isActive bit = 'true'
as
	begin
		set nocount on

		select rs.*
		from RuleSet rs with(nolock)
		join RuleSet_File_Map m with(nolock) on rs.RuleSetId = m.RuleSetId
		where rs.IsActive = @isActive
		and m.SourceFileId = @sourceFileId and m.IsActive = @isActive
	end
go

CREATE procedure [dbo].[e_SourceFile_Select_SourceFileIds]
@sfIds varchar(500)
as
	begin
		set nocount on
		select s.*
		from SourceFile s with(nolock)
		join fn_Split(@sfIds,',') i on s.SourceFileID = i.items
	end
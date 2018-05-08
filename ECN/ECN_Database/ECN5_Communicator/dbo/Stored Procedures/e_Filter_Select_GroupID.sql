CREATE PROCEDURE [dbo].[e_Filter_Select_GroupID]   
@GroupID int,
@ValidWhereOnly bit,
@ArchiveFilter varchar(20) = 'all'
AS
	IF @ValidWhereOnly = 0
		SELECT * 
		FROM Filter WITH (NOLOCK) 
		WHERE GroupID = @GroupID and 
		IsDeleted = 0 and 
		ISNULL(Archived,0) = case when @ArchiveFilter = 'archived' then 1 when @ArchiveFilter = 'all' then ISNULL(Archived,0) when @ArchiveFilter = 'active' then 0 END
	Else
		SELECT * 
		FROM Filter WITH (NOLOCK) 
		WHERE (GroupID = @GroupID and IsDeleted = 0 and WhereClause is not null) 
		and (Len(rtrim(ltrim(CONVERT(varchar(200), WhereClause)))) > 0)
		and ISNULL(Archived,0) = case when @ArchiveFilter = 'archived' then 1 when @ArchiveFilter = 'all' then ISNULL(Archived,0) when @ArchiveFilter = 'active' then 0 END
CREATE PROCEDURE [dbo].[e_MasterGroup_Select_MasterGroupsBySearch]
@Name varchar(100) = NULL,
@SearchCriteria varchar (20) = NULL,
@CurrentPage int,
@PageSize int,
@SortDirection varchar(20),
@SortColumn varchar(50)
AS
BEGIN
	WITH Results
	AS (SELECT ROW_NUMBER() OVER (ORDER BY
		CASE WHEN (@SortColumn = 'DisplayName' AND @SortDirection='ASC') THEN mg.DisplayName END ASC,
		CASE WHEN (@SortColumn = 'DisplayName' AND @SortDirection='DESC') THEN mg.DisplayName END DESC,
		CASE WHEN (@SortColumn = 'Name' AND @SortDirection='ASC') THEN mg.Name END ASC,
		CASE WHEN (@SortColumn = 'Name' AND @SortDirection='DESC') THEN mg.Name END DESC,		
		CASE WHEN (@SortColumn = 'IsActive' AND @SortDirection='ASC') THEN mg.IsActive END ASC,
		CASE WHEN (@SortColumn = 'IsActive' AND @SortDirection='DESC') THEN mg.IsActive END DESC,
		CASE WHEN (@SortColumn = 'EnableSubReporting' AND @SortDirection='ASC') THEN mg.EnableSubReporting END ASC,
		CASE WHEN (@SortColumn = 'EnableSubReporting' AND @SortDirection='DESC') THEN mg.EnableSubReporting END DESC,	
		CASE WHEN (@SortColumn = 'EnableSearching' AND @SortDirection='ASC') THEN mg.EnableSearching END ASC,
		CASE WHEN (@SortColumn = 'EnableSearching' AND @SortDirection='DESC') THEN mg.EnableSearching END DESC,		
		CASE WHEN (@SortColumn = 'EnableAdhocSearch' AND @SortDirection='ASC') THEN mg.EnableAdhocSearch END ASC,
		CASE WHEN (@SortColumn = 'EnableAdhocSearch' AND @SortDirection='DESC') THEN mg.EnableAdhocSearch END DESC		
    ) AS ROWNUM,
        Count(mg.MasterGroupID) over () AS TotalRecordCounts, 
		mg.MasterGroupID, mg.DisplayName, mg.Name, mg.IsActive, mg.EnableSubReporting, mg.EnableSearching, mg.EnableAdhocSearch
		FROM MasterGroups mg (nolock)
		WHERE (UPPER(mg.Name) LIKE case when @SearchCriteria = 'equals' and @Name is not null then @Name
									    when @SearchCriteria = 'starts' and @Name is not null then @Name + '%'
										when @SearchCriteria = 'ends' and @Name is not null then '%' + @Name
										when @SearchCriteria = 'like' and @Name is not null then '%' + @Name + '%'
										when @Name is null then UPPER(mg.Name) END) or
			(UPPER(mg.DisplayName) LIKE case when @SearchCriteria = 'equals' and @Name is not null then @Name
									    when @SearchCriteria = 'starts' and @Name is not null then @Name + '%'
										when @SearchCriteria = 'ends' and @Name is not null then '%' + @Name
										when @SearchCriteria = 'like' and @Name is not null then '%' + @Name + '%'
										when @Name is null then UPPER(mg.DisplayName) END)							
			group by mg.MasterGroupID, mg.DisplayName, mg.Name, mg.IsActive, mg.EnableSubReporting, mg.EnableSearching, mg.EnableAdhocSearch
	)
	SELECT * FROM Results
	WHERE ROWNUM between ((@CurrentPage - 1) * @PageSize + 1) and (@CurrentPage * @PageSize)
END
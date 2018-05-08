CREATE PROCEDURE [dbo].[e_CodeSheet_Select_Search]
@ResponseGroupID int,
@Name varchar(100) = NULL,
@SearchCriteria varchar (20) = NULL,
@CurrentPage int,
@PageSize int,
@SortDirection varchar(20),
@SortColumn varchar(50)
AS
BEGIN
	WITH Results
		AS (
		SELECT ROW_NUMBER() OVER (ORDER BY
			CASE WHEN (@SortColumn = 'Responsevalue' AND @SortDirection='ASC') THEN cs.Responsevalue END ASC,
			CASE WHEN (@SortColumn = 'Responsevalue' AND @SortDirection='DESC') THEN cs.Responsevalue END DESC,
			CASE WHEN (@SortColumn = 'Responsedesc' AND @SortDirection='ASC') THEN cs.Responsedesc END ASC,
			CASE WHEN (@SortColumn = 'Responsedesc' AND @SortDirection='DESC') THEN cs.Responsedesc END DESC,
			CASE WHEN (@SortColumn = 'DisplayName' AND @SortDirection='ASC') THEN rg.DisplayName END ASC,
			CASE WHEN (@SortColumn = 'DisplayName' AND @SortDirection='DESC') THEN rg.DisplayName END DESC,
			CASE WHEN (@SortColumn = 'IsActive' AND @SortDirection='ASC') THEN cs.IsActive END ASC,
			CASE WHEN (@SortColumn = 'IsActive' AND @SortDirection='DESC') THEN cs.IsActive END DESC,		
			CASE WHEN (@SortColumn = 'IsOther' AND @SortDirection='ASC') THEN cs.IsOther END ASC,
			CASE WHEN (@SortColumn = 'IsOther' AND @SortDirection='DESC') THEN cs.IsOther END DESC
		) AS ROWNUM,
        Count(cs.CodeSheetID ) over () AS TotalRecordCounts, 
		cs.CodeSheetID, cs.Responsevalue, cs.Responsedesc,  rg.DisplayName, cs.IsActive, isnull(cs.IsOther, 0) as IsOther
		FROM 
			CodeSheet cs (nolock) 
			left outer join ReportGroups rg (nolock) on cs.ReportGroupID = rg.ReportGroupID
		WHERE 
			cs.ResponseGroupID = @ResponseGroupID	and  
			(
				(UPPER(cs.Responsevalue) LIKE case when @SearchCriteria = 'equals' and @Name is not null then @Name
									    when @SearchCriteria = 'starts' and @Name is not null then @Name + '%'
										when @SearchCriteria = 'ends' and @Name is not null then '%' + @Name
										when @SearchCriteria = 'like' and @Name is not null then '%' + @Name + '%'
										when @Name is null then UPPER(cs.Responsevalue) END) or
				(UPPER(cs.Responsedesc) LIKE case when @SearchCriteria = 'equals' and @Name is not null then @Name
									    when @SearchCriteria = 'starts' and @Name is not null then @Name + '%'
										when @SearchCriteria = 'ends' and @Name is not null then '%' + @Name
										when @SearchCriteria = 'like' and @Name is not null then '%' + @Name + '%'
										when @Name is null then UPPER(cs.Responsedesc) END)	
			)	 					
			group by cs.CodeSheetID, cs.Responsevalue, cs.Responsedesc,  rg.DisplayName, cs.IsActive, cs.IsOther
	)
	SELECT * FROM Results
	WHERE ROWNUM between ((@CurrentPage - 1) * @PageSize + 1) and (@CurrentPage * @PageSize)
END

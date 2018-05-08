CREATE PROCEDURE [dbo].[e_ResponseGroup_Select_ResponseGroupsBySearch]
@PubID int,
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
			CASE WHEN (@SortColumn = 'DisplayName' AND @SortDirection='ASC') THEN rg.DisplayName END ASC,
			CASE WHEN (@SortColumn = 'DisplayName' AND @SortDirection='DESC') THEN rg.DisplayName END DESC,
			CASE WHEN (@SortColumn = 'ResponseGroupName' AND @SortDirection='ASC') THEN rg.ResponseGroupName END ASC,
			CASE WHEN (@SortColumn = 'ResponseGroupName' AND @SortDirection='DESC') THEN rg.ResponseGroupName END DESC,		
			CASE WHEN (@SortColumn = 'IsActive' AND @SortDirection='ASC') THEN rg.IsActive END ASC,
			CASE WHEN (@SortColumn = 'IsActive' AND @SortDirection='DESC') THEN rg.IsActive END DESC,
			CASE WHEN (@SortColumn = 'IsMultipleValue' AND @SortDirection='ASC') THEN rg.IsMultipleValue END ASC,
			CASE WHEN (@SortColumn = 'IsMultipleValue' AND @SortDirection='DESC') THEN rg.IsMultipleValue END DESC,	
			CASE WHEN (@SortColumn = 'IsRequired' AND @SortDirection='ASC') THEN rg.IsRequired END ASC,
			CASE WHEN (@SortColumn = 'IsRequired' AND @SortDirection='DESC') THEN rg.IsRequired END DESC		
		) AS ROWNUM,
        Count(rg.ResponseGroupID) over () AS TotalRecordCounts, 
		rg.ResponseGroupID, rg.DisplayName, rg.ResponseGroupName, rg.IsActive, isnull(rg.IsMultipleValue, 0) as IsMultipleValue, isnull(rg.IsRequired,0) as IsRequired, rg.PubID
		FROM 
			ResponseGroups rg (nolock)
		WHERE 
			rg.PubID = @PubID	and  
			(
				(UPPER(rg.ResponseGroupName) LIKE case when @SearchCriteria = 'equals' and @Name is not null then @Name
									    when @SearchCriteria = 'starts' and @Name is not null then @Name + '%'
										when @SearchCriteria = 'ends' and @Name is not null then '%' + @Name
										when @SearchCriteria = 'like' and @Name is not null then '%' + @Name + '%'
										when @Name is null then UPPER(rg.ResponseGroupName) END
				) or
				(UPPER(rg.DisplayName) LIKE case when @SearchCriteria = 'equals' and @Name is not null then @Name
									    when @SearchCriteria = 'starts' and @Name is not null then @Name + '%'
										when @SearchCriteria = 'ends' and @Name is not null then '%' + @Name
										when @SearchCriteria = 'like' and @Name is not null then '%' + @Name + '%'
										when @Name is null then UPPER(rg.DisplayName) END
				)
			)	 					
			group by rg.ResponseGroupID, rg.DisplayName, rg.ResponseGroupName, rg.IsActive, rg.IsMultipleValue, rg.IsRequired, rg.PubID
	)
	SELECT * FROM Results
	WHERE ROWNUM between ((@CurrentPage - 1) * @PageSize + 1) and (@CurrentPage * @PageSize)
END


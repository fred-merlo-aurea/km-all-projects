CREATE PROCEDURE [dbo].[e_ProductSubscriptionsExtensionMapper_Select_Search]
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
			CASE WHEN (@SortColumn = 'CustomField' AND @SortDirection='ASC') THEN pem.CustomField END ASC,
			CASE WHEN (@SortColumn = 'CustomField' AND @SortDirection='DESC') THEN pem.CustomField END DESC,
			CASE WHEN (@SortColumn = 'CustomFieldDataType' AND @SortDirection='ASC') THEN pem.CustomFieldDataType END ASC,
			CASE WHEN (@SortColumn = 'CustomFieldDataType' AND @SortDirection='DESC') THEN pem.CustomFieldDataType END DESC,		
			CASE WHEN (@SortColumn = 'Active' AND @SortDirection='ASC') THEN pem.Active END ASC,
			CASE WHEN (@SortColumn = 'Active' AND @SortDirection='DESC') THEN pem.Active END DESC
		) AS ROWNUM,
        Count(pem.PubSubscriptionsExtensionMapperID ) over () AS TotalRecordCounts, 
		pem.PubSubscriptionsExtensionMapperID, pem.CustomField, pem.CustomFieldDataType, pem.Active, pem.PubID
		FROM 
			PubSubscriptionsExtensionMapper pem (nolock)
		WHERE 
			pem.PubID = @PubID	and  
			(
				UPPER(pem.CustomField) LIKE case when @SearchCriteria = 'equals' and @Name is not null then @Name
									    when @SearchCriteria = 'starts' and @Name is not null then @Name + '%'
										when @SearchCriteria = 'ends' and @Name is not null then '%' + @Name
										when @SearchCriteria = 'like' and @Name is not null then '%' + @Name + '%'
										when @Name is null then UPPER(pem.CustomField) END
			)	 					
			group by pem.PubSubscriptionsExtensionMapperID, pem.CustomField, pem.CustomFieldDataType, pem.Active, pem.PubID
	)
	SELECT * FROM Results
	WHERE ROWNUM between ((@CurrentPage - 1) * @PageSize + 1) and (@CurrentPage * @PageSize)
END

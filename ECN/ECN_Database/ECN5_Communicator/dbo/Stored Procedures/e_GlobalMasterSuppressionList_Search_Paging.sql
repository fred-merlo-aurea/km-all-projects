﻿CREATE PROCEDURE [dbo].[e_GlobalMasterSuppressionList_Search_Paging]
		@EmailSearchString varchar(500), 
	@PageIndex int,
	@PageSize int,
	@SortColumn varchar(50),
	@SortDirection varchar(10)
AS
	WITH Results
            AS (SELECT ROW_NUMBER() OVER (ORDER BY
                  
                  CASE WHEN (@SortColumn = 'EmailAddress' AND @SortDirection='ASC') THEN EmailAddress END ASC,
                  CASE WHEN (@SortColumn = 'EmailAddress' AND @SortDirection='DESC') THEN EmailAddress END DESC,
                  CASE WHEN (@SortColumn = 'UpdatedDate' AND @SortDirection='ASC') THEN c.UpdatedDate END ASC,
                  CASE WHEN (@SortColumn = 'UpdatedDate' AND @SortDirection='DESC') THEN c.UpdatedDate END DESC,
                  CASE WHEN (@SortColumn = 'CreatedDate' AND @SortDirection='ASC') THEN ISNULL(c.CreatedDate,c.UpdatedDate) END ASC,
                  CASE WHEN (@SortColumn = 'CreatedDate' AND @SortDirection='DESC') THEN ISNULL(c.CreatedDate,c.UpdatedDate) END DESC                      
            ) AS ROWNUM,
            Count(*) over () AS TotalCount,
                  c.GSID, c.CreatedDate, c.CreatedUserID, c.EmailAddress, c.IsDeleted, c.UpdatedDate, c.UpdatedUserID
                  FROM GlobalMasterSuppressionList c (nolock)
                  
                  WHERE ISNULL(c.IsDeleted,0) = 0 and c.EmailAddress like  case when @EmailSearchString <> '' then '%' + @EmailSearchString + '%' else c.EmailAddress end 
            )
            SELECT * 
            FROM Results             
            WHERE ROWNUM between ((@PageIndex - 1) * @PageSize + 1) and (@PageIndex * @PageSize)
CREATE PROCEDURE [dbo].[e_EmailGroup_PreviewFilteredEmails_Paging]
	@GroupID int,
	@CustomerID int,
	@Filter varchar(8000),
	@SortColumn varchar(255),
	@SortDirection varchar(10),
	@PageSize int,
	@PageNumber int
AS
	
	--Message threshold enabled                                 
    declare  @FilteredEmails table (EmailID int, EmailAddress varchar(255))

	insert into @FilteredEmails
    exec e_EmailGroup_PreviewFilteredEmails @GroupID,@CustomerID, @Filter

	;WITH Results
            AS (SELECT ROW_NUMBER() OVER (ORDER BY
                  
                  CASE WHEN (@SortColumn = 'EmailAddress' AND @SortDirection='ASC') THEN EmailAddress END ASC,
                  CASE WHEN (@SortColumn = 'EmailAddress' AND @SortDirection='DESC') THEN EmailAddress END DESC
             
            ) AS ROWNUM,
            Count(*) over () AS TotalCount,EmailID, EmailAddress
			from @FilteredEmails
			)
            SELECT * 
            FROM Results 
            
            WHERE ROWNUM between ((@PageNumber - 1) * @PageSize + 1) and (@PageNumber * @PageSize)
	
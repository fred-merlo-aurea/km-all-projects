CREATE PROCEDURE [dbo].[v_Content_Select_Title] 
(
@ContentTitle varchar(250) = NULL,
@FolderID int = NULL,
@CustomerID int = NULL,
@UserID int = NULL,
@BaseChannelID int = NULL,
@UpdatedDateFrom datetime = NULL,
@UpdatedDateTo datetime = NULL,
@CurrentPage int,
@PageSize int,
@SortDirection varchar(20),
@SortColumn varchar(50),
@ArchiveFilter varchar(20) = 'all',
@ValidatedOnly int =0
)
AS

BEGIN
	
	WITH Results
	AS (SELECT ROW_NUMBER() OVER (ORDER BY
		CASE WHEN (@SortColumn = 'FolderName' AND @SortDirection='ASC') THEN FolderName END ASC,
		CASE WHEN (@SortColumn = 'FolderName' AND @SortDirection='DESC') THEN FolderName END DESC,
		CASE WHEN (@SortColumn = 'ContentTitle' AND @SortDirection='ASC') THEN ContentTitle END ASC,
		CASE WHEN (@SortColumn = 'ContentTitle' AND @SortDirection='DESC') THEN ContentTitle END DESC,
		CASE WHEN (@SortColumn = 'CreatedDate' AND @SortDirection = 'ASC') then ISNULL(c.CreatedDate,c.UpdatedDate) END ASC,
		CASE WHEN (@SortColumn = 'CreatedDate' AND @SortDirection = 'DESC') then ISNULL(c.CreatedDate,c.UpdatedDate) END DESC,
		CASE WHEN (@SortColumn = 'UpdatedDate' AND @SortDirection='ASC') THEN c.UpdatedDate END ASC,
		CASE WHEN (@SortColumn = 'UpdatedDate' AND @SortDirection='DESC') THEN c.UpdatedDate END DESC		
    ) AS ROWNUM,
    Count(c.ContentID) over () AS TotalCount, 
		c.ContentID,c.CreatedUserID,c.FolderID,c.LockedFlag,c.ContentSource,c.ContentText,c.ContentTypeCode,c.ContentCode,c.ContentTitle,
		c.CustomerID,c.ContentURL,c.ContentFilePointer,c.UpdatedDate,c.Sharing,c.MasterContentID,c.ContentMobile,c.ContentSMS,
		ISNULL(c.CreatedDate,c.UpdatedDate) as 'CreatedDate',c.IsDeleted,c.UpdatedUserID,c.UseWYSIWYGeditor,ISNULL(f.FolderName,'') as FolderName,ISNULL(c.Archived,0) as Archived
		,ISNULL(c.IsValidated,0) as Validated
		FROM Content c (nolock)
		Left Join Folder f (nolock) on f.FolderID = c.FolderID
		WHERE c.CustomerID = @CustomerID 
			and c.FolderID = case when @FolderID is not null then @FolderID ELSE c.FolderID END
			and UPPER(c.ContentTitle) LIKE case when @ContentTitle is not null then '%' + @ContentTitle + '%' ELSE UPPER(c.ContentTitle) END
			and ISNULL(c.UpdatedUserID,0) = Case when @UserID is not null then @UserID ELSE ISNULL(c.UpdatedUserID,0) END			
			and c.UpdatedDate >= case when @UpdatedDateFrom is not null then @UpdatedDateFrom ELSE c.UpdatedDate END
			and c.UpdatedDate <= case when @UpdatedDateTo is not null then @UpdatedDateTo ELSE c.UpdatedDate END
			and c.IsDeleted = 0 
			and ISNULL(c.IsValidated,0) = case when @ValidatedOnly = 1 then 1 ELSE ISNULL(c.IsValidated,0)  END
			and ISNULL(c.Archived,0) = case when @ArchiveFilter = 'archived' then 1 when @ArchiveFilter = 'all' then ISNULL(c.Archived,0) when @ArchiveFilter = 'active' then 0 END
	)
	SELECT *, (convert(varchar(255),ContentID) + '&chID='  + convert(varchar,@BaseChannelID)  + '&cuID='  + convert(varchar,@CustomerID)) as ContentIDPlus
	FROM Results
	WHERE ROWNUM between ((@CurrentPage - 1) * @PageSize + 1) and (@CurrentPage * @PageSize)
	

END
CREATE PROCEDURE [dbo].[e_Content_Select_FolderID_CustomerID]   
@FolderID int,
@CustomerID int,
@ArchiveFilter varchar(20)
AS
	SELECT * 
	FROM Content WITH (NOLOCK) 
	WHERE FolderID = @FolderID and 
	IsDeleted = 0 and IsValidated = 1 and 
	CustomerID=@CustomerID and
	ISNULL(Archived,0) = CASE WHEN @ArchiveFilter = 'active' then 0 when @ArchiveFilter = 'all' then ISNULL(Archived, 0) when @ArchiveFilter = 'archived' then 1 end
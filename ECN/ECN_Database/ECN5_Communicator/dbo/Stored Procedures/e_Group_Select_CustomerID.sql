CREATE PROCEDURE [dbo].[e_Group_Select_CustomerID]   
@CustomerID int,
@ArchiveFilter varchar(20) 
AS
	SELECT * 
	FROM [Groups] WITH (NOLOCK) 
	WHERE CustomerID = @CustomerID and
	ISNULL(Archived, 0) = CASE WHEN @ArchiveFilter = 'active' then 0 when @ArchiveFilter = 'archived' then 1 when @ArchiveFilter = 'all' then ISNULL(Archived, 0) end

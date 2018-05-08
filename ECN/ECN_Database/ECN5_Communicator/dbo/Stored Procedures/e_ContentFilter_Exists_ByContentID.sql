CREATE  PROC [dbo].[e_ContentFilter_Exists_ByContentID] 
(
	@ContentID int,
	@CustomerID int
)
AS 
BEGIN
	IF EXISTS (
		SELECT TOP 1 cf.FilterID
		from 
			ContentFilter cf with (nolock)
			join [Groups] g with (nolock) on cf.GroupID = g.GroupID
		where 
			g.CustomerID = @CustomerID AND cf.ContentID = @ContentID AND cf.IsDeleted = 0 
	) SELECT 1 ELSE SELECT 0
END
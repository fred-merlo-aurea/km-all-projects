CREATE  PROC [dbo].[e_ContentFilterDetail_Exists_ByFDID] 
(
	@FDID int = NULL,
	@FilterID int = NULL,
	@CustomerID int = NULL
)
AS 
BEGIN
	IF EXISTS (
		SELECT TOP 1 cfd.FDID
		from 
			ContentFilterDetail cfd with (nolock) 
			join ContentFilter cf with (nolock) on cfd.FilterID = cf.FilterID
			join [Groups] g with (nolock) on cf.GroupID = g.GroupID
		where 
			g.CustomerID = @CustomerID AND cfd.FilterID = @FilterID AND cfd.FDID = @FDID AND cfd.IsDeleted = 0 and cf.IsDeleted = 0 
	) SELECT 1 ELSE SELECT 0
END

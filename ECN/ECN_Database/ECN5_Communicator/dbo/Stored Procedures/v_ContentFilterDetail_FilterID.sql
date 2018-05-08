CREATE  PROC [dbo].[v_ContentFilterDetail_FilterID] 
(
	@FilterID int = NULL,
	@CustomerID int = NULL
)
AS 
BEGIN
	select cfd.*, g.CustomerID, cf.FilterName, c.ContentTitle
	from 
		ContentFilterDetail cfd with (nolock) 
		join ContentFilter cf with (nolock) on cfd.FilterID = cf.FilterID
		join Content c with (nolock) on cf.ContentID = c.ContentID
		join [Groups] g with (nolock) on cf.GroupID = g.GroupID
	where 
		cf.FilterID = @FilterID AND 
		cfd.IsDeleted = 0 AND
		cf.IsDeleted = 0 AND
		c.IsDeleted = 0 AND
		g.CustomerID = @CustomerID
END

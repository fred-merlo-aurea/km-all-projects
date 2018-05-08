CREATE  PROC [dbo].[e_ContentFilterDetail_Select_FilterID] 
(
	@FilterID int = NULL
)
AS 
BEGIN
	select cfd.*, g.CustomerID
	from 
		ContentFilterDetail cfd with (nolock) 
		join ContentFilter cf with (nolock) on cfd.FilterID = cf.FilterID
		join [Groups] g with (nolock) on cf.GroupID = g.GroupID
	where 
		cf.FilterID = @FilterID AND cfd.IsDeleted = 0 and cf.IsDeleted = 0 
END

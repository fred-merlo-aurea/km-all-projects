CREATE PROCEDURE [dbo].[e_Market_Select_UserID]
	@UserID int = 0
AS
Begin
	select m.*, b.BrandName 
	from userbrand ub with (nolock) 
	join  Brand b with (nolock) on ub.BrandID=b.BrandID 
	join Markets m with (nolock) on m.BrandID = b.BrandID 
	where b.IsDeleted = 0 and userID = @UserID order by Marketname asc
end

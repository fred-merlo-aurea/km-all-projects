CREATE PROCEDURE [dbo].[e_Brand_Select_PubID]
	@PubID int = 0
AS
BEGIN
	select b.* 
	from Brand b with (nolock) 
	join branddetails bd with (nolock) on b.BrandID = bd.brandID
	where b.IsDeleted = 0 and PubID = @PubID
END
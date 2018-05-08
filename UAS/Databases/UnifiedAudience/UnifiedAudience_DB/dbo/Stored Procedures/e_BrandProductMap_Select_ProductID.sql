CREATE PROCEDURE [dbo].[e_BrandProductMap_Select_ProductID]
@ProductID int
AS
BEGIN

	set nocount on

	select *
	from BrandProductMap with(nolock)
	where ProductID = @ProductID

END
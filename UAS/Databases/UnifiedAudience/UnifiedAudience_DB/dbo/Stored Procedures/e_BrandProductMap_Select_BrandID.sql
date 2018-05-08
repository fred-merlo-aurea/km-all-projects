CREATE PROCEDURE [dbo].[e_BrandProductMap_Select_BrandID]
@BrandID int
AS
BEGIN

	set nocount on

	select *
	from BrandProductMap with(nolock)
	where BrandID = @BrandID

END
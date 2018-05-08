CREATE PROCEDURE [dbo].[e_SecurityGroupBrandMap_Select_BrandID]
@BrandID int
AS
BEGIN

	SET NOCOUNT ON

	select *
	from SecurityGroupBrandMap with(nolock)
	where BrandID = @BrandID

END
GO
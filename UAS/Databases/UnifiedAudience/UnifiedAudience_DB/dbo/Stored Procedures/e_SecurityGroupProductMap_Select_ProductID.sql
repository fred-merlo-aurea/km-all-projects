CREATE PROCEDURE [dbo].[e_SecurityGroupProductMap_Select_ProductID]
@ProductID int
AS
BEGIN

	SET NOCOUNT ON

	select *
	from SecurityGroupProductMap with(nolock)
	where ProductID = @ProductID

END
GO
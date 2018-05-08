CREATE PROCEDURE e_AdHocDimension_Select_AdHocDimensionGroupId
@AdHocDimensionGroupId int
AS
BEGIN

	set nocount on

	Select * 
	from AdHocDimension WITH(NOLOCK)
	where AdHocDimensionGroupId = @AdHocDimensionGroupId

END
GO
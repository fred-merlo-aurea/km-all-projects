create procedure e_AdHocDimensionGroupPubcodeMap_Select_AdHocDimensionGroupId
@AdHocDimensionGroupId int
AS
BEGIN

	set nocount on

	select * 
	from AdHocDimensionGroupPubcodeMap WITH(NOLOCK)
	where AdHocDimensionGroupId = @AdHocDimensionGroupId

END
GO
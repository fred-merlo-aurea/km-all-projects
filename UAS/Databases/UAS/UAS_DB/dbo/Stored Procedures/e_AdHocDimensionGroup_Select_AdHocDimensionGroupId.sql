create procedure e_AdHocDimensionGroup_Select_AdHocDimensionGroupId
@AdHocDimensionGroupId int
as
BEGIN

	set nocount on

	select *
	from AdHocDimensionGroup with(nolock)
	where AdHocDimensionGroupId = @AdHocDimensionGroupId

END
go
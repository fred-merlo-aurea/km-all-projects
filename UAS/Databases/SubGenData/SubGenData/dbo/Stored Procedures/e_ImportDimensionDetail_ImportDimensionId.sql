create procedure e_ImportDimensionDetail_ImportDimensionId
@ImportDimensionId int
as
BEGIN

	set nocount on

	select *
	from ImportDimensionDetail with(nolock)
	where ImportDimensionId = @ImportDimensionId

END
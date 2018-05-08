create procedure e_CodeType_Select_CodeTypeId
@CodeTypeId int
as
BEGIN

	set nocount on

	select *
	from CodeType with(nolock)
	where CodeTypeId = @CodeTypeId

END
go
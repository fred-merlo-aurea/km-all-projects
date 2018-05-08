create procedure e_CodeType_Select
as
BEGIN

	set nocount on

	select *
	from CodeType with(nolock)
	order by CodeTypeName

END
go
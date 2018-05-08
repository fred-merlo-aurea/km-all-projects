create procedure e_CodeType_Select_CodeTypeName
@CodeTypeName varchar(50)
as
BEGIN

	set nocount on

	select *
	from CodeType with(nolock)
	where CodeTypeName = @CodeTypeName

END
go
create procedure e_Code_Select_CodeId
@CodeId int
as
BEGIN

	set nocount on

	select *
	from Code with(nolock)
	where CodeId = @CodeId

END
go
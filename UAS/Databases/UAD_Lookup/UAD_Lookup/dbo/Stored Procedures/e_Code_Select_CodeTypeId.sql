create procedure e_Code_Select_CodeTypeId
@CodeTypeId int
as
BEGIN

	set nocount on

	select *
	from Code with(nolock)
	where CodeTypeId = @CodeTypeId
	order by CodeName

END
go
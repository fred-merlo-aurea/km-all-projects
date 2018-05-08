create procedure e_Code_Select
as
BEGIN

	set nocount on

	select *
	from Code with(nolock)
	order by CodeName

END
go
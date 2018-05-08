create procedure e_Publication_Select
as
BEGIN

	set nocount on

	select *
	from Publication with(nolock)
	order by name

END
go
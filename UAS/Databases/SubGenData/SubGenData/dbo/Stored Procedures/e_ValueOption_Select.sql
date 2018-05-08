create procedure e_ValueOption_Select
@field_id int
as
BEGIN

	set nocount on

	select *
	from ValueOption with(nolock)
	where field_id = @field_id

END
go
create procedure e_CustomField_Select
@account_id int
as
BEGIN

	set nocount on

	select *
	from CustomField with(nolock)
	where account_id = @account_id

END
go
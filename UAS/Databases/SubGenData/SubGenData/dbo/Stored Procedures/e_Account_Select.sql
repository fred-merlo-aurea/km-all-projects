create procedure e_Account_Select
as
BEGIN

	set nocount on

	select *
	from Account with(nolock)
	order by company_name

END